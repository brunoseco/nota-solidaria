using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ICacheProfile
    {

        void Add(string roleId, int externalId, string name, object value);
        void Update(string roleId, object value);
        void Remove(string roleId);
        IEnumerable<object> Get(IEnumerable<string> rolesId);
        IEnumerable<object> Get(IEnumerable<int> externalsId);
        void RegisterClassMap<T>();

    }
}
