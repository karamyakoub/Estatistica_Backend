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
    public class LogProdutoConcorrenteToLogProdutoConcorrenteMappingProfile : Profile
    {
        public LogProdutoConcorrenteToLogProdutoConcorrenteMappingProfile()
        {
            CreateMap<LogConcorrenteProduto, LogConcorrenteProdutoGetResponse>()
                .ForMember(dest => dest.Concorrente, opt => opt.MapFrom(src => src.Concorrente.Nome))
                .ForMember(dest => dest.CodigoProdutoConcorrente, opt => opt.MapFrom(src => src.CodigoProdutoConcorrente.CodigoProdutoConcorrente))
                .ForMember(dest => dest.CodigoProdutoAnt, opt => opt.MapFrom(src => src.CodigoProdutoAnt == null ? string.Empty : src.CodigoProdutoAnt.CodigoProduto))
                .ForMember(dest => dest.CodigoProdutoAtual, opt => opt.MapFrom(src => src.CodigoProdutoAtual == null ? string.Empty : src.CodigoProdutoAtual.CodigoProduto))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioCadastro == null ? string.Empty : src.UsuarioCadastro));
                
        }
    }
}
