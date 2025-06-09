using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_AlertUser
    {
        public long PersonnelId { get; set; }
        public long ObjectId { get; set; }
        public string AlertName { get; set; }
        public string ObjectClientLookupId { get; set; }//V2-1136
        public string SelectedNotificationTab { get; set; }
        /// <summary>
        /// Update the AlertUser table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateByAlertIdAndUserId(
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
                Database.StoredProcedure.usp_AlertUser_UpdateByAlertIdAndUserId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        /// <summary>
        /// Retrieve AlertUser table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_AlertUser[] that contains the results</param>
        public static List<KeyValuePair<b_Alerts, b_AlertUser>> RetrieveAllBySearchCriteria(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            long clientId,
            long personnelId,
            bool RetrieveActionItems,
            bool RetrieveMessages,
            string AlertType,
            string From,
            string ObjectName,
            DateTime DateStart,
            DateTime DateEnd,
            string Column,
            string SearchText,
            int PageNumber,
            int ResultsPerPage,
            string OrderColumn,
            string OrderDirection,
            out int ResultCount
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
                return Database.StoredProcedure.usp_AlertComposite_RetrieveAllBySearchCriteria.CallStoredProcedure(command, callerUserInfoId,
                  clientId, personnelId, RetrieveActionItems, RetrieveMessages, AlertType, From, ObjectName, DateStart, DateEnd, Column,
                  SearchText, PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out ResultCount);
            }
            catch
            {
                ResultCount = 0;
                return null;
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
            }
        }



        public static int RetrieveCountBySearchCriteria(
            SqlConnection connection,
            SqlTransaction transaction,
            long clientId,
            long personnelId,
            bool IsRead,
             out int resultMaintenanceCount, out int resultInventoryCount, out int resultProcurementCount, out int resultSystemCount
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
                return Database.StoredProcedure.usp_AlertComposite_RetrieveCountByIsReadFlag_V2.CallStoredProcedure(command, 
                  clientId, personnelId, IsRead, out resultMaintenanceCount, out resultInventoryCount, out resultProcurementCount, out resultSystemCount);
            }
            catch
            {
                resultMaintenanceCount = 0;
                resultInventoryCount = 0;
                resultProcurementCount = 0;
                resultSystemCount = 0;
                return 0;
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


        /// <summary>
        /// Retrieve AlertUser table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_AlertUser[] that contains the results</param>
        public static int RetrievePendingByUserId(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            long clientId
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
                return Database.StoredProcedure.usp_AlertUser_RetrievePendingByUserId.CallStoredProcedure(command, callerUserInfoId, clientId);
            }
            catch
            {
                return 0;
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
            }
        }


        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_AlertUser object.
        /// This routine should be applied to the usp_AlertUser_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AlertUser_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_AlertUser object</returns>
        public static object ProcessRowForAlertComposite(SqlDataReader reader)
        {
            // Create instance of object
            b_AlertUser obj = new b_AlertUser();

            // Load the object from the database
            obj.LoadFromDatabaseForAlertComposite(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_AlertUser object.
        /// This routine should be applied to the usp_AlertUser_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AlertUser_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabaseForAlertComposite(SqlDataReader reader)
        {
            // IMPORTANT: This value must equal the total number of columns in the Alerts table, since the row will combine the two tables completely.
            int i = 15;

            // Note that general-broadcast messages may not yet have a AlertUser entry defined. Check for null and return if this is the case.
            if (reader.IsDBNull(i)) { return; }

            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // AlertUserId column, bigint, not null
                AlertUserId = reader.GetInt64(i++);

                // UserId column, bigint, not null
                UserId = reader.GetInt64(i++);

                // AlertsId column, bigint, not null
                AlertsId = reader.GetInt64(i++);

                // ActiveDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ActiveDate = reader.GetDateTime(i);
                }
                else
                {
                    ActiveDate = DateTime.MinValue;
                }
                i++;
                // Permission column, nvarchar(127), not null
                Permission = reader.GetString(i++);

                // IsRead column, bit, not null
                IsRead = reader.GetBoolean(i++);

                // IsDeleted column, bit, not null
                IsDeleted = reader.GetBoolean(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                // AD_AlertName column, nvarchar(63), not null
                AlertName = reader.GetString(i++);

                // ObjectClientLookupId column, nvarchar(63), not null
                ObjectClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["AU_ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AU_AlertUserId"].ToString(); }
                catch { missing.Append("AlertUserId "); }

                try { reader["AU_UserId"].ToString(); }
                catch { missing.Append("UserId "); }

                try { reader["AU_AlertsId"].ToString(); }
                catch { missing.Append("AU_AlertsId "); }

                try { reader["AU_ActiveDate"].ToString(); }
                catch { missing.Append("ActiveDate "); }

                try { reader["AU_Permission"].ToString(); }
                catch { missing.Append("Permission "); }

                try { reader["AU_IsRead"].ToString(); }
                catch { missing.Append("IsRead "); }

                try { reader["AU_IsDeleted"].ToString(); }
                catch { missing.Append("IsDeleted "); }

                try { reader["AU_UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["AD_AlertName"].ToString(); }
                catch { missing.Append("AlertName "); }

                try { reader["ObjectClientLookupId"].ToString(); }
                catch { missing.Append("ObjectClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void InsertByPersonnelIdIntoDatabase(
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
                Database.StoredProcedure.usp_AlertUser_CreateByPersonnelId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void RemoveAlertByObjectId(
         SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
        string callerUserName
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
                Database.StoredProcedure.usp_Alerts_RemoveByObjectId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
            }
        }

        public void UpdateAlertByUserId(
        SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
       string callerUserName
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
                Database.StoredProcedure.usp_AlertUser_UpdateByUserIdV2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
            }
        }

        public void UpdateAlertByUserIdAndNotificationTab(
               SqlConnection connection,
                  SqlTransaction transaction,
                  long callerUserInfoId,
              string callerUserName
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
                Database.StoredProcedure.usp_AlertUser_UpdateByUserIdAndNotificationTabV2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
            }
        }
        
        public void UpdateAlertByNotificationType(
       SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
      string callerUserName
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
                Database.StoredProcedure.usp_AlertUser_DeleteAllBySelectedNotificationTab_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
            }
        }
    }
}
