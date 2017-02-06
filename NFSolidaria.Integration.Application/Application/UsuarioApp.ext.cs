using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Interfaces;
using Common.Infrastructure.Cache;
using System.Collections.Generic;
using Common.Models;
using NFSolidaria.Core.Filters;
using Common.Interfaces;
using NFSolidaria.Core.Dto;
using NFSolidaria.Core.Domain;

namespace NFSolidaria.Integration.Application
{
    public partial class UsuarioApp 
    {
        private ICripto cripto;

		public void Init(string token)
        {
            this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.cripto = ConfigContainer.Container().GetInstance<ICripto>();
            this.repUsuario = ConfigContainer.Container().GetInstance<IRepository<Usuario>>();
            this.Usuario = new UsuarioCustom(this.repUsuario, cache, cripto);
            this.Usuario.SetToken(token);
		}

		private IEnumerable<UsuarioDto> MapperDomainToResult(UsuarioFilter filter, PaginateResult<Usuario> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private UsuarioDto MapperDomainToDtoOnSave(Usuario data, Common.Dto.DtoBase dto)
        {
            return AutoMapper.Mapper.Map<Usuario, UsuarioDto>(data);
        }

		private IEnumerable<UsuarioDto> MapperDomainToDtoOnSave(IEnumerable<Usuario> data, IEnumerable<Common.Dto.DtoBase> dtos)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioDto>>(data);
        }
				
		private UsuarioDto MapperDomainToDtoSpecialized(Usuario data)
        {
            var result =  AutoMapper.Mapper.Map<Usuario, UsuarioDtoSpecialized>(data);
            return result;
        }
	}
}
