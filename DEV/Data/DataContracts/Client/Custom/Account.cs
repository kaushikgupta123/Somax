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
* Date        JIRA Item Person          Description
* =========== ========= =============== ==========================================================
* 2014-Aug-02 SOM-264   Roger Lawton    Added InactiveFlad to RetrieveClientLookupIdBySearchCriteria
* 2016-Aug-22 SOM-1049  Roger Lawton    Added New method to retrieve for search
**************************************************************************************************
*/
using System.Collections.Generic;

namespace DataContracts
{
    using Database;
    using Database.Business;
    using static Database.Account_RetrieveAllTemplatesWithClient;

    public partial class Account : DataContractBase, IStoredProcedureValidation
    {
        public List<b_Account> AccountList { get; set; }
        private bool m_validateClientLookupId;
        private bool m_ActivateorInactivate;

        #region Property
        public string SiteName { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int TotalCount { get; set; }
        public List<Account> listOfAccount { get; set; }
        #endregion

        #region Transactions

        public List<b_Account> ToDatabaseObjectList()
        {
            List<b_Account> dbObj = new List<b_Account>();
            dbObj = this.AccountList;
            return dbObj;
        }

        public List<Account> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Account_RetrieveClientLookupIdBySearchCriteria trans = new Account_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AccountList = this.ToDatabaseObjectList();
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Account> accountList = new List<Account>();
            foreach (b_Account account in trans.AccountList)
            {
                Account tmpAccount = new Account()
                {
                    AccountId = account.AccountId,
                    ClientLookupId = account.ClientLookupId,
                    Name=account.Name,
                    InactiveFlag=account.InactiveFlag
                };
                accountList.Add(tmpAccount);
            }

            return accountList;
        }

        public List<Account> RetrieveForSearch(DatabaseKey dbKey)
        {
            Account_RetrieveForSearch trans = new Account_RetrieveForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UseTransaction = false;
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Account> AccountList = new List<Account>();
            foreach (b_Account dbobj in trans.AccountList)
            {
                Account tmp = new Account();
                tmp.UpdateFromDatabaseObject(dbobj);
                AccountList.Add(tmp);
            }

            return AccountList;
        }
        public List<Account> RetrieveForSearch_V2(DatabaseKey dbKey)
        {
            Account_RetrieveForSearch_V2 trans = new Account_RetrieveForSearch_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UseTransaction = false;
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Account> AccountList = new List<Account>();
            foreach (b_Account dbobj in trans.AccountList)
            {
                Account tmp = new Account();
                tmp.UpdateFromDatabaseObject(dbobj);
                AccountList.Add(tmp);
            }

            return AccountList;
        }

        public List<Account> RetrieveForSearchforSuperUser(DatabaseKey dbKey)
        {
            Account_RetrieveForSearchforSuperUser_V2 trans = new Account_RetrieveForSearchforSuperUser_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UseTransaction = false;
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Account> AccountList = new List<Account>();
            foreach (b_Account dbobj in trans.AccountList)
            {
                Account tmp = new Account();
                tmp.UpdateFromDatabaseObjectForSuperUser(dbobj);
                tmp.SiteName = dbobj.SiteName;
                AccountList.Add(tmp);
            }

            return AccountList;
        }

        public void UpdateFromDatabaseObjectForSuperUser(b_Account dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.AccountId = dbObj.AccountId;
            this.SiteId = dbObj.SiteId;            
            this.DepartmentId = dbObj.DepartmentId;            
            this.ClientLookupId = dbObj.ClientLookupId;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.Name = dbObj.Name;            
            this.IsExternal = dbObj.IsExternal;
            this.UpdateIndex = dbObj.UpdateIndex;

            // Turn on auditing
            AuditEnabled = true;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (m_validateClientLookupId)
            {
                AccountValidationByClientLookUpId trans = new AccountValidationByClientLookUpId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Account = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();


                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            if (m_ActivateorInactivate)
            {
                Account_ValidateByInactivateorActivate trans = new Account_ValidateByInactivateorActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Account = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            return errors;
        }

        public List<Account> RetrieveAllClientLookupId(DatabaseKey dbKey)
        {
            Account_RetrieveAllClientLookupId trans = new Account_RetrieveAllClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.AccountList = this.ToDatabaseObjectList();
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //return Department.UpdateFromDatabaseObjectList(trans.DepartmentList);
            List<Account> accountList = new List<Account>();
            foreach (b_Account account in trans.AccountList)
            {
                Account tmpAccount = new Account()
                {
                    AccountId = account.AccountId,
                    ClientLookupId = account.ClientLookupId,
                    Name = account.Name
                };
                accountList.Add(tmpAccount);
            }

            return accountList;
        }


        public static List<Account> UpdateFromDatabaseObjectList(List<b_Account> dbObjs)
        {
            List<Account> result = new List<Account>();

            foreach (b_Account  dbObj in dbObjs)
            {
                Account tmp = new Account();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);

            }
            return result;
        }
      
        #endregion

        #region Search Parameter Lists
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }




        #endregion

        public List<Account> RetrieveAllTemplatesWithClient(DatabaseKey dbKey)
        {
            Account_RetrieveAllTemplatesWithClient trans = new Account_RetrieveAllTemplatesWithClient()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Account.UpdateFromDatabaseObjectList(trans.AccountList);
        }
        public void CreateWithValidation(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<Account>(dbKey);

            if (IsValid)
            {

                Account_Create trans = new Account_Create();
                trans.Account = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Account);
            }
        }

        public void ChangeClientLookupId(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<Account>(dbKey);
            if (IsValid)
            {

                Account_ChangeClientLookupId trans = new Account_ChangeClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Account = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Account);
            }
        }

        public List<Account> RetrieveAll(DatabaseKey dbKey)
        {
            Account_RetrieveAll_V2 trans = new Account_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Account> AccountList = new List<Account>();
            foreach (b_Account Account in trans.AccountList)
            {
                Account tmpAccount = new Account();

                tmpAccount.UpdateFromDatabaseObject(Account);
                AccountList.Add(tmpAccount);
            }
            return AccountList;
        }

        public void CheckAccountIsInactivateorActivate(DatabaseKey dbKey)
        {
            m_ActivateorInactivate = true;
            Validate<Account>(dbKey);
        }
        public void ActivateOrInactivateRecord(DatabaseKey dbKey)
        {
            m_ActivateorInactivate = true;
            Validate<Account>(dbKey);

            if (IsValid)
            {
                this.Update(dbKey);
            }
        }
        public int UpdateByActivateorInactivate(DatabaseKey dbKey)
        {
            Account_UpdateByActivateorInactivate trans = new Account_UpdateByActivateorInactivate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UseTransaction = false;
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return trans.Account.UpdateIndex;
        }

        public void RetrieveforDetails(DatabaseKey dbKey)
        {
            
            Account_RetrieveCustom trans = new Account_RetrieveCustom();
            trans.Account = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectCustom(trans.Account);
        }

      

    public void UpdateFromDatabaseObjectCustom(b_Account dbObj)
    {
        this.ClientId = dbObj.ClientId;
        this.AccountId = dbObj.AccountId;
        this.SiteId = dbObj.SiteId;
        this.AreaId = dbObj.AreaId;
        this.DepartmentId = dbObj.DepartmentId;
        this.StoreroomId = dbObj.StoreroomId;
        this.ClientLookupId = dbObj.ClientLookupId;
        this.InactiveFlag = dbObj.InactiveFlag;
        this.Name = dbObj.Name;
        this.ParentId = dbObj.ParentId;
        this.IsExternal = dbObj.IsExternal;
        this.UpdateIndex = dbObj.UpdateIndex;
        this.SiteName = dbObj.SiteName;

        // Turn on auditing
        AuditEnabled = true;
    }
        #region Account Lookup list
        public List<Account> GetAllAccountLookupListMobileV2(DatabaseKey dbKey, string TimeZone)
        {
            Account_RetrieveChunkSearchLookupListMobile_V2 trans = new Account_RetrieveChunkSearchLookupListMobile_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Account = this.ToDateBaseObjectForAccountLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfAccount = new List<Account>();

            List<Account> Accountlist = new List<Account>();
            foreach (b_Account Account in trans.AccountList)
            {
                Account tmpAccount = new Account();

                tmpAccount.UpdateFromDatabaseObjectForAccountLookupListChunkSearch(Account, TimeZone);
                Accountlist.Add(tmpAccount);
            }
            return Accountlist;
        }

        public b_Account ToDateBaseObjectForAccountLookuplistChunkSearch()
        {
            b_Account dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Name = this.Name;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForAccountLookupListChunkSearch(b_Account dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCount = dbObj.TotalCount;

        }
        #endregion
    }
}
