using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/concorrente-produto")]
    public class ConcorrenteProdutoController : ControllerBase
    {
        private readonly IConcorrenteProdutoService concorrenteProdutoService;
        private readonly IProdutoService produtoService;

        public ConcorrenteProdutoController(IConcorrenteProdutoService concorrenteProdutoService,IProdutoService produtoService)
        {
            this.concorrenteProdutoService=concorrenteProdutoService;
            this.produtoService=produtoService;
        }


        [HttpGet("search")]
        public async Task<IActionResult> SearchConcorrenteProdutos([FromQuery] string? idsConcorrente, [FromQuery] string? descricaoProduto, [FromQuery] string? fabricante)
        {
            var result = await concorrenteProdutoService.SearchConcorrenteProdutos(idsConcorrente, descricaoProduto ?? string.Empty,fabricante ?? string.Empty);
            return Ok(result);
        }

        [HttpPut("link-product")]
        public async Task<IActionResult> LinkProduct([FromBody] LinkProductRequest dto)
        {
            var result = await concorrenteProdutoService.LinkProduct(dto.ConcorrenteProdutoId!, dto.InternalProductCode!);
            return Ok(result);
        }


        [HttpPut("unlink-product")]
        public async Task<IActionResult> UnLinkProduct([FromBody] LinkProductRequest dto)
        {
            var result = await concorrenteProdutoService.UnLinkProduct(dto.InternalProductCode!);
            return Ok(result);
        }

        [HttpGet("suggest-product")]
        public async Task<IActionResult> GetProductSuggestion([FromQuery string descricao)
        {
            return Ok(produtoService.GetProdutosSuggestion(descricao));
        }
    }
}
