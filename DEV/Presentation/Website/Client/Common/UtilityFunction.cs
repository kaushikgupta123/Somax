using Client.BusinessWrapper.Common;
using Client.Models;
using Client.Models.BusinessIntelligence;
using Client.Models.Common;
using Client.Models.Common.OnPremiseAttachment;

using Common.Constants;

using DataContracts;
using QRCoder;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Utility;

namespace Client.Common
{
    public static class UtilityFunction
    {
        private static object locker = new Object();
        private static string EncKeyVal = ConfigurationManager.AppSettings["encKeyV"].ToString();
        internal static void LogException(string FileWithPath, Exception ex, UserData user, bool mailSendingStatus = false)
        {
            lock (locker)
            {

                using (StreamWriter sW = new StreamWriter(HttpContext.Current.Server.MapPath(FileWithPath), true))
                {
                    string timestampString = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + " at " + DateTime.Now.TimeOfDay.ToString();
                    sW.WriteLine("Timestamp: " + timestampString);
                    sW.WriteLine("User: " + user.DatabaseKey.UserName ?? "Not Logged in.");
                    sW.WriteLine();
                    sW.WriteLine("Url : " + HttpContext.Current.Request.Url.AbsolutePath.ToString());
                    sW.WriteLine("Message:");
                    sW.WriteLine(ex.Message);
                    sW.WriteLine("StactTrace:");
                    sW.WriteLine(ex.StackTrace + Environment.NewLine);
                    if (ex.InnerException != null)
                    {
                        sW.WriteLine("Inner Exception Message:");
                        sW.WriteLine(ex.InnerException.Message + Environment.NewLine);
                    }

                    sW.WriteLine("####################");
                }
            }
        }       
        public static string GetMessageFromResource(string resourceId, string localizeResourceSetConstants)
        {
            var SessionData = HttpContext.Current.Session["userData"];
            if (SessionData != null)
            {
                var data = HttpContext.Current.Session["userData"];
                var userData = (UserData)data;
                LoginCacheSet _logCache = new LoginCacheSet();
                List<Localizations> locGlobalCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), localizeResourceSetConstants, userData.Site.Localization);

                var result = locGlobalCache.Where(x => x.ResourceId.Equals(resourceId)).Select(x => x.Value).FirstOrDefault();
                return result;
            }
            return string.Empty;
        }

        internal static List<DropDownModel> GetScheduleType()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("Calendar",LocalizeResourceSetConstants.StatusDetails),value="Calendar"},
                new DropDownModel{ text=GetMessageFromResource("Meter",LocalizeResourceSetConstants.Global),value="Meter"},
                new DropDownModel { text = GetMessageFromResource("OnDemand",LocalizeResourceSetConstants.StatusDetails), value = "OnDemand" }
             };
            return ddList;
        }

        internal static List<DropDownModel> GetPMLibraryScheduleType()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("Calendar",LocalizeResourceSetConstants.StatusDetails),value="Calendar"},
                new DropDownModel { text = GetMessageFromResource("OnDemand",LocalizeResourceSetConstants.StatusDetails), value = "OnDemand" }
             };
            return ddList;
        }

        internal static List<DropDownModel> GetSanitationScheduleType()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("OnDemand",LocalizeResourceSetConstants.StatusDetails),value="OnDemand"},
                new DropDownModel { text = GetMessageFromResource("spnDays",LocalizeResourceSetConstants.Global), value = "Days" }
             };
            return ddList;
        }
        internal static List<DropDownModel> populateScheduleMethodList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnMeterLastDue",LocalizeResourceSetConstants.PrevMaintDetails),value="LastDue"},
                new DropDownModel{ text=GetMessageFromResource("spnLastPerformed",LocalizeResourceSetConstants.Global),value="LastPerformed"}
             };
            return ddList;
        }
        internal static List<DropDownModel> populateFrequencyTypeList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnDays",LocalizeResourceSetConstants.Global),value="Days"},
                new DropDownModel{ text=GetMessageFromResource("spnMonthsDOW",LocalizeResourceSetConstants.Global),value="MonthsDOW"},
                new DropDownModel{ text=GetMessageFromResource("spnMonthsDate",LocalizeResourceSetConstants.Global),value="MonthsDate"}

             };
            return ddList;
        }
        internal static List<DropDownModel> populateMeterMethodList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text="0",value="LastDue"},
                new DropDownModel{ text="1",value="LastPerformed"}

             };
            return ddList;
        }
        internal static List<DropDownModel> populateChargeType()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("Equipment",LocalizeResourceSetConstants.SecurityProfileItemsDetails),value="Equipment"},
                new DropDownModel{ text = GetMessageFromResource("spnLocation", LocalizeResourceSetConstants.Global), value="Location"}

             };
            var SessionData = HttpContext.Current.Session["userData"];
            if (SessionData != null)
            {
                var data = HttpContext.Current.Session["userData"];
                var userData = (UserData)data;
                if (userData.DatabaseKey.Client.BusinessType.ToUpper().ToString() != BusinessTypeConstants.Facilities)
                {
                    var itemToRemove = ddList.Single(r => r.value == "Location");
                    ddList.Remove(itemToRemove);
                }
            }

            return ddList;
        }
        internal static List<DropDownModel> populateChargeTypeForInventoryCheckout()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("Equipment",LocalizeResourceSetConstants.SecurityProfileItemsDetails),value="Equipment"},
                new DropDownModel{ text=GetMessageFromResource("GlobalWorkOrder",LocalizeResourceSetConstants.Global),value="WorkOrder"},
                new DropDownModel{ text=GetMessageFromResource("spnLocation", LocalizeResourceSetConstants.Global),value="Location"}

             };
            return ddList;
        }
        internal static List<DropDownModel> populateChargeTypeForLineItem()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                 new DropDownModel{ text=GetMessageFromResource("Account", LocalizeResourceSetConstants.Menu),value="Account"},
                  new DropDownModel{ text = GetMessageFromResource("GlobalWorkOrder", LocalizeResourceSetConstants.Global), value="WorkOrder"},
                new DropDownModel{ text=GetMessageFromResource("Equipment",LocalizeResourceSetConstants.SecurityProfileItemsDetails),value="Equipment"},
             };
            return ddList;
        }
        #region V2-907
        internal static List<DropDownModel> populateChargeTypeForPurchaseRequestLineItem()
        {
            bool OraclePurchaseRequestExportInUse = false;
            var InterfacePropData = (List<InterfacePropModel>)System.Web.HttpContext.Current.Session["InterfacePropData"];
            if (InterfacePropData != null && InterfacePropData.Count > 0)
            {
                OraclePurchaseRequestExportInUse = InterfacePropData.Where(x => x.InterfaceType == ApiConstants.OraclePurchaseRequestExport).Select(x => x.InUse).FirstOrDefault();
            }
            if (OraclePurchaseRequestExportInUse)
            {
                List<DropDownModel> ddList = new List<DropDownModel>
                {
                new DropDownModel { text = GetMessageFromResource("GlobalWorkOrder", LocalizeResourceSetConstants.Global), value = "WorkOrder" },
                new DropDownModel { text = GetMessageFromResource("Equipment", LocalizeResourceSetConstants.SecurityProfileItemsDetails), value = "Equipment" },
                };
                return ddList;

            }
            else
            {
                List<DropDownModel> ddList = new List<DropDownModel>
                {
                new DropDownModel { text = GetMessageFromResource("Account", LocalizeResourceSetConstants.Menu), value = "Account" },
                new DropDownModel { text = GetMessageFromResource("GlobalWorkOrder", LocalizeResourceSetConstants.Global), value = "WorkOrder" },
                new DropDownModel { text = GetMessageFromResource("Equipment", LocalizeResourceSetConstants.SecurityProfileItemsDetails), value = "Equipment" },
                };
                return ddList;
            }
        }

        #endregion

        internal static List<DropDownModel> populateChargeTypeForPartHistory()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                 new DropDownModel{ text=GetMessageFromResource("Account", LocalizeResourceSetConstants.Menu),value="Account"},
                  new DropDownModel{ text = GetMessageFromResource("GlobalWorkOrder", LocalizeResourceSetConstants.Global), value="Work Order"},
                new DropDownModel{ text=GetMessageFromResource("Equipment",LocalizeResourceSetConstants.SecurityProfileItemsDetails),value="Equipment"},
             };
            return ddList;
        }
        internal static List<DropDownModel> populateSourceList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnInternal",LocalizeResourceSetConstants.Global),value="Internal"},
                new DropDownModel{ text=GetMessageFromResource("spnExternal",LocalizeResourceSetConstants.Global),value="External"}

             };
            return ddList;
        }
        internal static List<DropDownModel> WorkBenchStatusList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnShowAll",LocalizeResourceSetConstants.Global),value="0"},
                new DropDownModel{ text=GetMessageFromResource("spnAssignedToMe",LocalizeResourceSetConstants.Global),value="1"}

             };
            return ddList;
        }
        internal static List<DropDownModel> WorkBenchCreateDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnShowAll",LocalizeResourceSetConstants.Global),value="0"},
                new DropDownModel{ text=GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnOneWeek",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnThirtyDays",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnSixtyDays",LocalizeResourceSetConstants.Global),value="4"}

             };
            return ddList;
        }

        internal static List<DropDownModel> EquipmentDownTimeDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text=GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="3"}

             };
            return ddList;
        }
        internal static List<DropDownModel> WorkOrderDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnSelectDateRange",LocalizeResourceSetConstants.Global),value=""},
                new DropDownModel{ text=GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="4"}

             };
            return ddList;
        }


        internal static List<DropDownModel> PartDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnShowAll",LocalizeResourceSetConstants.Global),value="0"},
                new DropDownModel{ text=GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnOneWeek",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnThirtyDays",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnSixtyDays",LocalizeResourceSetConstants.Global),value="4"},
                 new DropDownModel{ text=GetMessageFromResource("spnSixMonths",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnOneYear",LocalizeResourceSetConstants.Global),value="6"}

             };
            return ddList;
        }

        internal static List<DropDownModel> CountryList()
        {
            List<DropDownModel> cntList = new List<DropDownModel>()
            {
                 new DropDownModel{ text=GetMessageFromResource("spnCanada",LocalizeResourceSetConstants.Global),value="Canada"},
                 new DropDownModel{ text=GetMessageFromResource("spnChina",LocalizeResourceSetConstants.Global),value="China"},
                 new DropDownModel{ text=GetMessageFromResource("spnMexico",LocalizeResourceSetConstants.Global),value="Mexico"},
                 new DropDownModel{ text=GetMessageFromResource("spnUSA",LocalizeResourceSetConstants.Global),value="USA"},
            };
            return cntList;
        }

        internal static List<DropDownModel> StateList()
        {
            List<DropDownModel> stList = new List<DropDownModel>()
            {
                    new DropDownModel{ text= "AL", value="Alabama"},
                    new DropDownModel{ text= "AK", value="Alaska"},
                    new DropDownModel{ text= "AZ", value="Arizona"},
                    new DropDownModel{ text= "AR", value="Arkansas"},
                    new DropDownModel{ text= "CA",  value="California"},
                    new DropDownModel{ text= "CO", value="Colorado"},
                    new DropDownModel{ text= "CT",value= "Connecticut"},
                    new DropDownModel{ text= "DE",value= "Delaware"},
                    new DropDownModel{ text= "FL",value= "Florida"},
                    new DropDownModel{ text= "GA",value= "Georgia"},
                    new DropDownModel{ text= "HI",value= "Hawaii"},
                    new DropDownModel{ text= "ID",value= "Idaho"},
                    new DropDownModel{ text= "IL",value= "Illinois"},
                    new DropDownModel{ text= "IA", value="Iowa"},
                    new DropDownModel{ text= "KS",value= "Kansas"},
                    new DropDownModel{ text= "KY",value= "Kentucky"},
                    new DropDownModel{ text= "LA",value= "Louisiana"},
                    new DropDownModel{ text= "ME",value= "Maine"},
                    new DropDownModel{ text= "MD",value= "Maryland"},
                    new DropDownModel{ text= "MA",value= "Massachusetts"},
                    new DropDownModel{ text= "MI",value= "Nebraska"},
                    new DropDownModel{ text= "NV",value= "Nevada"},
                    new DropDownModel{ text= "NH",value= "New Hampshire"},
                    new DropDownModel{ text= "NJ",value= "New Jersey"},
                    new DropDownModel{ text= "NM",value= "New Mexico"},
                    new DropDownModel{ text= "NY", value="New York"},
                    new DropDownModel{ text= "NC",value= "North Carolina"},
                    new DropDownModel{ text= "ND",value= "North Dakota"},
                    new DropDownModel{ text= "OH", value="Ohio"},
                    new DropDownModel{ text= "OK", value="Oklahoma"},
                    new DropDownModel{ text= "OR",value= "Oregon"},
                    new DropDownModel{ text= "PA", value="Pennsylvania"},
                    new DropDownModel{ text= "RI",value= "Rhode Island"},
                    new DropDownModel{ text= "SC", value="South Carolina"},
                    new DropDownModel{ text= "SD", value= "South Dakota"},
                    new DropDownModel{ text= "TN", value= "Tennessee"},
                    new DropDownModel{ text= "TX", value= "Texas"},
                    new DropDownModel{ text= "UT",value= "Utah"},
                    new DropDownModel{ text= "VT", value="Vermont"},
                    new DropDownModel{ text= "VA", value="Virginia"},
                    new DropDownModel{ text= "WA", value="Washington"},
                    new DropDownModel{ text= "WV", value="West Virginia"},
                    new DropDownModel{ text= "WI", value="Wisconsin"},
                    new DropDownModel{ text= "WY",value= "Wyoming"}
            };
            return stList;
        }
        internal static List<DropDownModel> DaysOfWeekList()
        {
            List<DropDownModel> stList = new List<DropDownModel>()
            {
                    new DropDownModel{ text= GetMessageFromResource("spnSunday",LocalizeResourceSetConstants.Global), value="0"},
                    new DropDownModel{ text= GetMessageFromResource("spnMonday",LocalizeResourceSetConstants.Global), value="1"},
                    new DropDownModel{ text= GetMessageFromResource("spnTuesday",LocalizeResourceSetConstants.Global), value="2"},
                    new DropDownModel{ text= GetMessageFromResource("spnWednesday",LocalizeResourceSetConstants.Global), value="3"},
                    new DropDownModel{ text= GetMessageFromResource("spnThursday",LocalizeResourceSetConstants.Global),  value="4"},
                    new DropDownModel{ text= GetMessageFromResource("spnFriday",LocalizeResourceSetConstants.Global), value="5"},
                    new DropDownModel{ text= GetMessageFromResource("spnSaturday",LocalizeResourceSetConstants.Global),value= "6"},
            };
            return stList;
        }

        #region Dashboard
        public static List<DropDownModel> DBEquipmentDownTimeDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
               // new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text= GetMessageFromResource("spnYesterday",LocalizeResourceSetConstants.Global),value="0"},
                new DropDownModel{ text= GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="3"}
             };
            return ddList;
        }
        public static List<DropDownModel> WOSourceTypeTimeDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
               // new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text= GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="4"}
             };
            return ddList;
        }
        public static List<DropDownModel> WOByTypeDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text= GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="4"}
             };
            return ddList;
        }
        public static List<DropDownModel> WoByPriorityDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text=GetMessageFromResource("spnSevenDays", LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnThirtyDays", LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays", LocalizeResourceSetConstants.Global),value="3"}
             };
            return ddList;
        }
        public static List<DropDownModel> WoSourceDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text=GetMessageFromResource("spnLastThreeMonths",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnLastSixMonths",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text = GetMessageFromResource("spnLastTwelveMonths", LocalizeResourceSetConstants.Global), value="12"}
             };
            return ddList;
        }
        public static List<DropDownModel> WoLaborHoursDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
               // new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text= GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="30"}
             };
            return ddList;
        }
        internal static List<DropDownModel> InventoryValuationDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
               // new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnLastThreeMonths",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnLastSixMonths",LocalizeResourceSetConstants.Global),value="4"}
             };
            return ddList;
        }
        internal static List<DropDownModel> PartConsignmentList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnChecked",LocalizeResourceSetConstants.Global), value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnUnchecked",LocalizeResourceSetConstants.Global), value="0"}
            };
            return ddList;
        }
        internal static List<DropDownModel> SanitExtractedList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("CancelAlertYes",LocalizeResourceSetConstants.JsAlerts), value="1"},
                new DropDownModel{ text=GetMessageFromResource("CancelAlertNo",LocalizeResourceSetConstants.JsAlerts), value="0"}
            };
            return ddList;
        }
        internal static List<DropDownModel> InActiveLookUpList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text="False", value="False"},
                new DropDownModel{ text="True", value="True"}
            };
            return ddList;
        }
        public static List<DropDownModel> APMChartDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="2"}
             };
            return ddList;
        }
        public static List<DropDownModel> SanitationOnlyChartDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text="Last 7 Days",value="1"},
                new DropDownModel{ text="Last 30 Days",value="2"},
                new DropDownModel{ text="Last 90 Days",value="3"}
             };
            return ddList;
        }
        #endregion

        #region Site Edit (Configararion)
        internal static List<DropDownModel> LocalizationTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnEnglishUS",LocalizeResourceSetConstants.Global),value="en-us"},
                new DropDownModel{ text = GetMessageFromResource("spnSpanishMexican", LocalizeResourceSetConstants.Global), value="es-mx"},
                new DropDownModel{ text=GetMessageFromResource("spnFrenchCanadian", LocalizeResourceSetConstants.Global),value="fr-ca"},
                new DropDownModel{ text=GetMessageFromResource("spnPortugueseBrazil", LocalizeResourceSetConstants.Global),value="pt-br"}

             };
            return ddList;
        }
        internal static List<DropDownModel> ActiveInactiveStatusTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="Active"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="Inactive"}//text="Inactive"
             };
            return ddList;
        }
        internal static List<DropDownModel> InactiveActiveStatusTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="2"}//text="Inactive"
             };
            return ddList;
        }
        #region V2-636 Inactive Active Status Types
        internal static List<DropDownModel> InactiveActiveStatusTypesforAsset()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="2"},//text="Inactive"
                new DropDownModel{ text=GetMessageFromResource("spnOutofService",LocalizeResourceSetConstants.EquipmentDetails),value="3"}//text="Out Of Service"
             };
            return ddList;
        }
        #endregion
        internal static List<DropDownModel> InactiveActiveStatusTypesforFleetAsset()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="2"},//text="Inactive"
                new DropDownModel{ text=GetMessageFromResource("spnOutofService",LocalizeResourceSetConstants.EquipmentDetails),value="3"}//text="Out Of Service"
             };
            return ddList;
        }
        internal static List<DropDownModel> InactiveActiveVendorStatusTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("GlobalActiveVendors",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("GlobalInactiveVendors",LocalizeResourceSetConstants.Global),value="2"}//text="Inactive"
             };
            return ddList;
        }
        internal static List<DropDownModel> InactiveStatusTypesWithBoolValue()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value= "false"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="true"}//text="Inactive"
             };
            return ddList;
        }

        internal static List<DropDownModel> IsExternalTypesWithBoolValue()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= "Internal",value= "false"},//text="Internal"
                new DropDownModel{ text="External",value="true"}//text="External"
             };
            return ddList;
        }

        internal static List<DropDownModel> PrintTypeList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("globalSmallQRCode",LocalizeResourceSetConstants.Global),value="SmallQRCode"},
                new DropDownModel{ text=GetMessageFromResource("globalLargeQRCode",LocalizeResourceSetConstants.Global),value="LargeQRCode"}
             };
            return ddList;
        }

        #endregion

        #region Event Info
        internal static List<DropDownModel> EventStatus()
        {
            List<DropDownModel> stList = new List<DropDownModel>()
            {
                    new DropDownModel{ text= GetMessageFromResource("Open",LocalizeResourceSetConstants.StatusDetails), value="Open"},
                    new DropDownModel{ text= GetMessageFromResource("spnProcessed",LocalizeResourceSetConstants.EventInfo), value="Processed"}
            };
            return stList;
        }
        internal static List<DropDownModel> EventSourceType()
        {
            List<DropDownModel> stList = new List<DropDownModel>()
            {
                    new DropDownModel{ text= GetMessageFromResource("spnElemco",LocalizeResourceSetConstants.Global), value="Elemco"},
                    new DropDownModel{ text=GetMessageFromResource("spnSensor",LocalizeResourceSetConstants.Global), value="Sensor"},
                    new DropDownModel{ text= GetMessageFromResource("spnPLC",LocalizeResourceSetConstants.Global), value="PLC"}
            };
            return stList;
        }
        internal static List<DropDownModel> EventType()
        {
            List<DropDownModel> stList = new List<DropDownModel>()
            {
                    new DropDownModel{ text=  GetMessageFromResource("spnWarning",LocalizeResourceSetConstants.Global), value="Warning"},
                    new DropDownModel{ text=  GetMessageFromResource("spNotification",LocalizeResourceSetConstants.Global), value="Notification"}
            };
            return stList;
        }
        internal static List<DropDownModel> EventDisposition()
        {
            List<DropDownModel> stList = new List<DropDownModel>()
            {
                    new DropDownModel{ text= GetMessageFromResource("spnAcknowledge",LocalizeResourceSetConstants.EventInfo), value="Acknowledge"},
                    new DropDownModel{ text= GetMessageFromResource("spnDismiss",LocalizeResourceSetConstants.EventInfo), value="Dismiss"},
                    new DropDownModel{ text=  GetMessageFromResource("GlobalWorkOrder", LocalizeResourceSetConstants.Global), value="WorkOrder"}
            };
            return stList;
        }

        #endregion

        #region Encription CriptoJS
        internal static string DecryptStringAES(string id, string key)
        {
            key = key.Replace(" ", "+");
            id = id.Replace(" ", "+");
            var keybytes = Encoding.UTF8.GetBytes(EncKeyVal);
            var iv = Encoding.UTF8.GetBytes(EncKeyVal);

            var encrypted = Convert.FromBase64String(key);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            string MainSalt = string.Format(decriptedFromJavascript);
            MainSalt = MainSalt.Substring(0, (MainSalt.Length - id.Length));
            string Requestid = UtilityFunction.DecryptStringAES2(id, MainSalt);
            return Requestid;
        }
        public static string DecryptStringAES2(string cipherText, string EncKeyValue)
        {

            var keybytes = Encoding.UTF8.GetBytes(EncKeyValue);
            var iv = Encoding.UTF8.GetBytes(EncKeyValue);
            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);

        }
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        #endregion

        internal static List<DropDownModel> GetMobileWOTimerList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnNoTimer",LocalizeResourceSetConstants.Global),value="NoTimer"},
                new DropDownModel{ text=GetMessageFromResource("spnUseTimer",LocalizeResourceSetConstants.Global),value="Timer"},
                new DropDownModel { text = GetMessageFromResource("spnUseTimerScan",LocalizeResourceSetConstants.Global), value = "Scan" }
             };
            return ddList;
        }
        internal static List<DropDownModel> GetPurchaseCostList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("LE100",LocalizeResourceSetConstants.PartManagementRequest),value="LE100"},
                new DropDownModel{ text=GetMessageFromResource("LE200",LocalizeResourceSetConstants.PartManagementRequest),value="LE200"},
                new DropDownModel{ text=GetMessageFromResource("LE500",LocalizeResourceSetConstants.PartManagementRequest),value="LE500"},
                new DropDownModel{ text=GetMessageFromResource("GT500",LocalizeResourceSetConstants.PartManagementRequest),value="GT500"}

             };
            return ddList;
        }

        internal static List<DropDownModel> GetPurchaseLeadTimeList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("FiveDays",LocalizeResourceSetConstants.PartManagementRequest),value="FiveDays"},
                new DropDownModel{ text=GetMessageFromResource("TwoWeeks",LocalizeResourceSetConstants.PartManagementRequest),value="TwoWeeks"},
                new DropDownModel{ text=GetMessageFromResource("Month",LocalizeResourceSetConstants.PartManagementRequest),value="Month"},
                new DropDownModel{ text=GetMessageFromResource("MonthPlus",LocalizeResourceSetConstants.PartManagementRequest),value="MonthPlus"}

             };
            return ddList;
        }

        internal static List<DropDownModel> GetPurchaseFrequencyList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("MonthFre",LocalizeResourceSetConstants.PartManagementRequest),value="Month"},
                new DropDownModel{ text=GetMessageFromResource("QuarterFre",LocalizeResourceSetConstants.PartManagementRequest),value="Quarter"},
                new DropDownModel{ text=GetMessageFromResource("SemiAnnualFre",LocalizeResourceSetConstants.PartManagementRequest),value="SemiAnnual"},
                new DropDownModel{ text=GetMessageFromResource("AnnualFre",LocalizeResourceSetConstants.PartManagementRequest),value="Annual"}

             };
            return ddList;
        }
        internal static List<DropDownModel> GetPartMasterRequestTypesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("Addition",LocalizeResourceSetConstants.PartManagementRequest),value="Addition"},
                new DropDownModel{ text=GetMessageFromResource("Replacement",LocalizeResourceSetConstants.PartManagementRequest),value="Replacement"},
                new DropDownModel{ text=GetMessageFromResource("Inactivation",LocalizeResourceSetConstants.PartManagementRequest),value="Inactivation"},
                new DropDownModel{ text=GetMessageFromResource("SX_Replacement",LocalizeResourceSetConstants.PartManagementRequest),value="SX_Replacement"},
            new DropDownModel { text = GetMessageFromResource("ECO_New",LocalizeResourceSetConstants.PartManagementRequest), value = "ECO_New" },
            new DropDownModel { text = GetMessageFromResource("ECO_Replace",LocalizeResourceSetConstants.PartManagementRequest), value = "ECO_Replace" },
            new DropDownModel { text = GetMessageFromResource("ECO_SX_Replace",LocalizeResourceSetConstants.PartManagementRequest), value = "ECO_SX_Replace" },
        };
            return ddList;
        }

        internal static List<DropDownModel> AssetCategoryList()
        {
            List<DropDownModel> AssetCategoryList = new List<DropDownModel>()
            {
                 new DropDownModel{ text=GetMessageFromResource("txtequipment",LocalizeResourceSetConstants.Global),value="Equipment"},
                 new DropDownModel{ text=GetMessageFromResource("spnLocation", LocalizeResourceSetConstants.Global),value="Location"},
                 new DropDownModel{ text=GetMessageFromResource("spnVehicle", LocalizeResourceSetConstants.EquipmentDetails),value="Vehicle"}
            };
            return AssetCategoryList;
        }
        internal static List<DropDownModel> PartViewSearchTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("globalActive",LocalizeResourceSetConstants.Global),value="1"},//text="Active"
                new DropDownModel{ text=GetMessageFromResource("globalInActive",LocalizeResourceSetConstants.Global),value="2"},//text="Inactive"
                new DropDownModel{ text=GetMessageFromResource("globalLowParts",LocalizeResourceSetConstants.Global),value="4"}
             };
            return ddList;
        }
        //V2-347
        internal static List<DropDownModel> GetTimeRangeDrop()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="10"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForWO()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="9"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="8"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="17"}
             };
            return ddList;
        }
        //V2-347
        //V2-364
        internal static List<DropDownModel> GetTimeRangeDropForPOCreateDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="11"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="12"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="13"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="14"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="15"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="16"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForWOCreateDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="18"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="19"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="20"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="21"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="22"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="23"}
             };
            return ddList;
        }
        //V2-364
        internal static List<DropDownModel> GetTimeRangeDropForReport()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="6"}
             };
            return ddList;
        }

        #region //Add on 23/06/2020 Create Contact Chips for mention user in comment widget
        //Add on 23/06/2020 Create Contact Chips for mention user in comment widget
        public static int IndexOfNth(this string str, string value, int nth = 0)
        {
            if (nth < 0)
                throw new ArgumentException("Can not find a negative index of substring in string. Must start with 0");

            int offset = str.IndexOf(value);
            for (int i = 0; i < nth; i++)
            {
                if (offset == -1) return -1;
                offset = str.IndexOf(value, offset + 1);
            }

            return offset;
        }
        public static string GetModifiedContactChipContent(List<Models.Common.UserMentionDataModel.UserMentionData> mentionDatas, string Content)
        {
            //01/07/2020
            string myorigialContent = Content;
            //01/07/2020
            string FinalContent = string.Empty;
            string ModifiedComments = string.Empty;
            List<Models.Common.UserMentionDataModel.UserMentionData> tempUserLists = new List<Models.Common.UserMentionDataModel.UserMentionData>();
            //02/07/2020
            var UserList = mentionDatas;
            foreach (var myitem in UserList)
            {
                if (Content.Contains(myitem.name))
                {
                    var nm = "@" + myitem.name;
                    var matches = Regex.Matches(Content, nm, RegexOptions.IgnoreCase);
                    if (matches.Count > 1)
                    {
                        for (int i = 0; i < matches.Count; i++)
                        {
                            tempUserLists.Add(myitem);
                        }
                    }
                    else
                    {
                        tempUserLists.Add(myitem);
                    }

                }
            }
            int k = Content.Split('@').Length - 1;
            //02/07/2020
            if (Content.Contains("@"))
            {
                string cmmtuserss = string.Empty;
                string cmtComments = string.Empty;
                //24/06/2020
                for (int myindex = 0; myindex < k; myindex++)
                {

                    int attheRateInmdex = UtilityFunction.IndexOfNth(Content, "@", myindex);
                    cmtComments = Content;
                    bool ismatched = false;
                    //02/07/2020
                    //foreach (var myitem in UserList)
                    foreach (var myitem in tempUserLists)
                    //02/07/2020
                    {

                        if (Content.Contains(myitem.name))
                        {
                            int mypos = Content.IndexOf(myitem.name);

                            int i = 0;
                            List<int> indexarr = new List<int>();
                            while ((i = Content.IndexOf(myitem.name, i)) != -1)
                            {
                                // Print out the index.
                                indexarr.Add(i);
                                //Console.WriteLine(i);
                                i++;
                            }

                            //if (mypos == attheRateInmdex + 1)
                            if (indexarr.Contains(attheRateInmdex + 1))
                            {
                                cmmtuserss = myitem.name;
                                string[] mycommentUsers1 = cmmtuserss.Split(' ');
                                string FName = mycommentUsers1[0].Substring(0, 1);
                                string LName = !String.IsNullOrEmpty(mycommentUsers1[1]) ? mycommentUsers1[1].Substring(0, 1) : "";
                                //string FullName = FName + ' ' + LName;
                                string FullName = ' ' + FName + LName;
                                int commentatthjeRateIndex = UtilityFunction.IndexOfNth(cmtComments, "@", myindex);
                                System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
                                sb1.Append("<span class='chip'>");
                                //sb1.Append("<img src='/Content/Images/img_avatar.png' alt='Person' width='96' height='96'>");
                                //sb1.Append("<span style='color:#F15009;float:left;background-color:#4685a3;margin:0 10px 0 -25px;height: 50px;width:50px; border-radius:50%;padding:0px 8px 0px 8px;'>" + FullName + "</span>");
                                sb1.Append("<span class='chiplikeimage'>" + FullName.ToUpper() + "</span>");
                                sb1.Append("@~" + cmmtuserss);
                                sb1.Append("<span></span>");
                                sb1.Append("</span>");
                                cmtComments = cmtComments.Remove(commentatthjeRateIndex, myitem.name.Length + 1).Insert(commentatthjeRateIndex, sb1.ToString());
                                Content = cmtComments;
                                ModifiedComments = cmtComments;

                            }
                            ModifiedComments = ModifiedComments.Replace("@~", "");
                            ismatched = true;
                        }

                    }
                    if (ismatched)
                    {
                        ModifiedComments = ModifiedComments.Replace("~", "");
                    }
                    else
                    {
                        ModifiedComments = myorigialContent;
                    }
                }

            }
            else
            {
                ModifiedComments = Content;
            }

            FinalContent = ModifiedComments;
            return FinalContent;
        }

        //Add on 23/06/2020 Create Contact Chips for mention user in comment widget


        //public static string GetModifiedNewContactChipContent(List<Models.Common.UserMentionDataModel.UserMentionData> mentionDatas, string Content)
        //{
        //    //01/07/2020
        //    string myorigialContent = Content;
        //    //01/07/2020
        //    string FinalContent = string.Empty;
        //    string ModifiedComments = string.Empty;
        //    List<Models.Common.UserMentionDataModel.UserMentionData> tempUserLists = new List<Models.Common.UserMentionDataModel.UserMentionData>();
        //    //02/07/2020
        //    var UserList = mentionDatas;
        //    foreach (var myitem in UserList)
        //    {
        //        if (Content.Contains(myitem.name))
        //        {
        //            tempUserLists.Add(myitem);
        //        }
        //    }
        //    int k = Content.Split('@').Length - 1;
        //    //02/07/2020
        //    if (Content.Contains("@"))
        //    {
        //        string cmmtuserss = string.Empty;
        //        string cmtComments = string.Empty;
        //        //24/06/2020
        //        for (int myindex = 0; myindex < k; myindex++)
        //        {

        //            int attheRateInmdex = UtilityFunction.IndexOfNth(Content, "@", myindex);
        //            cmtComments = Content;
        //            bool ismatched = false;
        //            //02/07/2020
        //            //foreach (var myitem in UserList)
        //            foreach (var myitem in tempUserLists)
        //            //02/07/2020
        //            {

        //                if (Content.Contains(myitem.name))
        //                {
        //                    int mypos = Content.IndexOf(myitem.name);

        //                    if (mypos == attheRateInmdex + 1)
        //                    {
        //                        cmmtuserss = myitem.name;
        //                        string[] mycommentUsers1 = cmmtuserss.Split(' ');
        //                        string FName = mycommentUsers1[0].Substring(0, 1);
        //                        string LName = mycommentUsers1[1].Substring(0, 1);
        //                        //string FullName = FName + ' ' + LName;
        //                        string FullName = ' ' + FName + LName;
        //                        int commentatthjeRateIndex = UtilityFunction.IndexOfNth(cmtComments, "@", myindex);
        //                        System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
        //                        sb1.Append("<span class='chip'>");
        //                        //sb1.Append("<img src='/Content/Images/img_avatar.png' alt='Person' width='96' height='96'>");
        //                        //sb1.Append("<span style='color:#F15009;float:left;background-color:#4685a3;margin:0 10px 0 -25px;height: 50px;width:50px; border-radius:50%;padding:0px 8px 0px 8px;'>" + FullName + "</span>");
        //                        sb1.Append("<span class='chiplikeimage'>" + FullName.ToUpper() + "</span>");
        //                        sb1.Append("@~" + cmmtuserss);
        //                        sb1.Append("<span></span>");
        //                        sb1.Append("</span>");
        //                        cmtComments = cmtComments.Remove(commentatthjeRateIndex, myitem.name.Length + 1).Insert(commentatthjeRateIndex, sb1.ToString());
        //                        Content = cmtComments;
        //                        ModifiedComments = cmtComments;

        //                    }
        //                    ModifiedComments = ModifiedComments.Replace("@~", "");
        //                    ismatched = true;
        //                }

        //            }
        //            if (ismatched)
        //            {
        //                ModifiedComments = ModifiedComments.Replace("~", "");
        //            }
        //            else
        //            {
        //                ModifiedComments = myorigialContent;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        ModifiedComments = Content;
        //    }

        //    FinalContent = ModifiedComments;
        //    return FinalContent;
        //}
        #endregion

        #region //Add on 23/06/2020 Modifications to Views in Invoice Matching V2-373
        //Add on 23/06/2020 Modifications to Views in Invoice Matching V2-373
        internal static List<DropDownModel> GetTimeRangeDropFORIHMATP()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="10"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropFORIHMPAID()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="11"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="12"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="13"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="14"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="15"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="16"}
             };
            return ddList;
        }
        //Add on 23/06/2020 Modifications to Views in Invoice Matching V2-373
        #endregion

        #region Get Constant Value
        internal static List<DropDownModel> PopulateSourceTypeList()
        {
            List<DropDownModel> conList = new List<DropDownModel>()
            {
                    new DropDownModel{ text= WorkOrderSourceTypes.PreventiveMaint, value=WorkOrderSourceTypes.PreventiveMaint},
                    new DropDownModel{ text= WorkOrderSourceTypes.OnDemand, value=WorkOrderSourceTypes.OnDemand},
                    new DropDownModel{ text= WorkOrderSourceTypes.Corrective, value=WorkOrderSourceTypes.Corrective},
                    new DropDownModel{ text= WorkOrderSourceTypes.Emergency, value=WorkOrderSourceTypes.Emergency},
                    new DropDownModel{ text= WorkOrderSourceTypes.WorkRequest,  value=WorkOrderSourceTypes.WorkRequest},
                    new DropDownModel{ text= WorkOrderSourceTypes.SanitationRequest, value=WorkOrderSourceTypes.SanitationRequest},
                    new DropDownModel{ text= WorkOrderSourceTypes.FollowUp,value= WorkOrderSourceTypes.FollowUp},
                    new DropDownModel{ text= WorkOrderSourceTypes.External,value= WorkOrderSourceTypes.External}

            };
            return conList;
        }
        #endregion

        //V2-523
        internal static List<DropDownModel> GetTimeRangeDropForPR()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //// RKL - 2020-Aug-05 - begin
                //new DropDownModel{ text=GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="18"},
                //new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="19"},
                //new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="20"},
                //new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="21"},
                //new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="22"},
                //// RKL - 2020-Aug-05 - End
                //new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="23"},
                 new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="24"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="25"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="26"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="27"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="28"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="29"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetTimeRangeDropForCancelPR()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                 new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="30"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="31"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="32"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="33"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="34"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="35"}
             };
            return ddList;
        }
        //V2-523
        internal static List<DropDownModel> GetTimeRangeDropForAllStatusPR()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="18"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="19"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="20"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="21"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="22"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="23"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForAllStatusFI()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="8"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetTimeRangeDropForAllStatusFS()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="11"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="12"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="13"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="14"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="15"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="16"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetTimeRangeDropForSO()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="8"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="9"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="10"}
             };
            return ddList;
        }
        //V2-398

        //internal static List<DropDownModel> GetTimeRangeDropForAllStatusSJ()
        //{
        //    List<DropDownModel> ddList = new List<DropDownModel>
        //    {
        //        new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="11"},
        //        new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="12"},
        //        new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="13"},
        //        new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="14"},
        //        new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="15"},
        //        new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="16"}
        //     };
        //    return ddList;
        //}
        internal static List<DropDownModel> GetTimeRangeDropForCompletedSJ()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="17"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="18"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="19"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="20"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="21"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="22"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForFailedSJ()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="23"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="24"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="25"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="26"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="27"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="28"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForPassedSJ()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="29"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="30"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="31"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="32"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="33"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="34"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForInvoiceMatching()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="17"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="18"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="19"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="20"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="21"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="22"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetTimeRangeDropForConfigDashboard()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="0"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="2"},
             };
            return ddList;
        }

        //V2-676
        internal static List<DropDownModel> GetWOBarCodeList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnNoBarcode",LocalizeResourceSetConstants.Global),value="NoBarcode"},
                new DropDownModel{ text=GetMessageFromResource("spnBarcode",LocalizeResourceSetConstants.Global),value="Barcode"},
                new DropDownModel { text = GetMessageFromResource("spnQRCode",LocalizeResourceSetConstants.Global), value = "QRCode" }
             };
            return ddList;
        }
        //V2-398

        #region System Unavailable Message
        public static string GetAMPMWithSpace(string Time)
        {
            string ModifiedTime = string.Empty;
            if (Time.Contains("AM") || Time.Contains("am"))
            {
                ModifiedTime = Time.ToUpper().Replace("AM", " AM");
            }
            else if (Time.Contains("PM") || Time.Contains("pm"))
            {
                ModifiedTime = Time.ToUpper().Replace("PM", " PM");
            }
            else
            {
                ModifiedTime = Time.ToUpper();
            }
            return ModifiedTime;
        }
        #endregion

        #region Fleet Only
        public static List<DropDownModel> SoLaborHoursDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="30"}
             };
            return ddList;
        }
        #endregion

        #region Report
        public static object GetFormattedData(object data, GridColumnsProp column)
        {
            decimal value = 0;
            if (column.NumericFormat == null)
            {
                column.NumericFormat = "";
            }
            if (column.NumericFormat.ToUpper() == ReportConstants.PERCENTAGE)
            {
                int i = 0;
            }
            if (!string.IsNullOrEmpty(column.NumericFormat) && (column.NumericFormat.ToUpper() == ReportConstants.PLAIN || column.NumericFormat.ToUpper() == ReportConstants.NUMBER
                || column.NumericFormat.ToUpper() == ReportConstants.CURRENCY || column.NumericFormat.ToUpper() == ReportConstants.PERCENTAGE))
            {
                value = Convert.ToDecimal(data);
            }
            else
            {
                return data;
            }
            string format = column.NumericFormat.ToUpper();
            decimal formattedValue = 0;
            string display = string.Empty;
            if (format == ReportConstants.PLAIN)
            {
                if (column.NumofDecPlaces > 0)
                {
                    formattedValue = TrunCateNumber(value, column.NumofDecPlaces);
                }
                else
                {
                    formattedValue = value;
                }
                display = formattedValue.ToString();
            }
            if (format == ReportConstants.NUMBER)
            {
                if (column.NumofDecPlaces > 0)
                {
                    formattedValue = TrunCateNumber(value, column.NumofDecPlaces);
                }
                else
                {
                    formattedValue = value;
                }
                display = string.Format("{0:n" + column.NumofDecPlaces + "}", formattedValue);
            }
            if (format == ReportConstants.CURRENCY)
            {
                NumberFormatInfo nfi = new CultureInfo(column.SiteLocalization, false).NumberFormat;
                if (column.NumofDecPlaces > 0)
                {
                    nfi.CurrencyDecimalDigits = column.NumofDecPlaces;
                }
                else
                {
                    nfi.CurrencyDecimalDigits = 0;
                }
                display = value.ToString("C", nfi);

            }
            if (format == ReportConstants.PERCENTAGE)
            {
                NumberFormatInfo nfi = new CultureInfo(column.SiteLocalization, false).NumberFormat;
                if (column.NumofDecPlaces > 0)
                {
                    nfi.PercentDecimalDigits = column.NumofDecPlaces;
                }
                if (column.NumofDecPlaces > 0)
                {
                    formattedValue = TrunCateNumber(value, column.NumofDecPlaces);
                }
                else
                {
                    formattedValue = value;
                }
                display = formattedValue.ToString("P", nfi);
            }

            return display;
        }
        public static decimal TrunCateNumber(decimal Number, int demalplaces = 3)
        {
            decimal step = (decimal)Math.Pow(10, demalplaces);
            decimal tmp = Math.Truncate(step * Number);
            return tmp / step;
        }
        #endregion

        #region Enterprise User
        public static List<DropDownModel> EnterpriseUserDatesList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text="Select Date Range",value=""},
                new DropDownModel{ text=GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="30"},
                new DropDownModel{ text=GetMessageFromResource("spnLastNinetyDays",LocalizeResourceSetConstants.Global),value="90"}

             };
            return ddList;
        }
        #endregion

        #region Labor Scheduling V2-524
        internal static List<DropDownModel> GetTimeRangeDropForScheduledDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //524 Localization
                //V2-562 Localization
                new DropDownModel{ text = GetMessageFromResource("spnLastWeek",LocalizeResourceSetConstants.Global),value="0"},
                new DropDownModel{ text = GetMessageFromResource("spnLastMonth",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text = GetMessageFromResource("GlobalThisWeek",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text = GetMessageFromResource("GlobalThisMonth",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text = GetMessageFromResource("GlobalNextWeek",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text = GetMessageFromResource("GlobalNextMonth",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text = GetMessageFromResource("GlobalNext90Days",LocalizeResourceSetConstants.Global),value="6"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetGroupingDataForLaborSchedulling()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //524 Localization
                new DropDownModel{ text = "Assigned",value="0"},
                new DropDownModel{ text = "Schedule Date",value="1"},
             };
            return ddList;
        }


        #endregion

        #region Project V2-594
        internal static List<DropDownModel> GetTimeRangeDropForAllStatusProj()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="6"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForCompletedProj()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="9"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="10"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="11"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="12"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="13"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="14"}
             };
            return ddList;
        }

        internal static List<DropDownModel> GetTimeRangeDropForClosedProj()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="15"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSevenDays",LocalizeResourceSetConstants.Global),value="16"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="17"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="18"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="19"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="20"}
             };
            return ddList;
        }
        #endregion

        #region Maintenance Completion Workbench
        public static List<DropDownModel> GetGroupingDataForCompletionWorkbench()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text = GetMessageFromResource("spnOpenAndAssignedToMe",LocalizeResourceSetConstants.Global), value="0"},
                new DropDownModel{ text = GetMessageFromResource("spnMyScheduledAndCompletedLastSevenDays",LocalizeResourceSetConstants.Global), value="1"},
                new DropDownModel{ text = GetMessageFromResource("spnMyScheduledAndCompletedLastThirtyDays",LocalizeResourceSetConstants.Global), value="2"},
                new DropDownModel{ text = GetMessageFromResource("spnOpenUnassigned",LocalizeResourceSetConstants.Global), value="3"},
             };
            return ddList;
        }

        #endregion

        public static List<string> FindItemsInListANotPresentInListB(List<string> ListA, List<string> ListB)
        {
            var ListC = ListA.Except(ListB);
            return ListC.ToList();
        }
        // RKL - 2023-Mar-15
        // V2-
        public static string OnPremiseAttachmentURL(string fileNameandPath)
        {
            string UploadFileOnPremiseFilePath = string.Empty;
            var OnPremisePath = GetOnPremiseDirectory();
            string AttachURL = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, fileNameandPath);
            return AttachURL;
        }
        public static string PhotoBase64ImgSrc(string fileNameandPath)
        {
            string UploadFileOnPremiseFilePath = string.Empty;
            int ConnectRemoteShareErrorCode = 0;
            bool fileexists = false;

            var OnPremisePath = GetOnPremiseDirectory();

            fileNameandPath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, fileNameandPath);

            NetworkCredential credentials = GetOnPremiseCredential();

            using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
            {
                if (ConnectRemoteShareErrorCode == 0)
                {
                    fileexists = File.Exists(fileNameandPath);
                    if (!fileexists)
                    { return string.Empty; }
                    else
                    {
                        byte[] byteArray = File.ReadAllBytes(fileNameandPath);
                        string base64 = Convert.ToBase64String(byteArray);

                        return string.Format("data:image/gif;base64,{0}", base64);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static string Base64SrcDevexpress(string fileNameandPath)
        {
            string UploadFileOnPremiseFilePath = string.Empty;
            int ConnectRemoteShareErrorCode = 0;
            bool fileexists = false;

            var OnPremisePath = GetOnPremiseDirectory();

            fileNameandPath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath, fileNameandPath);

            NetworkCredential credentials = GetOnPremiseCredential();

            using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
            {
                if (ConnectRemoteShareErrorCode == 0)
                {
                    fileexists = File.Exists(fileNameandPath);
                    if (!fileexists)
                    { return string.Empty; }
                    else
                    {
                        byte[] byteArray = File.ReadAllBytes(fileNameandPath);
                        string base64 = Convert.ToBase64String(byteArray);

                        return string.Format("{0}", base64);
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static OnPremiseDirectoryModel GetOnPremiseDirectory()
        {
            string RemoteDrivePath = ConfigurationManager.AppSettings["UploadFileOnPremiseDirectory"];
            string NetworkPath = ConfigurationManager.AppSettings["RemoteDrivePath"];

            OnPremiseDirectoryModel onPremiseDirectoryModel = new OnPremiseDirectoryModel();
            onPremiseDirectoryModel.RemoteDrivePath = string.IsNullOrEmpty(RemoteDrivePath) ? string.Empty : RemoteDrivePath.Trim();
            onPremiseDirectoryModel.NetworkPath = string.IsNullOrEmpty(NetworkPath) ? string.Empty : NetworkPath.Trim();

            return onPremiseDirectoryModel;

        }
        public static NetworkCredential GetOnPremiseCredential()
        {
            string userNameFromConfig = ConfigurationManager.AppSettings["RUserName"];
            string passwordFromConfig = ConfigurationManager.AppSettings["RUserPassword"];

            string UserName = string.IsNullOrEmpty(userNameFromConfig) ? string.Empty : userNameFromConfig.Trim();
            string UserPassword = string.IsNullOrEmpty(passwordFromConfig) ? string.Empty : passwordFromConfig.Trim();

            return new NetworkCredential(@UserName, UserPassword);
        }
        #region V2-637 Repairable Spare Assets
        public static List<DropDownModel> RepairableSpareAssignmentStatusList()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("spnGlobalAssigned",LocalizeResourceSetConstants.Global),value="Assigned"},
                new DropDownModel{ text=GetMessageFromResource("globalUnassigned",LocalizeResourceSetConstants.Global),value="Unassigned"},
             };
            return ddList;
        }
        #endregion

        public static string ReturnPropertyNameWithoutCaseComparison<T>(string ColumnName, T model)
        {
            string propertyName = model.GetType().GetProperties().ToList().Select(x => x.Name).ToList()
                                                .Where(x => x.ToLower() == ColumnName.Trim().ToLower()).FirstOrDefault();
            return propertyName ?? "";
        }

        #region QR Code 
        public static string QrGenerate(string value)
        {
            string QRCodeImagePath = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(value, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);

                using (Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, ImageFormat.Png);
                    QRCodeImagePath = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
            return QRCodeImagePath;
        }
        #endregion

        #region Approval Group Setup
        internal static List<DropDownModel> RequestTypeList()
        {
            List<DropDownModel> AssetCategoryList = new List<DropDownModel>()
            {
                new DropDownModel{ text=GetMessageFromResource("spnMaterialRequest",LocalizeResourceSetConstants.Global),value="MaterialReq"},
                new DropDownModel{ text=GetMessageFromResource("spnPurchaseRequest", LocalizeResourceSetConstants.Global),value="PurchaseReq"},
                new DropDownModel{ text=GetMessageFromResource("spnSanitationRequest",LocalizeResourceSetConstants.SanitationDetails),value="SanitationReq"},
                new DropDownModel{ text=GetMessageFromResource("spnWorkRequest", LocalizeResourceSetConstants.Global),value="WorkReq"},
            };
            return AssetCategoryList;
        }
        #endregion

        #region Approval V2-769
        internal static List<DropDownModel> FilterTypeListForPR()
        {
            List<DropDownModel> FilterTypeList = new List<DropDownModel>()
            {
                new DropDownModel{ text=GetMessageFromResource("spnAssignedToMe",LocalizeResourceSetConstants.Global),value="AssignedToMe"},
            };
            return FilterTypeList;
        }
        internal static List<DropDownModel> FilterTypeListForWR()
        {
            List<DropDownModel> FilterTypeList = new List<DropDownModel>()
            {
                new DropDownModel{ text=GetMessageFromResource("spnAssignedToMe",LocalizeResourceSetConstants.Global),value="AssignedToMe"},
            };
            return FilterTypeList;
        }
        internal static List<DropDownModel> FilterTypeListForMR()
        {
            List<DropDownModel> FilterTypeList = new List<DropDownModel>()
            {
                new DropDownModel{ text=GetMessageFromResource("spnAssignedToMe",LocalizeResourceSetConstants.Global),value="AssignedToMe"},
            };
            return FilterTypeList;
        }
        #endregion

        #region V2-823 BBU KPI Enterprise
        internal static List<DropDownModel> GetTimeRangeDropForBBUKPIEnterpriseCreateDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastHundredEightyDays",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="12"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetTimeRangeDropForBBUKPIEnterpriseSubmitDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="8"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="9"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="10"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastHundredEightyDays",LocalizeResourceSetConstants.Global),value="11"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="13"}
             };
            return ddList;
        }
        #endregion

        #region V2-823 BBU KPI Site
        internal static List<DropDownModel> GetTimeRangeDropForBBUKPISiteCreateDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="1"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="4"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="5"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="6"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastHundredEightyDays",LocalizeResourceSetConstants.Global),value="7"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="12"}
             };
            return ddList;
        }
        internal static List<DropDownModel> GetTimeRangeDropForBBUKPISiteSubmitDate()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                //new DropDownModel{ text= GetMessageFromResource("spnToday",LocalizeResourceSetConstants.Global),value="3"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastThirtyDays",LocalizeResourceSetConstants.Global),value="8"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastSixtyDays",LocalizeResourceSetConstants.Global),value="9"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastNinetyDays",LocalizeResourceSetConstants.Global),value="10"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalLastHundredEightyDays",LocalizeResourceSetConstants.Global),value="11"},
                new DropDownModel{ text=GetMessageFromResource("spnGlobalSelectTimePeriod",LocalizeResourceSetConstants.Global),value="13"}
             };
            return ddList;
        }
        #endregion

        #region V2-892
        internal static List<DropDownModel> DownRequiredStatusTypesWithBoolValue()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("globalYes",LocalizeResourceSetConstants.Global),value="true"},
                new DropDownModel{ text=GetMessageFromResource("globalNo",LocalizeResourceSetConstants.Global),value="false"}
             };
            return ddList;
        }
        #endregion

        //#region V2-910
        //internal static List<DropDownModel> SanitationViewSearchTypes()
        //{
        //    List<DropDownModel> ddList = new List<DropDownModel>
        //    {
        //        new DropDownModel{ text= GetMessageFromResource("globalAllStatUses",LocalizeResourceSetConstants.Global),value="1"},//text="All statuses"
        //        new DropDownModel{ text=GetMessageFromResource("GlobalPassed",LocalizeResourceSetConstants.Global),value="2"},//text="Passed"
        //        new DropDownModel{ text=GetMessageFromResource("GlobalFailed",LocalizeResourceSetConstants.Global),value="3"},//text="Failed"
        //        new DropDownModel{ text=GetMessageFromResource("spnGlobalCompleted",LocalizeResourceSetConstants.Global),value="4"} //text="Completed"
        //     };
        //    return ddList;
        //}
        //#endregion

        #region V2-905
        internal static List<DropDownModel> UserTypesListValue()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=/*GetMessageFromResource("globalYes",LocalizeResourceSetConstants.Global)*/ "Full",value="Full"},
                new DropDownModel{ text=/*GetMessageFromResource("globalNo",LocalizeResourceSetConstants.Global)*/"Admin",value="Admin"},
                new DropDownModel{ text=/*GetMessageFromResource("globalYes",LocalizeResourceSetConstants.Global)*/"WorkRequest",value="WorkRequest"},
                new DropDownModel{ text=/*GetMessageFromResource("globalNo",LocalizeResourceSetConstants.Global)*/"Enterprise",value="Enterprise"},
                new DropDownModel{ text=/*GetMessageFromResource("globalNo",LocalizeResourceSetConstants.Global)*/"Reference",value="Reference"}
             };
            return ddList;
        }
        internal static List<DropDownModel> IsActiveStatusWithBoolValue()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text=GetMessageFromResource("globalYes",LocalizeResourceSetConstants.Global),value="true"},
                new DropDownModel{ text=GetMessageFromResource("globalNo",LocalizeResourceSetConstants.Global),value="false"}
             };
            return ddList;
        }
        #endregion

        #region V2-536
        internal static List<DropDownModel> GetDeviceCategory()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("Meter",LocalizeResourceSetConstants.Global),value="Meter"},
                new DropDownModel { text = GetMessageFromResource("MonnitSensor",LocalizeResourceSetConstants.Global), value = "MonnitSensor" },
                new DropDownModel { text = GetMessageFromResource("spnSensor",LocalizeResourceSetConstants.Global), value = "Sensor" }
             };
            return ddList;
        }
        #endregion
                #region V2-540
        internal static List<DropDownModel> AllIoTEventOpenProcessedStatusTypes()
        {
            List<DropDownModel> ddList = new List<DropDownModel>
            {
                new DropDownModel{ text= GetMessageFromResource("spnAllEvents",LocalizeResourceSetConstants.IoTEvent),value="1"},
                new DropDownModel{ text=GetMessageFromResource("GlobalOpen",LocalizeResourceSetConstants.Global),value="2"},
                new DropDownModel{ text=GetMessageFromResource("spnProcessed",LocalizeResourceSetConstants.IoTEvent),value="3"}
             };
            return ddList;
        }
        #endregion
        #region V2-1087 ProjectCategory
        internal static List<DropDownModel> ProjectCategoryList()
        {
            List<DropDownModel> ProjectCategoryList = new List<DropDownModel>()
            {
                 new DropDownModel{ text= ProjectCategoryConstants.Project, value=ProjectCategoryConstants.Project},
                    new DropDownModel{ text= ProjectCategoryConstants.Release, value=ProjectCategoryConstants.Release},
                    new DropDownModel{ text= ProjectCategoryConstants.Master, value=ProjectCategoryConstants.Master}
            };
            return ProjectCategoryList;
        }
        #endregion
    }
}