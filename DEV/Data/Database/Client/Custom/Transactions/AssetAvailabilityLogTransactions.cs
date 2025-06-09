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
    class AssetAvailabilityLogTransactions
    {
    }

    public class AssetAvailabilityLog_LookupListBySearchCriteria : AbstractTransactionManager
    {
        public AssetAvailabilityLog_LookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public long ObjectId { get; set; }
        public string TransactionDate { get; set; }
        public string Event { get; set; }
        public long SiteId { get; set; }
        public string ReturnToService { get; set; }
        public string Reason { get; set; }
        public string ReasonCode { get; set; }

        public string PersonnelName { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_AssetAvailabilityLog> AssetAvailabilityLogList { get; set; }
        public int ResultCount { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                int tmp;

                AssetAvailabilityLogList = StoredProcedure.usp_AssetAvailabilityLog_LookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, SiteId,ObjectId,TransactionDate,Event,ReturnToService,Reason , ReasonCode, PersonnelName,
                      PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

                ResultCount = tmp;
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

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }
}
