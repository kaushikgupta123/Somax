using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.Configuration.Account
{
    public class NoteModel
    {
        public string ClientLookupId { get; set; }
        public long AccountID { get; set; }
        public long NotesId { get; set; }
        public string Subject { get; set; }
        public long OwnerId { get; set; }
        public long UpdateIndex { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string OwnerName { get; set; }
        [Required(ErrorMessage = "Please enter Content.")]
        public string Content { get; set; }
        public string Type { get; set; }
        public long ObjectId { get; set; }
        public string TableName { get; set; }

     }
}