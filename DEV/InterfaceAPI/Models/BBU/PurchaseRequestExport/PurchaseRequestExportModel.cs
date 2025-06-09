using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.Models.BBU.PurchaseRequestExport
{
    public class PurchaseRequestExportModel
    {
        public PurchaseRequestExportModel()
        {            
            SOMAXRequisitionNumber = "";
            SOMAXRequisitionID = 0;
            SOMAXRequisitionDescription = "";
            OracleVendorID = 0;
            OracleVendorSiteId = 0;
            RequestedBy = "";
            SOMAXRequisitionLineItemId = 0;
            SOMAXRequisitionLineNumber = 0;
            OraclePlantId = "";
            NeedByDate = "";
            OraclePartID = 0;
            OraclePartNumber = "";
            OracleSourceDocumentId = 0;
            OracleSourceDocumentNumber = "";
            OracleSourceDocumentLineId = 0;
            SourceDocumentLineNumber = 0;
            UNSPSCCodeIDTree = "";
            LineDescription = "";
            OrderQuantity = 0;
            PurchasingUOM = "";
            UnitCost = 0;
            ExpenseAccount = "";


        }
        public string SOMAXREQ_DAT
        {
            get { return "SOMAXREQ-DAT".PadRight(15); }
        }
        public string SOMAXRequisitionNumber { get; set; }
        public Int64 SOMAXRequisitionID { get; set; }
        public string SOMAXRequisitionDescription { get; set; }
        public Int64 OracleVendorID { get; set; }
        public Int64 OracleVendorSiteId { get; set; }
        public string RequestedBy { get; set; }
        public Int64 SOMAXRequisitionLineItemId { get; set; }
        public int SOMAXRequisitionLineNumber { get; set; }
        public string OraclePlantId { get; set; }
        public string NeedByDate { get; set; }
        public Int64 OraclePartID { get; set; }
        public string OraclePartNumber { get; set; }
        public Int64 OracleSourceDocumentId { get; set; }
        public string OracleSourceDocumentNumber { get; set; }
        public Int64 OracleSourceDocumentLineId { get; set; }
        public int SourceDocumentLineNumber { get; set; }
        public string UNSPSCCodeIDTree { get; set; }
        public string LineDescription { get; set; }
        public decimal OrderQuantity { get; set; }
        public string PurchasingUOM { get; set; }
        public decimal UnitCost { get; set; }
        public string ExpenseAccount { get; set; }

    }
}