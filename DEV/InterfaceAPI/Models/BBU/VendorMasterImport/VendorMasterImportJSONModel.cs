using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.VendorMasterImport
{
    public class VendorMasterImportJSONModel
    {
        public VendorMasterImportJSONModel()
        {
            SOMAXVENDOR_DAT = "";
            ORACLE_VENDOR_ID = 0;
            ORACLE_VENDOR_NUMBER = "";
            VENDOR_NAME = "";
            VENDOR_TYPE = "";
            ENABLED_FLAG = "";
            ORACLE_VENDOR_SITE_ID = 0;
            ORACLE_VENDOR_SITE_CODE = "";
            ADDRESS_LINE1 = "";
            ADDRESS_LINE2 = "";
            ADDRESS_LINE3 = "";
            CITY = "";
            STATE = "";
            COUNTRY = "";
            ZIP = "";
            INACTIVE_DATE = DateTime.MinValue;
        }
        public string SOMAXVENDOR_DAT { get; set; }
        public long ORACLE_VENDOR_ID { get; set; }
        public string ORACLE_VENDOR_NUMBER { get; set; }
        public string VENDOR_NAME { get; set; }
        public string VENDOR_TYPE { get; set; }
        public string ENABLED_FLAG { get; set; }
        public long ORACLE_VENDOR_SITE_ID { get; set; }
        public string ORACLE_VENDOR_SITE_CODE { get; set; }
        public string ADDRESS_LINE1 { get; set; }
        public string ADDRESS_LINE2 { get; set; }
        public string ADDRESS_LINE3 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string COUNTRY { get; set; }
        public string ZIP { get; set; }
        public DateTime? INACTIVE_DATE { get; set; }

    }
}