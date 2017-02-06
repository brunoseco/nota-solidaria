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
	[MetadataType(typeof(EntidadeCadastradorValidation))]
	public partial class EntidadeCadastrador
	{
		public EntidadeCadastrador()
        {
        }

        public virtual Cadastrador Cadastrador { get; set; }

        public EntidadeCadastrador Get(EntidadeCadastrador model)
        {
            return this.rep.GetAll(this.DataAgregation(new EntidadeCadastradorFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_=>_.EntidadeId == model.EntidadeId).Where(_=>_.CadastradorId == model.CadastradorId).SingleOrDefault();
        }

		public IQueryable<EntidadeCadastrador> GetByFilters(EntidadeCadastradorFilter filters, params Expression<Func<EntidadeCadastrador, object>>[] includes)
        {
			var queryBase = this.rep.GetAllAsNoTracking(this.DataAgregation(includes,filters));
			var queryFilter = queryBase;
			return this.SimpleFilters(filters, queryBase);
        }

		public IEnumerable<DataItem> GetDataItem(EntidadeCadastradorFilter filters)
		{
			var dataList = this.GetByFilters(filters)
				.Select(_ => new DataItem
				{
					Id = _.EntidadeId.ToString(),
				});

			return dataList.ToList();
		}

		public Common.Models.Summary GetSummary(IQueryable<EntidadeCadastrador> result)
		{
			return new Common.Models.Summary
            {
                Total = this.TotalCount
            };
		}

		public EntidadeCadastrador Save()
        {
            return Save(this);
        }

		public EntidadeCadastrador Save(EntidadeCadastrador model)
        {
			model = SaveDefault(model);
            this.rep.Commit();
            return model;
        }

		public IEnumerable<EntidadeCadastrador> Save(IEnumerable<EntidadeCadastrador> models)
        {
            var modelsInserted = new List<EntidadeCadastrador>();
            foreach (var item in models)
            {
                 modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

		public void Delete(EntidadeCadastrador model)
        {
            if (model.IsNull())
                throw new CustomBadRequestException("Delete sem parametros");

            var alvo = this.Get(model);
            this.DeleteFromRepository(alvo);
            this.rep.Commit();
        }

		private void SetInitialValues(EntidadeCadastrador model)
        {
		    

        }
		
		private void ValidationReletedClasses(EntidadeCadastrador model, EntidadeCadastrador modelOld)
        {

		}

		private void DeleteCollectionsOnSave(EntidadeCadastrador model)
        { 
        
        }

		protected virtual Expression<Func<EntidadeCadastrador, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<EntidadeCadastrador, object>>[] { }, filter);
        }

        protected virtual Expression<Func<EntidadeCadastrador, object>>[] DataAgregation(Expression<Func<EntidadeCadastrador, object>>[] includes, Filter filter)
        {
            return includes;
        }


		public override void Dispose()
        {
			if (this.rep != null)
				this.rep.Dispose();
        }

		~EntidadeCadastrador() {
            
        }

	}
}