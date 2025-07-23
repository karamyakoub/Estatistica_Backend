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
    public class Concorrente : FullAuditableEntity
    {
        [Key]
        [Column("conId")]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [Column("nome")]
        public required string Nome { get; set; }

       
    }
}
