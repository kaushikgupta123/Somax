using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    public partial class b_UIConfiguration
    {
        #region Property
        public long ViewId { get; set; }
        public String ColumnName { get; set; }
        public String ColumnLabel { get; set; }
        //public bool Required { get; set; } 
        //public bool SystemRequired { get; set; }
        public String TableName { get; set; }
        public String ColumnType { get; set; }
        public String LookupType { get; set; }
        public String LookupName { get; set; }
        public bool UDF { get; set; }
        public bool Enabled { get; set; }
        public bool DisplayonForm { get; set; } //V2-944
        public string AvailableConfigurationId { get; set; }
        public System.Data.DataTable UICSelectedListData = new System.Data.DataTable();
        
        #endregion


        public void RetrieveSelectedListByViewId_V2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
string callerUserName,
   ref List<b_UIConfiguration> data
)
        {
            {

                SqlCommand command = null;
                string message = String.Empty;
                List<b_UIConfiguration> results = null;
                data = new List<b_UIConfiguration>();

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    results = Database.StoredProcedure.usp_UIConfiguration_RetrieveSelectedListByViewId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                    if (results != null)
                    {
                        data = results;
                    }
                    else
                    {
                        data = new List<b_UIConfiguration>();
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

        }

        public void RetrieveDetailsByUIViewId_V2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
string callerUserName,
   ref List<b_UIConfiguration> data
)
        {
            {

                SqlCommand command = null;
                string message = String.Empty;
                List<b_UIConfiguration> results = null;
                data = new List<b_UIConfiguration>();

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    results = Database.StoredProcedure.usp_UIConfiguration_RetrieveDetailsByUIViewId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                    if (results != null)
                    {
                        data = results;
                    }
                    else
                    {
                        data = new List<b_UIConfiguration>();
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

        }
        public static object ProcessRowByRetrieveDetailsFromUIConfigurationByUIViewId(SqlDataReader reader)
        {
            // Create instance of object
            b_UIConfiguration obj = new b_UIConfiguration();

            // Load the object from the database
            obj.LoadFromDatabaseRetrieveDetailsFromUIConfigurationyByUIViewId(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseRetrieveDetailsFromUIConfigurationyByUIViewId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // UIConfigurationId column, bigint, not null
                UIConfigurationId = reader.GetInt64(i++);

                // TableName column, nvarchar(31), not null
                TableName = reader.GetString(i++);

                // ColumnName column, nvarchar(31), not null
                ColumnName = reader.GetString(i++);

                // ColumnLabel column, nvarchar(67), not null
                ColumnLabel = reader.GetString(i++);

                // ColumnType column, nvarchar(15), not null
                ColumnType = reader.GetString(i++);

                // Required column, bit, not null
                Required = reader.GetBoolean(i++);

                // LookupType column, nvarchar(15), not null
                LookupType = reader.GetString(i++);

                // LookupName column, nvarchar(31), not null
                LookupName = reader.GetString(i++);

                // UDF column, bit, not null
                UDF = reader.GetBoolean(i++);

                // Enabled column, bit, not null
                Enabled = reader.GetBoolean(i++);

                // SystemRequired column, bit, not null
                SystemRequired = reader.GetBoolean(i++);

                // Order column,  int, not null
                Order = reader.GetInt32(i++);

                // Display column, bit, not null
                Display = reader.GetBoolean(i++);

                // ViewOnly column, bit, not null
                ViewOnly = reader.GetBoolean(i++);

                // Section column, bit, not null
                Section = reader.GetBoolean(i++);

                // SectionName column, nvarchar(31), not null
                SectionName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["UIConfigurationId"].ToString(); }
                catch { missing.Append("UIConfigurationId "); }

                try { reader["TableName"].ToString(); }
                catch { missing.Append("TableName "); }

                try { reader["ColumnName"].ToString(); }
                catch { missing.Append("ColumnName "); }

                try { reader["ColumnLabel"].ToString(); }
                catch { missing.Append("ColumnLabel "); }

                try { reader["ColumnType"].ToString(); }
                catch { missing.Append("ColumnType "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["LookupType"].ToString(); }
                catch { missing.Append("LookupType "); }

                try { reader["LookupName"].ToString(); }
                catch { missing.Append("LookupName "); }

                try { reader["UDF"].ToString(); }
                catch { missing.Append("UDF "); }

                try { reader["Enabled"].ToString(); }
                catch { missing.Append("Enabled "); }

                try { reader["SystemRequired"].ToString(); }
                catch { missing.Append("SystemRequired "); }

                try { reader["Order"].ToString(); }
                catch { missing.Append("Order "); }


                try { reader["Display"].ToString(); }
                catch { missing.Append("Display "); }

                try { reader["ViewOnly"].ToString(); }
                catch { missing.Append("ViewOnly "); }

                try { reader["Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["SectionName"].ToString(); }
                catch { missing.Append("SectionName "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public static object ProcessRowByRetrieveSelectedListByViewId(SqlDataReader reader)
        {
            // Create instance of object
            b_UIConfiguration obj = new b_UIConfiguration();

            // Load the object from the database
            obj.LoadFromDatabaseRetrieveSelectedListByViewId(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseRetrieveSelectedListByViewId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // UIConfigurationId column, bigint, not null
                UIConfigurationId = reader.GetInt64(i++);

                // ColumnName column, nvarchar(31), not null
                ColumnName = reader.GetString(i++);

                // ColumnLabel column, nvarchar(67), not null
                ColumnLabel = reader.GetString(i++);

                // Order column,  int, not null
                Order = reader.GetInt32(i++);

                // Required column, bit, not null
                Required = reader.GetBoolean(i++);

                // SystemRequired column, bit, not null
                SystemRequired = reader.GetBoolean(i++);

                // DataDictionaryId column, bigint, not null
                DataDictionaryId = reader.GetInt64(i++);

                // Section column, bit, not null
                Section = reader.GetBoolean(i++);

                // SectionName column, nvarchar(31), not null
                SectionName = reader.GetString(i++);

                // UDF column, bit, not null
                UDF = reader.GetBoolean(i++);
                ColumnType = reader.GetString(i++);
                LookupType = reader.GetString(i++);
                LookupName = reader.GetString(i++);
                TableName = reader.GetString(i++);

                // DisplayonForm column, bit, not null
                DisplayonForm = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["UIConfigurationId"].ToString(); }
                catch { missing.Append("UIConfigurationId "); }

                try { reader["ColumnName"].ToString(); }
                catch { missing.Append("ColumnName "); }

                try { reader["ColumnLabel"].ToString(); }
                catch { missing.Append("ColumnLabel "); }

                try { reader["Order"].ToString(); }
                catch { missing.Append("Order "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["SystemRequired"].ToString(); }
                catch { missing.Append("SystemRequired "); }

                try { reader["DataDictionaryId"].ToString(); }
                catch { missing.Append("DataDictionaryId "); }

                try { reader["Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["SectionName"].ToString(); }
                catch { missing.Append("SectionName "); }

                try { reader["UDF"].ToString(); }
                catch { missing.Append("UDF "); }

                try { reader["ColumnType"].ToString(); }
                catch { missing.Append("ColumnType "); }

                try { reader["LookupType"].ToString(); }
                catch { missing.Append("LookupType "); }

                try { reader["LookupName"].ToString(); }
                catch { missing.Append("LookupName "); }

                try { reader["TableName"].ToString(); }
                catch { missing.Append("TableName "); }

                try { reader["DisplayonForm"].ToString(); }
                catch { missing.Append("DisplayonForm "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveAvailableListByViewId_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_UIConfiguration> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_UIConfiguration> results = null;
            data = new List<b_UIConfiguration>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_UIConfiguration_RetrieveAvailableListByViewId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_UIConfiguration>();
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
        public static b_UIConfiguration ProcessRowForRetrieveAvailableListByViewId_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_UIConfiguration obj = new b_UIConfiguration();

            // Load the object from the database
            obj.LoadFromDatabaseforRetrieveAvailableListByViewId_V2(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseforRetrieveAvailableListByViewId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // UIConfigurationId column, bigint, not null
                UIConfigurationId = reader.GetInt64(i++);

                // ColumnName column, nvarchar(31), not null
                ColumnName = reader.GetString(i++);

                // ColumnLabel column, nvarchar(67), not null
                ColumnLabel = reader.GetString(i++);

                // Order column,  int, not null
                Order = reader.GetInt32(i++);

                // Required column, bit, not null
                Required = reader.GetBoolean(i++);

                // SystemRequired column, bit, not null
                SystemRequired = reader.GetBoolean(i++);
                // DataDictionaryId column, bigint, not null
                DataDictionaryId = reader.GetInt64(i++);

                // Section column, bit, not null
                Section = reader.GetBoolean(i++);

                // SectionName column, nvarchar(31), not null
                SectionName = reader.GetString(i++);

                // UDF column, bit, not null
                UDF = reader.GetBoolean(i++);

                TableName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["UIConfigurationId"].ToString(); }
                catch { missing.Append("UIConfigurationId "); }

                try { reader["ColumnName"].ToString(); }
                catch { missing.Append("ColumnName "); }

                try { reader["ColumnLabel"].ToString(); }
                catch { missing.Append("ColumnLabel "); }

                try { reader["Order"].ToString(); }
                catch { missing.Append("Order "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["SystemRequired"].ToString(); }
                catch { missing.Append("SystemRequired "); }

                try { reader["DataDictionaryId"].ToString(); }
                catch { missing.Append("DataDictionaryId "); }

                try { reader["Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["SectionName"].ToString(); }
                catch { missing.Append("SectionName "); }

                try { reader["UDF"].ToString(); }
                catch { missing.Append("UDF "); }

                try { reader["TableName"].ToString(); }
                catch { missing.Append("TableName "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        #region Update Available and Selected list
        public void UiConfigurationUpdateForAvailableandSelectedListInDatabase(
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
                StoredProcedure.usp_UIConfiguration_UpdateAbailableandSelectedlistByUiConfigId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Validate Section Name by ui view id

        public void ValidateSectionNameByUIViewId(
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
                results = Database.StoredProcedure.usp_UIConfiguration_ValidateSectionNameByUiViewId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #endregion

        #region Get List of Columns From Table
        public void RetrieveListColumnsFromTable_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_UIConfiguration> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_UIConfiguration> results = null;
            data = new List<b_UIConfiguration>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_GetListColumnsFromTable_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_UIConfiguration>();
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

        public static b_UIConfiguration ProcessRowForRetrieveListColumnsFromTable_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_UIConfiguration obj = new b_UIConfiguration();

            // Load the object from the database
            obj.LoadFromDatabaseforRetrieveListColumnsFromTable_V2(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseforRetrieveListColumnsFromTable_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ColumnName column, nvarchar(31), not null
                ColumnName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

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
        #endregion

        #region Update Column while remove from selectedlist
        public void UpdateColumnSettingByDataDictionaryIdWhileRemove_V2(
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
                StoredProcedure.usp_UIConfiguration_UpdateColumnsWhileremoveColumnfromSelectedCard_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
