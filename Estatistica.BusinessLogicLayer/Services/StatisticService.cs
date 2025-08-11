using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.ReporsitoryContracts;

using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class StatisticService : IStatitsticService
    {
        private readonly ApplicationDbContext context;

        public StatisticService(ApplicationDbContext context)
        {
            this.context=context;
        }

        public async Task<IEnumerable<SearchProductPriceResponse>> SearchProductPrice(string dateTime, List<int> idConcorrente, string textSearch)
        {
            DateTime.TryParseExact(dateTime, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateFilter);
            var lista = await context.Nfis
                        .Include(nfi => nfi.ConcorrenteProduto)
                            .ThenInclude(cp => cp.Concorrente)
                        .Include(nfi => nfi.ConcorrenteProduto)
                            .ThenInclude(cp => cp.Produto)
                        .Include(nfi => nfi.Nfc)
                        .Where(nfi =>
                            nfi.Nfc.DataEmissao >= dateFilter &&
                            (idConcorrente.Count > 0 ? idConcorrente.Contains(nfi.ConcorrenteProduto.Concorrente.Id) : true) &&
                            (!string.IsNullOrWhiteSpace(textSearch) &&
                             nfi.ConcorrenteProduto.Produto != null &&
                             nfi.ConcorrenteProduto.Produto.CodigoProduto == textSearch.Trim())
                        )
                        .Select(nfi => new SearchProductPriceResponse
                        {
                            Id = nfi.Id,
                            CodigoProdutoConcorrente = nfi.ConcorrenteProduto.CodigoProdutoConcorrente,
                            DescricaoProdutoConcorrente = nfi.ConcorrenteProduto.DescricaoProdutoConcorrente,
                            CodigoProduto = nfi.ConcorrenteProduto.Produto != null ? nfi.ConcorrenteProduto.Produto.CodigoProduto : null,
                            DescricaoProduto = nfi.ConcorrenteProduto.Produto != null ? nfi.ConcorrenteProduto.Produto.Descricao : null,
                            Fabricante = nfi.ConcorrenteProduto.Produto != null ? nfi.ConcorrenteProduto.Produto.Fabricante : null,
                            Concorrente = nfi.ConcorrenteProduto.Concorrente.Nome,
                            Qtd = nfi.Qtde,
                            Unidade = nfi.Unidade,
                            DataEmissao = nfi.Nfc.DataEmissao.HasValue ? nfi.Nfc.DataEmissao.Value.ToString("dd/MM/yyyy") : null,
                            Valor = nfi.Valor
                        })
                        .ToListAsync();
            return lista;
        }
    }
}
