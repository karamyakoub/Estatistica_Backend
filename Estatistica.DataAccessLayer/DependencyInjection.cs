using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Estatistica.DataAccessLayer
{
    public static class DependencyInjection
    {        
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, string connString)
        {                                    
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IConcorrenteRepository, ConcorrenteRepository>();
            services.AddScoped<IConcorrenteFilialRepository, ConcorrenteFilialRepository>();
            services.AddScoped<IConcorrenteProdutoRepository,ConcorrenteProdutoRepository>();
            services.AddScoped<INfcRespository, NfcRespository>();
            services.AddScoped<INfiRespository, NfiRepository>();
            services.AddScoped<IPlanilhaRepository, PlanilhaRepository>();
            services.AddScoped<IPlanilhaStatusRepository, PlanilhaStatusRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IConcorrenteFilialPendenteRepository, ConcorrenteFilialPendenteRepository>();
            services.AddScoped<IConcorrenteFilialTempRepository, ConcorrenteFilialTempRepository>();
            return services;
        }
    }
}