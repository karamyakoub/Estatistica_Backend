using Estatistica.BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IUsuarioService
    {
        Task<UsuarioGetResponse?> GetUsuarioById(string userId);
        Task<bool> ChangeUserStatus(string userId, bool isActive);
        Task<bool> ChangeUserPassword(string userId, string newPassword);
        Task<UsuarioGetResponse> Registrar(UsuarioRegisterAddRequest usuarioRegisterAddRequest);
        Task<IEnumerable<UsuarioGetResponse>> GetUsuarios();
    }
}
