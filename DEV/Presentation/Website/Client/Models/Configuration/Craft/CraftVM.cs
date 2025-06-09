using Client.Models.Configuration.Craft;

namespace Client.Models.Configuration.ConfigCraft
{
    public class CraftVM : LocalisationBaseVM
    {
        public CraftModel craftModel { get; set; }
        public CraftPrintModel craftPrintModel { get; set; }
    }
}