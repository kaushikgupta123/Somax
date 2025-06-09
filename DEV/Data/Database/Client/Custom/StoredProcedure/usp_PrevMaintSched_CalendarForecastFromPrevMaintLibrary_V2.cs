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
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-25 SOM-338  Roger Lawton       Modified to use ProcessRowExtended
*                                         Added SiteId
****************************************************************************************************
*/

using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using System.Collections.Generic;
using Database.SqlClient;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    /// <summary>
    /// Access the usp_PrevMaintSched_RetrieveByEquipmentId stored procedure.
    /// </summary>
    public class usp_PrevMaintSched_CalendarForecastFromPrevMaintLibrary_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintSched_CalendarForecastFromPrevMaintLibrary_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PrevMaintSched_CalendarForecastFromPrevMaintLibrary_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_PrevMaintSched_RetrieveByEquipmentId stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="processRow">ProcessRow delegate containing method to call to process the row into an object.</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="clientId">long that contains the value of the @ClientId parameter</param>
        /// <param name="sessionId">System.Guid that contains the value of the @SessionId parameter</param>
        /// <returns>ArrayList containing the results of the query</returns>
        public static List<b_PrevMaintSched> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_PrevMaintSched obj
        )
        {
            int ReportTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["ReportTimeOut"].ToString());
            List<b_PrevMaintSched> records = new List<b_PrevMaintSched>();
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
            command.SetInputParameter(SqlDbType.DateTime2, "ScheduleThroughDate", obj.SchedueledDate);
            command.SetInputParameter(SqlDbType.DateTime2, "CurrentDate", obj.CurrentDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "assignedPMS", obj.AssignedTo_PersonnelIds, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "requiredDate", obj.PMForeCastRequiredDate, 500);
            command.SetStringInputParameter(SqlDbType.NVarChar, "shift", obj.Shift, 500);
            command.SetInputParameter(SqlDbType.Bit, "DownRequired", obj.ForecastDownRequired);
            command.CommandTimeout = 60 * ReportTimeOut;
            TimeSpan timeout = TimeSpan.FromMinutes(ReportTimeOut);
            var cancellationTokenSource = new CancellationTokenSource();

            try
            {
                var task = Task.Run(() =>
                {
                    // Execute stored procedure.
                    reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_PrevMaintSched)b_PrevMaintSched.ProcessRowExtendedForCalenderForecast_V2(reader));
                }

                reader.NextResult();

                while (reader.Read())
                {
                    // Add the record to the list.
                    //records.Add(reader.GetString(0));
                }
                }, cancellationTokenSource.Token);
                // Wait for the task to complete or timeout
                if (!task.Wait(timeout))
                {
                    // Cancel the task if it exceeds the timeout
                    cancellationTokenSource.Cancel();
                    throw new OperationCanceledException("The operation was canceled due to timeout.");
                }

            }
            catch (AggregateException ex)
            {
                // Unwrap AggregateException to get the inner exceptions
                foreach (var innerEx in ex.InnerExceptions)
                {
                    if (innerEx is SqlException sqlEx)
                    {
                        throw sqlEx;
                    }
                    if (innerEx is OperationCanceledException cancelEx)
                    {
                        throw cancelEx;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException exinvalid)
                {
                    throw exinvalid;
                }
                else
                {

                    throw ex;
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
                cancellationTokenSource.Dispose();
            }

            // Process the RETURN_CODE parameter value
            retCode = (int)RETURN_CODE_parameter.Value;
            AbstractTransactionManager.CheckReturnCodeStatus(STOREDPROCEDURE_NAME, retCode);
            // Return the result
            return records;
        }
    }
}