using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;

namespace Database.Business
{
    public partial class b_Instructions
    {

        #region Property
        public DateTime CreateDate { get; set; }
        #endregion
        public void RetrieveInstructionsByObjectIdFromDatabaseV2(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
string callerUserName,
      ref b_Instructions[] data
  )
        {
            Database.SqlClient.ProcessRow<b_Instructions> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Instructions[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Instructions>(reader => { b_Instructions obj = new b_Instructions(); obj.LoadExtendedFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Instructions_RetrieveByObjectId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, ObjectId, TableName);

                // Extract the results
                if (null != results)
                {
                    data = (b_Instructions[])results.ToArray(typeof(b_Instructions));
                }
                else
                {
                    data = new b_Instructions[0];
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
        public void LoadExtendedFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // ChangeLogId column, bigint, not null
                InstructionsId = reader.GetInt64(i++);                

                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);

                // TableName column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    TableName = reader.GetString(i);
                }
                else
                {
                    TableName = "";
                }
                i++;
                // Contents column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Contents = reader.GetString(i);
                }
                else
                {
                    Contents = "";
                }
                i++;
                CreateDate = reader.GetDateTime(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["InstructionsId"].ToString(); }
                catch { missing.Append("InstructionsId "); }                

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }

                try { reader["TableName"].ToString(); }
                catch { missing.Append("TableName "); }

                try { reader["Contents"].ToString(); }
                catch { missing.Append("Contents "); }


                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Instructions ProcessRowForInstructionsPrint(SqlDataReader reader)
        {
            // Create instance of object
            b_Instructions obj = new b_Instructions();

            // Load the object from the database
            obj.LoadFromDatabaseForInstructionsPrint(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForInstructionsPrint(SqlDataReader reader)
        {
            int i = 0;
            try
            {

              
                // ObjectId column, bigint, not null
                ObjectId = reader.GetInt64(i++);
              
                // Contents column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Contents = reader.GetString(i);
                }
                else
                {
                    Contents = "";
                }
                i++;              

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

              

                try { reader["ObjectId"].ToString(); }
                catch { missing.Append("ObjectId "); }
                
                try { reader["Contents"].ToString(); }
                catch { missing.Append("Contents "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
    }
}
