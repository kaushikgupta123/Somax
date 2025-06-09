using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_ServiceOrder
    {
        #region Properties
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string AssetName { get; set; }
        public string VIN { get; set; }
        public string PersonnelList { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public DateTime CreateDate { get; set; }
        public string Assigned { get; set; }
        public int ChildCount { get; set; }
        //public string EquipmentName { get; set; }
        public string Meter1Type { get; set; }
        public decimal Meter1CurrentReading { get; set; }
        public string Meter2Type { get; set; }
        public decimal Meter2CurrentReading { get; set; }
        public string CompletedByPersonnels { get; set; }
        public string CancelledByPersonnels { get; set; }
        public decimal ScheduledHours { get; set; }
        public bool IsDeleteFlag { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public DateTime? Meter1CurrentReadingDate { get; set; }
        public DateTime? Meter2CurrentReadingDate { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }
        public decimal LaborTotal { get; set; }
        public decimal PartTotal { get; set; }
        public decimal OtherTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public string ShiftDesc { get; set; }
        public string TypeDesc { get; set; }
        //public long Assign_PersonnelId { get; set; }

        #region Fleet Only
        public int ServiceOrderCount { get; set; }

        #endregion

        #endregion


        public static b_ServiceOrder ProcessRowForServiceOrderRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_ServiceOrder ServiceOrder = new b_ServiceOrder();
            ServiceOrder.LoadFromDatabaseForServiceOrderRetriveAllForSearch(reader);
            return ServiceOrder;
        }
        public int LoadFromDatabaseForServiceOrderRetriveAllForSearch(SqlDataReader reader)
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
                // ServiceOrderId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderId = reader.GetInt64(i);
                }
                else
                {
                    ServiceOrderId = 0;
                }
                i++;
                // SiteId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;
                // DepartmentId column, bigint, not null              
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                else
                {
                    DepartmentId = 0;
                }
                i++;
                // AreaId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                else
                {
                    AreaId = 0;
                }
                i++;
                // StoreroomId column, bigint, not null               
                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomId = 0;
                }
                i++;
                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                // EquipmentClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i);
                }
                else
                {
                    EquipmentClientLookupId = string.Empty;
                }
                i++;
                // AssetName column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i);
                }
                else
                {
                    AssetName = string.Empty;
                }
                i++;
                // VIN column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    VIN = reader.GetString(i);
                }
                else
                {
                    VIN = string.Empty;
                }
                i++;
                // Shift column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Shift = reader.GetString(i);
                }
                else
                {
                    Shift = string.Empty;
                }
                i++;
                // Description column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;
                // Status column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;
                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);
                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // Type column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = string.Empty;
                }
                i++;
                // Assigned column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = string.Empty;
                }
                i++;

                // Assigned_Per column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Assign_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Assign_PersonnelId = 0;
                }
                i++;

                // ScheduleDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduleDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduleDate = DateTime.MinValue;
                }
                i++;
                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                //ChildCount
                ChildCount = reader.GetInt32(i++);

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ServiceOrderId"].ToString(); }
                catch { missing.Append("ServiceOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["EquipmentClientlookupId"].ToString(); }
                catch { missing.Append("EquipmentClientlookupId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["Assign_PersonnelId"].ToString(); }
                catch { missing.Append("Assign_PersonnelId "); }

                try { reader["VIN"].ToString(); }
                catch { missing.Append("VIN "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["ScheduleDate"].ToString(); }
                catch { missing.Append("ScheduleDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

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

        public void RetrieveChunkSearch(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_ServiceOrder> results
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

                results = Database.StoredProcedure.usp_ServiceOrder_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Details
        public static b_ServiceOrder ProcessRowForRetrieveByServiceOrderId(SqlDataReader reader)
        {
            // Create instance of object
            b_ServiceOrder ServiceOrder = new b_ServiceOrder();
            ServiceOrder.LoadFromDatabaseForRetrieveByServiceOrderId(reader);
            return ServiceOrder;
        }
        public int LoadFromDatabaseForRetrieveByServiceOrderId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderId = reader.GetInt64(i);
                }
                else
                {
                    ServiceOrderId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i);
                }
                else
                {
                    EquipmentClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i);
                }
                else
                {
                    AssetName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    EquipmentId = reader.GetInt64(i);
                }
                else
                {
                    EquipmentId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter1Type = reader.GetString(i);
                }
                else
                {
                    Meter1Type = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter1CurrentReading = reader.GetDecimal(i);
                }
                else
                {
                    Meter1CurrentReading = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2Type = reader.GetString(i);
                }
                else
                {
                    Meter2Type = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2CurrentReading = reader.GetDecimal(i);
                }
                else
                {
                    Meter2CurrentReading = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter1CurrentReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    Meter1CurrentReadingDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2CurrentReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    Meter2CurrentReadingDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter1Units = reader.GetString(i);
                }
                else
                {
                    Meter1Units = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2Units = reader.GetString(i);
                }
                else
                {
                    Meter2Units = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ScheduleDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduleDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Assign_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    Assign_PersonnelId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Shift = reader.GetString(i);
                }
                else
                {
                    Shift = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    CompleteBy_PersonnelId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CompletedByPersonnels = reader.GetString(i);
                }
                else
                {
                    CompletedByPersonnels = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CancelDate = reader.GetDateTime(i);
                }
                else
                {
                    CancelDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CancelBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    CancelBy_PersonnelId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CancelledByPersonnels = reader.GetString(i);
                }
                else
                {
                    CancelledByPersonnels = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CancelReason = reader.GetString(i);
                }
                else
                {
                    CancelReason = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LaborTotal = reader.GetDecimal(i);
                }
                else
                {
                    LaborTotal = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    PartTotal = reader.GetDecimal(i);
                }
                else
                {
                    PartTotal = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    OtherTotal = reader.GetDecimal(i);
                }
                else
                {
                    OtherTotal = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    GrandTotal = reader.GetDecimal(i);
                }
                else
                {
                    GrandTotal = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ShiftDesc = reader.GetString(i);
                }
                else
                {
                    ShiftDesc = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TypeDesc = reader.GetString(i);
                }
                else
                {
                    TypeDesc = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ServiceOrderId"].ToString(); }
                catch { missing.Append("ServiceOrderId "); }

                try { reader["ServiceOrderClientLookupId"].ToString(); }
                catch { missing.Append("ServiceOrderClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["Meter1Type"].ToString(); }
                catch { missing.Append("Meter1Type "); }

                try { reader["Meter1CurrentReading"].ToString(); }
                catch { missing.Append("Meter1CurrentReading "); }

                try { reader["Meter2Type"].ToString(); }
                catch { missing.Append("Meter2Type "); }

                try { reader["Meter2CurrentReading"].ToString(); }
                catch { missing.Append("Meter2CurrentReading "); }

                try { reader["ScheduleDate"].ToString(); }
                catch { missing.Append("ScheduleDate "); }

                try { reader["AssignedPersonnels"].ToString(); }
                catch { missing.Append("AssignedPersonnels "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompletedByPersonnels"].ToString(); }
                catch { missing.Append("CompletedByPersonnels "); }

                try { reader["CancelDate"].ToString(); }
                catch { missing.Append("CancelDate "); }

                try { reader["CancelBy_PersonnelId"].ToString(); }
                catch { missing.Append("CancelBy_PersonnelId "); }

                try { reader["CancelledByPersonnels"].ToString(); }
                catch { missing.Append("CancelledByPersonnels "); }

                try { reader["CancelReason"].ToString(); }
                catch { missing.Append("CancelReason "); }

                try { reader["LaborTotal"].ToString(); }
                catch { missing.Append("LaborTotal "); }

                try { reader["PartTotal"].ToString(); }
                catch { missing.Append("PartTotal "); }

                try { reader["OtherTotal"].ToString(); }
                catch { missing.Append("OtherTotal "); }

                try { reader["GrandTotal"].ToString(); }
                catch { missing.Append("GrandTotal "); }

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
        public void RetrieveByServiceOrderId(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref b_ServiceOrder results
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

                results = Database.StoredProcedure.usp_ServiceOrder_RetrieveByServiceOrderId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Fleet Only
        public void LoadFromDatabaseDashBoard(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ServiceOrderCount = reader.GetInt32(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader[" ServiceOrderCount"].ToString(); }
                catch { missing.Append(" ServiceOrderCount "); }
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }
        }

        public static object ProcessRowDashboard(SqlDataReader reader)
        {
            // Create instance of object
            b_ServiceOrder obj = new b_ServiceOrder();
            // Load the object from the database
            obj.LoadFromDatabaseDashBoard(reader);
            // Return result
            return (object)obj;
        }
        public void ServiceOrder_RetrieveDashboardChart(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_ServiceOrder> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ServiceOrder> results = null;
            data = new List<b_ServiceOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ServiceOrder_RetrieveDashboardChart_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ServiceOrder>();
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

        public static b_ServiceOrder ProcessRowRetrievePersonnelInitial(SqlDataReader reader)
        {
            b_ServiceOrder serviceOrder = new b_ServiceOrder();
            serviceOrder.LoadFromDatabaseRetrievePersonnelInitial(reader);
            return serviceOrder;
        }
        public int LoadFromDatabaseRetrievePersonnelInitial(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // Personnels column, nvarchar(512), not null               
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = string.Empty;
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

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

        public void RetrievePersonnelInitial(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref b_ServiceOrder results
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

                results = Database.StoredProcedure.usp_ServiceOrder_RetrievePersonnelInitial_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void AddRemoveScheduleRecord(
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
                Database.StoredProcedure.usp_ServiceOrderSchedule_AddScheduleRecord_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Service order history
        public static b_ServiceOrder ProcessRowForServiceOrderHistory(SqlDataReader reader)
        {
            // Create instance of object
            b_ServiceOrder ServiceOrder = new b_ServiceOrder();
            ServiceOrder.LoadFromDatabaseForServiceOrderHistory(reader);
            return ServiceOrder;
        }
        public int LoadFromDatabaseForServiceOrderHistory(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ServiceOrderId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderId = reader.GetInt64(i);
                }
                else
                {
                    ServiceOrderId = 0;
                }
                i++;

                // ClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;

                // EquipmentClientLookupId column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i);
                }
                else
                {
                    EquipmentClientLookupId = string.Empty;
                }
                i++;

                // AssetName column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i);
                }
                else
                {
                    AssetName = string.Empty;
                }
                i++;

                // Status column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;

                // Type column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = string.Empty;
                }
                i++;

                // CompleteDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                //ChildCount
                ChildCount = reader.GetInt32(i);
                i++;
                //TotalCount
                TotalCount = reader.GetInt32(i);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ServiceOrderId"].ToString(); }
                catch { missing.Append("ServiceOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["EquipmentClientlookupId"].ToString(); }
                catch { missing.Append("EquipmentClientlookupId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }

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
        public void RetrieveServiceOrderHistory(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_ServiceOrder> results
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

                results = Database.StoredProcedure.usp_ServiceOrder_ServiceOrderHistory_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Retrieve by EquipmentId
        public static b_ServiceOrder ProcessRowForRetrieveByEquipmentId(SqlDataReader reader)
        {
            // Create instance of object
            b_ServiceOrder ServiceOrder = new b_ServiceOrder();
            ServiceOrder.LoadFromDatabaseForRetrieveByEquipmentId(reader);
            return ServiceOrder;
        }

        public int LoadFromDatabaseForRetrieveByEquipmentId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderId = reader.GetInt64(i);
                }
                else
                {
                    ServiceOrderId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = string.Empty;
                }
                i++;
                //
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i);
                }
                else
                {
                    EquipmentClientLookupId = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i);
                }
                else
                {
                    AssetName = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = string.Empty;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                //ChildCount
                ChildCount = reader.GetInt32(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ServiceOrderId"].ToString(); }
                catch { missing.Append("ServiceOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["ChildCount"].ToString(); }
                catch { missing.Append("ChildCount "); }


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

        public void RetrieveByEquipmentId(
     SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref List<b_ServiceOrder> results
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

                results = Database.StoredProcedure.usp_ServiceOrder_RetrieveByAssetId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
