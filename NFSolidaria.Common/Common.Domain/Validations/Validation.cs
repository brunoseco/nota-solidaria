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
    public class EntityValidationResult
    {

        public EntityValidationResult(IEnumerable<ValidationResult> errors = null, IEnumerable<ValidationResult> warnings = null, IEnumerable<ValidationConfirm> confirms = null)
        {
            this.errors = new List<ValidationResult>();
            if (errors.IsNotNull() && errors.Count() > 0) { this.errors.AddRange(errors); }

            this.warnings = new List<ValidationResult>();
            if (warnings.IsNotNull() && warnings.Count() > 0) { this.warnings.AddRange(warnings); }

            this.confirms = new List<ValidationConfirm>();
            if (confirms.IsNotNull() && confirms.Count() > 0) { this.confirms.AddRange(confirms); }
        }
        private List<ValidationResult> errors;

        public IEnumerable<ValidationResult> Errors
        {
            get { return errors; }
        }

        private List<ValidationResult> warnings;
        public IEnumerable<ValidationResult> Warnings
        {
            get { return warnings; }
        }

        private List<ValidationConfirm> confirms;
        public IEnumerable<ValidationConfirm> Confirms
        {
            get { return confirms; }
        }
        public bool HasError
        {
            get { return Errors.Count() > 0; }
        }

        public void AddErrorRange(IEnumerable<ValidationResult> errors = null)
        {
            this.errors.AddRange(errors);
        }

        public void AddWarningRange(IEnumerable<ValidationResult> warnings = null)
        {
            this.warnings.AddRange(warnings);
        }

        public void AddConfirmRange(IEnumerable<ValidationConfirm> config = null)
        {
            this.confirms.AddRange(config);
        }

    }


    public class EntityValidator<T> where T : class
    {
        public EntityValidationResult Validate(T entity)
        {

            var Errors = new List<ValidationResult>();
            var mdtObj = typeof(T).GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();

            if (mdtObj.IsNull())
                return new EntityValidationResult(Errors);

            var mdt = (MetadataTypeAttribute)mdtObj;
            var prop = mdt.MetadataClassType.GetProperties();

            foreach (var item in prop)
            {
                var atts = item.GetCustomAttributes(typeof(ValidationAttribute), true);

                foreach (ValidationAttribute att in atts)
                {
                    var propinfo = entity.GetType().GetProperty(item.Name);
                    var propValue = propinfo.GetValue(entity);

                    if (!att.IsValid(propValue))
                        Errors.Add(new ValidationResult(att.ErrorMessage, new string[] { propinfo.Name }));

                    if (propinfo.PropertyType == typeof(DateTime) && default(DateTime) == (DateTime)propValue)
                        Errors.Add(new ValidationResult(att.ErrorMessage, new string[] { propinfo.Name }));

                    if (att.GetType() == typeof(RequiredAttribute))
                    {
                        if (propinfo.PropertyType == typeof(int) && default(int) == (int)propValue)
                            Errors.Add(new ValidationResult(att.ErrorMessage, new string[] { propinfo.Name }));
                    }

                }

            }

            return new EntityValidationResult(Errors);
        }

        public EntityValidationResult Validate(string errorMessage, string key)
        {
            var Errors = new List<ValidationResult>();
            Errors.Add(new ValidationResult(errorMessage, new string[] { key }));
            return new EntityValidationResult(Errors);
        }
        public EntityValidationResult Warning(string warning, string key)
        {
            var warnings = new List<ValidationResult>();
            warnings.Add(new ValidationResult(warning, new string[] { key }));
            return new EntityValidationResult(warnings: warnings);
        }
        public EntityValidationResult Confirm(string confirm, string verifyBehavior)
        {
            var confirms = new List<ValidationConfirm>();
            confirms.Add(new ValidationConfirm(confirm, verifyBehavior));
            return new EntityValidationResult(confirms: confirms);
        }
    }

    public class ValidationHelper
    {
        private EntityValidationResult result = new EntityValidationResult();

        public void ValidateAll()
        {
            if (result.HasError)
            {
                var errors = result.Errors.Distinct().ToList();
                throw new CustomValidationException(errors);
            }
        }

        public void Validate<T>(T entity) where T : class
        {
            if (entity.IsNull())
                return;

            var currentValidation = new EntityValidator<T>().Validate(entity);
            result.AddErrorRange(currentValidation.Errors);
        }


        public void AddDomainValidation<T>(string errorMessage) where T : class
        {
            var currentValidation = new EntityValidator<T>().Validate(errorMessage, Guid.NewGuid().ToString());
            result.AddErrorRange(currentValidation.Errors);
        }

        public void AddDomainWarning<T>(string warningMessage) where T : class
        {
            var currentValidation = new EntityValidator<T>().Warning(warningMessage, Guid.NewGuid().ToString());
            result.AddWarningRange(currentValidation.Warnings);
        }

        public void AddDomainConfirm<T>(string confirmMessage, string verifyBehavior = null) where T : class
        {
            if (verifyBehavior.IsNull())
                verifyBehavior = "Confirmed";

            var currentValidation = new EntityValidator<T>().Confirm(confirmMessage, verifyBehavior);
            result.AddConfirmRange(currentValidation.Confirms);
        }

        public IEnumerable<string> GetDomainWarning()
        {
            return result.Warnings.IsNotNull() ? result.Warnings.Select(_ => _.ErrorMessage) : null;
        }

        public IEnumerable<ValidationConfirm> GetDomainConfirms()
        {
            return result.Confirms.IsAny() ? result.Confirms : null;
        }

        public IEnumerable<string> GetDomainErrors()
        {
            return result.Errors.IsNotNull() ? result.Errors.Select(_ => _.ErrorMessage) : null;
        }

        public bool HasErrors { get { return result.HasError; } }

        public void Validate<T>(IEnumerable<T> entitys) where T : class
        {
            if (entitys.IsNull())
                return;

            foreach (var entity in entitys)
                Validate(entity);
        }
    }
}
