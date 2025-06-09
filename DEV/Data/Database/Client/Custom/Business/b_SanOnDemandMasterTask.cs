
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using Database.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_SanOnDemandMasterTask
    {

        #region Task All Work
        public void RetrieveAllBy_SanOnDemandMasterId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_SanOnDemandMasterTask> data
            )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_SanOnDemandMasterTask> results = null;
            data = new List<b_SanOnDemandMasterTask>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_SanOnDemandMasterTask_RetrieveAllBySanOnDemandMasterId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_SanOnDemandMasterTask>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        #endregion
    }
}
