using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_Metrics
    {
        #region Properties
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public long TimeFrame { get; set; }
        public string Flag { get; set; }
        public string SiteName { get; set; }
        public string ClientName { get; set; }
        public decimal WorkOrdersCreated { get; set; }
        public decimal WorkOrdersCompleted { get; set; }
        public decimal LaborHours { get; set; }
        public decimal Valuation { get; set; }
        public decimal LowParts { get; set; }
        public decimal PurchaseOrdersCreated { get; set; }
        public decimal PurchaseOrdersCompleted { get; set; }
        public decimal ReceivedAmount { get; set; }
        public decimal TotalValue { get; set; }
        public int CaseNo { get; set; }
        public bool IsEnterprise { get; set; }
        
        #endregion

        #region Retrieve Maintenance Metrics Details
        public static b_Metrics ProcessRowForRetrieveMaintenanceDetails(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForRetrieveMaintenanceDetails(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForRetrieveMaintenanceDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WorkOrdersCreated = reader.GetDecimal(i);
                }
                else
                {
                    WorkOrdersCreated = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WorkOrdersCompleted = reader.GetDecimal(i);
                }
                else
                {
                    WorkOrdersCompleted = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LaborHours = reader.GetDecimal(i);
                }
                else
                {
                    LaborHours = 0;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["WorkOrdersCreated"].ToString(); }
                catch { missing.Append("WorkOrdersCreated "); }

                try { reader["WorkOrdersCompleted"].ToString(); }
                catch { missing.Append("WorkOrdersCompleted "); }

                try { reader["LaborHours"].ToString(); }
                catch { missing.Append("LaborHours "); }

               

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

        public void RetrieveMaintenanceForSiteId(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrieveMaintenanceDetailsByMetricsName_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Retrieve Inventory Metrics Details
        public static b_Metrics ProcessRowForRetrieveInventoryDetails(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForRetrieveInventoryDetails(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForRetrieveInventoryDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Valuation = reader.GetDecimal(i);
                }
                else
                {
                    Valuation = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LowParts = reader.GetDecimal(i);
                }
                else
                {
                    LowParts = 0;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["Valuation"].ToString(); }
                catch { missing.Append("Valuation "); }

                try { reader["LowParts"].ToString(); }
                catch { missing.Append("LowParts "); }

               
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

        public void RetrieveInventoryForSiteId(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrieveInventoryDetailsByMetricsName_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Retrieve Purchasing Metrics Details
        public static b_Metrics ProcessRowForRetrievePurchasingDetails(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForRetrievePurchasingDetails(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForRetrievePurchasingDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrdersCreated = reader.GetDecimal(i);
                }
                else
                {
                    PurchaseOrdersCreated = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrdersCompleted = reader.GetDecimal(i); 
                }
                else
                {
                    PurchaseOrdersCompleted =0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ReceivedAmount = reader.GetDecimal(i);
                }
                else
                {
                    ReceivedAmount = 0;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["PurchaseOrdersCreated"].ToString(); }
                catch { missing.Append("PurchaseOrdersCreated "); }

                try { reader["PurchaseOrdersCompleted"].ToString(); }
                catch { missing.Append("PurchaseOrdersCompleted "); }

                try { reader["ReceivedAmount"].ToString(); }
                catch { missing.Append("ReceivedAmount "); }


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

        public void RetrievePurchasingForSiteId(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrievePurchasingDetailsByMetricsName_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Retrieve Metrics Value Sum By Data Date
        public void RetrieveMetricsValueSumByDataDate(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrieveMetricsValueSumByDataDate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        
        public static b_Metrics ProcessRowForMetricsValueSumByDataDate(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForMetricsValueSumByDataDate(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForMetricsValueSumByDataDate(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    DataDate = reader.GetDateTime(i);
                }
                else
                {
                    DataDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TotalValue = reader.GetDecimal(i);
                }
                else
                {
                    TotalValue = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["DataDate"].ToString(); }
                catch { missing.Append("DataDate "); }

                try { reader["TotalValue"].ToString(); }
                catch { missing.Append("TotalValue "); }               

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

        #region For Admin
        #region Retrieve Maintenance Metrics Details
        public static b_Metrics ProcessRowForRetrieveMaintenanceDetailsForAdmin(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForRetrieveMaintenanceDetailsForAdmin(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForRetrieveMaintenanceDetailsForAdmin(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientName = reader.GetString(i);
                }
                else
                {
                    ClientName = string.Empty;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WorkOrdersCreated = reader.GetDecimal(i);
                }
                else
                {
                    WorkOrdersCreated = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WorkOrdersCompleted = reader.GetDecimal(i);
                }
                else
                {
                    WorkOrdersCompleted = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LaborHours = reader.GetDecimal(i);
                }
                else
                {
                    LaborHours = 0;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientName"].ToString(); }
                catch { missing.Append("ClientName "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["WorkOrdersCreated"].ToString(); }
                catch { missing.Append("WorkOrdersCreated "); }

                try { reader["WorkOrdersCompleted"].ToString(); }
                catch { missing.Append("WorkOrdersCompleted "); }

                try { reader["LaborHours"].ToString(); }
                catch { missing.Append("LaborHours "); }



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

        public void RetrieveMaintenanceForAdmin(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrieveMaintenanceDetailsByMetricsName_ForAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Retrieve Inventory Metrics Details
        public static b_Metrics ProcessRowForRetrieveInventoryDetailsForAdmin(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForRetrieveInventoryDetailsForAdmin(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForRetrieveInventoryDetailsForAdmin(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientName = reader.GetString(i);
                }
                else
                {
                    ClientName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Valuation = reader.GetDecimal(i);
                }
                else
                {
                    Valuation = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LowParts = reader.GetDecimal(i);
                }
                else
                {
                    LowParts = 0;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientName"].ToString(); }
                catch { missing.Append("ClientName "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["Valuation"].ToString(); }
                catch { missing.Append("Valuation "); }

                try { reader["LowParts"].ToString(); }
                catch { missing.Append("LowParts "); }


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

        public void RetrieveInventoryForAdmin(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrieveInventoryDetailsByMetricsName_ForAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Retrieve Purchasing Metrics Details
        public static b_Metrics ProcessRowForRetrievePurchasingDetailsForAdmin(SqlDataReader reader)
        {
            // Create instance of object
            b_Metrics Metrics = new b_Metrics();
            Metrics.LoadFromDatabaseForRetrievePurchasingDetailsForAdmin(reader);
            return Metrics;
        }

        public int LoadFromDatabaseForRetrievePurchasingDetailsForAdmin(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientName = reader.GetString(i);
                }
                else
                {
                    ClientName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrdersCreated = reader.GetDecimal(i);
                }
                else
                {
                    PurchaseOrdersCreated = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PurchaseOrdersCompleted = reader.GetDecimal(i);
                }
                else
                {
                    PurchaseOrdersCompleted = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ReceivedAmount = reader.GetDecimal(i);
                }
                else
                {
                    ReceivedAmount = 0;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientName"].ToString(); }
                catch { missing.Append("ClientName "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["PurchaseOrdersCreated"].ToString(); }
                catch { missing.Append("PurchaseOrdersCreated "); }

                try { reader["PurchaseOrdersCompleted"].ToString(); }
                catch { missing.Append("PurchaseOrdersCompleted "); }

                try { reader["ReceivedAmount"].ToString(); }
                catch { missing.Append("ReceivedAmount "); }


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

        public void RetrievePurchasingForAdmin(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_Metrics> results
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

                results = Database.StoredProcedure.usp_Metrics_RetrievePurchasingDetailsByMetricsName_ForAdmin_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #endregion
    }
}
