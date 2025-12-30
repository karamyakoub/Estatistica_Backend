using Estatistica.BusinessLogicLayer;
using Estatistica.DataAccessLayer;
using Estatistica.DataAccessLayer.Context;
using Estatistica.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.Services
{
    internal class MainService
    {
        private readonly ServiceCollection serviceContainer = new ServiceCollection();
        private ServiceProvider serviceProvider;
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
            serviceContainer.AddSingleton<CarregaProdutosHostedService>();
            serviceContainer.AddSingleton<ItatiaiaService>();
            serviceContainer.AddSingleton<PlanilhaProcessingService>();
            serviceProvider = serviceContainer.BuildServiceProvider();
        }



        public void Run()
        {

            //var carregaProdutosHostedService = serviceProvider.GetRequiredService<CarregaProdutosHostedService>();
            //tasks.Add(carregaProdutosHostedService.ExecuteAsync());
            var itatiaiaService = serviceProvider.GetRequiredService<ItatiaiaService>();
            tasks.Add(itatiaiaService.ExecuteAsync());
            //var planilhaProcessingService = serviceProvider.GetRequiredService<PlanilhaProcessingService>();
            //tasks.Add(planilhaProcessingService.ExecuteAsync());

            Task.WaitAll(tasks.ToArray());
        }
    }



}
