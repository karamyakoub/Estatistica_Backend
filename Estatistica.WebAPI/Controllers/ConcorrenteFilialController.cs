using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/concorrentefilial")]
    public class ConcorrenteFilialController : ControllerBase
    {
        private readonly IConcorrenteFilialService concorrenteFilialService;
        private readonly IPlanilhaService planilhaService;

        public ConcorrenteFilialController(IConcorrenteFilialService concorrenteFilialService,IPlanilhaService planilhaService)
        {
            this.concorrenteFilialService = concorrenteFilialService;
            this.planilhaService=planilhaService;
        }


        [HttpGet]
        public async Task<IActionResult> GetConcorrentesFiliais()
        {
            var concorrentesFiliais = await concorrenteFilialService.GetConcorrentesFiliais();
            return Ok(concorrentesFiliais);
        }

        [HttpGet("concorrente/{idConcorrente}")]
        public async Task<IActionResult> GetConcorrenteFilialByConcorrente([FromRoute] int idConcorrente)
        {
            var concorrenteFilial = await concorrenteFilialService.GetConcorrenteFiliaisByConcorrente(idConcorrente);
            if (concorrenteFilial is null)
                return NotFound();
            return Ok(concorrenteFilial);
        }

        [HttpPost]
        public async Task<IActionResult> AddConcorrenteFilial([FromBody] ConcorrenteFilialAddUpdateRequest dto)
        {
            var concorrenteFilial = await concorrenteFilialService.AddConcorrenteFilial(dto.Cnpj, dto.IdConcorrente);
            return Ok(concorrenteFilial);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateConcorrenteFilial([FromBody] ConcorrenteFilialAddUpdateRequest dto)
        {
            var concorrenteFilial = await concorrenteFilialService.UpdateConcorrenteFilial(dto.Cnpj, dto.IdConcorrente);
            if (concorrenteFilial is null)
                return NotFound();
            return Ok(concorrenteFilial);
        }

        [HttpGet("lista-pendentes")]
        public async Task<IActionResult> GetConcorrenteFilialPendentes()
        {
            var concorrenteFilialPendentes = await concorrenteFilialService.GetConcorrenteFilialPendentesAgrupado();
            return Ok(concorrenteFilialPendentes);
        }

        [HttpPut("pendente")]
        public async Task<IActionResult> UpdateConcorrenteFilialPendente([FromBody] ConcorrenteFilailPendenteUpdateRequest dto)
        {
            await concorrenteFilialService.UpdateConcorrenteFilialPendente(dto.Cnpj, dto.IdConcorrente);
            var concorrenteFilial = await concorrenteFilialService.AddConcorrenteFilial(dto.Cnpj, dto.IdConcorrente);
            if (concorrenteFilial is null)
                return BadRequest("Não foi possivo criar o vinculo");
            await planilhaService.CheckAndUpdatePlanilhaStatus();
            return Ok(concorrenteFilial);
        }
    }
}
