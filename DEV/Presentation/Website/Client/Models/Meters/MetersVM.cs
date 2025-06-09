using DataContracts;

namespace Client.Models.Meters
{
    public class MetersVM : LocalisationBaseVM
    {
        public MetersModel Meters { get; set; }
        public MetersReadingModel Readings { get; set; }
        public Security security { get; set; }
        public MetersResetModel metersResetModel { get; set; }
        public QRCodeModel qRCodeModel { get; set; }
    }
}