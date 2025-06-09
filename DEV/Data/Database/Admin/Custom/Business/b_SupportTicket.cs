using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_SupportTicket
    {
        #region Properties
        public DateTime? CreateDate { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public string Contact { get; set; }
        public string Agent { get; set; }
        public long PersonnelId { get; set; }
        public string TagName { get; set; }
        public List<List<b_SupportTicket>> TotalRecords { get; set; }
        #endregion
        public void RetrieveChunkSearch(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_SupportTicket> results
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

                results = Database.StoredProcedure.usp_SupportTicket_RetrieveForChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_SupportTicket ProcessRowForSupportTicketRetriveAllForChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_SupportTicket SupportTicket = new b_SupportTicket();
            SupportTicket.LoadFromDatabaseForSupportTicketRetriveAllForChunkSearch(reader);
            return SupportTicket;
        }

        public int LoadFromDatabaseForSupportTicketRetriveAllForChunkSearch(SqlDataReader reader)
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
                // SiteId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // SupportTicketId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    SupportTicketId = reader.GetInt64(i);
                }
                else
                {
                    SupportTicketId = 0;
                }
                i++;
                // Subject column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Subject = reader.GetString(i);
                }
                else
                {
                    Subject = string.Empty;
                }
                i++;
                // Contact column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Contact = reader.GetString(i);
                }
                else
                {
                    Contact = string.Empty;
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
                // Agent column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Agent = reader.GetString(i);
                }
                else
                {
                    Agent = string.Empty;
                }
                i++;
                // CreateDate column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // CompleteDate column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["SupportTicketId"].ToString(); }
                catch { missing.Append("SupportTicketId "); }

                try { reader["Subject"].ToString(); }
                catch { missing.Append("Subject "); }

                try { reader["Contact"].ToString(); }
                catch { missing.Append("Contact "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Agent"].ToString(); }
                catch { missing.Append("Agent "); }


                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

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
        public void RetrieveTags(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<List<b_SupportTicket>> data
    )
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<List<b_SupportTicket>> results = null;
            data = new List<List<b_SupportTicket>>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_SupportTicket_RetrieveTags_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<List<b_SupportTicket>>();
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
        public static object ProcessRowRetrievetags(SqlDataReader reader)
        {
            b_SupportTicket SupportTicket = new b_SupportTicket();
            SupportTicket.LoadFromDatabaseRetrieveTagName(reader);
            // Return result
            return (object)SupportTicket;
        }
        public void LoadFromDatabaseRetrieveTagName(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    TagName = reader.GetString(i);
                }
                else
                {
                    TagName = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["TagName"].ToString(); }
                catch { missing.Append("TagName "); }

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
