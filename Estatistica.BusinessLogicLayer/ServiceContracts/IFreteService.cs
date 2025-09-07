using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.ServiceContracts
{
    public interface IFreteService
    {
        Task AddUpdateFrete(string setor, string codigoMunicipio, decimal percentualFrete);
    }
}
