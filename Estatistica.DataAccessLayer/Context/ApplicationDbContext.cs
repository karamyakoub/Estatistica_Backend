using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.Utils;
using Google.Protobuf.Compiler;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Context
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Add auto increments primary keys
            builder.Entity<Concorrente>(e => e.Property(p => p.Id).ValueGeneratedOnAdd());
            builder.Entity<LogConcorrenteProduto>(e => e.Property(p => p.Id).ValueGeneratedOnAdd());
            builder.Entity<Planilha>(e => e.Property(p => p.Id).ValueGeneratedOnAdd());
            builder.Entity<PlanilhaStatus>(e => e.Property(p => p.Id).ValueGeneratedOnAdd());
            builder.Entity<ConcorrenteFilialTemp>(e => e.Property(p => p.Id).ValueGeneratedOnAdd());
            builder.Entity<ConcorrenteFilialPendente>(e => e.Property(p => p.Id).ValueGeneratedOnAdd());

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserProfileEnum.Admin.ToString(),
                        NormalizedName = UserProfileEnum.Admin.ToString()
                    },
                    new IdentityRole
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = UserProfileEnum.RCA.ToString(),
                        NormalizedName = UserProfileEnum.RCA.ToString()
                    }
                );

            base.OnModelCreating(builder);
        }



        public DbSet<Concorrente> Concorrentes { get; set; }
        public DbSet<ConcorrenteFilial> ConcorrenteFilials { get; set; }
        public DbSet<ConcorrenteProduto> ConcorrenteProdutos { get; set; }
        public DbSet<LogConcorrenteProduto> LogConcorrenteProdutos { get; set; }
        public DbSet<Nfc> Nfcs { get; set; }
        public DbSet<Nfi> Nfis { get; set; }
        public DbSet<Planilha> Planilhas { get; set; }
        public DbSet<PlanilhaStatus> PlanilhaStatuses { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ConcorrenteFilialPendente> ConcorrenteFilialPendentes { get; set; }
        public DbSet<ConcorrenteFilialTemp> ConcorrenteFilialTemps { get; set; }
    }
}
