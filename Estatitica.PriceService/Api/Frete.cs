using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatitica.PriceService.Api
{
    internal class Frete
    {
        public string? Setor { get; set; }
        public string? CodigoMunicipio { get; set; }
        public string? PercentualFrete { get; set; }
    }
}
