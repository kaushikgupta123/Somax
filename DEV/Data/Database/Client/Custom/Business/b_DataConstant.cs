using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_DataConstant
    {

        public void RetrieveLocaleForConstantType_V2(SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_DataConstant> data)
        {
            Database.SqlClient.ProcessRow<b_DataConstant> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            List<b_DataConstant> results = null;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_DataConstant>(reader => { b_DataConstant obj = new b_DataConstant(); obj.LoadFromDatabaseForLocaleByConstant(reader); return obj; });
                results = Database.StoredProcedure.usp_DataConstant_RetrieveLocaleForConstantType_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
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
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public int LoadFromDatabaseForLocaleByConstant(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // Value column, nvarchar(24), not null
                Value = reader.GetString(i++);

                // LocalName column, nvarchar(67), not null
                LocalName = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Value"].ToString(); }
                catch { missing.Append("Value "); }

                try { reader["LocalName"].ToString(); }
                catch { missing.Append("LocalName "); }

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


        public void RetrieveLocaleForConstantTypeWithId_V2(SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_DataConstant> data)
        {
            Database.SqlClient.ProcessRow<b_DataConstant> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;
            List<b_DataConstant> results = null;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_DataConstant>(reader => { b_DataConstant obj = new b_DataConstant(); obj.LoadFromDatabaseWithIdForLocaleByConstant(reader); return obj; });
                results = Database.StoredProcedure.usp_DataConstant_RetrieveLocaleForConstantTypeWithID_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
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
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public int LoadFromDatabaseWithIdForLocaleByConstant(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // Value column, bigint, not null
                DataConstantId = reader.GetInt64(i++);

                // LocalName column, nvarchar(67), not null
                LocalName = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["DataConstantId"].ToString(); }
                catch { missing.Append("DataConstantId "); }

                try { reader["LocalName"].ToString(); }
                catch { missing.Append("LocalName "); }

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
    }
}
