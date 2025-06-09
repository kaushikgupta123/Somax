using DataContracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.Models.Invoice
{
    public class InvoiceVM : LocalisationBaseVM
    {
        public InvoiceVM()
        {
            InvoiceMatchItemModelList = new List<InvoiceMatchItemModel>();
        }
        public InvoiceMatchHeaderModel InvoiceMatchHeaderModel { get; set; }
        public InvoiceMatchItemModel InvoiceMatchItemModel { get; set; }
        public InvoiceGridListModel ReceiptGridList { get; set; }
        public IEnumerable<SelectListItem> scheduleInvoiceList { get; set; }
        public InvoiceNoteModel NotesModel { get; set; }
        public AttachmentModel AttachmentModel { get; set; }
        public ChangeInvoiceModel changeInvoiceModel { get; set; }
        public Security security { get; set; }
        public UserData udata { get; set; }
        public int attachmentCount { get; set; }
        public List<InvoiceMatchItemModel> InvoiceMatchItemModelList { get; set; }
        public IEnumerable<SelectListItem> DateRangeDropList { get; set; }//V2-373
        public IEnumerable<SelectListItem> DatePaidRangeDropList { get; set; }//V2-373
        public IEnumerable<SelectListItem> StatusList { get; set; }
    }

}