using Client.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Utility;

//using Utility;
//using CompareAttributeAdapter = System.ComponentModel.DataAnnotations.Comp;

namespace Client.Localization
{
    public class LocalizableModelValidatorProvider : DataAnnotationsModelValidatorProvider
    {
        private static ValidationAttribute validationAttribute;
        private static DataAnnotationsModelValidationFactory factory;
        private static DataAnnotationsModelValidationFactory DefaultAttributeFactory = Create;
        public static Dictionary<Type, DataAnnotationsModelValidationFactory> AttributeFactories = new Dictionary<Type, DataAnnotationsModelValidationFactory>()
        {
            {
                typeof(RegularExpressionAttribute),(metadata, context, attribute) => new RegularExpressionAttributeAdapter(metadata, context, (RegularExpressionAttribute)attribute)
            },
            {
                typeof(RequiredAttribute), (metadata, context, attribute) => new RequiredAttributeAdapter(metadata, context, (RequiredAttribute)attribute)
            },
            {
                typeof(StringLengthAttribute), (metadata, context, attribute) => new StringLengthAttributeAdapter(metadata, context, (StringLengthAttribute)attribute)
            },
            {
                typeof(RangeAttribute), (metadata, context, attribute) => new RangeAttributeAdapter(metadata, context, (RangeAttribute)attribute)
            }
        };
        private static ModelValidator Create(ModelMetadata metadata, ControllerContext context, ValidationAttribute attribute)
        {
            return new DataAnnotationsModelValidator(metadata, context, attribute);
        }
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            if (IsModelAvailableForUIConfig(metadata))
            {
                var UIConfigurationDetails = AssignUIConfigurationDetails(metadata);
                var Configuration = UIConfigurationDetails.Where(x => x.ColumnName.Trim().ToLower() == metadata.PropertyName.ToLower()).FirstOrDefault();
                if (Configuration != null)
                {
                    if (Configuration.Required == true)
                    {
                        yield return RequiredModelValidator(metadata, context, Configuration.RequiredErrorMessage, Configuration.ColumnLabel);
                    }
                    else if (Configuration.SystemRequired == true)
                    {
                        yield return RequiredModelValidator(metadata, context, Configuration.SystemRequiredErrorMessage, Configuration.ColumnLabel);
                    }
                    if (!string.IsNullOrEmpty(Configuration.RegularExpression))
                    {
                        yield return RegularExpressionModelValidator(metadata, context, Configuration.RegularExpressionErrorMessage, Configuration.RegularExpression);
                    }
                    if (Configuration.MaxLength > 0 && Configuration.MinLength == 0)
                    {
                        yield return MaxLengthModelValidator(metadata, context, Configuration.MaxLengthErrorMessage, Configuration.MaxLength);
                    }
                    else if (Configuration.MinLength > 0 && Configuration.MaxLength == 0)
                    {
                        yield return MinLengthModelValidator(metadata, context, Configuration.MinLengthErrorMessage, Configuration.MinLength);
                    }
                    else if (Configuration.MinLength > 0 && Configuration.MaxLength > 0)
                    {
                        yield return MinMaxLengthModelValidator(metadata, context, Configuration.MaxLengthErrorMessage, Configuration.MinLength,
                            Configuration.MaxLength);
                    }
                    if (Configuration.Range == true && (Configuration.RangeMinValue > 0 || Configuration.RangeMaxValue > 0))
                    {
                        yield return RangeModelValidator(metadata, context, Configuration.RangeErrorMessage, Convert.ToDouble(Configuration.RangeMinValue),
                            Convert.ToDouble(Configuration.RangeMaxValue));
                    }
                }
            }
            else
            {
                var validators = base.GetValidators(metadata, context, attributes);
                foreach (var item in validators)
                {
                    yield return new LocalizableModelValidator(item, metadata, context);
                }
                //return validators.Select(validator => new LocalizableModelValidator(validator, metadata, context)).ToList();
            }
        }
        private bool IsModelAvailableForUIConfig(ModelMetadata metadata)
        {
            if (metadata.Container != null && !string.IsNullOrEmpty(metadata.ContainerType.Name)
                && (metadata.ContainerType.Name == "AddEquipmentModelDynamic" || metadata.ContainerType.Name == "AddWorkOrderModelDynamic" ||
                metadata.ContainerType.Name == "AddWorkRequestModelDynamic" || metadata.ContainerType.Name == "WoDescriptionModelDynamic" ||
                metadata.ContainerType.Name == "EditEquipmentModelDynamic" || metadata.ContainerType.Name == "EditWorkOrderModelDynamic" ||
                metadata.ContainerType.Name == "WorkOrderCompletionInformationModelDynamic" || metadata.ContainerType.Name == "AddPartModelDynamic"
                || metadata.ContainerType.Name == "EditPartModelDynamic" || metadata.ContainerType.Name == "EditVendorModelDynamic" 
                || metadata.ContainerType.Name == "AddVendorModelDynamic" || metadata.ContainerType.Name == "AddPurchaseOrderModelDynamic" 
                || metadata.ContainerType.Name == "EditPurchaseOrderModelDynamic"
                || metadata.ContainerType.Name == "AddPRLineItemPartNotInInventoryModelDynamic"
                || metadata.ContainerType.Name == "EditPRLineItemPartNotInInventoryModelDynamic"
                || metadata.ContainerType.Name == "EditPRLineItemPartInInventoryModelDynamic"
                || metadata.ContainerType.Name == "AddPurchaseRequestModelDynamic"
                || metadata.ContainerType.Name == "EditPurchaseRequestModelDynamic"
                || metadata.ContainerType.Name == "AddPOLineItemPartNotInInventoryDynamic"
                || metadata.ContainerType.Name == "EditPOLineItemPartNotInInventoryModelDynamic"
                || metadata.ContainerType.Name == "EditPOLineItemPartInInventoryModelDynamic"
                || metadata.ContainerType.Name == "AddPMSRecordsModelDynamic_Calendar"
                || metadata.ContainerType.Name == "EditPMSRecordsModelDynamic_Calendar"
                || metadata.ContainerType.Name == "AddPMSRecordsModelDynamic_Meter"
                || metadata.ContainerType.Name == "EditPMSRecordsModelDynamic_Meter"
                || metadata.ContainerType.Name == "AddPMSRecordsModelDynamic_OnDemand"
                || metadata.ContainerType.Name == "EditPMSRecordsModelDynamic_OnDemand"
                || metadata.ContainerType.Name== "AddPRLineItemPartInInventoryModelDynamic"
                || metadata.ContainerType.Name == "AddPOLineItemPartInInventoryModelDynamic"
                || metadata.ContainerType.Name == "EditPOLineItemPartInInventorySingleStockModelDynamic"
                || metadata.ContainerType.Name == "EditPRLineItemPartInInventorySingleStockModelDynamic"
                || metadata.ContainerType.Name == "AddProjectCostingModelDynamic"
                || metadata.ContainerType.Name == "EditProjectCostingModelDynamic"
                ))
            {
                return true;
            }
            return false;
        }
        private List<UIConfigurationDetailsForModelValidation> AssignUIConfigurationDetails(ModelMetadata metadata)
        {
            var UIConfigurationDetails = new List<UIConfigurationDetailsForModelValidation>();
            var uiConfigObj = new RetrieveDataForUIConfiguration();
            if (metadata.ContainerType.Name == "AddEquipmentModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddAsset, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditEquipmentModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditAsset, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddWorkOrderModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddWorkOrder, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "WoDescriptionModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.WorkOrderDescribeAdd, (UserData)HttpContext.Current.Session["UserData"]);
            }

            else if (metadata.ContainerType.Name == "AddWorkRequestModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddWorkRequest, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditWorkOrderModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditWorkOrder, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "WorkOrderCompletionInformationModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.WorkOrderCompletion, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddVendorModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddVendor, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPartModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPart, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPartModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPart, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditVendorModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditVendor, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPurchaseOrderModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrder, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPurchaseOrderModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrder, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPurchaseRequestModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequest, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPurchaseRequestModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequest, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPRLineItemPartNotInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequestLineItemPartNotInInventory, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPRLineItemPartNotInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemPartNotInInventory, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPRLineItemPartInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemPartInInventory, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPOLineItemPartNotInInventoryDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrderLineItemPartNotInInventory, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPOLineItemPartNotInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemPartNotInInventory, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPOLineItemPartInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemPartInInventory, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPMSRecordsModelDynamic_Calendar")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_Calendar, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPMSRecordsModelDynamic_Calendar")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_Calendar, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPMSRecordsModelDynamic_Meter")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_Meter, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPMSRecordsModelDynamic_Meter")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_Meter, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPMSRecordsModelDynamic_OnDemand")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddScheduleRecords_OnDemand, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPMSRecordsModelDynamic_OnDemand")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.ScheduleRecordsEdit_OnDemand, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPRLineItemPartInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPurchaseRequestLineItemStockSingle, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddPOLineItemPartInInventoryModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddPurchaseOrderLineItemStockSingle, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPOLineItemPartInInventorySingleStockModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseOrderLineItemStockSingle, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditPRLineItemPartInInventorySingleStockModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditPurchaseRequestLineItemStockSingle, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "AddProjectCostingModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.AddProject, (UserData)HttpContext.Current.Session["UserData"]);
            }
            else if (metadata.ContainerType.Name == "EditProjectCostingModelDynamic")
            {
                UIConfigurationDetails = uiConfigObj.Retrieve(DataDictionaryViewNameConstant.EditProject, (UserData)HttpContext.Current.Session["UserData"]);
            }
            return UIConfigurationDetails;
        }
        private static ModelValidator RequiredModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, string ColumnLabel)
        {
            validationAttribute = new RequiredAttribute();
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = string.Format(UtilityFunction.GetMessageFromResource("globalRequiredDynamicErr", LocalizeResourceSetConstants.Global).ToString()
                                , ColumnLabel);
            }
            validationAttribute.ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage;
            if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
            {
                factory = DefaultAttributeFactory;
            }
            return factory(metadata, context, validationAttribute);
        }
        private static ModelValidator RegularExpressionModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, string RegularExpression)
        {
            validationAttribute = new RegularExpressionAttribute(RegularExpression);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = UtilityFunction.GetMessageFromResource("globalRegExDynamicErr", LocalizeResourceSetConstants.Global);
            }
            validationAttribute.ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage;
            if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
            {
                factory = DefaultAttributeFactory;
            }
            return factory(metadata, context, validationAttribute);
        }
        private static ModelValidator MinLengthModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, int MinLength)
        {
            int MaxLength = 1073741823;
            validationAttribute = new StringLengthAttribute(MaxLength)
            {
                MinimumLength = MinLength
            };
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = string.Format(UtilityFunction.GetMessageFromResource("globalMinLengthDynamicErr", LocalizeResourceSetConstants.Global).ToString()
                                , MinLength);
            }
            validationAttribute.ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage;
            if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
            {
                factory = DefaultAttributeFactory;
            }
            return factory(metadata, context, validationAttribute);
        }
        private static ModelValidator MaxLengthModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, int MaxLength)
        {
            validationAttribute = new StringLengthAttribute(MaxLength);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = string.Format(UtilityFunction.GetMessageFromResource("globalMaxLengthDynamicErr", LocalizeResourceSetConstants.Global).ToString()
                                , MaxLength);
            }
            validationAttribute.ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage;
            if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
            {
                factory = DefaultAttributeFactory;
            }
            return factory(metadata, context, validationAttribute);
        }
        private static ModelValidator MinMaxLengthModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, int MinLength,
            int MaxLength)
        {
            validationAttribute = new StringLengthAttribute(MaxLength)
            {
                MinimumLength = MinLength
            };
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = string.Format(UtilityFunction.GetMessageFromResource("globalMinMaxLengthDynamicErr", LocalizeResourceSetConstants.Global).ToString()
                                , MinLength, MaxLength);
            }
            validationAttribute.ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage;
            if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
            {
                factory = DefaultAttributeFactory;
            }
            return factory(metadata, context, validationAttribute);
        }
        private static ModelValidator RangeModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, double MinValue,
            double MaxValue)
        {
            validationAttribute = new RangeAttribute(MinValue, MaxValue);
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                ErrorMessage = string.Format(UtilityFunction.GetMessageFromResource("globalRangeDynamicErr", LocalizeResourceSetConstants.Global).ToString(),
                                MinValue, MaxValue);
            }
            validationAttribute.ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage;
            if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
            {
                factory = DefaultAttributeFactory;
            }
            return factory(metadata, context, validationAttribute);
        }
        //private static ModelValidator CompareModelValidator(ModelMetadata metadata, ControllerContext context, string ErrorMessage, string OtherProperty)
        //{
        //    validationAttribute = new CompareAttribute(OtherProperty)
        //    {
        //        ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? " " : ErrorMessage
        //    };
        //    var adapter = new CompareAttributeAdapter(validationAttribute);
        //    if (!AttributeFactories.TryGetValue(validationAttribute.GetType(), out factory))
        //    {
        //        factory = DefaultAttributeFactory;
        //    }
        //    return factory(metadata, context, validationAttribute);
        //}
    }

    public class RetrieveDataForUIConfiguration
    {
        public List<UIConfigurationDetailsForModelValidation> Retrieve(string ViewName, UserData userData)
        {
            UIConfigurationDetailsForModelValidation config;
            List<UIConfigurationDetailsForModelValidation> configList = new List<UIConfigurationDetailsForModelValidation>();
            LoginCacheSet loginCacheSet = new LoginCacheSet();
            loginCacheSet.userData = userData;
            var uIConfigList = loginCacheSet.GetUIConfigurationAllCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), ViewName, userData);
            foreach (var item in uIConfigList)
            {
                config = new UIConfigurationDetailsForModelValidation();
                config.TableName = item.TableName;
                config.ColumnLabel = item.ColumnLabel;
                config.ColumnName = item.ColumnName;
                config.ColumnType = item.ColumnType;
                config.SystemRequired = item.SystemRequired;
                config.SystemRequiredErrorMessage = item.SystemRequiredErrorMessage;
                config.Required = item.Required;
                config.RequiredErrorMessage = item.RequiredErrorMessage;
                config.MaxLength = item.MaxLength;
                config.MaxLengthErrorMessage = item.MaxLengthErrorMessage;
                config.MinLength = item.MinLength;
                config.MinLengthErrorMessage = item.MinLengthErrorMessage;
                config.RegularExpression = item.RegularExpression;
                config.RegularExpressionErrorMessage = item.RegularExpressionErrorMessage;
                config.Range = item.Range;
                config.RangeMinValue = item.RangeMinValue;
                config.RangeMaxValue = item.RangeMaxValue;
                config.RangeErrorMessage = item.RangeErrorMessage;
                config.Section = item.Section;
                config.SectionName = item.SectionName;
                config.LookupType = item.LookupType;
                config.LookupName = item.LookupName;
                config.Display = item.Display;
                config.ViewOnly = item.ViewOnly;
                config.UDF = item.UDF;

                configList.Add(config);
            }
            return configList;
        }
    }
}