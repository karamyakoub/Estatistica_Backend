using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class SearchProductPriceResponse
    {
        public string? Id { get; set; }
        public string? CodigoProduto { get; set; }
        public string? DescricaoProduto { get; set; }
        public bool? IsLinked => string.IsNullOrEmpty(CodigoProduto);
        public string? CodigoProdutoConcorrente { get; set; }
        public string? DescricaoProdutoConcorrente { get; set; }
        public string? Fabricante { get; set; }
        public string? Unidade { get; set; }
        public string? Concorrente { get; set; }
        public string? DataEmissao { get; set; }
        public int? Qtd { get; set; }
        public decimal? Valor { get; set; }

    }
}
