using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class ConcorrenteProdutoService : IConcorrenteProdutoService
    {
        private readonly IConcorrenteProdutoRepository concorrenteProdutoRepository;
        private readonly IMapper mapper;

        public ConcorrenteProdutoService(IConcorrenteProdutoRepository concorrenteProdutoRepository,IMapper mapper)
        {
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.mapper=mapper;
        }
        

        public Task<IEnumerable<PageObject<ConcorrenteProdutoSearchDto>>> SearchConcorrenteProdutos(int? idConcorrente, string descricaoProduto, int pageSize, int pageNumber)
        {
            var concorrenteProdutos = await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTrackingSeaarch(x =>
                   (idConcorrente.HasValue ? x.Concorrente.Id == idConcorrente.Value : true) &&
                    (string.IsNullOrEmpty(descricaoProduto) ? true : x.DescricaoProdutoConcorrente.Contains(descricaoProduto, StringComparison.OrdinalIgnoreCase)
                ));

            return mapper.Map<IEnumerable<ConcorrenteProdutoSearchDto>>(concorrenteProdutos);
        }
    }
}
