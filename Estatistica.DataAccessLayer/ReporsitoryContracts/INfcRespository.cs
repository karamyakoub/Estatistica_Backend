using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface INfcRespository
    {
        Task<Nfc> AddNfc(Nfc nfc);
        Task<bool> AddNfcRange(List<Nfc> nfcList);
        Task<IEnumerable<Nfc>> GetNfcsByCondition(Expression<Func<Nfc, bool>> condition);
        Task<IEnumerable<Nfc>> GetNfcsByConditionNoTracking(Expression<Func<Nfc, bool>> condition);

    }
}
