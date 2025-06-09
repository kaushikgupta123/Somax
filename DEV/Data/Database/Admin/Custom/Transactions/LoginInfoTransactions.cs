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
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-18  SOM-376   Roger Lawton      Added RetrieveByUserInfoId
**************************************************************************************************
*/

using System;
using System.Collections.Generic;
using Common.Enumerations;
using Database.Business;

/*
 * Note: The generated code explicitly sets the client ID based on the user's current client ID. 
 * That logic will prevent super users from loading other clients. To avoid this, all generated
 * code has been moved to the custom section, and the LoginInfo_TransactionBaseClass has been modified
 * to check super user status before setting the client ID.
 */

namespace Database.Transactions
{
    
    public class LoginInfo_RetrieveByClientId : LoginInfo_TransactionBaseClass
    {
	
        public List<b_LoginInfo> LoginInfoList { get; set; }

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
            b_LoginInfo[] tmpArray = null;
           

            LoginInfo.RetrieveByClientIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            LoginInfoList = new List<b_LoginInfo>();
            foreach (b_LoginInfo tmpObj in tmpArray)
            {
                LoginInfoList.Add(tmpObj);
            }
        }
    }

    public class LoginInfo_RetrieveByResetPasswordCode : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.ResetPasswordCode == null || LoginInfo.ResetPasswordCode.Equals(Guid.Empty))
            {
                string message = "ResetPasswordCode is invalid.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            LoginInfo.RetrieveByResetPasswordCodeFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class LoginInfo_RetrieveByUserInfoId : LoginInfo_TransactionBaseClass
    {

      public override void PerformLocalValidation()
      {
        base.PerformLocalValidation();
        if (LoginInfo.UserInfoId == 0)
        {
          string message = "UserInfoId is invalid.";
          throw new Exception(message);
        }
      }

      public override void PerformWorkItem()
      {
        base.UseTransaction = false;
        LoginInfo.RetrieveByUserInfoId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
      }
    }


  public class LoginInfo_RetrieveBySearchCriteria : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (string.IsNullOrEmpty(LoginInfo.UserName) || string.IsNullOrEmpty(LoginInfo.Email))
            {
                string message = "User Name or Email is invalid.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            LoginInfo.RetrieveBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class LoginInfo_RetrieveBySearchCriteriaAdmin : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (string.IsNullOrEmpty(LoginInfo.UserName) || string.IsNullOrEmpty(LoginInfo.Email))
            {
                string message = "User Name or Email is invalid.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            LoginInfo.RetrieveBySearchCriteriaFromDatabaseAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class LoginInfo_UpdatePassword : AbstractTransactionManager
    {

        public LoginInfo_UpdatePassword()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Admin;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.LoginInfoId == 0)
            {
                string message = "LoginInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public b_LoginInfo LoginInfo { get; set; }
		public b_ChangeLog ChangeLog { get; set; }


        public override void PerformWorkItem()
        {
            // The dbKey might be from GetAdminDBKey function. Checking is preformed to make sure the client Id is not zero.
            if (dbKey.Client.ClientId != 0) { LoginInfo.ClientId = dbKey.Client.ClientId; }
            LoginInfo.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
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


    public class LoginInfo_UpdateCustom : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.LoginInfoId == 0)
            {
                string message = "LoginInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            if (dbKey.Client.ClientId != 0) { LoginInfo.ClientId = dbKey.Client.ClientId; }
            LoginInfo.UpdateInDatabaseCustom(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class LoginInfo_RetrieveByPKAdmin : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.LoginInfoId==0)
            {
                string message = "LoginInfoId is invalid.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            LoginInfo.RetrieveByPKFromDatabaseAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class LoginInfo_UpdateByPKAdmin : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.LoginInfoId == 0)
            {
                string message = "LoginInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            if (dbKey.Client.ClientId != 0) { LoginInfo.ClientId = dbKey.Client.ClientId; }
            LoginInfo.UpdateByPKAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class LoginInfo_RetrieveByLoginInfoId : LoginInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.LoginInfoId == 0)
            {
                string message = "LoginInfoId is invalid.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            LoginInfo.RetrieveLoginInfoByLoginInfoId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #region V2-962
    public class LoginInfo_RetrieveByUserInfoIdforAdmin : LoginInfo_TransactionBaseClass
    {
        public long customClientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (LoginInfo.UserInfoId == 0)
            {
                string message = "UserInfoId is invalid.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            LoginInfo.ClientId = customClientId;
            LoginInfo.RetrieveByUserInfoId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion
}
