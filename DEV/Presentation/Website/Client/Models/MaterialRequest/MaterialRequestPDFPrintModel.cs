using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MaterialRequest
{
    public class MaterialRequestPDFPrintModel: MaterialRequestPrintModel
    {
        public MaterialRequestPDFPrintModel()
        {
            ChildModelList = new List<ChildGridModel>();
        }
        public List<ChildGridModel> ChildModelList { get; set; }
        public decimal Total { get; set; }
        public string CreateDateString { get; set; }
        public string RequiredDateString { get; set; }
        public string CompleteDateString { get; set; }
    }
}