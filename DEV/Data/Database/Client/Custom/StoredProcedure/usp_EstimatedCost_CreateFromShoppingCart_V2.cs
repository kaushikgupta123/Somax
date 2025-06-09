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
    public class usp_EstimatedCost_CreateFromShoppingCart_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_EstimatedCost_CreateFromShoppingCart_V2";
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_EstimatedCosts obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetOutputParameter(SqlDbType.BigInt, "EstimatedCostsId");
            command.SetInputParameter(SqlDbType.BigInt, "ObjectId", obj.ObjectId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ObjectType", obj.ObjectType, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Category", obj.Category, 15);
            command.SetInputParameter(SqlDbType.BigInt, "CategoryId", obj.CategoryId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 127);
            command.SetInputParameter(SqlDbType.Decimal, "Duration", obj.Duration);
            command.SetInputParameter(SqlDbType.Decimal, "Quantity", obj.Quantity);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetInputParameter(SqlDbType.BigInt, "VendorId", obj.VendorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Source", obj.Source, 15);
            command.SetInputParameter(SqlDbType.BigInt, "UNSPSC", obj.UNSPSC);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitOfMeasure", obj.UnitOfMeasure, 15);
            command.SetInputParameter(SqlDbType.BigInt, "UpdateIndex", obj.UpdateIndex); 
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);

            // Execute stored procedure.
            command.ExecuteNonQuery();
            obj.EstimatedCostsId = (long)command.Parameters["@EstimatedCostsId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
