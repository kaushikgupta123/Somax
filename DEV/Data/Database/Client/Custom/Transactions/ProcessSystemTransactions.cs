/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2018 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
 * 2012-Feb-02 20120001 Roger Lawton        Changed to support additional columns on search
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
    
 
    public class ProcessSystemValidate : ProcessSystem_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (ProcedureMaster.ProcedureMasterId > 0)
            //{
            //    string message = "ProcedureMaster has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            ProcessSystem.Validate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class ProcessSystem_RetrieveForSearch : ProcessSystem_TransactionBaseClass
    {
        public List<b_ProcessSystem> ProcessSystemList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (InvoiceMatchItem.InvoiceMatchItemId > 0)
            //{
            //    string message = "InvoiceMatchItemId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_ProcessSystem> tmpList = null;
            ProcessSystem.RetrieveForSearch(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref tmpList);
            ProcessSystemList = new List<b_ProcessSystem>();
            foreach (b_ProcessSystem tmpObj in tmpList)
            {
                ProcessSystemList.Add(tmpObj);
            }
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

 
    public class ProcessSystem_RetrieveByPrimaryKey : ProcessSystem_TransactionBaseClass
    {
        public List<b_ProcessSystem> ProcessSystemList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (InvoiceMatchHeader.InvoiceMatchHeaderId > 0)
            //{
            //    string message = "InvoiceMatchItem has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_ProcessSystem> tmpList = null;
            ProcessSystem.RetrieveByPKForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            ProcessSystemList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class ProcessSystem_UpdateByForeignKey : ProcessSystem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ProcessSystem.ProcessSystemId == 0)
            {
                string message = "ProcessSystem has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            ProcessSystem.UpdateByPKForeignKey(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    //-----------------------End Added By Indusnet Technologies-----------------------------------------------
    //------------SOM - 835 ---Treelist---------------------------------------------------------------------------------------
  
    public class ProcessSystem_RetrieveAllForProcess : ProcessSystem_TransactionBaseClass
    {
        public List<b_ProcessSystem> ProcessSystemList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (InvoiceMatchItem.InvoiceMatchItemId > 0)
            //{
            //    string message = "InvoiceMatchItemId has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            
            b_ProcessSystem[] tmpArray = null;

           
            ProcessSystem.RetrieveAllForProcess(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            ProcessSystemList = new List<b_ProcessSystem>(tmpArray);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class ProcessSystem_CreateByForeignKeys : ProcessSystem_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (ProcessSystem.ProcessSystemId > 0)
            {
                string message = "ProcessSystem has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            ProcessSystem.CreateByPkForeignKey(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(ProcessSystem.ProcessSystemId > 0);
        }
    }
}
