using Client.CustomValidation;

using Common.Constants;

using System.Collections.Generic;
using Client.Models.Parts;

namespace Client.Models.PartIssue
{
    public class PartIssueValidationModel
    {
        public long personnelId { get; set; }
        public string IssueToClientLookupId { get; set; }
        public string chargeToClientLookupId { get; set; }
       public PartModel objPart { get; set; }
        public string ErrorMsg { get; set; }
        public long PartStoreroomId { get; set; }
       public PartHistoryModel partHistoryModel { get; set; }
        //V2-624
        public string Comments { get; set; }
        //V2-687
        public long StoreroomId { get; set; }
        public string StoreroomName { get; set; }
     

    }
}