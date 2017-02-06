
using Common.Domain;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;
public static class BehaviorExtensions
{
    public static U AttributeBehavior<T, U>(this IEnumerable<T> instances)
    {
        if (instances.IsNotNull())
            return instances.FirstOrDefault().AttributeBehavior<T, U>();

        return default(U);
    }

    public static U AttributeBehavior<T, U>(this T instance)
    {
        try
        {
            var domainInstace = instance as DomainBase;

            if (domainInstace.IsNotNull())
            {

                U res = (U)Enum.Parse(typeof(U), domainInstace.AttributeBehavior);
                if (!Enum.IsDefined(typeof(U), res)) return default(U);
                return res;
            }

            return default(U);


        }
        catch
        {
            return default(U);
        }
    }


}

