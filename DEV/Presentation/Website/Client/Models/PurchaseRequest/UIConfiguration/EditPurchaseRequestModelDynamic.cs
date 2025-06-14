﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Client.Models.PurchaseRequest.UIConfiguration
{
    public class EditPurchaseRequestModelDynamic
    {
        #region UDF columns
        public long PRHeaderUDFId { get; set; }
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

        #region Purchase Request table coulmn
        public long? PurchaseRequestId { get; set; }
        public string ClientLookupId { get; set; }
        public string Reason { get; set; }
        public long? ApprovedBy_PersonnelId { get; set; }
        public DateTime? Approved_Date { get; set; }
        public long? CreatedBy_PersonnelId { get; set; }
        public string Process_Comments { get; set; }
        public DateTime? Processed_Date { get; set; }
        public long? ProcessBy_PersonnelId { get; set; }
        public string Status { get; set; }
        public long? VendorId { get; set; }
        public long? PurchaseOrderId { get; set; }
        public bool AutoGenerated { get; set; }
        public string Return_Comments { get; set; }
        public long? ExtractLogId { get; set; }
        public long? ExRequestId { get; set; }
        public bool IsPunchOut { get; set; }
        public long? StoreroomId { get; set; }
        #endregion
        public string VendorName { get; set; }
        public string VendorClientLookupId { get; set; }
        public string Creator_PersonnelName { get; set; }
        public string Approved_PersonnelName { get; set; }
        public string Processed_PersonnelName { get; set; }
        public int? CountLineItem { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PurchaseOrderClientLookupId { get; set; }
        public string LocalizedStatus { get; set; }
        public int UpdateIndex { get; set; }       
        public string ViewName { get; set; }
        public string StoreroomName { get; set; }
    }
}