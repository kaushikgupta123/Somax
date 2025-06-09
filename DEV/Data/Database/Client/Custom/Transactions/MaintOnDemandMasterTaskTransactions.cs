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
 * Date        Task ID   Person             Description
 * =========== ======== =================== ===================================
 * 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
 * 2012-Feb-02 20120001 Roger Lawton        Changed to support additional columns on search
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{

    public class MaintOnDemandMasterTaskRetrieveAll : MaintOnDemandMasterTask_TransactionBaseClass
    {
        public List<b_MaintOnDemandMasterTask> MaintOnDemandMasterTaskList { get; set; }
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
            b_MaintOnDemandMasterTask[] tmpArray = null;

            MaintOnDemandMasterTask.MaintOnDemandMasterTask_RetrieveAll(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            MaintOnDemandMasterTaskList = new List<b_MaintOnDemandMasterTask>(tmpArray);
        }

    }


}
