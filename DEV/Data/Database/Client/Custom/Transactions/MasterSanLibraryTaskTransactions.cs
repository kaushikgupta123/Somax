/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================

****************************************************************************************************
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{


    public class MasterSanLibraryTask_RetrieveByPrevMaintLibraryId : MasterSanLibraryTask_TransactionBaseClass
    {

        public MasterSanLibraryTask_RetrieveByPrevMaintLibraryId()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_MasterSanLibraryTask> MasterSanLibraryTaskList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            List<b_MasterSanLibraryTask> tmpList = null;
            MasterSanLibraryTask.ClientId = this.dbKey.Client.ClientId;
            MasterSanLibraryTask.RetrieveByPrevMaintLibraryId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            MasterSanLibraryTaskList = tmpList;
        }
    }

}
