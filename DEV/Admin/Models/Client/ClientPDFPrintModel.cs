using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Admin.Models.Client
{
    public class ClientPDFPrintModel: ClientPrintModel
    {
        public ClientPDFPrintModel()
        {
            ChildModelList = new List<ChildGridModel>();
        }
        public List<ChildGridModel> ChildModelList { get; set; }
    }
}