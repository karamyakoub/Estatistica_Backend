using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/estatistica")]
    [Authorize(Roles = "Admin")]
    public class EstatisticaController : ControllerBase
    {
        private readonly IStatitsticService statitsticService;

        public EstatisticaController(IStatitsticService statitsticService)
        {
            this.statitsticService=statitsticService;
        }
        [HttpPut("atualiza-qtde-nfi")]
        public async Task<IActionResult> UpdateNfiCorrectionCount(string id, int count)
        {
            var result = await statitsticService.UpdateNfiCorrectionCount(id, count);
            if (result) return Ok();
            return BadRequest("Erro ao atualizar a quantidade da embalagem da NFI.");
        }

        [HttpGet("resultado-estatistica")]
        public async Task<IActionResult> GetStatisticResult([FromQuery] string? concorrenteId, [FromQuery] string? dtEmissaoIni, [FromQuery] string? dtEmissaoFin, [FromQuery] string? desconto, string? fabricante)
        {
            DateTime.TryParseExact(dtEmissaoIni, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime firstDateFilter);
            DateTime.TryParseExact(dtEmissaoFin, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateFilter);
            decimal.TryParse(desconto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal descontoNumero);
            var concorrenteIdList = string.IsNullOrWhiteSpace(concorrenteId) ? null : concorrenteId.Split(',').Select(id => Convert.ToInt32(id.Trim())).ToList();
            var result = await statitsticService.GetStatisticResult(concorrenteIdList, firstDateFilter, endDateFilter, descontoNumero, fabricante);
            return Ok(result);
        }
    }
}
