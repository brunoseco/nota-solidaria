using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Models;
using Common.Domain;
using NFSolidaria.Core.Filters;
using Common.Interfaces;
using Common.Domain.Interfaces;
using Common.Domain.CustomExceptions;

namespace NFSolidaria.Core.Domain
{	
	[MetadataType(typeof(CadastradorValidation))]
	public partial class Cadastrador
	{
		public Cadastrador()
        {
        }


        public virtual Usuario Usuario { get; set; }
        public Cadastrador Get(Cadastrador model)
        {
            return this.rep.GetAll(this.DataAgregation(new CadastradorFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_=>_.CadastradorId == model.CadastradorId).SingleOrDefault();
        }

		public IQueryable<Cadastrador> GetByFilters(CadastradorFilter filters, params Expression<Func<Cadastrador, object>>[] includes)
        {
			var queryBase = this.rep.GetAllAsNoTracking(this.DataAgregation(includes,filters));
			var queryFilter = queryBase;
			return this.SimpleFilters(filters, queryBase);
        }

		public IEnumerable<DataItem> GetDataItem(CadastradorFilter filters)
		{
			var dataList = this.GetByFilters(filters)
				.Select(_ => new DataItem
				{
					Id = _.CadastradorId.ToString(),
				});

			return dataList.ToList();
		}

		public Common.Models.Summary GetSummary(IQueryable<Cadastrador> result)
		{
			return new Common.Models.Summary
            {
                Total = this.TotalCount
            };
		}

		public Cadastrador Save()
        {
            return Save(this);
        }

		public Cadastrador Save(Cadastrador model)
        {
			model = SaveDefault(model);
            this.rep.Commit();
            return model;
        }

		public IEnumerable<Cadastrador> Save(IEnumerable<Cadastrador> models)
        {
            var modelsInserted = new List<Cadastrador>();
            foreach (var item in models)
            {
                 modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

		public void Delete(Cadastrador model)
        {
            if (model.IsNull())
                throw new CustomBadRequestException("Delete sem parametros");

            var alvo = this.Get(model);
            this.DeleteFromRepository(alvo);
            this.rep.Commit();
        }

		private void SetInitialValues(Cadastrador model)
        {
		    

        }
		
		private void ValidationReletedClasses(Cadastrador model, Cadastrador modelOld)
        {

		}

		private void DeleteCollectionsOnSave(Cadastrador model)
        { 
        
        }

		protected virtual Expression<Func<Cadastrador, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<Cadastrador, object>>[] { }, filter);
        }

        protected virtual Expression<Func<Cadastrador, object>>[] DataAgregation(Expression<Func<Cadastrador, object>>[] includes, Filter filter)
        {
            return includes;
        }


		public override void Dispose()
        {
			if (this.rep != null)
				this.rep.Dispose();
        }

		~Cadastrador() {
            
        }

	}
}