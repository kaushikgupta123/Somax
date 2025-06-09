using System.Collections.Generic;

namespace Client.Models.Personnel
{
    public class AssetGroupMasterQuery
    {
        public long PersonnelId { get; set; }
        public List<string> AssetGroup1Ids { get; set; }
        public List<string> AssetGroup2Ids { get; set; }
        public List<string> AssetGroup3Ids { get; set; }
    }
}