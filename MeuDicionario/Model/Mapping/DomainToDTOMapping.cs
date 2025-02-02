using AutoMapper;
using MeuDicionario.Model.DTOs;

namespace MeuDicionario.Model.Mapping
{
    public class DomainToDTOMapping : Profile
    {
        public DomainToDTOMapping()
        {
            CreateMap<WordCreate, Word>();
            CreateMap<TextCreate, Text>();
        }
    }
}
