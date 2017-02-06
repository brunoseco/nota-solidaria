using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAppCrud<T>
    {

        IEnumerable<T> GetAll();

        SearchResult<T> GetByFilters(T filter);

        T Get(T dto);

        IEnumerable<DataItem> GetDataItem(T filter);


        T Save(T dto);

        void Delete(T dto);

    }
}
    