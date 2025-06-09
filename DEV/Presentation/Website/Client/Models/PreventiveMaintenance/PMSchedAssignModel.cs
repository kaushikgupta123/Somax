
using Common.Constants;

using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class PMSchedAssignModel
    { 
        public long PersonnelId { get; set; }
        public long PMSchedAssignId { get; set; }
        public long PrevMaintSchedId { get; set; }
        [Required(ErrorMessage = "ValidatePersonnel|" + LocalizeResourceSetConstants.Global)]
        public string ClientLookupId { get; set; }
        public string PersonnelFullName { get; set; }
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(double.Epsilon, 99999999.99, ErrorMessage = "globalGreaterThan0TwoDecimalAfterTotalTenRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        public decimal? ScheduledHours { get; set; }
        public int TotalCount { get; set; }
    }

}