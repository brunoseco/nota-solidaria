using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IDataAgregation<T>
    {

        Expression<Func<T, object>>[] DataAgregation(Filter filter);

        Expression<Func<T, object>>[] DataAgregation(Expression<Func<T, object>>[] includes, Filter filter);

    }
}
