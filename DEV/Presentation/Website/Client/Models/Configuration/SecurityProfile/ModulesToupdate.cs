namespace Client.Models.Configuration.SecurityProfile
{
    public class ModulesToupdate
    {
        public long securityprofileid { get; set; }
        public long securityitemid { get; set; }
        public int sortorder { get; set; }
        public string invmodule { get; set; }
        public bool access { get; set; }
        public bool create { get; set; }
        public bool edit { get; set; }
        public bool del { get; set; }
        public int updateindex { get; set; }
    }
}