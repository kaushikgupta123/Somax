using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
  public partial class b_InterfaceProp
  {

   
    public void CheckInterfaceIsActive(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
    {
      Database.SqlClient.ProcessRow<b_InterfaceProp> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_InterfaceProp>(reader => { this.LoadFromDatabaseCustome(reader); return this; });
        StoredProcedure.usp_InterfaceProp_CheckInterfaceIsActive.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
    public void RetrieveInterfaceProperties(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
    {
      Database.SqlClient.ProcessRow<b_InterfaceProp> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;
          

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

             
                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_InterfaceProp>(reader => { this.LoadFromDatabase(reader); return this; });
        StoredProcedure.usp_InterfaceProp_RetrieveInterfaceProperties.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

    public int LoadFromDatabaseCustome(SqlDataReader reader)
    {
      int i = 0;
      try
      {     
        // InterfacePropId column, bigint, not null
        InterfacePropId = reader.GetInt64(i++);
        // Switch1 column, bit, not null
        Switch1 = reader.GetBoolean(i++);
      }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();
        try { reader["InterfacePropId"].ToString(); }
        catch { missing.Append("InterfacePropId "); }
        try { reader["Switch1"].ToString(); }
        catch { missing.Append("Switch1 "); }
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

        public void RetrieveInterfacePropertiesByClientIdSiteId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
   string callerUserName,
         ref b_InterfaceProp[] data
     )
        {
            Database.SqlClient.ProcessRow<b_InterfaceProp> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_InterfaceProp[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_InterfaceProp>(reader => { b_InterfaceProp obj = new b_InterfaceProp(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_InterfaceProp_RetrieveAllBySiteIDClientID_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId,SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_InterfaceProp[])results.ToArray(typeof(b_InterfaceProp));
                }
                else
                {
                    data = new b_InterfaceProp[0];
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
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
    }
}
