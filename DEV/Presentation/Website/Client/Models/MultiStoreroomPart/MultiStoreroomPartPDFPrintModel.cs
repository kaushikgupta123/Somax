using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Client.Models.MultiStoreroomPart
{
    public class MultiStoreroomPartPDFPrintModel:MultiStoreroomPartPrintModel
    {
        public MultiStoreroomPartPDFPrintModel()
        {
            ChildModelList = new List<MultiStoreroomPartChildModel>();
        }
        public List<MultiStoreroomPartChildModel> ChildModelList { get; set; }
    }
}