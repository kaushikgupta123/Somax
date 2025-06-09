using Client.Models.Common;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.WorkOrderStatus
{
    public class AnalyticsWorkOrderStatusVM
    {
        public AnalyticsWorkOrderStatusVM()
        {
            WOStatusModel = new List<WOStatusModel>();
            WOModel = new List<WOModel>();
        }
        public List<WOStatusModel> WOStatusModel {  get; set; }
        public List<WOModel> WOModel {  get; set; }
    }
}