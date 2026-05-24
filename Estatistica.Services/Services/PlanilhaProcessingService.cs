using Estatistica.BusinessLogicLayer.Data;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using ExcelDataReader;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;

namespace Estatistica.Services.Services
{
    internal class PlanilhaProcessingService
    {
        private IEnumerable<PlanilhaExcelModel>? planilhaLista;
        private readonly IServiceScopeFactory scopeFactory;

        public PlanilhaProcessingService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task ExecuteAsync()
        {
            while (true)
            {
                try
                {
                    using var scope = scopeFactory.CreateScope();
                    var planilhaService = scope.ServiceProvider.GetRequiredService<IPlanilhaService>();
                    var concorrenteFilialService = scope.ServiceProvider.GetRequiredService<IConcorrenteFilialService>();

                    var planilhas = await planilhaService.GetPlanilhasForProcessing();
                    Console.WriteLine($"Service {nameof(PlanilhaProcessingService)} Check for planilhas, {planilhas.Count()} planilhas encotradas");

                    foreach (var planilha in planilhas)
                    {
                        planilhaLista = null;
                        try
                        {
                            switch (planilha.Status)
                            {
                                case (int)PlanilhaStatusEnum.AguardandoInclusao:
                                    if (!string.IsNullOrWhiteSpace(planilha.Caminho))
                                        await readPlanilha(planilha, planilhaService);
                                    break;

                                case (int)PlanilhaStatusEnum.AguardandoProcessamento:
                                    var hasPendentes = await checkFiliaisPendentes(planilha, planilhaService, concorrenteFilialService);
                                    if (hasPendentes.HasValue && !hasPendentes.Value)
                                    {
                                        await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.EmProcesso, "Processando a planilha.");
                                        await filterPlanilha(planilha, planilhaService);
                                        await includePlanilhaProducts(planilha, planilhaService);
                                        await includePlanilhaHeader(planilha, planilhaService);
                                        await includePlanilhaItems(planilha, planilhaService);
                                        await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.ProcessamentoConcluido, "Planilha incluida com sucesso");
                                    }
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            try
                            {
                                var errorMsg = ex.Message.Length > 70 ? ex.Message.Substring(0, 65) : ex.Message;
                                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Erro, $"Erro ao processar a planilha: {errorMsg}");
                            }
                            catch (Exception updateEx)
                            {
                                Console.WriteLine($"Falha ao atualizar status da planilha {planilha.Id}: {updateEx}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no ciclo de processamento de planilhas: {ex}");
                }

                await Task.Delay(5 * 60 * 1000);
            }
        }

        private async Task readPlanilha(Planilha planilha, IPlanilhaService planilhaService)
        {
            var fullPath = GetPlanilhaFullPath(planilha.Caminho!);
            Console.WriteLine($"Starting read the planilha {fullPath}");
            var planilhaDataTable = readExcelFileToDataTable(fullPath);
            if (planilhaDataTable is null || !validaPlanilha(planilhaDataTable))
            {
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Erro, "Planilha invalida, favor validar a esquema se contain as colunas necessarias.");
                return;
            }

            planilhaLista = convertDataTableToPlanilhaExcelModels(planilhaDataTable);
            var added = await generateFiliasTemp(planilha, planilhaService);
            if (added)
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Incluida, "Incluida, aguardando o usuario escolher as empresas que deseja processar");
        }

        private async Task filterPlanilha(Planilha planilha, IPlanilhaService planilhaService)
        {
            Console.WriteLine($"Iniciando o filtro da planilha {planilha.Id} - {planilha.NomePlanilha}");
            var concorrenteFiliaisTempoIncluidos = (await planilhaService.GetConcorrenteFilialTempByPlanilhaId(planilha.Id))?.Where(x => x.Incluido.HasValue && x.Incluido.Value);
            planilhaLista = planilhaLista?.Where(x => concorrenteFiliaisTempoIncluidos?.Any(y => y.Cnpj == x.Cnpj) ?? false).ToList();
        }

        private async Task<bool?> checkFiliaisPendentes(Planilha planilha, IPlanilhaService planilhaService, IConcorrenteFilialService concorrenteFilialService)
        {
            var fullPath = GetPlanilhaFullPath(planilha.Caminho!);
            Console.WriteLine($"Iniciando a verificacao de filiais pendentes da planilha {planilha.Id} - {planilha.NomePlanilha} Caminho {fullPath}");
            var planilhaDataTable = readExcelFileToDataTable(fullPath);
            if (planilhaDataTable is null || !validaPlanilha(planilhaDataTable))
            {
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Erro, "Planilha invalida, favor validar a esquema se contain as colunas necessarias.");
                return null;
            }

            var filiaisTemp = (await planilhaService.GetConcorrenteFilialTempByPlanilhaId(planilha.Id)).Where(x => x.Incluido.HasValue && x.Incluido.Value);
            if (filiaisTemp is null || !filiaisTemp.Any())
            {
                Console.WriteLine($"Planilha {planilha.Id} aguardando selecao de filiais pelo usuario.");
                return null;
            }

            planilhaLista = convertDataTableToPlanilhaExcelModels(planilhaDataTable);
            var filiasConcorrentes = await concorrenteFilialService.GetConcorrentesFiliais();

            var filiaisPendentes = filiaisTemp.Select(x => x.Cnpj).ToList().Except(filiasConcorrentes.Select(x => x.Cnpj));

            if (filiaisPendentes.Any())
            {
                var filailPendentesWithName = filiaisTemp.Where(x => filiaisPendentes.Contains(x.Cnpj));
                await concorrenteFilialService.AddConcorrenteFilialPendenteRange(planilha.Id, filailPendentesWithName.Select(x => new ConcorrenteFilialPendente
                {
                    Cnpj = x.Cnpj!,
                    Nome = x.Nome!
                }).ToList());

                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Pendente, "Planilha pendente, usuario tem que criar o vinculo entre Cnpj da filial e o cadastro do concorrente.");

                return true;
            }
            return false;
        }

        private static string GetPlanilhaFullPath(string caminho)
        {
            if (Path.IsPathRooted(caminho))
                return caminho;

            var mainPath = ConfigurationManager.AppSettings["CaminhoPlanilha"];
            return Path.Combine(mainPath!, caminho);
        }

        private DataTable? readExcelFileToDataTable(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });
                    if (ds is null || ds.Tables.Count == 0)
                        return null;
                    return ds.Tables[0];
                }
            }
        }

        private IEnumerable<PlanilhaExcelModel> convertDataTableToPlanilhaExcelModels(DataTable dataTable)
        {
            var planilhaModels = new List<PlanilhaExcelModel>();
            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    planilhaModels.Add(PlanilhaExcelModel.mapFromDataRow(row));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return planilhaModels;
        }

        private bool validaPlanilha(DataTable dataTable)
        {
            List<string> listaColunas = ["Nota fiscal", "Data Emissão", "CNPJ", "Concorrente", "Fantasia", "Nome", "Municipio", "UF", "UF2", "CNPJ Cliente", "Nome Cliente", "codigo Produto", "EAN", "Produto", "NCM", "CFOP", "Unidade Comercial", "Quantidade Comercial", "Valor Unitario Comercial", "Valor Unitario", "ns1:chNFe"];
            foreach (var coluna in listaColunas)
            {
                if (!dataTable.Columns.Contains(coluna))
                    return false;
            }
            return true;
        }

        private async Task<bool> generateFiliasTemp(Planilha planilha, IPlanilhaService planilhaService)
        {
            try
            {
                if (planilhaLista is not null && planilhaLista.Any())
                {
                    var listaConcorrenteFilialTemp = planilhaLista.OrderBy(x => x.Cnpj).ThenBy(x => x.DataEmissao).Select(x => new { cnpj = x.Cnpj, concorrente = x.Concorrente }).GroupBy(x => x.cnpj).Select(x => new { cnpj = x.Key, nome = x.First().concorrente }).ToList();
                    var listaConcorrenteFilialTempToAdd = new List<ConcorrenteFilialTemp>();

                    foreach (var item in listaConcorrenteFilialTemp)
                    {
                        listaConcorrenteFilialTempToAdd.Add(new ConcorrenteFilialTemp
                        {
                            Cnpj = item.cnpj!,
                            Nome = item.nome!,
                            Planilha = planilha
                        });
                    }
                    return await planilhaService.AddCocorrenteFilialTempRange(listaConcorrenteFilialTempToAdd);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        private async Task includePlanilhaHeader(Planilha planilha, IPlanilhaService planilhaService)
        {
            try
            {
                Console.WriteLine($"Iniciando a inclusao dos cabecalhos das NFCs {planilha.Id} - {planilha.NomePlanilha}");
                var cnt = await planilhaService.IncludePlanilhaHeader(planilha, planilhaLista);
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.CabecalhosIncluidos, $"Nfc incluidos com sucesso, Qtd: {cnt}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task includePlanilhaProducts(Planilha planilha, IPlanilhaService planilhaService)
        {
            try
            {
                Console.WriteLine($"Iniciando a inclusao dos produtos da planilha {planilha.Id} - {planilha.NomePlanilha}");
                var cnt = await planilhaService.IncludePlanilhaProducts(planilha, planilhaLista);
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.ProdutosIncluidos, $"Produtos Incluidos Com Sucesso, Qtd: {cnt}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task includePlanilhaItems(Planilha planilha, IPlanilhaService planilhaService)
        {
            try
            {
                Console.WriteLine($"Iniciando a inclusao dos itens da planilha {planilha.Id} - {planilha.NomePlanilha}");
                var cnt = await planilhaService.IncludePlanilhaItems(planilha, planilhaLista);
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.ItensIncluidos, $"NFI incluidos com sucesso, {cnt} itens");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
