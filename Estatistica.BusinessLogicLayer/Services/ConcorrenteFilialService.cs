using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.Enums;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class ConcorrenteFilialService : IConcorrenteFilialService
    {
        private readonly IConcorrenteFilialRepository concorrenteFilialRepository;
        private readonly IConcorrenteRepository concorrenteRepository;
        private readonly IConcorrenteFilialPendenteRepository concorrenteFilialPendenteRepository;
        private readonly IPlanilhaRepository planilhaRepository;
        private readonly IMapper mapper;

        public ConcorrenteFilialService(IConcorrenteFilialRepository concorrenteFilialRepository,
            IConcorrenteRepository concorrenteRepository,
            IConcorrenteFilialPendenteRepository concorrenteFilialPendenteRepository,
            IPlanilhaRepository planilhaRepository,
            IMapper mapper)
        {
            this.concorrenteFilialRepository = concorrenteFilialRepository;
            this.concorrenteRepository = concorrenteRepository;
            this.concorrenteFilialPendenteRepository=concorrenteFilialPendenteRepository;
            this.planilhaRepository=planilhaRepository;
            this.mapper = mapper;
        }
        public async Task<ConcorrenteFilialGetResponse> AddConcorrenteFilial(string cnpj, int idConcorrente)
        {
            var concorrente = await concorrenteRepository.GetConcorrenteById(idConcorrente);
            if (concorrente is null)
                throw new ArgumentException("Concorrente nao encontrado");
            var concorrenteFilial = await concorrenteFilialRepository.GetConcorrenteFilialByCnpj(cnpj);
            if (concorrenteFilial is not null)
                throw new ArgumentException("Concorrente Filial ja existe");
            var addedConcorrenteFilial = await concorrenteFilialRepository.AddConcorrenteFilial(cnpj, concorrente);
            return mapper.Map<ConcorrenteFilialGetResponse>(addedConcorrenteFilial);
        }

        public async Task AddConcorrenteFilialPendenteRange(int planilhaId, List<ConcorrenteFilialPendente> concorrenteFilialPendenteList)
        {
            var planilha = await planilhaRepository.GetPlanilhaById(planilhaId);
            if (planilha is null)
                throw new ArgumentException("Planilha nao encontrada");
            foreach (var concorrenteFilialPendente in concorrenteFilialPendenteList)
            {
                concorrenteFilialPendente.Planilha = planilha;
            }
            await concorrenteFilialPendenteRepository.AddConcorrenteFilialPendenteRange(concorrenteFilialPendenteList);
        }

        public async Task<IEnumerable<ConcorrenteFilialGetResponse>> GetConcorrenteFiliaisByConcorrente(int idConcorrente)
        {
            var concorrente = await concorrenteRepository.GetConcorrenteById(idConcorrente);
            if (concorrente is null)
                throw new ArgumentException("Concorrente nao encontrado");
            return mapper.Map<IEnumerable<ConcorrenteFilialGetResponse>>(await concorrenteFilialRepository.GetConccorenteFiliaisByConcorrente(concorrente));
        }

        public async Task<IEnumerable<ConcorrenteFilialPendenteGetResponse>> GetConcorrenteFilialPendentesAgrupado()
        {
            var filiasPendentesTotal = await concorrenteFilialPendenteRepository.GetConcorrenteFilialPendentes(x => x.Concorrente == null);
            return mapper.Map<IEnumerable<ConcorrenteFilialPendenteGetResponse>>(filiasPendentesTotal.GroupBy(x => x.Cnpj).Select(x => x.First()).ToList());
        }

        public async Task<IEnumerable<ConcorrenteFilialGetResponse>> GetConcorrentesFiliais()
        {
            return mapper.Map<IEnumerable<ConcorrenteFilialGetResponse>>(await concorrenteFilialRepository.GetConcorrentesFiliais());
        }

        public async Task<ConcorrenteFilial?> GetConcorrentesFilialByCnpj(string cnpj)
        {
            return await concorrenteFilialRepository.GetConcorrenteFilialByCnpj(cnpj);
        }

        public async Task<ConcorrenteFilialGetResponse> UpdateConcorrenteFilial(string cnpj, int idConcorrente)
        {
            var concorrente = await concorrenteRepository.GetConcorrenteById(idConcorrente);
            if (concorrente is null)
                throw new ArgumentException("Concorrente nao encontrado");
            var concorrenteFilial = await concorrenteFilialRepository.GetConcorrenteFilialByCnpj(cnpj);
            if (concorrenteFilial is null)
                throw new ArgumentException("CNPJ nao encontrado");

            concorrenteFilial.Concorrente = concorrente;
            var updateConcorrenteFilial = await concorrenteFilialRepository.UpdateConcorrenteFilial(cnpj, concorrente);
            return mapper.Map<ConcorrenteFilialGetResponse>(updateConcorrenteFilial);
        }

        public async Task UpdateConcorrenteFilialPendente(string cnpj, Concorrente concorrente)
        {
            await concorrenteFilialPendenteRepository.UpdateConcorrenteFilialPendente(cnpj, concorrente);
        }
    }
}
