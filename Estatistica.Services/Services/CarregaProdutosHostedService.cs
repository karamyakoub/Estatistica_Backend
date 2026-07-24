
using Estatistica.BusinessLogicLayer.Data;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;

namespace Estatistica.Services.Services
{
    public class CarregaProdutosHostedService
    {
        private readonly string userName, password, url, authInfo;
        private readonly IProdutoService produtoService;
        private bool lastPage;
        public CarregaProdutosHostedService(IProdutoService produtoService)
        {

            url = ConfigurationManager.AppSettings["ConsultaProdutosUrl"]!;
            userName = ConfigurationManager.AppSettings["ConsultaProdutosUsuario"]!;
            password = ConfigurationManager.AppSettings["ConsultaProdutosSenha"]!;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
            this.produtoService=produtoService;
        }
        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {            
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"{nameof(CarregaProdutosHostedService)} - Carregando os produtos");
                lastPage = false;
                int pagina = 0;

                while (!lastPage)
                {
                    pagina++;                    
                    await saveProdutos(pagina);                    
                    if (pagina > 2000)
                        lastPage = true;
                }

                Console.WriteLine($"{nameof(CarregaProdutosHostedService)} - Produtos carregados com sucesso\nQuantidade de paginas: {pagina}");
                await Task.Delay(22 * 60 * 60  * 1000);
            }
        }
        private async Task saveProdutos(int pagina)
        {

            var produtosToAdd = new List<ProdutoConsulta.Content>();
            var produtosToUpdate = new List<ProdutoConsulta.Content>();
            var produtos = await getProdutos(pagina);
            if (produtos is not null)
            {
                foreach (var produto in produtos)
                {
                    if (produto is null || produto.cod == 0)
                        continue;

                    var exists = await produtoService.CheckProductExists(Convert.ToString(produto.cod));
                    if (exists)
                        produtosToUpdate.Add(produto);
                    else
                        produtosToAdd.Add(produto);
                }
                try
                {
                    if (produtosToAdd.Any())
                        await produtoService.AddProdutoRange(produtosToAdd);
                    if (produtosToUpdate.Any())
                        await produtoService.UpdateProdutoRange(produtosToUpdate);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Serviço {nameof(CarregaProdutosHostedService)}\nErro em salvar os produtos\nMessage: {ex.Message}");
                }
            }

        }

        private async Task<List<ProdutoConsulta.Content>?> getProdutos(int pagina)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
                    var uri = new Uri($"{url}?perfil=1&pagina={pagina}");
                    
                    var response = await client.GetAsync(uri);                    
                    var retorno = JsonConvert.DeserializeObject<ProdutoConsulta>(await response.Content.ReadAsStringAsync());
                    lastPage = retorno?.lastPage ?? false;
                    return retorno?.content;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Serviço {nameof(CarregaProdutosHostedService)}\nNão foi possivel carregar os produtos.\nMessage: {ex.Message}");
            }
            return null;
        }
    }
}
