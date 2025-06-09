using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
namespace Database.Business
{
  public partial class b_MeterReading : DataBusinessBase

  {
    public string ReadByClientLookupId { get; set; }
    public long SiteId { get; set; }
    public string meter_clientlookupid { get; set; }
    public string DateRead
    {
      get
      {
                if (this.ReadingDate != null && this.ReadingDate != default(DateTime))
                {
                    return this.ReadingDate.Value.ToString("MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);//String.Format("{0:d}", this.ReadingDate);
                }
                else
                {
                    return string.Empty;
                }
              
      }
    }

    public static object ProcessRowForMeterCrossReference(SqlDataReader reader)
    {
      // Create instance of object
      b_MeterReading obj = new b_MeterReading();

      // Load the object from the database
      obj.LoadFromDatabaseForMeterCrossReference(reader);

      // Return result
      return (object)obj;
    }

    public void LoadFromDatabaseForMeterCrossReference(SqlDataReader reader)
    {
      int i = 0;
      try
      {

        ClientId = reader.GetInt64(i++);
        MeterReadingId = reader.GetInt64(i++);
        MeterId = reader.GetInt64(i++);
        Reading = reader.GetDecimal(i++);
        ReadingBy_PersonnelId = reader.GetInt64(i++);
        ReadingDate = reader.GetDateTime(i++);
        Reset = reader.GetBoolean(i++);
        ReadByClientLookupId = reader.GetString(i++);

     }
      catch (Exception ex)
      {
        // Diagnostics
        StringBuilder missing = new StringBuilder();

        try { reader["ReadBy"].ToString(); }
        catch { missing.Append("ReadBy "); }

        StringBuilder msg = new StringBuilder();
        msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
        if (missing.Length > 0)
        {
          msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
        }

        throw new Exception(msg.ToString(), ex);
      }
    }

    public void RetrieveByMeterIdFromDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_MeterReading> data
   )
    {

      SqlCommand command = null;
      string message = String.Empty;
      List<b_MeterReading> results = null;
      data = new List<b_MeterReading>();

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        results = Database.StoredProcedure.usp_MeterReading_RetrieveByMeterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        if (results != null)
        {
          data = results;
        }
        else
        {
          data = new List<b_MeterReading>();
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
    /// <summary>
    /// Create Readings in the MeterReading Tablde
    /// </summary>
    /// <param name="connection">SqlConnection containing the database connection</param>
    /// <param name="transaction">SqlTransaction containing the database transaction</param>
    /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
    /// <param name="callerUserName">string that identifies the user calling the database</param>
    public void CreateReading(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
    string callerUserName
    )
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_MeterReading_CreateReading.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }


    public void ResetDataIntoDataBase(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
    string callerUserName
    )
    {
      SqlCommand command = null;

      try
      {
        command = connection.CreateCommand();
        if (null != transaction)
        {
          command.Transaction = transaction;
        }
        Database.StoredProcedure.usp_ResetMeter_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }
      }
    }
    public void ValidateMeterReadingProcessFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_StoredProcValidationError> data
        )
    {

      SqlCommand command = null;
      string message = String.Empty;
      List<b_StoredProcValidationError> results = null;
      data = new List<b_StoredProcValidationError>();

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        results = Database.StoredProcedure.usp_MeterReading_ValidateProces.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        if (results != null)
        {
          data = results;
        }
        else
        {
          data = new List<b_StoredProcValidationError>();
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
    public void ValidateResetMeterProcessFromDatabase(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_StoredProcValidationError> data
)
    {

      SqlCommand command = null;
      string message = String.Empty;
      List<b_StoredProcValidationError> results = null;
      data = new List<b_StoredProcValidationError>();

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        results = Database.StoredProcedure.usp_ResetMeter_ValidateProcess.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        if (results != null)
        {
          data = results;
        }
        else
        {
          data = new List<b_StoredProcValidationError>();
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
