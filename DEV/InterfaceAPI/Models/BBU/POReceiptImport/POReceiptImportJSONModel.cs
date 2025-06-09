using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.POReceiptImport
{
    public class POReceiptImportJSONModel
    {
        public POReceiptImportJSONModel()
        {
            SOMAXRECPT_DAT = "";
            RECEIPT_NUMBER = "";
            RECEIPT_DATE = "";
            ORACLE_RECEIPT_ID = 0;
            ORACLE_VENDOR_ID = "";
            ORACLE_VENDOR_NUMBER = "";
            TRANSACTION_ID = "";
            TRANSACTION_DATE = "";
            PURCHASE_ORDER_ID = "";
            PURCHASE_ORDER_NUM = "";
            PURCHASE_ORDER_LINE_ID = "";
            PURCHASE_ORDER_LINE_NUM = "";
            ORACLE_PARTID = "";
            ORACLE_PART_NUM = "";
            ITEM_DESCRIPTION = "";
            RECEIVED_QUANTITY = "";
            RECEIVED_UNIT_OF_MEASURE = "";
            PRIMARY_UNIT_OF_MEASURE = "";
            UOM_CONVERSION_RATE = "";
            REASON = "";
            SHIP_TO_LOCATION_ID = "";
        }
        public string SOMAXRECPT_DAT { get; set; }
        public string RECEIPT_NUMBER { get; set; }
        public string RECEIPT_DATE { get; set; }
        public long ORACLE_RECEIPT_ID { get; set; }
        public string ORACLE_VENDOR_ID { get; set; }
        public string ORACLE_VENDOR_NUMBER { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string PURCHASE_ORDER_ID { get; set; }
        public string PURCHASE_ORDER_NUM { get; set; }
        public string PURCHASE_ORDER_LINE_ID { get; set; }
        public string PURCHASE_ORDER_LINE_NUM { get; set; }
        public string ORACLE_PARTID { get; set; }
        public string ORACLE_PART_NUM { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string RECEIVED_QUANTITY { get; set; }
        public string RECEIVED_UNIT_OF_MEASURE { get; set; }
        public string PRIMARY_UNIT_OF_MEASURE { get; set; }
        public string UOM_CONVERSION_RATE { get; set; }
        public string REASON { get; set; }
        public string SHIP_TO_LOCATION_ID { get; set; }

    }
}