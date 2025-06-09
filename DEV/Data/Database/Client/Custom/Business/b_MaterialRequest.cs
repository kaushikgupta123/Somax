using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_MaterialRequest
    {
        #region Property
        public DateTime? CreateDate { get; set; }
        public string AdvRequiredDate { get; set; }
        public string AccountClientLookupId { get; set; }
        public string AdvCreateDate { get; set; }
        public string AdvCompleteDate { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public Int32 ChildCount { get; set; }
        public string Personnel_NameFirst { get; set; }
        public string Personnel_NameLast { get; set; }
        #endregion


        public void RetrieveChunkSearch(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_MaterialRequest> results
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

                results = Database.StoredProcedure.usp_MaterialRequest_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_MaterialRequest ProcessRowForMaterialRequestRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_MaterialRequest MaterialRequest = new b_MaterialRequest();
            MaterialRequest.LoadFromDatabaseForMaterialRequestRetriveAllForSearch(reader);
            return MaterialRequest;
        }

        public int LoadFromDatabaseForMaterialRequestRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;
                // PurchaseRequestId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    MaterialRequestId = reader.GetInt64(i);
                }
                else
                {
                    MaterialRequestId = 0;
                }
                i++;
                // Description column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;
                // RequiredDate column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                // AccountClientLookupId column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    AccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    AccountClientLookupId = string.Empty;
                }
                i++;
                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // RequiredDate column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // RequiredDate column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                //ChildCount
                ChildCount = reader.GetInt32(i++);

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["MaterialRequestId"].ToString(); }
                catch { missing.Append("MaterialRequestId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["AccountClientLookupId"].ToString(); }
                catch { missing.Append("AccountClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

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


        public void MaterialRequestRetrieveByMaterialRequestId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_MaterialRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_MaterialRequest>(reader => { this.LoadFromDatabaseByMaterialRequestId_V2(reader); return this; });
                Database.StoredProcedure.usp_MaterialRequest_RetrieveByMaterialRequestId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void LoadFromDatabaseByMaterialRequestId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);
                MaterialRequestId = reader.GetInt64(i++);
                SiteId = reader.GetInt64(i++);
                AreaId = reader.GetInt64(i++);
                DepartmentId = reader.GetInt64(i++);
                StoreroomId = reader.GetInt64(i++);
                AccountId = reader.GetInt64(i++);
                Status = reader.GetString(i++);
                Personnel_NameFirst = reader.GetString(i++);
                Personnel_NameLast = reader.GetString(i++);
                //CreateDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                //RequiredDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                //CompleteDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                AccountClientLookupId = reader.GetString(i++);
                Description = reader.GetString(i++);
                Requestor_PersonnelId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["MaterialRequestId"].ToString(); }
                catch { missing.Append("MaterialRequestId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Personnel_NameFirst"].ToString(); }
                catch { missing.Append("Personnel_NameFirst "); }

                try { reader["Personnel_NameLast"].ToString(); }
                catch { missing.Append("Personnel_NameLast "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["AccountClientLookupId"].ToString(); }
                catch { missing.Append("AccountClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Requestor_PersonnelId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelId "); }


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
