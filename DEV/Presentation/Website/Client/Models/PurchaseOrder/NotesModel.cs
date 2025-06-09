using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.PurchaseOrder
{
    public class NotesModel
    {
        public long NotesId { get; set; }
        [Display(Name = "globalSubject|" + LocalizeResourceSetConstants.Global)]
        public string Subject { get; set; }
        [Display(Name = "noteContent|" + LocalizeResourceSetConstants.Global)]
        [Required(ErrorMessage = "noteErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Content { get; set; }
        public string Type { get; set; }
        public long updatedindex { get; set; }
        public string ClientLookupId { get; set; }
        public long PurchaseOrderId { get; set; }

        public string OwnerName { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}