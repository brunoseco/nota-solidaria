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
    [MetadataType(typeof(UsuarioEntidadeFavoritaValidation))]
    public partial class UsuarioEntidadeFavorita
    {
        public UsuarioEntidadeFavorita()
        {
        }

        public virtual Entidade Entidade { get; set; }
        public virtual Usuario Usuario { get; set; }

        public UsuarioEntidadeFavorita Get(UsuarioEntidadeFavorita model)
        {
            return this.rep.GetAll(this.DataAgregation(new UsuarioEntidadeFavoritaFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_ => _.UsuarioId == model.UsuarioId).Where(_ => _.EntidadeId == model.EntidadeId).SingleOrDefault();
        }

        public IQueryable<UsuarioEntidadeFavorita> GetByFilters(UsuarioEntidadeFavoritaFilter filters, params Expression<Func<UsuarioEntidadeFavorita, object>>[] includes)
        {
            var queryBase = this.rep.GetAll(this.DataAgregation(includes, filters));
            var queryFilter = this.SimpleFilters(filters, queryBase);
            
            if (filters.Nome.IsSent())
                queryFilter = queryFilter.Where(_ => _.Entidade.Usuario.Nome.Contains(filters.Nome));

            return queryFilter;
        }

        public IEnumerable<DataItem> GetDataItem(UsuarioEntidadeFavoritaFilter filters)
        {
            var dataList = this.GetByFilters(filters)
                .Select(_ => new DataItem
                {
                    Id = _.EntidadeId.ToString(),
                    Name = _.Entidade.Usuario.Nome
                });

            return dataList.ToList();
        }

        public Common.Models.Summary GetSummary(IQueryable<UsuarioEntidadeFavorita> result)
        {
            return new Common.Models.Summary
            {
                Total = this.TotalCount
            };
        }

        public UsuarioEntidadeFavorita Save()
        {
            return Save(this);
        }

        public UsuarioEntidadeFavorita Save(UsuarioEntidadeFavorita model)
        {
            model = SaveDefault(model);
            this.rep.Commit();
            return model;
        }

        public IEnumerable<UsuarioEntidadeFavorita> Save(IEnumerable<UsuarioEntidadeFavorita> models)
        {
            var modelsInserted = new List<UsuarioEntidadeFavorita>();
            foreach (var item in models)
            {
                modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

        public void Delete(UsuarioEntidadeFavorita model)
        {
            if (model.IsNull())
                throw new CustomBadRequestException("Delete sem parametros");

            var alvo = this.Get(model);
            this.DeleteFromRepository(alvo);
            this.rep.Commit();
        }

        private void SetInitialValues(UsuarioEntidadeFavorita model)
        {


        }

        private void ValidationReletedClasses(UsuarioEntidadeFavorita model, UsuarioEntidadeFavorita modelOld)
        {

        }

        private void DeleteCollectionsOnSave(UsuarioEntidadeFavorita model)
        {

        }

        protected virtual Expression<Func<UsuarioEntidadeFavorita, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<UsuarioEntidadeFavorita, object>>[] { }, filter);
        }

        protected virtual Expression<Func<UsuarioEntidadeFavorita, object>>[] DataAgregation(Expression<Func<UsuarioEntidadeFavorita, object>>[] includes, Filter filter)
        {
            return includes;
        }


        public override void Dispose()
        {
            if (this.rep != null)
                this.rep.Dispose();
        }

        ~UsuarioEntidadeFavorita()
        {

        }

    }
}