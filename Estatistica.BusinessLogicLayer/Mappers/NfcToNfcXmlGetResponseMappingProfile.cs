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
    public class NfcToNfcXmlGetResponseMappingProfile : Profile
    {
        public NfcToNfcXmlGetResponseMappingProfile()
        {
            CreateMap<Nfc, NfcXmlGetResponse>()
                .ForMember(dest => dest.ChaveNfe, opt => opt.MapFrom(src => src.ChaveNfe))
                .ForMember(dest => dest.Concorrente, opt => opt.MapFrom(src => src.ConcorrenteCnpj.Concorrente.Nome))
                .ForMember(dest => dest.CnpjCliente, opt => opt.MapFrom(src => src.CnpjCliente))
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.NomeCliente))
                .ForMember(dest => dest.DataEmissao, opt => opt.MapFrom(src => src.DataEmissao))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataCadastro))
                .ForMember(dest => dest.IdUsuario, opt => opt.MapFrom(src => src.UsuarioCadastro));
        }
    }
}
