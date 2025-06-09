/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date         Log Entry   Person                  Description
 * ===========  =========   ======================= ===========================
 * 2011-Nov-22  201100000   Roger Lawton            Created
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database.Transactions
{


    public class SiteLookupList : AbstractTransactionManager
    {
        public SiteLookupList()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_Site> result;
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                // Call the stored procedure to retrieve the data
                result = usp_Site_RetrieveList.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }

    }
}
