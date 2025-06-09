using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.Vendor.UIConfiguration
{
    public class EditVendorModelDynamic
    {
        #region UDF columns
        public long VendorUDFId { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public bool Bit1 { get; set; }
        public bool Bit2 { get; set; }
        public bool Bit3 { get; set; }
        public bool Bit4 { get; set; }
        public decimal? Numeric1 { get; set; }
        public decimal? Numeric2 { get; set; }
        public decimal? Numeric3 { get; set; }
        public decimal? Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion


        #region Vendor table coulmn
        public long VendorId { get; set; }
        public string ClientLookupId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressCountry { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressState { get; set; }
        public string CustomerAccount { get; set; }
        public string EmailAddress { get; set; }
        public string FaxNumber { get; set; }
        public string FOBCode { get; set; }
        public bool InactiveFlag { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string RemitAddress1 { get; set; }
        public string RemitAddress2 { get; set; }
        public string RemitAddress3 { get; set; }
        public string RemitCity { get; set; }
        public string RemitCountry { get; set; }
        public string RemitPostCode { get; set; }
        public string RemitState { get; set; }
        public bool RemitUseBusiness { get; set; }
        public string Terms { get; set; }
        public string Type { get; set; }
        public string Website { get; set; }
        public long? VendorMasterId { get; set; }
        public bool AutoEmailPO { get; set; }
        public bool IsExternal { get; set; }
        public bool PunchoutIndicator { get; set; }
        public string VendorDomain { get; set; }
        public string VendorIdentity { get; set; }
        public string SharedSecret { get; set; }
        public string SenderDomain { get; set; }
        public string SenderIdentity { get; set; }
        public string PunchoutURL { get; set; }
        public bool AutoSendPunchOutPO { get; set; }
        public string SendPunchoutPOURL { get; set; }
        public string SendPunchoutPOEmail { get; set; }
        #endregion
    }
}