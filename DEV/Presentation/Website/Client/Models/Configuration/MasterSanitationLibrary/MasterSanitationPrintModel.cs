using System;
namespace Client.Models.Configuration.MasterSanitationLibrary
{
    public class MasterSanitationPrintModel
    {
        public string ClientLookUpId { get; set; }
        public string Description { get; set; }
        public decimal? JobDuration { get; set; }
        public string FrequencyType { get; set; }
        public int? Frequency { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}