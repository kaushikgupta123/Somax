using Database;
using Database.Business;
using Database.Client.Custom.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class VendorInsurance
    {
        #region Property
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public int TotalCount { get; set; }
        public long Vendor_InsuranceSource { get;set; }
        #endregion

        public List<VendorInsurance> RetrieveChunkSearchByVendorId(DatabaseKey dbKey)
        {
            VendorInsurance_RetrieveChunkSearchByVendorId trans = new VendorInsurance_RetrieveChunkSearchByVendorId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.VendorInsurance = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<VendorInsurance> VendorInsurancelist = new List<VendorInsurance>();

            foreach (b_VendorInsurance VendorInsurance in trans.VendorInsuranceList)
            {
                VendorInsurance tmpVendorInsurance = new VendorInsurance();
                tmpVendorInsurance.UpdateFromDatabaseObjectForRetriveAllForSearch(VendorInsurance);
                VendorInsurancelist.Add(tmpVendorInsurance);
            }
            return VendorInsurancelist;
        }

        public b_VendorInsurance ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_VendorInsurance dbObj = new b_VendorInsurance();
            dbObj.ClientId = this.ClientId;
            dbObj.VendorId = this.VendorId;

            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_VendorInsurance dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.VendorInsuranceId = dbObj.VendorInsuranceId;
            this.Company = dbObj.Company;
            this.Contact = dbObj.Contact;
            this.ExpireDate = dbObj.ExpireDate;
            this.Amount = dbObj.Amount;
            this.Vendor_InsuranceSource = dbObj.Vendor_InsuranceSource;
            this.TotalCount = dbObj.TotalCount;
        }

    }
}
