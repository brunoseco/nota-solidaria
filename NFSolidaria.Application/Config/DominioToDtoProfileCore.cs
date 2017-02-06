using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NFSolidaria.Core.Domain;
using NFSolidaria.Core.Dto;

namespace NFSolidaria.Core.Application.Config
{
    public class DominioToDtoProfileCore : AutoMapper.Profile
    {


        protected override void Configure()
        {

            AutoMapper.Mapper.CreateMap<Cadastrador, CadastradorDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Cadastrador, CadastradorDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Cadastrador, CadastradorDtoSpecializedResult>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Cupom, CupomDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Cupom, CupomDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Cupom, CupomDtoSpecializedResult>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Entidade, EntidadeDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Entidade, EntidadeDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Entidade, EntidadeDtoSpecializedResult>().ReverseMap();
            AutoMapper.Mapper.CreateMap<EntidadeCadastrador, EntidadeCadastradorDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<EntidadeCadastrador, EntidadeCadastradorDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<EntidadeCadastrador, EntidadeCadastradorDtoSpecializedResult>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Usuario, UsuarioDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Usuario, UsuarioDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<Usuario, UsuarioDtoSpecializedResult>().ReverseMap();
            AutoMapper.Mapper.CreateMap<UsuarioEntidadeFavorita, UsuarioEntidadeFavoritaDto>().ReverseMap();
            AutoMapper.Mapper.CreateMap<UsuarioEntidadeFavorita, UsuarioEntidadeFavoritaDtoSpecialized>().ReverseMap();
            AutoMapper.Mapper.CreateMap<UsuarioEntidadeFavorita, UsuarioEntidadeFavoritaDtoSpecializedResult>().ReverseMap();


        }
    }
}
