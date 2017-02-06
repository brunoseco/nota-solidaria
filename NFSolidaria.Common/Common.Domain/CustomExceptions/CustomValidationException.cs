using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.CustomExceptions
{
    public class CustomValidationException : Exception
    {
        public IList<ValidationResult> Errors { get; private set; }

        public CustomValidationException(IList<ValidationResult> erros)
        {
            this.Errors = erros;
        }

        public CustomValidationException(IEnumerable<string> erros)
            :base(erros.FirstOrDefault())
        {
            
            this.Errors = new List<ValidationResult>();
            foreach (var item in erros)
                this.Errors.Add(new ValidationResult(item));
        }
        public CustomValidationException(string message)
            : base(message)
        {
            this.Errors = new List<ValidationResult> { new ValidationResult(message) };
        }
    }



}
