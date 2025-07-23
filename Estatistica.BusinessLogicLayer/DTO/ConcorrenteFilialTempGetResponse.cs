using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record ConcorrenteFilialTempGetResponse(string? Cnpj,string? Nome,bool? Incluido)
    {
        public ConcorrenteFilialTempGetResponse() : this(default,default,default)
        {
            
        }
    }
}
