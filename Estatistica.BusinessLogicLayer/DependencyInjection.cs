using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            //Add services to the business logic
            services.AddAutoMapper(typeof(UsuarioGetResponse));
            services.AddScoped<IUsuarioService, UsuarioService>();            
            services.AddScoped<IConcorrenteService, ConcorrenteService>();            
            services.AddScoped<IConcorrenteFilialService, ConcorrenteFilialService>();            
            services.AddScoped<IPlanilhaService, PlanilhaService>();            
            services.AddScoped<IProdutoService, ProdutoService>();            
            services.AddScoped<IConcorrenteProdutoService, ConcorrenteProdutoService>();   
            services.AddScoped<INfeXmlService, NfeXmlService>();            
            services.AddScoped<IStatitsticService, StatisticService>();            
            services.AddScoped<ILogConcorrenteProdutoService, LogConcorrenteProdutoService>();            
            services.AddScoped<IFreteService, FreteService>();            
            return services;
        }
    }
}
