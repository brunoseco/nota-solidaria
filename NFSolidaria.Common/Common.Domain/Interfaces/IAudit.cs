using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Interfaces
{
    public interface IAudit
    {
        void Audit(object userId);
        void Audit(object userId, IAudit auditClass);

        int GetUserCreateId();
        DateTime GetUserCreateDate();

    }
}
