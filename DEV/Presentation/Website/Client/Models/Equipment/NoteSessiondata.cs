namespace Client.Models
{
    public class NoteSessiondata
    {
        public long NotesId { get; set; }
        public string ClientLookupId { get; set; }
        public string Subject { get; set; }
        public string OwnerName { get; set; }
        public string Content { get; set; }
        public long UpdateIndex { get; set; }
       
    }
}