using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO;

public class ConcorrenteAddUpdateRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "ConcorrenteNome e obrigatorio")]
    public string? ConcorrenteNome { get; set; }
}


