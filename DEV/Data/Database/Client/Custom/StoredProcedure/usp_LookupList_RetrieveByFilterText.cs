using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_LookupList_RetrieveByFilterText
    {
       private static string STOREDPROCEDURE_NAME = "usp_LookupList_RetrieveByFilterText";

       public usp_LookupList_RetrieveByFilterText()
        {
        }

       public static List<KeyValuePair<string,string>> CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         long clientId,
         string FilterText,
         int startIndex,
         int EndIndex,
         string FilterColumn,
         string ListName
     )
       {
           List<KeyValuePair<string, string>> records = new List<KeyValuePair<string, string>>();
           SqlDataReader reader = null;          
           SqlParameter RETURN_CODE_parameter = null;

           int retCode = 0;         

           // Setup command.
           command.SetProcName(STOREDPROCEDURE_NAME);
           RETURN_CODE_parameter = command.GetReturnCodeParameter();
           command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
           command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "FilterText", FilterText, 100);
           command.SetInputParameter(SqlDbType.Int, "StartIndex", startIndex);
           command.SetInputParameter(SqlDbType.Int, "EndIndex", EndIndex);
           if (!string.IsNullOrEmpty(FilterColumn))
           {
               command.SetStringInputParameter(SqlDbType.NVarChar, "FilterColumn", FilterColumn, 100);
           }
           if (!string.IsNullOrEmpty(ListName))
           {
               command.SetStringInputParameter(SqlDbType.NVarChar, "ListName", ListName, 100);
           }

           try
           {
               // Execute stored procedure.
               reader = command.ExecuteReader();            

               // Loop through dataset.
               while (reader.Read())
               {                
                   //// Add the record to the list.
                   records.Add( b_LookupList.ProcessRowForFilterLookUpList(reader));
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
