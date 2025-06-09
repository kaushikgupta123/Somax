using System;

namespace Client.Models.Sanitation
{
    public class SJCompleteDevExpressPrintModel
    {
        public long SanitationJobId { get; set; }
        public string CompleteDate { get; set; }
        public string CompleteBy { get; set; }
        public string CompleteComments { get; set; }

        #region Localization
        public string spnCompletion { get; set; }
        public string spnGlobalCompleted { get; set; }
        public string spnBy { get; set; }
        public string spnCompleteComments { get; set; }
        #endregion Localization
    }
}

