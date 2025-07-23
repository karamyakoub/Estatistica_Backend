using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estatistica.DataAccessLayer.Entities
{    
    public class ConcorrenteProduto : FullAuditableEntity
    {
        [Key]
        public string Id
        {
            get => $"{Concorrente?.Id}{CodigoProdutoConcorrente}";
            private set { }
        }
        [Column("conId")]
        public required Concorrente Concorrente { get; set; }
        [Column("codProdCon")]
        public required string CodigoProdutoConcorrente { get; set; }
        [Column("descProdCon")]
        public required string DescricaoProdutoConcorrente { get; set; }
        [Column("unProdCon")]
        public string? UnidadeProdutoConcorrente { get; set; }
        [Column("codBarraCon")]
        public string? CodigoBarraConcorrente { get; set; }       
        [Column("codProd")]
        public Produto? Produto { get; set; }
        [Column("planilhaId")]
        public Planilha? Planilha { get; set; }

    }
}
