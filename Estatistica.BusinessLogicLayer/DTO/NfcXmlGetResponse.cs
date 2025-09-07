using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.DTO
{
    public class NfcXmlGetResponse
    {
        public string? ChaveNfe { get; set; }                
        public string? Concorrente { get; set; }        
        public DateTime? DataEmissao { get; set; }                
        public string? CnpjCliente { get; set; }        
        public string? NomeCliente { get; set; }        
        public string? IdUsuario { get; set; }
        public string? Usuario { get; set; }
        public DateTime? DataInclusao { get; set; }
    }
}
