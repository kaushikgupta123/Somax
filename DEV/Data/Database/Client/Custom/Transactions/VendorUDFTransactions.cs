using Database.Business;

namespace Database
{
    public class VendorUDF_RetrieveByVendorId : VendorUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_VendorUDF tmpobj = null;
            VendorUDF.RetrieveByVendorId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            VendorUDF = tmpobj;
        }
    }
}
