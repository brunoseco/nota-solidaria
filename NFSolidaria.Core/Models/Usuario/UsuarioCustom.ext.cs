using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using System.Linq.Expressions;
using Common.Domain.Interfaces;

namespace NFSolidaria.Core.Domain
{
    [NotMapped]
    public class UsuarioCustom : Usuario
    {
        public UsuarioCustom(IRepository<Usuario> rep, ICache cache, ICripto cripto)
            : base(rep, cache, cripto)
        { }

        protected override Expression<Func<Usuario, object>>[] DataAgregation(Expression<Func<Usuario, object>>[] includes, Filter filter)
        {
            return includes;
        }

    }
}