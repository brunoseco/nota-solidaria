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
    public partial class UsuarioEntidadeFavoritaApp 
    {

		public void Init(string token)
        {
			this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.repUsuarioEntidadeFavorita = ConfigContainer.Container().GetInstance<IRepository<UsuarioEntidadeFavorita>>();
            this.UsuarioEntidadeFavorita = new UsuarioEntidadeFavorita(this.repUsuarioEntidadeFavorita, cache);
            this.UsuarioEntidadeFavorita.SetToken(token);
		}

		private IEnumerable<UsuarioEntidadeFavoritaDto> MapperDomainToResult(UsuarioEntidadeFavoritaFilter filter, PaginateResult<UsuarioEntidadeFavorita> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<UsuarioEntidadeFavorita>, IEnumerable<UsuarioEntidadeFavoritaDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private UsuarioEntidadeFavoritaDto MapperDomainToDtoOnSave(UsuarioEntidadeFavorita data, Common.Dto.DtoBase dto)
        {
            return AutoMapper.Mapper.Map<UsuarioEntidadeFavorita, UsuarioEntidadeFavoritaDto>(data);
        }

		private IEnumerable<UsuarioEntidadeFavoritaDto> MapperDomainToDtoOnSave(IEnumerable<UsuarioEntidadeFavorita> data, IEnumerable<Common.Dto.DtoBase> dtos)
        {
            return AutoMapper.Mapper.Map<IEnumerable<UsuarioEntidadeFavorita>, IEnumerable<UsuarioEntidadeFavoritaDto>>(data);
        }
				
		private UsuarioEntidadeFavoritaDto MapperDomainToDtoSpecialized(UsuarioEntidadeFavorita data)
        {
            var result =  AutoMapper.Mapper.Map<UsuarioEntidadeFavorita, UsuarioEntidadeFavoritaDtoSpecialized>(data);
            return result;
        }
	}
}
