using System;

namespace Client.Models.Sanitation
{
    public class SJVerificationDevExpressPrintModel
    {
        public long SanitationJobId { get; set; }
        public string VerificationStatus { get; set; }
        public string VerificationDate { get; set; }
        public string VerificationBy { get; set; }
        public bool VerificationCommentsVisible { get; set; }
        public bool VerificationReasonVisible { get; set; }
        public string VerificationReason { get; set; }
        public string VerificationComments { get; set; }

        #region Localization
        public string spnVerification { get; set; }
        public string spnStatus { get; set; }
        public string spnDate { get; set; }
        public string spnBy { get; set; }
        public string GlobalReason { get; set; }
        public string spnComment { get; set; }
        #endregion Localization
    }
}

