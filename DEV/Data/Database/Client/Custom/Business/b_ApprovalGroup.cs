using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
   public partial class b_ApprovalGroup
    {
        #region properties
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string AssetGroup1ClientLookupId { get; set; }
        public string AssetGroup2ClientLookupId { get; set; }
        public string AssetGroup3ClientLookupId { get; set; }
        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 ChildCount { get; set; }
        public List<b_ApprovalGroup> listOfApprovalGroup { get; set; }
        public string SearchText { get; set; }
        #endregion

        public void RetrieveApprovalGroupChunkSearchV2(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref b_ApprovalGroup results
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

                results = Database.StoredProcedure.usp_ApprovalGroup_RetrieveAllChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_ApprovalGroup ProcessRetrieveApprovalGroupForChunkV2(SqlDataReader reader)
        {
            b_ApprovalGroup ApprovalGroup = new b_ApprovalGroup();

            ApprovalGroup.LoadFromDatabaseForApprovalGroupChunkSearchV2(reader);
            return ApprovalGroup;
        }

        public int LoadFromDatabaseForApprovalGroupChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // ApprovalGroupId column, bigint, not null
                ApprovalGroupId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    RequestType = reader.GetString(i);
                }
                else
                {
                    RequestType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                //AreaId = reader.GetInt64(i++);
                AssetGroup1 = reader.GetInt64(i++);

                AssetGroup2 = reader.GetInt64(i++);

                AssetGroup3 = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup3ClientLookupId = "";
                }
                i++;

                AssetGroup1Id = reader.GetInt64(i++);
                AssetGroup2Id = reader.GetInt64(i++);
                AssetGroup3Id = reader.GetInt64(i++);

                ChildCount=reader.GetInt32(i++);
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ApprovalGroupId"].ToString(); }
                catch { missing.Append("ApprovalGroupId "); }

                try { reader["RequestType"].ToString(); }
                catch { missing.Append("RequestType "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }
                
                try { reader["AssetGroup1"].ToString(); }
                catch { missing.Append("AssetGroup1 "); }
                
                try { reader["AssetGroup2"].ToString(); }
                catch { missing.Append("AssetGroup2 "); }
                
                try { reader["AssetGroup3"].ToString(); }
                catch { missing.Append("AssetGroup3 "); }
                
                try { reader["AssetGroup1ClientLookupId"].ToString(); }
                catch { missing.Append("AssetGroup1ClientLookupId "); }
                
                try { reader["AssetGroup2ClientLookupId"].ToString(); }
                catch { missing.Append("AssetGroup2ClientLookupId "); }
                
                try { reader["AssetGroup3ClientLookupId"].ToString(); }
                catch { missing.Append("AssetGroup3ClientLookupId "); }

                try { reader["AssetGroup1Id"].ToString(); }
                catch { missing.Append("AssetGroup1Id "); }

                try { reader["AssetGroup2Id"].ToString(); }
                catch { missing.Append("AssetGroup2Id "); }

                try { reader["AssetGroup3Id"].ToString(); }
                catch { missing.Append("AssetGroup3Id "); }
                
                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }


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

        public void RetrieveByIdFromDatabase_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_ApprovalGroup> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ApprovalGroup>(reader => { this.LoadFromDatabaseById_V2(reader); return this; });
                Database.StoredProcedure.usp_ApprovalGroup_RetrieveById_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public int LoadFromDatabaseById_V2(SqlDataReader reader)
        {
            int i = 0;
            i=LoadFromDatabase(reader);
            try
            {
                AssetGroup1ClientLookupId = reader.GetString(i++);
                AssetGroup2ClientLookupId = reader.GetString(i++);
                AssetGroup3ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["AssetGroup1_ClientLookupId"].ToString(); }
                catch { missing.Append("AssetGroup1_ClientLookupId "); }

                try { reader["AssetGroup2_ClientLookupId"].ToString(); }
                catch { missing.Append("AssetGroup2_ClientLookupId "); }
                
                try { reader["AssetGroup3_ClientLookupId"].ToString(); }
                catch { missing.Append("AssetGroup3_ClientLookupId "); }

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
