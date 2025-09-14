using Estatitica.PriceService.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Estatitica.PriceService.Workers
{
    internal class AtualizaFreteWorker : WorkerBase
    {
        public static async Task ExecuteAsync()
        {
            Console.WriteLine("***************Inicio do processo de atualização de frete***************");
            var url = ConfigurationManager.AppSettings["BaseUrl"] + "api/Frete/AddUpdateFrete";
            var token = await GeraToken();
            if (string.IsNullOrEmpty(token))
                return;
            var freteList = FakeDataRespository.getFreteFake();
            foreach (var item in freteList)
            {
                var percentualFrete = HttpUtility.UrlEncode(item.PercentualFrete);
                var uri = new Uri($"{url}?setor={item.Setor}&codigoMunicipio={item.CodigoMunicipio}&percentualFrete={percentualFrete}");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(uri, null);
                var x = await response.Content.ReadAsStringAsync();
                //Caso não autorizado, gera um novo token e tenta novamente
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    token = await GeraToken();
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = await client.PutAsync(uri, null);
                }

                if (response.IsSuccessStatusCode)
                    Console.WriteLine($"Frete Para o setor {item.Setor} atualizado com sucesso");
                else
                    Console.WriteLine($"Erro em atualizar a frete do Setor {item.Setor}");


                Console.WriteLine("***************Fim do processo de atualização de frete***************");
            }
        }
    }
}