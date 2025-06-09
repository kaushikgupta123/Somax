using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.Work_Order
{
    public class WOPlannerModel
    {
        [Required(ErrorMessage = "Planner is Required")]
        public long Planner_PersonnelId { get; set; }
        public List<WOPlannerListModel> list { get; set; }
    }
    public class WOPlannerListModel
    {
        public long WorkOrderId { get; set; }
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
    }

 }