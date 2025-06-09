using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.FleetScheduledService
{
    public class FleetScheduledServiceModel
   {
        public long ClientId { get; set; }
        public long ScheduledServiceId { get; set; }
        public long SiteId { get; set; }
        public long AreaId { get; set; }
        public long DepartmentId { get; set; }
        public long StoreroomId { get; set; }
        //[Display(Name = "spnServiceTask|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Display(Name = "spnServiceTaskDesc|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SelectServiceTaskIdErrorMessage|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public long ServiceTaskId { get; set; }
        public long EquipmentId { get; set; }
        [Display(Name = "spnGlobalEquipmentId|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SelectEquipmentIDErrorMessage|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        public bool InactiveFlag { get; set; }
        public long Last_ServiceOrderId { get; set; }
        public DateTime? LastPerformedDate { get; set; }
        public decimal LastPerformedMeter1 { get; set; }
        public decimal LastPerformedMeter2 { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0, 999999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "spnOdometerInterval|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public decimal Meter1Interval { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0, 9999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Meter1Threshold { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0, 9999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)] 
        [Display(Name = "spnHourInterval|" + LocalizeResourceSetConstants.EquipmentDetails)]
        public decimal Meter2Interval { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,1}$", ErrorMessage = "globalOneDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0, 9999999.9, ErrorMessage = "GT0.0Min1OneDecimalAfterEightDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal Meter2Threshold { get; set; }
        public DateTime? NextDueDate { get; set; }
        public decimal NextDueMeter1 { get; set; }
        public decimal NextDueMeter2 { get; set; }
        [Display(Name = "spnTimeInterval|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Range(0, 9999, ErrorMessage = "GlobalGreaterThanOrEqualtoZeroErrMessage|" + LocalizeResourceSetConstants.Global)]
        public int TimeInterval { get; set; }
        public string TimeIntervalType { get; set; }
        [Display(Name = "spnTimeThreshold|" + LocalizeResourceSetConstants.EquipmentDetails)]
        [Range(0, 9999, ErrorMessage = "GlobalGreaterThanOrEqualtoZeroErrMessage|" + LocalizeResourceSetConstants.Global)]
        public int TimeThreshold { get; set; }
        public string TimeThresoldType { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        [DefaultValue("Add")]
        public string Pagetype { get; set; }
        public string Meter1Type { get; set; }
        public string Meter2Type { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public IEnumerable<SelectListItem> LookUpServiceTypeList { get; set; }    
        public IEnumerable<SelectListItem> LookUpTimeTypeList { get; set; }
        [Display(Name = "globalSystem|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnSystemRequired|" + LocalizeResourceSetConstants.Global)]
        public string System { get; set; }
        [Display(Name = "spnGlobalAssembly|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnAssemblyRequired|" + LocalizeResourceSetConstants.Global)]
        public string Assembly { get; set; }
        [Display(Name = "spnRepairReason|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnRepairReasonRequired|" + LocalizeResourceSetConstants.Global)]
        public string RepairReason { get; set; }
    }
}