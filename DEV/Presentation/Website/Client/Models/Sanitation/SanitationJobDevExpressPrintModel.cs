using iTextSharp.text.pdf.security;

using System.Collections.Generic;

namespace Client.Models.Sanitation
{
    public class SanitationJobDevExpressPrintModel
    {
        public long? SanitationJobId { get; set; }
        public string ClientLookupId { get; set; }
        public long? ChargeToId { get; set; }
        public string ChargeTo_ClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string ChargeType { get; set; }
        public string AssetLocation { get; set; }
        public string Shift { get; set; }
        public string Status { get; set; }
        public string ScheduledDate { get; set; }
        public decimal? ScheduledDuration { get; set; }
        public string Description { get; set; }
        public long? AssignedTo_PersonnelId { get; set; }
        public string Assigned { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string CompleteDate { get; set; }
        public string CompleteBy { get; set; }
        public string CompleteComments { get; set; }
        public string PassDate { get; set; }
        public string PassBy { get; set; }
        public bool OnPremise { get; set; }
        public string spnCopyRights { get; set; }


        #region Localization
        public string spnSanitationJob { get; set; }
        public string spnDetails { get; set; }
        public string spnChargeToName { get; set; }
        public string spnShift { get; set; }
        public string spnStatus { get; set; }
        public string spnScheduledDate { get; set; }
        public string spnScheduledDuration { get; set; }
        public string spnSanitationCreateBy { get; set; }
        public string spnAssigned { get; set; }
        public string spnDescription { get; set; }

        public string spnCompletion { get; set; }
        public string spnGlobalCompleted { get; set; }
        public string spnBy { get; set; }
        public string spnCompleteComments { get; set; }

        public string spnVerification { get; set; }
        public string globalCreateDate { get; set; }
        public string GlobalReason { get; set; }
        public string spnComment { get; set; }
        #endregion Localization

        public List<ToolsDevExpressPrintModel> Tools { get; set; }
        public List<ChemicalsDevExpressPrintModel> Chemicals { get; set; }
        public List<SJTasksDevExpressPrintModel> Tasks { get; set; }
        public List<SJLaborDevExpressPrintModel> Labors { get; set; }
        public List<SJCompleteDevExpressPrintModel> Completions { get; set; }
        public List<SJVerificationDevExpressPrintModel> Verifications { get; set; }

        public SanitationJobDevExpressPrintModel()
        {
            Tools = new List<ToolsDevExpressPrintModel>();
            Chemicals = new List<ChemicalsDevExpressPrintModel>();
            Tasks = new List<SJTasksDevExpressPrintModel>();
            Labors = new List<SJLaborDevExpressPrintModel>();
            Completions = new List<SJCompleteDevExpressPrintModel>();
            Verifications = new List<SJVerificationDevExpressPrintModel>();
        }
    }
}
