/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ============================================================
* 
 * ****************************************************************************************************
 */

using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
   
    public partial class b_PrevMaintLibraryTask
    { 
        public void RetrieveByPrevMaintLibraryId(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
    string callerUserName,
          ref List<b_PrevMaintLibraryTask> results
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
                results = Database.StoredProcedure.usp_PrevMaintLibraryTask_RetrieveByPrevMaintLibraryId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);              
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
        public static b_PrevMaintLibraryTask ProcessRowForPrevMaintTaskLibrary(SqlDataReader reader)
        {
            // Create instance of object
            b_PrevMaintLibraryTask lt = new b_PrevMaintLibraryTask();
            // Load the object from the database
            lt.LoadFromDatabase(reader);
            // Return result
            return lt;
        }

    }
}
