using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IMapper mapper;
        private readonly UserManager<IdentityUser> userManager;

        public UsuarioService(IUsuarioRepository usuarioRepository,IMapper mapper,UserManager<IdentityUser> userManager)
        {
            this.usuarioRepository=usuarioRepository;
            this.mapper=mapper;
            this.userManager=userManager;
        }

        public async Task<bool> ChangeUserPassword(string userId, string newPassword)
        {
            var user = await usuarioRepository.GetUsuarioById(userId);
            if (user is null)
                return false;
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ChangeUserStatus(string userId, bool isActive)
        {
            return await usuarioRepository.ChangeUserStatus(userId,isActive);
        }

        public async Task<UsuarioGetResponse?> GetUsuarioById(string userId)
        {                        
            var loggedinUser = await usuarioRepository.GetUsuarioById(userId);
            if (loggedinUser is null)
                return null;
            var userDto = mapper.Map<UsuarioGetResponse>(loggedinUser);
            userDto.Role = (await usuarioRepository.GetUserRolesByUserId(new Guid(loggedinUser.Id))).FirstOrDefault()?.Name;
            return userDto;
        }

        public async Task<IEnumerable<UsuarioGetResponse>> GetUsuarios()
        {
            var usuariosDto = mapper.Map<IEnumerable<UsuarioGetResponse>>(await usuarioRepository.GetUsers());
            foreach (var usuario in usuariosDto)                
                usuario.Role =  (await usuarioRepository.GetUserRolesByUserId(usuario.Id)).FirstOrDefault()?.Name;
            return usuariosDto;
        }

        public async Task<UsuarioGetResponse> Registrar(UsuarioRegisterAddRequest usuarioRegisterAddRequest)
        {
            //check user already exists
            var usuarioDb = await usuarioRepository.GetUsuarioByUserName(usuarioRegisterAddRequest.Email);
            if(usuarioDb is not null)
                throw new InvalidOperationException("Usuario ja existe");
            var usuario = new IdentityUser
            {
                Email = usuarioRegisterAddRequest.Email,
                UserName = usuarioRegisterAddRequest.Email
            };

            usuario = await usuarioRepository.AddUser(usuario,usuarioRegisterAddRequest.Senha);

            await usuarioRepository.AddRoleToUser(usuarioRegisterAddRequest.UserProfile, usuario);

            return mapper.Map<UsuarioGetResponse>(usuario);
        }
    }
}
