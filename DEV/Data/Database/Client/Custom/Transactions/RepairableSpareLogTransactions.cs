using Database.Business;

using System;
using System.Collections.Generic;

namespace Database
{
    public class RepairableSpareLog_RetrieveByEquipmentId:RepairableSpareLog_TransactionBaseClass
    {
        public List<b_RepairableSpareLog> RepairableSpareLogList { get; set; }

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
            List<b_RepairableSpareLog> tmpArray = null;

            RepairableSpareLog.RetrieveByEquipmentId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            RepairableSpareLogList = new List<b_RepairableSpareLog>();
            RepairableSpareLogList = tmpArray;

        }

    }



}
