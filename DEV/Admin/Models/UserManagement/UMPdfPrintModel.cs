using System.Collections.Generic;

namespace Admin.Models.UserManagement
{
    public class UMPdfPrintModel:PrintUserModel
    {
        public UMPdfPrintModel()
        {
            innerGridUserManagement = new List<InnerGridUserManagement>();
        }
        public List<InnerGridUserManagement> innerGridUserManagement { get; set; }
    }
}