using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTDataLayer.EL
{
    public class EquipmentEL
    {
        public Int64 ReportId
        { set; get; }
        public Int64 ClientId
        { set; get; }
        public Int64 UserInfoId
        { set; get; }
        public string UserName
        { set; get; }
        public Int64 EquipmentId
        { set; get; }
        public Int64 SiteId
        { set; get; }
        public Int64 AreaId
        { set; get; }
        public Int64 DepartmentId
        { set; get; }
        public Int64 StoreroomId
        { set; get; }
        public string ClientLookupId
        { set; get; }
        public decimal AcquiredCost
        { set; get; }
        public DateTime AcquiredDate
        { set; get; }
        public decimal BookValue
        { set; get; }
        public DateTime InstallDate
        { set; get; }
        public Int64 Labor_AccountId
        { set; get; }
        public int LifeinMonths
        { set; get; }
        public Int32 LifeinYears
        { set; get; }
        public string Location
        { set; get; }
        public Int64 LocationId
        { set; get; }
        public Int64 Maint_VendorId
        { set; get; }
        public string Maint_WarrantyDesc
        { set; get; }
        public DateTime Maint_WarrantyExpire
        { set; get; }
        public string Make
        { set; get; }
        public Int64 Material_AccountId
        { set; get; }
        public string Model
        { set; get; }
        public string Name
        { set; get; }
        public decimal OriginalValue
        { set; get; }
        public DateTime OutofService
        { set; get; }
        public Int64 ParentId { get; set; }
        public Int64 PartId { get; set; }
        public Int64 Purch_VendorId { get; set; }
        public string Purch_WarrantyDesc { get; set; }
        public DateTime Purch_WarrantyExpire
        { set; get; }
        public string SerialNumber
        { set; get; }
        public string Status
        { set; get; }
        public string Type
        { set; get; }
        public DateTime CreateDate
        { set; get; }
      
           
    }
}
