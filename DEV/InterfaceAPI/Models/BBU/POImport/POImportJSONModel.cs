using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.POImport
{
    public class POImportJSONModel
    {
        public POImportJSONModel()
        {
            SOMAXPO_DAT = "";
            SOMAX_REQUISITION_NUMBER = "";
            SOMAX_REQUISITION_ID = "";
            SOMAX_WORK_ORDER_REFERENCE = "";
            ORACLE_REQUSITION_NUMBER = "";
            PURCHASE_ORDER_NUMBER = "";
            PURCHASE_ORDER_ID = "";
            PO_CREATION_DATE = "";
            PO_CURRENCY = "";
            ORACLE_VENDOR_ID = "";
            ORACLE_VENDOR_NUMBER = "";
            ORACLE_VENDOR_SITE_ID = "";
            SOMAX_REQ_LINE_ID = "";
            ORACLE_PO_LINE_NUMBER = "";
            ORACLE_PO_LINE_ID = "";
            ORACLE_PART_ID = "";
            ORACLE_PART_NUMBER = "";
            ITEM_DESCRIPTION = "";
            UNSPSC_ID_TREE = "";
            QUANTITY = "";
            PURCHASING_UNIT_OF_MEASURE = "";
            UNIT_PRICE = "";
            PRIMARY_UNIT_OF_MEASURE = "";
            UOM_CONVERSION_RATE = "";
            SHIP_TO_ORGANIZATION_ID = "";
            SHIPMENT_STATUS = "";
            NEED_BY_DATE = "";
            REVISION_NUM = "";
            BILL_TO_ADDRESS1 = "";
            BILL_TO_ADDRESS2 = "";
            BILL_TO_CITY = "";
            BILL_TO_STATE = "";
            BILL_TO_ZIP = "";
            BILL_TO_COUNTRY = "";
            SHIP_TO_ADDRESS1 = "";
            SHIP_TO_ADDRESS2 = "";
            SHIP_TO_CITY = "";
            SHIP_TO_STATE = "";
            SHIP_TO_ZIP = "";
            SHIP_TO_COUNTRY = "";
            PAYMENT_TERMS = "";
            EXPENSE_ACCOUNT = "";

        }
        public string SOMAXPO_DAT { get; set; }
        public string SOMAX_REQUISITION_NUMBER { get; set; }
        public string SOMAX_REQUISITION_ID { get; set; }
        public string SOMAX_WORK_ORDER_REFERENCE { get; set; }
        public string ORACLE_REQUSITION_NUMBER { get; set; }
        public string PURCHASE_ORDER_NUMBER { get; set; }
        public string PURCHASE_ORDER_ID { get; set; }
        public string PO_CREATION_DATE { get; set; }
        public string PO_CURRENCY { get; set; }
        public string ORACLE_VENDOR_ID { get; set; }
        public string ORACLE_VENDOR_NUMBER { get; set; }
        public string ORACLE_VENDOR_SITE_ID { get; set; }
        public string SOMAX_REQ_LINE_ID { get; set; }
        public string ORACLE_PO_LINE_NUMBER { get; set; }
        public string ORACLE_PO_LINE_ID { get; set; }
        public string ORACLE_PART_ID { get; set; }
        public string ORACLE_PART_NUMBER { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string UNSPSC_ID_TREE { get; set; }
        public string QUANTITY { get; set; }
        public string PURCHASING_UNIT_OF_MEASURE { get; set; }
        public string UNIT_PRICE { get; set; }
        public string PRIMARY_UNIT_OF_MEASURE { get; set; }
        public string UOM_CONVERSION_RATE { get; set; }
        public string SHIP_TO_ORGANIZATION_ID { get; set; }
        public string SHIPMENT_STATUS { get; set; }
        public string NEED_BY_DATE { get; set; }
        public string REVISION_NUM { get; set; }
        public string BILL_TO_ADDRESS1 { get; set; }
        public string BILL_TO_ADDRESS2 { get; set; }
        public string BILL_TO_CITY { get; set; }
        public string BILL_TO_STATE { get; set; }
        public string BILL_TO_ZIP { get; set; }
        public string BILL_TO_COUNTRY { get; set; }
        public string SHIP_TO_ADDRESS1 { get; set; }
        public string SHIP_TO_ADDRESS2 { get; set; }
        public string SHIP_TO_CITY { get; set; }
        public string SHIP_TO_STATE { get; set; }
        public string SHIP_TO_ZIP { get; set; }
        public string SHIP_TO_COUNTRY { get; set; }
        public string PAYMENT_TERMS { get; set; }
        public string EXPENSE_ACCOUNT { get; set; }
    }
}