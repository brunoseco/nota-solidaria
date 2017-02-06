using Common.Domain.Attribute;
using Common.Domain.CustomExceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain
{
    public class ValidationConfirm
    {
        public string message { get; set; }
        public string verifyBehavior { get; set; }

        public ValidationConfirm(string message, string verifyBehavior)
        {
            this.message = message;
            this.verifyBehavior = verifyBehavior;
        }
        
    }
}
