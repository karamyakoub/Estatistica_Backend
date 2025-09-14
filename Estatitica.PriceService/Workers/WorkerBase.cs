using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatitica.PriceService.Workers
{
    internal abstract class WorkerBase
    {
        internal static async Task<string> GeraToken()
        {
            var url = ConfigurationManager.AppSettings["BaseUrl"] + "login";
            var usuario = ConfigurationManager.AppSettings["UsusarioEst"];
            var senha = ConfigurationManager.AppSettings["SenhaEst"];

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);            
            var content = new StringContent(JsonConvert.SerializeObject(new { email = usuario, password = senha }),encoding: Encoding.UTF8,"application/json");
            var response = await client.PostAsync(url, content);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return string.Empty;
            var resposneStr = await response.Content.ReadAsStringAsync();
            var responseDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(resposneStr);
            if (responseDict is null)
                return string.Empty;
            return responseDict["accessToken"];
        }
    }
}
