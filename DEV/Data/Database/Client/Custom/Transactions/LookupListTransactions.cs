/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* NOTES
* 2014-Nov-23 - This method should NOT be needed - Lookup lists should not have to be retrieved 
*               by callbacks as they should be retrieved completely the first time  
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =========================================================
* 2014-Nov-23 SOM-453   Roger Lawton       Added class LookupList_RetrieveAllActive
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Common.Enumerations;
using Database;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class LookupList_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public LookupList_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ListValue { get; set; }
        public string ListName { get; set; }
        public string Filter { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_LookupList> LookupListList { get; set; }
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

                LookupListList = usp_LookupList_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ListName, ListValue, Filter, Description, SiteId,
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


    public class RetrieveLookupList : AbstractTransactionManager
    {
        public RetrieveLookupList()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public string listname { get; set; }
        public string txtSearch { get; set; }
        public string listfilter { get; set; }
        public long ParentSiteID { get; set; }
        public List<b_LookupList> result;
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

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
                result = usp_LookupList_RetreiveList.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, listname, listfilter, ParentSiteID, txtSearch);
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

    public class RetrieveLookupListByFilterText :LookupList_TransactionBaseClass
    {
              
        public List<KeyValuePair<string,string>> RetLookUpList{get;set;}     

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {       
            List<KeyValuePair<string,string>> tmpList = null;
            LookupList.RetrieveLookupListByFilterText(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,dbKey.Client.ClientId, ref tmpList);
            RetLookUpList = tmpList;        
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class LookupList_CreateTemplate : LookupList_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LookupList.LookupListId > 0)
            {
                string message = "LookupList has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            LookupList.CreateTemplate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    // SOM-453 - 
    public class LookupList_RetrieveAllActive : AbstractTransactionManager
    {

      public LookupList_RetrieveAllActive()
      {
        // Set the database in which this table resides.
        // This must be called prior to base.PerformLocalValidation(), 
        // since that process will validate that the appropriate 
        // connection string is set.
        UseDatabase = DatabaseTypeEnum.Client;
      }


      public List<b_LookupList> LookupListList { get; set; }

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
        b_LookupList[] tmpArray = null;
        b_LookupList o = new b_LookupList();


        // Explicitly set tenant id from dbkey
        o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAllActiveFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

        LookupListList = new List<b_LookupList>(tmpArray);
      }
    }

    public class LookUpList_GetLookUpListByListName : AbstractTransactionManager
    {
        public string ListName { get; set; }
        public LookUpList_GetLookUpListByListName()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_LookupList> LookupListList { get; set; }

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
            b_LookupList[] tmpArray = null;
            b_LookupList o = new b_LookupList();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.GetLookUpListByListName(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ListName, ref tmpArray);

            LookupListList = new List<b_LookupList>(tmpArray);
        }
    }

    public class LookupList_DeleteByLookupListId_V2 : LookupList_TransactionBaseClass
    {
        public int Count { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }
        public override void PerformWorkItem()
        {
            Count = LookupList.DeleteByLookupListId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}
