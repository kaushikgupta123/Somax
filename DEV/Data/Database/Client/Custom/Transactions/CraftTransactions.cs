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
* =========== ======== ================== ==========================================================
* 2014-Oct-02 SOM-354  Roger Lawton       Changed class name from Craft_RetrieveBySiteName to 
*                                         Class RetrieveForSite
*                                         Added/Revises Validate Methods
* 2015-Nov-05 SOM-844  Roger Lawton       Added Craft_DeleteInactivate Transaction
*                                         Cleaned Up
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class Craft_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public Craft_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_Craft> CraftList { get; set; }
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

                CraftList = usp_Craft_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Description, SiteId,
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

    public class Craft_RetrieveForSite : Craft_TransactionBaseClass
    {

        public Craft_RetrieveForSite()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Craft> CraftList { get; set; }

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
            b_Craft[] tmpArray = null;
            b_Craft o = new b_Craft();
            o.ClientId = dbKey.Client.ClientId;
            o.SiteId = dbKey.User.DefaultSiteId;
            o.RetrieveForSite(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            CraftList = new List<b_Craft>(tmpArray);
        }
    }

    // SOM-354
    public class Craft_ValidateInsert : Craft_TransactionBaseClass
    {
        public Craft_ValidateInsert()
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
            List<b_StoredProcValidationError> errors = null;

            Craft.ValidateInsert(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

            StoredProcValidationErrorList = errors;
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

    public class Craft_ValidateSave : Craft_TransactionBaseClass
    {
        public Craft_ValidateSave()
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
            List<b_StoredProcValidationError> errors = null;

            Craft.ValidateSave(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

            StoredProcValidationErrorList = errors;
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
    public class Craft_DeleteInactivate : Craft_TransactionBaseClass
    {
        public Craft_DeleteInactivate()
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
            List<b_StoredProcValidationError> errors = null;

            Craft.DeleteInactivate(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

            StoredProcValidationErrorList = errors;
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

    #region V2-962
    public class Craft_RetrieveForSiteForAdmin : Craft_TransactionBaseClass
    {

        public Craft_RetrieveForSiteForAdmin()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_Craft> CraftList { get; set; }
        public long SiteId { get; set; }
        public long CustomClientId { get; set; }
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
            b_Craft[] tmpArray = null;
            b_Craft o = new b_Craft();
            o.ClientId =this.CustomClientId;
            o.SiteId =this.SiteId;
            o.RetrieveForSite(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            CraftList = new List<b_Craft>(tmpArray);
        }
    }
    #endregion


}
