using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record PlanilhaGetResponse(int Id, string? NomePlanilha, DateTime? DataCadastro, string? NomeUsuarioCadastro,string? Status)
    {
        public PlanilhaGetResponse() : this(default,default, default, default, default)
        {
            
        }
    }

}
