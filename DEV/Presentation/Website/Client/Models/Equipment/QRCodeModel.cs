using System.Collections.Generic;
namespace Client.Models
{
    public class QRCodeModel
    {
        public string QRCodeText { get; set; }
        public string QRCodeImagePath { get; set; }
        public List<string> EquipmentIdsList { get; set; }
        public List<string> QRCodeImageLists { get; set; }
        public List<string> PartIdsList { get; set; }
        public List<string> MeterIdsList { get; set; }
    }
}