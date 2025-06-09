using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.FleetService
{
    public class FleetServiceLineItemModel
    {
        public long ServiceOrderLineItemId { get; set; }
        public long ServiceOrderId { get; set; }
        public string Description { get; set; }
        [Display(Name = "spnLabour|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public string Labor { get; set; }
        [Display(Name = "spnParts|" + LocalizeResourceSetConstants.Global)]
        public string Materials { get; set; }
        [Display(Name = "spnTotal|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public decimal Total { get; set; }
        [Display(Name = "spnRepairReason|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnRepairReasonRequired|" + LocalizeResourceSetConstants.Global)]
        public string RepairReason { get; set; }
        [Display(Name = "spnLogComment|" + LocalizeResourceSetConstants.Global)]
        public string Comment { get; set; }
        [Display(Name = "spnOthers|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public decimal Others { get; set; }
        [Display(Name = "spnServiceTask|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "ServiceTaskIdErrMsg|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public long ServiceTaskId { get; set; }
        [Display(Name = "spnTotal|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public decimal DetailsTotal { get; set; }
        [Display(Name = "spnFleetIssues|" + LocalizeResourceSetConstants.FleetServiceOrder)]
        public long FleetIssuesId { get; set; }
        public long EquipmentId { get; set; }
        public string FIDescription { get; set; }
        public long Index { get; set; }

        public long SchedServiceId { get; set; }
        [Display(Name = "globalSystem|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnSystemRequired|" + LocalizeResourceSetConstants.Global)]
        public string System { get; set; }
        [Display(Name = "spnGlobalAssembly|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnAssemblyRequired|" + LocalizeResourceSetConstants.Global)]
        public string Assembly { get; set; }
        public string Status { get; set; }
    }
}