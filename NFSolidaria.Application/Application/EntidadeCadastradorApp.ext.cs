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
    public partial class EntidadeCadastradorApp 
    {

		public void Init(string token)
        {
			this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.repEntidadeCadastrador = ConfigContainer.Container().GetInstance<IRepository<EntidadeCadastrador>>();
            this.EntidadeCadastrador = new EntidadeCadastrador(this.repEntidadeCadastrador, cache);
            this.EntidadeCadastrador.SetToken(token);
		}

		private IEnumerable<EntidadeCadastradorDto> MapperDomainToResult(EntidadeCadastradorFilter filter, PaginateResult<EntidadeCadastrador> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<EntidadeCadastrador>, IEnumerable<EntidadeCadastradorDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private EntidadeCadastradorDto MapperDomainToDtoOnSave(EntidadeCadastrador data, Common.Dto.DtoBase dto)
        {
            return AutoMapper.Mapper.Map<EntidadeCadastrador, EntidadeCadastradorDto>(data);
        }

		private IEnumerable<EntidadeCadastradorDto> MapperDomainToDtoOnSave(IEnumerable<EntidadeCadastrador> data, IEnumerable<Common.Dto.DtoBase> dtos)
        {
            return AutoMapper.Mapper.Map<IEnumerable<EntidadeCadastrador>, IEnumerable<EntidadeCadastradorDto>>(data);
        }
				
		private EntidadeCadastradorDto MapperDomainToDtoSpecialized(EntidadeCadastrador data)
        {
            var result =  AutoMapper.Mapper.Map<EntidadeCadastrador, EntidadeCadastradorDtoSpecialized>(data);
            return result;
        }
	}
}
