using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.Configuration.UserManagement
{
    public class UserStoreroomModel
    {
        public long StoreroomAuthId { get; set; }
        public long ClientId { get; set; }
        [Required(ErrorMessage = "GlobalSiteSelect|" + LocalizeResourceSetConstants.Global)]
        public long SiteId { get; set; }
        public string SiteName { get; set; }
        public long PersonnelId { get; set; }
        [Required(ErrorMessage = "GlobalStoreroomSelect|" + LocalizeResourceSetConstants.Global)]
        public long StoreroomId { get; set; }
        public string StoreroomName { get; set; }
        public bool Maintain { get; set; }
        public bool Issue { get; set; }
        public bool IssueTransfer { get; set; }
        public bool ReceiveTransfer { get; set; }
        public bool PhysicalInventory { get; set; }
        public bool Purchase { get; set; }
        public bool ReceivePurchase { get; set; }
        //public string CreateBy { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string ModifyBy { get; set; }
        //public DateTime ModifyDate { get; set; }
        public string ClientLookupId { get; set; }
        public long UserInfoId { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> StoreroomList { get; set; }
        public int TotalCount { get; set; } 
    }
}