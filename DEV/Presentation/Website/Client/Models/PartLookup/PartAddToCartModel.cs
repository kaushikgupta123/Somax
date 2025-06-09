using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.PartLookup
{
    public class PartAddToCartModel: LocalisationBaseVM
    {
        public PartAddToCartModel()
        {
            lstpartAddToCartModels = new List<PartAddItemToCartModel>();
        }
        public List<PartAddItemToCartModel> lstpartAddToCartModels { get; set; }
    }
    public class PartAddItemToCartModel : LocalisationBaseVM
    {
        public string ImageUrl { get; set; }

        public string ClientLookUpId { get; set; }
        public string PartId { get; set; }

        public bool InVendorCatalog { get; set; }

        public string Description { get; set; }

        public decimal? PartQty { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal TotalUnitPrice { get; set; }
        public string PurchaseUnitofMeasure { get; set; }
        //V2-553
        public string PurchaseUOM { get; set; }
        public bool UOMConvRequired { get; set; }
        public decimal IssueOrder { get; set; }
        public long VendorCatalogItemId { get; set; }
        public DateTime RequiredDate { get; set; }
        public long PurchaseOrderId { get; set; }
        //V2-690
        public long VendorId { get; set; }
        public long PartCategoryMasterId { get; set; }
        public long indexid { get; set; }
        //V2-732
        public long StoreroomId { get; set; }
        public long PartStoreroomId { get; set; }
        public long AccountId { get; set; }
        public string UnitOfMeasure { get; set; }
    }


    public class PartAddToCartProcessModel
    {
        public long PurchaseRequestLineItemId { get; set; }
        public string PartId { get; set; }

        public long PurchaseRequestID { get; set; }

        public long PurchaseOrderID { get; set; }

        public int LineNumber { get; set; }

        public string ClientLookUpId { get; set; }

        public string Description { get; set; }

        public decimal OrderQuantity { get; set; }

        public decimal UnitCost { get; set; }

        public decimal TotalUnitCost { get; set; }

        public string UnitofMeasure { get; set; }
        //V2-553
        public string PurchaseUOM { get; set; }
        public bool UOMConvRequired { get; set; }
        public decimal IssueOrder { get; set; }
        public long VendorCatalogItemId { get; set; }
        public bool IsVendorCatalog { get; set; }
        public DateTime? RequiredDate { get; set; }
        public long WorkOrderID { get; set; }
        public long MaterialRequestId { get; set; }
        //V2-690
        public long VendorId { get; set; }
        public long PartCategoryMasterId { get; set; }
        //V2-732
        public long StoreroomId { get; set; }
        public long PartStoreroomId { get; set; }

        //V2-1068
        public long AccountId { get; set; }
        public string UnitOfMeasureIssue { get; set; }
        public long PreventiveMaintainId { get; set; }
    }
}