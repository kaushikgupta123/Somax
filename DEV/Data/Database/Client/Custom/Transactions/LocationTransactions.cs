/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ===========================================================
* 2016-Aug-21 SOM-1049 Roger Lawton     Changed to use similar data retrieval functionality as
*                                       other pages 
***************************************************************************************************
 */

using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Database
{
    public class Location_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public Location_RetrieveBySearchCriteria()
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
        public List<b_Location> LocationList { get; set; }
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

                LocationList = usp_Location_RetrieveBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department, Type,
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

    public class Location_RetrieveAllBySiteId : AbstractTransactionManager
    {
        public Location_RetrieveAllBySiteId()
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
        public List<b_Location> LocationList { get; set; }
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

                LocationList = usp_Location_RetrieveAllBySiteId.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department, Type,
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


    public class Location_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Location_RetrieveLookupListBySearchCriteria()
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
        public List<b_Location> LocationList { get; set; }
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

                LocationList = usp_Location_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, SiteId,
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


    public class Location_RetrieveClientLookupIdBySearchCriteria : Location_TransactionBaseClass
    {
        public List<b_Location> LocationList { get; set; }

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
            List<b_Location> tmpList = null;

            Location.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            LocationList = new List<b_Location>();
            foreach (b_Location tmpObj in tmpList)
            {
                LocationList.Add(tmpObj);
            }
        }
    }

    public class LocationValidationByClientLookUpId : Location_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (ProcedureMaster.ProcedureMasterId > 0)
            //{
            //    string message = "ProcedureMaster has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            Location.ValidateByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    //public class Location_RetrieveAllClientLookupId : Location_TransactionBaseClass
    //{
    //    public List<b_Location> LocationList { get; set; }

    //    public override void Preprocess()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public override void Postprocess()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        base.UseTransaction = false;
    //        List<b_Location> tmpList = null;

    //        Location.RetrieveAllClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

    //        LocationList = new List<b_Location>();
    //        foreach (b_Location tmpObj in tmpList)
    //        {
    //            LocationList.Add(tmpObj);
    //        }
    //    }
    //}
    public class Location_RetrieveForSearch : Location_TransactionBaseClass
    {
      public List<b_Location> LocationList { get; set; }

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
        b_Location[] tmpList = null;

        Location.RetrieveForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

        LocationList = new List<b_Location>(tmpList);
      }
    }

    public class Location_RetrieveAll_V2 : AbstractTransactionManager
    {

        public Location_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Location> LocationList { get; set; }

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
            b_Location[] tmpArray = null;
            b_Location o = new b_Location();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            LocationList = new List<b_Location>(tmpArray);
        }
    }

    #region V2
    public class Location_RetrieveClientLookupIdBySearchCriteria_V2 : Location_TransactionBaseClass
    {
        public long SiteId { get; set; }
        public List<b_Location> LocationList { get; set; }

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
            List<b_Location> tmpList = null;
            Location.SiteId = SiteId;
            Location.RetrieveClientLookupIdBySearchCriteriaFromDatabaseV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            LocationList = new List<b_Location>();
            foreach (b_Location tmpObj in tmpList)
            {
                LocationList.Add(tmpObj);
            }
        }
    }
    #region V2

    public class Location_RetrieveLookupListBySearchCriteria_V2 : AbstractTransactionManager
    {
        public Location_RetrieveLookupListBySearchCriteria_V2()
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
        public List<b_Location> LocationList { get; set; }
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

                LocationList = usp_Location_RetrieveLookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Name, SiteId,
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
        
   
    #endregion
}
