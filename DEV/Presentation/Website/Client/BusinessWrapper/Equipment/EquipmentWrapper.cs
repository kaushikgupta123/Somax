using Client.Common;
using Client.Localization;
using Client.Models;
using Client.Models.Configuration.Account;
using Client.Models.Equipment;
using Client.Models.Equipment.UIConfiguration;
using Client.ViewModels;
using Common.Constants;
using Database.Business;

using DataContracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Client.BusinessWrapper
{
    public class EquipmentWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public EquipmentWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region GetEquipments
        public Equipment GetEquipmentDetailsById(long EquipmentId)
        {
            DataContracts.Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };           
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);
            if (equipment.Maint_WarrantyExpire != null && equipment.Maint_WarrantyExpire != default(DateTime))
            {
                equipment.Maint_WarrantyExpire = equipment.Maint_WarrantyExpire;
            }
            else
            {
                equipment.Maint_WarrantyExpire = null;
            }
            if (equipment.AcquiredDate != null && equipment.AcquiredDate != default(DateTime))
            {
                equipment.AcquiredDate = equipment.AcquiredDate;
            }
            else
            {
                equipment.AcquiredDate = null;
            }
            if (equipment.InstallDate != null && equipment.InstallDate != default(DateTime))
            {
                equipment.InstallDate = equipment.InstallDate;
            }
            else
            {
                equipment.InstallDate = null;
            }
            return equipment;
        }

        public EquipmentModel GetEditEquipmentDetailsById(long EquipmentId)
        {
            EquipmentModel objEquipment = new EquipmentModel();
            DataContracts.Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };           
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);
            objEquipment = initializeControls(equipment);
            return objEquipment;
        }
        public EquipmentModel initializeControls(Equipment obj)
        {
            EquipmentModel objEquipment = new EquipmentModel();
            objEquipment.EquipmentID = Convert.ToString(obj.EquipmentId);
            objEquipment.Name = obj.Name;
            objEquipment.Location = obj.Location;
            objEquipment.DeptID = obj.DepartmentId;
            objEquipment.LineID = obj.LineId;
            objEquipment.SystemInfoId = obj.SystemInfoId;
            objEquipment.SerialNumber = obj.SerialNumber;
            objEquipment.Type = obj.Type;
            objEquipment.Make = obj.Make;
            objEquipment.ModelNumber = obj.Model;
            objEquipment.Account = obj.LaborAccountClientLookupId;
            objEquipment.ParentIdClientLookupId = obj.ParentIdClientLookupId;
            objEquipment.AssetNumber = obj.AssetNumber;
            objEquipment.InactiveFlag = obj.InactiveFlag;
            objEquipment.HiddenInactiveFlag = obj.InactiveFlag;
            objEquipment.CriticalFlag = obj.CriticalFlag;
            if (obj.Maint_WarrantyExpire != null && obj.Maint_WarrantyExpire != default(DateTime))
            {
                objEquipment.Maint_WarrantyExpire = obj.Maint_WarrantyExpire;
            }
            else
            {
                objEquipment.Maint_WarrantyExpire = null;
            }

            objEquipment.MaintVendorIdClientLookupId = obj.MaintVendorIdClientLookupId;
            objEquipment.Maint_WarrantyDesc = obj.Maint_WarrantyDesc;
            objEquipment.AssetCategory = obj.AssetCategory;
            objEquipment.ClientLookupId = obj.ClientLookupId;
            objEquipment.AssetGroup1Id = obj.AssetGroup1;
            objEquipment.AssetGroup2Id = obj.AssetGroup2;
            objEquipment.AssetGroup3Id = obj.AssetGroup3;
            return objEquipment;
        }   
        public List<DataContracts.Equipment> RetrieveequipmentDetailsByInactiveFlag(int InActiveFlag)
        {
            DataContracts.Equipment equipment = new DataContracts.Equipment();
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            bool inactiveFlag = InActiveFlag == 1 ? false : true;
            equipment.InactiveFlag = inactiveFlag;
            List<DataContracts.Equipment> equipmentList = new List<Equipment>();
            equipmentList = equipment.RetrieveAllBySiteId_V2(userData.DatabaseKey);
            return equipmentList;
        }

        public List<DataContracts.Equipment> GetEquipmentGridData(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string location = "", string serialNo = "", string type = "", string make = "", string modelNo = "", string account = "", string assetNumber = "", string searchText = "", int AssetGroup1Id = 0, int AssetGroup2Id = 0, int AssetGroup3Id = 0, string AssetAvailability = "")
        {
            DataContracts.Equipment equipment = new DataContracts.Equipment();

            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.CustomQueryDisplayId = CustomQueryDisplayId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = name;
            equipment.Location = location;
            equipment.SerialNo = serialNo;
            equipment.Type = type;
            equipment.Make = make;
            equipment.ModelNo = modelNo;
            equipment.Account = account;
            equipment.AssetNumber = assetNumber;
            equipment.SearchText = searchText;
            //<!--(Added on 25/06/2020)-->
            equipment.AssetGroup1Id = AssetGroup1Id;
            equipment.AssetGroup2Id = AssetGroup2Id;
            equipment.AssetGroup3Id = AssetGroup3Id;
            //<!--(Added on 25/06/2020)-->
            equipment.AssetAvailability = AssetAvailability;//V2-636 <!--(Added on 21/01/2022)-->
            equipment.EquipmentRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            //<!--(Added on 26/06/2020)-->
            return equipment.listOfEquipment;
        }
        #endregion

        #region Add-Update
     
        public List<AssetGroup1Model> GetAssetGroup1Dropdowndata()
        {
            AssetGroup1 assetGroup1 = new AssetGroup1()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup1.RetrieveAssetGroup1ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup1Model assetGroup1Model;
            List<AssetGroup1Model> AssetGroup1ModelList = new List<AssetGroup1Model>();
            foreach (var item in retData)
            {
                assetGroup1Model = new AssetGroup1Model();
                assetGroup1Model.AssetGroup1Id = item.AssetGroup1Id;
                assetGroup1Model.AssetGroup1DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup1ModelList.Add(assetGroup1Model);
            }
            return AssetGroup1ModelList;
        }
        public List<AssetGroup1Model> GetAllAssetGroup1Dropdowndata()
        {
            AssetGroup1 assetGroup1 = new AssetGroup1()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup1.RetrieveAllAssetGroup1ByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup1Model assetGroup1Model;
            List<AssetGroup1Model> AssetGroup1ModelList = new List<AssetGroup1Model>();
            foreach (var item in retData)
            {
                assetGroup1Model = new AssetGroup1Model();
                assetGroup1Model.AssetGroup1Id = item.AssetGroup1Id;
                assetGroup1Model.AssetGroup1DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup1ModelList.Add(assetGroup1Model);
            }
            return AssetGroup1ModelList;
        }
        public List<AssetGroup2Model> GetAssetGroup2Dropdowndata()
        {
            AssetGroup2 assetGroup2 = new AssetGroup2()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup2.RetrieveAssetGroup2ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup2Model assetGroup2Model;
            List<AssetGroup2Model> AssetGroup2ModelList = new List<AssetGroup2Model>();
            foreach (var item in retData)
            {
                assetGroup2Model = new AssetGroup2Model();
                assetGroup2Model.AssetGroup2Id = item.AssetGroup2Id;
                assetGroup2Model.AssetGroup2DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup2ModelList.Add(assetGroup2Model);
            }
            return AssetGroup2ModelList;
        }
        public List<AssetGroup2Model> GetAllAssetGroup2Dropdowndata()
        {
            AssetGroup2 assetGroup2 = new AssetGroup2()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup2.RetrieveAllAssetGroup2ByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup2Model assetGroup2Model;
            List<AssetGroup2Model> AssetGroup2ModelList = new List<AssetGroup2Model>();
            foreach (var item in retData)
            {
                assetGroup2Model = new AssetGroup2Model();
                assetGroup2Model.AssetGroup2Id = item.AssetGroup2Id;
                assetGroup2Model.AssetGroup2DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup2ModelList.Add(assetGroup2Model);
            }
            return AssetGroup2ModelList;
        }
        public List<AssetGroup3Model> GetAssetGroup3Dropdowndata()
        {
            AssetGroup3 assetGroup3 = new AssetGroup3()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup3.RetrieveAssetGroup3ByByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup3Model assetGroup3Model;
            List<AssetGroup3Model> AssetGroup3ModelList = new List<AssetGroup3Model>();
            foreach (var item in retData)
            {
                assetGroup3Model = new AssetGroup3Model();
                assetGroup3Model.AssetGroup3Id = item.AssetGroup3Id;
                assetGroup3Model.AssetGroup3DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup3ModelList.Add(assetGroup3Model);
            }
            return AssetGroup3ModelList;
        }
        public List<AssetGroup3Model> GetAllAssetGroup3Dropdowndata()
        {
            AssetGroup3 assetGroup3 = new AssetGroup3()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                InactiveFlag = false
            };
            var retData = assetGroup3.RetrieveAllAssetGroup3ByInActiveFlag_V2(this.userData.DatabaseKey);
            AssetGroup3Model assetGroup3Model;
            List<AssetGroup3Model> AssetGroup3ModelList = new List<AssetGroup3Model>();
            foreach (var item in retData)
            {
                assetGroup3Model = new AssetGroup3Model();
                assetGroup3Model.AssetGroup3Id = item.AssetGroup3Id;
                assetGroup3Model.AssetGroup3DescWithClientLookupId = item.ClientLookup_Desc;
                AssetGroup3ModelList.Add(assetGroup3Model);
            }
            return AssetGroup3ModelList;
        }
        public Equipment AddEquipment(string EQ_ClientLookupId, EquipmentCombined objEM, EquipmentCombined objComb)
        {
            Equipment equipment = new Equipment();
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientLookupId = EQ_ClientLookupId;
            equipment.Name = objEM.EquipModel.Name;
            equipment.Type = objEM.EquipModel.Type;
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            equipment.Location = objEM.EquipModel.Location;
            equipment.SerialNumber = objEM.EquipModel.SerialNumber;
            equipment.Make = objEM.EquipModel.Make;
            equipment.Model = objEM.EquipModel.ModelNumber;
            equipment.LaborAccountClientLookupId = objEM.EquipModel.Account;
            equipment.AssetNumber = objEM.EquipModel.AssetNumber;
            equipment.PlantLocationDescription = objEM.EquipModel.PlantLocationDescription;
            equipment.InactiveFlag = objEM.EquipModel.InactiveFlag;
            equipment.CriticalFlag = objEM.EquipModel.CriticalFlag;
            equipment.Maint_WarrantyExpire = objEM.EquipModel.Maint_WarrantyExpire;
            equipment.MaintVendorIdClientLookupId = objEM.EquipModel.MaintVendorIdClientLookupId;
            equipment.Maint_WarrantyDesc = objEM.EquipModel.Maint_WarrantyDesc;
            equipment.CreateMode = true;
            equipment.AssetGroup1 = objEM.EquipModel.AssetGroup1Id ?? 0;
            equipment.AssetGroup2 = objEM.EquipModel.AssetGroup2Id ?? 0;
            equipment.AssetGroup3 = objEM.EquipModel.AssetGroup3Id ?? 0;
            equipment.AssetCategory = objEM.EquipModel.AssetCategory;
            equipment.CreateByPKForeignKeys_V2(this.userData.DatabaseKey);
            return equipment;
        }
        public Equipment UpdateEquipment(EquipmentCombined equip)
        {
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(equip.EquipModel.EquipmentID)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);

            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientLookupId = equip.EquipModel.ClientLookupId;
            equipment.InactiveFlag = equip.EquipModel.InactiveFlag;
            equipment.CriticalFlag = equip.EquipModel.CriticalFlag;
            equipment.LaborAccountClientLookupId = !string.IsNullOrEmpty(equip.EquipModel.Account) ? equip.EquipModel.Account.Trim() : emptyValue;
            equipment.Location = equip.EquipModel.Location != null ? equip.EquipModel.Location : emptyValue;
            equipment.AssetNumber = equip.EquipModel.AssetNumber != null ? equip.EquipModel.AssetNumber : emptyValue;
            equipment.MaintVendorIdClientLookupId = !string.IsNullOrEmpty(equip.EquipModel.MaintVendorIdClientLookupId) ? equip.EquipModel.MaintVendorIdClientLookupId.Trim() : emptyValue;
            equipment.Maint_WarrantyDesc = equip.EquipModel.Maint_WarrantyDesc != null ? equip.EquipModel.Maint_WarrantyDesc : emptyValue;
            equipment.Maint_WarrantyExpire = equip.EquipModel.Maint_WarrantyExpire;
            equipment.Make = equip.EquipModel.Make != null ? equip.EquipModel.Make : emptyValue;
            equipment.Model = equip.EquipModel.ModelNumber != null ? equip.EquipModel.ModelNumber : emptyValue;
            equipment.Name = equip.EquipModel.Name;
            equipment.SerialNumber = equip.EquipModel.SerialNumber != null ? equip.EquipModel.SerialNumber : emptyValue;
            equipment.Type = equip.EquipModel.Type != null ? equip.EquipModel.Type : emptyValue;
            equipment.CreateMode = false;
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            equipment.AssetGroup1 = equip.EquipModel.AssetGroup1Id ?? 0;
            equipment.AssetGroup2 = equip.EquipModel.AssetGroup2Id ?? 0;
            equipment.AssetGroup3 = equip.EquipModel.AssetGroup3Id ?? 0;
            equipment.AssetCategory = equip.EquipModel.AssetCategory != null ? equip.EquipModel.AssetCategory : emptyValue;
            equipment.UpdateByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.PlantLocationDescription = equip.EquipModel.PlantLocationDescription != null ? equip.EquipModel.PlantLocationDescription : emptyValue;

            return equipment;
        }

        public Equipment ValidateEquStatusChange(long equipId, string flag, string clientLookupId)
        {
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = equipId,
                Flag = flag,
                ClientLookupId = clientLookupId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            equipment.CheckEquipmentIsInactivateorActivate(userData.DatabaseKey);
            return equipment;
        }
        public List<string> UpdateEquActiveStatus(long equipId, bool inActiveFlag)
        {
            List<string> errList = new List<string>();
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = equipId
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.InactiveFlag = !inActiveFlag;
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;           
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            #region V2-1133
            if (equipment.InactiveFlag)
            {
                equipment.RemoveFromService = true;
                equipment.RemoveFromServiceReason = RemoveFromServiceReasonCodeConstant.NotinUse;
                equipment.RemoveFromServiceDate = DateTime.UtcNow;
                equipment.RemoveFromServiceReasonCode = RemoveFromServiceReasonCodeConstant.NotinUse;
                equipment.Status = AssetStatusConstant.NotinUse;
                if (equipment.ExpectedReturnToService != null && equipment.ExpectedReturnToService == DateTime.MinValue)
                {
                    equipment.ExpectedReturnToService = null;
                }
            }
            else
            {
                equipment.RemoveFromService = false;
                equipment.RemoveFromServiceDate = null;
                equipment.ExpectedReturnToService = null;
                equipment.RemoveFromServiceReason = "";
                equipment.RemoveFromServiceDate = null;
                equipment.RemoveFromServiceReasonCode = "";
                equipment.Status = AssetStatusConstant.InService;
            }
            #endregion
            equipment.UpdateByPKForeignKeys_V2(userData.DatabaseKey);
            if (equipment.ErrorMessages == null || equipment.ErrorMessages.Count <= 0)
            {
                errList = UpdatePmScheduleRecords(equipId, inActiveFlag);
                return errList;
            }
            else
            {
                return equipment.ErrorMessages;
            }
           
        }
        private List<string> UpdatePmScheduleRecords(long equipId, bool inActiveFlag)
        {
            List<string> errList = new List<string>();
            PrevMaintSched pm = new PrevMaintSched()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                ChargeToId = equipId
            };

            List<PrevMaintSched> equipmentPrevMaintSchedList = PrevMaintSched.RetriveByEquipmentId(this.userData.DatabaseKey, pm);
            Parallel.ForEach(equipmentPrevMaintSchedList, v =>
             {
                 PrevMaintSched prevmaintsched = new PrevMaintSched()
                 {
                     ClientId = userData.DatabaseKey.Client.ClientId,
                     SiteId = userData.DatabaseKey.User.DefaultSiteId,
                     PrevMaintMasterId = v.PrevMaintMasterId,
                     PrevMaintSchedId = v.PrevMaintSchedId
                 };

                 prevmaintsched.RetrieveByForeignKeys(userData.DatabaseKey);
                 if (prevmaintsched.InactiveFlag != !inActiveFlag)
                 {
                     prevmaintsched.InactiveFlag = !inActiveFlag;
                     prevmaintsched.UpdateByForeignKeys(userData.DatabaseKey);
                     if (prevmaintsched.ErrorMessages != null && prevmaintsched.ErrorMessages.Count > 0)
                     {
                         foreach (var ev in prevmaintsched.ErrorMessages)
                         {
                             errList.Add(ev);
                         }
                     }
                 }

             });
            return errList;
        }
        public List<string> CreateAssetEvent(long equipId, string Event,string Comments)
        {
            AssetEventLog assetEventLog = new AssetEventLog();
            assetEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            assetEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetEventLog.EquipmentId = equipId;
            assetEventLog.TransactionDate = DateTime.UtcNow;            
            assetEventLog.Event = Event;                    
            assetEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            assetEventLog.Comments = Comments;
            assetEventLog.SourceId = 0;
            assetEventLog.Create(userData.DatabaseKey);
            return assetEventLog.ErrorMessages;
        }
        #region V2-1133
        public List<string> CreateAssetAvailability(long equipId, string Event, string ReasonCode)
        {
            AssetAvailabilityLog assetAvailabilityLog = new AssetAvailabilityLog();
            assetAvailabilityLog.ClientId = userData.DatabaseKey.User.ClientId;
            assetAvailabilityLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetAvailabilityLog.ObjectId = equipId;
            assetAvailabilityLog.TransactionDate = DateTime.UtcNow;
            assetAvailabilityLog.Event = Event;
            assetAvailabilityLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            assetAvailabilityLog.ReturnToService = null;
            assetAvailabilityLog.Reason = "";
            assetAvailabilityLog.ReasonCode = ReasonCode;
            assetAvailabilityLog.SourceId= 0;
            assetAvailabilityLog.Create(userData.DatabaseKey);
            return assetAvailabilityLog.ErrorMessages;
        }
        #endregion
        public List<String> ChangeEquipmentId(ChangeEquipmentIDModel _ChangeEquipmentIDModel)
        {
            List<string> EMsg = new List<string>();
            if (_ChangeEquipmentIDModel.EquipmentId > 0)
            {
                Equipment equip = new Equipment();
                equip.ClientId = userData.DatabaseKey.Client.ClientId;
                equip.SiteId = userData.DatabaseKey.Personnel.SiteId;
                equip.EquipmentId = _ChangeEquipmentIDModel.EquipmentId;
                equip.Lookup_ListName = LookupListConstants.Equipment_EquipType;
                equip.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
                equip.ClientLookupId = _ChangeEquipmentIDModel.ClientLookupId;
                equip.CreateMode = true;
                equip.ChangeClientLookupId(userData.DatabaseKey);
                if (equip.ErrorMessages.Count == 0)
                {
                    string Event = "ChangeID";
                    string Comments = "Previous Asset ID – " + _ChangeEquipmentIDModel.OldClientLookupId;
                    EMsg = CreateAssetEvent(_ChangeEquipmentIDModel.EquipmentId, Event, Comments);                  
                }
                else
                {
                    EMsg = equip.ErrorMessages;

                }
            }
            return EMsg;
        }
       
        public List<String> DeleteEquipment(long _eqid, ref string Result)
        {
            
            EquipmentCombined objComb = new EquipmentCombined();
            List<string> EMsg = new List<string>();

            if (_eqid > 0)
            {
                DataContracts.Equipment equipment = new DataContracts.Equipment()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EquipmentId = _eqid,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId
                };
                equipment.CheckEquipmentIsParentofanother(userData.DatabaseKey);

                if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                {
                    Result = "parentexist";
                    EMsg = equipment.ErrorMessages;
                }
                else
                {
                    if (equipment.DeleteEquipment(userData.DatabaseKey))
                    {
                        Result = "update";
                    }
                    else
                    {
                        Result = "delete";
                    }
                }
            }
            else
            {
                Result = JsonReturnEnum.failed.ToString();
            }
            return EMsg;
        }
        public List<Equipment> updateBasedOnCriteria(List<long> list, string Type)
        {
            System.Data.DataTable lulist = new DataTable();
            lulist.Columns.Add("EquipIds", typeof(string));

            foreach (var item in list)
            {
                long ids = item;
                lulist.Rows.Add(ids);

            }

            Equipment eqList = new Equipment();
            eqList.ClientId = this.userData.DatabaseKey.Client.ClientId;
            eqList.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            eqList.Type = Type;
            eqList.EquipIds = lulist;
            List<DataContracts.Equipment> equipmentList = eqList.RetrieveAllBasedOnCriteria(this.userData.DatabaseKey);
            return equipmentList;
        }
        public bool IsDepartMentActive(long DepartmentId)
        {
            Department dp = new Department();
            dp.ClientId = userData.DatabaseKey.Client.ClientId;
            dp.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            dp.DepartmentId = DepartmentId;
            dp.RetrieveByDepartmentId(userData.DatabaseKey);
            return dp.InactiveFlag;
        }
        public bool IsLineActive(long LineId)
        {
            Line Ln = new Line()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                LineId = LineId
            };
            Ln.RetrieveByLineId(userData.DatabaseKey);
            return Ln.InactiveFlag;
        }
        public bool IsSystemActive(long SystemInfo)
        {
            SystemInfo sysInfo = new SystemInfo()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                SystemInfoId = SystemInfo
            };
            sysInfo.RetrieveBySystemInfoId(userData.DatabaseKey);
            return sysInfo.InactiveFlag;
        }
        #endregion

        #region WO_Active
        public List<WOActiveModel> getDetailsWOActive(bool lActive, long EquipmentId)
        {

            WorkOrder workorder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeToId = EquipmentId,
                ChargeType = ChargeType.Equipment
            };
            List<WorkOrder> equipmentWorkOrderList = WorkOrder.RetriveByEquipmentId(this.userData.DatabaseKey, workorder, lActive);
            List<WOActiveModel> WOActiveModelList = new List<WOActiveModel>();
            WOActiveModel objWOActiveModel;

            if (equipmentWorkOrderList != null)
            {
                var eqpList = equipmentWorkOrderList.Select(x => new { x.ClientLookupId, x.Description, x.WorkAssigned_PersonnelClientLookupId, x.Status_Display, x.Type, x.CreateDate, x.WorkOrderId }).ToList();

                foreach (var v in eqpList)
                {
                    objWOActiveModel = new WOActiveModel();
                    objWOActiveModel.ClientLookupId = v.ClientLookupId;
                    objWOActiveModel.CreateDate = v.CreateDate;
                    objWOActiveModel.Description = v.Description;
                    objWOActiveModel.Status_Display = v.Status_Display;
                    objWOActiveModel.Type = v.Type;
                    objWOActiveModel.WorkOrderId = v.WorkOrderId;
                    objWOActiveModel.WorkAssigned_PersonnelClientLookupId = v.WorkAssigned_PersonnelClientLookupId;
                    WOActiveModelList.Add(objWOActiveModel);
                }
            }

            return WOActiveModelList;
        }
        #endregion

        #region WO_Complete
        public List<WOComplete> getDetailsWOComplete(bool lActive, long EquipmentId)
        {

            WorkOrder workorder = new WorkOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                ChargeToId = EquipmentId,
                ChargeType = ChargeType.Equipment
            };
            List<WorkOrder> equipmentWorkOrderList = WorkOrder.RetriveByEquipmentId(this.userData.DatabaseKey, workorder, lActive);
            List<WOComplete> WOCompleteList = new List<WOComplete>();
            WOComplete objWOComplete;

            if (equipmentWorkOrderList != null)
            {
                var eqpList = equipmentWorkOrderList.Select(x => new { x.ClientLookupId, x.Description, x.WorkAssigned_PersonnelClientLookupId, x.Status_Display, x.Type, x.CreateDate, x.WorkOrderId }).ToList();

                foreach (var v in eqpList)
                {
                    objWOComplete = new WOComplete();
                    objWOComplete.ClientLookupId = v.ClientLookupId;
                    objWOComplete.CreateDate = v.CreateDate;
                    objWOComplete.Description = v.Description;
                    objWOComplete.Status_Display = v.Status_Display;
                    objWOComplete.Type = v.Type;
                    objWOComplete.WorkOrderId = v.WorkOrderId;
                    objWOComplete.WorkAssigned_PersonnelClientLookupId = v.WorkAssigned_PersonnelClientLookupId;
                    WOCompleteList.Add(objWOComplete);
                }

            }

            return WOCompleteList;
        }
        #endregion

        #region Additional Information
        public List<ChildrenGridDataModel> GetChildren(long EquipmentId)
        {
            ChildrenGridDataModel objChildrenGridDataModel;
            List<ChildrenGridDataModel> ChildrenGridDataModelList = new List<ChildrenGridDataModel>();
            Equipment equipment = new Equipment();
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.EquipmentId = EquipmentId;
            List<Equipment> equipmentList = equipment.GetAllEquipmentChildren(this.userData.DatabaseKey);

            if (equipmentList != null)
            {
                var eqpList = equipmentList.Select(x => new { x.ClientLookupId, x.EquipmentId, x.Name, x.SerialNumber, x.Type, x.Make, x.Model }).ToList();
                foreach (var eq in eqpList)
                {
                    objChildrenGridDataModel = new ChildrenGridDataModel();
                    objChildrenGridDataModel.ClientLookupId = eq.ClientLookupId;
                    objChildrenGridDataModel.EquipmentId = eq.EquipmentId;
                    objChildrenGridDataModel.Make = eq.Make;
                    objChildrenGridDataModel.Model = eq.Model;
                    objChildrenGridDataModel.Name = eq.Name;
                    objChildrenGridDataModel.SerialNumber = eq.SerialNumber;
                    objChildrenGridDataModel.Type = eq.Type;
                    ChildrenGridDataModelList.Add(objChildrenGridDataModel);
                }
            }
            return ChildrenGridDataModelList;
        }
        public List<PartIssues> GetEquipmentPartIssued(long EquipmentId)
        {
            List<PartIssues> PartIssuesList = new List<PartIssues>();
            PartIssues objPartIssues;

            PartHistory partHistory = new PartHistory()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.Site.SiteId,
                ChargeToId_Primary = EquipmentId,
            };
            List<PartHistory> equipmentPartHistoryList = partHistory.RetriveByEquipmentId(this.userData.DatabaseKey);
            if (equipmentPartHistoryList != null)
            {
                var eqpList = equipmentPartHistoryList.Select(x => new { x.PartClientLookupId, x.Description, x.TransactionDate, x.ChargeToClientLookupId, x.TransactionQuantity, x.UnitofMeasure, x.Cost, x.IssuedTo }).ToList();
                foreach (var v in eqpList)
                {
                    objPartIssues = new PartIssues();
                    objPartIssues.ChargeToClientLookupId = v.ChargeToClientLookupId;
                    objPartIssues.Cost = v.Cost;
                    objPartIssues.Description = v.Description;
                    objPartIssues.IssuedTo = v.IssuedTo;
                    objPartIssues.PartClientLookupId = v.PartClientLookupId;
                    if (v.TransactionDate != null && v.TransactionDate == default(DateTime))
                    {
                        objPartIssues.TransactionDate = null;
                    }
                    else
                    {
                        objPartIssues.TransactionDate = v.TransactionDate;
                    }

                    objPartIssues.TransactionQuantity = v.TransactionQuantity;
                    objPartIssues.UnitofMeasure = v.UnitofMeasure;
                    PartIssuesList.Add(objPartIssues);
                }
            }
            return PartIssuesList;
        }
        public List<PMListModel> GetEquipmentPMList(long EquipmentId)
        {
            List<PMListModel> PMListModelList = new List<PMListModel>();
            PrevMaintSched pm = new PrevMaintSched()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                ChargeToId = EquipmentId
            };

            List<PrevMaintSched> equipmentPrevMaintSchedList = PrevMaintSched.RetriveByEquipmentId(this.userData.DatabaseKey, pm);
            if (equipmentPrevMaintSchedList != null)
            {
                PMListModelList = equipmentPrevMaintSchedList.Select(v => new PMListModel()
                {
                    AssignedTo_PersonnelName = v.AssignedTo_PersonnelName,
                    PrevMaintMasterId = v.PrevMaintMasterId,
                    ClientLookupId = v.ClientLookupId,
                    Description = v.Description,
                    LastPerformed = v.LastPerformed == DateTime.MinValue ? null : v.LastPerformed,
                    LastScheduled = v.LastScheduled == DateTime.MinValue ? null : v.LastScheduled
                })
                .ToList();
            }
            return PMListModelList;
        }
        #endregion

        #region Children
        public void AddChildren(string listofIds, long equipmentId)
        {
            if (!String.IsNullOrEmpty(listofIds))
            {
                string[] list = listofIds.Split(',');
                foreach (var id in list)
                {
                    Equipment equipment = new Equipment();
                    equipment.ClientId = userData.DatabaseKey.Client.ClientId;
                    equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    equipment.EquipmentId = Convert.ToInt64(id);
                    equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
                    equipment.ParentId = equipmentId;
                    equipment.ProcessSystemId = 0;
                    equipment.Update(userData.DatabaseKey);
                }
            }
        }
        public bool DeleteChild(string _eqid)
        {
            try
            {
                Equipment child = new Equipment()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                    EquipmentId = Convert.ToInt64(_eqid),
                };
                child.Retrieve(userData.DatabaseKey);
                child.ParentId = 0;
                child.Update(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Sensor
        public List<SensorGridDataModel> GetEquipment_Sensor(long EquipmentId)
        {
            Equipment_Sensor_Xref Eq_Xref = new Equipment_Sensor_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            List<Equipment_Sensor_Xref> equipmentSensorList = Eq_Xref.RetriveByEquipmentId(this.userData.DatabaseKey);
            List<SensorGridDataModel> SensorGridDataModelList = new List<SensorGridDataModel>();
            SensorGridDataModel objSensorGridDataModel;
            if (equipmentSensorList.Count > 0)
            {
                foreach (var v in equipmentSensorList)
                {
                    objSensorGridDataModel = new SensorGridDataModel();
                    objSensorGridDataModel.SensorName = v.SensorName;
                    objSensorGridDataModel.Sensor = v.Sensor;
                    objSensorGridDataModel.AssignedTo_Name = v.AssignedTo_Name;
                    objSensorGridDataModel.EquipmentId = v.EquipmentId;
                    objSensorGridDataModel.LastReading = v.LastReading;
                    objSensorGridDataModel.SensorAlertProcedureClientLookUpId = v.SensorAlertProcedureClientLookUpId;
                    objSensorGridDataModel.SensorId = v.SensorId;
                    objSensorGridDataModel.TriggerHigh = v.TriggerHigh;
                    objSensorGridDataModel.TriggerLow = v.TriggerLow;
                    objSensorGridDataModel.Equipment_Sensor_XrefId = v.Equipment_Sensor_XrefId;
                    SensorGridDataModelList.Add(objSensorGridDataModel);
                }
            }
            return SensorGridDataModelList;
        }

        public List<Equipment_Sensor_Xref> BindSensors(long EquipmentId)
        {
            Equipment_Sensor_Xref Eq_Xref = new Equipment_Sensor_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            List<Equipment_Sensor_Xref> DataSource = Eq_Xref.RetriveByEquipmentId(this.userData.DatabaseKey);
            return DataSource;
        }

        public void AddNewSensor(string _SensorIds, long EquipmentId)
        {
            string[] array = _SensorIds.Split(',');
            foreach (var item in array)
            {
                MonnitSensor.Sensor Senr = new MonnitSensor.Sensor();
                List<MonnitSensor.SensorModel> sm = new List<MonnitSensor.SensorModel>();
                sm = Senr.SensorList(userData.DatabaseKey.Client.ClientId, userData.Site.SiteId).Where(a => a.SensorID == item).ToList();
                Equipment_Sensor_Xref EquiSensorData = new Equipment_Sensor_Xref();
                EquiSensorData.EquipmentId = EquipmentId;
                EquiSensorData.SensorId = Convert.ToInt32(item);
                EquiSensorData.SensorName = sm[0].SensorName;
                EquiSensorData.SensorAppId = sm[0].MonnitApplicationID;
                EquiSensorData.Create(userData.DatabaseKey);
            }
        }

        public List<SensorAlertProcedure> GetAlertProcList()
        {
            SensorAlertProcedure alertprocs = new SensorAlertProcedure()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            var AlertProcList = alertprocs.RetrieveAllForSensorAlertData(userData.DatabaseKey, userData.Site.TimeZone);
            return AlertProcList;
        }
        public Equipment_Sensor_Xref ShowEditSensor(long EquipmentId, long SensorId, long Equipment_Sensor_XrefId)
        {
            SensorGridDataModel objSensorGridDataModel = new SensorGridDataModel();
            Equipment_Sensor_Xref sensor_Xref = new Equipment_Sensor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                Equipment_Sensor_XrefId = Equipment_Sensor_XrefId
            };
            sensor_Xref.Retrieve(userData.DatabaseKey);
            return sensor_Xref;
        }

        public List<string> UpdateSensor(EquipmentCombined objComb)
        {
            List<string> errorList = new List<string>();

            Equipment_Sensor_Xref sensor_Xref = new Equipment_Sensor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                Equipment_Sensor_XrefId = objComb.sensorGridDataModel.Equipment_Sensor_XrefId
            };
            sensor_Xref.Retrieve(userData.DatabaseKey);
            sensor_Xref.EquipmentId = objComb.sensorGridDataModel.EquipmentId;
            sensor_Xref.SensorAlertProcedureId = objComb.sensorGridDataModel.SensorAlertProcedureId;
            sensor_Xref.AssignedTo_PersonnelId = objComb.sensorGridDataModel.AssignedTo_PersonnelId ?? 0;
            sensor_Xref.TriggerLow = objComb.sensorGridDataModel.TriggerLow;
            sensor_Xref.TriggerHigh = objComb.sensorGridDataModel.TriggerHigh;
            sensor_Xref.Update(userData.DatabaseKey);
            return sensor_Xref.ErrorMessages;
        }

        public bool SensorDelete(long _xref)
        {
            try
            {
                Equipment_Sensor_Xref sensor_Xref = new Equipment_Sensor_Xref()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    Equipment_Sensor_XrefId = _xref
                };
                sensor_Xref.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region TechSpecs

        public List<TechSpecs> GetTechSpecsList()
        {
            TechSpecs techspecs = new TechSpecs()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };

            List<TechSpecs> TechSpecsList = TechSpecs.RetriveBySiteId(userData.DatabaseKey, techspecs);
            return TechSpecsList;
        }
        public List<TechSpecsModel> GetEquipmentTechSpecs(long EquipmentId)
        {
            List<TechSpecsModel> TechSpecsModelList = new List<TechSpecsModel>();
            TechSpecsModel objTechSpecsModel;
            Equipment_TechSpecs eq = new Equipment_TechSpecs()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            List<Equipment_TechSpecs> equipmentTechSpecsList = Equipment_TechSpecs.RetriveByEquipmentId(this.userData.DatabaseKey, eq);
            if (equipmentTechSpecsList != null)
            {
                var eqpList = equipmentTechSpecsList.Select(x => new { x.EquipmentId, x.ClientLookupId, x.SpecValue, x.Description, x.Comments, x.UnitOfMeasure, x.TechSpecId, x.Equipment_TechSpecsId, x.UpdateIndex }).ToList();

                foreach (var v in eqpList)
                {
                    objTechSpecsModel = new TechSpecsModel();
                    objTechSpecsModel.ClientLookupId = v.ClientLookupId;
                    objTechSpecsModel.Comments = v.Comments;
                    objTechSpecsModel.Description = v.Description;
                    objTechSpecsModel.EquipmentId = v.EquipmentId;
                    objTechSpecsModel.SpecValue = v.SpecValue;
                    objTechSpecsModel.UnitOfMeasure = v.UnitOfMeasure;
                    objTechSpecsModel.TechSpecId = v.TechSpecId;
                    objTechSpecsModel.Equipment_TechSpecsId = v.Equipment_TechSpecsId;
                    objTechSpecsModel.TechSpecsSecurity = userData.Security.Equipment.Edit == true ? "true" : "false";
                    objTechSpecsModel.updatedindex = v.UpdateIndex;
                    TechSpecsModelList.Add(objTechSpecsModel);
                }
            }
            return TechSpecsModelList;
        }
        public bool DeleteTechSpecs(string _EquipmentTechSpecsId)
        {
            try
            {
                Equipment_TechSpecs eq = new Equipment_TechSpecs()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    Equipment_TechSpecsId = long.Parse(_EquipmentTechSpecsId)
                };
                eq.Retrieve(userData.DatabaseKey);
                eq.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TechSpecsModel TechSpecsAdd(long EquipmentId, TechSpecsModel model, List<DataContracts.LookupList> techSpecList, long TechSpecId = 0)
        {
            string Mode = model.Mode;
            if (Mode == "update")
            {
                Equipment_TechSpecs eq = new Equipment_TechSpecs()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    Equipment_TechSpecsId = TechSpecId
                };
                eq.Retrieve(userData.DatabaseKey);
                model.TechSpecId = eq.TechSpecId;
                model.Equipment_TechSpecsId = eq.Equipment_TechSpecsId;
                model.SpecValue = eq.SpecValue;
                model.Comments = String.IsNullOrEmpty(eq.Comments) ? String.Empty : eq.Comments;

                model.TechSpecsList = techSpecList.Where(p => p.LookupListId == model.TechSpecId).Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.LookupListId.ToString() });

            }
            else
            {
                Equipment_TechSpecs eqs = new Equipment_TechSpecs()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EquipmentId = EquipmentId
                };
                List<Equipment_TechSpecs> equipmentTechSpecsList = Equipment_TechSpecs.RetriveByEquipmentId(this.userData.DatabaseKey, eqs);

                model.TechSpecsList = techSpecList.Where(p => !equipmentTechSpecsList.Any(p2 => p2.TechSpecId == p.LookupListId)).Select(x => new SelectListItem { Text = x.ListValue + " - " + x.Description, Value = x.LookupListId.ToString() });
                
            }
            return model;
        }      

        public EquipmentCombined TechSpecsAddEdit(EquipmentCombined ec)
        {
            if (ec.techSpecsModel.Mode == "add")
            {
                Equipment_TechSpecs eq = new Equipment_TechSpecs()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                };
                eq.ClientId = this.userData.DatabaseKey.Client.ClientId;
                eq.EquipmentId = Convert.ToInt64(ec.techSpecsModel.EquipmentId);
                eq.TechSpecId = ec.techSpecsModel.TechSpecId;
                eq.SpecValue = ec.techSpecsModel.SpecValue;
                eq.Comments = String.IsNullOrEmpty(ec.techSpecsModel.Comments) ? String.Empty : ec.techSpecsModel.Comments;
                //Check if ClientLookupId exists
                eq.ValidateByTechSpecId(this.userData.DatabaseKey);

                foreach (string s in eq.ErrorMessages)
                {
                    errorMessage.Add(s);
                }

                if (errorMessage.Count == 0)
                {
                    eq.Create(this.userData.DatabaseKey);
                }
                else
                {
                    ec.techSpecsModel.ErrorMessage = errorMessage;
                }
            }
            else
            {
                Equipment_TechSpecs eq = new Equipment_TechSpecs()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    Equipment_TechSpecsId = ec.techSpecsModel.Equipment_TechSpecsId
                };
                eq.Retrieve(userData.DatabaseKey);
                eq.SpecValue = ec.techSpecsModel.SpecValue;
                eq.Comments = String.IsNullOrEmpty(ec.techSpecsModel.Comments) ? String.Empty : ec.techSpecsModel.Comments;
                eq.UpdateIndex = eq.UpdateIndex;
                eq.Update(this.userData.DatabaseKey);
            }
            return ec;
        }
        #endregion

        #region Part
        public List<PartsModel> GetEquipmentParts(long EquipmentId, string PartClientLookUpId, string Description, string StockType)
        {
            List<PartsModel> PartsModelList = new List<PartsModel>();
            Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId,
                PartClientLookUpId= PartClientLookUpId,
                Description= Description,
                StockType= StockType
            };
            List<Equipment_Parts_Xref> equipmentParts_XrefList = Equipment_Parts_Xref.RetrieveByEquipmentId_V2(this.userData.DatabaseKey, eq);
            if (equipmentParts_XrefList != null)
            {
                var eqpList = equipmentParts_XrefList.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                PartsModel objPartsModel;
                foreach (var v in eqpList)
                {
                    objPartsModel = new PartsModel();
                    objPartsModel.Comment = v.Comment;
                    objPartsModel.EquipmentId = v.EquipmentId;
                    objPartsModel.Part_ClientLookupId = v.Part_ClientLookupId;
                    objPartsModel.Part_Description = v.Part_Description;
                    objPartsModel.QuantityNeeded = v.QuantityNeeded;
                    objPartsModel.QuantityUsed = v.QuantityUsed;
                    objPartsModel.Equipment_Parts_XrefId = v.Equipment_Parts_XrefId;
                    objPartsModel.UpdatedIndex = v.UpdateIndex;
                    objPartsModel.PartId = v.PartId;
                    PartsModelList.Add(objPartsModel);
                }
            }
            foreach (var item in PartsModelList)
            {
                item.PartsSecurity = "true";
            }
            return PartsModelList;
        }
        public EquipmentCombined EditParts(long EquipmentId, long Equipment_Parts_XrefId, EquipmentCombined objCombi)
        {
            PartsSessionData objPartsSessionData = new PartsSessionData();
            Equipment_Parts_Xref EquipmentPart = new Equipment_Parts_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                Equipment_Parts_XrefId = Equipment_Parts_XrefId,
            };
            EquipmentPart.Retrieve(userData.DatabaseKey);
            Part pa = new Part();
            pa.ClientId = this.userData.DatabaseKey.Client.ClientId;
            pa.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            pa.PartId = EquipmentPart.PartId;
            pa.Retrieve(userData.DatabaseKey);
            objPartsSessionData.EquipmentId = EquipmentPart.EquipmentId;
            objPartsSessionData.Part = pa.ClientLookupId;
            objPartsSessionData.QuantityNeeded = EquipmentPart.QuantityNeeded;
            objPartsSessionData.QuantityUsed = EquipmentPart.QuantityUsed;
            objPartsSessionData.Comment = EquipmentPart.Comment;
            objPartsSessionData.UpdateIndex = EquipmentPart.UpdateIndex;
            objPartsSessionData.Equipment_Parts_XrefId = EquipmentPart.Equipment_Parts_XrefId;
            objCombi.partsSessionData = objPartsSessionData;
            return objCombi;
        }

        public EquipmentCombined UpdatePart(EquipmentCombined ec)
        {
            List<string> errorMessage = new List<string>();
            Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                Equipment_Parts_XrefId = ec.partsSessionData.Equipment_Parts_XrefId,
            };
            eq.Retrieve(userData.DatabaseKey);
            eq.QuantityNeeded = ec.partsSessionData.QuantityNeeded ?? 0;
            eq.QuantityUsed = ec.partsSessionData.QuantityUsed ?? 0;
            eq.Comment = String.IsNullOrEmpty(ec.partsSessionData.Comment) ? String.Empty : ec.partsSessionData.Comment;
            eq.UpdateIndex = Convert.ToInt32(ec.partsSessionData.UpdateIndex);
            eq.Update(this.userData.DatabaseKey);

            if (eq.ErrorMessages != null && eq.ErrorMessages.Count > 0)
            {
                foreach (string s in eq.ErrorMessages)
                {
                    errorMessage.Add(s);
                }

                ec.partsSessionData.ErrorMessage = errorMessage;
            }
            return ec;
        }
        public EquipmentCombined AddPart(EquipmentCombined ec)
        {
            List<string> errorMessage = new List<string>();
            Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = ec.partsSessionData.EquipmentId,
                Part_ClientLookupId = ec.partsSessionData.Part,
                ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                Comment = String.IsNullOrEmpty(ec.partsSessionData.Comment) ? String.Empty : ec.partsSessionData.Comment
            };
            Part pa = new Part();
            pa.ClientId = this.userData.DatabaseKey.Client.ClientId;
            pa.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            pa.ClientLookupId = ec.partsSessionData.Part;
            pa.RetrieveByClientLookUpIdNUPCCode(this.userData.DatabaseKey);
            eq.PartId = pa.PartId;
            eq.QuantityNeeded = ec.partsSessionData.QuantityNeeded ?? 0;
            eq.QuantityUsed = ec.partsSessionData.QuantityUsed ?? 0;
            eq.CreatePKForeignKeys(this.userData.DatabaseKey);
            if (eq.ErrorMessages != null && eq.ErrorMessages.Count > 0)
            {
                foreach (string s in eq.ErrorMessages)
                {
                    errorMessage.Add(s);
                }
                ec.partsSessionData.ErrorMessage = eq.ErrorMessages;
            }
            return ec;
        }

        public bool DeleteParts(string _EquipmentPartSpecsId)
        {
            try
            {
                Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    Equipment_Parts_XrefId = Convert.ToInt64(_EquipmentPartSpecsId)
                };
                eq.Retrieve(this.userData.DatabaseKey);
                eq.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Downtime
        public List<DownTimeModel> GetEquipmentDowntime(long EquipmentId)
        {
            List<DownTimeModel> DownTimeModelList = new List<DownTimeModel>();
            DownTimeModel objDownTimeModel;
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            List<Downtime> equipmentDowntimeList = Downtime.RetriveByEquipmentId(this.userData.DatabaseKey, downtime);
            if (equipmentDowntimeList != null)
            {
                var eqpList = equipmentDowntimeList.Select(x => new { x.EquipmentId, x.WorkOrderClientLookupId, x.DateDown, x.MinutesDown, x.DowntimeId, x.PersonnelClientLookupId }).ToList();
                foreach (var v in eqpList)
                {
                    objDownTimeModel = new DownTimeModel();
                    objDownTimeModel.DateDown = Convert.ToDateTime(v.DateDown);
                    objDownTimeModel.EquipmentId = v.EquipmentId;
                    objDownTimeModel.MinutesDown = v.MinutesDown;
                    objDownTimeModel.WorkOrderClientLookupId = v.WorkOrderClientLookupId;
                    objDownTimeModel.DowntimeId = v.DowntimeId;
                    objDownTimeModel.DowntimeCreateSecurity = userData.Security.Downtime.Create;
                    objDownTimeModel.DowntimeEditSecurity = userData.Security.Downtime.Edit;
                    objDownTimeModel.DowntimeDeleteSecurity = userData.Security.Downtime.Delete;
                    objDownTimeModel.PersonnelClientLookupId = v.PersonnelClientLookupId;
                    DownTimeModelList.Add(objDownTimeModel);
                }
            }
            return DownTimeModelList;
        }
        public List<string> AddDownTime(EquipmentCombined ec)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = ec.downTimeModel.EquipmentId,
                ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                PersonnelClientLookupId = ec.downTimeModel.PersonnelClientLookupId ?? string.Empty,
                WorkOrderClientLookupId = ec.downTimeModel.WorkOrderClientLookupId ?? string.Empty,
                MinutesDown = ec.downTimeModel.MinutesDown ?? 0,
                DateDown = ec.downTimeModel.DateDown.Value,
                ReasonForDown=ec.downTimeModel.ReasonForDown,
                WorkOrderId=0,
                Operator_PersonnelId = this.userData.DatabaseKey.Personnel.PersonnelId
            };
            downtime.CreatePKForeignKeys(this.userData.DatabaseKey);
            return downtime.ErrorMessages;
        }
        public bool DeleteDownTime(string _DowntimeId)
        {
            try
            {
                Downtime down = new Downtime()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    DowntimeId = long.Parse(_DowntimeId),
                };

                down.Retrieve(userData.DatabaseKey);
                down.Delete(this.userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EquipmentCombined ShowEditDownTime(long EquipmentId, long DownTimeId, EquipmentCombined objComb, DownTimeModel objDownTimeModel)
        {
            DowntimeSessionData _DowntimeSessionData = new DowntimeSessionData();

            Downtime downtime = new Downtime()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId,
                DowntimeId = DownTimeId
            };

            downtime.Retrieve(userData.DatabaseKey);          

            WorkOrder workOrder = new WorkOrder()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                WorkOrderId = downtime.WorkOrderId
            };
            if (downtime.WorkOrderId != 0)
                workOrder.Retrieve(userData.DatabaseKey);

            if (downtime != null)
            {
                objDownTimeModel.EquipmentId = EquipmentId;
                objDownTimeModel.DowntimeId = DownTimeId;             
                objDownTimeModel.DateDown = downtime.DateDown;
                objDownTimeModel.MinutesDown =  downtime.MinutesDown;
                objDownTimeModel.WorkOrderClientLookupId = workOrder.ClientLookupId;
                //V2-695
                objDownTimeModel.ReasonForDown=downtime.ReasonForDown;
            }
            objComb.downTimeModel = objDownTimeModel;
            return objComb;
        }

        public Downtime EditDownTime(EquipmentCombined ec)
        {
            Downtime downtime = new Downtime()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = ec.downTimeModel.EquipmentId,
                DowntimeId = ec.downTimeModel.DowntimeId
            };

            downtime.Retrieve(userData.DatabaseKey);
            downtime.ParentSiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            downtime.DateDown = ec.downTimeModel.DateDown;
            downtime.MinutesDown = ec.downTimeModel.MinutesDown ?? 0;
            downtime.ReasonForDown=ec.downTimeModel.ReasonForDown;
            downtime.Update(this.userData.DatabaseKey);
            return downtime;
        }
        #endregion

        #region Equipment Dashboard graph
        public Chart EquipMentChartData(int EquipmentId, int timeframe)
        {
            var dataModel = new VMDashboardEquipChart();
            string color = "#34bfa3";
            List<string> ColorList = new List<string>();
            Chart _chart = new Chart();

            DataTable equipment_chartdata = DashboardReports.Equipment_RetrieveDownTimeByPK(userData.DatabaseKey, timeframe, EquipmentId);
            IList<VMDashboardEquipChart> itemsEquipChart = equipment_chartdata.AsEnumerable().Select(row =>
                    new VMDashboardEquipChart
                    {
                        EQNAME = row.Field<string>("EQNAME"),
                        MINS = row.Field<long>("MINS")
                    }).ToList();

            if (itemsEquipChart != null && itemsEquipChart.Count > 0)
            {
                dataModel.VMDashboardEquipCharts = itemsEquipChart.ToList();
                _chart.labels = dataModel.VMDashboardEquipCharts.Select(x => x.EQNAME).ToArray();
                _chart.datasets = new List<Datasets>();
                List<Datasets> _dataSet = new List<Datasets>();
                _dataSet.Add(new Datasets()
                {
                    data = dataModel.VMDashboardEquipCharts.Select(x => x.MINS).ToArray(),
                });
                if (_dataSet != null)
                {
                    for (int dataCount = 0; dataCount < _dataSet[0].data.Count(); dataCount++)
                    {
                        ColorList.Add(color);
                    }
                    _dataSet[0].backgroundColor = ColorList.ToArray();
                    _dataSet[0].borderColor = ColorList.ToArray();
                }

                _chart.datasets = _dataSet;
            }
            return _chart;
        }
        #endregion        

        #region Menu
        public bool AlertFollowIdExist(long _EquipmentId)
        {
            bool flag;
            AlertFollow alertFollow = new AlertFollow
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                UserInfoId = this.userData.DatabaseKey.User.UserInfoId,
                ObjectId = _EquipmentId,
                ObjectType =ChargeType.Equipment
            };
            alertFollow.AlertFollow_RetrieveByObjectForUser(this.userData.DatabaseKey);
            if (alertFollow.AlertFollowId > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;

            }
            return flag;
        }
        public CreatedLastUpdatedModel createdLastUpdatedModel(long _EquipmentId)
        {
            CreatedLastUpdatedModel _CreatedLastUpdatedModel = new CreatedLastUpdatedModel();
            DataContracts.Equipment eq = new DataContracts.Equipment();
            eq.ClientId = this.userData.DatabaseKey.Client.ClientId;
            eq.EquipmentId = _EquipmentId;
            eq.RetrieveCreateModifyDate(userData.DatabaseKey);
            _CreatedLastUpdatedModel.CreatedDateValue = eq.CreateDate.ToString();
            _CreatedLastUpdatedModel.CreatedUserValue = eq.CreateBy;
            _CreatedLastUpdatedModel.ModifyUserValue = eq.ModifyBy;
            _CreatedLastUpdatedModel.ModifyDatevalue = eq.ModifyDate.ToString();

            return _CreatedLastUpdatedModel;
        }
        #endregion

        #region BulkUpdate
        public List<string> EqpBulkUpload(EquipmentBulkUpdateModel eModel)
        {
            List<string> errorMessages = new List<string>();
            bool isChanged = false;
            if (!String.IsNullOrEmpty(eModel.EquipmentIdList))
            {
                foreach (var v in eModel.EquipmentIdList.Split(',').ToList())
                {
                    Equipment equipment = new Equipment()
                    {
                        ClientId = this.userData.DatabaseKey.Client.ClientId,
                        EquipmentId = Convert.ToInt64(v)
                    };
                    equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
                    if (!String.IsNullOrWhiteSpace(eModel.Type))
                    {
                        equipment.Type = eModel.Type;
                        isChanged = true;
                    }

                    if (eModel.AssetGroup1Id != null)
                    {
                        equipment.AssetGroup1 = eModel.AssetGroup1Id ?? 0;
                        isChanged = true;
                    }
                    if (eModel.AssetGroup2Id != null)
                    {
                        equipment.AssetGroup2 = eModel.AssetGroup2Id ?? 0;
                        isChanged = true;
                    }
                    if (eModel.AssetGroup3Id != null)
                    {
                        equipment.AssetGroup3 = eModel.AssetGroup3Id ?? 0;
                        isChanged = true;
                    }
                 
                    // V2-774 
                    // RKL - 2022-10-13
                    // Based on the sp - the AccountId has to be 0 for this to work
                    if (!string.IsNullOrEmpty(eModel.LaborAccountClientLookupId))
                    {
                        equipment.Labor_AccountId = 0;
                        equipment.LaborAccountClientLookupId = eModel.LaborAccountClientLookupId.Trim();
                        isChanged = true;
                    }
                    if (!string.IsNullOrEmpty(eModel.Location))
                    {
                        equipment.Location = eModel.Location.Trim();
                        isChanged = true;
                    }
                    #region V2-1158
                    if (!string.IsNullOrEmpty(eModel.Make))
                    {
                        equipment.Make = eModel.Make.Trim();
                        isChanged = true;
                    }
                    if (!string.IsNullOrEmpty(eModel.Model))
                    {
                        equipment.Model = eModel.Model.Trim();
                        isChanged = true;
                    }
                    #endregion
                    if (isChanged == true)
                    {
                        equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
                        
                        equipment.UpdateByPKForeignKeys_V2(userData.DatabaseKey);
                        if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count > 0)
                        {
                            errorMessages.AddRange(equipment.ErrorMessages);
                        }
                    }
                }
            }
            return errorMessages;
        }
        #endregion BulkUpdate

        #region Add-Edit dynamic
        public Equipment AddEquipmentDynamic(EquipmentCombined objEM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Equipment equipment = new Equipment();
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            equipment.CreateMode = true;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddAsset, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objEM.AddEquipment);
                getpropertyInfo = objEM.AddEquipment.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objEM.AddEquipment);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                
                setpropertyInfo = equipment.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(equipment, val);
            }

            equipment.CreateByPKForeignKeys_V2(this.userData.DatabaseKey);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count == 0 && configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
            {
                IEnumerable<string> errors = AddEquipmentUDFDynamic(objEM.AddEquipment, equipment.EquipmentId, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    equipment.ErrorMessages.AddRange(errors);
                }
            }

            return equipment;
        }
        public List<string> AddEquipmentUDFDynamic(AddEquipmentModelDynamic equipment, long EquipmentId, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            EquipmentUDF equipmentUDF = new EquipmentUDF();
            equipmentUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            equipmentUDF.EquipmentId = EquipmentId;
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, equipment);
                getpropertyInfo = equipment.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(equipment);


                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = equipmentUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(equipmentUDF, val);
            }
            equipmentUDF.Create(_dbKey);
            return equipmentUDF.ErrorMessages;
        }
        public Equipment EditEquipmentDynamic(EquipmentCombined equip)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(equip.EditEquipment.EquipmentId)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);

            equipment.CreateMode = false;
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditAsset, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, equip.EditEquipment);
                getpropertyInfo = equip.EditEquipment.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(equip.EditEquipment);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = equipment.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(equipment, val);
            }

            if (equipment.Maint_VendorId == 0)
            {
                equipment.MaintVendorIdClientLookupId = "";
            }
            if (equipment.Purch_VendorId == 0)
            {
                equipment.PurchVendorIdClientLookupId = "";
            }
            if (equipment.Material_AccountId == 0)
            {
                equipment.MaterialAccountClientLookupId = "";
            }
            if (equipment.Labor_AccountId == 0)
            {
                equipment.LaborAccountClientLookupId = "";
            }
            if (equipment.ElectricalParent == 0)
            {
                equipment.ElectricalParentClientLookupId = "";
            }
           
            equipment.UpdateByPKForeignKeys_V2(userData.DatabaseKey);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count == 0 &&
                configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                IEnumerable<string> errors = EditEquipmentUDFDynamic(equip.EditEquipment, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    equipment.ErrorMessages.AddRange(errors);
                }
            }
            return equipment;
        }
        public List<string> EditEquipmentUDFDynamic(EditEquipmentModelDynamic equipment, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            EquipmentUDF equipmentUDF = new EquipmentUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = equipment.EquipmentId
            };
            equipmentUDF = equipmentUDF.RetrieveByEquipmentId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, equipment);
                getpropertyInfo = equipment.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(equipment);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = equipmentUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(equipmentUDF, val);
            }
            if (equipmentUDF.EquipmentUDFId > 0)
            {
                equipmentUDF.Update(_dbKey);
            }
            else
            {
                equipmentUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                equipmentUDF.EquipmentId = equipment.EquipmentId;
                equipmentUDF.Create(_dbKey);
            }

            return equipmentUDF.ErrorMessages;
        }
        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }
        public EditEquipmentModelDynamic RetrieveEquipmentDetailsByEquipmentId(long EquipmentId)
        {
            EditEquipmentModelDynamic editEquipmentModelDynamic = new EditEquipmentModelDynamic();
            Equipment equipment = RetrieveEquipmentByEquipmentId(EquipmentId);
            EquipmentUDF equipmentUDF = RetrieveEquipmentUDFByEquipmentId(EquipmentId);
            editEquipmentModelDynamic = MapEquipmentUDFDataForEdit(editEquipmentModelDynamic, equipmentUDF);
            editEquipmentModelDynamic = MapEquipmentDataForEdit(editEquipmentModelDynamic, equipment);
            return editEquipmentModelDynamic;
        }
        public Equipment RetrieveEquipmentByEquipmentId(long EquipmentId)
        {
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);
            return equipment;
        }
        public EquipmentUDF RetrieveEquipmentUDFByEquipmentId(long EquipmentId)
        {
            EquipmentUDF equipmentUDF = new EquipmentUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };

            equipmentUDF = equipmentUDF.RetrieveByEquipmentId(this.userData.DatabaseKey);
            return equipmentUDF;
        }
        private EditEquipmentModelDynamic MapEquipmentUDFDataForEdit(EditEquipmentModelDynamic editEquipmentModelDynamic, EquipmentUDF equipmentUDF)
        {
            if (equipmentUDF != null)
            {
                editEquipmentModelDynamic.EquipmentUDFId = equipmentUDF.EquipmentUDFId;
                editEquipmentModelDynamic.Text1 = equipmentUDF.Text1;
                editEquipmentModelDynamic.Text2 = equipmentUDF.Text2;
                editEquipmentModelDynamic.Text3 = equipmentUDF.Text3;
                editEquipmentModelDynamic.Text4 = equipmentUDF.Text4;
                if (equipmentUDF.Date1 != null && equipmentUDF.Date1 == DateTime.MinValue)
                {
                    editEquipmentModelDynamic.Date1 = null;
                }
                else
                {
                    editEquipmentModelDynamic.Date1 = equipmentUDF.Date1;
                }
                if (equipmentUDF.Date2 != null && equipmentUDF.Date2 == DateTime.MinValue)
                {
                    editEquipmentModelDynamic.Date2 = null;
                }
                else
                {
                    editEquipmentModelDynamic.Date2 = equipmentUDF.Date2;
                }
                if (equipmentUDF.Date3 != null && equipmentUDF.Date3 == DateTime.MinValue)
                {
                    editEquipmentModelDynamic.Date3 = null;
                }
                else
                {
                    editEquipmentModelDynamic.Date3 = equipmentUDF.Date3;
                }
                if (equipmentUDF.Date4 != null && equipmentUDF.Date4 == DateTime.MinValue)
                {
                    editEquipmentModelDynamic.Date4 = null;
                }
                else
                {
                    editEquipmentModelDynamic.Date4 = equipmentUDF.Date4;
                }

                editEquipmentModelDynamic.Bit1 = equipmentUDF.Bit1;
                editEquipmentModelDynamic.Bit2 = equipmentUDF.Bit2;
                editEquipmentModelDynamic.Bit3 = equipmentUDF.Bit3;
                editEquipmentModelDynamic.Bit4 = equipmentUDF.Bit4;

                editEquipmentModelDynamic.Numeric1 = equipmentUDF.Numeric1;
                editEquipmentModelDynamic.Numeric2 = equipmentUDF.Numeric2;
                editEquipmentModelDynamic.Numeric3 = equipmentUDF.Numeric3;
                editEquipmentModelDynamic.Numeric4 = equipmentUDF.Numeric4;

                editEquipmentModelDynamic.Select1 = equipmentUDF.Select1;
                editEquipmentModelDynamic.Select2 = equipmentUDF.Select2;
                editEquipmentModelDynamic.Select3 = equipmentUDF.Select3;
                editEquipmentModelDynamic.Select4 = equipmentUDF.Select4;
            }
            return editEquipmentModelDynamic;
        }
        public EditEquipmentModelDynamic MapEquipmentDataForEdit(EditEquipmentModelDynamic editEquipmentModelDynamic, Equipment equipment)
        {
            editEquipmentModelDynamic.EquipmentId = equipment.EquipmentId;
            editEquipmentModelDynamic.ClientLookupId = equipment.ClientLookupId;
            editEquipmentModelDynamic.BusinessGroup = equipment.BusinessGroup;
            editEquipmentModelDynamic.AcquiredCost = equipment.AcquiredCost;
            if (equipment.AcquiredDate != null && equipment.AcquiredDate != default(DateTime))
            {
                editEquipmentModelDynamic.AcquiredDate = equipment.AcquiredDate;
            }
            else
            {
                editEquipmentModelDynamic.AcquiredDate = null;
            }
            editEquipmentModelDynamic.CatalogNumber = equipment.CatalogNumber;
            editEquipmentModelDynamic.Category = equipment.Category;
            editEquipmentModelDynamic.CostCenter = equipment.CostCenter;
            editEquipmentModelDynamic.InactiveFlag = equipment.InactiveFlag;
            editEquipmentModelDynamic.CriticalFlag = equipment.CriticalFlag;
            if (equipment.InstallDate != null && equipment.InstallDate != default(DateTime))
            {
                editEquipmentModelDynamic.InstallDate = equipment.InstallDate;
            }
            else
            {
                editEquipmentModelDynamic.InstallDate = null;
            }
            editEquipmentModelDynamic.Location = equipment.Location;
            if (equipment.Maint_VendorId == 0)
            {
                editEquipmentModelDynamic.Maint_VendorId = null;
            }
            else
            {
                editEquipmentModelDynamic.Maint_VendorId = equipment.Maint_VendorId;
            }
            editEquipmentModelDynamic.Maint_WarrantyDesc = equipment.Maint_WarrantyDesc;
            if (equipment.Maint_WarrantyExpire != null && equipment.Maint_WarrantyExpire != default(DateTime))
            {
                editEquipmentModelDynamic.Maint_WarrantyExpire = equipment.Maint_WarrantyExpire;
            }
            else
            {
                editEquipmentModelDynamic.Maint_WarrantyExpire = null;
            }
            editEquipmentModelDynamic.Make = equipment.Make;
            if (equipment.Material_AccountId == 0)
            {
                editEquipmentModelDynamic.Material_AccountId = null;
                editEquipmentModelDynamic.Material_AccountName = "";
            }
            else
            {
                editEquipmentModelDynamic.Material_AccountId = equipment.Material_AccountId;
                editEquipmentModelDynamic.Material_AccountName = equipment.MaterialAccountClientLookupId;
            }
            if (equipment.Labor_AccountId == 0)
            {
                editEquipmentModelDynamic.Labor_AccountId = null;
                editEquipmentModelDynamic.Labor_AccountName = "";
            }
            else
            {
                editEquipmentModelDynamic.Labor_AccountId = equipment.Labor_AccountId;
                editEquipmentModelDynamic.Labor_AccountName = equipment.LaborAccountClientLookupId;
            }
            editEquipmentModelDynamic.Model = equipment.Model;
            editEquipmentModelDynamic.Name = equipment.Name;
            editEquipmentModelDynamic.NoPartXRef = equipment.NoPartXRef;
            editEquipmentModelDynamic.OriginalValue = equipment.OriginalValue;
            editEquipmentModelDynamic.OriginalValue = equipment.OriginalValue;
            if (equipment.OutofService != null && equipment.OutofService != default(DateTime))
            {
                editEquipmentModelDynamic.OutofService = equipment.OutofService;
            }
            else
            {
                editEquipmentModelDynamic.OutofService = null;
            }
            if (equipment.Purch_VendorId == 0)
            {
                editEquipmentModelDynamic.Purch_VendorId = null;
            }
            else
            {
                editEquipmentModelDynamic.Purch_VendorId = equipment.Purch_VendorId;
            }
            editEquipmentModelDynamic.Purch_WarrantyDesc = equipment.Purch_WarrantyDesc;
            if (equipment.Purch_WarrantyExpire != null && equipment.Purch_WarrantyExpire != default(DateTime))
            {
                editEquipmentModelDynamic.Purch_WarrantyExpire = equipment.Purch_WarrantyExpire;
            }
            else
            {
                editEquipmentModelDynamic.Purch_WarrantyExpire = null;
            }
            editEquipmentModelDynamic.SerialNumber = equipment.SerialNumber;
            editEquipmentModelDynamic.Status = equipment.Status;
            editEquipmentModelDynamic.Type = equipment.Type;
            editEquipmentModelDynamic.AssetNumber = equipment.AssetNumber;
            editEquipmentModelDynamic.AssetCategory = equipment.AssetCategory;

            if (equipment.AssetGroup1 == 0)
            {
                editEquipmentModelDynamic.AssetGroup1 = null;
            }
            else
            {
                editEquipmentModelDynamic.AssetGroup1 = equipment.AssetGroup1;
            }
            if (equipment.AssetGroup2 == 0)
            {
                editEquipmentModelDynamic.AssetGroup2 = null;
            }
            else
            {
                editEquipmentModelDynamic.AssetGroup2 = equipment.AssetGroup2;
            }
            if (equipment.AssetGroup3 == 0)
            {
                editEquipmentModelDynamic.AssetGroup3 = null;
            }
            else
            {
                editEquipmentModelDynamic.AssetGroup3 = equipment.AssetGroup3;
            }
            if (equipment.ElectricalParent == 0)
            {
                editEquipmentModelDynamic.ElectricalParent = null;
                editEquipmentModelDynamic.ElectricalParent_ClientLookupId = "";
            }
            else
            {
                editEquipmentModelDynamic.ElectricalParent = equipment.ElectricalParent;
                editEquipmentModelDynamic.ElectricalParent_ClientLookupId = equipment.ElectricalParentClientLookupId;
            }
           
            editEquipmentModelDynamic.ParentIdClientLookupId = equipment.ParentIdClientLookupId;
            editEquipmentModelDynamic.Purch_VendorName = equipment.PurchVendorIdClientLookupId + "-" + equipment.PurchVendorName; // V2-1211 Modified from Vendor.ClientLookUpId to Vendor.ClientLookUpId + '-' + Vendor.Name
            editEquipmentModelDynamic.Maint_VendorName = equipment.MaintVendorIdClientLookupId + "-" + equipment.MaintVendorName; // V2-1211 Modified from Vendor.ClientLookUpId to Vendor.ClientLookUpId + '-' + Vendor.Name

            return editEquipmentModelDynamic;
        }
        public ViewEquipmentModelDynamic MapEquipmentUDFDataForView(ViewEquipmentModelDynamic viewEquipmentModelDynamic, EquipmentUDF equipmentUDF)
        {
            if (equipmentUDF != null)
            {
                viewEquipmentModelDynamic.EquipmentUDFId = equipmentUDF.EquipmentUDFId;

                viewEquipmentModelDynamic.Text1 = equipmentUDF.Text1;
                viewEquipmentModelDynamic.Text2 = equipmentUDF.Text2;
                viewEquipmentModelDynamic.Text3 = equipmentUDF.Text3;
                viewEquipmentModelDynamic.Text4 = equipmentUDF.Text4;

                if (equipmentUDF.Date1 != null && equipmentUDF.Date1 == DateTime.MinValue)
                {
                    viewEquipmentModelDynamic.Date1 = null;
                }
                else
                {
                    viewEquipmentModelDynamic.Date1 = equipmentUDF.Date1;
                }
                if (equipmentUDF.Date2 != null && equipmentUDF.Date2 == DateTime.MinValue)
                {
                    viewEquipmentModelDynamic.Date2 = null;
                }
                else
                {
                    viewEquipmentModelDynamic.Date2 = equipmentUDF.Date2;
                }
                if (equipmentUDF.Date3 != null && equipmentUDF.Date3 == DateTime.MinValue)
                {
                    viewEquipmentModelDynamic.Date3 = null;
                }
                else
                {
                    viewEquipmentModelDynamic.Date3 = equipmentUDF.Date3;
                }
                if (equipmentUDF.Date4 != null && equipmentUDF.Date4 == DateTime.MinValue)
                {
                    viewEquipmentModelDynamic.Date4 = null;
                }
                else
                {
                    viewEquipmentModelDynamic.Date4 = equipmentUDF.Date4;
                }

                viewEquipmentModelDynamic.Bit1 = equipmentUDF.Bit1;
                viewEquipmentModelDynamic.Bit2 = equipmentUDF.Bit2;
                viewEquipmentModelDynamic.Bit3 = equipmentUDF.Bit3;
                viewEquipmentModelDynamic.Bit4 = equipmentUDF.Bit4;

                viewEquipmentModelDynamic.Numeric1 = equipmentUDF.Numeric1;
                viewEquipmentModelDynamic.Numeric2 = equipmentUDF.Numeric2;
                viewEquipmentModelDynamic.Numeric3 = equipmentUDF.Numeric3;
                viewEquipmentModelDynamic.Numeric4 = equipmentUDF.Numeric4;

                viewEquipmentModelDynamic.Select1 = equipmentUDF.Select1;
                viewEquipmentModelDynamic.Select2 = equipmentUDF.Select2;
                viewEquipmentModelDynamic.Select3 = equipmentUDF.Select3;
                viewEquipmentModelDynamic.Select4 = equipmentUDF.Select4;

                viewEquipmentModelDynamic.Select1Name = equipmentUDF.Select1Name;
                viewEquipmentModelDynamic.Select2Name = equipmentUDF.Select2Name;
                viewEquipmentModelDynamic.Select3Name = equipmentUDF.Select3Name;
                viewEquipmentModelDynamic.Select4Name = equipmentUDF.Select4Name;
            }
            return viewEquipmentModelDynamic;
        }
        public ViewEquipmentModelDynamic MapEquipmentDataForView(ViewEquipmentModelDynamic viewEquipmentModelDynamic, Equipment equipment)
        {
            viewEquipmentModelDynamic.EquipmentId = equipment.EquipmentId;
            viewEquipmentModelDynamic.ClientLookupId = equipment.ClientLookupId;
            viewEquipmentModelDynamic.BusinessGroup = equipment.BusinessGroup;
            viewEquipmentModelDynamic.AcquiredCost = equipment.AcquiredCost;
            if (equipment.AcquiredDate != null && equipment.AcquiredDate != default(DateTime))
            {
                viewEquipmentModelDynamic.AcquiredDate = equipment.AcquiredDate;
            }
            else
            {
                viewEquipmentModelDynamic.AcquiredDate = null;
            }
            viewEquipmentModelDynamic.CatalogNumber = equipment.CatalogNumber;
            viewEquipmentModelDynamic.Category = equipment.Category;
            viewEquipmentModelDynamic.CostCenter = equipment.CostCenter;
            viewEquipmentModelDynamic.InactiveFlag = equipment.InactiveFlag;
            viewEquipmentModelDynamic.CriticalFlag = equipment.CriticalFlag;
            if (equipment.InstallDate != null && equipment.InstallDate != default(DateTime))
            {
                viewEquipmentModelDynamic.InstallDate = equipment.InstallDate;
            }
            else
            {
                viewEquipmentModelDynamic.InstallDate = null;
            }
            viewEquipmentModelDynamic.Location = equipment.Location;
            if (equipment.Maint_VendorId == 0)
            {
                viewEquipmentModelDynamic.Maint_VendorId = null;
            }
            else
            {
                viewEquipmentModelDynamic.Maint_VendorId = equipment.Maint_VendorId;
            }
            viewEquipmentModelDynamic.Maint_WarrantyDesc = equipment.Maint_WarrantyDesc;
            if (equipment.Maint_WarrantyExpire != null && equipment.Maint_WarrantyExpire != default(DateTime))
            {
                viewEquipmentModelDynamic.Maint_WarrantyExpire = equipment.Maint_WarrantyExpire;
            }
            else
            {
                viewEquipmentModelDynamic.Maint_WarrantyExpire = null;
            }
            viewEquipmentModelDynamic.Make = equipment.Make;
            if (equipment.Material_AccountId == 0)
            {
                viewEquipmentModelDynamic.Material_AccountId = null;
                viewEquipmentModelDynamic.Material_AccountName = "";
            }
            else
            {
                viewEquipmentModelDynamic.Material_AccountId = equipment.Material_AccountId;
                viewEquipmentModelDynamic.Material_AccountName = equipment.MaterialAccountClientLookupId;
            }
            if (equipment.Labor_AccountId == 0)
            {
                viewEquipmentModelDynamic.Labor_AccountId = null;
                viewEquipmentModelDynamic.Labor_AccountName = "";
            }
            else
            {
                viewEquipmentModelDynamic.Labor_AccountId = equipment.Labor_AccountId;
                viewEquipmentModelDynamic.Labor_AccountName = equipment.LaborAccountClientLookupId;
            }
            viewEquipmentModelDynamic.Model = equipment.Model;
            viewEquipmentModelDynamic.Name = equipment.Name;
            viewEquipmentModelDynamic.NoPartXRef = equipment.NoPartXRef;
            viewEquipmentModelDynamic.OriginalValue = equipment.OriginalValue;
            viewEquipmentModelDynamic.OriginalValue = equipment.OriginalValue;
            if (equipment.OutofService != null && equipment.OutofService != default(DateTime))
            {
                viewEquipmentModelDynamic.OutofService = equipment.OutofService;
            }
            else
            {
                viewEquipmentModelDynamic.OutofService = null;
            }
            if (equipment.Purch_VendorId == 0)
            {
                viewEquipmentModelDynamic.Purch_VendorId = null;
            }
            else
            {
                viewEquipmentModelDynamic.Purch_VendorId = equipment.Purch_VendorId;
            }
            viewEquipmentModelDynamic.Purch_WarrantyDesc = equipment.Purch_WarrantyDesc;
            if (equipment.Purch_WarrantyExpire != null && equipment.Purch_WarrantyExpire != default(DateTime))
            {
                viewEquipmentModelDynamic.Purch_WarrantyExpire = equipment.Purch_WarrantyExpire;
            }
            else
            {
                viewEquipmentModelDynamic.Purch_WarrantyExpire = null;
            }
            viewEquipmentModelDynamic.SerialNumber = equipment.SerialNumber;
            viewEquipmentModelDynamic.Status = equipment.Status;
            viewEquipmentModelDynamic.Type = equipment.Type;
            viewEquipmentModelDynamic.AssetNumber = equipment.AssetNumber;
            viewEquipmentModelDynamic.AssetCategory = equipment.AssetCategory;

            if (equipment.AssetGroup1 == 0)
            {
                viewEquipmentModelDynamic.AssetGroup1 = null;
                viewEquipmentModelDynamic.AssetGroup1Name = "";
            }
            else
            {
                viewEquipmentModelDynamic.AssetGroup1 = equipment.AssetGroup1;
                viewEquipmentModelDynamic.AssetGroup1Name = equipment.AssetGroup1ClientLookupId + " - " + equipment.AssetGroup1Desc;
            }
            if (equipment.AssetGroup2 == 0)
            {
                viewEquipmentModelDynamic.AssetGroup2 = null;
                viewEquipmentModelDynamic.AssetGroup2Name = "";
            }
            else
            {
                viewEquipmentModelDynamic.AssetGroup2 = equipment.AssetGroup2;
                viewEquipmentModelDynamic.AssetGroup2Name = equipment.AssetGroup2ClientLookupId + " - " + equipment.AssetGroup2Desc;
            }
            if (equipment.AssetGroup3 == 0)
            {
                viewEquipmentModelDynamic.AssetGroup3 = null;
                viewEquipmentModelDynamic.AssetGroup3Name = "";
            }
            else
            {
                viewEquipmentModelDynamic.AssetGroup3 = equipment.AssetGroup3;
                viewEquipmentModelDynamic.AssetGroup3Name = equipment.AssetGroup3ClientLookupId + " - " + equipment.AssetGroup3Desc;
            }
            if (equipment.ElectricalParent == 0)
            {
                viewEquipmentModelDynamic.ElectricalParent = null;
                viewEquipmentModelDynamic.ElectricalParent_ClientLookupId = "";
            }
            else
            {
                viewEquipmentModelDynamic.ElectricalParent = equipment.ElectricalParent;
                viewEquipmentModelDynamic.ElectricalParent_ClientLookupId = equipment.ElectricalParentClientLookupId;
            }

            viewEquipmentModelDynamic.ParentIdName = equipment.ParentIdClientLookupId;
            viewEquipmentModelDynamic.Purch_VendorName = equipment.PurchVendorIdClientLookupId + "-" + equipment.PurchVendorName; // V2-1211 Modified from Vendor.ClientLookUpId to Vendor.ClientLookUpId + '-' + Vendor.Name
            viewEquipmentModelDynamic.Maint_VendorName = equipment.MaintVendorIdClientLookupId + "-" + equipment.MaintVendorName; // V2-1211 Modified from Vendor.ClientLookUpId to Vendor.ClientLookUpId + '-' + Vendor.Name
            viewEquipmentModelDynamic.Material_AccountName = equipment.MaterialAccountClientLookupId;

            return viewEquipmentModelDynamic;
        }
        #endregion

        #region Asset Availability Remove
        public Equipment RemoveAssetAvailability(EquipmentCombined equip)
        {
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();

            #region Equipment
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(equip._AssetAvailabilityRemoveModel.EquipmentId)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.RemoveFromService = true;
            equipment.RemoveFromServiceDate = DateTime.UtcNow;
            equipment.ExpectedReturnToService = equip._AssetAvailabilityRemoveModel.ExpectedReturnToService;
            equipment.RemoveFromServiceReasonCode = equip._AssetAvailabilityRemoveModel.RemoveFromServiceReasonCode;
            if (equip._AssetAvailabilityRemoveModel.RemoveFromServiceReason == null)
            {
                equipment.RemoveFromServiceReason = "";
            }
            else
            {
                equipment.RemoveFromServiceReason = equip._AssetAvailabilityRemoveModel.RemoveFromServiceReason;
            }
            equipment.Status = equip._AssetAvailabilityRemoveModel.RemoveFromServiceReasonCode;
            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null)
            {
                CreateActivityEventLog(equipment.EquipmentId,EventStatusConstants.Remove, equipment.ExpectedReturnToService, equipment.RemoveFromServiceReason, equipment.RemoveFromServiceReasonCode);

            }
            #endregion

            return equipment;
        }

        public void CreateActivityEventLog(Int64 EQId, string Event, DateTime? ExpectedReturnToService, string RemoveFromServiceReason, string ReasonCode)
        {
            AssetAvailabilityLog log = new AssetAvailabilityLog();
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.SiteId = userData.DatabaseKey.Personnel.SiteId;
            log.ObjectId = EQId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = Event;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.ReturnToService = ExpectedReturnToService;
            log.Reason = RemoveFromServiceReason;
            log.ReasonCode = ReasonCode;
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion

        #region Return to Service
        public Equipment ReturnToservice(long EquipmentId)
        {
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();

            #region Equipment
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.RemoveFromService = false;
            equipment.RemoveFromServiceDate = null;
            equipment.ExpectedReturnToService = null;
            equipment.RemoveFromServiceReason = "";
            equipment.RemoveFromServiceReasonCode = "";
            equipment.Status = AssetStatusConstant.InService;
            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null)
            {
                CreateActivityEventLog(equipment.EquipmentId, EventStatusConstants.Return, null, "", "");
            }
            #endregion

            return equipment;
        }
        #endregion

        #region Asset Availability Update
        public Equipment UpdateAssetAvailability(EquipmentCombined equip)
        {
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();

            #region Equipment
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(equip._AssetAvailabilityUpdateModel.EquipmentId)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.ExpectedReturnToService = equip._AssetAvailabilityUpdateModel.ExpectedReturnToService;
            if (equip._AssetAvailabilityUpdateModel.RemoveFromServiceReason == null)
            {
                equipment.RemoveFromServiceReason = "";
            }
            else
            {
                equipment.RemoveFromServiceReason = equip._AssetAvailabilityUpdateModel.RemoveFromServiceReason;
            }
            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null)
            {
                CreateActivityEventLog(equipment.EquipmentId,"Update", equipment.ExpectedReturnToService, equipment.RemoveFromServiceReason, equipment.RemoveFromServiceReasonCode);

            }
            #endregion

            return equipment;
        }
        #endregion

        #region Scrap Equipment
        public Equipment ValidateEquipmentScrapped(long equipId, string flag, string clientLookupId)
        {
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = equipId,
                Flag = flag,
                ClientLookupId = clientLookupId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            equipment.CheckEquipmentIsScrapped(userData.DatabaseKey);
            return equipment;
        }
        public List<string> UpdateEquipmentStatustoScrap(long equipId, bool inActiveFlag)
        {
            List<string> errList = new List<string>();
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = equipId
            };
            equipment.Retrieve(userData.DatabaseKey);
            equipment.Status = AssetStatusConstant.Scrapped;
            equipment.InactiveFlag = true;
            equipment.RemoveFromService = true;
            equipment.RemoveFromServiceDate = DateTime.UtcNow;
            equipment.RemoveFromServiceReasonCode = AssetStatusConstant.Scrapped;
            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null || equipment.ErrorMessages.Count <= 0)
            {
                CreateActivityEventLog(equipId, EventStatusConstants.Remove, null, "", AssetStatusConstant.Scrapped);
                CreateAssetEventForScrapAndReturnScrapAsset(equipId, ActivationStatusConstant.InActivate);
                CreateAssetEventForScrapAndReturnScrapAsset(equipId, AssetStatusConstant.Scrapped);
                errList = UpdatePmScheduleRecords(equipId, inActiveFlag);
                return errList;
            }
            else
            {
                return equipment.ErrorMessages;
            }
        }
        #endregion

        #region Return Scrap Asset
        public List<string> UpdateAssetStatusForReturnAsset(long equipId)
        {
            List<string> errList = new List<string>();
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = equipId
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.Status = AssetStatusConstant.InService;
            equipment.InactiveFlag = false;
            equipment.RemoveFromService = false;
            equipment.RemoveFromServiceDate = null;
            equipment.RemoveFromServiceReasonCode = "";
            equipment.ModifyBy = this.userData.DatabaseKey.User.UserName;
            equipment.ModifyDate = DateTime.UtcNow;
            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null || equipment.ErrorMessages.Count <= 0)
            {
                CreateActivityEventLog(equipment.EquipmentId, EventStatusConstants.Return, null, "", "");
                CreateAssetEventForScrapAndReturnScrapAsset(equipment.EquipmentId, ActivationStatusConstant.Activate);
                CreateAssetEventForScrapAndReturnScrapAsset(equipment.EquipmentId, EventStatusConstants.Return);
                errList = UpdatePmScheduleRecords(equipId, false);
                return errList;
            }
            else
            {
                return equipment.ErrorMessages;
            }
        }
        public void CreateAssetEventForScrapAndReturnScrapAsset(long equipId, string Event)
        {
            AssetEventLog assetEventLog = new AssetEventLog();
            assetEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            assetEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetEventLog.EquipmentId = equipId;
            assetEventLog.TransactionDate = DateTime.UtcNow;
            assetEventLog.Event = Event;
            assetEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            assetEventLog.Comments = "";
            assetEventLog.SourceId = 0;
            assetEventLog.Create(userData.DatabaseKey);
        }
        #endregion

        #region Repairable Spare Asset
        public Equipment AddRepairableSpareAsset(EquipmentCombined objEM)
        {
            // V2-1043
            // eq_repspare    - is data contract for repairable spare 
            // eq_assignedto  - is the equipment to which the repairable spare has been assigned
            //                  Equipment if assigned
            //                  Location if unassigned
            //                  
            // Retrieve the assigned to equipment 
            Equipment eq_assigned = new Equipment()
            {
                EquipmentId = objEM.RepairableSpareModel.AssignedAssetId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            eq_assigned.RetrieveByPKForeignKeys_V2(_dbKey);
            // Create and build the repairable spare data contract
            Equipment eq_repspare = new Equipment();
            eq_repspare = BuildEquipmentDataForAdd(eq_repspare, objEM.RepairableSpareModel, eq_assigned);
            // Create and build the repairale spare log data contract
            RepairableSpareLog repairableSpareLog = new RepairableSpareLog();
            repairableSpareLog = BuildRepairableSpareLogData(repairableSpareLog, objEM.RepairableSpareModel, eq_repspare);
            eq_repspare.RepairableSpareLog = repairableSpareLog;
            // Add Repairable Spare 
            // Add Repairable Spare Log Entry 
            eq_repspare.AddRepairableSpareAsset(_dbKey);
            // This is not necessary - not updating the equipment that the user was "sitting on"
            /*
            // If the spare and the log added correctly 
            if (eq_repspare.ErrorMessages != null && eq_repspare.ErrorMessages.Count == 0)
            {
                eq_assignedto = BuildEquipmentDataForUpdate(eq_assignedto, objEM.RepairableSpareModel, equipmentAssigned);
                eq_assignedto.Update(_dbKey); //-- update asset
                if (eq_assignedto.ErrorMessages != null && eq_assignedto.ErrorMessages.Count > 0)
                {
                    eq_repspare.ErrorMessages.AddRange(eq_repspare.ErrorMessages);
                }
            }
            */
            return eq_repspare;
        }
        
        private Equipment BuildEquipmentDataForUpdate(Equipment equipment, RepairableSpareModel repairableSpareModel, Equipment equipmentAssigned)
        {
            equipment.Status = repairableSpareModel.RepairableSpareStatus;
            equipment.Assigned = repairableSpareModel.AssignedAssetId;
            if (equipment.Status == AssetStatusConstant.Assigned)
            {
                equipment.Location = equipmentAssigned.Location;
            }
            else if (equipment.Status == AssetStatusConstant.Unassigned)
            {
                equipment.Location = repairableSpareModel.Location;
            }
            equipment.ParentId = repairableSpareModel.AssignedAssetId;
            equipment.AssetGroup1 = equipmentAssigned.AssetGroup1;
            equipment.AssetGroup2 = equipmentAssigned.AssetGroup2;
            equipment.AssetGroup3 = equipmentAssigned.AssetGroup3;
            return equipment;
        }
        private Equipment BuildEquipmentDataForAdd(Equipment eq_repspare, RepairableSpareModel repairableSpareModel, Equipment eq_assigned)
        {
            eq_repspare.ClientId = userData.DatabaseKey.Client.ClientId;
            eq_repspare.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            eq_repspare.ClientLookupId = repairableSpareModel.ClientLookupId;
            eq_repspare.AssetCategory = AssetCategoryConstant.RepairableSpare;
            eq_repspare.Name = repairableSpareModel.Name;
            eq_repspare.SerialNumber = repairableSpareModel.SerialNumber;
            eq_repspare.Type = repairableSpareModel.Type;
            eq_repspare.Make = repairableSpareModel.Make;
            eq_repspare.Model = repairableSpareModel.Model;
            eq_repspare.Material_AccountId = repairableSpareModel.Material_AccountId ?? 0;
            eq_repspare.MaterialAccountClientLookupId = repairableSpareModel.MaterialAccountClientLookupId ?? "";
            eq_repspare.AssetNumber = repairableSpareModel.AssetNumber;
            eq_repspare.Maint_WarrantyExpire = repairableSpareModel.Maint_WarrantyExpire;
            eq_repspare.Maint_VendorId = repairableSpareModel.Maint_VendorId ?? 0;
            eq_repspare.MaintVendorIdClientLookupId = repairableSpareModel.MaintVendorIdClientLookupId ?? "";
            eq_repspare.Maint_WarrantyDesc = repairableSpareModel.Maint_WarrantyDesc?? "";
            eq_repspare.AcquiredCost = repairableSpareModel.AcquiredCost ?? 0;
            eq_repspare.AcquiredDate = repairableSpareModel.AcquiredDate;
            eq_repspare.CatalogNumber = repairableSpareModel.CatalogNumber;
            eq_repspare.CostCenter = repairableSpareModel.CostCenter;
            eq_repspare.CriticalFlag = repairableSpareModel.CriticalFlag;
            eq_repspare.InstallDate = repairableSpareModel.InstallDate;
            // V2-1043
            // Initialize data in the repairable spare record with data from the assigned to record
            eq_repspare.Status = repairableSpareModel.RepairableSpareStatus;
            eq_repspare.Assigned = eq_assigned.EquipmentId;
            eq_repspare.ParentId = eq_assigned.EquipmentId;
            eq_repspare.ParentIdClientLookupId = eq_assigned.ClientLookupId;
            eq_repspare.AssetGroup1 = eq_assigned.AssetGroup1;
            eq_repspare.AssetGroup2 = eq_assigned.AssetGroup2;
            eq_repspare.AssetGroup3 = eq_assigned.AssetGroup3;
            // If the repairable spare is assigned 
            if (eq_repspare.Status == AssetStatusConstant.Assigned)
            {
                eq_repspare.Location = eq_assigned.Location;
            }
            // If the repairable spare is not assigned 
            else if (eq_repspare.Status == AssetStatusConstant.Unassigned)
            {
                eq_repspare.Location = repairableSpareModel.Location;
            }
            eq_repspare.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            eq_repspare.CreateMode = true;
            return eq_repspare;
        }
        private RepairableSpareLog BuildRepairableSpareLogData(RepairableSpareLog repairableSpareLog
            ,RepairableSpareModel repairableSpareModel
            ,Equipment eq_repspare)
        {
            repairableSpareLog.ClientId = userData.DatabaseKey.Client.ClientId;
            repairableSpareLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            repairableSpareLog.TransactionDate = DateTime.UtcNow;
            repairableSpareLog.Status = eq_repspare.Status;
            repairableSpareLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            repairableSpareLog.Assigned = eq_repspare.Assigned;
            repairableSpareLog.Location = eq_repspare.Location;
            repairableSpareLog.ParentId = eq_repspare.Assigned;
            repairableSpareLog.AssetGroup1 = eq_repspare.AssetGroup1;
            repairableSpareLog.AssetGroup2 = eq_repspare.AssetGroup2;
            repairableSpareLog.AssetGroup3 = eq_repspare.AssetGroup3;
            return repairableSpareLog;
        }      

        public Equipment UpdateAssignmentSpareAsset(EquipmentCombined objEM)
        {
            RepairableSpareLog repairableSpareLog = new RepairableSpareLog();
            Equipment equipmentAssigned = new Equipment()
            {
                EquipmentId = objEM.assignmentModel.AssignedAssetId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            equipmentAssigned.Retrieve(_dbKey);
            Equipment equipment = new Equipment()
            {
                EquipmentId = objEM.assignmentModel.EquipmentId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            equipment.Retrieve(_dbKey);
            if (objEM.assignmentModel.RepairableSpareStatusAssign == AssetStatusConstant.Unassigned)
            {
                equipment.Status = AssetStatusConstant.Unassigned;
                equipment.Location = objEM.assignmentModel.Location;
            }
            else
            {
                equipment.Status = AssetStatusConstant.Assigned;
                equipment.Location = equipmentAssigned.Location;
            }
            equipment.Assigned = objEM.assignmentModel.AssignedAssetId;
            equipment.ParentId = objEM.assignmentModel.AssignedAssetId; // same value will be assigned and parentid
            equipment.AssetGroup1 = equipmentAssigned.AssetGroup1;
            equipment.AssetGroup2 = equipmentAssigned.AssetGroup2;
            equipment.AssetGroup3 = equipmentAssigned.AssetGroup3;
            equipment.Update(_dbKey);
            if (equipment.ErrorMessages == null)
            {
                if (objEM.assignmentModel.RepairableSpareStatusAssign == AssetStatusConstant.Unassigned)
                {
                    CreateRepairableSpareLog(equipment, AssetStatusConstant.Unassigned);
                }
                else
                {
                    CreateRepairableSpareLog(equipment, AssetStatusConstant.Assigned);
                }

            }

            return equipment;
        }

        public void CreateRepairableSpareLog(Equipment eq, string status)
        {
            RepairableSpareLog RepLog = new RepairableSpareLog();
            RepLog.ClientId = userData.DatabaseKey.Client.ClientId;
            RepLog.SiteId = userData.DatabaseKey.Personnel.SiteId;
            RepLog.EquipmentId = eq.EquipmentId;
            RepLog.TransactionDate = DateTime.UtcNow;
            RepLog.Status = status;
            RepLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            RepLog.Assigned = eq.Assigned;
            RepLog.Location = eq.Location;
            RepLog.ParentId = eq.ParentId;
            RepLog.AssetGroup1 = eq.AssetGroup1;
            RepLog.AssetGroup2 = eq.AssetGroup2;
            RepLog.AssetGroup3 = eq.AssetGroup3;
            RepLog.Create(userData.DatabaseKey);
        }

        public Equipment UpdateRepairableSpareAsset(RepairableSpareModel equip)
        {
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(equip.EquipmentId)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientLookupId = equip.ClientLookupId;
            equipment.Name = equip.Name;
            equipment.SerialNumber = equip.SerialNumber != null ? equip.SerialNumber : emptyValue;
            equipment.Type = equip.Type != null ? equip.Type : emptyValue;
            equipment.Make = equip.Make != null ? equip.Make : emptyValue;
            equipment.Model=equip.Model != null ? equip.Model : emptyValue;
            equipment.Material_AccountId = equip.Material_AccountId ?? 0;
            equipment.MaterialAccountClientLookupId = !string.IsNullOrEmpty(equip.MaterialAccountClientLookupId) ? equip.MaterialAccountClientLookupId.Trim() : emptyValue;
            equipment.AssetNumber = equip.AssetNumber != null ? equip.AssetNumber : emptyValue;
            equipment.Maint_WarrantyExpire = equip.Maint_WarrantyExpire;
            equipment.Maint_VendorId = equip.Maint_VendorId ?? 0;
            equipment.MaintVendorIdClientLookupId = !string.IsNullOrEmpty(equip.MaintVendorIdClientLookupId) ? equip.MaintVendorIdClientLookupId.Trim() : emptyValue;
            equipment.Maint_WarrantyDesc = equip.Maint_WarrantyDesc != null ? equip.Maint_WarrantyDesc : emptyValue;
            equipment.AcquiredCost = equip.AcquiredCost ?? 0;
            equipment.AcquiredDate = equip.AcquiredDate;
            equipment.CatalogNumber=equip.CatalogNumber != null ? equip.CatalogNumber : emptyValue;
            equipment.CostCenter=equip.CostCenter != null ? equip.CostCenter : emptyValue;
            equipment.CriticalFlag = equip.CriticalFlag;
            equipment.InstallDate = equip.InstallDate;
            equipment.Location = equip.Location != null ? equip.Location : emptyValue;
            equipment.CreateMode = false;
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            equipment.UpdateByPKForeignKeys_V2(userData.DatabaseKey);
            return equipment;
        }

        #region AssignmentViewLog
        public List<AssignmentViewLogLookUpModel> PopulateAssignmentViewLogList(int pageNumber, int resultsPerPage, string orderColumn, string orderDirection, long objectId, string TransactionDate, string Status, string PersonnelId = "", string Assigned = "", string Location = "", string ParentId = "", string AssetGroup1 = "", string AssetGroup2 = "", string AssetGroup3 = "")
        {
            List<AssignmentViewLogLookUpModel> assignmentViewLogLookUpModelList = new List<AssignmentViewLogLookUpModel>();
            AssignmentViewLogLookUpModel assignmentViewLogLookUpModel;
            List<RepairableSpareLog> repairableSpareLogList = new List<RepairableSpareLog>();
            RepairableSpareLog repairableSpareLog = new RepairableSpareLog();
            repairableSpareLog.EquipmentId = objectId;
            repairableSpareLog.ClientId = userData.DatabaseKey.Client.ClientId;
            repairableSpareLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            repairableSpareLog.OrderbyColumn = orderColumn;
            repairableSpareLog.OrderBy = orderDirection;
            repairableSpareLog.OffSetVal = resultsPerPage;
            repairableSpareLog.NextRow = pageNumber;
            repairableSpareLog.TransactionDateS = TransactionDate;
            repairableSpareLog.Status = Status;
            repairableSpareLog.PersonnelName = PersonnelId;
            repairableSpareLog.AssignedClientLookupId = Assigned;
            repairableSpareLog.Location = Location;
            repairableSpareLog.ParentClientLookupId = ParentId;
            repairableSpareLog.AssetGroup1Name = AssetGroup1;
            repairableSpareLog.AssetGroup2Name = AssetGroup2;
            repairableSpareLog.AssetGroup3Name = AssetGroup3;
            repairableSpareLogList = repairableSpareLog.RetrieveByEquipmentId(_dbKey);
            foreach (var item in repairableSpareLogList)
            {
                assignmentViewLogLookUpModel = new AssignmentViewLogLookUpModel();
                assignmentViewLogLookUpModel.ObjectId = item.EquipmentId;
                assignmentViewLogLookUpModel.TransactionDate = item.TransactionDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                assignmentViewLogLookUpModel.Status = item.Status;
                assignmentViewLogLookUpModel.PersonnelId = item.PersonnelId;
                assignmentViewLogLookUpModel.Assigned = item.Assigned;
                assignmentViewLogLookUpModel.Location = item.Location;
                assignmentViewLogLookUpModel.AssetGroup1 = item.AssetGroup1;
                assignmentViewLogLookUpModel.AssetGroup2 = item.AssetGroup2;
                assignmentViewLogLookUpModel.AssetGroup3 = item.AssetGroup3;
                assignmentViewLogLookUpModel.PersonnelName = item.PersonnelName;
                assignmentViewLogLookUpModel.ParentClientLookupId = item.ParentClientLookupId;
                assignmentViewLogLookUpModel.AssetGroup1Name = item.AssetGroup1Name;
                assignmentViewLogLookUpModel.AssetGroup2Name = item.AssetGroup2Name;
                assignmentViewLogLookUpModel.AssetGroup3Name = item.AssetGroup3Name;
                assignmentViewLogLookUpModel.AssignedClientLookupId = item.AssignedClientLookupId;
                assignmentViewLogLookUpModelList.Add(assignmentViewLogLookUpModel);
            }

            return assignmentViewLogLookUpModelList;
        }
        #endregion
        #endregion

        #region DowntimeByEquipmentId
        public List<DownTimeModel> GetEquipmentDowntime_V2(long EquipmentId, string order, string orderDir, int skip = 0, int length = 10)
        {
            List<DownTimeModel> DownTimeModelList = new List<DownTimeModel>();
            DownTimeModel objDownTimeModel;
            Downtime downtime = new Downtime()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            downtime.OrderbyColumn=order;
            downtime.OrderBy=orderDir;
            downtime.offset1 = skip;
            downtime.nextrow = length;
            List<Downtime> equipmentDowntimeList = Downtime.RetriveByEquipmentId_V2(this.userData.DatabaseKey, downtime);
            if (equipmentDowntimeList != null)
            {
                var eqpList = equipmentDowntimeList.Select(x => new { x.EquipmentId, x.WorkOrderClientLookupId, x.DateDown, x.MinutesDown, x.DowntimeId, x.PersonnelClientLookupId,x.ReasonForDownDescription,x.TotalCount,x.TotalMinutesDown }).ToList();
                foreach (var v in eqpList)
                {
                    objDownTimeModel = new DownTimeModel();
                    objDownTimeModel.DateDown = Convert.ToDateTime(v.DateDown);
                    objDownTimeModel.EquipmentId = v.EquipmentId;
                    objDownTimeModel.MinutesDown = v.MinutesDown;
                    objDownTimeModel.WorkOrderClientLookupId = v.WorkOrderClientLookupId;
                    objDownTimeModel.DowntimeId = v.DowntimeId;
                    objDownTimeModel.DowntimeCreateSecurity = userData.Security.Asset_Downtime.Create;
                    objDownTimeModel.DowntimeEditSecurity = userData.Security.Asset_Downtime.Edit;
                    objDownTimeModel.DowntimeDeleteSecurity = userData.Security.Asset_Downtime.Delete;
                    objDownTimeModel.PersonnelClientLookupId = v.PersonnelClientLookupId;
                    objDownTimeModel.ReasonForDownDescription = v.ReasonForDownDescription;
                    objDownTimeModel.TotalCount=v.TotalCount;
                    objDownTimeModel.TotalMinutesDown=v.TotalMinutesDown;
                    DownTimeModelList.Add(objDownTimeModel);
                }
            }
            return DownTimeModelList;
        }
        #endregion
        #region
        public List<DataContracts.Equipment> GetEquipmentGridDataMobile(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string location = "", string serialNo = "", string type = "", string make = "", string modelNo = "", string account = "", string assetNumber = "", string searchText = "", int AssetGroup1Id = 0, int AssetGroup2Id = 0, int AssetGroup3Id = 0, string AssetAvailability = "")
        {
            DataContracts.Equipment equipment = new DataContracts.Equipment();

            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.CustomQueryDisplayId = CustomQueryDisplayId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = name;
            equipment.Location = location;

            equipment.SerialNo = serialNo;
            equipment.Type = type;
            equipment.Make = make;
            equipment.ModelNo = modelNo;
            equipment.Account = account;
            equipment.AssetNumber = assetNumber;
            equipment.SearchText = searchText;
            //<!--(Added on 25/06/2020)-->
            equipment.AssetGroup1Id = AssetGroup1Id;
            equipment.AssetGroup2Id = AssetGroup2Id;
            equipment.AssetGroup3Id = AssetGroup3Id;
            //<!--(Added on 25/06/2020)-->
            equipment.AssetAvailability = AssetAvailability;//V2-636 <!--(Added on 21/01/2022)-->
            equipment.EquipmentRetrieveChunkSearchMobileV2(userData.DatabaseKey, userData.Site.TimeZone);




            //<!--(Added on 26/06/2020)-->
            return equipment.listOfEquipment;
        }

        #endregion

        #region V2-537
        public List<SensorGridDataModel> GetEquipment_SensorGridData(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", long EquipmentId = 0)
        {
            IoTDevice device = new IoTDevice();
            device.ClientId = userData.DatabaseKey.Client.ClientId;
            device.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            device.EquipmentId = EquipmentId;
            device.OrderbyColumn = orderbycol;
            device.OrderBy = orderDir;
            device.OffSetVal = skip;
            device.NextRow = length;
            device.RetrieveChunkSearchByEquipmentId(userData.DatabaseKey, userData.Site.TimeZone);
            List<IoTDevice> equipmentSensorList = device.listOfDevice;
            List<SensorGridDataModel> SensorGridDataModelList = new List<SensorGridDataModel>();
            SensorGridDataModel objSensorGridDataModel;
            if (equipmentSensorList.Count > 0)
            {
                foreach (var v in equipmentSensorList)
                {
                    objSensorGridDataModel = new SensorGridDataModel();
                    objSensorGridDataModel.IoTDeviceId = v.IoTDeviceId;
                    objSensorGridDataModel.ClientLookupId = v.ClientLookupId;
                    objSensorGridDataModel.SensorType = v.SensorType;
                    objSensorGridDataModel.Name = v.Name;
                    objSensorGridDataModel.LastReading = v.LastReading;
                    objSensorGridDataModel.SensorUnit = v.SensorUnit;
                    objSensorGridDataModel.LastReadingDate = v.LastReadingDate;
                    objSensorGridDataModel.InactiveFlag = v.InactiveFlag;
                    SensorGridDataModelList.Add(objSensorGridDataModel);
                }
            }
            return SensorGridDataModelList;
        }
        #endregion

        #region V2-1007
        public List<PartsModel> GetEquipmentPartsByEquipmentIdPartId(long PartId, long EquipmentId)
        {
            List<PartsModel> PartsModelList = new List<PartsModel>();
            Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId,
                PartId = PartId
            };
            List<Equipment_Parts_Xref> equipmentParts_XrefList = Equipment_Parts_Xref.RetrieveByEquipmentIdPartId_V2(this.userData.DatabaseKey, eq);
            if (equipmentParts_XrefList != null)
            {
                var eqpList = equipmentParts_XrefList.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId, x.Equipment_ClientLookupId }).ToList();

                PartsModel objPartsModel;
                foreach (var v in eqpList)
                {
                    objPartsModel = new PartsModel();
                    objPartsModel.Comment = v.Comment;
                    objPartsModel.EquipmentId = v.EquipmentId;
                    objPartsModel.Part_ClientLookupId = v.Part_ClientLookupId;
                    objPartsModel.Part_Description = v.Part_Description;
                    objPartsModel.QuantityNeeded = v.QuantityNeeded;
                    objPartsModel.QuantityUsed = v.QuantityUsed;
                    objPartsModel.Equipment_Parts_XrefId = v.Equipment_Parts_XrefId;
                    objPartsModel.UpdatedIndex = v.UpdateIndex;
                    objPartsModel.PartId = v.PartId;
                    objPartsModel.Equipment_ClientLookupId = v.Equipment_ClientLookupId;
                    PartsModelList.Add(objPartsModel);
                }
            }
            foreach (var item in PartsModelList)
            {
                item.PartsSecurity = "true";
            }
            return PartsModelList;
        }
        #endregion

        #region V2-1202
        public AddEquipmentModelDynamic RetrieveEquipmentDetailsForAssteModelByEquipmentId(long EquipmentId)
        {
            AddEquipmentModelDynamic addEquipmentModelDynamic = new AddEquipmentModelDynamic();
            Equipment equipment = RetrieveEquipmentByEquipmentId(EquipmentId);
            EquipmentUDF equipmentUDF = RetrieveEquipmentUDFByEquipmentId(EquipmentId);
            addEquipmentModelDynamic = MapEquipmentUDFDataForEdit(addEquipmentModelDynamic, equipmentUDF);
            addEquipmentModelDynamic = MapEquipmentDataForEdit(addEquipmentModelDynamic, equipment);
            return addEquipmentModelDynamic;
        }

        private AddEquipmentModelDynamic MapEquipmentUDFDataForEdit(AddEquipmentModelDynamic addEquipmentModelDynamic, EquipmentUDF equipmentUDF)
        {
            if (equipmentUDF != null)
            {
                addEquipmentModelDynamic.EquipmentUDFId = equipmentUDF.EquipmentUDFId;
                addEquipmentModelDynamic.Text1 = equipmentUDF.Text1;
                addEquipmentModelDynamic.Text2 = equipmentUDF.Text2;
                addEquipmentModelDynamic.Text3 = equipmentUDF.Text3;
                addEquipmentModelDynamic.Text4 = equipmentUDF.Text4;
                if (equipmentUDF.Date1 != null && equipmentUDF.Date1 == DateTime.MinValue)
                {
                    addEquipmentModelDynamic.Date1 = null;
                }
                else
                {
                    addEquipmentModelDynamic.Date1 = equipmentUDF.Date1;
                }
                if (equipmentUDF.Date2 != null && equipmentUDF.Date2 == DateTime.MinValue)
                {
                    addEquipmentModelDynamic.Date2 = null;
                }
                else
                {
                    addEquipmentModelDynamic.Date2 = equipmentUDF.Date2;
                }
                if (equipmentUDF.Date3 != null && equipmentUDF.Date3 == DateTime.MinValue)
                {
                    addEquipmentModelDynamic.Date3 = null;
                }
                else
                {
                    addEquipmentModelDynamic.Date3 = equipmentUDF.Date3;
                }
                if (equipmentUDF.Date4 != null && equipmentUDF.Date4 == DateTime.MinValue)
                {
                    addEquipmentModelDynamic.Date4 = null;
                }
                else
                {
                    addEquipmentModelDynamic.Date4 = equipmentUDF.Date4;
                }

                addEquipmentModelDynamic.Bit1 = equipmentUDF.Bit1;
                addEquipmentModelDynamic.Bit2 = equipmentUDF.Bit2;
                addEquipmentModelDynamic.Bit3 = equipmentUDF.Bit3;
                addEquipmentModelDynamic.Bit4 = equipmentUDF.Bit4;

                addEquipmentModelDynamic.Numeric1 = equipmentUDF.Numeric1;
                addEquipmentModelDynamic.Numeric2 = equipmentUDF.Numeric2;
                addEquipmentModelDynamic.Numeric3 = equipmentUDF.Numeric3;
                addEquipmentModelDynamic.Numeric4 = equipmentUDF.Numeric4;

                addEquipmentModelDynamic.Select1 = equipmentUDF.Select1;
                addEquipmentModelDynamic.Select2 = equipmentUDF.Select2;
                addEquipmentModelDynamic.Select3 = equipmentUDF.Select3;
                addEquipmentModelDynamic.Select4 = equipmentUDF.Select4;
            }
            return addEquipmentModelDynamic;
        }
        public AddEquipmentModelDynamic MapEquipmentDataForEdit(AddEquipmentModelDynamic addEquipmentModelDynamic, Equipment equipment)
        {
            addEquipmentModelDynamic.EquipmentId = equipment.EquipmentId;
            addEquipmentModelDynamic.ClientLookupId = equipment.ClientLookupId;
            addEquipmentModelDynamic.BusinessGroup = equipment.BusinessGroup;
            addEquipmentModelDynamic.AcquiredCost = equipment.AcquiredCost;
            if (equipment.AcquiredDate != null && equipment.AcquiredDate != default(DateTime))
            {
                addEquipmentModelDynamic.AcquiredDate = equipment.AcquiredDate;
            }
            else
            {
                addEquipmentModelDynamic.AcquiredDate = null;
            }
            addEquipmentModelDynamic.CatalogNumber = equipment.CatalogNumber;
            addEquipmentModelDynamic.Category = equipment.Category;
            addEquipmentModelDynamic.CostCenter = equipment.CostCenter;
            addEquipmentModelDynamic.InactiveFlag = equipment.InactiveFlag;
            addEquipmentModelDynamic.CriticalFlag = equipment.CriticalFlag;
            if (equipment.InstallDate != null && equipment.InstallDate != default(DateTime))
            {
                addEquipmentModelDynamic.InstallDate = equipment.InstallDate;
            }
            else
            {
                addEquipmentModelDynamic.InstallDate = null;
            }
            addEquipmentModelDynamic.Location = equipment.Location;
            if (equipment.Maint_VendorId == 0)
            {
                addEquipmentModelDynamic.Maint_VendorId = null;
            }
            else
            {
                addEquipmentModelDynamic.Maint_VendorId = equipment.Maint_VendorId;
            }
            addEquipmentModelDynamic.Maint_WarrantyDesc = equipment.Maint_WarrantyDesc;
            if (equipment.Maint_WarrantyExpire != null && equipment.Maint_WarrantyExpire != default(DateTime))
            {
                addEquipmentModelDynamic.Maint_WarrantyExpire = equipment.Maint_WarrantyExpire;
            }
            else
            {
                addEquipmentModelDynamic.Maint_WarrantyExpire = null;
            }
            addEquipmentModelDynamic.Make = equipment.Make;
            if (equipment.Material_AccountId == 0)
            {
                addEquipmentModelDynamic.Material_AccountId = null;
                addEquipmentModelDynamic.Material_AccountName = "";
            }
            else
            {
                addEquipmentModelDynamic.Material_AccountId = equipment.Material_AccountId;
                addEquipmentModelDynamic.Material_AccountName = equipment.MaterialAccountClientLookupId;
            }
            if (equipment.Labor_AccountId == 0)
            {
                addEquipmentModelDynamic.Labor_AccountId = null;
                addEquipmentModelDynamic.Labor_AccountName = "";
            }
            else
            {
                addEquipmentModelDynamic.Labor_AccountId = equipment.Labor_AccountId;
                addEquipmentModelDynamic.Labor_AccountName = equipment.LaborAccountClientLookupId;
            }
            addEquipmentModelDynamic.Model = equipment.Model;
            addEquipmentModelDynamic.Name = equipment.Name;
            addEquipmentModelDynamic.NoPartXRef = equipment.NoPartXRef;
            addEquipmentModelDynamic.OriginalValue = equipment.OriginalValue;
            addEquipmentModelDynamic.OriginalValue = equipment.OriginalValue;
            if (equipment.OutofService != null && equipment.OutofService != default(DateTime))
            {
                addEquipmentModelDynamic.OutofService = equipment.OutofService;
            }
            else
            {
                addEquipmentModelDynamic.OutofService = null;
            }
            if (equipment.Purch_VendorId == 0)
            {
                addEquipmentModelDynamic.Purch_VendorId = null;
            }
            else
            {
                addEquipmentModelDynamic.Purch_VendorId = equipment.Purch_VendorId;
            }
            addEquipmentModelDynamic.Purch_WarrantyDesc = equipment.Purch_WarrantyDesc;
            if (equipment.Purch_WarrantyExpire != null && equipment.Purch_WarrantyExpire != default(DateTime))
            {
                addEquipmentModelDynamic.Purch_WarrantyExpire = equipment.Purch_WarrantyExpire;
            }
            else
            {
                addEquipmentModelDynamic.Purch_WarrantyExpire = null;
            }
            addEquipmentModelDynamic.SerialNumber = equipment.SerialNumber;
            addEquipmentModelDynamic.Status = equipment.Status;
            addEquipmentModelDynamic.Type = equipment.Type;
            addEquipmentModelDynamic.AssetNumber = equipment.AssetNumber;
            addEquipmentModelDynamic.AssetCategory = equipment.AssetCategory;

            if (equipment.AssetGroup1 == 0)
            {
                addEquipmentModelDynamic.AssetGroup1 = null;
            }
            else
            {
                addEquipmentModelDynamic.AssetGroup1 = equipment.AssetGroup1;
            }
            if (equipment.AssetGroup2 == 0)
            {
                addEquipmentModelDynamic.AssetGroup2 = null;
            }
            else
            {
                addEquipmentModelDynamic.AssetGroup2 = equipment.AssetGroup2;
            }
            if (equipment.AssetGroup3 == 0)
            {
                addEquipmentModelDynamic.AssetGroup3 = null;
            }
            else
            {
                addEquipmentModelDynamic.AssetGroup3 = equipment.AssetGroup3;
            }
            if (equipment.ElectricalParent == 0)
            {
                addEquipmentModelDynamic.ElectricalParent = null;
                addEquipmentModelDynamic.ElectricalParent_ClientLookupId = "";
            }
            else
            {
                addEquipmentModelDynamic.ElectricalParent = equipment.ElectricalParent;
                addEquipmentModelDynamic.ElectricalParent_ClientLookupId = equipment.ElectricalParentClientLookupId;
            }

            addEquipmentModelDynamic.ParentIdClientLookupId = equipment.ParentIdClientLookupId;
            addEquipmentModelDynamic.Purch_VendorName = equipment.PurchVendorIdClientLookupId;
            addEquipmentModelDynamic.Maint_VendorName = equipment.MaintVendorIdClientLookupId;

            return addEquipmentModelDynamic;
        }

        public Equipment AddNewAssetModelDynamic(EquipmentCombined objEM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Equipment equipment = new Equipment();
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.Lookup_ListName = LookupListConstants.Equipment_EquipType;
            equipment.CreateMode = true;
            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddAsset, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, objEM.AddEquipment);
                getpropertyInfo = objEM.AddEquipment.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objEM.AddEquipment);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = equipment.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(equipment, val);
            }
            equipment.Status = AssetStatusConstant.InService;
            equipment.CreateByPKForeignKeys_V2(this.userData.DatabaseKey);
            if (equipment.ErrorMessages != null && equipment.ErrorMessages.Count == 0 && configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
            {
                IEnumerable<string> errors = AddNewAssetModelUDFDynamic(objEM.AddEquipment, equipment.EquipmentId, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    equipment.ErrorMessages.AddRange(errors);
                }
            }

            if (objEM.Copy_Part_Xref == true && equipment.ErrorMessages != null && equipment.ErrorMessages.Count == 0)
            {
                List<PartsModel> PartsModelList = new List<PartsModel>();
                Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EquipmentId = objEM.AddEquipment.EquipmentId ?? 0,
                };
                List<Equipment_Parts_Xref> equipmentParts_XrefList = Equipment_Parts_Xref.RetrieveByEquipmentId_V2(this.userData.DatabaseKey, eq);
                if (equipmentParts_XrefList != null)
                {
                    foreach (var item in equipmentParts_XrefList)
                    {
                        Equipment_Parts_Xref equipmentParts_Xref = new Equipment_Parts_Xref();
                        equipmentParts_Xref.ClientId = userData.DatabaseKey.Client.ClientId;
                        equipmentParts_Xref.EquipmentId = equipment.EquipmentId;
                        equipmentParts_Xref.PartId = item.PartId;
                        equipmentParts_Xref.Comment = item.Comment;
                        equipmentParts_Xref.QuantityNeeded = item.QuantityNeeded;
                        equipmentParts_Xref.QuantityUsed = 0;
                        equipmentParts_Xref.Create(userData.DatabaseKey);
                    }
                }
            }
            if (objEM.Copy_TechSpecs == true && equipment.ErrorMessages != null && equipment.ErrorMessages.Count == 0)
            {
                List<TechSpecsModel> TechSpecsModelList = new List<TechSpecsModel>();

                Equipment_TechSpecs eq = new Equipment_TechSpecs()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    EquipmentId = objEM.AddEquipment.EquipmentId ?? 0,
                };
                List<Equipment_TechSpecs> equipmentTechSpecsList = Equipment_TechSpecs.RetriveByEquipmentId(this.userData.DatabaseKey, eq);
                if (equipmentTechSpecsList != null)
                {
                    foreach (var item in equipmentTechSpecsList)
                    {
                        Equipment_TechSpecs equipmentTechSpecs = new Equipment_TechSpecs();
                        equipmentTechSpecs.ClientId = userData.DatabaseKey.Client.ClientId;
                        equipmentTechSpecs.EquipmentId = equipment.EquipmentId;
                        equipmentTechSpecs.TechSpecId = item.TechSpecId;
                        equipmentTechSpecs.SpecValue = item.SpecValue;
                        equipmentTechSpecs.Comments = item.Comments;
                        equipmentTechSpecs.Create(userData.DatabaseKey);
                    }
                }
            }
            if (objEM.Copy_Notes == true && equipment.ErrorMessages != null && equipment.ErrorMessages.Count == 0)
            {
                List<NoteModel> noteList = new List<NoteModel>();

                Notes note = new Notes()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    ObjectId = objEM.AddEquipment.EquipmentId ?? 0,
                    TableName = "Equipment"
                };
                List<DataContracts.Notes> lst = note.RetrieveByObjectId(this.userData.DatabaseKey, this.userData.Site.TimeZone);

                foreach (var item in lst)
                {
                    Notes model = new Notes();
                    model.ClientId = userData.DatabaseKey.Client.ClientId;
                    model.OwnerId = item.OwnerId;
                    model.OwnerName = item.OwnerName;
                    model.Subject = item.Subject;
                    model.Content = item.Content;
                    model.Type = item.Type;
                    model.ObjectId = equipment.EquipmentId;
                    model.TableName = "Equipment";
                    model.ModifiedDate = item.ModifiedDate;
                    model.Create(userData.DatabaseKey);
                }
            }

            string Event = "Create";
            string Comments = "Created from Asset Model";
            IEnumerable<string> errors1 = CreateAssetModelEvent(objEM.AddEquipment, equipment.EquipmentId, Event, Comments);
            if (errors1 != null && errors1.Count() > 0)
            {
                equipment.ErrorMessages.AddRange(errors1);
            }

            return equipment;
        }
        public List<string> AddNewAssetModelUDFDynamic(AddEquipmentModelDynamic equipment, long EquipmentId, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            EquipmentUDF equipmentUDF = new EquipmentUDF();
            equipmentUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            equipmentUDF.EquipmentId = EquipmentId;
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                item.ColumnName = UtilityFunction.ReturnPropertyNameWithoutCaseComparison(item.ColumnName, equipment);
                getpropertyInfo = equipment.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(equipment);


                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = equipmentUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(equipmentUDF, val);
            }
            equipmentUDF.Create(_dbKey);
            return equipmentUDF.ErrorMessages;
        }
        public List<string> CreateAssetModelEvent(AddEquipmentModelDynamic equipment, long EquipmentId, string Event, string Comments)
        {

            AssetEventLog assetEventLog = new AssetEventLog();
            assetEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            assetEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetEventLog.EquipmentId = EquipmentId;
            assetEventLog.TransactionDate = DateTime.UtcNow;
            assetEventLog.Event = Event;
            assetEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            assetEventLog.Comments = Comments;
            assetEventLog.SourceId = equipment.EquipmentId ?? 0;
            assetEventLog.Create(userData.DatabaseKey);
            return assetEventLog.ErrorMessages;
        }
        #endregion

        #region V2-1213
        public List<ChildrenGridDataModel> GetEquipment_ChildrenGridData(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", long EquipmentId = 0,string ClientLookupId="", string Name = "", string SerialNumber = "", string Type = "", string Make = "",string Model = "")
        {
            Equipment equipment = new Equipment();
            equipment.ClientId = userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.EquipmentId = EquipmentId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = ClientLookupId;
            equipment.Name = Name;
            equipment.SerialNumber = SerialNumber;
            equipment.Type = Type;
            equipment.Make = Make;
            equipment.Model = Model;           ;
            List<Equipment> equipmentChildrenList = equipment.RetrieveAllChildrenChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            List<ChildrenGridDataModel> ChildrenGridDataModelList = new List<ChildrenGridDataModel>();
            ChildrenGridDataModel objChildrenGridDataModel;
            if (equipmentChildrenList.Count > 0)
            {
                foreach (var v in equipmentChildrenList)
                {
                    objChildrenGridDataModel = new ChildrenGridDataModel();
                    objChildrenGridDataModel.EquipmentId = v.EquipmentId;
                    objChildrenGridDataModel.ClientLookupId = v.ClientLookupId;
                    objChildrenGridDataModel.Name = v.Name;
                    objChildrenGridDataModel.SerialNumber = v.SerialNumber;
                    objChildrenGridDataModel.Type = v.Type;
                    objChildrenGridDataModel.Make = v.Make;
                    objChildrenGridDataModel.Model = v.Model;
                    objChildrenGridDataModel.TotalCount = v.TotalCount;
                    ChildrenGridDataModelList.Add(objChildrenGridDataModel);
                }
            }
            return ChildrenGridDataModelList;
        }
        #endregion
    }

}
