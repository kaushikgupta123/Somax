using Database.Business;
using Database.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
  class usp_InterfaceProp_RetrieveInterfaceProperties
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_InterfaceProp_RetrieveInterfaceProperties";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_InterfaceProp_RetrieveInterfaceProperties()
    {
    }

    /// <summary>
    /// Static method to call the usp_InterfaceProp_RetrieveInterfaceProperties procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
    /// <returns>ArrayList containing the results of the query</returns>
    public static ArrayList CallStoredProcedure(
        SqlCommand command,
        Database.SqlClient.ProcessRow<b_InterfaceProp> processRow,
        long callerUserInfoId,
        string callerUserName,
        b_InterfaceProp obj
    )
    {
      ArrayList records = new ArrayList();
      SqlDataReader reader = null;
      b_InterfaceProp record = null;
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "InterfaceType",obj.InterfaceType,31);


      try
      {
        // Execute stored procedure.
        reader = command.ExecuteReader();

        // Loop through dataset.
        while (reader.Read())
        {
          // Process the current row into a record
          record = processRow(reader);

          // Add the record to the list.
          records.Add(record);
        }
      }
      finally
      {
        if (null != reader)
        {
          if (false == reader.IsClosed)
          {
            reader.Close();
          }
          reader = null;
        }
      }

      // Process the RETURN_CODE parameter value
      retCode = (int)RETURN_CODE_parameter.Value;
      AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);

      // Return the result
      return records;
    }
  
}
}
