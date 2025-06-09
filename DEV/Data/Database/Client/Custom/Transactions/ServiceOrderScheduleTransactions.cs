using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Client.Custom.Transactions
{
    public class ServiceOrderScheduleTransactions
    {
    }

    public class ServiceOrderSchdule_RetrievePersonnel : ServiceOrderSchedule_TransactionBaseClass
    {
        public List<List<b_ServiceOrderSchedule>> ServiceOrderSchedulePersonnelList { get; set; }
        public List<b_ServiceOrderSchedule> ServiceOrderScheduleList { get; set; }

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
            List<List<b_ServiceOrderSchedule>> tmpArray = null;

            ServiceOrderSchedule.RetrievePersonnel(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            ServiceOrderSchedulePersonnelList = new List<List<b_ServiceOrderSchedule>>();
            ServiceOrderScheduleList = new List<b_ServiceOrderSchedule>();
            foreach (List<b_ServiceOrderSchedule> tmpObj in tmpArray)
            {
                foreach (b_ServiceOrderSchedule tmpObj2 in tmpObj)
                {
                    ServiceOrderScheduleList.Add(tmpObj2);
                }
                ServiceOrderSchedulePersonnelList.Add(tmpObj);
            }


        }
    }
}
