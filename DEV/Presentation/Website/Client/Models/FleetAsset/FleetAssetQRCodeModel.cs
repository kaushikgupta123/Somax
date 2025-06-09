using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class FleetAssetQRCodeModel
    {
        public string QRCodeText { get; set; }
        public string QRCodeImagePath { get; set; }
        public List<string> EquipmentIdsList { get; set; }
        public List<string> QRCodeImageLists { get; set; }
        public List<string> PartIdsList { get; set; }
        public List<string> MeterIdsList { get; set; }
    }
}