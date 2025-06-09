using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Database.Business
{
    public partial class b_VendorInsurance
    {
        #region Property
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public int TotalCount { get; set; }
        public long Vendor_InsuranceSource { get; set; }
        #endregion

        public void RetrieveChunkSearchByVendorId(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_VendorInsurance> results
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

                results = Database.StoredProcedure.usp_VendorInsurance_RetrieveChunkSearchByVendorId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_VendorInsurance ProcessRowForVendorInsuranceChunkSearchSearchByVendorId(SqlDataReader reader)
        {
            // Create instance of object
            b_VendorInsurance MaterialRequest = new b_VendorInsurance();
            MaterialRequest.LoadFromDatabaseForVendorInsuranceChunkSearchSearchByVendorId(reader);
            return MaterialRequest;
        }

        public int LoadFromDatabaseForVendorInsuranceChunkSearchSearchByVendorId(SqlDataReader reader)
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
                // VendorInsuranceId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    VendorInsuranceId = reader.GetInt64(i);
                }
                else
                {
                    VendorInsuranceId = 0;
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
                // Amount column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Amount = reader.GetInt32(i);
                }
                else
                {
                    Amount = 0;
                }
                i++;
                // Vendor_InsuranceSource column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    Vendor_InsuranceSource = reader.GetInt64(i);
                }
                else
                {
                    Vendor_InsuranceSource = 0;
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

                try { reader["VendorInsuranceId"].ToString(); }
                catch { missing.Append("VendorInsuranceId "); }

                try { reader["Company"].ToString(); }
                catch { missing.Append("Company "); }

                try { reader["Contact"].ToString(); }
                catch { missing.Append("Contact "); }

                try { reader["ExpireDate"].ToString(); }
                catch { missing.Append("ExpireDate "); }

                try { reader["Amount"].ToString(); }
                catch { missing.Append("Amount "); }

                try { reader["Vendor_InsuranceSource"].ToString(); }
                catch { missing.Append("Vendor_InsuranceSource "); }

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

    }
}
