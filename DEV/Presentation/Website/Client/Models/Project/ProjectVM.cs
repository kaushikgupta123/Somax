using Client.Models.Common;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using static Client.Models.Common.UserMentionDataModel;

namespace Client.Models.Project
{
    public class ProjectVM : LocalisationBaseVM
    {
        public ProjectVM()
        {

        }
        public ProjectSearchModel projectSearchModel { get; set; }
        public ProjectModel projectModel { get; set; }

        #region Project 
        public Security security { get; set; }
        public  UserData _userdata { get; set; }
        public List<ProjectTaskModel> projectTaskModel { get; set; }
        public ProjectDetailsHeaderModel projectHeaderSummaryModel { get; set; }
        public List<EventLogModel> EventLogList { get; set; }
        public List<Notes> NotesList { get; set; }
        public List<UserMentionData> userMentionData { get; set; }
        public List<ProjectPDFPrintModel> projectPDFPrintModel { get; set; }
        public List<TableHaederProp> tableHaederProps { get; set; }
        public WorkOrderForProjectDetailsLookupListModel workOrderForProjectDetailsLookupListModel { get; set; }
        #endregion
        public ProjectAddOrEditModel projectAddorEdirModel { get; set; }

        public IEnumerable<SelectListItem> ProjectViewList { get; set; }
    }
}