using Common.Domain.Interfaces;
using Common.Infrastructure.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Infrastructure.ORM.Repositories
{
    class RepositoryTeste<T, U> : RepositoryBase<U>, IRepository<T> where T : class
    {
        private string connectionString;
        public RepositoryTeste(IUnitOfWork<U> unitOfWork, ILog log)
            : base(unitOfWork, log)
        {
            this.connectionString = unitOfWork.ConnectionStringComplete();
        }
        public void LazyLoadingEnabled(bool Enabled)
        {
            this.ctx.Configuration.ProxyCreationEnabled = Enabled;
            this.ctx.Configuration.LazyLoadingEnabled = Enabled;
            this.ctx.Configuration.AutoDetectChangesEnabled = Enabled;
        }
        public void Undo()
        {
            this.ctx.ChangeTracker.DetectChanges();

            var entries = this.ctx.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();

            foreach (var dbEntityEntry in entries)
            {
                var entity = dbEntityEntry.Entity;

                if (entity == null) continue;

                if (dbEntityEntry.State == EntityState.Added)
                {
                    var set = this.ctx.Set(entity.GetType());
                    set.Remove(entity);
                }
                else if (dbEntityEntry.State == EntityState.Modified)
                {
                    dbEntityEntry.Reload();
                }
                else if (dbEntityEntry.State == EntityState.Deleted)
                    dbEntityEntry.State = EntityState.Modified;
            }
        }
        public T Get(params object[] keyValues)
        {
            return this.ctx.Set<T>().Find(keyValues);
        }
        public IQueryable<T> GetAll()
        {
            return this.ctx.Set<T>();
        }
        public IQueryable<T> GetAll(string path)
        {
            return this.ctx.Set<T>().Include(path);
        }
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = ctx.Set<T>();
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }
        public IQueryable<T> GetAllAsNoTracking(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = ctx.Set<T>();
            var queryIncludend = includes.Aggregate(query, (current, include) => current.Include(include));
            return queryIncludend.AsNoTracking<T>();
        }
        public IQueryable<T> GetAllAsNoTracking(string path)
        {
            return this.ctx.Set<T>().Include(path).AsNoTracking<T>();
        }
        public IQueryable<T> GetAllAsNoTracking()
        {
            return this.ctx.Set<T>().AsNoTracking<T>();
        }
        public bool Delete(T model)
        {
            this.ctx.Set<T>().Remove(model);
            return true;
        }
        public bool DeleteRange(IEnumerable<T> models)
        {
            if (models.Count() == 0)
                return false;

            this.ctx.Set<T>().RemoveRange(models);
            return true;
        }
        public T Add(T model)
        {
            this.ctx.Set<T>().Add(model);
            return model;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> models)
        {
            this.ctx.Set<T>().AddRange(models);
            return models;
        }
        public T Update(T modelNew, T modelOld)
        {
            ctx.Entry(modelOld).CurrentValues.SetValues(modelNew);
            return modelNew;
        }
        public IEnumerable<T> ExecuteQuery(string commandText, object parameters = null)
        {
            return ctx.Database.ExecuteQuery<T>(commandText, parameters);
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
     
        public async Task<int> ExecuteCommandAsync(string storedProcedure, object parameters = null)
        {
            return await ctx.Database.ExecuteCommandAsync(storedProcedure, parameters);
        }

        public void BulkCopy(IEnumerable<T> models)
        {
            ctx.BulkCopy<T>(this.connectionString, models);
        }
        public void Commit()
        {
            try
            {
                this.ctx.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var excpetionMsg = string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        log.Error(excpetionMsg);
                        throw new InvalidOperationException(excpetionMsg);

                    }
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }
        public async void CommitAsync()
        {
            try
            {
                await this.ctx.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        var excpetionMsg = string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        log.Error(excpetionMsg);
                        throw new InvalidOperationException(excpetionMsg);

                    }
                }
            }

            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex;
            }
        }


        public void FilterColletionNested<TElement>(T Entity,
            Expression<Func<T, ICollection<TElement>>> navigationProperty,
            Expression<Func<TElement, bool>> predicate) where TElement : class
        {
            ctx.Entry(Entity)
               .Collection(navigationProperty)
               .Query()
               .Where(predicate)
               .Load();
        }

    }

}
