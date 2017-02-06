using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IDomainCrud<T> : IDomain<T>
    {

        T Save(T model);

        T Save();

        void Delete(T model);

     
    }
}
