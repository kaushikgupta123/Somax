using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_MaintOnDemandMasterTask
    {

        public void MaintOnDemandMasterTask_RetrieveAll(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_MaintOnDemandMasterTask[] data
    )
        {
            Database.SqlClient.ProcessRow<b_MaintOnDemandMasterTask> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_MaintOnDemandMasterTask[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_MaintOnDemandMasterTask>(reader => { b_MaintOnDemandMasterTask obj = new b_MaintOnDemandMasterTask(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_MaintOnDemandMasterTask_RetriveByMasterId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_MaintOnDemandMasterTask[])results.ToArray(typeof(b_MaintOnDemandMasterTask));
                }
                else
                {
                    data = new b_MaintOnDemandMasterTask[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


    }
}
