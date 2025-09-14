using Estatitica.PriceService.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatitica.PriceService.Managers
{
    internal static class FakeDataRespository
    {
        internal static List<PrecoProduto> getPrecoProdutosFake()
        {
            var listaPrecoProduto = new List<PrecoProduto>
            {
                new PrecoProduto { CodigoProduto = "10", Preco = "15.99", Custo ="10.50" },
                new PrecoProduto { CodigoProduto = "100", Preco = "25.00", Custo ="18.75" },
                new PrecoProduto { CodigoProduto = "1000", Preco = "99.99", Custo ="70.00" },
                new PrecoProduto { CodigoProduto = "100005", Preco = "150.00", Custo ="120.00" },
                new PrecoProduto { CodigoProduto = "1001", Preco = "35.50", Custo ="30.00" },
                new PrecoProduto { CodigoProduto = "1002", Preco = "45.75", Custo ="33.20" },
                new PrecoProduto { CodigoProduto = "1003", Preco = "12.99", Custo ="9.10" },
                new PrecoProduto { CodigoProduto = "1004", Preco = "60.00", Custo ="40.00" },
                new PrecoProduto { CodigoProduto = "1005", Preco = "89.99", Custo ="55.55" },
                new PrecoProduto { CodigoProduto = "1006", Preco = "19.90", Custo ="14.30" },
            };
            return listaPrecoProduto;
        }

        internal static List<Frete> getFreteFake()
        {
            var listaFrete = new List<Frete>
            {
                new Frete { Setor = "1", CodigoMunicipio = "1100015", PercentualFrete = "5" },
                new Frete { Setor = "2", CodigoMunicipio = "4305108", PercentualFrete = "8.5" },
                new Frete { Setor = "3", CodigoMunicipio = "3550308", PercentualFrete = "7" },
                new Frete { Setor = "4", CodigoMunicipio = "3304557", PercentualFrete = "6.25" },
                new Frete { Setor = "5", CodigoMunicipio = "5208707", PercentualFrete = "10" },
                new Frete { Setor = "6", CodigoMunicipio = "3106200", PercentualFrete = "9.75" },
                new Frete { Setor = "7", CodigoMunicipio = "1200401", PercentualFrete = "11" },
                new Frete { Setor = "8", CodigoMunicipio = "4205407", PercentualFrete = "7.5" },
                new Frete { Setor = "9", CodigoMunicipio = "2927408", PercentualFrete = "4.5" },
                new Frete { Setor = "10", CodigoMunicipio = "2211001", PercentualFrete = "6" }
            };
            return listaFrete;
        }
    }
}
