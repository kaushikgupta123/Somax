using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_KBTopics
    {
        #region Property
       
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
      
        public Int64 ObjectId { get; set; }
        public string ObjectName { get; set; }

      
        public string Flag { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public List<b_KBTopics> listOfKBTopics { get; set; }
       
        public Int32 TotalCount { get; set; }
        public string Assigned { get; set; }
        public long PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CategoryName { get; set; }
        public string locallang { get; set; }

        public List<List<b_KBTopics>> TotalRecords { get; set; }

        #endregion

        #region Search
        public static b_KBTopics ProcessRetrieveForChunkV2(SqlDataReader reader)
        {
            b_KBTopics Equipment = new b_KBTopics();

            Equipment.LoadFromDatabaseForKBTopicsChunkSearchV2(reader);
            return Equipment;
        }
        public int LoadFromDatabaseForKBTopicsChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            { 
                // KBTopicsId Id
                KBTopicsId = reader.GetInt64(i++);

                // Title column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                Title = reader.GetString(i);
                }
                else
                {
                Title = "";
                }
                i++;

                // Category column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    Category = reader.GetString(i);
                }
                else
                {
                    Category = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CategoryName = reader.GetString(i);
                }
                else
                {
                    CategoryName = "";
                }
                i++;

                // Description column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // Tags column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Tags = reader.GetString(i);
                }
                else
                {
                    Tags = "";
                }
                i++;
                // Folder column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Folder = reader.GetString(i);
                }
                else
                {
                    Folder = "";
                }
                i++;

                TotalCount = reader.GetInt32(i);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["KBTopicsId"].ToString(); }
                catch { missing.Append("KBTopicsId"); }

                try { reader["Title"].ToString(); }
                catch { missing.Append("Title"); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category"); }

                try { reader["CategoryName"].ToString(); }
                catch { missing.Append("CategoryName"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["Tags"].ToString(); }
                catch { missing.Append("Tags"); }


                try { reader["Folder"].ToString(); }
                catch { missing.Append("Folder"); }


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

        public int LoadFromDatabaseByPKForeignKey_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // KBTopicsId Id
                KBTopicsId = reader.GetInt64(i++);

                // Title column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Title = reader.GetString(i);
                }
                else
                {
                    Title = "";
                }
                i++;

                // Category column, string, not null
                if (false == reader.IsDBNull(i))
                {
                    Category = reader.GetString(i);
                }
                else
                {
                    Category = "";
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    CategoryName = reader.GetString(i);
                }
                else
                {
                    CategoryName = "";
                }
                i++;
                // Description column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // Tags column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Tags = reader.GetString(i);
                }
                else
                {
                    Tags = "";
                }
                i++;
                // Folder column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Folder = reader.GetString(i);
                }
                else
                {
                    Folder = "";
                }
                i++;

                

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["KBTopicsId"].ToString(); }
                catch { missing.Append("KBTopicsId"); }

                try { reader["Title"].ToString(); }
                catch { missing.Append("Title"); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["Tags"].ToString(); }
                catch { missing.Append("Tags"); }


                try { reader["Folder"].ToString(); }
                catch { missing.Append("Folder"); }


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

        
        public void RetrieveChunkSearchV2(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref b_KBTopics results
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

                results = Database.StoredProcedure.usp_KBTopics_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Details
        public void RetrieveByForeignKeysFromDatabase_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_KBTopics> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_KBTopics>(reader => { this.LoadFromDatabaseByPKForeignKey_V2(reader); return this; });
                StoredProcedure.usp_KBTopics_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        #endregion

        #region retrieve Tags
        public static object ProcessRowRetrievetags(SqlDataReader reader)
        {
            b_KBTopics kbtopics = new b_KBTopics();
            kbtopics.LoadFromDatabaseRetrieveTagName(reader);
            // Return result
            return (object)kbtopics;
        }
        public void LoadFromDatabaseRetrieveTagName(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    FullName = reader.GetString(i);
                }
                else
                {
                    FullName = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["FullName"].ToString(); }
                catch { missing.Append("FullName "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }


        }
        public void LoadFromDatabaseRetrieveTags(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NameMiddle = reader.GetString(i);
                }
                else
                {
                    NameMiddle = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    FullName = reader.GetString(i);
                }
                else
                {
                    FullName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    UserName = reader.GetString(i);
                }
                else
                {
                    UserName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Email = reader.GetString(i);
                }
                else
                {
                    Email = string.Empty;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameMiddle"].ToString(); }
                catch { missing.Append("NameMiddle "); }

                try { reader["FullName"].ToString(); }
                catch { missing.Append("FullName "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            
        }

        public void RetrievePersonnelInitial(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<List<b_KBTopics>> data
    )
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<List<b_KBTopics>> results = null;
            data = new List<List<b_KBTopics>>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_KBTopics_RetrieveTags_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<List<b_KBTopics>>();
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
    }

}
