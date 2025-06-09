using System.Collections.Generic;

namespace Client.Models.PurchaseApproval
{
    public class PurchaseApprovalVM : LocalisationBaseVM
    {
        public PurchaseApprovalModel purchaseApprovalModel { get; set; }
        public PRLineItemModel lineItem { get; set; }

        public List<PRLineItemModel> LineItemList { get; set; }

        #region V2-820
        public bool IncludePRReview { get; set; }
        public bool ShoppingCartIncludeBuyer { get; set; }
        #endregion

    }
}