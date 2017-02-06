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
	public partial class Cadastrador : ConfigDomainCore,  IDisposable
	{
        protected IRepository<Cadastrador> rep;
		protected ICache cache;
		public ValidationHelper ValidationHelper = new ValidationHelper();

        public Cadastrador(IRepository<Cadastrador> rep, ICache cache):base()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
			base.Config(this.cache);
        }

        public int CadastradorId { get; set;} 
        public string Pass { get; set;} 
        public bool Ativo { get; set;} 


		public virtual void Warnings(CadastradorFilter filters)
        {
            ValidationHelper.AddDomainWarning<Cadastrador>("");
        }

		public virtual PaginateResult<dynamic> GetDataListCustomPaging(CadastradorFilter filters)
        {
			var result = this.GetDataListCustom(filters).AsQueryable();
            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDynamic(filters));
		}
        
        public virtual PaginateResult<Cadastrador> GetByFiltersPaging(CadastradorFilter filters, params Expression<Func<Cadastrador, object>>[] includes)
        {
            var result = GetByFilters(filters, includes);

            if (filters.IsOrderByDynamic)
                return this.Paging(filters, result, result.OrderByDynamic(filters));

            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDescending(_ => _.CadastradorId));
        }

        public virtual Cadastrador GetOne(Cadastrador model)
        {
            return this.Get(model);
        }
		
		public Cadastrador GetFromContext(Cadastrador model)
        {
			return this.rep.Get(model.CadastradorId);
        }

		protected IQueryable<Cadastrador> SimpleFilters(CadastradorFilter filters,IQueryable<Cadastrador> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.CadastradorId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.CadastradorId == filters.CadastradorId);
			};
            if (filters.Pass.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Pass.Contains(filters.Pass));
			};
            if (filters.Ativo.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Ativo == filters.Ativo);
			};


            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(CadastradorFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.CadastradorId,
				CadastradorId = _.CadastradorId	
            });
        }
		
		public virtual dynamic GetDataCustom(CadastradorFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CadastradorId = _.CadastradorId	
            }).SingleOrDefault();
        }

		public virtual Summary GetSummaryDataListCustom(IEnumerable<dynamic> result)
        {
            return new Summary
            {
                Total = result.Count()
            };
        }

		
		protected virtual Cadastrador UpdateDefault(Cadastrador model,Cadastrador modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected virtual bool Continue(Cadastrador model, Cadastrador modelOld)
        {
            return true;
        }

		protected Cadastrador SaveDefault(Cadastrador model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                    return model;
            }

			this.SetInitialValues(model);
			
            ValidationHelper.Validate<Cadastrador>(model);
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

		public virtual Cadastrador SavePartial(Cadastrador model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(Cadastrador alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.Get("Cadastrador") as List<string>;
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("Cadastrador");
            }
            
        }
		
        protected virtual PaginateResult<Cadastrador> PagingAndDefineFields(CadastradorFilter filters, IQueryable<Cadastrador> queryFilter)
        {
            var queryOptimize = this.DefineFieldsGetByFilters(queryFilter, filters.QueryOptimizerBehavior);
			
			if (!filters.IsOrderByDomain)
                queryOptimize = queryOptimize.OrderByDynamic(filters);

            var paginateResult = this.Paging(filters, queryOptimize);
            var queryMapped = this.MapperGetByFiltersToDomainFields(queryFilter, paginateResult.ResultPaginatedData, filters.QueryOptimizerBehavior);

            return new PaginateResult<Cadastrador>
            {
                TotalCount = paginateResult.TotalCount,
                ResultPaginatedData = queryMapped,
                Source = queryFilter
            };
        }

        protected virtual Cadastrador GetAndDefineFields(IQueryable<Cadastrador> source, string queryOptimizerBehavior)
        {
            var queryOptimize = this.DefineFieldsGetOne(source, queryOptimizerBehavior);
            var queryMapped = this.MapperGetOneToDomainFields(source, queryOptimize, queryOptimizerBehavior);
            return queryMapped;
        }

        protected virtual dynamic DefineFieldsGetOne(IQueryable<Cadastrador> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                }).SingleOrDefault();
            }
            return source;
        }

        protected virtual IQueryable<dynamic> DefineFieldsGetByFilters(IQueryable<Cadastrador> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                });
            }
            return source;
        }

        protected virtual IQueryable<Cadastrador> MapperGetByFiltersToDomainFields(IQueryable<Cadastrador> source, IEnumerable<dynamic> result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return result.Select(_ => new Cadastrador
                {

                }).AsQueryable();
            }
            
			return result.Select(_=> (Cadastrador)_).AsQueryable();

        }

        protected virtual Cadastrador MapperGetOneToDomainFields(IQueryable<Cadastrador> source, dynamic result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return new Cadastrador
                {

                };
            }
            return source.SingleOrDefault();
        }


	
	}
}