using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PartMasterRequest_RetrieveDataByFiltering
    {
        private static string STOREDPROCEDURE_NAME = "usp_PartMasterRequest_RetrieveChunkSearch_V2";

        public usp_PartMasterRequest_RetrieveDataByFiltering()
        {
        }

        public static List<b_PartMasterRequest> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_PartMasterRequest obj
       )
        {
            List<b_PartMasterRequest> records = new List<b_PartMasterRequest>();
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
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.CreatedBy_PersonnelId);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn,100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy,10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow);
            command.SetInputParameter(SqlDbType.BigInt, "PartMasterRequestId", obj.PartMasterRequestId);
            command.SetInputParameter(SqlDbType.BigInt, "CreatedBy_PersonnelId", obj.CreateById);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Justification", obj.Justification, 1000);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RequestType", obj.RequestType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerId", obj.ManufacturerId, 63);
            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_PartMasterRequest tmpPartMasterRequest = b_PartMasterRequest.ProcessRowForRetriveAllForSearch(reader);
                    tmpPartMasterRequest.ClientId = obj.ClientId;
                    records.Add(tmpPartMasterRequest);
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
