using System;
namespace Client.Models
{
    public class EquipmentSearchModel
    {
        public long EquipmentId {get;set;}
        public string ClientLookupId{ get; set; }
        public string LaborAccountClientLookupId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string AssetNumber { get; set; }
        public string Area_Desc { get; set; }
        public string Line_Desc { get; set; }
        public string BusinessType { get; set; }
        public string Dept_Desc { get; set; }
        public string System_Desc { get; set; }
        public string AssetGroup1 { get; set; }
        public string AssetGroup2 { get; set; }
        public string AssetGroup3 { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
        #region Asset Availability V2-636
        public bool RemoveFromService { get; set; }
        public DateTime? RemoveFromServiceDate { get; set; }
        #endregion 
        public string imageURL { get; set; }
    }

}