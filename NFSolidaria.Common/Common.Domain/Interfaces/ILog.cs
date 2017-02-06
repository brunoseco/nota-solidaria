using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface ILog
    {
        void Debug(string format, params object[] arg);
        void Debug(string message);
        void Debug(string message, Exception exception);

        void Info(string format, params object[] arg);
        void Info(string message);
        void Info(string message, Exception exception);

        void Warn(string format, params object[] arg);
        void Warn(string message);
        void Warn(string message, Exception exception);

        void Error(string format, params object[] arg);
        void Error(string message);
        void Error(string message, Exception exception);

        void Fatal(string format, params object[] arg);
        void Fatal(string message);
        void Fatal(string message, Exception exception);

    }
}
