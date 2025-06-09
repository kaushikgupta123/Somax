using Database.Business;
using Database.Transactions;
using System.Collections.Generic;

namespace Database
{

    public class PMSchedAssign_RetriveByPMSchedId : PMSchedAssign_TransactionBaseClass
    {
        public List<b_PMSchedAssign> PMSchedAssignList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_PMSchedAssign> tmpArray = null;
            PMSchedAssign.RetrievePMSchedAssignSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            PMSchedAssignList = new List<b_PMSchedAssign>();
            foreach (b_PMSchedAssign tmpObj in tmpArray)
            {
                PMSchedAssignList.Add(tmpObj);
            }
        }

        public override void Postprocess()
        {
            base.Postprocess();
            //System.Diagnostics.Debug.Assert(PMSchedAssign.PMSchedAssignId == 0);
        }
    }

    #region V2-1204
    public class RetrieveByPMSchedId_V2 : PMSchedAssign_TransactionBaseClass
    {
        public List<b_PMSchedAssign> PMSchedAssignList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_PMSchedAssign> tmpArray = null;
            PMSchedAssign.RetrieveByPMSchedId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            PMSchedAssignList = new List<b_PMSchedAssign>();
            foreach (b_PMSchedAssign tmpObj in tmpArray)
            {
                PMSchedAssignList.Add(tmpObj);
            }
        }
    }
    #endregion
}
