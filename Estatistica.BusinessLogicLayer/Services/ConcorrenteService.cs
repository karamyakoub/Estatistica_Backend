using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.DTO;
using AutoMapper;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class ConcorrenteService : IConcorrenteService
    {
        private readonly IConcorrenteRepository repo;
        private readonly IConcorrenteFilialRepository fRepo;
        private readonly IMapper mapper;

        public ConcorrenteService(IConcorrenteRepository repo, IConcorrenteFilialRepository fRepo, IMapper mapper)
        {
            this.repo = repo;
            this.fRepo=fRepo;
            this.mapper = mapper;
        }
        public async Task<int> AddConcorrente(string nome)
        {
            var existedConcorrente = await repo.GetConcorrentesByCondition(x => x.Nome.ToUpper().Trim() == nome.ToUpper().Trim());
            if (existedConcorrente.Count() > 0)
                throw new InvalidOperationException("Concorrente ja existe");
            var concorrente = new Concorrente
            {
                Nome = nome,
            };
            return (await repo.AddConcorrente(concorrente)).Id;
        }

        public async Task<ConcorrenteGetResponse?> UpdateConcorrenteNome(int id, string nome)
        {
            var Concorerente = new Concorrente
            {
                Id = id,
                Nome = nome,
            };
            var result = await repo.UpdateConcorrenteNome(Concorerente);
            if (result)
                return mapper.Map<Concorrente, ConcorrenteGetResponse>(Concorerente);
            return null;
        }

        public async Task<IEnumerable<ConcorrenteGetResponse>> GetConcorrentes(OrderByEnum orderBy)
        {
            return (await repo.GetConcorrentes(includeFiliais: true))
                                .OrderBy(x => orderBy == OrderByEnum.Description ? x.Nome : null)
                                .Select(c => new ConcorrenteGetResponse
                                {
                                    Id = c.Id,
                                    Nome = c.Nome,
                                    Filiais = c.Filiais.Select(f => f.Cnpj).ToList()
                                });
        }


        public async Task<ConcorrenteGetResponse?> GetConcorrenteById(int id)
        {
            var concorrente = await repo.GetConcorrenteById(id);

            return concorrente is not null ? mapper.Map<Concorrente, ConcorrenteGetResponse>(concorrente) :
                null;
        }

    }
}
