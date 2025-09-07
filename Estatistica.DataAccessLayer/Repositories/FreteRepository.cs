using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class FreteRepository : IFreteRepository
    {
        private readonly ApplicationDbContext context;

        public FreteRepository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<Frete> AddFrete(Frete frete)
        {
            await context.Fretes.AddAsync(frete);
            await context.SaveChangesAsync();
            return frete;
        }

        public async Task<Frete?> GetFreteById(int id)
        {
            var frete = await context.Fretes.FirstOrDefaultAsync(f => f.Id == id);
            return frete;
        }

        public async Task<Frete?> GetFreteBySetorAndCodigoMunicipio(string setor, string codigoMunicipio)
        {
            return await context.Fretes.FirstOrDefaultAsync(f => f.Setor == setor && f.CodMunicipio == codigoMunicipio);
        }

        public async Task<Frete> UpdateFrete(Frete frete)
        {
            var existingFrete = context.Fretes.FirstOrDefault(f => f.Id == frete.Id);
            if (existingFrete is null) throw new Exception("Frete not found");
            existingFrete.PercentualFrete = frete.PercentualFrete;
            await context.SaveChangesAsync();
            return existingFrete;
        }
    }
}
