using Common.Constants;

using System.ComponentModel.DataAnnotations;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class AppGroupApproverModel
    {
        public long ApprovalGroupId { get; set; }
        public long AppGroupApproversId { get; set; }
        [Required(ErrorMessage = "globalSelectApprover|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "globalApprover|" + LocalizeResourceSetConstants.Global)]
        public long ApproverId { get; set; }
        public string ApproverName { get; set; }
        [Range(0, 9999999.99, ErrorMessage = "globalTwoDecimalAfterSevenDecimalBeforeTotalNineRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "globalLimit|" + LocalizeResourceSetConstants.Global)]
        public decimal? Limit { get; set; }
        [Required(ErrorMessage = "globalSelectLevel|" + LocalizeResourceSetConstants.Global)]
        [Display(Name = "globallevel|" + LocalizeResourceSetConstants.Global)]
        public int Level { get; set; }
        public string LevelName { get; set; }
        public int TotalCount { get; set; }
    }
}