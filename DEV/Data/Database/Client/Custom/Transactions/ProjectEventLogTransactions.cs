using Database.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class ProjectEventLog_RetrieveByProjectId : ProjectEventLog_TransactionBaseClass
    {
        public List<b_ProjectEventLog> ProjectEventLogList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_ProjectEventLog> tmpArray = null;

            ProjectEventLog.RetrieveByProjectIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ProjectEventLogList = new List<b_ProjectEventLog>();
            foreach (b_ProjectEventLog tmpObj in tmpArray)
            {
                ProjectEventLogList.Add(tmpObj);
            }
        }
    }
}
