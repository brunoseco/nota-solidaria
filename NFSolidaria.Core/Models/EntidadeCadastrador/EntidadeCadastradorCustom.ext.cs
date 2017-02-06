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
	public class EntidadeCadastradorCustom : EntidadeCadastrador
	{


        protected override Expression<Func<EntidadeCadastrador, object>>[] DataAgregation(Expression<Func<EntidadeCadastrador, object>>[] includes, Filter filter)
        {
            return includes;
        }

	}
}