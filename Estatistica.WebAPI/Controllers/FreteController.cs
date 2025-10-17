using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Estatistica.WebAPI.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    //public class FreteController : ControllerBase
    //{
    //    private readonly IFreteService freteService;

    //    public FreteController(IFreteService freteService)
    //    {
    //        this.freteService=freteService;
    //    }

    //    [HttpPost("AddUpdateFrete")]
    //    public async Task<IActionResult> AddUpdateFrete(string setor, string codigoMunicipio, string percentualFrete)
    //    {
    //        decimal.TryParse(percentualFrete, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal percentualFreteDecimal);
    //        if (percentualFreteDecimal <= 0)
    //            return BadRequest("Percentual de frete invalido");
    //        await freteService.AddUpdateFrete(setor, codigoMunicipio, percentualFreteDecimal);
    //        return Ok($"Frete do setor {setor} e municipio {codigoMunicipio} atualizado com sucesso.");
    //    }
    //}
}
