
/*
 *  Added By Indusnet Technologies
 */

using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Database.Business;


namespace Database.StoredProcedure
{
   public class usp_Personnel_RetrieveByClientId
    {
       private static string STOREDPROCEDURE_NAME = "usp_Personnel_RetrieveByClientId";

       public usp_Personnel_RetrieveByClientId()
        {

        }

       public static List<b_Personnel> CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           long clientId
         
       )
       {
           List<b_Personnel> records = new List<b_Personnel>();
           SqlDataReader reader = null;
           b_Personnel record = null;
           SqlParameter RETURN_CODE_parameter = null;

           int retCode = 0;    

           // Setup command.
           command.SetProcName(STOREDPROCEDURE_NAME);
           RETURN_CODE_parameter = command.GetReturnCodeParameter();
           command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
           command.SetInputParameter(SqlDbType.BigInt, "ClientId", clientId);
           try
           {
               // Execute stored procedure.
               reader = command.ExecuteReader();            
           
               // Loop through dataset.
               while (reader.Read())
               {
                   // Process the current row into a record
                   record = (b_Personnel)b_Personnel.ProcessRowExtendedForRetrieveByClientId(reader);

                   //// Add the record to the list.
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
