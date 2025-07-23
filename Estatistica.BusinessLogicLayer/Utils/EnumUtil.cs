using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Estatistica.BusinessLogicLayer.Utils
{
    public static class EnumUtil
    {
        public static string GetPlanilhaStatusDescription(int status)
        {
            switch (status)
            {
                case 0:
                    return "Aguardando Inclusão";
                case 1:
                    return "Incluída";
                case 2:
                    return "Aguardando Processamento";
                case 3:
                    return "Pendente";
                case 4:
                    return "Em Processo";
                case 5:
                    return "Produtos Incluídos";
                case 6:
                    return "Cabeçalhos Incluídos";
                case 7:
                    return "Itens Incluídos";
                case 8:
                    return "Processamento Concluído";
                case 9:
                    return "Erro";
                default:
                    return "Desconhecido";
            }
        }
    }
}

