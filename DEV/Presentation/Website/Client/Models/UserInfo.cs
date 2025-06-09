using DataContracts;
using SomaxMVC.ViewModels;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SomaxMVC.Models
{
    [Serializable]
    public class UserInfoDetails : ViewModelBase
    {
        [DisplayName("User Name")]
        //[Required] //User Name is required
        //[StringLength(100, MinimumLength = 3)]
        public String UserName { get; set; }

        //[Required] //Password is required
         //[StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        //public SelectList ListItem { get; set; }
        public string IsSelected { get; set; }

        public string FailureMessage { get; set; }
        public string FailureReason { get; set; }
        public string[] UserType { get; set; }
        public string ReturnUrl { get; set; }
        public string UserEmail { get; set; }
        public long? ClientId { get; set; }  /*V2-332*/
        public long? SiteId { get; set; }  /*V2-332*/
        public long? PersonnelId { get; set; } /*V2-332*/
        //public DatabaseKey dbKeyinfo { get; set; } /*V2-332*/
        public bool IsLoggedInFromMobile { get; set; }
        public bool IsSOMAXAdmin { get; set; }/*V2-911*/
        public long? ClientUserInfoListId { get; set; } /*V2-911*/
        public long? UserInfoId { get; set; } /*V2-911*/
    }
}