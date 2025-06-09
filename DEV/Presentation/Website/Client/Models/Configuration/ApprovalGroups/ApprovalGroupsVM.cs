using Client.Models.Common;

using DataContracts;

using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Configuration.ApprovalGroups
{
    public class ApprovalGroupsVM : LocalisationBaseVM
    {
        public ApprovalGroupsModel ApprovalGroupsModel { get; set; }
        public ApprovalGroupMasterModel ApprovalGroupMasterModel { get; set; }
        public Security security { get; set; }
        public List<LineItemModel> listLineItem { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public List<ApprovalGroupsPDFPrintModel> approvalGroupsPDFPrintModelList { get; set; }
        public List<SelectListItem> ApproverPersonnelList { get; set; }
        public List<SelectListItem> LevelList{ get; set; }
        public AppGroupApproverModel appGroupApproverModel { get; set; }
        public int LevelCount { get; set; }
    }
}