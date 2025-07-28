using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/concorrente-produto")]
    public class ConcorrenteProdutoController : ControllerBase
    {
        private readonly IConcorrenteProdutoService concorrenteProdutoService;

        public ConcorrenteProdutoController(IConcorrenteProdutoService concorrenteProdutoService)
        {
            this.concorrenteProdutoService=concorrenteProdutoService;
        }


        [HttpGet("/search")]
        public async Task<IActionResult> SearchConcorrenteProdutos([FromQuery] int? idConcorrente, [FromQuery] string? descricaoProduto)
        {
            var result = await concorrenteProdutoService.SearchConcorrenteProdutos(idConcorrente, descricaoProduto);
            return Ok(result);
        }

    }
}
