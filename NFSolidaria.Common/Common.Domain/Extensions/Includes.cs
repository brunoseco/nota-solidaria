using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


public static class Includes
{

    public static Expression<Func<T, object>>[] Add<T>(this Expression<Func<T, object>>[] source, params Expression<Func<T, object>>[] includes)
    {
        var newIncludes = source.ToList();
        newIncludes.AddRange(includes.ToList());
        return newIncludes.ToArray();
    }

}

