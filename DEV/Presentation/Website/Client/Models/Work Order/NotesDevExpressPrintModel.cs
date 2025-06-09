using System;

namespace Client.Models.Work_Order
{
    public class NotesDevExpressPrintModel
    {
        public long WorkOrderId { get; set; }
        public string Comments { get; set; }
        public string OwnerName { get; set; }
        public string CreateDate { get; set; }
        #region V2-944
        #region Localization
        public string SpnLogComment { get; set; } 
        public string spnCommenter { get; set; } 
        public string spnComment { get; set; } 
        public string spnDate { get; set; } 
        #endregion
        #endregion
    }
}