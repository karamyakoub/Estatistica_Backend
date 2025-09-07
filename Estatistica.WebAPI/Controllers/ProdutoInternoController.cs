using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProdutoInternoController : ControllerBase
    {
        private readonly IProdutoService produtoService;

        public ProdutoInternoController(IProdutoService produtoService)
        {
            this.produtoService=produtoService;
        }

        [HttpPut("UpdateProdutoPrice")]
        public async Task<IActionResult> UpdateProdutoPrice(string codigoProduto, decimal price, decimal custo)
        {
            if (price < 0 || custo < 0)
                return BadRequest("Preco invalido");
            await produtoService.UpdateProdutoPrice(codigoProduto, price, custo);
            return Ok($"Produto {codigoProduto} atualizado com sucesso.");
        }
    }
}
