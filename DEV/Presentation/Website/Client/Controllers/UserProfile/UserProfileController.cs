using Client.BusinessWrapper.UserProfile;
using Client.Common;
using Client.Controllers.Common;
using Client.Models.UserProfile;
using Common.Constants;
using System.Linq;
using System.Web.Mvc;

namespace Client.Controllers.UserProfile
{
    public class UserProfileController : SomaxBaseController
    {
        #region GetProfile
        public JsonResult GetUserProfile()
        {
            UserProfileWrapper uWrapper = new UserProfileWrapper(userData);            
            var userProfile = uWrapper.GetUserProfile();
            //-------V2 - 491
            var passwordSettings = uWrapper.RetrievePasswordSettings();
            if (passwordSettings != null)
            {
                userProfile.PasswordReqMinLength = passwordSettings.PWReqMinLength;
                userProfile.PasswordMinLength = passwordSettings.PWMinLength;
                userProfile.PasswordRequireNumber = passwordSettings.PWRequireNumber;
                userProfile.PasswordRequireAlpha = passwordSettings.PWRequireAlpha;
                userProfile.PasswordRequireMixedCase = passwordSettings.PWRequireMixedCase;
                userProfile.PasswordRequireSpecialChar = passwordSettings.PWRequireSpecialChar;
                userProfile.PasswordNoRepeatChar = passwordSettings.PWNoRepeatChar;
                userProfile.PasswordNotEqualUserName = passwordSettings.PWNotEqualUserName;
            }
            //------- V2-491
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