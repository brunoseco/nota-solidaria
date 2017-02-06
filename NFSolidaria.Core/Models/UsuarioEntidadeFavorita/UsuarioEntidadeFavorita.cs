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
	public partial class UsuarioEntidadeFavorita : ConfigDomainCore,  IDisposable
	{
        protected IRepository<UsuarioEntidadeFavorita> rep;
		protected ICache cache;
		public ValidationHelper ValidationHelper = new ValidationHelper();

        public UsuarioEntidadeFavorita(IRepository<UsuarioEntidadeFavorita> rep, ICache cache):base()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
			base.Config(this.cache);
        }

        public int UsuarioId { get; set;} 
        public int EntidadeId { get; set;} 


		public virtual void Warnings(UsuarioEntidadeFavoritaFilter filters)
        {
            ValidationHelper.AddDomainWarning<UsuarioEntidadeFavorita>("");
        }

		public virtual PaginateResult<dynamic> GetDataListCustomPaging(UsuarioEntidadeFavoritaFilter filters)
        {
			var result = this.GetDataListCustom(filters).AsQueryable();
            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDynamic(filters));
		}
        
        public virtual PaginateResult<UsuarioEntidadeFavorita> GetByFiltersPaging(UsuarioEntidadeFavoritaFilter filters, params Expression<Func<UsuarioEntidadeFavorita, object>>[] includes)
        {
            var result = GetByFilters(filters, includes);

            if (filters.IsOrderByDynamic)
                return this.Paging(filters, result, result.OrderByDynamic(filters));

            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDescending(_ => _.UsuarioId));
        }

        public virtual UsuarioEntidadeFavorita GetOne(UsuarioEntidadeFavorita model)
        {
            return this.Get(model);
        }
		
		public UsuarioEntidadeFavorita GetFromContext(UsuarioEntidadeFavorita model)
        {
			return this.rep.Get(model.UsuarioId,model.EntidadeId);
        }

		protected IQueryable<UsuarioEntidadeFavorita> SimpleFilters(UsuarioEntidadeFavoritaFilter filters,IQueryable<UsuarioEntidadeFavorita> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.UsuarioId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.UsuarioId == filters.UsuarioId);
			};
            if (filters.EntidadeId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.EntidadeId == filters.EntidadeId);
			};


            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(UsuarioEntidadeFavoritaFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.UsuarioId,
				UsuarioId = _.UsuarioId	
            });
        }
		
		public virtual dynamic GetDataCustom(UsuarioEntidadeFavoritaFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				UsuarioId = _.UsuarioId	
            }).SingleOrDefault();
        }

		public virtual Summary GetSummaryDataListCustom(IEnumerable<dynamic> result)
        {
            return new Summary
            {
                Total = result.Count()
            };
        }

		
		protected virtual UsuarioEntidadeFavorita UpdateDefault(UsuarioEntidadeFavorita model,UsuarioEntidadeFavorita modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected virtual bool Continue(UsuarioEntidadeFavorita model, UsuarioEntidadeFavorita modelOld)
        {
            return true;
        }

		protected UsuarioEntidadeFavorita SaveDefault(UsuarioEntidadeFavorita model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                    return model;
            }

			this.SetInitialValues(model);
			
            ValidationHelper.Validate<UsuarioEntidadeFavorita>(model);
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

		public virtual UsuarioEntidadeFavorita SavePartial(UsuarioEntidadeFavorita model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(UsuarioEntidadeFavorita alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.Get("UsuarioEntidadeFavorita") as List<string>;
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("UsuarioEntidadeFavorita");
            }
            
        }
		
        protected virtual PaginateResult<UsuarioEntidadeFavorita> PagingAndDefineFields(UsuarioEntidadeFavoritaFilter filters, IQueryable<UsuarioEntidadeFavorita> queryFilter)
        {
            var queryOptimize = this.DefineFieldsGetByFilters(queryFilter, filters.QueryOptimizerBehavior);
			
			if (!filters.IsOrderByDomain)
                queryOptimize = queryOptimize.OrderByDynamic(filters);

            var paginateResult = this.Paging(filters, queryOptimize);
            var queryMapped = this.MapperGetByFiltersToDomainFields(queryFilter, paginateResult.ResultPaginatedData, filters.QueryOptimizerBehavior);

            return new PaginateResult<UsuarioEntidadeFavorita>
            {
                TotalCount = paginateResult.TotalCount,
                ResultPaginatedData = queryMapped,
                Source = queryFilter
            };
        }

        protected virtual UsuarioEntidadeFavorita GetAndDefineFields(IQueryable<UsuarioEntidadeFavorita> source, string queryOptimizerBehavior)
        {
            var queryOptimize = this.DefineFieldsGetOne(source, queryOptimizerBehavior);
            var queryMapped = this.MapperGetOneToDomainFields(source, queryOptimize, queryOptimizerBehavior);
            return queryMapped;
        }

        protected virtual dynamic DefineFieldsGetOne(IQueryable<UsuarioEntidadeFavorita> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                }).SingleOrDefault();
            }
            return source;
        }

        protected virtual IQueryable<dynamic> DefineFieldsGetByFilters(IQueryable<UsuarioEntidadeFavorita> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                });
            }
            return source;
        }

        protected virtual IQueryable<UsuarioEntidadeFavorita> MapperGetByFiltersToDomainFields(IQueryable<UsuarioEntidadeFavorita> source, IEnumerable<dynamic> result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return result.Select(_ => new UsuarioEntidadeFavorita
                {

                }).AsQueryable();
            }
            
			return result.Select(_=> (UsuarioEntidadeFavorita)_).AsQueryable();

        }

        protected virtual UsuarioEntidadeFavorita MapperGetOneToDomainFields(IQueryable<UsuarioEntidadeFavorita> source, dynamic result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return new UsuarioEntidadeFavorita
                {

                };
            }
            return source.SingleOrDefault();
        }


	
	}
}