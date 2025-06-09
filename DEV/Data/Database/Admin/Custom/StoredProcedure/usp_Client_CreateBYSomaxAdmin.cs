/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID Person            Description
* =========== ======= ================= ============================================================
* 2015-Aug-24 SOM-785 Roger Lawton      Change size of WOPrintMessage parameter (255==>512)
****************************************************************************************************
*/

using System.Data;
using System.Data.SqlClient;

using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    public class usp_Client_CreateBYSomaxAdmin
    {
        private static string STOREDPROCEDURE_NAME = "usp_Client_CreateBYSomaxAdmin";

        public usp_Client_CreateBYSomaxAdmin()
        {
        }

        public static void CallStoredProcedure(
           SqlCommand command,
           long callerUserInfoId,
           string callerUserName,
           b_Client obj
       )
        {
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;

            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetOutputParameter(SqlDbType.BigInt, "CreatedClientId");

            command.SetInputParameter(SqlDbType.BigInt, "ClientId",obj.ClientId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompanyName", obj.CompanyName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "LegalName", obj.LegalName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrimaryContact", obj.PrimaryContact, 63);
            command.SetInputParameter(SqlDbType.Int, "NumberOfEmployees", obj.NumberOfEmployees);
            command.SetInputParameter(SqlDbType.BigInt, "AnnualSales", obj.AnnualSales);
            command.SetStringInputParameter(SqlDbType.NVarChar, "TaxIDNumber", obj.TaxIDNumber, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "VATNumber", obj.VATNumber, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Email", obj.Email, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Website", obj.Website, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "BusinessType", obj.BusinessType, 15);
            command.SetInputParameter(SqlDbType.DateTime2, "DateEstablished", obj.DateEstablished);
            command.SetInputParameter(SqlDbType.Int, "NumberOfLocations", obj.NumberOfLocations);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OfficerName", obj.OfficerName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OfficerTitle", obj.OfficerTitle, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "OfficerPhone", obj.OfficerPhone, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DunnsNumber", obj.DunnsNumber, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PackageLevel", obj.PackageLevel, 15);
            command.SetInputParameter(SqlDbType.Int, "AppUsers", obj.AppUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxAppUsers", obj.MaxAppUsers);
            command.SetInputParameter(SqlDbType.Int, "PhoneUsers", obj.PhoneUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxPhoneUsers", obj.MaxPhoneUsers);
            command.SetInputParameter(SqlDbType.Int, "WorkRequestUsers", obj.WorkRequestUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxWorkRequestUsers", obj.MaxWorkRequestUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxLimitedUsers", obj.MaxLimitedUsers);
            command.SetInputParameter(SqlDbType.Bit, "SiteControl", obj.SiteControl);
            command.SetInputParameter(SqlDbType.Bit, "Purchasing", obj.Purchasing);
            command.SetInputParameter(SqlDbType.Bit, "Sanitation", obj.Sanitation);
            command.SetInputParameter(SqlDbType.Int, "SanitationUsers", obj.SanitationUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxSanitationUsers", obj.MaxSanitationUsers);
            command.SetInputParameter(SqlDbType.Int, "SuperUsers", obj.SuperUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxSuperUsers", obj.MaxSuperUsers);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PrimarySICCode", obj.PrimarySICCode, 7);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NAICSCode", obj.NAICSCode, 7);
            command.SetInputParameter(SqlDbType.Int, "Sites", obj.Sites);
            command.SetInputParameter(SqlDbType.Int, "MaxSites", obj.MaxSites);
            command.SetStringInputParameter(SqlDbType.NVarChar, "MinorityStatus", obj.MinorityStatus, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Localization", obj.Localization, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DefaultTimeZone", obj.DefaultTimeZone, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DefaultCustomerManager", obj.DefaultCustomerManager, 63);
            command.SetInputParameter(SqlDbType.Int, "MaxAttempts", obj.MaxAttempts);
            command.SetInputParameter(SqlDbType.Int, "MaxTimeOut", obj.MaxTimeOut);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ConnectionString", obj.ConnectionString, 511);
            command.SetInputParameter(SqlDbType.Int, "TabletUsers", obj.TabletUsers);
            command.SetInputParameter(SqlDbType.Int, "MaxTabletUsers", obj.MaxTabletUsers);
            command.SetStringInputParameter(SqlDbType.NVarChar, "UIConfiguration", obj.UIConfiguration, 255);
            command.SetStringInputParameter(SqlDbType.NVarChar, "WOPrintMessage", obj.WOPrintMessage, 511);       // SOM-785
            command.SetStringInputParameter(SqlDbType.NVarChar, "PurchaseTermsandConds", obj.PurchaseTermsandConds, 511);
            // Execute stored procedure.
            command.ExecuteNonQuery();

            obj.CreatedClientId = (long)command.Parameters["@CreatedClientId"].Value;

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }


    }

}
