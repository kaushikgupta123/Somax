using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class PasswordNotEqualUsernameAttribute : ValidationAttribute, IClientValidatable
    {
        private string PasswordNotEqualUsernameProperty, UsernameProperty;
        public PasswordNotEqualUsernameAttribute(string PasswordNotEqualUsernameProperty, string UsernameProperty)
        {
            this.PasswordNotEqualUsernameProperty = PasswordNotEqualUsernameProperty;
            this.UsernameProperty = UsernameProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && !string.IsNullOrEmpty(PasswordNotEqualUsernameProperty) && !string.IsNullOrEmpty(UsernameProperty))
            {
                var PasswordNotEqualUsernameDet = validationContext.ObjectInstance.GetType().GetProperty(PasswordNotEqualUsernameProperty);
                var PasswordNotEqualUsername = PasswordNotEqualUsernameDet.GetValue(validationContext.ObjectInstance, null);

                var UsernameDet = validationContext.ObjectInstance.GetType().GetProperty(UsernameProperty);
                var Username = UsernameDet.GetValue(validationContext.ObjectInstance, null);

                if (PasswordNotEqualUsername != null && Username != null && Convert.ToBoolean(PasswordNotEqualUsername))
                {
                    if (value.ToString().ToLower() == Username.ToString().ToLower())
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "passwordnotequalusername",
                ErrorMessage = ErrorMessage
            };

            rule.ValidationParameters.Add("passwordnotequalusernameproperty", PasswordNotEqualUsernameProperty);
            rule.ValidationParameters.Add("usernameproperty", UsernameProperty);

            yield return rule;
        }
    }
}