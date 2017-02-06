using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM
{
    static class BulkInsertEstensions
    {
        public static void BulkCopy<T>(this DbContext ctx, string connectionString, IEnumerable<T> models)
        {
            var conn = new SqlConnection(connectionString);
            var configMappers = MapperEstensions.Config(models.GetType(), ctx);
            var tableDestination = configMappers.tableName;
            var data = DataTableExtensions.CreateDataTable<T>(models, ctx);

            conn.Open();

            using (var sqlBulkCopy = new SqlBulkCopy(conn))
            {
                sqlBulkCopy.DestinationTableName = tableDestination;
                foreach (DataColumn item in data.Columns)
                    sqlBulkCopy.ColumnMappings.Add(item.ColumnName, item.ColumnName);

                sqlBulkCopy.BulkCopyTimeout = 0;
                sqlBulkCopy.WriteToServer(data);
            }

            conn.Close();

        }
    }
}
