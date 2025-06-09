using System.Web.Mvc;

namespace Client.Models.Configuration.ClientSetUp
{
    public class WoCompletionSettingsModel
    {
        public long WOCompletionSettingsId { get; set; }
        public bool UseWOCompletionWizard { get; set; }
        public bool WOCommentTab { get; set; }
        public bool TimecardTab { get; set; }
        public bool AutoAddTimecard { get; set; }
        //V2-728***
        public bool WOCompCriteriaTab { get; set; }
        public string WOCompCriteriaTitle { get; set; }
        public string WOCompCriteria { get; set; }

    }
}