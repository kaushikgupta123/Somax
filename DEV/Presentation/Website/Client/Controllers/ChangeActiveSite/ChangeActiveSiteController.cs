using Client.BusinessWrapper.ActiveSiteWrapper;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.ChangeActiveSite;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers.ChangeActiveSite
{
    public class ChangeActiveSiteController : SomaxBaseController
    {
        #region GetActiveSiteList
        public JsonResult GetActiveSites()
        {
            List<ChangeActiveSiteModel> ChangeActiveSiteModelList = new List<ChangeActiveSiteModel>();
            ChangeActiveSiteModel objChangeActiveSiteModel = new ChangeActiveSiteModel();
            ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);

            objChangeActiveSiteModel.ChangeSiteSiteId = userData.DatabaseKey.User.DefaultSiteId;
            var siteList = objActiveSiteWrapper.GetSites();
            objChangeActiveSiteModel.SiteList = siteList;
            return Json(objChangeActiveSiteModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateActiveSite(ChangeActiveSiteModel model)
        {
            if (ModelState.IsValid)
            {
                ActiveSiteWrapper objActiveSiteWrapper = new ActiveSiteWrapper(userData);
                var IsLoggedInFromMobile = userData.IsLoggedInFromMobile; //V2-1001
                var AddResult = objActiveSiteWrapper.UpdateSite(model);
                #region V2-1001
                SetIsLoggedInFromMobileFlag(IsLoggedInFromMobile);
                #endregion
                if (AddResult != null && AddResult.UpdateIndex > 0)
                {
                    return Json(new { Result = JsonReturnEnum.success.ToString(), UpdateIndex = AddResult.UpdateIndex }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = JsonReturnEnum.failed.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #region V2-1001
        private void SetIsLoggedInFromMobileFlag(bool IsLoggedInFromMobile)
        {
            if (Session["userData"] != null)
            {
                var userData = (UserData)Session["userData"];
                userData.IsLoggedInFromMobile = IsLoggedInFromMobile;
                Session["userData"] = userData;
            }
        }
        #endregion
    }
}