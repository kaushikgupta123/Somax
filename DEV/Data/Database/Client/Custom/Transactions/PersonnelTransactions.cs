/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-06 SOM-315  Roger Lawton       Retrieve for lookup
* 2016-Oct-31 SOM-642  Roger Lawton       Update personnel info if appropriate
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
  public class Personnel_RetrieveBySearchCriteria : AbstractTransactionManager
  {
    public Personnel_RetrieveBySearchCriteria()
    {
      base.UseDatabase = DatabaseTypeEnum.Client;
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public string Query { get; set; }
    public string Area { get; set; }
    public string Site { get; set; }
    public string Department { get; set; }

    public string Status { get; set; }
    public string Crew { get; set; }
    public string Craft { get; set; }
    public string Shift { get; set; }

    public string ColumnName { get; set; }
    public string ColumnSearchText { get; set; }
    public bool MatchAnywhere { get; set; }

    public string DateSelection { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }

    public int PageNumber { get; set; }
    public int ResultsPerPage { get; set; }

    public string OrderColumn { get; set; }
    public string OrderDirection { get; set; }

    // Result Sets
    public List<b_Personnel> PersonnelList { get; set; }
    public int ResultCount { get; set; }

    public override void PerformWorkItem()
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand()
        {
          Connection = this.Connection,
          Transaction = this.Transaction
        };

        int tmp;

        PersonnelList = usp_Personnel_RetrieveBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department,
            Status, Crew, Craft, Shift, DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

        ResultCount = tmp;
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
      }
    }

    public override void Preprocess()
    {
      // throw new NotImplementedException();
    }

    public override void Postprocess()
    {
      // throw new NotImplementedException();
    }
  }

  public class Personnel_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
  {
    public Personnel_RetrieveLookupListBySearchCriteria()
    {
      base.UseDatabase = DatabaseTypeEnum.Client;
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    //public string PersonnelId { get; set; }
    public string ClientLookupId { get; set; }
    public string NameFull { get; set; }
    public string Email { get; set; }
    public long SiteId { get; set; }

    public int PageNumber { get; set; }
    public int ResultsPerPage { get; set; }

    public string OrderColumn { get; set; }
    public string OrderDirection { get; set; }

    // Result Sets
    public List<b_Personnel> PersonnelList { get; set; }
    public int ResultCount { get; set; }

    public override void PerformWorkItem()
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand()
        {
          Connection = this.Connection,
          Transaction = this.Transaction
        };

        int tmp;

        PersonnelList = usp_Personnel_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, NameFull, Email, SiteId,
                PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

        ResultCount = tmp;
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
      }
    }

    public override void Preprocess()
    {
      // throw new NotImplementedException();
    }

    public override void Postprocess()
    {
      // throw new NotImplementedException();
    }
  }
  // SOM-1688
  // Not Used Anywhere
  //public class Personnel_RetrieveByPKExtended : Personnel_TransactionBaseClass
  //{
  //  public override void Preprocess()
  //  {
  //    //throw new NotImplementedException();
  //  }

  //  public override void Postprocess()
  //  {
  //    //throw new NotImplementedException();
  //  }

  //  public override void PerformLocalValidation()
  //  {
  //    base.PerformLocalValidation();
  //  }

  //  public override void PerformWorkItem()
  //  {
  //    base.UseTransaction = false;

  //    Personnel.RetrieveByPKExtended(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

  //  }
  //}
  public class Personnel_ValidateExtended : Personnel_TransactionBaseClass
  {
    public Personnel_ValidateExtended()
    {
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();

    }

    public bool CreateMode { get; set; }
    public string Supervisor_ClientLookupId { get; set; }
    public string DefaultStoreroom_Name { get; set; }
    public System.Data.DataTable lulist { get; set; }
    // Result Sets
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

    public override void PerformWorkItem()
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {

        List<b_StoredProcValidationError> errors = null;


        Personnel.ValidateExtended(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                Supervisor_ClientLookupId, DefaultStoreroom_Name, CreateMode, lulist, ref errors);

        StoredProcValidationErrorList = errors;
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
      }
    }

    public override void Preprocess()
    {
      // throw new NotImplementedException();
    }

    public override void Postprocess()
    {
      // throw new NotImplementedException();
    }
  }


  public class Personnel_RetrieveClientLookupIdBySearchCriteria : Personnel_TransactionBaseClass
  {
    public List<b_Personnel> PersonnelList { get; set; }

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
      List<b_Personnel> tmpList = null;

      Personnel.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

      PersonnelList = new List<b_Personnel>();
      foreach (b_Personnel tmpObj in tmpList)
      {
        PersonnelList.Add(tmpObj);
      }
    }
  }
  public class Personnel_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
  {
    public Personnel_RetrieveInitialSearchConfigurationData()
    {
      UseDatabase = DatabaseTypeEnum.Client;
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria;

    public override void PerformWorkItem()
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        // Create the command to use in calling the stored procedures
        command = new SqlCommand()
        {
          Connection = this.Connection,
          Transaction = this.Transaction
        };

        // Call the stored procedure to retrieve the data
        SearchCriteria = usp_Personnel_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
      }
    }

    public override void Preprocess()
    {
      // throw new NotImplementedException();
    }

    public override void Postprocess()
    {
      // throw new NotImplementedException();
    }
  }

  public class Personnel_RetrieveByPKs : Personnel_TransactionBaseClass
  {
    public List<b_Personnel> PersonnelList { get; set; }
    public List<long> PersonnelIds { get; set; }

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
      PersonnelList = new List<b_Personnel>();

      foreach (long id in PersonnelIds)
      {
        b_Personnel tmp = new b_Personnel() { PersonnelId = id, ClientId = dbKey.Client.ClientId };
        tmp.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        PersonnelList.Add(tmp);
      }
    }
  }

  public class Personnel_UpdateExtended : Personnel_TransactionBaseClass
  {
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Personnel.PersonnelId == 0)
      {
        string message = "Personnel has an invalid ID.";
        throw new Exception(message);
      }
    }

    public override void PerformWorkItem()
    {
      Personnel.UpdateExtended(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
    }
  }

  public class Personnel_CreateExtended : Personnel_TransactionBaseClass
  {
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Personnel.PersonnelId > 0)
      {
        string message = "Personnel has an invalid ID.";
        throw new Exception(message);
      }
    }

    public override void PerformWorkItem()
    {
      Personnel.CreatedExtended(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
    }
    public override void Postprocess()
    {
      base.Postprocess();
      System.Diagnostics.Debug.Assert(this.Personnel.PersonnelId > 0);
    }
  }

  public class RetrievePersonnelListByFilterText : Personnel_TransactionBaseClass
  {

    public List<b_Personnel> RetPersonnelList { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public override void PerformWorkItem()
    {
      List<b_Personnel> tmpList = null;
      Personnel.RetrievePersonnelListByFilterText(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, dbKey.Client.ClientId, dbKey.User.DefaultSiteId, ref tmpList);
      RetPersonnelList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }

  }

  // SOM-315 - RetrieveForLookupList
  public class Personnel_RetrieveForLookupList : Personnel_TransactionBaseClass
  {

    public List<b_Personnel> PersonnelList { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public override void PerformWorkItem()
    {
      List<b_Personnel> tmpList = null;
      Personnel.RetrieveForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
      PersonnelList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }

  }
    // V2 631 - RetrieveForLookupList
    public class Personnel_RetrieveForLookupListV2 : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveForLookupListV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class Personnel_RetrieveForPlannerLookupList : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveForPlannerLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }



    public class Personnel_CreateInAdminSite : AbstractTransactionManager
  {
    public Personnel_CreateInAdminSite()
    {
      UseDatabase = DatabaseTypeEnum.Client;
    }

    public b_Personnel Personnel { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Personnel.PersonnelId > 0)
      {
        string message = "Personnel has an invalid ID.";
        throw new Exception(message);
      }

      CallerUserInfoId = dbKey.User.UserInfoId;
      CallerUserName = dbKey.UserName;
    }

    public override void PerformWorkItem()
    {
      Personnel.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    }

    public override void Preprocess()
    {

    }

    public override void Postprocess()
    {

    }
  }

  public class RetrievePersonnelListByClientId : Personnel_TransactionBaseClass
  {

    public List<b_Personnel> RetPersonnelList { get; set; }
    public string ClientConnectionString { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (!string.IsNullOrEmpty(ClientConnectionString))
      {
        base.ConnectionString = ClientConnectionString;
      }
    }

    public override void PerformWorkItem()
    {
      List<b_Personnel> tmpList = null;
      Personnel.RetrievePersonnelByClientId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
      RetPersonnelList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }

  }
  // SOM-1228
  public class RetrievePersonnelFromList : Personnel_TransactionBaseClass
  {

    public List<b_Personnel> RetPersonnelList { get; set; }
    public string ClientConnectionString { get; set; }
    public System.Data.DataTable pelist {get;set;}

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (!string.IsNullOrEmpty(ClientConnectionString))
      {
        base.ConnectionString = ClientConnectionString;
      }
    }

    public override void PerformWorkItem()
    {
      List<b_Personnel> tmpList = null;
      Personnel.RetrievePersonnelFromList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,pelist, ref tmpList);
      RetPersonnelList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }

  }


  public class Personnel_ValidateClientLookUpId : Personnel_TransactionBaseClass
  {
    public Personnel_ValidateClientLookUpId()
    {
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    // Result Sets
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

    public override void PerformWorkItem()
    {
      SqlCommand command = null;
      string message = String.Empty;

      try
      {
        List<b_StoredProcValidationError> errors = null;

        Personnel.ValidatePersonnelClientLookUpId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
             ref errors);

        StoredProcValidationErrorList = errors;
      }
      finally
      {
        if (null != command)
        {
          command.Dispose();
          command = null;
        }

        message = String.Empty;
      }
    }

    public override void Preprocess()
    {
      // throw new NotImplementedException();
    }

    public override void Postprocess()
    {
      // throw new NotImplementedException();
    }
  }

  public class Personnel_RetrieveForLookupListBySecurityItem : Personnel_TransactionBaseClass
  {

      public List<b_Personnel> PersonnelList { get; set; }

      public override void PerformLocalValidation()
      {
          base.PerformLocalValidation();
      }

      public override void PerformWorkItem()
      {
          List<b_Personnel> tmpList = null;
          Personnel.RetrieveForLookupListBySecurityItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
          PersonnelList = tmpList;
      }

      public override void Postprocess()
      {
          base.Postprocess();
      }

  }



    // --------SOM - 828-----------------------------------------------------------
    public class Personnel_UpdateForMultiUserSite : Personnel_TransactionBaseClass
  {
      public override void PerformLocalValidation()
      {
          base.PerformLocalValidation();
          //if (Personnel.PersonnelId == 0)
          //{
          //    string message = "Personnel has an invalid ID.";
          //    throw new Exception(message);
          //}
      }

      public override void PerformWorkItem()
      {
          Personnel.UpdateUpdateForMultiUserSite(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
         
      }
  }
  //SOM-642
  public class Personnel_UpdateFromUserInfo : Personnel_TransactionBaseClass
  {
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public override void PerformWorkItem()
    {
      Personnel.UpdateFromUserInfo(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

    }
  }
    public class Personnel_UpdateFromUserInfoAdmin : Personnel_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            Personnel.UpdateFromUserInfoAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class Personnel_RetrieveAll_V2 : AbstractTransactionManager
    {

        public Personnel_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Personnel> PersonnelList { get; set; }

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
            b_Personnel[] tmpArray = null;
            b_Personnel o = new b_Personnel();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PersonnelList = new List<b_Personnel>(tmpArray);
        }
    }

    public class Personnel_RetrieveForMention : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelMentionList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;

            Personnel.RetrieveForMention(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PersonnelMentionList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }


    public class Personnel_RetrieveByForeignKeys_V2 : Personnel_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Personnel.PersonnelId == 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Personnel.RetrieveByForeignKeysFromDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class Personnel_RetrieveEventsByPersonnelId : Personnel_TransactionBaseClass
    {
        public List<b_Personnel> PersonnelList { get; set; }

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
            List<b_Personnel> tmpArray = null;

            Personnel.RetrieveEventsByPersonnelIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PersonnelList = new List<b_Personnel>();
            foreach (b_Personnel tmpObj in tmpArray)
            {
                PersonnelList.Add(tmpObj);
            }
        }
    }
    public class Personnel_RetrieveV2ChunkSearch : Personnel_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_Personnel tmpList = null;
            Personnel.RetrieveChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class Personnel_RetrieveLaborByPersonnelId : Personnel_TransactionBaseClass
    {
        public List<b_Personnel> PLList { get; set; }

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
            List<b_Personnel> tmpArray = null;

            Personnel.RetrieveLaborByPersonnelIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PLList = new List<b_Personnel>();
            foreach (b_Personnel tmpObj in tmpArray)
            {
                PLList.Add(tmpObj);
            }
        }
    }

    public class Personnel_RetrievePersonnelAvailabilityByPersonnelId : Personnel_TransactionBaseClass
    {
        public List<b_Personnel> PAList { get; set; }

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
            List<b_Personnel> tmpArray = null;

            Personnel.RetrievePersonnelAvailabilityByPersonnelIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PAList = new List<b_Personnel>();
            foreach (b_Personnel tmpObj in tmpArray)
            {
                PAList.Add(tmpObj);
            }
        }
    }


    public class Personnel_RetrievePersonnelAttendanceByPersonnelId : Personnel_TransactionBaseClass
    {
        public List<b_Personnel> PersonnelAttendList { get; set; }

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
            List<b_Personnel> tmpArray = null;

            Personnel.RetrievePersonnelAttendanceByPersonnelIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            PersonnelAttendList = new List<b_Personnel>();
            foreach (b_Personnel tmpObj in tmpArray)
            {
                PersonnelAttendList.Add(tmpObj);
            }
        }
    }
    public class Personnel_RetrievePersonnelByPersonnelId_V2 : Personnel_TransactionBaseClass
    {
        //public List<b_Personnel> PersonnelList { get; set; }
      
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
            b_Personnel tmpobj = null;
            Personnel.RetrievePersonnelByPersonnel_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            //   objPersonnel = tmpobj;
            Personnel = tmpobj;

        }
    }

    #region Retrieve all personnel for Active and full user
    public class Personnel_RetrieveAllPersonnelForActiveandFullUser : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveAllForActiveandFullUsersPersonnel(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region Retrieve for WorkOrder completion wizard tab
    public class Personnel_RetrieveForWorkOrderCompletionWizard : Personnel_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Personnel.PersonnelId == 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            b_Personnel tmpobj = null;
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Personnel.RetrieveByPersonnelIdForWorkOrderCompletionWizard_V2(Connection, Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            Personnel = tmpobj;
        }

    }
    #endregion

    //V2-720***
    #region Personnel LookupList chunk search For ApprovalGroup
    public class Personnel_LookupListRetrieveForChunkSearch_V2 : Personnel_TransactionBaseClass
    {
        public List<b_Personnel> PersonnelList { get; set; }
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;
            base.PerformLocalValidation();
            //if (Personnel.PersonnelId == 0)
            //{
            //    string message = "Personnel has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpobj = null;
            Personnel.RetrieveChunkSearchLookupListForApprovalGroup_V2(Connection, Transaction, CallerUserInfoId, CallerUserName, ref tmpobj);
            PersonnelList = tmpobj;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
  #endregion
    #region V2-989
    public class Personnel_RetrievePartManagementForLookupList : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrievePartManagementForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion
    #region V2-798
    public class Personnel_RetrieveAllActiveForLookupList : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveAllActiveForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region V2-806
    public class Personnel_RetrieveAllActiveForLookupListForAdmin : AbstractTransactionManager
    {
        public Personnel_RetrieveAllActiveForLookupListForAdmin()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_Personnel Personnel { get; set; }
        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel==null)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveAllActiveForLookupListForAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
    public class Personnel_RetrieveByForeignKeys_V2ForAdmin : AbstractTransactionManager
    {
        public Personnel_RetrieveByForeignKeys_V2ForAdmin()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public b_Personnel Personnel { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId == 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformWorkItem()
        {
            Personnel.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Preprocess()
        {

        }

        public override void Postprocess()
        {

        }
    }
    #endregion

    #region V2-820
    public class Personnel_RetrieveForLookupListByMultipleSecurityItem : Personnel_TransactionBaseClass
    {
        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveForLookupListByMultipleSecurityItem(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Personnel, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion

    #region V2-929
    public class Personnel_RetrieveLookupListActiveAdminOrFullUser : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId < 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveLookupListActiveAdminOrFullUser(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-950
    public class Personnel_RetrieveLookupListActiveAdminOrFullUserPlanner : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId < 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveLookupListActiveAdminOrFullUserPlanner(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-712
    public class Personnel_RetrievePMAssignPersonneLookupList : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId < 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrievePMAssignPersonnelLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-981
    public class Personnel_RetrieveLookupListActive : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId < 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrievePersonnelLookupListActive(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion
    #region V2-962
  
    public class Personnel_DeleteForAdmin : Personnel_TransactionBaseClass
    {
        public long customClientid { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId == 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Personnel.ClientId = customClientid;
            Personnel.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Personnel_CreateforAdmin : Personnel_TransactionBaseClass
    {
        public long customClientid { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId > 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Personnel.ClientId = customClientid;
            Personnel.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Personnel.PersonnelId > 0);
        }
    }
    public class Personnel_RetrieveByUserInfoIdForAdminUserManagementChildGrid_V2 : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            var clientid = Personnel.ClientId;
            base.PerformLocalValidation();
            
            if (Personnel.PersonnelId < 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
            Personnel.ClientId = clientid;
        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveForAdminUserManagementChildGrid(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-1178

    public class Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2 : Personnel_TransactionBaseClass
    {

        public List<b_Personnel> PersonnelList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Personnel.PersonnelId < 0)
            {
                string message = "Personnel has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Personnel> tmpList = null;
            Personnel.RetrieveChunkSearchForActiveAdminOrFullUser_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            PersonnelList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class Personnel_RetrievePersonnelIdByClientLookupId : Personnel_TransactionBaseClass
    {

        public b_Personnel PersonnelResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (string.IsNullOrEmpty(Personnel.ClientLookupId))
            {
                string message = "Personnel has an Client Lookup ID.";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Personnel tmpList = null;
            Personnel.RetrievePersonnelIdByClientLookupIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            PersonnelResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
        #endregion
    }
}

