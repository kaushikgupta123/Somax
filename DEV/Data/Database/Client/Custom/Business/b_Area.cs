using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_Area
    { 
            public void RetrieveByClientIdSiteId(SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_Area> data)
            {
                SqlCommand command = null;
                string message = String.Empty;
                List<b_Area> results = null;
                data = new List<b_Area>();

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    results = Database.StoredProcedure.usp_Area_RetrieveByClientIdSiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                    if (results != null)
                    {
                        data = results;
                    }
                    else
                    {
                        data = new List<b_Area>();
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
            public static object ProcessRowForArea(SqlDataReader reader)
            {                        
                b_Area obj = new b_Area();
                obj.LoadFromDatabase(reader);               
                return (object)obj;
            }


            public void RetrieveAreaId(
                    SqlConnection connection,
                    SqlTransaction transaction,
                    long callerUserInfoId,
                    string callerUserName
                )
            {
                Database.SqlClient.ProcessRow<b_Area> processRow = null;
                SqlCommand command = null;
                string message = String.Empty;

                try
                {
                    // Create the command to use in calling the stored procedures
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.Transaction = transaction;

                    // Call the stored procedure to retrieve the data
                    processRow = new Database.SqlClient.ProcessRow<b_Area>(reader => { this.LoadFromDatabase(reader); return this; });
                    Database.StoredProcedure.usp_Area_RetrieveByAreaId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
