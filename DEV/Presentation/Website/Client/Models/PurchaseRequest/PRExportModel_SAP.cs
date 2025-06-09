using Newtonsoft.Json;

namespace Client.Models.PurchaseRequest
{
    public class PRExportModel_SAP
    {
        [JsonProperty(PropertyName = "RequestNo")]
        public string PurchaseRequestClientLookupId { get; set; }
        [JsonProperty(PropertyName = "RequisitionDate")]
        public string ApprovedDate { get; set; }
        [JsonProperty(PropertyName = "VendorNo")]
        public string VendorClientLookupId { get; set; }
        [JsonProperty(PropertyName = "Originator")]
        public string PersonnelEXOracleUserId { get; set; }
        [JsonProperty(PropertyName = "Requester")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "Somaxclientid")]
        public long ClientId { get; set; }
        [JsonProperty(PropertyName = "Somaxsiteid")]
        public long SiteId { get; set; }
        [JsonProperty(PropertyName = "Somaxprid")]
        public long PurchaseRequestId { get; set; }
        [JsonProperty(PropertyName = "LineNumber")]
        public long LineNumber { get; set; }
        [JsonProperty(PropertyName = "ItemDescription")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "AccountNo")]
        public string AccountClientLookupId { get; set; }
        [JsonProperty(PropertyName = "UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }
        [JsonProperty(PropertyName = "Cost")]
        public decimal UnitCost { get; set; }
        [JsonProperty(PropertyName = "Quantity")]
        public decimal OrderQuantity { get; set; }
    }
}