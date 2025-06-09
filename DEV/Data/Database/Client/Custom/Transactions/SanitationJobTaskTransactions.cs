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
* 2015-Mar-21 SOM-585  Roger Lawton        Cleaned up
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
    public class SanitationJobTaskTransactions_RetrieveForWorkBench : SanitationJobTask_TransactionBaseClass
    {
        public List<b_SanitationJobTask> SanitationJobTaskList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            //List<b_SanitationJobTask> tmpArray = null;
            ////b_SanitationJob o = new b_SanitationJob();

            ////SanitationJob
            //// Explicitly set tenant id from dbkey
            //SanitationJobTask.ClientId = this.dbKey.Client.ClientId;

            //SanitationJobTask.SanitationJobTask_RetrieveForWorkBench(this.Connection, this.Transaction, ref tmpArray);

            //SanitationJobTaskList = new List<b_SanitationJobTask>(tmpArray);
        }

    }

    

    public class SanitationJobTask_RetrieveBySanitationJob : SanitationJobTask_TransactionBaseClass
    {
        public List<b_SanitationJobTask> SanitationJobTaskList { get; set; }
        public SanitationJobTask_RetrieveBySanitationJob()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        //public List<b_SanitationJobTask> SanitationJobTaskList { get; set; }

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
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_SanitationJobTask> tmpArray = null;

           SanitationJobTask.RetrieveBySanitationJob(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobTaskList = new List<b_SanitationJobTask>();
            foreach (b_SanitationJobTask tmpObj in tmpArray)
            {
                SanitationJobTaskList.Add(tmpObj);
            }
        }
    }

    public class SanitationJobTask_UpdateBySanitationJobTaskId : SanitationJobTask_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (SanitationJobTask.SanitationJobTaskId == 0)
            //{
            //    string message = "SanitationJobTask has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
           // SanitationJobTask.UpdateBySanitationJobTaskId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    //====================
    public class SanitationJobTask_RetrieveBy_SanitationJobId : SanitationJobTask_TransactionBaseClass
    {
        public List<b_SanitationJobTask> SanitationJobTaskList { get; set; }
        public SanitationJobTask_RetrieveBy_SanitationJobId()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        //public List<b_SanitationJobTask> SanitationJobTaskList { get; set; }
        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_SanitationJobTask> tmpArray = null;

            SanitationJobTask.RetrieveBy_SanitationJobID(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SanitationJobTaskList = new List<b_SanitationJobTask>();
            foreach (b_SanitationJobTask tmpObj in tmpArray)
            {
                SanitationJobTaskList.Add(tmpObj);
            }
        }


    }
    public class CreateNew_SanitationJobTask : SanitationJobTask_TransactionBaseClass
    {

        public CreateNew_SanitationJobTask()
        {

            UseDatabase = DatabaseTypeEnum.Client;
        }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (SanitationJobTask.SanitationJobTaskId > 0)
            //{
            //    string message = "SanitationJobTask has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            SanitationJobTask.InsertNew_IntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        //public override void Postprocess()
        //{
        //    base.Postprocess();
        //    System.Diagnostics.Debug.Assert(SanitationJobTask.SanitationJobTaskId > 0);
        //}
    }
    public class Update_SanitationJobTaskBy_SanitationJobTaskId : SanitationJobTask_TransactionBaseClass
    {
        public Update_SanitationJobTaskBy_SanitationJobTaskId()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SanitationJobTask.SanitationJobTaskId == 0)
            {
                string message = "SanitationJobTask has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            SanitationJobTask.UpdateInDatabaseBy_SanitationJobTaskID(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
           // SanitationJobTask.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class SanitationJobTask_RetrieveSingleBy_SanitationJobId : SanitationJobTask_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (SanitationJobTask.SanitationJobTaskId == 0)
            //{
            //    string message = "SanitationJobTask has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
           SanitationJobTask.RetrieveSingleBy_SanitationJobId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
}
