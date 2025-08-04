using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public record ProductSuggestionResponse(string? CodigoProduto, string? Descricao,string? Fabricante)
    {
        public ProductSuggestionResponse() : this(default, default, default)
        {
            
        }
    }
}
