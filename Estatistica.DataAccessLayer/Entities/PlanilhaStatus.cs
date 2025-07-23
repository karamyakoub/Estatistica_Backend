using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    public class PlanilhaStatus
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("planilhaId")]
        public required Planilha Planhila { get; set; }
        [Column("situacao")]
        public required int Situacao { get; set; }
        [Column("obs")]
        [MaxLength(100)]
        public string? Obs { get; set; }
        [Column("dtInclusao")]
        public required DateTime DataInclusao { get; set; }
    }
}
