using System;

namespace InterfaceAPI.Models.BBU.PartCategoryMasterImport
{
    public class PartCategoryMasterImportModel
    {
        public PartCategoryMasterImportModel()
        {
            ClientId = 0;
            PartCategoryMasterImportId = 0;
            ClientLookupId = "";
            Description = "";
            InactiveFlag = false;
        }
        public Int64 ClientId { get; set; }
        public Int64 PartCategoryMasterImportId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
    }
}