/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2018 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2015-Dec-16  SOM-884   Roger Lawton      Do not need to send LaborAccountClientLookupId to the 
*                                          ProcessSystem_CreateByForeignKeys transaction
**************************************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{



    /// <summary>
    /// Business object that stores a record from the Contact table.
    /// </summary>
    public partial class ProcessSystem : DataContractBase, IStoredProcedureValidation
    {
        
        public string SiteName { get; set; }
        public string Account { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public bool InactiveFlag { get; set; }
        public string ProcessSystemDesc { get; set; }
        public string LaborAccountClientLookupId { get; set; }
        public string ProcessDesc { get; set; }
        public string SystemDesc { get; set; }
        public bool m_validateClientLookupId { get; set; }
        #region public Methods

        //public b_ProcessSystem ToDatabaseObject()
        //{
        //    b_ProcessSystem dbObj = new b_ProcessSystem();
        //    dbObj.ClientId = this.ClientId;
        //    dbObj.SiteId = this.SiteId;
        //    dbObj.Process = this.Process;
        //    dbObj.System = this.System;
        //    dbObj.Labor_AccountId = this.Labor_AccountId;
        //    //dbObj.InActiveFlag = this.InActiveFlag;
        //    return dbObj;
        //}   

        public List<ProcessSystem> RetrieveForSearch(DatabaseKey dbKey)
        {
            ProcessSystem_RetrieveForSearch trans = new ProcessSystem_RetrieveForSearch();
            List<ProcessSystem> processSystemList = new List<ProcessSystem>();
            trans.ProcessSystem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            foreach (b_ProcessSystem processSystem in trans.ProcessSystemList)
            {
                ProcessSystem tmpprocessSystem = new ProcessSystem();
                UpdateFromDatabaseObject(trans.ProcessSystem);
                tmpprocessSystem.UpdateFromDatabaseObjectExtended(processSystem);

                processSystemList.Add(tmpprocessSystem);
            }
            return processSystemList;
        }

        public void UpdateFromDatabaseObjectExtended(b_ProcessSystem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Account = dbObj.Account;
            this.ProcessSystemId = dbObj.ProcessSystemId;
            this.SiteName = dbObj.SiteName;
        }

        //public void UpdateFromDatabaseObject(b_ProcessSystem dbObj)
        //{
        //    this.SiteId = dbObj.SiteId;
        //    this.ClientId = dbObj.ClientId;
        //    this.ProcessSystemId = dbObj.ProcessSystemId;
        //    this.Process = dbObj.Process;
        //    this.System = dbObj.System;
        //    this.Labor_AccountId = dbObj.Labor_AccountId;
        //    this.Account = dbObj.Account;

        //    // Turn on auditing
        //    AuditEnabled = true;
        //}

        public void CreateTo(DatabaseKey dbKey)
        {
            m_validateClientLookupId = true;
            Validate<ProcessSystem>(dbKey);
            ProcessSystem_Create trans = new ProcessSystem_Create();
            trans.ProcessSystem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ProcessSystem);
        }

        public static List<ProcessSystem> UpdateFromDatabaseObjectList(List<b_ProcessSystem> dbObjs)
        {
            List<ProcessSystem> result = new List<ProcessSystem>();

            foreach (b_ProcessSystem dbObj in dbObjs)
            {
                ProcessSystem tmp = new ProcessSystem();
                tmp.UpdateFromDataBaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromDataBaseObjectExtended(b_ProcessSystem dbObj)
        {
            this.SiteId = dbObj.SiteId;
            this.ClientId = dbObj.ClientId;
            this.ProcessSystemId = dbObj.ProcessSystemId;
            this.Process = dbObj.Process;
            this.System = dbObj.System;
            this.Labor_AccountId = dbObj.Labor_AccountId;
            this.Account = dbObj.Account;
            //this.InactiveFlag = dbObj.InacTiveFlag;
        }

        public void RetrieveByPkForeignKey(DatabaseKey dbKey)
        {
            ProcessSystem_RetrieveByPrimaryKey trans = new ProcessSystem_RetrieveByPrimaryKey()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ProcessSystem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.ProcessSystem);
            this.Account = trans.ProcessSystem.Account;
            this.SiteName = trans.ProcessSystem.SiteName;
            
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            ProcessSystemValidate trans = new ProcessSystemValidate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ProcessSystem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.ProcessSystem.LaborAccountClientLookupId = this.LaborAccountClientLookupId;
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
            return errors;
        }

        public void UpdateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<ProcessSystem>(dbKey);
            if (IsValid)
            {

                ProcessSystem_UpdateByForeignKey trans = new ProcessSystem_UpdateByForeignKey()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.ProcessSystem = this.ToDatabaseObject();
                trans.ProcessSystem.LaborAccountClientLookupId = this.LaborAccountClientLookupId;

                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.ProcessSystem);
            }
        }

        #endregion

        public void CreateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<ProcessSystem>(dbKey);

            if (IsValid)
            {
                ProcessSystem_CreateByForeignKeys trans = new ProcessSystem_CreateByForeignKeys()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.ProcessSystem = this.ToDatabaseObject();
                // SOM-884 - Not Necessary
                //trans.ProcessSystem.LaborAccountClientLookupId = this.LaborAccountClientLookupId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.ProcessSystem);
            }
        }

        #region  Tree list
        public List<ProcessSystem> RetrieveForTreeList(DatabaseKey dbKey)
        {
             List<ProcessSystem> processSystemList = new List<ProcessSystem>();
             ProcessSystem_RetrieveAllForProcess trans = new ProcessSystem_RetrieveAllForProcess()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.ProcessSystem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            // SOM-1184
            trans.UseTransaction = false;
            trans.Execute();
             foreach (b_ProcessSystem processSystem in trans.ProcessSystemList)
            {
                ProcessSystem tmpprocessSystem = new ProcessSystem();
               
                tmpprocessSystem.UpdateFromDatabaseObjectExtended(processSystem);
                tmpprocessSystem.ProcessDesc = processSystem.ProcessDesc;
                tmpprocessSystem.SystemDesc = processSystem.SystemDesc;
                processSystemList.Add(tmpprocessSystem);

            }
            return processSystemList;
        }
        }


        #endregion
    }

