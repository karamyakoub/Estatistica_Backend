using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class FreteService : IFreteService
    {
        private readonly IFreteRepository freteRepository;

        public FreteService(IFreteRepository freteRepository)
        {
            this.freteRepository=freteRepository;
        }
        public async Task AddUpdateFrete(string setor, string codigoMunicipio, decimal percentualFrete)
        {
            var frete = await freteRepository.GetFreteByCodigoMunicipio(codigoMunicipio);
            if(frete is null)
            {
                frete = new Frete
                {
                    Setor = setor,
                    CodMunicipio = codigoMunicipio,
                    PercentualFrete = percentualFrete
                };
                await freteRepository.AddFrete(frete);
            }
            else
            {
                frete.Setor = setor;
                frete.PercentualFrete = percentualFrete;
                await freteRepository.UpdateFrete(frete);
            }
        }
    }
}
