using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Invoice
{
    public class InvoiceNoteModel
    {
        public long NotesId { get; set; }
        public string Subject { get; set; }
        [Required(ErrorMessage = "noteErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Content { get; set; }
        public string Type { get; set; }
        public long updatedindex { get; set; }
        public string ClientLookupId { get; set; }
        public long InvoiceId { get; set; }
        public string OwnerName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long OwnerId { get; set; }
        public string TableName { get; set; }
        public long ObjectId { get; set; }

    }
}