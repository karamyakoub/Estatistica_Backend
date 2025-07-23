using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class UsuarioToUsuarioListaGetReposneMappingProfile : Profile
    {
        public UsuarioToUsuarioListaGetReposneMappingProfile()
        {
            CreateMap<IdentityUser, UsuarioListaGetResposne>()
                .ReverseMap();
        }
    }
}
