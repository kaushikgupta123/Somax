using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Common.Enumerations;
using Database.Business;

namespace Database
{

    public class SanitationMasterTask_RetrieveBySanitationMasterId : SanitationMasterTask_TransactionBaseClass
    {
        public List<b_SanitationMasterTask> SanitationMasterTaskList { get; set; }
        public long ClientId { get; set; }
        public long SanitationMasterId { get; set; }

        

        public override void Preprocess()
        {
           // throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_SanitationMasterTask> tmpList = new List<b_SanitationMasterTask>();
            SanitationMasterTaskList = new List<b_SanitationMasterTask>();

            SanitationMasterTask.SanitationMasterTask_RetrieveBySanitationMasterId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ClientId, SanitationMasterId, ref tmpList);

            SanitationMasterTaskList = tmpList;
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }
    }


    
}
