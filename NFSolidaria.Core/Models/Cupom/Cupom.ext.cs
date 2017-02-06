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
using NFSolidaria.Enum;
using System.Text.RegularExpressions;

namespace NFSolidaria.Core.Domain
{
    [MetadataType(typeof(CupomValidation))]
    public partial class Cupom
    {
        public Cupom()
        {
        }

        public virtual Entidade Entidade { get; set; }

        public Cupom Get(Cupom model)
        {
            return this.rep.GetAll(this.DataAgregation(new CupomFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_ => _.CupomId == model.CupomId).SingleOrDefault();
        }

        public IQueryable<Cupom> GetByFilters(CupomFilter filters, params Expression<Func<Cupom, object>>[] includes)
        {
            var queryBase = this.rep.GetAll(this.DataAgregation(includes, filters));
            var queryFilter = this.SimpleFilters(filters, queryBase);

            if (filters.COONomeEntidade.IsSent())
                queryFilter = queryFilter
                    .Where(_ => _.COO.Contains(filters.COONomeEntidade) || 
                        _.Entidade.Usuario.Nome.Contains(filters.COONomeEntidade));

            return queryFilter;
        }

        public IEnumerable<DataItem> GetDataItem(CupomFilter filters)
        {
            var dataList = this.GetByFilters(filters)
                .Select(_ => new DataItem
                {
                    Id = _.CupomId.ToString(),
                });

            return dataList.ToList();
        }

        public Common.Models.Summary GetSummary(IQueryable<Cupom> result)
        {
            return new Common.Models.Summary
            {
                Total = this.TotalCount
            };
        }

        public Cupom Save()
        {
            return Save(this);
        }

        public Cupom Save(Cupom model)
        {
            model = SaveDefault(model);
            this.rep.Commit();
            return model;
        }

        public IEnumerable<Cupom> Save(IEnumerable<Cupom> models)
        {
            var modelsInserted = new List<Cupom>();
            foreach (var item in models)
            {
                modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

        public void Delete(Cupom model)
        {
            if (model.IsNull())
                throw new CustomBadRequestException("Delete sem parametros");

            var alvo = this.Get(model);
            this.DeleteFromRepository(alvo);
            this.rep.Commit();
        }

        private void SetInitialValues(Cupom model)
        {
            var modelOld = this.Get(model);
            if (modelOld.IsNull())
            {
                if (!model.UsuarioId.IsSent())
                {
                    var user = this.ValidateAuth(this.token, this.cache);
                    model.UsuarioId = user.UserId;
                }

                if (!model.Situacao.IsSent())
                    model.Situacao = (int)ESituacaoCupom.Pendente;

                if (!model.DataLancamento.IsSent())
                    model.DataLancamento = DateTime.Now;
            }
        }

        private void ValidationReletedClasses(Cupom model, Cupom modelOld)
        {


        }

        private void DeleteCollectionsOnSave(Cupom model)
        {

        }

        protected virtual Expression<Func<Cupom, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<Cupom, object>>[] { }, filter);
        }

        protected virtual Expression<Func<Cupom, object>>[] DataAgregation(Expression<Func<Cupom, object>>[] includes, Filter filter)
        {
            if (filter.QueryOptimizerBehavior == "ListaCuponsAplicativo")
            {
                this.rep.LazyLoadingEnabled(false);
                return includes.Add(_ => _.Entidade.Usuario);
            }

            return includes;
        }


        public override void Dispose()
        {
            if (this.rep != null)
                this.rep.Dispose();
        }

        ~Cupom()
        {

        }

    }
}