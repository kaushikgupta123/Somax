using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the Equipment_Parts_Xref table.
    /// </summary>
    public partial class b_Equipment_Parts_Xref
    {
        /// <summary>
        /// ClientId property
        /// </summary>
        public string Equipment_ClientLookupId { get; set; }
        public string Equipment_Name { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_Description { get; set; }
        public long ParentSiteId { get; set; }
        public string PartClientLookUpId { get; set; }
        public string Description { get; set; }
        public string StockType { get; set; }
        /// <summary>
        /// Process the current row in the input SqlDataReader into a b_Equipment_Parts_Xref object.
        /// This routine should be applied to the usp_Equipment_Parts_Xref_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Equipment_Parts_Xref_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        /// <returns>object cast of the b_Equipment_Parts_Xref object</returns>
        public static object ProcessRowForEquipmentCrossReference(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment_Parts_Xref obj = new b_Equipment_Parts_Xref();

            // Load the object from the database
            obj.LoadFromDatabaseForCrossReference(reader);

            // Return result
            return (object)obj;
        }
        public static object ProcessRowForEquipment(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment_Parts_Xref obj = new b_Equipment_Parts_Xref();

            // Load the object from the database
            obj.LoadFromDatabase(reader);

            // Return result
            return (object)obj;
        }
        /// <summary>
        /// Load the current row in the input SqlDataReader into a b_Equipment_Parts_Xref object.
        /// This routine should be applied to the usp_Equipment_Parts_Xref_RetrieveByPK stored procedure.
        /// This routine should be applied to the usp_Equipment_Parts_Xref_RetrieveAll. stored procedure.
        /// </summary>
        /// <param name="reader">SqlDataReader containing the reader to process for the next row</param>
        public void LoadFromDatabaseForCrossReference(SqlDataReader reader)
        {
            // SOM-562 - Use the base
            int i = this.LoadFromDatabase(reader);
            try
            {

                // Equipment Client Lookup Id
                Equipment_ClientLookupId = reader.GetString(i++);

                // Equipment Name 
                Equipment_Name = reader.GetString(i++);

                // Part ClientLookupId
                Part_ClientLookupId = reader.GetString(i++);

                // Part Description
                Part_Description = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Equipment_ClientLookupId"].ToString(); }
                catch { missing.Append("Equipment_ClientLookupId "); }

                try { reader["Equipment_Name"].ToString(); }
                catch { missing.Append("Equipment_Name "); }

                try { reader["Part_ClientLookupId"].ToString(); }
                catch { missing.Append("Part_ClientLookupId "); }

                try { reader["Part_ClientLookupId"].ToString(); }
                catch { missing.Append("Part_ClientLookupId "); }

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
                results = Database.StoredProcedure.usp_Equipment_Parts_Xref_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveByEquipmentIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment_Parts_Xref> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment_Parts_Xref> results = null;
            data = new List<b_Equipment_Parts_Xref>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_Parts_Xref_RetrieveByEquipmentId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment_Parts_Xref>();
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

        public void RetrieveByEquipmentIdFromDatabase_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_Equipment_Parts_Xref> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment_Parts_Xref> results = null;
            data = new List<b_Equipment_Parts_Xref>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_Parts_Xref_RetrieveByEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment_Parts_Xref>();
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

        // SOM-562 -Begin
        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveByPartIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Equipment_Parts_Xref> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment_Parts_Xref> results = null;
            data = new List<b_Equipment_Parts_Xref>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_Parts_Xref_RetrieveByPartId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment_Parts_Xref>();
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
        // SOM-562 - End

        /// <summary>
        /// Insert this object into the database as a Equipment_Parts_Xref table record.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void InsertIntoDatabaseByPKForeignKeys(
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
                Database.StoredProcedure.usp_Equipment_Parts_Xref_CreateByPKForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-1007
        public void RetrieveByEquipmentIdPartIdFromDatabase_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_Equipment_Parts_Xref> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment_Parts_Xref> results = null;
            data = new List<b_Equipment_Parts_Xref>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_Parts_Xref_RetrieveByEquipmentIdPartId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment_Parts_Xref>();
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
        #endregion

    }
}
