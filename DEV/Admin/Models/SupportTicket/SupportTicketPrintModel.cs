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
    public class SupportTicketPrintModel
    {
        public long SupportTicketId { get; set; }
        public string Subject { get; set; }
        public string Contact { get; set; }
        public string Status { get; set; }
        public string Agent { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int TotalCount { get; set; }
    }
}