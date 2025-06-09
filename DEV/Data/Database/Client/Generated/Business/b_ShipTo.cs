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
    /// Business object that stores a record from the ShipTo table.InsertIntoDatabase
    /// </summary>
    [Serializable()]
    public partial class b_ShipTo : DataBusinessBase
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public b_ShipTo()
        {
            ClientId = 0;
            ShipToId = 0;
            SiteId = 0;
            AreaId = 0;
            DepartmentId = 0;
            StoreroomId = 0;
            ClientLookupId = String.Empty;
            Address1 = String.Empty;
            Address2 = String.Empty;
            Address3 = String.Empty;
            AddressCity = String.Empty;
            AddressCountry = String.Empty;
            AddressPostCode = String.Empty;
            AddressState = String.Empty;
            EmailAddress = String.Empty;
            FaxNumber = String.Empty;
            AttnName = String.Empty;
            PhoneNumber = String.Empty;
        }

        /// <summary>
        /// ShipToId property
        /// </summary>
        public long ShipToId { get; set; }

        /// <summary>
        /// SiteId property
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// AreaId property
        /// </summary>
        public long AreaId { get; set; }

        /// <summary>
        /// DepartmentId property
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// StoreroomId property
        /// </summary>
        public long StoreroomId { get; set; }

        /// <summary>
        /// ClientLookupId property
        /// </summary>
        public string ClientLookupId { get; set; }

        /// <summary>
        /// Address1 property
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Address2 property
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Address3 property
        /// </summary>
        public string Address3 { get; set; }

        /// <summary>
        /// AddressCity property
        /// </summary>
        public string AddressCity { get; set; }

        /// <summary>
        /// AddressCountry property
        /// </summary>
        public string AddressCountry { get; set; }

        /// <summary>
        /// AddressPostCode property
        /// </summary>
        public string AddressPostCode { get; set; }

        /// <summary>
        /// AddressState property
        /// </summary>
        public string AddressState { get; set; }

        /// <summary>
        /// EmailAddress property
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// FaxNumber property
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// AttnName property
        /// </summary>
        public string AttnName { get; set; }

        /// <summary>
        /// PhoneNumber property
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_ShipTo object.
        /// This routine should be applied to the usp_ShipTo_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_ShipTo_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_ShipTo object</returns>
        public static object ProcessRow(SqlDataReader reader)
        {
            // Create instance of object
            b_ShipTo obj = new b_ShipTo();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }

        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_ShipTo object.
        /// This routine should be applied to the usp_ShipTo_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_ShipTo_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public int LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ShipToId column, bigint, not null
                ShipToId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Address1 column, nvarchar(63), not null
                Address1 = reader.GetString(i++);

                // Address2 column, nvarchar(63), not null
                Address2 = reader.GetString(i++);

                // Address3 column, nvarchar(63), not null
                Address3 = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                AddressCity = reader.GetString(i++);

                // AddressCountry column, nvarchar(63), not null
                AddressCountry = reader.GetString(i++);

                // AddressPostCode column, nvarchar(31), not null
                AddressPostCode = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                AddressState = reader.GetString(i++);

                // EmailAddress column, nvarchar(63), not null
                EmailAddress = reader.GetString(i++);

                // FaxNumber column, nvarchar(31), not null
                FaxNumber = reader.GetString(i++);

                // AttnName column, nvarchar(63), not null
                AttnName = reader.GetString(i++);

                // PhoneNumber column, nvarchar(31), not null
                PhoneNumber = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ShipToId"].ToString(); }
                catch { missing.Append("ShipToId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Address1"].ToString(); }
                catch { missing.Append("Address1 "); }

                try { reader["Address2"].ToString(); }
                catch { missing.Append("Address2 "); }

                try { reader["Address3"].ToString(); }
                catch { missing.Append("Address3 "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressCountry"].ToString(); }
                catch { missing.Append("AddressCountry "); }

                try { reader["AddressPostCode"].ToString(); }
                catch { missing.Append("AddressPostCode "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

                try { reader["EmailAddress"].ToString(); }
                catch { missing.Append("EmailAddress "); }

                try { reader["FaxNumber"].ToString(); }
                catch { missing.Append("FaxNumber "); }

                try { reader["AttnName"].ToString(); }
                catch { missing.Append("AttnName "); }

                try { reader["PhoneNumber"].ToString(); }
                catch { missing.Append("PhoneNumber "); }


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
        /// Insert this object into the database as a ShipTo table record.
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
                Database.StoredProcedure.usp_ShipTo_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Update the ShipTo table record represented by this object in the database.
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
                Database.StoredProcedure.usp_ShipTo_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Delete the ShipTo table record represented by this object from the database.
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
                Database.StoredProcedure.usp_ShipTo_DeleteByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// Retrieve all ShipTo table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_ShipTo[] that contains the results</param>
        public void RetrieveAllFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
        string callerUserName,
            ref b_ShipTo[] data
        )
        {
            Database.SqlClient.ProcessRow<b_ShipTo> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_ShipTo[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShipTo>(reader => { b_ShipTo obj = new b_ShipTo(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_ShipTo_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_ShipTo[])results.ToArray(typeof(b_ShipTo));
                }
                else
                {
                    data = new b_ShipTo[0];
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
        /// Retrieve ShipTo table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        /// <param name="data">b_ShipTo[] that contains the results</param>
        public override void RetrieveByPKFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
        long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_ShipTo> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ShipTo>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_ShipTo_RetrieveByPK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /// Test equality of two b_ShipTo objects.
        /// </summary>
        /// <param name="obj">b_ShipTo object to compare against the current object.</param>
        public bool Equals(b_ShipTo obj)
        {
            if (ClientId != obj.ClientId) return false;
            if (ShipToId != obj.ShipToId) return false;
            if (SiteId != obj.SiteId) return false;
            if (AreaId != obj.AreaId) return false;
            if (DepartmentId != obj.DepartmentId) return false;
            if (StoreroomId != obj.StoreroomId) return false;
            if (!ClientLookupId.Equals(obj.ClientLookupId)) return false;
            if (!Address1.Equals(obj.Address1)) return false;
            if (!Address2.Equals(obj.Address2)) return false;
            if (!Address3.Equals(obj.Address3)) return false;
            if (!AddressCity.Equals(obj.AddressCity)) return false;
            if (!AddressCountry.Equals(obj.AddressCountry)) return false;
            if (!AddressPostCode.Equals(obj.AddressPostCode)) return false;
            if (!AddressState.Equals(obj.AddressState)) return false;
            if (!EmailAddress.Equals(obj.EmailAddress)) return false;
            if (!FaxNumber.Equals(obj.FaxNumber)) return false;
            if (!AttnName.Equals(obj.AttnName)) return false;
            if (!PhoneNumber.Equals(obj.PhoneNumber)) return false;
            return true;
        }

        /// <summary>
        /// Test equality of two b_ShipTo objects.
        /// </summary>
        /// <param name="obj1">b_ShipTo object to use in the comparison.</param>
        /// <param name="obj2">b_ShipTo object to use in the comparison.</param>
        public static bool Equals(b_ShipTo obj1, b_ShipTo obj2)
        {
            if ((null == obj1) && (null == obj2)) return true;
            if ((null == obj1) && (null != obj2)) return false;
            if ((null != obj1) && (null == obj2)) return false;
            return obj1.Equals(obj2);
        }
    }
}
