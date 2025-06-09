using Database.Business;
using Database.SqlClient;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_Attachment_RetrieveURLCount_ByObjectAndFileName_V2.
    /// </summary>
    public class usp_Attachment_RetrieveURLCount_ByObjectAndFileName_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Attachment_RetrieveURLCount_ByObjectAndFileName_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Attachment_RetrieveURLCount_ByObjectAndFileName_V2()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="processRow"></param>
        /// <param name="callerUserInfoId"></param>
        /// <param name="callerUserName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int CallStoredProcedure(
            SqlCommand command,
            Database.SqlClient.ProcessRow<b_Attachment> processRow,
            long callerUserInfoId,
            string callerUserName,
            b_Attachment obj
        )
        {
            ArrayList records = new ArrayList();
            SqlDataReader reader = null;
            b_Attachment record = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            int count = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectName", obj.ObjectName, 31);
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "FileName", obj.FileName, 255);
            try
            {
                // Execute stored procedure.
                count =(int)command.ExecuteScalar();

                // Loop through dataset.

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
            return count;
        }
    }
}
