/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person           Description
* ===========  ========= ================ ========================================================
* 2014-Oct-27  SOM-384   Roger Lawton     Added SecurityProfile_RetrieveByName class
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;
using System.Data.SqlClient;

namespace Database
{
    public class SecurityProfile_RetrieveAllProfiles : AbstractTransactionManager
    {

        public SecurityProfile_RetrieveAllProfiles()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_SecurityProfile> SecurityProfileList { get; set; }
        public long AccessClientId { get; set; }

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
            b_SecurityProfile[] tmpArray = null;
            b_SecurityProfile o = new b_SecurityProfile();


            // Explicitly set tenant id from dbkey
            AccessClientId = AccessClientId < 0 ? this.dbKey.Client.ClientId : AccessClientId;


            o.RetrieveAllProfiles(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,AccessClientId,ref tmpArray);

            SecurityProfileList = new List<b_SecurityProfile>(tmpArray);
        }
    }
   

    public class SecurityProfile_RetrieveAllProfilesforEnterprise : SecurityProfile_TransactionBaseClass
    {
        public List<b_SecurityProfile> SecurityProfileList = new List<b_SecurityProfile>();
        public long AccessClientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_SecurityProfile> temp = new List<b_SecurityProfile>();
            AccessClientId = AccessClientId < 0 ? this.dbKey.Client.ClientId : AccessClientId;
            SecurityProfile.RetrieveAllProfilesforEnterprise(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, AccessClientId, ref temp);
            SecurityProfileList = temp;
        }
    }

    public class SecurityProfile_ValidateSecurityProfileName : SecurityProfile_TransactionBaseClass
    {
        public SecurityProfile_ValidateSecurityProfileName()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();         
       }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public long AccessClientId { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                AccessClientId = AccessClientId < 0 ? dbKey.Client.ClientId : AccessClientId;

                SecurityProfile.ValidateSecurityProfileName(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,AccessClientId,
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

    public class SecurityProfile_CreateClone : SecurityProfile_TransactionBaseClass
    {

        public long AccessClientId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();           
        }
        public override void PerformWorkItem()
        {
            AccessClientId = AccessClientId < 0 ? dbKey.Client.ClientId : AccessClientId;
            SecurityProfile.CreateClone(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,AccessClientId);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(SecurityProfile.SecurityProfileId > 0);
        }
    }

    public class SecurityProfile_Rename : SecurityProfile_TransactionBaseClass
    {

        public long AccessClientId { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (SecurityProfile.SecurityProfileId == 0)
            {
                string message = "SecurityProfile has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
           AccessClientId= AccessClientId < 0 ? dbKey.Client.ClientId : AccessClientId;

           SecurityProfile.SecurityProfileRename(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, AccessClientId);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class SecurityProfile_RetrieveByName : SecurityProfile_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
      }

      public override void PerformWorkItem()
      {
        SecurityProfile.SecurityProfileRetrieveByName(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }

    public class SecurityProfile_RetrieveByPackageLevel : SecurityProfile_TransactionBaseClass
    {
        public List<b_SecurityProfile> SecurityProfileList = new List<b_SecurityProfile>();
        public int Rescount;
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_SecurityProfile> temp = new List<b_SecurityProfile>();
            SecurityProfile.SecurityProfileRetrieveByPackageLevel(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,ref temp, ref Rescount);
            SecurityProfileList = temp;
        }
    }

    #region for search grid V2-500
    public class SecurityProfile_CustomRetrieveChunkSearchV2 : SecurityProfile_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }
        public List<b_SecurityProfile> SecurityProfileList { get; set; }
        public override void PerformWorkItem()
        {
            b_SecurityProfile[] tmpArray = null;
            SecurityProfile.RetrieveCustomSecurityProfileChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            SecurityProfileList = new List<b_SecurityProfile>(tmpArray);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
       
    }

    public class SecurityProfile_AddForSecurityItem : SecurityProfile_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
           
        }

        public override void PerformWorkItem()
        {
            SecurityProfile.SecurityItemAddInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    #region Validate Profile Name at the time of Add and Update
    public class SecurityProfile_ValidateName : SecurityProfile_TransactionBaseClass
    {
        public SecurityProfile_ValidateName()
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

                SecurityProfile.ValidateName(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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
    #endregion

    #endregion

    #region V2-802
    public class SecurityProfile_RetrieveByClientIdV2 : SecurityProfile_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_SecurityProfile> SecurityProfileList { get; set; }
        public override void PerformWorkItem()
        {
            b_SecurityProfile[] tmpArray = null;
            SecurityProfile.RetrieveSecurityProfileByClientIdV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            SecurityProfileList = new List<b_SecurityProfile>(tmpArray);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    #endregion v2-802
}
