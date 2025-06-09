using Database.Business;
using Database.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_KBTopics_RetrieveChunkSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_KBTopics_RetrieveChunkSearch_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_KBTopics_RetrieveChunkSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_KBTopics_RetrieveChunkSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_KBTopics CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_KBTopics obj
        )
        {
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Title", obj.Title, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Category", obj.Category, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Folder", obj.Folder, 63);         
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Tags", obj.Tags, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "localization", obj.locallang, 30);



            try
            {

                List<b_KBTopics> records = new List<b_KBTopics>();
               
                reader = command.ExecuteReader();
                obj.listOfKBTopics = new List<b_KBTopics>();
                while (reader.Read())
                {
                    b_KBTopics tmpKBTopics = b_KBTopics.ProcessRetrieveForChunkV2(reader);
                    obj.listOfKBTopics.Add(tmpKBTopics);
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
            return obj;
        }
    }
}
