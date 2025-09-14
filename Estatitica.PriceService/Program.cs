using Estatitica.PriceService.Workers;

Console.WriteLine($"Serviço de estatistica iniciado {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");
Thread.Sleep(5000); //Simula o tempo de processamento de 5 segundos
await AtualizaPrecoProdutoWorker.ExecuteAsync();
await AtualizaFreteWorker.ExecuteAsync();


Console.WriteLine($"Serviço de estatistica finalizado {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}");