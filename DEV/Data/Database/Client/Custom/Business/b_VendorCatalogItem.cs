using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_VendorCatalogItem
    {
        public string VendorName { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public int TotalCount { get; set; }
        public void RetrieveByPartMasterId_V2FromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_VendorCatalogItem> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_VendorCatalogItem> results = null;
            data = new List<b_VendorCatalogItem>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_VendorCatalogItem_RetrieveByPartMasterId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_VendorCatalogItem>();
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
        public static object ProcessRowForRetrieveByPartMasterId_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_VendorCatalogItem obj = new b_VendorCatalogItem();

            // Load the object from the database
            //obj.LoadFromDatabaseForEquipmentCrossReference(reader);
            obj.LoadFromDatabaseForRetrieveByPartMasterId_V2(reader);
            // Return result
            return (object)obj;
        }
        public void LoadFromDatabaseForRetrieveByPartMasterId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // VendorCatalogItemId column, bigint, not null
                VendorCatalogItemId = reader.GetInt64(i++);

                // VendorName 
                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;
                // UnitCost
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i);
                }
                else
                {
                    UnitCost = 0;
                }
                i++;
                // PurchaseUOM column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    PurchaseUOM = reader.GetString(i);
                }
                else
                {
                    PurchaseUOM = "";
                }
                i++;
                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["VendorCatalogItemId"].ToString(); }
                catch { missing.Append("VendorCatalogItemId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["PurchaseUOM"].ToString(); }
                catch { missing.Append("PurchaseUOM "); }

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
    }
}
