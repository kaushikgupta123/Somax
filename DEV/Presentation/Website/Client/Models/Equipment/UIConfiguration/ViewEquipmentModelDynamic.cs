using System;

namespace Client.Models.Equipment.UIConfiguration
{
    public class ViewEquipmentModelDynamic 
    {
        #region UDF columns
        public long EquipmentUDFId { get; set; }
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
        public decimal Numeric1 { get; set; }
        public decimal Numeric2 { get; set; }
        public decimal Numeric3 { get; set; }
        public decimal Numeric4 { get; set; }
        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }
        public string Select4 { get; set; }
        #endregion

        #region Equipment table columns
        public long EquipmentId { get; set; }
        public string ClientLookupId { get; set; }
        public decimal AcquiredCost { get; set; }
        public DateTime? AcquiredDate { get; set; }
        public string CatalogNumber { get; set; }
        public string Category { get; set; }
        public string CostCenter { get; set; }
        public bool InactiveFlag { get; set; }
        public bool CriticalFlag { get; set; }
        public DateTime? InstallDate { get; set; }
        public string Location { get; set; }
        public long? Maint_VendorId { get; set; }
        public string Maint_WarrantyDesc { get; set; }
        public DateTime? Maint_WarrantyExpire { get; set; }
        public string Make { get; set; }
        public long? Material_AccountId { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public bool NoPartXRef { get; set; }
        public decimal OriginalValue { get; set; }
        public DateTime? OutofService { get; set; }
        public long? Purch_VendorId { get; set; }
        public string Purch_WarrantyDesc { get; set; }
        public DateTime? Purch_WarrantyExpire { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string AssetNumber { get; set; }
        public string AssetCategory { get; set; }
        public long? AssetGroup1 { get; set; }
        public long? AssetGroup2 { get; set; }
        public long? AssetGroup3 { get; set; }
        public long? Labor_AccountId { get; set; }
        public long? ParentId { get; set; }
        public string BusinessGroup { get; set; }
        public long? ElectricalParent { get; set; }
        #endregion

        public string Purch_VendorName { get; set; }
        public string Maint_VendorName { get; set; }
        public string Material_AccountName { get; set; }
        public string Labor_AccountName { get; set; }
        public string AssetGroup1Name { get; set; }
        public string AssetGroup2Name { get; set; }
        public string AssetGroup3Name { get; set; }
        public string ParentIdName { get; set; }
        public string Select1Name { get; set; }
        public string Select2Name { get; set; }
        public string Select3Name { get; set; }
        public string Select4Name { get; set; }
        public string ElectricalParent_ClientLookupId { get; set; }
    }
}