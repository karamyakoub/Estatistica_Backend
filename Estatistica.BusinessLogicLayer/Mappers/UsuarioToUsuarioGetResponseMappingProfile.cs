using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class UsuarioToUsuarioGetResponseMappingProfile : Profile
    {
        public UsuarioToUsuarioGetResponseMappingProfile()
        {
            CreateMap<IdentityUser, UsuarioGetResponse>()
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => !(src.LockoutEnabled && src.LockoutEnd.HasValue && src.LockoutEnd.Value > DateTimeOffset.Now)))
                .ReverseMap();
        }
    }
}
