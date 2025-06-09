using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_DashboardContent
    {
        #region Property
        public int GridColWidth { get; set; }
        public string ViewName { get; set; }
        public string Name { get; set; }
        #endregion
        public static b_DashboardContent ProcessRowForDashboardContentGetAllV2(SqlDataReader reader)
        {
            // Create instance of object
            b_DashboardContent obj = new b_DashboardContent();

            // Load the object from the database
            obj.LoadFromDatabaseForDashBoardContent(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForDashBoardContent(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EquipmentId column, bigint, not null
                DashboardContentId = reader.GetInt64(i++);

                //DashboardListingId
                DashboardListingId = reader.GetInt64(i++);

                //WidgetListingId
                WidgetListingId = reader.GetInt64(i++);

                //Display
                Display = reader.GetBoolean(i++);

                //Required
                Required = reader.GetBoolean(i++);

                //GridColWidth
                GridColWidth = reader.GetInt32(i++);

                //ViewName
                if (false == reader.IsDBNull(i))
                {
                    ViewName = reader.GetString(i);
                }
                else
                {
                    ViewName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;


            }
            catch (Exception ex)
            {
               
                StringBuilder missing = new StringBuilder();

                try { reader["DashboardContentId"].ToString(); }
                catch { missing.Append("DashboardContentId "); }

                try { reader["DashboardListingId"].ToString(); }
                catch { missing.Append("DashboardListingId "); }

                try { reader["WidgetListingId"].ToString(); }
                catch { missing.Append("WidgetListingId "); }

                try { reader["Display"].ToString(); }
                catch { missing.Append("Display "); }

                try { reader["Required"].ToString(); }
                catch { missing.Append("Required "); }

                try { reader["GridColWidth"].ToString(); }
                catch { missing.Append("GridColWidth "); }

                try { reader["ViewName"].ToString(); }
                catch { missing.Append("ViewName "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

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

        public void GetAllDashboardContent(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_DashboardContent> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_DashboardContent> results = null;
            data = new List<b_DashboardContent>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;


                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_DashboardContent_RetrieveByDashboardListingId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_DashboardContent>();
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
