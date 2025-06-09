/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014-2017 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person            Description
* =========== ========= ================= =======================================================
* 2017-Dec-05 SOM-1515  Roger Lawton      Add Alerts_ClearAlert Class
**************************************************************************************************
*/


using System;

namespace Database
{
  public class Alerts_UpdateClear : Alerts_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Alerts.AlertsId > 0)
            {
                string message = "Alerts has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Alerts.InsertAndClear(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Alerts.AlertsId > 0);
        }
    }

    public class Alerts_ClearAlert : Alerts_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
      }
      public override void PerformWorkItem()
      {
        Alerts.ClearAlert(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }

      public override void Postprocess()
      {
        base.Postprocess();
      }
    }
  
  public class Alerts_RetrieveList : Alerts_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }

        public override void PerformWorkItem()
        {
            Alerts.AlertClear(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


}
