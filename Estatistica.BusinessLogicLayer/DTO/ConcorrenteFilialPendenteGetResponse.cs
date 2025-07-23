using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record ConcorrenteFilialPendenteGetResponse(int Id,string? Cnpj,string? Nome)
    {
        public ConcorrenteFilialPendenteGetResponse() : this(default,default,default)
        {

        }        
    }
}
