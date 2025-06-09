using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.Business;
using Database.SqlClient;

namespace Database.StoredProcedure
{
    internal class usp_PrevMaintSched_SchedulingRecordsForReassign_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_PrevMaintSched_SchedulingRecordsForReassign_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_PrevMaintSched_SchedulingRecordsForReassign_V2()
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
            command.SetStringInputParameter(SqlDbType.VarChar, "orderbyColumn", obj.OrderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.VarChar, "orderBy", obj.OrderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset", obj.Offset);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.Nextrow);
            command.SetStringInputParameter(SqlDbType.NVarChar, "AssignedTo", obj.AssignedTo, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "PMID", obj.PMID, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Description", obj.Description, 256);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToClientLookupId", obj.ChargeToClientLookupId, 31);
            command.SetStringInputParameter(SqlDbType.NVarChar, "ChargeToName", obj.ChargeToName, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "NextDue", obj.NextDue, 500);

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.
                    records.Add((b_PrevMaintSched)b_PrevMaintSched.ProcessRowForSchedulingRecordsForMultipleAssignment(reader));
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
