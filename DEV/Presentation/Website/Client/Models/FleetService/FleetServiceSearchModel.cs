using System;

namespace Client.Models.FleetService
{
    public class FleetServiceSearchModel
    {
        //public string orderbyColumn { get; set; }
        //public string orderBy { get; set; }
        //public Int32 offset1 { get; set; }
        //public Int32 nextrow { get; set; }
        //public long EquipmentId { get; set; }

        public string ServiceOrderID { get; set; }
        public string AssetID { get; set; }
        public string AssetName { get; set; }       
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime? Created { get; set; }
        public string Assigned { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? Completed { get; set; }
       // public int TotalCount { get; set; }


    }
}