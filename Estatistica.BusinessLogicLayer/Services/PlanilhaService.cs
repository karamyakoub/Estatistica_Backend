using AutoMapper;
using Estatistica.BusinessLogicLayer.Data;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.BusinessLogicLayer.Utils;
using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http.Headers;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class PlanilhaService : IPlanilhaService
    {
        private readonly IPlanilhaRepository planilhaRepository;
        private readonly IPlanilhaStatusRepository planilhaStatusRepository;
        private readonly IConcorrenteFilialTempRepository concorrenteFilialTempRepository;
        private readonly IConcorrenteFilialPendenteRepository concorrenteFilialPendenteRepository;
        private readonly IConcorrenteFilialRepository concorrenteFilialRepository;
        private readonly INfcRespository nfcRespository;
        private readonly IConcorrenteRepository concorrenteRepository;
        private readonly IConcorrenteProdutoRepository concorrenteProdutoRepository;
        private readonly INfiRespository nfiRespository;
        private readonly IProdutoRepository produtoRepository;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IMapper mapper;
        private readonly ILogger<PlanilhaService> logger;
        private readonly ApplicationDbContext dbContext;

        public PlanilhaService(IPlanilhaRepository planilhaRepository,
            IPlanilhaStatusRepository planilhaStatusRepository,
            IConcorrenteFilialTempRepository concorrenteFilialTempRepository,
            IConcorrenteFilialPendenteRepository concorrenteFilialPendenteRepository,
            IConcorrenteFilialRepository concorrenteFilialRepository,
            INfcRespository nfcRespository,
            IConcorrenteRepository concorrenteRepository,
            IConcorrenteProdutoRepository concorrenteProdutoRepository,
            INfiRespository nfiRespository,
            IProdutoRepository produtoRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            ILogger<PlanilhaService> logger,
            ApplicationDbContext dbContext)
        {
            this.planilhaRepository = planilhaRepository;
            this.planilhaStatusRepository=planilhaStatusRepository;
            this.concorrenteFilialTempRepository=concorrenteFilialTempRepository;
            this.concorrenteFilialPendenteRepository=concorrenteFilialPendenteRepository;
            this.concorrenteFilialRepository=concorrenteFilialRepository;
            this.nfcRespository=nfcRespository;
            this.concorrenteRepository=concorrenteRepository;
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.nfiRespository=nfiRespository;
            this.produtoRepository=produtoRepository;
            this.usuarioRepository=usuarioRepository;
            this.mapper = mapper;
            this.logger=logger;
            this.dbContext=dbContext;
        }

        public async Task<bool> AddCocorrenteFilialTempRange(List<ConcorrenteFilialTemp> concorrenteFilialTemps)
        {
            return await concorrenteFilialTempRepository.AddCocorrenteFilialTempRange(concorrenteFilialTemps);
        }

        public async Task<int?> AddPlanilha(string nomePlanilha, string caminho)
        {
            var planilhaToAdd = new Planilha
            {
                NomePlanilha = nomePlanilha,
                Caminho = caminho,
                Status = (int)PlanilhaStatusEnum.AguardandoInclusao,
            };
            return (await planilhaRepository.AddPlanilha(planilhaToAdd)).Id;
        }

        public async Task<PlanilhaStatusGetResponse> AddPlanilhaStatus(int idPlanilha, PlanilhaStatusEnum situacao, string obs)
        {
            var planilha = await planilhaRepository.GetPlanilhaById(idPlanilha);
            if (planilha is null)
                throw new ArgumentException("Planilha not found");
            PlanilhaStatus statusToAdd = new PlanilhaStatus
            {
                Planhila = planilha,
                DataInclusao = DateTime.Now,
                Obs = obs,
                Situacao = (int)situacao
            };
            return mapper.Map<PlanilhaStatusGetResponse>(await planilhaStatusRepository.AddPlanilhaStatus(statusToAdd));
        }

        public async Task DeleteConcorrenteFilialTempRange(int planilhaId)
        {
            var concorrenteFilialTempsToDelete = (await concorrenteFilialTempRepository.GetConcorrenteFilialTempByCondition(x => x.Planilha != null && x.Planilha.Id == planilhaId)).ToList();
            await concorrenteFilialTempRepository.DeleteConcorrenteFilialTempRange(concorrenteFilialTempsToDelete);
        }

        public async Task<IEnumerable<ConcorrenteFilialTempGetResponse>> GetConcorrenteFilialTempByPlanilhaId(int planilhaId)
        {
            return mapper.Map<IEnumerable<ConcorrenteFilialTempGetResponse>>(await concorrenteFilialTempRepository.GetConcorrenteFilialTempByConditionNoTracking(x => x.Planilha != null && x.Planilha.Id == planilhaId));
        }

        public async Task<PageObject<PlanilhaGetResponse>> GetPlanilhas(int pageSize, int pageCount, OrderByEnum orderBy)
        {
            var planilhas = (await planilhaRepository.GetPlanilhasByCondition(x => x.Id > 0));
            if (orderBy == OrderByEnum.Id)
                planilhas = planilhas.OrderBy(x => x.Id)
                .Skip(pageSize * (pageCount - 1))
                .Take(pageSize)
                .ToList();
            else if (orderBy == OrderByEnum.Description)
                planilhas = planilhas.OrderBy(x => x.NomePlanilha)
                .Skip(pageSize * (pageCount - 1))
                .Take(pageSize)
                .ToList();
            else
                planilhas = planilhas.OrderByDescending(x => x.DataCadastro)
                .Skip(pageSize * (pageCount - 1))
                .Take(pageSize)
                .ToList();

            foreach (var planilha in planilhas)
            {
                var usuario = await usuarioRepository.GetUsuarioById(planilha.UsuarioCadastro);
                planilha.UsuarioCadastro = usuario?.Email ?? "Desconhecido";
            }

            return new PageObject<PlanilhaGetResponse>(mapper.Map<List<PlanilhaGetResponse>>(planilhas), pageSize, pageCount, planilhas.Count());
        }

        public async Task<IEnumerable<Planilha>> GetPlanilhasForProcessing()
        {
            //TODO: Implementar o filtro de planilhas que estão aguardando inclusão
            var statusList = new List<int>
            {
                (int)PlanilhaStatusEnum.AguardandoInclusao,
                (int)PlanilhaStatusEnum.AguardandoProcessamento,
            };
            return await planilhaRepository.GetPlanilhasByConditionNoTracking(x => statusList.Contains(x.Status));
        }

        public async Task<IEnumerable<PlanilhaStatusGetResponse>> GetPlanilhaStatus(int id)
        {
            var planilhasResponse = mapper.Map<IEnumerable<PlanilhaStatusGetResponse>>(await planilhaStatusRepository.GetPlanilhaStatusByPlanilhaId(id));
            foreach (var planilha in planilhasResponse)
            {
                planilha.Situacao = EnumUtil.GetPlanilhaStatusDescription(Int16.Parse(planilha.Situacao ?? "0"));
            }
            return planilhasResponse;
        }

        public async Task UpdateConcorrenteFilialTempRange(int planilhaId, List<ConcorrenteFilialTempUpdateRequest> concorrenteFilialTemps)
        {
            await concorrenteFilialTempRepository.UpdateConcorrenteFilialTempRange(planilhaId, mapper.Map<List<ConcorrenteFilialTemp>>(concorrenteFilialTemps));
        }

        public async Task<PlanilhaGetResponse> UpdatePlanilhaStatus(int planilhaId, PlanilhaStatusEnum planilhaStatus, string obs)
        {
            var planilha = await planilhaRepository.GetPlanilhaById(planilhaId);
            if (planilha is null)
                throw new ArgumentException("Planilha not found");
            planilha.Status = (int)planilhaStatus;
            await planilhaRepository.UpdatePlanilhaStatus(planilha);

            //Include the planilha status row
            await planilhaStatusRepository.AddPlanilhaStatus(new PlanilhaStatus
            {
                DataInclusao = DateTime.Now,
                Situacao = (int)planilhaStatus,
                Obs = obs,
                Planhila = planilha
            });
            return mapper.Map<PlanilhaGetResponse>(planilha);
        }

        public async Task CheckAndUpdatePlanilhaStatus()
        {
            var planilhasPendentes = await planilhaRepository.GetPlanilhasByStatus((int)PlanilhaStatusEnum.Pendente);
            foreach (var planilha in planilhasPendentes)
            {
                var filiaisPendentes = await concorrenteFilialPendenteRepository.GetConcorrenteFilialPendentes(x => x.Planilha != null && x.Planilha.Id == planilha.Id);
                if (filiaisPendentes.All(x => x.Concorrente != null))
                {
                    await UpdatePlanilhaStatus(planilha.Id, PlanilhaStatusEnum.AguardandoProcessamento, "Todas as filiais pendentes foram vinculadas a um concorrente.");
                }

            }
        }

        public async Task<int> IncludePlanilhaHeader(Planilha planilha, IEnumerable<PlanilhaExcelModel>? planilhaLista)
        {
            if (planilhaLista is not null || planilhaLista!.Any())
            {
                // Step 1: Get distinct records (grouped by ChaveNfe)
                var distinctPlanilha = planilhaLista!
                    .OrderBy(x => x.DataEmissao)
                    .GroupBy(x => x.ChaveNfe)
                    .Select(x => x.First())
                    .ToList();

                // Step 2: Get concorrenteFiliais from the service
                var concorrenteFiliais = await concorrenteFilialRepository.GetConcorrentesFiliais();

                // Step 3: Perform the join
                var headers = (
                    from p in distinctPlanilha
                    join cf in concorrenteFiliais on p.Cnpj equals cf.Cnpj into gj
                    from cfJoined in gj.DefaultIfEmpty() // left join
                    select new Nfc
                    {
                        ChaveNfe = p.ChaveNfe!,
                        ConcorrenteCnpj = cfJoined, // or cfJoined?.Id if you just need the ID
                        DataEmissao = p.DataEmissao,
                        CnpjCliente = p.CnpjCliente,
                        NomeCliente = p.NomeCliente,
                        Planilha = planilha,
                        DataCadastro = DateTime.Now,
                        UsuarioCadastro = planilha.UsuarioCadastro
                    }
                ).ToList();

                var existedHeaders = await nfcRespository.GetNfcsByCondition(x => headers.Select(x => x.ChaveNfe).Contains(x.ChaveNfe));


                // Step 4: Filter out existing headers
                var newHeaders = headers.Where(x => !existedHeaders.Any(y => y.ChaveNfe == x.ChaveNfe)).ToList();
                await nfcRespository.AddNfcRange(newHeaders);

                return newHeaders.Count;
            }
            return 0;
        }

        public async Task<int> IncludePlanilhaProducts(Planilha planilha, IEnumerable<PlanilhaExcelModel>? planilhaLista)
        {
            if (planilhaLista is null)
                return 0;
            var concorrentesFilial = await concorrenteFilialRepository.GetConcorrentesFiliais();
            var concorrentes = await concorrenteRepository.GetConcorrentes();

            var planilhaListaWithCodigoConcorrente = from p in planilhaLista
                                                     join cf in concorrentesFilial on p.Cnpj equals cf.Cnpj into gj
                                                     from cfJoined in gj.DefaultIfEmpty()
                                                     join c in concorrentes on cfJoined.Concorrente.Id equals c.Id into gj2
                                                     from cJoined in gj2.DefaultIfEmpty()
                                                     select new { Planilha = p, Concorrente = cJoined };

            var distinctPlanilha = planilhaListaWithCodigoConcorrente
                .GroupBy(x => new { x.Concorrente.Id, x.Planilha.CodigoProduto })
                .Select(x => x.First())
                .ToList();

            var products = distinctPlanilha
                .Where(x => x.Concorrente != null && !string.IsNullOrWhiteSpace(x.Planilha.CodigoProduto))
                .Select(x => new ConcorrenteProduto
                {
                    Id = $"{x.Concorrente.Id}{x.Planilha.CodigoProduto}",
                    Concorrente = x.Concorrente,
                    CodigoProdutoConcorrente = x.Planilha.CodigoProduto!,
                    DescricaoProdutoConcorrente = x.Planilha.DescricaoProduto!,
                    UnidadeProdutoConcorrente = x.Planilha.Unidade,
                    CodigoBarraConcorrente = x.Planilha.CodigoBarra,
                    Planilha = planilha,
                    DataCadastro = DateTime.Now,
                    UsuarioCadastro = planilha.UsuarioCadastro
                }).ToList();

            var ids = products.Select(p => p.Id).ToList();

            var existingProducts = await concorrenteProdutoRepository
                .GetConcorrenteProdutosByConditionNoTracking(x => ids.Contains(x.Id));

            var productsToInclude = products
                .Where(p => !existingProducts.Select(e => e.Id).Contains(p.Id))
                .ToList();
            productsToInclude = productsToInclude.Where(p => !string.IsNullOrEmpty(p.Id)).ToList();

            if (productsToInclude is null)
                return 0;
            var internalProducts = await produtoRepository.GetProdutosByCondition(x => productsToInclude.Select(p => p.CodigoBarraConcorrente).Contains(x.CodigoBarra));


            foreach (var product in productsToInclude)
            {
                product.Produto = internalProducts.FirstOrDefault(x => x.CodigoBarra == product.CodigoBarraConcorrente);
                if (product.Produto is not null)
                    product.TipoVinculo = "CB";
            }
            var usuCadastro = planilha.UsuarioCadastro ?? string.Empty;
            foreach (var item in productsToInclude)
            {
                try
                {
                    await dbContext.Database.ExecuteSqlRawAsync("insert into concorrenteprodutos (Id,ConcorrenteId,codProdCon,descProdCon,unProdCon,codBarraCon,ProdutoCodigoProduto,PlanilhaId,tipoVinculo,dtCadastro,usuCadastro) values (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)",
                        item.Id, item.Concorrente.Id, item.CodigoProdutoConcorrente, item.DescricaoProdutoConcorrente, item.UnidadeProdutoConcorrente ?? string.Empty,
                        item.CodigoBarraConcorrente ?? string.Empty, item.Produto?.CodigoProduto,
                        item.Planilha!.Id , item.TipoVinculo ?? string.Empty,
                        DateTime.Now, usuCadastro
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting product into ConcorrenteProdutos table. Concorrente: {ConcorrenteId}, Codigo: {ConcorrenteProdutoCodigo}", item.Concorrente.Id,item.CodigoProdutoConcorrente);
                }
            }

            return productsToInclude.Count();
        }

        public async Task<int> IncludePlanilhaItems(Planilha planilha, IEnumerable<PlanilhaExcelModel>? planilhaLista)
        {
            if (planilhaLista is null)
                return 0;
            var nfcLista = await nfcRespository.GetNfcsByCondition(x => x.Planilha!.Id == planilha.Id);
            var concorrenteFiliais = await concorrenteFilialRepository.GetConcorrentesFiliais(x => x.Cnpj != null);

            var concorrenteProdutos = await concorrenteProdutoRepository
                .GetConcorrenteProdutosByCondition(x => x.Concorrente != null);


            var itemsJoin = (from p in planilhaLista!
                             join n in nfcLista on p.ChaveNfe equals n.ChaveNfe into gj
                             from nJoined in gj
                             join cf in concorrenteFiliais on p.Cnpj equals cf.Cnpj into gj2
                             from cfJoined in gj2
                             join prod in concorrenteProdutos on new { ConcorrenteId = cfJoined.Concorrente.Id, Prod = p.CodigoProduto } equals new { ConcorrenteId = prod.Concorrente.Id, Prod = prod.CodigoProdutoConcorrente } into gj3
                             from pJoined in gj3
                             select new Nfi
                             {
                                 Id = $"{nJoined.ChaveNfe ?? string.Empty}{p.CodigoProduto}",
                                 Nfc = nJoined,
                                 CodigoBarra = p.CodigoBarra,
                                 DataCadastro = DateTime.Now,
                                 Qtde = p.Qtd,
                                 CodMunicipio = p.CodigoMunicipio,
                                 UfDestino = p.UfDestino,
                                 UfOrigin = p.UfOrigin,
                                 Unidade = p.Unidade,
                                 UsuarioCadastro = planilha.UsuarioCadastro,
                                 ConcorrenteProduto = pJoined,
                                 Valor = p.ValorUnitario
                             }).ToList();

            var existedItems = await nfiRespository.GetNfisByConditionNoTracking(x => itemsJoin.Select(x => x.Id).Contains(x.Id));
            var existedIds = new HashSet<string>(existedItems.Select(x => x.Id));
            var itemsToAdd = itemsJoin.Where(x => !existedIds.Contains(x.Id)).ToList();

            itemsToAdd = itemsToAdd.GroupBy(x => x.Id)
                .Select(x => x.First())
                .ToList();

            var usuCadastro = nfcLista.FirstOrDefault()?.UsuarioCadastro ?? string.Empty;
            foreach (var item in itemsToAdd)
            {
                try
                {
                    await dbContext.Database.ExecuteSqlRawAsync("insert into Nfis (Id, ConcorrenteProdutoId, NfcChaveNfe, qtde, valor, ufOrigin, ufDestino, codBarra, unidade, dtCadastro, usuCadastro, codMuni) values (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)",
                        item.Id, item.ConcorrenteProduto.Id, item.Nfc.ChaveNfe, item.Qtde, item.Valor,
                        item.UfOrigin ?? string.Empty, item.UfDestino ?? string.Empty,
                        item.CodigoBarra ?? string.Empty, item.Unidade ?? string.Empty,
                        DateTime.Now, usuCadastro, item.CodMunicipio ?? string.Empty
                        );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting item into Nfis table. Item ID: {ItemId}, ChaveNfe: {ChaveNfe}", item.Id, item.Nfc.ChaveNfe);
                }
            }

            return itemsToAdd.Count;
        }
    }
}
