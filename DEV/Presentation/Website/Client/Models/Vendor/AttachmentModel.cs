using Common.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Client.CustomValidation;
using System;

namespace Client.Models
{
    public class AttachmentModel
    {
        #region Attachment Table Properties
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string ContentType { get; set; }
        [Display(Name = "globalFileName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "VendorAttachmentFileExists|" + LocalizeResourceSetConstants.VendorDetails)]
        [FileSizeValidation(10, ErrorMessage = "fileSizeError|" + LocalizeResourceSetConstants.Global)]
        public HttpPostedFile FileContent { get; set; }
        [Display(Name = "globalSubject|" + LocalizeResourceSetConstants.Global)]
        public string Subject { get; set; }
       
        public List<HttpPostedFileBase> Files { get; set; }
        public long ObjectId { get; set; }
        public string TableName { get; set; }
        public long FileAttachmentId { get; set; }
        public long FileInfoId { get; set; }
        public string OwnerName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FileSizeWithUnit { get; set; }
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
        public long UserInfoId { get; set; }
        public long PersonnelId { get; set; }
        public string RequestType { get; set; }
        public string AttachmentUrl { get; set; }
        public string Description { get; set; }
        public bool Image { get; set; }
        public bool Profile { get; set; }
        public bool External { get; set; }
        public bool Reference { get; set; }
        public bool PrintwithForm { get; set; }
        #endregion

        #region External Tablewise ObjectId
        public string ClientLookupId { get; set; }
        public long VendorId { get; set; }       
        public long EquipmentId { get; set; }
        public long PartId { get; set; }
        public long PrevMaintMasterId { get; set; }
        public long WorkOrderId { get; set; }
        public long SanitationMasterId { get; set; }
        public long PartMasterRequestId { get; set; }
        public long ServiceOrderId { get; set; }
        #endregion
    }
}