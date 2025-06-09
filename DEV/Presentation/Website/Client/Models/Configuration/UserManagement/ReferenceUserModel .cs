using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Client.Models.Configuration.UserManagement
{
    public class ReferenceUserModel
    {
        
        [Display(Name = "UserInfoId|" + LocalizeResourceSetConstants.UserDetails)]
        public long UserInfoId { get; set; }
        [Required(ErrorMessage = "UserNameErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        [Display(Name = "UserName|" + LocalizeResourceSetConstants.Global)]
        [Remote("CheckIfUserExistForReferenceUserAdd", "UserManagement", HttpMethod = "POST", ErrorMessage = "504|" + LocalizeResourceSetConstants.StoredProcValidation)]
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
        [Display(Name = "Craft|" + LocalizeResourceSetConstants.UserDetails)]
        public long? CraftId { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}