using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest
{
    public class PRLineExportModel_Coupa
    {
        [JsonProperty(PropertyName = "description")]
        public string description { get; set; }
        [JsonProperty(PropertyName = "line-num")]
        public int linenumber { get; set; }
        [JsonProperty(PropertyName = "need-by-date")]
        public string NeedByDate { get; set; }
        //[JsonProperty(PropertyName = "need-by-date")]
        //public DateTimeOffset? NeedByDate { get; set; }
        [JsonProperty(PropertyName = "source-part-num")]
        public string PartNumber { get; set; }
        [JsonProperty(PropertyName = "source-aux-part-num")]
        public string AuxPartNumber { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public decimal OrderQuantity { get; set; }
        [JsonProperty(PropertyName = "total")]
        public decimal TotalCost { get; set; }
        [JsonProperty(PropertyName = "source-type")]
        public string SourceType { get; set; }
        [JsonProperty(PropertyName = "line-type")]
        public string LineType { get; set; }
        [JsonProperty(PropertyName = "unit-price")]
        public decimal UnitCost { get; set; }
        [JsonProperty(PropertyName = "custom-fields")]
        public Coupa_PR_Line_CustomFields CustomFields { get; set; }
        [JsonProperty(PropertyName = "account")]
        public Coupa_Account Account { get; set; }
        [JsonProperty(PropertyName = "commodity")]
        public Coupa_Commodity Commodity { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public Coupa_Currency Currency { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public Coupa_Vendor Vendor { get; set; }
        [JsonProperty(propertyName: "payment-term")]
        public Coupa_Payment_Terms PaymentTerm { get; set; }
        [JsonProperty(propertyName: "uom")]
        public Coupa_UOM UnitOfMeasure { get; set; }
        /*
        // Original
        public string account { get; set; }
        public string description { get; set; }
        public string item { get; set; }
        [JsonProperty(PropertyName = "line-num")]
        public int linenum { get; set; }
        [JsonProperty(PropertyName = "line-type")]
        public string linetype { get; set; }
        [JsonProperty(PropertyName = "need-by-date")]
        public string needbydate { get; set; }
        [JsonProperty(PropertyName = "source-part-num")]
        public string sourcepartnum { get; set; }
        public string supplier { get; set; }
        [JsonProperty(PropertyName = "unit-price")]
        public decimal unitprice { get; set; }
        public string uom { get; set; }
        public string PurchaseRequestLineItemId { get; set; }
        */
    }
    public class Coupa_PR_Line_CustomFields
    {
        //public Dictionary<string, string> custfields;
        [JsonProperty(PropertyName = "nontaxable")]
        public string NonTaxable { get; set; }
        [JsonProperty(PropertyName = "somax-line-doc-id")]
        public string PurchaseRequestLineItemId { get; set; }
    }
    public class Coupa_Account
    {
        public Coupa_Account(string Acct)
        {
            // This character should probably be external (Interface Properties)
            this.AccountCode = Acct;
            char[] cs = { '-' };
            string[] acct_segments = Acct.Split(cs, 20, StringSplitOptions.RemoveEmptyEntries);
            if (acct_segments.Count() > 0)
                Account_Segment_1 = acct_segments[0];
            if (acct_segments.Count() > 1)
                Account_Segment_2 = acct_segments[1];
            if (acct_segments.Count() > 2)
                Account_Segment_3 = acct_segments[2];
            if (acct_segments.Count() > 3)
                Account_Segment_4 = acct_segments[3];
            if (acct_segments.Count() > 4)
                Account_Segment_5 = acct_segments[4];
            if (acct_segments.Count() > 5)
                Account_Segment_6 = acct_segments[5];
            if (acct_segments.Count() > 6)
                Account_Segment_7 = acct_segments[6];
            if (acct_segments.Count() > 7)
                Account_Segment_8 = acct_segments[7];
            if (acct_segments.Count() > 8)
                Account_Segment_9 = acct_segments[8];
            if (acct_segments.Count() > 9)
                Account_Segment_10 = acct_segments[9];
            if (acct_segments.Count() > 10)
                Account_Segment_11 = acct_segments[10];
            if (acct_segments.Count() > 11)
                Account_Segment_12 = acct_segments[11];
            if (acct_segments.Count() > 12)
                Account_Segment_13 = acct_segments[12];
            if (acct_segments.Count() > 13)
                Account_Segment_14 = acct_segments[13];
            if (acct_segments.Count() > 14)
                Account_Segment_15 = acct_segments[14];
            if (acct_segments.Count() > 15)
                Account_Segment_16 = acct_segments[15];
            if (acct_segments.Count() > 16)
                Account_Segment_17 = acct_segments[16];
            if (acct_segments.Count() > 17)
                Account_Segment_18 = acct_segments[17];
            if (acct_segments.Count() > 18)
                Account_Segment_19 = acct_segments[18];
            if (acct_segments.Count() > 19)
                Account_Segment_20 = acct_segments[19];
        }
        //public Dictionary<string, string> custfields;
        [JsonProperty(PropertyName = "code")]
        public string AccountCode { get; set; }
        [JsonProperty(PropertyName = "segment-1")]
        public string Account_Segment_1 { get; set; }
        [JsonProperty(PropertyName = "segment-2")]
        public string Account_Segment_2 { get; set; }
        [JsonProperty(PropertyName = "segment-3")]
        public string Account_Segment_3 { get; set; }
        [JsonProperty(PropertyName = "segment-5")]
        public string Account_Segment_4 { get; set; }
        [JsonProperty(PropertyName = "segment-6")]
        public string Account_Segment_5 { get; set; }
        [JsonProperty(PropertyName = "segment-7")]
        public string Account_Segment_6 { get; set; }
        [JsonProperty(PropertyName = "segment-8")]
        public string Account_Segment_7 { get; set; }
        [JsonProperty(PropertyName = "segment-9")]
        public string Account_Segment_8 { get; set; }
        [JsonProperty(PropertyName = "segment-0")]
        public string Account_Segment_9 { get; set; }
        [JsonProperty(PropertyName = "segment-10")]
        public string Account_Segment_10 { get; set; }
        [JsonProperty(PropertyName = "segment-11")]
        public string Account_Segment_11 { get; set; }
        [JsonProperty(PropertyName = "segment-12")]
        public string Account_Segment_12 { get; set; }
        [JsonProperty(PropertyName = "segment-13")]
        public string Account_Segment_13 { get; set; }
        [JsonProperty(PropertyName = "segment-14")]
        public string Account_Segment_14 { get; set; }
        [JsonProperty(PropertyName = "segment-15")]
        public string Account_Segment_15 { get; set; }
        [JsonProperty(PropertyName = "segment-16")]
        public string Account_Segment_16 { get; set; }
        [JsonProperty(PropertyName = "segment-17")]
        public string Account_Segment_17 { get; set; }
        [JsonProperty(PropertyName = "segment-18")]
        public string Account_Segment_18 { get; set; }
        [JsonProperty(PropertyName = "segment-19")]
        public string Account_Segment_19 { get; set; }
        [JsonProperty(PropertyName = "segment-20")]
        public string Account_Segment_20 { get; set; }
        [JsonProperty(PropertyName = "account-type")]
        public Coupa_Account_Type Account_Type { get; set; }
        //[JsonProperty(PropertyName = "commodity")]
        //public Coupa_Commodity Commodity { get; set; }
        //[JsonProperty(PropertyName = "currency")]
        //public Coupa_Currency Currency { get; set; }
        //[JsonProperty(PropertyName ="supplier")]
        //public Coupa_Vendor Vendor { get; set; }
        //[JsonProperty(PropertyName ="payment-term")]
        //public Coupa_Payment_Terms PaymentTerms { get; set; }
    }
    public class Coupa_Account_Type
    {
        [JsonProperty(PropertyName = "name")]
        public string Account_Name { get; set; }
        [JsonProperty(PropertyName = "currency")]
        public Coupa_Currency Account_Currency { get; set; }
    }
    public class Coupa_Commodity
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
    public class Coupa_Vendor
    {
        //private string _name;
        //[JsonProperty(PropertyName ="name")]
        ////public string VendorName { get; set; }  
        //public string VendorName
        //{ 
        //  get { return _name + '-' + VendorNumber; }
        //  set { this._name = value; }
        //}
        [JsonProperty(PropertyName = "number")]
        public string VendorNumber { get; set; }
    }
    public class Coupa_Payment_Terms
    {
        [JsonProperty(PropertyName = "code")]
        public string TermsCode { get; set; }
    }
}