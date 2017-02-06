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
    public partial class CadastradorApp 
    {

		public void Init(string token)
        {
			this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.repCadastrador = ConfigContainer.Container().GetInstance<IRepository<Cadastrador>>();
            this.Cadastrador = new Cadastrador(this.repCadastrador, cache);
            this.Cadastrador.SetToken(token);
		}

		private IEnumerable<CadastradorDto> MapperDomainToResult(CadastradorFilter filter, PaginateResult<Cadastrador> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<Cadastrador>, IEnumerable<CadastradorDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private CadastradorDto MapperDomainToDtoOnSave(Cadastrador data, Common.Dto.DtoBase dto)
        {
            return AutoMapper.Mapper.Map<Cadastrador, CadastradorDto>(data);
        }

		private IEnumerable<CadastradorDto> MapperDomainToDtoOnSave(IEnumerable<Cadastrador> data, IEnumerable<Common.Dto.DtoBase> dtos)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Cadastrador>, IEnumerable<CadastradorDto>>(data);
        }
				
		private CadastradorDto MapperDomainToDtoSpecialized(Cadastrador data)
        {
            var result =  AutoMapper.Mapper.Map<Cadastrador, CadastradorDtoSpecialized>(data);
            return result;
        }
	}
}
