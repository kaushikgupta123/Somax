using Common.Structures;
using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class Equipment : DataContractBase, IStoredProcedureValidation
    {

        #region Properties
        public bool CreateMode { get; set; }
        public string ParentIdClientLookupId { get; set; }
        public string ElectricalParentClientLookupId { get; set; }
        public string MaintVendorIdClientLookupId { get; set; }
        public string PurchVendorIdClientLookupId { get; set; }
        public string PartIdClientLookupId { get; set; }
        public string LocationIdClientLookupId { get; set; }
        public string LaborAccountClientLookupId { get; set; }
        public string MaterialAccountClientLookupId { get; set; }
        public string DepartmentName { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public string Process { get; set; }
        public string System { get; set; }
        public System.Data.DataTable EquipIds { get; set; }

        public string ProcessSystemDesc { get; set; }
        public string PlantLocationDescription { get; set; }

        public string ValidateFor = string.Empty;
        public string ImageUrl { get; set; }
        public string Lookup_ListName { get; set; }
        public string Area_Desc { get; set; }
        public string Dept_Desc { get; set; }
        public string Line_Desc { get; set; }
        public string System_Desc { get; set; }
        public string EquipmentIdList { get; set; }
        public Int64 ObjectId { get; set; }
        public string ObjectName { get; set; }

        public string DeptClientLookUpId { get; set; }

        public string LineClientLookUpId { get; set; }
        public string SysClientLookUpId { get; set; }
        public string Flag { get; set; }

        //V2-292
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string Department { get; set; }
        public string Line { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string Account { get; set; }
        public string SearchText { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public List<Equipment> listOfEquipment { get; set; }
        public Int32 TotalCount { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }

        public string AssetGroup1Desc { get; set; }
        public string AssetGroup2Desc { get; set; }
        public string AssetGroup3Desc { get; set; }

        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }

        //V2-385     
        public Int64 FleetDimensionsId { get; set; }
        public Int64 FleetEngineId { get; set; }
        public Int64 FleetWheelId { get; set; }
        public Int64 FleetFluidsId { get; set; }
        public string Color { get; set; }
        public string BodyType { get; set; }
        public Decimal Width { get; set; }
        public Decimal Height { get; set; }
        public Decimal Length { get; set; }
        public Decimal PassengerVolume { get; set; }
        public Decimal CargoVolume { get; set; }
        public Decimal GroundClearance { get; set; }
        public Decimal BedLength { get; set; }
        public Decimal CurbWeight { get; set; }
        public Decimal VehicleWeight { get; set; }
        public Decimal TowingCapacity { get; set; }
        public Decimal MaxPayload { get; set; }
        public string EngineBrand { get; set; }
        public string Aspiration { get; set; }
        public Decimal Bore { get; set; }
        public string Cam { get; set; }
        public Decimal Compression { get; set; }
        public Decimal Cylinders { get; set; }
        public Decimal Displacement { get; set; }
        public string FuelInduction { get; set; }
        public Decimal FuelQuality { get; set; }
        public Decimal MaxHP { get; set; }
        public Decimal MaxTorque { get; set; }
        public Decimal RedlineRPM { get; set; }
        public Decimal Stroke { get; set; }
        public Decimal Valves { get; set; }
        public string TransmissionBrand { get; set; }
        public string TransmissionType { get; set; }
        public Decimal Gears { get; set; }
        public string BrakeSystem { get; set; }
        public Decimal RearTrackWidth { get; set; }
        public Decimal Wheelbase { get; set; }
        public Decimal FrontWheelDiameter { get; set; }
        public Decimal RearWheelDiameter { get; set; }
        public Decimal FrontTirePSI { get; set; }
        public Decimal RearTirePSI { get; set; }
        public string FleetFluidsFuelQuality { get; set; }
        public string FuelType { get; set; }
        public Decimal FuelTankCapacity1 { get; set; }
        public Decimal FuelTankCapacity2 { get; set; }
        public Decimal EPACity { get; set; }
        public Decimal EPAHighway { get; set; }
        public Decimal EPACombined { get; set; }

        public FleetDimensions fleetDimensions { get; set; } = new FleetDimensions();
        public FleetEngine fleetEngine { get; set; } = new FleetEngine();
        public FleetFluids fleetFluids { get; set; } = new FleetFluids();
        public FleetWheel fleetWheel { get; set; } = new FleetWheel();

        public bool isfleetDimensionData { get; set; }
        public bool isfleetEngineData { get; set; }
        public bool isfleetFluidsData { get; set; }
        public bool isfleetWheelData { get; set; }
        public Int32 FleetDimensionUpdateIndex { get; set; }
        public Int32 FleetEngineUpdateIndex { get; set; }
        public Int32 FleetWheelUpdateIndex { get; set; }
        public Int32 FleetFluidUpdateIndex { get; set; }
        //V2-385

        //V2-392
        public string EquipmentImage { get; set; }
        public DateTime FMRReadingDate { get; set; }
        public Int64 NoofDays { get; set; }
        public string MeterReadingL1 { get; set; }
        public string MeterReadingL2 { get; set; }
        public string SourceType { get; set; }
        public string Action { get; set; }
        public Boolean Meter2Indicator { get; set; }
        public string ReadingDateStart { get; set; }
        public string ReadingDateEnd { get; set; }
        public Int64 SourceId { get; set; }
        public string meter1currentreadingdate { get; set; }
        public string meter2currentreadingdate { get; set; }
        public Boolean FirstMeterVoid { get; set; }
        public Boolean SecondMeterVoid { get; set; }
        //V2-391

        public string ReadingStartDate { get; set; }
        public string ReadingEndDate { get; set; }
        public string EquipImage { get; set; }
        public DateTime ReadingDate { get; set; }
        public decimal FuelAmount { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public Int64 FuelTrackingId { get; set; }
        public bool Void { get; set; }
        public string FTModifyBy { get; set; }
        public DateTime FTModifyDate { get; set; }
        public string FMModifyBy { get; set; }
        public DateTime FMModifyDate { get; set; }
        public string AssetModifyBy { get; set; }
        public DateTime AssetModifyDate { get; set; }
        public decimal Reading { get; set; }
        public string FuelUnit { get; set; }
        public long FleetMeterReadingId { get; set; }
        public string TableName { get; set; }
        public bool IsAssigned { get; set; }//V2-637
        public RepairableSpareLog RepairableSpareLog { get; set; } //V2-637
        public string AssignedClientlookupid { get; set; } //V2-637
        public string AssignedAssetName { get; set; } //V2-637
        public int ChildCount { get; set; }
        #region V2-1211
        public string MaintVendorName { get; set; }
        public string PurchVendorName { get; set; }
        #endregion
        #endregion
        public string AssetAvailability { get; set; }

        #region Constructor

        //public override Equipment()
        //{
        //    fleetDimensions = new FleetDimensions();
        //    fleetEngine = new FleetEngine();
        //    fleetFluids = new FleetFluids();
        //    fleetWheel = new FleetWheel();

        //}
        #endregion

        #region Transactions

        public void CreateByPKForeignKeys(DatabaseKey dbKey) 
        {
            Validate<Equipment>(dbKey);

            if (IsValid)
            {
                Equipment_CreateByForeignKeys trans = new Equipment_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();

                trans.Equipment.ParentIdClientLookupId = this.ParentIdClientLookupId;
                trans.Equipment.ElectricalParentClientLookupId = this.ElectricalParentClientLookupId;
                trans.Equipment.MaintVendorIdClientLookupId = this.MaintVendorIdClientLookupId;
                trans.Equipment.PurchVendorIdClientLookupId = this.PurchVendorIdClientLookupId;
                trans.Equipment.PartIdClientLookupId = this.PartIdClientLookupId;
                trans.Equipment.LocationIdClientLookupId = this.LocationIdClientLookupId;
                trans.Equipment.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
                trans.Equipment.MaterialAccountClientLookupId = this.MaterialAccountClientLookupId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Equipment);
            }
        }

        public void CreateByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            Validate<Equipment>(dbKey);

            if (IsValid)
            {
                Equipment_CreateByForeignKeys_V2 trans = new Equipment_CreateByForeignKeys_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();

                trans.Equipment.ParentIdClientLookupId = this.ParentIdClientLookupId;
                trans.Equipment.ElectricalParentClientLookupId = this.ElectricalParentClientLookupId;
                trans.Equipment.MaintVendorIdClientLookupId = this.MaintVendorIdClientLookupId;
                trans.Equipment.PurchVendorIdClientLookupId = this.PurchVendorIdClientLookupId;
                trans.Equipment.PartIdClientLookupId = this.PartIdClientLookupId;
                trans.Equipment.LocationIdClientLookupId = this.LocationIdClientLookupId;
                trans.Equipment.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
                trans.Equipment.MaterialAccountClientLookupId = this.MaterialAccountClientLookupId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Equipment);
            }
        }
  
        public void RetrieveByClientLookupId(DatabaseKey dbKey)
        {
            Equipment_RetrieveByClientLookupId trans = new Equipment_RetrieveByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Equipment);

        }


        public void RetrieveByPKForeignKeys(DatabaseKey dbKey)
        {
            Equipment_RetrieveByForeignKeys trans = new Equipment_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Equipment);
            this.ParentIdClientLookupId = trans.Equipment.ParentIdClientLookupId;
            this.ElectricalParentClientLookupId = trans.Equipment.ElectricalParentClientLookupId;
            this.MaintVendorIdClientLookupId = trans.Equipment.MaintVendorIdClientLookupId;
            this.PurchVendorIdClientLookupId = trans.Equipment.PurchVendorIdClientLookupId;
            this.PartIdClientLookupId = trans.Equipment.PartIdClientLookupId;
            this.LocationIdClientLookupId = trans.Equipment.LocationIdClientLookupId;
            this.LaborAccountClientLookupId = trans.Equipment.LaborAccountClientLookupId;
            this.MaterialAccountClientLookupId = trans.Equipment.MaterialAccountClientLookupId;
            this.ProcessSystemDesc = trans.Equipment.ProcessSystemDesc;
            this.PlantLocationDescription = trans.Equipment.PlantLocationDescription;
            this.Dept_Desc = trans.Equipment.Dept_Desc;
            this.Line_Desc = trans.Equipment.Line_Desc;
            this.System_Desc = trans.Equipment.System_Desc;


        }

        public void RetrieveByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            Equipment_RetrieveByForeignKeys_V2 trans = new Equipment_RetrieveByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Equipment);
            this.AssetGroup1ClientLookupId = trans.Equipment.AssetGroup1ClientLookupId;
            this.AssetGroup2ClientLookupId = trans.Equipment.AssetGroup2ClientLookupId;
            this.AssetGroup3ClientLookupId = trans.Equipment.AssetGroup3ClientLookupId;
            this.AssetGroup1Desc = trans.Equipment.AssetGroup1Desc;
            this.AssetGroup2Desc = trans.Equipment.AssetGroup2Desc;
            this.AssetGroup3Desc = trans.Equipment.AssetGroup3Desc;
            this.FleetDimensionsId = trans.Equipment.FleetDimensionsId;
            this.Color = trans.Equipment.Color;
            this.BodyType = trans.Equipment.BodyType;
            this.Width = trans.Equipment.Width;
            this.Height = trans.Equipment.Height;
            this.Length = trans.Equipment.Length;
            this.PassengerVolume = trans.Equipment.PassengerVolume;
            this.CargoVolume = trans.Equipment.CargoVolume;
            this.GroundClearance = trans.Equipment.GroundClearance;
            this.BedLength = trans.Equipment.BedLength;
            this.CurbWeight = trans.Equipment.CurbWeight;
            this.VehicleWeight = trans.Equipment.VehicleWeight;
            this.TowingCapacity = trans.Equipment.TowingCapacity;
            this.MaxPayload = trans.Equipment.MaxPayload;
            this.FleetDimensionUpdateIndex = trans.Equipment.FleetDimensionUpdateIndex;
            this.FleetEngineId = trans.Equipment.FleetEngineId;
            this.EngineBrand = trans.Equipment.EngineBrand;
            this.Aspiration = trans.Equipment.Aspiration;
            this.Bore = trans.Equipment.Bore;
            this.Cam = trans.Equipment.Cam;
            this.Compression = trans.Equipment.Compression;
            this.Cylinders = trans.Equipment.Cylinders;
            this.Displacement = trans.Equipment.Displacement;
            this.FuelInduction = trans.Equipment.FuelInduction;
            this.FuelQuality = trans.Equipment.FuelQuality;
            this.MaxHP = trans.Equipment.MaxHP;
            this.MaxTorque = trans.Equipment.MaxTorque;
            this.RedlineRPM = trans.Equipment.RedlineRPM;
            this.Stroke = trans.Equipment.Stroke;
            this.Valves = trans.Equipment.Valves;
            this.TransmissionBrand = trans.Equipment.TransmissionBrand;
            this.TransmissionType = trans.Equipment.TransmissionType;
            this.Gears = trans.Equipment.Gears;
            this.FleetEngineUpdateIndex = trans.Equipment.FleetEngineUpdateIndex;
            this.FleetWheelId = trans.Equipment.FleetWheelId;
            this.BrakeSystem = trans.Equipment.BrakeSystem;
            this.RearTrackWidth = trans.Equipment.RearTrackWidth;
            this.Wheelbase = trans.Equipment.Wheelbase;
            this.FrontWheelDiameter = trans.Equipment.FrontWheelDiameter;
            this.RearWheelDiameter = trans.Equipment.RearWheelDiameter;
            this.FrontTirePSI = trans.Equipment.FrontTirePSI;
            this.RearTirePSI = trans.Equipment.RearTirePSI;
            this.RearTirePSI = trans.Equipment.RearTirePSI;
            this.FleetWheelUpdateIndex = trans.Equipment.FleetWheelUpdateIndex;
            this.FleetFluidsId = trans.Equipment.FleetFluidsId;
            this.FleetFluidsFuelQuality = trans.Equipment.FleetFluidsFuelQuality;
            this.FuelType = trans.Equipment.FuelType;
            this.FuelTankCapacity1 = trans.Equipment.FuelTankCapacity1;
            this.FuelTankCapacity2 = trans.Equipment.FuelTankCapacity2;
            this.EPACity = trans.Equipment.EPACity;
            this.EPAHighway = trans.Equipment.EPAHighway;
            this.EPACombined = trans.Equipment.EPACombined;
            this.FleetFluidUpdateIndex = trans.Equipment.FleetFluidUpdateIndex;
            this.ParentIdClientLookupId = trans.Equipment.ParentIdClientLookupId;
            this.ElectricalParentClientLookupId = trans.Equipment.ElectricalParentClientLookupId;
            this.MaintVendorIdClientLookupId = trans.Equipment.MaintVendorIdClientLookupId;
            this.PurchVendorIdClientLookupId = trans.Equipment.PurchVendorIdClientLookupId;
            this.PartIdClientLookupId = trans.Equipment.PartIdClientLookupId;
            this.LocationIdClientLookupId = trans.Equipment.LocationIdClientLookupId;
            this.LaborAccountClientLookupId = trans.Equipment.LaborAccountClientLookupId;
            this.MaterialAccountClientLookupId = trans.Equipment.MaterialAccountClientLookupId;
            this.ProcessSystemDesc = trans.Equipment.ProcessSystemDesc;
            this.PlantLocationDescription = trans.Equipment.PlantLocationDescription;
            this.Dept_Desc = trans.Equipment.Dept_Desc;
            this.Line_Desc = trans.Equipment.Line_Desc;
            this.System_Desc = trans.Equipment.System_Desc;
            this.FuelUnits = trans.Equipment.FuelUnits;
            this.Meter1Type = trans.Equipment.Meter1Type;
            this.Meter1Units = trans.Equipment.Meter1Units;
            this.Meter2Type = trans.Equipment.Meter2Type;
            this.Meter2Units = trans.Equipment.Meter2Units;
            this.AssignedClientlookupid = trans.Equipment.AssignedClientlookupid;
            this.AssignedAssetName = trans.Equipment.AssignedAssetName;
            this.ElectricalParentClientLookupId = trans.Equipment.ElectricalParentClientLookupId;
            this.MaintVendorName = trans.Equipment.MaintVendorName;
            this.PurchVendorName = trans.Equipment.PurchVendorName;
        }

        public List<Equipment> RetrieveAllBySiteId_V2(DatabaseKey dbKey)
        {
            Equipment_RetrieveAllBySiteId_V2 trans = new Equipment_RetrieveAllBySiteId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseAllBySiteId(trans.equipList));
        }


        public void RetrieveByEquipmentIdandFuelTrackingId(DatabaseKey dbKey)
        {
            Equipment_RetrieveByEquipmentIdandFuelTrackingId trans = new Equipment_RetrieveByEquipmentIdandFuelTrackingId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.Equipment.FuelTrackingId = this.FuelTrackingId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Equipment);
            this.Void = trans.Equipment.Void;
            this.FuelAmount = trans.Equipment.FuelAmount;
            this.UnitCost = trans.Equipment.UnitCost;
            this.FuelType = trans.Equipment.FuelType;
            this.EquipmentId= trans.Equipment.EquipmentId;
            this.FuelUnit= trans.Equipment.FuelUnit;
            this.FleetMeterReadingId= trans.Equipment.FleetMeterReadingId;
            this.Reading = trans.Equipment.Reading;
            this.ReadingDate= trans.Equipment.ReadingDate;
            this.Meter1Units= trans.Equipment.Meter1Units;


            trans.Equipment.FTModifyBy = this.FTModifyBy;
            trans.Equipment.FTModifyDate = this.FTModifyDate;
            trans.Equipment.Reading = this.Reading;
            trans.Equipment.FMRReadingDate = this.FMRReadingDate;
            trans.Equipment.Void = this.Void;
            trans.Equipment.FMModifyBy = this.FMModifyBy;
            trans.Equipment.FMModifyDate = this.FMModifyDate;
            trans.Equipment.AssetModifyBy = this.AssetModifyBy;
            trans.Equipment.AssetModifyDate = this.AssetModifyDate;



        }
        public void UpdateForPlantLocation(DatabaseKey dbKey)
        {
            Equipment_UpdateForPlantLocation trans = new Equipment_UpdateForPlantLocation()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();

            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment);
        }


        public void UpdateForVoidbyFleetMeterandEquipmentId(DatabaseKey dbKey)
        {
            Equipment_UpdateForVoidbyFleetMeterandEquipmentId trans = new Equipment_UpdateForVoidbyFleetMeterandEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.Equipment.FleetMeterReadingId = this.FleetMeterReadingId;
            trans.Equipment.Void = this.Void;
            trans.Equipment.Meter2Indicator = this.Meter2Indicator;

            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment);
        }



        public void UpdateEquipmentForVoidFromFuelTracking(DatabaseKey dbKey)
        {
            Equipment_UpdateForVoidFromFuelTracking trans = new Equipment_UpdateForVoidFromFuelTracking()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment);
        }

        public void UpdateForFleetMeter(DatabaseKey dbKey)
        {
            Equipment_UpdateFoFeetMeter trans = new Equipment_UpdateFoFeetMeter()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.Equipment.FirstMeterVoid = this.FirstMeterVoid;
            trans.Equipment.SecondMeterVoid = this.SecondMeterVoid;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment);
        }

        public void EquipmentUpdateFORFuelTracking(DatabaseKey dbKey)
        {
            Equipment_UpdateFORFuelTracking trans = new Equipment_UpdateFORFuelTracking()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.Equipment.meter1currentreadingdate = this.meter1currentreadingdate;
            trans.Equipment.meter2currentreadingdate = this.meter2currentreadingdate;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment);
        }
        public List<Equipment> RetrieveAllBasedOnCriteria(DatabaseKey dbKey)
        {
            RetrieveAllBasedOnCriteria trans = new RetrieveAllBasedOnCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SiteId = this.SiteId;
            trans.EquipIds = EquipIds;
            trans.Type = Type;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseObjectList(trans.EquipmentList));

        }

        public void UpdateByPKForeignKeys(DatabaseKey dbKey)
        {
            ValidateFor = "ProcessSystem";
            Validate<Equipment>(dbKey);
            if (IsValid)
            {

                Equipment_UpdateByForeignKeys trans = new Equipment_UpdateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Equipment = this.ToDatabaseObject();

                trans.Equipment.ParentIdClientLookupId = this.ParentIdClientLookupId;
                trans.Equipment.ElectricalParentClientLookupId = this.ElectricalParentClientLookupId;
                trans.Equipment.MaintVendorIdClientLookupId = this.MaintVendorIdClientLookupId;
                trans.Equipment.PurchVendorIdClientLookupId = this.PurchVendorIdClientLookupId;
                trans.Equipment.PartIdClientLookupId = this.PartIdClientLookupId;
                trans.Equipment.LocationIdClientLookupId = this.LocationIdClientLookupId;
                trans.Equipment.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
                trans.Equipment.MaterialAccountClientLookupId = this.MaterialAccountClientLookupId;

                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Equipment);
            }
        }

        public void UpdateByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            ValidateFor = "ProcessSystem";
            Validate<Equipment>(dbKey);
            if (IsValid)
            {

                Equipment_UpdateByForeignKeys_V2 trans = new Equipment_UpdateByForeignKeys_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Equipment = this.ToDatabaseObject();

                trans.Equipment.ParentIdClientLookupId = this.ParentIdClientLookupId;
                trans.Equipment.ElectricalParentClientLookupId = this.ElectricalParentClientLookupId;
                trans.Equipment.MaintVendorIdClientLookupId = this.MaintVendorIdClientLookupId;
                trans.Equipment.PurchVendorIdClientLookupId = this.PurchVendorIdClientLookupId;
                trans.Equipment.PartIdClientLookupId = this.PartIdClientLookupId;
                trans.Equipment.LocationIdClientLookupId = this.LocationIdClientLookupId;
                trans.Equipment.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
                trans.Equipment.MaterialAccountClientLookupId = this.MaterialAccountClientLookupId;

                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Equipment);
            }
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            //--------Check For Equipment is Parent of another-------------------------------------------------
            if (ValidateFor == "CheckInactive")
            {

                Equipment_ValidateEquipmentIsParentOfAnother trans = new Equipment_ValidateEquipmentIsParentOfAnother()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }
            else if (ValidateFor == "CheckDuplicate")
            {
                Equipment_ValidateClientLookupId trans = new Equipment_ValidateClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }
            else if (ValidateFor == "CheckForeignFieldToDelete")
            {
                Equipment_ValidateForeignField trans = new Equipment_ValidateForeignField()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }

            else if (ValidateFor == "CheckIfDepartmentUsedInEquipment")
            {
                Department_ValidateIfUsedInEquipment trans = new Department_ValidateIfUsedInEquipment()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }

            else if (ValidateFor == "CheckIfLineUsedInEquipment")
            {
                Line_ValidateIfUsedInEquipment trans = new Line_ValidateIfUsedInEquipment()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }

            else if (ValidateFor == "CheckIfSystemInfoUsedInEquipment")
            {
                SystemInfo_ValidateIfUsedInEquipment trans = new SystemInfo_ValidateIfUsedInEquipment()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }

            else if (ValidateFor == "CheckIfInactivateorActivate")
            {

                Equipment_ValidateByInactivateorActivate trans = new Equipment_ValidateByInactivateorActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.Flag = this.Flag;
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }

            else if (ValidateFor == "CheckIfAssetGroup1UsedInEquipment")
            {
                AssetGroup1_ValidateIfUsedInEquipment trans = new AssetGroup1_ValidateIfUsedInEquipment()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }


            else if (ValidateFor == "CheckIfAssetGroup2UsedInEquipment")
            {
                AssetGroup2_ValidateIfUsedInEquipment trans = new AssetGroup2_ValidateIfUsedInEquipment()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }

            else if (ValidateFor == "CheckIfAssetGroup3UsedInEquipment")
            {
                AssetGroup3_ValidateIfUsedInEquipment trans = new AssetGroup3_ValidateIfUsedInEquipment()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            else if (ValidateFor == "CheckMeter1CurrentReading")
            {
                FleetFuel_ValidateMeter1Reading trans = new FleetFuel_ValidateMeter1Reading()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.Reading = this.Reading;
                trans.Equipment.ObjectName = this.ObjectName;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }
            else if (ValidateFor == "CheckBothMeterReading")
            {
                FleetMeterReading_ValidateMeterReadingForBothMeter trans = new FleetMeterReading_ValidateMeterReadingForBothMeter()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.Meter1CurrentReading = this.Meter1CurrentReading;
                trans.Equipment.Meter2CurrentReading = this.Meter2CurrentReading;
                trans.Equipment.FirstMeterVoid = this.FirstMeterVoid;
                trans.Equipment.SecondMeterVoid = this.SecondMeterVoid;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }

            else if (ValidateFor == "ValidiationForFleetMeterandFuelTrackingUnvoid")
            {
                FleetMeterReading_ValidiationForUnvoidforFleetMeterandFuelTracking trans = new FleetMeterReading_ValidiationForUnvoidforFleetMeterandFuelTracking()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.FleetMeterReadingId = this.FleetMeterReadingId;
                trans.Equipment.TableName = this.TableName;
                trans.Equipment.Meter2Indicator = this.Meter2Indicator;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);


            }
            else if (ValidateFor == "CheckIfScrap")
            {
                Equipment_ValidateByScrap trans = new Equipment_ValidateByScrap()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();
                trans.Equipment.Flag = this.Flag;
                trans.Equipment.ObjectName = this.ObjectName;
                trans.Equipment.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);
            }
            else
            {
                // Create a table to hold the columns that need to be validated against their lookup list
                System.Data.DataTable lulist = new DataTable("lulist");
                lulist.Columns.Add("RowID", typeof(Int64));
                lulist.Columns.Add("SiteID", typeof(Int64));
                lulist.Columns.Add("ColumnName", typeof(string));
                lulist.Columns.Add("ColumnValue", typeof(string));
                lulist.Columns.Add("ListName", typeof(string));
                lulist.Columns.Add("ListFilter", typeof(string));
                lulist.Columns.Add("ErrorID", typeof(Int64));
                // Add a row for each column to be validated
                // Possible future is to process through the properties in the uiconfig and add based on values
                int rowid = 0;
                string filter = "";
                // Type

                //NEED TO OPEN THE COMMENTED CODE LATER
                //if (!string.IsNullOrWhiteSpace(uicEquipment.Type.Lookup_Filter_Property)) { filter = this.GetPropertyValue(uicEquipment.Type.Lookup_Filter_Property); }
                lulist.Rows.Add(++rowid, SiteId, "Type", Type, Lookup_ListName, "", 3);

                Equipment_ValidateByClientLookupId trans = new Equipment_ValidateByClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                    CreateMode = this.CreateMode,
                    lulist = lulist
                    // 20110039
                };
                trans.Equipment = this.ToDatabaseObject();

                trans.Equipment.ParentIdClientLookupId = this.ParentIdClientLookupId;
                trans.Equipment.ElectricalParentClientLookupId = this.ElectricalParentClientLookupId;
                trans.Equipment.MaintVendorIdClientLookupId = this.MaintVendorIdClientLookupId;
                trans.Equipment.PurchVendorIdClientLookupId = this.PurchVendorIdClientLookupId;
                trans.Equipment.PartIdClientLookupId = this.PartIdClientLookupId;
                trans.Equipment.LocationIdClientLookupId = this.LocationIdClientLookupId;
                trans.Equipment.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
                trans.Equipment.MaterialAccountClientLookupId = this.MaterialAccountClientLookupId;
                trans.Equipment.InactiveFlag = this.InactiveFlag;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }

            return errors;
        }

        public List<Equipment> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Equipment_RetrieveClientLookupIdBySearchCriteria trans = new Equipment_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Equipment> equipments = new List<Equipment>();
            foreach (b_Equipment eq in trans.EquipmentList)
            {
                Equipment tmpEq = new Equipment()
                {
                    EquipmentId = eq.EquipmentId,
                    ClientLookupId = eq.ClientLookupId
                };
                equipments.Add(tmpEq);
            }

            return equipments;
        }

        //public void RetrieveInitialSearchConfigurationData(DatabaseKey dbKey, ClientWebSite local)
        //{

        //    Equipment_RetrieveInitialSearchConfigurationData trans = new Equipment_RetrieveInitialSearchConfigurationData()
        //    {
        //        // RKL - Could we modify the dbkey.set method of abstract transaction manager to set these two?
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName
        //    };
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    SearchCriteria = trans.SearchCriteria;
        //    // add the Dates
        //    Load_DateSelection(local);
        //    // add the 'Columns'
        //    Load_ColumnSelection(local);
        //}

        //------------------------------Added By Indusnet Technologies-------------------------------------
        public List<Equipment> RetrieveAllBySiteId(DatabaseKey dbKey)
        {
            RetrieveAllEquipmentBySiteId trans = new RetrieveAllEquipmentBySiteId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SiteId = this.SiteId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseObjectList(trans.EquipmentList));

        }

        public List<Equipment> GetAllEquipment(DatabaseKey dbKey)
        {
            GetAllEquipment trans = new GetAllEquipment();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseObjectList(trans.EquipmentList));

        }


        public List<Equipment> Equipment_GetAllDeptLineSys(DatabaseKey dbKey)
        {
            Equipment_GetAllDeptLineSys trans = new Equipment_GetAllDeptLineSys();
            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseDeptLineSys(trans.EquipmentList));

        }
        //------------------------------End Added By Indusnet Technologies-----------------------------------
        public bool DeleteEquipment(DatabaseKey dbKey)
        {
            Equipment_DeleteByFK trans = new Equipment_DeleteByFK();
            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return trans.retValue;
        }
        public void CheckEquipmentIsParentofanother(DatabaseKey dbKey)
        {
            ValidateFor = "CheckInactive";
            Validate<Equipment>(dbKey);
        }

        public void CheckEquipmentIsInactivateorActivate(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfInactivateorActivate";
            Validate<Equipment>(dbKey);
        }

        public void CheckEquipmentIsScrapped(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfScrap";
            Validate<Equipment>(dbKey);
        }

        //------SOM-774------//
        public void ChangeClientLookupId(DatabaseKey dbKey)
        {
            Validate<Equipment>(dbKey);
            if (IsValid)
            {

                Equipment_ChangeClientLookupId trans = new Equipment_ChangeClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                AuditEnabled = true;
                trans.Equipment = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Equipment);
            }
        }

        public void CheckIfAssetGroup1UsedInEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfAssetGroup1UsedInEquipment";
            Validate<Equipment>(dbKey);
        }

        public void CheckIfAssetGroup2UsedInEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfAssetGroup2UsedInEquipment";
            Validate<Equipment>(dbKey);
        }

        public void CheckIfAssetGroup3UsedInEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfAssetGroup3UsedInEquipment";
            Validate<Equipment>(dbKey);
        }

        //------SOM-784------//
        public void RetrieveCreateModifyDate(DatabaseKey dbKey)
        {
            Equipment_RetrieveCreateModifyDate trans = new Equipment_RetrieveCreateModifyDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Equipment);
            this.CreateBy = trans.Equipment.CreateBy;
            this.CreateDate = trans.Equipment.CreateDate;
            this.ModifyBy = trans.Equipment.ModifyBy;
            this.ModifyDate = trans.Equipment.ModifyDate;
        }
        #endregion

        #region Search Parameter Lists
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }
       

        #endregion

        public static List<Equipment> UpdateFromDatabaseObjectList(List<b_Equipment> dbObjs)
        {
            List<Equipment> result = new List<Equipment>();

            foreach (b_Equipment dbObj in dbObjs)
            {
                Equipment tmp = new Equipment();
                tmp.UpdateFromDatabaseObject(dbObj);

                {

                    tmp.DepartmentName = dbObj.DepartmentName;
                    tmp.LaborAccountClientLookupId = dbObj.LaborAccountClientLookupId;
                    tmp.Process = dbObj.Process;
                    tmp.System = dbObj.System;
                    // SOM-805
                    tmp.LocationIdClientLookupId = dbObj.LocationIdClientLookupId;
                    //SOM-1259
                    tmp.Area_Desc = dbObj.Area_Desc;
                    tmp.Dept_Desc = dbObj.Dept_Desc;
                    tmp.Line_Desc = dbObj.Line_Desc;
                    tmp.System_Desc = dbObj.System_Desc;
                }
                result.Add(tmp);
            }
            return result;
        }

        public static List<Equipment> UpdateFromDatabaseAllBySiteId(List<b_Equipment> dbObjs)
        {
            List<Equipment> result = new List<Equipment>();

            foreach (b_Equipment dbObj in dbObjs)
            {
                Equipment tmp = new Equipment();
                tmp.UpdateFromDatabaseObject(dbObj);

                {
                    tmp.LaborAccountClientLookupId = dbObj.LaborAccountClientLookupId;
                    tmp.Process = dbObj.Process;
                    tmp.System = dbObj.System;
                    tmp.LocationIdClientLookupId = dbObj.LocationIdClientLookupId;
                    tmp.Area_Desc = dbObj.Area_Desc;
                    tmp.AssetGroup1Desc = dbObj.AssetGroup1Desc;
                    tmp.AssetGroup2Desc = dbObj.AssetGroup2Desc;
                    tmp.AssetGroup3Desc = dbObj.AssetGroup3Desc;
                    tmp.AssetGroup1ClientLookupId = dbObj.AssetGroup1ClientLookupId;
                    tmp.AssetGroup2ClientLookupId = dbObj.AssetGroup2ClientLookupId;
                    tmp.AssetGroup3ClientLookupId = dbObj.AssetGroup3ClientLookupId;
                }
                result.Add(tmp);
            }
            return result;
        }


        public static List<Equipment> UpdateFromDatabaseDeptLineSys(List<b_Equipment> dbObjs)
        {
            List<Equipment> result = new List<Equipment>();

            foreach (b_Equipment dbObj in dbObjs)
            {
                Equipment tmp = new Equipment();
                tmp.UpdateFromDatabaseObject(dbObj);

                {

                    tmp.DeptClientLookUpId = dbObj.DeptClientLookUpId;
                    tmp.Dept_Desc = dbObj.Dept_Desc;
                    tmp.LineClientLookUpId = dbObj.LineClientLookUpId;
                    tmp.SysClientLookUpId = dbObj.SysClientLookUpId;

                    tmp.LocationIdClientLookupId = dbObj.LocationIdClientLookupId;
                    //SOM-1259
                    tmp.System_Desc = dbObj.System_Desc;
                    tmp.Line_Desc = dbObj.Line_Desc;
                }
                result.Add(tmp);
            }
            return result;
        }

        public static List<Equipment> RetrieveByLocationId(DatabaseKey dbKey, Equipment equipment)
        {
            Equipment_RetrieveByLocationId trans = new Equipment_RetrieveByLocationId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = equipment.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Equipment.UpdateFromDatabaseObjectList(trans.EquipmentList);
        }

        public static List<ReportDataStructure> RetrieveRawReportList(DatabaseKey databaseKey, string columnName, string tableName, string referenceTable,
            string joinOnColumn, string searchOnColumn, List<string> selectedVals, List<string> selectedCols, List<string> selectedNums, string textColumn,
            string searchText, bool useLike, string dateColumn, DateTime startDate, DateTime endDate)
        {
            RetrieveEquipmentReport trans = new RetrieveEquipmentReport()
            {
                dbKey = databaseKey.ToTransDbKey(),
                CallerUserInfoId = databaseKey.User.UserInfoId,
                CallerUserName = databaseKey.UserName,
                PrimaryColumn = columnName,
                PrimaryTable = tableName,
                JoinOnColumn = joinOnColumn,
                SearchOnColumn = searchOnColumn,
                JoinedTable = referenceTable,
                SelectedValues = selectedVals,
                SelectedColumns = selectedCols,
                SelectedNumerics = selectedNums,
                Column = textColumn,
                SearchText = searchText,
                UseLike = useLike,
                DateSelection = dateColumn,
                DateStart = startDate,
                DateEnd = endDate
            };
            trans.Execute();
            return trans.RawList;
        }

        //----SOM-893---//
        public List<Equipment> GetAllEquipmentChildren(DatabaseKey dbKey)
        {
            Equipment_GetAllEquipmentChildren trans = new Equipment_GetAllEquipmentChildren()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.EquipmentList));

        }
        public List<Equipment> GetAllEquipmentFreeChildren(DatabaseKey dbKey)
        {
            Equipment_GetAllEquipmentFreeChildren trans = new Equipment_GetAllEquipmentFreeChildren()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.EquipmentList));

        }




        public void CheckDuplicateEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckDuplicate";
            Validate<Equipment>(dbKey);
        }

        public void CheckForeignFieldToDelete(DatabaseKey dbKey)
        {
            ValidateFor = "CheckForeignFieldToDelete";
            Validate<Equipment>(dbKey);
        }

        public void CheckIfDepartmentUsedInEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfDepartmentUsedInEquipment";
            Validate<Equipment>(dbKey);
        }

        public void CheckIfLineUsedInEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfLineUsedInEquipment";
            Validate<Equipment>(dbKey);
        }

        public void CheckIfSystemInfoUsedInEquipment(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfSystemInfoUsedInEquipment";
            Validate<Equipment>(dbKey);
        }


        public void ImportEquipment(DatabaseKey dbKey)
        {
         
            Equipment_Create trans = new Equipment_Create()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Equipment);
            
        }
        public bool CheckAssetCategory()
        {
            bool isValid = true;
            List<string> CatList = new List<string>();
            CatList.Add(Common.Constants.AssetCategoryConstant.Equipment.ToLower());
            CatList.Add(Common.Constants.AssetCategoryConstant.Location.ToLower());
            CatList.Add(Common.Constants.AssetCategoryConstant.Vehicle.ToLower());
            if (!CatList.Contains(this.AssetCategory.ToLower()))
            {
                isValid = false;
                this.ErrorMessages.Add("Invalid Asset Category");
            }

            return isValid;

        }
        public List<Equipment> RetrieveAll(DatabaseKey dbKey)
        {
           

            Equipment_RetrieveAll_V2 trans = new Equipment_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Equipment> EquipmentList = new List<Equipment>();
            foreach (b_Equipment Equipment in trans.EquipmentList)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObject(Equipment);
                EquipmentList.Add(tmpEquipment);
            }
            return EquipmentList;
        }
        public void UpdateBulk(DatabaseKey dbKey)
        {
            Equipment_UpdateBulk trans = new Equipment_UpdateBulk();
            trans.Equipment = this.ToDatabaseObject();
            trans.Equipment.EquipmentIdList = this.EquipmentIdList;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment);
        }


        public void RetrieveforMentionAlert(DatabaseKey dbKey)
        {
            Equipment_RetrieveforMentionAlert trans = new Equipment_RetrieveforMentionAlert()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Equipment);
            this.CreateBy = trans.Equipment.CreateBy;
            this.CreateDate = trans.Equipment.CreateDate;
            this.ModifyBy = trans.Equipment.ModifyBy;
            this.ModifyDate = trans.Equipment.ModifyDate;

        }



        public Equipment EquipmentRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveChunkSearchV2 trans = new Equipment_RetrieveChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfEquipment = new List<Equipment>();


            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.Equipment.listOfEquipment)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            this.listOfEquipment.AddRange(Equipmentlist);
            return this;
        }


        public List<Equipment> FleetAssetRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveFleetAssetChunkSearchV2 trans = new Equipment_RetrieveFleetAssetChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

       
            this.listOfEquipment = new List<Equipment>();


            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.Equipment.listOfEquipment)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForFleetAssetChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }
        #region V2-1213

        public List<Equipment> RetrieveAllChildrenChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveAllChildrenChunkSearchV2 trans = new Equipment_RetrieveAllChildrenChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();          
            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.Equipment.listOfEquipment)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForChildrenChunkSearchV2(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }

        public void UpdateFromDatabaseObjectForChildrenChunkSearchV2(b_Equipment dbObj, string TimeZone)
        {            
            this.TotalCount = dbObj.TotalCount;
            this.EquipmentId = dbObj.EquipmentId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Make = dbObj.Make;
            this.Model = dbObj.Model;
            this.Name = dbObj.Name;
            this.SerialNumber = dbObj.SerialNumber;
            this.Type = dbObj.Type;           
        }
        #endregion
        public List<Equipment> FleetMeterReadingRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveFleetMeterReadingChunkSearchV2 trans = new Equipment_RetrieveFleetMeterReadingChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForFleetMeterReadingChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();

            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.Equipment.listOfEquipment)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForFleetMeterReadingChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }
        public b_Equipment ToDateBaseObjectForFleetMeterReadingChunkSearch()
        {
            b_Equipment dbObj = this.ToDatabaseObject();


            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ReadingDateStart = this.ReadingDateStart;
            dbObj.ReadingDateEnd = this.ReadingDateEnd;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public List<Equipment> FleetFuelRetrieveChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveFleetFuelChunkSearchV2 trans = new Equipment_RetrieveFleetFuelChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();
            this.FleetMeterReadingId = trans.Equipment.FleetMeterReadingId;
            this.Meter1CurrentReading= trans.Equipment.Meter1CurrentReading;
            List<Equipment> Equipmentlist = new List<Equipment>();
            foreach (b_Equipment Equipment in trans.Equipment.listOfEquipment)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForFleetAssetChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }
        public b_Equipment ToDateBaseObjectForChunkSearch()
        {
            b_Equipment dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.SearchText = this.SearchText;
            dbObj.AssetGroup1ClientLookupId = this.AssetGroup1ClientLookupId;
            dbObj.AssetGroup2ClientLookupId = this.AssetGroup2ClientLookupId;
            dbObj.AssetGroup3ClientLookupId = this.AssetGroup3ClientLookupId;
            dbObj.SerialNo = this.SerialNo;
            dbObj.ModelNo = this.ModelNo;
            dbObj.Account = this.Account;
            //<!--(Added on 25/06/2020)-->
            dbObj.AssetGroup1Id = this.AssetGroup1Id;
            dbObj.AssetGroup2Id = this.AssetGroup2Id;
            dbObj.AssetGroup3Id = this.AssetGroup3Id;
            //<!--(Added on 25/06/2020)-->
            dbObj.ReadingStartDate = this.ReadingStartDate;
            dbObj.ReadingEndDate = this.ReadingEndDate;
            dbObj.Reading = this.Reading;
            dbObj.AssetAvailability = this.AssetAvailability;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForFleetMeterReadingChunkSearch(b_Equipment dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.EquipmentImage = dbObj.EquipmentImage;
            this.FMRReadingDate = dbObj.FMRReadingDate;
            this.NoofDays = dbObj.NoofDays;
            this.MeterReadingL1 = dbObj.MeterReadingL1;
            this.MeterReadingL2= dbObj.MeterReadingL2;
            this.FleetMeterReadingId = dbObj.FleetMeterReadingId;
            this.SourceType = dbObj.SourceType;
            this.Action = dbObj.Action;
            this.Meter2Indicator = dbObj.Meter2Indicator;
            this.SourceId = dbObj.SourceId;
            this.TotalCount = dbObj.TotalCount;
        }

        public void UpdateFromDatabaseObjectForChunkSearch(b_Equipment dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.LaborAccountClientLookupId = dbObj.LaborAccountClientLookupId;
            this.Process = dbObj.Process;
            this.System = dbObj.System;
            this.LocationIdClientLookupId = dbObj.LocationIdClientLookupId;
            this.Area_Desc = dbObj.Area_Desc;
            this.AssetGroup1ClientLookupId = dbObj.AssetGroup1ClientLookupId;
            this.AssetGroup2ClientLookupId = dbObj.AssetGroup2ClientLookupId;
            this.AssetGroup3ClientLookupId = dbObj.AssetGroup3ClientLookupId;
            this.TotalCount = dbObj.TotalCount;



        }

        public void UpdateFromDatabaseObjectForFleetAssetChunkSearch(b_Equipment dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCount = dbObj.TotalCount;

            this.ReadingDate = dbObj.ReadingDate;
            this.FuelAmount = dbObj.FuelAmount;
            this.FuelUnits = dbObj.FuelUnits;
            this.UnitCost = dbObj.UnitCost;
            this.TotalCost = dbObj.TotalCost;
            this.FuelTrackingId = dbObj.FuelTrackingId;
            this.Void = dbObj.Void;
            this.EquipImage = dbObj.EquipImage;
            this.FleetMeterReadingId = dbObj.FleetMeterReadingId;
            this.Meter1CurrentReading = dbObj.Meter1CurrentReading;
            this.Reading = dbObj.Reading;
        }

       
        public b_Equipment ToDatabaseObjectExtd()
        {
            b_Equipment dbObj = new b_Equipment();

            dbObj.isfleetDimensionData = this.isfleetDimensionData;
            dbObj.isfleetEngineData = this.isfleetEngineData;
            dbObj.isfleetFluidsData = this.isfleetFluidsData;
            dbObj.isfleetWheelData = this.isfleetWheelData;

            dbObj.fleetDimensions = this.fleetDimensions.ToDatabaseObject();
            dbObj.fleetEngine = this.fleetEngine.ToDatabaseObject();
            dbObj.fleetFluids = this.fleetFluids.ToDatabaseObject();
            dbObj.fleetWheel = this.fleetWheel.ToDatabaseObject();

            dbObj.ClientId = this.ClientId;
            dbObj.EquipmentId = this.EquipmentId;
            dbObj.SiteId = this.SiteId;
            dbObj.AreaId = this.AreaId;
            dbObj.DepartmentId = this.DepartmentId;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.AcquiredCost = this.AcquiredCost;
            dbObj.AcquiredDate = this.AcquiredDate;
            dbObj.BIMIdentifier = this.BIMIdentifier;
            dbObj.BookValue = this.BookValue;
            dbObj.BusinessGroup = this.BusinessGroup;
            dbObj.CatalogNumber = this.CatalogNumber;
            dbObj.Category = this.Category;
            dbObj.CostCenter = this.CostCenter;
            dbObj.DeprCode = this.DeprCode;
            dbObj.DeprLifeToDate = this.DeprLifeToDate;
            dbObj.DeprPercent = this.DeprPercent;
            dbObj.DeprYearToDate = this.DeprYearToDate;
            dbObj.ElectricalParent = this.ElectricalParent;
            dbObj.InactiveFlag = this.InactiveFlag;
            dbObj.CriticalFlag = this.CriticalFlag;
            dbObj.InstallDate = this.InstallDate;
            dbObj.Labor_AccountId = this.Labor_AccountId;
            dbObj.LifeinMonths = this.LifeinMonths;
            dbObj.LifeinYears = this.LifeinYears;
            dbObj.Location = this.Location;
            dbObj.LocationId = this.LocationId;
            dbObj.Maint_VendorId = this.Maint_VendorId;
            dbObj.Maint_WarrantyDesc = this.Maint_WarrantyDesc;
            dbObj.Maint_WarrantyExpire = this.Maint_WarrantyExpire;
            dbObj.Make = this.Make;
            dbObj.Material_AccountId = this.Material_AccountId;
            dbObj.Model = this.Model;
            dbObj.Name = this.Name;
            dbObj.NoCostRollUp = this.NoCostRollUp;
            dbObj.NoPartXRef = this.NoPartXRef;
            dbObj.OriginalValue = this.OriginalValue;
            dbObj.OutofService = this.OutofService;
            dbObj.ParentId = this.ParentId;
            dbObj.PartId = this.PartId;
            dbObj.Purch_VendorId = this.Purch_VendorId;
            dbObj.Purch_WarrantyDesc = this.Purch_WarrantyDesc;
            dbObj.Purch_WarrantyExpire = this.Purch_WarrantyExpire;
            dbObj.RIMEClass = this.RIMEClass;
            dbObj.SalvageValue = this.SalvageValue;
            dbObj.SerialNumber = this.SerialNumber;
            dbObj.Size = this.Size;
            dbObj.SizeUnits = this.SizeUnits;
            dbObj.Status = this.Status;
            dbObj.Type = this.Type;
            dbObj.AssetNumber = this.AssetNumber;
            dbObj.ProcessSystemId = this.ProcessSystemId;
            dbObj.PlantLocationId = this.PlantLocationId;
            dbObj.EquipmentMasterId = this.EquipmentMasterId;
            dbObj.LineId = this.LineId;
            dbObj.SystemInfoId = this.SystemInfoId;
            dbObj.AssetCategory = this.AssetCategory;
            dbObj.SubType = this.SubType;
            dbObj.AssetGroup1 = this.AssetGroup1;
            dbObj.AssetGroup2 = this.AssetGroup2;
            dbObj.AssetGroup3 = this.AssetGroup3;
            dbObj.VIN = this.VIN;
            dbObj.VehicleType = this.VehicleType;
            dbObj.License = this.License;
            dbObj.RegistrationLoc = this.RegistrationLoc;
            dbObj.VehicleYear = this.VehicleYear;
            dbObj.CurrentReading = this.CurrentReading;
            dbObj.CurrentReadingDate = this.CurrentReadingDate;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }

        public void CheckMeter1CurrentReading(DatabaseKey dbKey)
        {
            ValidateFor = "CheckMeter1CurrentReading";
            Validate<Equipment>(dbKey);
        }

        public void CheckBothMeterReading(DatabaseKey dbKey)
        {
            ValidateFor = "CheckBothMeterReading";
            Validate<Equipment>(dbKey);
        }


        public void ValidiationForFleetMeterandFuelTrackingUnvoid(DatabaseKey dbKey)
        {
            ValidateFor = "ValidiationForFleetMeterandFuelTrackingUnvoid";
            Validate<Equipment>(dbKey);
        }

        #region Fuel Tracking Retrieve By Equipment Id
        public List<Equipment> FleetFuelRetrieveByEquipmentIdV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveFleetFuelByEquipmentIdV2 trans = new Equipment_RetrieveFleetFuelByEquipmentIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForRetrieveByEquipmentId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();
            this.FleetMeterReadingId = trans.Equipment.FleetMeterReadingId;
            this.Meter1CurrentReading = trans.Equipment.Meter1CurrentReading;
            List<Equipment> Equipmentlist = new List<Equipment>();
            foreach (b_Equipment Equipment in trans.EquipmentList)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForFleetAssetRetrieveByEquipmentId(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }

        public b_Equipment ToDateBaseObjectForRetrieveByEquipmentId()
        {
            b_Equipment dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.FuelTrackingId = this.FuelTrackingId;
            dbObj.FuelAmount = this.FuelAmount;
            dbObj.UnitCost = this.UnitCost;
            dbObj.ReadingDate = this.ReadingDate;
            dbObj.FuelUnit = this.FuelUnit;
            dbObj.Meter1Units = this.Meter1Units;
            dbObj.Meter2Units = this.Meter2Units;
            dbObj.TotalCost = this.TotalCost;


            return dbObj;
        }

        public void UpdateFromDatabaseObjectForFleetAssetRetrieveByEquipmentId(b_Equipment dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
           

            this.ClientId = dbObj.ClientId;
            this.FuelTrackingId = dbObj.FuelTrackingId;
            this.FuelAmount = dbObj.FuelAmount;
            this.UnitCost = dbObj.UnitCost;
            this.ReadingDate = dbObj.ReadingDate;
            this.FuelUnit = dbObj.FuelUnit;
            this.Meter1Units = dbObj.Meter1Units;
            this.Meter2Units = dbObj.Meter2Units;
            this.TotalCost = dbObj.TotalCost;

        }

        public List<Equipment> FleetMeterReadingRetrieveByEquipmentIdV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveFleetMeterReadingByEquipmentIdV2 trans = new Equipment_RetrieveFleetMeterReadingByEquipmentIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForFleetMeterReadingChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();

            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.EquipmentList)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForFleetMeterReadingChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }
        #endregion

        #region Equipment Lookup list
        public b_Equipment ToDateBaseObjectForEquipmentLookuplistChunkSearch()
        {
            b_Equipment dbObj = this.ToDatabaseObject();


            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Name = this.Name;
            dbObj.Model = this.Model;
            dbObj.Type = this.Type;
            dbObj.SerialNumber = this.SerialNumber;
            dbObj.AssetGroup1ClientLookupId = this.AssetGroup1ClientLookupId;
            dbObj.AssetGroup2ClientLookupId = this.AssetGroup2ClientLookupId;
            dbObj.AssetGroup3ClientLookupId = this.AssetGroup3ClientLookupId;
            return dbObj;
        }
        public List<Equipment> GetAllEquipmentLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveChunkSearchLookupListV2 trans = new Equipment_RetrieveChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForEquipmentLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();

            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.EquipmentList)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForEquipmentLookupListChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }

        public List<Equipment> GetAllEquipmentLookupListMobileV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveChunkSearchLookupListMobile_V2 trans = new Equipment_RetrieveChunkSearchLookupListMobile_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForEquipmentLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();

            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.EquipmentList)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForEquipmentLookupListChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }

        public void UpdateFromDatabaseObjectForEquipmentLookupListChunkSearch(b_Equipment dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.TotalCount = dbObj.TotalCount;
            this.AssetGroup1ClientLookupId = dbObj.AssetGroup1ClientLookupId;
            this.AssetGroup2ClientLookupId = dbObj.AssetGroup2ClientLookupId;
            this.AssetGroup3ClientLookupId = dbObj.AssetGroup3ClientLookupId;

        }
        #endregion

        #region Retrieve EquipmentID By ClientLookupId V2-625
        public Equipment RetrieveEquipmentIdByClientLookupIdV2(DatabaseKey dbKey)
        {
            Equipment_RetrieveEquipmentIdbyClientLookupId_V2 trans = new Equipment_RetrieveEquipmentIdbyClientLookupId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForRetrieveEquipmentIdIdByClientLookupIdV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Equipment tmpequipment = new Equipment()
            {
                EquipmentId = trans.EquipmentResult.EquipmentId,
                ClientLookupId = trans.EquipmentResult.ClientLookupId,
            };

            return tmpequipment;
        }
        public b_Equipment ToDateBaseObjectForRetrieveEquipmentIdIdByClientLookupIdV2()
        {
            b_Equipment dbObj = this.ToDatabaseObject();
            dbObj.ClientLookupId = this.ClientLookupId;

            return dbObj;
        }
        #endregion

        #region  V2-637 Repairable Spare Asset
        public List<Equipment> GetEquipmentForRepSpareAssetLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveRepSpareAssetChunkSearchLookupListV2 trans = new Equipment_RetrieveRepSpareAssetChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForRepSpareAssetLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfEquipment = new List<Equipment>();

            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.EquipmentList)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForRepSpareAssetLookupListChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            return Equipmentlist;
        }

        public b_Equipment ToDateBaseObjectForRepSpareAssetLookuplistChunkSearch()
        {
            b_Equipment dbObj = this.ToDatabaseObject();

            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Name = this.Name;
            dbObj.Model = this.Model;
            dbObj.Type = this.Type;
            dbObj.Make = this.Make;
            dbObj.IsAssigned = this.IsAssigned;
            return dbObj;
        }
        public void AddRepairableSpareAsset(DatabaseKey dbKey)
        {
            Validate<Equipment>(dbKey);

            if (IsValid)
            {
                Equipment_RepairableSpareAsset_V2 trans = new Equipment_RepairableSpareAsset_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.Equipment = this.ToDatabaseObject();

                trans.Equipment.ParentIdClientLookupId = this.ParentIdClientLookupId;
                trans.Equipment.ElectricalParentClientLookupId = this.ElectricalParentClientLookupId;
                trans.Equipment.MaintVendorIdClientLookupId = this.MaintVendorIdClientLookupId;
                trans.Equipment.PurchVendorIdClientLookupId = this.PurchVendorIdClientLookupId;
                trans.Equipment.PartIdClientLookupId = this.PartIdClientLookupId;
                trans.Equipment.LocationIdClientLookupId = this.LocationIdClientLookupId;
                trans.Equipment.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
                trans.Equipment.MaterialAccountClientLookupId = this.MaterialAccountClientLookupId;
                trans.RepairableSpareLog = RepairableSpareLogDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Equipment);
            }
        }
        private b_RepairableSpareLog RepairableSpareLogDatabaseObject()
        {
            b_RepairableSpareLog dbObj = new b_RepairableSpareLog();
            dbObj.ClientId = this.ClientId;
            dbObj.RepairableSpareLogId = RepairableSpareLog.RepairableSpareLogId;
            dbObj.SiteId = RepairableSpareLog.SiteId;
            dbObj.EquipmentId = RepairableSpareLog.EquipmentId;
            dbObj.TransactionDate = RepairableSpareLog.TransactionDate;
            dbObj.Status = RepairableSpareLog.Status;
            dbObj.PersonnelId = RepairableSpareLog.PersonnelId;
            dbObj.Location = RepairableSpareLog.Location;
            dbObj.ParentId = RepairableSpareLog.ParentId;
            dbObj.AssetGroup1 = RepairableSpareLog.AssetGroup1;
            dbObj.AssetGroup2 = RepairableSpareLog.AssetGroup2;
            dbObj.AssetGroup3 = RepairableSpareLog.AssetGroup3;
            dbObj.Assigned = RepairableSpareLog.Assigned;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRepSpareAssetLookupListChunkSearch(b_Equipment dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
           
            this.TotalCount = dbObj.TotalCount;


        }
        #endregion

        #region

        public Equipment EquipmentRetrieveChunkSearchMobileV2(DatabaseKey dbKey, string TimeZone)
        {
            Equipment_RetrieveChunkSearchMobileV2 trans = new Equipment_RetrieveChunkSearchMobileV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfEquipment = new List<Equipment>();


            List<Equipment> Equipmentlist = new List<Equipment>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Equipment Equipment in trans.Equipment.listOfEquipment)
            {
                Equipment tmpEquipment = new Equipment();

                tmpEquipment.UpdateFromDatabaseObjectForChunkSearch(Equipment, TimeZone);
                Equipmentlist.Add(tmpEquipment);
            }
            this.listOfEquipment.AddRange(Equipmentlist);
            return this;
        }

        #endregion

        #region V2-948
        public Equipment RetrieveLaborAccountByEquipmentIdV2(DatabaseKey dbKey)
        {
            Equipment_RetrieveAccountByEquipmentIdV2 trans = new Equipment_RetrieveAccountByEquipmentIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment = this.ToDateBaseObjectRetrieveAccountByEquipmentIdV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Equipment tmpequipment = new Equipment()
            {
                EquipmentId = trans.EquipmentResult.EquipmentId,
                Labor_AccountId = trans.EquipmentResult.Labor_AccountId,
                LaborAccountClientLookupId = trans.EquipmentResult.LaborAccountClientLookupId
            };

            return tmpequipment;
        }
        public b_Equipment ToDateBaseObjectRetrieveAccountByEquipmentIdV2()
        {
            b_Equipment dbObj = new b_Equipment();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.EquipmentId = this.EquipmentId;
            return dbObj;
        }
        #endregion[

        #region V2-846
        public List<Equipment> GetAllEquipmentParent(DatabaseKey dbKey)
        {
            GetAllEquipmentParentV2 trans = new GetAllEquipmentParentV2();
            trans.Equipment = this.ToDateBaseObjectForGetAllParent();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (UpdateFromDatabaseObjectListForParent(trans.EquipmentList));

        }
        public b_Equipment ToDateBaseObjectForGetAllParent()
        {
            b_Equipment dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }
        public static List<Equipment> UpdateFromDatabaseObjectListForParent(List<b_Equipment> dbObjs)
        {
            List<Equipment> result = new List<Equipment>();

            foreach (b_Equipment dbObj in dbObjs)
            {
                Equipment tmp = new Equipment();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.ChildCount = dbObj.ChildCount;
                tmp.TotalCount = dbObj.TotalCount;
                result.Add(tmp);
            }
            return result;
        }
        public List<Equipment> GetAllEquipmentChildrenForParent(DatabaseKey dbKey)
        {
            Equipment_GetAllEquipmentChildrenForParent trans = new Equipment_GetAllEquipmentChildrenForParent()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Equipment = this.ToDateBaseObjectForGetAllChildrenForParent();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectListForChildren(trans.EquipmentList));

        }
        public b_Equipment ToDateBaseObjectForGetAllChildrenForParent()
        {
            b_Equipment dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.EquipmentId = this.EquipmentId;
            return dbObj;
        }
        public static List<Equipment> UpdateFromDatabaseObjectListForChildren(List<b_Equipment> dbObjs)
        {
            List<Equipment> result = new List<Equipment>();

            foreach (b_Equipment dbObj in dbObjs)
            {
                Equipment tmp = new Equipment();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.ChildCount = dbObj.ChildCount;
                result.Add(tmp);
            }
            return result;
        }
        #endregion
    }
}
