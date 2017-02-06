using Common.Domain;
using Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class RefplectionExtensions
{

    public static Type GetTypeInCollection(this Type source)
    {
        var itemType = default(Type);
        if (source.IsGenericType && source.GetGenericTypeDefinition() == typeof(List<>))
            itemType = source.GetGenericArguments()[0];

        return itemType;

    }

    public static bool IsCollection(this PropertyInfo item)
    {
        var isString = item.PropertyType == typeof(string);
        if (isString)
            return false;

        return item.PropertyType.GetInterfaces().Any(x => x == typeof(IEnumerable));
    }
    public static bool IsNotNullOrDefault(this PropertyInfo item, object obj, bool allowZero = false, bool allowNull = false)
    {
        return !IsNullOrDefault(item, obj, allowZero, allowNull);
    }
    public static bool IsNullOrDefault(this PropertyInfo item, object obj, bool allowZero = false, bool allowNull = false)
    {
        var value = item.GetValue(obj);

        if (allowNull == true)
            return false;

        if (value.IsNumber() && value != "-")
            if (Convert.ToDecimal(value) != 0 || allowZero)
                return false;

        if (item.PropertyType == typeof(string))
            if (value.IsNotNull())
                return false;

        if (item.PropertyType == typeof(bool))
            return false;

        if (item.PropertyType == typeof(bool?))
            if (value.IsNotNull())
                return false;

        if (item.PropertyType == typeof(DateTime))
            if (Convert.ToDateTime(value) != default(DateTime))
                return false;

        if (item.PropertyType == typeof(DateTime?))
            if (value.IsNotNull())
                return false;

        if (item.PropertyType == typeof(DomainBase))
            if (value.IsNotNull())
                return false;

        return true;

    }


}

