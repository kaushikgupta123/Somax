using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_ServiceOrderSchedule
    {
        #region Property
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Flag { get; set; }
        public long SiteId { get; set; }   
        public List<List<b_ServiceOrderSchedule>> TotalRecords { get; set; }
        #endregion

        public static object ProcessRowRetrievePersonnel(SqlDataReader reader)
        {

            // Create instance of object
            b_ServiceOrderSchedule obj = new b_ServiceOrderSchedule();

            // Load the object from the database
            obj.LoadFromDatabaseRetrievePersonnel(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseRetrievePersonnel(SqlDataReader reader)
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

        public void RetrievePersonnel(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<List<b_ServiceOrderSchedule>> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<List<b_ServiceOrderSchedule>> results = null;
            data = new List<List<b_ServiceOrderSchedule>>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ServiceOrderSchedule_RetrievePersonnel_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<List<b_ServiceOrderSchedule>>();
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
