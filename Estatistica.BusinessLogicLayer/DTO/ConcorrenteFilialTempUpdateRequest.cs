using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record ConcorrenteFilialTempUpdateRequest(string? Cnpj, bool Incluido)
    {
        public ConcorrenteFilialTempUpdateRequest() : this(default,default)
        {
            
        }
    }
    
}
