using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.MultiStoreroomPart.UIConfiguration
{
    public class AddPartModelDynamic
    {
        #region UDF columns
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }
        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public bool Bit1 { get; set; }
        public bool Bit2 { get; set; }
        public bool Bit3 { get; set; }
        public bool Bit4 { get; set; }
        public decimal? Numeric1 { get; set; }
        public decimal? Numeric2 { get; set; }
        public decimal? Numeric3 { get; set; }
        public decimal? Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion

        #region Part Table Columns
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string UPCCode { get; set; }
        public long? AccountId { get; set; }
        public string StockType { get; set; }
        public string IssueUnit { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerId { get; set; }
        public bool Critical { get; set; }
        public string MSDSContainerCode { get; set; }
        public string MSDSPressureCode { get; set; }
        public string MSDSReference { get; set; }
        public bool MSDSRequired { get; set; }
        public string MSDSTemperatureCode { get; set; }
        public bool NoEquipXref { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? AppliedCost { get; set; }
        public bool Consignment { get; set; }
        public bool AutoPurch { get; set; }
        #endregion

        #region PartStoreroom Table Columns
        public string Location1_5 { get; set; }
        public string Location1_1 { get; set; }
        public string Location1_2 { get; set; }
        public string Location1_3 { get; set; }
        public string Location1_4 { get; set; }
        public int? CountFrequency { get; set; }
        public DateTime? LastCounted { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal QtyMaximum { get; set; }
        public decimal QtyReorderLevel { get; set; }
        
        #endregion

        public string AccountClientLookupId { get; set; }
        public decimal LastPurchaseCost { get; set; }
        public decimal QtyOnRequest { get; set; }

        #region V2-1203
        public bool Copy_Equipment_Xref { get; set; }
        public bool Copy_Vendor_Xref { get; set; }
        public bool Copy_Notes { get; set; }
        #endregion

    }
}