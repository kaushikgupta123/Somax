using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.VendorMasterImport
{
    public class VendorMasterImportModel
    {
        public VendorMasterImportModel()
        {
            ClientId = 0;
            VendorMasterImportId = 0;
            ClientLookupId = "";
            ExVendorId = 0;
            ExVendorSiteId = 0;
            ExVendorSiteCode = "";
            Name = "";
            Type = "";
            Terms = "";
            FOBCode = "";
            Address1 = "";
            Address2 = "";
            Address3 = "";
            AddressCity = "";
            AddressState = "";
            AddressPostCode = "";
            AddressCountry = "";
            RemitAddress1 = "";
            RemitAddress2 = "";
            RemitAddress3 = "";
            RemitAddressCity = "";
            RemitAddressState = "";
            RemitAddressPostCode = "";
            RemitAddressCountry = "";
            RemitUseBusiness = false;
            Enabled = "";
            FaxNumber = "";
            PhoneNumber = "";
            EmailAddress = "";
            ErrorMessage = "";
            InactiveDate = DateTime.MinValue;

        }
        public long ClientId { get; set; }
        public long VendorMasterImportId { get; set; }
        public string ClientLookupId { get; set; }
        public long ExVendorId { get; set; }
        public long ExVendorSiteId { get; set; }
        public string ExVendorSiteCode { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Terms { get; set; }
        public string FOBCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostCode { get; set; }
        public string AddressCountry { get; set; }
        public string RemitAddress1 { get; set; }
        public string RemitAddress2 { get; set; }
        public string RemitAddress3 { get; set; }
        public string RemitAddressCity { get; set; }
        public string RemitAddressState { get; set; }
        public string RemitAddressPostCode { get; set; }
        public string RemitAddressCountry { get; set; }
        public bool RemitUseBusiness { get; set; }
        public string Enabled { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime? InactiveDate { get; set; }

    }
}