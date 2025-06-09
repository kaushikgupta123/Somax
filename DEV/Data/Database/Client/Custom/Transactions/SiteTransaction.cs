/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person            Description
* =========== ======== ================== ========================================================
* 2014-Aug-11 SOM-282  Roger Lawton       Modify the validation logic 
* 2014-Oct-21 SOM-384  Roger Lawton       Additional Parameters in ValidateNewUserAdd and 
* 2015-Nov-06 SOM-851  Roger Lawton       Support Multi-Site - Get list of authorized sites for a 
*                                         particular user (AuthorizedUser) 
***************************************************************************************************
*/
using System;
using System.Collections.Generic;
using Database.Business;
using System.Data.SqlClient;
using Common.Enumerations;
using Data.Database;

namespace Database.Transactions
{
  public class Site_RetrieveAuthorizedForUser : Site_TransactionBaseClass
  {

    public List<b_Site> SiteList { get; set; }

    public override void PerformLocalValidation()
    {
      base.PerformLocalValidation();
    }

    public override void PerformWorkItem()
    {
      List<b_Site> tmpList = null;
      Site.RetrieveAuthorizedForUser(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Site, ref tmpList);
      SiteList = tmpList;
    }

    public override void Postprocess()
    {
      base.Postprocess();
    }

  }
  public class Site_RetrieveAllFromAdmin : AbstractTransactionManager
    {
        public Site_RetrieveAllFromAdmin()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public new string ConnectionString { get; set; }
        public long SearchClientId { get; set; }
  
        public List<b_Site> SiteList { get; set; }

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
            base.m_ConnectionString = ConnectionString;
        }

        public override void PerformWorkItem()
        {
            b_Site[] tmpArray = null;
            b_Site o = new b_Site();
            o.ClientId = SearchClientId;
            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>(tmpArray);
        }
    }

    public class Site_RetrieveBySearchFromAdmin : AbstractTransactionManager
    {
        public Site_RetrieveBySearchFromAdmin()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public new string ConnectionString { get; set; }
        public long SearchClientId { get; set; }

        public List<b_Site> SiteList { get; set; }
        public b_Site Site { get; set; }

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
            base.m_ConnectionString = ConnectionString;
        }

        public override void PerformWorkItem()
        {
            List<b_Site> tmpArray = null;
            //b_Site o = new b_Site();
            Site.ClientId = SearchClientId;
            Site.RetrieveBySearchFromAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>(tmpArray);
        }
    }

    public class Site_CreateFromAdmin : Site_TransactionBaseClass
    {
        public new string ConnectionString { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId > 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }

            base.m_ConnectionString = ConnectionString;
        }
        public override void PerformWorkItem()
        {
            Site.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Site.SiteId > 0);
        }
    }

    public class Site_UpdateFromAdmin : Site_TransactionBaseClass
    {
        public new string ConnectionString { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
            base.m_ConnectionString = ConnectionString;
        }

        public override void PerformWorkItem()
        {
            Site.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class Site_RetrieveFromAdmin : Site_TransactionBaseClass
    {
        public new string ConnectionString { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
            base.m_ConnectionString = ConnectionString;
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Site.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Site_ValidateNewUserAdd : Site_TransactionBaseClass
    {
        public Site_ValidateNewUserAdd()
        {
        }

        public bool IsSuperUser { get; set; }
        public string UserType { get; set; }

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

                Site.ValidateNewUserAddFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
                     IsSuperUser,UserType,ref errors);

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

    public class Site_ValidateUserUpdate : Site_TransactionBaseClass
    {
        public Site_ValidateUserUpdate()
        {
        }


        public string UserType { get; set; }
        public bool UserIsActive { get; set; }
        public bool IsSuperUser { get; set; }

        public string CurrentUserType { get; set; }
        public bool CurrentUserIsActive { get; set; }
        public bool CurrentIsSuperUser { get; set; }

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

                Site.ValidateUserUpdateFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName
                      ,UserType, UserIsActive,IsSuperUser,CurrentUserType, CurrentUserIsActive, CurrentIsSuperUser,ref errors);

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

    //-------SOM-899----------------------------------------
    public class Site_ValidateByProcessSystemId : Site_TransactionBaseClass
    {
        public Site_ValidateByProcessSystemId()
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

                Site.ValidateByProcessSystemId(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);
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

    
    public class Site_RetrieveAssetGroupNameV2 : AbstractTransactionManager
    {

        public Site_RetrieveAssetGroupNameV2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Site> SiteList { get; set; }

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
            b_Site[] tmpArray = null;
            b_Site o = new b_Site();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAssetGroupName(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>(tmpArray);
        }
    }

    #region V2-419 Enterprise User Management - Add/Remove Sites
   

    public class Site_RetrieveAllAssignedSiteByUser : Site_TransactionBaseClass
    {

        public List<b_Site> SiteList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Site> tmpList = null;
            Site.RetrieveAllAssignedSiteByUser(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Site, ref tmpList);
            SiteList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class Site_RetrieveDefaultBuyer : Site_TransactionBaseClass
    {
        public List<b_Site> SiteList { get; set; }

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
            List<b_Site> tmpArray = null;

            Site.RetriveDefaultBuyer(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>();
            foreach (b_Site tmpObj in tmpArray)
            {
                SiteList.Add(tmpObj);
            }
        }
    }
    #endregion

    #region V2-435 Admin Site

    public class Admin_RetrieveSiteChunkSearchV2 : Site_TransactionBaseClass
    {
        public  void PerformCustomValidation()
        {
           
            if (Site == null)
            {
                string message = "Site has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;


            // Explicitly set tenant id from dbkey
           

        }

        public override void PerformLocalValidation()
        {
            // PerformCustomValidation();

            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Site tmpList = null;
            Site.RetrieveSiteChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

   
    }

    public class Site_RetrieveAllBySiteId_V2 : Site_TransactionBaseClass
    {

        public Site_RetrieveAllBySiteId_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Site> SiteList { get; set; }

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
            b_Site tmpArray = null;
           // b_Site o = new b_Site();


            // Explicitly set tenant id from dbkey
            //o.ClientId = this.dbKey.Client.ClientId;
            //o.SiteId = this.dbKey.User.DefaultSiteId;

           Site.RetrieveAllSiteBySiteId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            this.Site = tmpArray;
            //SiteList = new List<b_Site>(tmpArray);
        }
    }

    public class Site_RetrieveCreateModifyDate : Site_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            Site.RetrieveCreateModifyDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    #endregion

    #region V2-806
    //public class Site_RetrieveClientIdForLookupList : Site_TransactionBaseClass
    //{

    //    public List<b_Site> SiteList { get; set; }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        List<b_Site> tmpList = null;
    //        Site.RetrieveByClientIdForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Site, ref tmpList);
    //        SiteList = tmpList;
    //    }

    //    public override void Postprocess()
    //    {
    //        base.Postprocess();
    //    }

    //}
    public class Site_RetrieveByClientIdForLookupList : AbstractTransactionManager
    {
        public Site_RetrieveByClientIdForLookupList()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public new string ConnectionString { get; set; }

        public List<b_Site> SiteList { get; set; }
        public b_Site Site { get; set; }

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
            base.m_ConnectionString = ConnectionString;
        }

        public override void PerformWorkItem()
        {
            List<b_Site> tmpArray = null;
            //Site.RetrieveByClientIdForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Site, ref tmpArray);
            Site.RetrieveByClientIdForLookupList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>(tmpArray);
        }
    }
    #endregion

    #region V2-964
    public class Site_RetrieveForAllActiveLookupListV2 : Site_TransactionBaseClass
    {

        public List<b_Site> SiteList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Site> tmpList = null;
            Site.RetrieveForAllActiveLookupListV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Site, ref tmpList);
            SiteList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class RetrieveSitesForClientChildGrid : Site_TransactionBaseClass
    {
        public void PerformCustomValidation()
        {

            if (Site == null)
            {
                string message = "Site has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformLocalValidation()
        {
            // PerformCustomValidation();

            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_Site tmpList = null;
            Site.RetrieveSiteChildGridForAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }


    }
    public class Site_CreateFromAdminClient : Site_TransactionBaseClass
    {
        public new string ConnectionString { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId > 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            Site.InsertIntoDatabaseFromClientBySomaxAdminV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(Site.SiteId > 0);
        }
    }

    public class RetrieveSiteByClientIdSiteId_V2 : Site_TransactionBaseClass
    {
        public void PerformCustomValidation()
        {

            if (Site == null)
            {
                string message = "Site has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public override void PerformLocalValidation()
        {
            // PerformCustomValidation();

            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Site.RetrieveSiteByClientIdSiteId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }


    }
    public class Site_RetrieveSiteByClientIdSiteId : Site_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
            Site.ClientId = Site.CustomClientId;
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Site.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class Site_RetrieveAllSitesFromAdmin : AbstractTransactionManager
    {
        public Site_RetrieveAllSitesFromAdmin()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public long SearchClientId { get; set; }

        public List<b_Site> SiteList { get; set; }

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
            b_Site[] tmpArray = null;
            b_Site o = new b_Site();
            o.ClientId = SearchClientId;
            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>(tmpArray);
        }
    }
    public class Site_UpdateFromAdmin_V2 : Site_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Site.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion
    #region V2-536
    public class Site_UpdateIoTDeviceCount : Site_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            Site.UpdateIoTDeviceCount(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-962
    public class Site_RetrieveDefaultBuyerForAdmin : Site_TransactionBaseClass
    {
        public List<b_Site> SiteList { get; set; }
        public long customClientId { get; set; }
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
            List<b_Site> tmpArray = null;
            Site.ClientId = customClientId;
            Site.RetriveDefaultBuyer(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>();
            foreach (b_Site tmpObj in tmpArray)
            {
                SiteList.Add(tmpObj);
            }
        }
    }
    public class Site_UpdateforAdmin : Site_TransactionBaseClass
    {
        public long customClientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            Site.ClientId = customClientId;
            Site.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class Site_RetrieveForAdmin : Site_TransactionBaseClass
    {
        public long customclientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (Site.SiteId == 0)
            {
                string message = "Site has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            Site.ClientId = customclientId;
            Site.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class Site_RetrieveAllAssignedSiteByUserForAdmin : Site_TransactionBaseClass
    {

        public List<b_Site> SiteList { get; set; }
        public long customclientId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_Site> tmpList = null;
            Site.ClientId = customclientId;
            Site.RetrieveAllAssignedSiteByUser(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.Site, ref tmpList);
            SiteList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class Site_RetrieveLookupListForAdmin_V2 : AbstractTransactionManager
    {
        public Site_RetrieveLookupListForAdmin_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public long SearchClientId { get; set; }
        b_Site Site = new b_Site();
        public List<b_Site> SiteList { get; set; }

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
            
            Site.ClientId = SearchClientId;
            List<b_Site> tmpArray = null;
            Site.RetrieveSiteForLookupListAdmin_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            SiteList = new List<b_Site>(tmpArray);
        }
    }
    #endregion
}
