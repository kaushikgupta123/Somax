using Client.BusinessWrapper.Common;
using Client.Common;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.CustomValidation
{
    public class RequiredAsPerUIAttribute : ValidationAttribute, IClientValidatable
    {
        public string ViewName { get; set; }
        public string ViewName1 { get; set; }
        private String PropertyName { get; set; }
        private String PropertyName1 { get; set; }
        private Object DesiredValue { get; set; }
        public bool reqResult { get; set; }
        public bool hidResult { get; set; }
        public bool disabResult { get; set; }
        private string externvalValue { get; set; }
        private string disabledValue { get; set; }
        private readonly RequiredAttribute _innerAttribute;

        /// <summary>
        /// This custom validator has been used for the same model item which is used for Add and Edit. Hence both Add and Edit view have been passed from the corresponding model along with the Id field name and the "0". 
        /// The Id field is assigned to the objId and the "0" is assigned to desiredValue(see the code below)
        /// If the Id is "0" in the corresponding model item, the Add view will be considered else the Edit view will be considered
        /// </summary>
        /// <param name="viewNameAdd"></param> ---for Add view
        /// <param name="viewNameEdit"></param> ---for Edit view
        /// <param name="objId"></param>  ---for Id field
        /// <param name="desiredZerovalue"></param>  ---for "0"
        /// <param name="externalVal"></param>
        /// <param name="disableVal"></param>
        public RequiredAsPerUIAttribute(string viewNameAdd, string viewNameEdit, String objId, Object desiredZerovalue, string externalVal = "", string disableVal = "")
        {
            ViewName = viewNameAdd;
            PropertyName = objId;
            PropertyName1 = externalVal;
            ViewName1 = viewNameEdit;
            DesiredValue = desiredZerovalue;
            disabledValue = disableVal;
            _innerAttribute = new RequiredAttribute();
        }
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var requiredVal = context.MemberName;
            var userData = (UserData)HttpContext.Current.Session["userData"];
            CommonWrapper cWrapper = new CommonWrapper(userData);
            string isHide = "";
            string isRequired = "";
            string target = "validator";
            bool isExternal = false;
            string vuName = string.Empty;
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName).GetValue(context.ObjectInstance, null);

            string dependentValue1 = string.Empty;
            if (!string.IsNullOrEmpty(PropertyName1))
            {
                dependentValue1 = Convert.ToString(context.ObjectInstance.GetType().GetProperty(PropertyName1).GetValue(context.ObjectInstance, null));
            }
            
            if (dependentValue != null && dependentValue.ToString().ToLower() != DesiredValue.ToString().ToLower())
            {
                vuName = ViewName1;
            }
            else
            {
                vuName = ViewName;
            }

            if (string.IsNullOrEmpty(PropertyName1))
            {
                externvalValue = UiConfigConstants.IsExternalNone;
            }
            else
            {
                isExternal = Convert.ToBoolean(dependentValue1);
                if (isExternal)
                {
                    externvalValue = UiConfigConstants.IsExternalTrue;
                }
                else
                {
                    externvalValue = UiConfigConstants.IsExternalFalse;
                }
            }

            //var allList = cWrapper.UiConfigAllColumnsCustom(vuName, isHide, isRequired, externvalValue, target).Where(x => (x.ColumnName == requiredVal)).ToList();
            var allList = cWrapper.GetAllUiConfigList(vuName,  externvalValue).Where(x => (x.ColumnName == requiredVal)).ToList();

            if (allList != null && allList.Count > 0)
            {

                reqResult = allList.Where(x => x.ColumnName == requiredVal).FirstOrDefault().Required;
                hidResult = allList.Where(x => x.ColumnName == requiredVal).FirstOrDefault().Hide;
                disabResult = allList.Where(x => x.ColumnName == requiredVal).FirstOrDefault().Disable;

                if (!string.IsNullOrEmpty(disabledValue))
                {
                    if (reqResult == true && hidResult == false )
                    {
                        if (!_innerAttribute.IsValid(value))
                        {
                            return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                        }
                    }
                }
                else
                {
                    if (reqResult == true && hidResult == false && disabResult == false)
                    {
                        if (!_innerAttribute.IsValid(value))
                        {
                            return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                        }
                    }
                }
                
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var propname = metadata.PropertyName;
            var requireduirule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = "requiredasperui",
            };
            requireduirule.ValidationParameters["propname"] = propname;
            yield return requireduirule;
        }
    }
}