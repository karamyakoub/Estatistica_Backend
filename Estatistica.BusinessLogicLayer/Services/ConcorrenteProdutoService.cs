using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class ConcorrenteProdutoService : IConcorrenteProdutoService
    {
        private readonly IConcorrenteProdutoRepository concorrenteProdutoRepository;
        private readonly IMapper mapper;
        private readonly IProdutoRepository produtoRepository;
        private readonly IUsuarioRepository usuarioRepository;
        private readonly ILogConcorrenteProdutoRepository logConcorrenteProdutoRepository;
        private readonly IUsuarioService usuarioService;
        private readonly INfiRespository nfiRespository;

        public ConcorrenteProdutoService(IConcorrenteProdutoRepository concorrenteProdutoRepository, IMapper mapper,
            IProdutoRepository produtoRepository,
            IUsuarioRepository usuarioRepository,
            ILogConcorrenteProdutoRepository logConcorrenteProdutoRepository,
            IUsuarioService usuarioService,
            INfiRespository nfiRespository)
        {
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.mapper=mapper;
            this.produtoRepository=produtoRepository;
            this.usuarioRepository=usuarioRepository;
            this.logConcorrenteProdutoRepository=logConcorrenteProdutoRepository;
            this.usuarioService=usuarioService;
            this.nfiRespository=nfiRespository;
        }

        public async Task<List<DashboardDto.DashboardProductsCountByConcorrente>> GetTop10ProductsCount()
        {
            return (await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTracking(x => true))?
                .GroupBy(x => x.Concorrente.Nome)
                .Select(g => new DashboardDto.DashboardProductsCountByConcorrente { Concorrente = g.Key, Count = g.Count() })
                .OrderByDescending(X => X.Count)
                .Take(10)
                .ToList() ?? new List<DashboardDto.DashboardProductsCountByConcorrente>();
        }

        public async Task<int> GetTotalCount()
        {
            return (await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTracking(x => true)).Count();
        }

        public async Task<bool> LinkProduct(string CodigoProdutoConcorrente, string idProduto)
        {
            var produto = await produtoRepository.GetProductByCodigo(idProduto);
            if (produto is null) throw new ArgumentNullException("Produto nao encontrado");
            var result = await concorrenteProdutoRepository.LinkConcorrenteProduto(CodigoProdutoConcorrente, produto);
            if (result)
            {
                var concorrenteProduto = await concorrenteProdutoRepository.GetConcorrenteProdutosByProdutoId(CodigoProdutoConcorrente);
                if (concorrenteProduto is not null)
                {
                    await logConcorrenteProdutoRepository.Add(new LogConcorrenteProduto
                    {
                        CodigoProdutoConcorrente = concorrenteProduto,
                        Concorrente = concorrenteProduto.Concorrente,
                        CodigoProdutoAtual = produto,
                        DescricaoProdutoConcorrenteAnt = string.Empty,
                        DescricaoProdutoConcorrenteAtual = string.Empty,
                    });
                }
            }

            return result;
        }

        public async Task<IEnumerable<ConcorrenteProdutoSearchDto>> SearchConcorrenteProdutos(string? idConcorrente, string descricaoProduto, string fabricante)
        {
            List<int>? ids = null;

            if (!string.IsNullOrWhiteSpace(idConcorrente))
                ids = idConcorrente.Split(",").Select(x => int.Parse(x)).ToList();

            var concorrenteProdutosQuery = await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTrackingSeaarchAsQueryable(x =>
                   (ids == null || ids.Count() == 0 ? true : ids.Contains(x.Concorrente.Id)) &&
                    (string.IsNullOrWhiteSpace(descricaoProduto) ? true : x.DescricaoProdutoConcorrente.Contains(descricaoProduto, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(fabricante) ? true : x.Produto != null && !string.IsNullOrWhiteSpace(x.Produto.Fabricante) && x.Produto.Fabricante.Contains(fabricante, StringComparison.OrdinalIgnoreCase))
                );


            var nfisQuery = await nfiRespository.GetNfisByConditionNoTrackingAsQueryable(x => true);

            var ultimaDataPorProduto = nfisQuery
                                .GroupBy(x => x.ConcorrenteProduto.Id)
                                .Select(g => new
                                {
                                    ConcorrenteProdutoId = g.Key,
                                    UltimaDataInclusao = g.Max(x => x.DataCadastro)
                                });

            var concorrenteProdutos = from cp in concorrenteProdutosQuery
                                      join nfi in ultimaDataPorProduto
                                          on cp.Id equals nfi.ConcorrenteProdutoId into nfiGroup
                                      from nfi in nfiGroup.DefaultIfEmpty()
                                      select new
                                      {
                                          ConcorrenteProduto = cp,
                                          UltimaDataInclusao = nfi != null
                                              ? nfi.UltimaDataInclusao
                                              : (DateTime?)null
                                      };


            var result = await concorrenteProdutos.ToListAsync();


            var productList = result.Select(x =>
                            {
                                var dto =
                                    mapper.Map<ConcorrenteProdutoSearchDto>(
                                        x.ConcorrenteProduto);

                                if (x.UltimaDataInclusao.HasValue)
                                {
                                    dto.UltimaDataInclusao =
                                        DateOnly.FromDateTime(
                                            x.UltimaDataInclusao.Value);
                                }

                                return dto;
                            }).ToList();


            var users = await usuarioRepository.GetUsersByCondition(x => true);
            var prodList = from p in productList
                           join u in users
                           on p.UsuarioCadastro equals u.Id into grp
                           from user in grp.DefaultIfEmpty()
                           select changeProductUser(user.UserName ?? "", p);

            return prodList;
        }
        public async Task<bool> UnLinkProduct(string CodigoProdutoConcorrente)
        {
            var result = await concorrenteProdutoRepository.UnlinkConcorrenteProduto(CodigoProdutoConcorrente);
            if (result)
            {
                var concorrenteProduto = await concorrenteProdutoRepository.GetConcorrenteProdutosByProdutoId(CodigoProdutoConcorrente);
                if (concorrenteProduto is not null)
                {
                    await logConcorrenteProdutoRepository.Add(new LogConcorrenteProduto
                    {
                        CodigoProdutoConcorrente = concorrenteProduto,
                        Concorrente = concorrenteProduto.Concorrente,
                        CodigoProdutoAtual = null,
                        DescricaoProdutoConcorrenteAnt = string.Empty,
                        DescricaoProdutoConcorrenteAtual = string.Empty,
                    });
                }
            }
            return result;
        }

        private ConcorrenteProdutoSearchDto changeProductUser(string userName, ConcorrenteProdutoSearchDto p)
        {
            p.UsuarioCadastro = userName;
            return p;
        }
    }
}
