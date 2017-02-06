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
	public partial class Cupom : ConfigDomainCore,  IDisposable
	{
        protected IRepository<Cupom> rep;
		protected ICache cache;
		public ValidationHelper ValidationHelper = new ValidationHelper();

        public Cupom(IRepository<Cupom> rep, ICache cache):base()
        {
            this.rep = rep;
            this.cache = cache;
			this.Init();
			base.Config(this.cache);
        }

        public int CupomId { get; set;} 
        public string ChaveAcesso { get; set;} 
        public DateTime DataCompra { get; set;} 
        public string COO { get; set;} 
        public int TipoNota { get; set;} 
        public decimal Valor { get; set;} 
        public int EntidadeId { get; set;} 
        public int UsuarioId { get; set;} 
        public int? CadastradorId { get; set;} 
        public string CNPJEmissor { get; set;} 
        public int Situacao { get; set;} 
        public DateTime DataLancamento { get; set;} 
        public DateTime? DataProcessamento { get; set;} 
        public byte[] Imagem1 { get; set;} 
        public byte[] Imagem2 { get; set;} 


		public virtual void Warnings(CupomFilter filters)
        {
            ValidationHelper.AddDomainWarning<Cupom>("");
        }

		public virtual PaginateResult<dynamic> GetDataListCustomPaging(CupomFilter filters)
        {
			var result = this.GetDataListCustom(filters).AsQueryable();
            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDynamic(filters));
		}
        
        public virtual PaginateResult<Cupom> GetByFiltersPaging(CupomFilter filters, params Expression<Func<Cupom, object>>[] includes)
        {
            var result = GetByFilters(filters, includes);

            if (filters.IsOrderByDynamic)
                return this.Paging(filters, result, result.OrderByDynamic(filters));

            if (filters.IsOrderByDomain)
                return this.Paging(filters, result);

            return this.Paging(filters, result, result.OrderByDescending(_ => _.CupomId));
        }

        public virtual Cupom GetOne(Cupom model)
        {
            return this.Get(model);
        }
		
		public Cupom GetFromContext(Cupom model)
        {
			return this.rep.Get(model.CupomId);
        }

		protected IQueryable<Cupom> SimpleFilters(CupomFilter filters,IQueryable<Cupom> queryBase)
        {

			var queryFilter = queryBase;

            if (filters.CupomId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.CupomId == filters.CupomId);
			};
            if (filters.ChaveAcesso.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.ChaveAcesso.Contains(filters.ChaveAcesso));
			};
            if (filters.DataCompraStart.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.DataCompra >= filters.DataCompraStart );
			};
            if (filters.DataCompraEnd.IsSent()) 
			{ 
				filters.DataCompraEnd = filters.DataCompraEnd.AddDays(1).AddMilliseconds(-1);
				queryFilter = queryFilter.Where(_=>_.DataCompra  <= filters.DataCompraEnd);
			};

            if (filters.COO.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.COO.Contains(filters.COO));
			};
            if (filters.TipoNota.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.TipoNota == filters.TipoNota);
			};
            if (filters.Valor.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Valor == filters.Valor);
			};
            if (filters.EntidadeId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.EntidadeId == filters.EntidadeId);
			};
            if (filters.UsuarioId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.UsuarioId == filters.UsuarioId);
			};
            if (filters.CadastradorId.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.CadastradorId != null && _.CadastradorId.Value == filters.CadastradorId);
			};
            if (filters.CNPJEmissor.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.CNPJEmissor.Contains(filters.CNPJEmissor));
			};
            if (filters.Situacao.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Situacao == filters.Situacao);
			};
            if (filters.DataLancamentoStart.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.DataLancamento >= filters.DataLancamentoStart );
			};
            if (filters.DataLancamentoEnd.IsSent()) 
			{ 
				filters.DataLancamentoEnd = filters.DataLancamentoEnd.AddDays(1).AddMilliseconds(-1);
				queryFilter = queryFilter.Where(_=>_.DataLancamento  <= filters.DataLancamentoEnd);
			};

            if (filters.DataProcessamentoStart.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.DataProcessamento != null && _.DataProcessamento.Value >= filters.DataProcessamentoStart.Value);
			};
            if (filters.DataProcessamentoEnd.IsSent()) 
			{ 
				filters.DataProcessamentoEnd = filters.DataProcessamentoEnd.Value.AddDays(1).AddMilliseconds(-1);
				queryFilter = queryFilter.Where(_=>_.DataProcessamento != null &&  _.DataProcessamento.Value <= filters.DataProcessamentoEnd);
			};

            if (filters.Imagem1.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Imagem1 == filters.Imagem1);
			};
            if (filters.Imagem2.IsSent()) 
			{ 
				
				queryFilter = queryFilter.Where(_=>_.Imagem2 == filters.Imagem2);
			};


            return queryFilter;
        }

		public virtual IEnumerable<dynamic> GetDataListCustom(CupomFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CustomFieldOrder =_.CupomId,
				CupomId = _.CupomId	
            });
        }
		
		public virtual dynamic GetDataCustom(CupomFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
				CupomId = _.CupomId	
            }).SingleOrDefault();
        }

		public virtual Summary GetSummaryDataListCustom(IEnumerable<dynamic> result)
        {
            return new Summary
            {
                Total = result.Count()
            };
        }

		
		protected virtual Cupom UpdateDefault(Cupom model,Cupom modelOld)
		{
			var alvo = this.GetFromContext(model);
            model.TransferTo(alvo);
            this.rep.Update(alvo, modelOld);
			return model;
		}

		protected virtual bool Continue(Cupom model, Cupom modelOld)
        {
            return true;
        }

		protected Cupom SaveDefault(Cupom model, bool validation = true, bool questionToContinue = true)
        {
            var modelOld = this.Get(model);
            var isNew = modelOld.IsNull();

			if (questionToContinue)
            {
                if (Continue(model, modelOld) == false)
                    return model;
            }

			this.SetInitialValues(model);
			
            ValidationHelper.Validate<Cupom>(model);
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

		public virtual Cupom SavePartial(Cupom model)
        {
  		    model = SaveDefault(model, false);
			this.rep.Commit();
			return model;
        }
		public virtual void DeleteFromRepository(Cupom alvo)
        {
            this.rep.Delete(alvo);
			this.ClearCache();
        }

		public virtual void ClearCache()
        {
			if (this.cache.IsNotNull())
            {
                var tag = this.cache.Get("Cupom") as List<string>;
				if (tag.IsNull()) return;
                foreach (var item in tag)
                {
                    this.cache.Remove(item);    
                }
                this.cache.Remove("Cupom");
            }
            
        }
		
        protected virtual PaginateResult<Cupom> PagingAndDefineFields(CupomFilter filters, IQueryable<Cupom> queryFilter)
        {
            var queryOptimize = this.DefineFieldsGetByFilters(queryFilter, filters.QueryOptimizerBehavior);
			
			if (!filters.IsOrderByDomain)
                queryOptimize = queryOptimize.OrderByDynamic(filters);

            var paginateResult = this.Paging(filters, queryOptimize);
            var queryMapped = this.MapperGetByFiltersToDomainFields(queryFilter, paginateResult.ResultPaginatedData, filters.QueryOptimizerBehavior);

            return new PaginateResult<Cupom>
            {
                TotalCount = paginateResult.TotalCount,
                ResultPaginatedData = queryMapped,
                Source = queryFilter
            };
        }

        protected virtual Cupom GetAndDefineFields(IQueryable<Cupom> source, string queryOptimizerBehavior)
        {
            var queryOptimize = this.DefineFieldsGetOne(source, queryOptimizerBehavior);
            var queryMapped = this.MapperGetOneToDomainFields(source, queryOptimize, queryOptimizerBehavior);
            return queryMapped;
        }

        protected virtual dynamic DefineFieldsGetOne(IQueryable<Cupom> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                }).SingleOrDefault();
            }
            return source;
        }

        protected virtual IQueryable<dynamic> DefineFieldsGetByFilters(IQueryable<Cupom> source, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return source.Select(_ => new
                {

                });
            }
            return source;
        }

        protected virtual IQueryable<Cupom> MapperGetByFiltersToDomainFields(IQueryable<Cupom> source, IEnumerable<dynamic> result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return result.Select(_ => new Cupom
                {

                }).AsQueryable();
            }
            
			return result.Select(_=> (Cupom)_).AsQueryable();

        }

        protected virtual Cupom MapperGetOneToDomainFields(IQueryable<Cupom> source, dynamic result, string queryOptimizerBehavior)
        {
            if (queryOptimizerBehavior == "queryOptimizerBehavior")
            {
                return new Cupom
                {

                };
            }
            return source.SingleOrDefault();
        }


	
	}
}