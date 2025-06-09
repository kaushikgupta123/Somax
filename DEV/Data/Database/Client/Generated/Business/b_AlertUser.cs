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
    /// Business object that stores a record from the AlertUser table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_AlertUser : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_AlertUser()
        {
            ClientId = 0;
            AlertUserId = 0;
            UserId = 0;
            AlertsId = 0;
            ActiveDate = DateTime.MinValue;
            Permission = String.Empty;
            IsRead = false;
            IsDeleted = false;
            UpdateIndex = 0;
        }

        /// <summary>
        /// AlertUserId property
        /// </summary>
        public long AlertUserId { get; set; }

        /// <summary>
        /// UserId property
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// AlertsId property
        /// </summary>
        public long AlertsId { get; set; }

        /// <summary>
        /// ActiveDate property
        /// </summary>
        public DateTime ActiveDate { get; set; }

        /// <summary>
        /// Permission property
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// IsRead property
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// IsDeleted property
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        public int UpdateIndex { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_AlertUser object.
        /// This routine should be applied to the usp_AlertUser_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AlertUser_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_AlertUser object</returns>
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_AlertUser obj = new b_AlertUser();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_AlertUser object.
        /// This routine should be applied to the usp_AlertUser_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_AlertUser_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
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
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["AlertUserId"].ToString(); }
                catch { missing.Append("AlertUserId "); }

                try { reader["UserId"].ToString(); }
                catch { missing.Append("UserId "); }

                try { reader["AlertsId"].ToString(); }
                catch { missing.Append("AlertsId "); }

                try { reader["ActiveDate"].ToString(); }
                catch { missing.Append("ActiveDate "); }

                try { reader["Permission"].ToString(); }
                catch { missing.Append("Permission "); }

                try { reader["IsRead"].ToString(); }
                catch { missing.Append("IsRead "); }

                try { reader["IsDeleted"].ToString(); }
                catch { missing.Append("IsDeleted "); }

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
        }

        /// <summary>
        /// Insert this object into the database as a AlertUser table record.
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
               Database.StoredProcedure.usp_AlertUser_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the AlertUser table record represented by this object in the database.
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
                Database.StoredProcedure.usp_AlertUser_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the AlertUser table record represented by this object from the database.
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
                Database.StoredProcedure.usp_AlertUser_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all AlertUser table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_AlertUser[] that contains the results</param>
        public void RetrieveAllFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_AlertUser[] data
        )
        {
            Database.SqlClient.ProcessRow<b_AlertUser> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_AlertUser[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AlertUser>(reader => { b_AlertUser obj = new b_AlertUser(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_AlertUser_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_AlertUser[])results.ToArray(typeof(b_AlertUser));
                }
                else
                {
                    data = new b_AlertUser[0];
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
        /// Retrieve AlertUser table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_AlertUser[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_AlertUser> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_AlertUser>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_AlertUser_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_AlertUser objects.
        /// </summary>
        /// <param name="obj">b_AlertUser object to compare against the current object.</param>
        public bool Equals(b_AlertUser obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (AlertUserId != obj.AlertUserId) return false;
            if (UserId != obj.UserId) return false;
            if (AlertsId != obj.AlertsId) return false;
            if (ActiveDate != obj.ActiveDate) return false;
            if (!Permission.Equals(obj.Permission)) return false;
            if (IsRead != obj.IsRead) return false;
            if (IsDeleted != obj.IsDeleted) return false;
            if (UpdateIndex != obj.UpdateIndex) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_AlertUser objects.
        /// </summary>
        /// <param name="obj1">b_AlertUser object to use in the comparison.</param>
        /// <param name="obj2">b_AlertUser object to use in the comparison.</param>
        public static bool Equals(b_AlertUser obj1, b_AlertUser obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
