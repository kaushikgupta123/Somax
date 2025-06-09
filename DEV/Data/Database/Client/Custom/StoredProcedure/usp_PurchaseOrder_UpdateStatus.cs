/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* usp_PurchaseOrder_UpdateStatus.cs 
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2015-Feb-04 SOM-529  Roger Lawton       Added to update purchase order status
* 2015-Mar-05 SOM-594  Roger Lawton       Added the ReceiveBy_PersonnelId parameter 
*                                         Passed the b_POReceiptsHeader as an input parm
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.StoredProcedure
{
   public class usp_PurchaseOrder_UpdateStatus
    {
        private static string STOREDPROCEDURE_NAME = "usp_PurchaseOrder_UpdateStatus";

        public usp_PurchaseOrder_UpdateStatus()
        {
        }

        public static void CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PurchaseOrder PO,
            b_POReceiptHeader Receipt
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
          command.SetInputParameter(SqlDbType.BigInt, "ClientId", PO.ClientId);
          command.SetInputParameter(SqlDbType.BigInt, "PurchaseOrderId", PO.PurchaseOrderId);
          command.SetInputParameter(SqlDbType.Int, "Receive_PersonnelId", Receipt.ReceiveBy_PersonnelID);
          command.SetInputParameter(SqlDbType.Int, "UpdateIndex", PO.UpdateIndex);

          // Setup updateIndexOut parameter.
          updateIndexOut_parameter = command.Parameters.Add("@UpdateIndexOut", SqlDbType.Int);
          updateIndexOut_parameter.Direction = ParameterDirection.Output;

          // Execute stored procedure.
          command.ExecuteNonQuery();

          PO.UpdateIndex = (int)updateIndexOut_parameter.Value;

          // Process the RETURN_CODE parameter value
          retCode = (int)RETURN_CODE_parameter.Value;
          AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
        }


    }
}
