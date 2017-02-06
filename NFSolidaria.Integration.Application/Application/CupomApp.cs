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
using Common.Domain;
using System.Transactions;
using Common.Infrastructure.Log;

namespace NFSolidaria.Integration.Application
{
    public partial class CupomApp : IDisposable
    {
        private IRepository<Cupom> repCupom;
        private ICache cache;
        private Cupom Cupom;
		public ValidationHelper ValidationHelper;

        public CupomApp(string token)
        {
			this.Init(token);
			this.ValidationHelper = Cupom.ValidationHelper;
        }
			

		public CupomDto SavePartial(CupomDtoSpecialized dto)
        {
			var result = default(CupomDto);
			using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))            
			{
				var model =  AutoMapper.Mapper.Map<CupomDtoSpecialized, Cupom>(dto);
				var data = this.Cupom.SavePartial(model);
				result =  this.MapperDomainToDtoOnSave(data, dto);
				transaction.Complete();
			}
			return result;
        }

        public void Dispose()
        {
            this.Cupom.Dispose();
        }
    }
}
