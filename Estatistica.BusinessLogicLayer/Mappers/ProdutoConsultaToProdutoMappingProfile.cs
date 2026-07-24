using AutoMapper;
using Estatistica.BusinessLogicLayer.Data;
using Estatistica.DataAccessLayer.Entities;

namespace Estatistica.BusinessLogicLayer.Mappers;

public class ProdutoConsultaToProdutoMappingProfile : Profile
{
    public ProdutoConsultaToProdutoMappingProfile()
    {
        CreateMap<ProdutoConsulta.Content, Produto>()
            .ForMember(dest => dest.CodigoProduto,
                opt => opt.MapFrom(src => src.cod.ToString()))
            .ForMember(dest => dest.Descricao,
                opt => opt.MapFrom(src => src.descricaoCurta ?? string.Empty))
            .ForMember(dest => dest.CodigoBarra,
                opt => opt.MapFrom(src => src.codBarras ?? string.Empty))
            .ForMember(dest => dest.CodigoFabrica,
                opt => opt.MapFrom(src => src.codFabrica ?? string.Empty))
            .ForMember(dest => dest.CodigoMarca,
                opt => opt.MapFrom(src => src.marca != null ? src.marca.codigo : 0))
            .ForMember(dest => dest.DescricaoMarca,
                opt => opt.MapFrom(src => src.marca != null ? src.marca.descricao : string.Empty))
            .ForMember(dest => dest.Fabricante,
                opt => opt.MapFrom(src => src.divisaogerencial != null
                    ? src.divisaogerencial.fabricante
                    : string.Empty))
            .ForMember(dest => dest.Familia,
                opt => opt.MapFrom(src => src.divisaogerencial != null && src.divisaogerencial.familia != null
                    ? src.divisaogerencial.familia.descricao
                    : string.Empty))
            .ForMember(dest => dest.Unidade,
                opt => opt.MapFrom(src => src.um ?? string.Empty))
            .ForMember(dest => dest.Linha,
                opt => opt.MapFrom(src => src.divisaogerencial != null && src.divisaogerencial.linha != null
                    ? src.divisaogerencial.linha.descricao
                    : string.Empty))
            .ForMember(dest => dest.Tipo,
                opt => opt.MapFrom(src => src.divisaogerencial != null && src.divisaogerencial.tipo != null
                    ? src.divisaogerencial.tipo.descricao
                    : string.Empty))
            .ForMember(dest => dest.SubTipo,
                opt => opt.MapFrom(src => src.divisaogerencial != null && src.divisaogerencial.subtipo != null
                    ? src.divisaogerencial.subtipo.descricao
                    : string.Empty));
    }

}
