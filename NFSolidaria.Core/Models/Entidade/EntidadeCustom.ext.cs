using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using System.Linq.Expressions;
using Common.Domain.Interfaces;
using NFSolidaria.Core.Filters;
using NFSolidaria.Enum;

namespace NFSolidaria.Core.Domain
{
    [NotMapped]
    public class EntidadeCustom : Entidade
    {

        public EntidadeCustom(IRepository<Entidade> rep, ICache cache)
            : base(rep, cache)
        {
        }

        public override IEnumerable<dynamic> GetDataListCustom(EntidadeFilter filters)
        {
            var result = this.GetByFilters(filters);

            return result.Select(_ => new
            {
                CustomFieldOrder = _.EntidadeId,
                EntidadeId = _.EntidadeId,
                IdentificadorNFP = _.IdentificadorNFP,
                Cadastrador = _.CollectionEntidadeCadastrador.Where(__ => __.Cadastrador.Ativo).Select(__ => new
                {
                    CPF = __.Cadastrador.Usuario.CPF_CNPJ,
                    Pass = __.Cadastrador.Pass,
                }).FirstOrDefault(),
                CollectionCupom = _.CollectionCupom.Where(__ => __.Situacao == (int)ESituacaoCupom.Pendente).Select(__ => new
                {
                    CupomId = __.CupomId,
                    CNPJEmissor = __.CNPJEmissor,
                    TipoNota = __.TipoNota,
                    DataCompra = __.DataCompra,
                    COO = __.COO,
                    Valor = __.Valor
                })
            });
        }

        protected override Expression<Func<Entidade, object>>[] DataAgregation(Expression<Func<Entidade, object>>[] includes, Filter filter)
        {
            return includes;
        }

    }
}