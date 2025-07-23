
using Estatistica.BusinessLogicLayer.Data;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.BusinessLogicLayer.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace Estatistica.WebAPI.Services
{
    public class CarregaProdutosHostedService : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly IServiceScopeFactory scopeFactory;        
        private readonly string userName, password, url, authInfo;
        public CarregaProdutosHostedService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            this.configuration=configuration;
            this.scopeFactory=scopeFactory;            
            url = configuration.GetSection("ApPCOnfigs")["ConsultaProdutosUrl"]!;
            userName = configuration.GetSection("ApPCOnfigs")["ConsultaProdutosUsuario"]!;
            password = configuration.GetSection("ApPCOnfigs")["ConsultaProdutosSenha"]!;
            authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userName}:{password}"));
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(CarregaProdutosHostedService)} foi iniciada");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pageCount = await getTotalPages();
                if (pageCount > 0)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        await saveProdutos(i);
                    }
                }
                await Task.Delay(22 * 60 * 60  * 1000);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(CarregaProdutosHostedService)} foi cancelada");            
        }

        private async Task saveProdutos(int pageCount)
        {
            using(var scope = scopeFactory.CreateScope())
            {
                var produtoService = scope.ServiceProvider.GetRequiredService<IProdutoService>();
                var produtosToAdd = new List<Produto>();
                var produtosToUpdate = new List<Produto>();
                var produtos = await getProdutos(pageCount);
                if (produtos is not null)
                {
                    foreach (var produto in produtos)
                    {
                        var exists = await produtoService.CheckProductExists(Convert.ToString(produto.cod));
                        if (exists)
                            produtosToUpdate.Add(produto);
                        else
                            produtosToAdd.Add(produto);
                    }
                    if (produtosToAdd.Any())
                        await produtoService.AddProdutoRange(produtosToAdd);
                    if (produtosToUpdate.Any())
                        await produtoService.UpdateProdutoRange(produtosToUpdate);
                }
            }            
        }

        private async Task<IEnumerable<Produto>?> getProdutos(int pagina)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
                var uri = new Uri(url);
                var body = JsonConvert.SerializeObject(new
                {
                    getfoto = "N",
                    getficha = "N",
                    pagina = pagina

                });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                var retorno = JsonConvert.DeserializeObject<ProdutoApiConsulta>(await response.Content.ReadAsStringAsync());

                return retorno?.retorno.produtos;

            }
        }
        private async Task<int> getTotalPages()
        {
            return (await getProdutos(1) ?? new List<Produto>()).Count();
        }
    }
}
