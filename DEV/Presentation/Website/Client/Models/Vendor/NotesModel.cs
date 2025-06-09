using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models
{
    public class NotesModel
    {
        public long NotesId { get; set; }
        [Display(Name = "globalSubject|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Subject { get; set; }
        [Display(Name = "VendorNotesContent|" + LocalizeResourceSetConstants.VendorDetails)]
        [Required(ErrorMessage = "VendorNoteErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Content { get; set; }
        public string Type { get; set; }
        public long VendorId { get; set; }
        public long updatedindex { get; set; }
        public string ClientLookupId { get; set; }
        public long EquipmentId { get; set; }
        public long PartId { get; set; }
        public long PrevMasterID { get; set; }
        public long WorkOrderId { get; set; }
        public string OwnerName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long OwnerId { get; set; }
        public string TableName { get; set; }
        public long ObjectId { get; set; }
        public long UserInfoId { get; set; }
        public long SanitationMasterId { get; set; }

        public string EqClientLookupId { get; set; }
    }
}