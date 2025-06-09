using Client.Models.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.WorkOrderStatus
{
    public class WOStatusModel
    {
        public string Status { get; set; }
        public int StatusCount { get; set; }
    }
}