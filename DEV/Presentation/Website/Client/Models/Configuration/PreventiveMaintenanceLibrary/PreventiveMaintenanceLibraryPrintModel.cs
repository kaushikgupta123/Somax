using System;
namespace Client.Models.Configuration.PreventiveMaintenanceLibrary
{
    public class PreventiveMaintenanceLibraryPrintModel
    {
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public decimal? JobDuration { get; set; }
        public string FrequencyType { get; set; }
        public int Frequency { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}