using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Data
{
    public class ProdutoConsulta
    {
        public List<object>? excluidos { get; set; }
        public List<Content>? content { get; set; }
        public int total { get; set; }
        public bool lastPage { get; set; }


        public class Caracteristicas
        {
            public double litros { get; set; }
            public double largura { get; set; }
            public double altura { get; set; }
            public double profundidade { get; set; }
            public double embalagem { get; set; }
            public string? cor { get; set; }
            public string? modelo { get; set; }
            public string? voltagem { get; set; }
        }

        public class Categoria
        {
            public int codigo { get; set; }
            public string? descricao { get; set; }
        }

        public class Content
        {
            public int produtoPaiId { get; set; }
            public int cod { get; set; }
            public string? desc { get; set; }
            public string? um { get; set; }
            public string? umEcommerce { get; set; }
            public string? codBarras { get; set; }
            public string? codFabrica { get; set; }
            public string? referencia { get; set; }
            public double peso { get; set; }
            public string? inat { get; set; }
            public string? dropShipping { get; set; }
            public string? referenciaProduto { get; set; }
            public string? referenciaSku { get; set; }
            public string? ficha { get; set; }
            public string? priorizaDeposito { get; set; }
            public string? tituloDaPagina { get; set; }
            public string? palavrasChave { get; set; }
            public string? descricaoLonga { get; set; }
            public string? descricaoCurta { get; set; }
            public string? termosPesquisa { get; set; }
            public List<object> midias { get; set; }
            public string? ncm { get; set; }
            public string? norma { get; set; }
            public string? cest { get; set; }
            public string? data { get; set; }
            public string? hora { get; set; }
            public string? descricaoEcom { get; set; }
            public Caracteristicas? caracteristicas { get; set; }
            public Divisaogerencial? divisaogerencial { get; set; }
            public Marca? marca { get; set; }
            public Departamento departamento { get; set; }
            public Categoria? categoria { get; set; }
        }

        public class Departamento
        {
            public int codigo { get; set; }
            public string? descricao { get; set; }
        }

        public class Divisaogerencial
        {
            public string? fabricante { get; set; }
            public Tipo? tipo { get; set; }
            public Subtipo? subtipo { get; set; }
            public Linha? linha { get; set; }
            public Familia? familia { get; set; }
            public Marca? marca { get; set; }
            public int codMaster { get; set; }
            public string? descMaster { get; set; }
        }

        public class Familia
        {
            public string? codigo { get; set; }
            public string? descricao { get; set; }
        }

        public class Linha
        {
            public string? codigo { get; set; }
            public string? descricao { get; set; }
        }

        public class Marca
        {
            public int? codigo { get; set; }
            public string? descricao { get; set; }
        }

        public class Subtipo
        {
            public string? codigo { get; set; }
            public string? descricao { get; set; }
        }

        public class Tipo
        {
            public string? codigo { get; set; }
            public string? descricao { get; set; }
        }

    }    
}
