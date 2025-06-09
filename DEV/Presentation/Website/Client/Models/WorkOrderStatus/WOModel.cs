using Client.Models.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.WorkOrderStatus
{
    public class WOModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string CompleteStatus { get; set; }
        public string PrevMaintCompleteStatus { get; set; }
        public string Priority { get; set; }
        public string SourceType { get; set; }
        public decimal MaterialCosts { get; set; }
        public decimal LaborHours { get; set; }
        public decimal LaborCosts { get; set; }
        public decimal TotalCosts { get; set; }
    }
}