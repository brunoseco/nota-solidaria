using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM.Repositories
{
    public class RepositoryBase<U> : IRepository
    {
        protected DbContext ctx;
        protected ILog log;
        protected string connectionString;

        public RepositoryBase(IUnitOfWork<U> unitOfWork, ILog log)
        {
            this.ctx = unitOfWork as DbContext;
            this.log = log;
        }

        public IRepository<T2> NewInstance<T2>() where T2 : class
        {
            var unitOfWork = this.ctx as IUnitOfWork<U>;
            return new Repository<T2, U>(unitOfWork, this.log);

        }
        public IEnumerable<dynamic> ExecuteDynamicQuery(string commandText, object parameters = null, CommandType commandType = CommandType.StoredProcedure, bool MARS = false)
        {
            if (MARS)
            {
                return ctx.Database.ExecuteReaderMARS(commandText, this.connectionString, parameters, commandType);
            }

            return ctx.Database.ExecuteReader(commandText, this.connectionString, parameters, commandType);
        }

        public int ExecuteCommand(string commandText, object parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return ctx.Database.ExecuteNonQuery(commandText, connectionString, parameters, commandType);
        }


        public void Dispose()
        {
             if (this.ctx != null)
                this.ctx.Dispose();
        }


    }
}
