using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{

    public partial class b_AssetGroup2 : DataBusinessBase
    {
        #region Properties
        public string ClientLookup_Desc { get; set; }
        #endregion
        public static object ProcessRowForAssetGroup2(SqlDataReader reader)
        {
            b_AssetGroup2 obj = new b_AssetGroup2();
            obj.LoadFromDatabaseCustom(reader);
            return (object)obj;
        }
        public int LoadFromDatabaseCustom(SqlDataReader reader)
        {
            int i = 0;
            i = LoadFromDatabase(reader);
            try
            {
                ClientLookup_Desc = reader.GetString(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientLookup_Desc"].ToString(); }
                catch { missing.Append("ClientLookup_Desc "); }

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

        public void AssetGroup2RetrieveByClientIdSiteId(SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_AssetGroup2> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_AssetGroup2> results = null;
            data = new List<b_AssetGroup2>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_RetrieveByClientIdSiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_AssetGroup2>();
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

        public void AssetGroup2RetrieveByAssetGroup2Id(SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_AssetGroup2> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_AssetGroup2> results = null;
            data = new List<b_AssetGroup2>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_RetrieveByAssetGroup2Id.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_AssetGroup2>();
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


        public void AssetGroup2RetrieveByInActiveFlag_V2(SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_AssetGroup2> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_AssetGroup2> results = null;
            data = new List<b_AssetGroup2>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_RetrieveByInActiveFlag_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_AssetGroup2
>();
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

        public void AssetGroup2RetrieveAllByInActiveFlag_V2(SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_AssetGroup2> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_AssetGroup2> results = null;
            data = new List<b_AssetGroup2>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_RetrieveAllByInActiveFlag_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_AssetGroup2>();
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

        public void AssetGroup2_ValidateNewClientLookupIdV2(
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


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_ValidateNewClientLookupIdV2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void AssetGroup2ValidateOldClientLookupIdV2(
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


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_AssetGroup2_ValidateOldClientLookupIdV2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
    }
}