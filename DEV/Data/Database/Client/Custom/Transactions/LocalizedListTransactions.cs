/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2014-Aug-01 SOM_246   Roger Lawton       Added 
***************************************************************************************************
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

  public class RetrieveLanguageSpecificLocalizedList : LocalizedList_TransactionBaseClass
  {

    public RetrieveLanguageSpecificLocalizedList()
    {
      // Set the database in which this table resides.
      // This must be called prior to base.PerformLocalValidation(), 
      // since that process will validate that the appropriate 
      // connection string is set.
      UseDatabase = DatabaseTypeEnum.Client;
      UseTransaction = false;
    }

    public List<b_LocalizedList> LocalizedListList { get; set; }

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
      b_LocalizedList[] tmpArray = null;


      // Explicitly set tenant id from dbkey
      this.LocalizedList.ClientId = this.dbKey.Client.ClientId;


      this.LocalizedList.RetrieveLanguageSpecificLocalizedList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

      LocalizedListList = new List<b_LocalizedList>(tmpArray);
    }
  }
}
