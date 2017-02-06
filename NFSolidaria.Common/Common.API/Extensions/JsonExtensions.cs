using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class JsonExtensions
    {

        public static T CastJsonToType<T>(this object instance)
        {
            var destination = Activator.CreateInstance<T>();
            if (instance != null)
                return JsonConvert.DeserializeObject<T>(instance.ToString());

            return destination;
        }

        public static IEnumerable<T> CastJsonToType<T>(this IEnumerable<object> instances)
        {
            var result = new List<T>();
            foreach (var item in instances)
            {
                result.Add(item.CastJsonToType<T>());
            }
            return result;
        }

    }

