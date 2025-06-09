/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PopupAddWorkRequest
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Sep-01 SOM-1037 Roger Lawton       Added RetrieveTargetList
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using Common.Enumerations;
using Database.Business;

namespace Database.Transactions
{
  public class AlertTarget_RetrieveTargetList : AbstractTransactionManager
  {
    public Int64 AlertSetupId;
    public bool Include_Inactive;
        public List<b_AlertTarget> AlertTargetList { get; set; }

    public AlertTarget_RetrieveTargetList()
    {
      // Set the database in which this table resides.
      // This must be called prior to base.PerformLocalValidation(), 
      // since that process will validate that the appropriate 
      // connection string is set.
      UseDatabase = DatabaseTypeEnum.Client;
      // Do not need a transaction - only reading data
      this.UseTransaction = false;
    }

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
      b_AlertTarget[] tmpArray = null;
      b_AlertTarget o = new b_AlertTarget();
      o.ClientId = dbKey.Client.ClientId;
      o.RetrieveTargetList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, AlertSetupId, Include_Inactive, ref tmpArray);
      AlertTargetList = new List<b_AlertTarget>(tmpArray);
    }
  }
}
