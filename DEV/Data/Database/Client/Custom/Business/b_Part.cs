/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014-2019 by SOMAX Inc.
* Part.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
*============ ======== ================== ==========================================================
* 2013-May-16          Roger Lawton       Added Storeroom Table information
* 2014-Dec-20 SOM-451  Roger Lawton       Added Account_ClientLookupId, AltPart1-3, LastPurchaseCost
* 2015-Feb-05 SOM-531  Roger Lawton       Add Manufacturer and ManufacturerId
* 2017-Mar-08 SOM-1250 Roger Lawton       Added Location1_5 to exception area of  
*                                         LoadFromDatabaseExtended method
*                                         Also added Location1_5 to LoadFromDatabaseForPartSearch 
* 2019-Jun-06 SOM-1694 Roger Lawton       Removed unused code - Removed PartImage Column       
* 2019-Jul-14 SOM-1708 Roger Lawton       Added Manufacturer                                  
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
    /// <summary>
    /// Business object that stores a record from the TechSpecs table.
    /// </summary>
    public partial class b_Part
    {
        #region properties
        // Storeroom Table Information
        public Int64 PartStoreroomId { get; set; }
        public int CountFrequency { get; set; }
        public DateTime? LastCounted { get; set; }
        public string Location1_1 { get; set; }
        public string Location1_5 { get; set; }
        public string Location1_2 { get; set; }
        public string Location1_3 { get; set; }
        public string Location1_4 { get; set; }
        public decimal QtyMaximum { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal QtyReorderLevel { get; set; }
        public string ReorderMethod { get; set; }
        public Int32 Storeroom_UpdateIndex { get; set; }
        // Calculated Quantities
        public decimal QtyOnOrder { get; set; }
        public decimal QtyOnRequest { get; set; }
        public decimal QtyReserved { get; set; }
        public decimal LastPurchaseCost { get; set; }               // SOM-451
        // Additional ClientLookups
        public string Account_ClientLookupId { get; set; }      // SOM-451
        public string AltPartId1_ClientLookupId { get; set; }   // SOM-451
        public string AltPartId2_ClientLookupId { get; set; }   // SOM-451
        public string AltPartId3_ClientLookupId { get; set; }   // SOM-451
        public string PartMasterClientLookupId { get; set; }
        public string Location { get; set; }

        public string ShortDescription { get; set; }
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }

        //public string ManufacturerId { get; set; }
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ProcessMode { get; set; }
        public Int64 PersonnelId { get; set; }
        public decimal AverageCostBefore { get; set; }
        public decimal AverageCostAfter { get; set; }
        public decimal CostAfter { get; set; }
        public decimal CostBefore { get; set; }
        public Int64 PerformById { get; set; }
        public string TransactionType { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public string LastPurchaseVendor { get; set; }
        public string SiteName { get; set; }
        public string PartIdList { get; set; }

        //SOM-1495
        public int CustomQueryDisplayId { get; set; }
        public int OperModeFlag { get; set; }
        public Int64 AutoPurchLogId { get; set; }
        public Int64 VendorId { get; set; }
        public string Vendor_ClientLookupId { get; set; }
        public string Vendor_Name { get; set; }
        public decimal QtyToOrder { get; set; }
        public decimal UnitCost { get; set; }
        public string Flag { get; set; }



        public string PONumber { get; set; }

        public string POType { get; set; }

        public string POStatus { get; set; }

        public int LineNumber { get; set; }

        public string LineStatus { get; set; }
        public string PlDesc { get; set; }
        public string VendorClientlookupId { get; set; }
        public string VendorName { get; set; }

        //V2-291
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string ManufactureId { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string Shelf { get; set; }
        public string Bin { get; set; }
        public string PlaceArea { get; set; }
        public string Stock { get; set; }
        public string SearchText { get; set; }
        public string PMClientlookupId { get; set; }
        public string PMDescription { get; set; }
        public Int64 VendorMasterId { get; set; }
        public Int64 VendorCatalogItemId { get; set; }
        public Int64 VendorCatalogId { get; set; }
        public string PurchaseUOM { get; set; }
        public string VendorPartNumber { get; set; }
        public int VendorLeadTime { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public bool PartInactiveFlag { get; set; }
        public string CategoryDescription { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public List<b_Part> listOfPart { get; set; }
        public int TotalCount { get; set; }
        public long RowNum { get; set; }
        public string AttachmentUrl { get; set; }
        public bool CheckFlag { get; set; }
        public string CatalogType { get; set; }
        public long PoPrId { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        //V2-553
        public string IssueUOM { get; set; }
        public decimal UOMConversion { get; set; }
        public bool UOMConvRequired { get; set; }
        public int VC_Count { get; set; }
        public decimal IssueOrder { get; set; }
        public decimal TransactionQuantity { get; set; }
        public DateTime? GenerateThrough { get; set; }
        public long PartHistoryId { get; set; }

        //V2-668
        public string PartMaster_ClientLookupId { get; set; }
        public string LongDescription { get; set; }
        // public string ShortDescription { get; set; }
        public string Category { get; set; }
        public string CategoryDesc { get; set; }

        //V2-670
        public int ChildCount { get; set; }
        public string AccountClientLookupId { get; set; }
        public string IssueUnitDescription { get; set; }
        public string StockTypeDescription { get; set; }
        //V2-690
        public long PartCategoryMasterId { get; set; }
        public long IndexId { get; set; }

        //V2-932
        public decimal OnOrderQty { get; set; }
        public decimal OnRequestQTY { get; set; }
        #region V2-1025
        public string Storerooms { get; set; }
        public string DefStoreroom { get; set; }
        public decimal TotalOnRequest { get; set; }
        public decimal TotalOnOrder { get; set; }
        public decimal TotalOnHand { get; set; }
        #endregion
        public bool AutoPurchase { get; set; }
        #endregion

        public static b_Part ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return parts;
        }

        public void LoadFromDatabaseForPartIdByClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

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

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
                Description = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

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
        // SOM-1694 - Not used - no reference 
        /*
        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveBySiteIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Part> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Part> results = null;
            data = new List<b_Part>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //results = Database.StoredProcedure.usp_Part_RetrieveBySiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Part>();
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
        */
        /// <summary>
        /// Update the Part and Storeroom records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void UpdateByPartId(
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
                Database.StoredProcedure.usp_PartStorerooms_UpdateByPartId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void ProcessPart(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_Part_ProcessPart.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        /// Retrieve Part table record and extended properties using specified part id
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="key">System.Guid that contains the key to use in the lookup</param>
        public void RetrieveByPartId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
                Database.StoredProcedure.usp_PartsStorerooms_RetrieveByPartId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /**********************************************Added By Indusnet Technologies*****************************/

        //
        public void RetrieveByPartIdForPI(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseExtendedForPI(reader); return this; });
                Database.StoredProcedure.usp_Part_RetrieveByPartIdForPI.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        //


        public void RetrieveForSearchBySiteId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_RetrieveForSearchBySiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrievePartSiteReview(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_Part> results

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

                results = Database.StoredProcedure.usp_Part_RetrievePartSiteReview.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void RetrieveForSearchForMultipleSite(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_Part> results

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

                results = Database.StoredProcedure.usp_Part_RetrieveForSearchByMultipleSiteId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, FilterText);

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
        public static b_Part ProcessRowForPartSiteReview(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForPartSiteReview(reader);

            // Return result
            return parts;
        }
        public static b_Part ProcessRowForPartSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForPartSearch(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseForPartSiteReview(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);

                }
                else
                {
                    ClientLookupId = "";
                }
                i++;
                AppliedCost = reader.GetDecimal(i++);

                AverageCost = reader.GetDecimal(i++);

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
                    InactiveFlag = reader.GetBoolean(i);

                }
                else
                {
                    InactiveFlag = false;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);

                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                PartMasterId = reader.GetInt64(i++);

                QtyOnHand = reader.GetDecimal(i++);

                QtyOnOrder = reader.GetDecimal(i++);

                QtyOnRequest = reader.GetDecimal(i++);

                LastPurchaseCost = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    LastPurchaseDate = reader.GetDateTime(i);

                }
                else
                {
                    LastPurchaseDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LastPurchaseVendor = reader.GetString(i);

                }
                else
                {
                    LastPurchaseVendor = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag"); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit"); }

                try { reader["PartMasterId"].ToString(); }      // SOM-531
                catch { missing.Append("PartMasterId"); }       // SOM-531

                try { reader["QtyOnHand"].ToString(); }    // SOM-531
                catch { missing.Append("QtyOnHand"); }     // SOM-531

                try { reader["QtyOnOrder"].ToString(); }
                catch { missing.Append("QtyOnOrder"); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand"); }

                try { reader["QtyOnRequest"].ToString(); }
                catch { missing.Append("QtyOnRequest"); }

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost"); }

                try { reader["LastPurchaseDate"].ToString(); }
                catch { missing.Append("LastPurchaseDate"); }

                try { reader["LastPurchaseVendor"].ToString(); }
                catch { missing.Append("LastPurchaseVendor"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void LoadFromDatabaseForPartSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                UPCCode = reader.GetString(i++);

                Description = reader.GetString(i++);

                StockType = reader.GetString(i++);

                Manufacturer = reader.GetString(i++);         // SOM-531

                ManufacturerId = reader.GetString(i++);       // SOM-531

                InactiveFlag = reader.GetBoolean(i++);        // SOM-1495

                PrevClientLookupId = reader.GetString(i++);   // SOM-1576

                Consignment = reader.GetBoolean(i++);          // SOM-1610

                Location1_1 = reader.GetString(i++);

                QtyOnHand = reader.GetDecimal(i++);

                Location1_2 = reader.GetString(i++);

                Location1_3 = reader.GetString(i++);

                Location1_4 = reader.GetString(i++);

                Location1_5 = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode"); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType"); }

                try { reader["Manufacturer"].ToString(); }      // SOM-531
                catch { missing.Append("Manufacturer"); }       // SOM-531

                try { reader["ManufacturerId"].ToString(); }    // SOM-531
                catch { missing.Append("ManufacturerId"); }     // SOM-531

                try { reader["InactiveFlag"].ToString(); }      // SOM-1495 
                catch { missing.Append("InactiveFlag"); }       // SOM-1495 

                try { reader["PrevClientLookupId"].ToString(); }  // SOM-1576
                catch { missing.Append("PrevClientLookupId "); }  // SOM-1576

                try { reader["Consignment"].ToString(); }         // SOM-1610
                catch { missing.Append("Consignment "); }         // SOM-1610

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1"); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand"); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2"); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3"); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4"); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Part ProcessRowForPartMultipleSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForPartMultipleSearch(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseForPartMultipleSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);
                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                Description = reader.GetString(i++);

                QtyOnHand = reader.GetDecimal(i++);

                Manufacturer = reader.GetString(i++);       // SOM-1708

                ManufacturerId = reader.GetString(i++);

                Name = reader.GetString(i++);

                AddressCity = reader.GetString(i++);

                AddressState = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }


                try { reader["Description"].ToString(); }
                catch { missing.Append("Description"); }


                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand"); }

                try { reader["Manufacturer"].ToString(); }    // SOM-1708
                catch { missing.Append("Manufacturer"); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId"); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name"); }

                try { reader["AddressCity"].ToString(); }
                catch { missing.Append("AddressCity"); }

                try { reader["AddressState"].ToString(); }
                catch { missing.Append("AddressState"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Part ProcessRowForRetrieveByClientLookUpIdNUPCCode(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForRetrieveByClientLookUpIdNUPCCode(reader);

            // Return result
            return parts;
        }

        public void LoadFromDatabaseForRetrieveByClientLookUpIdNUPCCode(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // UPCCode column, nvarchar(31), not null
                UPCCode = reader.GetString(i++);

                // Description column
                Description = reader.GetString(i++);

                AppliedCost = reader.GetDecimal(i++);

                AverageCost = reader.GetDecimal(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

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
        }

        public void RetrieveByPartClientLookUpIdNUPCCode(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref b_Part retPart
            )
        {
            //Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                // processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseExtended(reader); return this; });
                retPart = Database.StoredProcedure.usp_Part_RetrieveByClientLookUpIdNUPCCode.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
            }
            message = String.Empty;
            callerUserInfoId = 0;
            callerUserName = String.Empty;

        }
        public void RetrievePartByFilterText(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_RetrieveBYFilterText.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Part ProcessRowForRetrieveByFilterText(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForRetrieveByFilterText(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseForRetrieveByFilterText(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // PartsId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description column
                Description = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void SearchForCart(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCart_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void SearchForCartWO(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCartWO_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Part ProcessRowSearchForCart(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCart(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseSearchForCart(SqlDataReader reader)
        {
            int i = 0;
            try
            { // IndexId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // StockType column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;

                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;

                // ManufacturerId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // IssueUnit column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                // AppliedCost column, bigint, not null
                AppliedCost = reader.GetDecimal(i++);

                // AverageCost column, bigint, not null
                AverageCost = reader.GetDecimal(i++);

                // QtyOnHand column, bigint, not null
                QtyOnHand = reader.GetDecimal(i++);

                //InactiveFlag, boolean
                InactiveFlag = reader.GetBoolean(i++);

                // AttachmentUrl column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    AttachmentUrl = reader.GetString(i);
                }
                else
                {
                    AttachmentUrl = "";
                }
                i++;
                //V2-424            
                if (false == reader.IsDBNull(i))
                {
                    QtyMaximum = reader.GetDecimal(i);
                }
                else
                {
                    QtyMaximum = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyReorderLevel = reader.GetDecimal(i);
                }
                else
                {
                    QtyReorderLevel = 0;
                }
                i++;
                //V2-424

                if (false == reader.IsDBNull(i))
                {
                    IssueOrder = reader.GetDecimal(i);
                }
                else
                {
                    IssueOrder = 1.0m;
                }
                i++;

                UOMConvRequired = reader.GetBoolean(i++);
                //V2-932
                if (false == reader.IsDBNull(i))
                {
                    OnOrderQty = reader.GetDecimal(i);
                }
                else
                {
                    OnOrderQty = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    OnRequestQTY = reader.GetDecimal(i);
                }
                else
                {
                    OnRequestQTY = 0;
                }
                i++;

                // TotalCount column, bigint, not null
                TotalCount = reader.GetInt32(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["AttachmentUrl"].ToString(); }
                catch { missing.Append("AttachmentUrl "); }

                //V2-424              
                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }
                //V2-424

                try { reader["IssueOrder"].ToString(); }
                catch { missing.Append("IssueOrder "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_Part ProcessRowSearchForCartWO(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCartWO(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseSearchForCartWO(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // IndexId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // StockType column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;

                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;

                // ManufacturerId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // IssueUnit column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                // AppliedCost column, bigint, not null
                AppliedCost = reader.GetDecimal(i++);

                // AverageCost column, bigint, not null
                AverageCost = reader.GetDecimal(i++);

                // QtyOnHand column, bigint, not null
                QtyOnHand = reader.GetDecimal(i++);

                //InactiveFlag, boolean
                InactiveFlag = reader.GetBoolean(i++);

                // AttachmentUrl column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    AttachmentUrl = reader.GetString(i);
                }
                else
                {
                    AttachmentUrl = "";
                }
                i++;
                //V2-424            
                if (false == reader.IsDBNull(i))
                {
                    QtyMaximum = reader.GetDecimal(i);
                }
                else
                {
                    QtyMaximum = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyReorderLevel = reader.GetDecimal(i);
                }
                else
                {
                    QtyReorderLevel = 0;
                }
                i++;
                //V2-424

                if (false == reader.IsDBNull(i))
                {
                    IssueOrder = reader.GetDecimal(i);
                }
                else
                {
                    IssueOrder = 1.0m;
                }
                i++;

                UOMConvRequired = reader.GetBoolean(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // VendorClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientlookupId = "";
                }
                i++;
                // AccountId V2-1068
                AccountId = reader.GetInt64(i++);
                // V2-1068 AccountId UnitOfMeasure is already taken IssueUnit from Part table
                // TotalCount column, bigint, not null
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["AttachmentUrl"].ToString(); }
                catch { missing.Append("AttachmentUrl "); }

                //V2-424              
                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }
                //V2-424

                try { reader["IssueOrder"].ToString(); }
                catch { missing.Append("IssueOrder "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VendorClientlookupId"].ToString(); }
                catch { missing.Append("VendorClientlookupId "); }
                //V2-1068
                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void SearchForCart_VendorCatalog(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCart_VendorCatalog_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void SearchForCart_VendorCatalogWO(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCart_VendorCatalogWO_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_Part ProcessRowSearchForCart_VendorCatalog(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCart_VendorCatalog(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseSearchForCart_VendorCatalog(SqlDataReader reader)
        {
            int i = 0;
            try
            {  // IndexId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // PMClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PMClientlookupId = reader.GetString(i);
                }
                else
                {
                    PMClientlookupId = "";
                }
                i++;
                // PMDescription column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PMDescription = reader.GetString(i);
                }
                else
                {
                    PMDescription = "";
                }
                i++;
                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;
                // ManufacturerId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;
                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // VendorName column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                // VendorMasterId column, bigint, not null
                VendorMasterId = reader.GetInt64(i++);
                // VendorCatalogItemId column, bigint, not null
                VendorCatalogItemId = reader.GetInt64(i++);
                // VendorCatalogId column, bigint, not null
                VendorCatalogId = reader.GetInt64(i++);
                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);
                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // IssueUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUOM = reader.GetString(i);
                }
                else
                {
                    IssueUOM = "";
                }
                i++;

                // UnitCost column, decimal, not null
                UnitCost = reader.GetDecimal(i++);

                // VendorPartNumber column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorPartNumber = reader.GetString(i);
                }
                else
                {
                    VendorPartNumber = "";
                }
                i++;

                // VendorLeadTime column, int, not null
                VendorLeadTime = reader.GetInt32(i++);

                // MinimumOrderQuantity column, int, not null
                MinimumOrderQuantity = reader.GetInt32(i++);

                // PartInactiveFlag column, bool, not null
                PartInactiveFlag = reader.GetBoolean(i++);


                // CategoryDescription column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CategoryDescription = reader.GetString(i);
                }
                else
                {
                    CategoryDescription = "";
                }
                i++;

                // AttachmentUrl column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    AttachmentUrl = reader.GetString(i);
                }
                else
                {
                    AttachmentUrl = "";
                }
                i++;

                // CatalogType column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CatalogType = reader.GetString(i);
                }
                else
                {
                    CatalogType = "";
                }
                i++;

                //V2-424
                if (false == reader.IsDBNull(i))
                {
                    QtyOnHand = reader.GetDecimal(i);
                }
                else
                {
                    QtyOnHand = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyMaximum = reader.GetDecimal(i);
                }
                else
                {
                    QtyMaximum = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyReorderLevel = reader.GetDecimal(i);
                }
                else
                {
                    QtyReorderLevel = 0;
                }
                i++;
                //V2-424
                //V2-553
                if (false == reader.IsDBNull(i))
                {
                    UOMConversion = reader.GetDecimal(i);
                }
                else
                {
                    UOMConversion = 0;
                }
                i++;

                UOMConvRequired = reader.GetBoolean(i++);

                VC_Count = reader.GetInt32(i++);

                //IssueUnit
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;
                // PartCategoryMasterId column, long, not null
                PartCategoryMasterId = reader.GetInt64(i++);

                //V2-932
                if (false == reader.IsDBNull(i))
                {
                    OnOrderQty = reader.GetDecimal(i);
                }
                else
                {
                    OnOrderQty = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    OnRequestQTY = reader.GetDecimal(i);
                }
                else
                {
                    OnRequestQTY = 0;
                }
                i++;

                //V2-1068 AccountId column and (UnitOfMeasure is already taken IssueUOM from PartMaster table)
                AccountId = reader.GetInt64(i++);

                // TotalCount column, int, not null
                TotalCount = reader.GetInt32(i++);



            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["PMClientlookupId"].ToString(); }
                catch { missing.Append("PMClientlookupId "); }

                try { reader["PMDescription"].ToString(); }
                catch { missing.Append("PMDescription "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorMasterId"].ToString(); }
                catch { missing.Append("VendorMasterId "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VendorCatalogItemId "); }

                try { reader["VendorCatalogId"].ToString(); }
                catch { missing.Append("VendorCatalogId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["IssueUOM"].ToString(); }
                catch { missing.Append("IssueUOM "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["VendorPartNumber"].ToString(); }
                catch { missing.Append("VendorPartNumber "); }

                try { reader["VendorLeadTime"].ToString(); }
                catch { missing.Append("VendorLeadTime "); }

                try { reader["MinimumOrderQuantity"].ToString(); }
                catch { missing.Append("MinimumOrderQuantity "); }

                try { reader["PartInactiveFlag"].ToString(); }
                catch { missing.Append("PartInactiveFlag "); }

                try { reader["CategoryDescription"].ToString(); }
                catch { missing.Append("CategoryDescription "); }

                try { reader["AttachmentUrl"].ToString(); }
                catch { missing.Append("AttachmentUrl "); }

                try { reader["CatalogType"].ToString(); }
                catch { missing.Append("CatalogType "); }

                //V2-424
                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }
                //V2-424

                try { reader["UOMConversion"].ToString(); }
                catch { missing.Append("UOMConversion "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["VC_Count"].ToString(); }
                catch { missing.Append("VC_Count "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["PartCategoryMasterId"].ToString(); }
                catch { missing.Append("PartCategoryMasterId "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

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

        public static b_Part ProcessRowSearchForCart_VendorCatalogWO(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCart_VendorCatalogWO(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseSearchForCart_VendorCatalogWO(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // IndexId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // PMClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PMClientlookupId = reader.GetString(i);
                }
                else
                {
                    PMClientlookupId = "";
                }
                i++;
                // PMDescription column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PMDescription = reader.GetString(i);
                }
                else
                {
                    PMDescription = "";
                }
                i++;
                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;
                // ManufacturerId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;
                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // VendorName column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                // VendorClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientlookupId = "";
                }
                i++;

                // VendorMasterId column, bigint, not null
                VendorMasterId = reader.GetInt64(i++);
                // VendorCatalogItemId column, bigint, not null
                VendorCatalogItemId = reader.GetInt64(i++);
                // VendorCatalogId column, bigint, not null
                VendorCatalogId = reader.GetInt64(i++);
                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);
                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // IssueUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUOM = reader.GetString(i);
                }
                else
                {
                    IssueUOM = "";
                }
                i++;

                // UnitCost column, decimal, not null
                UnitCost = reader.GetDecimal(i++);

                // VendorPartNumber column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorPartNumber = reader.GetString(i);
                }
                else
                {
                    VendorPartNumber = "";
                }
                i++;

                // VendorLeadTime column, int, not null
                VendorLeadTime = reader.GetInt32(i++);

                // MinimumOrderQuantity column, int, not null
                MinimumOrderQuantity = reader.GetInt32(i++);

                // PartInactiveFlag column, bool, not null
                PartInactiveFlag = reader.GetBoolean(i++);


                // CategoryDescription column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CategoryDescription = reader.GetString(i);
                }
                else
                {
                    CategoryDescription = "";
                }
                i++;

                // AttachmentUrl column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    AttachmentUrl = reader.GetString(i);
                }
                else
                {
                    AttachmentUrl = "";
                }
                i++;

                // CatalogType column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CatalogType = reader.GetString(i);
                }
                else
                {
                    CatalogType = "";
                }
                i++;

                //V2-424
                if (false == reader.IsDBNull(i))
                {
                    QtyOnHand = reader.GetDecimal(i);
                }
                else
                {
                    QtyOnHand = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyMaximum = reader.GetDecimal(i);
                }
                else
                {
                    QtyMaximum = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyReorderLevel = reader.GetDecimal(i);
                }
                else
                {
                    QtyReorderLevel = 0;
                }
                i++;
                //V2-424
                //V2-553
                if (false == reader.IsDBNull(i))
                {
                    UOMConversion = reader.GetDecimal(i);
                }
                else
                {
                    UOMConversion = 0;
                }
                i++;

                UOMConvRequired = reader.GetBoolean(i++);

                VC_Count = reader.GetInt32(i++);

                //IssueUnit
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;
                // PartCategoryMasterId column, long, not null
                PartCategoryMasterId = reader.GetInt64(i++);

                // AccountId V2-1068
                AccountId = reader.GetInt64(i++);
                //UnitOfmesure V2-1068 is allready exit in Part Master - IssueUOM table

                // TotalCount column, int, not null
                TotalCount = reader.GetInt32(i++);



            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["PMClientlookupId"].ToString(); }
                catch { missing.Append("PMClientlookupId "); }

                try { reader["PMDescription"].ToString(); }
                catch { missing.Append("PMDescription "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorMasterId"].ToString(); }
                catch { missing.Append("VendorMasterId "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VendorCatalogItemId "); }

                try { reader["VendorCatalogId"].ToString(); }
                catch { missing.Append("VendorCatalogId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["IssueUOM"].ToString(); }
                catch { missing.Append("IssueUOM "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["VendorPartNumber"].ToString(); }
                catch { missing.Append("VendorPartNumber "); }

                try { reader["VendorLeadTime"].ToString(); }
                catch { missing.Append("VendorLeadTime "); }

                try { reader["MinimumOrderQuantity"].ToString(); }
                catch { missing.Append("MinimumOrderQuantity "); }

                try { reader["PartInactiveFlag"].ToString(); }
                catch { missing.Append("PartInactiveFlag "); }

                try { reader["CategoryDescription"].ToString(); }
                catch { missing.Append("CategoryDescription "); }

                try { reader["AttachmentUrl"].ToString(); }
                catch { missing.Append("AttachmentUrl "); }

                try { reader["CatalogType"].ToString(); }
                catch { missing.Append("CatalogType "); }

                //V2-424
                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }
                //V2-424

                try { reader["UOMConversion"].ToString(); }
                catch { missing.Append("UOMConversion "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["VC_Count"].ToString(); }
                catch { missing.Append("VC_Count "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["PartCategoryMasterId"].ToString(); }
                catch { missing.Append("PartCategoryMasterId "); }

                //V2-1068

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

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

        public void ValidateClientLookupIdForPI(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_ValidateClientLookupIdForPI.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateClientLookupId(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_ValidateClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void ValidateByInactivateorActivate(
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


            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_ValidateByInactivateorActivate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        /// <summary>
        /// Retrieve all Part table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_Part[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_Part[] data
        )
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Part[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { b_Part obj = new b_Part(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_Part_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_Part[])results.ToArray(typeof(b_Part));
                }
                else
                {
                    data = new b_Part[0];
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
        /**********************************************End*******************************************************/

        public void LoadFromDatabaseExtendedForPI(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);
                PartId = reader.GetInt64(i++);
                SiteId = reader.GetInt64(i++);
                ClientLookupId = reader.GetString(i++);
                Description = reader.GetString(i++);
                Location = reader.GetString(i++);
                IssueUnit = reader.GetString(i++);
                PartMasterClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost "); }
                // SOM-451 - End

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

        }


        public void LoadFromDatabaseExtended(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                PartStoreroomId = reader.GetInt64(i++);
                CountFrequency = reader.GetInt32(i++);
                // RKL - 2014-Jul-10
                // Last Counted can be null
                //LastCounted = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    LastCounted = reader.GetDateTime(i);
                }
                else
                {
                    LastCounted = DateTime.MinValue;
                }
                i++;
                Location1_1 = reader.GetString(i++);
                Location1_2 = reader.GetString(i++);
                Location1_3 = reader.GetString(i++);
                Location1_4 = reader.GetString(i++);
                Location1_5 = reader.GetString(i++);
                QtyMaximum = reader.GetDecimal(i++);
                QtyOnHand = reader.GetDecimal(i++);
                QtyReorderLevel = reader.GetDecimal(i++);
                ReorderMethod = reader.GetString(i++);
                Storeroom_UpdateIndex = reader.GetInt32(i++);
                QtyOnOrder = reader.GetDecimal(i++);
                QtyOnRequest = reader.GetDecimal(i++);
                QtyReserved = reader.GetDecimal(i++);
                // SOM-451 - Begin
                Account_ClientLookupId = reader.GetString(i++);
                AltPartId1_ClientLookupId = reader.GetString(i++);
                AltPartId2_ClientLookupId = reader.GetString(i++);
                AltPartId3_ClientLookupId = reader.GetString(i++);
                LastPurchaseCost = reader.GetDecimal(i++);
                // SOM-451 - End

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId"); }

                try { reader["CountFrequency"].ToString(); }
                catch { missing.Append("CountFrequency"); }

                try { reader["LastCounted"].ToString(); }
                catch { missing.Append("LastCounted"); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1"); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2"); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3"); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4"); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5"); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum"); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel"); }

                try { reader["ReorderMethod"].ToString(); }
                catch { missing.Append("ReorderMethod"); }

                try { reader["Storeroom_UpdateIndex"].ToString(); }
                catch { missing.Append("Storeroom_UpdateIndex"); }

                try { reader["QtyOnOrder"].ToString(); }
                catch { missing.Append("QtyOnOrder "); }

                try { reader["QtyOnRequest"].ToString(); }
                catch { missing.Append("QtyOnRequest "); }

                try { reader["QtyReserved"].ToString(); }
                catch { missing.Append("QtyReserved "); }

                // SOM-451 - Begin
                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["AltPartId1_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId1_ClientLookupId "); }

                try { reader["AltPartId2_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId2_ClientLookupId "); }

                try { reader["AltPartId3_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId3_ClientLookupId "); }

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost "); }
                // SOM-451 - End


                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

        }

        //-----SOM-786-----//
        public void LoadFromDatabaseCreateModifyDate(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                // Create By
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

                // Modify By
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;

                // Modify Date 
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    ModifyDate = DateTime.Now;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }

        public void RetrieveCreateModifyDate(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseCreateModifyDate(reader); return this; });
                Database.StoredProcedure.usp_Part_RetrieveCreateModifyDate.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        /*-------------------This Is Used In Api-------------------------------------------------------------------------------------------*/
        public void RetrieveBySiteIdAndClientLookUpId(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_RetrieveBySiteIdAndClientLookUpId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateByIssueUnitStockTypeFromDatabase(SqlConnection connection,
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
                results = Database.StoredProcedure.usp_Part_ValidateByIssueUnitStockType.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void ValidateAdd(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_ValidateAdd.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void UpdateInBulk(
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
                Database.StoredProcedure.usp_Part_UpdateBulk.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void RetrievePOandPRforPart(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
    string callerUserName,
          ref b_Part[] data
      )
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_Part[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { b_Part obj = new b_Part(); obj.LoadFromDatabaseforPOandPR(reader); return obj; });
                results = Database.StoredProcedure.usp_Part_RetrievePOandPR_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_Part[])results.ToArray(typeof(b_Part));
                }
                else
                {
                    data = new b_Part[0];
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


        public int LoadFromDatabaseforPOandPR(SqlDataReader reader)
        {
            int i = 0;
            try
            {



                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);
                PartId = reader.GetInt64(i++);


                if (false == reader.IsDBNull(i))
                {
                    POType = reader.GetString(i);
                }

                else
                {
                    POType = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    PONumber = reader.GetString(i);
                }

                else
                {
                    PONumber = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    POStatus = reader.GetString(i);
                }

                else
                {
                    POStatus = "";
                }
                i++;
                LineNumber = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    LineStatus = reader.GetString(i);
                }
                else
                {
                    LineStatus = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    PlDesc = reader.GetString(i);
                }

                else
                {
                    PlDesc = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }

                else
                {
                    VendorClientlookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }

                else
                {
                    VendorName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;
                PoPrId = reader.GetInt64(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }


                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }


                try { reader["POType"].ToString(); }
                catch { missing.Append("POType "); }

                try { reader["PONumber"].ToString(); }
                catch { missing.Append("PONumber "); }

                try { reader["POStatus"].ToString(); }
                catch { missing.Append("POStatus "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["LineStatus"].ToString(); }
                catch { missing.Append("LineStatus "); }

                try { reader["LineDesc"].ToString(); }
                catch { missing.Append("LineDesc "); }

                try { reader["VendorClientlookupId"].ToString(); }
                catch { missing.Append("VendorClientlookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["PoPrId"].ToString(); }
                catch { missing.Append("PoPrId "); }

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

        public void PartRetrieveForMentionAlert(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseforMentionAlert(reader); return this; });
                StoredProcedure.usp_Part_RetrieveForMentionAlert_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void LoadFromDatabaseforMentionAlert(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Create By
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

                // Modify By
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;

                // Modify Date 
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    ModifyDate = DateTime.Now;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

        }

        public static b_Part ProcessRetrieveForChunkV2(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseForChunkRetrieveV2(reader);
            return Part;
        }

        public int LoadFromDatabaseForChunkRetrieveV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // UPCCode
                if (false == reader.IsDBNull(i))
                {
                    UPCCode = reader.GetString(i);
                }
                else
                {
                    UPCCode = "";
                }
                i++;
                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                // StockType 
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;

                // Manufacturer 
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;

                // ManufacturerId 
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;
                InactiveFlag = reader.GetBoolean(i++);

                // PrevClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    PrevClientLookupId = reader.GetString(i);
                }
                else
                {
                    PrevClientLookupId = "";
                }
                i++;

                Consignment = reader.GetBoolean(i++);
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                // AppliedCost V2-836
                AppliedCost = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Location1_1 = reader.GetString(i);
                }
                else
                {
                    Location1_1 = "";
                }
                i++;

                QtyOnHand = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Location1_2 = reader.GetString(i);
                }
                else
                {
                    Location1_2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_3 = reader.GetString(i);
                }
                else
                {
                    Location1_3 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_4 = reader.GetString(i);
                }
                else
                {
                    Location1_4 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_5 = reader.GetString(i);
                }
                else
                {
                    Location1_5 = "";
                }
                i++;

                QtyReorderLevel = reader.GetDecimal(i++);
                QtyMaximum = reader.GetDecimal(i++);

                TotalCount = reader.GetInt32(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["PrevClientLookupId"].ToString(); }
                catch { missing.Append("PrevClientLookupId "); }

                try { reader["Consignment"].ToString(); }
                catch { missing.Append("Consignment "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1 "); }


                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2 "); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3 "); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4 "); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5 "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }
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

        public void PartRetrieveChunkSearchV2(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void PartRetrieveSearchForPrintV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveForSearchForPrint_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_Part ProcessRetrieveSearchForPrintV2(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseRetrieveSearchForPrintV2(reader);
            return Part;
        }

        public int LoadFromDatabaseRetrieveSearchForPrintV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // UPCCode
                if (false == reader.IsDBNull(i))
                {
                    UPCCode = reader.GetString(i);
                }
                else
                {
                    UPCCode = "";
                }
                i++;
                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
                // StockType 
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;

                // Manufacturer 
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;

                // ManufacturerId 
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;
                InactiveFlag = reader.GetBoolean(i++);

                // PrevClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    PrevClientLookupId = reader.GetString(i);
                }
                else
                {
                    PrevClientLookupId = "";
                }
                i++;

                Consignment = reader.GetBoolean(i++);

                if (false == reader.IsDBNull(i))
                {
                    Location1_1 = reader.GetString(i);
                }
                else
                {
                    Location1_1 = "";
                }
                i++;

                QtyOnHand = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Location1_2 = reader.GetString(i);
                }
                else
                {
                    Location1_2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_3 = reader.GetString(i);
                }
                else
                {
                    Location1_3 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_4 = reader.GetString(i);
                }
                else
                {
                    Location1_4 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_5 = reader.GetString(i);
                }
                else
                {
                    Location1_5 = "";
                }
                i++;

                QtyReorderLevel = reader.GetDecimal(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["PrevClientLookupId"].ToString(); }
                catch { missing.Append("PrevClientLookupId "); }

                try { reader["Consignment"].ToString(); }
                catch { missing.Append("Consignment "); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1 "); }


                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2 "); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3 "); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4 "); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5 "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }

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



        public static b_Part ProcessRetrieveForChunkSearchLookupListV2(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseForChunkSearchLookupListV2(reader);
            return Part;
        }

        public int LoadFromDatabaseForChunkSearchLookupListV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // UPCCode
                if (false == reader.IsDBNull(i))
                {
                    UPCCode = reader.GetString(i);
                }
                else
                {
                    UPCCode = "";
                }
                i++;


                // Manufacturer 
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;

                // ManufacturerId 
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;

                // StockType 
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;

                UpdateIndex = reader.GetInt32(i++);

                #region V2-1124
                // IssueUnit 
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                // AppliedCost 
                if (false == reader.IsDBNull(i))
                {
                    AppliedCost = reader.GetDecimal(i);
                }
                else
                {
                    AppliedCost = 0;
                }
                i++;

                #endregion
                // PartStoreroomId column, bigint, not null
                PartStoreroomId = reader.GetInt64(i++);

                TotalCount = reader.GetInt32(i++);
       

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode "); }


                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }
                
                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }
                
                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId "); }


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



        public void PartChunkSearchLookupListV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_Part ProcessRowForPartIdByClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseForPartIdByClientIdLookup(reader);

            // Return result
            return parts;
        }
        public void RetrievePartIdByClientLookupIdFromDatabase(
                 SqlConnection connection,
                 SqlTransaction transaction,
                 long callerUserInfoId,
string callerUserName,
                 ref b_Part result
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

                result = Database.StoredProcedure.usp_Part_RetrieveByClientLookUpId_V2.CallStoredProcedure(command, this);

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

        #region V2-563
        public static b_Part ProcessRowFor_CalatogEntriesForPartChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCalatogEntriesForPartChunkSearch(reader);

            // Return result
            return parts;
        }

        public void LoadFromDatabaseSearchForCalatogEntriesForPartChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                VendorId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Vendor_ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Vendor_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    Vendor_ClientLookupId = "";
                }
                i++;

                // Vendor_Name column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Vendor_Name = reader.GetString(i);
                }
                else
                {
                    Vendor_Name = "";
                }
                i++;

                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorPartNumber = reader.GetString(i);
                }
                else
                {
                    VendorPartNumber = "";
                }
                i++;

                // UnitCost column, decimal, not null
                UnitCost = reader.GetDecimal(i++);

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // PartStoreroomId column, bigint, not null
                PartStoreroomId = reader.GetInt64(i++);

                // IssueUnit column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                // VendorCatalogItemId column, bigint, not null
                VendorCatalogItemId = reader.GetInt64(i++);

                // TotalCount column, int, not null
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Vendor_ClientLookupId"].ToString(); }
                catch { missing.Append("Vendor_ClientLookupId "); }

                try { reader["Vendor_Name"].ToString(); }
                catch { missing.Append("Vendor_Name "); }

                try { reader["VendorPartNumber"].ToString(); }
                catch { missing.Append("VendorPartNumber "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VendorCatalogItemId "); }

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

        public void SearchForCalatogEntriesForPartChunkSearch(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_CalatogEntriesForPartChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Part cycle count chunk search
        public static b_Part ProcessRetrieveForCycleCountChunkSearch(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseForCycleCountChunkSearch(reader);
            return Part;
        }

        public int LoadFromDatabaseForCycleCountChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // QtyOnHand column, decimal(15,6), not null
                QtyOnHand = reader.GetDecimal(i++);


                if (false == reader.IsDBNull(i))
                {
                    Location1_5 = reader.GetString(i);
                }
                else
                {
                    Location1_5 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_1 = reader.GetString(i);
                }
                else
                {
                    Location1_1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_2 = reader.GetString(i);
                }
                else
                {
                    Location1_2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_3 = reader.GetString(i);
                }
                else
                {
                    Location1_3 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_4 = reader.GetString(i);
                }
                else
                {
                    Location1_4 = "";
                }
                i++;

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5 "); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1 "); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2 "); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3 "); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4 "); }

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
        public void PartRetrieveCycleCountChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_CycleCountChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region V2-668

        public void RetrieveByPartId_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseExtended_V2(reader); return this; });
                Database.StoredProcedure.usp_PartsStorerooms_RetrieveByPartId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseExtended_V2(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                PartStoreroomId = reader.GetInt64(i++);
                CountFrequency = reader.GetInt32(i++);
                // RKL - 2014-Jul-10
                // Last Counted can be null
                //LastCounted = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    LastCounted = reader.GetDateTime(i);
                }
                else
                {
                    LastCounted = DateTime.MinValue;
                }
                i++;
                Location1_1 = reader.GetString(i++);
                Location1_2 = reader.GetString(i++);
                Location1_3 = reader.GetString(i++);
                Location1_4 = reader.GetString(i++);
                Location1_5 = reader.GetString(i++);
                QtyMaximum = reader.GetDecimal(i++);
                QtyOnHand = reader.GetDecimal(i++);
                QtyReorderLevel = reader.GetDecimal(i++);
                ReorderMethod = reader.GetString(i++);
                Storeroom_UpdateIndex = reader.GetInt32(i++);
                QtyOnOrder = reader.GetDecimal(i++);
                QtyOnRequest = reader.GetDecimal(i++);
                QtyReserved = reader.GetDecimal(i++);
                // SOM-451 - Begin
                Account_ClientLookupId = reader.GetString(i++);
                AltPartId1_ClientLookupId = reader.GetString(i++);
                AltPartId2_ClientLookupId = reader.GetString(i++);
                AltPartId3_ClientLookupId = reader.GetString(i++);
                LastPurchaseCost = reader.GetDecimal(i++);
                // SOM-451 - End
                // V2-668 Begin
                if (false == reader.IsDBNull(i))
                {
                    PartMaster_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartMaster_ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LongDescription = reader.GetString(i);
                }
                else
                {
                    LongDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShortDescription = reader.GetString(i);
                }
                else
                {
                    ShortDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Category = reader.GetString(i);
                }
                else
                {
                    Category = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CategoryDesc = reader.GetString(i);
                }
                else
                {
                    CategoryDesc = "";
                }
                i++;
                // V2-668 End

                #region V2-1196
                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientlookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                 i++;
                AutoPurchase = reader.GetBoolean(i);
                i++;
                #endregion


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId"); }

                try { reader["CountFrequency"].ToString(); }
                catch { missing.Append("CountFrequency"); }

                try { reader["LastCounted"].ToString(); }
                catch { missing.Append("LastCounted"); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1"); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2"); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3"); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4"); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5"); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum"); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel"); }

                try { reader["ReorderMethod"].ToString(); }
                catch { missing.Append("ReorderMethod"); }

                try { reader["Storeroom_UpdateIndex"].ToString(); }
                catch { missing.Append("Storeroom_UpdateIndex"); }

                try { reader["QtyOnOrder"].ToString(); }
                catch { missing.Append("QtyOnOrder "); }

                try { reader["QtyOnRequest"].ToString(); }
                catch { missing.Append("QtyOnRequest "); }

                try { reader["QtyReserved"].ToString(); }
                catch { missing.Append("QtyReserved "); }

                // SOM-451 - Begin
                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["AltPartId1_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId1_ClientLookupId "); }

                try { reader["AltPartId2_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId2_ClientLookupId "); }

                try { reader["AltPartId3_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId3_ClientLookupId "); }

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost "); }
                // SOM-451 - End

                // V2-668 - Begin
                try { reader["PartMaster_ClientLookupId"].ToString(); }
                catch { missing.Append("PartMaster_ClientLookupId "); }

                try { reader["LongDescription"].ToString(); }
                catch { missing.Append("LongDescription "); }

                try { reader["ShortDescription"].ToString(); }
                catch { missing.Append("ShortDescription "); }

                try { reader["Catogery"].ToString(); }
                catch { missing.Append("Catogery "); }

                try { reader["DecodeCatogeryDescription"].ToString(); }
                catch { missing.Append("DecodeCatogeryDescription "); }
                // SOM-668 - End
                try { reader["VendorClientlookupId"].ToString(); }
                catch { missing.Append("VendorClientlookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["AutoPurchase"].ToString(); }
                catch { missing.Append("AutoPurchase "); }

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

        #region V2-670
        public void ValidateClientLookupIdV2(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Part_ValidateClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Part ProcessRetrieveForChunkMultipartStoreroomV2(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseForChunkRetrieveMultipartStoreroomV2(reader);
            return Part;
        }

        public int LoadFromDatabaseForChunkRetrieveMultipartStoreroomV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // StockType 
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;
                //AppliedCost
                if (false == reader.IsDBNull(i))
                {
                    AppliedCost = reader.GetDecimal(i);
                }
                else
                {
                    AppliedCost = 0;
                }
                i++;

                ChildCount = reader.GetInt32(i++);
                TotalCount = reader.GetInt32(i++);
                //Storeroom Name with Description
                if (false == reader.IsDBNull(i))
                {
                    DefStoreroom = reader.GetString(i);
                }
                else
                {
                    DefStoreroom = "";
                }
                //i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["DefStoreroom"].ToString(); }
                catch { missing.Append("DefStoreroom "); }

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

        public void MultiPartStoreroomRetrieveChunkSearchV2(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchForMultiPartStoreroomGrid_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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



        public void MultiStoreroomRetrieveByPartId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseByPartId_V2(reader); return this; });
                Database.StoredProcedure.usp_Part_RetrieveByPartId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void LoadFromDatabaseByPartId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);
                PartId = reader.GetInt64(i++);
                SiteId = reader.GetInt64(i++);
                AreaId = reader.GetInt64(i++);
                DepartmentId = reader.GetInt64(i++);
                StoreroomId = reader.GetInt64(i++);
                ClientLookupId = reader.GetString(i++);
                ABCCode = reader.GetString(i++);
                ABCStoreCost = reader.GetString(i++);
                AccountClientLookupId = reader.GetString(i++);
                AppliedCost = reader.GetDecimal(i++);
                AverageCost = reader.GetDecimal(i++);
                Consignment = reader.GetBoolean(i++);
                Critical = reader.GetBoolean(i++);
                Description = reader.GetString(i++);
                InactiveFlag = reader.GetBoolean(i++);
                IssueUnit = reader.GetString(i++);
                IssueUnitDescription = reader.GetString(i++);
                Manufacturer = reader.GetString(i++);
                ManufacturerId = reader.GetString(i++);
                MSDSContainerCode = reader.GetString(i++);
                MSDSPressureCode = reader.GetString(i++);
                MSDSReference = reader.GetString(i++);
                MSDSRequired = reader.GetBoolean(i++);
                MSDSTemperatureCode = reader.GetString(i++);
                NoEquipXref = reader.GetBoolean(i++);
                StockType = reader.GetString(i++);
                StockTypeDescription = reader.GetString(i++);
                UPCCode = reader.GetString(i++);
                Location1_1 = reader.GetString(i++);
                Location1_2 = reader.GetString(i++);
                Location1_3 = reader.GetString(i++);
                Location1_4 = reader.GetString(i++);
                Location1_5 = reader.GetString(i++);
                QtyMaximum = reader.GetDecimal(i++);
                QtyReorderLevel = reader.GetDecimal(i++);
                AccountId = reader.GetInt64(i++);
                TotalOnHand = reader.GetDecimal(i++);
                TotalOnRequest = reader.GetDecimal(i++);
                TotalOnOrder = reader.GetDecimal(i++);
                DefStoreroom = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

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

                try { reader["ABCCode"].ToString(); }
                catch { missing.Append("ABCCode "); }

                try { reader["ABCStoreCost"].ToString(); }
                catch { missing.Append("ABCStoreCost "); }

                try { reader["AccountClientLookupId"].ToString(); }
                catch { missing.Append("AccountClientLookupId "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

                try { reader["Consignment"].ToString(); }
                catch { missing.Append("Consignment "); }

                try { reader["Critical"].ToString(); }
                catch { missing.Append("Critical "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["MSDSContainerCode"].ToString(); }
                catch { missing.Append("MSDSContainerCode "); }

                try { reader["MSDSPressureCode"].ToString(); }
                catch { missing.Append("MSDSPressureCode "); }

                try { reader["MSDSReference"].ToString(); }
                catch { missing.Append("MSDSReference "); }

                try { reader["MSDSRequired"].ToString(); }
                catch { missing.Append("MSDSRequired "); }

                try { reader["MSDSTemperatureCode"].ToString(); }
                catch { missing.Append("MSDSTemperatureCode "); }

                try { reader["NoEquipXref"].ToString(); }
                catch { missing.Append("NoEquipXref "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode "); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1"); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2"); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3"); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4"); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5"); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum"); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel"); }

                try { reader["TotalOnHand"].ToString(); }
                catch { missing.Append("TotalOnHand"); }

                try { reader["TotalOnRequest"].ToString(); }
                catch { missing.Append("TotalOnRequest"); }

                try { reader["TotalOnOrder"].ToString(); }
                catch { missing.Append("TotalOnOrder"); }

                try { reader["DefaultStoreroom"].ToString(); }
                catch { missing.Append("DefaultStoreroom"); }


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

        #region V2-687 Part Chunk Search LookupList For MultiStoreroom
        public void RetrievePartIdByStoreroomIdAndClientLookupIdFromDatabase(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
string callerUserName,
                ref b_Part result
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

                result = Database.StoredProcedure.usp_Part_RetrieveByStoreroomIdAndClientLookUpId_V2.CallStoredProcedure(command, this);

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
        public void PartChunkSearchLookupListForMultiStoreroom_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchLookupListForMultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region V2-687 Part cycle count chunk search for Multistoreroom
        public static b_Part ProcessRetrieveForCycleCountChunkSearchForMultiStoreroom(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseForCycleCountChunkSearchForMultiStoreroom(reader);
            return Part;
        }

        public int LoadFromDatabaseForCycleCountChunkSearchForMultiStoreroom(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // QtyOnHand column, decimal(15,6), not null
                QtyOnHand = reader.GetDecimal(i++);


                if (false == reader.IsDBNull(i))
                {
                    Location1_5 = reader.GetString(i);
                }
                else
                {
                    Location1_5 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_1 = reader.GetString(i);
                }
                else
                {
                    Location1_1 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_2 = reader.GetString(i);
                }
                else
                {
                    Location1_2 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_3 = reader.GetString(i);
                }
                else
                {
                    Location1_3 = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Location1_4 = reader.GetString(i);
                }
                else
                {
                    Location1_4 = "";
                }
                i++;

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5 "); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1 "); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2 "); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3 "); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4 "); }

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
        public void PartRetrieveCycleCountChunkSearchForMultiStoreroom(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_CycleCountChunkSearchForMultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        #region V2-687 Retrieve By PartId  For MultiStoreroom

        public void RetrieveByPartIdForMultiStoreroom_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseExtendedForMultiStoreroom_V2(reader); return this; });
                Database.StoredProcedure.usp_PartsStorerooms_RetrieveByPartIdForMultiStoreroom_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseExtendedForMultiStoreroom_V2(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);

            try
            {
                PartStoreroomId = reader.GetInt64(i++);
                CountFrequency = reader.GetInt32(i++);
                // RKL - 2014-Jul-10
                // Last Counted can be null
                //LastCounted = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    LastCounted = reader.GetDateTime(i);
                }
                else
                {
                    LastCounted = DateTime.MinValue;
                }
                i++;
                Location1_1 = reader.GetString(i++);
                Location1_2 = reader.GetString(i++);
                Location1_3 = reader.GetString(i++);
                Location1_4 = reader.GetString(i++);
                Location1_5 = reader.GetString(i++);
                QtyMaximum = reader.GetDecimal(i++);
                QtyOnHand = reader.GetDecimal(i++);
                QtyReorderLevel = reader.GetDecimal(i++);
                ReorderMethod = reader.GetString(i++);
                Storeroom_UpdateIndex = reader.GetInt32(i++);
                QtyOnOrder = reader.GetDecimal(i++);
                QtyOnRequest = reader.GetDecimal(i++);
                QtyReserved = reader.GetDecimal(i++);
                // SOM-451 - Begin
                Account_ClientLookupId = reader.GetString(i++);
                AltPartId1_ClientLookupId = reader.GetString(i++);
                AltPartId2_ClientLookupId = reader.GetString(i++);
                AltPartId3_ClientLookupId = reader.GetString(i++);
                LastPurchaseCost = reader.GetDecimal(i++);
                // SOM-451 - End
                // V2-668 Begin
                if (false == reader.IsDBNull(i))
                {
                    PartMaster_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    PartMaster_ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    LongDescription = reader.GetString(i);
                }
                else
                {
                    LongDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ShortDescription = reader.GetString(i);
                }
                else
                {
                    ShortDescription = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Category = reader.GetString(i);
                }
                else
                {
                    Category = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CategoryDesc = reader.GetString(i);
                }
                else
                {
                    CategoryDesc = "";
                }
                i++;

                // V2-668 End
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId"); }

                try { reader["CountFrequency"].ToString(); }
                catch { missing.Append("CountFrequency"); }

                try { reader["LastCounted"].ToString(); }
                catch { missing.Append("LastCounted"); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1"); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2"); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3"); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4"); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5"); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum"); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel"); }

                try { reader["ReorderMethod"].ToString(); }
                catch { missing.Append("ReorderMethod"); }

                try { reader["Storeroom_UpdateIndex"].ToString(); }
                catch { missing.Append("Storeroom_UpdateIndex"); }

                try { reader["QtyOnOrder"].ToString(); }
                catch { missing.Append("QtyOnOrder "); }

                try { reader["QtyOnRequest"].ToString(); }
                catch { missing.Append("QtyOnRequest "); }

                try { reader["QtyReserved"].ToString(); }
                catch { missing.Append("QtyReserved "); }

                // SOM-451 - Begin
                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["AltPartId1_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId1_ClientLookupId "); }

                try { reader["AltPartId2_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId2_ClientLookupId "); }

                try { reader["AltPartId3_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId3_ClientLookupId "); }

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost "); }
                // SOM-451 - End

                // V2-668 - Begin
                try { reader["PartMaster_ClientLookupId"].ToString(); }
                catch { missing.Append("PartMaster_ClientLookupId "); }

                try { reader["LongDescription"].ToString(); }
                catch { missing.Append("LongDescription "); }

                try { reader["ShortDescription"].ToString(); }
                catch { missing.Append("ShortDescription "); }

                try { reader["Catogery"].ToString(); }
                catch { missing.Append("Catogery "); }

                try { reader["DecodeCatogeryDescription"].ToString(); }
                catch { missing.Append("DecodeCatogeryDescription "); }
                // SOM-668 - End

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

        #region V2-732

        public void SearchForCart_VendorCatalogWOMultiStoreroom(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCart_VendorCatalogWO_MultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_Part ProcessRowSearchForCart_VendorCatalogWOMultiStoreroom(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCart_VendorCatalogWOMultiStoreroom(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseSearchForCart_VendorCatalogWOMultiStoreroom(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // IndexId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // PMClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PMClientlookupId = reader.GetString(i);
                }
                else
                {
                    PMClientlookupId = "";
                }
                i++;
                // PMDescription column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PMDescription = reader.GetString(i);
                }
                else
                {
                    PMDescription = "";
                }
                i++;
                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;
                // ManufacturerId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;
                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // VendorName column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                // VendorClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientlookupId = "";
                }
                i++;

                // VendorMasterId column, bigint, not null
                VendorMasterId = reader.GetInt64(i++);
                // VendorCatalogItemId column, bigint, not null
                VendorCatalogItemId = reader.GetInt64(i++);
                // VendorCatalogId column, bigint, not null
                VendorCatalogId = reader.GetInt64(i++);
                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);
                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // IssueUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUOM = reader.GetString(i);
                }
                else
                {
                    IssueUOM = "";
                }
                i++;

                // UnitCost column, decimal, not null
                UnitCost = reader.GetDecimal(i++);

                // VendorPartNumber column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorPartNumber = reader.GetString(i);
                }
                else
                {
                    VendorPartNumber = "";
                }
                i++;

                // VendorLeadTime column, int, not null
                VendorLeadTime = reader.GetInt32(i++);

                // MinimumOrderQuantity column, int, not null
                MinimumOrderQuantity = reader.GetInt32(i++);

                // PartInactiveFlag column, bool, not null
                PartInactiveFlag = reader.GetBoolean(i++);


                // CategoryDescription column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CategoryDescription = reader.GetString(i);
                }
                else
                {
                    CategoryDescription = "";
                }
                i++;

                // AttachmentUrl column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    AttachmentUrl = reader.GetString(i);
                }
                else
                {
                    AttachmentUrl = "";
                }
                i++;

                // CatalogType column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    CatalogType = reader.GetString(i);
                }
                else
                {
                    CatalogType = "";
                }
                i++;

                //V2-424
                if (false == reader.IsDBNull(i))
                {
                    QtyOnHand = reader.GetDecimal(i);
                }
                else
                {
                    QtyOnHand = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyMaximum = reader.GetDecimal(i);
                }
                else
                {
                    QtyMaximum = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyReorderLevel = reader.GetDecimal(i);
                }
                else
                {
                    QtyReorderLevel = 0;
                }
                i++;
                //V2-424
                //V2-553
                if (false == reader.IsDBNull(i))
                {
                    UOMConversion = reader.GetDecimal(i);
                }
                else
                {
                    UOMConversion = 0;
                }
                i++;

                UOMConvRequired = reader.GetBoolean(i++);

                VC_Count = reader.GetInt32(i++);

                //IssueUnit
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;
                // PartCategoryMasterId column, long, not null
                PartCategoryMasterId = reader.GetInt64(i++);

                // StoreroomId column, long, not null
                StoreroomId = reader.GetInt64(i++);

                // PartStoreroomId column, long, not null
                PartStoreroomId = reader.GetInt64(i++);

                // AccountId 
                AccountId = reader.GetInt64(i++); //V21068

                // TotalCount column, int, not null
                TotalCount = reader.GetInt32(i++);



            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["PMClientlookupId"].ToString(); }
                catch { missing.Append("PMClientlookupId "); }

                try { reader["PMDescription"].ToString(); }
                catch { missing.Append("PMDescription "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorMasterId"].ToString(); }
                catch { missing.Append("VendorMasterId "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VendorCatalogItemId "); }

                try { reader["VendorCatalogId"].ToString(); }
                catch { missing.Append("VendorCatalogId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["IssueUOM"].ToString(); }
                catch { missing.Append("IssueUOM "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["VendorPartNumber"].ToString(); }
                catch { missing.Append("VendorPartNumber "); }

                try { reader["VendorLeadTime"].ToString(); }
                catch { missing.Append("VendorLeadTime "); }

                try { reader["MinimumOrderQuantity"].ToString(); }
                catch { missing.Append("MinimumOrderQuantity "); }

                try { reader["PartInactiveFlag"].ToString(); }
                catch { missing.Append("PartInactiveFlag "); }

                try { reader["CategoryDescription"].ToString(); }
                catch { missing.Append("CategoryDescription "); }

                try { reader["AttachmentUrl"].ToString(); }
                catch { missing.Append("AttachmentUrl "); }

                try { reader["CatalogType"].ToString(); }
                catch { missing.Append("CatalogType "); }

                //V2-424
                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }
                //V2-424

                try { reader["UOMConversion"].ToString(); }
                catch { missing.Append("UOMConversion "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["VC_Count"].ToString(); }
                catch { missing.Append("VC_Count "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["PartCategoryMasterId"].ToString(); }
                catch { missing.Append("PartCategoryMasterId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId "); }
                //V2-1068
                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

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

        public void SearchForCartWOMultiStoreroom(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCartWO_MultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Part ProcessRowSearchForCartWOMultiStoreroom(SqlDataReader reader)
        {
            // Create instance of object
            b_Part parts = new b_Part();

            // Load the object from the database
            parts.LoadFromDatabaseSearchForCartWOMultiStoreroom(reader);

            // Return result
            return parts;
        }
        public void LoadFromDatabaseSearchForCartWOMultiStoreroom(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // IndexId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    IndexId = reader.GetInt64(i);
                }
                else
                {
                    IndexId = 0;
                }
                i++;

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                // StockType column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    StockType = reader.GetString(i);
                }
                else
                {
                    StockType = "";
                }
                i++;

                // Manufacturer column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    Manufacturer = reader.GetString(i);
                }
                else
                {
                    Manufacturer = "";
                }
                i++;

                // ManufacturerId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    ManufacturerId = reader.GetString(i);
                }
                else
                {
                    ManufacturerId = "";
                }
                i++;

                // PurchaseUOM column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;

                // IssueUnit column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    IssueUnit = reader.GetString(i);
                }
                else
                {
                    IssueUnit = "";
                }
                i++;

                // AppliedCost column, bigint, not null
                AppliedCost = reader.GetDecimal(i++);

                // AverageCost column, bigint, not null
                AverageCost = reader.GetDecimal(i++);

                // QtyOnHand column, bigint, not null
                QtyOnHand = reader.GetDecimal(i++);

                //InactiveFlag, boolean
                InactiveFlag = reader.GetBoolean(i++);

                // AttachmentUrl column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    AttachmentUrl = reader.GetString(i);
                }
                else
                {
                    AttachmentUrl = "";
                }
                i++;
                //V2-424            
                if (false == reader.IsDBNull(i))
                {
                    QtyMaximum = reader.GetDecimal(i);
                }
                else
                {
                    QtyMaximum = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    QtyReorderLevel = reader.GetDecimal(i);
                }
                else
                {
                    QtyReorderLevel = 0;
                }
                i++;
                //V2-424

                if (false == reader.IsDBNull(i))
                {
                    IssueOrder = reader.GetDecimal(i);
                }
                else
                {
                    IssueOrder = 1.0m;
                }
                i++;

                UOMConvRequired = reader.GetBoolean(i++);

                // VendorId column, bigint, not null
                VendorId = reader.GetInt64(i++);

                // VendorClientlookupId column, nvarchar(31), not null
                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientlookupId = "";
                }
                i++;

                // StoreroomId column, long, not null
                StoreroomId = reader.GetInt64(i++);

                // PartStoreroomId column, long, not null
                PartStoreroomId = reader.GetInt64(i++);

                //Account Id 1068
                AccountId = reader.GetInt64(i++);

                // TotalCount column, bigint, not null
                TotalCount = reader.GetInt32(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["IndexId"].ToString(); }
                catch { missing.Append("IndexId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["AttachmentUrl"].ToString(); }
                catch { missing.Append("AttachmentUrl "); }

                //V2-424              
                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel "); }
                //V2-424

                try { reader["IssueOrder"].ToString(); }
                catch { missing.Append("IssueOrder "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["UOMConvRequired"].ToString(); }
                catch { missing.Append("UOMConvRequired "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["VendorClientlookupId"].ToString(); }
                catch { missing.Append("VendorClientlookupId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId "); }
                //V2-1068
                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

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

        #region V2-738
        public void SearchForCartMultiStoreroom_V2(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCartForMultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void SearchForCart_VendorCatalogForMultiStoreroom_V2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Part> results
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

                results = Database.StoredProcedure.usp_Part_SearchForCart_VendorCatalogForMultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region V2-736
        public void PartChunkSearchLookupListForMultiStoreroomMobile_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchLookupListForMultiStoreroomMobile_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        
        public void PartChunkSearchLookupListMobileV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchLookupListMobile_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_Part ProcessRetrieveForChunkSearchLookupListMobileV2(SqlDataReader reader)
        {
            b_Part Part = new b_Part();

            Part.LoadFromDatabaseForChunkSearchLookupListMobile_V2(reader);
            return Part;
        }
        public int LoadFromDatabaseForChunkSearchLookupListMobile_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                SiteId = reader.GetInt64(i++);

                //  ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                UpdateIndex = reader.GetInt32(i++);

                TotalCount = reader.GetInt32(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

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
        #region Chnage Part Clientlookup 
        public void ChangeClientLookupId_V2(
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
                StoredProcedure.usp_Part_ChangeClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region V2-1045
        public void RetrievePartIdByClientLookupIdForFindPartFromDatabase(
                 SqlConnection connection,
                 SqlTransaction transaction,
                 long callerUserInfoId,
string callerUserName,
                 ref b_Part result
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

                result = Database.StoredProcedure.usp_Part_RetrieveByClientLookUpIdForFindPart_V2.CallStoredProcedure(command, this);

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
        #endregion

        #region RKL-MAIL-Label Printing from Receipts
        public void RetrieveByPartIdAndPartStoreroomId_V2(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Part> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Part>(reader => { this.LoadFromDatabaseExtendedForPartIdAndPartStoreroomId_V2(reader); return this; });
                Database.StoredProcedure.usp_PartStoreroom_RetrieveByPartIdAndPartStoreroomId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
       
        public void LoadFromDatabaseExtendedForPartIdAndPartStoreroomId_V2(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PartId column, bigint, not null
                PartId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(70), not null
                ClientLookupId = reader.GetString(i++);

                // ABCCode column, nvarchar(7), not null
                ABCCode = reader.GetString(i++);

                // ABCStoreCost column, nvarchar(7), not null
                ABCStoreCost = reader.GetString(i++);

                // AccountId column, bigint, not null
                AccountId = reader.GetInt64(i++);

                // AltPartId1 column, bigint, not null
                AltPartId1 = reader.GetInt64(i++);

                // AltPartId2 column, bigint, not null
                AltPartId2 = reader.GetInt64(i++);

                // AltPartId3 column, bigint, not null
                AltPartId3 = reader.GetInt64(i++);

                // AppliedCost column, decimal(15,5), not null
                AppliedCost = reader.GetDecimal(i++);

                // AverageCost column, decimal(15,5), not null
                AverageCost = reader.GetDecimal(i++);

                // Consignment column, bit, not null
                Consignment = reader.GetBoolean(i++);

                // CostCalcMethod column, nvarchar(15), not null
                CostCalcMethod = reader.GetString(i++);

                // CostMultiplier column, decimal(6,2), not null
                CostMultiplier = reader.GetDecimal(i++);

                // Critical column, bit, not null
                Critical = reader.GetBoolean(i++);

                // Description column, nvarchar(127), not null
                Description = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

                // IssueUnit column, nvarchar(15), not null
                IssueUnit = reader.GetString(i++);

                // Manufacturer column, nvarchar(31), not null
                Manufacturer = reader.GetString(i++);

                // ManufacturerId column, nvarchar(63), not null
                ManufacturerId = reader.GetString(i++);

                // MSDSContainerCode column, nvarchar(7), not null
                MSDSContainerCode = reader.GetString(i++);

                // MSDSPressureCode column, nvarchar(7), not null
                MSDSPressureCode = reader.GetString(i++);

                // MSDSReference column, nvarchar(31), not null
                MSDSReference = reader.GetString(i++);

                // MSDSRequired column, bit, not null
                MSDSRequired = reader.GetBoolean(i++);

                // MSDSTemperatureCode column, nvarchar(7), not null
                MSDSTemperatureCode = reader.GetString(i++);

                // NoEquipXref column, bit, not null
                NoEquipXref = reader.GetBoolean(i++);

                // PrintNoLabel column, bit, not null
                PrintNoLabel = reader.GetBoolean(i++);

                // PurchaseText column, nvarchar(MAX), not null
                PurchaseText = reader.GetString(i++);

                // RepairablePart column, bit, not null
                RepairablePart = reader.GetBoolean(i++);

                // StockType column, nvarchar(15), not null
                StockType = reader.GetString(i++);

                // TaxLevel1 column, decimal(7,3), not null
                TaxLevel1 = reader.GetDecimal(i++);

                // TaxLevel2 column, decimal(7,3), not null
                TaxLevel2 = reader.GetDecimal(i++);

                // Taxable column, bit, not null
                Taxable = reader.GetBoolean(i++);

                // Tool column, bit, not null
                Tool = reader.GetBoolean(i++);

                // Type column, int, not null
                Type = reader.GetInt32(i++);

                // UPCCode column, nvarchar(31), not null
                UPCCode = reader.GetString(i++);

                // UseCostMultiplier column, bit, not null
                UseCostMultiplier = reader.GetBoolean(i++);

                // Chemical column, bit, not null
                Chemical = reader.GetBoolean(i++);

                // AutoPurch column, bit, not null
                AutoPurch = reader.GetBoolean(i++);

                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);

                // PrevClientLookupId column, nvarchar(70), not null
                PrevClientLookupId = reader.GetString(i++);

                // DefaultStoreroom column, bigint, not null
                DefaultStoreroom = reader.GetInt64(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                PartStoreroomId = reader.GetInt64(i++);
                CountFrequency = reader.GetInt32(i++);
              
                // Last Counted can be null
                //LastCounted = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    LastCounted = reader.GetDateTime(i);
                }
                else
                {
                    LastCounted = DateTime.MinValue;
                }
                i++;
                Location1_1 = reader.GetString(i++);
                Location1_2 = reader.GetString(i++);
                Location1_3 = reader.GetString(i++);
                Location1_4 = reader.GetString(i++);
                Location1_5 = reader.GetString(i++);
                QtyMaximum = reader.GetDecimal(i++);
                QtyOnHand = reader.GetDecimal(i++);
                QtyReorderLevel = reader.GetDecimal(i++);
                ReorderMethod = reader.GetString(i++);
                Storeroom_UpdateIndex = reader.GetInt32(i++);
                QtyOnOrder = reader.GetDecimal(i++);
                QtyOnRequest = reader.GetDecimal(i++);
                QtyReserved = reader.GetDecimal(i++);
                Account_ClientLookupId = reader.GetString(i++);
                AltPartId1_ClientLookupId = reader.GetString(i++);
                AltPartId2_ClientLookupId = reader.GetString(i++);
                AltPartId3_ClientLookupId = reader.GetString(i++);
                LastPurchaseCost = reader.GetDecimal(i++);
               

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

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

                try { reader["ABCCode"].ToString(); }
                catch { missing.Append("ABCCode "); }

                try { reader["ABCStoreCost"].ToString(); }
                catch { missing.Append("ABCStoreCost "); }

                try { reader["AccountId"].ToString(); }
                catch { missing.Append("AccountId "); }

                try { reader["AltPartId1"].ToString(); }
                catch { missing.Append("AltPartId1 "); }

                try { reader["AltPartId2"].ToString(); }
                catch { missing.Append("AltPartId2 "); }

                try { reader["AltPartId3"].ToString(); }
                catch { missing.Append("AltPartId3 "); }

                try { reader["AppliedCost"].ToString(); }
                catch { missing.Append("AppliedCost "); }

                try { reader["AverageCost"].ToString(); }
                catch { missing.Append("AverageCost "); }

                try { reader["Consignment"].ToString(); }
                catch { missing.Append("Consignment "); }

                try { reader["CostCalcMethod"].ToString(); }
                catch { missing.Append("CostCalcMethod "); }

                try { reader["CostMultiplier"].ToString(); }
                catch { missing.Append("CostMultiplier "); }

                try { reader["Critical"].ToString(); }
                catch { missing.Append("Critical "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["IssueUnit"].ToString(); }
                catch { missing.Append("IssueUnit "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["MSDSContainerCode"].ToString(); }
                catch { missing.Append("MSDSContainerCode "); }

                try { reader["MSDSPressureCode"].ToString(); }
                catch { missing.Append("MSDSPressureCode "); }

                try { reader["MSDSReference"].ToString(); }
                catch { missing.Append("MSDSReference "); }

                try { reader["MSDSRequired"].ToString(); }
                catch { missing.Append("MSDSRequired "); }

                try { reader["MSDSTemperatureCode"].ToString(); }
                catch { missing.Append("MSDSTemperatureCode "); }

                try { reader["NoEquipXref"].ToString(); }
                catch { missing.Append("NoEquipXref "); }

                try { reader["PrintNoLabel"].ToString(); }
                catch { missing.Append("PrintNoLabel "); }

                try { reader["PurchaseText"].ToString(); }
                catch { missing.Append("PurchaseText "); }

                try { reader["RepairablePart"].ToString(); }
                catch { missing.Append("RepairablePart "); }

                try { reader["StockType"].ToString(); }
                catch { missing.Append("StockType "); }

                try { reader["TaxLevel1"].ToString(); }
                catch { missing.Append("TaxLevel1 "); }

                try { reader["TaxLevel2"].ToString(); }
                catch { missing.Append("TaxLevel2 "); }

                try { reader["Taxable"].ToString(); }
                catch { missing.Append("Taxable "); }

                try { reader["Tool"].ToString(); }
                catch { missing.Append("Tool "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["UPCCode"].ToString(); }
                catch { missing.Append("UPCCode "); }

                try { reader["UseCostMultiplier"].ToString(); }
                catch { missing.Append("UseCostMultiplier "); }

                try { reader["Chemical"].ToString(); }
                catch { missing.Append("Chemical "); }

                try { reader["AutoPurch"].ToString(); }
                catch { missing.Append("AutoPurch "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["PrevClientLookupId"].ToString(); }
                catch { missing.Append("PrevClientLookupId "); }

                try { reader["DefaultStoreroom"].ToString(); }
                catch { missing.Append("DefaultStoreroom "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["PartStoreroomId"].ToString(); }
                catch { missing.Append("PartStoreroomId"); }

                try { reader["CountFrequency"].ToString(); }
                catch { missing.Append("CountFrequency"); }

                try { reader["LastCounted"].ToString(); }
                catch { missing.Append("LastCounted"); }

                try { reader["Location1_1"].ToString(); }
                catch { missing.Append("Location1_1"); }

                try { reader["Location1_2"].ToString(); }
                catch { missing.Append("Location1_2"); }

                try { reader["Location1_3"].ToString(); }
                catch { missing.Append("Location1_3"); }

                try { reader["Location1_4"].ToString(); }
                catch { missing.Append("Location1_4"); }

                try { reader["Location1_5"].ToString(); }
                catch { missing.Append("Location1_5"); }

                try { reader["QtyMaximum"].ToString(); }
                catch { missing.Append("QtyMaximum"); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                try { reader["QtyReorderLevel"].ToString(); }
                catch { missing.Append("QtyReorderLevel"); }

                try { reader["ReorderMethod"].ToString(); }
                catch { missing.Append("ReorderMethod"); }

                try { reader["Storeroom_UpdateIndex"].ToString(); }
                catch { missing.Append("Storeroom_UpdateIndex"); }

                try { reader["QtyOnOrder"].ToString(); }
                catch { missing.Append("QtyOnOrder "); }

                try { reader["QtyOnRequest"].ToString(); }
                catch { missing.Append("QtyOnRequest "); }

                try { reader["QtyReserved"].ToString(); }
                catch { missing.Append("QtyReserved "); }

              
                try { reader["Account_ClientLookupId"].ToString(); }
                catch { missing.Append("Account_ClientLookupId "); }

                try { reader["AltPartId1_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId1_ClientLookupId "); }

                try { reader["AltPartId2_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId2_ClientLookupId "); }

                try { reader["AltPartId3_ClientLookupId"].ToString(); }
                catch { missing.Append("AltPartId3_ClientLookupId "); }

                try { reader["LastPurchaseCost"].ToString(); }
                catch { missing.Append("LastPurchaseCost "); }
            


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
        #region V2-1167
        public void PartChunkSearchLookupListForSingleStockLineItemV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchLookupListForSingleStockLineItem_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void PartChunkSearchLookupListForForSingleStockLineMultiStoreroom_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_Part results
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

                results = Database.StoredProcedure.usp_Part_RetrieveChunkSearchLookupListForSingleStockLineItemMultiStoreroom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

    }


}
