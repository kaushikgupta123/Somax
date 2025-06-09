using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Alerts table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_Alerts : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_Alerts()
        {
            ClientId = 0;
            AlertsId = 0;
            AlertDefineId = 0;
            Headline = String.Empty;
            Summary = String.Empty;
            Details = String.Empty;
            From = String.Empty;
            AlertType = String.Empty;
            ObjectId = 0;
            ObjectName = String.Empty;
            Action = String.Empty;
            ObjectState = String.Empty;
            IsCleared = false;
            Notes = String.Empty;
            UpdateIndex = 0;
        }

        /// <summary>
        /// AlertsId property
        /// </summary>
        public long AlertsId { get; set; }

        /// <summary>
        /// AlertDefineId property
        /// </summary>
        public long AlertDefineId { get; set; }

        /// <summary>
        /// Headline property
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// Summary property
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Details property
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// From property
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// AlertType property
        /// </summary>
        public string AlertType { get; set; }

        /// <summary>
        /// ObjectId property
        /// </summary>
        public long ObjectId { get; set; }

        /// <summary>
        /// ObjectName property
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// Action property
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// ObjectState property
        /// </summary>
        public string ObjectState { get; set; }

        /// <summary>
        /// IsCleared property
        /// </summary>
        public bool IsCleared { get; set; }

        /// <summary>
        /// Notes property
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public int UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Alerts object.
        /// This routine should be applied to the usp_Alerts_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Alerts_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Alerts object</returns>
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_Alerts obj = new b_Alerts();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Alerts object.
        /// This routine should be applied to the usp_Alerts_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Alerts_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // AlertsId column, bigint, not null
                AlertsId = reader.GetInt64(i++);

                // AlertDefineId column, bigint, not null
                AlertDefineId = reader.GetInt64(i++);

                // Headline column, nvarchar(63), not null
                Headline = reader.GetString(i++);

                // Summary column, nvarchar(127), not null
                Summary = reader.GetString(i++);

                // Details column, nvarchar(4000), not null
                Details = reader.GetString(i++);

                // From column, nvarchar(127), not null
                From = reader.GetString(i++);

                // AlertType column, nvarchar(127), not null
                AlertType = reader.GetString(i++);

                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // ObjectName column, nvarchar(127), not null
                ObjectName = reader.GetString(i++);

                // Action column, nvarchar(63), not null
                Action = reader.GetString(i++);

                // ObjectState column, nvarchar(31), not null
                ObjectState = reader.GetString(i++);

                // IsCleared column, bit, not null
                IsCleared = reader.GetBoolean(i++);

                // Notes column, nvarchar(4000), not null
                Notes = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AlertsId"].ToString(); }
                catch { missing.Append("AlertsId "); }

                try { reader["AlertDefineId"].ToString(); }
                catch { missing.Append("AlertDefineId "); }

                try { reader["Headline"].ToString(); }
                catch { missing.Append("Headline "); }

                try { reader["Summary"].ToString(); }
                catch { missing.Append("Summary "); }

                try { reader["Details"].ToString(); }
                catch { missing.Append("Details "); }

                try { reader["From"].ToString(); }
                catch { missing.Append("From "); }

                try { reader["AlertType"].ToString(); }
                catch { missing.Append("AlertType "); }

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["ObjectName"].ToString(); }
                catch { missing.Append("ObjectName "); }

                try { reader["Action"].ToString(); }
                catch { missing.Append("Action "); }

                try { reader["ObjectState"].ToString(); }
                catch { missing.Append("ObjectState "); }

                try { reader["IsCleared"].ToString(); }
                catch { missing.Append("IsCleared "); }

                try { reader["Notes"].ToString(); }
                catch { missing.Append("Notes "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }

        /// <summary>
        /// Insert this object into the database as a Alerts table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void InsertIntoDatabase(
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
               Database.StoredProcedure.usp_Alerts_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the Alerts table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void UpdateInDatabase(
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
               Database.StoredProcedure.usp_Alerts_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the Alerts table record represented by this object from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public override void DeleteFromDatabase(
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
                Database.StoredProcedure.usp_Alerts_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all Alerts table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Alerts[] that contains the results</param>
        public void RetrieveAllFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Alerts[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Alerts> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Alerts[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Alerts>(reader => { b_Alerts obj = new b_Alerts(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Alerts_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Alerts[])results.ToArray(typeof(b_Alerts));
                }
                else
                {
                    data = new b_Alerts[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        /// <summary>
        /// Retrieve Alerts table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_Alerts[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
           Database.SqlClient.ProcessRow<b_Alerts> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Alerts>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Alerts_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        /// <summary>
        /// Test equality of two b_Alerts objects.
        /// </summary>
        /// <param name="obj">b_Alerts object to compare against the current object.</param>
        public bool Equals(b_Alerts obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (AlertsId != obj.AlertsId) return false;
            if (AlertDefineId != obj.AlertDefineId) return false;
            if (!Headline.Equals(obj.Headline)) return false;
            if (!Summary.Equals(obj.Summary)) return false;
            if (!Details.Equals(obj.Details)) return false;
            if (!From.Equals(obj.From)) return false;
            if (!AlertType.Equals(obj.AlertType)) return false;
            if (ObjectId != obj.ObjectId) return false;
            if (!ObjectName.Equals(obj.ObjectName)) return false;
            if (!Action.Equals(obj.Action)) return false;
            if (!ObjectState.Equals(obj.ObjectState)) return false;
            if (IsCleared != obj.IsCleared) return false;
            if (!Notes.Equals(obj.Notes)) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_Alerts objects.
        /// </summary>
        /// <param name="obj1">b_Alerts object to use in the comparison.</param>
        /// <param name="obj2">b_Alerts object to use in the comparison.</param>
        public static bool Equals(b_Alerts obj1, b_Alerts obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
