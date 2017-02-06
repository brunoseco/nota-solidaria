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
    public partial class CupomApp 
    {

		public void Init(string token)
        {
			this.cache = ConfigContainer.Container().GetInstance<ICache>();
            this.repCupom = ConfigContainer.Container().GetInstance<IRepository<Cupom>>();
            this.Cupom = new Cupom(this.repCupom, cache);
            this.Cupom.SetToken(token);
		}

		private IEnumerable<CupomDto> MapperDomainToResult(CupomFilter filter, PaginateResult<Cupom> dataList)
        {
            var result = filter.IsOnlySummary ? null : AutoMapper.Mapper.Map<IEnumerable<Cupom>, IEnumerable<CupomDtoSpecializedResult>>(dataList.ResultPaginatedData);
            return result;
        }

		private CupomDto MapperDomainToDtoOnSave(Cupom data, Common.Dto.DtoBase dto)
        {
            return AutoMapper.Mapper.Map<Cupom, CupomDto>(data);
        }

		private IEnumerable<CupomDto> MapperDomainToDtoOnSave(IEnumerable<Cupom> data, IEnumerable<Common.Dto.DtoBase> dtos)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Cupom>, IEnumerable<CupomDto>>(data);
        }
				
		private CupomDto MapperDomainToDtoSpecialized(Cupom data)
        {
            var result =  AutoMapper.Mapper.Map<Cupom, CupomDtoSpecialized>(data);
            return result;
        }
	}
}
