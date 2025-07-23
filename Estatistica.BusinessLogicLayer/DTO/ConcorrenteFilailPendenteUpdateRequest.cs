using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class ConcorrenteFilailPendenteUpdateRequest
    {
        [Required]
        public required string Cnpj { get; set; }
        [Required]
        public int IdConcorrente { get; set; }
    }
}
