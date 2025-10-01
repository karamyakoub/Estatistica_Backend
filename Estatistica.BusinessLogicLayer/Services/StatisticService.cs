using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.ReporsitoryContracts;

using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class StatisticService : IStatitsticService
    {
        private readonly ApplicationDbContext context;
        private readonly INfiRespository nfiRespository;
        private readonly IMapper mapper;

        public StatisticService(ApplicationDbContext context, INfiRespository nfiRespository, IMapper mapper)
        {
            this.context=context;
            this.nfiRespository=nfiRespository;
            this.mapper=mapper;
        }

        public async Task<IEnumerable<StatistcDTO>> GetStatisticResult(List<int>? concorrenteId, DateTime? dtEmissaoIni, DateTime? dtEmissaoFin, decimal? desconto, string? fabricante)
        {
            var query = context.vW_Estatisticas.AsQueryable();

            if (concorrenteId != null && concorrenteId.Any())
            {
                query = query.Where(x => concorrenteId.Contains(x.ConcorrenteId ?? 0));
            }

            if (dtEmissaoIni.HasValue)
            {
                query = query.Where(x => x.DataEmissao >= dtEmissaoIni.Value);
            }

            if (dtEmissaoFin.HasValue)
            {
                query = query.Where(x => x.DataEmissao <= dtEmissaoFin.Value);
            }

            if (!string.IsNullOrWhiteSpace(fabricante))
            {
                var fabUpper = fabricante.ToUpper();
                query = query.Where(x => !string.IsNullOrWhiteSpace(x.Fabricante) && x.Fabricante.ToUpper().Contains(fabUpper));
            }

            var result = await query.ToListAsync();
            var resultDto = mapper.Map<IEnumerable<StatistcDTO>>(result).ToList();

            for (int i = 0; i < resultDto.Count(); i++)
            {
                resultDto[i].SetDiscount(desconto ?? 0);
            }
            
                

            return resultDto;
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

        public async Task<bool> UpdateNfiCorrectionCount(string id, int correctionCount)
        {
            return await nfiRespository.UpdateNfiCountCorrection(id, correctionCount);
        }
    }
}
