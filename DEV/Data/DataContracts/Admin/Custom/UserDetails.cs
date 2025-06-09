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
* 2014-Oct-21  SOM-384   Roger Lawton     Added method to retrievesiteusercounts
* 2014-Oct-22  SOM-384   Roger Lawton     Validate when deleting
* 2014-Oct-24  SOM-384   Roger Lawton     Added LimitedUser Count
*                                         Comment the phone, tablet, webappuser properties
*                                         Comment the phonecount and tabletcount properties
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Enumerations;
using Database;
using Database.Business;
using DataContracts;
using Data.Database;

namespace DataContracts
{
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class UserDetails : DataContractBase, IStoredProcedureValidation
    {
        #region Private Variable

        string ValidateFor = string.Empty;

        #region V2-417 Inactivate and Active Users
        public Int64 ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string Flag { get; set; }

        public bool APM { get; set; }
        public bool CMMS { get; set; }
        public bool Sanitation { get; set; }
        public bool Fleet { get; set; }

        public int ProductGrouping { get; set; }
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UserDetails()
        {
            Initialize();
        }

        private List<UserDetails> UpdateFromDatabaseObject(List<b_UserDetails> dbObj)
        {
            List<UserDetails> result = new List<UserDetails>();
            foreach (b_UserDetails userDetails in dbObj)
            {
                UserDetails tmp = new UserDetails();
                tmp.UpdateFromDatabaseObject(userDetails);
                result.Add(tmp);
            }

            return result;
        }

        private void Initialize()
        {
            b_UserDetails dbObj = new b_UserDetails();
            this.ClientId = dbObj.ClientId;
            this.CompanyName = dbObj.CompanyName;
            this.SiteControlled = dbObj.SiteControlled;

            this.UserInfoId = dbObj.UserInfoId;
            this.FirstName = dbObj.FirstName;
            this.LastName = dbObj.LastName;
            this.MiddleName = dbObj.MiddleName;
            this.Email = dbObj.Email;
            this.Localization = dbObj.Localization;
            this.TimeZone = dbObj.TimeZone;
            this.IsSuperUser = dbObj.IsSuperUser;
            this.UIConfiguration = dbObj.UIConfiguration;
            this.UserUpdateIndex = dbObj.UserUpdateIndex;
            this.ResultsPerPage = dbObj.ResultsPerPage;
            this.StartPage = dbObj.StartPage;
            this.IsPasswordTemporary = dbObj.IsPasswordTemporary;
            this.DefaultSiteId = dbObj.DefaultSiteId;

            this.LoginInfoId = dbObj.LoginInfoId;
            this.UserName = dbObj.UserName;
            this.Password = dbObj.Password;
            this.SecurityQuestion = dbObj.SecurityQuestion;
            this.SecurityResponse = dbObj.SecurityResponse;
            this.FailedAttempts = dbObj.FailedAttempts;
            this.LastFailureDate = dbObj.LastFailureDate;
            this.LastLoginDate = dbObj.LastLoginDate;
            this.IsActive = dbObj.IsActive;
            this.LoginUpdateIndex = dbObj.LoginUpdateIndex;
            this.ResetPasswordCode = dbObj.ResetPasswordCode;
            this.ResetPasswordRequestDate = dbObj.ResetPasswordRequestDate;
            this.TempPassword = dbObj.TempPassword;
            //this.TabletUser = dbObj.TabletUser;
            //this.PhoneUser = dbObj.TabletUser;
            //this.WebAppUser = dbObj.WebAppUser;
            this.UserType = dbObj.UserType;
            this.SecurityProfileId = dbObj.SecurityProfileId;
            this.SecurityProfileName = dbObj.SecurityProfileName;

            Personnel personnel = new Personnel();
            personnel.UpdateFromDatabaseObject(dbObj.Personnel);
            this.Personnel = personnel;

            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;

            LookupList lookuplist = new LookupList();
            lookuplist.UpdateFromDatabaseObjectExtended(dbObj.LookupList);
            this.LookupList = lookuplist;

            Craft craft = new Craft();
            craft.UpdateFromDatabaseObject(dbObj.Craft);
            this.Craft = craft;

            Department department = new Department();
            department.UpdateFromDatabaseObject(dbObj.Department);
            this.Department = department;
        }

        public b_UserDetails ToDatabaseObject()
        {
            b_UserDetails dbObj = new b_UserDetails();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.CompanyName = this.CompanyName;
            dbObj.SiteControlled = this.SiteControlled;

            dbObj.UserInfoId = this.UserInfoId;
            dbObj.FirstName = this.FirstName;
            dbObj.LastName = this.LastName;
            dbObj.MiddleName = this.MiddleName;
            dbObj.Email = this.Email;
            dbObj.Localization = this.Localization;
            dbObj.TimeZone = this.TimeZone;
            dbObj.IsSuperUser = this.IsSuperUser;
            dbObj.UIConfiguration = this.UIConfiguration;
            dbObj.UserUpdateIndex = this.UserUpdateIndex;
            dbObj.ResultsPerPage = this.ResultsPerPage;
            dbObj.StartPage = this.StartPage;
            dbObj.IsPasswordTemporary = this.IsPasswordTemporary;
            dbObj.IsSiteAdmin = this.IsSiteAdmin;
            dbObj.DefaultSiteId = this.DefaultSiteId;
            dbObj.CMMSUser = this.CMMSUser;
            dbObj.SanitationUser = this.SanitationUser;

            dbObj.LoginInfoId = this.LoginInfoId;
            dbObj.UserName = this.UserName;
            dbObj.Password = this.Password;
            dbObj.SecurityQuestion = this.SecurityQuestion;
            dbObj.SecurityResponse = this.SecurityResponse;
            dbObj.FailedAttempts = this.FailedAttempts;
            dbObj.LastFailureDate = this.LastFailureDate;
            dbObj.LastLoginDate = this.LastLoginDate;
            dbObj.IsActive = this.IsActive;
            dbObj.LoginUpdateIndex = this.LoginUpdateIndex;
            dbObj.ResetPasswordCode = this.ResetPasswordCode;
            dbObj.ResetPasswordRequestDate = this.ResetPasswordRequestDate;
            dbObj.TempPassword = this.TempPassword;
            //dbObj.TabletUser = this.TabletUser;
            //dbObj.PhoneUser = this.PhoneUser;
            //dbObj.WebAppUser = this.WebAppUser;
            dbObj.UserType = this.UserType;
            dbObj.SecurityProfileId = this.SecurityProfileId;
            dbObj.SecurityProfileName = this.SecurityProfileName;
            dbObj.EmployeeId = this.EmployeeId; //V2-877
            dbObj.Personnel = this.Personnel.ToDatabaseObject();

            dbObj.FilterText = this.FilterText;
            dbObj.FilterStartIndex = this.FilterStartIndex;
            dbObj.FilterEndIndex = this.FilterEndIndex;
            dbObj.LookupList = this.LookupList.ToDatabaseObjectExtended();
            dbObj.Craft = this.Craft.ToDatabaseObject();
            dbObj.ClientUpdateIndex = this.ClientUpdateIndex;


            return dbObj;
        }


        public b_UserDetails ToDatabaseObjectUserChange()
        {
            b_UserDetails dbObj = new b_UserDetails();
            dbObj.ClientId = this.ClientId;
            dbObj.CompanyName = this.CompanyName;
            dbObj.DefaultSiteId = this.DefaultSiteId;
            dbObj.UserInfoId = this.UserInfoId;
            dbObj.IsSuperUser = this.IsSuperUser;
            dbObj.UserUpdateIndex = this.UserUpdateIndex;
            dbObj.IsSiteAdmin = this.IsSiteAdmin;
            dbObj.UserType = this.UserType;
            dbObj.SecurityProfileId = this.SecurityProfileId;

            return dbObj;
        }

        public void UpdateFromDatabaseObject(b_UserDetails dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.CompanyName = dbObj.CompanyName;
            this.SiteControlled = dbObj.SiteControlled;

            this.UserInfoId = dbObj.UserInfoId;
            this.FirstName = dbObj.FirstName;
            this.LastName = dbObj.LastName;
            this.MiddleName = dbObj.MiddleName;
            this.Email = dbObj.Email;
            this.Localization = dbObj.Localization;
            this.TimeZone = dbObj.TimeZone;
            this.IsSuperUser = dbObj.IsSuperUser;
            this.UIConfiguration = dbObj.UIConfiguration;
            this.UserUpdateIndex = dbObj.UserUpdateIndex;
            this.ResultsPerPage = dbObj.ResultsPerPage;
            this.StartPage = dbObj.StartPage;
            this.IsPasswordTemporary = dbObj.IsPasswordTemporary;
            this.DefaultSiteId = dbObj.DefaultSiteId;

            this.LoginInfoId = dbObj.LoginInfoId;
            this.UserName = dbObj.UserName;
            this.Password = dbObj.Password;
            this.SecurityQuestion = dbObj.SecurityQuestion;
            this.SecurityResponse = dbObj.SecurityResponse;
            this.FailedAttempts = dbObj.FailedAttempts;
            this.LastFailureDate = dbObj.LastFailureDate;
            this.LastLoginDate = dbObj.LastLoginDate;
            this.IsActive = dbObj.IsActive;
            this.LoginUpdateIndex = dbObj.LoginUpdateIndex;
            this.ResetPasswordCode = dbObj.ResetPasswordCode;
            this.ResetPasswordRequestDate = dbObj.ResetPasswordRequestDate;
            this.TempPassword = dbObj.TempPassword;
            this.PackageLevel = dbObj.PackageLevel;
            //this.TabletUser = dbObj.TabletUser;
            //this.PhoneUser = dbObj.TabletUser;
            //this.WebAppUser = dbObj.WebAppUser;
            this.UserType = dbObj.UserType;
            this.SecurityProfileId = dbObj.SecurityProfileId;
            this.EmployeeId = dbObj.EmployeeId; //V2-877
            this.SecurityProfileName = dbObj.SecurityProfileName;
            //this.CountTabletUser = dbObj.CountTabletUser;
            //this.CountPhoneUser = dbObj.CountPhoneUser;
            this.CountWebAppUser = dbObj.CountWebAppUser;
            this.CountLimitedUser = dbObj.CountLimitedUser;
            this.CountWorkRequestUser = dbObj.CountWorkRequestUser;
            this.CountSanitationUser = dbObj.CountSanitationUser;
            this.CountSuperUser = dbObj.CountSuperUser;
            this.CountProdUser = dbObj.CountProdUser;
            this.CountSanAppUser = dbObj.CountSanAppUser;
            this.PersonnelId = dbObj.PersonnelId;

            Personnel personnel = new Personnel();
            personnel.UpdateFromDatabaseObject(dbObj.Personnel);
            this.Personnel = personnel;
            //--add sitename
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;
            this.SiteName = dbObj.Personnel.SiteName;
            LookupList lookuplist = new LookupList();
            lookuplist.UpdateFromDatabaseObjectExtended(dbObj.LookupList);
            this.LookupList = lookuplist;

            Craft craft = new Craft();
            craft.UpdateFromDatabaseObject(dbObj.Craft);
            this.Craft = craft;

            Department department = new Department();
            department.UpdateFromDatabaseObject(dbObj.Department);
            this.Department = department;
            this.Count = Count;

        }

        public void UpdateFromChangeUserAccessDatabaseObject(b_UserDetails dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.CompanyName = dbObj.CompanyName;
            this.SiteControlled = dbObj.SiteControlled;

            this.UserInfoId = dbObj.UserInfoId;
            this.FirstName = dbObj.FirstName;
            this.LastName = dbObj.LastName;
            this.MiddleName = dbObj.MiddleName;
            this.Email = dbObj.Email;
            this.Localization = dbObj.Localization;
            this.TimeZone = dbObj.TimeZone;
            this.IsSuperUser = dbObj.IsSuperUser;
            this.UIConfiguration = dbObj.UIConfiguration;
            this.UserUpdateIndex = dbObj.UserUpdateIndex;
            this.ResultsPerPage = dbObj.ResultsPerPage;
            this.StartPage = dbObj.StartPage;
            this.IsPasswordTemporary = dbObj.IsPasswordTemporary;
            this.DefaultSiteId = dbObj.DefaultSiteId;

            this.LoginInfoId = dbObj.LoginInfoId;
            this.UserName = dbObj.UserName;
            this.Password = dbObj.Password;
            this.SecurityQuestion = dbObj.SecurityQuestion;
            this.SecurityResponse = dbObj.SecurityResponse;
            this.FailedAttempts = dbObj.FailedAttempts;
            this.LastFailureDate = dbObj.LastFailureDate;
            this.LastLoginDate = dbObj.LastLoginDate;
            this.IsActive = dbObj.IsActive;
            this.LoginUpdateIndex = dbObj.LoginUpdateIndex;
            this.ResetPasswordCode = dbObj.ResetPasswordCode;
            this.ResetPasswordRequestDate = dbObj.ResetPasswordRequestDate;
            this.TempPassword = dbObj.TempPassword;
            this.PackageLevel = dbObj.PackageLevel;
            //this.TabletUser = dbObj.TabletUser;
            //this.PhoneUser = dbObj.TabletUser;
            //this.WebAppUser = dbObj.WebAppUser;
            this.UserType = dbObj.UserType;
            this.SecurityProfileId = dbObj.SecurityProfileId;
            this.SecurityProfileName = dbObj.SecurityProfileName;
            //this.CountTabletUser = dbObj.CountTabletUser;
            //this.CountPhoneUser = dbObj.CountPhoneUser;
            this.CountWebAppUser = dbObj.CountWebAppUser;
            this.CountLimitedUser = dbObj.CountLimitedUser;
            this.CountWorkRequestUser = dbObj.CountWorkRequestUser;
            this.CountSanitationUser = dbObj.CountSanitationUser;
            this.CountSuperUser = dbObj.CountSuperUser;
            this.CountSanAppUser = dbObj.CountSanAppUser;


            Personnel personnel = new Personnel();
            personnel.UpdateFromDatabaseObject(dbObj.Personnel);
            this.Personnel = personnel;
            //--add sitename
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;
            this.SiteName = dbObj.Personnel.SiteName;
            LookupList lookuplist = new LookupList();
            lookuplist.UpdateFromDatabaseObjectExtended(dbObj.LookupList);
            this.LookupList = lookuplist;

            Craft craft = new Craft();
            craft.UpdateFromDatabaseObject(dbObj.Craft);
            this.Craft = craft;

            Department department = new Department();
            department.UpdateFromDatabaseObject(dbObj.Department);
            this.Department = department;


            this.APM = dbObj.APM;
            this.CMMS = dbObj.CMMS;
            this.Sanitation = dbObj.Sanitation;
            this.Fleet = dbObj.Fleet;
            this.ProductGrouping = dbObj.ProductGrouping;
            this.Count = Count;

        }
        public void UpdateFromDatabaseObjectUserChange(b_UserDetails dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.CompanyName = dbObj.CompanyName;
            this.SiteControlled = dbObj.SiteControlled;
            this.UserInfoId = dbObj.UserInfoId;
            this.IsSuperUser = dbObj.IsSuperUser;
            this.UserUpdateIndex = dbObj.UserUpdateIndex;
            this.DefaultSiteId = dbObj.DefaultSiteId;
            this.UserType = dbObj.UserType;
            this.SecurityProfileId = dbObj.SecurityProfileId;
        }
        #endregion

        #region Transaction Methods
        public void RetrieveUserDetailsByUserInfoID(DatabaseKey dbKey)
        {
            UserDetails_RetrieveUserDetialsByUserID trans = new UserDetails_RetrieveUserDetialsByUserID()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            //  UpdateFromDatabaseObject(trans.UserDetailsList);
            UpdateFromDatabaseObject(trans.UserDetails);
        }

        public void RetrieveChangeUserAccessDetailsByUserInfoID(DatabaseKey dbKey)
        {
            UserDetails_RetrieveChangeUserAccessDetialsByUserID trans = new UserDetails_RetrieveChangeUserAccessDetialsByUserID()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            //  UpdateFromDatabaseObject(trans.UserDetailsList);
            UpdateFromChangeUserAccessDatabaseObject(trans.UserDetails);
        }

        public void CreateNewUserWithLoginData(DatabaseKey dbKey)
        {
            UserDetails_CreateNewUserWithLoginData trans = new UserDetails_CreateNewUserWithLoginData();
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.UserDetails);
        }

        public void CreateNewUserWithLoginData_V2(DatabaseKey dbKey)
        {
            UserDetails_CreateNewUserWithLoginData_V2 trans = new UserDetails_CreateNewUserWithLoginData_V2();
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.UserDetails);
        }

        //public void RetrievePersonnelByUserInfoId(DatabaseKey dbKey,string ClientConnectionString)
        //{
        //    UserDetails_RetrievePersonnelByUserInfoId trans = new UserDetails_RetrievePersonnelByUserInfoId()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName
        //    };
        //    trans.UserDetails = this.ToDatabaseObject();
        //    trans.ClientConnectionString = ClientConnectionString;
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.UseDatabase = DatabaseTypeEnum.Client;
        //    trans.Execute();

        //    UpdateFromDatabaseObject(trans.UserDetails);
        //}

        public void RetrievePersonnelByUserInfoId(DatabaseKey dbKey, string ClientConnectionString)
        {
            UserDetails_RetrievePersonnelByUserInfoId_V2 trans = new UserDetails_RetrievePersonnelByUserInfoId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.ClientConnectionString = ClientConnectionString;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseDatabase = DatabaseTypeEnum.Client;
            trans.Execute();

            UpdateFromDatabaseObject(trans.UserDetails);
        }
        // SOM-384
        public void RetrieveSiteUserCounts(DatabaseKey dbKey)
        {
            UserDetails_RetrieveSiteUserCounts trans = new UserDetails_RetrieveSiteUserCounts()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.UserDetails);
        }
        public void RetrieveSiteUserCounts_V2(DatabaseKey dbKey)
        {
            UserDetails_RetrieveSiteUserCounts_V2 trans = new UserDetails_RetrieveSiteUserCounts_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.UserDetails);
        }


        #endregion

        #region private Methods
        public void ValidateNewUserAdd(DatabaseKey dbKey)
        {
            ValidateFor = "NewUserAdd";
            Validate<UserDetails>(dbKey);
        }

        public void ValidateNewUserAdd_V2(DatabaseKey dbKey)
        {
            ValidateFor = "NewUserAdd_V2";
            Validate<UserDetails>(dbKey);
        }
        public void ValidateNewProductionUserAdd_V2(DatabaseKey dbKey)
        {
            ValidateFor = "NewProductionUserAdd_V2";
            Validate<UserDetails>(dbKey);
        }
        public void ValidateUserUpdate(DatabaseKey dbKey)
        {
            ValidateFor = "UserUpdate";
            Validate<UserDetails>(dbKey);
        }

        // SOM-384
        public void ValidateUserDelete(DatabaseKey dbKey)
        {
            ValidateFor = "UserDelete";
            Validate<UserDetails>(dbKey);
        }
        public void ValidateUserAccess_V2(DatabaseKey dbKey)
        {
            ValidateFor = "UserAccess_V2";
            Validate<UserDetails>(dbKey);
        }
        public void ValidateProductionUserAccess_V2(DatabaseKey dbKey)
        {
            ValidateFor = "ProductionUserAccess_V2";
            Validate<UserDetails>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (ValidateFor == "NewUserAdd")
            {
                UserDetails_ValidateNewUserAdd trans = new UserDetails_ValidateNewUserAdd()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
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
            else if (ValidateFor == "UserUpdate")
            {
                UserDetails_ValidateUserUpdate trans = new UserDetails_ValidateUserUpdate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
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
            // SOM-384
            // Currently only validating the personnel delete
            // The Login and UserInfo records can be deleted if the personnel record can be deleted
            else if (ValidateFor == "UserDelete")
            {
                UserDetails_ValidateUserDelete trans = new UserDetails_ValidateUserDelete()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.UserDetails = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                // Personnel Table is in the Client Database 
                // By setting the following - the client connection string will be used
                trans.UseDatabase = DatabaseTypeEnum.Client;
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
            else if (ValidateFor == "NewUserAdd_V2")
            {
                UserDetails_ValidateNewUserAdd_V2 trans = new UserDetails_ValidateNewUserAdd_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
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
            else if (ValidateFor == "NewProductionUserAdd_V2")
            {
                UserDetails_ValidateNewProductionUserAdd_V2 trans = new UserDetails_ValidateNewProductionUserAdd_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
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
            else if (ValidateFor == "UserAccess_V2")
            {
                UserDetails_ValidateUserAccess_V2 trans = new UserDetails_ValidateUserAccess_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
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
            else if (ValidateFor == "ProductionUserAccess_V2")
            {
                UserDetails_ValidateProductionUserAccess_V2 trans = new UserDetails_ValidateProductionUserAccess_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
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
            #region V2-417 Inactivate and Active Users           
            if (ValidateFor == "CheckIfInactivate")
            {
                UserDetails_ValidateByInactivate trans = new UserDetails_ValidateByInactivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.UserDetails = this.ToDatabaseObject();
                trans.UserDetails.ObjectName = this.ObjectName;
                trans.UserDetails.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }

            if (ValidateFor == "CheckIfActivate")
            {
                UserDetails_ValidateByActivate trans = new UserDetails_ValidateByActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.UserDetails = this.ToDatabaseObject();
                trans.UserDetails.ObjectName = this.ObjectName;
                trans.UserDetails.ObjectId = this.ObjectId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                errors = StoredProcValidationError.UpdateFromDatabaseObjects(trans.StoredProcValidationErrorList);

            }
            #endregion
            #region V2-419 Enterprise User Management - Add/Remove Sites
            else if (ValidateFor == "NewSiteForExistingUser_V2")
            {
                UserDetails_ValidateUseForAddSite_V2 trans = new UserDetails_ValidateUseForAddSite_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
                trans.UserDetails.ObjectName = this.ObjectName;
                trans.UserDetails.ObjectId = this.ObjectId;
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

            else if (ValidateFor == "RemoveSiteForExistingUserAdd_V2")
            {
                UserDetails_ValidateUseForRemoveSite_V2 trans = new UserDetails_ValidateUseForRemoveSite_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.UserDetails = this.ToDatabaseObject();
                trans.UserDetails.ObjectName = this.ObjectName;
                trans.UserDetails.ObjectId = this.ObjectId;
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
            #endregion

            return errors;
        }

        public void UpdateUserByUserInfoIdWithLoginData(DatabaseKey dbKey)
        {
            UserDetails_UpdateUserWithLogin trans = new UserDetails_UpdateUserWithLogin();
            trans.UserDetails = this.ToDatabaseObject();
            // trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.UserDetails);
        }

        public void UpdateUserByUserInfoIdWithLoginDataV2(DatabaseKey dbKey)
        {
            UserDetails_UpdateUserWithLoginV2 trans = new UserDetails_UpdateUserWithLoginV2();
            trans.UserDetails = this.ToDatabaseObject();
            // trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.UserDetails);
        }
        public void UpdateByUserInfoIdWithUserAccessV2(DatabaseKey dbKey)
        {
            UserDetails_UpdateByUserInfoIdWithUserAccessV2 trans = new UserDetails_UpdateByUserInfoIdWithUserAccessV2();
            trans.UserDetails = this.ToDatabaseObjectUserChange();
            // trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObjectUserChange(trans.UserDetails);
        }


        //---------------Getting List of personnel from admin site by applying connection string of client--------------
        public List<Personnel> RetrievePersonnelListByClientId(DatabaseKey dbKey, string ClientConnectionString)
        {
            RetrievePersonnelListByClientId trans = new RetrievePersonnelListByClientId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Personnel = this.Personnel.ToDatabaseObject();
            trans.ClientConnectionString = ClientConnectionString;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            return (this.Personnel.UpdateFromDatabaseObjectlist(trans.RetPersonnelList));
        }

        //---------------Getting List of LookupList from admin site by applying connection string of client--------------
        public List<KeyValuePair<string, string>> RetrieveLookupListByFilterText(DatabaseKey dbKey, string ClientConnectionString)
        {
            UserDetails_RetrieveLookupListByFilterText trans = new UserDetails_RetrieveLookupListByFilterText()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.ClientConnectionString = ClientConnectionString;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (trans.RetLookUpList);
        }

        //---------------Getting List of Craft from admin site by applying connection string of client--------------
        public List<Craft> RetrieveCraftByFilterText(DatabaseKey dbKey, string ClientConnectionString)
        {
            UserDetails_RetrieveCraftByFilterText trans = new UserDetails_RetrieveCraftByFilterText()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.ClientConnectionString = ClientConnectionString;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (Craft.UpdateFromDatabaseObjectList(trans.RetCraftList));
        }
        //---------------Getting List of Craft from admin site by applying connection string of client--------------
        public List<Department> RetrieveAllDepartment(DatabaseKey dbKey, string ClientConnectionString)
        {
            UserDetails_RetrieveAllDepartment trans = new UserDetails_RetrieveAllDepartment()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.ClientConnectionString = ClientConnectionString;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return (Department.UpdateFromDatabaseObjectList(trans.DepartmentList));
        }

        public List<UserDetails> RetrieveCountForUserExist(DatabaseKey dbKey)
        {
            Retrieve_CountforUser trans = new Retrieve_CountforUser()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.UserDetails);
            List<UserDetails> UserDetailsList = new List<UserDetails>();
            foreach (b_UserDetails UserDetails in trans.countList)
            {
                UserDetails tmpUserDetails = new UserDetails()
                {
                    Count = UserDetails.Count,
                };
                UserDetailsList.Add(tmpUserDetails);
            }
            return UserDetailsList;
        }

        public List<UserDetails> RetrieveValidateUserExists(DatabaseKey dbKey)
        {
            Retrieve_ValidateUserExists trans = new Retrieve_ValidateUserExists()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.UserDetails);
            List<UserDetails> UserDetailsList = new List<UserDetails>();
            foreach (b_UserDetails UserDetails in trans.countList)
            {
                UserDetails tmpUserDetails = new UserDetails()
                {
                    LoginInfoId = UserDetails.LoginInfoId,
                };
                UserDetailsList.Add(tmpUserDetails);
            }
            return UserDetailsList;
        }
        //------------------------------------------------------------------------------------------------------------
        #endregion

        public long ClientId { get; set; }
        public string CompanyName { get; set; }
        public bool SiteControlled { get; set; }

        public long UserInfoId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Localization { get; set; }
        public string TimeZone { get; set; }
        public bool IsSuperUser { get; set; }
        public string UIConfiguration { get; set; }
        public long UserUpdateIndex { get; set; }
        // public DateTime CreateDate { get; set; }
        //public string CreateBy { get; set; }
        //public DateTime ModifyDate { get; set; }
        // public string ModifyBy { get; set; }
        public int ResultsPerPage { get; set; }
        public string StartPage { get; set; }
        public bool IsPasswordTemporary { get; set; }
        public long DefaultSiteId { get; set; }

        public long LoginInfoId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityResponse { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime? LastFailureDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public long LoginUpdateIndex { get; set; }
        public System.Guid ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordRequestDate { get; set; }
        public string TempPassword { get; set; }
        // SOM-384
        //public bool TabletUser { get; set; }
        //public bool PhoneUser { get; set; }
        //public bool WebAppUser { get; set; }
        public string UserType { get; set; }
        public long SecurityProfileId { get; set; }
        public string SecurityProfileName { get; set; }

        public Personnel Personnel { get; set; }
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        public LookupList LookupList { get; set; }
        public Craft Craft { get; set; }
        public Department Department { get; set; }
        public long ClientUpdateIndex { get; set; }
        public bool CMMSUser { get; set; }
        public bool SanitationUser { get; set; }
        public string SiteName { get; set; }

        // SOM-384
        //public long CountTabletUser { get; set; }
        //public long CountPhoneUser { get; set; }
        public long CountWebAppUser { get; set; }
        public long CountLimitedUser { get; set; }
        public long CountWorkRequestUser { get; set; }
        public long CountSanitationUser { get; set; }
        public long CountSuperUser { get; set; }
        public long CountSanAppUser { get; set; }
        public long CountProdUser { get; set; }

        public Int32 Count { get; set; }
        public bool IsSiteAdmin { get; set; }
        //V2-402
        public string PackageLevel { get; set; }

        public string EmployeeId { get; set; } //V2-877

        public long SiteId { get; set; } //V2-903
        public long PersonnelId { get; set; } //V2-903

        #region V2-803
        public long LoginSSOId { get; set; }
        public string GmailId { get; set; }
        public string MicroSoftmailId { get; set; }
        public string WindowsADUserId { get; set; }
        #endregion

        #region V2-417 Inactivate and Active Users
        public void CheckUserIsInactivate(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfInactivate";
            Validate<UserDetails>(dbKey);
        }
        public void CheckUserIsActivate(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfActivate";
            Validate<UserDetails>(dbKey);
        }


        public void UpdateByPKForeignKeys_V2(DatabaseKey dbKey)
        {

            UserDetails_UpdateByForeignKeys_V2 trans = new UserDetails_UpdateByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.UserDetails);
        }

        #endregion

        #region V2-419 Enterprise User Management - Add/Remove Sites
        public void ValidateNewSiteForExistingUser_V2(DatabaseKey dbKey)
        {
            ValidateFor = "NewSiteForExistingUser_V2";
            Validate<UserDetails>(dbKey);
        }

        public void ValidateRemoveSiteForExistingUser_V2(DatabaseKey dbKey)
        {
            ValidateFor = "RemoveSiteForExistingUserAdd_V2";
            Validate<UserDetails>(dbKey);
        }
        #endregion

        public void RetrievePersonnelAndUserdetailsByUserInfoID(DatabaseKey dbKey)
        {
            UserDetails_RetrieveFromPersonnelAndUserdetailsByUserInfoId trans = new UserDetails_RetrieveFromPersonnelAndUserdetailsByUserInfoId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectForPersonnelAndUserdetails(trans.UserDetails);
        }
        public void UpdateFromDatabaseObjectForPersonnelAndUserdetails(b_UserDetails dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.CompanyName = dbObj.CompanyName;
            this.SiteControlled = dbObj.SiteControlled;

            this.UserInfoId = dbObj.UserInfoId;
            this.FirstName = dbObj.FirstName;
            this.LastName = dbObj.LastName;
            this.MiddleName = dbObj.MiddleName;
            this.Email = dbObj.Email;
            this.Localization = dbObj.Localization;
            this.TimeZone = dbObj.TimeZone;
            this.IsSuperUser = dbObj.IsSuperUser;
            this.UIConfiguration = dbObj.UIConfiguration;
            this.UserUpdateIndex = dbObj.UserUpdateIndex;
            this.ResultsPerPage = dbObj.ResultsPerPage;
            this.StartPage = dbObj.StartPage;
            this.IsPasswordTemporary = dbObj.IsPasswordTemporary;
            this.DefaultSiteId = dbObj.DefaultSiteId;

            this.LoginInfoId = dbObj.LoginInfoId;
            this.UserName = dbObj.UserName;
            this.Password = dbObj.Password;
            this.SecurityQuestion = dbObj.SecurityQuestion;
            this.SecurityResponse = dbObj.SecurityResponse;
            this.FailedAttempts = dbObj.FailedAttempts;
            this.LastFailureDate = dbObj.LastFailureDate;
            this.LastLoginDate = dbObj.LastLoginDate;
            this.IsActive = dbObj.IsActive;
            this.LoginUpdateIndex = dbObj.LoginUpdateIndex;
            this.ResetPasswordCode = dbObj.ResetPasswordCode;
            this.ResetPasswordRequestDate = dbObj.ResetPasswordRequestDate;
            this.TempPassword = dbObj.TempPassword;
            this.PackageLevel = dbObj.PackageLevel;
            //this.TabletUser = dbObj.TabletUser;
            //this.PhoneUser = dbObj.TabletUser;
            //this.WebAppUser = dbObj.WebAppUser;
            this.UserType = dbObj.UserType;
            this.SecurityProfileId = dbObj.SecurityProfileId;
            this.SecurityProfileName = dbObj.SecurityProfileName;
            //this.CountTabletUser = dbObj.CountTabletUser;
            //this.CountPhoneUser = dbObj.CountPhoneUser;
            this.CountWebAppUser = dbObj.CountWebAppUser;
            this.CountLimitedUser = dbObj.CountLimitedUser;
            this.CountWorkRequestUser = dbObj.CountWorkRequestUser;
            this.CountSanitationUser = dbObj.CountSanitationUser;
            this.CountSuperUser = dbObj.CountSuperUser;
            this.CountSanAppUser = dbObj.CountSanAppUser;


            Personnel personnel = new Personnel();
            personnel.UpdateFromDatabaseObject(dbObj.Personnel);
            this.Personnel = personnel;
            //--add sitename
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;
            this.SiteName = dbObj.Personnel.SiteName;
            LookupList lookuplist = new LookupList();
            lookuplist.UpdateFromDatabaseObjectExtended(dbObj.LookupList);
            this.LookupList = lookuplist;

            Craft craft = new Craft();
            craft.UpdateFromDatabaseObject(dbObj.Craft);
            this.Craft = craft;

            Department department = new Department();
            department.UpdateFromDatabaseObject(dbObj.Department);
            this.Department = department;


            this.APM = dbObj.APM;
            this.CMMS = dbObj.CMMS;
            this.Sanitation = dbObj.Sanitation;
            this.Fleet = dbObj.Fleet;
            this.ProductGrouping = dbObj.ProductGrouping;

            //V2-803
            this.LoginSSOId = dbObj.LoginSSOId;
            this.GmailId = dbObj.GMailId;
            this.MicroSoftmailId = dbObj.MicrosoftMailId;
            this.WindowsADUserId = dbObj.WindowsADUserId;
            this.EmployeeId=dbObj.EmployeeId;//V2-877

        }

        //#region V2-803
        //public void RetrieveLoginSSOByUserInfoId(DatabaseKey dbKey)
        //{
        //    RetrieveLoginSSOByUserInfoId_V2 trans = new RetrieveLoginSSOByUserInfoId_V2()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName
        //    };
        //    trans.UserDetails = this.ToDatabaseObject();
        //    trans.ClientConnectionString = ClientConnectionString;
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.UseDatabase = DatabaseTypeEnum.Client;
        //    trans.Execute();

        //    UpdateFromDatabaseObject(trans.UserDetails);
        //}
        //#endregion
        #region V2-962
        public void RetrieveSiteUserCountsForAdmin_V2(DatabaseKey dbKey)
        {
            UserDetails_RetrieveSiteUserCountsForAdmin_V2 trans = new UserDetails_RetrieveSiteUserCountsForAdmin_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserDetails = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.customClientId = this.ClientId;
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.UserDetails);
        }

        #endregion
    }

}
