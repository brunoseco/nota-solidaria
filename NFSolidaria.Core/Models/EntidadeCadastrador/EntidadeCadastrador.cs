using System;
using System.Linq;
using Common.Domain;
using Common.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using NFSolidaria.Core.Filters;
using Common.Models;
using System.Threading.Tasks;
using NFSolidaria.Core.Base;

namespace NFSolidaria.Core.Domain
{
	public partial class EntidadeCadastrador : ConfigDomainCore,  IDisposable
	{
        protected IRepository<EntidadeCadastrador> rep;
		protected ICache cache;
		public ValidationHelper ValidationHelper = new ValidationHelper();

        public EntidadeCadastrador(IRepository<EntidadeCadastrador> rep, ICache cache):base()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
			base.Config(this.cache);
        }

        public int EntidadeId { get; set;} 
        public int CadastradorId { get; set;} 


		public virtual void Warnings(EntidadeCadastradorFilter filters)
        {
            ValidationHelper.AddDomainWarning<EntidadeCadastrador>("");
        }

		public virtual PaginateResult<dynamic> GetDataListCustomPaging(EntidadeCadastradorFilter filters)
        {
			var result = this.GetDataListCustom(filters).AsQueryable();
            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDynamic(filters));
		}
        
        public virtual PaginateResult<EntidadeCadastrador> GetByFiltersPaging(EntidadeCadastradorFilter filters, params Expression<Func<EntidadeCadastrador, object>>[] includes)
        {
            var result = GetByFilters(filters, includes);

            if (filters.IsOrderByDynamic)
                return this.Paging(filters, result, result.OrderByDynamic(filters));

            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDescending(_ => _.EntidadeId));
        }

        public virtual EntidadeCadastrador GetOne(EntidadeCadastrador model)
        {
            return this.Get(model);
        }
		
		public EntidadeCadastrador GetFromContext(EntidadeCadastrador model)
        {
			return this.rep.Get(model.EntidadeId,model.CadastradorId);
        }

		protected IQueryable<EntidadeCadastrador> SimpleFilters(EntidadeCadastradorFilter filters,IQueryable<EntidadeCadastrador> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.EntidadeId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.EntidadeId == filters.EntidadeId);
			};
            if (filters.CadastradorId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.CadastradorId == filters.CadastradorId);
			};


            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(EntidadeCadastradorFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.EntidadeId,
				EntidadeId = _.EntidadeId	
            });
        }
		
		public virtual dynamic GetDataCustom(EntidadeCadastradorFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				EntidadeId = _.EntidadeId	
            }).SingleOrDefault();
        }

		public virtual Summary GetSummaryDataListCustom(IEnumerable<dynamic> result)
        {
            return new Summary
            {
                Total = result.Count()
            };
        }

		
		protected virtual EntidadeCadastrador UpdateDefault(EntidadeCadastrador model,EntidadeCadastrador modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected virtual bool Continue(EntidadeCadastrador model, EntidadeCadastrador modelOld)
        {
            return true;
        }

		protected EntidadeCadastrador SaveDefault(EntidadeCadastrador model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                    return model;
            }

			this.SetInitialValues(model);
			
            ValidationHelper.Validate<EntidadeCadastrador>(model);
            this.ValidationReletedClasses(model, modelOld);
            if (validation) ValidationHelper.ValidateAll();

            this.DeleteCollectionsOnSave(model);

            if (isNew)
                this.rep.Add(model);
            else
				this.UpdateDefault(model, modelOld);
           
		    this.ClearCache();
            return model;
        }

		public virtual EntidadeCadastrador SavePartial(EntidadeCadastrador model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(EntidadeCadastrador alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.Get("EntidadeCadastrador") as List<string>;
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("EntidadeCadastrador");
            }
            
        }
		
        protected virtual PaginateResult<EntidadeCadastrador> PagingAndDefineFields(EntidadeCadastradorFilter filters, IQueryable<EntidadeCadastrador> queryFilter)
        {
            var queryOptimize = this.DefineFieldsGetByFilters(queryFilter, filters.QueryOptimizerBehavior);
			
			if (!filters.IsOrderByDomain)
                queryOptimize = queryOptimize.OrderByDynamic(filters);

            var paginateResult = this.Paging(filters, queryOptimize);
            var queryMapped = this.MapperGetByFiltersToDomainFields(queryFilter, paginateResult.ResultPaginatedData, filters.QueryOptimizerBehavior);

            return new PaginateResult<EntidadeCadastrador>
            {
                TotalCount = paginateResult.TotalCount,
                ResultPaginatedData = queryMapped,
                Source = queryFilter
            };
        }

        protected virtual EntidadeCadastrador GetAndDefineFields(IQueryable<EntidadeCadastrador> source, string queryOptimizerBehavior)
        {
            var queryOptimize = this.DefineFieldsGetOne(source, queryOptimizerBehavior);
            var queryMapped = this.MapperGetOneToDomainFields(source, queryOptimize, queryOptimizerBehavior);
            return queryMapped;
        }

        protected virtual dynamic DefineFieldsGetOne(IQueryable<EntidadeCadastrador> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                }).SingleOrDefault();
            }
            return source;
        }

        protected virtual IQueryable<dynamic> DefineFieldsGetByFilters(IQueryable<EntidadeCadastrador> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                });
            }
            return source;
        }

        protected virtual IQueryable<EntidadeCadastrador> MapperGetByFiltersToDomainFields(IQueryable<EntidadeCadastrador> source, IEnumerable<dynamic> result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return result.Select(_ => new EntidadeCadastrador
                {

                }).AsQueryable();
            }
            
			return result.Select(_=> (EntidadeCadastrador)_).AsQueryable();

        }

        protected virtual EntidadeCadastrador MapperGetOneToDomainFields(IQueryable<EntidadeCadastrador> source, dynamic result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return new EntidadeCadastrador
                {

                };
            }
            return source.SingleOrDefault();
        }


	
	}
}