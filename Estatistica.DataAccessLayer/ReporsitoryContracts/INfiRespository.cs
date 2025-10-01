using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface INfiRespository
    {
        Task<Nfi> AddNfi(Nfi nfi);
        Task<bool> AddNfiRange(List<Nfi> nfiList);
        Task<IEnumerable<Nfi>> GetNfisByCondition(Expression<Func<Nfi, bool>> condition);
        Task<IEnumerable<Nfi>> GetNfisByConditionNoTracking(Expression<Func<Nfi, bool>> condition);
        Task<bool> UpdateNfiCountCorrection(string id, int count);
    }
}
