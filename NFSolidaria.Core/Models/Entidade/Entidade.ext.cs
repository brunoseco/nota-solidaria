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

namespace NFSolidaria.Core.Domain
{
    [MetadataType(typeof(EntidadeValidation))]
    public partial class Entidade
    {
        public Entidade()
        {
        }


        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<EntidadeCadastrador> CollectionEntidadeCadastrador { get; set; }
        public virtual ICollection<UsuarioEntidadeFavorita> CollectionUsuarioEntidadeFavorita { get; set; }
        public virtual ICollection<Cupom> CollectionCupom { get; set; }

        public Entidade Get(Entidade model)
        {
            return this.rep.GetAll(this.DataAgregation(new EntidadeFilter
            {
                QueryOptimizerBehavior = model.QueryOptimizerBehavior

            })).Where(_ => _.EntidadeId == model.EntidadeId).SingleOrDefault();
        }

        public IQueryable<Entidade> GetByFilters(EntidadeFilter filters, params Expression<Func<Entidade, object>>[] includes)
        {
            var queryBase = this.rep.GetAll(this.DataAgregation(includes, filters));
            var queryFilter = this.SimpleFilters(filters, queryBase);

            if (filters.NaoFavoritadaPeloUsuarioId.IsSent())
                queryFilter = queryFilter
                    .Where(_ => !_.CollectionUsuarioEntidadeFavorita
                        .Where(__ => __.UsuarioId == filters.NaoFavoritadaPeloUsuarioId).Any());

            if (filters.ComUsuarioCadastrador)
                queryFilter = queryFilter.Where(_ => _.CollectionEntidadeCadastrador.Where(__ => __.Cadastrador.Ativo).Any());

            if (filters.ComCupomPendente)
                queryFilter = queryFilter.Where(_ => _.CollectionCupom.Where(__ => __.Situacao == (int)ESituacaoCupom.Pendente).Any());

            if (filters.Nome.IsSent())
                queryFilter = queryFilter.Where(_ => _.Usuario.Nome.Contains(filters.Nome));

            return queryFilter;
        }

        public IEnumerable<DataItem> GetDataItem(EntidadeFilter filters)
        {
            var dataList = this.GetByFilters(filters)
                .Select(_ => new DataItem
                {
                    Id = _.EntidadeId.ToString(),
                    Name = _.Usuario.Nome
                });

            return dataList.ToList();
        }

        public Common.Models.Summary GetSummary(IQueryable<Entidade> result)
        {
            return new Common.Models.Summary
            {
                Total = this.TotalCount
            };
        }

        public Entidade Save()
        {
            return Save(this);
        }

        public Entidade Save(Entidade model)
        {
            model = SaveDefault(model);
            this.rep.Commit();
            return model;
        }

        public IEnumerable<Entidade> Save(IEnumerable<Entidade> models)
        {
            var modelsInserted = new List<Entidade>();
            foreach (var item in models)
            {
                modelsInserted.Add(SaveDefault(item));
            }

            this.rep.Commit();
            return modelsInserted;
        }

        public void Delete(Entidade model)
        {
            if (model.IsNull())
                throw new CustomBadRequestException("Delete sem parametros");

            var alvo = this.Get(model);
            this.DeleteFromRepository(alvo);
            this.rep.Commit();
        }

        private void SetInitialValues(Entidade model)
        {


        }

        private void ValidationReletedClasses(Entidade model, Entidade modelOld)
        {

        }

        private void DeleteCollectionsOnSave(Entidade model)
        {

        }

        protected virtual Expression<Func<Entidade, object>>[] DataAgregation(Filter filter)
        {
            return DataAgregation(new Expression<Func<Entidade, object>>[] { }, filter);
        }

        protected virtual Expression<Func<Entidade, object>>[] DataAgregation(Expression<Func<Entidade, object>>[] includes, Filter filter)
        {
            return includes;
        }


        public override void Dispose()
        {
            if (this.rep != null)
                this.rep.Dispose();
        }

        ~Entidade()
        {

        }

    }
}