using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Client.Models.PreventiveMaintenance
{
    public class PMGenerateWorkOrdersModel
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
        public List<string> AssetGroup1Ids { get; set; }
        public List<string> AssetGroup2Ids { get; set; }
        public List<string> AssetGroup3Ids { get; set; }
        public List<string> PrevMaintSchedType { get; set; }
        public List<string> PrevMaintMasterType { get; set; }

        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string WOType { get; set; }
        public string PMType { get; set; }

        public long PrevMaintBatchEntryId { get; set; }

        public long PrevMaintBatchHeaderId { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        public IEnumerable<SelectListItem> WOTypeList { get; set; }
        public IEnumerable<SelectListItem> PMTypeList { get; set; }  

        public IEnumerable<SelectListItem> PrevMaintSchedTypeList { get; set; }
        public IEnumerable<SelectListItem> PrevMaintMasterTypeList { get; set; }
        #region V2-1082
        public string Shift { get; set; }
        public bool? DownRequired { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> DownRequiredInactiveFlagList { get; set; }
        #endregion
        
    }
}