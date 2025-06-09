using Client.CustomValidation;
using Common.Constants;
using DataContracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.BusinessIntelligence
{
    public class UserReportsModel
    {
        public long UserReportsId { get; set; }
        public long OwnerId { get; set; }
        [Required(ErrorMessage = "globalValidName|" + LocalizeResourceSetConstants.Global)]
        [Remote("CheckIfUserReportNameExist", "Reports", HttpMethod = "POST", ErrorMessage = "spnReportExist|" + LocalizeResourceSetConstants.Report)]
        public string ReportName { get; set; }
        public string Description { get; set; }
        public string ReportGroup { get; set; }
        public string SourceName { get; set; }
        public bool UseSP { get; set; }
        public string PrimarySortColumn { get; set; }
        public string SecondarySortColumn { get; set; }
        public bool IsGrouped { get; set; }
        public string GroupColumn { get; set; }
        public bool IncludePrompt { get; set; }
        public string Prompt1Source { get; set; }
        public string Prompt1Type { get; set; }
        public string Prompt1ListSource { get; set; }
        public string Prompt1List { get; set; }
        public string Prompt2Source { get; set; }
        public string Prompt2Type { get; set; }
        public string Prompt2ListSource { get; set; }
        public string Prompt2List { get; set; }
        public string ChildLinkColumn { get; set; }
        public string ChildSourceName { get; set; }
        public string MasterLinkColumn { get; set; }
        public string Filter { get; set; }
        public string SaveType { get; set; }
        public bool Del { get; set; }

        public long SourceId { get; set; }
        public bool IsUserReport { get; set; }
        public bool IncludeChild { get; set; }

        public string BaseQuery { get; set; }

        public bool IsEnterprise { get; set; }
    }
}