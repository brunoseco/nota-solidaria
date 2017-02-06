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
    public partial class EntidadeApp : IDisposable
    {
        private IRepository<Entidade> repEntidade;
        private ICache cache;
        private Entidade Entidade;
        public ValidationHelper ValidationHelper;

        public EntidadeApp(string token)
        {

            this.Init(token);
            this.ValidationHelper = Entidade.ValidationHelper;
        }

        public List<dynamic> GetDataListCustom(EntidadeFilter filters)
        {
            var result = default(List<dynamic>);
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                result = this.Entidade.GetDataListCustom(filters).ToList();
            }
            return result;
        }

        public void Dispose()
        {
            this.Entidade.Dispose();
        }
    }
}
