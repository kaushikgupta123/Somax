using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PurchaseRequest
{
    public class ApprovalRouteModelByObjectId
    {
        public long ApprovalRouteId { get; set; }
        public long ApprovalGroupId { get; set; }
    }
}