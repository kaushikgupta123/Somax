using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_Part_CycleCountChunkSearchForMultiStoreroom_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Part_CycleCountChunkSearchForMultiStoreroom_V2";
        public usp_Part_CycleCountChunkSearchForMultiStoreroom_V2()
        {
        }
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartId", obj.ClientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Section", obj.Section, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Area", obj.PlaceArea, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Row", obj.Row, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shelf", obj.Shelf, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Bin", obj.Bin, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "StockType", obj.StockType, 15);
            command.SetInputParameter(SqlDbType.Bit, "Critical", obj.Critical);
            command.SetInputParameter(SqlDbType.Bit, "Consignment", obj.Consignment);
            command.SetInputParameter(SqlDbType.Date, "GenerateThrough", obj.GenerateThrough);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);

            try
            {

                List<b_Part> records = new List<b_Part>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                obj.listOfPart = new List<b_Part>();
                while (reader.Read())
                {
                    b_Part tmpPart = b_Part.ProcessRetrieveForCycleCountChunkSearchForMultiStoreroom(reader);
                    obj.listOfPart.Add(tmpPart);
                }
            }
            catch (Exception ex)
            {

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
