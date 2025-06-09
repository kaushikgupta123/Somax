namespace Client.Models.PartsManagement.PartsReview
{
    public class PartsReviewModel
    {
        public long PartId { get; set; }
        public string ClientLookupId { get; set; }
        public string LongDescription { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public string Category { get; set; }
        public string CategoryMasterDescription { get; set; }
        public string ImageURL { get; set; } //V2-1215
    }
}