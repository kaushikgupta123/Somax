using Common.Enumerations;

using Database.Business;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
   
        public class SanMasterBatchEntryTransactionsForSanitationMasterFromSanitationJob_V2 : AbstractTransactionManager
        {
            public long clientId { get; set; }
            public long siteid { get; set; }
            public string ScheduleType { get; set; }
            public DateTime ScheduleThroughDate { get; set; }
            public string OnDemandgroup { get; set; }
            public bool PrintSanitationJob { get; set; }
            public bool PrintAttachments { get; set; }
            public string AssetGroup1Ids { get; set; }
            public string AssetGroup2Ids { get; set; }
            public string AssetGroup3Ids { get; set; }
            

            public List<b_SanMasterBatchEntry> SanMasterBatchEntryList { get; set; }

            public SanMasterBatchEntryTransactionsForSanitationMasterFromSanitationJob_V2()
            {
                base.UseDatabase = DatabaseTypeEnum.Client;
            }


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
                base.UseTransaction = true;

                //base.Transaction.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

                SqlCommand command = null;
                string message = String.Empty;
                List<b_SanMasterBatchEntry> tempList = null;
                SanMasterBatchEntryList = new List<b_SanMasterBatchEntry>();

                try
                {

                b_SanMasterBatchEntry be = new b_SanMasterBatchEntry();
                    be.SanMasterBatchEntry_ForSanitationMasterFromSanitationJob_V2(this.Connection, this.Transaction,
                        CallerUserInfoId, CallerUserName, clientId, siteid, ScheduleType,
                        ScheduleThroughDate, OnDemandgroup, PrintSanitationJob, PrintAttachments, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids,
                        ref tempList);

                    if (tempList.Count > 0)
                    {
                        tempList.ForEach(x =>
                        {
                            SanMasterBatchEntryList.Add(x);
                        });
                    }

                }
                finally
                {
                    if (null != command)
                    {
                        command.Dispose();
                        command = null;
                    }

                    message = String.Empty;
                }

            }

        }
    public class SanMasterBatchEntryTransactionsForSanitationJobFromSanitationMasterChunkSearch_V2 : SanMasterBatchEntry_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanMasterBatchEntry.SanMasterBatchEntryId > 0)
            {
                string message = "SanMasterBatchEntryId has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_SanMasterBatchEntry tmpList = null;
            SanMasterBatchEntry.SanMasterBatchEntry_ForSanitationJobFromSanitationMasterChunkSearch_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

}
