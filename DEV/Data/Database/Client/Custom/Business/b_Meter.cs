using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_Meter
    {

        #region Properties
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public long PersonnelId { get; set; }
        public string PMWOClientLookupIds { get; set; } //V2-784
        public List<long> PMWOList { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int TotalCount { get; set; }
        #endregion

        public void RetrieveForSearchBySiteAndReadingDate(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_Meter> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Meter_RetrieveBySearchBySiteAndReadingDate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Meter ProcessRowForSearchBySiteAndReadingDate(SqlDataReader reader)
        {
            // Create instance of object
            b_Meter meters = new b_Meter();

            // Load the object from the database
            meters.LoadFromDatabaseForSearchBySiteAndReadingDate(reader);

            // Return result
            return meters;
        }
        public void LoadFromDatabaseForSearchBySiteAndReadingDate(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                // PartsId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                Name = reader.GetString(i++);
                InactiveFlag = reader.GetBoolean(i++);   //SOM -928
                ReadingCurrent = reader.GetDecimal(i++);

                // ReadingDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    ReadingDate = DateTime.MinValue;
                }
                i++;

                // ReadingLife column, decimal(17,3), not null
                ReadingLife = reader.GetDecimal(i++);

                // ReadingMax column, decimal(17,3), not null
                ReadingMax = reader.GetDecimal(i++);

                // ReadingUnits column, nvarchar(15), not null
                ReadingUnits = reader.GetString(i++);

                // ReadingBy column, nvarchar(31), not null
                ReadingBy = reader.GetInt64(i++);
                

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                PersonnelClientLookupId = reader.GetString(i++);
               
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ReadingCurrent"].ToString(); }
                catch { missing.Append("ReadingCurrent "); }

                try { reader["ReadingDate"].ToString(); }
                catch { missing.Append("ReadingDate "); }

                try { reader["ReadingLife"].ToString(); }
                catch { missing.Append("ReadingLife "); }
            

                try { reader["ReadingUnits"].ToString(); }
                catch { missing.Append("ReadingUnits "); }

                try { reader["ReadingBy"].ToString(); }
                catch { missing.Append("ReadingBy "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("PersonnelClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }



        public static b_Meter ProcessRowByPKForeignKey(SqlDataReader reader)
        {
            
            b_Meter eq = new b_Meter();
           
            eq.LoadFromDatabaseByPKForeignKey(reader);
            
            return eq;
        }

        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {               

                

            }
            catch (Exception ex)
            {
             

            }
        }

        public static b_Meter ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Meter eq = new b_Meter();

            // Load the object from the database
            eq.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return eq;
        }

        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // MeterId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void ValidateByClientLookupIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            bool createMode,
            System.Data.DataTable lulist,
            ref List<b_StoredProcValidationError> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Meter_ValidateByClientId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, createMode, lulist);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
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
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        /// <summary>
        /// Update the Equipment table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateByForeignKeysInDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_Meter_UpdateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void RetrieveByForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Meter> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Meter>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_Meter_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void InsertByForeignKeysIntoDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_Meter_CreateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Meter> results
        )
        {

            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Meter_RetrieveClientLookupIdSearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }




        public static b_Meter ProcessRowForRetrieveByClientLookUpId(SqlDataReader reader)
        {
            // Create instance of object
            b_Meter meters = new b_Meter();

            // Load the object from the database
            meters.LoadFromDatabaseForRetrieveByClientLookUpId(reader);

            // Return result
            return meters;
        }

        public void LoadFromDatabaseForRetrieveByClientLookUpId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // MeterId column, bigint, not null
                MeterId = reader.GetInt64(i++);
                

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveByMeterClientLookUpId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_Meter retMeter
        )
        {
            //Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                
                retMeter = Database.StoredProcedure.usp_Meter_RetrieveByClientLookUpId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
            message = String.Empty;
            callerUserInfoId = 0;
            callerUserName = String.Empty;

        }



       public void RetrieveMeterLookupListByClientID(SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       long clientId,
       ref List<b_Meter> retMeter)
        {
            Database.SqlClient.ProcessRow<b_Meter> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            ArrayList arl = new ArrayList();


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Meter>(reader =>
                {
                    return ProcessRowForClientIdLookup(reader);
                });
                arl = Database.StoredProcedure.usp_Meter_RetrieveAllByClientId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, clientId);

                retMeter = new List<b_Meter>();
                foreach (b_Meter t in arl)
                {
                    retMeter.Add(t);
                }

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
            message = String.Empty;
            callerUserInfoId = 0;
            callerUserName = String.Empty;

        }

        // SOM: 928
       public void ActiveInactiveByPrimaryKeyInDatabase(
                   SqlConnection connection,
                   SqlTransaction transaction,
                   long callerUserInfoId,
                   string callerUserName
               )
       {
           SqlCommand command = null;

           try
           {
               command = connection.CreateCommand();
               if (null != transaction)
               {
                   command.Transaction = transaction;
               }
               Database.StoredProcedure.usp_Meter_ActiveInactiveByPrimaryKey.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
           }
           finally
           {
               if (null != command)
               {
                   command.Dispose();
                   command = null;
               }
           }

       }
       // SOM: 928
       public void GeneratePMWorkOrders(SqlConnection connection,SqlTransaction transaction,long callerUserInfoId,string callerUserName)
       {
         SqlCommand command = null;

         try
         {
           command = connection.CreateCommand();
           if (null != transaction)
           {
             command.Transaction = transaction;
           }
           Database.StoredProcedure.usp_Meter_GeneratePMWorkOrders.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
         }
         finally
         {
           if (null != command)
           {
             command.Dispose();
             command = null;
           }
         }
       }

        public static b_Meter ProcessRowForSearchBySiteAndReadingDate_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_Meter meters = new b_Meter();

            // Load the object from the database
            meters.LoadFromDatabaseForSearchBySiteAndReadingDate_V2(reader);

            // Return result
            return meters;
        }

        public void LoadFromDatabaseForSearchBySiteAndReadingDate_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                // PartsId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                Name = reader.GetString(i++);
                InactiveFlag = reader.GetBoolean(i++);   //SOM -928
                ReadingCurrent = reader.GetDecimal(i++);

                // ReadingDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    ReadingDate = DateTime.MinValue;
                }
                i++;

                // ReadingLife column, decimal(17,3), not null
                ReadingLife = reader.GetDecimal(i++);

                // ReadingMax column, decimal(17,3), not null
                ReadingMax = reader.GetDecimal(i++);

                // ReadingUnits column, nvarchar(15), not null
                ReadingUnits = reader.GetString(i++);

                // ReadingBy column, nvarchar(31), not null
                ReadingBy = reader.GetInt64(i++);


                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                PersonnelClientLookupId = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ReadingCurrent"].ToString(); }
                catch { missing.Append("ReadingCurrent "); }

                try { reader["ReadingDate"].ToString(); }
                catch { missing.Append("ReadingDate "); }

                try { reader["ReadingLife"].ToString(); }
                catch { missing.Append("ReadingLife "); }


                try { reader["ReadingUnits"].ToString(); }
                catch { missing.Append("ReadingUnits "); }

                try { reader["ReadingBy"].ToString(); }
                catch { missing.Append("ReadingBy "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("PersonnelClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveForSearchBySiteAndReadingDate_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Meter> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Meter_RetrieveBySearchBySiteAndReadingDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        #region V2-950
        public void RetrieveForTableLookupList_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Meter> results
)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Meter_RetrieveForTableLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public static b_Meter ProcessRowRetrieveForTableLookupList_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_Meter meters = new b_Meter();

            // Load the object from the database
            meters.LoadFromDatabaseRetrieveForTableLookupList_V2(reader);

            // Return result
            return meters;
        }

        public void LoadFromDatabaseRetrieveForTableLookupList_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                // PartsId column, bigint, not null
                MeterId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                Name = reader.GetString(i++);
                InactiveFlag = reader.GetBoolean(i++);   //SOM -928
                ReadingCurrent = reader.GetDecimal(i++);
                TotalCount = reader.GetInt32(i++);

                

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["MeterId"].ToString(); }
                catch { missing.Append("MeterId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ReadingCurrent"].ToString(); }
                catch { missing.Append("ReadingCurrent "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        #endregion
    }
}
