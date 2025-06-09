using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseApproval
{
  public class PRExportModel_Coupa
  {
    //private decimal _TotalAmount { get; set; }
    // RKL - Mimic the JSON file that was provided by Dean Foods.
    // Test if we can remove Null items
    public PRExportModel_Coupa()
    {
      requisitionlines = new List<PRLineExportModel_Coupa>();
    }
    [JsonProperty(PropertyName = "buyer-note")]
    public string BuyerNote { get; set; }
    [JsonProperty(PropertyName = "justification")]
    public string Justification { get; set; }
    [JsonProperty(PropertyName = "need-by-date")]
    public DateTimeOffset? NeedByDate { get; set; }
    [JsonProperty(PropertyName = "ship-to-attention")]
    public string ShipToAttention { get; set; }
    [JsonProperty(PropertyName = "receiving-warehouse-id")]
    public string ReceivingWarehouseId { get; set; }
    [JsonProperty(PropertyName = "line-count")]
    public int LineCount { get; set; }
    [JsonProperty(PropertyName = "total")]
    public decimal TotalAmount { get; set; }
    //{
    //  get { return Math.Round(_TotalAmount, 2); }
    //  set { this._TotalAmount = value; }
    //}
    [JsonProperty(PropertyName="custom-fields")]
    public Coupa_PR_Header_CustomFields CustomFields { get; set; }
    [JsonProperty(PropertyName = "currency")]
    public Coupa_Currency Currency { get; set; }
    [JsonProperty(PropertyName = "requested-by")]
    public Coupa_RequestedBy  RequestedBy { get; set; }
    [JsonProperty(PropertyName = "ship-to-address")]
    public Coupa_ShipToAddress ShipTo { get; set; }
    [JsonProperty(PropertyName = "requisition-lines")]
    public List<PRLineExportModel_Coupa> requisitionlines { get; set; }
    //public long PurchaseRequestId { get; set; }
    //public string approver { get; set; }
    //[JsonProperty(PropertyName = "created-by")]
    //public string createdby { get; set; }
    //public string requester { get; set; }
    //[JsonProperty(PropertyName = "ship-to-address")]
    //public string shiptoaddress { get; set; }
    //public int somax_client_id { get; set; }
    //public int somax_site_id { get; set; }
    //public string somax_pr { get; set; }
    //public long somax_document_id { get; set; }

    /*
    // Original 
    public PRExportModel_Coupa()
    {
        requisitionlines = new List<PRLineExportModel_Coupa>();
    }
    public long PurchaseRequestId { get; set; }
    public string approver { get; set; }
    [JsonProperty(PropertyName = "created-by")]
    public string createdby { get; set; }
    public string justification { get; set; }
    [JsonProperty(PropertyName = "need-by-date")]
    public string needbydate { get; set; }
    public string requester { get; set; }
    [JsonProperty(PropertyName = "ship-to-address")]
    public string shiptoaddress { get; set; }
    [JsonProperty(PropertyName = "ship-to-attention")]
    public string shiptoattention { get; set; }
    public int somax_client_id { get; set; }
    public int somax_site_id { get; set; }
    public string somax_pr { get; set; }
    public long somax_document_id { get; set; }
    public List<PRLineExportModel_Coupa> requisitionlines { get; set; }
    */
  }
  public class Coupa_PR_Header_CustomFields
  {
    //public Dictionary<string, string> custfields;
    [JsonProperty(PropertyName = "somax-pr")]
    public string PRNumber { get; set; }
    [JsonProperty(PropertyName = "somax-document-id")]
    public string PurchaseRequestId { get; set; }
    [JsonProperty(PropertyName = "somax-client-id")]
    public string ClientId { get; set; }
    [JsonProperty(PropertyName = "somax-site-id")]
    public string SiteId { get; set; }
  }
  public class Coupa_Currency
  {
    [JsonProperty(PropertyName ="code")]
    public string Code { get; set; }
  }
  public class Coupa_RequestedBy
  {
    [JsonProperty(PropertyName = "email")]
    public string EmailAddress { get; set; }
  }
  public class Coupa_ShipToAddress
  {
    [JsonProperty(PropertyName = "name")]
    public string ShipToName { get; set; }
  }

}