using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Database.SqlClient;

using Database.Business;
namespace Database.StoredProcedure
{
   public class usp_Site_RetrieveBySearchFromAdmin
    {

       private static string STOREDPROCEDURE_NAME = "usp_Site_RetrieveBySearchFromAdmin";

       public usp_Site_RetrieveBySearchFromAdmin()
        {
        }
           

       public static List<b_Site> CallStoredProcedure(
         SqlCommand command,
         long callerUserInfoId,
         string callerUserName,
         b_Site obj
     )
       {
           List<b_Site> records = new List<b_Site>();
           SqlDataReader reader = null;
           b_Site record = null;
           SqlParameter RETURN_CODE_parameter = null;
           //SqlParameter        clientId_parameter = null;
           int retCode = 0;

           // Setup command.
           command.SetProcName(STOREDPROCEDURE_NAME);
           RETURN_CODE_parameter = command.GetReturnCodeParameter();
           command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);

           // Setup clientId parameter.
           command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
           command.SetStringInputParameter(SqlDbType.NVarChar, "SiteName", obj.Name, 63);
           command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 255);


           try
           {
               // Execute stored procedure.
               reader = command.ExecuteReader();

               // Loop through dataset.
               while (reader.Read())
               {
                   // Process the current row into a record
                   record = b_Site.ProcessRowForSiteRetriveFromAdmin(reader);

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
