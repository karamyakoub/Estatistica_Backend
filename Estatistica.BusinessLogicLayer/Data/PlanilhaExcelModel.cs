using System.Data;
using System.Globalization;

namespace Estatistica.BusinessLogicLayer.Data
{
    public class PlanilhaExcelModel
    {
        public string? Notafiscal { get; set; }//4
        public DateTime? DataEmissao { get; set; }//5
        public DateTime? DataSaida { get; set; }//6
        public string? Cnpj { get; set; }//19
        public string? Concorrente { get; set; }//20
        public string? Fantasia { get; set; }//21
        public string? NomeMunicipio { get; set; }//26
        public string? UfOrigin { get; set; }//27
        public string? UfDestino { get; set; }//41
        public string? CodigoMunicipio { get; set; }//25
        public string? CnpjCliente { get; set; }//34
        public string? NomeCliente { get; set; }//35
        public string? CodigoProduto { get; set; }//57
        public string? CodigoBarra { get; set; }//58
        public string? DescricaoProduto { get; set; }//59
        public string? Ncm { get; set; }//60
        public string? Cfop { get; set; }//63
        public int Qtd { get; set; } = 0; //65
        public decimal ValorUnitario { get; set; } = 0;//71
        public string? NumeroPedido { get; set; }//73
        public decimal? BCST { get; set; }//107
        public decimal? VST { get; set; }//108
        public string? ChaveNfe { get; set; }//160
        public string? Unidade { get; set; }

        



        public static PlanilhaExcelModel mapFromDataRow(DataRow dr)
        {
            DateTime.TryParseExact(Convert.ToString(dr[4]), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataEmissao);
            DateTime.TryParseExact(Convert.ToString(dr[5]), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datasaida);
            var model = new PlanilhaExcelModel();
            model.Notafiscal = Convert.ToString(dr[3]);
            model.DataEmissao = dataEmissao;
            model.DataSaida = datasaida;
            model.Cnpj = (Convert.ToString(dr[18]) ?? string.Empty).PadLeft(14,'0');
            model.Concorrente = Convert.ToString(dr[19]);
            model.Fantasia = Convert.ToString(dr[20]);
            model.NomeMunicipio = Convert.ToString(dr[25]);
            model.UfOrigin = Convert.ToString(dr[26]);
            model.UfDestino = Convert.ToString(dr[40]);
            model.CodigoMunicipio = Convert.ToString(dr[24]);
            model.CnpjCliente = Convert.ToString(dr[33]);
            model.NomeCliente = Convert.ToString(dr[34]);
            model.CodigoProduto = Convert.ToString(dr[56]);
            model.CodigoBarra = Convert.ToString(dr[57]);
            model.DescricaoProduto = Convert.ToString(dr[58]);
            model.Ncm = Convert.ToString(dr[59]);
            model.Cfop = Convert.ToString(dr[62]);
            model.Qtd  = Convert.ToInt32(dr[64] == DBNull.Value ? "0" : dr[64]);
            model.Unidade  = Convert.ToString(dr[63]);
            var t = (Convert.ToString(dr[65]) ?? Convert.ToString(dr[70]) ?? string.Empty);
            model.ValorUnitario = Convert.ToDecimal((Convert.ToString(dr[65]) ?? Convert.ToString(dr[70]) ?? string.Empty));
            model.NumeroPedido = Convert.ToString(dr[72]);
            model.BCST = Convert.ToDecimal((Convert.ToString(dr[106]) ?? string.Empty));
            model.VST = Convert.ToDecimal((Convert.ToString(dr[107]) ?? string.Empty));
            model.ChaveNfe = Convert.ToString(dr[159]);
            return model;
        }

    }
}
