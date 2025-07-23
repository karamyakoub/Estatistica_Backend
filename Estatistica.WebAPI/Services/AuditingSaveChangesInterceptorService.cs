using Estatistica.DataAccessLayer.Entities;
using Estatistica.WebAPI.ServiceContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Estatistica.WebAPI.Context
{
    public class AuditingSaveChangesInterceptorService : SaveChangesInterceptor
    {
        private readonly ICurrentUserService currentUserService;

        public AuditingSaveChangesInterceptorService(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            callAudit(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            callAudit(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        private void callAudit(DbContextEventData eventData)
        {
            var dbContext = eventData.Context;
            var usuarioLogado = currentUserService.GetCurrentUserId();
            if (dbContext is not null && usuarioLogado is not null)
            {
                foreach (var entry in dbContext.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.Entity is FullAuditableEntity auditableFull)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            auditableFull.DataCadastro = DateTime.UtcNow;
                            auditableFull.UsuarioCadastro = usuarioLogado;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            auditableFull.DataAlteracao = DateTime.UtcNow;
                            auditableFull.UsuarioAlteracao = usuarioLogado;
                        }
                    }
                    else if (entry.Entity is CreationAuditableEntity auditable)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            auditable.DataCadastro = DateTime.UtcNow;
                            auditable.UsuarioCadastro = usuarioLogado;
                        }
                    }
                }
            }
        }
    }
}
