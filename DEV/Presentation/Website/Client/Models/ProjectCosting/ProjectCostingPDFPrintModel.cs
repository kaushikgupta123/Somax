using System.Collections.Generic;

namespace Client.Models.ProjectCosting
{
    public class ProjectCostingPDFPrintModel : ProjectCostingSearchPrintModel
    {
        public ProjectCostingPDFPrintModel()
        {
            projectCostingTaskModel = new List<ProjectCostingTaskModel>();
        }
        public List<ProjectCostingTaskModel> projectCostingTaskModel { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string CompletedDateString { get; set; }
    }
}