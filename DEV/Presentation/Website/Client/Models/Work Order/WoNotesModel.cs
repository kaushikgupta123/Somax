using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Work_Order
{
    public class WoNotesModel
    {
        public long NotesId { get; set; }
        [Display(Name = "globalSubject|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Subject { get; set; }
        [Display(Name = "VendorNotesContent|" + LocalizeResourceSetConstants.VendorDetails)]
        [Required(ErrorMessage = "VendorNoteErrMsg|" + LocalizeResourceSetConstants.VendorDetails)]
        public string Content { get; set; }
        public string Type { get; set; }       
        public long updatedindex { get; set; }
        public string ClientLookupId { get; set; }
        public long WorkOrderId { get; set; }
        public string WoClientLookupId { get; set; }
    }
}