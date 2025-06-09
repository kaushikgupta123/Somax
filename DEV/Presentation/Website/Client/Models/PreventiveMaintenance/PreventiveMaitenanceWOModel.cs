using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.PreventiveMaintenance
{
    public class PreventiveMaitenanceWOModel
    {
        [Display(Name = "spnSchdType|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        [Required(ErrorMessage = "validscheduletype|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public string ScheduleType { get; set; }
        public string OnDemand { get; set; }
        [Required(ErrorMessage = "spnPleaseselectdate|" + LocalizeResourceSetConstants.PrevMaintDetails)]
        public DateTime? GenerateThrough { get; set; }
        public bool chkPrintWorkOrder { get; set; }
        public bool chkGenerate_WorkOrders { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
        public long ? AssetGroup1Id { get; set; }
        public long ? AssetGroup2Id { get; set; }
        public long ? AssetGroup3Id { get; set; }
        public string PrevMaintSchedType { get; set; }
        public string PrevMaintMasterType { get; set; }


    }
}