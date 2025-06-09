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
* 2016-Aug-10 SOM-1037 Roger Lawton       Added to retreive alert information for notification
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class AlertSetup_RetrieveForNotification : AlertSetup_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            AlertSetup.RetrieveForNotification(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class AlertSetup_RetrieveNotificationDetails : AlertSetup_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            AlertSetup.RetrieveNotificationDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
