using Admin.CustomValidation;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Admin.Models.UserManagement
{
    public class UserModel
    {
        public UserModel()
        {
            UserDetail = new UserDetails();
        }
        public UserDetails UserDetail { get; set; }
        [Display(Name = "UserInfoId|" + LocalizeResourceSetConstants.UserDetails)]
        public long UserInfoId { get; set; }
        [Required(ErrorMessage = "UserNameErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [Display(Name = "UserName|" + LocalizeResourceSetConstants.Global)]
        [Remote("CheckIfUserExistForUserAdd", "UserManagement", HttpMethod = "POST", ErrorMessage = "504|" + LocalizeResourceSetConstants.StoredProcValidation)]
        public string UserName { get; set; }
        [Display(Name = "FirstName|" + LocalizeResourceSetConstants.UserDetails)]
        [Required(ErrorMessage = "GlobalFirstNameErrorMessage|" + LocalizeResourceSetConstants.Global)]
        [RegularExpression("^[A-Za-z0-9 _]*[A-Za-z0-9][A-Za-z0-9 _]*$", ErrorMessage = "SpecialCharacterErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string FirstName { get; set; }
        [Display(Name = "MiddleName|" + LocalizeResourceSetConstants.UserDetails)]
        public string MiddleName { get; set; }
        [Display(Name = "LastName|" + LocalizeResourceSetConstants.UserDetails)]
        [Required(ErrorMessage = "UserLastNameErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string LastName { get; set; }
        public IEnumerable<SelectListItem> LookupCraftList { get; set; }
        public IEnumerable<SelectListItem> LookupShiftList { get; set; }
        //[StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        //[Required(ErrorMessage = "UserPasswordErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "SecurityQuestion|" + LocalizeResourceSetConstants.UserDetails)]
        //[Required(ErrorMessage = "UserSecurityQuestionErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string SecurityQuestion { get; set; }
        [Display(Name = "SecurityResponse|" + LocalizeResourceSetConstants.UserDetails)]
        //[Required(ErrorMessage = "UserSecurityResponseErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]       
        public string SecurityResponse { get; set; }
        [Display(Name = "Email|" + LocalizeResourceSetConstants.VendorDetails)]
        [EmailAddress(ErrorMessage = "UserEmailErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [RequiredIf("UserType", "admin", ErrorMessage = "Please enter email.")]
        public string Email { get; set; }
        public IEnumerable<SelectListItem> LookupUserTypeList { get; set; }
        public IEnumerable<SelectListItem> LookupSecurityProfileIdList { get; set; }
        [Display(Name = "globalActive|" + LocalizeResourceSetConstants.Global)]
        public bool IsActive { get; set; }
        [Display(Name = "Administrator|" + LocalizeResourceSetConstants.UserDetails)]
        public bool IsSuperUser { get; set; }
        [Display(Name = "Buyer|" + LocalizeResourceSetConstants.UserDetails)]
        public bool Buyer { get; set; }
        [Display(Name = "HomePhone|" + LocalizeResourceSetConstants.UserDetails)]
        //[RegularExpression(@"^[\+0-9]*$", ErrorMessage = "UserHomePhoneErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string Phone1 { get; set; }
        [Display(Name = "MobilePhone|" + LocalizeResourceSetConstants.UserDetails)]
        //[RegularExpression(@"^[\+0-9]*$", ErrorMessage = "UserMobilePhoneErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string Phone2 { get; set; }
        [Display(Name = "Address|" + LocalizeResourceSetConstants.UserDetails)]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        [Display(Name = "City|" + LocalizeResourceSetConstants.UserDetails)]
        public string AddressCity { get; set; }
        [Display(Name = "State|" + LocalizeResourceSetConstants.UserDetails)]
        public string AddressState { get; set; }
        [Display(Name = "PostalCode|" + LocalizeResourceSetConstants.UserDetails)]
        public string AddressPostCode { get; set; }
        [Display(Name = "Country|" + LocalizeResourceSetConstants.UserDetails)]
        public string AddressCountry { get; set; }
        [Display(Name = "Craft|" + LocalizeResourceSetConstants.UserDetails)]
        public long? CraftId { get; set; }
        [Display(Name = "Shift|" + LocalizeResourceSetConstants.UserDetails)]
        public string Shift { get; set; }
        [Display(Name = "UserType|" + LocalizeResourceSetConstants.UserDetails)]
        public string UserType { get; set; }
        //[RequiredSecurityProfileForUser("UserType", ErrorMessage = "UserSecurityProfileerrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [Display(Name = "SecurityProfile|" + LocalizeResourceSetConstants.UserDetails)]
        public long? SecurityProfileId { get; set; }
        public long PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public List<string> ErrorMessages { get; set; }
        public string CraftName { get; set; }
        public string SecurityProfile { get; set; }

        public bool SecurityBBUUser { get; set; }
        public UserData _userdata { get; set; }
        //[Display(Name ="User Access")]
        //[Required(ErrorMessage = "Please select User Access")]
        [Required(ErrorMessage = "GlobalUserAccessSelect|" + LocalizeResourceSetConstants.Global)]
        public string SecurityProfileName { get; set; }
        public bool CMMSUser { get; set; }
        public bool SanitationUser { get; set; }
        public int ProductGrouping { get; set; }
        public string PackageLevel { get; set; }
        public bool IsPasswordTemporary { get; set; }
        [RequiredIf("PackageLevel", UserTypeConstants.Enterprise, ErrorMessage = "GlobalSiteSelect|" + LocalizeResourceSetConstants.Global)]
        public long? SiteId { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public string TimeZone { get; set; }
        public string SiteName { get; set; }
        public int FailedAttempts { get; set; }
        public long? DefaultSiteId { get; set; }
        public long? DefaultPersonnelId { get; set; }
        public bool SiteControlled { get; set; }
        //V2-803
        public long LoginSSOId { get; set; }
        [Display(Name = "spnGmailId|" + LocalizeResourceSetConstants.UserDetails)]
        [EmailAddress(ErrorMessage = "GmailIdErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string GMailId { get; set; }
        [Display(Name = "spnMicrosoftmailId|" + LocalizeResourceSetConstants.UserDetails)]
        [EmailAddress(ErrorMessage = "MicrosoftmailIdErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string MicrosoftMailId { get; set; }
        [Display(Name = "spnWindowsADUserId|" + LocalizeResourceSetConstants.UserDetails)]
        //[EmailAddress(ErrorMessage = "MicrosoftmailIdErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string WindowsADUserId { get; set; }        
        public IEnumerable<SelectListItem> SecurityProfileIdList { get; set; } /*v2-802*/
        [Display(Name = "spnGlobalEmployeeId|" + LocalizeResourceSetConstants.Global)]
        public string EmployeeId { get; set; } //V2-877
        #region V2-905
        public IEnumerable<SelectListItem> UserTypeList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> IsActiveList { get; set; }
        #endregion
        #region V2-962
        public string SecurityProfileDescription { get; set; }
        #endregion
    }
}