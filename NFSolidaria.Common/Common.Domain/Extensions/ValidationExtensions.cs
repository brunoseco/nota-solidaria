using Common.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Extensions
{

    public static class ValidationExtensions
    {


        public static IAudit AuditRecursive(this IAudit model, object userId, IAudit auditClass)
        {
            model.Audit(userId, auditClass);

            var propsWithAudit = model.GetType().GetProperties()
                .Where(_ => typeof(IAudit).IsAssignableFrom(_.PropertyType));

            foreach (var item in propsWithAudit)
            {
                if (item.GetValue(model).IsNotNull())
                {
                    var propModel = item.GetValue(model) as IAudit;

                    propModel.Audit(userId, auditClass);
                }
            }


            return model;

        }


    }
}

