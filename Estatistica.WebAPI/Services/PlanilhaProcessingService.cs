using Estatistica.BusinessLogicLayer.Data;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Estatistica.WebAPI.Services
{
    public class PlanilhaProcessingService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger logger;
        private IEnumerable<PlanilhaExcelModel>? planilhaLista;
        private readonly IServiceScope scope;
        private readonly IPlanilhaService planilhaService;
        private readonly IConcorrenteFilialService concorrenteFilialService;
        private readonly ApplicationDbContext context;

        public PlanilhaProcessingService(IServiceScopeFactory serviceScopeFactory, ILogger<PlanilhaProcessingService> logger)
        {
            this.serviceScopeFactory=serviceScopeFactory;
            this.logger=logger;
            scope = serviceScopeFactory.CreateScope();
            planilhaService = scope.ServiceProvider.GetRequiredService<IPlanilhaService>();
            concorrenteFilialService = scope.ServiceProvider.GetRequiredService<IConcorrenteFilialService>();
            context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        #region Service Overrides
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var planilhas = await planilhaService.GetPlanilhasForProcessing();
                context.AttachRange(planilhas);

                foreach (var planilha in planilhas)
                {
                    try
                    {
                        switch (planilha.Status)
                        {
                            //Just added
                            case (int)PlanilhaStatusEnum.AguardandoInclusao:
                                if (!string.IsNullOrWhiteSpace(planilha.Caminho))
                                    await readPlanilha(planilha);
                                break;
                            //Ready to process
                            case (int)PlanilhaStatusEnum.AguardandoProcessamento:
                                var hasPendentes = await checkFiliaisPendentes(planilha);
                                if (hasPendentes.HasValue && !hasPendentes.Value)
                                {
                                    //Process the planilha
                                    await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.EmProcesso, "Processando a planilha.");
                                    //Include just items in concorrentefilialTemp
                                    await filterPlanilha(planilha);
                                    await includePlanilhaProducts(planilha);
                                    await includePlanilhaHeader(planilha);
                                    await includePlanilhaItems(planilha);
                                    await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.ProcessamentoConcluido, "Planilha incluida com sucesso");
                                }
                                break;
                        }
                        context.Entry(planilha).State = EntityState.Detached;
                    }
                    catch (Exception ex)
                    {
                        await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Erro, $"Erro ao processar a planilha: {ex.Message}");
                    }
                    await Task.Delay(5 * 60 * 1000);
                }
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(PlanilhaProcessingService)} foi iniciada");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(PlanilhaProcessingService)} foi cancelada");
        }

        #endregion

        #region Principal Methods
        private async Task readPlanilha(Planilha planilha)
        {
            Console.WriteLine($"Starting read the planilha {planilha.NomePlanilha}");
            var planilhaDataTable = readExcelFileToDataTable(planilha.Caminho!);
            if (planilhaDataTable is null || validaPlanilha(planilhaDataTable))
            {
                planilha.Status = (int)PlanilhaStatusEnum.Erro;
                await planilhaService.AddPlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Erro, "Planilha invalida, favor validar a esquema se contain as colunas necessarias.");
            }
            planilhaLista = convertDataTableToPlanilhaExcelModels(planilhaDataTable!);
            var added = await generateFiliasTemp(planilha);
            if (added)
                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Incluida, "Incluida, aguardando o usuario escolher as empresas que deseja processar");
        }

        private async Task filterPlanilha(Planilha planilha)
        {
            Console.WriteLine($"Iniciando o filtro da planilha {planilha.Id} - {planilha.NomePlanilha}");
            var concorrenteFiliaisTempoIncluidos = (await planilhaService.GetConcorrenteFilialTempByPlanilhaId(planilha.Id))?.Where(x => x.Incluido.HasValue && x.Incluido.Value);
            planilhaLista = planilhaLista?.Where(x => concorrenteFiliaisTempoIncluidos?.Any(y => y.Cnpj == x.Cnpj) ?? false).ToList();
        }

        private async Task<bool?> checkFiliaisPendentes(Planilha planilha)
        {
            Console.WriteLine($"Iniciando a verificacao de filiais pendentes da planilha {planilha.Id} - {planilha.NomePlanilha}");
            var planilhaDataTable = readExcelFileToDataTable(planilha.Caminho!);
            if (planilhaDataTable is null || validaPlanilha(planilhaDataTable))
            {
                planilha.Status = (int)PlanilhaStatusEnum.Erro;
                await planilhaService.AddPlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Erro, "Planilha invalida, favor validar a esquema se contain as colunas necessarias.");
                return null;
            }

            var filiaisTemp = (await planilhaService.GetConcorrenteFilialTempByPlanilhaId(planilha.Id)).Where(x => x.Incluido.HasValue && x.Incluido.Value);
            if (filiaisTemp is null || !filiaisTemp.Any(x => x.Incluido.HasValue && x.Incluido.Value))
                return null;

            planilhaLista = convertDataTableToPlanilhaExcelModels(planilhaDataTable!);
            var filiasConcorrentes = await concorrenteFilialService.GetConcorrentesFiliais();

            var filiaisPendentes = filiaisTemp.Select(x => x.Cnpj).ToList().Except(filiasConcorrentes.Select(x => x.Cnpj));

            //No need to filiais temp any more
            //await planilhaService.DeleteConcorrenteFilialTempRange(planilha.Id);
            if (filiaisPendentes.Any())
            {
                var filailPendentesWithName = filiaisTemp.Where(x => filiaisPendentes.Contains(x.Cnpj));
                await concorrenteFilialService.AddConcorrenteFilialPendenteRange(planilha.Id, filailPendentesWithName.Select(x => new ConcorrenteFilialPendente
                {
                    Cnpj = x.Cnpj!,
                    Planilha = planilha,
                    Nome = x.Nome!
                }).ToList());


                await planilhaService.UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.Pendente, "Planilha pendente, usuario tem que criar o vinculo entre Cnpj da filial e o cadastro do concorrente.");

                return true;
            }
            return false;
        }

        #endregion


        #region Private Methods
        /// <summary>
        /// Method to read an Excel file and convert it to a DataTable.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        private DataTable? readExcelFileToDataTable(string path)
        {            
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
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
        /// <summary>
        /// Method to convert the excel datatable to list of model
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Methdo to validate the planilha
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to generate the temporary filias from the planilha
        /// </summary>
        /// <param name="planilha"></param>
        /// <returns></returns>
        private async Task<bool> generateFiliasTemp(Planilha planilha)
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

            }
            return false;
        }

        private async Task includePlanilhaHeader(Planilha planilha)
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


        private async Task includePlanilhaProducts(Planilha planilha)
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

        private async Task includePlanilhaItems(Planilha planilha)
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


        #endregion
    }
}
