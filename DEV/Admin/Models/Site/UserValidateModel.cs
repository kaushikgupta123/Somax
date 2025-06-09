using Admin.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Site
{
    public class UserValidateModel
    {
        public long SiteId { get; set; }

        public long ClientId { get; set; }

        public long LoginInfoId { get; set; }

        [Required(ErrorMessage = "Please enter User Name")]
        public string UserName { get; set; } //V2-1178
       
    }
}