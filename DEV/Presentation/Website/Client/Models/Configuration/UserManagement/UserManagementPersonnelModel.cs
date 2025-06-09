using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Configuration.UserManagement
{
    public class UserManagementPersonnelModel
    {       
        public long ClientId { get; set; }
        public long PersonnelId { get; set; }
        public long UserInfoId { get; set; }  
        public long SiteId { get; set; }   
        public string UserSiteName { get; set; }
        public string CraftDescription { get; set; }
        public List<string> ErrorMessages { get; set; }


    }
}