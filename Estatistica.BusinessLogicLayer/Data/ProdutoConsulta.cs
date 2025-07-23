using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Data
{
    public class ProdutoApiConsulta
    {
        public string status { get; set; }
        public Retorno retorno { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Caracteristicas
    {
        public decimal litros { get; set; }
        public decimal largura { get; set; }
        public decimal altura { get; set; }
        public decimal profundidade { get; set; }
        public decimal embalagem { get; set; }
        public string cor { get; set; }
        public string modelo { get; set; }
        public string voltagem { get; set; }
    }

    public class Categoria
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Departamento
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Divisaogerencial
    {
        public string fabricante { get; set; }
        public Tipo tipo { get; set; }
        public Subtipo subtipo { get; set; }
        public Linha linha { get; set; }
        public Familia familia { get; set; }
        public Marca marca { get; set; }
        public int codMaster { get; set; }
        public string descMaster { get; set; }
    }

    public class Familia
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Linha
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Marca
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Paginacao
    {
        public int pagina { get; set; }
        public int totalPaginas { get; set; }
    }

    public class Perfil
    {
        public int codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Produto
    {
        public int cod { get; set; }
        public string desc { get; set; }
        public string um { get; set; }
        public string umEcommerce { get; set; }
        public string codBarras { get; set; }
        public string codFabrica { get; set; }
        public string referencia { get; set; }
        public double peso { get; set; }
        public string inat { get; set; }
        public string dropShipping { get; set; }
        public string ficha { get; set; }
        public string priorizaDeposito { get; set; }
        public string palavrasChave { get; set; }
        public string descricaoLonga { get; set; }
        public List<object> midias { get; set; }
        public string ncm { get; set; }
        public string norma { get; set; }
        public string data { get; set; }
        public string hora { get; set; }
        public string descricaoEcom { get; set; }
        public Caracteristicas caracteristicas { get; set; }
        public Divisaogerencial divisaogerencial { get; set; }
        public Marca marca { get; set; }
        public Departamento departamento { get; set; }
        public Categoria categoria { get; set; }
        public Perfil perfil { get; set; }
    }

    public class Retorno
    {
        public List<Produto> produtos { get; set; }
        public Paginacao paginacao { get; set; }
    }

    public class Subtipo
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }

    public class Tipo
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
    }


}
