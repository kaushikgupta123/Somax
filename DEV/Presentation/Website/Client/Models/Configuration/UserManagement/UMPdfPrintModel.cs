using System.Collections.Generic;

namespace Client.Models.Configuration.UserManagement
{
    public class UMPdfPrintModel:PrintUserModel
    {
        public UMPdfPrintModel()
        {
            innerGridCrafts = new List<InnerGridCraft>();
        }
        public List<InnerGridCraft> innerGridCrafts { get; set; }
    }
}