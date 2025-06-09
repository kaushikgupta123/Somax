using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_HierarchicalList : DataBusinessBase
    {
        public static object ProcessRowForList(SqlDataReader reader)
        {
            // Create instance of object
            b_HierarchicalList obj = new b_HierarchicalList();

            // Load the object from the database
            obj.LoadFromDatabaseForList(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseForList(SqlDataReader reader)
        {            
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);           
                
                SiteId = reader.GetInt64(i++);    
                
                AreaId = reader.GetInt64(i++);     
                
                DepartmentId = reader.GetInt64(i++);

                StoreroomId = reader.GetInt64(i++);

                ListName = reader.GetString(i++);

                InactiveFlag = reader.GetBoolean(i++);         
                
                Language = reader.GetString(i++);

                Culture = reader.GetString(i++);    

                UpdateIndex = reader.GetInt32(i++);

                Level1Value = reader.GetString(i++);

                Level1Description = reader.GetString(i++);

                Level2Value = reader.GetString(i++);

                Level2Description = reader.GetString(i++);

                Level3Value = reader.GetString(i++);

                Level3Description = reader.GetString(i++);

                Level4Value = reader.GetString(i++);

                Level4Description = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ListName"].ToString(); }
                catch { missing.Append("ListName "); }                

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Language"].ToString(); }
                catch { missing.Append("Language "); }

                try { reader["Culture"].ToString(); }
                catch { missing.Append("Culture "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["Level1Value"].ToString(); }
                catch { missing.Append("Level1Value "); }

                try { reader["Level1Description"].ToString(); }
                catch { missing.Append("Level1Description "); }

                try { reader["Level2Value"].ToString(); }
                catch { missing.Append("Level2Value "); }

                try { reader["Level2Description"].ToString(); }
                catch { missing.Append("Level2Description "); }

                try { reader["Level3Value"].ToString(); }
                catch { missing.Append("Level3Value "); }

                try { reader["Level3Description"].ToString(); }
                catch { missing.Append("Level3Description "); }

                try { reader["Level4Value"].ToString(); }
                catch { missing.Append("Level4Value "); }

                try { reader["Level4Description"].ToString(); }
                catch { missing.Append("Level4Description "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveActiveListFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref b_HierarchicalList[] data
        )
        {
            //Database.SqlClient.ProcessRow<b_LookupList> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_HierarchicalList[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_HierarchicalList_RetrieveActiveListByName_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_HierarchicalList[])results.ToArray(typeof(b_HierarchicalList));
                }
                else
                {
                    data = new b_HierarchicalList[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                //processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
    }
}
