using Client.CustomValidation;
using Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Client.Models.MasterSanitationSchedule
{
    public class MasterSanitAttachmentModel
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        public string FileType { get; set; }
        public string ContentType { get; set; }
        [Display(Name = "atchFileName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "atchSelectFile|" + LocalizeResourceSetConstants.Global)]
        [FileSizeValidation(10, ErrorMessage = "fileSizeError|" + LocalizeResourceSetConstants.Global)]
        public HttpPostedFile FileContent { get; set; }
        [Display(Name = "globalSubject|" + LocalizeResourceSetConstants.Global)]
        public string Subject { get; set; }
        public long FileAttachmentId { get; set; }
        public long FileInfoId { get; set; }
        public string ClientLookupId { get; set; }
        public long SanitationJobId { get; set; }
        public long SanitationMasterId { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }
        public string OwnerName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string FileSizeWithUnit { get; set; }
        public DateTime? CreateDate { get; set; }

        public long ObjectId { get; set; }

        public string TableName { get; set; }        
    }
}