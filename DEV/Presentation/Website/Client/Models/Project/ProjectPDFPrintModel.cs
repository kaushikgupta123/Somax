using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Project
{
    public class ProjectPDFPrintModel : ProjectSearchPrintModel
    {
        public ProjectPDFPrintModel()
        {
            projectTaskmodel = new List<ProjectTaskModel>();
        }
        public List<ProjectTaskModel> projectTaskmodel { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string CompletedDateString { get; set; }
    }
}