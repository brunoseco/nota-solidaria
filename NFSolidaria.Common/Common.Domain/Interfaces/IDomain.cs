using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IDomain<T>
    {
        T Get(T model);

        T GetFromContext(T model);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        int Total();

    }
}
