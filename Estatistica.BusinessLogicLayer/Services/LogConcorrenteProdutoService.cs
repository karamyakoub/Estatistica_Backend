using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
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
    public class LogConcorrenteProdutoService : ILogConcorrenteProdutoService
    {
        private readonly ILogConcorrenteProdutoRepository logConcorrenteProdutoRepository;
        private readonly IMapper mapper;
        private readonly IUsuarioRepository usuarioRepository;

        public LogConcorrenteProdutoService(ILogConcorrenteProdutoRepository logConcorrenteProdutoRepository, IMapper mapper, IUsuarioRepository usuarioRepository)
        {
            this.logConcorrenteProdutoRepository=logConcorrenteProdutoRepository;
            this.mapper=mapper;
            this.usuarioRepository=usuarioRepository;
        }
        public async Task<bool> Add(LogConcorrenteProduto log)
        {
            var addedLog = await logConcorrenteProdutoRepository.Add(log);
            return addedLog == null;
        }

        public async Task<IEnumerable<LogConcorrenteProdutoGetResponse>> GetbyFilter(DateTime startDate, DateTime endDate, string filterText)
        {
            var logs = mapper.Map<IEnumerable<LogConcorrenteProdutoGetResponse>>(
                        await logConcorrenteProdutoRepository.GetByCondition(x =>
                            x.DataCadastro.HasValue &&
                            x.DataCadastro.Value.Date >= startDate &&
                            x.DataCadastro.Value.Date <= endDate &&
                            (string.IsNullOrWhiteSpace(filterText) ||
                             (x.CodigoProdutoConcorrente != null &&
                              x.CodigoProdutoConcorrente.DescricaoProdutoConcorrente.Contains(filterText, StringComparison.InvariantCultureIgnoreCase)))
                        ));


            foreach (var log in logs)
            {
                if (!string.IsNullOrWhiteSpace(log.UsuarioId))
                {
                    var usuario = (await usuarioRepository.GetUsuarioById(log.UsuarioId));
                    if (usuario != null)
                        log.Usuario = usuario.Email;

                }
            }
            return logs;
        }
    }
}
