/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2014-Jul-30  SOM-263   Roger Lawton           Created to Complete Work Orders 
**************************************************************************************************
*/

using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    public class usp_WorkOrder_RetrieveAllForSearch_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveChunkForSearch_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveAllForSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_CompleteWorkOrder stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_WorkOrder CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_WorkOrder obj
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateRange", obj.DateRange, 30);
            command.SetStringInputParameter(SqlDbType.NVarChar, "DateColumn", obj.DateColumn, 30);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.OffSetVal);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.NextRow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "workorder", obj.ClientLookupId, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "description", obj.Description, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Chargeto", obj.ChargeToClientLookupId, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Chargetoname", obj.ChargeTo_Name, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "type", obj.Type, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "status", obj.Status, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "shift", obj.Shift, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "priority", obj.Priority, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "creator", obj.Creator, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "created", obj.Created, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "assigned", obj.Assigned, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Scheduled", obj.Scheduled, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "failcode", obj.FailureCode, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ActualFinish", obj.ActualFinish, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "completioncomments", obj.CompleteComments, 500);
            //V2-271
            command.SetStringInputParameter(SqlDbType.NVarChar, "CompletedDate ", obj.CompletedDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PersonnelList ", obj.PersonnelList, 512);
            command.SetInputParameter(SqlDbType.BigInt, "LoggedInUserPEID", obj.LoggedInUserPEID);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup1ClientLookUpId", obj.AssetGroup1ClientlookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup2ClientLookUpId", obj.AssetGroup2ClientlookupId, 67);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssetGroup3ClientLookUpId", obj.AssetGroup3ClientlookupId, 67);          
            command.SetInputParameter(SqlDbType.Decimal, "ActualDuration", obj.ActualDuration);
            try
            {

                List<b_WorkOrder> records = new List<b_WorkOrder>();
                // Execute stored procedure.
                obj.utilityAdd = new UtilityAdd();
                obj.utilityAdd.list1 = new List<string>();
                reader = command.ExecuteReader();
                while (reader.Read())
                {

                    obj.utilityAdd.list1.Add(reader.GetValue(0).ToString());
                }
                reader.NextResult();
                obj.utilityAdd.list2 = new List<string>();
                while (reader.Read())
                {
                    obj.utilityAdd.list2.Add(reader.GetValue(0).ToString());
                }
                reader.NextResult();
                obj.utilityAdd.list3 = new List<string>();
                while (reader.Read())
                {
                    obj.utilityAdd.list3.Add(reader.GetValue(0).ToString());
                }
                reader.NextResult();
                obj.utilityAdd.list4 = new List<string>();
                while (reader.Read())
                {
                    obj.utilityAdd.list4.Add(reader.GetValue(0).ToString());
                }
                //Add New for failure code
                reader.NextResult();
                obj.utilityAdd.list5 = new List<string>();
                while (reader.Read())
                {
                    obj.utilityAdd.list5.Add(reader.GetValue(0).ToString());
                }
                reader.NextResult();
                obj.listOfWO = new List<b_WorkOrder>();
                while (reader.Read())
                {
                    b_WorkOrder tmpWorkOrders = b_WorkOrder.ProcessRetrieveV2(reader);
                    obj.listOfWO.Add(tmpWorkOrders);
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