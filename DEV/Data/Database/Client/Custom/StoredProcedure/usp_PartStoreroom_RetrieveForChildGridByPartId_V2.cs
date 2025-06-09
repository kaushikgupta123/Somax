using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Database.Business;
using Database.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_PartStoreroom_RetrieveForChildGridByPartId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PartStoreroom_RetrieveForChildGridByPartId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PartStoreroom_RetrieveForChildGridByPartId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PartStoreroom_RetrieveForChildGridByPartId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_PartStoreroom CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PartStoreroom obj
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
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);

            try
            {

                List<b_PartStoreroom> records = new List<b_PartStoreroom>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfPartStoreroom = new List<b_PartStoreroom>();
                while (reader.Read())
                {
                    b_PartStoreroom tmpPart = b_PartStoreroom.ProcessPartStoreroomRetrieveForRetrieveForChildGridByPartIdV2(reader);
                    obj.listOfPartStoreroom.Add(tmpPart);
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
