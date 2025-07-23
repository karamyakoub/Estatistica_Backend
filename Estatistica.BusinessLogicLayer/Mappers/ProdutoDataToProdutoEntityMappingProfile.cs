using AutoMapper;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class ProdutoDataToProdutoEntityMappingProfile : Profile
    {
        public ProdutoDataToProdutoEntityMappingProfile()
        {
            CreateMap<Estatistica.BusinessLogicLayer.Data.Produto, Estatistica.DataAccessLayer.Entities.Produto>()
                .ForMember(dst => dst.CodigoProduto, opt => opt.MapFrom(x => x.cod))
                .ForMember(dst => dst.Descricao, opt => opt.MapFrom(x => x.desc))
                .ForMember(dst => dst.CodigoMarca, opt => opt.MapFrom(x => x.marca.codigo))
                .ForMember(dst => dst.DescricaoMarca, opt => opt.MapFrom(x => x.marca.descricao))
                .ForMember(dst => dst.Fabricante, opt => opt.MapFrom(x => x.divisaogerencial.fabricante))
                .ForMember(dst => dst.Unidade, opt => opt.MapFrom(x => x.umEcommerce))
                .ForMember(dst => dst.CodigoProduto, opt => opt.MapFrom(x => x.cod))
                .ForMember(dst => dst.CodigoFabrica, opt => opt.MapFrom(x => x.codFabrica))
                .ForMember(dst => dst.CodigoBarra, opt => opt.MapFrom(x => x.codBarras))
                .ForMember(dst => dst.Tipo, opt => opt.MapFrom(x => x.divisaogerencial.tipo.descricao))
                .ForMember(dst => dst.SubTipo, opt => opt.MapFrom(x => x.divisaogerencial.subtipo.descricao))
                .ForMember(dst => dst.Linha, opt => opt.MapFrom(x => x.divisaogerencial.linha.descricao))
                .ForMember(dst => dst.Familia, opt => opt.MapFrom(x => x.divisaogerencial.familia.descricao))                
                .ReverseMap();
        }
    }
}
