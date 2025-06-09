using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Part_RetrieveBYFilterText
    {
         private static string STOREDPROCEDURE_NAME = "usp_Part_RetrieveBYFilterText";

         public usp_Part_RetrieveBYFilterText()
         {
         }

         public static List<b_Part> CallStoredProcedure(
        SqlCommand command,
        long callerUserInfoId,
        string callerUserName,
        b_Part obj
    )
         {
             List<b_Part> records = new List<b_Part>();
             SqlDataReader reader = null;
             SqlParameter RETURN_CODE_parameter = null;

             int retCode = 0;

             // Setup command.
             command.SetProcName(STOREDPROCEDURE_NAME);
             RETURN_CODE_parameter = command.GetReturnCodeParameter();
             command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
             command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
             command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
             command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
             command.SetStringInputParameter(SqlDbType.NVarChar, "FilterText", obj.FilterText, 100);
             command.SetInputParameter(SqlDbType.Int, "StartIndex", obj.FilterStartIndex);
             command.SetInputParameter(SqlDbType.Int, "EndIndex", obj.FilterEndIndex);

             try
             {
                 // Execute stored procedure.
                 reader = command.ExecuteReader();

                 // Loop through dataset.
                 while (reader.Read())
                 {
                     //// Add the record to the list.
                     records.Add(b_Part.ProcessRowForRetrieveByFilterText(reader));
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
