using Common.Constants;
using DevExpress.XtraReports.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models
{
    public class VendorInsuranceWidgetModel
    {
        public long VendorId { get; set; }
        public string VendorClientLookupId { get; set; }
        public bool InsuranceRequired { get; set; }
        public DateTime? InsuranceExpireDate { get; set; }
        public string NAICSCode { get; set; }
        public string SICCode { get; set; }
        public string ClassCode { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,4}$", ErrorMessage = "globalFourDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.0000, 999999.9999, ErrorMessage = "GlobalValidDecimalNoWithMaximum4DecimalPlacesAnd10DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? ExpModRate { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,3}$", ErrorMessage = "globalThreeDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0.000, 9999999.999, ErrorMessage = "GlobalValidDecimalNoWithMaximum3DecimalPlacesAnd10DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? OSHARate { get; set; }
        public bool InsuranceOverride { get; set; }
        public DateTime? InsuranceOverrideDate { get; set; }
        public long? ContractorOwner { get; set; }
        public string ContractorOwner_ClientLookupId { get; set; }
        public IEnumerable<SelectListItem> LookupClassCodeList { get; set; }
    }
}