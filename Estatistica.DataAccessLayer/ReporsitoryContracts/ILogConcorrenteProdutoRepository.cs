using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.ReporsitoryContracts
{
    public interface ILogConcorrenteProdutoRepository
    {
        Task<LogConcorrenteProduto> Add(LogConcorrenteProduto log);
        Task<IEnumerable<LogConcorrenteProduto>> GetByCondition(Expression<Func<LogConcorrenteProduto,bool>> condition);
    }
}
