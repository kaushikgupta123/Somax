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
    public class usp_Equipment_UpdateByPKForeignKeys_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_Equipment_UpdateByPKForeignKeys_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_Equipment_UpdateByPKForeignKeys_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_UpdateByPKForeignKeys stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_Equipment obj
        )
        {
            SqlParameter RETURN_CODE_parameter = null;
            SqlParameter updateIndexOut_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.EquipmentId);
            command.SetInputParameter(SqlDbType.BigInt, "SiteId", obj.SiteId);
            command.SetInputParameter(SqlDbType.BigInt, "AreaId", obj.AreaId);
            command.SetInputParameter(SqlDbType.BigInt, "DepartmentId", obj.DepartmentId);
            command.SetInputParameter(SqlDbType.BigInt, "StoreRoomId", obj.StoreroomId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ClientLookupId", obj.ClientLookupId, 31);
            command.SetInputParameter(SqlDbType.Decimal, "AcquiredCost", obj.AcquiredCost);
            command.SetInputParameter(SqlDbType.DateTime2, "AcquiredDate", obj.AcquiredDate);
            command.SetInputParameter(SqlDbType.UniqueIdentifier, "BIMIdentifier", obj.BIMIdentifier);
            command.SetInputParameter(SqlDbType.Decimal, "BookValue", obj.BookValue);
            command.SetStringInputParameter(SqlDbType.NVarChar, "BusinessGroup", obj.BusinessGroup, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CatalogNumber", obj.CatalogNumber, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Category", obj.Category, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CostCenter", obj.CostCenter, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DeprCode", obj.DeprCode, 15);
            command.SetInputParameter(SqlDbType.Decimal, "DeprLifeToDate", obj.DeprLifeToDate);
            command.SetInputParameter(SqlDbType.Decimal, "DeprPercent", obj.DeprPercent);
            command.SetInputParameter(SqlDbType.Decimal, "DeprYearToDate", obj.DeprYearToDate);
            command.SetInputParameter(SqlDbType.BigInt, "ElectricalParent", obj.ElectricalParent);
            command.SetInputParameter(SqlDbType.Bit, "InactiveFlag", obj.InactiveFlag);
            command.SetInputParameter(SqlDbType.Bit, "CriticalFlag", obj.CriticalFlag);
            command.SetInputParameter(SqlDbType.DateTime2, "InstallDate", obj.InstallDate);
            command.SetInputParameter(SqlDbType.BigInt, "Labor_AccountId", obj.Labor_AccountId);
            command.SetInputParameter(SqlDbType.Int, "LifeinMonths", obj.LifeinMonths);
            command.SetInputParameter(SqlDbType.Int, "LifeinYears", obj.LifeinYears);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Location", obj.Location, 63);
            command.SetInputParameter(SqlDbType.BigInt, "LocationId", obj.LocationId);
            command.SetInputParameter(SqlDbType.BigInt, "Maint_VendorId", obj.Maint_VendorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Maint_WarrantyDesc", obj.Maint_WarrantyDesc, 127);
            command.SetInputParameter(SqlDbType.DateTime2, "Maint_WarrantyExpire", obj.Maint_WarrantyExpire);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Make", obj.Make, 31);
            command.SetInputParameter(SqlDbType.BigInt, "Material_AccountId", obj.Material_AccountId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Model", obj.Model, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Name", obj.Name, 63);
            command.SetInputParameter(SqlDbType.Bit, "NoCostRollUp", obj.NoCostRollUp);
            command.SetInputParameter(SqlDbType.Bit, "NoPartXRef", obj.NoPartXRef);
            command.SetInputParameter(SqlDbType.Decimal, "OriginalValue", obj.OriginalValue);
            command.SetInputParameter(SqlDbType.DateTime2, "OutofService", obj.OutofService);
            command.SetInputParameter(SqlDbType.BigInt, "ParentId", obj.ParentId);
            command.SetInputParameter(SqlDbType.BigInt, "PartId", obj.PartId);
            command.SetInputParameter(SqlDbType.BigInt, "Purch_VendorId", obj.Purch_VendorId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Purch_WarrantyDesc", obj.Purch_WarrantyDesc, 127);
            command.SetInputParameter(SqlDbType.DateTime2, "Purch_WarrantyExpire", obj.Purch_WarrantyExpire);
            command.SetInputParameter(SqlDbType.Int, "RIMEClass", obj.RIMEClass);
            command.SetInputParameter(SqlDbType.Decimal, "SalvageValue", obj.SalvageValue);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SerialNumber", obj.SerialNumber, 63);
            command.SetInputParameter(SqlDbType.Int, "Size", obj.Size);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SizeUnits", obj.SizeUnits, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Type", obj.Type, 15);
            command.SetInputParameter(SqlDbType.Int, "UpdateIndex", obj.UpdateIndex);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ParentID_ClientLookupId", obj.ParentIdClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ElectricalParent_ClientLookupId", obj.ElectricalParentClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Maint_VendorID_ClientLookupId", obj.MaintVendorIdClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Purch_VendorID_ClientLookupId", obj.PurchVendorIdClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PartID_ClientLookupId", obj.PartIdClientLookupId, 70);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LocationID_ClientLookupId", obj.LocationIdClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MaterialAccountClientLookupId", obj.MaterialAccountClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LaborAccountClientLookupId", obj.LaborAccountClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetNumber", obj.AssetNumber, 31);
            command.SetInputParameter(SqlDbType.BigInt, "LineId", obj.LineId);
            command.SetInputParameter(SqlDbType.BigInt, "SystemInfoId", obj.SystemInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetCategory", obj.AssetCategory,15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SubType", obj.SubType,15);
            command.SetInputParameter(SqlDbType.BigInt, "AssetGroup1", obj.AssetGroup1);
            command.SetInputParameter(SqlDbType.BigInt, "AssetGroup2", obj.AssetGroup2);
            command.SetInputParameter(SqlDbType.BigInt, "AssetGroup3", obj.AssetGroup3);
            #region V2-1133
            command.SetInputParameter(SqlDbType.Bit, "RemoveFromService", obj.RemoveFromService);
            command.SetInputParameter(SqlDbType.DateTime2, "RemoveFromServiceDate", obj.RemoveFromServiceDate);
            command.SetInputParameter(SqlDbType.DateTime2, "ExpectedReturnToService", obj.ExpectedReturnToService);
            command.SetStringInputParameter(SqlDbType.NVarChar, "RemoveFromServiceReason", obj.RemoveFromServiceReason, 1073741823);           
            command.SetStringInputParameter(SqlDbType.NVarChar, "RemoveFromServiceReasonCode", obj.RemoveFromServiceReasonCode,15);
            #endregion
            // Setup updateIndexOut parameter.
            updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
            updateIndexOut_parameter.Direction = ParameterDirection.Output;

            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.UpdateIndex = (int)updateIndexOut_parameter.Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }
    }
}
