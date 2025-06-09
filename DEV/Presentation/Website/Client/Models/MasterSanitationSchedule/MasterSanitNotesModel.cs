using Common.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.Models.MasterSanitationSchedule
{
    public class MasterSanitNotesModel
    {
        public DateTime? ModifiedDate { get; set; }
        public long UserInfoId { get; set; }
        public bool IsEditable { get; set; }
        public long NotesId { get; set; }
        public long OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string Subject { get; set; }
        [Required(ErrorMessage = "noteErrMsg|" + LocalizeResourceSetConstants.Global)]
        public string Content { get; set; }
        public string Type { get; set; }
        public long ObjectId { get; set; }
        public string TableName { get; set; }
        public long UpdateIndex { get; set; }
        public long SanitationMasterId { get; set; }
        public string Description { get; set; }
    }
}