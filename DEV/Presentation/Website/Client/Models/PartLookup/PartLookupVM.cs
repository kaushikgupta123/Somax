using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartLookup
{
    public class PartLookupVM : LocalisationBaseVM
    {
        public PartLookupVM()
        {
            partLookupModels = new List<PartLookupModel>();
        }
        public List<PartLookupModel> partLookupModels { get; set; }
        public PartLookupModel partLookup { get; set; }
        public long PurchaseRequestId { get; set; }
        public long PurchaseOrderId { get; set; }

        public string ClientLookupId { get; set; }
        public long VendorId { get; set; }
        public List<AdditionalCatalogItemModel> additionalCatalogItemlist { get; set; }
        public bool ShoppingCart { get; set; }

        public string Status { get; set; }
        public long WorkOrderID { get; set; }
        public long MaterialRequestId { get; set; }
        public string ModeForRedirect { get; set; }
        public long? StoreroomId { get; set; }//V2-732
        #region V2-894
        public List<LineItemOnOrderCheckModel> lineItemOnOrderCheckModel { get; set; }
        public bool IsOnOderCheck { get; set; }
        #endregion
        #region V2-1151
        public long PreventiveMaintainId { get; set; } 
        public string PMClientLookupId { get; set; }
        #endregion

    }
}