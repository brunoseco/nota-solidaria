using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ObjectExtensions
{

    public static string CompositeKey(this object source)
    {

        var propertys = source.GetType().GetProperties();
        var keys = new List<object>();
        foreach (var item in propertys)
        {

            var propertyValue = item.GetValue(source);
            if (propertyValue != null)
                keys.Add(propertyValue);

        }
        keys.Add(source.GetType().Name);
        return CompositeKey(keys.ToArray());


    }
    private static string CompositeKey(object[] keys)
    {
        var key = string.Empty;
        keys.ToList().ForEach(_ =>
        {
            if (_ != null)
                key += _.ToString()
                    .Replace(" ", string.Empty)
                    .Replace(default(DateTime).ToString(), string.Empty)
                    .Replace("01/01/000100:00:00",string.Empty);
        });

        if (key.Length > 250)
            return key.Substring(0, 249);

        return key;
    }



}

