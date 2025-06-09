using Admin.BusinessWrapper.UserProfile;
using Admin.Common;
using Admin.Controllers.Common;
using Admin.Models.UserProfile;
using Common.Constants;
using System.Linq;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class UserProfileController : SomaxBaseController
    {
        #region GetProfile
        public JsonResult GetUserProfile()
        {
            UserProfileWrapper uWrapper = new UserProfileWrapper(userData);
            var userProfile = uWrapper.GetUserProfile();
            return Json(userProfile, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region UpdateProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateUserProfile(UserProfileModel model)
        {
            if (ModelState.IsValid)
            {
                UserProfileWrapper uWrapper = new UserProfileWrapper(userData);
                var result = uWrapper.UpdateUserProfile(model);
                if (result.Item1 == true)
                    return Json(new { Result = JsonReturnEnum.success.ToString(), userFirstName = model.UserProfileFirstName, userLastName = model.UserProfileLastName }, JsonRequestBehavior.AllowGet);
                else
                    return Json(result.Item2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ModelValidationFailedMessage = UtilityFunction.GetMessageFromResource("globalModelStateValidationMsg", LocalizeResourceSetConstants.Global);
                return Json(ModelValidationFailedMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}