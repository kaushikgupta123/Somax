using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class MinLengthPasswordAttribute : ValidationAttribute, IClientValidatable
    {
        public string PasswordReqMinLengthProperty, PasswordMinLengthProperty;

        public MinLengthPasswordAttribute(string PasswordReqMinLengthProperty, string PasswordMinLengthProperty)
        {
            this.PasswordReqMinLengthProperty = PasswordReqMinLengthProperty;
            this.PasswordMinLengthProperty = PasswordMinLengthProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(PasswordReqMinLengthProperty) && !string.IsNullOrEmpty(PasswordMinLengthProperty))
            {
                var PasswordReqMinLengthDet = validationContext.ObjectInstance.GetType().GetProperty(PasswordReqMinLengthProperty);
                var PasswordReqMinLength = PasswordReqMinLengthDet.GetValue(validationContext.ObjectInstance, null);

                var PasswordMinLengthDet = validationContext.ObjectInstance.GetType().GetProperty(PasswordMinLengthProperty);
                var PasswordMinLength = PasswordMinLengthDet.GetValue(validationContext.ObjectInstance, null);

                if (PasswordReqMinLength != null && PasswordMinLength != null
                    && Convert.ToBoolean(PasswordReqMinLength) && Convert.ToInt32(PasswordMinLength) > 0)
                {
                    int valLength = value.ToString().Count();
                    if (valLength < Convert.ToInt32(PasswordMinLength))
                    {
                        return new ValidationResult(ErrorMessage.Replace("{0}", PasswordMinLength.ToString()).ToString());
                    }
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "minlengthpassword",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("passwordreqminlengthproperty", PasswordReqMinLengthProperty);
            rule.ValidationParameters.Add("passwordminlengthproperty", PasswordMinLengthProperty);

            yield return rule;
        }
    }
}