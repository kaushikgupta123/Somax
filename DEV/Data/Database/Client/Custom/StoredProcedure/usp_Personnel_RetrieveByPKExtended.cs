using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;
using Database.Business;


namespace Database.StoredProcedure
{
  class usp_Personnel_RetrieveByPKExtended
  {
    /// <summary>
    /// Constants.
    /// </summary>
    private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveByPKExtended";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public usp_Personnel_RetrieveByPKExtended()
    {
    }

    /// <summary>
    /// Static method to call the usp_Personnel_RetrieveByPKExtended stored procedure using SqlClient.
    /// </summary>
    /// <param name="command">SqlCommand object to use to call the stored procedure</param>
    /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
    /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
    /// <returns>ArrayList containing the results of the query</returns>
    public static void CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Personnel personnel
    )
    {
      SqlDataReader reader = null;
      SqlParameter RETURN_CODE_parameter = null;
      int retCode = 0;

      // Setup command.
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = STOREDPROCEDURE_NAME;
      command.Parameters.Clear();

      // Setup RETURN_CODE parameter.
      command.SetProcName(STOREDPROCEDURE_NAME);
      RETURN_CODE_parameter = command.GetReturnCodeParameter();
      command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
      command.SetInputParameter(SqlDbType.BigInt, "ClientId", personnel.ClientId);
      command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", personnel.PersonnelId);
      command.SetStringInputParameter(SqlDbType.NVarChar, "TableName", personnel.TableName, 64);

      try
      {

        // Execute stored procedure.
        reader = command.ExecuteReader();

        // Loop through dataset.
        while (reader.Read())
        {
          // Add the record to the list.
          personnel.LoadFromDatabaseExtended(reader);
        }

        reader.NextResult();

        // Loop through dataset.
        while (reader.Read())
        {
          // Add the record to the list.
          // Entended includes the modified date
          personnel.Notes.Add((b_Notes)b_Notes.ProcessRowExtended(reader));
        }

        reader.NextResult();

        // Loop through dataset.
        while (reader.Read())
        {
          // Add the record to the list.
         // personnel.FileInfo.Add((b_FileInfo)b_FileInfo.ProcessRowExtended(reader));
        }

        reader.NextResult();

        // Loop through dataset.
        while (reader.Read())
        {
          // Add the record to the list.
          personnel.Contacts.Add((b_Contact)b_Contact.ProcessRow(reader));
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
    }
  }
}

