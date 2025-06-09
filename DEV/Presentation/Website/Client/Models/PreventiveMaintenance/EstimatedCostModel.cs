using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Common.Constants;
namespace Client.Models.PreventiveMaintenance
{
    public class EstimatedCostModel
    {
        public long EstimatedCostsId { get; set; }
        public string ObjectType { get; set; }
        public long PrevMaintMasterId { get; set; }
        public string PrevmaintClientlookUp { get; set; }
        public string Category { get; set; }
        public long CategoryId { get; set; }
        public string Description { get; set; }
        public decimal? Duration { get; set; }
        public decimal? UnitCost { get; set; }
        public decimal? Quantity { get; set; }
        public string Source { get; set; }
        public long VendorId { get; set; }
        public long UpdateIndex { get; set; }
        [Required(ErrorMessage = "validationselectpart|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        
        public string ClientLookupId { get; set; }
        public string VendorClientLookupId { get; set; }        
        public decimal TotalPartCost { get; set; }
        public decimal TotalLaborHours { get; set; }
        public decimal TotalCraftCost { get; set; }
        public decimal TotalExternalCost { get; set; }
        public decimal TotalInternalCost { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal TotalSummeryCost { get; set; }
        public bool Security { get; set; }

        public IEnumerable<SelectListItem> SourceList { get; set; }
        public IEnumerable<SelectListItem> VendorLookUpList { get; set; }
        public long CraftId { get; set; }

        public IEnumerable<SelectListItem> PartsList { get; set; }
        public IEnumerable<SelectListItem> CraftList { get; set; }
    }
}