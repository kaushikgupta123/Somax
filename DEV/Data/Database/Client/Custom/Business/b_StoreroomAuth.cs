using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_StoreroomAuth
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SiteName { get; set; }
        public string SearchText { get; set; }
        public string StoreroomName { get; set; }
        public Int32 totalCount { get; set; }

        public void RetrieveChunkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_StoreroomAuth> results
        )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_StoreroomAuth_RetrieveByClientId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static b_StoreroomAuth ProcessRowForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_StoreroomAuth storeroomAuth = new b_StoreroomAuth();
            storeroomAuth.LoadFromDatabaseForChunkSearch(reader);
            return storeroomAuth;
        }

        public int LoadFromDatabaseForChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // i = LoadFromDatabase(reader);


                // PartCategoryMasterId column, bigint, not null
                StoreroomAuthId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                SiteName = reader.GetString(i++);

                // Description column, nvarchar(63), not null
                StoreroomName = reader.GetString(i++);

                

                // totalCount column, int, not null
                totalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["StoreroomAuthId"].ToString(); }
                catch { missing.Append("StoreroomAuthId "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["StoreroomName"].ToString(); }
                catch { missing.Append("StoreroomName "); }

                try { reader["totalCount"].ToString(); }
                catch { missing.Append("totalCount "); }


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

        public void RetrieveAllStoreroomBySiteId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_StoreroomAuth obj, ref List<b_StoreroomAuth> results)
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Storeroom_RetrieveAllBySiteId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        public static b_StoreroomAuth ProcessRowStoreroomForSiteId(SqlDataReader reader)
        {
            // Create instance of object
            b_StoreroomAuth storeroomAuth = new b_StoreroomAuth();
            storeroomAuth.LoadFromDatabaseStoreroomForSiteId(reader);
            return storeroomAuth;
        }
        public int LoadFromDatabaseStoreroomForSiteId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // i = LoadFromDatabase(reader);


                // PartCategoryMasterId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                StoreroomName = reader.GetString(i++);

                

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["StoreroomName"].ToString(); }
                catch { missing.Append("StoreroomName "); }


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

        public void RetrieveByStoreroomAuthId(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_StoreroomAuth> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_StoreroomAuth>(reader => { this.LoadRetrieveByStoreroomAuthIdFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_StoreroomAuth_RetrieveByStoreroomAuthId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public int LoadRetrieveByStoreroomAuthIdFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // StoreroomAuthId column, bigint, not null
                StoreroomAuthId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // Maintain column, bit, not null
                Maintain = reader.GetBoolean(i++);

                // Issue column, bit, not null
                Issue = reader.GetBoolean(i++);

                // IssueTransfer column, bit, not null
                IssueTransfer = reader.GetBoolean(i++);

                // ReceiveTransfer column, bit, not null
                ReceiveTransfer = reader.GetBoolean(i++);

                // PhysicalInventory column, bit, not null
                PhysicalInventory = reader.GetBoolean(i++);

                // Purchase column, bit, not null
                Purchase = reader.GetBoolean(i++);

                // ReceivePurchase column, bit, not null
                ReceivePurchase = reader.GetBoolean(i++);

                SiteName = reader.GetString(i++);

                StoreroomName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["StoreroomAuthId"].ToString(); }
                catch { missing.Append("StoreroomAuthId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Maintain"].ToString(); }
                catch { missing.Append("Maintain "); }

                try { reader["Issue"].ToString(); }
                catch { missing.Append("Issue "); }

                try { reader["IssueTransfer"].ToString(); }
                catch { missing.Append("IssueTransfer "); }

                try { reader["ReceiveTransfer"].ToString(); }
                catch { missing.Append("ReceiveTransfer "); }

                try { reader["PhysicalInventory"].ToString(); }
                catch { missing.Append("PhysicalInventory "); }

                try { reader["Purchase"].ToString(); }
                catch { missing.Append("Purchase "); }

                try { reader["ReceivePurchase"].ToString(); }
                catch { missing.Append("ReceivePurchase "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["StoreroomName"].ToString(); }
                catch { missing.Append("StoreroomName "); }


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

        public void ValidateSiteId(
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
                results = Database.StoredProcedure.usp_StoreroomAuth_ValidateSiteId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
