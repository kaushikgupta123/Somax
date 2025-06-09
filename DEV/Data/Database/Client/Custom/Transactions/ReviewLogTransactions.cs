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

    public class ReviewLog_RetrieveByPMRId : ReviewLog_TransactionBaseClass
    {

        public List<b_ReviewLog> reviewLogList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_ReviewLog> tmpList = null;
            ReviewLog.RetrieveLogDeatilsByPMRId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            reviewLogList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

}

