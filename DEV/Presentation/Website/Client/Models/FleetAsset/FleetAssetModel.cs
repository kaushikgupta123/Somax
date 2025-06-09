using Client.Common;
using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetAsset
{
    public class FleetAssetModel
    {
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "EquipmentIDErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [StringLength(31, ErrorMessage = "EquipmentStrLenErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression("^[A-Z0-9\\%\\-\\:\\/\\$\\*\\+\\.]+$", ErrorMessage = "EquipmentIDRegErrMsg|" + LocalizeResourceSetConstants.EquipmentDetails)]
        //[Remote("CheckExistingId", "Equipment", HttpMethod = "POST", ErrorMessage = "{0}")] //For Checking existing email with suggessation
        public string EquipmentID { get; set; }
        [Required(ErrorMessage = "EquipmentNameErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        public string Name { get; set; }
        [Display(Name = "spnLocation|" + LocalizeResourceSetConstants.Global)]
        public string Location { get; set; }
        [Display(Name = "spnSerialNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string SerialNumber { get; set; }
        //[Required(ErrorMessage = "EquipmenttypeErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "GlobalType|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        [Display(Name = "spnMake|" + LocalizeResourceSetConstants.Global)]
        public string Make { get; set; }
        [Display(Name = "GlobalModel|" + LocalizeResourceSetConstants.Global)]
        public string ModelNumber { get; set; }
        [Display(Name = "spnAccount|" + LocalizeResourceSetConstants.Global)]
        public string Account { get; set; }
        [Display(Name = "spnAssetNo|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string AssetNumber { get; set; }
        public string Area { get; set; }
        public string Line { get; set; }
        public string Photos { get; set; }
        [Display(Name = "spnParentId|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string ParentIdClientLookupId { get; set; }
        public string PlantLocationDescription { get; set; }
        [Display(Name = "globalInActive|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public bool InactiveFlag { get; set; }
        public bool HiddenInactiveFlag { get; set; }

        public bool CriticalFlag { get; set; }
        [Display(Name = "spnExpires|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public DateTime? Maint_WarrantyExpire { get; set; }
        [Display(Name = "spnVendor|" + LocalizeResourceSetConstants.Global)]
        public string MaintVendorIdClientLookupId { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        public string Maint_WarrantyDesc { get; set; }

        public string HiddenType { get; set; }
        public HttpPostedFileBase TypeImageFile { get; set; }

        public IEnumerable<SelectListItem> AccountList { get; set; }
        public IEnumerable<SelectListItem> LookupTypeList { get; set; }
        public IEnumerable<SelectListItem> VendorList { get; set; }
        public IEnumerable<SelectListItem> AssetCategoryList { get; set; }
        public bool AlertFollowedEquipment { get; set; }

        public string ErrorMessages { get; set; }
        public string ClientLookupId { get; set; }
        public Guid hdLoginId { get; set; }

        public string localurl { get; set; }
        public long? LineID { get; set; }
        public long EquID { get; set; }
        public long? DeptID { get; set; }
        public long? SystemInfoId { get; set; }
        public byte[] EquipmentImage { get; set; }
        [Display(Name = "spnCategory|" + LocalizeResourceSetConstants.Global)]
        public string AssetCategory { get; set; }
        public int UpdateIndex { get; set; }     
        public IEnumerable<SelectListItem> ActiveDeptList { get; set; }     

        #region from Equipment table

        [Display(Name = "spnVIN|" + LocalizeResourceSetConstants.Global)]
        public string VIN { get; set; }
        [Required(ErrorMessage = "EquipmentVehicleTypeErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnVehicleType|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string VehicleType { get; set; }
        [Display(Name = "spnVehicleYear|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public int VehicleYear { get; set; }
        [Display(Name = "GlobalModel|" + LocalizeResourceSetConstants.Global)]
        public string Model { get; set; }
        [Display(Name = "spnLicense|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string License { get; set; }
        [Display(Name = "spnRegistrationLoc|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string RegistrationLoc { get; set; }

        #region For 391-392 
        [Display(Name = "spnFuelUnits|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string FuelUnits { get; set; }
        [Display(Name = "spnMeter1Type|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Meter1Type { get; set; }
        [Display(Name = "spnMeter1Units|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RequiredIfValueExist("FleetAssetModel_Meter1Type", ErrorMessage = "Meter1UnitsErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Meter1Units { get; set; }
        [Display(Name = "spnMeter2Type|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Meter2Type { get; set; }
        [Display(Name = "spnMeter2Units|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RequiredIfValueExist("FleetAssetModel_Meter2Type", ErrorMessage = "Meter2UnitsErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Meter2Units { get; set; }

        public string FleetAssetModel_Meter2Type { get; set; }
        public string FleetAssetModel_Meter1Type { get; set; }

        #endregion
        #endregion

        #region From FleetDimensions table

        [Display(Name = "spnColor|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Color { get; set; }
        [Display(Name = "spnBodyType|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string BodyType { get; set; }
        [Display(Name = "spnWidth|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Width { get; set; }
        [Display(Name = "spnHeight|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Height { get; set; }
        [Display(Name = "spnLength|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Length { get; set; }
        [Display(Name = "spnPassengerVolume|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal PassengerVolume { get; set; }
        [Display(Name = "spnCargoVolume|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal CargoVolume { get; set; }
        [Display(Name = "spnGroundClearance|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal GroundClearance { get; set; }
        [Display(Name = "spnBedLength|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal BedLength { get; set; }
        // RKL - V2-521
        [Display(Name = "spnCurbWeight|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal CurbWeight { get; set; }
        // RKL - V2-521
        [Display(Name = "spnVehicleWeight|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        // RKL - V2-521
        public decimal VehicleWeight { get; set; }
        [Display(Name = "spnTowingCapacity|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        // RKL - V2-521
        public decimal TowingCapacity { get; set; }
        [Display(Name = "spnMaxPayload|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal MaxPayload { get; set; }
        #endregion

        #region From FleetEngine table

        [Display(Name = "spnEngineBrand|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string EngineBrand { get; set; }
        [Display(Name = "spnAspiration|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Aspiration { get; set; }
        [Display(Name = "spnBore|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Bore { get; set; }
        [Display(Name = "spnCam|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string Cam { get; set; }
        [Display(Name = "spnCompression|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Compression { get; set; }
        [Display(Name = "spnCylinders|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Cylinders { get; set; }
        [Display(Name = "spnDisplacement|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Displacement { get; set; }
        [Display(Name = "spnFuelInduction|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string FuelInduction { get; set; }
        [Display(Name = "spnFuelQuality|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal FuelQuality { get; set; }
        [Display(Name = "spnMaxHP|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal MaxHP { get; set; }
        [Display(Name = "spnMaxTorque|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal MaxTorque { get; set; }
        [Display(Name = "spnRedlineRPM|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal RedlineRPM { get; set; }
        [Display(Name = "spnStroke|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Stroke { get; set; }
        [Display(Name = "spnValves|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Valves { get; set; }
        [Display(Name = "spnTransmissionBrand|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string TransmissionBrand { get; set; }
        [Display(Name = "spnTransmissionType|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string TransmissionType { get; set; }
        [Display(Name = "spnGears|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Gears { get; set; }
        #endregion

        #region From FleetWheel table

        [Display(Name = "spnBrakeSystem|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string BrakeSystem { get; set; }
        [Display(Name = "spnRearTrackWidth|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal RearTrackWidth { get; set; }
        [Display(Name = "spnWheelbase|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Wheelbase { get; set; }
        [Display(Name = "spnFrontWheelDiameter|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal FrontWheelDiameter { get; set; }
        [Display(Name = "spnRearWheelDiameter|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal RearWheelDiameter { get; set; }
        [Display(Name = "spnFrontTirePSI|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal FrontTirePSI { get; set; }
        [Display(Name = "spnRearTirePSI|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal RearTirePSI { get; set; }
        #endregion

        #region From FleetFluids Table

        [Display(Name = "spnFuelQuality|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string FleetFuelQuality { get; set; }// FuelQuality change to FleetFuelQuality
        [Display(Name = "spnFuelType|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public string FuelType { get; set; }
        [Display(Name = "spnFuelTank1Capacity|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal FuelTankCapacity1 { get; set; }
        [Display(Name = "spnFuelTank2Capacity|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal FuelTankCapacity2 { get; set; }
        [Display(Name = "spnEPACity|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal EPACity { get; set; }
        [Display(Name = "spnEPAHighway|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal EPAHighway { get; set; }
        [Display(Name = "spnEPACombined|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999.99, ErrorMessage = "globalTwoDecimalAfterFourDecimalBeforeTotalSixRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal EPACombined { get; set; }
       
        #endregion

       

        [DefaultValue(false)]
        public bool isfleetDimensionData { get; set; }
        [DefaultValue(false)]
        public bool isfleetEngineData { get; set; }
        [DefaultValue(false)]
        public bool isfleetFluidsData { get; set; }
        [DefaultValue(false)]
        public bool isfleetWheelData { get; set; }
        //[DefaultValue(false)]
        //public bool isfleetSetUpData { get; set; }

        #region Asset Availability
        public bool RemoveFromService { get; set; }
        public DateTime? RemoveFromServiceDate { get; set; }
        public DateTime? ExpectedReturnToService { get; set; }
        public string RemoveFromServiceReason { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        public string AssetAvailability { get; set; }
        public bool IsAssetAvailability { get; set; }
        public DateTime RemoveServiceDate { get; set; }
        #endregion
        public bool ClientOnPremise { get; set; }

    }
}