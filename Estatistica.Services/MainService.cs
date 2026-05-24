using Estatistica.BusinessLogicLayer;
using Estatistica.DataAccessLayer;
using Estatistica.DataAccessLayer.Context;
using Estatistica.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Estatistica.Services
{
    internal class MainService
    {
        private readonly ServiceCollection serviceContainer = new ServiceCollection();
        private readonly ServiceProvider serviceProvider;
        private readonly List<Task> tasks = new List<Task>();

        internal MainService()
        {
            var env = ConfigurationManager.AppSettings["ENV"] ?? "TEST";
            var connString = env == "TEST" ? ConfigurationManager.AppSettings["ConnStrTest"] : ConfigurationManager.AppSettings["ConnStrProd"];
            serviceContainer.AddDataAccessLayer();
            serviceContainer.AddBusinessLogicLayer();
            serviceContainer.AddDbContext<ApplicationDbContext>(options => options.UseMySQL(connString!));
            serviceContainer.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
            }).AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>();
            serviceContainer.AddScoped<CarregaProdutosHostedService>();
            serviceContainer.AddScoped<ItatiaiaService>();
            serviceContainer.AddSingleton<PlanilhaProcessingService>();
            serviceProvider = serviceContainer.BuildServiceProvider();
        }

        public void Run()
        {
            tasks.Add(Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<CarregaProdutosHostedService>();
                await svc.ExecuteAsync();
            }));

            tasks.Add(Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var svc = scope.ServiceProvider.GetRequiredService<ItatiaiaService>();
                await svc.ExecuteAsync();
            }));

            tasks.Add(Task.Run(async () =>
            {
                var svc = serviceProvider.GetRequiredService<PlanilhaProcessingService>();
                await svc.ExecuteAsync();
            }));

            Task.WaitAll(tasks.ToArray());
        }
    }
}
