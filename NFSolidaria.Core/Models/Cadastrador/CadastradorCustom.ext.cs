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
	public class CadastradorCustom : Cadastrador
	{


        protected override Expression<Func<Cadastrador, object>>[] DataAgregation(Expression<Func<Cadastrador, object>>[] includes, Filter filter)
        {
            return includes;
        }

	}
}