using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class ClientEL
    {
        public long ClientId { set; get; }
        public string CompanyName { set; get; }
        public string LegalName { set; get; }
        public string PrimaryContact { set; get; }
        public int NumberOfEmployees { set; get; }
        public long AnnualSales { set; get; }
        public string TaxIDNumber { set; get; }
        public string VATNumber { set; get; }
        public string Email { set; get; }
        public string Website { set; get; }
        public string Status { set; get; }
        public string BusinessType { set; get; }
        public DateTime DateEstablished { set; get; }
        public int NumberOfLocations { set; get; }
        public string OfficerName { set; get; }
        public string OfficerTitle { set; get; }
        public string OfficerPhone { set; get; }
        public string DunnsNumber { set; get; }
        public string PackageLevel { set; get; }
        public int AppUsers { set; get; }
        public int MaxAppUsers { set; get; }
        public int PhoneUsers { set; get; }
        public int MaxPhoneUsers { set; get; }
        public string PrimarySICCode { set; get; }
        public string NAICSCode { set; get; }
        public int Sites { set; get; }
        public string MinorityStatus { set; get; }
        public string Localization { set; get; }
        public string DefaultTimeZone { set; get; }
        public string DefaultCustomerManager { set; get; }
        public int MaxAttempts { set; get; }
        public int MaxTimeOut { set; get; }
        public string ConnectionString { set; get; }
        public int TabletUsers { set; get; }
        public int MaxTabletUsers { set; get; }
        public string UIConfiguration { set; get; }
        public long UpdateIndex { set; get; }
        public string WOPrintMessage { set; get; }
        public string PurchaseTermsandConds { set; get; }
        public string CreateBy { set; get; }
        public DateTime ModifyDate { set; get; }
        public string ModifyBy { set; get; }
    }
}
