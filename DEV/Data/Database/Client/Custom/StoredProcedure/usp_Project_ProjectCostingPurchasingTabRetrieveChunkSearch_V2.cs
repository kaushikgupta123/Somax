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
    class usp_Project_ProjectCostingPurchasingTabRetrieveChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_Project_ProjectCostingPurchasingTabRetrieveChunkSearch_V2";

        public usp_Project_ProjectCostingPurchasingTabRetrieveChunkSearch_V2()
        {

        }

        public static List<b_Project> CallStoredProcedure(
    SqlCommand command,
    long callerUserInfoId,
    string callerUserName,
    b_Project obj
    )
        {
            List<b_Project> records = new List<b_Project>();
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
            command.SetInputParameter(SqlDbType.Int, "Page", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "ResultsPerPage", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderColumn", obj.OrderbyColumn, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OrderDirection", obj.OrderBy, 30);
            command.SetInputParameter(SqlDbType.Int, "ProjectId", obj.ProjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.PurchasOrderClientLookupId, 15);
            command.SetInputParameter(SqlDbType.Int, "Line", obj.Line);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartID", obj.PartID, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 500);
            command.SetInputParameter(SqlDbType.Decimal, "Quantity", obj.Quantity);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetInputParameter(SqlDbType.Decimal, "TotalCost", obj.TotalCost);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Buyer", obj.Buyer, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompleteDate", obj.PC_PO_CompleteDate, 15);
            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_Project tmpServiceOrder = b_Project.ProcessRowForProject_ProjectCostingPurchasingTabLookupListBySearchCriteria(reader);
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
