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
    public class Nfc : CreationAuditableEntity
    {
        [Key]
        [Column("chaveNfe")]
        public required string ChaveNfe { get; set; }        
        [Column("cnpjCon")]
        [MaxLength(14)]        
        public required ConcorrenteFilial ConcorrenteCnpj { get; set; }
        [Column("dtEmissao")]        
        public DateTime? DataEmissao { get; set; }
        [Column("cnpjCliente")]
        [MaxLength(14)]
        public string? CnpjCliente { get; set; }
        [MaxLength(100)]
        [Column("nomeCliente")]
        public string? NomeCliente { get; set; }
        [Column("planilhaId")]
        public Planilha? Planilha { get; set; }
    }
}
