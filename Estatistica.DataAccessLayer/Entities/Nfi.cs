using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{    
    public class Nfi : CreationAuditableEntity
    {
        [Key]
        public string Id
        {
            get => $"{Nfc?.ChaveNfe}{ConcorrenteProduto.CodigoProdutoConcorrente}";
            private set { }
        }
        [Column("codProd")]        
        public required ConcorrenteProduto ConcorrenteProduto { get; set; }
        [Column("chaveNfe")]        
        public required Nfc Nfc { get; set; }
        [Column("qtde")]
        public int Qtde { get; set; }
        [Column("valor")]
        public decimal Valor { get; set; }
        [Column("ufOrigin")]
        public string? UfOrigin { get; set; }
        [Column("ufDestino")]
        public string? UfDestino { get; set; }
        [Column("codBarra")]
        public string? CodigoBarra { get; set; }
        [Column("unidade")]
        public string? Unidade { get; set; }       

    }
}
