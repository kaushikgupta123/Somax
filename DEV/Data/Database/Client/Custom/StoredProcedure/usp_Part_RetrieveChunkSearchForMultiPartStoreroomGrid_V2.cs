using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Database.StoredProcedure
{
    public class usp_Part_RetrieveChunkSearchForMultiPartStoreroomGrid_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Part_RetrieveChunkSearchForMultiPartStoreroomGrid_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Part_RetrieveChunkSearchForMultiPartStoreroomGrid_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Part_RetrieveChunkSearchForMultiPartStoreroomGrid_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_Part CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Part obj
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
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartId", obj.ClientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Stock", obj.StockType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Storerooms ", obj.Storerooms, 300);

            try
            {

                List<b_Part> records = new List<b_Part>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfPart = new List<b_Part>();
                while (reader.Read())
                {
                    b_Part tmpPart = b_Part.ProcessRetrieveForChunkMultipartStoreroomV2(reader);
                    obj.listOfPart.Add(tmpPart);
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
