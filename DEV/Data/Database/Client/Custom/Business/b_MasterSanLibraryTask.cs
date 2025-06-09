/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* b_PrevMaintTask.cs (Data Object)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Jul-29 SOM-259  Roger Lawton       Added AssignedTo_ClientLookupId and ChargeTo_ClientLookupId
*                                         Properties
*                                         Added LoadFromDatabaseExtended method
*                                         Modified PrevMaintTaskRetrieveByPrevMaintMasterId method
*                                         by removing the ProcessRow delegate - was not working as
*                                         expected (see change log in class
*                                         usp_PrevMaintTask_RetrieveByPrevMaintMasterId)
* 2014-Sep-05 SOM-304  Roger Lawton       Added Validation, Creat and Update Methods
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

    public partial class b_MasterSanLibraryTask
    {
        public void RetrieveByPrevMaintLibraryId(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
    string callerUserName,
          ref List<b_MasterSanLibraryTask> results
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
                results = Database.StoredProcedure.usp_MasterSanLibraryTask_RetrieveByMasterSanLibraryId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_MasterSanLibraryTask ProcessRowForMasterSanLibraryTaskLibrary(SqlDataReader reader)
        {
            // Create instance of object
            b_MasterSanLibraryTask lt = new b_MasterSanLibraryTask();
            // Load the object from the database
            lt.LoadFromDatabase(reader);
            // Return result
            return lt;
        }

    }
}
