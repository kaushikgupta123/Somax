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
    public class STNotesModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public long STNotesId { get; set; }
        public long SupportTicketId { get; set; }
        public long From_PersonnelId { get; set; }
        //[Required(ErrorMessage = "noteErrMsg|" + LocalizeResourceSetConstants.Global)]
        [AllowHtml]
        public string Content { get; set; }
        public string Type { get; set; }
    }
}