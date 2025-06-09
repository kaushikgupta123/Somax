/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person            Description
* =========== ======== ================== =========================================================
* 2014-Oct-16 SOM-369  Roger Lawton       Added columns when processing for a lookup list
*                                         These columns are used to extract personnel based on the 
*                                         value of the columns (Scheduler, Buyer, ScheduleEmployee)
*                                         We will probably add new columns later.
* 2015-Feb-09 SOM-536  Roger Lawton       Added UserSec datatable property - used by the stored 
*                                         procedure for the RetrieveForLookupListBySecurityItem 
*                                         method
* 2016-Oct-31 SOM-642  Roger Lawton       Update personnel info if appropriate
****************************************************************************************************
 */

using System;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;
using Database.SqlClient;
using System.Reflection;
using System.IO.Ports;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the TechSpecs table.
    /// </summary>
    public partial class b_Personnel
    {
        /// <summary>   
        /// Supervisor_ClientLookupId - property
        /// </summary>
        public string Supervisor_ClientLookupId { get; set; }

        /// <summary>   
        /// DefaultStoreRoom_Name - property
        /// </summary>
        public string DefaultStoreRoom_Name { get; set; }
        // SOM-1688
        //public List<b_FileInfo> FileInfo { get; set; }
        public List<b_Notes> Notes { get; set; }
        public List<b_Contact> Contacts { get; set; }
        public string TableName { get; set; }
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        public string CraftDescription { get; set; }
        public string SiteDescription { get; set; }
        // V2-478
        public string SecItems { get; set; }
        public string SecProps { get; set; }
        //public bool ItemAccess { get; set; }
        //public bool ItemCreate { get; set; }
        //public bool ItemEdit { get; set; }
        //public bool ItemDelete { get; set; }
        //public string ItemName { get; set; }
        public string ItemName { get; set; }
        public string SiteName { get; set; }
        // SOM-536 
        public System.Data.DataTable UserSec { get; set; }

        //V2-276
        public string Searchtext { get; set; }
        public string FullName { get; set; }
        private string InitFirstName;
        private string InitLastName;
        //V2-720
        public string requestType { get; set; }
        public Int64 approvalGroupId { get; set; }

        public Int64 LoginSSOId { get; set; }


        public string PersonnelInitial
        {

            get
            {
                if (!string.IsNullOrEmpty(NameFirst))
                { InitFirstName = NameFirst.Trim().Substring(0, 1); }

                if (!string.IsNullOrEmpty(NameLast))
                { InitLastName = NameLast.Trim().Substring(0, 1); }

                return (InitFirstName + InitLastName);
            }
        }
        public string UserName { get; set; }

        public Int64 EventsId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? CompleteDate { get; set; }
        public DateTime? ExpireDate { get; set; }

        //---------------------------------------------------------------------------------------------------------
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public string Name { get; set; }
        public string ShiftDescription { get; set; }
        public string ScheduleGroupDescription { get; set; }
        public string CrewDescription { get; set; }
        public string CraftClientLookupId { get; set; }
        public int TotalCount { get; set; }
        public List<b_Personnel> listOfPersonnels { get; set; }
        public Int64 TimecardId { get; set; }
        public decimal Hours { get; set; }
        public decimal Value { get; set; }
        public string WOClientLookupId { get; set; }
        public DateTime laborstartdate { get; set; }
        public DateTime PersonnelAvailabilityDate { get; set; }
        public decimal PAHours { get; set; }
        public string PAShift { get; set; }
        public Int64 PersonnelAvailabilityId { get; set; }
        public DateTime PersonnelAttendDate { get; set; }
        public decimal PersonnelAttendHours { get; set; }
        public string PersonnelAttendShift { get; set; }
        public string PersonnelAttendShiftDecription { get; set; }
        public Int64 PersonnelAttendanceId { get; set; }
        public string DepartmentDescription { get; set; }
        public string AssetGroup1Names { get; set; }
        public string AssetGroup2Names { get; set; }
        public string AssetGroup3Names { get; set; }
        public string Personnel_ClientLookupId { get; set; } //V2-962

        public int InActiveStatus { get; set; } //V2-1098
        #region V2-1108
        public string AssignedAssetGroup1Names { get; set; }
        public string AssignedAssetGroup2Names { get; set; }
        public string AssignedAssetGroup3Names { get; set; }
        public string AssignedAssetGroup1ClientlookupId { get; set; }
        public string AssignedAssetGroup2ClientlookupId { get; set; }
        public string AssignedAssetGroup3ClientlookupId { get; set; }
        public long AssignedAssetGroup1Id { get; set; }
        public long AssignedAssetGroup2Id { get; set; }
        public long AssignedAssetGroup3Id { get; set; }
        #endregion
        public string DefaultStoreroom { get; set; } //V2-1178

        public List<b_Personnel> listOfPersonnel { get; set; } //V2-1178
        public static b_Personnel ProcessRowForFilterPlannerPersonnel(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForFilterPlannerPersonnel(reader);

            // Return result
            return obj;
        }


        public static b_Personnel ProcessRowForFilterPersonnel(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForFilterPersonnel(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForFilterPersonnel(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);

                // NameLast column, nvarchar(31), not null
                NameLast = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                NameFirst = reader.GetString(i++);

                // Buyer column, bool, not null
                Buyer = reader.GetBoolean(i++);

                // Scheduler column, book, not null
                Scheduler = reader.GetBoolean(i++);

                // ScheduleEmployee column, bool, not null
                ScheduleEmployee = reader.GetBoolean(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["Buyer"].ToString(); }
                catch { missing.Append("Buyer "); }

                try { reader["Scheduler"].ToString(); }
                catch { missing.Append("Scheduler "); }

                try { reader["ScheduleEmployee"].ToString(); }
                catch { missing.Append("ScheduleEmployee "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void LoadFromDatabaseForFilterPlannerPersonnel(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // NameLast column, nvarchar(31), not null
                NameLast = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                NameFirst = reader.GetString(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public static b_Personnel ProcessRowForFilterPersonnelByActiveUser(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForFilterPersonnelByActiveUser(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForFilterPersonnelByActiveUser(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = "";
                }

                // FullName column
                if (false == reader.IsDBNull(i))
                {
                    FullName = reader.GetString(i++);
                }

                else

                {
                    FullName = "";
                }


            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["FullName"].ToString(); }
                catch { missing.Append("FullName "); }



                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        // SOM-315 - RetrieveForLookupList
        public void RetrieveForLookupList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveForLookupList.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        //-----------------------------------------------------------------------------------------------------------    
        //V2 631 - RetrieveForLookupList
        public void RetrieveForLookupListV2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        public void RetrieveForPlannerLookupList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_Planner_RetrieveForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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


        public void RetrievePersonnelListByFilterText(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        long ClientId,
        long SiteId,
       ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_PersonnelList_RetrieveByFilterText.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SiteId, FilterText, FilterStartIndex, FilterEndIndex);

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
        //-----------------------------------------------------------------------------------------------------------    

        public static b_Personnel ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel personnel = new b_Personnel();

            // Load the object from the database
            personnel.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return personnel;
        }
        // 2012-Mar-13 - Roger Lawton
        //SOM-1688 
        // Not used anywhere
        //public void RetrieveByPKExtended(
        //  SqlConnection connection,
        //  SqlTransaction transaction,
        //  long callerUserInfoId,
        //  string callerUserName)
        //{
        //  SqlCommand command = null;
        //  string message = String.Empty;

        //  try
        //  {
        //    // Create the command to use in calling the stored procedures
        //    command = new SqlCommand();
        //    command.Connection = connection;
        //    command.Transaction = transaction;

        //    // Call the stored procedure to retrieve the data
        //    Database.StoredProcedure.usp_Personnel_RetrieveByPKExtended.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        //  }
        //  finally
        //  {
        //    if (null != command)
        //    {
        //      command.Dispose();
        //      command = null;
        //    }
        //    message = String.Empty;
        //    callerUserInfoId = 0;
        //    callerUserName = String.Empty;
        //  }
        //}

        // Used to process data returned from procedure usp_Personnel_RetrieveByPKExtended 
        //public static object ProcessRowExtended(SqlDataReader reader)
        //{
        //  // Create instance of object
        //  b_Personnel obj = new b_Personnel();

        //  // Load the object from the database
        //  obj.LoadFromDatabaseExtended(reader);

        //  // Return result
        //  return (object)obj;
        //}
        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                #region - Copy of Generated Code
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // UserInfoId column, bigint, not null
                UserInfoId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreRoomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);

                // Address1 column, nvarchar(63), not null
                Address1 = reader.GetString(i++);

                // Address2 column, nvarchar(63), not null
                Address2 = reader.GetString(i++);

                // Address3 column, nvarchar(63), not null
                Address3 = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                AddressCity = reader.GetString(i++);

                // AddressCountry column, nvarchar(63), not null
                AddressCountry = reader.GetString(i++);

                // AddressPostCode column, nvarchar(31), not null
                AddressPostCode = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                AddressState = reader.GetString(i++);

                // ApprovalLimitPO column, decimal(15,0), not null
                ApprovalLimitPO = reader.GetDecimal(i++);

                // ApprovalLimitWO column, decimal(15,0), not null
                ApprovalLimitWO = reader.GetDecimal(i++);

                // BasePay column, decimal(10,2), not null
                BasePay = reader.GetDecimal(i++);

                // Craft column, nvarchar(15), not null
                CraftId = reader.GetInt64(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentLevel column, nvarchar(15), not null
                CurrentLevel = reader.GetString(i++);

                // DateofBirth column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DateofBirth = reader.GetDateTime(i);
                }
                else
                {
                    DateofBirth = DateTime.MinValue;
                }
                i++;
                // Default_StoreroomId column, bigint, not null
                Default_StoreroomId = reader.GetInt64(i++);

                // DistancefromWork column, decimal(6,2), not null
                DistancefromWork = reader.GetDecimal(i++);

                // Email column, nvarchar(255), not null
                Email = reader.GetString(i++);

                // Floater column, bit, not null
                Floater = reader.GetBoolean(i++);

                // Gender column, nvarchar(15), not null
                Gender = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // InitialLevel column, nvarchar(15), not null
                InitialLevel = reader.GetString(i++);

                // InitialPay column, decimal(10,2), not null
                InitialPay = reader.GetDecimal(i++);

                // LastSalaryReview column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastSalaryReview = reader.GetDateTime(i);
                }
                else
                {
                    LastSalaryReview = DateTime.MinValue;
                }
                i++;
                // MaritalStatus column, nvarchar(15), not null
                MaritalStatus = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                NameFirst = reader.GetString(i++);

                // NameFull column, nvarchar(127), not null
                //NameFull = reader.GetString(i++);

                // NameLast column, nvarchar(31), not null
                NameLast = reader.GetString(i++);

                // NameMiddle column, nvarchar(31), not null
                NameMiddle = reader.GetString(i++);

                // Phone1 column, nvarchar(31), not null
                Phone1 = reader.GetString(i++);

                // Phone2 column, nvarchar(31), not null
                Phone2 = reader.GetString(i++);

                // Planner column, bit, not null
                Planner = reader.GetBoolean(i++);

                // Scheduler column, bit, not null
                Scheduler = reader.GetBoolean(i++);

                // ScheduleEmployee column, bit, not null
                ScheduleEmployee = reader.GetBoolean(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // SocialSecurityNumber column, nvarchar(15), not null
                SocialSecurityNumber = reader.GetString(i++);

                // StartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }
                i++;
                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Supervisor_PersonnelId column, bigint, not null
                Supervisor_PersonnelId = reader.GetInt64(i++);

                // TerminationDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    TerminationDate = reader.GetDateTime(i);
                }
                else
                {
                    TerminationDate = DateTime.MinValue;
                }
                i++;
                // TerminationReason column, nvarchar(15), not null
                TerminationReason = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                #endregion
                #region - Custom Code
                // Supervisor_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    Supervisor_ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    Supervisor_ClientLookupId = "";
                }

                // Supervisor_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    DefaultStoreRoom_Name = reader.GetString(i++);
                }
                else
                {
                    DefaultStoreRoom_Name = "";
                }

                #endregion
            }
            catch (Exception ex)
            {
                #region - Copy of generated Code
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["UserInfoId"].ToString(); }
                catch { missing.Append("UserInfoId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

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

                try { reader["Craft"].ToString(); }
                catch { missing.Append("Craft "); }

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

                //try { reader["NameFull"].ToString(); }
                //catch { missing.Append("NameFull "); }

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

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }
                #endregion

                #region - Custom Code
                // Supervisor_ClientLookupId column
                try { reader["Supervisor_ClientLookupId"].ToString(); }
                catch { missing.Append("Supervisor_ClientLookupId"); }

                // DefautlStoreRoom_Name column
                try { reader["DefautlStoreRoom_Name"].ToString(); }
                catch { missing.Append("DefautlStoreRoom_Name"); }

                #endregion
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Personnel> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Personnel_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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

        /// <summary>
        /// Create a new Persponnel record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void CreatedExtended(
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
                Database.StoredProcedure.usp_Personnel_CreateExtended.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        /// <summary>
        /// Update the Persponnel table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateExtended(
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
                Database.StoredProcedure.usp_Personnel_UpdateExtended.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ValidateExtended(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        string Supervisor_ClientLookupId,
        string DefaultStoreroom_Name,
        bool createMode,
        System.Data.DataTable lulist,
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
                results = Database.StoredProcedure.usp_Personnel_ValidateExtended.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, Supervisor_ClientLookupId,
                    DefaultStoreroom_Name, createMode, lulist);

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

        //----------------------------------------------------------

        public void RetrievePersonnelByClientId(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
               string callerUserName,
             ref List<b_Personnel> data
           )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Personnel_RetrieveByClientId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this.ClientId);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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
        // SOM-1228
        public void RetrievePersonnelFromList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, System.Data.DataTable pelist, ref List<b_Personnel> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Personnel_RetrieveFromList.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, pelist);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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

        //--------------------------------------------------Added By Indusnet Technologies------------------
        public static object ProcessRowExtendedForRetrieveByClientId(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseExtendedForRetrieveByClientId(reader);

            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseExtendedForRetrieveByClientId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                #region - Copy of Generated Code
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // UserInfoId column, bigint, not null
                UserInfoId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreRoomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);

                // Address1 column, nvarchar(63), not null
                Address1 = reader.GetString(i++);

                // Address2 column, nvarchar(63), not null
                Address2 = reader.GetString(i++);

                // Address3 column, nvarchar(63), not null
                Address3 = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                AddressCity = reader.GetString(i++);

                // AddressCountry column, nvarchar(63), not null
                AddressCountry = reader.GetString(i++);

                // AddressPostCode column, nvarchar(31), not null
                AddressPostCode = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                AddressState = reader.GetString(i++);

                // ApprovalLimitPO column, decimal(15,0), not null
                ApprovalLimitPO = reader.GetDecimal(i++);

                // ApprovalLimitWO column, decimal(15,0), not null
                ApprovalLimitWO = reader.GetDecimal(i++);

                // BasePay column, decimal(10,2), not null
                BasePay = reader.GetDecimal(i++);

                // Craft column, nvarchar(15), not null
                CraftId = reader.GetInt64(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentLevel column, nvarchar(15), not null
                CurrentLevel = reader.GetString(i++);

                // DateofBirth column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DateofBirth = reader.GetDateTime(i);
                }
                else
                {
                    DateofBirth = DateTime.MinValue;
                }
                i++;
                // Default_StoreroomId column, bigint, not null
                Default_StoreroomId = reader.GetInt64(i++);

                // DistancefromWork column, decimal(6,2), not null
                DistancefromWork = reader.GetDecimal(i++);

                // Email column, nvarchar(255), not null
                Email = reader.GetString(i++);

                // Floater column, bit, not null
                Floater = reader.GetBoolean(i++);

                // Gender column, nvarchar(15), not null
                Gender = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // InitialLevel column, nvarchar(15), not null
                InitialLevel = reader.GetString(i++);

                // InitialPay column, decimal(10,2), not null
                InitialPay = reader.GetDecimal(i++);

                // LastSalaryReview column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastSalaryReview = reader.GetDateTime(i);
                }
                else
                {
                    LastSalaryReview = DateTime.MinValue;
                }
                i++;
                // MaritalStatus column, nvarchar(15), not null
                MaritalStatus = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                NameFirst = reader.GetString(i++);

                // NameFull column, nvarchar(127), not null
                //NameFull = reader.GetString(i++);

                // NameLast column, nvarchar(31), not null
                NameLast = reader.GetString(i++);

                // NameMiddle column, nvarchar(31), not null
                NameMiddle = reader.GetString(i++);

                // Phone1 column, nvarchar(31), not null
                Phone1 = reader.GetString(i++);

                // Phone2 column, nvarchar(31), not null
                Phone2 = reader.GetString(i++);

                // Planner column, bit, not null
                Planner = reader.GetBoolean(i++);

                // Scheduler column, bit, not null
                Scheduler = reader.GetBoolean(i++);

                // ScheduleEmployee column, bit, not null
                ScheduleEmployee = reader.GetBoolean(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // SocialSecurityNumber column, nvarchar(15), not null
                SocialSecurityNumber = reader.GetString(i++);

                // StartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }
                i++;
                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Supervisor_PersonnelId column, bigint, not null
                Supervisor_PersonnelId = reader.GetInt64(i++);

                // TerminationDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    TerminationDate = reader.GetDateTime(i);
                }
                else
                {
                    TerminationDate = DateTime.MinValue;
                }
                i++;
                // TerminationReason column, nvarchar(15), not null
                TerminationReason = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                #endregion
                #region - Custom Code
                // Supervisor_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    CraftDescription = reader.GetString(i++);
                }
                else
                {
                    CraftDescription = "";
                }

                #endregion
            }
            catch (Exception ex)
            {
                #region - Copy of generated Code
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["UserInfoId"].ToString(); }
                catch { missing.Append("UserInfoId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

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

                try { reader["Craft"].ToString(); }
                catch { missing.Append("Craft "); }

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

                //try { reader["NameFull"].ToString(); }
                //catch { missing.Append("NameFull "); }

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

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }
                #endregion

                #region - Custom Code
                // Supervisor_ClientLookupId column
                try { reader["CraftDescription"].ToString(); }
                catch { missing.Append("CraftDescription"); }



                #endregion
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void ValidatePersonnelClientLookUpId(
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
                results = Database.StoredProcedure.usp_Personnel_ValidateClientLookUp.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrieveForLookupListBySecurityItem(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveForLookupListBySecurityItem.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        //----------------SOM-828-------------------------------------------------------------------------------
        public void UpdateUpdateForMultiUserSite(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_Personnel_UpdateForMultiUserSite.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //SOM-642
        public void UpdateFromUserInfo(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_Personnel_UpdateFromUserInfo.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void UpdateFromUserInfoAdmin(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_AdminPersonnel_UpdateFromUserInfo_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        /// <summary>
        /// Retrieve all Personnel table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Personnel[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Personnel[] data
        )
        {
            ProcessRow<b_Personnel> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Personnel[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new ProcessRow<b_Personnel>(reader => { b_Personnel obj = new b_Personnel(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Personnel_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Personnel[])results.ToArray(typeof(b_Personnel));
                }
                else
                {
                    data = new b_Personnel[0];
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

        public static b_Personnel ProcessRowForPersonnel_RetrieveMention(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel PersonnelSearchdata = new b_Personnel();

            // Load the object from the database
            PersonnelSearchdata.LoadFromDatabaseForPersonnel_RetrieveMention(reader);

            // Return result
            return PersonnelSearchdata;
        }


        public void LoadFromDatabaseForPersonnel_RetrieveMention(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = "";
                }

                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i++);
                }
                else
                {
                    NameFirst = "";
                }

                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i++);
                }
                else
                {
                    NameLast = "";
                }

                if (false == reader.IsDBNull(i))
                {
                    // SearchText column, nvarchar(500), not null
                    NameMiddle = reader.GetString(i++);
                }
                else
                {
                    NameMiddle = "";
                }

                if (false == reader.IsDBNull(i))
                {
                    FullName = reader.GetString(i++);
                }

                else

                {
                    FullName = "";
                }

                if (false == reader.IsDBNull(i))
                {
                    UserName = reader.GetString(i++);
                }
                else
                {
                    UserName = "";
                }
                if (false == reader.IsDBNull(i))
                {
                    Email = reader.GetString(i++);
                }
                else
                {
                    Email = "";
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameMiddle"].ToString(); }
                catch { missing.Append("NameMiddle "); }

                try { reader["NameFull"].ToString(); }
                catch { missing.Append("NameFull "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrieveForMention(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Personnel> results

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

                results = Database.StoredProcedure.usp_Personnel_RetrieveMention.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveChunkSearchV2(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref b_Personnel results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static object ProcessRowForSiteName(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForSiteName(reader);

            // Return result
            return (object)obj;
        }
        public int LoadFromDatabaseForSiteName(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // UserInfoId column, bigint, not null
                UserInfoId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);

                // Address1 column, nvarchar(63), not null
                Address1 = reader.GetString(i++);

                // Address2 column, nvarchar(63), not null
                Address2 = reader.GetString(i++);

                // Address3 column, nvarchar(63), not null
                Address3 = reader.GetString(i++);

                // AddressCity column, nvarchar(63), not null
                AddressCity = reader.GetString(i++);

                // AddressCountry column, nvarchar(63), not null
                AddressCountry = reader.GetString(i++);

                // AddressPostCode column, nvarchar(31), not null
                AddressPostCode = reader.GetString(i++);

                // AddressState column, nvarchar(63), not null
                AddressState = reader.GetString(i++);

                // ApprovalLimitPO column, decimal(15,0), not null
                ApprovalLimitPO = reader.GetDecimal(i++);

                // ApprovalLimitWO column, decimal(15,0), not null
                ApprovalLimitWO = reader.GetDecimal(i++);

                // BasePay column, decimal(10,2), not null
                BasePay = reader.GetDecimal(i++);

                // CraftId column, bigint, not null
                CraftId = reader.GetInt64(i++);

                // Crew column, nvarchar(15), not null
                Crew = reader.GetString(i++);

                // CurrentLevel column, nvarchar(15), not null
                CurrentLevel = reader.GetString(i++);

                // DateofBirth column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    DateofBirth = reader.GetDateTime(i);
                }
                else
                {
                    DateofBirth = DateTime.MinValue;
                }
                i++;
                // Default_StoreroomId column, bigint, not null
                Default_StoreroomId = reader.GetInt64(i++);

                // DistancefromWork column, decimal(6,2), not null
                DistancefromWork = reader.GetDecimal(i++);

                // Email column, nvarchar(255), not null
                Email = reader.GetString(i++);

                // Floater column, bit, not null
                Floater = reader.GetBoolean(i++);

                // Gender column, nvarchar(15), not null
                Gender = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // InitialLevel column, nvarchar(15), not null
                InitialLevel = reader.GetString(i++);

                // InitialPay column, decimal(10,2), not null
                InitialPay = reader.GetDecimal(i++);

                // LastSalaryReview column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastSalaryReview = reader.GetDateTime(i);
                }
                else
                {
                    LastSalaryReview = DateTime.MinValue;
                }
                i++;
                // MaritalStatus column, nvarchar(15), not null
                MaritalStatus = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                NameFirst = reader.GetString(i++);

                // NameLast column, nvarchar(31), not null
                NameLast = reader.GetString(i++);

                // NameMiddle column, nvarchar(31), not null
                NameMiddle = reader.GetString(i++);

                // Phone1 column, nvarchar(31), not null
                Phone1 = reader.GetString(i++);

                // Phone2 column, nvarchar(31), not null
                Phone2 = reader.GetString(i++);

                // Planner column, bit, not null
                Planner = reader.GetBoolean(i++);

                // Scheduler column, bit, not null
                Scheduler = reader.GetBoolean(i++);

                // ScheduleEmployee column, bit, not null
                ScheduleEmployee = reader.GetBoolean(i++);

                // Section column, nvarchar(15), not null
                Section = reader.GetString(i++);

                // Shift column, nvarchar(15), not null
                Shift = reader.GetString(i++);

                // SocialSecurityNumber column, nvarchar(15), not null
                SocialSecurityNumber = reader.GetString(i++);

                // StartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }
                i++;
                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Supervisor_PersonnelId column, bigint, not null
                Supervisor_PersonnelId = reader.GetInt64(i++);

                // TerminationDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    TerminationDate = reader.GetDateTime(i);
                }
                else
                {
                    TerminationDate = DateTime.MinValue;
                }
                i++;
                // TerminationReason column, nvarchar(15), not null
                TerminationReason = reader.GetString(i++);

                // Buyer column, bit, not null
                Buyer = reader.GetBoolean(i++);

                // ExOracleUserId column, nvarchar(63), not null
                ExOracleUserId = reader.GetString(i++);

                //Site name
                SiteName = reader.GetString(i++);
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1 = reader.GetString(i);
                }
                else
                {
                    AssetGroup1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2 = reader.GetString(i);
                }
                else
                {
                    AssetGroup2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3 = reader.GetString(i);
                }
                else
                {
                    AssetGroup3 = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["UserInfoId"].ToString(); }
                catch { missing.Append("UserInfoId "); }

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
        public static b_Personnel ProcessRowForRetrieveByPersonnelId(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByPersonnelId(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForRetrieveByPersonnelId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // Email column, nvarchar(255), not null
                Email = reader.GetString(i++);

                // UserName column, nvarchar(63), not null
                UserName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["Email"].ToString(); }
                catch { missing.Append("Email "); }

                try { reader["UserName"].ToString(); }
                catch { missing.Append("UserName "); }

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
        public void RetrieveByForeignKeysFromDatabase_V2(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName
   )
        {
            Database.SqlClient.ProcessRow<b_Personnel> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Personnel>(reader => { this.LoadFromDatabaseByPKForeignKey_V2(reader); return this; });
                StoredProcedure.usp_Personnel_RetrieveByPKForeignKeys_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void LoadFromDatabaseByPKForeignKey_V2(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                ClientId = reader.GetInt64(i++);

                PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    NameMiddle = reader.GetString(i);
                }
                else
                {
                    NameMiddle = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = "";
                }
                i++;

                DepartmentId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    DepartmentDescription = reader.GetString(i);
                }
                else
                {
                    DepartmentDescription = "";
                }
                i++;

                CraftId = reader.GetInt64(i++);


                if (false == reader.IsDBNull(i))
                {
                    CraftDescription = reader.GetString(i);
                }
                else
                {
                    CraftDescription = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Shift = reader.GetString(i);
                }
                else
                {
                    Shift = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Crew = reader.GetString(i);
                }
                else
                {
                    Crew = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ScheduleGroup = reader.GetString(i);
                }
                else
                {
                    ScheduleGroup = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Planner = reader.GetBoolean(i);
                }
                else
                {
                    Planner = false;
                }
                i++;

                UpdateIndex = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    ShiftDescription = reader.GetString(i);
                }
                else
                {
                    ShiftDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CrewDescription = reader.GetString(i);
                }
                else
                {
                    CrewDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ScheduleGroupDescription = reader.GetString(i);
                }
                else
                {
                    ScheduleGroupDescription = "";
                }
                i++;
                BasePay = reader.GetDecimal(i++);

                InitialPay = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    LastSalaryReview = reader.GetDateTime(i);
                }
                else
                {
                    LastSalaryReview = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    StartDate = reader.GetDateTime(i);
                }
                else
                {
                    StartDate = DateTime.MinValue;
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1 = reader.GetString(i);
                }
                else
                {
                    AssetGroup1 = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2 = reader.GetString(i);
                }
                else
                {
                    AssetGroup2 = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3 = reader.GetString(i);
                }
                else
                {
                    AssetGroup3 = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1Names = reader.GetString(i);
                }
                else
                {
                    AssetGroup1Names = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2Names = reader.GetString(i);
                }
                else
                {
                    AssetGroup2Names = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3Names = reader.GetString(i);
                }
                else
                {
                    AssetGroup3Names = "";
                }
                i++;
                //V2-831
                if (false == reader.IsDBNull(i))
                {
                    ExOracleUserId = reader.GetString(i);
                }
                else
                {
                    ExOracleUserId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ScheduleEmployee = reader.GetBoolean(i);
                }
                else
                {
                    ScheduleEmployee = false;
                }
                i++;
                //V2-1098
                if (false == reader.IsDBNull(i))
                {
                    InactiveFlag = reader.GetBoolean(i);
                }
                else
                {
                    InactiveFlag = false;
                }
                i++;
                //#region V2-1108
                AssignedAssetGroup1 = reader.GetInt64(i++);
                AssignedAssetGroup2 = reader.GetInt64(i++);
                AssignedAssetGroup3 = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetGroup1Names = reader.GetString(i);
                }
                else
                {
                    AssignedAssetGroup1Names = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetGroup2Names = reader.GetString(i);
                }
                else
                {
                    AssignedAssetGroup2Names = "";
                }

                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetGroup3Names = reader.GetString(i);
                }
                else
                {
                    AssignedAssetGroup3Names = "";
                }
                i++;
                //#endregion
                //#region V2-1178
                if (false == reader.IsDBNull(i))
                {
                    DefaultStoreroom = reader.GetString(i);
                }
                else
                {
                    DefaultStoreroom = "";
                }
                i++;
                Default_StoreroomId = reader.GetInt64(i++);
                //#endregion

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameMiddle"].ToString(); }
                catch { missing.Append("NameMiddle "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["DepartmentDescription"].ToString(); }
                catch { missing.Append("DepartmentDescription "); }

                try { reader["CraftId"].ToString(); }
                catch { missing.Append("CraftId "); }

                try { reader["CraftDescription"].ToString(); }
                catch { missing.Append("CraftDescription "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Crew"].ToString(); }
                catch { missing.Append("Crew "); }

                try { reader["ScheduleGroup"].ToString(); }
                catch { missing.Append("ScheduleGroup "); }

                try { reader["Planner"].ToString(); }
                catch { missing.Append("Planner "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["ShiftDescription"].ToString(); }
                catch { missing.Append("ShiftDescription "); }

                try { reader["CrewDescription"].ToString(); }
                catch { missing.Append("CrewDescription "); }

                try { reader["ScheduleGroupDescription"].ToString(); }
                catch { missing.Append("ScheduleGroupDescription "); }

                try { reader["BasePay"].ToString(); }
                catch { missing.Append("BasePay "); }

                try { reader["InitialLevel"].ToString(); }
                catch { missing.Append("InitialLevel "); }

                try { reader["LastSalaryReview"].ToString(); }
                catch { missing.Append("LastSalaryReview "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["AssetGroup1"].ToString(); }
                catch { missing.Append("AssetGroup1 "); }

                try { reader["AssetGroup2"].ToString(); }
                catch { missing.Append("AssetGroup2 "); }

                try { reader["AssetGroup3"].ToString(); }
                catch { missing.Append("AssetGroup3 "); }

                try { reader["AssetGroup1Names"].ToString(); }
                catch { missing.Append("AssetGroup1Names "); }

                try { reader["AssetGroup2Names"].ToString(); }
                catch { missing.Append("AssetGroup2Names "); }

                try { reader["AssetGroup3Names"].ToString(); }
                catch { missing.Append("AssetGroup3Names "); }

                try { reader["ExOracleUserId"].ToString(); }  //V2-831
                catch { missing.Append("ExOracleUserId "); }

                try { reader["ScheduleEmployee"].ToString(); }
                catch { missing.Append("ScheduleEmployee "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }
                //#region V2-1108
                try { reader["AssignedAssetGroup1"].ToString(); }
                catch { missing.Append("AssignedAssetGroup1 "); }

                try { reader["AssignedAssetGroup2"].ToString(); }
                catch { missing.Append("AssignedAssetGroup2 "); }

                try { reader["AssignedAssetGroup3"].ToString(); }
                catch { missing.Append("AssignedAssetGroup3 "); }

                try { reader["AssignedAssetGroup1Names"].ToString(); }
                catch { missing.Append("AssignedAssetGroup1Names "); }

                try { reader["AssignedAssetGroup2Names"].ToString(); }
                catch { missing.Append("AssignedAssetGroup2Names "); }

                try { reader["AssignedAssetGroup3Names"].ToString(); }
                catch { missing.Append("AssignedAssetGroup3Names "); }
                //#endregion 
                //#region V2-1178
                try { reader["DefaultStoreroom"].ToString(); }
                catch { missing.Append("DefaultStoreroom "); }

                try { reader["Default_StoreroomId"].ToString(); }
                catch { missing.Append("Default_StoreroomId "); }

                //#endregion                

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }



        public void RetrieveEventsByPersonnelIdFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_Personnel> data
      )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Events_RetrieveByPersonnelId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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


        public static object ProcessRowForRetrievingEventsByPersonnelId(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrievingEventsByPersonnelId(reader);

            // Return result
            return (object)obj;
        }



        public void LoadFromDatabaseForRetrievingEventsByPersonnelId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);


                EventsId = reader.GetInt64(i++);


                PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = null;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    ExpireDate = reader.GetDateTime(i);
                }
                else
                {
                    ExpireDate = null;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["EventsId"].ToString(); }
                catch { missing.Append("EventsId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["ExpireDate"].ToString(); }
                catch { missing.Append("ExpireDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Personnel ProcessChunkSearchV2(SqlDataReader reader)
        {
            b_Personnel personnel = new b_Personnel();

            personnel.LoadFromDatabaseForPersonnelChunkSearchV2(reader);
            return personnel;
        }
        public int LoadFromDatabaseForPersonnelChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                PersonnelId = reader.GetInt64(i++);

                //  ClientLookupId
                ClientLookupId = reader.GetString(i++);

                //  NameFirst
                NameFirst = reader.GetString(i++);

                //NameLast
                NameLast = reader.GetString(i++);

                //ScheduleGroup
                ScheduleGroup = reader.GetString(i++);

                //Sche
                ScheduleGroupDescription = reader.GetString(i++);

                //Shift
                Shift = reader.GetString(i++);

                //ShiftDescription
                ShiftDescription = reader.GetString(i++);

                //Craft
                Crew = reader.GetString(i++);

                //CrewDescription
                CrewDescription = reader.GetString(i++);

                //CraftId
                CraftId = reader.GetInt64(i++);

                //CraftClientLookupId
                CraftClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetGroup1ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssignedAssetGroup1ClientlookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetGroup2ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssignedAssetGroup2ClientlookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssignedAssetGroup3ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssignedAssetGroup3ClientlookupId = "";
                }
                i++;

                AssignedAssetGroup1Id = reader.GetInt64(i++);
                AssignedAssetGroup2Id = reader.GetInt64(i++);
                AssignedAssetGroup3Id = reader.GetInt64(i++);

                //TotalCount
                TotalCount = reader.GetInt32(i);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["SiteID"].ToString(); }
                catch { missing.Append("SiteID "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["ScheduleGroup"].ToString(); }
                catch { missing.Append("ScheduleGroup"); }

                try { reader["ScheduleGroupDescription"].ToString(); }
                catch { missing.Append("ScheduleGroupDescription"); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["ShiftDescription"].ToString(); }
                catch { missing.Append("ShiftDescription "); }

                try { reader["Crew"].ToString(); }
                catch { missing.Append("Crew"); }

                try { reader["CrewDescription"].ToString(); }
                catch { missing.Append("CrewDescription"); }

                try { reader["CraftId"].ToString(); }
                catch { missing.Append("CraftId "); }

                try { reader["CraftClientLookupId"].ToString(); }
                catch { missing.Append("CraftClientLookupId "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }

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



        public static object ProcessRowForRetrievingLaborByPersonnelId(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrievingLaborByPersonnelId(reader);

            // Return result
            return (object)obj;
        }



        public void LoadFromDatabaseForRetrievingLaborByPersonnelId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);


                TimecardId = reader.GetInt64(i++);


                PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    laborstartdate = reader.GetDateTime(i);
                }
                else
                {
                    laborstartdate = DateTime.MinValue; ;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Hours = reader.GetDecimal(i);
                }
                else
                {
                    Hours = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Value = reader.GetDecimal(i);
                }
                else
                {
                    Value = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    WOClientLookupId = reader.GetString(i);
                }
                else
                {
                    WOClientLookupId = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["TimecardId"].ToString(); }
                catch { missing.Append("TimecardId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["Hours"].ToString(); }
                catch { missing.Append("Hours "); }

                try { reader["Value"].ToString(); }
                catch { missing.Append("Value "); }

                try { reader["WOClientLookupId"].ToString(); }
                catch { missing.Append("WOClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }



        public void RetrieveLaborByPersonnelIdFromDatabase(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_Personnel> data
     )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Labor_RetrieveByPersonnelId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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


        public void RetrievePersonnelAvailabilityByPersonnelIdFromDatabase(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_Personnel> data
   )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PersonnelAvailability_RetrieveByPersonnelId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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


        public static object ProcessRowForRetrievingPersonnelAvailabilityByPersonnelId(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrievingPersonnelAvailabilityByPersonnelId(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseForRetrievingPersonnelAvailabilityByPersonnelId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);


                PersonnelAvailabilityId = reader.GetInt64(i++);


                PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    PersonnelAvailabilityDate = reader.GetDateTime(i);
                }
                else
                {
                    PersonnelAvailabilityDate = DateTime.MinValue; ;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PAHours = reader.GetDecimal(i);
                }
                else
                {
                    PAHours = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PAShift = reader.GetString(i);
                }
                else
                {
                    PAShift = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelAvailabilityId"].ToString(); }
                catch { missing.Append("PersonnelAvailabilityId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelAvailabilityDate"].ToString(); }
                catch { missing.Append("PersonnelAvailabilityDate "); }

                try { reader["PAHours"].ToString(); }
                catch { missing.Append("PAHours "); }

                try { reader["PAShift"].ToString(); }
                catch { missing.Append("PAShift "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public void RetrievePersonnelAttendanceByPersonnelIdFromDatabase(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_Personnel> data
 )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Personnel> results = null;
            data = new List<b_Personnel>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_PersonnelAttendance_RetrieveByPersonnelId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Personnel>();
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
        public static object ProcessRowForRetrievingPersonnelAttendanceByPersonnelId(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrievingPersonnelAttendanceByPersonnelId(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseForRetrievingPersonnelAttendanceByPersonnelId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);


                PersonnelAttendanceId = reader.GetInt64(i++);


                PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    PersonnelAttendDate = reader.GetDateTime(i);
                }
                else
                {
                    PersonnelAttendDate = DateTime.MinValue; ;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PersonnelAttendHours = reader.GetDecimal(i);
                }
                else
                {
                    PersonnelAttendHours = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PersonnelAttendShift = reader.GetString(i);
                }
                else
                {
                    PersonnelAttendShift = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PersonnelAttendShiftDecription = reader.GetString(i);
                }
                else
                {
                    PersonnelAttendShiftDecription = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelAttendanceId"].ToString(); }
                catch { missing.Append("PersonnelAttendanceId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelAttendDate"].ToString(); }
                catch { missing.Append("PersonnelAttendDate "); }

                try { reader["PersonnelAttendHours"].ToString(); }
                catch { missing.Append("PersonnelAttendHours "); }

                try { reader["PersonnelAttendShift"].ToString(); }
                catch { missing.Append("PersonnelAttendShift "); }

                try { reader["PersonnelAttendShiftDecription"].ToString(); }
                catch { missing.Append("PersonnelAttendShiftDecription "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrievePersonnelByPersonnel_V2(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
               string callerUserName,
                ref b_Personnel results
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
                results = Database.StoredProcedure.usp_Personnel_RetrieveByPersonnelId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);


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


        #region Retrieve personnel for all Active and Full Users
        public static b_Personnel ProcessRowForAllActiveandFullUsersPersonnel(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForAllActiveandFullUsersPersonnel(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForAllActiveandFullUsersPersonnel(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(63), not null
                ClientLookupId = reader.GetString(i++);

                // NameLast column, nvarchar(31), not null
                NameLast = reader.GetString(i++);

                // NameFirst column, nvarchar(31), not null
                NameFirst = reader.GetString(i++);

                // Buyer column, bool, not null
                Buyer = reader.GetBoolean(i++);

                // ScheduleEmployee column, bool, not null
                ScheduleEmployee = reader.GetBoolean(i++);
            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["Buyer"].ToString(); }
                catch { missing.Append("Buyer "); }

                try { reader["ScheduleEmployee"].ToString(); }
                catch { missing.Append("ScheduleEmployee "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrieveAllForActiveandFullUsersPersonnel(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveAllActiveFullUser_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        #region Retrieve for WorkOrder completion wizard tab
        public void RetrieveByPersonnelIdForWorkOrderCompletionWizard_V2(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
               string callerUserName,
               ref b_Personnel result
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
                result = Database.StoredProcedure.usp_Personnel_RetrieveByPersonnelIdForWorkOrderCompletionWizard_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);


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
        public static b_Personnel ProcessRowForWorkOrderCompletionWizard(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForWorkOrderCompletionWizard(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForWorkOrderCompletionWizard(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                Value = reader.GetDecimal(i++);

                ClientLookupId = reader.GetString(i++);

                FullName = reader.GetString(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["Value"].ToString(); }
                catch { missing.Append("Value "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["FullName"].ToString(); }
                catch { missing.Append("FullName "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        #endregion

        #region Retrieve Personnel LookupList For ApprovalGroup
        //V2-720
        public void RetrieveChunkSearchLookupListForApprovalGroup_V2(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
               string callerUserName,
               ref List<b_Personnel> result
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
                result = Database.StoredProcedure.usp_Personnel_RetrieveChunkSearchLookupListForApprovalGroup_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static b_Personnel ProcessRowOfPersonnelLookupListForApprovalGroup(SqlDataReader reader)
        {
            // Create instance of object  Retrieve Personnel LookupList For ApprovalGroup
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabasePersonnelLookupListForApprovalGroup(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabasePersonnelLookupListForApprovalGroup(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                PersonnelId = reader.GetInt64(i++);
                ClientLookupId = reader.GetString(i++);
                FullName = reader.GetString(i++);
                AssetGroup1 = reader.GetString(i++);
                AssetGroup2 = reader.GetString(i++);
                AssetGroup3 = reader.GetString(i++);
                AssetGroup1Names = reader.GetString(i++);
                AssetGroup2Names = reader.GetString(i++);
                AssetGroup3Names = reader.GetString(i++);
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                StringBuilder missing = new StringBuilder();
                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }
                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }
                try { reader["FullName"].ToString(); }
                catch { missing.Append("FullName "); }
                try { reader["AssetGroup1"].ToString(); }
                catch { missing.Append("AssetGroup1 "); }
                try { reader["AssetGroup2"].ToString(); }
                catch { missing.Append("AssetGroup2 "); }
                try { reader["AssetGroup3"].ToString(); }
                catch { missing.Append("AssetGroup3 "); }
                try { reader["AssetGroup1ClientLookUpId"].ToString(); }
                catch { missing.Append("AssetGroup1ClientLookUpId "); }
                try { reader["AssetGroup2ClientLookUpId"].ToString(); }
                catch { missing.Append("AssetGroup2ClientLookUpId "); }
                try { reader["AssetGroup3ClientLookUpId"].ToString(); }
                catch { missing.Append("AssetGroup3ClientLookUpId "); }
                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        #endregion

        #region V2-989
        public void RetrievePartManagementForLookupList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrievePartManagementForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        #region V2-798
        public void RetrieveAllActiveForLookupList(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveAllActiveForLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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
        #region V2-806
        public void RetrieveAllActiveForLookupListForAdmin(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveAllActiveForLookupListForAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        #region V2-820
        public void RetrieveForLookupListByMultipleSecurityItem(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, b_Personnel obj, ref List<b_Personnel> results)
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveForLookupListByMultipleSecurityItem_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);

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

        #region V2-929
        public void RetrieveLookupListActiveAdminOrFullUser(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveLookupListChunkSearchForActiveAdminOrFullUser_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_Personnel ProcessRowForRetriveLookupListActiveAdminOrFullUser(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel Personnel = new b_Personnel();
            Personnel.LoadFromDatabaseForRetriveLookupListActiveAdminOrFullUser(reader);
            return Personnel;
        }

        public int LoadFromDatabaseForRetriveLookupListActiveAdminOrFullUser(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;
                // ClientLookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // NameLast column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = string.Empty;
                }
                i++;
                // NameFirst column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = string.Empty;
                }
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

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

        #endregion
        #region V2-1178
        public void RetrieveChunkSearchForActiveAdminOrFullUser_V2(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveChunkSearchForActiveAdminOrFullUser_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-950
        public void RetrieveLookupListActiveAdminOrFullUserPlanner(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveLookupListChunkSearchForActiveAdminOrFullUserPlanner_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_Personnel ProcessRowForRetriveLookupListActiveAdminOrFullUserPlanner(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel Personnel = new b_Personnel();
            Personnel.LoadFromDatabaseForRetriveLookupListActiveAdminOrFullUserPlanner(reader);
            return Personnel;
        }

        public int LoadFromDatabaseForRetriveLookupListActiveAdminOrFullUserPlanner(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PersonnelId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;
                // ClientLookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // NameLast column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = string.Empty;
                }
                i++;
                // NameFirst column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = string.Empty;
                }
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

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

        #endregion

        #region V2-712
        public void RetrievePMAssignPersonnelLookupList(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveForPrevMaintAssignLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-981
        public void RetrievePersonnelLookupListActive(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_Personnel ProcessRowForRetriveLookupListActive(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel Personnel = new b_Personnel();
            Personnel.LoadFromDatabaseForRetriveLookupListActive(reader);
            return Personnel;
        }

        public int LoadFromDatabaseForRetriveLookupListActive(SqlDataReader reader)
        {

            int i = 0;
            try
            {

                // Client Id
                ClientId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;

                // ClientLookupId column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // NameLast column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    NameFirst = reader.GetString(i);
                }
                else
                {
                    NameFirst = string.Empty;
                }
                i++;
                // NameFirst column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i);
                }
                else
                {
                    NameLast = string.Empty;
                }
                i++;

                //TotalCount
                if (false == reader.IsDBNull(i))
                {
                    TotalCount = reader.GetInt32(i);
                }
                else
                {
                    TotalCount = 0;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["NameFirst"].ToString(); }
                catch { missing.Append("NameFirst "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

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

        #endregion
        #region V2-962
        public static b_Personnel ProcessRowForForAdminUserManagementChildGrid(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel obj = new b_Personnel();

            // Load the object from the database
            obj.LoadFromDatabaseForAdminUserManagementChildGrid(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForAdminUserManagementChildGrid(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Personnel_ClientLookupId column, nvarchar(63), not null
                Personnel_ClientLookupId = reader.GetString(i++);

                // CraftId column,  bigint, not null
                CraftId = reader.GetInt64(i++);

                // CraftDescription column, nvarchar(31), not null
                CraftDescription = reader.GetString(i++);

                // SiteName column, nvarchar(31), not null
                SiteName = reader.GetString(i++);

                SiteId = reader.GetInt64(i++);

                // Buyer column, bool, not null
                Buyer = reader.GetBoolean(i++);

                // Planner column, bool, not null
                Planner = reader.GetBoolean(i++);

            }
            catch (Exception ex)
            {

                StringBuilder missing = new StringBuilder();


                try { reader["Personnel_ClientLookupId"].ToString(); }
                catch { missing.Append("Personnel_ClientLookupId "); }

                try { reader["CraftId"].ToString(); }
                catch { missing.Append("CraftId "); }

                try { reader["CraftDescription"].ToString(); }
                catch { missing.Append("CraftDescription "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Buyer"].ToString(); }
                catch { missing.Append("Buyer "); }

                try { reader["Planner"].ToString(); }
                catch { missing.Append("Planner "); }


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void RetrieveForAdminUserManagementChildGrid(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Personnel> results
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

                results = Database.StoredProcedure.usp_Personnel_RetrieveByUserInfoIdForAdminUserManagementChildGrid_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-1178

        public void RetrievePersonnelIdByClientLookupIdFromDatabase(
              SqlConnection connection,
              SqlTransaction transaction,
              long callerUserInfoId,
string callerUserName,
              ref b_Personnel result
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

                result = Database.StoredProcedure.usp_Personnel_RetrieveByClientLookUpId_V2.CallStoredProcedure(command, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                ClientLookupId = String.Empty;
            }
        }
        public static b_Personnel ProcessRowForPersonnelIdByClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Personnel personnels = new b_Personnel();

            // Load the object from the database
            personnels.LoadFromDatabaseForPersonnelIdByClientIdLookup(reader);

            // Return result
            return personnels;
        }

        public void LoadFromDatabaseForPersonnelIdByClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

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
        #endregion
    }
}
