using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public DashBoardController(IUsuarioService usuarioService,
            IConcorrenteService concorrenteService,
            IConcorrenteProdutoService concorrenteProdutoService,
            IConcorrenteProdutoRepository concorrenteProdutoRepository,
            INfcRespository nfcRespository)
        {
            this.usuarioService=usuarioService;
            this.concorrenteService=concorrenteService;
            this.concorrenteProdutoService=concorrenteProdutoService;
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.nfcRespository=nfcRespository;
        }
        [HttpGet("data")]
        public async Task<IActionResult> GetDashboardData()
        {            
            var dashboardData = new
            {                
                TotalUsers = (await usuarioService.GetUsuarios()).Count(),
                TotalConcorrentes = (await concorrenteService.GetConcorrentes(OrderByEnum.Id)).Count(),
                TotalProducts = await concorrenteProdutoService.GetTotalCount(),
                TotalNotas = (await nfcRespository.GetNfcsByConditionNoTracking(x => true)).Count(),
                TotalLinkedProducts = (await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTracking(x => x.Produto != null)).Count(),
                TotalUnLinkedProducts = (await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTracking(x => x.Produto == null)).Count(),
            };
            return Ok(dashboardData);
        }
    }
}
