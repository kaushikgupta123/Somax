using Client.Models.Common;
using Client.Models.Project;
using Client.Models.ProjectCosting.UIConfiguration;
using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Models.ProjectCosting
{
    public class ProjectCostingVM : LocalisationBaseVM
    {
        public ProjectCostingVM()
        {

        }
        public ProjectCostingSearchModel projectCostingSearchModel { get; set; }
        public ProjectCostingModel projectCostingModel { get; set; }

        #region Project Costing 
        public Security security { get; set; }
        public UserData _userdata { get; set; }
        public List<ProjectCostingTaskModel> projectCostingTaskModel { get; set; }
        public ProjectCostingDetailsHeaderModel projectCostingHeaderSummaryModel { get; set; }
        public List<EventLogModel> EventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public List<UserMentionData> userMentionData { get; set; }
        public List<ProjectCostingPDFPrintModel> projectCostingPDFPrintModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public WorkOrderForProjectDetailsLookupListModel workOrderForProjectDetailsLookupListModel { get; set; }
        #endregion

        #region ProjectCosting Details
        public ProjectCostingDetailsHeaderModel projectHeaderSummaryModel { get; set; }
        public ViewProjectCostingModelDynamic ViewProjectCosting { get; set; }
        public List<Client.Common.UIConfigurationDetailsForModelValidation> UIConfigurationDetails { get; set; }
        #endregion

        public IEnumerable<UILookupList> AllRequiredLookUplist { get; set; }
        public EditProjectCostingModelDynamic EditProject { get; set; }
        public AddProjectCostingModelDynamic AddProject { get; set; }
        public IEnumerable<SelectListItem> ProjectCategoryList { get; set; }
        public IEnumerable<SelectListItem> ProjectViewList { get; set; }
        public IEnumerable<SelectListItem> OwnerPersonnelList { get; set; }
        public IEnumerable<SelectListItem> CoordinatorPersonnelList { get; set; }
        public string AssignedGroup1Label { get; set; }
        public string AssignedGroup2Label { get; set; }
        public string AssignedGroup3Label { get; set; }
      
        public IEnumerable<SelectListItem> AssignedGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssignedGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssignedGroup3List { get; set; }
        public long? AssetGroup1Id { get; set; }
        public long? AssetGroup2Id { get; set; }
        public long? AssetGroup3Id { get; set; }
    }
}