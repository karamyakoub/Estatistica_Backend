using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    public class Frete
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("setor")]
        public required string Setor { get; set; }
        [Column("CodMuni")]
        public required string CodMunicipio { get; set; }
        [Column("percFrete")]
        public required decimal PercentualFrete { get; set; }
    }
}
