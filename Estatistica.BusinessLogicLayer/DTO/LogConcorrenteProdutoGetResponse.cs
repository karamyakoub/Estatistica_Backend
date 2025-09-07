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
    public class LogConcorrenteProdutoGetResponse
    {
        public int? Id { get; set; }
        public string? Concorrente { get; set; }
        public string? CodigoProdutoConcorrente { get; set; }
        public string? CodigoProdutoAnt { get; set; }
        public string? CodigoProdutoAtual { get; set; }
        public string? DescricaoProdutoConcorrenteAnt { get; set; }
        public string? DescricaoProdutoConcorrenteAtual { get; set; }
        public string? UnidadeProdutoConcorrenteAnt { get; set; }
        public string? UnidadeProdutoConcorrenteAtual { get; set; }
        public string? CodigoBarraConcorrenteAnt { get; set; }
        public string? CodigoBarraConcorrenteAtual { get; set; }
        public string? Observacao { get; set; }
        public string? UsuarioId { get; set; }
        public string? Usuario { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
