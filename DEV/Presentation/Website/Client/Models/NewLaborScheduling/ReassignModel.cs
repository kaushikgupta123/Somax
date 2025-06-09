using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.NewLaborScheduling
{
    public class ReassignModel
    {
        public long PersonnelId { get; set; }
        public IEnumerable<SelectListItem> Personnellist { get; set; }      
        public string WorkOrderSchedIds { get; set; }
      
    }
}