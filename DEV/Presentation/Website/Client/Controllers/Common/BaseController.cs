using AzureUtil;
using Business.Authentication;
using Client.ActionFilters;
using Client.BusinessWrapper;
using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Models;
using Client.Models.Common;
using Client.Models.PartsManagement.PartsManagementRequest;
using Common.Constants;
using Common.Enumerations;
using Common.Extensions;
using Database;
using Database.Business;
using DataContracts;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utility;
using System.Globalization;
using static Client.Models.Common.UserMentionDataModel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Location = DataContracts.Location;
using System.Net;
using QRCoder;
using Client.BusinessWrapper.Work_Order;
using INTDataLayer.EL;
using Client.Models.Meters;
using Client.BusinessWrapper.Configuration.ClientSetUp;
using Client.Models.ClientMessages;
using DevExpress.Internal;
using Client.Models.SensorAlert;

namespace Client.Controllers
{
    [CustomAuthorize(Roles = new string[] { "*" })]
    [SessionExpired]
    [NoCacheActionFilter]
    [SiteUnderMaintennce]
    public class BaseController : Controller
    {
        #region Initialization
        internal DataContracts.UserData userData { get; set; }
        internal long objectId { get; set; }
        protected JsonSerializerSettings JsonSerializerDateSettings { get; set; }
        protected JsonSerializerSettings JsonSerializer24HoursDateAndTimeSettings { get; set; }
        protected JsonSerializerSettings JsonSerializer12HoursDateAndTimeSettings { get; set; }
        protected JsonSerializerSettings JsonSerializer12HoursDateAndTimeUptoMinuteSettings { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var SessionData = Session["userData"];
            if (SessionData != null)
            {
                userData = (DataContracts.UserData)Session["userData"];
            }
            if (Session[SessionConstants.OBJECT_ID] != null)
            {
                objectId = Convert.ToInt64(Session[SessionConstants.OBJECT_ID]);
                Session[SessionConstants.OBJECT_ID] = null;
            }
            else
            {
                objectId = objectId;
            }

            this.JsonSerializerDateSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy"
            };

            this.JsonSerializer24HoursDateAndTimeSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy HH:mm:ss"
            };

            this.JsonSerializer12HoursDateAndTimeSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy hh:mm:ss tt"
            };
            this.JsonSerializer12HoursDateAndTimeUptoMinuteSettings = new JsonSerializerSettings
            {
                DateFormatString = "MM/dd/yyyy hh:mm tt"
            };

        }
        #endregion

        #region LookupList
        public List<DataModel> GetLookUpList_Parts()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Part_RetrieveAll_V2 trans = new Part_RetrieveAll_V2()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };

            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            foreach (b_Part p in trans.PartList)
            {
                dModel = new DataModel();
                Part part = new Part();
                part.UpdateFromDatabaseObject(p);
                dModel.PartId = part.PartId;
                dModel.Part = part.ClientLookupId;
                dModel.Name = part.Description;
                model.data.Add(dModel);
            }
            return model.data;
        }
        public List<DataModel> GetLookupList_Account()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            Account_RetrieveAll_V2 trans = new Account_RetrieveAll_V2()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,

            };
            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            foreach (b_Account a in trans.AccountList)
            {
                dModel = new DataModel();
                Account account = new Account();
                account.UpdateFromDatabaseObject(a);
                dModel.AccountId = account.AccountId;
                dModel.Account = account.ClientLookupId;
                dModel.Name = account.Name;
                dModel.InactiveFlag = account.InactiveFlag;
                model.data.Add(dModel);
            }
            return model.data;
        }
        //V2-379:To get active list only
        public List<DataModel> GetAccountByActiveState(bool State)
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            Account_RetrieveByActiveState_V2 trans = new Account_RetrieveByActiveState_V2()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
                IsActive = State
            };
            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            foreach (b_Account a in trans.AccountList)
            {
                dModel = new DataModel();
                Account account = new Account();
                account.UpdateFromDatabaseObject(a);
                dModel.AccountId = account.AccountId;
                dModel.Account = account.ClientLookupId;
                dModel.Name = account.Name;
                dModel.InactiveFlag = account.InactiveFlag;
                model.data.Add(dModel);
            }
            return model.data;
        }
        public List<DataModel> GetLookupList_Vendor()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            Vendor_RetrieveAll_V2 trans = new Vendor_RetrieveAll_V2
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };
            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            if (trans != null && trans.VendorList != null)
            {
                var ActiveVendors = trans.VendorList.Where(x => x.InactiveFlag == false);
                foreach (b_Vendor v in ActiveVendors)
                {
                    dModel = new DataModel();
                    Vendor vendor = new Vendor();
                    vendor.UpdateFromDatabaseObject(v);
                    dModel.VendorId = vendor.VendorId;
                    dModel.Vendor = vendor.ClientLookupId;
                    dModel.Name = vendor.Name;
                    model.data.Add(dModel);
                }
            }

            return model.data;

        }
        public List<DataModel> RetrieveForLookupVendor()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            Vendor vendor_retrieve = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };

            List<Vendor> vendorList = vendor_retrieve.Vendor_RetrieveForLookup(userData.DatabaseKey);

            if (vendorList != null && vendorList != null)
            {
                foreach (var v in vendorList)
                {
                    dModel = new DataModel();
                    dModel.VendorId = v.VendorId;
                    dModel.Vendor = v.ClientLookupId;
                    dModel.Name = v.Name;
                    model.data.Add(dModel);
                }
            }

            return model.data;

        }
        public List<DataModel> ListofAllEquipment()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            List<Equipment> equipList = new Equipment().GetAllEquipment(this.userData.DatabaseKey);

            if (equipList != null && equipList != null)
            {
                foreach (var v in equipList)
                {
                    dModel = new DataModel();
                    dModel.EquipmentId = v.EquipmentId;
                    dModel.Equipment = v.ClientLookupId;
                    dModel.Name = v.Name;
                    model.data.Add(dModel);
                }
            }

            return model.data;

        }
        public List<DataModel> ListOfLocation()
        {

            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            List<Location> locList = new Location().RetrieveClientLookupIdBySearchCriteriaV2(this.userData);
            if (locList != null && locList != null)
            {
                foreach (var v in locList)
                {
                    dModel = new DataModel();
                    dModel.LocationId = v.LocationId;
                    dModel.LocationClientLookupId = v.ClientLookupId;
                    dModel.Name = v.Name;
                    model.data.Add(dModel);
                }
            }

            return model.data;
        }
        public List<DataModel> GetLookUpList_Equipment()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Equipment_RetrieveAll_V2 trans = new Equipment_RetrieveAll_V2()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };

            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();
            if (trans != null && trans.EquipmentList != null)
            {
                var ActiveEquipments = trans.EquipmentList.Where(x => x.InactiveFlag == false);
                foreach (b_Equipment e in ActiveEquipments)
                {
                    dModel = new DataModel();
                    Equipment equipment = new Equipment();
                    equipment.UpdateFromDatabaseObject(e);
                    dModel.EquipmentId = equipment.EquipmentId;
                    dModel.Equipment = equipment.ClientLookupId;
                    dModel.Name = equipment.Name;
                    model.data.Add(dModel);
                }
            }
            return model.data;
        }
        public List<DataModel> GetList_Personnel()
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupList(this.userData.DatabaseKey);

            return model.data = ReturnPersonnelList(PersonnelList);
        }
        public List<DataModel> GetList_PersonnelV2()
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupListV2(this.userData.DatabaseKey);

            return model.data = ReturnPersonnelList(PersonnelList);
        }

        public List<Personnel> GetList_PalnnerPersonnel()
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveForPersonalPlannerLookupList(this.userData.DatabaseKey);
            return PersonnelList;
        }

        public List<DataModel> GetLookUpList_Personnel()
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupList(this.userData.DatabaseKey);
            personnel.LookUpFilterName = UserTypeConstants.Buyer;
            if (!string.IsNullOrEmpty(personnel.LookUpFilterName))
            {
                switch (personnel.LookUpFilterName)
                {
                    case "Buyer":
                        PersonnelList = PersonnelList.Where(x => x.Buyer == true).ToList();
                        break;
                    default:
                        break;
                }
            }
            return model.data = ReturnPersonnelList(PersonnelList);
        }
        // V2-478 - RKL 
        // Provide parameters
        /// <summary>
        /// Parameters:
        /// secitems - security items separated with a hashtag (#)
        /// secprops - security properties (i.e. ItemAccess, ItemCreate, ItemEdit, ItemDelete) 
        ///            separated with a hastag.  There MUST be a 1 to 1 correspondence with the 
        ///            security items 
        /// Exmples:
        /// secitems: 'Equipment#Part'
        /// secprops: 'ItemAccess#ItemDelete'
        /// This means the personnel list retrieved will contain all users for the client and site that have equipment - item access 
        /// or Part - Item Delete security access.  
        /// </summary>
        /// <returns></returns>
        public List<DataModel> Get_PersonnelList(string secitems, string secprops)
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId,
                SecItems = secitems,
                SecProps = secprops
                //ItemAccess = true,
                //ItemName = "PurchaseRequest-Approve"
            };

            List<Personnel> PersonnelList = personnel.RetrieveForLookupListBySecurityItem(this.userData.DatabaseKey);

            return model.data = ReturnPersonnelList(PersonnelList);
        }
        private List<DataModel> ReturnPersonnelList(List<Personnel> PersonnelList)
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            foreach (var p in PersonnelList)
            {
                dModel = new DataModel();

                dModel.AssignedTo_PersonnelId = p.PersonnelId;
                dModel.AssignedTo_PersonnelClientLookupId = p.ClientLookupId;
                dModel.NameFirst = p.NameFirst;
                dModel.NameLast = p.NameLast;

                model.data.Add(dModel);
            }
            return model.data;
        }
        public List<DataModel> GetLookUpList_WorkOrder()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            WorkOrder_RetrieveAll_V2 trans = new WorkOrder_RetrieveAll_V2()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };
            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();
            foreach (b_WorkOrder w in trans.WorkOrderList)
            {
                dModel = new DataModel();
                WorkOrder wo = new WorkOrder();
                wo.UpdateFromDatabaseObject(w);

                dModel.WorkOderId = wo.WorkOrderId;
                dModel.WorkOder = wo.ClientLookupId;
                dModel.Description = wo.Description;
                dModel.Status = wo.Status;
                model.data.Add(dModel);
            }
            return model.data;
        }
        public List<DataModel> GetLookUpList_Meter()
        {
            LookUpListModel model = new LookUpListModel();
            List<Meter> meterList = new List<Meter>();
            DataModel dModel;
            Meter meter = new Meter()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            meterList = meter.RetrieveForSearchBySiteAndReadingDate_V2(this.userData.DatabaseKey);

            foreach (var m in meterList)
            {
                dModel = new DataModel();
                dModel.MeterId = m.MeterId;
                dModel.Meter_ClientLookupId = m.ClientLookupId;
                dModel.ReadingCurrent = m.ReadingCurrent;
                dModel.Name = m.Name;
                model.data.Add(dModel);
            }
            return model.data;
        }
        public List<DataModel> GetLookUpList_Craft()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            List<Craft> CraftList = new Craft().RetriveAllForSite(userData.DatabaseKey).Where(a => a.InactiveFlag == false).ToList();
            foreach (var c in CraftList)
            {
                dModel = new DataModel();

                dModel.CraftId = c.CraftId;
                dModel.ClientLookUpId = c.ClientLookupId;
                dModel.Description = c.Description;
                dModel.ChargeRate = c.ChargeRate;
                model.data.Add(dModel);
            }
            return model.data;

        }
        public List<DataModel> PopulatelookUpListByType(string _type = "")
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            if (_type == "Equipment")
            {
                Equipment_RetrieveAll_V2 trans = new Equipment_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();
                if (trans != null && trans.EquipmentList != null)
                {
                    var ActiveEquipmentList = trans.EquipmentList.Where(x => x.InactiveFlag == false);
                    foreach (b_Equipment e in ActiveEquipmentList)
                    {
                        dModel = new DataModel();
                        Equipment equipment = new Equipment();
                        equipment.UpdateFromDatabaseObject(e);
                        dModel.ChargeToId = equipment.EquipmentId;
                        dModel.ChargeToClientLookupId = equipment.ClientLookupId;
                        dModel.Name = equipment.Name;
                        model.data.Add(dModel);
                    }
                }
            }
            if (_type == "WorkOrder")
            {
                WorkOrder_RetrieveAll_V2 trans = new WorkOrder_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };
                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();
                foreach (b_WorkOrder w in trans.WorkOrderList)
                {
                    dModel = new DataModel();
                    WorkOrder wo = new WorkOrder();
                    wo.UpdateFromDatabaseObject(w);

                    dModel.ChargeToId = wo.WorkOrderId;
                    dModel.ChargeToClientLookupId = wo.ClientLookupId;
                    dModel.Description = wo.Description;
                    dModel.Status = wo.Status;
                    model.data.Add(dModel);
                }
            }
            else if (_type == "Location")
            {
                Location_RetrieveAll_V2 trans = new Location_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                foreach (b_Location e in trans.LocationList)
                {
                    dModel = new DataModel();
                    Location loc = new Location();
                    loc.UpdateFromDatabaseObject(e);
                    dModel.ChargeToId = loc.LocationId;
                    dModel.ChargeToClientLookupId = loc.ClientLookupId;
                    dModel.Name = loc.Name;
                    dModel.Complex = loc.Complex;
                    dModel.Type = loc.Type;
                    model.data.Add(dModel);
                }
            }
            else if (_type == "Account")
            {
                Account_RetrieveAll_V2 trans = new Account_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,

                };
                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                foreach (b_Account a in trans.AccountList)
                {
                    dModel = new DataModel();
                    Account account = new Account();
                    account.UpdateFromDatabaseObject(a);
                    dModel.ChargeToId = account.AccountId;
                    dModel.ChargeToClientLookupId = account.ClientLookupId;
                    dModel.Name = account.Name;
                    model.data.Add(dModel);
                }
            }
            else if (_type == "Department")
            {
                List<Department> department = new DataContracts.Department().RetrieveAllTemplatesWithClient(userData.DatabaseKey);
                Location_RetrieveAll_V2 trans = new Location_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                foreach (b_Location e in trans.LocationList)
                {
                    dModel = new DataModel();
                    Location loc = new Location();
                    loc.UpdateFromDatabaseObject(e);
                    dModel.ChargeToId = loc.LocationId;
                    dModel.ChargeToClientLookupId = loc.ClientLookupId;
                    dModel.Name = loc.Name;
                    dModel.Complex = loc.Complex;
                    dModel.Type = loc.Type;
                    model.data.Add(dModel);
                }
            }
            if (_type == "Part")
            {
                Part_RetrieveAll_V2 trans = new Part_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();
                if (trans != null && trans.PartList != null)
                {
                    foreach (b_Part e in trans.PartList)
                    {
                        dModel = new DataModel();
                        Part Part = new Part();
                        Part.UpdateFromDatabaseObject(e);
                        dModel.ChargeToId = Part.PartId;
                        dModel.ChargeToClientLookupId = Part.ClientLookupId;
                        dModel.Description = Part.Description;
                        dModel.Manufacturer = Part.Manufacturer;
                        dModel.ManufacturerId = Part.ManufacturerId;
                        dModel.UPCCode = Part.UPCCode;
                        dModel.ChargeRate = Part.AverageCost;
                        dModel.InactiveFlag = Part.InactiveFlag;
                        model.data.Add(dModel);
                    }
                }
            }

            return model.data;

        }
        public JsonResult GetLookUpListByType(string _type = "")
        {
            var ChargeToList = PopulatelookUpListByType(_type);
            var jsonResult = Json(ChargeToList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public List<DataModel> PopulatelookUpListByTypeLineItem(string _type = "")
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            if (_type == "Equipment")
            {
                Equipment_RetrieveAll_V2 trans = new Equipment_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();
                if (trans != null && trans.EquipmentList != null)
                {
                    var ActiveEquipmentList = trans.EquipmentList.Where(x => x.InactiveFlag == false);
                    foreach (b_Equipment e in ActiveEquipmentList)
                    {
                        dModel = new DataModel();
                        Equipment equipment = new Equipment();
                        equipment.UpdateFromDatabaseObject(e);
                        dModel.ChargeToId = equipment.EquipmentId;
                        dModel.ChargeToClientLookupId = equipment.ClientLookupId;
                        dModel.Name = equipment.Name;
                        model.data.Add(dModel);
                    }
                }
            }
            if (_type == "WorkOrder")
            {
                WorkOrder_RetrieveAll_V2 trans = new WorkOrder_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };
                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                if (trans != null && trans.WorkOrderList != null)
                {
                    var WorkOrderListList = trans.WorkOrderList.Where(x => x.Status == "Approved" || x.Status == "Scheduled" || x.Status == "WorkRequest" || x.Status == "AwaitingApproval");
                    foreach (b_WorkOrder e in WorkOrderListList)
                    {
                        dModel = new DataModel();
                        WorkOrder wo = new WorkOrder();
                        wo.UpdateFromDatabaseObject(e);

                        dModel.ChargeToId = wo.WorkOrderId;
                        dModel.ChargeToClientLookupId = wo.ClientLookupId;
                        dModel.Description = wo.Description;
                        dModel.Status = wo.Status;
                        model.data.Add(dModel);
                    }
                }
            }
            else if (_type == "Location")
            {
                Location_RetrieveAll_V2 trans = new Location_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                foreach (b_Location e in trans.LocationList)
                {
                    dModel = new DataModel();
                    Location loc = new Location();
                    loc.UpdateFromDatabaseObject(e);
                    dModel.ChargeToId = loc.LocationId;
                    dModel.ChargeToClientLookupId = loc.ClientLookupId;
                    dModel.Name = loc.Name;
                    dModel.Complex = loc.Complex;
                    dModel.Type = loc.Type;
                    model.data.Add(dModel);
                }
            }
            else if (_type == "Account")
            {
                Account_RetrieveAll_V2 trans = new Account_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,

                };
                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                if (trans != null && trans.AccountList != null)
                {
                    var ActiveAccountList = trans.AccountList.Where(x => x.InactiveFlag == false);
                    foreach (b_Account e in ActiveAccountList)
                    {
                        dModel = new DataModel();
                        Account account = new Account();
                        account.UpdateFromDatabaseObject(e);
                        dModel.ChargeToId = account.AccountId;
                        dModel.ChargeToClientLookupId = account.ClientLookupId;
                        dModel.Name = account.Name;
                        model.data.Add(dModel);
                    }
                }
            }
            else if (_type == "Department")
            {

                List<Department> department = new DataContracts.Department().RetrieveAllTemplatesWithClient(userData.DatabaseKey);
                Location_RetrieveAll_V2 trans = new Location_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();

                foreach (b_Location e in trans.LocationList)
                {
                    dModel = new DataModel();
                    Location loc = new Location();
                    loc.UpdateFromDatabaseObject(e);
                    dModel.ChargeToId = loc.LocationId;
                    dModel.ChargeToClientLookupId = loc.ClientLookupId;
                    dModel.Name = loc.Name;
                    dModel.Complex = loc.Complex;
                    dModel.Type = loc.Type;
                    model.data.Add(dModel);
                }
            }
            if (_type == "Part")
            {
                Part_RetrieveAll_V2 trans = new Part_RetrieveAll_V2()
                {
                    CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                    CallerUserName = this.userData.DatabaseKey.UserName,
                };

                trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
                trans.Execute();
                if (trans != null && trans.PartList != null)
                {
                    foreach (b_Part e in trans.PartList)
                    {
                        dModel = new DataModel();
                        Part Part = new Part();
                        Part.UpdateFromDatabaseObject(e);
                        dModel.ChargeToId = Part.PartId;
                        dModel.ChargeToClientLookupId = Part.ClientLookupId;
                        dModel.Description = Part.Description;
                        dModel.Manufacturer = Part.Manufacturer;
                        dModel.ManufacturerId = Part.ManufacturerId;
                        dModel.UPCCode = Part.UPCCode;
                        dModel.ChargeRate = Part.AverageCost;
                        dModel.InactiveFlag = Part.InactiveFlag;
                        model.data.Add(dModel);
                    }
                }
            }

            return model.data;
        }
        public JsonResult GetLookUpListByTypeLineItem(string _type = "")
        {
            var ChargeToList = PopulatelookUpListByTypeLineItem(_type);
            var jsonResult = Json(ChargeToList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public List<DataModel> GetLookupList_PurchaseOrder()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            PurchaseOrder_RetrieveAll_V2 trans = new PurchaseOrder_RetrieveAll_V2()
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };
            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            foreach (b_PurchaseOrder v in trans.PurchaseOrderList)
            {
                dModel = new DataModel();
                PurchaseOrder POrder = new PurchaseOrder();
                POrder.UpdateFromDatabaseObject(v);
                dModel.PurchaseOrderId = POrder.PurchaseOrderId;
                dModel.POClientLookupId = POrder.ClientLookupId;
                model.data.Add(dModel);
            }
            return model.data;

        }
        public List<DataModel> GetLookupList_Vendor5000()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            Vendor vendor_retrieve = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            List<Vendor> vendor = vendor_retrieve.Vendor_RetrieveForLookup5000(userData.DatabaseKey);

            foreach (var v in vendor)
            {
                dModel = new DataModel();
                dModel.VendorId = v.VendorId;
                dModel.Vendor = v.ClientLookupId;
                dModel.Name = v.Name;
                model.data.Add(dModel);
            }
            return model.data;
        }

        public string GetPartLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string UPCcode = "", string Manufacturer = "", string ManufacturerId = "", string StockType = "")
        {
            List<PartXRefGridDataModel> model = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            UPCcode = !string.IsNullOrEmpty(UPCcode) ? UPCcode.Trim() : string.Empty;
            Manufacturer = !string.IsNullOrEmpty(Manufacturer) ? Manufacturer.Trim() : string.Empty;
            ManufacturerId = !string.IsNullOrEmpty(ManufacturerId) ? ManufacturerId.Trim() : string.Empty;
            StockType = !string.IsNullOrEmpty(StockType) ? StockType.Trim() : string.Empty;

            model = commonWrapper.PopulatePartIds(start.Value, length.Value, order, orderDir, ClientLookupId, Description, UPCcode, Manufacturer, ManufacturerId, StockType);
            if (model != null)
            {
                model = GetAllPartsSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }

        private List<PartXRefGridDataModel> GetAllPartsSortByColumnWithOrder(string order, string orderDir, List<PartXRefGridDataModel> data)
        {
            List<PartXRefGridDataModel> lst = new List<PartXRefGridDataModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Manufacturer).ToList() : data.OrderBy(p => p.Manufacturer).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ManufacturerID).ToList() : data.OrderBy(p => p.ManufacturerID).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.StockType).ToList() : data.OrderBy(p => p.StockType).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetVendorLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "")
        {
            List<VendorLookupModel> model = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Name = !string.IsNullOrEmpty(Name) ? Name.Trim() : string.Empty;

            model = commonWrapper.PopulateVendorIds(start.Value, length.Value, order, orderDir, ClientLookupId, Name);
            if (model != null)
            {
                model = GetAllVendorSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        public string GetInternalVendorLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "")
        {
            List<VendorLookupModel> model = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Name = !string.IsNullOrEmpty(Name) ? Name.Trim() : string.Empty;

            model = commonWrapper.PopulateInternalVendorIds(start.Value, length.Value, order, orderDir, ClientLookupId, Name);
            if (model != null)
            {
                model = GetAllVendorSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        public string GetPunchOutVendorLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "", string addressCity = "", string addressState = "")
        {
            List<VendorLookupModel> model = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Name = !string.IsNullOrEmpty(Name) ? Name.Trim() : string.Empty;
            //V2-759
            addressCity = !string.IsNullOrEmpty(addressCity) ? addressCity.Trim() : string.Empty;
            addressState = !string.IsNullOrEmpty(addressState) ? addressState.Trim() : string.Empty;

            model = commonWrapper.PopulatePunchOutVendorIds(start.Value, length.Value, order, orderDir, ClientLookupId, Name, addressCity, addressState);
            if (model != null)
            {
                model = GetAllVendorSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        private List<VendorLookupModel> GetAllVendorSortByColumnWithOrder(string order, string orderDir, List<VendorLookupModel> data)
        {
            List<VendorLookupModel> lst = new List<VendorLookupModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        public string GetEquipmentLookupList(int? draw, int? start, int? length, string clientLookupId = "", string name = "", string model = "", string type = "", string serialNumber = "", bool InactiveFlag = true)
        {
            List<EquipmentLookupModel> modelList = new List<EquipmentLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            model = !string.IsNullOrEmpty(model) ? model.Trim() : string.Empty;
            type = !string.IsNullOrEmpty(type) ? type.Trim() : string.Empty;
            serialNumber = !string.IsNullOrEmpty(serialNumber) ? serialNumber.Trim() : string.Empty;

            modelList = commonWrapper.PopulateEquipmentList(start.Value, length.Value, order, orderDir, clientLookupId, name, model, type, serialNumber, InactiveFlag);
            //Order of Columns  Done by Store Procedure
            //if (modelList != null)
            //{
            //    modelList = GetAllEqpSortByColumnWithOrder(order, orderDir, modelList);
            //}
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }
        private List<EquipmentLookupModel> GetAllEqpSortByColumnWithOrder(string order, string orderDir, List<EquipmentLookupModel> data)
        {
            List<EquipmentLookupModel> lst = new List<EquipmentLookupModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Model).ToList() : data.OrderBy(p => p.Model).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SerialNumber).ToList() : data.OrderBy(p => p.SerialNumber).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetLocationLookupList(int? draw, int? start, int? length, string clientLookupId = "", string name = "")
        {
            List<LocationLookUpModel> modelList = new List<LocationLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;

            modelList = commonWrapper.PopulateLocationList_V2(start.Value, length.Value, order, orderDir, clientLookupId, name);
            if (modelList != null)
            {
                modelList = GetAllLocSortByColumnWithOrder(order, orderDir, modelList);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }

        private List<LocationLookUpModel> GetAllLocSortByColumnWithOrder(string order, string orderDir, List<LocationLookUpModel> data)
        {
            List<LocationLookUpModel> lst = new List<LocationLookUpModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Complex).ToList() : data.OrderBy(p => p.Complex).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Type).ToList() : data.OrderBy(p => p.Type).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetPartMasterLookupList(int? draw, int? start, int? length, string ClientLookupId, string LongDescription, string RequestType)
        {
            List<PartManagementGridDataModel> model = new List<PartManagementGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            LongDescription = !string.IsNullOrEmpty(LongDescription) ? LongDescription.Trim() : string.Empty;
            RequestType = !string.IsNullOrEmpty(RequestType) ? RequestType.Trim() : string.Empty;

            model = commonWrapper.PopulatePartMasterIds(start.Value, length.Value, order, orderDir, ClientLookupId, LongDescription, RequestType);
            if (model != null)
            {
                model = GetAllPartMastersSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        private List<PartManagementGridDataModel> GetAllPartMastersSortByColumnWithOrder(string order, string orderDir, List<PartManagementGridDataModel> data)
        {
            List<PartManagementGridDataModel> lst = new List<PartManagementGridDataModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LongDescription).ToList() : data.OrderBy(p => p.LongDescription).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        public string GetPartReplaceLookupList(int? draw, int? start, int? length, string ClientLookupId, string Description, string RequestType)
        {
            List<PartManagementReplaceGridModel> model = new List<PartManagementReplaceGridModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            RequestType = !string.IsNullOrEmpty(RequestType) ? RequestType.Trim() : string.Empty;

            model = commonWrapper.PopulatePartReplacementIds(start.Value, length.Value, order, orderDir, ClientLookupId, Description, RequestType);
            if (model != null)
            {
                model = GetAllPartReplSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        private List<PartManagementReplaceGridModel> GetAllPartReplSortByColumnWithOrder(string order, string orderDir, List<PartManagementReplaceGridModel> data)
        {
            List<PartManagementReplaceGridModel> lst = new List<PartManagementReplaceGridModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetManufacturerLookupList(int? draw, int? start, int? length, string ClientLookupId, string Name)
        {
            List<PartMgmtManufacGridModel> model = new List<PartMgmtManufacGridModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            model = commonWrapper.ManufacturerMasterLookupList(start.Value, length.Value, order, orderDir);
            if (model != null)
            {
                model = GetAllManufacturerSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }

        private List<PartMgmtManufacGridModel> GetAllManufacturerSortByColumnWithOrder(string order, string orderDir, List<PartMgmtManufacGridModel> data)
        {
            List<PartMgmtManufacGridModel> lst = new List<PartMgmtManufacGridModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public string GetAccountLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "")
        {
            List<AccountLookUpModel> model = new List<AccountLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Name = !string.IsNullOrEmpty(Name) ? Name.Trim() : string.Empty;


            model = commonWrapper.PopulateAccountIds(start.Value, length.Value, order, orderDir, ClientLookupId, Name);
            if (model != null)
            {
                model = GetAllAccountSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        private List<AccountLookUpModel> GetAllAccountSortByColumnWithOrder(string order, string orderDir, List<AccountLookUpModel> data)
        {
            List<AccountLookUpModel> lst = new List<AccountLookUpModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }
        public string GetWorkOrderLookupList(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string ChargeTo = "", string WorkAssigned = "", string Requestor = "", string Status = "")
        {
            List<WorkOrderLookUpModel> model = new List<WorkOrderLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            WorkAssigned = !string.IsNullOrEmpty(WorkAssigned) ? WorkAssigned.Trim() : string.Empty;
            Requestor = !string.IsNullOrEmpty(Requestor) ? Requestor.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;

            model = commonWrapper.PopulateWorkOrderIds(start.Value, length.Value, order, orderDir, ClientLookupId, Description, ChargeTo, WorkAssigned, Requestor, Status);
            //if (model != null)
            //{
            //    model = GetAllWorkOrderSortByColumnWithOrder(order, orderDir, model);
            //}
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        //private List<WorkOrderLookUpModel> GetAllWorkOrderSortByColumnWithOrder(string order, string orderDir, List<WorkOrderLookUpModel> data)
        //{
        //    List<WorkOrderLookUpModel> lst = new List<WorkOrderLookUpModel>();
        //    if (data != null)
        //    {
        //        switch (order)
        //        {
        //            case "0":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
        //                break;
        //            case "1":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
        //                break;
        //            case "2":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ChargeTo).ToList() : data.OrderBy(p => p.ChargeTo).ToList();
        //                break;
        //            case "3":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.WorkAssigned).ToList() : data.OrderBy(p => p.WorkAssigned).ToList();
        //                break;
        //            case "4":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Requestor).ToList() : data.OrderBy(p => p.Requestor).ToList();
        //                break;
        //            case "5":
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
        //                break;
        //            default:
        //                lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
        //                break;
        //        }
        //    }
        //    return lst;
        //}
        public string GetAssetLookupList(int? draw, int? start, int? length, string clientLookupId = "", string name = "", string assetType = "", string department = "", string line = "", string system = "")
        {
            List<AssetPopupModel> modelList = new List<AssetPopupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            assetType = !string.IsNullOrEmpty(assetType) ? assetType.Trim() : string.Empty;
            department = !string.IsNullOrEmpty(department) ? department.Trim() : string.Empty;
            line = !string.IsNullOrEmpty(line) ? line.Trim() : string.Empty;
            system = !string.IsNullOrEmpty(system) ? system.Trim() : string.Empty;

            modelList = commonWrapper.PopulateAssetList();
            if (modelList != null)
            {
                modelList = this.AssetSearchResult(modelList, clientLookupId, name, assetType, department, line, system);
            }
            if (modelList != null)
            {
                modelList = GetAllAssetSortColumnWithOrder(order, orderDir, modelList);
            }
            var totalRecords = 0;
            var recordsFiltered = 0;
            start = start.HasValue
                ? start / length
                : 0;
            recordsFiltered = modelList.Count();
            totalRecords = modelList.Count();
            int initialPage = start.Value;
            var filteredResult = modelList
                .Skip(initialPage * length ?? 0)
                .Take(length ?? 0)
                .ToList();
            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult });
        }
        private List<AssetPopupModel> AssetSearchResult(List<AssetPopupModel> assetList, string clientLookupId = "", string name = "", string assetType = "", string department = "", string line = "", string system = "")
        {
            if (!string.IsNullOrEmpty(clientLookupId))
            {
                clientLookupId = clientLookupId.ToUpper();
                assetList = assetList.Where(x => (!string.IsNullOrWhiteSpace(x.ClientLookUpId) && x.ClientLookUpId.ToUpper().Contains(clientLookupId))).ToList();
            }
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToUpper();
                assetList = assetList.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.ToUpper().Contains(name))).ToList();
            }
            if (!string.IsNullOrEmpty(assetType))
            {
                assetType = assetType.ToUpper();
                assetList = assetList.Where(x => (!string.IsNullOrWhiteSpace(x.AssetType) && x.AssetType.ToUpper().Contains(assetType))).ToList();
            }
            if (!string.IsNullOrEmpty(department))
            {
                department = department.ToUpper();
                assetList = assetList.Where(x => (!string.IsNullOrWhiteSpace(x.DeptClientLookupId) && x.DeptClientLookupId.ToUpper().Contains(department))).ToList();
            }
            if (!string.IsNullOrEmpty(line))
            {
                line = line.ToUpper();
                assetList = assetList.Where(x => (!string.IsNullOrWhiteSpace(x.LineClientLookupId) && x.LineClientLookupId.ToUpper().Contains(line))).ToList();
            }
            if (!string.IsNullOrEmpty(system))
            {
                system = system.ToUpper();
                assetList = assetList.Where(x => (!string.IsNullOrWhiteSpace(x.SystemClientLookupId) && x.SystemClientLookupId.ToUpper().Contains(system))).ToList();
            }
            return assetList;
        }
        private List<AssetPopupModel> GetAllAssetSortColumnWithOrder(string order, string orderDir, List<AssetPopupModel> data)
        {
            List<AssetPopupModel> lst = new List<AssetPopupModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookUpId).ToList() : data.OrderBy(p => p.ClientLookUpId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetType).ToList() : data.OrderBy(p => p.AssetType).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DeptClientLookupId).ToList() : data.OrderBy(p => p.DeptClientLookupId).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.LineClientLookupId).ToList() : data.OrderBy(p => p.LineClientLookupId).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SystemClientLookupId).ToList() : data.OrderBy(p => p.SystemClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }

        public List<DataContracts.LookupList> GetLookUpListByListName(string ListName)
        {
            DataContracts.LookupList lookupList = new DataContracts.LookupList();
            lookupList.ListName = ListName;
            var lists = lookupList.GetLookUpListByListName(userData.DatabaseKey);
            return lists;
        }

        public List<DropDownModel> GetDepartmenttByInActiveFlag(bool InactiveFlag)
        {
            List<DropDownModel> departments = new List<DropDownModel>();
            List<Department> DeptListCustom = new List<Department>();
            Department DeptCustom = new Department();
            DeptCustom.ClientId = userData.DatabaseKey.Client.ClientId;
            DeptCustom.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            DeptCustom.InactiveFlag = InactiveFlag;
            DeptListCustom = DeptCustom.RetrieveByInActiveFlag(userData.DatabaseKey);
            if (DeptListCustom != null && DeptListCustom.Count > 0)
            {
                departments = DeptListCustom.Select(x => new DropDownModel { value = x.DepartmentId.ToString(), text = x.Description }).ToList();
            }
            return departments;
        }

        #endregion

        #region Localisations
        internal void LocalizeControls(LocalisationBaseVM objComb, string ResourceType)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            var connstring = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
            //------------Retrieve by Specific Resource Type Localization--------------//

            List<Localizations> locSpecificPageCache = _logCache.GetLocalizationCommon(connstring, ResourceType, userData.Site.Localization);

            //------------Retrieve Global Localization--------------------//
            List<Localizations> locGlobalCache = _logCache.GetLocalizationCommon(connstring, LocalizeResourceSetConstants.Global, userData.Site.Localization);

            if (locSpecificPageCache != null && locSpecificPageCache.Count > 0)
            {
                objComb.Loc = locSpecificPageCache;
            }
            else
            {
                //------Retrieve "en-us" data if other lang fail----------//
                locSpecificPageCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), ResourceType, "en-us");
                objComb.Loc = locSpecificPageCache;
            }
            if (locGlobalCache != null && locGlobalCache.Count > 0)
            {
                objComb.Loc.AddRange(locGlobalCache);
            }
            else
            {
                //------Retrieve "en-us" data if other lang fail----------//
                locGlobalCache = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.Global, "en-us");
                objComb.Loc.AddRange(locGlobalCache);
            }

        }
        public JsonResult GetDataTableLanguageJson(bool InnerGrid = false, bool nGrid = false)
        {
            DataTableLanguageRoot dtlanguageDetails = new DataTableLanguageRoot();
            string sLengthMenu = "<select class='searchdt-menu select2picker'>" +
                                              "<option value='10'>10</option>" +
                                              "<option value='20'>20</option>" +
                                              "<option value='30'>30</option>" +
                                              "<option value='40'>40</option>" +
                                              "<option value='50'>50</option>" +
                                              "</select>";

            dtlanguageDetails.sEmptyTable = UtilityFunction.GetMessageFromResource("sEmptyTable", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfo = UtilityFunction.GetMessageFromResource("sInfo", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoEmpty = UtilityFunction.GetMessageFromResource("sInfoEmpty", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoFiltered = UtilityFunction.GetMessageFromResource("sInfoFiltered", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoPostFix = UtilityFunction.GetMessageFromResource("sInfoPostFix", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sInfoThousands = UtilityFunction.GetMessageFromResource("sInfoThousands", LocalizeResourceSetConstants.DataTableEntry);
            if (nGrid)
            {
                dtlanguageDetails.sLengthMenu = sLengthMenu;
            }
            else if (InnerGrid)
            {
                dtlanguageDetails.sLengthMenu = "<select class='innergrid-searchdt-menu form-control search'>" +
                                              "<option value='10'>10</option>" +
                                              "<option value='20'>20</option>" +
                                              "</select>";
            }
            else
            {
                dtlanguageDetails.sLengthMenu = "Page size :&nbsp;" + sLengthMenu;
            }

            dtlanguageDetails.sLoadingRecords = UtilityFunction.GetMessageFromResource("sLoadingRecords", LocalizeResourceSetConstants.DataTableEntry);

            dtlanguageDetails.sProcessing = "<img src='../Content/Images/image_1197421.gif'>";

            dtlanguageDetails.sSearch = UtilityFunction.GetMessageFromResource("sSearch", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.sZeroRecords = UtilityFunction.GetMessageFromResource("sZeroRecords", LocalizeResourceSetConstants.DataTableEntry);
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sFirst = "<img src='../images/drop-grid-first.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sFirst = UtilityFunction.GetMessageFromResource("sFirst", LocalizeResourceSetConstants.DataTableEntry);
            }
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sPrevious = "<img src='../images/drop-grid-prev.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sPrevious = UtilityFunction.GetMessageFromResource("sPrevious", LocalizeResourceSetConstants.DataTableEntry);
            }

            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sNext = "<img src='../images/drop-grid-next.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sNext = UtilityFunction.GetMessageFromResource("sNext", LocalizeResourceSetConstants.DataTableEntry);
            }
            if (InnerGrid || nGrid)
            {
                dtlanguageDetails.oPaginate.sLast = "<img src='../images/drop-grid-last.png'>";
            }
            else
            {
                dtlanguageDetails.oPaginate.sLast = UtilityFunction.GetMessageFromResource("sLast", LocalizeResourceSetConstants.DataTableEntry);
            }

            dtlanguageDetails.oAria.sSortAscending = UtilityFunction.GetMessageFromResource("sSortAscending", LocalizeResourceSetConstants.DataTableEntry);
            dtlanguageDetails.oAria.sSortDescending = UtilityFunction.GetMessageFromResource("sSortDescending", LocalizeResourceSetConstants.DataTableEntry);

            return Json(dtlanguageDetails, JsonRequestBehavior.AllowGet);
        }
        public List<Localizations> MsgLocalizeDetailsBase(DataContracts.UserData userData)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            List<Localizations> jsAlertmsglist = new List<Localizations>();

            jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.JsAlerts, userData.Site.Localization);

            if (jsAlertmsglist != null && jsAlertmsglist.Count > 0)
            {
                return jsAlertmsglist;
            }
            else
            {
                jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.JsAlerts, "en-us");
                return jsAlertmsglist;
            }
        }
        public List<Localizations> MsgLocalizeStatusDetails(DataContracts.UserData userData)
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            List<Localizations> jsAlertmsglist = new List<Localizations>();

            jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.StatusDetails, userData.Site.Localization);

            if (jsAlertmsglist != null && jsAlertmsglist.Count > 0)
            {
                return jsAlertmsglist;
            }
            else
            {
                jsAlertmsglist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.StatusDetails, "en-us");
                return jsAlertmsglist;
            }
        }
        public List<Localizations> GetTimeZoneList()
        {
            LoginCacheSet _logCache = new LoginCacheSet();
            List<Localizations> objlist = new List<Localizations>();
            var connstring = ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString();
            objlist = _logCache.GetLocalizationCommon(ConfigurationManager.ConnectionStrings[WebConfigConstants.ADMIN_CONNECTION_STRING].ToString(), LocalizeResourceSetConstants.TimeZoneDetails, "en-us");
            return objlist;
        }
        #endregion

        #region File-Upload
        [HttpPost]
        public ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images", Server.MapPath(@"\write\")));
                        string pathString = Path.Combine(originalDirectory.ToString(), userData.SessionId.ToString());
                        bool isExists = Directory.Exists(pathString);
                        if (!isExists)
                            Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        TempData["ImageDirectory"] = pathString;
                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
        [HttpPost]
        public ActionResult SaveUploadedFileToServer(string fileName, long objectId, string TableName, string AttachObjectName)
        {
            string imageurl = string.Empty;
            string sasToken = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            int result = 0;
            if (OnPremise)
            {
                imageurl = UploadNewImageToOnPremise(fileName, objectId, TableName, AttachObjectName, out result);
            }
            else
            {
                imageurl = UploadNewImageToAzure(fileName, objectId, TableName, AttachObjectName);
            }


            var jsonResult = Json(new { imageurl = imageurl, result = result });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string UploadNewImageToAzure(string fileName, long objectId, string TableName, string AttachObjectName)
        {
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            string uploadedURL = string.Empty;
            string imageurl = string.Empty;
            string imagePath = string.Empty;
            string content_type = string.Empty;
            var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            System.IO.FileInfo[] files = originalDirectory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name == fileName && (TableName.ToLower() != "partmaster" && TableName.ToLower() != "client"))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                    content_type = MimeMapping.GetMimeMapping(file.FullName);

                    if (TableName.ToLower() != "partmasterrequest")
                    {
                        imagePath = aset.CreateFileNamebyObject(TableName, objectId.ToString(), fileName);
                        uploadedURL = aset.UploadToAzureBlob(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imagePath, bytes, content_type);
                        SaveToDatabase(file, objectId, AttachObjectName, uploadedURL);
                        imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, uploadedURL);
                    }
                    else if (TableName.ToLower() == "partmasterrequest")
                    {
                        imagePath = aset.CreateFileNamebyObject("PartMasterRequestImage", objectId.ToString(), fileName);
                        DataContracts.PartMasterRequest pmr = new DataContracts.PartMasterRequest()
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            PartMasterRequestId = objectId
                        };

                        pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                        uploadedURL = aset.UploadToAzureBlob(pmr.ClientId, pmr.SiteId, imagePath, bytes, content_type);
                        SavePartMasterRequest(pmr, uploadedURL);// should be removed later
                        SaveToDatabase(file, objectId, AttachObjectName, uploadedURL);
                        imageurl = aset.GetSASUrlClientSite(pmr.ClientId, pmr.SiteId, uploadedURL);
                    }
                }
                else if (file.Name == fileName && (TableName.ToLower() == "partmaster" || TableName.ToLower() == "client"))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                    content_type = MimeMapping.GetMimeMapping(file.FullName);
                    imagePath = aset.CreateFileNamebyObject(TableName, objectId.ToString(), fileName);
                    uploadedURL = aset.UploadToAzureBlob(userData.DatabaseKey.Client.ClientId, imagePath, bytes, content_type);
                    SaveToDatabase(file, objectId, AttachObjectName, uploadedURL);
                    imageurl = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, uploadedURL);
                }
            }
            return imageurl;
        }
        private string UploadNewImageToOnPremise(string fileName, long objectId, string TableName, string AttachObjectName, out int result)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string imageurl = string.Empty;
            string Filepath = string.Empty;

            int ConnectRemoteShareErrorCode = 0;

            NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();

            var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();

            using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
            {
                Filepath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath);
                if (ConnectRemoteShareErrorCode == 0)
                {
                    System.IO.FileInfo[] files = originalDirectory.GetFiles();
                    string DBFilePath = string.Empty;
                    foreach (var file in files)
                    {
                        if (file.Name == fileName && (TableName.ToLower() != "partmaster"))
                        {

                            if (TableName.ToLower() != "partmasterrequest")
                            {
                                Filepath = commonWrapper.UploadFileOnPremise(Filepath, objectId, TableName, out DBFilePath);
                                DBFilePath = Path.Combine(DBFilePath, fileName);
                                Filepath = Path.Combine(Filepath, file.Name);
                                file.CopyTo(Filepath, true);
                                SaveToDatabase(file, objectId, AttachObjectName, DBFilePath);
                                imageurl = Filepath;

                            }
                            else if (TableName.ToLower() == "partmasterrequest")
                            {
                                DataContracts.PartMasterRequest pmr = new DataContracts.PartMasterRequest()
                                {
                                    ClientId = userData.DatabaseKey.Client.ClientId,
                                    PartMasterRequestId = objectId
                                };
                                pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                                Filepath = commonWrapper.UploadFileOnPremise(Filepath, objectId, TableName, out DBFilePath);
                                DBFilePath = Path.Combine(DBFilePath, fileName);
                                Filepath = Path.Combine(Filepath, file.Name);
                                file.CopyTo(Filepath, true);
                                SavePartMasterRequest(pmr, Filepath);// should be removed later
                                SaveToDatabase(file, objectId, AttachObjectName, DBFilePath);
                                imageurl = Filepath;

                            }
                        }
                        else if (file.Name == fileName && (TableName.ToLower() == "partmaster"))
                        {
                            Filepath = commonWrapper.UploadFileOnPremise(Filepath, objectId, TableName, out DBFilePath);
                            DBFilePath = Path.Combine(DBFilePath, fileName);
                            Filepath = Path.Combine(Filepath, file.Name);
                            file.CopyTo(Filepath, true);
                            SaveToDatabase(file, objectId, AttachObjectName, DBFilePath);
                            imageurl = Filepath;
                        }

                    }
                }

            }
            result = ConnectRemoteShareErrorCode;
            if (ConnectRemoteShareErrorCode == 0)
            {
                return UtilityFunction.PhotoBase64ImgSrc(imageurl);
            }
            else
            {
                return "";
            }


        }

        private string UploadImageToAzure(byte[] uploadedFile, long objectId, string TableName)
        {
            string rtrData = string.Empty;
            AzureBlob ablob = new AzureBlob();
            AzureSetup aset = new AzureSetup();

            if (uploadedFile.Length > 1)
            {
                Int64 Clientid = userData.DatabaseKey.Client.ClientId;
                Int64 Siteid = userData.DatabaseKey.User.DefaultSiteId;
                string imgName = TableName + "_" + DateTime.Now.Ticks.ToString() + "." + "jpg";
                string fileName = aset.CreateFileNamebyObject(TableName, objectId.ToString(), imgName);
                rtrData = aset.ConnectToAzureBlob(Clientid, Siteid, fileName, uploadedFile);
            }

            return rtrData;
        }
        private void SaveToDatabase(System.IO.FileInfo fileInfo, long objectId, string attachObjectName, string attachmentURL)
        {
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = attachObjectName,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);

            if (AList.Count > 0)
            {
                attach.AttachmentId = AList.First().AttachmentId;
                attach.Retrieve(userData.DatabaseKey);
                attach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                attach.Description = "Profile Image";
                attach.FileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                attach.FileType = Path.GetExtension(fileInfo.Name).Remove(0, 1);
                attach.ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName);
                attach.FileSize = Convert.ToInt32(fileInfo.Length);
                attach.Image = true;
                attach.Profile = true;
                attach.External = false;
                attach.Reference = false;
                attach.AttachmentURL = attachmentURL;
                attach.Update(userData.DatabaseKey);
            }
            else
            {
                attach.ObjectName = attachObjectName;
                attach.ObjectId = objectId;
                attach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                attach.Description = "Profile Image";
                attach.FileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                attach.FileType = Path.GetExtension(fileInfo.Name).Remove(0, 1);
                attach.ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName);
                attach.FileSize = Convert.ToInt32(fileInfo.Length);
                attach.Image = true;
                attach.Profile = true;
                attach.External = false;
                attach.Reference = false;
                attach.AttachmentURL = attachmentURL;
                attach.Create(userData.DatabaseKey);
            }
        }
        private void SavePartMasterRequest(PartMasterRequest pmr, string imageUrl)
        {
            pmr.ImageURL = imageUrl == null ? "" : imageUrl;
            pmr.Update(this.userData.DatabaseKey);
        }
        #endregion

        #region Rotativa-Print
        internal bool CheckLoginSession(string LoginSessionID)
        {
            Guid LogsessionId = Guid.Empty;
            LogsessionId = new Guid(LoginSessionID);
            DataContracts.DatabaseKey dbKey = Authentication.GetAdminOnlyKey();
            this.userData = new DataContracts.UserData() { SessionId = LogsessionId, WebSite = WebSiteEnum.Client };
            this.userData.Retrieve(dbKey);
            Authentication auth = new Authentication() { UserData = this.userData };
            auth.UserData.LoginAuditing.CreateDate = DateTime.Now;
            auth.VerifyCurrentUser();
            return auth.IsAuthenticated;
        }
        #endregion        

        #region Convert DataTable To List
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion

        #region Attachment
        internal List<AttachmentModel> GetAllAttachmentsSortByColumnWithOrder(string order, string orderDir, List<AttachmentModel> data)
        {
            List<AttachmentModel> lst = new List<AttachmentModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileSize).ToList() : data.OrderBy(p => p.FileSize).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                }
            }
            return lst;
        }
        internal List<AttachmentModel> GetAllAttachmentsPrintWithFormSortByColumnWithOrder(string order, string orderDir, List<AttachmentModel> data)
        {
            List<AttachmentModel> lst = new List<AttachmentModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileName).ToList() : data.OrderBy(p => p.FileName).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.FileSize).ToList() : data.OrderBy(p => p.FileSize).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.OwnerName).ToList() : data.OrderBy(p => p.OwnerName).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.CreateDate).ToList() : data.OrderBy(p => p.CreateDate).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PrintwithForm).ToList() : data.OrderBy(p => p.PrintwithForm).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Subject).ToList() : data.OrderBy(p => p.Subject).ToList();
                        break;
                }
            }
            return lst;
        }
        public JsonResult IsOnpremiseCredentialValid()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            return Json(commonWrapper.IsOnpremiseCredentialValid(), JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region GetUrl
        public string GetUrl()
        {
            string localurl = string.Empty;
            int iPort = System.Web.HttpContext.Current.Request.Url.Port;
            if (iPort != 443 && iPort != 80)
            {
                string[] url = new string[3];
                url[0] = System.Web.HttpContext.Current.Request.Url.Host;
                url[1] = System.Web.HttpContext.Current.Request.Url.Port.ToString();
                localurl = string.Format(System.Web.HttpContext.Current.Request.Url.Scheme + "://{0}:{1}", url);
            }
            else if (iPort == 443)
            {
                string[] url = new string[2];
                url[0] = System.Web.HttpContext.Current.Request.Url.Host;
                localurl = string.Format("https://{0}", url);
            }
            else
            {
                string[] url = new string[2];
                url[0] = System.Web.HttpContext.Current.Request.Url.Host;
                localurl = string.Format(System.Web.HttpContext.Current.Request.Url.Scheme + "://{0}", url);
            }
            return localurl;
        }
        #endregion

        #region Notification
        public PartialViewResult GetMaintenanceNotification()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<NotificationModel> NotificationModelList = new List<NotificationModel>();
            NotificationModelVM objNotificationModelVM = new NotificationModelVM();
            int UnreadCount = 0;
            int UnreadSelectedtabCount = 0;
            NotificationModelList = commonWrapper.GetNotificationList("Maintenance", ref UnreadCount, ref UnreadSelectedtabCount);
            objNotificationModelVM.NotificationList = NotificationModelList;
            objNotificationModelVM.NewNotificationCount = UnreadCount;
            objNotificationModelVM.NewNotificationSelectedtabCount = UnreadSelectedtabCount;
            return PartialView("_MaintenanceNotification", objNotificationModelVM);
        }
        public PartialViewResult GetInventoryNotification()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<NotificationModel> NotificationModelList = new List<NotificationModel>();
            NotificationModelVM objNotificationModelVM = new NotificationModelVM();
            objNotificationModelVM.PageNumber = 0;
            int UnreadCount = 0;
            int UnreadSelectedtabCount = 0;
            NotificationModelList = commonWrapper.GetNotificationList("Inventory", ref UnreadCount, ref UnreadSelectedtabCount);
            objNotificationModelVM.NotificationList = NotificationModelList;
            objNotificationModelVM.NewNotificationCount = UnreadCount;
            objNotificationModelVM.NewNotificationSelectedtabCount = UnreadSelectedtabCount;
            return PartialView("_InventoryNotification", objNotificationModelVM);
        }
        public PartialViewResult GetProcurementNotification()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<NotificationModel> NotificationModelList = new List<NotificationModel>();
            NotificationModelVM objNotificationModelVM = new NotificationModelVM();
            objNotificationModelVM.PageNumber = 0;
            int UnreadCount = 0;
            int UnreadSelectedtabCount = 0;
            NotificationModelList = commonWrapper.GetNotificationList("Procurement", ref UnreadCount, ref UnreadSelectedtabCount);
            objNotificationModelVM.NotificationList = NotificationModelList;
            objNotificationModelVM.NewNotificationCount = UnreadCount;
            objNotificationModelVM.NewNotificationSelectedtabCount = UnreadSelectedtabCount;
            return PartialView("_ProcurementNotification", objNotificationModelVM);
        }
        public PartialViewResult GetSystemNotification()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<NotificationModel> NotificationModelList = new List<NotificationModel>();
            NotificationModelVM objNotificationModelVM = new NotificationModelVM();
            objNotificationModelVM.PageNumber = 0;
            int UnreadCount = 0;
            int UnreadSelectedtabCount = 0;
            NotificationModelList = commonWrapper.GetNotificationList("System", ref UnreadCount, ref UnreadSelectedtabCount);
            objNotificationModelVM.NotificationList = NotificationModelList;
            objNotificationModelVM.NewNotificationCount = UnreadCount;
            objNotificationModelVM.NewNotificationSelectedtabCount = UnreadSelectedtabCount;
            return PartialView("_SystemNotification", objNotificationModelVM);
        }
        public ActionResult GetUnreadNotificationCount()
        {
            int ResultMaintenanceCount = 0;
            int ResultInventoryCount = 0;
            int ResultProcurementCount = 0;
            int ResultSystemCount = 0;
            int UnreadCount = 0;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            UnreadCount = commonWrapper.GetUnreadNotificationCount(out int resultMaintenanceCount, out int resultInventoryCount, out int resultProcurementCount, out int resultSystemCount);
            ResultMaintenanceCount = resultMaintenanceCount;
            ResultInventoryCount = resultInventoryCount;
            ResultProcurementCount = resultProcurementCount;
            ResultSystemCount = resultSystemCount;

            return Json(new { UnreadCount = UnreadCount, ResultMaintenanceCount = ResultMaintenanceCount, ResultInventoryCount = ResultInventoryCount, ResultProcurementCount = ResultProcurementCount, ResultSystemCount = ResultSystemCount }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult MarkNotificationAsRead()
        {
            AlertUser objAlertUser = new AlertUser();
            objAlertUser.IsRead = true;
            objAlertUser.UpdateAlertByUserId(this.userData.DatabaseKey);
            return Json(JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteNotification(long AlertUserId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            if (commonWrapper.DeleteNotification(AlertUserId))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ClearNotificationSelectedTab(string Type)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            if (commonWrapper.ClearNotification(Type))
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region GridLayout
        public JsonResult CreateUpdateState(string GridName, string LayOutInfo, string FilterInfo = "")
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            gridStateWrapper.CreateUpdateState(GridName, LayOutInfo, FilterInfo);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetState(string GridName)
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            string currentState = gridStateWrapper.GetState(GridName);
            return Json(currentState, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLayout(string GridName)
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            var currentState = gridStateWrapper.GetLayout(GridName);
            return Json(currentState, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Menu
        public JsonResult SetMenuOpenState(string state)
        {
            Session["MenuState"] = state;
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMenuItemsCount()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<MenuStatusModel> rListObj = new List<MenuStatusModel>();
            MenuStatusModel rObj;
            List<Menu> openCount = commonWrapper.GetEventInfoCountByStatus();
            foreach (var item in openCount)
            {
                rObj = new MenuStatusModel();
                rObj.ModuleName = item.ModuleName;
                rObj.ItemCount = item.ItemCount;
                rListObj.Add(rObj);
            }
            return Json(rListObj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Common Text Search
        public JsonResult PopulateNewSearchList(string tableName)
        {
            List<string> searchOptionList = new List<string>();
            string tabName = GetTableName(tableName) == "" ? tableName : GetTableName(tableName);

            CommonWrapper coWrapper = new CommonWrapper(userData);
            var optList = coWrapper.GetSearchOptionList(tabName);

            if (optList != null)
            {
                foreach (var item in optList)
                {
                    searchOptionList.Add(item.SearchText);
                }
                var returnOjb = new { success = true, searchOptionList = searchOptionList };
                var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            else
            {
                var jsonResult = Json("failed", JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
        }

        [HttpPost]
        public JsonResult ModifyNewSearchList(string tableName, string searchText = "", bool isClear = false)
        {
            List<string> searchOptionList = new List<string>();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            string tabName = GetTableName(tableName) == "" ? tableName : GetTableName(tableName);
            var optList = coWrapper.ModifySearchOptionList(tabName, searchText, isClear);
            foreach (var item in optList)
            {
                searchOptionList.Add(item.SearchText);
            }
            var returnOjb = new { success = true, searchOptionList = searchOptionList };
            var jsonResult = Json(returnOjb, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        private string GetTableName(string table)
        {
            string tabName = string.Empty;
            switch (table)
            {
                case "WorkOrder":
                    tabName = AttachmentTableConstant.WorkOrder;
                    break;
                case "Device":
                    tabName = AttachmentTableConstant.IotDevice;
                    break;
                case "Equipment":
                    tabName = AttachmentTableConstant.Equipment;
                    break;
                case "Part":
                    tabName = AttachmentTableConstant.Part;
                    break;
                case "PrevMaintMaster":
                    tabName = AttachmentTableConstant.PreventiveMaintenance;
                    break;
                case "PurchaseRequest":
                    tabName = AttachmentTableConstant.PurchaseRequest;
                    break;
                case "Vendor":
                    tabName = AttachmentTableConstant.Vendor;
                    break;
                case "PurchaseOrder":
                    tabName = AttachmentTableConstant.PurchaseOrder;
                    break;
                case "SanitationJob":
                    tabName = AttachmentTableConstant.SanitationJob;
                    break;
                case "InvoiceMatchHeader":
                    tabName = AttachmentTableConstant.InvoiceMatchHeader;
                    break;
                case "FleetAsset":
                    tabName = AttachmentTableConstant.FleetAsset;
                    break;
                case "FleetFuel":
                    tabName = AttachmentTableConstant.FleetFuel;
                    break;
                case "UserManagement":
                    tabName = AttachmentTableConstant.UserManagement;
                    break;
                case "FleetIssues":
                    tabName = AttachmentTableConstant.FleetIssues;
                    break;
                case "PurchaseOrderReceipt": //V2-331
                    tabName = AttachmentTableConstant.PurchaseOrderReceipt;
                    break;
                case "CustomSecurityProfile":
                    tabName = AttachmentTableConstant.CustomSecurityProfile;
                    break;
                case "ListLaborScheduling":
                    tabName = AttachmentTableConstant.ListLaborScheduling;
                    break;
                case "Personnel":
                    tabName = AttachmentTableConstant.Personnel;
                    break;
                case "ResourceList":
                    tabName = AttachmentTableConstant.ResourceList;
                    break;
                case "Project_Search":
                    tabName = AttachmentTableConstant.Project_Search;
                    break;
                case "MaintenanceCompletionWorkbench_Search":
                    tabName = AttachmentTableConstant.MaintenanceCompletionWorkbench_Search;
                    break;
            }
            return tabName;
        }
        #endregion

        #region Common Comment Section

        public JsonResult GetMentionList(string searchText = "")
        {
            UserMentionData userMentionData;
            List<UserMentionData> userMentionDatas = new List<UserMentionData>();
            List<string> searchedNameList = new List<string>();
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var namelist = coWrapper.MentionList(searchText);
            foreach (var item in namelist)
            {
                userMentionData = new UserMentionData();
                userMentionData.id = item.UserName;
                userMentionData.name = item.FullName;
                userMentionData.type = item.PersonnelInitial;
                userMentionDatas.Add(userMentionData);
            }
            var jsonResult = Json(userMentionDatas, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        [HttpPost]
        public ActionResult DeleteComment(long _notesId)
        {
            CommonWrapper coWrapper = new CommonWrapper(userData);
            var deleteResult = coWrapper.DeleteComment(_notesId);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region UiConfig Custom Validation

        ////----------V2-375----------////
        [HttpPost]
        public JsonResult UiConfigAllColumns(string viewName, string isExternal)
        {
            CommonWrapper cWrapper = new CommonWrapper(userData);

            string externvalValue = string.Empty;
            if (isExternal == "True")
            {
                externvalValue = UiConfigConstants.IsExternalTrue;
            }
            else if (isExternal == "False")
            {
                externvalValue = UiConfigConstants.IsExternalFalse;
            }
            else
            {
                externvalValue = UiConfigConstants.IsExternalNone;
            }
            string target = "validator";
            //var vList = cWrapper.UiConfigAllColumnsCustom(viewName, UiConfigConstants.IsHideNone, UiConfigConstants.IsRequiredTrue, externvalValue, target);
            var vList = cWrapper.GetAllUiConfigListForClientSideValidator(viewName, externvalValue);
            return Json(new { vList = vList }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fleet

        public JsonResult FleetAssetLookupList(int? draw, int? start, int? length, string clientLookupId = "", string name = "", string model = "", string make = "", string vin = "", bool InactiveFlag = true)
        {
            List<FleetAssetLookupModel> modelList = new List<FleetAssetLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            model = !string.IsNullOrEmpty(model) ? model.Trim() : string.Empty;
            make = !string.IsNullOrEmpty(make) ? make.Trim() : string.Empty;
            vin = !string.IsNullOrEmpty(vin) ? vin.Trim() : string.Empty;

            modelList = commonWrapper.PopulateFleetEquipmentList(start.Value, length.Value, order, orderDir, clientLookupId, name, model, make, vin);
            if (modelList != null)
            {
                modelList = GetAllFleetAssetSortByColumnWithOrder(order, orderDir, modelList);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            // return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
            var jsonResult = Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }



        private List<FleetAssetLookupModel> GetAllFleetAssetSortByColumnWithOrder(string order, string orderDir, List<FleetAssetLookupModel> data)
        {
            List<FleetAssetLookupModel> lst = new List<FleetAssetLookupModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Model).ToList() : data.OrderBy(p => p.Model).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Make).ToList() : data.OrderBy(p => p.Make).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.VIN).ToList() : data.OrderBy(p => p.VIN).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ClientLookupId).ToList() : data.OrderBy(p => p.ClientLookupId).ToList();
                        break;
                }
            }
            return lst;
        }



        public JsonResult FleetIssueLookupList(int? draw, int? start, int? length, long Equipmentid, string Date, string Description, string Status, string Defects)
        {
            List<FleetIssuesLookupModel> modelList = new List<FleetIssuesLookupModel>();

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            Date = !string.IsNullOrEmpty(Date) ? Date.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;
            Defects = !string.IsNullOrEmpty(Defects) ? Defects.Trim() : string.Empty;

            modelList = commonWrapper.PopulateFleetIssuesList(start.Value, length.Value, order, orderDir, Equipmentid, Date, Description, Status, Defects);
            if (modelList != null && order != "0")
            {
                modelList = GetAllFleetIssuesSortByColumnWithOrder(order, orderDir, modelList);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            // return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
            var jsonResult = Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        private List<FleetIssuesLookupModel> GetAllFleetIssuesSortByColumnWithOrder(string order, string orderDir, List<FleetIssuesLookupModel> data)
        {
            List<FleetIssuesLookupModel> lst = new List<FleetIssuesLookupModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Description).ToList() : data.OrderBy(p => p.Description).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Status).ToList() : data.OrderBy(p => p.Status).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Defects).ToList() : data.OrderBy(p => p.Defects).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Readingdate).ToList() : data.OrderBy(p => p.Readingdate).ToList();
                        break;
                }
            }
            return lst;
        }

        public JsonResult AssetAvailabilityLogLookupList(int? draw, int? start, int? length, long ObjectId, string TransactionDate, string Event, string ReturnToService, string Reason, string ReasonCode = "", string PersonnelName = "")
        {
            List<AssetAvailabilityLogLookUpModel> modelList = new List<AssetAvailabilityLogLookUpModel>();

            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            TransactionDate = !string.IsNullOrEmpty(TransactionDate) ? TransactionDate.Trim() : string.Empty;
            Event = !string.IsNullOrEmpty(Event) ? Event.Trim() : string.Empty;
            ReturnToService = !string.IsNullOrEmpty(ReturnToService) ? ReturnToService.Trim() : string.Empty;
            Reason = !string.IsNullOrEmpty(Reason) ? Reason.Trim() : string.Empty;
            ReasonCode = !string.IsNullOrEmpty(ReasonCode) ? ReasonCode.Trim() : string.Empty;
            PersonnelName = !string.IsNullOrEmpty(PersonnelName) ? PersonnelName.Trim() : string.Empty;

            modelList = commonWrapper.PopulateAssetAvailabilityLogList(start.Value, length.Value, order, orderDir, ObjectId, TransactionDate, Event, ReturnToService, Reason, ReasonCode, PersonnelName);
            if (modelList != null && order != "0")
            {
                modelList = GetAllAssetAvailabilitySortByColumnWithOrder(order, orderDir, modelList);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            // return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
            var jsonResult = Json(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private List<AssetAvailabilityLogLookUpModel> GetAllAssetAvailabilitySortByColumnWithOrder(string order, string orderDir, List<AssetAvailabilityLogLookUpModel> data)
        {
            List<AssetAvailabilityLogLookUpModel> lst = new List<AssetAvailabilityLogLookUpModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Event).ToList() : data.OrderBy(p => p.Event).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReturnToService).ToList() : data.OrderBy(p => p.ReturnToService).ToList();
                        break;
                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ReasonCode).ToList() : data.OrderBy(p => p.ReasonCode).ToList();
                        break;
                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reason).ToList() : data.OrderBy(p => p.Reason).ToList();
                        break;
                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PersonnelName).ToList() : data.OrderBy(p => p.PersonnelName).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TransactionDate).ToList() : data.OrderBy(p => p.TransactionDate).ToList();
                        break;
                }
            }
            return lst;
        }

        #endregion

        #region UserAccess List for V2-332

        public string GetUserAccessList(int? draw, int? start, int? length, string SecurityProfileName = "", string SecurityProfileDescription = "")
        {
            List<UserAccessLookupModel> model = new List<UserAccessLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            SecurityProfileName = !string.IsNullOrEmpty(SecurityProfileName) ? SecurityProfileName.Trim() : string.Empty;
            SecurityProfileDescription = !string.IsNullOrEmpty(SecurityProfileDescription) ? SecurityProfileDescription.Trim() : string.Empty;

            model = commonWrapper.PopulateUserAccessList(start.Value, length.Value, order, orderDir, SecurityProfileName, SecurityProfileDescription);
            if (model != null)
            {
                model = GetUserAccessListSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }

        public string GetChangeUserAccessList(int? draw, int? start, int? length, string SecurityProfileName = "", string SecurityProfileDescription = "", bool APM = false, bool CMMS = false, bool Sanitation = false, bool Fleet = false, bool Production = false)
        {
            List<UserAccessLookupModel> model = new List<UserAccessLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            SecurityProfileName = !string.IsNullOrEmpty(SecurityProfileName) ? SecurityProfileName.Trim() : string.Empty;
            SecurityProfileDescription = !string.IsNullOrEmpty(SecurityProfileDescription) ? SecurityProfileDescription.Trim() : string.Empty;

            model = commonWrapper.PopulateUserAccessChangeList(start.Value, length.Value, order, orderDir, SecurityProfileName, SecurityProfileDescription, APM, CMMS, Sanitation, Fleet, Production);
            if (model != null)
            {
                model = GetUserAccessListSortByColumnWithOrder(order, orderDir, model);
            }
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (model != null && model.Count > 0)
            {
                totalRecords = model[0].TotalCount;
                recordsFiltered = model[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = model });
        }
        private List<UserAccessLookupModel> GetUserAccessListSortByColumnWithOrder(string order, string orderDir, List<UserAccessLookupModel> data)
        {
            List<UserAccessLookupModel> lst = new List<UserAccessLookupModel>();
            if (data != null)
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SecurityProfileName).ToList() : data.OrderBy(p => p.SecurityProfileName).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SecurityProfileDescription).ToList() : data.OrderBy(p => p.SecurityProfileDescription).ToList();
                        break;
                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.SecurityProfileName).ToList() : data.OrderBy(p => p.SecurityProfileName).ToList();
                        break;
                }
            }
            return lst;
        }

        #endregion

        #region  System Unavailable Message
        public JsonResult GetSiteMaintenanceMessage()
        {

            SiteMaintenanceWrapper objSiteMainWrapper = new SiteMaintenanceWrapper(userData);

            string maintenanceMessage = string.Empty;
            if (Session["SitetenanceMessage"] == null)
            {
                var mydata = objSiteMainWrapper.GetNextSitemaintenance("y");
                if (mydata.SiteMaintenanceId != 0)
                {
                    // RKL - the start and end time are entered into the message in eastern (to be compatible with V1).
                    // Use User's timezone for messaging
                    DateTime start_user = TimeZoneInfo.ConvertTime(
                                                TimeZoneInfo.ConvertTimeToUtc(mydata.DowntimeStart, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))
                                              , TimeZoneInfo.FindSystemTimeZoneById(userData.Site.TimeZone));
                    DateTime end_user = TimeZoneInfo.ConvertTime(
                                                TimeZoneInfo.ConvertTimeToUtc(mydata.DowntimeEnd, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))
                                              , TimeZoneInfo.FindSystemTimeZoneById(userData.Site.TimeZone));

                    DateTime now_user = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(userData.Site.TimeZone));
                    if (start_user.Date == end_user.Date && start_user.Date != now_user.Date)
                    {
                        maintenanceMessage = string.Format("{0} from {1} to {2} on {3}",
                        mydata.DashboardMessage, start_user.ToString("t"), end_user.ToString("t"), start_user.ToString("D"));
                    }
                    else if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date == DateTime.Now.Date)
                    {
                        maintenanceMessage = string.Format("{0} today from {1} to {2}",
                        mydata.DashboardMessage, start_user.ToString("t"), end_user.ToString("t"));
                    }
                    else
                    {
                        mydata = objSiteMainWrapper.GetNextSitemaintenance("n");
                        maintenanceMessage = string.Format("{0} from {1} to {2}", mydata.DashboardMessage,
                        start_user.ToString("f"), end_user.ToString("f"));
                    }
                    /*
                    // Convert to user's timezone 
                    mydata.EasternStartTime = start_user.ToString("f");
                    mydata.EasternEndTime = end_user.ToString("f");
                    //mydata.EasternEndTime = UtilityFunction.GetAMPMWithSpace(mydata.EasternEndTime);
                    //mydata.EasternStartTime = UtilityFunction.GetAMPMWithSpace(mydata.EasternStartTime);
                    if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date != DateTime.Now.Date)
                    {
                        maintenanceMessage = string.Format("{0} from {1} to {2} on {3}",
                        mydata.DashboardMessage, mydata.EasternStartTime, mydata.EasternEndTime, mydata.DowntimeStart.ToString("d"));
                        //mydata.DashboardMessage, mydata.EasternStartTime, mydata.EasternEndTime, mydata.DowntimeStart.ToString("dd-MM-yyyy"));
                    }
                    else if (mydata.DowntimeStart.Date == mydata.DowntimeEnd.Date && mydata.DowntimeStart.Date == DateTime.Now.Date)
                    {
                        maintenanceMessage = string.Format("{0} today from {1} to {2}",
                        mydata.DashboardMessage, mydata.EasternStartTime, mydata.EasternEndTime);
                    }
                    else
                    {
                        mydata = objSiteMainWrapper.GetNextSitemaintenance("n");
                        maintenanceMessage = string.Format("{0} from {1} {2} to {3} {4}",
                        mydata.DashboardMessage, mydata.DowntimeStart.ToString("d"), mydata.EasternStartTime, mydata.DowntimeEnd.ToString("d"), mydata.EasternEndTime);
                        //mydata.DashboardMessage, mydata.DowntimeStart.ToString("dd-MM-yyyy"), mydata.EasternStartTime, mydata.DowntimeEnd.ToString("dd-MM-yyyy"), mydata.EasternEndTime);
                    }
                    */
                }

            }
            else
            {
                maintenanceMessage = string.Empty;
            }
            Session["SitetenanceMessage"] = maintenanceMessage;
            var jsonResult = Json(maintenanceMessage, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        #region V2-491 Password Expiration
        public JsonResult GetPasswordExpirationMessage()
        {

            PasswordExpirationWrapper objPasswordExpiWrapper = new PasswordExpirationWrapper(userData);

            string passwordexpireMessage = string.Empty;
            if (Session["PasswordExpirationMessage"] == null)
            {
                var mydata = objPasswordExpiWrapper.GetPasswordExpirationDays();
                if (mydata > 0 && mydata <= 10)
                {
                    passwordexpireMessage = string.Format("Your password will expire in {0} days ", mydata);

                }

            }
            else
            {
                passwordexpireMessage = string.Empty;
            }
            Session["PasswordExpirationMessage"] = passwordexpireMessage;
            var jsonResult = Json(passwordexpireMessage, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion

        #region V2-533 Equipment chunk lookup list
        public string GetEquipmentLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "", string name = "", string model = "", string type = "", string AssetGroup1ClientLookupId = "", string AssetGroup2ClientLookupId = "", string AssetGroup3ClientLookupId = "", string serialNumber = "", bool InactiveFlag = true)
        {
            List<EquipmentLookupModel> modelList = new List<EquipmentLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            model = !string.IsNullOrEmpty(model) ? model.Trim() : string.Empty;
            type = !string.IsNullOrEmpty(type) ? type.Trim() : string.Empty;
            AssetGroup1ClientLookupId = !string.IsNullOrEmpty(AssetGroup1ClientLookupId) ? AssetGroup1ClientLookupId.Trim() : string.Empty;
            AssetGroup2ClientLookupId = !string.IsNullOrEmpty(AssetGroup2ClientLookupId) ? AssetGroup2ClientLookupId.Trim() : string.Empty;
            AssetGroup3ClientLookupId = !string.IsNullOrEmpty(AssetGroup3ClientLookupId) ? AssetGroup3ClientLookupId.Trim() : string.Empty;

            serialNumber = !string.IsNullOrEmpty(serialNumber) ? serialNumber.Trim() : string.Empty;

            modelList = commonWrapper.GetEquipmentLookupListGridData(order, orderDir, skip, length.Value, clientLookupId, name, model, serialNumber, type, AssetGroup1ClientLookupId, AssetGroup2ClientLookupId, AssetGroup3ClientLookupId);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }
        #endregion


        #region Vendor chunk lookup list
        public string GetVendorLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "",
            string name = "", string addressCity = "", string addressState = "", bool InactiveFlag = true)
        {
            List<VendorLookupModel> modelList = new List<VendorLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            //V2-759
            addressCity = !string.IsNullOrEmpty(addressCity) ? addressCity.Trim() : string.Empty;
            addressState = !string.IsNullOrEmpty(addressState) ? addressState.Trim() : string.Empty;

            modelList = commonWrapper.GetVendorLookupListGridData(order, orderDir, skip, length.Value,
                clientLookupId, name, addressCity, addressState);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion

        #region V2-533 Workorder chunk LookupList 
        public string GetWorkOrderLookupListChunkSearch(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string ChargeTo = "", string WorkAssigned = "", string Requestor = "", string Status = "")
        {
            List<WorkOrderLookUpModel> modelList = new List<WorkOrderLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            ChargeTo = !string.IsNullOrEmpty(ChargeTo) ? ChargeTo.Trim() : string.Empty;
            WorkAssigned = !string.IsNullOrEmpty(WorkAssigned) ? WorkAssigned.Trim() : string.Empty;
            Requestor = !string.IsNullOrEmpty(Requestor) ? Requestor.Trim() : string.Empty;
            Status = !string.IsNullOrEmpty(Status) ? Status.Trim() : string.Empty;

            modelList = commonWrapper.GetWorkOrderLookupListGridData(order, orderDir, skip, length.Value, ClientLookupId, Description, ChargeTo, WorkAssigned, Requestor, Status);
            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }

        #endregion
     
        #region Part Chunk Lookuplist
        public string GetPartLookupListchunksearch(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string UPCcode = "", string Manufacturer = "", string ManufacturerId = "", string StockType = "", string Storeroomid = "")
        {
            List<PartXRefGridDataModel> modelList = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            UPCcode = !string.IsNullOrEmpty(UPCcode) ? UPCcode.Trim() : string.Empty;
            Manufacturer = !string.IsNullOrEmpty(Manufacturer) ? Manufacturer.Trim() : string.Empty;
            ManufacturerId = !string.IsNullOrEmpty(ManufacturerId) ? ManufacturerId.Trim() : string.Empty;
            StockType = !string.IsNullOrEmpty(StockType) ? StockType.Trim() : string.Empty;
            modelList = commonWrapper.GetPartLookupListGridData(order, orderDir, skip, length.Value,
            ClientLookupId, Description, UPCcode, Manufacturer, ManufacturerId, StockType, Storeroomid);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion

        #region Export

        //  creating group header row
        public Row CreateGroupHeader(string headername, int count)
        {
            Row tRow = new Row();

            Cell c1 = new Cell();
            c1.DataType = ResolveCellDataTypeOnValue(headername);
            c1.CellValue = new CellValue(headername);
            c1.StyleIndex = 3U;
            tRow.Append(c1);
            //this logic will generate blank cells for group header
            for (int i = 0; i < count - 1; i++)
            {
                Cell c2 = new Cell();
                c2.DataType = CellValues.String;
                c2.CellValue = new CellValue("");
                c2.StyleIndex = 3U;
                tRow.Append(c2);
            }

            return tRow;
        }

        //check for cell input data
        public EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }

        //creating cells based on text passed
        public Cell CreateCell(string text, uint styleIndex)
        {
            if (ResolveCellDataTypeOnValue(text) == CellValues.Number)
            {

                Cell cell = new Cell();
                cell.StyleIndex = styleIndex;
                cell.DataType = ResolveCellDataTypeOnValue(text);
                cell.CellValue = new CellValue(Convert.ToDouble(text));
                return cell;
            }
            else
            {
                Cell cell = new Cell();
                cell.StyleIndex = styleIndex;
                cell.DataType = ResolveCellDataTypeOnValue(text);
                cell.DataType = CellValues.InlineString;
                cell.InlineString = new InlineString() { Text = new Text(text) };
                cell.CellValue = new CellValue(text);
                return cell;
            }
        }
        //   

        //creating row based on style index 
        //by default 1U means no darker cell
        //2u means darker cells
        public Row CreateRowData(List<string> rowCells, int MaxgridColCount = 0, UInt32 styleIndex = 1U)
        {
            Row workRow = new Row();
            int missingcells = MaxgridColCount - rowCells.Count;

            //this logic will generate blank cells for row for better ui
            if (MaxgridColCount > rowCells.Count)
            {
                for (int i = 0; i < missingcells; i++)
                {
                    rowCells.Add("");
                }
            }
            foreach (var celldata in rowCells)
            {
                workRow.Append(CreateCell(celldata, styleIndex));
            }
            return workRow;
        }

        //stylesheet for excel sheet
        public void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1, bool Hasbigrows = false)
        {
            //set different style attribute for sheet
            Stylesheet stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            Fonts fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            Font font1 = new Font();
            FontSize fontSize1 = new FontSize() { Val = 11D };
            Color color1 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName1 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            Font font2 = new Font();
            Bold bold1 = new Bold();
            FontSize fontSize2 = new FontSize() { Val = 11D };
            Color color2 = new Color() { Theme = (UInt32Value)1U };
            FontName fontName2 = new FontName() { Val = "Calibri" };
            FontFamilyNumbering fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            FontScheme fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

            font2.Append(bold1);
            font2.Append(fontSize2);
            font2.Append(color2);
            font2.Append(fontName2);
            font2.Append(fontFamilyNumbering2);
            font2.Append(fontScheme2);

            fonts1.Append(font1);
            fonts1.Append(font2);

            Fills fills1 = new Fills() { Count = (UInt32Value)3U };

            Fill fill1 = new Fill();
            PatternFill patternFill1 = new PatternFill() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            Fill fill2 = new Fill();
            PatternFill patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

            Fill fill3 = new Fill();
            PatternFill patternFill3 = new PatternFill() { PatternType = PatternValues.Solid };
            // patternFill3.BackgroundColor = new BackgroundColor() { Rgb = HexBinaryValue.FromString("70AD47") };

            fill3.Append(patternFill3);

            fill2.Append(patternFill2);

            fills1.Append(fill1);
            fills1.Append(fill2);

            Borders borders1 = new Borders() { Count = (UInt32Value)2U };

            Border border1 = new Border();
            LeftBorder leftBorder1 = new LeftBorder();
            RightBorder rightBorder1 = new RightBorder();
            TopBorder topBorder1 = new TopBorder();
            BottomBorder bottomBorder1 = new BottomBorder();
            DiagonalBorder diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            Border border2 = new Border();

            LeftBorder leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            Color color3 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color3);

            RightBorder rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            Color color4 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color4);

            TopBorder topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            Color color5 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color5);

            BottomBorder bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            Color color6 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color6);
            DiagonalBorder diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            borders1.Append(border1);
            borders1.Append(border2);

            CellStyleFormats cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)3U };
            CellFormat cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };
            //if conten has bigger size row we add style for wrap text 
            // otherwise no wrapping the text
            if (Hasbigrows)
            {
                cellFormat1.Alignment = new Alignment() { WrapText = true };
            }

            cellStyleFormats1.Append(cellFormat1);


            CellFormats cellFormats1 = new CellFormats() { Count = (UInt32Value)4U };
            CellFormat cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            if (Hasbigrows)
            {
                cellFormat2.Alignment = new Alignment() { WrapText = true };
            }

            CellFormat cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
            if (Hasbigrows)
            {
                cellFormat3.Alignment = new Alignment() { WrapText = true };
            }

            CellFormat cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };
            if (Hasbigrows)
            {
                cellFormat4.Alignment = new Alignment() { WrapText = true };
            }

            CellFormat cellFormat5 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };
            if (Hasbigrows)
            {
                cellFormat5.Alignment = new Alignment() { WrapText = true, Horizontal = HorizontalAlignmentValues.Center };
            }
            else
            {
                cellFormat5.Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center };
            }

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);
            cellFormats1.Append(cellFormat5);

            CellStyles cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            CellStyle cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);
            DifferentialFormats differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            TableStyles tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            StylesheetExtensionList stylesheetExtensionList1 = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            //X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            //  stylesheetExtension1.Append(slicerStyles1);

            StylesheetExtension stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            //X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

            //stylesheetExtension2.Append(timelineStyles1);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);

            workbookStylesPart1.Stylesheet = stylesheet1;
        }

        //Generate style sheet layout ui on this method
        public void GenerateWorksheetPartContent(WorksheetPart worksheetPart1, SheetData sheetData1, List<int> groupRowIndexes, int TotalHeaderColumn, bool hasBigRow = false)
        {
            Worksheet worksheet = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            SheetDimension sheetDimension1 = new SheetDimension() { Reference = "A1" };

            SheetViews sheetViews1 = new SheetViews();

            SheetView sheetView1 = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            Selection selection1 = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

            sheetView1.Append(selection1);

            sheetViews1.Append(sheetView1);
            SheetFormatProperties sheetFormatProperties1;

            //if content has bigsize row we make column width fixed 
            //to handle wrap the text and prvent column cell bigger in height
            if (hasBigRow)
            {
                sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D, DefaultColumnWidth = 25D };
            }
            else
            {
                sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D };
            }
            PageMargins pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            worksheet.Append(sheetDimension1);
            worksheet.Append(sheetViews1);
            worksheet.Append(sheetFormatProperties1);
            worksheet.Append(sheetData1);
            worksheet.Append(pageMargins1);
            worksheetPart1.Worksheet = worksheet;

            //This logic is used to merge the cells of 
            //group heaer and align it to center of the all columns
            if (groupRowIndexes != null && groupRowIndexes.Count > 0)
            {
                MergeCells mergeCells = new MergeCells();
                string lastcolumn = getExportColumnList().ElementAt(TotalHeaderColumn - 1);
                foreach (var index in groupRowIndexes)
                {
                    string reference = string.Empty;
                    reference = "A" + index.ToString() + ":" + lastcolumn + index.ToString();
                    mergeCells.Append(new MergeCell() { Reference = new StringValue(reference) });
                }

                worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
            }
        }

        //generate workbook
        public void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
        {
            Workbook workbook1 = new Workbook();
            Sheets sheets1 = new Sheets();
            Sheet sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets1.Append(sheet1);
            workbook1.Append(sheets1);
            workbookPart1.Workbook = workbook1;
        }

        private List<string> getExportColumnList()
        {
            List<string> columnlist = new List<string>();
            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                columnlist.Add(letter.ToString());
            }
            return columnlist;
        }

        #endregion

        #region Project chunk lookup list
        public string GetProjectLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "", string description = "")
        {
            List<ProjectLookupModel> modelList = new List<ProjectLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            description = !string.IsNullOrEmpty(description) ? description.Trim() : string.Empty;

            modelList = commonWrapper.GetProjectLookupListGridData(order, orderDir, skip, length.Value, clientLookupId, description);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion

        #region V2-637 Asset Lookup for Repairable Spare Asset
        public string GetRepSpareAssetLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "", string name = "", string make = "", string model = "", string type = "", bool InactiveFlag = true, bool IsAssigned = true)
        {
            List<RepariableSpareAssetLookupModel> modelList = new List<RepariableSpareAssetLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            name = !string.IsNullOrEmpty(name) ? name.Trim() : string.Empty;
            make = !string.IsNullOrEmpty(make) ? make.Trim() : string.Empty;
            model = !string.IsNullOrEmpty(model) ? model.Trim() : string.Empty;
            type = !string.IsNullOrEmpty(type) ? type.Trim() : string.Empty;

            modelList = commonWrapper.GetEquipmentRepSpareAssetLookupListGridData(order, orderDir, skip, length.Value, clientLookupId, name, make, model, type, IsAssigned);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = modelList });
        }
        #endregion

        #region V2-676 QRCode Detail
        [HttpPost]
        public ActionResult LoadQRCode(string WorkorderClientLookupId)
        {
            string QRCodeImagePath = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(WorkorderClientLookupId, QRCodeGenerator.ECCLevel.H);
                QRCode qrCode = new QRCode(qrCodeData);
                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    QRCodeImagePath = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }
            return Json(QRCodeImagePath, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Retrieve Part Id By StoreroomId And ClientLookUp

        [HttpGet]
        public JsonResult GetPartIdByStoreroomIdAndClientLookUpforMultiStoreroom(string ClientLookupId = "", string StoreroomId = "")
        {
            var commonWrapper = new CommonWrapper(userData);
            var part = commonWrapper.RetrievePartIdByStoreroomIdAndClientLookUp(ClientLookupId, StoreroomId);
            return Json(new { PartId = part.PartId, MultiStoreroomError = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region V2-716 Multiple File-Upload

        [HttpPost]
        public ActionResult SaveMultipleUploadedFileToServer(string fileName, long objectId, string TableName, string AttachObjectName)
        {
            string imageurl = string.Empty;
            string sasToken = string.Empty;
            bool OnPremise = userData.DatabaseKey.Client.OnPremise;
            int result = 0;
            string outcomes = "";
            CommonWrapper objCommonWrapper = new CommonWrapper(userData);
            if (fileName.Contains('.'))
            {
                int index = fileName.IndexOf('.');
                outcomes = fileName.Substring(0, index);

            }
            int Duplicate_Check = objCommonWrapper.AttachmentCount_ByObjectAndFileName(objectId, TableName, outcomes);
            if (Duplicate_Check > 0)
            {
                result = 1;
            }
            else
            {
                if (OnPremise)
                {
                    imageurl = UploadMultipleNewImageToOnPremise(fileName, objectId, TableName, AttachObjectName, out result);
                }
                else
                {
                    imageurl = UploadMultipleNewImageToAzure(fileName, objectId, TableName, AttachObjectName);
                }
                try
                {
                    string pathString = TempData["ImageDirectory"].ToString();
                    bool isExists = Directory.Exists(pathString);
                    if (isExists)
                    {
                        Directory.Delete(pathString, true);
                    }
                }
                catch (Exception ex)
                { }
            }
            var jsonResult = Json(new { imageurl = imageurl, result = result });
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string UploadMultipleNewImageToAzure(string fileName, long objectId, string TableName, string AttachObjectName)
        {
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            string uploadedURL = string.Empty;
            string imageurl = string.Empty;
            string imagePath = string.Empty;
            string content_type = string.Empty;


            //HttpPostedFileBase file_chk = Request.Files[fileName];
            //var originalDirectory_chk = new DirectoryInfo(string.Format("{0}UploadedImages\\images", Server.MapPath(@"\write\")));
            //string pathString = Path.Combine(originalDirectory_chk.ToString(), userData.SessionId.ToString());
            //bool isExists = Directory.Exists(pathString);
            //if (!isExists)
            //{
            //    #region Directory Exists or not
            //    Directory.CreateDirectory(pathString);
            //    #endregion
            //    var path = string.Format("{0}\\{1}", pathString, file_chk.FileName);
            //    file_chk.SaveAs(path);
            //}

            var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            System.IO.FileInfo[] files = originalDirectory.GetFiles();
            foreach (var file in files)
            {
                if (file.Name == fileName && (TableName.ToLower() != "partmaster"))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                    content_type = MimeMapping.GetMimeMapping(file.FullName);

                    if (TableName.ToLower() != "partmasterrequest")
                    {
                        imagePath = aset.CreateFileNamebyObject(TableName, objectId.ToString(), fileName);
                        uploadedURL = aset.UploadToAzureBlob(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imagePath, bytes, content_type);
                        SaveMultipleToDatabase(file, objectId, AttachObjectName, uploadedURL);
                        imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, uploadedURL);
                    }
                    else if (TableName.ToLower() == "partmasterrequest")
                    {
                        imagePath = aset.CreateFileNamebyObject("PartMasterRequestImage", objectId.ToString(), fileName);
                        DataContracts.PartMasterRequest pmr = new DataContracts.PartMasterRequest()
                        {
                            ClientId = userData.DatabaseKey.Client.ClientId,
                            PartMasterRequestId = objectId
                        };

                        pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                        uploadedURL = aset.UploadToAzureBlob(pmr.ClientId, pmr.SiteId, imagePath, bytes, content_type);
                        SaveMultiplePartMasterRequest(pmr, uploadedURL);// should be removed later
                        SaveMultipleToDatabase(file, objectId, AttachObjectName, uploadedURL);
                        imageurl = aset.GetSASUrlClientSite(pmr.ClientId, pmr.SiteId, uploadedURL);
                    }
                }
                else if (file.Name == fileName && (TableName.ToLower() == "partmaster"))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(file.FullName);
                    content_type = MimeMapping.GetMimeMapping(file.FullName);
                    imagePath = aset.CreateFileNamebyObject(TableName, objectId.ToString(), fileName);
                    uploadedURL = aset.UploadToAzureBlob(userData.DatabaseKey.Client.ClientId, imagePath, bytes, content_type);
                    SaveMultipleToDatabase(file, objectId, AttachObjectName, uploadedURL);
                    imageurl = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId, uploadedURL);
                }
            }
            return imageurl;

        }
        private string UploadMultipleNewImageToOnPremise(string fileName, long objectId, string TableName, string AttachObjectName, out int result)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string imageurl = string.Empty;
            string Filepath = string.Empty;

            int ConnectRemoteShareErrorCode = 0;

            NetworkCredential credentials = UtilityFunction.GetOnPremiseCredential();

            var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images\\{1}", Server.MapPath(@"\write\"), userData.SessionId.ToString()));
            var OnPremisePath = UtilityFunction.GetOnPremiseDirectory();

            using (new ConnectToSharedFolder(OnPremisePath.NetworkPath, credentials, out ConnectRemoteShareErrorCode))
            {
                Filepath = Path.Combine(OnPremisePath.NetworkPath, OnPremisePath.RemoteDrivePath);
                if (ConnectRemoteShareErrorCode == 0)
                {
                    System.IO.FileInfo[] files = originalDirectory.GetFiles();
                    string DBFilePath = string.Empty;
                    foreach (var file in files)
                    {
                        if (file.Name == fileName && (TableName.ToLower() != "partmaster"))
                        {

                            if (TableName.ToLower() != "partmasterrequest")
                            {
                                Filepath = commonWrapper.UploadFileOnPremise(Filepath, objectId, TableName, out DBFilePath);
                                DBFilePath = Path.Combine(DBFilePath, fileName);
                                Filepath = Path.Combine(Filepath, file.Name);
                                file.CopyTo(Filepath, true);
                                SaveMultipleToDatabase(file, objectId, AttachObjectName, DBFilePath);
                                imageurl = Filepath;

                            }
                            else if (TableName.ToLower() == "partmasterrequest")
                            {
                                DataContracts.PartMasterRequest pmr = new DataContracts.PartMasterRequest()
                                {
                                    ClientId = userData.DatabaseKey.Client.ClientId,
                                    PartMasterRequestId = objectId
                                };
                                pmr.RetrieveByPKForeignKeys(userData.DatabaseKey, userData.Site.TimeZone);
                                Filepath = commonWrapper.UploadFileOnPremise(Filepath, objectId, TableName, out DBFilePath);
                                DBFilePath = Path.Combine(DBFilePath, fileName);
                                Filepath = Path.Combine(Filepath, file.Name);
                                file.CopyTo(Filepath, true);
                                SaveMultiplePartMasterRequest(pmr, Filepath);// should be removed later
                                SaveMultipleToDatabase(file, objectId, AttachObjectName, DBFilePath);
                                imageurl = Filepath;

                            }
                        }
                        else if (file.Name == fileName && (TableName.ToLower() == "partmaster"))
                        {
                            Filepath = commonWrapper.UploadFileOnPremise(Filepath, objectId, TableName, out DBFilePath);
                            DBFilePath = Path.Combine(DBFilePath, fileName);
                            Filepath = Path.Combine(Filepath, file.Name);
                            file.CopyTo(Filepath, true);
                            SaveMultipleToDatabase(file, objectId, AttachObjectName, DBFilePath);
                            imageurl = Filepath;
                        }

                    }
                }

            }
            result = ConnectRemoteShareErrorCode;
            if (ConnectRemoteShareErrorCode == 0)
            {
                return UtilityFunction.PhotoBase64ImgSrc(imageurl);
            }
            else
            {
                return "";
            }


        }

        private string UploadMultipleImageToAzure(byte[] uploadedFile, long objectId, string TableName)
        {
            string rtrData = string.Empty;
            AzureBlob ablob = new AzureBlob();
            AzureSetup aset = new AzureSetup();

            if (uploadedFile.Length > 1)
            {
                Int64 Clientid = userData.DatabaseKey.Client.ClientId;
                Int64 Siteid = userData.DatabaseKey.User.DefaultSiteId;
                string imgName = TableName + "_" + DateTime.Now.Ticks.ToString() + "." + "jpg";
                string fileName = aset.CreateFileNamebyObject(TableName, objectId.ToString(), imgName);
                rtrData = aset.ConnectToAzureBlob(Clientid, Siteid, fileName, uploadedFile);
            }

            return rtrData;
        }
        private void SaveMultipleToDatabase(System.IO.FileInfo fileInfo, long objectId, string attachObjectName, string attachmentURL)
        {
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = attachObjectName,
                ObjectId = objectId,
                Profile = true,
                Image = true
            };
            //List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);

            //if (AList.Count > 0)
            //{
            //    attach.AttachmentId = AList.First().AttachmentId;
            //    attach.Retrieve(userData.DatabaseKey);
            //    attach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            //    attach.Description = "Profile Image";
            //    attach.FileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            //    attach.FileType = Path.GetExtension(fileInfo.Name).Remove(0, 1);
            //    attach.ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName);
            //    attach.FileSize = Convert.ToInt32(fileInfo.Length);
            //    attach.Image = true;
            //    attach.Profile = true;
            //    attach.External = false;
            //    attach.Reference = false;
            //    attach.AttachmentURL = attachmentURL;
            //    attach.Update(userData.DatabaseKey);
            //}
            //else
            //{
            attach.ObjectName = attachObjectName;
            attach.ObjectId = objectId;
            attach.UploadedBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            //attach.Description = "Profile Image";
            attach.FileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
            attach.FileType = Path.GetExtension(fileInfo.Name).Remove(0, 1);
            attach.ContentType = MimeMapping.GetMimeMapping(fileInfo.FullName);
            attach.FileSize = Convert.ToInt32(fileInfo.Length);
            attach.Image = true;
            attach.Profile = false;
            attach.External = false;
            attach.Reference = false;
            attach.AttachmentURL = attachmentURL;
            attach.Create(userData.DatabaseKey);
            //}
        }
        private void SaveMultiplePartMasterRequest(PartMasterRequest pmr, string imageUrl)
        {
            pmr.ImageURL = imageUrl == null ? "" : imageUrl;
            pmr.Update(this.userData.DatabaseKey);
        }

        public JsonResult SetImageAsDefault(long AttachmentId, string objectId, string TableName)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
            string isSuccess = string.Empty;
            string imageurl = string.Empty;
            comWrapper.SetAsDefaultImage(AttachmentId, Convert.ToInt64(objectId), TableName, ref isSuccess, ref imageurl);
            if (userData.DatabaseKey.Client.OnPremise)
            {
                imageurl = UtilityFunction.PhotoBase64ImgSrc(imageurl);
            }
            else
            {
                imageurl = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imageurl);
            }
            var jsonResult = Json(new { imageurl = imageurl, result = isSuccess.ToLower() }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult DeleteMultipleImageFromAzure(long AttachmentId, string objectId, string TableName)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteAzureMultipleImage(AttachmentId, Convert.ToInt64(objectId), TableName, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteMultipleImageFromOnPremise(long AttachmentId, string objectId, string TableName)
        {
            CommonWrapper comWrapper = new CommonWrapper(userData);
            string isSuccess = string.Empty;
            comWrapper.DeleteOnPremiseMultipleImage(AttachmentId, Convert.ToInt64(objectId), TableName, ref isSuccess);
            return Json(isSuccess.ToLower(), JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region PartCategoryMaster chunk lookup list
        public string GetPartCategoryMasterLookupListchunksearch(int? draw, int? start, int? length, string clientLookupId = "",
            string Description = "")
        {
            List<Models.Configuration.CategoryMaster.CategoryMasterModel> modelList = new List<Models.Configuration.CategoryMaster.CategoryMasterModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            clientLookupId = !string.IsNullOrEmpty(clientLookupId) ? clientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;

            Client.BusinessWrapper.Configuration.CategoryMasterWrapper cWrapper = new Client.BusinessWrapper.Configuration.CategoryMasterWrapper(userData);
            modelList = cWrapper.GetPartCategoryLookupList(order, length ?? 0, orderDir, skip, clientLookupId, Description);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion

        #region V2-726
        public List<DataModel> Get_ApproverList(long RequestorId, string RequestType, int Level)
        {
            LookUpListModel model = new LookUpListModel();
            AppGroupApprovers personnel = new AppGroupApprovers()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                RequestorId = RequestorId,
                RequestType = RequestType,
                Level = Level
            };

            List<AppGroupApprovers> PersonnelList = personnel.RetrieveApproversForApproval(this.userData.DatabaseKey);

            return model.data = ReturnApproverList(PersonnelList);
        }
        private List<DataModel> ReturnApproverList(List<AppGroupApprovers> PersonnelList)
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            foreach (var p in PersonnelList)
            {
                dModel = new DataModel();

                dModel.ApproverId = p.ApproverId;
                dModel.ApproverName = p.ApproverName;
                model.data.Add(dModel);
            }
            return model.data;
        }
        public long RetrieveApprovalGroupIdByRequestorIdAndRequestType(string RequestType)
        {
            AppGroupRequestors appGroupRequestors = new AppGroupRequestors()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                RequestorId = this.userData.DatabaseKey.Personnel.PersonnelId,
                RequestType = RequestType
            };

            var records = appGroupRequestors.RetrieveByRequestorIdAndRequestType(this.userData.DatabaseKey);
            return records.ApprovalGroupId;
        }
        #endregion
        #region V2-730
        public List<DataModel> Get_MultiLevelApproverList(long ApproverId, string RequestType, long ObjectId, long ApprovalGroupId)
        {
            LookUpListModel model = new LookUpListModel();
            AppGroupApprovers personnel = new AppGroupApprovers()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ApproverId = ApproverId,
                ObjectId = ObjectId,
                RequestType = RequestType,
                ApprovalGroupId = ApprovalGroupId
            };

            List<AppGroupApprovers> PersonnelList = personnel.RetrieveApproversForMultiLevelApproval(this.userData.DatabaseKey);

            return model.data = ReturnMultiLevelApproverList(PersonnelList);
        }
        private List<DataModel> ReturnMultiLevelApproverList(List<AppGroupApprovers> PersonnelList)
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;
            foreach (var p in PersonnelList)
            {
                dModel = new DataModel();

                dModel.ApproverId = p.ApproverId;
                dModel.ApproverName = p.ApproverName;
                model.data.Add(dModel);
            }
            return model.data;
        }
        #endregion

        #region V2-791
        //For Native Camera Picture Upload
        [HttpPost]
        public ActionResult SaveUploadedFile_Mobile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        var originalDirectory = new DirectoryInfo(string.Format("{0}UploadedImages\\images", Server.MapPath(@"\write\")));
                        string pathString = Path.Combine(originalDirectory.ToString(), userData.SessionId.ToString());
                        bool isExists = Directory.Exists(pathString);
                        if (!isExists)
                            Directory.CreateDirectory(pathString);

                        var path = string.Format("{0}\\{1}", pathString, /*file.FileName*/fileName);
                        TempData["ImageDirectory"] = pathString;
                        file.SaveAs(path);
                    }
                }
            }
            catch (Exception)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
    //
    #endregion
    #region V2-989
    public List<DataModel> GetAllPartManagement_Personnel()
    {
      LookUpListModel model = new LookUpListModel();
      Personnel personnel = new Personnel()
      {
        ClientId = this.userData.DatabaseKey.Client.ClientId,
        SiteId = this.userData.DatabaseKey.User.DefaultSiteId
      };

      List<Personnel> PersonnelList = personnel.RetrievePartManagementForLookupList(this.userData.DatabaseKey);

      return model.data = ReturnPersonnelList(PersonnelList);
    }
    #endregion
    #region V2-798
    public List<DataModel> GetAllActiveList_Personnel()
        {
            LookUpListModel model = new LookUpListModel();
            Personnel personnel = new Personnel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Personnel> PersonnelList = personnel.RetrieveAllActiveForLookupList(this.userData.DatabaseKey);

            return model.data = ReturnPersonnelList(PersonnelList);
        }
        #endregion

        #region V2-853 Delete GridDataLayout
        public JsonResult DeleteState(string GridName)
        {
            GridStateWrapper gridStateWrapper = new GridStateWrapper(userData);
            var deleteResult = gridStateWrapper.DeleteState(GridName);
            if (deleteResult)
            {
                return Json(new { Result = JsonReturnEnum.success.ToString() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-929
        [HttpPost]
        public string GetActiveAdminOrFullPersonnelLookupListGridData(int? draw, int? start, int? length, string ClientLookupId = "", string NameFirst = "", string NameLast = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper _VendorObj = new CommonWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<PersonnelLookUpModel> pList = _VendorObj.GetActiveAdminOrFullPersonnelLookupListGridData(order, orderDir, skip, length ?? 0, ClientLookupId, NameFirst, NameLast);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pList != null && pList.Count > 0)
            {
                recordsFiltered = pList[0].TotalCount;
                totalRecords = pList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        #endregion

        #region V2-948
        [HttpGet]
        public JsonResult GetAccountByEquipmentId(long EquipmentId)
        {
            CommonWrapper objcommonWrapper = new CommonWrapper(userData);
            Equipment equipment = objcommonWrapper.GetAccountByEquipmentId(EquipmentId);
            return Json(new { data = equipment }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-950
        [HttpPost]
        public string GetActiveAdminOrFullPlannerPersonnelLookupListGridData(int? draw, int? start, int? length, string ClientLookupId = "", string NameFirst = "", string NameLast = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper _VendorObj = new CommonWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<PersonnelLookUpModel> pList = _VendorObj.GetActiveAdminOrFullPlannerPersonnelLookupListGridData(order, orderDir, skip, length ?? 0, ClientLookupId, NameFirst, NameLast);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (pList != null && pList.Count > 0)
            {
                recordsFiltered = pList[0].TotalCount;
                totalRecords = pList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = pList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }

        public string GetMeterLookupListGridData(int? draw, int? start, int? length, string ClientLookupId = "", string Name = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper _VendorObj = new CommonWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<MetersModel> mList = _VendorObj.GetMeterLookupListGridData(order, orderDir, skip, length ?? 0, ClientLookupId, Name);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (mList != null && mList.Count > 0)
            {
                recordsFiltered = mList[0].TotalCount;
                totalRecords = mList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = mList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        #endregion



        #region  Client Notification Message 993
        public JsonResult GetClientMessage()
        {
            ClientWrapper objClientWrapper = new ClientWrapper(userData);
            List<ClientMessageModel> _lstClientMessageNotification = new List<ClientMessageModel>();
            var lstmydata = objClientWrapper.GetClientMessage(userData.DatabaseKey.Client.ClientId);
            List<ClientMessage> listOfAllClientMeassge = lstmydata.listOfAllClientMeassge;

           if (Session["ClientMessage"] == null)
            {
                //Get All Messages
                foreach (var mydata in listOfAllClientMeassge)
                {
                    ClientMessageModel clientMessageModel = new ClientMessageModel
                    {
                        ClientId = mydata.ClientId,
                        Message = mydata.Message
                    };
                    _lstClientMessageNotification.Add(clientMessageModel);
                }
            }

            else
            {
                _lstClientMessageNotification.Clear();
            }

            Session["ClientMessage"] = _lstClientMessageNotification;
            var jsonResult = Json(_lstClientMessageNotification, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }
        #endregion


        #region V2-981
        #region Purchase Order chunk lookup list
        public string GetPurchaseOrderLookupListchunksearch(int? draw, int? start, int? length, string poclientLookupId = "",
            string status = "", string vendorClientLookupId = "", string vendorName = "")
        {
            List<PurchaseOrderLookupModel> modelList = new List<PurchaseOrderLookupModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            poclientLookupId = !string.IsNullOrEmpty(poclientLookupId) ? poclientLookupId.Trim() : string.Empty;
            status = !string.IsNullOrEmpty(status) ? status.Trim() : string.Empty;
            vendorClientLookupId = !string.IsNullOrEmpty(vendorClientLookupId) ? vendorClientLookupId.Trim() : string.Empty;
            vendorName = !string.IsNullOrEmpty(vendorName) ? vendorName.Trim() : string.Empty;


            modelList = commonWrapper.GetPurchaseOrderLookupListGridData(order, orderDir, skip, length.Value,
                poclientLookupId, status, vendorClientLookupId, vendorName);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion

        #region Personnel chunk lookup list
        public string GetPersonnelLookupListGridData(int? draw, int? start, int? length, string PersonnelClientLookupId = "", string PersonnelNameFirst = "", string PersonnelNameLast = "")

        {
            List<PersonnelLookUpModel> modelList = new List<PersonnelLookUpModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);

            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;

            PersonnelClientLookupId = !string.IsNullOrEmpty(PersonnelClientLookupId) ? PersonnelClientLookupId.Trim() : string.Empty;
            PersonnelNameFirst = !string.IsNullOrEmpty(PersonnelNameFirst) ? PersonnelNameFirst.Trim() : string.Empty;
            PersonnelNameLast = !string.IsNullOrEmpty(PersonnelNameLast) ? PersonnelNameLast.Trim() : string.Empty;

            modelList = commonWrapper.GetPersonnelLookupListGridData(order, orderDir, skip, length ?? 0, PersonnelClientLookupId, PersonnelNameFirst, PersonnelNameLast);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion

        #endregion

        #region V2-536
        public string GetActiveSensorProcedureAlertLookupListGridData(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string Type = "")
        {
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            List<SensorAlertModel> SAPList = commonWrapper.GetActiveSensorAlertProcedureLookupListGridData(order, orderDir, skip, length ?? 0, ClientLookupId, Description, Type);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (SAPList != null && SAPList.Count > 0)
            {
                recordsFiltered = SAPList[0].TotalCount;
                totalRecords = SAPList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;
            var filteredResult = SAPList
              .ToList();

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = filteredResult }, JsonSerializer12HoursDateAndTimeSettings);

        }
        #endregion

        #region V2-538
        public PartialViewResult GetAPMNotification()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<NotificationModel> NotificationModelList = new List<NotificationModel>();
            NotificationModelVM objNotificationModelVM = new NotificationModelVM();
            objNotificationModelVM.PageNumber = 0;
            int UnreadCount = 0;
            int UnreadSelectedtabCount = 0;
            NotificationModelList = commonWrapper.GetNotificationList("APM", ref UnreadCount, ref UnreadSelectedtabCount);
            objNotificationModelVM.NotificationList = NotificationModelList;
            objNotificationModelVM.NewNotificationCount = UnreadCount;
            objNotificationModelVM.NewNotificationSelectedtabCount = UnreadSelectedtabCount;
            return PartialView("_APMNotification", objNotificationModelVM);
        }
        #endregion

        #region V2-846 Equipment Tree Grid
        public string EquipmentHierarchyTreeGrid(int? draw, int? start, int? length, string SearchText = "")
        {
            start = start.HasValue
          ? start / length
          : 0;
            int skip = start * length ?? 0;
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var equipmentList = commonWrapper.GetAllParentChunkList(skip, length ?? 0, SearchText);
            var totalRecords = 0;
            var recordsFiltered = 0;
            if (equipmentList != null && equipmentList.Count > 0)
            {
                recordsFiltered = equipmentList[0].TotalCount;
                totalRecords = equipmentList[0].TotalCount;
            }
            else
            {
                recordsFiltered = 0;
                totalRecords = 0;
            }
            int initialPage = start.Value;

            var filteredResult = equipmentList
              .ToList();
            EquipmentTreeGridModel eSearchModel;
            List<EquipmentTreeGridModel> eSearchModelList = new List<EquipmentTreeGridModel>();
            foreach (var item in filteredResult)
            {
                eSearchModel = new EquipmentTreeGridModel();
                eSearchModel.EquipmentId = item.EquipmentId;
                eSearchModel.ParentId = item.ParentId;
                eSearchModel.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                eSearchModel.ChildCount = item.ChildCount;
                eSearchModelList.Add(eSearchModel);
            }

            return JsonConvert.SerializeObject(new { draw = draw, recordsTotal = totalRecords, recordsFiltered = recordsFiltered, data = eSearchModelList }, JsonSerializerDateSettings);
        }
        public JsonResult GetEquipmentTreeChildGridData(long EquipmentId)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var equipmentList = commonWrapper.GetAllChildrenForEquipmentTree(EquipmentId);
            EquipmentTreeGridModel eSearchModel;
            List<EquipmentTreeGridModel> eSearchModelList = new List<EquipmentTreeGridModel>();
            foreach (var item in equipmentList)
            {
                eSearchModel = new EquipmentTreeGridModel();
                eSearchModel.EquipmentId = item.EquipmentId;
                eSearchModel.ParentId = item.ParentId;
                eSearchModel.ClientLookupId = item.ClientLookupId + " ( " + item.Name + " )";
                eSearchModel.ChildCount = item.ChildCount;
                eSearchModelList.Add(eSearchModel);
            }
            return Json(eSearchModelList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region V2-1086 ShipToAddress 
        public List<DataModel> GetLookupList_ShipToAddress()
        {
            LookUpListModel model = new LookUpListModel();
            DataModel dModel;

            ShipTo_RetrieveAll trans = new ShipTo_RetrieveAll
            {
                CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                CallerUserName = this.userData.DatabaseKey.UserName,
            };
            trans.dbKey = this.userData.DatabaseKey.ToTransDbKey();
            trans.Execute();

            if (trans != null && trans.ShipToList != null)
            {
                foreach (b_ShipTo v in trans.ShipToList)
                {
                    dModel = new DataModel();
                    ShipTo shipto = new ShipTo();
                    shipto.UpdateFromDatabaseObject(v);
                    dModel.ShipToId = shipto.ShipToId;
                    dModel.ClientLookUpId = shipto.ClientLookupId;
                    dModel.Name = shipto.AttnName;
                    model.data.Add(dModel);
                }
            }

            return model.data;

        }
        #endregion

        #region V2-1167 Part Chunk Lookuplist For SingleStockLineItem
        public string GetPartLookupListchunksearchForSingleStockLineItem(int? draw, int? start, int? length, string ClientLookupId = "", string Description = "", string UPCcode = "", string Manufacturer = "", string ManufacturerId = "", string StockType = "", string Storeroomid = "", string VendorId = "")
        {
            List<PartXRefGridDataModel> modelList = new List<PartXRefGridDataModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];

            start = start.HasValue
              ? start / length
              : 0;
            int skip = start * length ?? 0;

            ClientLookupId = !string.IsNullOrEmpty(ClientLookupId) ? ClientLookupId.Trim() : string.Empty;
            Description = !string.IsNullOrEmpty(Description) ? Description.Trim() : string.Empty;
            UPCcode = !string.IsNullOrEmpty(UPCcode) ? UPCcode.Trim() : string.Empty;
            Manufacturer = !string.IsNullOrEmpty(Manufacturer) ? Manufacturer.Trim() : string.Empty;
            ManufacturerId = !string.IsNullOrEmpty(ManufacturerId) ? ManufacturerId.Trim() : string.Empty;
            StockType = !string.IsNullOrEmpty(StockType) ? StockType.Trim() : string.Empty;
            modelList = commonWrapper.GetPartLookupListGridDataForSingleStockLineItem(order, orderDir, skip, length.Value,
            ClientLookupId, Description, UPCcode, Manufacturer, ManufacturerId, StockType, Storeroomid, VendorId);

            long totalRecords = 0;
            long recordsFiltered = 0;
            if (modelList != null && modelList.Count > 0)
            {
                totalRecords = modelList[0].TotalCount;
                recordsFiltered = modelList[0].TotalCount;
            }

            return JsonConvert.SerializeObject(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = recordsFiltered,
                data = modelList
            });
        }
        #endregion
    }
}
