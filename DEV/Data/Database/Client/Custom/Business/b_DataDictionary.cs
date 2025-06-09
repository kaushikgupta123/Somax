using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_DataDictionary
    {
        #region Property
        public long UiViewId { get; set; }
        public long UIConfigurationId { get; set; }
        public bool Required { get; set; } //temporarry added because this field was removed from db but used in a SP. So need to remove from SP and the code.
        #endregion
        public void RetrieveDetailsByClientId_V2(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref List<b_DataDictionary> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_DataDictionary> results = null;
            data = new List<b_DataDictionary>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_DataDictionary_RetrieveDetailsByClientId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_DataDictionary>();
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
        public static b_DataDictionary ProcessRowByRetrieveDetailsByClientId(SqlDataReader reader)
        {
            // Create instance of object
            b_DataDictionary obj = new b_DataDictionary();

            // Load the object from the database
            obj.LoadFromDatabaseforRetrieveDetailsByClientId_V2(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseforRetrieveDetailsByClientId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // DataDictionaryId column, bigint, not null
                DataDictionaryId = reader.GetInt64(i++);

                // ColumnName column, nvarchar(31), not null
                ColumnName = reader.GetString(i++);
              
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["DataDictionaryId"].ToString(); }
                catch { missing.Append("DataDictionaryId "); }

                try { reader["ColumnName"].ToString(); }
                catch { missing.Append("ColumnName "); }
              

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        #region Update Column setting
        public void UpdateColumnSettingByDataDictionaryId_V2(
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
                StoredProcedure.usp_DataDictionary_UpdateColumnSettingByDataDictionaryId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #endregion
    }
}
