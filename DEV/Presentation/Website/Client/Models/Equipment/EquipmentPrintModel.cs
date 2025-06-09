namespace Client.Models
{
    public class EquipmentPrintModel
    {        
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AsseteGroup1ClientLookupId { get; set; }
        public string AsseteGroup2ClientLookupId { get; set; }
        public string AsseteGroup3ClientLookupId { get; set; }
        //public string Dept_Desc { get; set; }
        //public string Line_Desc { get; set; }        
        //public string System_Desc { get; set; }
        public string SerialNumber { get; set; }
        public string Type { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string LaborAccountClientLookupId { get; set; }       
        public string AssetNumber { get; set; }
        //V2-636
        public string RemoveFromService { get; set; }
        public string RemoveFromServiceDate { get; set; }
    }
}