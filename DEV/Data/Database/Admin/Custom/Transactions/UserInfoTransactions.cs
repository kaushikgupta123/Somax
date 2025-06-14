/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using Database.Business;

/*
 * Note: The generated code explicitly sets the client ID based on the user's current client ID. 
 * That logic will prevent super users from loading other clients. To avoid this, all generated
 * code has been moved to the custom section, and the UserInfo_TransactionBaseClass has been modified
 * to check super user status before setting the client ID.
 */

namespace Database.Transactions
{
    /*
    public class UserInfo_TransactionBaseClass : AbstractTransactionManager
    {
        public UserInfo_TransactionBaseClass()
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
            if (UserInfo == null)
            {
                string message = "UserInfo has not been set.";
                throw new Exception(message);
            }

            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;

            if (!dbKey.User.IsSuperUser)
            {
                // Explicitly set tenant id from dbkey
                this.UserInfo.ClientId = this.dbKey.Client.ClientId;
            }
        }

        public b_UserInfo UserInfo { get; set; }
        public b_ChangeLog ChangeLog { get; set; }

        public override void PerformWorkItem()
        {
            // 
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

    public class UserInfo_Create : UserInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId > 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserInfo.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserInfo.UserInfoId > 0);
        }
    }

    public class UserInfo_Retrieve : UserInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            UserInfo.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class UserInfo_Update : UserInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserInfo.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class UserInfo_Delete : UserInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserInfo.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class UserInfo_RetrieveAll : AbstractTransactionManager
    {

        public UserInfo_RetrieveAll()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Admin;
        }


        public List<b_UserInfo> UserInfoList { get; set; }

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
            b_UserInfo[] tmpArray = null;
            b_UserInfo o = new b_UserInfo()
            {
                ClientId = this.dbKey.Client.ClientId,
                UserInfoId = this.dbKey.User.UserInfoId
            };

            o.RetrieveAllFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            UserInfoList = new List<b_UserInfo>(tmpArray);
        }
    }
    */

    public class UserInfo_RetrieveByClientId : UserInfo_TransactionBaseClass
    {
	
        public List<b_UserInfo> UserInfoList { get; set; }

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
            b_UserInfo[] tmpArray = null;
            
            UserInfo.RetrieveByClientIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            UserInfoList = new List<b_UserInfo>();
            foreach (b_UserInfo tmpObj in tmpArray)
            {
                UserInfoList.Add(tmpObj);
            }
        }
    }

    public class UserInfo_UpdateWithLogin : UserInfo_TransactionBaseClass
    {
        public b_LoginInfo LoginInfo { get; set; }
        public b_ChangeLog LoginInfoChangeLog { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0 || LoginInfo.LoginInfoId == 0)
            {
                string message = "UserInfo or LoingInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserInfo.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }

            LoginInfo.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            
            if (LoginInfoChangeLog != null) {
                LoginInfoChangeLog.ClientId = dbKey.Client.ClientId;
                LoginInfoChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            }
        }
    }

    public class UserInfo_DeleteWithLogin : UserInfo_TransactionBaseClass
    {
        public b_LoginInfo LoginInfo { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0 || LoginInfo.LoginInfoId == 0)
            {
                string message = "UserInfo or LoginInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            LoginInfo.ClientId = dbKey.Client.ClientId;
            LoginInfo.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            UserInfo.DeleteFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class UserInfo_CreateWithLogin : UserInfo_TransactionBaseClass
    {
        public b_LoginInfo LoginInfo { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId > 0 || LoginInfo.LoginInfoId > 0)
            {
                string message = "UserInfo or LoginInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserInfo.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

            LoginInfo.UserInfoId = UserInfo.UserInfoId;
            LoginInfo.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserInfo.UserInfoId > 0);
        }
    }
    public class UserLoginInfo_Retrieve : UserInfo_TransactionBaseClass
    {
       
        
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfoId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            //base.UseTransaction = false;  this is too late - connection and txn are started before performworkitem executed
            UserInfo.RetrieveLoginInfoFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class UserInfo_UserDelete : UserInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserInfo.DeleteUserFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class AdminUserInfo_RetrieveByPK : UserInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfoId has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserInfo.RetrieveUserInfoFromDatabaseAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class UserInfo_UpdateByPKAdmin : UserInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserInfo.UpdateByPKAdmin(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    public class UserInfo_RetrieveUserType : UserInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            if (string.IsNullOrEmpty(UserInfo.UserName) && string.IsNullOrEmpty(UserInfo.Email))
            {
                string message = "UserInfoId has an invalid UserName or Email";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserInfo.RetrieveUserInfoUserTypeFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class UserInfo_Create_V2 : UserInfo_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId > 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserInfo.InsertIntoDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserInfo.UserInfoId > 0);
        }
    }
    #region Update V2-911
    public class UserInfo_UpdateCustomForSomaxAdminDetails : UserInfo_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserInfo.UserInfoId == 0)
            {
                string message = "UserInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            UserInfo.UpdateInDatabaseCustomForSomaxAdminDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion
    #region V2-962
    public class UserInfo_RetrieveUserDetailsByUserInfoIdFromAdmin : UserInfo_TransactionBaseClass
    {
        
        public override void PerformLocalValidation()
        {
            long clientId = UserInfo.ClientId;
            base.UseTransaction = false;    // moved from PerformWorkItem
            base.PerformLocalValidation();
            UserInfo.ClientId = clientId;
            if (UserInfo.UserInfoId == 0 && UserInfo.ClientId==0)//check both UserInfoId and ClientId
            {
                string message = "UserInfoId and ClientId are invalid ID.";
                throw new Exception(message);
            }
        }


        public override void PerformWorkItem()
        {
            b_UserInfo tmpResult = null;
            UserInfo.RetrieveUserDeatilsByUserInfoIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpResult);
            UserInfo = tmpResult;
        }
    }
    #endregion
}
