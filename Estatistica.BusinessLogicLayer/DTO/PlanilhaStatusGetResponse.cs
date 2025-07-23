using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO;

public class PlanilhaStatusGetResponse()
{
    public int Id {get;set;}
    public string? Situacao {get;set;}
    public string? Obs {get;set;}
    public string DataInclusao { get; set; }
}
