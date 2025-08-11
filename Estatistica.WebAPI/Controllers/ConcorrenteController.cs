using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Estatistica.WebAPI.Controllers
{
    
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
        [Authorize]
        public async Task<IActionResult> GetConcorrentes(OrderByEnum orderBy)
        {
            return Ok(await concorrenteService.GetConcorrentes(orderBy));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateConcorrente(ConcorrenteAddUpdateRequest dto)
        {
            var id = await concorrenteService.AddConcorrente(dto.ConcorrenteNome!);
            return Ok(new { id = id });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateConcorrenteNome([FromRoute] int id, ConcorrenteAddUpdateRequest dto)
        {
            var concorrente = await concorrenteService.UpdateConcorrenteNome(id, dto.ConcorrenteNome!);
            if (concorrente is null)
                return NotFound();
            return Ok(concorrente);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetConcorrenteById([FromRoute] int id)
        {
            var concorrente = await concorrenteService.GetConcorrenteById(id);
            if (concorrente is null)
                return NotFound();
            return Ok(concorrente);
        }
    }
}
