using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class VW_EstatisticaToStaisticDTOMappingProfile : Profile
    {
        public VW_EstatisticaToStaisticDTOMappingProfile()
        {
            CreateMap<VW_Estatistica, StatistcDTO>()
                .ForMember(opt => opt.DataEmissao, act => act.MapFrom(src => src.DataEmissao.ToString("yyyy-MM-dd")))
                .ReverseMap();
        }
    }
}
