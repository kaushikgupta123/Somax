using System;

namespace Client.Models.PurchaseOrder
{
    public class ReviewLogModel
    {
        public long ReviewLogId { get; set; }
        public string TableName { get; set; }
        public long ObjectId { get; set; }
        public string Function { get; set; }
        public long PersonnelId { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string Comments { get; set; }
        public string Reviewed_By { get; set; }
        public string Function_Display { get; set; }
    }
}