using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_UserReports
    {
        #region Properties
        public int IsFavorite { get; set; }
        public long ReportFavoritesId { get; set; }
        public long PersonnelId { get; set; }
        #endregion
        public Int32 Count { get; set; }
        public static object ProcessRowForCount(SqlDataReader reader)
        {
            b_UserReports obj = new b_UserReports();
            obj.LoadFromDatabaseCustom(reader);
            return (object)obj;
        }

        public int LoadFromDatabaseCustom(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                Count = reader.GetInt32(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["Count"].ToString(); }
                catch { missing.Append("Count "); }

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

        public void RetrieveCountforReportName(SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_UserReports> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_UserReports> results = null;
            data = new List<b_UserReports>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserReport_CountIfReportNameExist_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_UserReports>();
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

        public void RetrieveByGroup(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
       string callerUserName,
             ref List<b_UserReports> data
         )
        {
            Database.SqlClient.ProcessRow<b_UserReports> processRow = null;
            List<b_UserReports> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new List<b_UserReports>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data                
                results = Database.StoredProcedure.usp_UserReports_RetrieveByGroup.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_UserReports ProcessRowForGroupList(SqlDataReader reader)
        {
            // Create instance of object
            b_UserReports UserReports = new b_UserReports();

            // Load the object from the database
            UserReports.LoadFromDatabaseByGroup(reader);

            // Return result
            return UserReports;
        }
        public int LoadFromDatabaseByGroup(SqlDataReader reader)
        {
           //int i = LoadFromDatabase(reader);
            int i = 0;
            try
            {
                UserReportsId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    ReportName = reader.GetString(i);

                }
                else
                {
                    ReportName = "";
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

                if (false == reader.IsDBNull(i))
                {
                    ReportGroup = reader.GetString(i);
                }
                else
                {
                    ReportGroup = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SourceName = reader.GetString(i);
                }
                else
                {
                    SourceName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    UseSP = reader.GetBoolean(i);

                }
                else
                {
                    UseSP = false;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PrimarySortColumn = reader.GetString(i);
                }
                else
                {
                    PrimarySortColumn = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SecondarySortColumn = reader.GetString(i);
                }
                else
                {
                    SecondarySortColumn = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IsGrouped = reader.GetBoolean(i);

                }
                else
                {
                    IsGrouped = false;
                }
                i++;
                               
                if (false == reader.IsDBNull(i))
                {
                    GroupColumn = reader.GetString(i);
                }
                else
                {
                    GroupColumn = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IncludePrompt = reader.GetBoolean(i);

                }
                else
                {
                    IncludePrompt = false;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Prompt1Source = reader.GetString(i);
                }
                else
                {
                    Prompt1Source = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Prompt1Type = reader.GetString(i);
                }
                else
                {
                    Prompt1Type = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Prompt1ListSource = reader.GetString(i);
                }
                else
                {
                    Prompt1ListSource = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Prompt1List = reader.GetString(i);
                }
                else
                {
                    Prompt1List = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Prompt2Source = reader.GetString(i);
                }
                else
                {
                    Prompt2Source = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Prompt2Type = reader.GetString(i);
                }
                else
                {
                    Prompt2Type = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Prompt2ListSource = reader.GetString(i);
                }
                else
                {
                    Prompt2ListSource = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Prompt2List = reader.GetString(i);
                }
                else
                {
                    Prompt2List = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ChildLinkColumn = reader.GetString(i);
                }
                else
                {
                    ChildLinkColumn = "";
                }
                i++;
              
                if (false == reader.IsDBNull(i))
                {
                    MasterLinkColumn = reader.GetString(i);
                }
                else
                {
                    MasterLinkColumn = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ChildSourceName = reader.GetString(i);
                }
                else
                {
                    ChildSourceName = "";
                }
                i++;
               
                if (false == reader.IsDBNull(i))
                {
                    SaveType = reader.GetString(i);
                }
                else
                {
                    SaveType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IncludeChild = reader.GetBoolean(i);

                }
                else
                {
                    IncludeChild = false;
                }
                i++;
                IsFavorite = reader.GetInt32(i++);

                ReportFavoritesId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["UserReportsId"].ToString(); }
                catch { missing.Append("UserReportsId "); }

                try { reader["ReportName"].ToString(); }
                catch { missing.Append("ReportName "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ReportGroup"].ToString(); }
                catch { missing.Append("ReportGroup "); }

                try { reader["SourceName"].ToString(); }
                catch { missing.Append("SourceName "); }

                try { reader["UseSP"].ToString(); }
                catch { missing.Append("UseSP "); }

                try { reader["PrimarySortColumn"].ToString(); }
                catch { missing.Append("PrimarySortColumn "); }

                try { reader["SecondarySortColumn"].ToString(); }
                catch { missing.Append("SecondarySortColumn "); }

                try { reader["IsGrouped"].ToString(); }
                catch { missing.Append("IsGrouped "); }

                try { reader["GroupColumn"].ToString(); }
                catch { missing.Append("GroupColumn "); }

                try { reader["IncludePrompt"].ToString(); }
                catch { missing.Append("IncludePrompt "); }

                try { reader["Prompt1Source"].ToString(); }
                catch { missing.Append("Prompt1Source "); }

                try { reader["Prompt1Type"].ToString(); }
                catch { missing.Append("Prompt1Type "); }

                try { reader["Prompt1ListSource"].ToString(); }
                catch { missing.Append("Prompt1ListSource "); }

                try { reader["Prompt1List"].ToString(); }
                catch { missing.Append("Prompt1List "); }

                try { reader["Prompt2Source"].ToString(); }
                catch { missing.Append("Prompt2Source "); }

                try { reader["Prompt2Type"].ToString(); }
                catch { missing.Append("Prompt2Type "); }

                try { reader["Prompt2ListSource"].ToString(); }
                catch { missing.Append("Prompt2ListSource "); }

                try { reader["Prompt2List"].ToString(); }
                catch { missing.Append("Prompt2List "); }             

                try { reader["ChildLinkColumn"].ToString(); }
                catch { missing.Append("ChildLinkColumn "); }

                try { reader["MasterLinkColumn"].ToString(); }
                catch { missing.Append("MasterLinkColumn "); }

                try { reader["ChildSourceName"].ToString(); }
                catch { missing.Append("ChildSourceName "); }

                try { reader["Filter"].ToString(); }
                catch { missing.Append("Filter "); }

                try { reader["SaveType"].ToString(); }
                catch { missing.Append("SaveType "); }

                try { reader["IncludeChild"].ToString(); }
                catch { missing.Append("IncludeChild "); }

                try { reader["IsFavorite"].ToString(); }
                catch { missing.Append("IsFavorite "); }

                try { reader["ReportFavoritesId"].ToString(); }
                catch { missing.Append("IsFavorite "); }

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
