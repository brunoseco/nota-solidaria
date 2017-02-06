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
    public class Repository<T, U> : RepositoryBase<U>, IRepository<T> where T : class
    {
        
        public Repository(IUnitOfWork<U> unitOfWork, ILog log)
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
            if (model.IsNotNull())
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
        
        public void BulkCopy(IEnumerable<T> models)
        {
            ctx.BulkCopy<T>(this.connectionString, models);
        }
        public void Commit()
        {
            this.ctx.SaveChanges();
        }
    }

}
