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
    public class Planilha : CreationAuditableEntity
    {
        [Key]
        [Column("planilhaId")]
        public int Id { get; set; }
        [Column("nomePlanilha")]
        public string? NomePlanilha { get; set; }
        [Column("caminho")]
        public string? Caminho { get; set; }
        [Column("status")]
        public required int Status { get; set; }      
    }
}
