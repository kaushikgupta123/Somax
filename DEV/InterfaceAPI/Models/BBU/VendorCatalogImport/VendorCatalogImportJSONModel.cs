using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.VendorCatalogImport
{
    public class VendorCatalogImportJSONModel
    {
        public VendorCatalogImportJSONModel()
        {
            ORACLE_VENDOR_ID = "";
            ORACLE_VENDOR_NUMBER = "";
            ORACLE_VENDOR_SITE_ID = "";
            ORACLE_VENDOR_SITE_CODE = "";
            ORC_SRC_DOC_ID = "";
            ORC_SRC_DOC_NUM = "";
            EFFECTIVE_START_DATE = "";
            EFFECTIVE_END_DATE = "";
            CANCELLED_FLAG = "";
            ORC_SRC_DOC_LINE_NUM = "";
            ORC_SRC_DOC_LINE_ID = "";
            ORACLE_PART_ID = "";
            ORACLE_PART_NUMBER = "";
            UNSPC_CODE_ID_TREE = "";
            LINE_DESCRIPTION = "";
            PURCHASING_UOM = "";
            UNIT_PRICE = "";
            PRIMARY_UOM = "";
            UOM_CONVERSION = "";
            VENDOR_PART_NUMBER = "";
            VENDOR_LEAD_TIME = "";
            MINIMUM_ORDER_QUANTITY = "";
            EXPIRATION_DATE = "";
        }
        public string ORACLE_VENDOR_ID { get; set; }
        public string ORACLE_VENDOR_NUMBER { get; set; }
        public string ORACLE_VENDOR_SITE_ID { get; set; }
        public string ORACLE_VENDOR_SITE_CODE { get; set; }
        public string ORC_SRC_DOC_ID { get; set; }
        public string ORC_SRC_DOC_NUM { get; set; }
        public string EFFECTIVE_START_DATE { get; set; }
        public string EFFECTIVE_END_DATE { get; set; }
        public string CANCELLED_FLAG { get; set; }
        public string ORC_SRC_DOC_LINE_NUM { get; set; }
        public string ORC_SRC_DOC_LINE_ID { get; set; }
        public string ORACLE_PART_ID { get; set; }
        public string ORACLE_PART_NUMBER { get; set; }
        public string UNSPC_CODE_ID_TREE { get; set; }
        public string LINE_DESCRIPTION { get; set; }
        public string PURCHASING_UOM { get; set; }
        public string UNIT_PRICE { get; set; }
        public string PRIMARY_UOM { get; set; }
        public string UOM_CONVERSION { get; set; }
        public string VENDOR_PART_NUMBER { get; set; }
        public string VENDOR_LEAD_TIME { get; set; }
        public string MINIMUM_ORDER_QUANTITY { get; set; }
        public string EXPIRATION_DATE { get; set; }

    }
}