
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
  public partial class b_POImpHdr
  {
        public Int64 PersonnelId { get; set; }
        public int spErrCode { get; set; }
        public string PurchasingEventCreate { get; set; }
        public string PurchasingEventUpdate { get; set; }
        public Int64 PurchaseOrderId { get; set; }

        public void POImpHdr_ValidateImport(
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
        results = Database.StoredProcedure.usp_POImpHdr_ValidateImport.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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



    public void POImpHdr_ProcessImport(
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
        Database.StoredProcedure.usp_POImpHdr_ProcessImport.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


//    public void RetrievePOImpHdrbyPOIdFromDatabase(
//  SqlConnection connection,
//  SqlTransaction transaction,
//  long callerUserInfoId,
//  string callerUserName
//)
//    {
//      Database.SqlClient.ProcessRow<b_POImpHdr> processRow = null;
//      SqlCommand command = null;
//      string message = String.Empty;

//      try
//      {
//        // Create the command to use in calling the stored procedures
//        command = new SqlCommand();
//        command.Connection = connection;
//        command.Transaction = transaction;

//        // Call the stored procedure to retrieve the data
//        processRow = new Database.SqlClient.ProcessRow<b_POImpHdr>(reader => { this.LoadFromDatabase(reader); return this; });
//        StoredProcedure.usp_POImpHdr_RetrieveByPOId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

//      }
//      finally
//      {
//        if (null != command)
//        {
//          command.Dispose();
//          command = null;
//        }
//        processRow = null;
//        message = String.Empty;
//        callerUserInfoId = 0;
//        callerUserName = String.Empty;
//      }
//    }


    public void RetrievePOImpHdrbyPRIdFromDatabase(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName
  )
    {
      Database.SqlClient.ProcessRow<b_POImpHdr> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_POImpHdr>(reader => { this.LoadFromDatabase(reader); return this; });
        StoredProcedure.usp_POImpHdr_RetrieveByPRId.CallStoredProcedure(command, processRow,callerUserInfoId, callerUserName, this);

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





  //  public void RetrievePOImpHdrbySomaxPRIDFromDatabase(
  //  SqlConnection connection,
  //  SqlTransaction transaction,
  //  long callerUserInfoId,
  //  string callerUserName
  //)
  //  {
  //    Database.SqlClient.ProcessRow<b_POImpHdr> processRow = null;
  //    SqlCommand command = null;
  //    string message = String.Empty;

  //    try
  //    {
  //      // Create the command to use in calling the stored procedures
  //      command = new SqlCommand();
  //      command.Connection = connection;
  //      command.Transaction = transaction;

  //      // Call the stored procedure to retrieve the data
  //      processRow = new Database.SqlClient.ProcessRow<b_POImpHdr>(reader => { this.LoadFromDatabase(reader); return this; });
  //      StoredProcedure.usp_POImpHdr_RetrieveBySomaxPRId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

  //    }
  //    finally
  //    {
  //      if (null != command)
  //      {
  //        command.Dispose();
  //        command = null;
  //      }
  //      processRow = null;
  //      message = String.Empty;
  //      callerUserInfoId = 0;
  //      callerUserName = String.Empty;
  //    }
  //  }



    public void RetrievePOImpHdrbyEXPOIdFromDatabase(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName
)
    {
      Database.SqlClient.ProcessRow<b_POImpHdr> processRow = null;
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand();
        command.Connection = connection;
        command.Transaction = transaction;

        // Call the stored procedure to retrieve the data
        processRow = new Database.SqlClient.ProcessRow<b_POImpHdr>(reader => { this.LoadFromDatabase(reader); return this; });
        StoredProcedure.usp_POImpHdr_RetrieveByEXPOID.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


//    public void RetrieveByPKCustom(SqlConnection connection,
//SqlTransaction transaction,
//long callerUserInfoId,
//string callerUserName)
//    {
//      Database.SqlClient.ProcessRow<b_POImpHdr> processRow = null;
//      SqlCommand command = null;
//      string message = String.Empty;

//      try
//      {
//        // Create the command to use in calling the stored procedures
//        command = new SqlCommand();
//        command.Connection = connection;
//        command.Transaction = transaction;

//        // Call the stored procedure to retrieve the data
//        processRow = new Database.SqlClient.ProcessRow<b_POImpHdr>(reader => { this.LoadFromDatabase(reader); return this; });
//        StoredProcedure.usp_POImpHdr_RetrieveByPKCustom.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

//      }
//      finally
//      {
//        if (null != command)
//        {
//          command.Dispose();
//          command = null;
//        }
//        processRow = null;
//        message = String.Empty;
//        callerUserInfoId = 0;
//        callerUserName = String.Empty;
//      }
//    }

    public  void CustomInsertIntoDatabase(
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
        Database.StoredProcedure.usp_POImpHdr_CreateCustom.CallStoredProcedure(command,callerUserInfoId, callerUserName,  this);
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



    public  void UpdateInDatabaseCustom(
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
        Database.StoredProcedure.usp_POImpHdr_UpdateByPKCustom.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

  }
  }
      
