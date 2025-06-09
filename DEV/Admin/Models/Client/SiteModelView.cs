using Admin.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.Client
{
    public class SiteModelView
    {
        public long SiteId { get; set; }
        public long ClientId { get; set; }
        [Display(Name = "spnName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "SiteNameErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Name { get; set; }
        [Display(Name = "spnDescription|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "DescriptionErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string Description { get; set; }
        [Display(Name = "GlobalTimeZone|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "TimeZoneErrorMessage|" + LocalizeResourceSetConstants.ClientDetails)]
        public string TimeZone { get; set; }
        public bool APM { get; set; }
        public bool CMMS { get; set; }
        public bool Sanitation { get; set; }
        public bool Production { get; set; }
        public string Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool UsePunchOut { get; set; }
        public int AppUsers { get; set; }
        public int? MaxAppUsers { get; set; }
        public int WorkRequestUsers { get; set; }
        public int? MaxWorkRequestUsers { get; set; }
        public int SanitationUsers { get; set; }
        public int? MaxSanitationUsers { get; set; }
        public int ProdAppUsers { get; set; }
        public int? MaxProdAppUsers { get; set; }
        public int UpdateIndex { get; set; }
    }
}