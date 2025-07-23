using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{    
    public class ConcorrenteFilialTemp
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("cnpj")]
        [StringLength(14)]
        public required string Cnpj { get; set; }
        [Column("nome")]
        public required string Nome { get; set; }
        [Column("planilhaId")]
        public Planilha? Planilha { get; set; }
        [Column("incluido")]
        public bool Incluido { get; set; } = false;
    }
}
