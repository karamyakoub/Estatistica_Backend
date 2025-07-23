using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    public class ConcorrenteFilial : FullAuditableEntity
    {
        [Key]
        [MaxLength(14)]
        [Column("cnpj")]
        public required string Cnpj { get; set; }
        [Column("conId")]       
        public required Concorrente Concorrente { get; set; }
       
    }
}
