
/*
 *  Added By Indusnet Technologies
 * 
 */

using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_UserData_UserDetailsByUserId
    {
        //private static string STOREDPROCEDURE_NAME = "usp_UserData_UserDetailsByUserId";
        private static string STOREDPROCEDURE_NAME = "usp_UserData_UserDetailsByUserId_V2";

        public usp_UserData_UserDetailsByUserId()
        {
        }
         
       public static ArrayList CallStoredProcedure(
           SqlCommand command,
           ProcessRow<b_UserDetails> processRow,
           long callerUserInfoId,
           string callerUserName,
           b_UserDetails obj
       )
       {
           ArrayList records = new ArrayList();
           SqlDataReader reader = null;
           b_UserDetails record = null;
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
           command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
           command.SetInputParameter(SqlDbType.BigInt, "UserInfoId", obj.UserInfoId);         
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
