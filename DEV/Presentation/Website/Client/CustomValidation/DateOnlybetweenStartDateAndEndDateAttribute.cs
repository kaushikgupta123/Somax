using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class DateOnlybetweenStartDateAndEndDateAttribute : ValidationAttribute, IClientValidatable
    {

        public string otherPropertyName1;
        public string otherPropertyName2;

        public DateOnlybetweenStartDateAndEndDateAttribute() { }

        public DateOnlybetweenStartDateAndEndDateAttribute(string otherPropertyName1, string otherPropertyName2, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName1 = otherPropertyName1;
            this.otherPropertyName2 = otherPropertyName2;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var containerType = validationContext.ObjectInstance.GetType();
                var field1 = containerType.GetProperty(this.otherPropertyName1);
                var field2 = containerType.GetProperty(this.otherPropertyName2);
                var extensionValue1 = field1.GetValue(validationContext.ObjectInstance, null);
                var extensionValue2 = field2.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue1 == null || extensionValue2 == null)
                {
                    //validationResult = new ValidationResult("Start Date is empty");
                    return validationResult;
                }
                var datatype1 = extensionValue1.GetType();
                var datatype2 = extensionValue2.GetType();
                //var otherPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(this.otherPropertyName);
                if (field1 == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName1));

                if (field2 == null)
                    return new ValidationResult(String.Format("Unknown property: {0}.", otherPropertyName2));
                // Let's check that otherProperty is of type DateTime as we expect it to be
                if ((field1.PropertyType == typeof(DateTime) || (field1.PropertyType.IsGenericType && field1.PropertyType == typeof(Nullable<DateTime>))) && (field2.PropertyType == typeof(DateTime) || (field2.PropertyType.IsGenericType && field2.PropertyType == typeof(Nullable<DateTime>))))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty1 = (DateTime)field1.GetValue(validationContext.ObjectInstance, null);
                    DateTime referenceProperty2 = (DateTime)field2.GetValue(validationContext.ObjectInstance, null);
                    // if the end date is lower than the start date, than the validationResult will be set to false and return
                    // a properly formatted error message
                    //if (toValidate.CompareTo(referenceProperty1) <= 1 && referenceProperty2.CompareTo(toValidate) <= 1)
                    if (toValidate < referenceProperty1 || toValidate > referenceProperty2)
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw ex;
            }

            return validationResult;
        }
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "betweenstartdateandenddate",
            };
            rule.ValidationParameters.Add("otherproperty1", otherPropertyName1);
            rule.ValidationParameters.Add("otherproperty2", otherPropertyName2);
            yield return rule;

        }
    }
}