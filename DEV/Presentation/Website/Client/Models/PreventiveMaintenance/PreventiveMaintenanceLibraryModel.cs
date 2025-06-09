using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PreventiveMaintenance
{
    public class PreventiveMaintenanceLibraryModel
    {
        public long ClientId{get;set;}
        public long PrevMaintLibraryId{get;set;}
        public long SiteId{get;set;}
        public string ClientLookupId{get;set;}
        public string Description{get;set;}
        public bool DownRequired{get;set;}
        public int Frequency{get;set;}
        public string FrequencyType{get;set;}
        public decimal JobDuration{get;set;}
        public string Type{get;set;}
        public string ScheduleMethod{get;set;}
        public string ScheduleType{get;set;}
        public bool InactiveFlag{get;set;}
        public bool Del{get;set;}
        public int UpdateIndex{get;set;}
    }
}