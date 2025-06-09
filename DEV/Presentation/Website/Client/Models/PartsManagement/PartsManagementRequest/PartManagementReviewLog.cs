using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartManagementReviewLog
    {
        public long ClientId{get;set;}
        public long ReviewLogId{get;set;}
        public string TableName{get;set;}
        public long ObjectId{get;set;}
        public string Function{get;set;}
        public long PersonnelId{get;set;}
        public DateTime? ReviewDate{get;set;}
        public string Comments{get;set;}
        public string Reviewed_By { get; set; }
    }
}