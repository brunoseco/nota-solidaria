using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ILdap<T> where T : class
    {
        bool Auth(string login, string password);

        T GetByDocument(string document);

        T GetByLogin(string login);        
    }
}
