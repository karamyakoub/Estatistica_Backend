using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record ConcorrenteFilialGetResponse(string? Cnpj, int IdConcorrente)
    {
        public ConcorrenteFilialGetResponse() : this(default, default)
        {

        }
    }
}
