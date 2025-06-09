using Database.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.SqlClient;

namespace Database.Admin.Custom.StoredProcedure
{
    public class usp_LoginAuditing_RetrieveLoginRecordsCountByCreateDate_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_LoginAuditing_RetrieveLoginRecordsCountByCreateDate_V2";

        public usp_LoginAuditing_RetrieveLoginRecordsCountByCreateDate_V2()
        {

        }


        public static List<b_LoginAuditing> CallStoredProcedure(
          SqlCommand command,
          long callerUserInfoId,
          string callerUserName,
         b_LoginAuditing obj
      )
        {
            List<b_LoginAuditing> records = new List<b_LoginAuditing>();
            SqlDataReader reader = null;
            b_LoginAuditing record = null;
            SqlParameter RETURN_CODE_parameter = null;

            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CaseNo);
            command.SetInputParameter(SqlDbType.Int, "IsEnterprise", obj.IsEnterprise);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Process the current row into a record
                    record = (b_LoginAuditing)b_LoginAuditing.ProcessRowLoginRecordsCountByCreateDate(reader);

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
