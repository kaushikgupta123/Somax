using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_PartMaster
    {
        public long ShoppingCartKey { get; set; }
        public string Mode { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Createby { get; set; }
        public DateTime CreateDate { get; set; }
        public string CM_Description { get; set; }
        public decimal VI_UnitCost { get; set; }
        public string VI_PurchaseUOM { get; set; }
        public Int64 VI_VendorCatalogId { get; set; }
        public Int64 VI_VendorCatalogItemId { get; set; }
        public string VCI_PartNumber { get; set; }
        public string SearchCriteria { get; set; }
        public string CategoryDescription { get; set; }
        public long PartId { get; set; }
        public string PartClientLookupId { get; set; }
        public long VendorCatalogItemId { get; set; }
        public string PartNumber { get; set; }
        public string CategoryMasterDescription { get; set; }
        public string VendorCatalogDescription { get; set; }
        public string VendorName { get; set; }
        public decimal VUnitCost { get; set; }
        public string PurchaseUOM { get; set; }
        public long VendorId { get; set; }
        public string PartDescription { get; set; }
        public bool CheckFlag { get; set; }
        public string CatalogType { get; set; }
        public decimal PartPrice { get; set; }
        public string Description { get; set; }
        public string VendorClientLookupId { get; set; }
        public decimal QtyOnHand { get; set; }

        public int pageNumber { get; set; }
        public int resultsPerPage { get; set; }
        public string orderColumn { get; set; }
        public string orderDirection { get; set; }
        public int ResultCount { get; set; }
        public Int64 SiteId { get; set; }
        public string RequestType { get; set; }

        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {


                if (false == reader.IsDBNull(i))
                {
                    Createby = reader.GetString(i);
                }
                else
                {
                    Createby = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.Now;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.Now;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }

                try { reader["CompleteBy_PersonnelName"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelName "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["Created"].ToString(); }
                catch { missing.Append("Created "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        //som-1576
        public static b_PartMaster ProcessRowForLookUp(SqlDataReader reader)
        { // Create instance of object
            b_PartMaster obj = new b_PartMaster();

            // Load the object from the database
            obj.LoadFromDatabaseForLookUp(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForLookUp(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                //Description column, nvarchar(63), not null
                Description = reader.GetString(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                
                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

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
            return i;
        }

        public static b_PartMaster ProcessRowForLookUp2(SqlDataReader reader)
        { // Create instance of object
            b_PartMaster obj = new b_PartMaster();

            // Load the object from the database
            obj.LoadFromDatabaseForLookUp2(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForLookUp2(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                //Description column, nvarchar(63), not null
                LongDescription = reader.GetString(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["LongDescription"].ToString(); }
                catch { missing.Append("LongDescription "); }


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

        //=============
        public static b_PartMaster ProcessRowForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_PartMaster obj = new b_PartMaster();

            // Load the object from the database
            obj.LoadFromDatabaseForCriteriaSearch(reader);

            // Return result
            return obj;
        }

        //SOM-1497
        public void PartMasterReview_RetrieveBySearchCriteria(
SqlConnection connection,
SqlTransaction transaction,
string SearchCriterion,
long Siteid,
ref b_PartMaster[] data
)
        {
            Database.SqlClient.ProcessRow<b_PartMaster> processRow = null;
            SqlCommand command = null;
            ArrayList results = null;
            string message = String.Empty;
            data = new b_PartMaster[0];
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMaster>(reader => { b_PartMaster obj = new b_PartMaster(); obj.LoadForPartMasterReview(reader, this); return obj; });
                results = Database.StoredProcedure.usp_PartMaster_PartMasterReview.CallStoredProcedure(command, processRow, SearchCriterion, Siteid, this);
                if (null != results)
                {
                    data = (b_PartMaster[])results.ToArray(typeof(b_PartMaster));
                }
                else
                {
                    data = new b_PartMaster[0];
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
                message = String.Empty;
                Siteid = 0;
                SearchCriterion = String.Empty;
            }
        }


        public void PartMaster_RetrieveByImageURL(
SqlConnection connection,
SqlTransaction transaction,
ref b_PartMaster[] data
)
        {
            Database.SqlClient.ProcessRow<b_PartMaster> processRow = null;
            SqlCommand command = null;
            ArrayList results = null;
            string message = String.Empty;
            data = new b_PartMaster[0];
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMaster>(reader => { b_PartMaster obj = new b_PartMaster(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_PartMaster_RetrieveByImageURL.CallStoredProcedure(command, processRow, this);
                if (null != results)
                {
                    data = (b_PartMaster[])results.ToArray(typeof(b_PartMaster));
                }
                else
                {
                    data = new b_PartMaster[0];
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
                message = String.Empty;
            }
        }


        public void LoadForPartMasterReview(SqlDataReader reader, b_PartMaster obj)
        {
            int i = 0;// this.LoadFromDatabase(reader);
            try
            {

                ClientId = reader.GetInt64(i++);
                PartMasterId = reader.GetInt64(i++);
                ClientLookupId = reader.GetString(i++);
                LongDescription = reader.GetString(i++);
                ShortDescription = reader.GetString(i++);
                Category = reader.GetString(i++);
                Manufacturer = reader.GetString(i++);
                ManufacturerId = reader.GetString(i++);
                CategoryMasterDescription = reader.GetString(i++);
                ImageURL = reader.GetString(i++); //V2-1215
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["LongDescription"].ToString(); }
                catch { missing.Append("LongDescription "); }

                try { reader["ShortDescription"].ToString(); }
                catch { missing.Append("ShortDescription "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["CategoryMasterDescription"].ToString(); }
                catch { missing.Append("CategoryMasterDescription "); }
                
                try { reader["ImageURL"].ToString(); }
                catch { missing.Append("ImageURL "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public int LoadFromDatabaseForCriteriaSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // ShortDescription column, nvarchar(63), not null
                ShortDescription = reader.GetString(i++);

                // InactiveFlag column, bit, not null
                InactiveFlag = reader.GetBoolean(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ShortDescription"].ToString(); }
                catch { missing.Append("ShortDescription "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }



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
        public void RetrieveByForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_PartMaster> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMaster>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_PartMaster_RetrieveByFK.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void PartMaster_RetrieveAll_ByInactiveFlag(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref b_PartMaster[] data,
        bool InactiveFlag
    )
        {
            Database.SqlClient.ProcessRow<b_PartMaster> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_PartMaster[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMaster>(reader => { b_PartMaster obj = new b_PartMaster(); obj.LoadFromDatabaseForSearch(reader); return obj; });
                results = Database.StoredProcedure.usp_PartMaster_RetrieveAll_ByInactiveFlag.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this, InactiveFlag);

                // Extract the results
                if (null != results)
                {
                    data = (b_PartMaster[])results.ToArray(typeof(b_PartMaster));
                }
                else
                {
                    data = new b_PartMaster[0];
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
        public void ValidateForChangeClientLookupId(SqlConnection connection,
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
                results = Database.StoredProcedure.usp_PartMaster_ValidationChangeClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ShoppingCart_RetrieveBySearchCriteria(
    SqlConnection connection,
    SqlTransaction transaction,
    string SearchCriterion,
    long Siteid,
     ref b_PartMaster[] data
)
        {
            Database.SqlClient.ProcessRow<b_PartMaster> processRow = null;
            SqlCommand command = null;
            ArrayList results = null;
            string message = String.Empty;
            data = new b_PartMaster[0];
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandTimeout = 180;         // SOM-1501

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMaster>(reader => { b_PartMaster obj = new b_PartMaster(); obj.LoadForShoppingCart(reader, this); return obj; });
                results = Database.StoredProcedure.usp_ShoppingCart_RetrieveBySearchCriteria.CallStoredProcedure(command, processRow, SearchCriterion, Siteid, this);
                if (null != results)
                {
                    data = (b_PartMaster[])results.ToArray(typeof(b_PartMaster));
                }
                else
                {
                    data = new b_PartMaster[0];
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
                message = String.Empty;
                Siteid = 0;
                SearchCriterion = String.Empty;
            }
        }
        public void LoadForShoppingCart(SqlDataReader reader, b_PartMaster obj)
        {
            int i = 0;// this.LoadFromDatabase(reader);
            try
            {
                ShoppingCartKey = reader.GetInt64(i++);
                ClientId = reader.GetInt64(i++);
                PartMasterId = reader.GetInt64(i++);
                ClientLookupId = reader.GetString(i++);
                LongDescription = reader.GetString(i++);
                ShortDescription = reader.GetString(i++);
                Category = reader.GetString(i++);
                Manufacturer = reader.GetString(i++);
                ManufacturerId = reader.GetString(i++);
                PartId = reader.GetInt64(i++);
                PartClientLookupId = reader.GetString(i++);
                VendorCatalogItemId = reader.GetInt64(i++);
                PartNumber = reader.GetString(i++);
                CategoryMasterDescription = reader.GetString(i++);
                ImageURL = reader.GetString(i++);
                VendorCatalogDescription = reader.GetString(i++);
                VendorName = reader.GetString(i++);
                VUnitCost = reader.GetDecimal(i++);
                PurchaseUOM = reader.GetString(i++);
                VendorId = reader.GetInt64(i++);
                PartDescription = reader.GetString(i++);
                CatalogType = reader.GetString(i++);
                PartPrice = reader.GetDecimal(i++);
                QtyOnHand = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ShoppingCartKey"].ToString(); }
                catch { missing.Append("ShoppingCartKey "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["LongDescription"].ToString(); }
                catch { missing.Append("LongDescription "); }

                try { reader["ShortDescription"].ToString(); }
                catch { missing.Append("ShortDescription "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["PartId"].ToString(); }
                catch { missing.Append("PartId "); }

                try { reader["PartClientLookupId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("PartClientLookupId "); }

                try { reader["PartNumber"].ToString(); }
                catch { missing.Append("PartNumber "); }

                try { reader["CategoryMasterDescription"].ToString(); }
                catch { missing.Append("CategoryMasterDescription "); }

                try { reader["ImageURL"].ToString(); }
                catch { missing.Append("ImageURL "); }

                try { reader["VendorCatalogDescription"].ToString(); }
                catch { missing.Append("VendorCatalogDescription "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VUnitCost"].ToString(); }
                catch { missing.Append("VUnitCost "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["PartDescription"].ToString(); }
                catch { missing.Append("PartDescription "); }

                try { reader["CatalogType"].ToString(); }
                catch { missing.Append("CatalogType "); }

                try { reader["PartPrice"].ToString(); }
                catch { missing.Append("PartPrice "); }

                try { reader["QtyOnHand"].ToString(); }
                catch { missing.Append("QtyOnHand "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        public void ValidateByClientLookupId(SqlConnection connection,
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
                results = Database.StoredProcedure.usp_PartMaster_ValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void ChangeClientLookupId(
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
                Database.StoredProcedure.usp_PartMaster_ChangeClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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




        ////////////////////////////////////////////////////////////////////////
        public void RetrieveVendorCatalogBySearchCriteria(
       SqlConnection connection,
       SqlTransaction transaction,
       ref List<b_PartMaster> results
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

                results = Database.StoredProcedure.usp_VendorCatalog_RetrieveBySearchCriteria.CallStoredProcedure(command, this);

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
        public static b_PartMaster ProcessRowForSearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_PartMaster pm = new b_PartMaster();

            // Load the object from the database
            pm.LoadFromDatabaseForSearchCriteria(reader);

            // Return result
            return pm;
        }
        public int LoadFromDatabaseForSearchCriteria(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // LongDescription column, nvarchar(255), not null
                LongDescription = reader.GetString(i++);

                // Category column, nvarchar(31), not null
                Category = reader.GetString(i++);

                // Manufacturer column, nvarchar(31), not null
                Manufacturer = reader.GetString(i++);

                // ManufacturerId column, nvarchar(63), not null
                ManufacturerId = reader.GetString(i++);

                // CM_Description column, nvarchar(255), not null
                CM_Description = reader.GetString(i++);

                // VI_UnitCost column, bigint not null
                VI_UnitCost = reader.GetDecimal(i++);

                // VI_PurchaseUOM column, nvarchar(31), not null
                VI_PurchaseUOM = reader.GetString(i++);

                // VI_VendorCatalogId column, bigint not null
                VI_VendorCatalogId = reader.GetInt64(i++);

                // VI_VendorCatalogItemId column, bigint not null
                VI_VendorCatalogItemId = reader.GetInt64(i++);

                // VCI_PartNumber column, nvarchar(63), not null
                VCI_PartNumber = reader.GetString(i++);

                // Vendor Name, nvarchar(63), not null
                VendorName = reader.GetString(i++);

                // Vendor ClientLookupId, nvarchar(31), not null
                VendorClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["PartMasterId"].ToString(); }
                catch { missing.Append("PartMasterId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["LongDescription"].ToString(); }
                catch { missing.Append("LongDescription "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["Manufacturer"].ToString(); }
                catch { missing.Append("Manufacturer "); }

                try { reader["ManufacturerId"].ToString(); }
                catch { missing.Append("ManufacturerId "); }

                try { reader["CM_Description"].ToString(); }
                catch { missing.Append("CM_Description "); }

                try { reader["VI_UnitCost"].ToString(); }
                catch { missing.Append("VI_UnitCost "); }

                try { reader["VI_PurchaseUOM"].ToString(); }
                catch { missing.Append("VI_PurchaseUOM "); }

                try { reader["VI_VendorCatalogId"].ToString(); }
                catch { missing.Append("VI_VendorCatalogId "); }

                try { reader["VI_VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VI_VendorCatalogItemId "); }

                try { reader["VCI_PartNumber"].ToString(); }
                catch { missing.Append("VCI_PartNumber "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["VendorClientLookupId"].ToString(); }
                catch { missing.Append("VendorClientLookupId "); }

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

        public int LoadFromDatabaseForSearch(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            try
            {
                // CategoryDescription column, nvarchar(31), not null
                CategoryDescription = reader.GetString(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CategoryDescription"].ToString(); }
                catch { missing.Append("CategoryDescription "); }

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
        public void PartMaster_Retrieve_ByPart(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName
     )
        {
            Database.SqlClient.ProcessRow<b_PartMaster> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_PartMaster>(reader => { this.LoadFromDatabaseByPart(reader); return this; });
                Database.StoredProcedure.usp_PartMaster_RetrieveByPartTable.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public void LoadFromDatabaseByPart(SqlDataReader reader)
        {
            int i = 0;//this.LoadFromDatabase(reader);
            try
            {
                ClientId = reader.GetInt64(i++);

                // PartMasterId column, bigint, not null
                PartMasterId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // OEMPart column, bit, not null
                OEMPart = reader.GetBoolean(i++);
                EXAltPartId1 = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    ExUniqueId = reader.GetGuid(i);
                }
                else
                {
                    ExUniqueId = Guid.Empty;
                }
                i++;

                InactiveFlag = reader.GetBoolean(i++);

                // LongDescription column, nvarchar(255), not null
                LongDescription = reader.GetString(i++);

                // Manufacturer column, nvarchar(31), not null
                Manufacturer = reader.GetString(i++);

                // ManufacturerId column, nvarchar(63), not null
                ManufacturerId = reader.GetString(i++);

                // ShortDescription column, nvarchar(255), not null
                ShortDescription = reader.GetString(i++);

                // UnitCost column, decimal(15,5), not null
                UnitCost = reader.GetDecimal(i++);
                UnitOfMeasure = reader.GetString(i++);

                // Category column, nvarchar(31), not null
                Category = reader.GetString(i++);

                // UPCCode column, nvarchar(15), not null
                UPCCode = reader.GetString(i++);

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    Createby = reader.GetString(i);
                }
                else
                {
                    Createby = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.Now;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.Now;
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


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }
            }

        }


    }
}
