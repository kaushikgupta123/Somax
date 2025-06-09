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
* 2014-Oct-21  SOM-384   Roger Lawton     Added method RetrieveSiteUserCounts
* 2014-Oct-22  SOM-384   Roger Lawton     Added method ValidateUserDelete
* 2014-Oct-24  SOM-384   Roger Lawton     Added LimitedUser Count
*                                         Comment the phone, tablet, webappuser properties
*                                         Comment the phonecount and tabletcount properties
**************************************************************************************************
*/
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Database.Business
{
    public partial class b_UserDetails
    {

        public b_UserDetails()
        {
            ClientId = 0;
            CompanyName = string.Empty;

            UserInfoId = 0;
            FirstName = string.Empty;
            LastName = string.Empty;
            MiddleName = string.Empty;
            Email = string.Empty;
            Localization = string.Empty;
            TimeZone = string.Empty;
            IsSuperUser = false;
            UIConfiguration = string.Empty;
            UserUpdateIndex = 0;
            ResultsPerPage = 0;
            StartPage = string.Empty;
            IsPasswordTemporary = false;
            DefaultSiteId = 0;

            LoginInfoId = 0;
            UserName = string.Empty;
            Password = string.Empty;
            SecurityQuestion = string.Empty;
            SecurityResponse = string.Empty;
            FailedAttempts = 0;
            LastFailureDate = new System.Nullable<System.DateTime>();
            LastLoginDate = DateTime.MinValue;
            IsActive = false;
            LoginUpdateIndex = 0;
            ResetPasswordCode = System.Guid.Empty;
            ResetPasswordRequestDate = new System.Nullable<System.DateTime>();
            TempPassword = String.Empty;
            //TabletUser = false;
            //PhoneUser = false;
            //WebAppUser = false;
            UserType = string.Empty;
            SecurityProfileId = 0;
            SecurityProfileName = string.Empty;

            Personnel = new b_Personnel();
            FilterText = string.Empty;
            FilterStartIndex = 0;
            FilterEndIndex = 0;
            LookupList = new b_LookupList();
            Craft = new b_Craft();
            Department = new b_Department();
        }

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
        //public bool TabletUser { get; set; }
        //public bool PhoneUser { get; set; }
        //public bool WebAppUser { get; set; }
        public string UserType { get; set; }
        public long SecurityProfileId { get; set; }
        public string SecurityProfileName { get; set; }

        public bool IsFirstUser { get; set; }

        public b_Personnel Personnel { get; set; }
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        public b_LookupList LookupList { get; set; }
        public b_Craft Craft { get; set; }
        public b_Department Department { get; set; }

        public long ClientUpdateIndex { get; set; }

        //public long CountTabletUser { get; set; }
        //public long CountPhoneUser { get; set; }
        public long CountWebAppUser { get; set; }
        public long CountLimitedUser { get; set; }
        public long CountWorkRequestUser { get; set; }
        public long CountSanitationUser { get; set; }
        public long CountSuperUser { get; set; }
        //V2-613 
        public long CountProdUser { get; set; }
        public bool CMMSUser { get; set; }
        public bool SanitationUser { get; set; }
        public Int32 Count { get; set; }
        public long CountSanAppUser { get; set; }
        //V2-401
        public bool IsSiteAdmin { get; set; }
        //V2-402
        public string PackageLevel { get; set; }

        #region V2-417 Inactivate and Active Users
        public string Flag { get; set; }
        public Int64 ObjectId { get; set; }
        public string ObjectName { get; set; }

        #endregion

        #region V2-487
        public bool APM { get; set; }
        public bool CMMS { get; set; }
        public bool Sanitation { get; set; }
        public bool Fleet { get; set; }

        public int ProductGrouping { get; set; }

        #endregion

        #region V2-803
        public long LoginSSOId { get; set; }
        public string GMailId { get; set; }
        public string MicrosoftMailId { get; set; }
        public string WindowsADUserId { get; set; }
        #endregion

        public string EmployeeId { get; set; } //V2-877
        public long SiteId { get; set; } //V2-903
        public long PersonnelId { get; set; } //V2-903
        public void LoadFromDatabase(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                CompanyName = reader.GetString(i++);

                SiteControlled = reader.GetBoolean(i++);


                UserInfoId = reader.GetInt64(i++);

                FirstName = reader.GetString(i++);

                LastName = reader.GetString(i++);

                MiddleName = reader.GetString(i++);

                Email = reader.GetString(i++);

                Localization = reader.GetString(i++);

                TimeZone = reader.GetString(i++);

                IsSuperUser = reader.GetBoolean(i++);

                UIConfiguration = reader.GetString(i++);

                UserUpdateIndex = reader.GetInt64(i++);

                ResultsPerPage = reader.GetInt32(i++);

                StartPage = reader.GetString(i++);

                IsPasswordTemporary = reader.GetBoolean(i++);

                DefaultSiteId = reader.GetInt64(i++);

                //TabletUser = reader.GetBoolean(i++);

                //PhoneUser = reader.GetBoolean(i++);

                //WebAppUser = reader.GetBoolean(i++);

                UserType = reader.GetString(i++);

                SecurityProfileId = reader.GetInt64(i++);

                EmployeeId = reader.GetString(i++); //    v2-877

                SecurityProfileName = reader.GetString(i++);

                LoginInfoId = reader.GetInt64(i++);

                UserName = reader.GetString(i++);

                Password = reader.GetString(i++);

                SecurityQuestion = reader.GetString(i++);

                SecurityResponse = reader.GetString(i++);

                FailedAttempts = reader.GetInt32(i++);

                // LastFailureDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastFailureDate = reader.GetDateTime(i);
                }
                else
                {
                    LastFailureDate = DateTime.MinValue;
                }
                i++;
                // LastLoginDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastLoginDate = reader.GetDateTime(i);
                }
                else
                {
                    LastLoginDate = DateTime.MinValue;
                }
                i++;

                IsActive = reader.GetBoolean(i++);

                LoginUpdateIndex = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ResetPasswordCode = reader.GetGuid(i);
                }
                else
                {
                    ResetPasswordCode = Guid.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ResetPasswordRequestDate = reader.GetDateTime(i);
                }
                else
                {
                    ResetPasswordRequestDate = DateTime.MinValue;
                }
                i++;

                TempPassword = reader.GetString(i++);

                PackageLevel = reader.GetString(i++);
                PersonnelId = reader.GetInt64(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["CompanyName"].ToString(); }
                catch { missing.Append("CompanyName "); }

                try { reader["FirstName"].ToString(); }
                catch { missing.Append("FristName "); }

                try { reader["LastName"].ToString(); }
                catch { missing.Append("LastName "); }

                try { reader["MiddleName"].ToString(); }
                catch { missing.Append("MiddleName "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                try { reader["Localization"].ToString(); }
                catch { missing.Append("Localization "); }

                try { reader["TimeZone"].ToString(); }
                catch { missing.Append("TimeZone "); }

                try { reader["IsSuperUser"].ToString(); }
                catch { missing.Append("IsSuperUser "); }

                try { reader["UIConfiguration"].ToString(); }
                catch { missing.Append("UIConfiguration "); }

                try { reader["ResultsPerPage"].ToString(); }
                catch { missing.Append("ResultsPerPage "); }

                try { reader["StartPage"].ToString(); }
                catch { missing.Append("StartPage "); }

                try { reader["IsPasswordTemporary"].ToString(); }
                catch { missing.Append("IsPasswordTemporary "); }

                try { reader["DefaultSiteId"].ToString(); }
                catch { missing.Append("DefaultSiteId "); }

                //try { reader["TabletUser"].ToString(); }
                //catch { missing.Append("TabletUser "); }

                //try { reader["PhoneUser"].ToString(); }
                //catch { missing.Append("PhoneUser "); }

                //try { reader["WebAppUser"].ToString(); }
                //catch { missing.Append("WebAppUser "); }

                try { reader["UserType"].ToString(); }
                catch { missing.Append("UserType "); }

                try { reader["SecurityProfileId"].ToString(); }
                catch { missing.Append("SecurityProfileId "); }

                try { reader["EmployeeId"].ToString(); }  //V2-877
                catch { missing.Append("EmployeeId "); }

                try { reader["SecurityProfileName"].ToString(); }
                catch { missing.Append("SecurityProfileName"); }

                try { reader["UserUpdateIndex"].ToString(); }
                catch { missing.Append("UserUpdateIndex "); }

                try { reader["LoginInfoId"].ToString(); }
                catch { missing.Append("LoginInfoId "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                try { reader["Password"].ToString(); }
                catch { missing.Append("Password "); }

                try { reader["SecurityQuestion"].ToString(); }
                catch { missing.Append("SecurityQuestion "); }

                try { reader["SecurityResponse"].ToString(); }
                catch { missing.Append("SecurityResponse "); }

                try { reader["FailedAttempts"].ToString(); }
                catch { missing.Append("FailedAttempts "); }

                try { reader["LastFailureDate"].ToString(); }
                catch { missing.Append("LastFailureDate "); }

                try { reader["LastLoginDate"].ToString(); }
                catch { missing.Append("LastLoginDate "); }

                try { reader["IsActive"].ToString(); }
                catch { missing.Append("IsActive "); }

                try { reader["LoginUpdateIndex"].ToString(); }
                catch { missing.Append("LoginUpdateIndex "); }

                try { reader["ResetPasswordCode"].ToString(); }
                catch { missing.Append("ResetPasswordCode "); }

                try { reader["ResetPasswordRequestDate"].ToString(); }
                catch { missing.Append("ResetPasswordRequestDate "); }

                try { reader["TempPassword"].ToString(); }
                catch { missing.Append("TempPassword "); }

                try { reader["PackageLevel"].ToString(); }
                catch { missing.Append("PackageLevel "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void LoadFromDatabaseChangeUserAccess(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                CompanyName = reader.GetString(i++);

                SiteControlled = reader.GetBoolean(i++);


                UserInfoId = reader.GetInt64(i++);

                FirstName = reader.GetString(i++);

                LastName = reader.GetString(i++);

                MiddleName = reader.GetString(i++);

                Email = reader.GetString(i++);

                Localization = reader.GetString(i++);

                TimeZone = reader.GetString(i++);

                IsSuperUser = reader.GetBoolean(i++);

                UIConfiguration = reader.GetString(i++);

                UserUpdateIndex = reader.GetInt64(i++);

                ResultsPerPage = reader.GetInt32(i++);

                StartPage = reader.GetString(i++);

                IsPasswordTemporary = reader.GetBoolean(i++);

                DefaultSiteId = reader.GetInt64(i++);

                //TabletUser = reader.GetBoolean(i++);

                //PhoneUser = reader.GetBoolean(i++);

                //WebAppUser = reader.GetBoolean(i++);

                UserType = reader.GetString(i++);

                SecurityProfileId = reader.GetInt64(i++);

                SecurityProfileName = reader.GetString(i++);

                LoginInfoId = reader.GetInt64(i++);

                UserName = reader.GetString(i++);

                Password = reader.GetString(i++);

                SecurityQuestion = reader.GetString(i++);

                SecurityResponse = reader.GetString(i++);

                FailedAttempts = reader.GetInt32(i++);

                // LastFailureDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastFailureDate = reader.GetDateTime(i);
                }
                else
                {
                    LastFailureDate = DateTime.MinValue;
                }
                i++;
                // LastLoginDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastLoginDate = reader.GetDateTime(i);
                }
                else
                {
                    LastLoginDate = DateTime.MinValue;
                }
                i++;

                IsActive = reader.GetBoolean(i++);

                LoginUpdateIndex = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ResetPasswordCode = reader.GetGuid(i);
                }
                else
                {
                    ResetPasswordCode = Guid.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ResetPasswordRequestDate = reader.GetDateTime(i);
                }
                else
                {
                    ResetPasswordRequestDate = DateTime.MinValue;
                }
                i++;

                TempPassword = reader.GetString(i++);

                PackageLevel = reader.GetString(i++);

                APM = reader.GetBoolean(i++);
                CMMS = reader.GetBoolean(i++);
                Sanitation = reader.GetBoolean(i++);
                Fleet = reader.GetBoolean(i++);
                ProductGrouping = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["CompanyName"].ToString(); }
                catch { missing.Append("CompanyName "); }

                try { reader["FirstName"].ToString(); }
                catch { missing.Append("FristName "); }

                try { reader["LastName"].ToString(); }
                catch { missing.Append("LastName "); }

                try { reader["MiddleName"].ToString(); }
                catch { missing.Append("MiddleName "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                try { reader["Localization"].ToString(); }
                catch { missing.Append("Localization "); }

                try { reader["TimeZone"].ToString(); }
                catch { missing.Append("TimeZone "); }

                try { reader["IsSuperUser"].ToString(); }
                catch { missing.Append("IsSuperUser "); }

                try { reader["UIConfiguration"].ToString(); }
                catch { missing.Append("UIConfiguration "); }

                try { reader["ResultsPerPage"].ToString(); }
                catch { missing.Append("ResultsPerPage "); }

                try { reader["StartPage"].ToString(); }
                catch { missing.Append("StartPage "); }

                try { reader["IsPasswordTemporary"].ToString(); }
                catch { missing.Append("IsPasswordTemporary "); }

                try { reader["DefaultSiteId"].ToString(); }
                catch { missing.Append("DefaultSiteId "); }

                //try { reader["TabletUser"].ToString(); }
                //catch { missing.Append("TabletUser "); }

                //try { reader["PhoneUser"].ToString(); }
                //catch { missing.Append("PhoneUser "); }

                //try { reader["WebAppUser"].ToString(); }
                //catch { missing.Append("WebAppUser "); }

                try { reader["UserType"].ToString(); }
                catch { missing.Append("UserType "); }

                try { reader["SecurityProfileId"].ToString(); }
                catch { missing.Append("SecurityProfileId "); }

                try { reader["SecurityProfileName"].ToString(); }
                catch { missing.Append("SecurityProfileName"); }

                try { reader["UserUpdateIndex"].ToString(); }
                catch { missing.Append("UserUpdateIndex "); }





                try { reader["LoginInfoId"].ToString(); }
                catch { missing.Append("LoginInfoId "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                try { reader["Password"].ToString(); }
                catch { missing.Append("Password "); }

                try { reader["SecurityQuestion"].ToString(); }
                catch { missing.Append("SecurityQuestion "); }

                try { reader["SecurityResponse"].ToString(); }
                catch { missing.Append("SecurityResponse "); }

                try { reader["FailedAttempts"].ToString(); }
                catch { missing.Append("FailedAttempts "); }

                try { reader["LastFailureDate"].ToString(); }
                catch { missing.Append("LastFailureDate "); }

                try { reader["LastLoginDate"].ToString(); }
                catch { missing.Append("LastLoginDate "); }

                try { reader["IsActive"].ToString(); }
                catch { missing.Append("IsActive "); }

                try { reader["ResetPasswordCode"].ToString(); }
                catch { missing.Append("ResetPasswordCode "); }

                try { reader["ResetPasswordRequestDate"].ToString(); }
                catch { missing.Append("ResetPasswordRequestDate "); }

                try { reader["TempPassword"].ToString(); }
                catch { missing.Append("TempPassword "); }

                try { reader["LoginUpdateIndex"].ToString(); }
                catch { missing.Append("LoginUpdateIndex "); }

                try { reader["APM"].ToString(); }
                catch { missing.Append("APM "); }

                try { reader["CMMS"].ToString(); }
                catch { missing.Append("CMMS "); }

                try { reader["Sanitation"].ToString(); }
                catch { missing.Append("Sanitation "); }

                try { reader["Fleet"].ToString(); }
                catch { missing.Append("Fleet "); }

                try { reader["ProductGrouping"].ToString(); }
                catch { missing.Append("ProductGrouping "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveUserFullDetails(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_UserDetails> processRow = null;
            // ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            // data = new b_UserDetails[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_UserDetails>(reader => { b_UserDetails obj = new b_UserDetails(); obj.LoadFromDatabase(reader); return obj; });
                //results = Database.StoredProcedure.usp_UserData_UserDetailsByUserId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
                processRow = new Database.SqlClient.ProcessRow<b_UserDetails>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_UserData_UserDetailsByUserId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                //if (null != results)
                //{
                //    data = (b_UserDetails[])results.ToArray(typeof(b_UserDetails));
                //}
                //else
                //{
                //    data = new b_UserDetails[0];
                //}

                //// Clear the results collection
                //if (null != results)
                //{
                //    results.Clear();
                //    results = null;
                //}
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                //results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void RetrieveChangeUserAccessFullDetails(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_UserDetails> processRow = null;
            // ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            // data = new b_UserDetails[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_UserDetails>(reader => { b_UserDetails obj = new b_UserDetails(); obj.LoadFromDatabase(reader); return obj; });
                //results = Database.StoredProcedure.usp_UserData_UserDetailsByUserId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);
                processRow = new Database.SqlClient.ProcessRow<b_UserDetails>(reader => { this.LoadFromDatabaseChangeUserAccess(reader); return this; });
                Database.StoredProcedure.usp_UserData_ChangeUserAccessDetailsByUserId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                //if (null != results)
                //{
                //    data = (b_UserDetails[])results.ToArray(typeof(b_UserDetails));
                //}
                //else
                //{
                //    data = new b_UserDetails[0];
                //}

                //// Clear the results collection
                //if (null != results)
                //{
                //    results.Clear();
                //    results = null;
                //}
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                //results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void ValidateNewUserAddFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_StoredProcValidationError> data
      )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateNewUserAdd.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        public void ValidateNewUserAddFromDatabase_V2(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_StoredProcValidationError> data
 )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateNewUserAdd_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void ValidateNewProductionUserAddFromDatabase_V2(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateNewProductionUserAdd_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void ValidateUserAccessFromDatabase_V2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateUserAccess_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void ValidateProductionUserAccessFromDatabase_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateProductionUserAccess_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        public void CreateNewUserWithLoginData(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
    string callerUserName
    )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserData_CreateNewUserWithLoginData.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void CreateNewUserWithLoginData_V2(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
    string callerUserName
    )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserData_CreateNewUserWithLoginData_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void ValidateUserUpdateFromDatabase(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_StoredProcValidationError> data
 )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateUserUpdate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        public void UpdateUserByUserIdWithLoginData(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserData_UpdateByUserInfoIdWithLogin.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        public void UpdateUserByUserIdWithLoginDataV2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserData_UpdateByUserInfoIdWithLogin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }


        public void UpdateByUserInfoIdWithUserAccessV2(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName
   )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_UserData_UpdateByUserInfoIdWithUserAccess_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }
        public void RetrievePersonnelByUserInfoId(
                   SqlConnection connection,
                   SqlTransaction transaction,
                   long callerUserInfoId,
                   string callerUserName
               )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<Object> results = null;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Personnel_RetrieveByUserInfoId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (results.Count > 0 && results[0] != null)
                {
                    Personnel = (b_Personnel)results[0];
                }
                else
                {
                    Personnel = new b_Personnel();
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrievePersonnelByUserInfoId_V2(
                  SqlConnection connection,
                  SqlTransaction transaction,
                  long callerUserInfoId,
                  string callerUserName
              )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<Object> results = null;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Personnel_RetrieveByUserInfoId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (results.Count > 0 && results[0] != null)
                {
                    Personnel = (b_Personnel)results[0];
                }
                else
                {
                    Personnel = new b_Personnel();
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        // SOM-384 
        // NOTICE THIS IS CALLING A STORED PROCEDURE FROM THE CLIENT DATABASE
        public void ValidateUserDelete(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to perform the validation and return any errors
                results = Database.StoredProcedure.usp_Personnel_ValidateDelete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public void RetrieveLookupListByFilterText(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
           ref List<KeyValuePair<String, string>> results

           )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_LookupList_RetrieveByFilterText.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, FilterText, FilterStartIndex, FilterEndIndex, LookupList.Filter, LookupList.ListName);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void RetrieveCraftByFilterText(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_Craft> results
            )
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_Craft_RetrieveBYFilterText.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, FilterText, FilterStartIndex, FilterEndIndex);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }



        public void RetrieveAllDepartmentFromDatabase(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
         string callerUserName,
               ref b_Department[] data
           )
        {
            Database.SqlClient.ProcessRow<b_Department> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Department[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Department>(reader => { b_Department obj = new b_Department(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Department_RetrieveAll.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Department[])results.ToArray(typeof(b_Department));
                }
                else
                {
                    data = new b_Department[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        //SOM-384
        public void RetrieveSiteUserCounts(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;
            string message = String.Empty;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                Database.StoredProcedure.usp_UserData_RetrieveSiteUserCounts.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        public void RetrieveSiteUserCounts_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;
            string message = String.Empty;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                Database.StoredProcedure.usp_UserData_RetrieveSiteUserCounts_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static object ProcessRowForCount(SqlDataReader reader)
        {
            b_UserDetails obj = new b_UserDetails();
            obj.LoadFromDatabaseCustom(reader);
            return (object)obj;
        }


        public int LoadFromDatabaseCustom(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                Count = reader.GetInt32(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["Count"].ToString(); }
                catch { missing.Append("Count "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }


        public static object ProcessRowForLoginInfoId(SqlDataReader reader)
        {
            b_UserDetails obj = new b_UserDetails();
            obj.LoadFromDatabaseCustomLoginInfoId(reader);
            return (object)obj;
        }


        public int LoadFromDatabaseCustomLoginInfoId(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                LoginInfoId = reader.GetInt64(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["LoginInfoId"].ToString(); }
                catch { missing.Append("LoginInfoId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }

        public void RetrieveCountforUser(SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_UserDetails> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_UserDetails> results = null;
            data = new List<b_UserDetails>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_CountIfUserExist.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_UserDetails>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #region V2-1178
        public void RetrieveValidateUserExist(SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_UserDetails> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_UserDetails> results = null;
            data = new List<b_UserDetails>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateUserExist_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_UserDetails>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }
        #endregion
        #region V2-417 Inactivate and Active Users      
        //InActive
        public void ValidateByInactivate(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_StoredProcValidationError> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_UserData_ValidateInActive_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }

        //Active
        public void ValidateByActivate(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_StoredProcValidationError> data
       )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = StoredProcedure.usp_UserData_ValidateActive_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        //
        public void UpdateByForeignKeysInDatabase_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                StoredProcedure.usp_UserData_UpdateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        #endregion

        #region V2-419 Enterprise User Management - Add/Remove Sites
        public void ValidateUserForSiteAddFromDatabase_V2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateAddUserToAnySite_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }


        public void ValidateUserForSiteReomoveFromDatabase_V2(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_StoredProcValidationError> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_UserData_ValidateRemoveUserToAnySite_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_StoredProcValidationError>();
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;

            }
        }
        #endregion

        public void RetrieveFromPersonnelAndUserdetailsByUserInfoId_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref b_UserDetails results
      )
        {

            SqlCommand command = null;
            string message = String.Empty;
            //  List<b_UserDetails> results = null;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_RetrieveFromPersonnelAndUserdetailsByUserInfoId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }

        public static b_UserDetails ProcessRowForPersonnelAndUserdetails(SqlDataReader reader)
        {
            b_UserDetails obj = new b_UserDetails();
            obj.LoadFromDatabaseForPersonnelAndUserdetails(reader);
            return (b_UserDetails)obj;
        }
        public int LoadFromDatabaseForPersonnelAndUserdetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);

                CompanyName = reader.GetString(i++);

                SiteControlled = reader.GetBoolean(i++);


                UserInfoId = reader.GetInt64(i++);

                FirstName = reader.GetString(i++);

                LastName = reader.GetString(i++);

                MiddleName = reader.GetString(i++);

                Email = reader.GetString(i++);

                Localization = reader.GetString(i++);

                TimeZone = reader.GetString(i++);

                IsSuperUser = reader.GetBoolean(i++);

                UIConfiguration = reader.GetString(i++);

                UserUpdateIndex = reader.GetInt64(i++);

                ResultsPerPage = reader.GetInt32(i++);

                StartPage = reader.GetString(i++);

                IsPasswordTemporary = reader.GetBoolean(i++);

                DefaultSiteId = reader.GetInt64(i++);

                //TabletUser = reader.GetBoolean(i++);

                //PhoneUser = reader.GetBoolean(i++);

                //WebAppUser = reader.GetBoolean(i++);

                UserType = reader.GetString(i++);

                SecurityProfileId = reader.GetInt64(i++);

                EmployeeId = reader.GetString(i++); //V2-877

                SecurityProfileName = reader.GetString(i++);

                LoginInfoId = reader.GetInt64(i++);

                UserName = reader.GetString(i++);

                Password = reader.GetString(i++);

                SecurityQuestion = reader.GetString(i++);

                SecurityResponse = reader.GetString(i++);

                FailedAttempts = reader.GetInt32(i++);

                // LastFailureDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastFailureDate = reader.GetDateTime(i);
                }
                else
                {
                    LastFailureDate = DateTime.MinValue;
                }
                i++;
                // LastLoginDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastLoginDate = reader.GetDateTime(i);
                }
                else
                {
                    LastLoginDate = DateTime.MinValue;
                }
                i++;

                IsActive = reader.GetBoolean(i++);

                LoginUpdateIndex = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ResetPasswordCode = reader.GetGuid(i);
                }
                else
                {
                    ResetPasswordCode = Guid.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ResetPasswordRequestDate = reader.GetDateTime(i);
                }
                else
                {
                    ResetPasswordRequestDate = DateTime.MinValue;
                }
                i++;

                TempPassword = reader.GetString(i++);

                PackageLevel = reader.GetString(i++);

                APM = reader.GetBoolean(i++);

                CMMS = reader.GetBoolean(i++);

                Sanitation = reader.GetBoolean(i++);

                Fleet = reader.GetBoolean(i++);

                ProductGrouping = reader.GetInt32(i++);

                // ClientId column, bigint, not null
                Personnel.ClientId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                Personnel.PersonnelId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                Personnel.SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                Personnel.AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                Personnel.DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                Personnel.StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                Personnel.ClientLookupId = reader.GetString(i++);

                // Address1 column, nvarchar(63), not null
                Personnel.Address1 = reader.GetString(i++);

                // Address2 column, nvarchar(63), not null
                Personnel.Address2 = reader.GetString(i++);

                // Address3 column, nvarchar(63), not null
                Personnel.Address3 = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                Personnel.AddressCity = reader.GetString(i++);

                // AddressCountry column, nvarchar(63), not null
                Personnel.AddressCountry = reader.GetString(i++);

                // AddressPostCode column, nvarchar(31), not null
                Personnel.AddressPostCode = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                Personnel.AddressState = reader.GetString(i++);

                // ApprovalLimitPO column, decimal(15,0), not null
                Personnel.ApprovalLimitPO = reader.GetDecimal(i++);

                // ApprovalLimitWO column, decimal(15,0), not null
                Personnel.ApprovalLimitWO = reader.GetDecimal(i++);

                // BasePay column, decimal(10,2), not null
                Personnel.BasePay = reader.GetDecimal(i++);

                // CraftId column, bigint, not null
                Personnel.CraftId = reader.GetInt64(i++);

                // Crew column, nvarchar(15), not null
                Personnel.Crew = reader.GetString(i++);

                // CurrentLevel column, nvarchar(15), not null
                Personnel.CurrentLevel = reader.GetString(i++);

                // DateofBirth column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Personnel.DateofBirth = reader.GetDateTime(i);
                }
                else
                {
                    Personnel.DateofBirth = DateTime.MinValue;
                }
                i++;
                // Default_StoreroomId column, bigint, not null
                Personnel.Default_StoreroomId = reader.GetInt64(i++);

                // DistancefromWork column, decimal(6,2), not null
                Personnel.DistancefromWork = reader.GetDecimal(i++);

                // Email column, nvarchar(255), not null
                Personnel.Email = reader.GetString(i++);

                // Floater column, bit, not null
                Personnel.Floater = reader.GetBoolean(i++);

                // Gender column, nvarchar(15), not null
                Personnel.Gender = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                Personnel.InactiveFlag = reader.GetBoolean(i++);

                // InitialLevel column, nvarchar(15), not null
                Personnel.InitialLevel = reader.GetString(i++);

                // InitialPay column, decimal(10,2), not null
                Personnel.InitialPay = reader.GetDecimal(i++);

                // LastSalaryReview column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Personnel.LastSalaryReview = reader.GetDateTime(i);
                }
                else
                {
                    Personnel.LastSalaryReview = DateTime.MinValue;
                }
                i++;
                // MaritalStatus column, nvarchar(15), not null
                Personnel.MaritalStatus = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                Personnel.NameFirst = reader.GetString(i++);

                // NameLast column, nvarchar(31), not null
                Personnel.NameLast = reader.GetString(i++);

                // NameMiddle column, nvarchar(31), not null
                Personnel.NameMiddle = reader.GetString(i++);

                // Phone1 column, nvarchar(31), not null
                Personnel.Phone1 = reader.GetString(i++);

                // Phone2 column, nvarchar(31), not null
                Personnel.Phone2 = reader.GetString(i++);

                // Planner column, bit, not null
                Personnel.Planner = reader.GetBoolean(i++);

                // Scheduler column, bit, not null
                Personnel.Scheduler = reader.GetBoolean(i++);

                // ScheduleEmployee column, bit, not null
                Personnel.ScheduleEmployee = reader.GetBoolean(i++);

                // Section column, nvarchar(15), not null
                Personnel.Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Personnel.Shift = reader.GetString(i++);

                // SocialSecurityNumber column, nvarchar(15), not null
                Personnel.SocialSecurityNumber = reader.GetString(i++);

                // StartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Personnel.StartDate = reader.GetDateTime(i);
                }
                else
                {
                    Personnel.StartDate = DateTime.MinValue;
                }
                i++;
                // Status column, nvarchar(15), not null
                Personnel.Status = reader.GetString(i++);

                // Supervisor_PersonnelId column, bigint, not null
                Personnel.Supervisor_PersonnelId = reader.GetInt64(i++);

                // TerminationDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    Personnel.TerminationDate = reader.GetDateTime(i);
                }
                else
                {
                    Personnel.TerminationDate = DateTime.MinValue;
                }
                i++;
                // TerminationReason column, nvarchar(15), not null
                Personnel.TerminationReason = reader.GetString(i++);

                // Buyer column, bit, not null
                Personnel.Buyer = reader.GetBoolean(i++);

                // ExOracleUserId column, nvarchar(63), not null
                Personnel.ExOracleUserId = reader.GetString(i++);

                //Site name
                Personnel.SiteName = reader.GetString(i++);

                //LoginSSOId
                if (false == reader.IsDBNull(i))
                {
                    LoginSSOId = reader.GetInt64(i);
                }
                else
                {
                    LoginSSOId = 0;
                }
                i++;

                //GmailId
                if (false == reader.IsDBNull(i))
                {
                    GMailId = reader.GetString(i);
                }
                else
                {
                    GMailId = "";
                }
                i++;

                //MicrosoftId
                if (false == reader.IsDBNull(i))
                {
                    MicrosoftMailId = reader.GetString(i);
                }
                else
                {
                    MicrosoftMailId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WindowsADUserId = reader.GetString(i);
                }
                else
                {
                    WindowsADUserId = "";
                }
                i++;

                // UpdateIndex column, int, not null
                Personnel.UpdateIndex = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["CompanyName"].ToString(); }
                catch { missing.Append("CompanyName "); }

                try { reader["FirstName"].ToString(); }
                catch { missing.Append("FristName "); }

                try { reader["LastName"].ToString(); }
                catch { missing.Append("LastName "); }

                try { reader["MiddleName"].ToString(); }
                catch { missing.Append("MiddleName "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                try { reader["Localization"].ToString(); }
                catch { missing.Append("Localization "); }

                try { reader["TimeZone"].ToString(); }
                catch { missing.Append("TimeZone "); }

                try { reader["IsSuperUser"].ToString(); }
                catch { missing.Append("IsSuperUser "); }

                try { reader["UIConfiguration"].ToString(); }
                catch { missing.Append("UIConfiguration "); }

                try { reader["ResultsPerPage"].ToString(); }
                catch { missing.Append("ResultsPerPage "); }

                try { reader["StartPage"].ToString(); }
                catch { missing.Append("StartPage "); }

                try { reader["IsPasswordTemporary"].ToString(); }
                catch { missing.Append("IsPasswordTemporary "); }

                try { reader["DefaultSiteId"].ToString(); }
                catch { missing.Append("DefaultSiteId "); }

                //try { reader["TabletUser"].ToString(); }
                //catch { missing.Append("TabletUser "); }

                //try { reader["PhoneUser"].ToString(); }
                //catch { missing.Append("PhoneUser "); }

                //try { reader["WebAppUser"].ToString(); }
                //catch { missing.Append("WebAppUser "); }

                try { reader["UserType"].ToString(); }
                catch { missing.Append("UserType "); }

                try { reader["SecurityProfileId"].ToString(); }
                catch { missing.Append("SecurityProfileId "); }

                try { reader["SecurityProfileName"].ToString(); }
                catch { missing.Append("SecurityProfileName"); }

                try { reader["EmployeeId"].ToString(); }  //V2-877
                catch { missing.Append("EmployeeId "); }

                try { reader["UserUpdateIndex"].ToString(); }
                catch { missing.Append("UserUpdateIndex "); }

                try { reader["LoginInfoId"].ToString(); }
                catch { missing.Append("LoginInfoId "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                try { reader["Password"].ToString(); }
                catch { missing.Append("Password "); }

                try { reader["SecurityQuestion"].ToString(); }
                catch { missing.Append("SecurityQuestion "); }

                try { reader["SecurityResponse"].ToString(); }
                catch { missing.Append("SecurityResponse "); }

                try { reader["FailedAttempts"].ToString(); }
                catch { missing.Append("FailedAttempts "); }

                try { reader["LastFailureDate"].ToString(); }
                catch { missing.Append("LastFailureDate "); }

                try { reader["LastLoginDate"].ToString(); }
                catch { missing.Append("LastLoginDate "); }

                try { reader["IsActive"].ToString(); }
                catch { missing.Append("IsActive "); }

                try { reader["ResetPasswordCode"].ToString(); }
                catch { missing.Append("ResetPasswordCode "); }

                try { reader["ResetPasswordRequestDate"].ToString(); }
                catch { missing.Append("ResetPasswordRequestDate "); }

                try { reader["TempPassword"].ToString(); }
                catch { missing.Append("TempPassword "); }

                try { reader["LoginUpdateIndex"].ToString(); }
                catch { missing.Append("LoginUpdateIndex "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                //try { reader["UserInfoId"].ToString(); }
                //catch { missing.Append("UserInfoId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Address1"].ToString(); }
                catch { missing.Append("Address1 "); }

                try { reader["Address2"].ToString(); }
                catch { missing.Append("Address2 "); }

                try { reader["Address3"].ToString(); }
                catch { missing.Append("Address3 "); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity "); }

                try { reader["AddressCountry"].ToString(); }
                catch { missing.Append("AddressCountry "); }

                try { reader["AddressPostCode"].ToString(); }
                catch { missing.Append("AddressPostCode "); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState "); }

                try { reader["ApprovalLimitPO"].ToString(); }
                catch { missing.Append("ApprovalLimitPO "); }

                try { reader["ApprovalLimitWO"].ToString(); }
                catch { missing.Append("ApprovalLimitWO "); }

                try { reader["BasePay"].ToString(); }
                catch { missing.Append("BasePay "); }

                try { reader["CraftId"].ToString(); }
                catch { missing.Append("CraftId "); }

                try { reader["Crew"].ToString(); }
                catch { missing.Append("Crew "); }

                try { reader["CurrentLevel"].ToString(); }
                catch { missing.Append("CurrentLevel "); }

                try { reader["DateofBirth"].ToString(); }
                catch { missing.Append("DateofBirth "); }

                try { reader["Default_StoreroomId"].ToString(); }
                catch { missing.Append("Default_StoreroomId "); }

                try { reader["DistancefromWork"].ToString(); }
                catch { missing.Append("DistancefromWork "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                try { reader["Floater"].ToString(); }
                catch { missing.Append("Floater "); }

                try { reader["Gender"].ToString(); }
                catch { missing.Append("Gender "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["InitialLevel"].ToString(); }
                catch { missing.Append("InitialLevel "); }

                try { reader["InitialPay"].ToString(); }
                catch { missing.Append("InitialPay "); }

                try { reader["LastSalaryReview"].ToString(); }
                catch { missing.Append("LastSalaryReview "); }

                try { reader["MaritalStatus"].ToString(); }
                catch { missing.Append("MaritalStatus "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameMiddle"].ToString(); }
                catch { missing.Append("NameMiddle "); }

                try { reader["Phone1"].ToString(); }
                catch { missing.Append("Phone1 "); }

                try { reader["Phone2"].ToString(); }
                catch { missing.Append("Phone2 "); }

                try { reader["Planner"].ToString(); }
                catch { missing.Append("Planner "); }

                try { reader["Scheduler"].ToString(); }
                catch { missing.Append("Scheduler "); }

                try { reader["ScheduleEmployee"].ToString(); }
                catch { missing.Append("ScheduleEmployee "); }

                try { reader["Section"].ToString(); }
                catch { missing.Append("Section "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["SocialSecurityNumber"].ToString(); }
                catch { missing.Append("SocialSecurityNumber "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Supervisor_PersonnelId"].ToString(); }
                catch { missing.Append("Supervisor_PersonnelId "); }

                try { reader["TerminationDate"].ToString(); }
                catch { missing.Append("TerminationDate "); }

                try { reader["TerminationReason"].ToString(); }
                catch { missing.Append("TerminationReason "); }

                try { reader["Buyer"].ToString(); }
                catch { missing.Append("Buyer "); }

                try { reader["ExOracleUserId"].ToString(); }
                catch { missing.Append("ExOracleUserId "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["LoginSSOId"].ToString(); }
                catch { missing.Append("LoginSSOId "); }

                try { reader["GMailId"].ToString(); }
                catch { missing.Append("GMailId "); }

                try { reader["MicrosoftMailId"].ToString(); }
                catch { missing.Append("MicrosoftMailId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
            return i;
        }
    }
}
