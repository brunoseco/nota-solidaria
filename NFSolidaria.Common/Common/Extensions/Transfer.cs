using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;


internal static class Transfer
{

    public static void CopyPropertys(this object source, object destination)
    {
        if (source != null)
        {

            var propertys = source.GetType().GetProperties();
            foreach (var item in propertys)
            {
                var propDestination = destination.GetType().GetProperty(item.Name);
                var propertyValue = item.GetValue(source);
                if (propDestination != null)
                    propDestination.SetValue(destination, propertyValue);
            }
        }
    }

    
}