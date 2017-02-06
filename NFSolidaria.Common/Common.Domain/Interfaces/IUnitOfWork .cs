using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IUnitOfWork<T>
    {
        string ConnectionStringComplete();
        void Commit();
    }

    public interface IUnitOfWork
    {
        string ConnectionStringComplete();
        void Commit();
    }
}
