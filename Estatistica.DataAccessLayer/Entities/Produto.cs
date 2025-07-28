using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    public class Produto
    {
        [Key]
        [MaxLength(15)]
        [Column("codProd")]
        public required string CodigoProduto { get; set; }
        [MaxLength(50)]
        [Column("codFab")]
        public string? CodigoFabrica { get; set; }
        [MaxLength(50)]
        [Column("codBarra")]
        public string? CodigoBarra { get; set; }
        [Column("descricao")]
        [MaxLength(120)]
        public string? Descricao { get; set; }
        [Column("fabricante")]
        [MaxLength(120)]
        public string? Fabricante { get; set; }
        [Column("codMarca")]
        [MaxLength(10)]
        public int? CodigoMarca { get; set; }
        [Column("descMarca")]
        [MaxLength(120)]
        public string? DescricaoMarca { get; set; }
        [Column("tipo")]
        [MaxLength(120)]
        public string? Tipo { get; set; }
        [Column("subtipo")]
        [MaxLength(120)]
        public string? SubTipo { get; set; }
        [Column("linha")]
        [MaxLength(120)]
        public string? Linha { get; set; }
        [Column("familia")]
        [MaxLength(120)]
        public string? Familia { get; set; }
        [Column("unidade")]
        [MaxLength(4)]
        public string? Unidade { get; set; }
        [Column("dtCadastro")]
        public DateTime DataCadastro { get; set; }

    }
}
