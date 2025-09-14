using Estatitica.PriceService.Managers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Estatitica.PriceService.Workers
{
    internal class AtualizaPrecoProdutoWorker : WorkerBase
    {
        public static async Task ExecuteAsync()
        {
            Console.WriteLine("***************Inicio do processo de atualização de preços***************");
            var url = ConfigurationManager.AppSettings["BaseUrl"] + "api/ProdutoInterno/UpdateProdutoPrice";
            var token = await GeraToken();
            if (string.IsNullOrEmpty(token))
                return;
            var produtoPrecoList = FakeDataRespository.getPrecoProdutosFake();
            foreach (var item in produtoPrecoList)
            {
                var priceParam = WebUtility.UrlEncode(item.Preco.ToString());
                var custoParam = WebUtility.UrlEncode(item.Custo.ToString());
                var uri = new Uri($"{url}?codigoProduto={item.CodigoProduto}&price={priceParam}&custo={custoParam}");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PutAsync(uri, null);
                //Caso não autorizado, gera um novo token e tenta novamente
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    token = await GeraToken();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = await client.PutAsync(uri, null);
                }

                if (response.IsSuccessStatusCode)
                    Console.WriteLine($"Produto {item.CodigoProduto} atualizado com sucesso");
                else
                    Console.WriteLine($"Erro em atualizar o precdo do Produto {item.CodigoProduto}");                
            }
            Console.WriteLine("***************Fim do processo de atualização de preços***************");
        }
    }
}
