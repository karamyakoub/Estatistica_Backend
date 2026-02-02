using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize]
    public class DashBoardController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        private readonly IConcorrenteService concorrenteService;
        private readonly IConcorrenteProdutoService concorrenteProdutoService;
        private readonly IConcorrenteProdutoRepository concorrenteProdutoRepository;
        private readonly INfcRespository nfcRespository;
        private readonly IMemoryCache memoryCache;
        private const string DashboardCacheKey = "DashboardData";

        public DashBoardController(IUsuarioService usuarioService,
            IConcorrenteService concorrenteService,
            IConcorrenteProdutoService concorrenteProdutoService,
            IConcorrenteProdutoRepository concorrenteProdutoRepository,
            INfcRespository nfcRespository,
            IMemoryCache memoryCache)
        {
            this.usuarioService=usuarioService;
            this.concorrenteService=concorrenteService;
            this.concorrenteProdutoService=concorrenteProdutoService;
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.nfcRespository=nfcRespository;
            this.memoryCache = memoryCache;
        }
        [HttpGet("data")]
        public async Task<IActionResult> GetDashboardData()
        {
            object? dashboardData;

            memoryCache.TryGetValue(DashboardCacheKey, out dashboardData);
            if (dashboardData is not null)
                return Ok(dashboardData);

            //var limitDate = DateTime.Today.AddMonths(-9);
            dashboardData = new DashboardDto
            {                                
                TotalUsers = (await usuarioService.GetUsuarios()).Count(),
                TotalConcorrentes = (await concorrenteService.GetConcorrentes(OrderByEnum.Id)).Count(),
                TotalProducts = await concorrenteProdutoService.GetTotalCount(),
                TotalNotas = (await nfcRespository.GetNfcsByConditionNoTracking(x => true)).Count(),
                TotalLinkedProducts = (await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTracking(x => x.Produto != null)).Count(),
                TotalUnLinkedProducts = (await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTracking(x => x.Produto == null)).Count(),
                TotalProductsConcorrente = await concorrenteProdutoService.GetTop10ProductsCount(),
                TotalNotasConcorrente = (await nfcRespository.GetNfcsByConditionNoTracking(x => true))
                    .GroupBy(x => x.ConcorrenteCnpj.Concorrente.Nome)
                    .Select(g =>  new DashboardDto.DashboardNotasPorConcorrente
                    {
                        Concorrente = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .Take(10)
                    .ToList(),
                //TotalNotasData = (await nfcRespository.GetNfcsByConditionNoTracking(x => x.DataCadastro != null && x.DataCadastro >= limitDate))
                //    .GroupBy(x => x.DataCadastro!.Value.Date)
                //    .Select(g => new DashboardDto.DashboardNotasPorDate
                //    {
                //        Data = g.Key,
                //        Count = g.Count()
                //    })
                //    .OrderByDescending(x => x.Count)
                //    .Take(10)
                //    .ToList()
            };

            var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
            memoryCache.Set(DashboardCacheKey, dashboardData, cacheOptions);
            return Ok(dashboardData);
        }
    }
}
