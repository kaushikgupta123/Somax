using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models.PartsManagement.PartsManagementRequest
{
    public class PartsManagementAttachmentModel
    {
        public long ClientId { get; set; }
        public long AttachmentId { get; set; }
        public string ObjectName { get; set; }
        public long ObjectId { get; set; }
        public string AttachmentURL { get; set; }
        public long UploadedByPersonnelId { get; set; }
        //[Required(ErrorMessage = "Please enter description")]
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public bool Image { get; set; }
        public bool Profile { get; set; }
        public bool External { get; set; }
        public bool Reference { get; set; }
        public string ContentType { get; set; }
        public int FileSize { get; set; }
        public long FileAttachmentId { get; set; }
        public long FileInfoId { get; set; }
        public int UpdateIndex { get; set; }
        public DateTime? CreateDate { get; set; }
        public string FullName { get; set; }
        public string UploadedBy { get; set; }
        public bool IsEditable { get; set; }
        public string ModifiedBy { get; set; }
        public long? PartMasterRequestId { get; set; }
        public string ClientLookupId { get; set; }
        [Display(Name = "globalFileName|" + LocalizeResourceSetConstants.Global)]
       [RequiredIf("AttachmentId", "0", ErrorMessage = "atchSelectFile|" + LocalizeResourceSetConstants.Global)]
        [FileSizeValidation(10, ErrorMessage = "fileSizeError|" + LocalizeResourceSetConstants.Global)]
        public HttpPostedFile FileContent { get; set; }
        public string RequestType { get; set; }
    }
}