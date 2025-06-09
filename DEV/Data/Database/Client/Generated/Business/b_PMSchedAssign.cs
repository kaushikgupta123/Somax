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
    /// Business object that stores a record from the PMSchedAssign table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_PMSchedAssign : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_PMSchedAssign()
        {
            ClientId = 0;
            PMSchedAssignId = 0;
            PrevMaintSchedId = 0;
            PersonnelId = 0;
            ScheduledHours = 0;
        }

        /// <summary>
        /// PMSchedAssignId property
        /// </summary>
        public long PMSchedAssignId { get; set; }

        /// <summary>
        /// PrevMaintSchedId property
        /// </summary>
        public long PrevMaintSchedId { get; set; }

        /// <summary>
        /// PersonnelId property
        /// </summary>
        public long PersonnelId { get; set; }

        /// <summary>
        /// ScheduledHours property
        /// </summary>
        public decimal ScheduledHours { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_PMSchedAssign object.
        /// This routine should be applied to the usp_PMSchedAssign_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_PMSchedAssign_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_PMSchedAssign object</returns>
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_PMSchedAssign obj = new b_PMSchedAssign();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_PMSchedAssign object.
        /// This routine should be applied to the usp_PMSchedAssign_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_PMSchedAssign_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PMSchedAssignId column, bigint, not null
                PMSchedAssignId = reader.GetInt64(i++);

                // PrevMaintSchedId column, bigint, not null
                PrevMaintSchedId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // ScheduledHours column, decimal(8,2), not null
                ScheduledHours = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PMSchedAssignId"].ToString(); }
                catch { missing.Append("PMSchedAssignId "); }

                try { reader["PrevMaintSchedId"].ToString(); }
                catch { missing.Append("PrevMaintSchedId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours "); }


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
        /// Insert this object into the database as a PMSchedAssign table record.
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
                Database.StoredProcedure.usp_PMSchedAssign_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the PMSchedAssign table record represented by this object in the database.
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
                Database.StoredProcedure.usp_PMSchedAssign_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the PMSchedAssign table record represented by this object from the database.
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
                Database.StoredProcedure.usp_PMSchedAssign_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all PMSchedAssign table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_PMSchedAssign[] that contains the results</param>
        public void RetrieveAllFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_PMSchedAssign[] data
        )
        {
            Database.SqlClient.ProcessRow<b_PMSchedAssign> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_PMSchedAssign[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PMSchedAssign>(reader => { b_PMSchedAssign obj = new b_PMSchedAssign(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_PMSchedAssign_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_PMSchedAssign[])results.ToArray(typeof(b_PMSchedAssign));
                }
                else
                {
                    data = new b_PMSchedAssign[0];
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
        /// Retrieve PMSchedAssign table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_PMSchedAssign[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PMSchedAssign> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PMSchedAssign>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_PMSchedAssign_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_PMSchedAssign objects.
        /// </summary>
        /// <param name="obj">b_PMSchedAssign object to compare against the current object.</param>
        public bool Equals(b_PMSchedAssign obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (PMSchedAssignId != obj.PMSchedAssignId) return false;
            if (PrevMaintSchedId != obj.PrevMaintSchedId) return false;
            if (PersonnelId != obj.PersonnelId) return false;
            if (ScheduledHours != obj.ScheduledHours) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_PMSchedAssign objects.
        /// </summary>
        /// <param name="obj1">b_PMSchedAssign object to use in the comparison.</param>
        /// <param name="obj2">b_PMSchedAssign object to use in the comparison.</param>
        public static bool Equals(b_PMSchedAssign obj1, b_PMSchedAssign obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
