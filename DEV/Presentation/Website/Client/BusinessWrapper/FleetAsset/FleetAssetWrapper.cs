using Client.BusinessWrapper.Common;
using Client.Models;
using Client.Models.FleetAsset;
using Common.Constants;
using Common.Extensions;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Client.BusinessWrapper.FleetAsset
{
    public class FleetAssetWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public FleetAssetWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region GetFleetAsset       
        public FleetAssetModel GetFleetAssetDetailsById(long EquipmentId)
        {
            FleetAssetModel FleetAssetDetails = new FleetAssetModel();
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
            FleetAssetDetails = initializeDetailsControls(equipment);

            return FleetAssetDetails;
        }

        public FleetAssetModel GetEditFleetAssetDetailsById(long EquipmentId)
        {
            FleetAssetModel objFleetAsset = new FleetAssetModel();
            DataContracts.Equipment equipment = new DataContracts.Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            equipment.RetrieveByPKForeignKeys_V2(_dbKey);
            objFleetAsset = initializeDetailsControls(equipment);
            return objFleetAsset;
        }
        public FleetAssetModel initializeDetailsControls(Equipment obj)
        {
            FleetAssetModel objFleetAsset = new FleetAssetModel();
            objFleetAsset.EquipmentID = Convert.ToString(obj.EquipmentId);
            objFleetAsset.Name = obj.Name;
            objFleetAsset.SerialNumber = obj.SerialNumber;
            objFleetAsset.VIN = obj.VIN;
            objFleetAsset.Make = obj.Make;
            objFleetAsset.ModelNumber = obj.Model;
            objFleetAsset.Model = obj.Model;
            objFleetAsset.VehicleType = obj.VehicleType;
            objFleetAsset.VehicleYear = obj.VehicleYear;
            objFleetAsset.License = obj.License;
            objFleetAsset.RegistrationLoc = obj.RegistrationLoc;
            objFleetAsset.Color = obj.Color;
            objFleetAsset.BodyType = obj.BodyType;
            objFleetAsset.Width = obj.Width;
            objFleetAsset.Height = obj.Height;
            objFleetAsset.Length = obj.Length;
            objFleetAsset.PassengerVolume = obj.PassengerVolume;
            objFleetAsset.CargoVolume = obj.CargoVolume;
            objFleetAsset.GroundClearance = obj.GroundClearance;
            objFleetAsset.BedLength = obj.BedLength;
            objFleetAsset.CurbWeight =obj.CurbWeight;
            objFleetAsset.VehicleWeight =obj.VehicleWeight;
            objFleetAsset.TowingCapacity = obj.TowingCapacity;
            objFleetAsset.MaxPayload =obj.MaxPayload;
            objFleetAsset.EngineBrand = obj.EngineBrand;
            objFleetAsset.Aspiration = obj.Aspiration;
            objFleetAsset.Bore = obj.Bore;
            objFleetAsset.Cam = obj.Cam;
            objFleetAsset.Compression = obj.Compression;
            objFleetAsset.Cylinders = obj.Cylinders;
            objFleetAsset.Displacement = obj.Displacement;
            objFleetAsset.FuelInduction = obj.FuelInduction;
            objFleetAsset.FuelQuality = obj.FuelQuality;
            objFleetAsset.MaxHP = obj.MaxHP;
            objFleetAsset.MaxTorque = obj.MaxTorque;
            objFleetAsset.RedlineRPM = obj.RedlineRPM;
            objFleetAsset.Stroke = obj.Stroke;
            objFleetAsset.Valves = obj.Valves;
            objFleetAsset.TransmissionBrand = obj.TransmissionBrand;
            objFleetAsset.TransmissionType = obj.TransmissionType;
            objFleetAsset.Gears = obj.Gears;
            objFleetAsset.BrakeSystem = obj.BrakeSystem;
            objFleetAsset.RearTrackWidth = obj.RearTrackWidth;
            objFleetAsset.Wheelbase = obj.Wheelbase;
            objFleetAsset.FrontWheelDiameter = obj.FrontWheelDiameter;
            objFleetAsset.RearWheelDiameter = obj.RearWheelDiameter;
            objFleetAsset.FrontTirePSI = obj.FrontTirePSI;
            objFleetAsset.RearTirePSI = obj.RearTirePSI;
            objFleetAsset.FleetFuelQuality = obj.FleetFluidsFuelQuality;
            objFleetAsset.FuelType = obj.FuelType;
            objFleetAsset.FuelTankCapacity1 = obj.FuelTankCapacity1;
            objFleetAsset.FuelTankCapacity2 = obj.FuelTankCapacity2;
            objFleetAsset.EPACity = obj.EPACity;
            objFleetAsset.EPAHighway = obj.EPAHighway;
            objFleetAsset.EPACombined = obj.EPACombined;           
            objFleetAsset.InactiveFlag = obj.InactiveFlag;
            objFleetAsset.HiddenInactiveFlag = obj.InactiveFlag;
            objFleetAsset.CriticalFlag = obj.CriticalFlag;
            objFleetAsset.MaintVendorIdClientLookupId = obj.MaintVendorIdClientLookupId;
            objFleetAsset.ClientLookupId = obj.ClientLookupId;
            objFleetAsset.FuelUnits = obj.FuelUnits;
            objFleetAsset.Meter1Type = obj.Meter1Type;
            objFleetAsset.Meter1Units = obj.Meter1Units;
            objFleetAsset.Meter2Type = obj.Meter2Type;
            objFleetAsset.Meter2Units = obj.Meter2Units;
            objFleetAsset.RemoveFromService = obj.RemoveFromService;
            objFleetAsset.RemoveFromServiceDate = obj.RemoveFromServiceDate;
            objFleetAsset.RemoveFromServiceReason = obj.RemoveFromServiceReason;
            objFleetAsset.ExpectedReturnToService = obj.ExpectedReturnToService;
            objFleetAsset.Meter1CurrentReading = obj.Meter1CurrentReading;
            objFleetAsset.Meter2CurrentReading = obj.Meter2CurrentReading;
            objFleetAsset.RemoveServiceDate=Convert.ToDateTime(obj.RemoveFromServiceDate).ToUserTimeZone(userData.Site.TimeZone);
            objFleetAsset.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            return objFleetAsset;
        }      

        public List<FleetAssetSearchModel> GetFleetAssetGridData(int CustomQueryDisplayId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, string clientLookupId = "", string name = "", string make = "", string model = "", string vIN = "", string vehicleType = "", string AssetAvailability = "", string searchText = "")
        {
            FleetAssetSearchModel fleetAssetSearchModel;
            List<FleetAssetSearchModel> fleetAssetSearchModelList = new List<FleetAssetSearchModel>();
            List<Equipment> equipmentList = new List<Equipment>();
            Equipment equipment = new Equipment();
            equipment.ClientId = this.userData.DatabaseKey.Client.ClientId;
            equipment.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            equipment.CustomQueryDisplayId = CustomQueryDisplayId;
            equipment.OrderbyColumn = orderbycol;
            equipment.OrderBy = orderDir;
            equipment.OffSetVal = skip;
            equipment.NextRow = length;
            equipment.ClientLookupId = clientLookupId;
            equipment.Name = name;
            equipment.Make = make;
            equipment.Model = model;
            equipment.VIN = vIN;
            equipment.VehicleType = vehicleType;
            equipment.AssetAvailability = AssetAvailability;
            equipment.SearchText = searchText;
            equipmentList = equipment.FleetAssetRetrieveChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in equipmentList)
            {
                fleetAssetSearchModel = new FleetAssetSearchModel();
                fleetAssetSearchModel.EquipmentId = item.EquipmentId;
                fleetAssetSearchModel.ClientLookupId = item.ClientLookupId;
                fleetAssetSearchModel.Name = item.Name;
                fleetAssetSearchModel.VIN = item.VIN;
                fleetAssetSearchModel.VehicleType = item.VehicleType;
                fleetAssetSearchModel.VehicleYear = item.VehicleYear;
                fleetAssetSearchModel.Make = item.Make;
                fleetAssetSearchModel.Model = item.Model;
                fleetAssetSearchModel.RemoveFromService = item.RemoveFromService;
                if (item.RemoveFromServiceDate != null && item.RemoveFromServiceDate == default(DateTime))
                {
                    fleetAssetSearchModel.RemoveFromServiceDate = null;
                }
                else
                {
                    fleetAssetSearchModel.RemoveFromServiceDate = item.RemoveFromServiceDate;
                }
                fleetAssetSearchModel.TotalCount = item.TotalCount;
                fleetAssetSearchModelList.Add(fleetAssetSearchModel);
            }

            return fleetAssetSearchModelList;
        }
        
        #endregion
        #region common functionalities 

        public Equipment ValidateFltAstStatusChange(long equipId, string flag, string clientLookupId)
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

        public List<string> UpdateFltAstActiveStatus(long equipId, bool inActiveFlag)
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

        public List<string> CreateAssetEvent(long equipId, bool inActiveFlag)
        {
            AssetEventLog assetEventLog = new AssetEventLog();
            assetEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            assetEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            assetEventLog.EquipmentId = equipId;
            assetEventLog.TransactionDate = DateTime.UtcNow;
            if (inActiveFlag)
            {
                assetEventLog.Event = "Activate";
            }
            else
            {
                assetEventLog.Event = "Inactivate";
            }
            assetEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            assetEventLog.Comments = "";
            assetEventLog.SourceId = 0;
            assetEventLog.Create(userData.DatabaseKey);
            return assetEventLog.ErrorMessages;
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
        public Equipment GetFltAstDetailsById(long EquipmentId)
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
            return equipment;
        }
        public CreatedLastUpdatedFleetAssetModel createdLastUpdatedModel(long _EquipmentId)
        {
            CreatedLastUpdatedFleetAssetModel _CreatedLastUpdatedModel = new CreatedLastUpdatedFleetAssetModel();
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
        public List<String> ChangeFltAstId(long _eqid, ChangeFleetAssetIDModel _ChangeFleetAssetIDModel)
        {
            FleetAssetVM objComb = new FleetAssetVM();
            FleetAssetModel objEq = new FleetAssetModel();
            List<string> EMsg = new List<string>();
            if (_eqid > 0)
            {
                Equipment equip = new Equipment();
                equip.ClientId = userData.DatabaseKey.Client.ClientId;
                equip.SiteId = userData.DatabaseKey.Personnel.SiteId;
                equip.EquipmentId = _eqid;
                equip.ClientLookupId = _ChangeFleetAssetIDModel.ClientLookupId;
                equip.UpdateIndex = Convert.ToInt32(_ChangeFleetAssetIDModel.UpdateIndex);
                equip.Lookup_ListName = LookupListConstants.Equipment_EquipType;
                equip.CreateMode = true;
                equip.ChangeClientLookupId(userData.DatabaseKey);
                if (equip.ErrorMessages.Count == 0)
                {
                    EMsg = equip.ErrorMessages;

                }
                else
                {
                    EMsg = equip.ErrorMessages;

                }
            }
            return EMsg;
        }

        #region Part
        public List<FleetAssetPartsModel> GetFleetAssetParts(long EquipmentId)
        {
            List<FleetAssetPartsModel> PartsModelList = new List<FleetAssetPartsModel>();
            Equipment_Parts_Xref eq = new Equipment_Parts_Xref()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = EquipmentId
            };
            List<Equipment_Parts_Xref> equipmentParts_XrefList = Equipment_Parts_Xref.RetrieveByEquipmentId(this.userData.DatabaseKey, eq);
            if (equipmentParts_XrefList != null)
            {
                var eqpList = equipmentParts_XrefList.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                FleetAssetPartsModel objPartsModel;
                foreach (var v in eqpList)
                {
                    objPartsModel = new FleetAssetPartsModel();
                    objPartsModel.Comment = v.Comment;
                    objPartsModel.EquipmentId = v.EquipmentId;
                    objPartsModel.Part_ClientLookupId = v.Part_ClientLookupId;
                    objPartsModel.Part_Description = v.Part_Description;
                    objPartsModel.QuantityNeeded = v.QuantityNeeded;
                    objPartsModel.QuantityUsed = v.QuantityUsed;
                    objPartsModel.Equipment_Parts_XrefId = v.Equipment_Parts_XrefId;
                    //objPartsModel.PartsSecurity = userData.Security.Parts.Part_Equipment_XRef == true ? "true" : "false";
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

        public FleetAssetVM EditParts(long EquipmentId, long Equipment_Parts_XrefId, FleetAssetVM objCombi)
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

        public FleetAssetVM UpdatePart(FleetAssetVM ec)
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

        public FleetAssetVM AddPart(FleetAssetVM ec)
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

        #endregion

        #region Fleet Asset Add-Edit
        public Equipment AddFleetAsset(string FA_ClientLookupId, FleetAssetVM objEM)
        {
            FleetAssetModel objFleetAsset = new FleetAssetModel();

            #region Equipment
            Equipment equipment = new Equipment();
            equipment.ClientId = this.userData.DatabaseKey.User.ClientId;
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientLookupId = FA_ClientLookupId;
            equipment.Name = objEM.FleetAssetModel.Name;
            equipment.VIN = objEM.FleetAssetModel.VIN;
            equipment.VehicleType = objEM.FleetAssetModel.VehicleType;
            equipment.VehicleYear = objEM.FleetAssetModel.VehicleYear;
            equipment.Make = objEM.FleetAssetModel.Make;
            equipment.Model = objEM.FleetAssetModel.Model;
            equipment.License = objEM.FleetAssetModel.License;
            equipment.RegistrationLoc = objEM.FleetAssetModel.RegistrationLoc;
            equipment.AssetCategory = "Vehicle";
            equipment.CheckDuplicateEquipment(this.userData.DatabaseKey);
            equipment.FuelUnits = objEM.FleetAssetModel.FuelUnits;
            equipment.Meter1Type= objEM.FleetAssetModel.Meter1Type;
            equipment.Meter1Units = objEM.FleetAssetModel.Meter1Units;
            equipment.Meter2Type = objEM.FleetAssetModel.Meter2Type;
            equipment.Meter2Units = objEM.FleetAssetModel.Meter2Units;

            if (equipment.ErrorMessages == null || equipment.ErrorMessages.Count == 0)

            { equipment.Create(this.userData.DatabaseKey);

                #endregion
                #region Tasking         
                Task[] tasks = new Task[4];
            if (objEM.FleetAssetModel.isfleetDimensionData == true)
            {

                #region FleetDimensions
                FleetDimensions fleetDimensions = new FleetDimensions();
                fleetDimensions.ClientId = this.userData.DatabaseKey.User.ClientId;
                fleetDimensions.EquipmentId = equipment.EquipmentId;
                fleetDimensions.Color = objEM.FleetAssetModel.Color;
                fleetDimensions.BodyType = objEM.FleetAssetModel.BodyType;
                fleetDimensions.Width = objEM.FleetAssetModel.Width;
                fleetDimensions.Height = objEM.FleetAssetModel.Height;
                fleetDimensions.Length = objEM.FleetAssetModel.Length;
                fleetDimensions.PassengerVolume = objEM.FleetAssetModel.PassengerVolume;
                fleetDimensions.CargoVolume = objEM.FleetAssetModel.CargoVolume;
                fleetDimensions.GroundClearance = objEM.FleetAssetModel.GroundClearance;
                fleetDimensions.BedLength = objEM.FleetAssetModel.BedLength;
                fleetDimensions.CurbWeight = objEM.FleetAssetModel.CurbWeight;
                fleetDimensions.VehicleWeight = objEM.FleetAssetModel.VehicleWeight;
                fleetDimensions.TowingCapacity = objEM.FleetAssetModel.TowingCapacity;
                fleetDimensions.MaxPayload = objEM.FleetAssetModel.MaxPayload;
                tasks[0] = Task.Factory.StartNew(() => fleetDimensions.Create(this.userData.DatabaseKey));
                #endregion

            }
            if (objEM.FleetAssetModel.isfleetEngineData == true)
            {
                #region FleetEngine
                FleetEngine fleetEngine = new FleetEngine();
                fleetEngine.ClientId = this.userData.DatabaseKey.User.ClientId;
                fleetEngine.EquipmentId = equipment.EquipmentId;
                fleetEngine.EngineBrand = objEM.FleetAssetModel.EngineBrand;
                fleetEngine.Aspiration = objEM.FleetAssetModel.Aspiration;
                fleetEngine.Bore = objEM.FleetAssetModel.Bore;
                fleetEngine.Cam = objEM.FleetAssetModel.Cam;
                fleetEngine.Compression = objEM.FleetAssetModel.Compression;
                fleetEngine.Cylinders = objEM.FleetAssetModel.Cylinders;
                fleetEngine.Displacement = objEM.FleetAssetModel.Displacement;
                fleetEngine.FuelInduction = objEM.FleetAssetModel.FuelInduction;
                fleetEngine.FuelQuality = objEM.FleetAssetModel.FuelQuality;
                fleetEngine.MaxHP = objEM.FleetAssetModel.MaxHP;
                fleetEngine.MaxTorque = objEM.FleetAssetModel.MaxTorque;
                fleetEngine.RedlineRPM = objEM.FleetAssetModel.RedlineRPM;
                fleetEngine.Stroke = objEM.FleetAssetModel.Stroke;
                fleetEngine.Valves = objEM.FleetAssetModel.Valves;
                fleetEngine.TransmissionBrand = objEM.FleetAssetModel.TransmissionBrand;
                fleetEngine.TransmissionType = objEM.FleetAssetModel.TransmissionType;
                fleetEngine.Gears = objEM.FleetAssetModel.Gears;
                tasks[1] = Task.Factory.StartNew(() => fleetEngine.Create(this.userData.DatabaseKey));
                #endregion
            }
            if (objEM.FleetAssetModel.isfleetFluidsData == true)
            {
                #region FleetFluids
                FleetFluids fleetFluids = new FleetFluids();
                fleetFluids.ClientId = this.userData.DatabaseKey.User.ClientId;
                fleetFluids.EquipmentId = equipment.EquipmentId;
                fleetFluids.FuelQuality = objEM.FleetAssetModel.FleetFuelQuality;
                fleetFluids.FuelType = objEM.FleetAssetModel.FuelType;
                fleetFluids.FuelTankCapacity1 = objEM.FleetAssetModel.FuelTankCapacity1;
                fleetFluids.FuelTankCapacity2 = objEM.FleetAssetModel.FuelTankCapacity2;
                fleetFluids.EPACity = objEM.FleetAssetModel.EPACity;
                fleetFluids.EPAHighway = objEM.FleetAssetModel.EPAHighway;
                fleetFluids.EPACombined = objEM.FleetAssetModel.EPACombined;
                tasks[2] = Task.Factory.StartNew(() => fleetFluids.Create(this.userData.DatabaseKey));
                #endregion
            }

            if (objEM.FleetAssetModel.isfleetWheelData == true)
            {
                #region FleetFleetWheel
                FleetWheel fleetWheel = new FleetWheel();
                fleetWheel.ClientId = this.userData.DatabaseKey.User.ClientId;
                fleetWheel.EquipmentId = equipment.EquipmentId;
                fleetWheel.BrakeSystem = objEM.FleetAssetModel.BrakeSystem;
                fleetWheel.RearTrackWidth = objEM.FleetAssetModel.RearTrackWidth;
                fleetWheel.Wheelbase = objEM.FleetAssetModel.Wheelbase;
                fleetWheel.FrontWheelDiameter = objEM.FleetAssetModel.FrontWheelDiameter;
                fleetWheel.RearWheelDiameter = objEM.FleetAssetModel.RearWheelDiameter;
                fleetWheel.FrontTirePSI = objEM.FleetAssetModel.FrontTirePSI;
                fleetWheel.RearTirePSI = objEM.FleetAssetModel.RearTirePSI;
                tasks[3] = Task.Factory.StartNew(() => fleetWheel.Create(this.userData.DatabaseKey));
                #endregion
            }

            Task.WaitAll();
                #endregion
            }
            return equipment;
        }
       
        public Equipment UpdateFleetAsset(FleetAssetVM flpast)
        {
            string emptyValue = string.Empty;
            FleetAssetModel objFleetAsset = new FleetAssetModel();
            List<string> ErrorList = new List<string>();
           
            #region Equipment
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(flpast.FleetAssetModel.EquipmentID)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);

            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            equipment.ClientLookupId = flpast.FleetAssetModel.ClientLookupId;
            equipment.Name = flpast.FleetAssetModel.Name;
            equipment.VIN = flpast.FleetAssetModel.VIN != null ? flpast.FleetAssetModel.VIN : emptyValue; 
            equipment.VehicleType = flpast.FleetAssetModel.VehicleType != null ? flpast.FleetAssetModel.VehicleType : emptyValue;
            equipment.VehicleYear = flpast.FleetAssetModel.VehicleYear != 0 ? flpast.FleetAssetModel.VehicleYear : 0;
            equipment.Make = flpast.FleetAssetModel.Make != null ? flpast.FleetAssetModel.Make : emptyValue;
            equipment.Model = flpast.FleetAssetModel.Model != null ? flpast.FleetAssetModel.Model : emptyValue;
            equipment.License = flpast.FleetAssetModel.License != null ? flpast.FleetAssetModel.License : emptyValue;
            equipment.RegistrationLoc = flpast.FleetAssetModel.RegistrationLoc != null ? flpast.FleetAssetModel.RegistrationLoc : emptyValue;
            equipment.FuelUnits= flpast.FleetAssetModel.FuelUnits != null ? flpast.FleetAssetModel.FuelUnits : emptyValue;
            equipment.Meter1Type = flpast.FleetAssetModel.Meter1Type != null ? flpast.FleetAssetModel.Meter1Type : emptyValue;
            equipment.Meter1Units = flpast.FleetAssetModel.Meter1Units != null ? flpast.FleetAssetModel.Meter1Units : emptyValue;
            equipment.Meter2Type = flpast.FleetAssetModel.Meter2Type != null ? flpast.FleetAssetModel.Meter2Type : emptyValue;
            equipment.Meter2Units = flpast.FleetAssetModel.Meter2Units != null ? flpast.FleetAssetModel.Meter2Units : emptyValue;
            equipment.Update(userData.DatabaseKey);
            #endregion
            #region Tasking  
            Task[] tasks = new Task[4];
            #region FleetDimensions
            FleetDimensions fleetDimensions = new FleetDimensions()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(flpast.FleetAssetModel.EquipmentID)
            };
            fleetDimensions.Color = flpast.FleetAssetModel.Color != null ? flpast.FleetAssetModel.Color : emptyValue;
            fleetDimensions.BodyType = flpast.FleetAssetModel.BodyType != null ? flpast.FleetAssetModel.BodyType : emptyValue;
            fleetDimensions.Width = flpast.FleetAssetModel.Width;
            fleetDimensions.Height = flpast.FleetAssetModel.Height;
            fleetDimensions.Length = flpast.FleetAssetModel.Length;
            fleetDimensions.PassengerVolume = flpast.FleetAssetModel.PassengerVolume;
            fleetDimensions.CargoVolume = flpast.FleetAssetModel.CargoVolume;
            fleetDimensions.GroundClearance = flpast.FleetAssetModel.GroundClearance;
            fleetDimensions.BedLength = flpast.FleetAssetModel.BedLength;
            fleetDimensions.CurbWeight = flpast.FleetAssetModel.CurbWeight;
            fleetDimensions.VehicleWeight = flpast.FleetAssetModel.VehicleWeight;
            fleetDimensions.TowingCapacity = flpast.FleetAssetModel.TowingCapacity;
            fleetDimensions.MaxPayload = flpast.FleetAssetModel.MaxPayload;

            if (equipment.FleetDimensionsId > 0)
            {
                fleetDimensions.FleetDimensionsId = equipment.FleetDimensionsId;
                fleetDimensions.UpdateIndex = equipment.FleetDimensionUpdateIndex;
                tasks[0] = Task.Factory.StartNew(() => fleetDimensions.Update(this.userData.DatabaseKey));
            }
            else
            {
                if (flpast.FleetAssetModel.isfleetDimensionData == true)
                {
                    tasks[0] = Task.Factory.StartNew(() => fleetDimensions.Create(this.userData.DatabaseKey));
                }
            }

            #endregion

            #region FleetEngine
            FleetEngine fleetEngine = new FleetEngine()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(flpast.FleetAssetModel.EquipmentID)
            };
            fleetEngine.EngineBrand = flpast.FleetAssetModel.EngineBrand != null ? flpast.FleetAssetModel.EngineBrand : emptyValue;
            fleetEngine.Aspiration = flpast.FleetAssetModel.Aspiration != null ? flpast.FleetAssetModel.Aspiration : emptyValue;
            fleetEngine.Bore = flpast.FleetAssetModel.Bore;
            fleetEngine.Cam = flpast.FleetAssetModel.Cam != null ? flpast.FleetAssetModel.Cam : emptyValue;
            fleetEngine.Compression = flpast.FleetAssetModel.Compression;
            fleetEngine.Cylinders = flpast.FleetAssetModel.Cylinders;
            fleetEngine.Displacement = flpast.FleetAssetModel.Displacement;
            fleetEngine.FuelInduction = flpast.FleetAssetModel.FuelInduction != null ? flpast.FleetAssetModel.FuelInduction : emptyValue;
            fleetEngine.FuelQuality = flpast.FleetAssetModel.FuelQuality;
            fleetEngine.MaxHP = flpast.FleetAssetModel.MaxHP;
            fleetEngine.MaxTorque = flpast.FleetAssetModel.MaxTorque;
            fleetEngine.RedlineRPM = flpast.FleetAssetModel.RedlineRPM;
            fleetEngine.Stroke = flpast.FleetAssetModel.Stroke;
            fleetEngine.Valves = flpast.FleetAssetModel.Valves;
            fleetEngine.TransmissionBrand = flpast.FleetAssetModel.TransmissionBrand != null ? flpast.FleetAssetModel.TransmissionBrand : emptyValue;
            fleetEngine.TransmissionType = flpast.FleetAssetModel.TransmissionType != null ? flpast.FleetAssetModel.TransmissionType : emptyValue;
            fleetEngine.Gears = flpast.FleetAssetModel.Gears;

            if (equipment.FleetEngineId > 0)
            {
                fleetEngine.FleetEngineId= equipment.FleetEngineId;
                fleetEngine.UpdateIndex = equipment.FleetEngineUpdateIndex;
                tasks[1] = Task.Factory.StartNew(() => fleetEngine.Update(this.userData.DatabaseKey));
            }
            else
            {
                if (flpast.FleetAssetModel.isfleetEngineData == true)
                {
                    tasks[1] = Task.Factory.StartNew(() => fleetEngine.Create(this.userData.DatabaseKey));
                }
            }

            #endregion

            #region FleetFluids
            FleetFluids fleetFluids = new FleetFluids()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(flpast.FleetAssetModel.EquipmentID)
            };
            fleetFluids.FuelQuality = flpast.FleetAssetModel.FleetFuelQuality != null ? flpast.FleetAssetModel.FleetFuelQuality : emptyValue;
            fleetFluids.FuelType = flpast.FleetAssetModel.FuelType != null ? flpast.FleetAssetModel.FuelType : emptyValue;
            fleetFluids.FuelTankCapacity1 = flpast.FleetAssetModel.FuelTankCapacity1;
            fleetFluids.FuelTankCapacity2 = flpast.FleetAssetModel.FuelTankCapacity2;
            fleetFluids.EPACity = flpast.FleetAssetModel.EPACity;
            fleetFluids.EPAHighway = flpast.FleetAssetModel.EPAHighway;
            fleetFluids.EPACombined = flpast.FleetAssetModel.EPACombined;
            if (equipment.FleetFluidsId> 0)
            {
                fleetFluids.FleetFluidsId = equipment.FleetFluidsId;
                fleetFluids.UpdateIndex = equipment.FleetFluidUpdateIndex;
                tasks[2] = Task.Factory.StartNew(() => fleetFluids.Update(this.userData.DatabaseKey));
            }
            else
            {
                if (flpast.FleetAssetModel.isfleetFluidsData == true)
                {
                    tasks[2] = Task.Factory.StartNew(() => fleetFluids.Create(this.userData.DatabaseKey));
                }
            }
            #endregion

            #region  FleetFleetWheel
            FleetWheel fleetWheel = new FleetWheel()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(flpast.FleetAssetModel.EquipmentID)
            };
            fleetWheel.BrakeSystem = flpast.FleetAssetModel.BrakeSystem != null ? flpast.FleetAssetModel.BrakeSystem : emptyValue;
            fleetWheel.RearTrackWidth = flpast.FleetAssetModel.RearTrackWidth;
            fleetWheel.Wheelbase = flpast.FleetAssetModel.Wheelbase;
            fleetWheel.FrontWheelDiameter = flpast.FleetAssetModel.FrontWheelDiameter;
            fleetWheel.RearWheelDiameter = flpast.FleetAssetModel.RearWheelDiameter;
            fleetWheel.FrontTirePSI = flpast.FleetAssetModel.FrontTirePSI;
            fleetWheel.RearTirePSI = flpast.FleetAssetModel.RearTirePSI;
            if (equipment.FleetWheelId> 0)
            {
                fleetWheel.FleetWheelId= equipment.FleetWheelId;
                fleetWheel.UpdateIndex = equipment.FleetWheelUpdateIndex;
                tasks[3] = Task.Factory.StartNew(() => fleetWheel.Update(this.userData.DatabaseKey));
            }
            else
            {
                if (flpast.FleetAssetModel.isfleetWheelData == true)
                {
                    tasks[3] = Task.Factory.StartNew(() => fleetWheel.Create(this.userData.DatabaseKey));
                }
            }
            #endregion

            Task.WaitAll();
            #endregion
            return equipment;
        }
        #endregion

        #region Service Order Retrieve By EquipmentId
        public List<FleetAssetServiceOrderModel> GetFleetAssetServiceOrder(long EquipmentId)
        {
            List<FleetAssetServiceOrderModel> ServiceOrderModelList = new List<FleetAssetServiceOrderModel>();
            ServiceOrder so = new ServiceOrder()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId= this.userData.DatabaseKey.User.DefaultSiteId,
                EquipmentId = EquipmentId
            };
            var serv = so.RetrieveByEquipmentId(this.userData.DatabaseKey);
          
            if (serv != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                FleetAssetServiceOrderModel objSoModel;
                foreach (var v in serv)
                {
                    objSoModel = new FleetAssetServiceOrderModel();
                    objSoModel.ServiceOrderId = v.ServiceOrderId;
                    objSoModel.ClientLookupId = v.ClientLookupId;
                    objSoModel.EquipmentClientLookupId = v.EquipmentClientLookupId;
                    objSoModel.AssetName = v.AssetName;
                    objSoModel.Status = v.Status;
                    objSoModel.Type = v.Type;
                    objSoModel.CreateDate = v.CreateDate;
                    if (v.CompleteDate != null && v.CompleteDate == default(DateTime))
                    {
                        objSoModel.CompleteDate = null;
                    }
                    else
                    {
                        objSoModel.CompleteDate = v.CompleteDate;
                    }
                    //objSoModel.CompleteDate = v.CompleteDate;
                    objSoModel.ChildCount = v.ChildCount;
                    ServiceOrderModelList.Add(objSoModel);
                }
            }
            
            return ServiceOrderModelList;
        }
        #endregion

        #region Scheduled Service Retrieve By EquipmentId
        public List<FleetAssetScheduledServiceModel> GetFleetAssetScheduledService(long EquipmentId)
        {
            List<FleetAssetScheduledServiceModel> ServiceOrderModelList = new List<FleetAssetScheduledServiceModel>();

            DataContracts.ScheduledService so = new DataContracts.ScheduledService()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                EquipmentId = EquipmentId
            };
            var ScheduleServ = so.ScheduledServiceRetrieveByEquipmentIdV2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (ScheduleServ != null)
            {
                // var eqpList = serv.Select(x => new { x.EquipmentId, x.Part_ClientLookupId, x.Part_Description, x.QuantityNeeded, x.QuantityUsed, x.Comment, x.Equipment_Parts_XrefId, x.UpdateIndex, x.PartId }).ToList();

                FleetAssetScheduledServiceModel objSSModel;
                foreach (var v in ScheduleServ)
                {
                    objSSModel = new FleetAssetScheduledServiceModel();
                    objSSModel.ScheduledServiceId = v.ScheduledServiceId;
                    objSSModel.ServiceTaskId = v.ServiceTaskId;
                    objSSModel.ServiceTaskDescription = v.ServiceTasksDescription;
                    objSSModel.TimeInterval = v.TimeInterval;
                    objSSModel.TimeIntervalType = v.TimeIntervalType;
                    objSSModel.Meter1Interval = v.Meter1Interval;
                    objSSModel.Meter1Units = v.Meter1Units;
                    objSSModel.Meter2Interval = v.Meter2Interval;
                    objSSModel.Meter2Units = v.Meter2Units;
                    //objSSModel.NextDueDate = v.NextDueDate;
                    if (v.NextDueDate != null && v.NextDueDate == default(DateTime))
                    {
                        objSSModel.NextDueDate = null;
                    }
                    else
                    {
                        objSSModel.NextDueDate = v.NextDueDate;
                    }
                    objSSModel.TimeThresoldType = v.TimeThresoldType;
                    //objSSModel.LastPerformedDate = v.LastPerformedDate;
                    if (v.LastPerformedDate != null && v.LastPerformedDate == default(DateTime))
                    {
                        objSSModel.LastPerformedDate = null;
                    }
                    else
                    {
                        objSSModel.LastPerformedDate = v.LastPerformedDate;
                    }
                    objSSModel.LastPerformedMeter1 = v.LastPerformedMeter1;
                    objSSModel.LastPerformedMeter2 = v.LastPerformedMeter2;
                    objSSModel.NextDueMeter1 = v.NextDueMeter1;
                    objSSModel.NextDueMeter2 = v.NextDueMeter2;
                    objSSModel.Meter1Type = v.Meter1Type;
                    objSSModel.Meter2Type = v.Meter2Type;
                    //objSoModel.CompleteDate = v.CompleteDate;

                    ServiceOrderModelList.Add(objSSModel);
                }
            }

            return ServiceOrderModelList;
        }
        #endregion

        #region Fleet Fuel Retrieve By EquipmentId
        public List<FleetAssetFuelTrackingModel> GetFleetAssetFuelTracking(long EquipmentId)
        {
            List<FleetAssetFuelTrackingModel> FuelTrackingModelList = new List<FleetAssetFuelTrackingModel>();

            Equipment eq = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                EquipmentId = EquipmentId
            };
            var fleetfuel = eq.FleetFuelRetrieveByEquipmentIdV2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (fleetfuel != null)
            {
                FleetAssetFuelTrackingModel objFFModel;
                foreach (var v in fleetfuel)
                {
                    objFFModel = new FleetAssetFuelTrackingModel();
                    objFFModel.FuelTrackingId = v.FuelTrackingId;
                    objFFModel.FuelAmount = v.FuelAmount;
                    objFFModel.UnitCost = v.UnitCost;
                    objFFModel.ReadingDate = v.ReadingDate;
                    objFFModel.FuelUnits = v.FuelUnits;
                    objFFModel.Meter1Units = v.Meter1Units;
                    objFFModel.Meter2Units = v.Meter2Units;
                    objFFModel.TotalCost = v.TotalCost;


                    FuelTrackingModelList.Add(objFFModel);
                }
            }

            return FuelTrackingModelList;
        }
        #endregion

        #region Fleet Meter Retrieve By EquipmentId
        public List<FleetAssetMeterReadingModel> GetFleetAssetMeterReading(long EquipmentId)
        {
            List<FleetAssetMeterReadingModel> MeterReadingModelList = new List<FleetAssetMeterReadingModel>();

            Equipment eq = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                EquipmentId = EquipmentId
            };
            var fleetMeter = eq.FleetMeterReadingRetrieveByEquipmentIdV2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (fleetMeter != null)
            {
                FleetAssetMeterReadingModel objMRModel;
                foreach (var v in fleetMeter)
                {
                    objMRModel = new FleetAssetMeterReadingModel();
                    objMRModel.ReadingDate = v.FMRReadingDate;
                    objMRModel.NoOfDays = v.NoofDays;
                    objMRModel.ReadingLine1 = v.MeterReadingL1;
                    objMRModel.ReadingLine2 = v.MeterReadingL2;
                    objMRModel.SourceType = v.SourceType;
                    objMRModel.Meter2Indicator = v.Meter2Indicator;
                    objMRModel.VIN = v.VIN;
                    objMRModel.Make = v.Make;
                    objMRModel.Model = v.Model;

                    MeterReadingModelList.Add(objMRModel);
                }
            }

            return MeterReadingModelList;
        }
        #endregion

        #region Fleet Issue Retrieve By EquipmentId
        public List<FleetAssetFleetIssueModel> GetFleetAssetFleetIssue(long EquipmentId)
        {
            List<FleetAssetFleetIssueModel> FleetIssueModelList = new List<FleetAssetFleetIssueModel>();

            FleetIssues FI = new FleetIssues()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId,
                EquipmentId = EquipmentId
            };
            var fleetIssue = FI.FleetIssueRetrieveByEquipmentIdV2(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (fleetIssue != null)
            {
                FleetAssetFleetIssueModel objFIModel;
                foreach (var v in fleetIssue)
                {
                    objFIModel = new FleetAssetFleetIssueModel();
                    objFIModel.FleetIssuesId = v.FleetIssuesId;
                    objFIModel.EquipmentId = v.EquipmentId;
                    objFIModel.EquipmentClientLookupId = v.EquipmentClientLookupId;
                    //objFIModel.RecordDate = v.RecordDate;
                    if (v.RecordDate != null && v.RecordDate == default(DateTime))
                    {
                        objFIModel.RecordDate = null;
                    }
                    else
                    {
                        objFIModel.RecordDate = v.RecordDate;
                    }
                    objFIModel.Defects = v.Defects;
                    objFIModel.Description = v.Description;
                    objFIModel.DriverName = v.DriverName;
                    objFIModel.Status = v.Status;
                    //objFIModel.CompleteDate = v.CompleteDate;
                    if (v.CompleteDate != null && v.CompleteDate == default(DateTime))
                    {
                        objFIModel.CompleteDate = null;
                    }
                    else
                    {
                        objFIModel.CompleteDate = v.CompleteDate;
                    }
                    objFIModel.ServiceOrderClientLookupId = v.ServiceOrderClientLookupId;


                    FleetIssueModelList.Add(objFIModel);
                }
            }

            return FleetIssueModelList;
        }
        #endregion

        #region Asset Availability
        public Equipment AssetAvailability(FleetAssetVM flpast)
        {
            string emptyValue = string.Empty;
            FleetAssetModel objFleetAsset = new FleetAssetModel();
            List<string> ErrorList = new List<string>();

            #region Equipment
            Equipment equipment = new Equipment()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                EquipmentId = Convert.ToInt64(flpast._AssetAvailabilityModel.EquipmentId)
            };
            equipment.RetrieveByPKForeignKeys_V2(userData.DatabaseKey);
            equipment.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            if (flpast._AssetAvailabilityModel.RemoveFromService == false)
            {
                equipment.RemoveFromService = true;
                equipment.RemoveFromServiceDate = DateTime.UtcNow;
            }
            equipment.ExpectedReturnToService = flpast._AssetAvailabilityModel.ExpectedReturnToService;
            equipment.RemoveFromServiceReason = flpast._AssetAvailabilityModel.RemoveFromServiceReason;
            
            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null)
            {
                if (flpast._AssetAvailabilityModel.RemoveFromService == false)
                {
                    CreateEventLog(equipment.EquipmentId,"Remove", equipment.ExpectedReturnToService, equipment.RemoveFromServiceReason);
                }
                else
                {
                    CreateEventLog(equipment.EquipmentId, "Update", equipment.ExpectedReturnToService, equipment.RemoveFromServiceReason);
                }
                    
            }
            #endregion
         
            return equipment;
        }
        public Equipment ReturnToInservice(long EquipmentId)
        {
            string emptyValue = string.Empty;
            FleetAssetModel objFleetAsset = new FleetAssetModel();
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

            equipment.Update(userData.DatabaseKey);
            if (equipment.ErrorMessages == null)
            {
                CreateEventLog(equipment.EquipmentId,"Return", equipment.ExpectedReturnToService, equipment.RemoveFromServiceReason);
            }
            #endregion

            return equipment;
        }

        private void CreateEventLog(Int64 EQId,string Event, DateTime? ExpectedReturnToService,string RemoveFromServiceReason)
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
            log.SourceId = 0;
            log.Create(userData.DatabaseKey);
        }
        #endregion
    }
}