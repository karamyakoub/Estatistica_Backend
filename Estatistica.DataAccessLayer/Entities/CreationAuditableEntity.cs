using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    public abstract class CreationAuditableEntity
    {
        [Column("dtCadastro")]
        public DateTime? DataCadastro { get; set; }
        
        [Column("usuCadastro")]
        public string? UsuarioCadastro { get; set; }

    }
}
