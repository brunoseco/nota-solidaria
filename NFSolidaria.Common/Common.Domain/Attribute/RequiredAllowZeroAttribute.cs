using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.Attribute
{
    public class RequiredAllowZeroAttribute : ValidationAttribute
    {

        public override bool IsValid(object value)
        {
            if (value.IsNumber())
                return true;

            return false;
        }

    }
}
