using System;
using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_PurchaseOrderLineItem_CreateWithReplication_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrderLineItem_CreateWithReplication_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PurchaseOrderLineItem_CreateWithReplication_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PurchaseOrderLineItem_CreateWithReplication stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrderLineItem obj
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
            command.SetOutputParameter(SqlDbType.BigInt, "PurchaseOrderLineItemId");
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderId", obj.PurchaseOrderId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreroomId", obj.StoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "AccountId", obj.AccountId);
            command.SetInputParameter(SqlDbType.BigInt, "ChargeToId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetInputParameter(SqlDbType.BigInt, "CompleteBy_PersonnelId", obj.CompleteBy_PersonnelId);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetInputParameter(SqlDbType.BigInt, "Creator_PersonnelId", obj.Creator_PersonnelId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 1073741823);
            command.SetInputParameter(SqlDbType.DateTime2, "EstimatedDelivery", obj.EstimatedDelivery);
            command.SetInputParameter(SqlDbType.Int, "LineNumber", obj.LineNumber);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "PartStoreroomId", obj.PartStoreroomId);
            command.SetInputParameter(SqlDbType.BigInt, "PRCreator_PersonnelId", obj.PRCreator_PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "PurchaseRequestId", obj.PurchaseRequestId);
            command.SetInputParameter(SqlDbType.Decimal, "OrderQuantity", obj.OrderQuantity);
            command.SetInputParameter(SqlDbType.Decimal, "OrderQuantityOriginal", obj.OrderQuantityOriginal);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetInputParameter(SqlDbType.Bit, "Taxable", obj.Taxable);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UnitOfMeasure", obj.UnitOfMeasure, 15);
            command.SetInputParameter(SqlDbType.Decimal, "UnitCost", obj.UnitCost);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SupplierPartId", obj.SupplierPartId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SupplierPartAuxiliaryId", obj.SupplierPartAuxiliaryId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ManufacturerPartId", obj.ManufacturerPartId, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Manufacturer", obj.Manufacturer, 31);
            command.SetInputParameter(SqlDbType.Decimal, "PurchaseQuantity", obj.PurchaseQuantity);
            command.SetInputParameter(SqlDbType.Decimal, "UOMConversion", obj.UOMConversion);
            command.SetInputParameter(SqlDbType.BigInt, "VendorCatalogItemId", obj.VendorCatalogItemId);
            command.SetInputParameter(SqlDbType.Decimal, "PurchaseCost", obj.PurchaseCost);
            command.SetInputParameter(SqlDbType.BigInt, "UNSPSC", obj.UNSPSC);
            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.PurchaseOrderLineItemId = (long)command.Parameters["@PurchaseOrderLineItemId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
