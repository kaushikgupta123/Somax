using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Sanitation
{
    public class LaborModel
    {
        public long TimecardId { get; set; }
        
        public long? SiteId { get; set; }
      
        [Required(ErrorMessage = "spnReqPersonnelIDrequired|" + LocalizeResourceSetConstants.SanitationDetails)]
        public long PersonnelId { get; set; }

        [Display(Name = "spnDate|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnReqDate|" + LocalizeResourceSetConstants.Global)]
        public DateTime? StartDate { get; set; }


        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "spnReqHours|" + LocalizeResourceSetConstants.SanitationDetails)]
       
        public decimal? Hours { get; set; }

        public int UpdateIndex { get; set; }

        public string ChargeType_Primary { get; set; }

        public long ChargeToId_Primary { get; set; }
           
        public IEnumerable<SelectListItem> PersonnelIdList { get; set; }

        public decimal? Value { get; set; }

        public string NameFull { get; set; }

        public string ClientLookupId { get; set; }
        public string PersonnelClientLookupId { get; set; }
        
    }
}