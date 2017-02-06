using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using System.Linq.Expressions;

namespace NFSolidaria.Core.Domain
{	
	[NotMapped]
	public class UsuarioEntidadeFavoritaCustom : UsuarioEntidadeFavorita
	{


        protected override Expression<Func<UsuarioEntidadeFavorita, object>>[] DataAgregation(Expression<Func<UsuarioEntidadeFavorita, object>>[] includes, Filter filter)
        {
            return includes;
        }

	}
}