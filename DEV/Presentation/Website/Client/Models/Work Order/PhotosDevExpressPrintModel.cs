namespace Client.Models.Work_Order
{
    public class PhotosDevExpressPrintModel
    {
        public string PhotoUrl { get; set; }
        public string FileName { get; set; }
        public string FileType{ get; set; }
        public int? FileSize{ get; set; }
        #region Localization
        public string spnPhotos { get; set; }
        public string spnDetails { get; set; }
        #endregion Localization
    }
}