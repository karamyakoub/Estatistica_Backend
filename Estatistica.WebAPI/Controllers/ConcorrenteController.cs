using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/concorrente")]
    public class ConcorrenteController : ControllerBase
    {
        private readonly IConcorrenteService concorrenteService;

        public ConcorrenteController(IConcorrenteService concorrenteService)
        {
            this.concorrenteService = concorrenteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetConcorrentes(OrderByEnum orderBy)
        {
            return Ok(await concorrenteService.GetConcorrentes(orderBy));
        }

        [HttpPost]
        public async Task<IActionResult> CreateConcorrente(ConcorrenteAddUpdateRequest dto)
        {
            var id = await concorrenteService.AddConcorrente(dto.ConcorrenteNome!);
            return Ok(new { id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConcorrenteNome([FromRoute] int id, ConcorrenteAddUpdateRequest dto)
        {
            var concorrente = await concorrenteService.UpdateConcorrenteNome(id, dto.ConcorrenteNome!);
            if (concorrente is null)
                return NotFound();
            return Ok(concorrente);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConcorrenteById([FromRoute] int id)
        {
            var concorrente = await concorrenteService.GetConcorrenteById(id);
            if (concorrente is null)
                return NotFound();
            return Ok(concorrente);
        }
    }
}
