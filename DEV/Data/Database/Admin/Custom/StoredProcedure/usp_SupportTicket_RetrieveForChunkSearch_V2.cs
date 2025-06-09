using Database.Business;
using Database.SqlClient;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Database.StoredProcedure
{
    public class usp_SupportTicket_RetrieveForChunkSearch_V2
    {
        private static string STOREDPROCEDURE_NAME = "usp_SupportTicket_RetrieveForChunkSearch_V2";
        /// <summary>
        /// Default constructor.
        /// </summary>
        public usp_SupportTicket_RetrieveForChunkSearch_V2()
        {
        }

        /// <summary>
        /// Static method to call the usp_KBTopics_RetrieveChunkSearch_V2 stored procedure using SqlClient.
        /// </summary>
        /// <param name="command">SqlCommand object to use to call the stored procedure</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public static List<b_SupportTicket> CallStoredProcedure(
            SqlCommand command,
            long callerUserInfoId,
            string callerUserName,
            b_SupportTicket obj
        )
        {
            List<b_SupportTicket> records = new List<b_SupportTicket>();
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
            command.SetInputParameter(SqlDbType.BigInt, "PersonnelId", obj.PersonnelId);
            command.SetInputParameter(SqlDbType.BigInt, "SupportTicketId", obj.SupportTicketId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderbyColumn", obj.orderbyColumn, 100);
            command.SetStringInputParameter(SqlDbType.NVarChar, "orderBy", obj.orderBy, 10);
            command.SetInputParameter(SqlDbType.Int, "offset1", obj.offset1);
            command.SetInputParameter(SqlDbType.Int, "nextrow", obj.nextrow);
            command.SetInputParameter(SqlDbType.Int, "CaseNo", obj.CustomQueryDisplayId);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Contact", obj.Contact,63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Subject", obj.Subject, 63);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Status", obj.Status, 15);
            command.SetStringInputParameter(SqlDbType.NVarChar, "Agent", obj.Agent,63);
            command.SetInputParameter(SqlDbType.DateTime2, "CreateDate", obj.CreateDate);
            command.SetInputParameter(SqlDbType.DateTime2, "CompleteDate", obj.CompleteDate);
            command.SetStringInputParameter(SqlDbType.NVarChar, "SearchText ", obj.SearchText, 800);



            try
            {

                // Execute stored procedure.
                reader = command.ExecuteReader();
                // Loop through dataset.
                while (reader.Read())
                {
                    // Add the record to the list.0
                    b_SupportTicket tmpSupportTicket = b_SupportTicket.ProcessRowForSupportTicketRetriveAllForChunkSearch(reader);
                    //tmpSupportTicket.ClientId = obj.ClientId;
                    records.Add(tmpSupportTicket);
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
            return records;
        }
    }
}
