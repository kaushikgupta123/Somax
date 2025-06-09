using System.Collections.Generic;

namespace Client.Models
{
    public class LookUpListModel
    {
        public LookUpListModel()
        {
            data = new List<DataModel>();
        }
        public string msg { get; set; }
        public long count { get; set; }
        public List<DataModel> data { get; set; }
    }
    public class DataModel
    {
        public string Name { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public long ChargeToId { get; set; }
        #region  Account
        public long AccountId { get; set; }
        public string Account { get; set; }
        #endregion
        #region Vendor
        public long VendorId { get; set; }
        public string Vendor { get; set; }
        #endregion
        #region Equipment
        public long EquipmentId { get; set; }
        public string Equipment { get; set; }
        #endregion 
        #region Parts
        public long PartId { get; set; }
        public string Part { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public string StockType { get; set; }
        public string UPCCode { get; set; }
        public string Description { get; set; }
        public bool InactiveFlag { get; set; }
        #endregion
        #region Personnel
        public long PersonnelId { get; set; }
        public string Personnel { get; set; }
        public long AssignedTo_PersonnelId { get; set; }
        public string AssignedTo_PersonnelClientLookupId { get; set; }
        public string Email { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameFull { get; set; }
        public bool Buyer { get; set; }
        #endregion
        #region WorkOrder
        public long WorkOderId { get; set; }
        public string WorkOder { get; set; }
        public string ClientLookUpId { get; set; }
        public string Status { get; set; }
        #endregion
        #region Meter
        public long MeterId { get; set; }
        public string Meter_ClientLookupId { get; set; }
        public decimal ReadingCurrent { get; set; }

        #endregion
        #region Location
        public long LocationId { get; set; }
        public string LocationClientLookupId { get; set; }
        public string Complex { get; set; }
        public string Type { get; set; }

        #endregion
        #region Craft
        public long CraftId { get; set; }
        public string Craft { get; set; }
        public decimal ChargeRate { get; set; }
        #endregion

        #region Purchaseorder

        public long PurchaseOrderId { get; set; }
        public string POClientLookupId { get; set; }
        #endregion

        #region V2-726 AppGroupApprovers
        public long ApproverId { get; set; }
        public string ApproverName { get; set; }
        #endregion
        public long ShipToId { get; set; } //V2-1086
    }
}