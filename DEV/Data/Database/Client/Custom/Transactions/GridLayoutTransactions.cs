/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* Grid Layout 
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Aug-07 SOM-1049 Roger Lawton       Added new class to store/retrieve grid layout information
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
  public class GridDataLayout_RetrievebyGridSiteUser : GridDataLayout_TransactionBaseClass
    {
      
        public override void PerformLocalValidation()
        {
          base.UseTransaction = false;
          base.PerformLocalValidation();
          if (GridDataLayout.GridDataLayoutId > 0)
          {
              string message = "GridDataLayout has an invalid ID.";
              throw new Exception(message);
          }
        }
        public override void PerformWorkItem()
        {
          GridDataLayout.RetrieveByGridSiteUser(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
