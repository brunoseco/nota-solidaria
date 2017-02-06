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

namespace NFSolidaria.Core.Application
{
    public partial class EntidadeApp 
    {

		public void Init(string token)
        {
			this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.repEntidade = ConfigContainer.Container().GetInstance<IRepository<Entidade>>();
            this.Entidade = new EntidadeCustom(this.repEntidade, cache);
            this.Entidade.SetToken(token);
		}

		private IEnumerable<EntidadeDto> MapperDomainToResult(EntidadeFilter filter, PaginateResult<Entidade> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<Entidade>, IEnumerable<EntidadeDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private EntidadeDto MapperDomainToDtoOnSave(Entidade data, Common.Dto.DtoBase dto)
        {
            return AutoMapper.Mapper.Map<Entidade, EntidadeDto>(data);
        }

		private IEnumerable<EntidadeDto> MapperDomainToDtoOnSave(IEnumerable<Entidade> data, IEnumerable<Common.Dto.DtoBase> dtos)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Entidade>, IEnumerable<EntidadeDto>>(data);
        }
				
		private EntidadeDto MapperDomainToDtoSpecialized(Entidade data)
        {
            var result =  AutoMapper.Mapper.Map<Entidade, EntidadeDtoSpecialized>(data);
            return result;
        }
	}
}
