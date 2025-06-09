using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.NotificationSetUp
{
    public class AlertTargetModel : LocalisationBaseVM
    {
        public string Personnel_ClientLookupId { get; set; }
        public long AlertSetupId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public long AlertTargetId { get; set; }
        public int UpdateIndex { get; set; }
        [Required(ErrorMessage = "spnUserNameErrorMsg|" + LocalizeResourceSetConstants.NotificationDetails)]
        public long ClientLookupID { get; set; }
        public IEnumerable<SelectListItem> ClientLookupList { get; set; }
        public long? AlertSiteId { get; set; }
        public string PackageLevelDef { get; set; }

        public bool IsSuperUser { get; set; }

    }
}