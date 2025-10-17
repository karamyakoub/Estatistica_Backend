using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Estatistica.WebAPI.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    //public class ProdutoInternoController : ControllerBase
    //{
    //    private readonly IProdutoService produtoService;

    //    public ProdutoInternoController(IProdutoService produtoService)
    //    {
    //        this.produtoService=produtoService;
    //    }

    //    [HttpPut("UpdateProdutoPrice")]
    //    public async Task<IActionResult> UpdateProdutoPrice(string codigoProduto, string price, string custo)
    //    {
    //        decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal priceParsed);
    //        decimal.TryParse(price, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal custoParsed);
    //        if (priceParsed <= 0 || custoParsed <= 0)
    //            return BadRequest("Preco invalido");
    //        await produtoService.UpdateProdutoPrice(codigoProduto, priceParsed, custoParsed);
    //        return Ok($"Produto {codigoProduto} atualizado com sucesso.");
    //    }
    //}
}
