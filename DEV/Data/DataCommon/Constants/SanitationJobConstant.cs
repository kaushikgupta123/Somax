using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class SanitationJobConstant
    {
        public const string Open = "Open";
        public const string Complete = "Complete";
        public const string Denied = "Denied";
        public const string Canceled = "Canceled";
        public const string Approved = "Approved";
        public const string Request = "Request";
        public const string Scheduled = "Scheduled";
        public const string Pass = "Passed";
        public const string Fail = "Fail";
        public const string SourceType_FailedValidation = "Falied Valid";
        public const string JobRequest = "JobRequest";
        public const string SanitationLabor_ChargeType_Primary = "SanitationJob";
        public const string SanitationPlanning_CategoryTool = "Tool";
        public const string SanitationPlanning_CategoryChemical = "Chemical";
        public const string SanitationPlanning_Tool = "SANIT_TOOLS";
        public const string SanitationChemicalSupplies_Tool = "SANIT_CHEMICALS";
        public const bool SanitaionJob_AutoGenerateEnabled = true;
        public const string SourceType_Request = "Request";
        public const string SourceType_NewJob = "NewJob";
        public const string TaskComplete = "Complete";
        public const string TaskCancel = "Cancel";
        public const string TaskReOpen = "Re-Open";
        public const string SANIT_TOOLS = "SANIT_TOOLS";
        public const string SANIT_CHEMICALS = "SANIT_CHEMICALS";
        public const string Sanitation_NotesTableName = "SanitationJob";
        public const string SourceType_WorkBenchAdd = "WorkbenchAdd";
        public const string SANIT_ON_DEMAND = "SANIT_ON_DEMAND";
        public const string Followup = "Followup";
    }
}
