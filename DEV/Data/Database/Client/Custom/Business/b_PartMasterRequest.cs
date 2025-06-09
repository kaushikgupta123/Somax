/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
****************************************************************************************************
 */
using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_PartMasterRequest
    {
        public long CustomQueryDisplayId { get; set; }
        public string Requester { get; set; }
        public string Requester_Email { get; set; }
        public string ApproveDenyBy { get; set; }
        public string ApproveDenyBy2 { get; set; }
        public string LastReviewedBy { get; set; }
        public string PartMasterCreateBy { get; set; }
        public long EXPartId { get; set; }
        public long EXPartId_Replace { get; set; }
        public string PartMaster_ClientLookupId { get; set; }
        public string Part_ClientLookupId { get; set; }
        public string Part_PrevClientLookupId { get; set; }
        public string PartMasterClientLookUpId_Replace { get; set; }
        public string Part_Description { get; set; }
        public string CompleteBy { get; set; }
        public string PartMaster_LongDescription { get; set; }
        public string Site_Name { get; set; }
        public string ExOrganizationId { get; set; }
        #region V2-798
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public long CreateById { get; set; }
        public int TotalCount { get; set; }
        #endregion

        public void CreateNewPartNewRequestor(
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
                Database.StoredProcedure.usp_PartMasterRequest_CreateNewPartMasterRequest.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveAllForSearch(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_PartMasterRequest> results
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

                results = Database.StoredProcedure.usp_PartMasterRequest_RetrieveDataByFiltering.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveExtractData(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_PartMasterRequest> results
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

                results = Database.StoredProcedure.usp_PartMasterRequest_RetrieveForPMRExport.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByPKForPMLocalAddFromDB(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName
)
        {
            Database.SqlClient.ProcessRow<b_PartMasterRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMasterRequest>(reader => { this.LoadFromDatabaseForPMLocalAddition(reader); return this; });
                Database.StoredProcedure.usp_PartMasterRequest_RetrieveForPMLocalAddition.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByForeignKeysFromDatabase(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName
    )
        {
            Database.SqlClient.ProcessRow<b_PartMasterRequest> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMasterRequest>(reader => { this.LoadFromDatabaseByFK(reader); return this; });
                Database.StoredProcedure.usp_PartMasterRequest_RetrieveByFK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void UpdateForDenied(
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
                Database.StoredProcedure.usp_PartMasterRequest_UpdateForDenied.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateForPMLocalAddition(
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
                Database.StoredProcedure.usp_PartMasterRequest_UpdateForPMLocalAddition.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ValidationPartMasterNumberLookup(SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_PartMaster_ValidationNumberLookup.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ValidationSomaxPartLookup(SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<b_StoredProcValidationError> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_StoredProcValidationError> results = null;
            data = new List<b_StoredProcValidationError>();
            try
            {
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                results = Database.StoredProcedure.usp_PartMaster_ValidationSomaxPartLookup.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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



        //--------------------------------------------
        public static b_PartMasterRequest ProcessRowForExtract(SqlDataReader reader)
        {
            // Create instance of object
            b_PartMasterRequest partMasterRequest = new b_PartMasterRequest();

            partMasterRequest.LoadFromDatabaseForForExtract(reader);

            return partMasterRequest;
        }
        public void LoadFromDatabaseForForExtract(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Part_ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Part_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Part_ClientLookupId = string.Empty;
                }
                i++;
                // PartMaster_ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PartMaster_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartMaster_ClientLookupId = string.Empty;
                }
                i++;
                // PartMasterClientLookUpId_Replace column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PartMasterClientLookUpId_Replace = reader.GetString(i);
                }
                else
                {
                    PartMasterClientLookUpId_Replace = string.Empty;
                }
                i++;
                // EXPartId column, bigint, not null
                EXPartId = reader.GetInt64(i++);
                // EXPartId_Replace column, bigint, not null
                EXPartId_Replace = reader.GetInt64(i++);
                // PartMasterRequestId column, bigint, not null
                PartMasterRequestId = reader.GetInt64(i++);
                // ExOrganizationId column, nvarchar(15), not null
                ExOrganizationId = reader.GetString(i++);
                // RequestType,  not null
                if (false == reader.IsDBNull(i))
                {
                    RequestType = reader.GetString(i);
                }
                else
                {
                    RequestType = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch
                {
                    missing.Append("SiteId ");

                    try { reader["SanitationJobId"].ToString(); }
                    catch { missing.Append("SanitationJobId "); }

                    try { reader["ClientLookupId"].ToString(); }
                    catch { missing.Append("ClientLookupId "); }

                    try { reader["Description"].ToString(); }
                    catch { missing.Append("Description"); }

                    try { reader["ChargeToId"].ToString(); }
                    catch { missing.Append("ChargeToId "); }

                    try { reader["ChargeToClientLookupId"].ToString(); }
                    catch { missing.Append("ChargeToClientLookupId "); }

                    try { reader["ChargeTo_Name"].ToString(); }
                    catch { missing.Append("ChargeTo_Name "); }

                    try { reader["Status"].ToString(); }
                    catch { missing.Append("Status "); }

                    try { reader["Shift"].ToString(); }
                    catch { missing.Append("Shift "); }

                    try { reader["CreateDate"].ToString(); }
                    catch { missing.Append("CreateDate "); }

                }
            }

        }
        //----------------------------------------

        public static b_PartMasterRequest ProcessRowForRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PartMasterRequest partMasterRequest = new b_PartMasterRequest();

            partMasterRequest.LoadFromDatabaseForRetriveAllForSearch(reader);

            return partMasterRequest;
        }
        public void LoadFromDatabaseForRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // PartMasterRequestId column, bigint, not null
                PartMasterRequestId = reader.GetInt64(i++);

                // Requester column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Requester = reader.GetString(i);
                }
                else
                {
                    Requester = string.Empty;
                }
                i++;

                // Justification column, not null
                if (false == reader.IsDBNull(i))
                {
                    Justification = reader.GetString(i);

                }
                else
                {
                    Justification = string.Empty;
                }
                i++;
                // Status RequestType,  not null
                if (false == reader.IsDBNull(i))
                {
                    RequestType = reader.GetString(i);
                }
                else
                {
                    RequestType = string.Empty;
                }
                i++;
                // Status column,  not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // Manufacturer column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = string.Empty;
                }
                i++;

                // ManufacturerId column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = string.Empty;
                }
                i++;

                // TotalCount column, bigint, not null
                TotalCount = reader.GetInt32(i++);






            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch
                {
                    missing.Append("SiteId ");

                    try { reader["SanitationJobId"].ToString(); }
                    catch { missing.Append("SanitationJobId "); }

                    try { reader["ClientLookupId"].ToString(); }
                    catch { missing.Append("ClientLookupId "); }

                    try { reader["Description"].ToString(); }
                    catch { missing.Append("Description"); }

                    try { reader["ChargeToId"].ToString(); }
                    catch { missing.Append("ChargeToId "); }

                    try { reader["ChargeToClientLookupId"].ToString(); }
                    catch { missing.Append("ChargeToClientLookupId "); }

                    try { reader["ChargeTo_Name"].ToString(); }
                    catch { missing.Append("ChargeTo_Name "); }

                    try { reader["Status"].ToString(); }
                    catch { missing.Append("Status "); }

                    try { reader["Shift"].ToString(); }
                    catch { missing.Append("Shift "); }

                    try { reader["CreateDate"].ToString(); }
                    catch { missing.Append("CreateDate "); }

                }
                try { reader["PartMasterRequestId"].ToString(); }
                catch { missing.Append("PartMasterRequestId "); }

                try { reader["Requester"].ToString(); }
                catch { missing.Append("Requester "); }

                try { reader["Justification"].ToString(); }
                catch { missing.Append("Justification "); }

                try { reader["RequestType"].ToString(); }
                catch { missing.Append("RequestType "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }
            }

        }

        public void LoadFromDatabaseForPMLocalAddition(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // ApproveDenyBy column
                if (false == reader.IsDBNull(i))
                {
                    ApproveDenyBy = reader.GetString(i);

                }
                else
                {
                    ApproveDenyBy = string.Empty;
                }
                i++;

                // LastReviewedBy column
                if (false == reader.IsDBNull(i))
                {
                    LastReviewedBy = reader.GetString(i);
                }
                else
                {
                    LastReviewedBy = string.Empty;
                }
                i++;

                // Requester column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Requester = reader.GetString(i);
                }
                else
                {
                    Requester = string.Empty;
                }
                i++;

                // PartMaster_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    PartMaster_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartMaster_ClientLookupId = string.Empty;
                }
                i++;
                // Site Name
                if (false == reader.IsDBNull(i))
                {
                    Site_Name = reader.GetString(i);
                }
                else
                {
                    Site_Name = string.Empty;
                }
                i++;
                // Part_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    Part_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Part_ClientLookupId = string.Empty;
                }
                i++;

                // Part_Description column
                if (false == reader.IsDBNull(i))
                {
                    Part_Description = reader.GetString(i);
                }
                else
                {
                    Part_Description = string.Empty;
                }
                i++;
                // PartMaster_LongDescription column
                if (false == reader.IsDBNull(i))
                {
                    PartMaster_LongDescription = reader.GetString(i);
                }
                else
                {
                    PartMaster_LongDescription = string.Empty;
                }
                i++;
                // CompleteBy column
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy = reader.GetString(i);
                }
                else
                {
                    CompleteBy = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ApproveDenyBy"].ToString(); }
                catch { missing.Append("ApproveDenyBy "); }

                try { reader["LastReviewedBy"].ToString(); }
                catch { missing.Append("LastReviewedBy "); }

                try { reader["PartMasterCreateBy"].ToString(); }
                catch { missing.Append("PartMasterCreateBy "); }

                try { reader["PartMaster_ClientLookupId"].ToString(); }
                catch { missing.Append("PartMaster_ClientLookupId "); }

            }
        }
        public void LoadFromDatabaseByFK(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // Requester column
                if (false == reader.IsDBNull(i))
                {
                    Requester = reader.GetString(i);
                }
                else
                {
                    Requester = string.Empty;
                }
                i++;

                // Requester Email column
                if (false == reader.IsDBNull(i))
                {
                    Requester_Email = reader.GetString(i);
                }
                else
                {
                    Requester_Email = string.Empty;
                }
                i++;

                // ApproveDenyBy column
                if (false == reader.IsDBNull(i))
                {
                    ApproveDenyBy = reader.GetString(i);

                }
                else
                {
                    ApproveDenyBy = string.Empty;
                }
                i++;
                // ApproveDenyBy2 column
                if (false == reader.IsDBNull(i))
                {
                    ApproveDenyBy2 = reader.GetString(i);

                }
                else
                {
                    ApproveDenyBy2 = string.Empty;
                }
                i++;
                // LastReviewedBy column
                if (false == reader.IsDBNull(i))
                {
                    LastReviewedBy = reader.GetString(i);
                }
                else
                {
                    LastReviewedBy = string.Empty;
                }
                i++;
                // PartMasterCreateBy column
                if (false == reader.IsDBNull(i))
                {
                    PartMasterCreateBy = reader.GetString(i);
                }
                else
                {
                    PartMasterCreateBy = string.Empty;
                }
                i++;
                // PartMaster_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    PartMaster_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartMaster_ClientLookupId = string.Empty;
                }
                i++;
                // Part_ClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    Part_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Part_ClientLookupId = string.Empty;
                }
                i++;
                // Part_PrevClientLookupId column
                if (false == reader.IsDBNull(i))
                {
                    Part_PrevClientLookupId = reader.GetString(i);
                }
                else
                {
                    Part_PrevClientLookupId = string.Empty;
                }
                i++;
                // Site Name
                if (false == reader.IsDBNull(i))
                {
                    Site_Name = reader.GetString(i);
                }
                else
                {
                    Site_Name = string.Empty;
                }
                i++;
                // Site Name
                if (false == reader.IsDBNull(i))
                {
                    Part_Description = reader.GetString(i);
                }
                else
                {
                    Part_Description = string.Empty;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Requester"].ToString(); }
                catch { missing.Append("Requester "); }

                try { reader["Requester_Email"].ToString(); }
                catch { missing.Append("Requester_Email "); }

                try { reader["ApproveDenyBy"].ToString(); }
                catch { missing.Append("ApproveDenyBy "); }

                try { reader["LastReviewedBy"].ToString(); }
                catch { missing.Append("LastReviewedBy "); }

                try { reader["PartMasterCreateBy"].ToString(); }
                catch { missing.Append("PartMasterCreateBy "); }

                try { reader["PartMaster_ClientLookupId"].ToString(); }
                catch { missing.Append("PartMaster_ClientLookupId "); }

                try { reader["Part_ClientLookupId"].ToString(); }
                catch { missing.Append("Part_ClientLookupId "); }

                try { reader["Part_PrevClientLookupId"].ToString(); }
                catch { missing.Append("Part_PrevClientLookupId "); }

                try { reader["Site_Name"].ToString(); }
                catch { missing.Append("Site_Name "); }

                try { reader["Part_Description"].ToString(); }
                catch { missing.Append("Part_Description "); }

            }
        }

        public void CreateByFK(
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
                Database.StoredProcedure.usp_PartMasterRequest_CreateByFK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
