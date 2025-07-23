using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class ConcorrenteFilialAddUpdateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cnpj e obrigatorio")]
        [MaxLength(14)]
        public required string Cnpj { get; set; }
        [Required(ErrorMessage = "IdConcorrente e obrigatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "IdConcorrente deve ser maior que zero")]
        public int IdConcorrente { get; set; }
    }
}
