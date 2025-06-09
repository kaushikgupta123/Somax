using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Client.Models.Invoice
{
    public class AttachmentModel
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string ContentType { get; set; }
        [Required(ErrorMessage = "atchSelectFile|" + LocalizeResourceSetConstants.Global)]
        public HttpPostedFile FileContent { get; set; }
        public string Subject { get; set; }
        public long VendorId { get; set; }
        public string ClientLookupId { get; set; }
        public long EquipmentId { get; set; }
        public long PartId { get; set; }
        public long PrevMaintMasterId { get; set; }
        public long InvoiceId { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }      
        public string OwnerName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string FileSizeWithUnit { get; set; }
        public DateTime? CreateDate { get; set; }
        public long ObjectId { get; set; }
        public string TableName { get; set; }
        public long FileAttachmentId { get; set; }
        public long FileInfoId { get; set; }
    }
}