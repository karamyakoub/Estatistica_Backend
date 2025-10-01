using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Estatistica.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class IdentityExtensionController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;
        public IdentityExtensionController(IUsuarioService usuarioService)
        {
            this.usuarioService=usuarioService;            
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetUsuarioLogado()
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId is null)
                return NotFound("Usuario nao encontrado");            
            return Ok(await usuarioService.GetUsuarioById(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UsuarioRegisterAddRequest usuarioRegisterAddRequest)
        {            
            var usuario = await usuarioService.Registrar(usuarioRegisterAddRequest);
            return CreatedAtAction(nameof(GetUsuarioLogado), new { userId = usuario.Id }, usuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/activate")]
        public async Task<IActionResult> ActivateUser([FromRoute] string userId)
        {
            return Ok(await usuarioService.ChangeUserStatus(userId, true));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/deactivate")]
        public async Task<IActionResult> DeactivateUser([FromRoute] string userId)
        {
            return Ok(await usuarioService.ChangeUserStatus(userId, false));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(await usuarioService.GetUsuarios());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/resetpassword")]
        public async Task<IActionResult> UpdateUserPassword([FromRoute]string userId, [FromQuery] string passwordBase64)
        {
            var password = Encoding.UTF8.GetString(Convert.FromBase64String(passwordBase64));
            return Ok(await usuarioService.ChangeUserPassword(userId, password));
        }
    }
}
