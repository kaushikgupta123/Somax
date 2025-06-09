using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_VendorAssetMgt
    {
        #region Property
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public int TotalCount { get; set; }
        public long AssetMgtSource { get; set; }
        public bool AssetMgtRequired { get; set; }
        public DateTime? AssetMgtExpireDate { get; set; }
        public bool AssetMgtOverride { get; set; }
        public DateTime? AssetMgtOverrideDate { get; set; }
        #endregion

        public void RetrieveVendorAssetMgtChunkSearchByVendorId(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_VendorAssetMgt> results
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

                results = Database.StoredProcedure.usp_VendorAssetMgt_RetrieveChunkSearchByVendorId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_VendorAssetMgt ProcessRowForVendorAssetMgtChunkSearchSearchByVendorId(SqlDataReader reader)
        {
            // Create instance of object
            b_VendorAssetMgt VendorAssetMgt = new b_VendorAssetMgt();
            VendorAssetMgt.LoadFromDatabaseForVendorAssetMgtChunkSearchSearchByVendorId(reader);
            return VendorAssetMgt;
        }

        public int LoadFromDatabaseForVendorAssetMgtChunkSearchSearchByVendorId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;
                // VendorAssetMgtId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    VendorAssetMgtId = reader.GetInt64(i);
                }
                else
                {
                    VendorAssetMgtId = 0;
                }
                i++;
                // Company column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Company = reader.GetString(i);
                }
                else
                {
                    Company = string.Empty;
                }
                i++;
                // Contact column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Contact = reader.GetString(i);
                }
                else
                {
                    Contact = string.Empty;
                }
                i++;
                // Contract column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Contract = reader.GetString(i);
                }
                else
                {
                    Contract = string.Empty;
                }
                i++;
                // ExpireDate column, datetime, not null               
                if (false == reader.IsDBNull(i))
                {
                    ExpireDate = reader.GetDateTime(i);
                }
                else
                {
                    ExpireDate = DateTime.MinValue;
                }
                i++;
                // AssetMgtSource column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    AssetMgtSource = reader.GetInt64(i);
                }
                else
                {
                    AssetMgtSource =0;
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

                try { reader["VendorAssetMgtId"].ToString(); }
                catch { missing.Append("VendorAssetMgtId "); }

                try { reader["Company"].ToString(); }
                catch { missing.Append("Company "); }

                try { reader["Contact"].ToString(); }
                catch { missing.Append("Contact "); }

                try { reader["Contract"].ToString(); }
                catch { missing.Append("Contract "); }

                try { reader["ExpireDate"].ToString(); }
                catch { missing.Append("ExpireDate "); }
                
                try { reader["AssetMgtSource"].ToString(); }
                catch { missing.Append("AssetMgtSource "); }
               
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

        #region Header
        public void RetrieveVendorAssetMgtHeaderByVendorId(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_VendorAssetMgt> results
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

                results = Database.StoredProcedure.usp_VendorAssetMgt_RetrieveForHeaderByVendorId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_VendorAssetMgt ProcessRowForVendorAssetMgtHeaderByVendorId(SqlDataReader reader)
        {
            // Create instance of object
            b_VendorAssetMgt VendorAssetMgt = new b_VendorAssetMgt();
            VendorAssetMgt.LoadFromDatabaseForVendorAssetMgtByVendorId(reader);
            return VendorAssetMgt;
        }

        public int LoadFromDatabaseForVendorAssetMgtByVendorId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // VendorId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    VendorId = reader.GetInt64(i);
                }
                else
                {
                    VendorId = 0;
                }
                i++;
                // AssetMgtRequired column, bit, not null
                AssetMgtRequired = reader.GetBoolean(i++);
                // AssetMgtExpireDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetMgtExpireDate = reader.GetDateTime(i);
                }
                else
                {
                    AssetMgtExpireDate = DateTime.MinValue;
                }
                i++;
                // AssetMgtOverride column, bit, not null
                AssetMgtOverride = reader.GetBoolean(i++);

                // AssetMgtOverrideDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    AssetMgtOverrideDate = reader.GetDateTime(i);
                }
                else
                {
                    AssetMgtOverrideDate = DateTime.MinValue;
                }
                i++;
                // Company column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Company = reader.GetString(i);
                }
                else
                {
                    Company = string.Empty;
                }
                i++;
                // Contact column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Contact = reader.GetString(i);
                }
                else
                {
                    Contact = string.Empty;
                }
                i++;
                // Contract column, nvarchar(200), not null
                if (false == reader.IsDBNull(i))
                {
                    Contract = reader.GetString(i);
                }
                else
                {
                    Contract = string.Empty;
                }
                i++;
                // VendorAssetMgtId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    VendorAssetMgtId = reader.GetInt64(i);
                }
                else
                {
                    VendorAssetMgtId = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["VendorId"].ToString(); }
                catch { missing.Append("VendorId "); }

                try { reader["AssetMgtRequired"].ToString(); }
                catch { missing.Append("AssetMgtRequired "); }

                try { reader["AssetMgtExpireDate"].ToString(); }
                catch { missing.Append("AssetMgtExpireDate "); }

                try { reader["AssetMgtOverride"].ToString(); }
                catch { missing.Append("AssetMgtOverride "); }

                try { reader["AssetMgtOverrideDate"].ToString(); }
                catch { missing.Append("AssetMgtOverrideDate "); }

                try { reader["Company"].ToString(); }
                catch { missing.Append("Company "); }

                try { reader["Contact"].ToString(); }
                catch { missing.Append("Contact "); }

                try { reader["Contract"].ToString(); }
                catch { missing.Append("Contract "); }

                try { reader["VendorAssetMgtId"].ToString(); }
                catch { missing.Append("VendorAssetMgtId "); }

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

    }
}
