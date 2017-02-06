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
	public partial class Usuario : ConfigDomainCore,  IDisposable
	{
        protected IRepository<Usuario> rep;
		protected ICache cache;
		public ValidationHelper ValidationHelper = new ValidationHelper();

        public Usuario(IRepository<Usuario> rep, ICache cache):base()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
			base.Config(this.cache);
        }

        public int UsuarioId { get; set;} 
        public string CPF_CNPJ { get; set;} 
        public string Nome { get; set;} 
        public string RazaoSocial { get; set;} 
        public string Email { get; set;} 
        public DateTime? DataNascimento { get; set;} 
        public string SenhaMD5 { get; set;} 
        public string Cidade { get; set;} 
        public string UF { get; set;} 
        public string LastToken { get; set;} 


		public virtual void Warnings(UsuarioFilter filters)
        {
            ValidationHelper.AddDomainWarning<Usuario>("");
        }

		public virtual PaginateResult<dynamic> GetDataListCustomPaging(UsuarioFilter filters)
        {
			var result = this.GetDataListCustom(filters).AsQueryable();
            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDynamic(filters));
		}
        
        public virtual PaginateResult<Usuario> GetByFiltersPaging(UsuarioFilter filters, params Expression<Func<Usuario, object>>[] includes)
        {
            var result = GetByFilters(filters, includes);

            if (filters.IsOrderByDynamic)
                return this.Paging(filters, result, result.OrderByDynamic(filters));

            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDescending(_ => _.UsuarioId));
        }

        public virtual Usuario GetOne(Usuario model)
        {
            return this.Get(model);
        }
		
		public Usuario GetFromContext(Usuario model)
        {
			return this.rep.Get(model.UsuarioId);
        }

		protected IQueryable<Usuario> SimpleFilters(UsuarioFilter filters,IQueryable<Usuario> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.UsuarioId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.UsuarioId == filters.UsuarioId);
			};
            if (filters.CPF_CNPJ.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.CPF_CNPJ.Contains(filters.CPF_CNPJ));
			};
            if (filters.Nome.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Nome.Contains(filters.Nome));
			};
            if (filters.RazaoSocial.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.RazaoSocial.Contains(filters.RazaoSocial));
			};
            if (filters.Email.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Email.Contains(filters.Email));
			};
            if (filters.DataNascimentoStart.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.DataNascimento != null && _.DataNascimento.Value >= filters.DataNascimentoStart.Value);
			};
            if (filters.DataNascimentoEnd.IsSent()) 
			{ 
				filters.DataNascimentoEnd = filters.DataNascimentoEnd.Value.AddDays(1).AddMilliseconds(-1);
				queryFilter = queryFilter.Where(_=>_.DataNascimento != null &&  _.DataNascimento.Value <= filters.DataNascimentoEnd);
			};

            if (filters.SenhaMD5.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.SenhaMD5.Contains(filters.SenhaMD5));
			};
            if (filters.Cidade.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Cidade.Contains(filters.Cidade));
			};
            if (filters.UF.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.UF.Contains(filters.UF));
			};
            if (filters.LastToken.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.LastToken.Contains(filters.LastToken));
			};


            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(UsuarioFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.UsuarioId,
				UsuarioId = _.UsuarioId	
            });
        }
		
		public virtual dynamic GetDataCustom(UsuarioFilter filters)
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

		
		protected virtual Usuario UpdateDefault(Usuario model,Usuario modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected virtual bool Continue(Usuario model, Usuario modelOld)
        {
            return true;
        }

		protected Usuario SaveDefault(Usuario model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                    return model;
            }

			this.SetInitialValues(model);
			
            ValidationHelper.Validate<Usuario>(model);
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

		public virtual Usuario SavePartial(Usuario model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(Usuario alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.Get("Usuario") as List<string>;
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("Usuario");
            }
            
        }
		
        protected virtual PaginateResult<Usuario> PagingAndDefineFields(UsuarioFilter filters, IQueryable<Usuario> queryFilter)
        {
            var queryOptimize = this.DefineFieldsGetByFilters(queryFilter, filters.QueryOptimizerBehavior);
			
			if (!filters.IsOrderByDomain)
                queryOptimize = queryOptimize.OrderByDynamic(filters);

            var paginateResult = this.Paging(filters, queryOptimize);
            var queryMapped = this.MapperGetByFiltersToDomainFields(queryFilter, paginateResult.ResultPaginatedData, filters.QueryOptimizerBehavior);

            return new PaginateResult<Usuario>
            {
                TotalCount = paginateResult.TotalCount,
                ResultPaginatedData = queryMapped,
                Source = queryFilter
            };
        }

        protected virtual Usuario GetAndDefineFields(IQueryable<Usuario> source, string queryOptimizerBehavior)
        {
            var queryOptimize = this.DefineFieldsGetOne(source, queryOptimizerBehavior);
            var queryMapped = this.MapperGetOneToDomainFields(source, queryOptimize, queryOptimizerBehavior);
            return queryMapped;
        }

        protected virtual dynamic DefineFieldsGetOne(IQueryable<Usuario> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                }).SingleOrDefault();
            }
            return source;
        }

        protected virtual IQueryable<dynamic> DefineFieldsGetByFilters(IQueryable<Usuario> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                });
            }
            return source;
        }

        protected virtual IQueryable<Usuario> MapperGetByFiltersToDomainFields(IQueryable<Usuario> source, IEnumerable<dynamic> result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return result.Select(_ => new Usuario
                {

                }).AsQueryable();
            }
            
			return result.Select(_=> (Usuario)_).AsQueryable();

        }

        protected virtual Usuario MapperGetOneToDomainFields(IQueryable<Usuario> source, dynamic result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return new Usuario
                {

                };
            }
            return source.SingleOrDefault();
        }


	
	}
}