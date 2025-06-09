using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{

    public partial class b_UIConfig
    {

        #region Property
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string isHide { get; set; }
        public string isRequired { get; set; }
        public string Isexternal { get; set; }

        #endregion
        public static b_UIConfig ProcessRowForusp_UIConfig_RetrieveHiddenByViewOrTable(SqlDataReader reader)
        {
            // Create instance of object
            b_UIConfig obj = new b_UIConfig();

            // Load the object from the database
            obj.LoadFromDatabaseforRetrieveHiddenByViewOrTable(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseforRetrieveHiddenByViewOrTable(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {
                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                // CreateBy column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;


                // ModifyDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                }
                i++;

                // ModifyBy column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
               
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }               

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }              


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

        public void RetrieveHiddenByViewOrTable(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_UIConfig> data
      )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_UIConfig> results = null;
            data = new List<b_UIConfig>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_UIConfig_RetrieveHiddenByViewOrTable.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_UIConfig>();
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
}
