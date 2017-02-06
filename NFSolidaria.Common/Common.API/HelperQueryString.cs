using Common.API;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


public static class HelperQueryString
{


    public static QueryStringParameter ToQueryString(this object model)
    {
        var collection = new QueryStringParameter();

        if (model != null)
        {
            var props = model.GetType().GetProperties();
            if (props.IsNotNull())
            {
                foreach (var item in props)
                {
                    var value = item.GetValue(model);

                    if (value.IsNotNull())
                    {
                        if (value.GetType().IsArray)
                        {
                            var array = (Array)value;
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (array.GetValue(i).IsNotNull())
                                    AddValueToCollection(collection, item, array.GetValue(i).ToString());
                            }
                        }
                        else
                        {
                            AddValueToCollection(collection, item, value);
                        }

                    }
                }
            }
        }

        return collection;

    }

    private static void AddValueToCollection(QueryStringParameter collection, System.Reflection.PropertyInfo item, object value)
    {
        if (String.IsNullOrEmpty(value.ToString()) || value == null) return;

        if (item.PropertyType == typeof(DateTime) || item.PropertyType == typeof(DateTime?))
        {
            var NewValue = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
            if (NewValue != default(DateTime).ToString("yyyy-MM-dd HH:mm:ss"))
                collection.Add(item.Name, clearValue(NewValue));
            return;
        }
        if (item.PropertyType == typeof(decimal) || item.PropertyType == typeof(decimal?))
        {
            var NewValue = Convert.ToDecimal(value).ToString("0.00", CultureInfo.InvariantCulture);
            if (NewValue != default(decimal).ToString("0.00", CultureInfo.InvariantCulture))
                collection.Add(item.Name, NewValue);
            return;
        }
        else
            collection.Add(item.Name, value.ToString());
    }

    public static string clearValue(string valor)
    {
        valor = valor.Replace(" 00:00:00", "");
        return valor;
    }


    public static string ToQueryString(this NameValueCollection collection)
    {
        var url = String.Format("?{0}", String.Join("&", collection.AllKeys
            .Where(key => collection.GetValues(key) != null)
                    .SelectMany(key => collection.GetValues(key)
                        .Select(value => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value))))
                        .ToArray()));

        return url;
    }

}

