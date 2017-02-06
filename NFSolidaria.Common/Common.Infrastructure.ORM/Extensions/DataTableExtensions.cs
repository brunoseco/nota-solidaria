using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM
{
    class DataTableExtensions
    {
        public static DataTable CreateDataTable<T>(IEnumerable<T> models)
        {
            return CreateDataTable<T>(models, null);
        }
        public static DataTable CreateDataTable<T>(IEnumerable<T> models, DbContext ctx)
        {
            var configMappers = ctx.IsNotNull() ? MapperEstensions.Config(models.GetType(), ctx) : null;
            var type = typeof(T);
            var properties = type.GetProperties();

            var dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                var name = configMappers.IsNotNull() ? configMappers.CollumsMappers.Where(_ => _.Key == info.Name).SingleOrDefault().Value : info.Name;
                if (name.IsNotNull())
                    dataTable.Columns.Add(new DataColumn(name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in models)
            {
                var values = new object[dataTable.Columns.Count];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

    }
}
