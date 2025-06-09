/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
****************************************************************************************************
 */
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_ReviewLog
    {
        public string Reviewed_By { get; set; }



        public void RetrieveLogDeatilsByPMRId(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_ReviewLog> results
       )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_ReviewLog_RetrieveLogDeatilsByPMRId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_ReviewLog ProcessRowForRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_ReviewLog reviewLog = new b_ReviewLog();

            reviewLog.LoadFromDatabaseByFK(reader);

            return reviewLog;
        }
        public void LoadFromDatabaseByFK(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // Reviewed_By column
                if (false == reader.IsDBNull(i))
                {
                    Reviewed_By = reader.GetString(i);
                }
                else
                {
                    Reviewed_By = string.Empty;
                }
                i++;

               


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Reviewed_By"].ToString(); }
                catch { missing.Append("Reviewed_By "); }

              
            }

        }


    }
}
