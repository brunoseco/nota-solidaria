using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain.Enums;

namespace Common.Domain.Interfaces
{
    public interface IElapsedLog
    {
        void LogRequestEnd(string layer, string className);
        void LogRequestIni(string layer, string className);

    }
}
