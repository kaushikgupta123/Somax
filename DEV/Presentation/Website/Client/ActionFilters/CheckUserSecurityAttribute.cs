using Client.Common;
using Client.Models.Common;

using Common.Constants;
using DataContracts;

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Client.ActionFilters
{
    public class CheckUserSecurityAttribute : ActionFilterAttribute
    {
        public string securityType { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var data = filterContext.HttpContext.Session["userData"];
            var userData = (UserData)data;
            var redirectResult = new RedirectResult("/error/NotAuthorized");
            // Work Request profile is not 9 for work request 
            // V2 Security Profiles
            //if (userData.DatabaseKey.User.UserType.ToLower() ==UserTypeConstants.WorkRequest.ToLower()  && userData.DatabaseKey.User.SecurityProfileId == 9)
            if (userData.DatabaseKey.User.UserType.ToLower() ==UserTypeConstants.WorkRequest.ToLower())
            {
                if (!userData.Security.Reports.Access)
                {
                    filterContext.Result = redirectResult;
                }
            }
            else
            {
                if (securityType == SecurityConstants.Equipment)
                {
                    if (!userData.Security.Equipment.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.WorkOrders)
                {
                    if ((!userData.Security.WorkOrders.Access && !userData.Security.WorkOrders.CreateRequest) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.WorkOrder_Approve)
                {
                    if (userData.Security.WorkOrders.Access)
                    {
                        if (!userData.Security.WorkOrders.Approve || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.WorkOrder_LaborScheduling)
                {
                    if (userData.Security.WorkOrders.Access)
                    {
                        if (!userData.Security.WorkOrders.LaborScheduling || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.WorkOrder_Planning)
                {
                    if (userData.Security.WorkOrders.Access)
                    {
                        if (!userData.Security.WorkOrders.Planning)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PrevMaint)
                {
                    if (!userData.Security.PrevMaint.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PrevMaint_PMForecast)
                {
                    if (userData.Security.PrevMaint.Access)
                    {
                        if (!userData.Security.PrevMaint.PrevMaintPMForecast || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Meters)
                {
                    if (!userData.Security.Meters.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Sensors)
                {
                    if (!userData.Security.Sensors.Access || IsSanitationOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.SensorSearch)
                {
                    if (!userData.Security.Sensors.Access || IsSanitationOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                    //else
                    //{
                    //    filterContext.Result = redirectResult;
                    //}
                }
                else if (securityType == SecurityConstants.AlertProcedures )
                {
                    if (userData.Security.Sensors.Access)
                    {
                        if (!userData.Security.Sensors.AlertProcedures || IsSanitationOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts)
                {
                    if (!userData.Security.Parts.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PartMasterRequest_Review)
                {
                    if (userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster && (userData.Security.Parts.SiteReview || userData.Security.PartMasterRequest.Access))
                    {
                        if (!userData.Security.Parts.SiteReview || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PartMasterRequest)
                {
                    if (userData.DatabaseKey.Client.PackageLevel.ToUpper() ==PackageLevelConstant.Enterprise && userData.Site.UsePartMaster && (userData.Security.Parts.SiteReview || userData.Security.PartMasterRequest.Access))
                    {
                        if (!userData.Security.PartMasterRequest.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts_Checkout)
                {
                    if (userData.Security.Parts.Access)
                    {
                        if (!userData.Security.Parts.Checkout || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts_Issue)
                {
                    if (userData.Security.Parts.Access)
                    {
                        if (!userData.Security.Parts_Issue.Access)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts_Receipt)
                {
                    if (userData.Security.Parts.Access)
                    {
                        if (!userData.Security.Parts.Receipt || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts_Physical)
                {
                    if (userData.Security.Parts.Access)
                    {
                        if (!userData.Security.Parts.Physical || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts_Multi_Site_Search)
                {
                    if (userData.Security.Parts.Access)
                    {
                        if (!userData.Security.Parts.MultiSiteSearch || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PartTransfers)
                {
                    if (userData.Security.Parts.Access)
                    {
                        if (!(userData.Security.PartTransfers.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster == true) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Vendors)
                {
                    if (!userData.Security.Vendors.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Purchasing)
                {
                    if (!userData.Security.Purchasing.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PurchaseRequest) //****Purchase vs Purchase Request
                {
                    if (!userData.Security.PurchaseRequest.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Purchasing_Approve)
                {
                    if (userData.Security.Purchasing.Access || userData.Security.PurchaseRequest.Access)
                    {
                        if (!userData.Security.Purchasing.Approve || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                /*else if (securityType == SecurityConstants.Purchasing) //For Purchasing Order
                {
                    if (!userData.Security.Purchasing.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }*/
                else if (securityType == SecurityConstants.Purchasing_Receive)
                {
                    if (userData.Security.Purchasing.Access || userData.Security.PurchaseRequest.Access)
                    {
                        if (!userData.Security.Purchasing.ReceiveAccess || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.InvoiceMatching)
                {
                    if (userData.Security.Purchasing.Access || userData.Security.PurchaseRequest.Access)
                    {
                        if (!userData.Security.InvoiceMatching.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                /*else if (securityType == SecurityConstants.PurchaseRequest)
                {
                    if (!userData.Security.PurchaseRequest.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }*/
                else if (securityType == SecurityConstants.Reports)
                {
                    if (!userData.Security.Reports.Access || IsSanitationOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                /*else if(securityType == SecurityConstants.Sanitation)
                {
                    if(!userData.Security.Sanitation.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }*/
                else if (securityType == SecurityConstants.SanitationJob)
                {
                    if (userData.Security.Sanitation.Access)
                    {
                        if (!(userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices && userData.Security.SanitationJob.Access) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.SanitationJob_ApprovalWorkbench)
                {
                    if (userData.Security.Sanitation.Access)
                    {
                        if (!(userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices && userData.Security.Sanitation.Approve && !userData.Site.ExternalSanitation) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Sanitation_Verification)
                {
                    if (userData.Security.Sanitation.Access)
                    {
                        if (!(userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices && userData.Security.Sanitation.Verification && !userData.Site.ExternalSanitation) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Master_Schedule_Search) //For Master Schedule
                {
                    if (userData.Security.Sanitation.Access)
                    {
                        if (!(userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices && userData.Security.Sanitation.Access) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Master_Schedule_Forecast) //For Master Schedule Forecast
                {
                    if (userData.Security.Sanitation.Access)
                    {
                        if (!(userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices && userData.Security.Sanitation.JobGeneration && !userData.Site.ExternalSanitation) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Personnel) //For Personnel
                {
                    if (!userData.Security.Personnel.Access)
                    {
                            filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Project) //For Project
                {
                    if (!userData.Security.Project.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PurchaseRequest_AutoGeneration) //For AutoPRGeneration
                {
                    if (userData.Security.PurchaseRequest.Access)
                    {
                        if (!userData.Security.PurchaseRequest.AutoGeneration)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Parts_CycleCount) //For Part Cycle count
                {
                    if (userData.Security.Parts.CycleCount)
                    {
                        if (!userData.Security.Parts.CycleCount)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                #region Config
                else if (securityType == SecurityConstants.Accounts)
                {
                    if (!userData.Security.Accounts.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                    else
                    {
                        if ( IsSanitationOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                }
                else if (securityType == SecurityConstants.OnDemand_Library)
                {
                    if (userData.Security.OnDemandLibrary.Access || userData.Security.PrevMaintLibrary.Access)
                    {
                        if (!userData.Security.OnDemandLibrary.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PrevMaint_Library)
                {
                    if (userData.Security.OnDemandLibrary.Access || userData.Security.PrevMaintLibrary.Access)
                    {
                        if (!userData.Security.PrevMaintLibrary.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Sanitation_OnDemand)
                {
                    //if (userData.Security.OnDemandLibrary.Access || userData.Security.PrevMaintLibrary.Access)
                    //{
                    //    if (!(userData.Security.Sanitation.OnDemand && userData.DatabaseKey.Client.BusinessType.ToUpper() == BusinessTypeConstants.FoodServices) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    //    {
                    //        filterContext.Result = redirectResult;
                    //    }
                    //}                   
                    if (userData.Security.Sanitation.Access)
                    {
                        if (!(userData.Security.Sanitation.OnDemand))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.MasterSanitation_Library)
                {
                    if (!userData.Security.MasterSanitation.Access || IsSanitationOnly(userData) || IsAPMOnly(userData))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Fleet_Service_Task)
                {
                    if (!(userData.Site.Fleet && userData.Security.Fleet_ServiceTask.Access))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.EquipmentMaster)
                {
                    if (userData.Security.EquipmentMaster != null && userData.Security.EquipmentMaster.Access)
                    {
                        if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper()==PackageLevelConstant.Enterprise) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.VendorMaster)
                {
                    if (userData.Security.VendorMaster != null && userData.Security.VendorMaster.Access)
                    {
                        if (!(userData.Security.VendorMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() ==PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.VendorCatalog)
                {
                    if (userData.Security.VendorMaster != null && userData.Security.VendorMaster.Access)
                    {
                        if (!(userData.Security.VendorCatalog.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() ==PackageLevelConstant.Enterprise && userData.Site.UseVendorMaster) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PartMaster)
                {
                    if (userData.Security.PartMaster != null && userData.Security.PartMaster.Access)
                    {
                        if (!(userData.Security.PartMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() ==PackageLevelConstant.Enterprise && userData.Site.UsePartMaster) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.ManufacturerMaster)
                {
                    if (userData.Security.PartMaster != null && userData.Security.PartMaster.Access)
                    {
                        if (!(userData.Security.ManufacturerMaster.Access && userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.Site.UsePartMaster) || IsSanitationOnly(userData) || IsAPMOnly(userData))
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Fleet_Assets)
                {
                    if (!userData.Security.Fleet_Assets.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Fleet_Meter_History)
                {
                    if (!(userData.Site.Fleet && userData.Security.Fleet_MeterHistory.Access))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Fleet_Fuel_Tracking)
                {
                    if (!(userData.Site.Fleet && userData.Security.Fleet_FuelTracking.Access))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Fleet_Scheduled_Service)
                {
                    if (!(userData.Site.Fleet && userData.Security.Fleet_Scheduled.Access))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Fleet_Service_Order)
                {
                    if (!(userData.Site.Fleet && userData.Security.Fleet_ServiceOrder.Access))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.CustomSecurityProfile)
                {
                    if (!(userData.DatabaseKey.Client.PackageLevel.ToUpper() == PackageLevelConstant.Enterprise && userData.DatabaseKey.User.IsSuperUser))
                    {
                        filterContext.Result = redirectResult;
                    }                   
                }
                else if (securityType == SecurityConstants.PartCategoryMaster)
                {
                    if (!(userData.Security.PartCategoryMaster.Access && userData.Site.UsePartMaster))
                    {
                        filterContext.Result = redirectResult;
                    }
                   
                }
                #endregion
                else if(securityType== SecurityConstants.Storeroom) // For Storeroom Setup
                {
                    if (!(userData.DatabaseKey.User.UserType.ToUpper() == UserTypeConstants.Admin.ToUpper() || userData.DatabaseKey.User.IsSuperUser && userData.DatabaseKey.Client.UseMultiStoreroom==true))
                    {
                        filterContext.Result = redirectResult;
                    }
                }

                else if (securityType == SecurityConstants.Parts_MaterialRequest) //For Material Request
                {
                    if (userData.Security.Parts.MaterialRequest)
                    {
                        if (!userData.Security.Parts.MaterialRequest)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.ApprovalGroupsConfiguration) //For ApprovalGroupsConfiguration V2-720
                {
                    if (userData.Security.ApprovalGroupsConfiguration.Access)
                    {
                        if (!userData.Security.ApprovalGroupsConfiguration.Access)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }

                else if (securityType == SecurityConstants.StoreroomTransfer) // For Storeroom Transfer
                {
                    if (!userData.DatabaseKey.Client.UseMultiStoreroom)
                    {
                        filterContext.Result = redirectResult;
                    }
                }


                else if (securityType == SecurityConstants.WorkOrder_ApprovalPage) //V2-730 Work Order Approval Page
                {
                    if (userData.Security.WorkOrders.Access)
                    {
                        if (!userData.Security.WorkOrders.Approve || IsSanitationOnly(userData) || IsAPMOnly(userData) || userData.DatabaseKey.ApprovalGroupSettings.WorkRequests)
                        {
                            filterContext.Result = redirectResult;
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }

                else if (securityType == SecurityConstants.PurchaseRequest_ApprovalPage) //V2-730 Purchase Request Approval Page
                {
                    if (userData.Security.Purchasing.Access || userData.Security.PurchaseRequest.Access)
                    {
                        #region V2-822
                        bool OraclePurchaseRequestExportInUse = false;
                        var InterfacePropData = (List<InterfacePropModel>)System.Web.HttpContext.Current.Session["InterfacePropData"];
                        if (InterfacePropData != null && InterfacePropData.Count > 0)
                        {
                            OraclePurchaseRequestExportInUse = InterfacePropData.Where(x => x.InterfaceType == ApiConstants.OraclePurchaseRequestExport).Select(x => x.InUse).FirstOrDefault();
                        }
                        #endregion
                        if (!userData.Security.Purchasing.Approve || IsSanitationOnly(userData) || IsAPMOnly(userData) || userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests)
                        {
                            filterContext.Result = redirectResult;
                        }
                        else // V2-822
                        {
                            if (OraclePurchaseRequestExportInUse)
                            {
                                if (!(userData.DatabaseKey.Personnel.ExOracleUserId != "" && userData.Security.PurchaseRequest.Approve))
                                {
                                    filterContext.Result = redirectResult;
                                }
                            }
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Approval) //For Approval V2-769
                {
                    if (userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests || userData.DatabaseKey.ApprovalGroupSettings.MaterialRequests || userData.DatabaseKey.ApprovalGroupSettings.WorkRequests)
                    {
                        if (!(userData.DatabaseKey.ApprovalGroupSettings.PurchaseRequests && userData.Security.PurchaseRequest.Approve) && !(userData.DatabaseKey.ApprovalGroupSettings.MaterialRequests && userData.Security.MaterialRequest_Approve.Access) && !(userData.DatabaseKey.ApprovalGroupSettings.WorkRequests && userData.Security.WorkOrders.Approve))
                        {
                            filterContext.Result = redirectResult;
                            
                        }
                    }
                    else
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.BBUKPI_Enterprise) //For BBUKPI_Enterprise V2-823
                {
                    if (!userData.Security.BBUKPI_Enterprise.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.BBUKPI_Site) //For BBUKPI_Enterprise V2-823
                {
                    if (!userData.Security.BBUKPI_Site.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.PartTransfer_Auto_Transfer_Generation) // For Auto Transfer Request Setup V2-1059
                {
                    if (!(userData.DatabaseKey.Client.UseMultiStoreroom && userData.Security.PartTransfer_Auto_Transfer_Generation.Access))
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.ShipToAddress) //For ShipToAddress V2-1086
                {
                    if (!userData.Security.ShipToAddress.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Vendor_Create_Vendor_Request)
                {
                    if (!userData.Security.Vendor_Create_Vendor_Request.Access)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
                else if (securityType == SecurityConstants.Analytics_WorkOrderStatus) //For Analytics WO Status Dashboard V2-1177
                {
                    if (!userData.Security.Analytics.WorkOrderStatus)
                    {
                        filterContext.Result = redirectResult;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
        public bool IsAPMOnly(UserData userData)
        {
            if (userData.Site.Sanitation == false && userData.Site.CMMS == false && userData.Site.APM == true)
            {
                return true;
            }
            return false;
        }
        public bool IsSanitationOnly(UserData userData)
        {
            if (userData.Site.Sanitation == true && userData.Site.CMMS == false && userData.Site.APM == false)
            {
                return true;
            }
            return false;
        }
    }

}