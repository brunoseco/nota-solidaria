using Common.Domain;
using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class AuditExtensions
{

    public static void Audits<T>(this IEnumerable<T> collection, int userId) where T : IAudit
    {
        if (collection.IsNotNull())
        {
            foreach (var item in collection)
            {
                if (item.IsNotNull())
                    item.Audit(userId);
            }
        }
    }

    public static void Audits<T>(this IEnumerable<T> collection, int userId, IAudit auditClass) where T : IAudit
    {
        if (collection.IsNotNull())
        {
            foreach (var item in collection)
            {
                if (item.IsNotNull())
                    item.Audit(userId, auditClass);
            }
        }
    }
}








