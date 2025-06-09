using Client.CustomValidation;
using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Client.Models.Configuration.Account
{
    public class AttachmentModel
    {
        public string ClientLookupId { get; set; }
        public long AccountID { get; set; }
        [Display(Name = "globalFileName|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "VendorAttachmentFileExists|" + LocalizeResourceSetConstants.VendorDetails)]
        [FileSizeValidation(10, ErrorMessage = "fileSizeError|" + LocalizeResourceSetConstants.Global)]
        public HttpPostedFile FileContent { get; set; }
        public string Subject { get; set; }

        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
        public string FileSizeWithUnit { get; set; }
        public string OwnerName { get; set; }
        public long FileAttachmentId { get; set; }
        public long FileInfoId { get; set; }
        public long OwnerId { get; set; }
        public long ObjectId { get; set; }
        public string TableName { get; set; }    
        public long UserInfoId { get; set; }
        public bool IsEditable { get; set; }
    }
}