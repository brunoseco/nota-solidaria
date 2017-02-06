using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IServiceLocator
    {
        void ServiceTypeAdd(Type key, Type value);

        T GetServiceLazy<T>(params object[] args);

        T GetService<T>();

    }
}
