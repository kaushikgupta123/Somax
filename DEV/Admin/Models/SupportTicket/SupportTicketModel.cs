using Admin.Common;
using Admin.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Admin.Models.SupportTicket
{
    public class SupportTicketModel
    {
        [Required(ErrorMessage = "ClientErrorMessage|" + LocalizeResourceSetConstants.SupportTicketDetails)]
        public long ClientId { get; set; }
        [Required(ErrorMessage = "GlobalSiteSelect|" + LocalizeResourceSetConstants.Global)]
        public long SiteId { get; set; }
        public long SupportTicketId { get; set; }
        [Required( ErrorMessage = "ContactErrorMessage|" + LocalizeResourceSetConstants.SupportTicketDetails)]
        public long Contact_PersonnelId { get; set; }
        [Required(ErrorMessage = "AgentErrorMessage|" + LocalizeResourceSetConstants.SupportTicketDetails)]
        public long Agent_PersonnelId { get; set; }
        [Required(ErrorMessage = "NoteSubjectErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Subject { get; set; }
        [Required(ErrorMessage = "validationDescription|" + LocalizeResourceSetConstants.Global)]
        [AllowHtml]
        public string Description { get; set; }
        [Required(ErrorMessage = "TypeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }
        public IEnumerable<SelectListItem> ContactList { get; set; }
        public IEnumerable<SelectListItem> AgentList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public string Contact { get; set; }
        public string Agent { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<SelectListItem> SiteList { get; set; }
        public IEnumerable<SelectListItem> ClientList { get; set; }
    }
}