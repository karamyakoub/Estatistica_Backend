using AutoMapper;
using Estatistica.BusinessLogicLayer.DTO;
using Estatistica.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estatistica.BusinessLogicLayer.Mappers
{
    public class ConcorrenteProdutoToConcorrenteProdutoSearchDTOMappingProfile : Profile
    {
        public ConcorrenteProdutoToConcorrenteProdutoSearchDTOMappingProfile()
        {
            CreateMap<ConcorrenteProduto, ConcorrenteProdutoSearchDto>()                
                .ForMember(dest => dest.Concorrente, opt => opt.MapFrom(src => src.Concorrente.Nome))                
                .ForMember(dest => dest.CodigoProduto, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.CodigoProduto))
                .ForMember(dest => dest.CodigoFabrica, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.CodigoFabrica))
                .ForMember(dest => dest.CodigoBarra, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.CodigoBarra))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.Descricao))
                .ForMember(dest => dest.Fabricante, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.Fabricante))
                .ForMember(dest => dest.CodigoMarca, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.CodigoMarca))
                .ForMember(dest => dest.DescricaoMarca, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.DescricaoMarca))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.Tipo))
                .ForMember(dest => dest.SubTipo, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.SubTipo))
                .ForMember(dest => dest.Linha, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.Linha))
                .ForMember(dest => dest.Familia, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.Familia))
                .ForMember(dest => dest.Unidade, opt => opt.MapFrom(src => src.Produto == null ? null : src.Produto.Unidade));
        }
    }
}
