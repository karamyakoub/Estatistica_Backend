using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Estatistica.DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Services
{
    public class ConcorrenteProdutoService : IConcorrenteProdutoService
    {
        private readonly IConcorrenteProdutoRepository concorrenteProdutoRepository;
        private readonly IMapper mapper;
        private readonly IProdutoRepository produtoRepository;
        private readonly IUsuarioRepository usuarioRepository;

        public ConcorrenteProdutoService(IConcorrenteProdutoRepository concorrenteProdutoRepository, IMapper mapper,
            IProdutoRepository produtoRepository,
            IUsuarioRepository usuarioRepository)
        {
            this.concorrenteProdutoRepository=concorrenteProdutoRepository;
            this.mapper=mapper;
            this.produtoRepository=produtoRepository;
            this.usuarioRepository=usuarioRepository;
        }

        public async Task<bool> LinkProduct(string CodigoProdutoConcorrente, string idProduto)
        {
            var produto = await produtoRepository.GetProductByCodigo(idProduto);
            if (produto is null) throw new ArgumentNullException("Produto nao encontrado");
            return await concorrenteProdutoRepository.LinkConcorrenteProduto(CodigoProdutoConcorrente, produto);
        }

        public async Task<IEnumerable<ConcorrenteProdutoSearchDto>> SearchConcorrenteProdutos(string? idConcorrente, string descricaoProduto, string fabricante)
        {
            List<int>? ids = null; 
            if(!string.IsNullOrWhiteSpace(idConcorrente))
                idConcorrente.Split(",").Select(x => int.Parse(x));
            var concorrenteProdutos = await concorrenteProdutoRepository.GetConcorrenteProdutosByConditionNoTrackingSeaarch(x =>
                   (ids == null || ids.Count() == 0 ? true : ids.Contains(x.Concorrente.Id)) &&
                    (string.IsNullOrWhiteSpace(descricaoProduto) ? true : x.DescricaoProdutoConcorrente.Contains(descricaoProduto, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(fabricante) ? true : x.Produto != null && !string.IsNullOrWhiteSpace(x.Produto.Fabricante) && x.Produto.Fabricante.Contains(fabricante, StringComparison.OrdinalIgnoreCase))
                );
            var users = await usuarioRepository.GetUsersByCondition(x => true);
            var productList = mapper.Map<IEnumerable<ConcorrenteProdutoSearchDto>>(concorrenteProdutos);
            var prodList = from p in productList
                           join u in users
                           on p.UsuarioCadastro equals u.Id into grp
                           from user in grp.DefaultIfEmpty()
                           select changeProductUser(user.UserName ?? "", p);
            return productList;
        }
        public async Task<bool> UnLinkProduct(string CodigoProdutoConcorrente)
        {
            return await concorrenteProdutoRepository.UnlinkConcorrenteProduto(CodigoProdutoConcorrente);
        }

        private ConcorrenteProdutoSearchDto changeProductUser(string userName, ConcorrenteProdutoSearchDto p)
        {
            p.UsuarioCadastro = userName;
            return p;
        }
    }
}
