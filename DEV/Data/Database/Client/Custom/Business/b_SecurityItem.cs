/*
 *  Added By Indusnet Technologies
 */ 

using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_SecurityItem
    {
        #region Properties

        public bool Protected { get; set; }
        public long AccessClientId { get; set;}
        public string SecurityItemTab { get; set; }
        public string PackageLevel { get; set; }
        public int ProductGrouping { get; set; }
        #endregion

        public static b_SecurityItem ProcessRowForRetrivalAllByClientAndSecurityProfile(SqlDataReader reader)
        {
            // Create instance of object
            b_SecurityItem obj = new b_SecurityItem();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrivalAllByClientAndSecurityProfile(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForRetrivalAllByClientAndSecurityProfile(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {

                //// ClientId column, bigint, not null
                //ClientId = reader.GetInt64(i++);

                //// SecurityItemId column, bigint, not null
                //SecurityItemId = reader.GetInt64(i++);

                //// SecurityProfileId column, bigint, not null
                //SecurityProfileId = reader.GetInt64(i++);

                //// ItemName column, nvarchar(63), not null
                //ItemName = reader.GetString(i++);

                //// SortOrder column, int, not null
                //SortOrder = reader.GetInt32(i++);

                //// SingleItem column, bit, not null
                //SingleItem = reader.GetBoolean(i++);

                //// ItemAccess column, bit, not null
                //ItemAccess = reader.GetBoolean(i++);

                //// ItemCreate column, bit, not null
                //ItemCreate = reader.GetBoolean(i++);

                //// ItemEdit column, bit, not null
                //ItemEdit = reader.GetBoolean(i++);

                //// ItemDelete column, bit, not null
                //ItemDelete = reader.GetBoolean(i++);

                //// ReportItem column, bit, not null
                //ReportItem = reader.GetBoolean(i++);

                //// UpdateIndex column, int, not null
                //UpdateIndex = reader.GetInt32(i++);

                Protected = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                //try { reader["ClientId"].ToString(); }
                //catch { missing.Append("ClientId "); }

                //try { reader["SecurityItemId"].ToString(); }
                //catch { missing.Append("SecurityItemId "); }

                //try { reader["SecurityProfileId"].ToString(); }
                //catch { missing.Append("SecurityProfileId "); }

                //try { reader["ItemName"].ToString(); }
                //catch { missing.Append("ItemName "); }

                //try { reader["SortOrder"].ToString(); }
                //catch { missing.Append("SortOrder "); }

                //try { reader["SingleItem"].ToString(); }
                //catch { missing.Append("SingleItem "); }

                //try { reader["ItemAccess"].ToString(); }
                //catch { missing.Append("ItemAccess "); }

                //try { reader["ItemCreate"].ToString(); }
                //catch { missing.Append("ItemCreate "); }

                //try { reader["ItemEdit"].ToString(); }
                //catch { missing.Append("ItemEdit "); }

                //try { reader["ItemDelete"].ToString(); }
                //catch { missing.Append("ItemDelete "); }

                //try { reader["UpdateIndex"].ToString(); }
                //catch { missing.Append("UpdateIndex "); }

                try { reader["Protected"].ToString(); }
                catch { missing.Append("Protected "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }         

        }





        public int CreateTemplate(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName           
         )
        {
            SqlCommand command = null;
            int retNoOfTemplateCreated = 0;
            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                retNoOfTemplateCreated = Database.StoredProcedure.usp_SecurityItem_CreateTemplate.CallStoredProcedure(command, callerUserInfoId, callerUserName,ClientId);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }

            return (retNoOfTemplateCreated);
        }


        public void RetrieveAllByClientAndSecurityProfileId(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
    string callerUserName,
         ref List<b_SecurityItem> data
      )
        {
            List<b_SecurityItem> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new List<b_SecurityItem>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SecurityItem_RetrieveAllByClientAndSecurityProfile.CallStoredProcedure(command,callerUserInfoId, callerUserName,this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SecurityItem>();
                }

                // Clear the results collection
                //if (null != results)
                //{
                //    results.Clear();
                //    results = null;
                //}
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }



        public void CustomRetrieveAll_V2(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
 string callerUserName,
      ref List<b_SecurityItem> data
   )
        {
            List<b_SecurityItem> results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new List<b_SecurityItem>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_CustomSecurityItem_RetrieveAll_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SecurityItem>();
                }

                // Clear the results collection
                //if (null != results)
                //{
                //    results.Clear();
                //    results = null;
                //}
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

    }


}
