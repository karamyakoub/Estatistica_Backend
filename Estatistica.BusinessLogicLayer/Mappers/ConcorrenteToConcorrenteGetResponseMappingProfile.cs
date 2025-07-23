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
    public class ConcorrenteToConcorrenteGetResponseMappingProfile : Profile
    {
        public ConcorrenteToConcorrenteGetResponseMappingProfile()
        {
            CreateMap<Concorrente, ConcorrenteGetResponse>()
                .ReverseMap();
        }
    }
}
