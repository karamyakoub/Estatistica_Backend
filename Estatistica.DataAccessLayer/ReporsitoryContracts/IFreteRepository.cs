using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface IFreteRepository
    {
        Task<Frete> AddFrete(Frete frete);
        Task<Frete> UpdateFrete(Frete frete);
        Task<Frete?> GetFreteById(int id);
        Task<Frete?> GetFreteBySetorAndCodigoMunicipio(string setor, string codigoMunicipio);
    }
}
