using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using Common.Infrastructure.ORM.Context;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM.Context
{
    [Obsolete("Utilize os metodos ExecuteCommand/ExecuteDynamicQuery do repositorio")]
    public class QueryExecute<T>
    {

        private DbContext ctx;
        private ILog log;
        private List<DbParameter> parameters;
        public delegate void ConfigValues(DbDataReader reader,List<T> result);

        public QueryExecute(IUnitOfWork unitOfWork, ILog log)
        {
            this.ctx = unitOfWork as DbContext;
            this.log = log;
            this.parameters = new List<DbParameter>();

        }

        public void AddParametersWithValue(string parameterName, object value)
        {
            parameters.Add(new SqlParameter(parameterName, value));
        }


        public IEnumerable<T> Execute(string commandText, ConfigValues configValues)
        {
           var result = new List<T>();

            using (this.ctx)
            {

                this.ctx.Database.Connection.Open();
                var command = this.ctx.Database.Connection.CreateCommand();
                command.CommandText = commandText;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (var item in parameters)
                    command.Parameters.Add(item);
                
                var reader = command.ExecuteReader();

                while (reader.Read())
                    configValues(reader, result);
  
            }

            return result;

        }


    }
}
