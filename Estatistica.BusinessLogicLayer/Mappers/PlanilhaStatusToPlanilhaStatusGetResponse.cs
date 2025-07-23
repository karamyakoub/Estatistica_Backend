using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.Extensions;
using Estatistica.BusinessLogicLayer.Utils;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class PlanilhaStatusToPlanilhaStatusGetResponse : Profile
    {
        public PlanilhaStatusToPlanilhaStatusGetResponse()
        {
            CreateMap<PlanilhaStatus, PlanilhaStatusGetResponse>()
                .ForMember(dst => dst.DataInclusao, opt => opt.MapFrom(x => x.DataInclusao.ToString("dd/MM/yyyy HH:mm")));    
        }
    }
}
