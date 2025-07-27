using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.WebAPI.Attribuites;
using Estatistica.WebAPI.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/planilha")]
    [Authorize(Roles = "Admin")]
    public class PlanilhaController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly IConfiguration configuration;
        private readonly IPlanilhaService planilhaService;

        public PlanilhaController(IFileService fileService, IConfiguration configuration, IPlanilhaService planilhaService)
        {
            this.fileService = fileService;
            this.configuration=configuration;
            this.planilhaService=planilhaService;
        }


        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [MultipartFormData]
        [DisableFormValueModelBinding]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadPlanilha()
        {
            var dir = configuration.GetSection("AppConfigs")["PlanilhaExcelDirectory"];
            var fileUploadSummary = await fileService.UploadPlanilha(Request.Body, Request.ContentType!, [".xlsx", ".xls"], dir!);
            return CreatedAtAction(nameof(UploadPlanilha), fileUploadSummary);
        }

        [HttpGet]
        public async Task<IActionResult> GetPlanilhas([FromQuery, Required] int pageSize, [FromQuery, Required] int pageCount, [FromQuery, Required] OrderByEnum orderBy)
        {
            var planilhas = await planilhaService.GetPlanilhas(pageSize, pageCount, orderBy);
            return Ok(planilhas);
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetPlanilhaStatus([FromRoute] int id)
        {
            return Ok(await planilhaService.GetPlanilhaStatus(id));
        }

        [HttpGet("{id}/filiais-temp")]
        public async Task<IActionResult> GetFiliasConcorrenteTemp([FromRoute] int id)
        {
            return Ok(await planilhaService.GetConcorrenteFilialTempByPlanilhaId(id));
        }

        [HttpPut("{id}/filiais-temp")]
        public async Task<IActionResult> UpdateFiliasConcorrenteTemp([FromRoute] int id, [FromBody] List<ConcorrenteFilialTempUpdateRequest> concorrenteFilialTemps)
        {
            await planilhaService.UpdateConcorrenteFilialTempRange(id, concorrenteFilialTemps);            
            await planilhaService.UpdatePlanilhaStatus(id, PlanilhaStatusEnum.AguardandoProcessamento, "Aguardando o processamento.");
            return Ok();
        }

        /*[HttpPut("{id}/reprocessa")]
        public async Task<IActionResult> ReprocessPlanilha([FromRoute] int id)
        {
            await planilhaService.UpdatePlanilhaStatus(id, PlanilhaStatusEnum.AguardandoProcessamento, "Renviar planilha para processamento");
            return NoContent();
        }*/
    }
}
