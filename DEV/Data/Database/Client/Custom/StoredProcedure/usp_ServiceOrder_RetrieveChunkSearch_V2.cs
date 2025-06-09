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
    public class usp_ServiceOrder_RetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_ServiceOrder_RetrieveChunkSearch_V2";

        public usp_ServiceOrder_RetrieveChunkSearch_V2()
        {

        }

        public static List<b_ServiceOrder> CallStoredProcedure(
       SqlCommand command,
       long callerUserInfoId,
       string callerUserName,
       b_ServiceOrder obj
       )
        {
            List<b_ServiceOrder> records = new List<b_ServiceOrder>();
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "offset1", obj.offset1, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "nextrow", obj.nextrow, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "EquipmentClientLookupId", obj.EquipmentClientLookupId, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetName", obj.AssetName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description,500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Shift", obj.Shift, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VIN", obj.VIN, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PersonnelList", obj.PersonnelList, 512);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateStartDateVw", obj.CreateStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CreateEndDateVw", obj.CreateEndDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteStartDateVw", obj.CompleteStartDateVw, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteEndDateVw", obj.CompleteEndDateVw, 500);
            //command.SetStringInputParameter(SqlDbType.NVarChar, "personnelList", obj.PersonnelList, 512);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText", obj.SearchText, 30);
           
            try
            {
                //obj.utilityAdd = new UtilityAdd();
                //obj.utilityAdd.list1 = new List<string>();
                // Execute stored procedure.
                reader = command.ExecuteReader();
                //while (reader.Read())
                //{

                //    obj.utilityAdd.list1.Add(reader.GetValue(0).ToString());
                //}
                //reader.NextResult();
                //obj.utilityAdd.list2 = new List<string>();
                //while (reader.Read())
                //{
                //    obj.utilityAdd.list2.Add(reader.GetValue(0).ToString());
                //}
                //reader.NextResult();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_ServiceOrder tmpServiceOrder = b_ServiceOrder.ProcessRowForServiceOrderRetriveAllForSearch(reader);
                    tmpServiceOrder.ClientId = obj.ClientId;
                    records.Add(tmpServiceOrder);
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
