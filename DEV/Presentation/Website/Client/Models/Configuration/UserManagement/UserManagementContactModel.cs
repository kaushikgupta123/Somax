using Client.CustomValidation;
using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.UserManagement
{
    public class UserManagementContactModel
    {       
        public long ClientId { get; set; }
        public long ContactId { get; set; }
        public long ObjectId { get; set; }
        public string TableName { get; set; }
        public long OwnerId { get; set; }
        public string OwnerName { get; set; }
        [Required(ErrorMessage = "UserNameErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        [ValidatePinForUSA("AddressCountry", ErrorMessage = "ZipErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string AddressPostCode { get; set; }
        public string AddressState { get; set; }
        public string AddressStateForUSA { get; set; }
        public string AddressStateForOther { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        [EmailAddress(ErrorMessage = "UserEmailErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string Email1 { get; set; }
        [EmailAddress(ErrorMessage = "UserEmailErrorMessage|" + LocalizeResourceSetConstants.UserDetails)]
        public string Email2 { get; set; }
        public int UpdateIndex { get; set; }
        public bool IsEditable { get; set; }
        public string ClientLookupId { get; set; }
        public long UserInfoId { get; set; }
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
    }
}