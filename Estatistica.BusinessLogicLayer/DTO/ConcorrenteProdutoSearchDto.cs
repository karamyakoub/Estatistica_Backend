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
    public class ConcorrenteProdutoSearchDto
    {
        public int Id { get; set; }        
        public string? Concorrente { get; set; }        
        public required string CodigoProdutoConcorrente { get; set; }        
        public required string DescricaoProdutoConcorrente { get; set; }        
        public string? UnidadeProdutoConcorrente { get; set; }        
        public string? CodigoBarraConcorrente { get; set; }        
        public string? UsuarioCadastro { get; set; }
        public required string CodigoProduto { get; set; }
        public string? CodigoFabrica { get; set; }
        public string? CodigoBarra { get; set; }                
        public string? Descricao { get; set; }                
        public string? Fabricante { get; set; }                
        public int? CodigoMarca { get; set; }                
        public string? DescricaoMarca { get; set; }                
        public string? Tipo { get; set; }                
        public string? SubTipo { get; set; }                
        public string? Linha { get; set; }                
        public string? Familia { get; set; }                
        public string? Unidade { get; set; }

    }
}
