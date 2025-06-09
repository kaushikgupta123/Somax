using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
   public partial class b_ReportGridDefintion : DataBusinessBase
    {
        public  void RetrieveByReportListingId(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
    string callerUserName,   
             ref List<b_ReportGridDefintion> data
      )
        {
            Database.SqlClient.ProcessRow<b_ReportGridDefintion> processRow = null;
            List<b_ReportGridDefintion> results = null;
            SqlCommand command = null;
            string message = String.Empty;
            // Initialize the results
            data = new List<b_ReportGridDefintion>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ReportGridDefintion>(reader => { b_ReportGridDefintion obj = new b_ReportGridDefintion(); obj.LoadFromDatabase(reader); return obj; });
                //processRow = new Database.SqlClient.ProcessRow<b_ReportGridDefintion>(reader => { this.LoadFromDatabase(reader); return this; });
                // Database.StoredProcedure.usp_ReportGridDefintion_RetrieveByReportListingId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
                results = Database.StoredProcedure.usp_ReportGridDefintion_RetrieveByReportListingId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
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
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void RetrieveAllByReportListingId_V2(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
string callerUserName,
         ref List<b_ReportGridDefintion> data
  )
        {
            Database.SqlClient.ProcessRow<b_ReportGridDefintion> processRow = null;
            List<b_ReportGridDefintion> results = null;
            SqlCommand command = null;
            string message = String.Empty;
            // Initialize the results
            data = new List<b_ReportGridDefintion>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ReportGridDefintion>(reader => { b_ReportGridDefintion obj = new b_ReportGridDefintion(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_ReportGridDefintion_RetrieveAllByReportListingId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = results;
                }
                else
                {
                    data = null;
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
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
    }
}
