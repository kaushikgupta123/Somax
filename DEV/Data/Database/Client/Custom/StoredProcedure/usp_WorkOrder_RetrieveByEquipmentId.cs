/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2014-Sep-29 SOM-346  Roger Lawton        Changed to use ProcessRowForEquipmentList 
*                                          Added chargetype and Active parameters
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
    /// <summary>
    /// Access the usp_WorkOrder_RetrieveByEquipmentId stored procedure.
    /// </summary>
    public class usp_WorkOrder_RetrieveByEquipmentId
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WorkOrder_RetrieveByEquipmentId";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WorkOrder_RetrieveByEquipmentId()
        {
        }

        /// <summary>
        /// Static method to call the usp_WorkOrder_RetrieveByEquipmentId stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_WorkOrder> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            bool retrieveactive,
            b_WorkOrder obj
        )
        {
            List<b_WorkOrder> records = new List<b_WorkOrder>();
            SqlDataReader reader = null;
            SqlParameter RETURN_CODE_parameter = null;
            int retCode = 0;
            // Setup command.
            command.SetProcName(STOREDPROCEDURE_NAME);
            RETURN_CODE_parameter = command.GetReturnCodeParameter();
            command.SetInputParameter(SqlDbType.BigInt, "CallerUserInfoId", callerUserInfoId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "CallerUserName", callerUserName, 256);
            command.SetInputParameter(SqlDbType.BigInt, "ClientId", obj.ClientId);
            command.SetInputParameter(SqlDbType.BigInt, "EquipmentId", obj.ChargeToId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeType", obj.ChargeType, 15);
            command.SetInputParameter(SqlDbType.Bit, "Active", retrieveactive);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                  // Add the record to the list.
                  // SOM-346
                  records.Add((b_WorkOrder)b_WorkOrder.ProcessRowForList(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    //records.Add(reader.GetString(0));
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