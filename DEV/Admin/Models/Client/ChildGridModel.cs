
namespace Admin.Models.Client
{
    public class ChildGridModel
    {
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool APM { get; set; }
        public bool CMMS { get; set; }
        public bool Sanitation { get; set; } 
        public int UpdateIndex { get; set; } 
    }
}