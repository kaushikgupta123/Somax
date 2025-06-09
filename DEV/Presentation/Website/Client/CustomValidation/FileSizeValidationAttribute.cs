using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class FileSizeValidationAttribute : ValidationAttribute, IClientValidatable
    {
        int FileSize = 0;
        public FileSizeValidationAttribute(int _FileSize)
        {
            FileSize = _FileSize;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rules = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "filesize"
            };
            rules.ValidationParameters.Add("maxsize", this.FileSize);
            yield return rules;
        }


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int MaxContentLength = 1024 * 1024 * 10;
            if (value != null)
            {
                var file = value as HttpPostedFileBase;

                if (file.ContentLength > MaxContentLength)
                {
                    ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
                    return new ValidationResult(ErrorMessage);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
                return ValidationResult.Success;

        }

    }
}