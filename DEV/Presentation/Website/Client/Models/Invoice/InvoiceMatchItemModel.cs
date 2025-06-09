using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Invoice
{
    public class InvoiceMatchItemModel
    {
        public InvoiceMatchItemModel()
        {
            UnitOfMeasureList = new List<SelectListItem>();
            AccountList = new List<SelectListItem>();
        }
        public Int64 LineNumber { get; set; }

        //[Required(ErrorMessage = "InvoiceQtyErrMsg|" + LocalizeResourceSetConstants.InvoiceDetails)]
        [Range(0, 9999999999.99999, ErrorMessage = "InvoiceInputErrMsg|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public decimal? Quantity { get; set; }

        //[Required(ErrorMessage = "InvoiceUnitErrMsg|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public string UnitOfMeasure { get; set; }

        //[Required(ErrorMessage = "InvoiceUnitCostErrMsg|" + LocalizeResourceSetConstants.InvoiceDetails)]
        [Range(0, 9999999999.99999, ErrorMessage = "InvoiceInputErrMsg|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public decimal? UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string PurchaseOrder { get; set; }
        public string Account { get; set; }
        public string Description { get; set; }
        public long ClientId { get; set; }
        public long Creator_PersonnelId { get; set; }
        public long InvoiceMatchHeaderId { get; set; }
        public long InvoiceMatchItemId { get; set; }
        //[Required(ErrorMessage = "InvoiceAcctErrMsg|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public long? AccountId { get; set; }
        public long POReceiptItemID { get; set; }
        public bool IsValid { get; set; }
        public string Status_Display { get; set; }
        public long StoreroomId { get; set; }
        public IEnumerable<SelectListItem> UnitOfMeasureList { get; set; }
        public IEnumerable<SelectListItem> AccountList { get; set; }

    }
}