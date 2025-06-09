using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Work_Order
{
    public class WorkOrderCompletionWizard
    {
        public WorkOrderCompletionWizard()
        {
            WOLabors = new List<CompletionLaborWizard>();
            WorkOrderIds = new List<WoCancelAndPrintListModel>();
        }
        [AllowHtml]
        public string CompletionComments { get; set; }
        public List<CompletionLaborWizard> WOLabors { get; set; }
        public IList<WoCancelAndPrintListModel> WorkOrderIds { get; set; }
        public string WOLaborsString { get; set; }
        public string CommentUserIds { get; set; }
        public bool CompletionCriteriaConfirm { get; set; }
    }
}