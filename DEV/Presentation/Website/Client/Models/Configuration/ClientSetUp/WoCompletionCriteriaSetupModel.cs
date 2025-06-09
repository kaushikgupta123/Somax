using Common.Constants;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.ClientSetUp
{
    public class WoCompletionCriteriaSetupModel
    {      
        public bool WOCompCriteriaTab { get; set; }
        [Required(ErrorMessage = "spnPleaseEnterTitle|" + LocalizeResourceSetConstants.SetUpDetails)]
        public string WOCompCriteriaTitle { get; set; }
        [AllowHtml]
        [Required]
        public string WOCompCriteria { get; set; }
    }
}