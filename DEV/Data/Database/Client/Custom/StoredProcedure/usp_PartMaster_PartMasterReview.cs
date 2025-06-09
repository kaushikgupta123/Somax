using Database.Business;
using Database.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PartMaster_PartMasterReview stored procedure.
    /// </summary>
   public class usp_PartMaster_PartMasterReview
    {
        /// <summary>
        /// Constants.
        /// </summary>
       private static string STOREDPROCEDURE_NAME = "usp_PartMaster_PartMasterReview";

        /// <summary>
        /// Default constructor.
        /// </summary>
       public usp_PartMaster_PartMasterReview()
        {
        }

       public static ArrayList CallStoredProcedure(
          SqlCommand command,
          Database.SqlClient.ProcessRow<b_PartMaster> processRow,
          string SearchCriterion,
          long Siteid,
          b_PartMaster obj
      )
       {
           ArrayList records = new ArrayList();
           SqlDataReader reader = null;
           b_PartMaster record = null;
           SqlParameter RETURN_CODE_parameter = null;
           int retCode = 0;

           // Setup command.
           command.SetProcName(STOREDPROCEDURE_NAME);
           RETURN_CODE_parameter = command.GetReturnCodeParameter();

           command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
           command.SetInputParameter(SqlDbType.BigInt, "SiteId", Siteid);
           command.SetStringInputParameter(SqlDbType.NVarChar, "SearchCriteria", SearchCriterion, 50);
          
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
