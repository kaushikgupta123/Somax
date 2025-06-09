using Database.Business;
using Database.Transactions;

using System;
using System.Collections.Generic;

namespace Database.Transactions
{
    public class AppGroupRequestors_RetrieveChunkSearchForDetailsById : AppGroupRequestors_TransactionBaseClass
    {
        public List<b_AppGroupRequestors> AppGroupRequestorsList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (AppGroupRequestors.ApprovalGroupId > 0)
            //{
            //    string message = "ApprovalId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_AppGroupRequestors> tmpList = null;
            AppGroupRequestors.RetrieveChunkSearchFromDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
             AppGroupRequestorsList = new List<b_AppGroupRequestors>();
            foreach (b_AppGroupRequestors tmpObj in tmpList)
            {
                AppGroupRequestorsList.Add(tmpObj);
            }
        }
    }
    public class AppGroupRequestors_RetrieveByRequestorIdAndRequestType : AppGroupRequestors_TransactionBaseClass
    {
        public b_AppGroupRequestors AppGroupRequestorsData { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (AppGroupRequestors.ApprovalGroupId > 0)
            //{
            //    string message = "ApprovalId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_AppGroupRequestors tmpRecord = null;
            AppGroupRequestors.RetrieveByRequestorIdAndRequestType(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpRecord);
            AppGroupRequestorsData = tmpRecord;
        }
    }
}
