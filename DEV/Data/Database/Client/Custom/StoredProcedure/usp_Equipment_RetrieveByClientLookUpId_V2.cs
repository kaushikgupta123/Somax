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
    public class usp_Equipment_RetrieveByClientLookUpId_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_RetrieveByClientLookUpId_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_RetrieveByClientLookUpId_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveByClientLookUpId_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_Equipment CallStoredProcedure(
            SqlCommand command,
            b_Equipment obj
        )
        {
            SqlDataReader reader = null;

            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookUpId", obj.ClientLookupId, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            try
            {

                b_Equipment record = new b_Equipment();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    record = b_Equipment.ProcessRowForEquipmentIdByClientIdLookupV2(reader);
                    obj.EquipmentId = record.EquipmentId;
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
