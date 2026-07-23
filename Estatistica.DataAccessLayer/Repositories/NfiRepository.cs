using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class NfiRepository : INfiRespository
    {
        private readonly ApplicationDbContext context;

        public NfiRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Nfi> AddNfi(Nfi nfi)
        {
            await context.Nfis.AddAsync(nfi);
            await context.SaveChangesAsync();
            return nfi;
        }

        public async Task<bool> AddNfiRange(List<Nfi> nfiList)
        {
            await context.Nfis.AddRangeAsync(nfiList);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Nfi>> GetNfisByCondition(Expression<Func<Nfi, bool>> condition)
        {
            return await context.Nfis.Include(x => x.Nfc).Where(condition)                                
                .ToListAsync();
        }

        public async Task<IEnumerable<Nfi>> GetNfisByConditionNoTracking(Expression<Func<Nfi, bool>> condition)
        {
            return await context.Nfis.Include(x => x.Nfc).AsNoTracking().Where(condition)
                .ToListAsync();
        }

        public async Task<Nfi> UpdateNfi(Nfi nfi)
        {
            var nfiToUpdate = await context.Nfis.Include(x => x.Nfc).FirstOrDefaultAsync(x => x.Id == nfi.Id);
            if (nfiToUpdate is null)
                throw new NullReferenceException($"Nfe não encontrado para {nfi.Nfc.ChaveNfe}, Prod: {nfi.ConcorrenteProduto}");

            nfiToUpdate.Qtde = nfi.Qtde;
            nfiToUpdate.Valor = nfi.Valor;
            nfiToUpdate.UfOrigin = nfi.UfOrigin;
            nfiToUpdate.UfDestino = nfi.UfDestino;
            nfiToUpdate.CodMunicipio = nfi.CodMunicipio;
            nfiToUpdate.CodigoBarra = string.IsNullOrWhiteSpace(nfi.CodigoBarra) ? null : nfi.CodigoBarra;
            nfiToUpdate.Unidade = nfi.Unidade;

            await context.SaveChangesAsync();
            return nfiToUpdate;
        }

        public async Task<bool> UpdateNfiCountCorrection(string id,int count)
        {
            var nfiToUpdate = await context.Nfis.FindAsync(id);
            if(nfiToUpdate == null)
                return false;
            nfiToUpdate.QtdeCorrecao = count;
            return (await context.SaveChangesAsync()) > 0;            
        }
    }
}
