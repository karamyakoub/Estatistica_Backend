using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FreteController : ControllerBase
    {
        private readonly IFreteService freteService;

        public FreteController(IFreteService freteService)
        {
            this.freteService=freteService;
        }

        [HttpPost("AddUpdateFrete")]
        public async Task<IActionResult> AddUpdateFrete(string setor, string codigoMunicipio, decimal percentualFrete)
        {
            if (percentualFrete < 0)
                return BadRequest("Percentual de frete invalido");
            await freteService.AddUpdateFrete(setor, codigoMunicipio, percentualFrete);
            return Ok($"Frete do setor {setor} e municipio {codigoMunicipio} atualizado com sucesso.");
        }
    }
}
