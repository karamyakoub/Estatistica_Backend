using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/concorrente-produto")]
    public class ConcorrenteProdutoController : ControllerBase
    {
        private readonly IConcorrenteProdutoService concorrenteProdutoService;
        private readonly IProdutoService produtoService;
        private readonly IStatitsticService statitsticService;

        public ConcorrenteProdutoController(
            IConcorrenteProdutoService concorrenteProdutoService,
            IProdutoService produtoService,
            IStatitsticService statitsticService)
        {
            this.concorrenteProdutoService=concorrenteProdutoService;
            this.produtoService=produtoService;
            this.statitsticService=statitsticService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchConcorrenteProdutos([FromQuery] string? idsConcorrente, [FromQuery] string? descricaoProduto, [FromQuery] string? fabricante)
        {
            var result = await concorrenteProdutoService.SearchConcorrenteProdutos(idsConcorrente, descricaoProduto ?? string.Empty, fabricante ?? string.Empty);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("link-product")]
        public async Task<IActionResult> LinkProduct([FromBody] LinkProductRequest dto)
        {
            var result = await concorrenteProdutoService.LinkProduct(dto.ConcorrenteProdutoId!, dto.InternalProductCode!);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("unlink-product")]
        public async Task<IActionResult> UnLinkProduct([FromBody] LinkProductRequest dto)
        {
            var result = await concorrenteProdutoService.UnLinkProduct(dto.InternalProductCode!);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("suggest-product")]
        public async Task<IActionResult> GetProductSuggestion([FromQuery] string descricao)
        {
            var suggestionList = await produtoService.GetProdutosSuggestion(descricao);
            return Ok(suggestionList);
        }
        [Authorize]
        [HttpGet("search-product")]
        public async Task<IActionResult> SearchProduct([FromQuery] string? dateTime, [FromQuery] string? idConcorrente, [FromQuery] string? textSearch)
        {
            var concoreentesIds = string.IsNullOrWhiteSpace(idConcorrente) ? null : idConcorrente.Split(",").Select(x => int.Parse(x)).ToList();
            return Ok(await statitsticService.SearchProductPrice(dateTime, concoreentesIds ?? new List<int>(), textSearch));
        }
    }
}
