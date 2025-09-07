using Estatistica.BusinessLogicLayer.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;

namespace Estatistica.WebAPI.Controllers
{
    [ApiController]
    [Route("api/nfexml")]
    [Authorize(Roles = "Admin")]
    public class NfeXmlController : ControllerBase
    {
        private readonly INfeXmlService nfeXmlService;

        public NfeXmlController(INfeXmlService nfeXmlService)
        {
            this.nfeXmlService=nfeXmlService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadNfeXml(IFormFile xmlFile)
        {
            if (xmlFile is null || xmlFile.Length == 0)
                return BadRequest("Arquivo invalido");
            using (var stream = new MemoryStream())
            {
                await xmlFile.CopyToAsync(stream);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                var xmlContent = await reader.ReadToEndAsync();
                var isValid = nfeXmlService.IsValidNfe(xmlContent!);
                if (!isValid)
                    return BadRequest("Arquivo invalido");

                var result = await nfeXmlService.CheckFilialPendente(xmlContent!);
                if (result.GetType() == typeof(int) && result == 0)
                {
                    await nfeXmlService.ImportNfeXml(xmlContent!);
                    return Created();
                }
                return Ok(result);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetNfcXmlLista(string? startDate, string? endDate)
        {
            DateTime.TryParseExact(startDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDateFilter);
            DateTime.TryParseExact(endDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDateFilter);
            return Ok(await nfeXmlService.GetNfcXmlByPeriod(startDateFilter, endDateFilter));
        }
    }
}
