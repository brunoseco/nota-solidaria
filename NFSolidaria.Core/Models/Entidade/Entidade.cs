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
	public partial class Entidade : ConfigDomainCore,  IDisposable
	{
        protected IRepository<Entidade> rep;
		protected ICache cache;
		public ValidationHelper ValidationHelper = new ValidationHelper();

        public Entidade(IRepository<Entidade> rep, ICache cache):base()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
			base.Config(this.cache);
        }

        public int EntidadeId { get; set;} 
        public string IdentificadorNFP { get; set;} 
        public bool Ativo { get; set;} 


		public virtual void Warnings(EntidadeFilter filters)
        {
            ValidationHelper.AddDomainWarning<Entidade>("");
        }

		public virtual PaginateResult<dynamic> GetDataListCustomPaging(EntidadeFilter filters)
        {
			var result = this.GetDataListCustom(filters).AsQueryable();
            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDynamic(filters));
		}
        
        public virtual PaginateResult<Entidade> GetByFiltersPaging(EntidadeFilter filters, params Expression<Func<Entidade, object>>[] includes)
        {
            var result = GetByFilters(filters, includes);

            if (filters.IsOrderByDynamic)
                return this.Paging(filters, result, result.OrderByDynamic(filters));

            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDescending(_ => _.EntidadeId));
        }

        public virtual Entidade GetOne(Entidade model)
        {
            return this.Get(model);
        }
		
		public Entidade GetFromContext(Entidade model)
        {
			return this.rep.Get(model.EntidadeId);
        }

		protected IQueryable<Entidade> SimpleFilters(EntidadeFilter filters,IQueryable<Entidade> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.EntidadeId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.EntidadeId == filters.EntidadeId);
			};
            if (filters.IdentificadorNFP.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.IdentificadorNFP.Contains(filters.IdentificadorNFP));
			};
            if (filters.Ativo.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Ativo == filters.Ativo);
			};


            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(EntidadeFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.EntidadeId,
				EntidadeId = _.EntidadeId	
            });
        }
		
		public virtual dynamic GetDataCustom(EntidadeFilter filters)
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

		
		protected virtual Entidade UpdateDefault(Entidade model,Entidade modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected virtual bool Continue(Entidade model, Entidade modelOld)
        {
            return true;
        }

		protected Entidade SaveDefault(Entidade model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                    return model;
            }

			this.SetInitialValues(model);
			
            ValidationHelper.Validate<Entidade>(model);
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

		public virtual Entidade SavePartial(Entidade model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(Entidade alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.Get("Entidade") as List<string>;
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("Entidade");
            }
            
        }
		
        protected virtual PaginateResult<Entidade> PagingAndDefineFields(EntidadeFilter filters, IQueryable<Entidade> queryFilter)
        {
            var queryOptimize = this.DefineFieldsGetByFilters(queryFilter, filters.QueryOptimizerBehavior);
			
			if (!filters.IsOrderByDomain)
                queryOptimize = queryOptimize.OrderByDynamic(filters);

            var paginateResult = this.Paging(filters, queryOptimize);
            var queryMapped = this.MapperGetByFiltersToDomainFields(queryFilter, paginateResult.ResultPaginatedData, filters.QueryOptimizerBehavior);

            return new PaginateResult<Entidade>
            {
                TotalCount = paginateResult.TotalCount,
                ResultPaginatedData = queryMapped,
                Source = queryFilter
            };
        }

        protected virtual Entidade GetAndDefineFields(IQueryable<Entidade> source, string queryOptimizerBehavior)
        {
            var queryOptimize = this.DefineFieldsGetOne(source, queryOptimizerBehavior);
            var queryMapped = this.MapperGetOneToDomainFields(source, queryOptimize, queryOptimizerBehavior);
            return queryMapped;
        }

        protected virtual dynamic DefineFieldsGetOne(IQueryable<Entidade> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                }).SingleOrDefault();
            }
            return source;
        }

        protected virtual IQueryable<dynamic> DefineFieldsGetByFilters(IQueryable<Entidade> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                });
            }
            return source;
        }

        protected virtual IQueryable<Entidade> MapperGetByFiltersToDomainFields(IQueryable<Entidade> source, IEnumerable<dynamic> result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return result.Select(_ => new Entidade
                {

                }).AsQueryable();
            }
            
			return result.Select(_=> (Entidade)_).AsQueryable();

        }

        protected virtual Entidade MapperGetOneToDomainFields(IQueryable<Entidade> source, dynamic result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return new Entidade
                {

                };
            }
            return source.SingleOrDefault();
        }


	
	}
}