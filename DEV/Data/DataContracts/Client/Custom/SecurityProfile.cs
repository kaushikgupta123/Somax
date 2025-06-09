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
* 2014-Oct-27  SOM-384   Roger Lawton     Added RetrieveByName method
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
   public partial class SecurityProfile:DataContractBase,IStoredProcedureValidation
   {

       #region Properties
        private string ValidateFor { get; set; }
        public long AccessClientId { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        public int ResultCount { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        public string SearchText { get; set; }
        public Int32 TotalCount { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
      
        #endregion

        public static List<SecurityProfile> UpdateFromDatabaseObjectList(List<b_SecurityProfile> dbObjs)
       {
           List<SecurityProfile> result = new List<SecurityProfile>();

           foreach (b_SecurityProfile dbObj in dbObjs)
           {
               SecurityProfile tmp = new SecurityProfile();
               tmp.UpdateFromDatabaseObject(dbObj);
               result.Add(tmp);
           }
           return result;
       }

       public List<SecurityProfile> RetrieveAllProfiles(DatabaseKey dbKey)
       {
           SecurityProfile_RetrieveAllProfiles trans = new SecurityProfile_RetrieveAllProfiles()
           {
               CallerUserInfoId = dbKey.User.UserInfoId,
               CallerUserName = dbKey.UserName
           };
           trans.dbKey = dbKey.ToTransDbKey();
           trans.AccessClientId = this.AccessClientId;
           trans.Execute();
           return UpdateFromDatabaseObjectList(trans.SecurityProfileList);
       }
        public List<SecurityProfile> RetrieveAllProfilesForEnterPrisePackagelevel(DatabaseKey dbKey)
        {
            SecurityProfile_RetrieveAllProfilesforEnterprise trans = new SecurityProfile_RetrieveAllProfilesforEnterprise()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SecurityProfile = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.AccessClientId = this.AccessClientId;
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.SecurityProfileList);

        }

        public void ValidateSecurityProfileName(DatabaseKey dbKey)
       {
           ValidateFor = "SecurityProfileName";
           Validate<SecurityProfile>(dbKey);
       }
         #region Validate Profile Name at the time of Add and Update
        public void ValidateProfileName(DatabaseKey dbKey)
        {
            ValidateFor = "ProfileName";
            Validate<SecurityProfile>(dbKey);
        }
        #endregion
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
       {
           List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

           if (ValidateFor == "SecurityProfileName")
           {
               SecurityProfile_ValidateSecurityProfileName trans = new SecurityProfile_ValidateSecurityProfileName()
               {
                   CallerUserInfoId = dbKey.User.UserInfoId,
                   CallerUserName = dbKey.UserName,
               };
               trans.SecurityProfile = this.ToDatabaseObject();
               trans.dbKey = dbKey.ToTransDbKey();
               trans.AccessClientId = this.AccessClientId;
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
           else
            {
                SecurityProfile_ValidateName trans = new SecurityProfile_ValidateName()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.SecurityProfile = this.ToDatabaseObject();
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

       public void CreateClone(DatabaseKey dbKey)
       {
           SecurityProfile_CreateClone trans = new SecurityProfile_CreateClone();
           trans.SecurityProfile = this.ToDatabaseObject();
           trans.dbKey = dbKey.ToTransDbKey();
           trans.AccessClientId = this.AccessClientId;
           trans.Execute();

           // The create procedure may have populated an auto-incremented key. 
           UpdateFromDatabaseObject(trans.SecurityProfile);
       }

       public void SecurityProfileRename(DatabaseKey dbKey)
       {
           SecurityProfile_Rename trans = new SecurityProfile_Rename();
           trans.SecurityProfile = this.ToDatabaseObject();
           trans.ChangeLog = GetChangeLogObject(dbKey);
           trans.dbKey = dbKey.ToTransDbKey();
           trans.AccessClientId = this.AccessClientId;
           trans.Execute();

           // The create procedure changed the Update Index.
           UpdateFromDatabaseObject(trans.SecurityProfile);
       }
       public void RetrieveByName(DatabaseKey dbKey)
       {
         SecurityProfile_RetrieveByName trans = new SecurityProfile_RetrieveByName();
         trans.SecurityProfile = this.ToDatabaseObject();
         trans.dbKey = dbKey.ToTransDbKey();
         trans.Execute();
         UpdateFromDatabaseObject(trans.SecurityProfile);
       }

        public List<SecurityProfile> RetrieveByPackageLevel(DatabaseKey dbKey)
        {
            SecurityProfile_RetrieveByPackageLevel trans = new SecurityProfile_RetrieveByPackageLevel()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SecurityProfile = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.SecurityProfile.Page = this.Page;
            trans.SecurityProfile.ResultsPerPage = this.ResultsPerPage;
            trans.SecurityProfile.OrderColumn = this.OrderColumn;
            trans.SecurityProfile.OrderDirection = this.OrderDirection;
            
            trans.Execute();
            this.ResultCount= trans.Rescount;
            return UpdateFromDatabaseObjectList(trans.SecurityProfileList);

        }

        #region for search grid V2-500
        public List<SecurityProfile> CustomSecurityProfileRetrieveChunkSearchV2(DatabaseKey dbKey)
        {
            SecurityProfile_CustomRetrieveChunkSearchV2 trans = new SecurityProfile_CustomRetrieveChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SecurityProfile= this.ToDatabaseObjectExtend();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<SecurityProfile> splist = new List<SecurityProfile>();
            foreach (b_SecurityProfile sp in trans.SecurityProfileList)
            {
                SecurityProfile tmpSecurityProfile = new SecurityProfile();
                tmpSecurityProfile.UpdateFromDatabaseObjectForRetriveAllForSearch(sp);
                splist.Add(tmpSecurityProfile);
            }
            return splist;
        }
        public void UpdateFromDatabaseObjectForRetriveAllForSearch(b_SecurityProfile dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            
            this.orderbyColumn = dbObj.OrderColumn;
            this.orderBy = dbObj.OrderDirection;
            this.offset1 = dbObj.Page;
            this.nextrow = dbObj.ResultsPerPage;
            this.SearchText = dbObj.SearchText;
            this.TotalCount = dbObj.TotalCount;

        }
        public void SecurityItemAdd(DatabaseKey dbKey)
        {
            SecurityProfile_AddForSecurityItem trans = new SecurityProfile_AddForSecurityItem()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SecurityProfile = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SecurityProfile);
        }
        public b_SecurityProfile ToDatabaseObjectExtend()
        {
            b_SecurityProfile dbObj = new b_SecurityProfile();
            dbObj = this.ToDatabaseObject();
            dbObj.OrderColumn = this.OrderColumn;
            dbObj.OrderDirection = this.OrderDirection;
            dbObj.Page = this.Page;
            dbObj.ResultsPerPage = this.ResultsPerPage;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        #endregion

        #region V2-802
        public List<SecurityProfile> CustomSecurityProfileRetrieveByClientIdV2(DatabaseKey dbKey)
        {
            SecurityProfile_RetrieveByClientIdV2 trans = new SecurityProfile_RetrieveByClientIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SecurityProfile = this.ToDatabaseObjectExtend();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<SecurityProfile> splist = new List<SecurityProfile>();
            foreach (b_SecurityProfile sp in trans.SecurityProfileList)
            {
                SecurityProfile tmpSecurityProfile = new SecurityProfile();
                tmpSecurityProfile.UpdateFromDatabaseObjectForSecurityProfileRetrieveByClientId(sp);
                splist.Add(tmpSecurityProfile);
            }
            return splist;
        }
        public void UpdateFromDatabaseObjectForSecurityProfileRetrieveByClientId(b_SecurityProfile dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
           
        }
       
        #endregion
    }
}
