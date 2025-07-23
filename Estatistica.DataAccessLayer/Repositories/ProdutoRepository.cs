using Estatistica.DataAccessLayer.Context;
using Estatistica.DataAccessLayer.Entities;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.DataAccessLayer.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext context;

        public ProdutoRepository(ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task<Produto> AddProduto(Produto produto)
        {
            produto.DataCadastro = DateTime.Now;
            await context.Produtos.AddAsync(produto);
            await context.SaveChangesAsync();
            return produto;
        }

        public async Task<bool> AddProdutoRange(IEnumerable<Produto> produtoList)
        {
            foreach (var prod in produtoList)
                prod.DataCadastro = DateTime.Now;
            await context.Produtos.AddRangeAsync(produtoList);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Produto?> GetProductByCodigo(string codigoProduto)
        {
            return await context.Produtos.FindAsync(codigoProduto);
        }

        public async Task<Produto?> GetProductByCodigoBarra(string codigoBarra)
        {
            return await context.Produtos.FirstOrDefaultAsync(x => x.CodigoBarra == codigoBarra);
        }

        public async Task<Produto> UpdateProduto(Produto produto)
        {
            var produtoDb = await context.Produtos.FindAsync(produto.CodigoProduto);
            if (produtoDb is null)
                throw new ArgumentNullException("Produto nao encontrado");
            produtoDb.Descricao = produto.Descricao;
            produtoDb.SubTipo = produto.SubTipo;
            produtoDb.Linha = produto.Linha;
            produtoDb.Tipo = produto.Tipo;
            produtoDb.CodigoBarra = produto.CodigoBarra;
            produtoDb.CodigoFabrica = produto.CodigoFabrica;
            produtoDb.Familia = produto.Familia;
            produtoDb.Fabricante = produto.Fabricante;
            produtoDb.CodigoMarca = produto.CodigoMarca;
            produtoDb.DescricaoMarca = produto.DescricaoMarca;
            produtoDb.Unidade = produto.Unidade;
            await context.SaveChangesAsync();
            return produtoDb;
        }

        public async Task<bool> UpdateProdutoRange(IEnumerable<Produto> produtoList)
        {
            foreach (var produto in produtoList)
            {
                var produtoDb = await context.Produtos.FindAsync(produto.CodigoProduto);
                if (produtoDb is not null)
                {
                    produtoDb.Descricao = produto.Descricao;
                    produtoDb.SubTipo = produto.SubTipo;
                    produtoDb.Linha = produto.Linha;
                    produtoDb.Tipo = produto.Tipo;
                    produtoDb.CodigoBarra = produto.CodigoBarra;
                    produtoDb.CodigoFabrica = produto.CodigoFabrica;
                    produtoDb.Familia = produto.Familia;
                    produtoDb.Fabricante = produto.Fabricante;
                    produtoDb.CodigoMarca = produto.CodigoMarca;
                    produtoDb.DescricaoMarca = produto.DescricaoMarca;
                    produtoDb.Unidade = produto.Unidade;
                }
            }
            await context.SaveChangesAsync();
            return true;
        }
    }
}
