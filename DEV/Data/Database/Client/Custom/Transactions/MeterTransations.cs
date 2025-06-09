using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Structures;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;
namespace Database
{
    public class Meter_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
    {
        public Meter_RetrieveInitialSearchConfigurationData()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public Dictionary<string, List<KeyValuePair<string, string>>> Search_Criteria;
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

                // Call the stored procedure to retrieve the data
                Search_Criteria = usp_Meter_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
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

    public class Meter_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Meter_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        
        public long MeterId { get; set; }
        public long SiteId { get; set; }
        public string Name { get; set; }
        public decimal ReadingCurrent { get; set; }        
        public DateTime ReadingDate { get; set; }
        public string ClientLookupId { get; set; }       
        public string Type { get; set; }
        public decimal ReadingMax { get; set; }
        public decimal ReadingLife { get; set; }      

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Meter> MeterList { get; set; }
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

                MeterList = usp_Meter_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name,SiteId,Type,
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

    public class RetrieveMeterReport : AbstractTransactionManager
    {
        public RetrieveMeterReport()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<ReportDataStructure> RawList { get; set; }

        public List<string> SelectedValues { get; set; }
        public List<string> SelectedColumns { get; set; }
        public List<string> SelectedNumerics { get; set; }

        public string PrimaryColumn { get; set; }
        public string PrimaryTable { get; set; }

        public string JoinOnColumn { get; set; }
        public string SearchOnColumn { get; set; }
        public string JoinedTable { get; set; }

        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Column { get; set; }
        public string SearchText { get; set; }
        public bool UseLike { get; set; }

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

                RawList = usp_Equipment_RetrieveForListReport.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId,
                    PrimaryColumn, PrimaryTable, JoinOnColumn, SearchOnColumn, JoinedTable, SelectedValues, SelectedColumns, SelectedNumerics,
                    DateSelection, DateStart, DateEnd, Column, SearchText, UseLike);
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

    public class RetrieveMeterBySearchCriteria : AbstractTransactionManager
    {
        public RetrieveMeterBySearchCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public string Query { get; set; }
        public string Site { get; set; }
        public string Area { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Column { get; set; }
        public string SearchText { get; set; }
        public bool UseLike { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Meter> MeterList { get; set; }
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

                // Call the stored procedure to retrieve the data
                Database.SqlClient.ProcessRow<b_Meter> processRow =
                    new Database.SqlClient.ProcessRow<b_Meter>(reader => { b_Meter obj = new b_Meter(); obj.LoadFromDatabase(reader); return obj; });

                int tmp;

                MeterList = usp_Meter_RetrieveBySearchCriteria.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId,
                    Query, Site, Area, Department, Type, Status, DateSelection, DateStart, DateEnd, Column, SearchText, Page, ResultsPerPage, UseLike, OrderColumn, OrderDirection, out tmp);
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

    public class Meter_ValidateByClientLookupId : Meter_TransactionBaseClass
    {
        public Meter_ValidateByClientLookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public bool CreateMode { get; set; }
        public System.Data.DataTable lulist { get; set; }


        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationError_List { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {

                List<b_StoredProcValidationError> errors = null;


            Meter.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, CreateMode, lulist, ref errors);

                StoredProcValidationError_List = errors;
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

    public class Meter_UpdateByForeignKeys : Meter_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId == 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Meter.UpdateByForeignKeysInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Meter_RetrieveByForeignKeys : Meter_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Meter.MeterId == 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Meter.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Meter_CreateByForeignKeys : Meter_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId > 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Meter.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            //base.Postprocess();
            //System.Diagnostics.Debug.Assert(Meter.MeterId > 0);
        }
    }

    public class Meter_RetrieveClientLookupIdBySearchCriteria : Meter_TransactionBaseClass
    {

        public List<b_Meter> MeterList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId > 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Meter> tmpList = null;
            Meter.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

           MeterList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Meter_RetrieveByClientLookUpId : Meter_TransactionBaseClass
    {
        //public b_Part objRetPart { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId > 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            b_Meter temp = null;
            base.UseTransaction = false;
            Meter.RetrieveByMeterClientLookUpId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref temp);

            this.Meter = temp;
        }
    }

    public class Meters_SearchBySiteAndReadingDate : Meter_TransactionBaseClass
    {

        public List<b_Meter> MeterList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId > 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Meter> tmpList = null;
            Meter.RetrieveForSearchBySiteAndReadingDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            MeterList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Meter_RetriveAllCustom : AbstractTransactionManager
    {


        public Meter_RetriveAllCustom()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Meter> MeterList { get; set; }
        public long ClientId { get; set; }

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
            List<b_Meter> tmpArray = null;
            b_Meter o = new b_Meter();
            o.ClientId = ClientId;

            o.RetrieveMeterLookupListByClientID(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ClientId, ref tmpArray);

            MeterList = new List<b_Meter>(tmpArray);
        }

    }

    //- SOM: 928
    public class Meter_ActiveInactiveByPrimaryKey : Meter_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId == 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Meter.ActiveInactiveByPrimaryKeyInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            //base.Postprocess();
            //System.Diagnostics.Debug.Assert(Meter.MeterId > 0);
        }
    }
    //- SOM: 928
    public class Meter_GeneratePMWorkOrders : Meter_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (Meter.MeterId == 0)
        {
          string message = "Meter has an invalid ID.";
          throw new Exception(message);
        }
      }
      public override void PerformWorkItem()
      {
        Meter.GeneratePMWorkOrders(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }

      public override void Postprocess()
      {
        //base.Postprocess();
        //System.Diagnostics.Debug.Assert(Meter.MeterId > 0);
      }
    }

    public class Meters_SearchBySiteAndReadingDate_V2 : Meter_TransactionBaseClass
    {

        public List<b_Meter> MeterList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId > 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Meter> tmpList = null;
            Meter.RetrieveForSearchBySiteAndReadingDate_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            MeterList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region V2-950
    public class Meters_RetrieveForTableLookupList_V2 : Meter_TransactionBaseClass
    {

        public List<b_Meter> MeterList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Meter.MeterId > 0)
            {
                string message = "Meter has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Meter> tmpList = null;
            Meter.RetrieveForTableLookupList_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            MeterList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
}
