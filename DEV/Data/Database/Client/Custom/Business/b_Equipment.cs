using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_Equipment
    {
        #region Property
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
        public string Area_Desc { get; set; }
        public string PlantLocationDescription { get; set; }
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
        public List<b_Equipment> listOfEquipment { get; set; }
        public Int64 StoreRoomId { get; set; }
        public Int64 ParentID { get; set; }
        public Int64 PartID { get; set; }
        public Int32 TotalCount { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
        public string AssetGroup1Desc { get; set; }
        public string AssetGroup2Desc { get; set; }
        public string AssetGroup3Desc { get; set; }
        //<!--(Added on 25/06/2020)-->
        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }
        //<!--(Added on 25/06/2020)-->

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

        public b_FleetDimensions fleetDimensions { get; set; }
        public b_FleetEngine fleetEngine { get; set; }
        public b_FleetFluids fleetFluids { get; set; }
        public b_FleetWheel fleetWheel { get; set; }

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
        public Boolean FirstMeterVoid { get; set; }
        public Boolean SecondMeterVoid { get; set; }
        //V2-391
        public string ReadingDateStart { get; set; }
        public string ReadingDateEnd { get; set; }
        public Int64 SourceId { get; set; }
        public string meter1currentreadingdate { get; set; }
        public string meter2currentreadingdate { get; set; }
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

        //V2-385
        public string ReadingStartDate { get; set; }
        public string ReadingEndDate { get; set; }
        public string EquipImage { get; set; }
        public DateTime ReadingDate { get; set; }
        public decimal FuelAmount { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public Int64 FuelTrackingId { get; set; }
        public bool Void { get; set; }
        public string AssetAvailability { get; set; }
        public bool IsAssigned { get; set; }//V2-637
        public b_RepairableSpareLog RepairableSpareLog { get; set; }
        public string AssignedClientlookupid { get; set; }
        public string AssignedAssetName { get; set; }
        public int ChildCount { get; set; }
        #region V2-1211
        public string MaintVendorName { get; set; }
        public string PurchVendorName { get; set; }
        #endregion
        #endregion

        public static b_Equipment ProcessRowByPKForeignKey(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment eq = new b_Equipment();

            // Load the object from the database
            eq.LoadFromDatabaseByPKForeignKey(reader);

            // Return result
            return eq;
        }

        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {

                //-----------------------------------
                if (false == reader.IsDBNull(i))
                {
                    ParentIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    ParentIdClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ElectricalParentClientLookupId = reader.GetString(i);
                }
                else
                {
                    ElectricalParentClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    MaintVendorIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    MaintVendorIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PurchVendorIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    PurchVendorIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LocationIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    LocationIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    MaterialAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    MaterialAccountClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LaborAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    LaborAccountClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProcessSystemDesc = reader.GetString(i);
                }
                else
                {
                    ProcessSystemDesc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PlantLocationDescription = reader.GetString(i);
                }
                else
                {
                    PlantLocationDescription = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Dept_Desc = reader.GetString(i);
                }
                else
                {
                    Dept_Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Line_Desc = reader.GetString(i);
                }
                else
                {
                    Line_Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    System_Desc = reader.GetString(i);
                }
                else
                {
                    System_Desc = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SiteID"].ToString(); }
                catch { missing.Append("SiteID "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AcquiredCost"].ToString(); }
                catch { missing.Append("AcquiredCost "); }

                try { reader["AcquiredDate"].ToString(); }
                catch { missing.Append("AcquiredDate "); }

                try { reader["BIMIdentifier"].ToString(); }
                catch { missing.Append("BIMIdentifier"); }

                //try { reader["Attachments"].ToString(); }
                //catch { missing.Append("Attachments "); }

                try { reader["BookValue"].ToString(); }
                catch { missing.Append("BookValue "); }

                try { reader["BusinessGroup"].ToString(); }
                catch { missing.Append("BusinessGroup "); }

                try { reader["CatalogNumber"].ToString(); }
                catch { missing.Append("CatalogNumber "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["CostCenter"].ToString(); }
                catch { missing.Append("CostCenter "); }

                try { reader["DeprCode"].ToString(); }
                catch { missing.Append("DeprCode "); }

                try { reader["DeprLifeToDate"].ToString(); }
                catch { missing.Append("DeprLifeToDate "); }

                try { reader["DeprPercent"].ToString(); }
                catch { missing.Append("DeprPercent "); }

                try { reader["DeprYearToDate"].ToString(); }
                catch { missing.Append("DeprYearToDate "); }

                try { reader["ElectricalParent"].ToString(); }
                catch { missing.Append("ElectricalParent "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["InstallDate"].ToString(); }
                catch { missing.Append("InstallDate "); }

                try { reader["Labor_AccountID"].ToString(); }
                catch { missing.Append("Labor_AccountID "); }

                try { reader["LifeinMonths"].ToString(); }
                catch { missing.Append("LifeinMonths "); }

                try { reader["LifeinYears"].ToString(); }
                catch { missing.Append("LifeinYears "); }

                try { reader["Location"].ToString(); }
                catch { missing.Append("Location "); }

                try { reader["LocationID"].ToString(); }
                catch { missing.Append("LocationID "); }

                try { reader["Maint_VendorID"].ToString(); }
                catch { missing.Append("Maint_VendorID "); }

                try { reader["Maint_WarrantyDesc"].ToString(); }
                catch { missing.Append("Maint_WarrantyDesc "); }

                try { reader["Maint_WarrantyExpire"].ToString(); }
                catch { missing.Append("Maint_WarrantyExpire "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["Material_AccountID"].ToString(); }
                catch { missing.Append("Material_AccountID "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["NoCostRollUp"].ToString(); }
                catch { missing.Append("NoCostRollUp "); }

                try { reader["NoPartXRef"].ToString(); }
                catch { missing.Append("NoPartXRef "); }

                try { reader["Notes"].ToString(); }
                catch { missing.Append("Notes "); }

                try { reader["OriginalValue"].ToString(); }
                catch { missing.Append("OriginalValue "); }

                try { reader["OutofService"].ToString(); }
                catch { missing.Append("OutofService "); }

                try { reader["ParentID"].ToString(); }
                catch { missing.Append("ParentID "); }

                try { reader["PartID"].ToString(); }
                catch { missing.Append("PartID "); }

                try { reader["Purch_VendorID"].ToString(); }
                catch { missing.Append("Purch_VendorID "); }

                try { reader["Purch_WarrantyDesc"].ToString(); }
                catch { missing.Append("Purch_WarrantyDesc "); }

                try { reader["Purch_WarrantyExpire"].ToString(); }
                catch { missing.Append("Purch_WarrantyExpire "); }

                try { reader["RIMEClass"].ToString(); }
                catch { missing.Append("RIMEClass "); }

                try { reader["SalvageValue"].ToString(); }
                catch { missing.Append("SalvageValue "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["Size"].ToString(); }
                catch { missing.Append("Size "); }

                try { reader["SizeUnits"].ToString(); }
                catch { missing.Append("SizeUnits "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["UserDefined_Bit01"].ToString(); }
                catch { missing.Append("UserDefined_Bit01 "); }

                try { reader["UserDefined_Char01"].ToString(); }
                catch { missing.Append("UserDefined_Char01 "); }

                try { reader["UserDefined_Date01"].ToString(); }
                catch { missing.Append("UserDefined_Date01 "); }

                try { reader["UserDefined_Memo01"].ToString(); }
                catch { missing.Append("UserDefined_Memo01 "); }

                try { reader["UserDefined_Num01"].ToString(); }
                catch { missing.Append("UserDefined_Num01 "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["ParentIdClientLookupId"].ToString(); }
                catch { missing.Append("ParentIdClientLookupId "); }

                try { reader["ElectricalParentClientLookupId"].ToString(); }
                catch { missing.Append("ElectricalParentClientLookupId "); }

                try { reader["PartIdClientLookupId"].ToString(); }
                catch { missing.Append("PartIdClientLookupId "); }

                try { reader["MaintVendorIdClientLookupId"].ToString(); }
                catch { missing.Append("MaintVendorIdClientLookupId "); }

                try { reader["PurchVendorIdClientLookupId"].ToString(); }
                catch { missing.Append("PurchVendorIdClientLookupId "); }

                try { reader["LocationIdClientLookupId"].ToString(); }
                catch { missing.Append("LocationIdClientLookupId "); }

                try { reader["MaterialAccountClientLookupId"].ToString(); }
                catch { missing.Append("MaterialAccountClientLookupId "); }

                try { reader["LaborAccountClientLookupId"].ToString(); }
                catch { missing.Append("LaborAccountClientLookupId "); }

                try { reader["ProcessSystemDesc"].ToString(); }
                catch { missing.Append("ProcessSystemDesc"); }

                try { reader["Dept_Desc"].ToString(); }
                catch { missing.Append("Dept_Desc"); }

                try { reader["Line_Desc"].ToString(); }
                catch { missing.Append("Line_Desc"); }

                try { reader["System_Desc"].ToString(); }
                catch { missing.Append("System_Desc"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void LoadFromDatabaseByPKForeignKey_V2(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup3ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1Desc = reader.GetString(i);
                }
                else
                {
                    AssetGroup1Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2Desc = reader.GetString(i);
                }
                else
                {
                    AssetGroup2Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3Desc = reader.GetString(i);
                }
                else
                {
                    AssetGroup3Desc = "";
                }
                i++;
                //-----------------------------------

                //385
                if (false == reader.IsDBNull(i))
                {
                    FleetDimensionsId = reader.GetInt64(i);
                }
                else
                {
                    FleetDimensionsId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Color = reader.GetString(i);
                }
                else
                {
                    Color = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    BodyType = reader.GetString(i);
                }
                else
                {
                    BodyType = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Width = reader.GetDecimal(i);
                }
                else
                {
                    Width = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Height = reader.GetDecimal(i);
                }
                else
                {
                    Height = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Length = reader.GetDecimal(i);
                }
                else
                {
                    Length = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PassengerVolume = reader.GetDecimal(i);
                }
                else
                {
                    PassengerVolume = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CargoVolume = reader.GetDecimal(i);
                }
                else
                {
                    CargoVolume = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    GroundClearance = reader.GetDecimal(i);
                }
                else
                {
                    GroundClearance = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    BedLength = reader.GetDecimal(i);
                }
                else
                {
                    BedLength = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CurbWeight = reader.GetDecimal(i);
                }
                else
                {
                    CurbWeight = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VehicleWeight = reader.GetDecimal(i);
                }
                else
                {
                    VehicleWeight = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TowingCapacity = reader.GetDecimal(i);
                }
                else
                {
                    TowingCapacity = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxPayload = reader.GetDecimal(i);
                }
                else
                {
                    MaxPayload = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetDimensionUpdateIndex = reader.GetInt32(i);
                }

                else
                {
                    FleetDimensionUpdateIndex = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetEngineId = reader.GetInt64(i);
                }

                else
                {
                    FleetEngineId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EngineBrand = reader.GetString(i);
                }
                else
                {
                    EngineBrand = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Aspiration = reader.GetString(i);
                }
                else
                {
                    Aspiration = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Bore = reader.GetDecimal(i);
                }
                else
                {
                    Bore = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Cam = reader.GetString(i);
                }
                else
                {
                    Cam = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    Compression = reader.GetDecimal(i);
                }
                else
                {
                    Compression = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Cylinders = reader.GetDecimal(i);
                }
                else
                {
                    Cylinders = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Displacement = reader.GetDecimal(i);
                }
                else
                {
                    Displacement = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FuelInduction = reader.GetString(i);
                }
                else
                {
                    FuelInduction = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FuelQuality = reader.GetDecimal(i);
                }
                else
                {
                    FuelQuality = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxHP = reader.GetDecimal(i);
                }
                else
                {
                    MaxHP = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    MaxTorque = reader.GetDecimal(i);
                }
                else
                {
                    MaxTorque = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    RedlineRPM = reader.GetDecimal(i);
                }
                else
                {
                    RedlineRPM = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Stroke = reader.GetDecimal(i);
                }
                else
                {
                    Stroke = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Valves = reader.GetDecimal(i);
                }
                else
                {
                    Valves = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    TransmissionBrand = reader.GetString(i);
                }
                else
                {
                    TransmissionBrand = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    TransmissionType = reader.GetString(i);
                }
                else
                {
                    TransmissionType = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Gears = reader.GetDecimal(i);
                }
                else
                {
                    Gears = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetEngineUpdateIndex = reader.GetInt32(i);
                }

                else
                {
                    FleetEngineUpdateIndex = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetWheelId = reader.GetInt64(i);
                }

                else
                {
                    FleetWheelId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    BrakeSystem = reader.GetString(i);
                }
                else
                {
                    BrakeSystem = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    RearTrackWidth = reader.GetDecimal(i);
                }
                else
                {
                    RearTrackWidth = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Wheelbase = reader.GetDecimal(i);
                }
                else
                {
                    Wheelbase = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FrontWheelDiameter = reader.GetDecimal(i);
                }
                else
                {
                    FrontWheelDiameter = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    RearWheelDiameter = reader.GetDecimal(i);
                }
                else
                {
                    RearWheelDiameter = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FrontTirePSI = reader.GetDecimal(i);
                }
                else
                {
                    FrontTirePSI = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    RearTirePSI = reader.GetDecimal(i);
                }
                else
                {
                    RearTirePSI = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetWheelUpdateIndex = reader.GetInt32(i);
                }

                else
                {
                    FleetWheelUpdateIndex = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetFluidsId = reader.GetInt64(i);
                }

                else

                {
                    FleetFluidsId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetFluidsFuelQuality = reader.GetString(i);
                }
                else
                {
                    FleetFluidsFuelQuality = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FuelType = reader.GetString(i);
                }
                else
                {
                    FuelType = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FuelTankCapacity1 = reader.GetDecimal(i);
                }
                else
                {
                    FuelTankCapacity1 = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FuelTankCapacity2 = reader.GetDecimal(i);
                }
                else
                {
                    FuelTankCapacity2 = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EPACity = reader.GetDecimal(i);
                }
                else
                {
                    EPACity = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EPAHighway = reader.GetDecimal(i);
                }
                else
                {
                    EPAHighway = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EPACombined = reader.GetDecimal(i);
                }
                else
                {
                    EPACombined = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FleetFluidUpdateIndex = reader.GetInt32(i);
                }

                else
                {
                    FleetFluidUpdateIndex = 0;
                }
                i++;

                //385
                if (false == reader.IsDBNull(i))
                {
                    ParentIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    ParentIdClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ElectricalParentClientLookupId = reader.GetString(i);
                }
                else
                {
                    ElectricalParentClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    MaintVendorIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    MaintVendorIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PurchVendorIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    PurchVendorIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LocationIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    LocationIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    MaterialAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    MaterialAccountClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LaborAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    LaborAccountClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProcessSystemDesc = reader.GetString(i);
                }
                else
                {
                    ProcessSystemDesc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PlantLocationDescription = reader.GetString(i);
                }
                else
                {
                    PlantLocationDescription = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Dept_Desc = reader.GetString(i);
                }
                else
                {
                    Dept_Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Line_Desc = reader.GetString(i);
                }
                else
                {
                    Line_Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    System_Desc = reader.GetString(i);
                }
                else
                {
                    System_Desc = "";
                }
                i++;

                // Assigned clientlookupid
                if (false == reader.IsDBNull(i))
                {
                    AssignedClientlookupid = reader.GetString(i);
                }
                else
                {
                    AssignedClientlookupid = "";
                }
                i++;

                // Assigned AssetName
                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetName = reader.GetString(i);
                }
                else
                {
                    AssignedAssetName = "";
                }
                i++;

                // MaintVendorName
                if (false == reader.IsDBNull(i))
                {
                    MaintVendorName = reader.GetString(i);
                }
                else
                {
                    MaintVendorName = "";
                }
                i++;

                // PurchVendorName
                if (false == reader.IsDBNull(i))
                {
                    PurchVendorName = reader.GetString(i);
                }
                else
                {
                    PurchVendorName = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SiteID"].ToString(); }
                catch { missing.Append("SiteID "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AcquiredCost"].ToString(); }
                catch { missing.Append("AcquiredCost "); }

                try { reader["AcquiredDate"].ToString(); }
                catch { missing.Append("AcquiredDate "); }

                try { reader["BIMIdentifier"].ToString(); }
                catch { missing.Append("BIMIdentifier"); }

                //try { reader["Attachments"].ToString(); }
                //catch { missing.Append("Attachments "); }

                try { reader["BookValue"].ToString(); }
                catch { missing.Append("BookValue "); }

                try { reader["BusinessGroup"].ToString(); }
                catch { missing.Append("BusinessGroup "); }

                try { reader["CatalogNumber"].ToString(); }
                catch { missing.Append("CatalogNumber "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["CostCenter"].ToString(); }
                catch { missing.Append("CostCenter "); }

                try { reader["DeprCode"].ToString(); }
                catch { missing.Append("DeprCode "); }

                try { reader["DeprLifeToDate"].ToString(); }
                catch { missing.Append("DeprLifeToDate "); }

                try { reader["DeprPercent"].ToString(); }
                catch { missing.Append("DeprPercent "); }

                try { reader["DeprYearToDate"].ToString(); }
                catch { missing.Append("DeprYearToDate "); }

                try { reader["ElectricalParent"].ToString(); }
                catch { missing.Append("ElectricalParent "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["InstallDate"].ToString(); }
                catch { missing.Append("InstallDate "); }

                try { reader["Labor_AccountID"].ToString(); }
                catch { missing.Append("Labor_AccountID "); }

                try { reader["LifeinMonths"].ToString(); }
                catch { missing.Append("LifeinMonths "); }

                try { reader["LifeinYears"].ToString(); }
                catch { missing.Append("LifeinYears "); }

                try { reader["Location"].ToString(); }
                catch { missing.Append("Location "); }

                try { reader["LocationID"].ToString(); }
                catch { missing.Append("LocationID "); }

                try { reader["Maint_VendorID"].ToString(); }
                catch { missing.Append("Maint_VendorID "); }

                try { reader["Maint_WarrantyDesc"].ToString(); }
                catch { missing.Append("Maint_WarrantyDesc "); }

                try { reader["Maint_WarrantyExpire"].ToString(); }
                catch { missing.Append("Maint_WarrantyExpire "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["Material_AccountID"].ToString(); }
                catch { missing.Append("Material_AccountID "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["NoCostRollUp"].ToString(); }
                catch { missing.Append("NoCostRollUp "); }

                try { reader["NoPartXRef"].ToString(); }
                catch { missing.Append("NoPartXRef "); }

                try { reader["Notes"].ToString(); }
                catch { missing.Append("Notes "); }

                try { reader["OriginalValue"].ToString(); }
                catch { missing.Append("OriginalValue "); }

                try { reader["OutofService"].ToString(); }
                catch { missing.Append("OutofService "); }

                try { reader["ParentID"].ToString(); }
                catch { missing.Append("ParentID "); }

                try { reader["PartID"].ToString(); }
                catch { missing.Append("PartID "); }

                try { reader["Purch_VendorID"].ToString(); }
                catch { missing.Append("Purch_VendorID "); }

                try { reader["Purch_WarrantyDesc"].ToString(); }
                catch { missing.Append("Purch_WarrantyDesc "); }

                try { reader["Purch_WarrantyExpire"].ToString(); }
                catch { missing.Append("Purch_WarrantyExpire "); }

                try { reader["RIMEClass"].ToString(); }
                catch { missing.Append("RIMEClass "); }

                try { reader["SalvageValue"].ToString(); }
                catch { missing.Append("SalvageValue "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["Size"].ToString(); }
                catch { missing.Append("Size "); }

                try { reader["SizeUnits"].ToString(); }
                catch { missing.Append("SizeUnits "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["UserDefined_Bit01"].ToString(); }
                catch { missing.Append("UserDefined_Bit01 "); }

                try { reader["UserDefined_Char01"].ToString(); }
                catch { missing.Append("UserDefined_Char01 "); }

                try { reader["UserDefined_Date01"].ToString(); }
                catch { missing.Append("UserDefined_Date01 "); }

                try { reader["UserDefined_Memo01"].ToString(); }
                catch { missing.Append("UserDefined_Memo01 "); }

                try { reader["UserDefined_Num01"].ToString(); }
                catch { missing.Append("UserDefined_Num01 "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["ParentIdClientLookupId"].ToString(); }
                catch { missing.Append("ParentIdClientLookupId "); }

                try { reader["ElectricalParentClientLookupId"].ToString(); }
                catch { missing.Append("ElectricalParentClientLookupId "); }

                try { reader["PartIdClientLookupId"].ToString(); }
                catch { missing.Append("PartIdClientLookupId "); }

                try { reader["MaintVendorIdClientLookupId"].ToString(); }
                catch { missing.Append("MaintVendorIdClientLookupId "); }

                try { reader["PurchVendorIdClientLookupId"].ToString(); }
                catch { missing.Append("PurchVendorIdClientLookupId "); }

                try { reader["LocationIdClientLookupId"].ToString(); }
                catch { missing.Append("LocationIdClientLookupId "); }

                try { reader["MaterialAccountClientLookupId"].ToString(); }
                catch { missing.Append("MaterialAccountClientLookupId "); }

                try { reader["LaborAccountClientLookupId"].ToString(); }
                catch { missing.Append("LaborAccountClientLookupId "); }

                try { reader["ProcessSystemDesc"].ToString(); }
                catch { missing.Append("ProcessSystemDesc"); }

                try { reader["Dept_Desc"].ToString(); }
                catch { missing.Append("Dept_Desc"); }

                try { reader["Line_Desc"].ToString(); }
                catch { missing.Append("Line_Desc"); }

                try { reader["System_Desc"].ToString(); }
                catch { missing.Append("System_Desc"); }

                try { reader["AssignedClientlookupid"].ToString(); }
                catch { missing.Append("AssignedClientlookupid "); }

                try { reader["AssignedAssetName"].ToString(); }
                catch { missing.Append("AssignedAssetName "); }
                
                try { reader["MaintVendorName"].ToString(); }
                catch { missing.Append("MaintVendorName "); }
                
                try { reader["PurchVendorName"].ToString(); }
                catch { missing.Append("PurchVendorName "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Equipment LoadFromDatabaseAllBySiteId_V2(SqlDataReader reader)
        {
            b_Equipment obj = new b_Equipment();
            int i = obj.LoadFromDatabase(reader);

            try
            {
                if (false == reader.IsDBNull(i))
                {
                    obj.LaborAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    obj.LaborAccountClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.Process = reader.GetString(i);
                }
                else
                {
                    obj.Process = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.System = reader.GetString(i);
                }
                else
                {
                    obj.System = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.LocationIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    obj.LocationIdClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.Area_Desc = reader.GetString(i);
                }
                else
                {
                    obj.Area_Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.AssetGroup1Desc = reader.GetString(i);
                }
                else
                {
                    obj.AssetGroup1Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.AssetGroup2Desc = reader.GetString(i);
                }
                else
                {
                    obj.AssetGroup2Desc = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    obj.AssetGroup3Desc = reader.GetString(i);
                }
                else
                {
                    obj.AssetGroup3Desc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.AssetGroup1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    obj.AssetGroup1ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.AssetGroup2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    obj.AssetGroup2ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    obj.AssetGroup3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    obj.AssetGroup3ClientLookupId = "";
                }
                i++;
                return obj;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["LaborAccountClientLookupId"].ToString(); }
                catch { missing.Append("LaborAccountClientLookupId "); }

                try { reader["Process"].ToString(); }
                catch { missing.Append("Process "); }

                try { reader["System"].ToString(); }
                catch { missing.Append("System "); }

                try { reader["LocationIdClientLookupId"].ToString(); }
                catch { missing.Append("LocationIdClientLookupId "); }

                try { reader["Area_Desc"].ToString(); }
                catch { missing.Append("Area_Desc "); }

                try { reader["AssetGroup1Desc"].ToString(); }
                catch { missing.Append("AssetGroup1Desc "); }

                try { reader["AssetGroup2Desc"].ToString(); }
                catch { missing.Append("AssetGroup2Desc "); }

                try { reader["AssetGroup3Desc"].ToString(); }
                catch { missing.Append("AssetGroup3Desc "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Equipment ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment eq = new b_Equipment();

            // Load the object from the database
            eq.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return eq;
        }

        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void LoadFromDatabaseforSearch(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                // Account ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    LaborAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    LaborAccountClientLookupId = "";
                }
                i++;
                //SOM 827

                if (false == reader.IsDBNull(i))
                {
                    Process = reader.GetString(i);
                }
                else
                {
                    Process = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    System = reader.GetString(i);
                }
                else
                {
                    System = "";
                }
                // SOM-805
                i++;
                // Location ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    LocationIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    LocationIdClientLookupId = "";
                }
                i++;
                // Area_Desc
                if (false == reader.IsDBNull(i))
                {
                    Area_Desc = reader.GetString(i);
                }
                else
                {
                    Area_Desc = "";
                }
                i++;
                // Dept_Desc
                if (false == reader.IsDBNull(i))
                {
                    Dept_Desc = reader.GetString(i);
                }
                else
                {
                    Dept_Desc = "";
                }
                i++;
                // Line_Desc
                if (false == reader.IsDBNull(i))
                {
                    Line_Desc = reader.GetString(i);
                }
                else
                {
                    Line_Desc = "";
                }
                i++;
                // System_Desc
                if (false == reader.IsDBNull(i))
                {
                    System_Desc = reader.GetString(i);
                }
                else
                {
                    System_Desc = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["DepartmentName"].ToString(); }
                catch { missing.Append("DepartmentName "); }

                try { reader["LaborAccountClientLookupId"].ToString(); }
                catch { missing.Append("LaborAccountClientLookupId "); }

                try { reader["Process"].ToString(); }
                catch { missing.Append("Process "); }

                try { reader["System"].ToString(); }
                catch { missing.Append("System"); }

                // SOM-805
                try { reader["LocationIdClientLookupId"].ToString(); }
                catch { missing.Append("LocationIdClientLookupId "); }

                try { reader["Area_Desc"].ToString(); }
                catch { missing.Append("Area_Desc "); }

                try { reader["Dept_Desc"].ToString(); }
                catch { missing.Append("Dept_Desc "); }

                try { reader["Line_Desc"].ToString(); }
                catch { missing.Append("Line_Desc "); }

                try { reader["System_Desc"].ToString(); }
                catch { missing.Append("System_Desc "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }


        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void ValidateEquipmentIsParentOfAnother(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           bool createMode,
           System.Data.DataTable lulist,
           ref List<b_StoredProcValidationError> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_ValidateEquipmentIsParentOfAnother.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, createMode, lulist);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        public void ValidateByClientLookupIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            bool createMode,
            System.Data.DataTable lulist,
            ref List<b_StoredProcValidationError> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, createMode, lulist);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void ValidateByInactivateorActivate(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_ValidateByInactivateorActivate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        /// <summary>
        /// Update the Equipment table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateByForeignKeysInDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void UpdateByForeignKeysInDatabase_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_UpdateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public void EquipmentUpdateFORFuelTrackingInDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_UpdateforFueltracking_v2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void UpdateForPlantLocationInDatabase(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName
     )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_UpdateForPlantLocation.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void UpdateForVoidbyFleetMeterandEquipmentIdInDatabase(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_FleetMeter_VoidbyFleetMeterandEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }


        public void UpdateEquipmentForVoidFromFuelTrackingInDatabase(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_FuelTracking_UpdateEquipmentForVoid_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }


        public void UpdateForFleetMeterInDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName
   )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_UpdateforFleetMeter_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void RetrieveByClientLookupIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment>(reader => { this.LoadFromDatabase(reader); return this; });
                StoredProcedure.usp_Equipment_RetrieveByClientLookupId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveByForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                StoredProcedure.usp_Equipment_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void RetrieveByForeignKeysFromDatabase_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment>(reader => { this.LoadFromDatabaseByPKForeignKey_V2(reader); return this; });
                StoredProcedure.usp_Equipment_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveAllBySiteId_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_Equipment> temp
      )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
              

                temp = StoredProcedure.usp_Equipment_RetrieveAllBySiteID_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);


            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void InsertByForeignKeysIntoDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_CreateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public void InsertByForeignKeysIntoDatabase_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_CreateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment> results
        )
        {

            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = StoredProcedure.usp_Equipment_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveByLocationIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_RetrieveByLocationId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveByEquipmentIdandFuelTrackingIdFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment>(reader => { this.LoadFromDatabaseforRetrieveByEquipmentIdandFuelTrackingId(reader); return this; });
                StoredProcedure.usp_FuelTracking_RetrieveByEquipmentIdandFuelTrackingId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public int LoadFromDatabaseforRetrieveByEquipmentIdandFuelTrackingId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                //  FuelTrackingId column, bigint, not null
                FuelTrackingId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    FuelType = reader.GetString(i);

                }
                else
                {
                    FuelType = "";
                }
                i++;
                //  FuelTrackingId column, Decimal, not null
                FuelAmount = reader.GetDecimal(i++);


                //  UnitCost column, Decimal, not null
                UnitCost = reader.GetDecimal(i++);


                // ReadingDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    ReadingDate = DateTime.MinValue;
                }
                i++;
                // FTModifyBy column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    FTModifyBy = reader.GetString(i);
                }
                else
                {
                    FTModifyBy = "";
                }
                i++;

                // FTModifyDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    FTModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    FTModifyDate = DateTime.MinValue;
                }
                i++;

                //FuelUnit column, nvarchar,not null
                if (false == reader.IsDBNull(i))
                {
                    FuelUnit = reader.GetString(i);
                }
                else
                {
                    FuelUnit = "";
                }
                i++;

                //FleetMeterReadingId ,bigint, not null
                FleetMeterReadingId = reader.GetInt64(i++);

                //  Reading column, Decimal, not null
                Reading = reader.GetDecimal(i++);

                // FMRReadingDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    FMRReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    FMRReadingDate = DateTime.MinValue;
                }
                i++;

                //  Void column, Void, not null
                Void = reader.GetBoolean(i++);

                // FMModifyBy column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    FMModifyBy = reader.GetString(i);
                }
                else
                {
                    FMModifyBy = "";
                }
                i++;

                // FMModifyDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    FMModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    FMModifyDate = DateTime.MinValue;
                }
                i++;

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                // ClientLookupId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;
                // Meter1CurrentReading column, decimal(9,1), not null
                Meter1CurrentReading = reader.GetDecimal(i++);


                // Meter1CurrentReadingDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Meter1CurrentReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    Meter1CurrentReadingDate = DateTime.MinValue;
                }
                i++;

                // AssetModifyBy column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetModifyBy = reader.GetString(i);
                }
                else
                {
                    AssetModifyBy = "";

                }
                i++;

                // AssetModifyDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    AssetModifyDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter1Units = reader.GetString(i);
                }
                else
                {
                    Meter1Units = "";

                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }


        public static b_Equipment ProcessRowForusp_Equipment_GetAll(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return obj;
        }
        public static b_Equipment ProcessRowForusp_Equipment_GetAllFreeChildren(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseGetAllFreeChildren(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseGetAllFreeChildren(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);
                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);
                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);
                // SerialNumber column, nvarchar(63), not null
                SerialNumber = reader.GetString(i++);
                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);
                // Make column, nvarchar(31), not null
                Make = reader.GetString(i++);

                // Model column, nvarchar(63), not null
                Model = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        public void GetAllEquipment(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            int ClientId,
            int SiteId,
            ref List<b_Equipment> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_GetAll_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SiteId);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void DeleteEquipment(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName, ref bool retValue
        )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_Delete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, ref retValue);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        //-----SOM-774-----//
        public void ChangeClientLookupId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_ChangeClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        //-----SOM-784-----//

        public void LoadFromDatabaseCreateModifyDate(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                // Create By
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

                // Modify By
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;

                // Modify Date 
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    ModifyDate = DateTime.Now;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }
        public void RetrieveCreateModifyDate(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new SqlClient.ProcessRow<b_Equipment>(reader => { this.LoadFromDatabaseCreateModifyDate(reader); return this; });
                StoredProcedure.usp_Equipment_RetrieveCreateModifyDate.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        //-----SOM -899----------------
        public void ValidateByProcessSystem(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_ValidateByChildProcessSystem.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        //---------SOM-893-----------------------------
        public void GetAllEquipmentChildren(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_GetAllChildren_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void GetAllEquipmentFreeChildren(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_GetAllFreeChildren_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void ValidateClientLookupId(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_StoredProcValidationError> data
      )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_ValidateClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void ValidateForeignField(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_StoredProcValidationError> data
     )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_ValidateForeignField.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void Department_ValidateIfUsedInEquipment(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Department_ValidateIfUsedInEquipment_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void Line_ValidateIfUsedInEquipment(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Line_ValidateIfUsedInEquipment_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void SystemInfo_ValidateIfUsedInEquipment(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SystemInfo_ValidateIfUsedInEquipment_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        public void AssetGroup1_ValidateIfUsedInEquipment(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup1_ValidateIfUsedInEquipment_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void AssetGroup2_ValidateIfUsedInEquipment(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_ValidateIfUsedInEquipment_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void AssetGroup3_ValidateIfUsedInEquipment(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup3_ValidateIfUsedInEquipment_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        /// <summary>
        /// Retrieve all Equipment table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Equipment[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Equipment[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Equipment[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment>(reader => { b_Equipment obj = new b_Equipment(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Equipment_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Equipment[])results.ToArray(typeof(b_Equipment));
                }
                else
                {
                    data = new b_Equipment[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void UpdateInBulk(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_Equipment_UpdateBulk.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void GetAllDeptLineSys(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_Equipment> data
     )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_GetAllDeptLineSys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_Equipment ProcessRowForEquipment_GetAllDeptLineSys(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseforGetAllDeptLineSys(reader);

            // Return result
            return obj;
        }

        public void LoadFromDatabaseforGetAllDeptLineSys(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;
                //InactiveFlag column,GetBoolean,not null
                InactiveFlag = reader.GetBoolean(i++);

                //Name column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                // ParentId column, bigint, not null
                ParentId = reader.GetInt64(i++);

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                //Status column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;

                //Type column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;

                // ProcessSystemId column, bigint, not null
                ProcessSystemId = reader.GetInt64(i++);

                // PlantLocationId column, bigint, not null
                PlantLocationId = reader.GetInt64(i++);

                // EquipmentMasterId column, bigint, not null
                EquipmentMasterId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // LineId column, bigint, not null
                LineId = reader.GetInt64(i++);

                // SystemInfoId column, bigint, not null
                SystemInfoId = reader.GetInt64(i++);

                //DeptClientLookUpId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    DeptClientLookUpId = reader.GetString(i);
                }
                else
                {
                    DeptClientLookUpId = "";
                }
                i++;
                //DeptDesc column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Dept_Desc = reader.GetString(i);
                }
                else
                {
                    Dept_Desc = "";
                }
                i++;
                //LineClientLookUpId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    LineClientLookUpId = reader.GetString(i);
                }
                else
                {
                    LineClientLookUpId = "";
                }
                i++;
                //LineDesc column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Line_Desc = reader.GetString(i);
                }
                else
                {
                    Line_Desc = "";
                }
                i++;
                //SysClientLookUpId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    SysClientLookUpId = reader.GetString(i);
                }
                else
                {
                    SysClientLookUpId = "";
                }
                i++;
                //SysDesc column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    System_Desc = reader.GetString(i);
                }
                else
                {
                    System_Desc = "";
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["ProcessSystemId"].ToString(); }
                catch { missing.Append("ProcessSystemId "); }

                try { reader["PlantLocationId"].ToString(); }
                catch { missing.Append("PlantLocationId "); }

                try { reader["EquipmentMasterId"].ToString(); }
                catch { missing.Append("EquipmentMasterId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["LineId"].ToString(); }
                catch { missing.Append("LineId "); }

                try { reader["SystemInfoId"].ToString(); }
                catch { missing.Append("SystemInfoId "); }

                try { reader["DeptClientLookUpId"].ToString(); }
                catch { missing.Append("DeptClientLookUpId "); }

                try { reader["DeptDesc"].ToString(); }
                catch { missing.Append("DeptDesc "); }

                try { reader["LineClientLookUpId"].ToString(); }
                catch { missing.Append("LineClientLookUpId "); }

                try { reader["LineDesc"].ToString(); }
                catch { missing.Append("LineDesc "); }

                try { reader["SysClientLookUpId"].ToString(); }
                catch { missing.Append("SysClientLookUpId "); }

                try { reader["SysDesc"].ToString(); }
                catch { missing.Append("SysDesc "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }


        public void RetrieveForMentionAlert(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_Equipment> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment>(reader => { this.LoadFromDatabaseforMentionAlert(reader); return this; });
                StoredProcedure.usp_Equipment_RetrieveForMentionAlert.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void LoadFromDatabaseforMentionAlert(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Create By
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

                // Modify By
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;

                // Modify Date 
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    ModifyDate = DateTime.Now;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }


        public void RetrieveChunkSearchV2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref b_Equipment results
    )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Equipment_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void RetrieveFleetAssetChunkSearchV2(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref b_Equipment results
   )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_FleetAsset_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #region V2-1213
        public void RetrieveAllChildrenChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Equipment results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Equipment_GetAllChildrenChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion
        public void RetrieveFleetFuelChunkSearchV2(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref b_Equipment results
  )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_FuelTracking_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_Equipment ProcessRetrieveForChunkV2(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForEquipmentChunkSearchV2(reader);
            return Equipment;
        }
        public static b_Equipment ProcessRetrieveForFleetAssetChunkV2(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForFleetAssetChunkSearchV2(reader);
            return Equipment;
        }
        #region V2-1213
        public static b_Equipment ProcessRetrieveForChildEquipmentChunkSearchV2(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForChildEquipmentChunkSearchV2(reader);
            return Equipment;
        }
        #endregion
        public static b_Equipment ProcessRetrieveForFleetFuelChunkV2(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForFleetFuelChunkSearchV2(reader);
            return Equipment;
        }
        public int LoadFromDatabaseForEquipmentChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // AreaId Id
                AreaId = reader.GetInt64(i++);

                //// AreaId Id
                //AreaId = reader.GetInt64(i++);
                AssetGroup1 = reader.GetInt64(i++);

                AssetGroup2 = reader.GetInt64(i++);

                AssetGroup3 = reader.GetInt64(i++);

                // DepartmentId column, nvarchar(31), not null
                DepartmentId = reader.GetInt64(i++);

                // StoreRoomId column, nvarchar(31), not null

                StoreRoomId = reader.GetInt64(i++);

                //  ClientLookupId
                ClientLookupId = reader.GetString(i++);

                //  AcquiredCost
                AcquiredCost = reader.GetDecimal(i++);

                //  AcquiredDate
                if (false == reader.IsDBNull(i))
                {
                    AcquiredDate = reader.GetDateTime(i);
                }
                else
                {
                    // AcquiredDate should never be null
                    AcquiredDate = DateTime.Now;
                }
                i++;

                // BIMIdentifier column, uniqueidentifier, not null
                if (false == reader.IsDBNull(i))
                {
                    BIMIdentifier = reader.GetGuid(i);
                }
                else
                {
                    BIMIdentifier = Guid.Empty;
                }
                i++;

                // BookValue 

                BookValue = reader.GetDecimal(i++);

                // BusinessGroup
                if (false == reader.IsDBNull(i))
                {
                    BusinessGroup = reader.GetString(i);
                }
                else
                {
                    BusinessGroup = "";
                }
                i++;
                // CatalogNumber
                if (false == reader.IsDBNull(i))
                {
                    CatalogNumber = reader.GetString(i);
                }
                else
                {
                    CatalogNumber = "";
                }
                i++;
                // Category 
                if (false == reader.IsDBNull(i))
                {
                    Category = reader.GetString(i);
                }
                else
                {
                    Category = "";
                }
                i++;

                // CostCenter 
                if (false == reader.IsDBNull(i))
                {
                    CostCenter = reader.GetString(i);
                }
                else
                {
                    CostCenter = "";
                }
                i++;

                // DeprCode 
                if (false == reader.IsDBNull(i))
                {
                    DeprCode = reader.GetString(i);
                }
                else
                {
                    DeprCode = "";
                }
                i++;

                DeprLifeToDate = reader.GetDecimal(i++);

                DeprPercent = reader.GetDecimal(i++);

                DeprYearToDate = reader.GetDecimal(i++);

                ElectricalParent = reader.GetInt64(i++);

                InactiveFlag = reader.GetBoolean(i++);

                CriticalFlag = reader.GetBoolean(i++);

                // InstallDate 
                if (false == reader.IsDBNull(i))
                {
                    InstallDate = reader.GetDateTime(i);
                }
                else
                {
                    // InstallDate should never be null
                    InstallDate = DateTime.Now;
                }
                i++;

                Labor_AccountId = reader.GetInt64(i++);

                LifeinMonths = reader.GetInt32(i++);

                LifeinYears = reader.GetInt32(i++);

                // Location
                if (false == reader.IsDBNull(i))
                {
                    Location = reader.GetString(i);
                }
                else
                {
                    Location = "";
                }
                i++;
                LocationId = reader.GetInt64(i++);

                Maint_VendorId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Maint_WarrantyDesc = reader.GetString(i);
                }
                else
                {
                    Maint_WarrantyDesc = "";
                }
                i++;

                // Maint_WarrantyExpire 
                if (false == reader.IsDBNull(i))
                {
                    Maint_WarrantyExpire = reader.GetDateTime(i);
                }
                else
                {
                    // Maint_WarrantyExpire should never be null
                    Maint_WarrantyExpire = DateTime.Now;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;

                Material_AccountId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                NoCostRollUp = reader.GetBoolean(i++);

                NoPartXRef = reader.GetBoolean(i++);

                OriginalValue = reader.GetDecimal(i++);

                // OutofService 
                if (false == reader.IsDBNull(i))
                {
                    OutofService = reader.GetDateTime(i);
                }
                else
                {
                    // OutofService should never be null
                    OutofService = DateTime.Now;
                }
                i++;

                ParentID = reader.GetInt64(i++);

                PartID = reader.GetInt64(i++);

                Purch_VendorId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Purch_WarrantyDesc = reader.GetString(i);
                }
                else
                {
                    Purch_WarrantyDesc = "";
                }
                i++;


                // Purch_WarrantyExpire 
                if (false == reader.IsDBNull(i))
                {
                    Purch_WarrantyExpire = reader.GetDateTime(i);
                }
                else
                {
                    // Purch_WarrantyExpire should never be null
                    Purch_WarrantyExpire = DateTime.Now;
                }
                i++;

                RIMEClass = reader.GetInt32(i++);

                SalvageValue = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    SerialNumber = reader.GetString(i);
                }
                else
                {
                    SerialNumber = "";
                }
                i++;

                Size = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    SizeUnits = reader.GetString(i);
                }
                else
                {
                    SizeUnits = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetNumber = reader.GetString(i);
                }
                else
                {
                    AssetNumber = "";
                }
                i++;

                ProcessSystemId = reader.GetInt64(i++);
                // RemoveFromService column, bit, not null      V2-636
                RemoveFromService = reader.GetBoolean(i++);

                // RemoveFromServiceDate column, datetime2, not null   V2-636
                if (false == reader.IsDBNull(i))
                {
                    RemoveFromServiceDate = reader.GetDateTime(i);
                }
                else
                {
                    RemoveFromServiceDate = DateTime.MinValue;
                }

                i++;
                UpdateIndex = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    LaborAccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    LaborAccountClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Process = reader.GetString(i);
                }
                else
                {
                    Process = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    System = reader.GetString(i);
                }
                else
                {
                    System = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LocationIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    LocationIdClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Area_Desc = reader.GetString(i);
                }
                else
                {
                    Area_Desc = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup3ClientLookupId = "";
                }
                i++;

                AssetGroup1Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                AssetGroup2Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                AssetGroup3Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->

                PlantLocationId = reader.GetInt64(i++);
                EquipmentMasterId = reader.GetInt64(i++);
                LineId = reader.GetInt64(i++);
                SystemInfoId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssetCategory = reader.GetString(i);
                }
                else
                {
                    AssetCategory = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SubType = reader.GetString(i);
                }
                else
                {
                    SubType = "";
                }
                i++;

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SiteID"].ToString(); }
                catch { missing.Append("SiteID "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AcquiredCost"].ToString(); }
                catch { missing.Append("AcquiredCost "); }

                try { reader["AcquiredDate"].ToString(); }
                catch { missing.Append("AcquiredDate "); }

                try { reader["BIMIdentifier"].ToString(); }
                catch { missing.Append("BIMIdentifier "); }

                try { reader["BookValue"].ToString(); }
                catch { missing.Append("BookValue "); }


                try { reader["BusinessGroup"].ToString(); }
                catch { missing.Append("BusinessGroup "); }

                try { reader["CatalogNumber"].ToString(); }
                catch { missing.Append("CatalogNumber "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["CostCenter"].ToString(); }
                catch { missing.Append("CostCenter "); }

                try { reader["DeprCode"].ToString(); }
                catch { missing.Append("DeprCode "); }

                try { reader["DeprLifeToDate"].ToString(); }
                catch { missing.Append("DeprLifeToDate "); }

                try { reader["DeprPercent"].ToString(); }
                catch { missing.Append("DeprPercent "); }

                try { reader["DeprYearToDate"].ToString(); }
                catch { missing.Append("DeprYearToDate "); }
                //V2-271
                try { reader["ElectricalParent"].ToString(); }
                catch { missing.Append("ElectricalParent "); }

                try { reader[" InactiveFlag"].ToString(); }
                catch { missing.Append(" InactiveFlag "); }

                try { reader[" CriticalFlag"].ToString(); }
                catch { missing.Append(" CriticalFlag "); }

                try { reader[" InstallDate"].ToString(); }
                catch { missing.Append(" InstallDate "); }

                try { reader[" Labor_AccountId"].ToString(); }
                catch { missing.Append(" Labor_AccountId "); }

                try { reader[" LifeinMonths"].ToString(); }
                catch { missing.Append(" LifeinMonths "); }

                try { reader[" LifeinYears"].ToString(); }
                catch { missing.Append(" LifeinYears "); }

                try { reader[" Location"].ToString(); }
                catch { missing.Append(" Location "); }

                try { reader[" LocationId"].ToString(); }
                catch { missing.Append(" LocationId "); }

                try { reader[" Maint_VendorId"].ToString(); }
                catch { missing.Append(" Maint_VendorId "); }

                try { reader[" Maint_WarrantyDesc"].ToString(); }
                catch { missing.Append(" Maint_WarrantyDesc "); }

                try { reader[" Maint_WarrantyExpire"].ToString(); }
                catch { missing.Append(" Maint_WarrantyExpire "); }

                try { reader[" Make"].ToString(); }
                catch { missing.Append(" Make "); }

                try { reader[" Material_AccountId"].ToString(); }
                catch { missing.Append(" Material_AccountId "); }

                try { reader[" Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader[" Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader[" NoCostRollUp"].ToString(); }
                catch { missing.Append(" NoCostRollUp "); }

                try { reader[" NoPartXRef"].ToString(); }
                catch { missing.Append(" NoPartXRef "); }

                try { reader[" OriginalValue"].ToString(); }
                catch { missing.Append(" OriginalValue "); }

                try { reader[" OutofService"].ToString(); }
                catch { missing.Append(" OutofService "); }

                try { reader[" ParentID"].ToString(); }
                catch { missing.Append(" ParentID "); }

                try { reader[" PartID"].ToString(); }
                catch { missing.Append(" PartID "); }

                try { reader[" Purch_VendorId"].ToString(); }
                catch { missing.Append(" Purch_VendorId "); }

                try { reader[" Purch_WarrantyDesc"].ToString(); }
                catch { missing.Append(" Purch_WarrantyDesc "); }

                try { reader[" Purch_WarrantyExpire"].ToString(); }
                catch { missing.Append(" Purch_WarrantyExpire "); }

                try { reader[" RIMEClass"].ToString(); }
                catch { missing.Append(" RIMEClass "); }

                try { reader[" SalvageValue"].ToString(); }
                catch { missing.Append(" SalvageValue "); }

                try { reader[" SerialNumber"].ToString(); }
                catch { missing.Append(" SerialNumber "); }

                try { reader[" Size"].ToString(); }
                catch { missing.Append(" Size "); }

                try { reader[" SizeUnits"].ToString(); }
                catch { missing.Append(" SizeUnits "); }

                try { reader[" Status"].ToString(); }
                catch { missing.Append(" Status "); }

                try { reader[" Type"].ToString(); }
                catch { missing.Append(" Type "); }

                try { reader[" AssetNumber"].ToString(); }
                catch { missing.Append(" AssetNumber "); }

                try { reader[" ProcessSystemId"].ToString(); }
                catch { missing.Append(" ProcessSystemId "); }
                //V2-636
                try { reader[" RemoveFromService"].ToString(); }
                catch { missing.Append(" RemoveFromService "); }
                //V2-636
                try { reader[" RemoveFromServiceDate"].ToString(); }
                catch { missing.Append(" RemoveFromServiceDate "); }

                try { reader[" UpdateIndex"].ToString(); }
                catch { missing.Append(" UpdateIndex "); }

                try { reader[" LaborAccountClientLookupId"].ToString(); }
                catch { missing.Append(" LaborAccountClientLookupId "); }

                try { reader[" Process"].ToString(); }
                catch { missing.Append(" Process "); }

                try { reader[" System"].ToString(); }
                catch { missing.Append(" System "); }

                try { reader[" LocationIdClientLookupId"].ToString(); }
                catch { missing.Append(" LocationIdClientLookupId "); }

                try { reader[" Area_Desc"].ToString(); }
                catch { missing.Append(" Area_Desc "); }

                try { reader[" Dept_Desc"].ToString(); }
                catch { missing.Append(" Dept_Desc "); }

                try { reader[" Line_Desc"].ToString(); }
                catch { missing.Append(" Line_Desc "); }

                try { reader[" System_Desc"].ToString(); }
                catch { missing.Append(" System_Desc "); }

                try { reader[" PlantLocationId"].ToString(); }
                catch { missing.Append(" PlantLocationId "); }

                try { reader[" EquipmentMasterId"].ToString(); }
                catch { missing.Append(" EquipmentMasterId "); }

                try { reader[" LineId"].ToString(); }
                catch { missing.Append(" LineId "); }

                try { reader[" SystemInfoId"].ToString(); }
                catch { missing.Append(" SystemInfoId "); }

                try { reader[" AssetCategory"].ToString(); }
                catch { missing.Append(" AssetCategory "); }

                try { reader[" SubType"].ToString(); }
                catch { missing.Append(" SubType "); }

                try { reader[" TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }
                //<!--(Added on 25/06/2020)-->
                try { reader["AssetGroup1Id"].ToString(); }
                catch { missing.Append("AssetGroup1Id "); }

                try { reader["AssetGroup2Id"].ToString(); }
                catch { missing.Append("AssetGroup2Id "); }

                try { reader["AssetGroup3Id"].ToString(); }
                catch { missing.Append("AssetGroup3Id "); }
                //<!--(Added on 25/06/2020)-->


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            return i;
        }

        public int LoadFromDatabaseForFleetAssetChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //  ClientLookupId
                ClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetCategory = reader.GetString(i);
                }
                else
                {
                    AssetCategory = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VIN = reader.GetString(i);
                }
                else
                {
                    VIN = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VehicleType = reader.GetString(i);
                }
                else
                {
                    VehicleType = "";
                }
                i++;
                VehicleYear = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    License = reader.GetString(i);
                }
                else
                {
                    License = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    RegistrationLoc = reader.GetString(i);
                }
                else
                {
                    RegistrationLoc = "";
                }
                i++;
                // RemoveFromService column, bit, not null
                RemoveFromService = reader.GetBoolean(i++);

                // RemoveFromServiceDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    RemoveFromServiceDate = reader.GetDateTime(i);
                }
                else
                {
                    RemoveFromServiceDate = DateTime.MinValue;
                }
                i++;
                UpdateIndex = reader.GetInt32(i++);

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader[" Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader[" AssetCategory"].ToString(); }
                catch { missing.Append(" AssetCategory "); }

                try { reader[" VIN"].ToString(); }
                catch { missing.Append(" VIN "); }

                try { reader[" VehicleType"].ToString(); }
                catch { missing.Append(" VehicleType "); }

                try { reader[" VehicleYear"].ToString(); }
                catch { missing.Append(" VehicleYear "); }

                try { reader[" Make"].ToString(); }
                catch { missing.Append(" Make "); }

                try { reader[" Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader[" License"].ToString(); }
                catch { missing.Append(" License "); }

                try { reader[" RegistrationLoc"].ToString(); }
                catch { missing.Append(" RegistrationLoc "); }

                try { reader[" RemoveFromService"].ToString(); }
                catch { missing.Append(" RemoveFromService "); }

                try { reader[" RemoveFromServiceDate"].ToString(); }
                catch { missing.Append(" RemoveFromServiceDate "); }

                try { reader[" UpdateIndex"].ToString(); }
                catch { missing.Append(" UpdateIndex "); }

                try { reader[" TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            return i;
        }

        #region V2-1213
        public int LoadFromDatabaseForChildEquipmentChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {            
            
                EquipmentId = reader.GetInt64(i++);                
                ClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SerialNumber = reader.GetString(i);
                }
                else
                {
                    SerialNumber = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;             

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }
            
                try { reader[" TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            return i;
        }
        #endregion


        public void RetrieveFleetMeterReadingChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Equipment results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_FleetMeterReading_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_Equipment ProcessRetrieveForFleetMeterReadingChunkV2(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForFleetMeterReadingChunkSearchV2(reader);
            return Equipment;
        }
        public int LoadFromDatabaseForFleetMeterReadingChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);


                //EquipmentImage
                if (false == reader.IsDBNull(i))
                {
                    EquipmentImage = reader.GetString(i);
                }
                else
                {
                    EquipmentImage = "";
                }
                i++;
                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                // FMRReadingDate should never be null

                if (false == reader.IsDBNull(i))
                {
                    FMRReadingDate = reader.GetDateTime(i);
                }
                else
                {

                    FMRReadingDate = DateTime.Now;
                }
                i++;

                // NoofDays column, bigint, not null
                NoofDays = reader.GetInt64(i++);

                //MeterReadingL1
                if (false == reader.IsDBNull(i))
                {
                    MeterReadingL1 = reader.GetString(i);
                }
                else
                {
                    MeterReadingL1 = "";
                }
                i++;

                //MeterReadingL2
                if (false == reader.IsDBNull(i))
                {
                    MeterReadingL2 = reader.GetString(i);
                }
                else
                {
                    MeterReadingL2 = "";
                }
                i++;
                // FleetMeterReadingId bigint not null
                FleetMeterReadingId = reader.GetInt64(i++);
                //SourceType
                if (false == reader.IsDBNull(i))
                {
                    SourceType = reader.GetString(i);
                }
                else
                {
                    SourceType = "";
                }
                i++;

                //Action
                if (false == reader.IsDBNull(i))
                {
                    Action = reader.GetString(i);
                }
                else
                {
                    Action = "";
                }
                i++;

                //Meter2Indicator
                Meter2Indicator = reader.GetBoolean(i++);

                //VIN
                if (false == reader.IsDBNull(i))
                {
                    VIN = reader.GetString(i);
                }
                else
                {
                    VIN = "";
                }
                i++;

                //Make
                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;

                //Model
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;
                SourceId = reader.GetInt64(i++);
                UpdateIndex = reader.GetInt32(i++);

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["EquipmentImage"].ToString(); }
                catch { missing.Append("EquipmentImage "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader["FMRReadingDate"].ToString(); }
                catch { missing.Append(" FMRReadingDate "); }

                try { reader["NoofDays"].ToString(); }
                catch { missing.Append(" NoofDays "); }

                try { reader["MeterReadingL1"].ToString(); }
                catch { missing.Append(" MeterReadingL1 "); }

                try { reader["MeterReadingL2"].ToString(); }
                catch { missing.Append(" MeterReadingL2 "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append(" SourceType "); }

                try { reader["Action"].ToString(); }
                catch { missing.Append(" Action "); }

                try { reader["VIN"].ToString(); }
                catch { missing.Append(" VIN "); }

                try { reader["Meter2Indicator"].ToString(); }
                catch { missing.Append(" Meter2Indicator "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append(" Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader["SourceId"].ToString(); }
                catch { missing.Append(" SourceId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("  UpdateIndex "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("  TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }

        public int LoadFromDatabaseForFleetFuelChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //  ClientLookupId
                ClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VIN = reader.GetString(i);
                }
                else
                {
                    VIN = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    EquipImage = reader.GetString(i);
                }
                else
                {
                    EquipImage = "";
                }
                i++;
                FuelTrackingId = reader.GetInt64(i++);

                Void = reader.GetBoolean(i++);
                FleetMeterReadingId = reader.GetInt64(i++);
                Reading = reader.GetDecimal(i);
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    ReadingDate = DateTime.MinValue;
                }
                i++;

                FuelAmount = reader.GetDecimal(i);
                i++;
                Meter1CurrentReading = reader.GetDecimal(i);
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FuelUnits = reader.GetString(i);
                }
                else
                {
                    FuelUnits = "";
                }
                i++;

                UnitCost = reader.GetDecimal(i);
                i++;
                TotalCost = reader.GetDecimal(i);
                i++;
                TotalCount = reader.GetInt32(i);
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append(" Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader["VIN"].ToString(); }
                catch { missing.Append(" VIN "); }

                try { reader["EquipImage"].ToString(); }
                catch { missing.Append(" EquipImage "); }

                try { reader["ReadingDate"].ToString(); }
                catch { missing.Append(" ReadingDate "); }

                try { reader["FuelAmount"].ToString(); }
                catch { missing.Append(" FuelAmount "); }

                try { reader["Meter1CurrentReading"].ToString(); }
                catch { missing.Append(" Meter1CurrentReading "); }

                try { reader["FuelUnit"].ToString(); }
                catch { missing.Append(" FuelUnit "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append(" UnitCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append(" TotalCost "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            return i;
        }

        public void ValidateFleetFuelMeter1CurrentReading(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_StoredProcValidationError> data
     )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_FuelTracking_ValidateMeterReading_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        public void ValidateMeterReadingForBothMeter(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_StoredProcValidationError> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_FleetMeter_ValidateMeterReadingForBothMeter_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }



        public void ValidiationForUnvoidforFleetMeterandFuelTracking(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_StoredProcValidationError> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_FleetMeterandFuelTracking_ValidiationForUnvoid_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        


        #region Furl Tracking Retrieve By Equipment Id
        public static b_Equipment ProcessRetrieveForFleetFuelByEquipmentIdV2(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForFleetFuelRetrieveByEquipmentIdV2(reader);
            return Equipment;
        }

        public int LoadFromDatabaseForFleetFuelRetrieveByEquipmentIdV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // FuelTrackingId column, bigint, not null
                FuelTrackingId = reader.GetInt64(i++);
                FuelAmount = reader.GetDecimal(i);
                i++;
                UnitCost = reader.GetDecimal(i);
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    ReadingDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FuelUnits = reader.GetString(i);
                }
                else
                {
                    FuelUnits = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Meter1Units = reader.GetString(i);
                }
                else
                {
                    Meter1Units = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Meter2Units = reader.GetString(i);
                }
                else
                {
                    Meter2Units = "";
                }
                i++;

                TotalCost = reader.GetDecimal(i);
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["FuelTrackingId"].ToString(); }
                catch { missing.Append("FuelTrackingId "); }

                try { reader["FuelAmount"].ToString(); }
                catch { missing.Append("FuelAmount "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append(" UnitCost "); }

                try { reader["ReadingDate"].ToString(); }
                catch { missing.Append(" ReadingDate "); }

                try { reader["FuelUnits"].ToString(); }
                catch { missing.Append(" FuelUnits "); }

                try { reader["Meter1Units"].ToString(); }
                catch { missing.Append(" Meter1Units "); }

                try { reader["Meter2Units"].ToString(); }
                catch { missing.Append(" Meter2Units "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append(" TotalCost "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            return i;
        }
        public void RetrieveFleetFuelByEquipmentIdV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Equipment> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_FuelTracking_RetrieveByEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion

        #region Meter Reading
        public void RetrieveFleetMeterReadingByEquipmentIdV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Equipment> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_FleetMeterReading_RetrieveByEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion

        #region Chunk Search Lookup list

        public void RetrieveEquipmentLookuplistChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Equipment> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Equipment_RetrieveChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        #region Retrieve Equipment Lookuplist ChunkSearch Mobile_V2
        public void RetrieveEquipmentLookuplistChunkSearchMobile_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Equipment> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Equipment_RetrieveChunkSearchLookupListMobile_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Equipment ProcessRowForChunkSearchLookupList_Mobile(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForLookupListChunkSearchV2_Mobile(reader);
            return Equipment;
        }
        public int LoadFromDatabaseForLookupListChunkSearchV2_Mobile(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }   

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("  TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }
        #endregion end Mobile_V2

        public static b_Equipment ProcessRowForChunkSearchLookupList(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForLookupListChunkSearchV2(reader);
            return Equipment;
        }
        public int LoadFromDatabaseForLookupListChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //Model
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup3ClientLookupId = "";
                }
                i++;



                //SerialNumber
                if (false == reader.IsDBNull(i))
                {
                    SerialNumber = reader.GetString(i);
                }
                else
                {
                    SerialNumber = "";
                }
                i++;

                //Type


                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append(" SerialNumber "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append(" Type "); }


                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("  TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }
        #endregion

        #region Get all Equipment V2
        public static b_Equipment ProcessRowForusp_Equipment_GetAllV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseForGetAllEquipment(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForGetAllEquipment(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //InactiveFlag column,GetBoolean,not null
                InactiveFlag = reader.GetBoolean(i++);

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //ParentId
                ParentId = reader.GetInt64(i++);

                //ProcessSystemId
                ProcessSystemId = reader.GetInt64(i++);

                //PlantLocationId
                PlantLocationId = reader.GetInt64(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["ProcessSystemId"].ToString(); }
                catch { missing.Append("ProcessSystemId "); }

                try { reader["PlantLocationId"].ToString(); }
                catch { missing.Append("PlantLocationId "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }

        #endregion

        #region Get All Equipment Children V2
        public static b_Equipment ProcessRowForusp_Equipment_GetAllChildrenV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseForGetAllEquipmentChildren(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForGetAllEquipmentChildren(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Make
                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;

                //Model
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //SerialNumber
                if (false == reader.IsDBNull(i))
                {
                    SerialNumber = reader.GetString(i);
                }
                else
                {
                    SerialNumber = "";
                }
                i++;

                //Type
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }



                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }
        #endregion

        #region Retrieve Equipment LookupList By Search Criteria V2
        public static object ProcessRowForRetrieveLookupListBySearchCriteriaV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveLookupListBySearchCriteria(reader);

            // Return result
            return (object)obj;
        }
        public int LoadFromDatabaseForRetrieveLookupListBySearchCriteria(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Name column, nvarchar(63), not null
                Name = reader.GetString(i++);

                // Model column, nvarchar(63), not null
                Model = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // SerialNumber column, nvarchar(63), not null
                SerialNumber = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
        #endregion

        #region FleetLookupListBySearchCriteria      
        public static object ProcessRowForEquipmentFleetLookupListV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseForEquipmentFleetLookupListV2(reader);

            // Return result
            return (object)obj;
        }


        public int LoadFromDatabaseForEquipmentFleetLookupListV2(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                EquipmentId = reader.GetInt64(i++);

                ClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VIN = reader.GetString(i);
                }
                else
                {
                    VIN = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = string.Empty;
                }
                i++;

                TotalCount = reader.GetInt32(i++);

                Meter1CurrentReading = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Meter1CurrentReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    Meter1CurrentReadingDate = DateTime.MinValue;
                }
                i++;

                Meter1Type = reader.GetString(i++);

                Meter1Units = reader.GetString(i++);

                Meter2CurrentReading = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Meter2CurrentReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    Meter2CurrentReadingDate = DateTime.MinValue;
                }
                i++;

                Meter2Type = reader.GetString(i++);

                Meter2Units = reader.GetString(i++);

                FuelUnits = reader.GetString(i++);
            }
            catch (Exception ex)
            // Diagnostics
            {
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["VIN"].ToString(); }
                catch { missing.Append("VIN "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["FuelUnits"].ToString(); }
                catch { missing.Append("FuelUnits "); }

                try { reader["Meter1CurrentReading"].ToString(); }
                catch { missing.Append("Meter1CurrentReading "); }

                try { reader["Meter1CurrentReadingDate"].ToString(); }
                catch { missing.Append("Meter1CurrentReadingDate "); }

                try { reader["Meter1Type"].ToString(); }
                catch { missing.Append("Meter1Type "); }

                try { reader["Meter1Units"].ToString(); }
                catch { missing.Append("Meter1Units "); }

                try { reader["Meter2CurrentReading"].ToString(); }
                catch { missing.Append("Meter2CurrentReading "); }

                try { reader["Meter2CurrentReadingDate"].ToString(); }
                catch { missing.Append("Meter2CurrentReadingDate "); }

                try { reader["Meter2Type"].ToString(); }
                catch { missing.Append("Meter2Type "); }

                try { reader["Meter2Units"].ToString(); }
                catch { missing.Append("Meter2Units "); }

            }
            return i;

        }
        #endregion
        #region load EquipmentId By ClientIdLookup V2-625
        public void LoadFromDatabaseForEquipmentIdByClientIdLookupV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public static b_Equipment ProcessRowForEquipmentIdByClientIdLookupV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment equipment = new b_Equipment();

            // Load the object from the database
            equipment.LoadFromDatabaseForEquipmentIdByClientIdLookupV2(reader);

            // Return result
            return equipment;
        }
        public void RetrieveEquipmentIdByClientLookupIdV2FromDatabase(
                 SqlConnection connection,
                 SqlTransaction transaction,
                 long callerUserInfoId,
string callerUserName,
                 ref b_Equipment result
             )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                result = Database.StoredProcedure.usp_Equipment_RetrieveByClientLookUpId_V2.CallStoredProcedure(command, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                ClientLookupId = String.Empty;
            }
        }
        #endregion

        #region Validate by scrap
        public void ValidateByScrap(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_ValidateByScrap_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        #endregion

        #region V2-637 Repairable Spare Asset
        public void RetrieveEquipmentLookuplistForRepSpareAssetChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Equipment> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Equipment_RetrieveChunkSearchLookupListForRepSpareAsset_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Equipment ProcessRowForChunkSearchLookupListForRepSpareAsset(SqlDataReader reader)
        {
            b_Equipment Equipment = new b_Equipment();

            Equipment.LoadFromDatabaseForRepSpareAssetLookupListChunkSearchV2(reader);
            return Equipment;
        }
        public int LoadFromDatabaseForRepSpareAssetLookupListChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //Make
                if (false == reader.IsDBNull(i))
                {
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;

                //Model
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;

                //Type
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append(" Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append(" Type "); }


                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("  TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }
         
        public void AddRepairableSpareAssetWizard(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_Equipment_CreateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                // V2-1043 - Add this back 
                if (RepairableSpareLog!=null)
                {
                    RepairableSpareLog.EquipmentId = EquipmentId;
                }
                StoredProcedure.usp_RepairableSpareLog_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, RepairableSpareLog);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        #endregion

        #region V2-835 
        public void RetrieveChunkSearchMobile_V2(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref b_Equipment results
 )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Equipment_RetrieveChunkSearchMobile_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion

        #region V2-948
        public void LoadFromDatabaseRetrieveForLaborAccountV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);
                Labor_AccountId = reader.GetInt64(i++);
                LaborAccountClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["Labor_AccountId"].ToString(); }
                catch { missing.Append("Labor_AccountId "); }

                try { reader["LaborAccountClientLookupId"].ToString(); }
                catch { missing.Append("LaborAccountClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public static b_Equipment ProcessRowRetrieveForLaborAccountV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment equipment = new b_Equipment();

            // Load the object from the database
            equipment.LoadFromDatabaseRetrieveForLaborAccountV2(reader);

            // Return result
            return equipment;
        }
        public void RetrieveAccountByEquipmentIdV2FromDatabase(
                 SqlConnection connection,
                 SqlTransaction transaction,
                 long callerUserInfoId,
string callerUserName,
                 ref b_Equipment result
             )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                result = Database.StoredProcedure.usp_Equipment_RetrieveForLaborAccount_V2.CallStoredProcedure(command, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                ClientLookupId = String.Empty;
            }
        }
        #endregion

        #region V2-846 Equipment Tree Grid

        public void GetAllEquipmentParent(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_GetAllParent_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Equipment ProcessRowForusp_Equipment_GetAllParentV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseForGetAllEquipmentForParentV2(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForGetAllEquipmentForParentV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //InactiveFlag column,GetBoolean,not null
                InactiveFlag = reader.GetBoolean(i++);

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //ParentId
                ParentId = reader.GetInt64(i++);

                //ProcessSystemId
                ProcessSystemId = reader.GetInt64(i++);

                //PlantLocationId
                PlantLocationId = reader.GetInt64(i++);

                ChildCount = reader.GetInt32(i++);

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["ProcessSystemId"].ToString(); }
                catch { missing.Append("ProcessSystemId "); }

                try { reader["PlantLocationId"].ToString(); }
                catch { missing.Append("PlantLocationId "); }
                
                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }
                
                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }
        public void GetAllEquipmentChildrenForParent(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment> results = null;
            data = new List<b_Equipment>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_Equipment_GetAllChildrenForParent_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Equipment ProcessRowForusp_Equipment_GetAllChildrenForParentV2(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment obj = new b_Equipment();

            // Load the object from the database
            obj.LoadFromDatabaseForGetAllEquipmentForChildrenV2(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForGetAllEquipmentForChildrenV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //InactiveFlag column,GetBoolean,not null
                InactiveFlag = reader.GetBoolean(i++);

                //Name
                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                //ParentId
                ParentId = reader.GetInt64(i++);

                //ProcessSystemId
                ProcessSystemId = reader.GetInt64(i++);

                //PlantLocationId
                PlantLocationId = reader.GetInt64(i++);

                ChildCount = reader.GetInt32(i++);

                //TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ParentId"].ToString(); }
                catch { missing.Append("ParentId "); }

                try { reader["ProcessSystemId"].ToString(); }
                catch { missing.Append("ProcessSystemId "); }

                try { reader["PlantLocationId"].ToString(); }
                catch { missing.Append("PlantLocationId "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

                //try { reader["TotalCount"].ToString(); }
                //catch { missing.Append("TotalCount "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

            return i;
        }
        #endregion
    }
}
