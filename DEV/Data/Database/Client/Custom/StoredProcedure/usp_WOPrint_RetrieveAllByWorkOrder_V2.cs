using System;
using System.Data;
using System.Data.SqlClient;
using Database.Business;
using Database.SqlClient;
using System.Data.Common;
using System.Collections.Generic;

namespace Database.StoredProcedure
{
    
    public class usp_WOPrint_RetrieveAllByWorkOrder_V2
    {
        /// <summary>
        /// Constants.
        /// </summary>
        private static string STOREDPROCEDURE_NAME = "usp_WOPrint_RetrieveAllByWorkOrder_V2";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_WOPrint_RetrieveAllByWorkOrder_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_Equipment_RetrieveChunkSearch_V2 stored procedure using SqlClient.
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
            command.SetStringInputParameter(SqlDbType.NVarChar, "WorkOrderIDList", obj.WorkOrderIDList.TrimStart(','), 1073741823);
            command.CommandTimeout = 300;

            try
            {
                // Execute stored procedure.
                reader = command.ExecuteReader();

                obj.listOfAttachment = new List<b_Attachment>();
                while (reader.Read())
                {
                    b_Attachment tmpAttachments = b_Attachment.ProcessRowForAttachmentPrint(reader);
                    obj.listOfAttachment.Add(tmpAttachments);
                }
                reader.NextResult();
                obj.listOfWO = new List<b_WorkOrder>();
                while (reader.Read())
                {
                    b_WorkOrder tmpWorkOrders = b_WorkOrder.ProcessRowForWorkOrderPrint(reader);
                    obj.listOfWO.Add(tmpWorkOrders);
                }
                reader.NextResult();
                obj.listOfWOTask = new List<b_WorkOrderTask>();
                while (reader.Read())
                {
                    b_WorkOrderTask tmpWOTask = b_WorkOrderTask.ProcessRowForWorkOrderTaskPrint(reader);
                    obj.listOfWOTask.Add(tmpWOTask);
                }
                reader.NextResult();

                obj.listOfTimecard = new List<b_Timecard>();
                while (reader.Read())
                {
                    b_Timecard tmpTimeCard = b_Timecard.ProcessRowForTimeCardPrint(reader);
                    obj.listOfTimecard.Add(tmpTimeCard);
                }
                reader.NextResult();
                obj.listOfPartHistory = new List<b_PartHistory>();
                while (reader.Read())
                {
                    b_PartHistory tmpPartHistory = b_PartHistory.ProcessRowFortmpPartHistoryPrint(reader);
                    obj.listOfPartHistory.Add(tmpPartHistory);
                }
                reader.NextResult();

                obj.listOfOtherCosts = new List<b_OtherCosts>();
                while (reader.Read())
                {
                    b_OtherCosts tmpOtherCosts = b_OtherCosts.ProcessRowFortmpOtherCostsPrint(reader);
                    obj.listOfOtherCosts.Add(tmpOtherCosts);
                }
                reader.NextResult();
                obj.listOfSummery = new List<b_OtherCosts>();
                while (reader.Read())
                {
                    b_OtherCosts tmpSummery = b_OtherCosts.ProcessRowForSummeryPrint(reader);
                    obj.listOfSummery.Add(tmpSummery);
                }
                reader.NextResult();
                obj.listOfInstructions = new List<b_Instructions>();
                while (reader.Read())
                {
                    b_Instructions tmpInstructions = b_Instructions.ProcessRowForInstructionsPrint(reader);
                    obj.listOfInstructions.Add(tmpInstructions);
                }
                reader.NextResult();
                obj.listOfWorkOrderUDF = new List<b_WorkOrderUDF>();
                while (reader.Read())
                {
                    b_WorkOrderUDF tmpWorkOrderUDF = b_WorkOrderUDF.ProcessRowForWorkOrderUDFPrint(reader);
                    obj.listOfWorkOrderUDF.Add(tmpWorkOrderUDF);
                }
                reader.NextResult();
                obj.listOfWorkOrderSchedule = new List<b_WorkOrderSchedule>();
                while (reader.Read())
                {
                    b_WorkOrderSchedule tmpWorkOrderSchedule = b_WorkOrderSchedule.ProcessRowForWorkOrderSchedulePrint(reader);
                    obj.listOfWorkOrderSchedule.Add(tmpWorkOrderSchedule);
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
