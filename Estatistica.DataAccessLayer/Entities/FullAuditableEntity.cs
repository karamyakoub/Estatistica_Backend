using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    public abstract class FullAuditableEntity : CreationAuditableEntity
    {
        [Column("dtAlter")]
        public DateTime? DataAlteracao { get; set; }
        [Column("usuAlter")]
        public string? UsuarioAlteracao { get; set; }
    }
}
