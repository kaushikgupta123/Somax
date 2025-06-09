using Database.Business;

namespace Database
{
   public class PartUDF_RetrieveByPartId:PartUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_PartUDF tmpobj = null;
            PartUDF.RetrieveByPartId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            PartUDF = tmpobj;
        }
    }
}
