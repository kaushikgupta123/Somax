using Client.CustomValidation;

using Common.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Client.Models.Invoice
{
    public class InvoiceMatchHeaderModel
    {

        #region Index Search
        public long InvoiceMatchHeaderId { get; set; }


        [Required(ErrorMessage = "spnIdInvoiceisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public string ClientLookupId { get; set; }
        public string Status { get; set; }
        [RequiredIf("InvoiceMatchHeaderId","0", ErrorMessage = "spnIdSelectVendor|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }

        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "spnIdReceiptdateisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public DateTime? ReceiptDate { get; set; }

        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "spnIdPurchaseOrderIdisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public string POClientLookUpId { get; set; }

        public string Responsible { get; set; }//V2-981
        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "spnIdResponsibleIdisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public string ResponsibleWithClientLookupId { get; set; }//V2-981
        #endregion

        #region Details Page
        public long? ClientId { get; set; }
        public long? SiteId { get; set; }
        public long AreaId { get; set; }
        public long? DepartmentId { get; set; }
        public long StoreroomId { get; set; }

        public bool AuthorizedToPay { get; set; }
        public DateTime? AuthorizedToPayDate { get; set; }
        public long AuthorizedToPay_PersonnelId { get; set; }
        public long? Creator_PersonnelId { get; set; }
        [RequiredIf("InvoiceMatchHeaderId","0", ErrorMessage = "spnIdDuedateisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        [Display(Name = "spnIdDueDate|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public DateTime? DueDate { get; set; }
        public string OverrideCode { get; set; }
        public string OverrideComments { get; set; }


        public long? PurchaseOrderId { get; set; }


        public long? Responsible_PersonnelId { get; set; }

        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "spnIdShippingAmountisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999999.99, ErrorMessage = "GlobalValidDecimalNoWithMaximum2DecimalPlacesAnd15DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? ShipAmount { get; set; }

        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "spnIdTaxAmountisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999999.99, ErrorMessage = "GlobalValidDecimalNoWithMaximum2DecimalPlacesAnd15DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? TaxAmount { get; set; }

        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "spnIdInputAmountisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]
        [RegularExpression(@"^\d*\.?\d{0,2}$", ErrorMessage = "globalTwoDecimalRegErrMsg|" + LocalizeResourceSetConstants.Global)]
        [Range(0, 9999999999999.99, ErrorMessage = "GlobalValidDecimalNoWithMaximum2DecimalPlacesAnd15DigitsinTotal|" + LocalizeResourceSetConstants.Global)]
        public decimal? TotalInput { get; set; }        

        [RequiredIf("InvoiceMatchHeaderId", "0", ErrorMessage = "TypeErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Type { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }
        public long? VendorId { get; set; }
        [Display(Name = "spnIdInvoiceDate|" + LocalizeResourceSetConstants.InvoiceDetails)]
        public DateTime? InvoiceDate { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public long Paid_PersonnelId { get; set; }
        public long UpdateIndex { get; set; }

        
        public DateTime? CreateDate { get; set; }//V2-981
        public string CreateBy { get; set; }//V2-981

        public DateTime? ModifyDate { get; set; }//V2-981
        public string ModifyBy { get; set; }//V2-981
        public string AuthorizedToPayBy { get; set; }//V2-981
        public string PaidBy { get; set; }//V2-981

        //public string Responsible { get; set; }
        #endregion


        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string Assigned { get; set; }
        public string Status_Display { get; set; }
        public int PersonnelId { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal Total { get; set; }
        public decimal Variance { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public int NumberOfLineItems { get; set; }
        public string Flag { get; set; }
        public IEnumerable<SelectListItem> VendorList { get; set; }
        public IEnumerable<SelectListItem> POList { get; set; }


        public bool OpenStatusSecurity { get; set; }
        public bool AuthorisedToPayStatusSecurity { get; set; }
        public bool SecurityInvoicePaid { get; set; }
        public bool SecurityInvoiceEdit { get; set; }
        public Int32 ChildCount { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropListForInvoiceCreatedate { get; set; }

    }
    public class ChangeInvoiceModel
    {
        [Required(ErrorMessage = "spnIdInvoiceisrequired|" + LocalizeResourceSetConstants.InvoiceDetails)]

        public string ClientLookupId { get; set; }
        public long invoiceId { get; set; }
        public string oldClientLookupId { get; set; }

    }


}