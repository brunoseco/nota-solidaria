using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProjetoRankingNFE.Models.Repository
{
    public class RepositoryBase<T> : IDisposable where T : class
    {
        public DBContexto Db = new DBContexto();

        public void Dispose()
        {
            
        }

        public void Add(T entity)
        {
            Db.Set<T>().Add(entity);
            Db.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return Db.Set<T>().ToList();
        }

        public T GetById(int? id)
        {
            return Db.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            Db.Entry(entity).State = EntityState.Modified;
            Db.SaveChanges();
        }

        public void Remove(T entity)
        {
            Db.Set<T>().Remove(entity);
            Db.SaveChanges();
        }
    }
}
