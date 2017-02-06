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
	public class CupomCustom : Cupom
	{


        protected override Expression<Func<Cupom, object>>[] DataAgregation(Expression<Func<Cupom, object>>[] includes, Filter filter)
        {
            return includes;
        }

	}
}