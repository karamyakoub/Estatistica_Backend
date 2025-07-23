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
    public class ConcorrenteFilialTempUpdateRequestToConcorrenteFilialTempMappingProfile : Profile
    {
        public ConcorrenteFilialTempUpdateRequestToConcorrenteFilialTempMappingProfile()
        {
            CreateMap<ConcorrenteFilialTempUpdateRequest, ConcorrenteFilialTemp>()
                .ReverseMap();
                
        }
    }
}
