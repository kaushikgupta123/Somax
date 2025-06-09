using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_ClientUserInfoList
    {
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public Int32 totalCount { get; set; }

        public void RetrieveChunkSearchClientUserInfoList(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_ClientUserInfoList> results
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

                results = Database.StoredProcedure.usp_ClientUserInfoList_RetrieveByUserInfoIdChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static b_ClientUserInfoList ProcessRowForChunkSearchClientUserInfoList(SqlDataReader reader)
        {
            // Create instance of object
            b_ClientUserInfoList ClientUserInfoList = new b_ClientUserInfoList();
            ClientUserInfoList.LoadFromDatabaseForChunkSearchClientUserInfoList(reader);
            return ClientUserInfoList;
        }

        public int LoadFromDatabaseForChunkSearchClientUserInfoList(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                

                // PartCategoryMasterId column, bigint, not null
                ClientUserInfoListID = reader.GetInt64(i++);



                // ClientName column, nvarchar(63), not null
                ClientName = reader.GetString(i++);

                // SiteName column, nvarchar(63), not null
                SiteName = reader.GetString(i++);

                // ClientId column, int, not null
                ClientId = reader.GetInt64(i++);

                // DefaultSiteId column, int, not null
                DefaultSiteId = reader.GetInt64(i++);

                // totalCount column, int, not null
                totalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();

                try { reader["ClientUserInfoListId"].ToString(); }
                catch { missing.Append("ClientUserInfoListId "); }

                try { reader["ClientName"].ToString(); }
                catch { missing.Append("ClientName "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["DefaultSiteId"].ToString(); }
                catch { missing.Append("DefaultSiteId "); }

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

    }
}
