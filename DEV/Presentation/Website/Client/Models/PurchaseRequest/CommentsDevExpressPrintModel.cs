using System;

namespace Client.Models.PurchaseRequest
{
    public class CommentsDevExpressPrintModel
    {
        public long PurchaseRequestId { get; set; }
        public string Comments { get; set; }
        public string OwnerName { get; set; }
        public string CreateDate { get; set; }
        #region Localization
        public string spnGlobalNote { get; set; } 
        public string globalOwner { get; set; } 
        public string noteContent { get; set; } 
        public string spnDate { get; set; } 
        #endregion
    }
}