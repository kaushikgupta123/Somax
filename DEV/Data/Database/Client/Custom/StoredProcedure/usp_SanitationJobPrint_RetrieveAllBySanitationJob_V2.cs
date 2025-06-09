using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    
    public class usp_SanitationJobPrint_RetrieveAllBySanitationJob_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_SanitationJobPrint_RetrieveAllBySanitationJob_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SanitationJobPrint_RetrieveAllBySanitationJob_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_SanitationJobPrint_RetrieveAllBySanitationJob_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static b_SanitationJob CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SanitationJob obj
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "SanitationJobIDList", obj.SanitationJobIdList.TrimStart(','), 1073741823);
            command.CommandTimeout = 300;

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                //SanitationJob
                obj.ListSanJob = new List<b_SanitationJob>();
                while (reader.Read())
                {
                    b_SanitationJob tmpSanitationJob = b_SanitationJob.ProcessRowForDevExpressSanitationJob(reader);
                    obj.ListSanJob.Add(tmpSanitationJob);
                }

                //Tool List
                reader.NextResult();
                obj.listOfSanitationTool = new List<b_SanitationPlanning>();
                while (reader.Read())
                {
                    b_SanitationPlanning tmpSanitationPlanning = b_SanitationPlanning.ProcessRowForDevExpressSanitationPlanning(reader);
                    obj.listOfSanitationTool.Add(tmpSanitationPlanning);
                }

                //Chemical List
                reader.NextResult();
                obj.listOfSanitationChemical = new List<b_SanitationPlanning>();
                while (reader.Read())
                {
                    b_SanitationPlanning tmpSanitationPlanning = b_SanitationPlanning.ProcessRowForDevExpressSanitationPlanningChemical(reader);
                    obj.listOfSanitationChemical.Add(tmpSanitationPlanning);
                }

                //Task List
                reader.NextResult();
                obj.listOfSanitationTask = new List<b_SanitationJobTask>();
                while (reader.Read())
                {
                    b_SanitationJobTask tmpSanitationJobTask = b_SanitationJobTask.ProcessRowForDevExpressSanitationJobTask(reader);
                    obj.listOfSanitationTask.Add(tmpSanitationJobTask);
                }

                //Labour List
                reader.NextResult();
                obj.listOfTimecard = new List<b_Timecard>();
                while (reader.Read())
                {
                    b_Timecard tmpTimeCard = b_Timecard.ProcessRowForDevExpressTimeCardPrint(reader);
                    obj.listOfTimecard.Add(tmpTimeCard);
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
