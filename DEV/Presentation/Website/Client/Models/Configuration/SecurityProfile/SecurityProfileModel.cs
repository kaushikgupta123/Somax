namespace Client.Models.Configuration.SecurityProfile
{
    public class SecurityProfileModel
    {
        public long ClientId { get; set; }
        public long SecurityProfileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool Protected { get; set; }
        public int UpdateIndex { get; set; }

    }
}