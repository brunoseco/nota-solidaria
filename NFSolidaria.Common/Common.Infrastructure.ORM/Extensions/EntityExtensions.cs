using Common.Infrastructure.ORM.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM
{
    static class EntityExtensions
    {
        public static int ExecuteCommand(this Database self, string commandText, object parameters = null)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText");

            var arguments = Parameters.PrepareArguments(commandText, parameters);
            return self.ExecuteSqlCommand(arguments.Item1, arguments.Item2);
        }

        public static async Task<int> ExecuteCommandAsync(this Database self, string commandText, object parameters = null)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText");

            var arguments = Parameters.PrepareArguments(commandText, parameters);
            return await self.ExecuteSqlCommandAsync(arguments.Item1, arguments.Item2);
        }

        public static IEnumerable<T> ExecuteQuery<T>(this Database self, string commandText, object parameters = null)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (string.IsNullOrEmpty(commandText))
                throw new ArgumentException("commandText");

            var arguments = Parameters.PrepareArguments(commandText, parameters);
            return self.SqlQuery<T>(arguments.Item1, arguments.Item2);
        }


    }
}
