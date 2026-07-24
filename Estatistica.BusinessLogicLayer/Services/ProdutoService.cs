using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.BusinessLogicLayer.ServiceContracts;
using Estatistica.DataAccessLayer.ReporsitoryContracts;
using static Estatistica.BusinessLogicLayer.Data.ProdutoConsulta;

namespace Estatistica.BusinessLogicLayer.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository produtoRepository;
    private readonly IMapper mapper;

    public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper)
    {
        this.produtoRepository=produtoRepository;
        this.mapper=mapper;
    }

    public async Task AddProdutoRange(IEnumerable<Content> produtos)
    {
        var produtosEntity = mapper.Map<IEnumerable<Estatistica.DataAccessLayer.Entities.Produto>>(produtos);
        await produtoRepository.AddProdutoRange(produtosEntity);
    }

    public async Task<bool> CheckProductExists(string codigoProduto)
    {
        return (await produtoRepository.GetProductByCodigo(codigoProduto)) is not null;
    }

    public async Task<IEnumerable<ProductSuggestionResponse>> GetProdutosSuggestion(string descricao)
    {                     
        return mapper.Map<IEnumerable<ProductSuggestionResponse>>(await produtoRepository.GetProdutosBySugesstion(descricao));
    }

    public async Task<bool> UpdateProdutoPrice(string codigoProduto, decimal price,decimal custo)
    {
        var produto = await produtoRepository.GetProductByCodigo(codigoProduto);
        if (produto is null) return false;
        produto.PrecoVenda = price;
        produto.Custo = custo;
        await produtoRepository.UpdateProduto(produto);
        return true;
    }

    public async Task UpdateProdutoRange(IEnumerable<Content> produtos)
    {
        var produtosEntity = mapper.Map<IEnumerable<Estatistica.DataAccessLayer.Entities.Produto>>(produtos);
        await produtoRepository.UpdateProdutoRange(produtosEntity);
    }


}
