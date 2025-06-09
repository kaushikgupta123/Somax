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
* 2014-Oct-21  SOM-384   Roger Lawton     Added class to retrievesiteusercounts
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Common.Enumerations;

namespace Data.Database
{
    public class UserDetails_TransactionBaseClass : AbstractTransactionManager
    {
        public UserDetails_TransactionBaseClass()
        {
            UseDatabase = DatabaseTypeEnum.Admin;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails == null)
            {
                string message = "UserDetails has not been set.";
                throw new Exception(message);
            }
            // SOM-384 - added following two lines
            CallerUserInfoId = dbKey.User.UserInfoId;
            CallerUserName = dbKey.UserName;
        }

        public b_UserDetails UserDetails { get; set; }

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

    public class UserDetails_RetrieveUserDetialsByUserID : AbstractTransactionManager
    {
        public UserDetails_RetrieveUserDetialsByUserID()
        {
            UseDatabase = DatabaseTypeEnum.Admin;
        }

        public List<b_UserDetails> UserDetailsList { get; set; }
        public b_UserDetails UserDetails { get; set; }

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
            UserDetails.RetrieveUserFullDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);         
        }
    }

    public class UserDetails_RetrieveChangeUserAccessDetialsByUserID : AbstractTransactionManager
    {
        public UserDetails_RetrieveChangeUserAccessDetialsByUserID()
        {
            UseDatabase = DatabaseTypeEnum.Admin;
        }

        public List<b_UserDetails> UserDetailsList { get; set; }
        public b_UserDetails UserDetails { get; set; }

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
            UserDetails.RetrieveChangeUserAccessFullDetails(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class UserDetails_ValidateNewUserAdd: UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateNewUserAdd()
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

                  UserDetails.ValidateNewUserAddFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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

    public class UserDetails_ValidateNewUserAdd_V2 : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateNewUserAdd_V2()
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

                UserDetails.ValidateNewUserAddFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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

    public class UserDetails_ValidateNewProductionUserAdd_V2 : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateNewProductionUserAdd_V2()
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

                UserDetails.ValidateNewProductionUserAddFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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
    public class UserDetails_ValidateUserAccess_V2 : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateUserAccess_V2()
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

                UserDetails.ValidateUserAccessFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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

    public class UserDetails_ValidateProductionUserAccess_V2 : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateProductionUserAccess_V2()
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

                UserDetails.ValidateProductionUserAccessFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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
    public class UserDetails_CreateNewUserWithLoginData : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId > 0)
            {
                string message = "User has invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            //Client.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            UserDetails.CreateNewUserWithLoginData(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserDetails.UserInfoId > 0);
        }
    }

    public class UserDetails_CreateNewUserWithLoginData_V2 : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId > 0)
            {
                string message = "User has invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            //Client.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            UserDetails.CreateNewUserWithLoginData_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(UserDetails.UserInfoId > 0);
        }
    }

    public class UserDetails_ValidateUserUpdate: UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateUserUpdate()
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

                UserDetails.ValidateUserUpdateFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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

    public class UserDetails_UpdateUserWithLogin : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId == 0)
            {
                string message = "User Info has an invalid ID.";
                throw new Exception(message);
            }
            if (UserDetails.LoginInfoId == 0)
            {
                string message = "Login Info has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserDetails.UpdateUserByUserIdWithLoginData(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            // if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    public class UserDetails_UpdateUserWithLoginV2 : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId == 0)
            {
                string message = "User Info has an invalid ID.";
                throw new Exception(message);
            }
            if (UserDetails.LoginInfoId == 0)
            {
                string message = "Login Info has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserDetails.UpdateUserByUserIdWithLoginDataV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            // if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }


    public class UserDetails_UpdateByUserInfoIdWithUserAccessV2 : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId == 0)
            {
                string message = "User Info has an invalid ID.";
                throw new Exception(message);
            }
            //if (UserDetails.LoginInfoId == 0)
            //{
            //    string message = "Login Info has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            UserDetails.UpdateByUserInfoIdWithUserAccessV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            // if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    //public class UserDetails_RetrievePersonnelByUserInfoId : UserDetails_TransactionBaseClass
    //{
    //    public string ClientConnectionString { get; set; }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //        if (UserDetails.UserInfoId == 0)
    //        {
    //            string message = "User Info has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //        //if (UserDetails.LoginInfoId == 0)
    //        //{
    //        //    string message = "Login Info has an invalid ID.";
    //        //    throw new Exception(message);
    //        //}
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        if (!string.IsNullOrEmpty(ClientConnectionString))
    //        {
    //            base.ConnectionString = ClientConnectionString;
    //        }

    //        UserDetails.RetrievePersonnelByUserInfoId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
    //        // If no have been made, no change log is created
    //        // if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
    //    }
    //}

    public class UserDetails_RetrievePersonnelByUserInfoId_V2 : UserDetails_TransactionBaseClass
    {
        public string ClientConnectionString { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId == 0)
            {
                string message = "User Info has an invalid ID.";
                throw new Exception(message);
            }
            //if (UserDetails.LoginInfoId == 0)
            //{
            //    string message = "Login Info has an invalid ID.";
            //    throw new Exception(message);
            //}
        }

        public override void PerformWorkItem()
        {
            if (!string.IsNullOrEmpty(ClientConnectionString))
            {
                base.ConnectionString = ClientConnectionString;
            }

            //UserDetails.RetrievePersonnelByUserInfoId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            UserDetails.RetrievePersonnelByUserInfoId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // If no have been made, no change log is created
            // if (ChangeLog != null) {ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }

    // SOM-384 - validate that personnel record can be deleted
    public class UserDetails_ValidateUserDelete : UserDetails_TransactionBaseClass
    {
      public UserDetails_ValidateUserDelete()
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

          UserDetails.ValidateUserDelete(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);

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


    public class UserDetails_RetrieveLookupListByFilterText : UserDetails_TransactionBaseClass
    {

        public List<KeyValuePair<string, string>> RetLookUpList { get; set; }
        public string ClientConnectionString { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            base.ConnectionString = ClientConnectionString;
        }

        public override void PerformWorkItem()
        {
            List<KeyValuePair<string, string>> tmpList = null;
            UserDetails.RetrieveLookupListByFilterText(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,ref tmpList);
            RetLookUpList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class UserDetails_RetrieveCraftByFilterText : UserDetails_TransactionBaseClass
    {

        public List<b_Craft> RetCraftList { get; set; }
        public string ClientConnectionString { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            base.ConnectionString = ClientConnectionString;
        }

        public override void PerformWorkItem()
        {
            List<b_Craft> tmpList = null;
            UserDetails.RetrieveCraftByFilterText(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            RetCraftList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class UserDetails_RetrieveAllDepartment : UserDetails_TransactionBaseClass
    {

        public List<b_Department> DepartmentList { get; set; }
        public string ClientConnectionString { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            base.ConnectionString = ClientConnectionString;
        }

        public override void PerformWorkItem()
        {
            b_Department[] tmpArray = null;
            UserDetails.RetrieveAllDepartmentFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            DepartmentList = new List<b_Department>(tmpArray);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }
    public class UserDetails_RetrieveSiteUserCounts : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            UserDetails.RetrieveSiteUserCounts(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class UserDetails_RetrieveSiteUserCounts_V2 : UserDetails_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            UserDetails.RetrieveSiteUserCounts_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class Retrieve_CountforUser : UserDetails_TransactionBaseClass
    {

        public List<b_UserDetails> countList { get; set; }

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
            List<b_UserDetails> tmpArray = null;
            UserDetails.RetrieveCountforUser(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            countList = new List<b_UserDetails>();
            foreach (b_UserDetails tmpObj in tmpArray)
            {
                countList.Add(tmpObj);
            }
        }
      

    }
    #region V2-1178
    public class Retrieve_ValidateUserExists : UserDetails_TransactionBaseClass
    {

        public List<b_UserDetails> countList { get; set; }

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
            List<b_UserDetails> tmpArray = null;
            UserDetails.RetrieveValidateUserExist(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            countList = new List<b_UserDetails>();
            foreach (b_UserDetails tmpObj in tmpArray)
            {
                countList.Add(tmpObj);
            }
        }


    }
    #endregion
    #region V2-417 Inactivate and Active Users

    public class UserDetails_ValidateByInactivate : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateByInactivate()
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


                UserDetails.ValidateByInactivate(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class UserDetails_ValidateByActivate : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateByActivate()
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


                UserDetails.ValidateByActivate(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors);

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

    public class UserDetails_UpdateByForeignKeys_V2 : UserDetails_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (UserDetails.UserInfoId == 0)
            {
                string message = "User has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            UserDetails.UpdateByForeignKeysInDatabase_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            //if (ChangeLog != null) { ChangeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
        }
    }
    #endregion

    #region V2-419 Enterprise User Management - Add/Remove Sites
    public class UserDetails_ValidateUseForAddSite_V2 : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateUseForAddSite_V2()
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

                UserDetails.ValidateUserForSiteAddFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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

    public class UserDetails_ValidateUseForRemoveSite_V2 : UserDetails_TransactionBaseClass
    {
        public UserDetails_ValidateUseForRemoveSite_V2()
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
                UserDetails.ValidateUserForSiteReomoveFromDatabase_V2(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName,
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
    #endregion

    public class UserDetails_RetrieveFromPersonnelAndUserdetailsByUserInfoId : AbstractTransactionManager
    {
        public UserDetails_RetrieveFromPersonnelAndUserdetailsByUserInfoId()
        {
            UseDatabase = DatabaseTypeEnum.Admin;
        }

        public List<b_UserDetails> UserDetailsList { get; set; }
        public b_UserDetails UserDetails { get; set; }

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
            b_UserDetails tmpResult = null;
            UserDetails.RetrieveFromPersonnelAndUserdetailsByUserInfoId_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpResult);
            UserDetails = tmpResult;
        }
    }

    #region V2-962
    public class UserDetails_RetrieveSiteUserCountsForAdmin_V2 : UserDetails_TransactionBaseClass
    {
        public long customClientId { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            UserDetails.ClientId = this.customClientId;
            UserDetails.RetrieveSiteUserCounts_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    #endregion
}
