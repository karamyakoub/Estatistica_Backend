using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Extensions;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class PlaniliaToPlanilhaGetResponseMappingProfile : Profile
    {
        public PlaniliaToPlanilhaGetResponseMappingProfile()
        {
            CreateMap<Planilha, PlanilhaGetResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((Enums.PlanilhaStatusEnum)src.Status).ToString().PascalCaseToStringWithSpaces()))
                .ForMember(dest => dest.NomeUsuarioCadastro, opt => opt.MapFrom(src => src.UsuarioCadastro))
                .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DataCadastro!.Value.Date))
                .ReverseMap();
        }
    }
}
