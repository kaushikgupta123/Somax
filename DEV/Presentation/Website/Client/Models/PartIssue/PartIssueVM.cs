using DataContracts;

using System.Collections.Generic;
namespace Client.Models.PartIssue
{
    public class PartIssueVM : LocalisationBaseVM
    {
        public PartIssueVM()
        {

        }
        public PartIssueModel partIssueModel { get; set; }

        public PartHistoryModel partHistoryModel { get; set; }
        public PartIssueValidationModel partIssueValidationModel { get; set; }
        public EquipmentTreeModel equipmentTreeModel { get; set; }
        public UserData userData { get; set; }
        //V2-624
        public bool IsMobile { get; set; }
        //V2-1178
        public PartIssueReturnModel partIssueReturnModel { get; set; }
    }
}