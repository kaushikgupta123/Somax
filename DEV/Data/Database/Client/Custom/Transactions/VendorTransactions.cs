/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Aug-29 SOM-304  Roger Lawton       Added trans cladd  to load and process for lookup
* 2014-Sep-15 SOM-106  Roger Lawton       Added trans to retrieve for search purposes
*                                         Removed Vendor_RetrieveAllBySiteID - not used 
****************************************************************************************************
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
  public class Vendor_RetrieveBySearchCriteria : AbstractTransactionManager
  {
    public Vendor_RetrieveBySearchCriteria()
    {
      UseDatabase = DatabaseTypeEnum.Client;
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public string Query { get; set; }
    public string Area { get; set; }
    public string Site { get; set; }
    public string Department { get; set; }
    public string Type { get; set; }

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
    public List<b_Vendor> VendorList { get; set; }
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

        VendorList = usp_Vendor_RetrieveBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department, Type,
            DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

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

  public class Vendor_RetrieveForSearch : Vendor_TransactionBaseClass
  {

    public List<b_Vendor> VendorList { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Vendor.VendorId > 0)
      {
        string message = "Vendor has an invalid ID.";
        throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      
      b_Vendor[] tmpList = null;

      Vendor.RetrieveForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
      
      VendorList = new List<b_Vendor>(tmpList);

    }

    public override void Postprocess()
    {
      base.Postprocess();
    }
  }

  public class Vendor_RetrieveClientLookupIdBySearchCriteria : Vendor_TransactionBaseClass
  {

    public List<b_Vendor> VendorList { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Vendor.VendorId > 0)
      {
        string message = "Vendor has an invalid ID.";
        throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      List<b_Vendor> tmpList = null;
      Vendor.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

      VendorList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }
  }
  public class Vendor_RetrieveForLookupList : Vendor_TransactionBaseClass
  {

      public List<b_Vendor> VendorList { get; set; }

      public override void PerformLocalValidation()
      {
          base.PerformLocalValidation();
      }

      public override void PerformWorkItem()
      {
          List<b_Vendor> tmpList = null;
          Vendor.RetrieveForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Vendor, ref tmpList);
          VendorList = tmpList;
      }

      public override void Postprocess()
      {
          base.Postprocess();
      }

  }

  public class Vendor_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
  {
    public Vendor_RetrieveInitialSearchConfigurationData()
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
        SearchCriteria = usp_Vendor_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
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

    public class Vendor_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Vendor_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Vendor> VendorList { get; set; }
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
                    Transaction = this.Transaction,
                    CommandTimeout = 180              // SOM-1501

                };

                int tmp;

                VendorList = usp_Vendor_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, SiteId,
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

    public class Vendor_RetrieveLookupListBySearchCriteriaAndInternal_V2 : AbstractTransactionManager
    {
        public Vendor_RetrieveLookupListBySearchCriteriaAndInternal_V2()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Vendor> VendorList { get; set; }
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
                    Transaction = this.Transaction,
                    CommandTimeout = 180              // SOM-1501

                };

                int tmp;

                VendorList = usp_Vendor_RetrieveLookupListBySearchCriteriaAndInternal_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, SiteId,
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

    /****************************Added By Indusnet Technologies******************************/

    public class RetrieveVendorListByFilterText : Vendor_TransactionBaseClass
  {

    public List<b_Vendor> RetVendorList { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public override void PerformWorkItem()
    {
      List<b_Vendor> tmpList = null;
      Vendor.RetrieveVendorByFilterText(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, dbKey.Client.ClientId, ref tmpList);
      RetVendorList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }

  }


  public class Vendor_RetrieveForLookup : Vendor_TransactionBaseClass
  {
    public Vendor_RetrieveForLookup()
    {
      base.UseDatabase = DatabaseTypeEnum.Client;
    }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    //public string PersonnelId { get; set; }
    //public string ClientLookupId { get; set; }
    //public string Name { get; set; }
    //public long SiteId { get; set; }
    //public long clientId { get; set; }


    // Result Sets
    public List<b_Vendor> VendorList { get; set; }

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

        b_Vendor[] tmpArray = null;

        this.Vendor.RetrieveForLookup(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

        VendorList = new List<b_Vendor>(tmpArray);


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

  public class ProcedureVendorCreateValidationTransaction : Vendor_TransactionBaseClass
  {
    public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
      if (Vendor.VendorId > 0)
      {
        string message = "Vendor has an invalid ID.";
        //throw new Exception(message);
      }
    }
    public override void PerformWorkItem()
    {
      List<b_StoredProcValidationError> errors = null;
      Vendor.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
      //Vendor.ValidateByClientLookupId(this.Connection,this.Transaction,CallerUserInfoId,CallerUserName);
      StoredProcValidationErrorList = errors;
    }

    public override void Postprocess()
    {

    }
  }


    public class ValidateByInactivateorActivate : Vendor_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Vendor.ValidateByInactivateorActivate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);           
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }
    public class VendorNotExistValidationTransaction : Vendor_TransactionBaseClass
  {
      public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
      public override void PerformLocalValidation()
      {
          base.PerformLocalValidation();
          if (Vendor.VendorId < 0)
          {
              string message = "Vendor has an invalid ID.";
              throw new Exception(message);
          }
      }
      public override void PerformWorkItem()
      {
          List<b_StoredProcValidationError> errors = null;
          Vendor.ValidateByClientLookupIdSiteId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
          //Vendor.ValidateByClientLookupId(this.Connection,this.Transaction,CallerUserInfoId,CallerUserName);
          StoredProcValidationErrorList = errors;
      }

      public override void Postprocess()
      {

      }
  }

  public class ValidateClientLookupIdTransaction : Vendor_TransactionBaseClass
  {
      public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
      public override void PerformLocalValidation()
      {
          base.PerformLocalValidation();
          
      }
      public override void PerformWorkItem()
      {
          List<b_StoredProcValidationError> errors = null;
          Vendor.ValidateClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
          StoredProcValidationErrorList = errors;
      }

      public override void Postprocess()
      {

      }
  }

  //-----SOM-788-----//
  //public class Vendor_RetrieveCreateModifyDate : Vendor_TransactionBaseClass
  //{

  //    public override void PerformLocalValidation()
  //    {
  //        base.UseTransaction = false;    // moved from PerformWorkItem
  //        base.PerformLocalValidation();
  //        if (Vendor.VendorId == 0)
  //        {
  //            string message = "Vendor has an invalid ID.";
  //            throw new Exception(message);
  //        }
  //    }

  //    public override void PerformWorkItem()
  //    {
  //        //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
  //        Vendor.RetrieveCreateModifyDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
  //    }
  //}
  /*************************************End************************************************/
//-------------------------Calls- From---API SOM--918---------------------------------------
  public class Vendor_RetrieveBySiteIdAndClientLookUpId : Vendor_TransactionBaseClass
  {

      public List<b_Vendor> VendorList { get; set; }
      public override void PerformLocalValidation()
      {
          base.PerformLocalValidation();
          if (Vendor.VendorId > 0)
          {
              string message = "Vendor has an invalid ID.";
              throw new Exception(message);
          }
      }
      public override void PerformWorkItem()
      {
          List<b_Vendor> tmpList = null;

          Vendor.RetrieveBySiteIdAndClientLookUpId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

          VendorList = tmpList;
      }

      public override void Postprocess()
      {
          base.Postprocess();
      }
  }
    //public class Vendor_RetrieveByVMId : Vendor_TransactionBaseClass
    //{

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //        if (Vendor.VendorMasterId == 0)
    //        {
    //            string message = "VendorMasterId has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        base.UseTransaction = false;
    //        Vendor.RetrieveByVMId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //    }
    //}

    public class Vendor_RetrieveAll_V2 : AbstractTransactionManager
    {

        public Vendor_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Vendor> VendorList { get; set; }

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
            b_Vendor[] tmpArray = null;
            b_Vendor o = new b_Vendor();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;

            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            VendorList = new List<b_Vendor>(tmpArray);
        }
    }

    public class Vendor_RetrieveChunkSearch : Vendor_TransactionBaseClass
    {

        public Vendor_RetrieveChunkSearch()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public List<b_Vendor> VendorList { get; set; }

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
            List<b_Vendor> tmpArray = null;
            
            // Explicitly set tenant id from dbkey
           Vendor.ClientId = this.dbKey.Client.ClientId;
            Vendor.SiteId = this.dbKey.User.DefaultSiteId;
            Vendor.RetrieveChunkSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            VendorList = new List<b_Vendor>(tmpArray);
            //VendorList=tmpArray;
        }
    }

    public class Vendor_ChangeClientLookupId : Vendor_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Vendor.VendorId == 0)
            {
                string message = "Vendor has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Vendor.ChangeClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class ValidateClientIdVendorAndMaster : Vendor_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Vendor.ValidateClientIdVendorAndMaster(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }


    public class PunchOutVendor_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public PunchOutVendor_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Vendor> VendorList { get; set; }
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
                    Transaction = this.Transaction,
                    CommandTimeout = 180              // SOM-1501

                };

                int tmp;

                VendorList = usp_PunchOutVendor_RetrieveLookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, AddressCity, AddressState, SiteId,
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

    #region Vendor Lookuplist chunk search
    public class Vendor_RetrieveChunkSearchLookupListV2 : Vendor_TransactionBaseClass
    {
        public List<b_Vendor> VendorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Vendor.VendorId > 0)
            {
                string message = "Vendor has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_Vendor> tmpList = null;
            Vendor.RetrieveVendorLookuplistChunkSearchV2(this.Connection, this.Transaction,
                CallerUserInfoId, CallerUserName, ref tmpList);
            VendorList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion


    #region V2
    public class Vendor_RetrieveLookupListBySearchCriteria_V2 : AbstractTransactionManager
    {
        public Vendor_RetrieveLookupListBySearchCriteria_V2()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Vendor> VendorList { get; set; }
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
                    Transaction = this.Transaction,
                    CommandTimeout = 180              // SOM-1501

                };

                int tmp;

                VendorList = usp_Vendor_RetrieveLookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, SiteId,
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
    #endregion
}
