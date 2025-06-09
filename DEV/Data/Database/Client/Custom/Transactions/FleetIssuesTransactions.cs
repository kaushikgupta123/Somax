
using Common.Enumerations;
using Common.Structures;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class FleetIssues_RetrieveFleetIssueChunkSearchV2 : FleetIssues_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (FleetIssues.FleetIssuesId > 0)
            {
                string message = "FleetIssues has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_FleetIssues tmpList = null;
            FleetIssues.RetrieveFleetIssueChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class FleetIssues_RetrieveByFleetIssuesIdFromDatabase : FleetIssues_TransactionBaseClass
        {

            public override void PerformLocalValidation()
            {
                base.UseTransaction = false;
                base.PerformLocalValidation();

            }

            public override void PerformWorkItem()
            {
                FleetIssues.RetrieveByFleetIssuesIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            }
        }


    public class FleetIssue_LookupListBySearchCriteria : AbstractTransactionManager
    {
        public FleetIssue_LookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Defects { get; set; }
        public long SiteId { get; set; }
        //public string Type { get; set; }
        //public string SerialNumber { get; set; }
       
        public string Date { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        public Int64 EquipmentId { get; set; }

        // Result Sets
        public List<b_FleetIssues> FleetIssuesList { get; set; }
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

                FleetIssuesList = StoredProcedure.usp_FleetIssue_LookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, SiteId, EquipmentId, Date, Description, Status, Defects, 
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

    #region Fleet Only
    public class FleetIssues_RetrieveDashboardChart : FleetIssues_TransactionBaseClass
    {
        public List<b_FleetIssues> FleetIssuesList { get; set; }

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
            List<b_FleetIssues> tmpArray = null;

            FleetIssues.FleetIssues_RetrieveDashboardChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            FleetIssuesList = new List<b_FleetIssues>();
            foreach (b_FleetIssues tmpObj in tmpArray)
            {
                FleetIssuesList.Add(tmpObj);
            }
        }
    }

    #endregion

    #region Retrieve By Equipment Id
    public class FleetIssues_RetrieveFleetIssueByEquipmentIdV2 : FleetIssues_TransactionBaseClass
    {
        public List<b_FleetIssues> FleetIssuesList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (FleetIssues.EquipmentId == 0)
            {
                string message = "FleetIssues has an invalid Equipment ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_FleetIssues> tmpList = null;
            FleetIssues.RetrieveFleetIssueByEquipmentIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            FleetIssuesList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    public class FleetIssues_RetrieveByServiceOrderId_V2 : FleetIssues_TransactionBaseClass
    {
        public List<b_FleetIssues> FIList = new List<b_FleetIssues>();
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_FleetIssues> tempList = new List<b_FleetIssues>();
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            FleetIssues.RetrieveByServiceOrderIdFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tempList);
            FIList = tempList;
        }
    }

        public class FleetIssues_UpdateforPrevandNewFleetissues : FleetIssues_TransactionBaseClass
        {
            public override void PerformLocalValidation()
            {
                base.PerformLocalValidation();
                if (FleetIssues.FleetIssuesId == 0)
                {
                    string message = "FleetIssues has an invalid ID.";
                    throw new Exception(message);
                }
            }

            public override void PerformWorkItem()
            {
                FleetIssues.FleetIssuesUpdateforPrevandNewFleetissuesInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
                if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
            }
        }

    public class FleetIssues_IfServiceOrdrExist : FleetIssues_TransactionBaseClass
    {
        public FleetIssues_IfServiceOrdrExist()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                FleetIssues.ValidateIfServiceOrdrExist(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

                StoredProcValidationErrorList = errors;
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


