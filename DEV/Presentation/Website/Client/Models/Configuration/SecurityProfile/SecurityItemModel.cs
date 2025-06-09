namespace Client.Models.Configuration.SecurityProfile
{
    public class SecurityItemModel
    {
        public long ClientId { get; set; }
        public long SecurityItemId { get; set; }
        public long SecurityProfileId { get; set; }
        public string ItemName { get; set; }
        public string SecurityLocalizedName { get; set; }
        public int SortOrder { get; set; }
        public bool Protected { get; set; }
        public bool SingleItem { get; set; }
        public bool ItemAccess { get; set; }
        public bool ItemCreate { get; set; }
        public bool ItemEdit { get; set; }
        public bool ItemDelete { get; set; }
        public int UpdateIndex { get; set; }
    }
}