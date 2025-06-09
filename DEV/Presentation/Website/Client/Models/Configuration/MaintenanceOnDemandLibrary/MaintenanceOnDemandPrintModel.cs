using System;
namespace Client.Models.Configuration.MaintenanceOnDemandLibrary
{
    public class MaintenanceOnDemandPrintModel
    {       
        public string ClientLookUpId {get;set;}
        public string Description {get;set;}
        public string Type {get;set;}
        public DateTime CreateDate { get; set; }

    }
}