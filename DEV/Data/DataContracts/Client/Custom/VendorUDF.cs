using Database;
using Database.Business;

namespace DataContracts
{
    public partial class VendorUDF : DataContractBase
    {
        #region Variables
        public string Select1Name { get; set; }
        public string Select2Name { get; set; }
        public string Select3Name { get; set; }
        public string Select4Name { get; set; }
        #endregion

        public VendorUDF RetrieveByVendorId(DatabaseKey dbKey)
        {
            VendorUDF_RetrieveByVendorId trans = new VendorUDF_RetrieveByVendorId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.VendorUDF = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            VendorUDF objEquipmentUDF = new VendorUDF();
            objEquipmentUDF.UpdateFromDatabaseObjectExtended(trans.VendorUDF);
            return objEquipmentUDF;
        }
        public void UpdateFromDatabaseObjectExtended(b_VendorUDF dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.VendorUDFId = dbObj.VendorUDFId;
            this.VendorId = dbObj.VendorId;
            this.Text1 = dbObj.Text1;
            this.Text2 = dbObj.Text2;
            this.Text3 = dbObj.Text3;
            this.Text4 = dbObj.Text4;
            this.Date1 = dbObj.Date1;
            this.Date2 = dbObj.Date2;
            this.Date3 = dbObj.Date3;
            this.Date4 = dbObj.Date4;
            this.Bit1 = dbObj.Bit1;
            this.Bit2 = dbObj.Bit2;
            this.Bit3 = dbObj.Bit3;
            this.Bit4 = dbObj.Bit4;
            this.Numeric1 = dbObj.Numeric1;
            this.Numeric2 = dbObj.Numeric2;
            this.Numeric3 = dbObj.Numeric3;
            this.Numeric4 = dbObj.Numeric4;
            this.Select1 = dbObj.Select1;
            this.Select2 = dbObj.Select2;
            this.Select3 = dbObj.Select3;
            this.Select4 = dbObj.Select4;
            this.Select1Name = dbObj.Select1Name;
            this.Select2Name = dbObj.Select2Name;
            this.Select3Name = dbObj.Select3Name;
            this.Select4Name = dbObj.Select4Name;

            AuditEnabled = true;
        }
    }
}
