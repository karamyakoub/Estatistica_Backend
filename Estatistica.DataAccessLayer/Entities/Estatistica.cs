using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Entities
{
    [NotMapped]
    public class VW_Estatistica
    {
        public string? Id { get; set; }
        public int? ConcorrenteId { get; set; }
        public string? ConcorrenteNome { get; set; }
        public string? ConcorrenteCnpj { get; set; }
        public string? ChaveNfe { get; set; }
        public DateTime DataEmissao { get; set; }
        public string? CnpjCliente { get; set; }
        public string? NomeCliente { get; set; }
        public string? ConcorrenteProdutoCodigo { get; set; }
        public string? ConcorrenteProdutoDescricao { get; set; }
        public string? ConcorrenteProdutoUnidade { get; set; }
        public decimal? Qtde { get; set; }
        public decimal? ConcorrenteProdutoValorOriginal { get; set; }
        public decimal? ConcorrenteProdutoValorCorregido { get; set; }
        public decimal? ConcorrenteValorTotal { get; set; }
        public string? HouveCorrecao { get; set; }
        public string? CodigoProduto { get; set; }
        public string? CodigoFabrica { get; set; }
        public string? CodigoBarra { get; set; }
        public string? Descricao { get; set; }
        public string? Fabricante { get; set; }
        public string? Tipo { get; set; }
        public string? SubTipo { get; set; }
        public string? Linha { get; set; }
        public string? Famila { get; set; }
        public string? Unidade { get; set; }
        public decimal? Custo { get; set; }
        public decimal? CustoTotal { get; set; }
        public decimal? Pvenda { get; set; }
        public decimal? PvendaTotal { get; set; }
        public decimal? PercFrete { get; set; }
        public decimal? PvendaComFrete { get; set; }
        public decimal? PvendaTotalComFrete { get; set; }
    }
}
