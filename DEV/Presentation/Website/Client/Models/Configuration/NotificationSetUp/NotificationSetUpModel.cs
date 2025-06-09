using Client.Common;
using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.NotificationSetUp
{
    public class NotificationSetUpModel : LocalisationBaseVM
    {
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "SelectAlertNotificationErrMsg|" + LocalizeResourceSetConstants.NotificationDetails)]
        public long AlertSetupId { get; set; }
        [RequiredIf("IsSiteRequired", true, ErrorMessage = "GlobalSiteSelect|" + LocalizeResourceSetConstants.Global)]
        public long? AlertSiteId { get; set; }
        public IEnumerable<SelectListItem> AlertSetUpSiteList { get; set; }
        public string PackageLevelDef { get; set; }
        public bool IsSuperUser { get; set; }
        public long AlertDefineId { get; set; }
        public bool IsTargetListShow { get; set; }
        public int UpdateIndex { get; set; }
        public bool IsEmailSend { get; set; }
        public bool IsShowEmailSend { get; set; }
        public bool IsIncludeEmailAttachedment { get; set; }
        public IEnumerable<SelectListItem> AlertSetUpListUpList { get; set; }
        public AlertTargetModel alertModel { get; set; }
        public bool IsSiteRequired
        {
            get
            {
                if (PackageLevelDef != null)
                {
                    return (PackageLevelDef.ToUpper() == PackageLevelConstant.Enterprise.ToUpper() && IsSuperUser == true) ? true : false;
                }
                else
                {
                    return false;
                }
            }
        }
    }

}