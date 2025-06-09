using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Models.GenerateSanitationJobs
{
    public class GenerateSanitationJobsModel
    {
        [Display(Name = "spnScheduleType|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "validationScheduleType|" + LocalizeResourceSetConstants.SanitationDetails)]
        public string ScheduleType { get; set; }
        public string OnDemand { get; set; }
        [Required(ErrorMessage = "DateErrMsg|" + LocalizeResourceSetConstants.Global)]
        public DateTime? GenerateThrough { get; set; }
        public bool chkPrintSanitation { get; set; }
        public bool chkGenerate_Sanitation { get; set; }
        public IEnumerable<SelectListItem> ScheduleTypeList { get; set; }
        public IEnumerable<SelectListItem> OnDemandList { get; set; }
        public List<string> AssetGroup1Ids { get; set; }
        public List<string> AssetGroup2Ids { get; set; }
        public List<string> AssetGroup3Ids { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public long SanMasterBatchEntryId { get; set; }
        public long SanMasterBatchHeaderId { get; set; }
        public IEnumerable<SelectListItem> AssetGroup1List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup2List { get; set; }
        public IEnumerable<SelectListItem> AssetGroup3List { get; set; }
        
    }
}