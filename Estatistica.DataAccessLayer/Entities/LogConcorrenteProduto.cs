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
    public class LogConcorrenteProduto : CreationAuditableEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("conId")]
        public required Concorrente Concorrente { get; set; }
        [Column("codProdCon")]
        public required ConcorrenteProduto CodigoProdutoConcorrente { get; set; }
        [Column("codigoProdutoAnt")]
        public Produto? CodigoProdutoAnt { get; set; }
        [Column("descProdConAnt")]
        public required string DescricaoProdutoConcorrenteAnt { get; set; }
        [Column("descProdConAtual")]
        public required string DescricaoProdutoConcorrenteAtual { get; set; }
        [Column("unProdConAnt")]
        public string? UnidadeProdutoConcorrenteAnt { get; set; }
        [Column("unProdConAtual")]
        public string? UnidadeProdutoConcorrenteAtual { get; set; }
        [Column("codBarraConAnt")]
        public string? CodigoBarraConcorrenteAnt { get; set; }
        [Column("codBarraConAtual")]
        public string? CodigoBarraConcorrenteAtual { get; set; }
        [Column("obs")]
        [MaxLength(50)]
        public string? Observacao { get; set; }
    }
}
