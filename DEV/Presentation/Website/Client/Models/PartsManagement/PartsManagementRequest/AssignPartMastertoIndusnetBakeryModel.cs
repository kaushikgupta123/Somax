using Client.CustomValidation;

using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class AssignPartMastertoIndusnetBakeryModel
    {
        [Required(ErrorMessage = "ValidPartId|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string PartMaster_ClientLookupId { get; set; }
        [Required(ErrorMessage = "ValidJustification|" + LocalizeResourceSetConstants.PartsManagementDetail)]
        public string Justification { get; set; }
        public IEnumerable<SelectListItem> PartMasterIdList { get; set; }
        public string RequestType { get; set; }
        #region V2-798
        [Required(ErrorMessage = "GlobalPleaseEnterLocation|" + LocalizeResourceSetConstants.Global)]
        public string Location { get; set; }
        [Required(ErrorMessage = "validationUnitCost|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,5}$", ErrorMessage = "globalFiveDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 9999999999.99999, ErrorMessage = "globalFiveDecimalAfterTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? UnitCost { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.00, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QtyMinimum { get; set; }
        [Required(ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,6}$", ErrorMessage = "globalSixDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [GreaterThan("QtyMinimum", ErrorMessage = "globalGreaterThanToMinimumValueErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.01, 999999999.999999, ErrorMessage = "globalSixDecimalAfterNineDecimalBeforeTotalFifteenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? QtyMaximum { get; set; }
        public long PartMasterRequestId { get; set; }
        public string PartMaster_LongDescription { get; set; }
        public string Part_ClientLookupId { get; set; }
        #endregion
    }
}