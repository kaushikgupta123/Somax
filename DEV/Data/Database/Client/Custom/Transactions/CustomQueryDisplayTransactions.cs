/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Structures;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class RetrieveCustomQueryDisplayByTableAndLanguageTransaction : AbstractTransactionManager
    {
        public List<b_CustomQueryDisplay> Items;

        public string TableName { get; set; }
        public string Language { get; set; }
        public string Culture { get; set; }

        public RetrieveCustomQueryDisplayByTableAndLanguageTransaction()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria;
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
                SqlClient.ProcessRow<b_CustomQueryDisplay> processRow = 
                    new SqlClient.ProcessRow<b_CustomQueryDisplay>(reader => { b_CustomQueryDisplay obj = new b_CustomQueryDisplay(); obj.LoadFromDatabase(reader); return obj; });

                b_CustomQueryDisplay cqd = new b_CustomQueryDisplay()
                {
                    ClientId = this.dbKey.Client.ClientId,
                    TableName = this.TableName,
                    Language = this.Language,
                    Culture = this.Culture
                };
                Items = usp_CustomQueryDisplay_RetrieveByTableAndLanguage.CallStoredProcedure(command, processRow, dbKey.User.UserInfoId, dbKey.UserName, cqd);
                    
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
