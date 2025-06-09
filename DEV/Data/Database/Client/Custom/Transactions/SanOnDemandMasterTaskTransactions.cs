using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    #region Task All Work
    public class SanitationJobTask_RetrieveBy_SanOnDemandMasterId : SanOnDemandMasterTask_TransactionBaseClass
    {
        public List<b_SanOnDemandMasterTask> SanOnDemandMasterTaskList { get; set; }
        public SanitationJobTask_RetrieveBy_SanOnDemandMasterId()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        //public List<b_SanitationJobTask> SanitationJobTaskList { get; set; }
        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_SanOnDemandMasterTask> tmpArray = null;

            SanOnDemandMasterTask.RetrieveAllBy_SanOnDemandMasterId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanOnDemandMasterTaskList = new List<b_SanOnDemandMasterTask>();
            foreach (b_SanOnDemandMasterTask tmpObj in tmpArray)
            {
                SanOnDemandMasterTaskList.Add(tmpObj);
            }
        }


    }
    #endregion
}
