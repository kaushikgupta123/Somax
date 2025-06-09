using Database.Business;

namespace Database
{
    public class ProjectUDF_RetrieveByProjectId : ProjectUDF_TransactionBaseClass
    {
        public override void PerformWorkItem()
        {
            b_ProjectUDF tmpobj = null;
            ProjectUDF.RetrieveByProjectId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            ProjectUDF = tmpobj;
        }
    }
}
