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


using System.Collections.Generic;
using System.Runtime.Serialization;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the LoginInfo table.
    /// </summary>
    public partial class LoginInfo : DataContractBase 
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        // Provide Access to the AuditEnabled Flag 
        // 2024-May-15 - RKL - V2-10??
        public bool SetAuditEnable(bool lEnable)
        {
          bool oldvalue = this.AuditEnabled;
          this.AuditEnabled = lEnable;
          return oldvalue;
        }
        public b_LoginInfo ToExtendedDatabaseObject()
        {
            b_LoginInfo dbObj = ToDatabaseObject();
            dbObj.Email = this.Email;
         
            return dbObj;
        }    


        #region Transaction Methods
       

        public void RetrieveUserNames(DatabaseKey dbKey) 
        {
            LoginInfo_RetrieveByClientId trans = new LoginInfo_RetrieveByClientId()
			{
				CallerUserInfoId = dbKey.User.UserInfoId,
				CallerUserName = dbKey.UserName
			};
            trans.LoginInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UserNames = new List<KeyValuePair<string,string>>();
            foreach(b_LoginInfo loginInfo in trans.LoginInfoList)
            {
                UserNames.Add(new KeyValuePair<string,string>(loginInfo.UserInfoId.ToString(), loginInfo.UserName));
            }
        }

        public void RetrieveByUserName(DatabaseKey dbKey)
        {
            b_LoginDataSet loginData = new b_LoginDataSet();
            loginData.UserName = UserName;

            LoginDataSet_RetrieveByUsername trans = new LoginDataSet_RetrieveByUsername()
            {
                dbKey = dbKey.ToTransDbKey(),
                LoginData = loginData,
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginData.LoginInfo);
        }

        public void RetrieveByResetPasswordCode(DatabaseKey dbKey)
        {
            LoginInfo_RetrieveByResetPasswordCode trans = new LoginInfo_RetrieveByResetPasswordCode()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LoginInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginInfo);
        }

        public void RetrieveByUserInfoId(DatabaseKey dbKey)
        {
          LoginInfo_RetrieveByUserInfoId trans = new LoginInfo_RetrieveByUserInfoId()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };
          trans.LoginInfo = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();

          UpdateFromDatabaseObject(trans.LoginInfo);
        }

        public void RetrieveBySearchCriteria(DatabaseKey dbKey)
        {
            LoginInfo_RetrieveBySearchCriteria trans = new LoginInfo_RetrieveBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LoginInfo = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginInfo);
        }

        public void RetrieveBySearchCriteriaAdmin(DatabaseKey dbKey)
        {
            LoginInfo_RetrieveBySearchCriteriaAdmin trans = new LoginInfo_RetrieveBySearchCriteriaAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LoginInfo = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginInfo);
        }

        public void UpdatePassword(DatabaseKey dbKey)
        {
            LoginInfo_UpdatePassword trans = new LoginInfo_UpdatePassword();
            trans.LoginInfo = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.LoginInfo);
        }

        public void UpdateCustom(DatabaseKey dbKey)
        {
            LoginInfo_UpdateCustom trans = new LoginInfo_UpdateCustom();
            trans.LoginInfo = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.LoginInfo);
        }
        public void RetrieveByPKAdmin(DatabaseKey dbKey)
        {
            LoginInfo_RetrieveByPKAdmin trans = new LoginInfo_RetrieveByPKAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LoginInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginInfo);
        }
        public void UpdateByPKAdmin(DatabaseKey dbKey)
        {
            LoginInfo_UpdateByPKAdmin trans = new LoginInfo_UpdateByPKAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            }; ;
            trans.LoginInfo = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.LoginInfo);
        }
        #endregion

        #region Properties


        /// <summary>
        /// ClientId property
        /// </summary>
        [DataMember]
        public List<KeyValuePair<string, string>> UserNames { get; set; }

        [DataMember]
        public string Email { get; set; }
        #endregion

        #region Public Methods
        public b_ChangeLog GetChangeLog(DatabaseKey dbKey)
        {
            return GetChangeLogObject(dbKey);
        }
        #endregion

        public void RetrieveByLoginInfoId(DatabaseKey dbKey)
        {
            LoginInfo_RetrieveByLoginInfoId trans = new LoginInfo_RetrieveByLoginInfoId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.LoginInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginInfo);
        }
        #region V2-962
        public void RetrieveByUserInfoIdforAdmin(DatabaseKey dbKey)
        {
            LoginInfo_RetrieveByUserInfoIdforAdmin trans = new LoginInfo_RetrieveByUserInfoIdforAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.customClientId = this.ClientId;
            trans.LoginInfo = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.LoginInfo);
        }
        #endregion
    }
}
