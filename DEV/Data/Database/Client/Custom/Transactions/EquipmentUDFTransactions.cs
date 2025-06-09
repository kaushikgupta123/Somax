using Database.Business;

namespace Database
{
    public class EquipmentUDF_RetrieveByEquipmentId : EquipmentUDF_TransactionBaseClass
    {       
        public override void PerformWorkItem()
        {
            b_EquipmentUDF tmpobj = null;
            EquipmentUDF.RetrieveByEquipmentId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            EquipmentUDF = tmpobj;
        }
    }
}
