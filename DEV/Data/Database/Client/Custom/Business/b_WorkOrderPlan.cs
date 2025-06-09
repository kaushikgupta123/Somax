using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial  class b_WorkOrderPlan
    {
        #region Property

        public string PersonnelList { get; set; }

        public string ScheduledDateStart { get; set; }
        public string ScheduledDateEnd { get; set; }

        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string ClientLookupId { get; set; }
        public string ChargeTo  { get; set; }
        public string ChargeTo_Name { get; set; }
        public string RequireDate { get; set; }
        public string Type { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string SearchText { get; set; }

        public long WorkOrderId { get; set; }
        public Int64 PersonnelId { get; set; }
        public long WorkOrderScheduleId { get; set; }
        public string WorkOrderClientLookupId { get; set; }

        public decimal ScheduledHours { get; set; }
        public DateTime? ScheduledStartDate { get; set; }

        public DateTime? RequiredDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string NameLast { get; set; }
        public string PersonnelName { get; set; }
        public decimal SumPersonnelHour { get; set; }
        public decimal SumScheduledateHour { get; set; }
        public decimal GrandTotalHour { get; set; }
        public long PerIDNextValue { get; set; }
        public int TotalCount { get; set; }
        public string PerNextValue { get; set; }
        public DateTime SDNextValue { get; set; }
        public DateTime CreateDate { get; set; }
        public int ChildCount { get; set; }
        public bool IsDeleteFlag { get; set; }
        public bool EquipDown { get; set; }
        public bool DownRequired { get; set; }
        public string DepartmentName { get; set; }
        public string WorkAssigned_Name { get; set; }
        public decimal ScheduledDuration { get; set; }
        public string ChargeType { get; set; }

        public string SeriesName { get; set; }
        public Decimal Total { get; set; }
        #endregion

        #region  Work Order Plan Property
        public string PersonneNameFirst { get; set; }
        public string PersonneNameLast { get; set; }
        public string Priority { get; set; }
        public long ChargeToId { get; set; }
        public long WOPlanLineItemId { get; set; }
        public string WOPlanLineItemType { get; set; }
        #endregion
        public static b_WorkOrderPlan ProcessRowForResourceListChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForResourceListChunkSearch(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForResourceListChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                WorkOrderScheduleId = reader.GetInt64(i++);
                // WorkOrderClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientLookupId = reader.GetString(i++);
                }
                else
                {
                    WorkOrderClientLookupId = "";
                    i++;
                }

                // Type               
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i++);
                }
                else
                {
                    Type = "";
                    i++;
                }
                // Description               
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = "";
                    i++;
                }
                // ScheduledHours               
                if (false == reader.IsDBNull(i))
                {
                    ScheduledHours = reader.GetDecimal(i++);
                }
                else
                {
                    ScheduledHours = 0;
                    i++;
                }
                // Scheduled Start Date
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;
                // RequiredDate 
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                // EquipmentClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i++);
                }
                else
                {
                    EquipmentClientLookupId = "";
                    i++;
                }

                // ChargeTo_Name
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = "";
                    i++;
                }

                // NameLast
                if (false == reader.IsDBNull(i))
                {
                    NameLast = reader.GetString(i++);
                }
                else
                {
                    NameLast = "";
                    i++;
                }
                // Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = "";
                    i++;
                }
                // PersonnelName
                if (false == reader.IsDBNull(i))
                {
                    PersonnelName = reader.GetString(i++);
                }
                else
                {
                    PersonnelName = "";
                    i++;
                }                   
                if (false == reader.IsDBNull(i))
                {
                    SumPersonnelHour = reader.GetDecimal(i++);
                }
                else
                {
                    SumPersonnelHour = 0;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    SumScheduledateHour = reader.GetDecimal(i++);
                }
                else
                {
                    SumScheduledateHour = 0;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    GrandTotalHour = reader.GetDecimal(i++);
                }
                else
                {
                    GrandTotalHour = 0;
                    i++;
                }
                // TotalCount
                TotalCount = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    PerNextValue = reader.GetString(i++);
                }
                else
                {
                    PerNextValue = "";
                    i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    PerIDNextValue = reader.GetInt64(i++);
                }
                else
                {
                    PerIDNextValue = 0;
                    i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    SDNextValue = reader.GetDateTime(i);
                }
                else
                {
                    SDNextValue = DateTime.MinValue;

                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["WorkOrderScheduleId"].ToString(); }
                catch { missing.Append("WorkOrderScheduleId "); }

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours"); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["NameLast"].ToString(); }
                catch { missing.Append("NameLast "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["SumPersonnelHour"].ToString(); }
                catch { missing.Append("SumPersonnelHour "); }

                try { reader["SumScheduledateHour"].ToString(); }
                catch { missing.Append("SumScheduledateHour "); }

                try { reader["GrandTotalHour"].ToString(); }
                catch { missing.Append("GrandTotalHour "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["PerNextValue"].ToString(); }
                catch { missing.Append("PerNextValue "); }

                try { reader["PerIDNextValue"].ToString(); }//
                catch { missing.Append("PerIDNextValue "); }

                try { reader["SDNextValue"].ToString(); }
                catch { missing.Append("SDNextValue "); }

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

        public void RetrieveResourceListForChunkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WorkOrderPlan_RetrieveForChunkSearchResourceList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Work Order Plan Chunk Search List

        public static b_WorkOrderPlan ProcessRowForWorkOrderPlanRetriveAllChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan WOP = new b_WorkOrderPlan();
            WOP.LoadFromDatabaseForServiceOrderRetriveAllForSearch(reader);
            return WOP;
        }
        public int LoadFromDatabaseForServiceOrderRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderPlanId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderPlanId = reader.GetInt64(i);
                }
                else
                {
                    WorkOrderPlanId = 0;
                }
                i++;
                // Description column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;
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
                // EndDate column, datetime2, not null                
                if (false == reader.IsDBNull(i))
                {
                    EndDate = reader.GetDateTime(i);
                }
                else
                {
                    EndDate = DateTime.MinValue;
                }
                i++;
                // Status column, nvarchar(15, not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                // CreateDate column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // EquipmentClientLookupId column, nvarchar(15), not null
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


                try { reader["WorkOrderPlanId"].ToString(); }
                catch { missing.Append("WorkOrderPlanId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["EndDate"].ToString(); }
                catch { missing.Append("EndDate "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

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
ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WorkOrderPlan_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Resource calendar
        public static b_WorkOrderPlan ProcessRowForResourceCalendarChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForResourceCalendarChunkSearch(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForResourceCalendarChunkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // WorkOrderScheduleId column, bigint, not null
                WorkOrderScheduleId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                // PersonnelName column, bigint, not null
                PersonnelName = reader.GetString(i++);

                // WorkOrderClientLookupId 
                WorkOrderClientLookupId = reader.GetString(i++);

                // Description
                Description = reader.GetString(i++);

                // ScheduledHours
                ScheduledHours = reader.GetDecimal(i++);

                // Scheduled Start Date
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["WorkOrderScheduleId"].ToString(); }
                catch { missing.Append("WorkOrderScheduleId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["Personnel"].ToString(); }
                catch { missing.Append("Personnel "); }

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours"); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

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

        public void RetrieveResourceCalendarForChunkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WorkOrderPlan_RetrieveForSearchResourceCalendar_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_WorkOrderPlan ProcessRowForResourceCalendarAddSchedule(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForResourceCalendarAddSchedule(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForResourceCalendarAddSchedule(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // WorkOrderClientLookupId 
                WorkOrderClientLookupId = reader.GetString(i++);

                // Description
                Description = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

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

        public void RetrieveWorkOrderForAddSchedule(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WorkorderPlan_RetrieveWOForScheduleCalendar_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void AddScheduleRecordFromResourceCalendar(
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
                Database.StoredProcedure.usp_WorkOrderPlan_AddScheduleRecord_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RemoveScheduleRecordFromResourceCalendar(
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
                Database.StoredProcedure.usp_WorkOrderPlan_RemoveScheduleRecordFromResourceCalendar_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void DragWorkOrderScheduleFromCalendar(
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
                Database.StoredProcedure.usp_WorkOrderPlan_DragScheduleRecordFromResourceCalendar_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateScheduleRecordFromResourceCalendar(
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
                Database.StoredProcedure.usp_WorkOrderPlan_UpdateScheduleRecordFromResourceCalendar_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Work Order Plan Details
        public void RetrieveWorkOrderPlanListForRetrieveByWorkOrderPlanId(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_WorkOrderPlan results
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

                results = Database.StoredProcedure.usp_WorkOrderPlan_RetrieveByWorkOrderPlanId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_WorkOrderPlan ProcessRowForRetrieveByWorkOrderPlanId(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveByWorkOrderPlanId(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForRetrieveByWorkOrderPlanId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderPlanId column, bigint, not null
                WorkOrderPlanId = reader.GetInt64(i++);

                // Description column, nvarchar(200), not null
                Description = reader.GetString(i++);

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
                // EndDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    EndDate = reader.GetDateTime(i);
                }
                else
                {
                    EndDate = DateTime.MinValue;
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


                // LockPlan column, bit, not null
                LockPlan = reader.GetBoolean(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);
                // PersonneNameFirst column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    PersonneNameFirst = reader.GetString(i);
                }
                else
                {
                    PersonneNameFirst =string.Empty;

                }
                i++;
                // PersonneNameLast column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    PersonneNameLast = reader.GetString(i);
                }
                else
                {
                    PersonneNameLast = string.Empty;

                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderPlanId"].ToString(); }
                catch { missing.Append("WorkOrderPlanId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["StartDate"].ToString(); }
                catch { missing.Append("StartDate "); }

                try { reader["EndDate"].ToString(); }
                catch { missing.Append("EndDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["LockPlan"].ToString(); }
                catch { missing.Append("LockPlan "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["PersonneNameFirst"].ToString(); }
                catch { missing.Append("PersonneNameFirst "); }

                try { reader["PersonneNameLast"].ToString(); }
                catch { missing.Append("PersonneNameLast "); }


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

        #region Work Order Search Grid inside Planning
        public void RetrieveWorkOrderListForWorkOrderPlanForChunkSearch(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WorkOrderPlan_RetrieveForChunkSearchPlanList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_WorkOrderPlan ProcessRowForWorkOrderForWorkOrderPlanRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForWorkOrderForWorkOrderPlanRetriveAllForSearch(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForWorkOrderForWorkOrderPlanRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {  // WOPlanLineItemId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    WOPlanLineItemId = reader.GetInt64(i);
                }
                else
                {
                    WOPlanLineItemId = 0;
                }
                i++;
                // WorkOrderId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderId = reader.GetInt64(i);
                }
                else
                {
                    WorkOrderId = 0;
                }
                i++;
                // WorkOrderPlanId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderPlanId = reader.GetInt64(i);
                }
                else
                {
                    WorkOrderPlanId = 0;
                }
                i++;
                // WorkOrderClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientLookupId = reader.GetString(i++);
                }
                else
                {
                    WorkOrderClientLookupId = "";
                    i++;
                }
                // Description               
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = "";
                    i++;
                }
                // Type               
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i++);
                }
                else
                {
                    Type = "";
                    i++;
                }
                // RequiredDate 
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                // EquipmentClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i++);
                }
                else
                {
                    EquipmentClientLookupId = "";
                    i++;
                }
                // ChargeTo_Name
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = "";
                    i++;
                }
                // TotalCount
                TotalCount = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    WOPlanLineItemType = reader.GetString(i++);
                }
                else
                {
                    WOPlanLineItemType = "";
                    i++;
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["WOPlanLineItemId"].ToString(); }
                catch { missing.Append("WOPlanLineItemId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["WOPlanLineItemType"].ToString(); }
                catch { missing.Append("WOPlanLineItemType "); }

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

        #region WorkOrder WorkOrderPlan LookupList By Search Criteria
        public static b_WorkOrderPlan ProcessRowForWorkOrder_WorkOrderPlanLookupListBySearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForWorkOrder_WorkOrderPlanLookupListBySearchCriteria(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForWorkOrder_WorkOrderPlanLookupListBySearchCriteria(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // ChargeToId column, bigint, not null
                ChargeTo = reader.GetString(i++);

                // ChargeTo_Name column, nvarchar(63), not null
                ChargeTo_Name = reader.GetString(i++);

                // Description column, nvarchar(max), not null
                Description = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Priority column, nvarchar(15), not null
                Priority = reader.GetString(i++);
                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // RequiredDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;


                // TotalCount column, int, not null
                TotalCount = reader.GetInt32(i++);


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ChargeTo"].ToString(); }
                catch { missing.Append("ChargeTo "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }
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

        public void RetrieveWorkOrder_WorkOrderPlanLookupListBySearchCriteria(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WorkOrder_WorkOrderPlanLookupListBySearchCriteria_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
                public void RetrieveAvailableWorkSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                        results = Database.StoredProcedure.usp_WorkOrderPlan_GetAvailableWorkBySearchCriteria_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForAvailableWorkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForAvailableWorkSearch(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForAvailableWorkSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderClientLookupId 
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientLookupId = reader.GetString(i++);
                }
                else
                {
                    WorkOrderClientLookupId = "";
                    i++;
                }
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // Type               
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i++);
                }
                else
                {
                    Type = "";
                    i++;
                }
                // Description               
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = "";
                    i++;
                }
                // ScheduledHours
                ScheduledHours = reader.GetDecimal(i++);

                // ChargeTo_Name
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = "";
                    i++;
                }

                // Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = "";
                    i++;
                }

                EquipDown = reader.GetBoolean(i++);


                if (false == reader.IsDBNull(i))
                {
                    Priority = reader.GetString(i++);
                }
                else
                {
                    Priority = "";
                    i++;
                }

                // Scheduled Start Date
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;

                ChargeToId = reader.GetInt64(i++);
                // Scheduled Start Date
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;
                DownRequired = reader.GetBoolean(i++);

                // RequiredDate 
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;

                // DepartmentName
                if (false == reader.IsDBNull(i))
                {
                    DepartmentName = reader.GetString(i++);
                }
                else
                {
                    DepartmentName = "";
                    i++;
                }

                // WorkAssigned_Name
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i++);
                }
                else
                {
                    WorkAssigned_Name = "";
                    i++;
                }
                // ScheduledDuration
                ScheduledDuration = reader.GetDecimal(i++);

                // ChargeTo
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo = reader.GetString(i++);
                }
                else
                {
                    ChargeTo = "";
                    i++;
                }
                // ChargeType
                if (false == reader.IsDBNull(i))
                {
                    ChargeType = reader.GetString(i++);
                }
                else
                {
                    ChargeType = "";
                    i++;
                }


                // TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours"); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["EquipDown"].ToString(); }
                catch { missing.Append("EquipDown "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["DepartmentName"].ToString(); }
                catch { missing.Append("DepartmentName "); }

                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                try { reader["ScheduledDuration"].ToString(); }
                catch { missing.Append("ScheduledDuration "); }

                try { reader["ChargeTo"].ToString(); }
                catch { missing.Append("ChargeTo "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

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

        public void RetrievePlannedWorkorderEstimatedHoursByAssigned(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_PlannedWorkorderEstimatedHoursByAssigned_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForPlannedWorkorderEstimatedHoursByAssigned(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForProcessRowForPlannedWorkorderEstimatedHoursByAssigned(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProcessRowForPlannedWorkorderEstimatedHoursByAssigned(SqlDataReader reader)
        {
            int i = 0;
            try
            {  // PersonnelId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;
                //PersonnelName
                if (false == reader.IsDBNull(i))
                {
                    PersonnelName = reader.GetString(i++);
                }
                else
                {
                    PersonnelName = "";
                    i++;
                }

                // SeriesName               
                if (false == reader.IsDBNull(i))
                {
                    SeriesName = reader.GetString(i++);
                }
                else
                {
                    SeriesName = "";
                    i++;
                }

                //Total
                Total = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["SeriesName"].ToString(); }
                catch { missing.Append("SeriesName "); }

                try { reader["Total"].ToString(); }
                catch { missing.Append("Total "); }

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

        public void RetrievePlannedWorkorderActualHoursByAssigned(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_PlannedWorkorderActualHoursByAssigned_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForPlannedWorkorderActualHoursByAssigned(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForProcessRowForPlannedWorkorderActualHoursByAssigned(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProcessRowForPlannedWorkorderActualHoursByAssigned(SqlDataReader reader)
        {
            int i = 0;
            try
            {  // PersonnelId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;
                //PersonnelName
                if (false == reader.IsDBNull(i))
                {
                    PersonnelName = reader.GetString(i++);
                }
                else
                {
                    PersonnelName = "";
                    i++;
                }

                // SeriesName               
                if (false == reader.IsDBNull(i))
                {
                    SeriesName = reader.GetString(i++);
                }
                else
                {
                    SeriesName = "";
                    i++;
                }

                //Total
                Total = reader.GetDecimal(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["SeriesName"].ToString(); }
                catch { missing.Append("SeriesName "); }

                try { reader["Total"].ToString(); }
                catch { missing.Append("Total "); }

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
        public void RetrieveCompleteWorkorderByAssigned(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_CompleteWorkorderByAssigned_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForCompleteWorkorderByAssigned(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForProcessRowForCompleteWorkorderByAssigned(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProcessRowForCompleteWorkorderByAssigned(SqlDataReader reader)
        {
            int i = 0;
            try
            {  // PersonnelId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;
                //PersonnelName
                if (false == reader.IsDBNull(i))
                {
                    PersonnelName = reader.GetString(i++);
                }
                else
                {
                    PersonnelName = "";
                    i++;
                }

                // SeriesName               
                if (false == reader.IsDBNull(i))
                {
                    SeriesName = reader.GetString(i++);
                }
                else
                {
                    SeriesName = "";
                    i++;
                }

                //Total
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["SeriesName"].ToString(); }
                catch { missing.Append("SeriesName "); }

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

        public void RetrieveInCompleteWorkorderByAssigned(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_IncompleteWorkorderByAssigned_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForInCompleteWorkorderByAssigned(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForProcessRowForInCompleteWorkorderByAssigned(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProcessRowForInCompleteWorkorderByAssigned(SqlDataReader reader)
        {
            int i = 0;
            try
            {  // PersonnelId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    PersonnelId = 0;
                }
                i++;
                //PersonnelName
                if (false == reader.IsDBNull(i))
                {
                    PersonnelName = reader.GetString(i++);
                }
                else
                {
                    PersonnelName = "";
                    i++;
                }

                // SeriesName               
                if (false == reader.IsDBNull(i))
                {
                    SeriesName = reader.GetString(i++);
                }
                else
                {
                    SeriesName = "";
                    i++;
                }

                //Total
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["PersonnelName"].ToString(); }
                catch { missing.Append("PersonnelName "); }

                try { reader["SeriesName"].ToString(); }
                catch { missing.Append("SeriesName "); }

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
        public  long RetrieveCountPlannedWorkorderByComplete(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
             long ClientId,
         long SiteId,
         long WorkOrderPlanId
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
               // processRow = new Database.SqlClient.ProcessRow<b_WorkOrderPlan>(reader => { this.LoadFromDatabaseCountPlannedWorkorderByStatus(reader); return this; });
                return Database.StoredProcedure.usp_WOPlanLineItem_PlannedWorkorderByComplete_V2.CallStoredProcedure(command ,callerUserInfoId, callerUserName,ClientId,SiteId,WorkOrderPlanId);


                // Call the stored procedure to retrieve the data
                //return Database.StoredProcedure.usp_WOPlanLineItem_PlannedWorkorderByComplete_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName,clientId, SiteId, obj.WorkOrderPlanId);
            }
            catch
            {
                return 0;
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
            }
        }
        public long RetrieveCountPlannedWorkorderByInComplete(
     SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
             long ClientId,
         long SiteId,
         long WorkOrderPlanId
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
                // Call the stored procedure to retrieve the data
                return Database.StoredProcedure.usp_WOPlanLineItem_PlannedWorkorderByInComplete_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, SiteId, WorkOrderPlanId);

            }
            catch
            {
                return 0;
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
            }
        }

        public void RetrieveCompleteWorkorderByType(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_CompleteWorkorderByType_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForCompleteWorkorderByType(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForProcessRowForCompleteWorkorderByType(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProcessRowForCompleteWorkorderByType(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // SeriesName               
                if (false == reader.IsDBNull(i))
                {
                    SeriesName = reader.GetString(i++);
                }
                else
                {
                    SeriesName = "";
                    i++;
                }

                //TotalCount
                TotalCount = reader.GetInt32(i++);

                //Type
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i++);
                }
                else
                {
                    Type = "";
                    i++;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SeriesName"].ToString(); }
                catch { missing.Append("SeriesName "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

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
        public void RetrieveIncompleteWorkorderByType(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_WorkOrderPlan> results
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

                results = Database.StoredProcedure.usp_WOPlanLineItem_InCompleteWorkorderByType_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrderPlan ProcessRowForIncompleteWorkorderByType(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrderPlan obj = new b_WorkOrderPlan();

            // Load the object from the database
            obj.LoadFromDatabaseForProcessRowForIncompleteWorkorderByType(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProcessRowForIncompleteWorkorderByType(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // SeriesName               
                if (false == reader.IsDBNull(i))
                {
                    SeriesName = reader.GetString(i++);
                }
                else
                {
                    SeriesName = "";
                    i++;
                }

                //TotalCount
                TotalCount = reader.GetInt32(i++);

                //Type
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i++);
                }
                else
                {
                    Type = "";
                    i++;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SeriesName"].ToString(); }
                catch { missing.Append("SeriesName "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

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


        public void PlannerWorkOrderLineItemsStatuses(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName,
         ref List<KeyValuePair<string, long>> results
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

                results = StoredProcedure.usp_WOPlanLineItem_RetrievePlanLineItemStatuses_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void WorkOrderPlanningEstimateHours(
             SqlConnection connection,
             SqlTransaction transaction,
             long callerUserInfoId,
             string callerUserName,
             ref List<KeyValuePair<string, decimal>> results
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

                results = StoredProcedure.usp_WorkOrderSchedule_RetreivePlanEstimatedHours_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
    }
}
