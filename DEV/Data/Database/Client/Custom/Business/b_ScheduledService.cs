using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_ScheduledService
    {
        #region Property
        public string ClientLookupId { get; set; }
        /// <summary>
        /// Name property
        /// </summary>
        public string Name { get; set; }
        public string ServiceTasksDescription { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Flag { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public List<b_ScheduledService> listOfScheduledService { get; set; }
        public Int64 StoreRoomId { get; set; }
        public Int32 TotalCount { get; set; }
        public string Schedule { get; set; }
        public string NextDue { get; set; }
        public string ImageUrl { get; set; }
        public string LastCompleted { get; set; }
        public string ServiceTaskDesc { get; set; }
        public string LastCompletedLine1 { get; set; }
        public string LastCompletedLine2 { get; set; }
        public string NextDueDateStr { get; set; }
        public string LDateStr { get; set; }
        public string LastCompletedstr { get; set; }
        public string Meter1Type { get; set; }
        public string Meter2Type { get; set; }
        public string Meter1Units { get; set; }
        public string Meter2Units { get; set; }

        #region Fleet Only
        public int PastDueServiceCount { get; set; }

        public string Status { get; set; }
        #endregion
        #endregion

        #region Retrieve Search
        public void RetrieveScheduledServiceChunkSearchV2(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref b_ScheduledService results
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
                results = Database.StoredProcedure.usp_FleetScheduledService_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_ScheduledService ProcessRetrieveForSchduleServiceChunkV2(SqlDataReader reader)
        {
            b_ScheduledService SchduleService = new b_ScheduledService();

            SchduleService.LoadFromDatabaseForrSchduleServiceChunkSearchV2(reader);
            return SchduleService;
        }
        public int LoadFromDatabaseForrSchduleServiceChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // ServiceTask Id
                ServiceTaskId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //  ClientLookupId
                ClientLookupId = reader.GetString(i++);

                //  Name
                Name = reader.GetString(i++);

                //  ServiceTasksDescription
                ServiceTasksDescription = reader.GetString(i++);

                // TimeInterval column, int, not null
                TimeInterval = reader.GetInt32(i++);
                // Meter1Interval column, decimal(9,1), not null
                Meter1Interval = reader.GetDecimal(i++);
                // Meter2Interval column, decimal(9,1), not null
                Meter2Interval = reader.GetDecimal(i++);
                // TimeIntervalType column, nvarchar(9), not null
                TimeIntervalType = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Meter1Units = reader.GetString(i);
                }
                else
                {
                    Meter1Units = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Meter2Units = reader.GetString(i);
                }
                else
                {
                    Meter2Units = "";

                }
                i++;
                // NextDueDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                // NextDueMeter1 column, decimal(9,1), not null
                NextDueMeter1 = reader.GetDecimal(i++);

                // NextDueMeter2 column, decimal(9,1), not null
                NextDueMeter2 = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    LastPerformedDate = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformedDate = DateTime.MinValue;
                }
                i++;

                // LastPerformedMeter1 column, decimal(9,1), not null
                LastPerformedMeter1 = reader.GetDecimal(i++);

                // LastPerformedMeter2 column, decimal(9,1), not null
                LastPerformedMeter2 = reader.GetDecimal(i++);


                if (false == reader.IsDBNull(i))
                {
                    ScheduledServiceId = reader.GetInt64(i);
                }
                else
                {
                    ScheduledServiceId = 0;
                }
                i++;



                if (false == reader.IsDBNull(i))
                {
                    ImageUrl = reader.GetString(i);
                }
                else
                {
                    ImageUrl = "";
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

                //ImageUrl = reader.GetString(i++);
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }


                try { reader["ServiceTaskId"].ToString(); }
                catch { missing.Append("ServiceTaskId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["ServiceTasksDescription"].ToString(); }
                catch { missing.Append("ServiceTasksDescription "); }

                try { reader["TimeInterval"].ToString(); }
                catch { missing.Append("TimeInterval "); }

                try { reader["Meter1Interval"].ToString(); }
                catch { missing.Append("Meter1Interval "); }

                try { reader["Meter2Interval"].ToString(); }
                catch { missing.Append("Meter2Interval "); }

                try { reader["TimeIntervalType"].ToString(); }
                catch { missing.Append("TimeIntervalType "); }

                try { reader["TimeIntervalType"].ToString(); }
                catch { missing.Append("TimeIntervalType "); }

                try { reader["Meter1Units"].ToString(); }
                catch { missing.Append("Meter1Units "); }


                try { reader["Meter2Units"].ToString(); }
                catch { missing.Append("Meter2Units "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }


                try { reader["NextDueMeter1"].ToString(); }
                catch { missing.Append("NextDueMeter1 "); }

                try { reader["NextDueMeter2"].ToString(); }
                catch { missing.Append("NextDueMeter2 "); }

                try { reader["LastPerformedDate"].ToString(); }
                catch { missing.Append("LastPerformedDate"); }

                try { reader["LastPerformedMeter1"].ToString(); }
                catch { missing.Append("LastPerformedMeter1"); }

                try { reader["LastPerformedMeter2"].ToString(); }
                catch { missing.Append("LastPerformedMeter2 "); }


                try { reader["ScheduledServiceId"].ToString(); }
                catch { missing.Append("ScheduledServiceId"); }

                try { reader["ImageUrl"].ToString(); }
                catch { missing.Append("ImageUrl"); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag"); }

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

        #region Get Edited Data
        public void RetrieveByEquipmentIdandScheduledServiceIdFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_ScheduledService> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ScheduledService>(reader => { this.LoadFromDatabaseforRetrieveByEquipmentIdandScheduledServiceId(reader); return this; });
                StoredProcedure.usp_FleetScheduledService_RetrieveByEquipmentIdandScheduledServiceId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public int LoadFromDatabaseforRetrieveByEquipmentIdandScheduledServiceId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);
                //  ScheduledServiceId column, bigint, not null
                ScheduledServiceId = reader.GetInt64(i++);
                //  ServiceTaskId column, bigint, not null
                ServiceTaskId = reader.GetInt64(i++);
                Last_ServiceOrderId = reader.GetInt64(i++);
                // LastPerformedDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    LastPerformedDate = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformedDate = DateTime.MinValue;
                }
                i++;
                //  LastPerformedMeter1 column, Decimal, not null
                LastPerformedMeter1 = reader.GetDecimal(i++);
                //  LastPerformedMeter2 column, Decimal, not null
                LastPerformedMeter2 = reader.GetDecimal(i++);
                //  Meter1Interval column, Decimal, not null
                Meter1Interval = reader.GetDecimal(i++);
                //  Meter1Threshold column, Decimal, not null
                Meter1Threshold = reader.GetDecimal(i++);
                //  Meter2Interval column, Decimal, not null
                Meter2Interval = reader.GetDecimal(i++);
                //  Meter2Threshold column, Decimal, not null
                Meter2Threshold = reader.GetDecimal(i++);
                // LastPerformedDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;
                //  NextDueMeter1 column, Decimal, not null
                NextDueMeter1 = reader.GetDecimal(i++);
                //  NextDueMeter2 column, Decimal, not null
                NextDueMeter2 = reader.GetDecimal(i++);
                //  TimeInterval column, Decimal, not null
                TimeInterval = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    TimeIntervalType = reader.GetString(i);
                }
                else
                {
                    TimeIntervalType = "";
                }
                i++;
                //  TimeInterval column, Decimal, not null
                TimeThreshold = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    TimeThresoldType = reader.GetString(i);
                }
                else
                {
                    TimeThresoldType = "";
                }
                i++;
                // CreateDate column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";

                }
                i++;
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
                // ModifyBy column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";

                }
                i++;
                // CreateDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                }
                i++;
                //  EquipmentId column, Int64, not null
                EquipmentId = reader.GetInt64(i++);
                // ClientLookupId column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";

                }
                i++;
                //  AreaId column, Int64, not null
                AreaId = reader.GetInt64(i++);
                //  DepartmentId column, Int64, not null
                DepartmentId = reader.GetInt64(i++);
                //  StoreroomId column, Int64, not null
                StoreroomId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    Meter1Type = reader.GetString(i);
                }
                else
                {
                    Meter1Type = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Meter2Type = reader.GetString(i);
                }
                else
                {
                    Meter2Type = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Meter1Units = reader.GetString(i);
                }
                else
                {
                    Meter1Units = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Meter2Units = reader.GetString(i);
                }
                else
                {
                    Meter2Units = "";

                }
                i++;
                // ClientLookupId column, nvarchar, not null
                if (false == reader.IsDBNull(i))
                {
                    ImageUrl = reader.GetString(i);
                }
                else
                {
                    ImageUrl = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    RepairReason = reader.GetString(i);
                }
                else
                {
                    RepairReason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VMRSSystem = reader.GetString(i);
                }
                else
                {
                    VMRSSystem = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VMRSAssembly = reader.GetString(i);
                }
                else
                {
                    VMRSAssembly = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
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

        #region Insert Data
        public void InsertIntoDatabaseCustom(
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
                Database.StoredProcedure.usp_FleetScheduledService_CreateCustom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region Update Data
        public void UpdateInDatabaseCustom(
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
                Database.StoredProcedure.usp_FleetScheduledService_UpdateCustom_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Active/Inactive ScService
        public void RetrieveByForeignKeysFromDatabase_V2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_ScheduledService> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_ScheduledService>(reader => { this.LoadFromDatabaseByPKForeignKey_V2(reader); return this; });
                StoredProcedure.usp_ScheduledService_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                //StoredProcedure.usp_ScheduledService_UpdateByPKForeignKeys_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
            int i = this.LoadFromDatabase(reader);

            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ScheduledServiceId = reader.GetInt64(i);
                }
                else
                {
                    ScheduledServiceId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i);
                }
                else
                {
                    ClientId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SiteId = reader.GetInt64(i);
                }
                else
                {
                    SiteId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AreaId = reader.GetInt64(i);
                }
                else
                {
                    AreaId = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    DepartmentId = reader.GetInt64(i);
                }
                else
                {
                    DepartmentId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    StoreroomId = reader.GetInt64(i);
                }
                else
                {
                    StoreroomId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ServiceTaskId = reader.GetInt64(i);
                }
                else
                {
                    ServiceTaskId = 0;
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
                    InactiveFlag = reader.GetBoolean(i);
                }
                else
                {
                    InactiveFlag = false;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Last_ServiceOrderId = reader.GetInt64(i);
                }
                else
                {
                    Last_ServiceOrderId = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LastPerformedDate = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformedDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LastPerformedMeter1 = reader.GetDecimal(i);
                }
                else
                {
                    LastPerformedMeter1 = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    LastPerformedMeter2 = reader.GetDecimal(i);
                }
                else
                {
                    LastPerformedMeter2 = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    Meter1Interval = reader.GetDecimal(i);
                }
                else
                {
                    Meter1Interval = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2Interval = reader.GetDecimal(i);
                }
                else
                {
                    Meter2Interval = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2Threshold = reader.GetDecimal(i);
                }
                else
                {
                    Meter2Threshold = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    NextDueMeter1 = reader.GetDecimal(i);
                }
                else
                {
                    NextDueMeter1 = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NextDueMeter2 = reader.GetDecimal(i);
                }
                else
                {
                    NextDueMeter2 = 0;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TimeInterval = reader.GetInt32(i);
                }
                else
                {
                    TimeInterval = 0;
                }
                i++;



                if (false == reader.IsDBNull(i))
                {
                    TimeIntervalType = reader.GetString(i);
                }
                else
                {
                    TimeIntervalType = "";
                }
                i++;
                //  TimeInterval column, Decimal, not null

                if (false == reader.IsDBNull(i))
                {
                    TimeThreshold = reader.GetInt32(i);
                }
                else
                {
                    TimeThreshold = 0;
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    TimeThresoldType = reader.GetString(i);
                }
                else
                {
                    TimeThresoldType = "";
                }
                i++;


            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ScheduledServiceId"].ToString(); }
                catch { missing.Append("ScheduledServiceId"); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId"); }


                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId"); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId"); }



                try { reader["StoreRoomId"].ToString(); }
                catch { missing.Append("StoreRoomId "); }

                try { reader["ServiceTaskId"].ToString(); }
                catch { missing.Append("ServiceTaskId"); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["Last_ServiceOrderId"].ToString(); }
                catch { missing.Append("Last_ServiceOrderId"); }

                try { reader["LastPerformedMeter1"].ToString(); }
                catch { missing.Append("LastPerformedMeter1"); }

                try { reader["LastPerformedMeter2"].ToString(); }
                catch { missing.Append("LastPerformedMeter1"); }

                try { reader["Meter1Interval"].ToString(); }
                catch { missing.Append("Meter1Interval"); }

                try { reader["Meter1Threshold"].ToString(); }
                catch { missing.Append("Meter1Threshold"); }

                try { reader["Meter2Interval"].ToString(); }
                catch { missing.Append("Meter2Interval"); }

                try { reader["Meter2Threshold"].ToString(); }
                catch { missing.Append("Meter2Threshold"); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("Meter2Threshold"); }

                try { reader["NextDueMeter1"].ToString(); }
                catch { missing.Append("NextDueMeter1"); }


                try { reader["NextDueMeter2"].ToString(); }
                catch { missing.Append("NextDueMeter2"); }

                try { reader["TimeInterval"].ToString(); }
                catch { missing.Append("TimeInterval"); }

                try { reader["TimeIntervalType"].ToString(); }
                catch { missing.Append("TimeIntervalType"); }

                try { reader["TimeThreshold"].ToString(); }
                catch { missing.Append("TimeThreshold"); }

                try { reader["TimeThresoldType"].ToString(); }
                catch { missing.Append("TimeThresoldType"); }

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
        #region 

        public void UpdateByForeignKeysInDatabase_V2(
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
                StoredProcedure.usp_ScheduledService_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Validate Equipment and Service Task Id
        public void ValidateEquipmentAndServiceTaskId(
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
                results = Database.StoredProcedure.usp_ScheduledService_ValidateEquipIdAndServiceTaskId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #endregion

        #region Fleet Only
        public void LoadFromDatabaseDashBoard(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                PastDueServiceCount = reader.GetInt32(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader[" PastDueServiceCount"].ToString(); }
                catch { missing.Append(" PastDueServiceCount "); }
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
            b_ScheduledService obj = new b_ScheduledService();

            // Load the object from the database
            obj.LoadFromDatabaseDashBoard(reader);

            // Return result
            return (object)obj;
        }
        public void ScheduledService_RetrieveDashboardChart(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_ScheduledService> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_ScheduledService> results = null;
            data = new List<b_ScheduledService>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_ScheduledService_RetrieveDashboardChart_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_ScheduledService>();
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

        #region Scheduled Service Retrieve By EquipmentId
        public static b_ScheduledService ProcessRetrieveForSchduleServiceByEquipmentIdV2(SqlDataReader reader)
        {
            b_ScheduledService SchduleService = new b_ScheduledService();

            SchduleService.LoadFromDatabaseForSchduleServiceRetrieveByEquipmentIdV2(reader);
            return SchduleService;
        }
        public int LoadFromDatabaseForSchduleServiceRetrieveByEquipmentIdV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                ScheduledServiceId = reader.GetInt64(i++);

                ServiceTaskId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ServiceTasksDescription = reader.GetString(i);
                }
                else
                {
                    ServiceTasksDescription = "";

                }
                i++;

                TimeInterval = reader.GetInt32(i++);

                TimeIntervalType = reader.GetString(i++);

                Meter1Interval = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Meter1Units = reader.GetString(i);
                }
                else
                {
                    Meter1Units = "";
                }
                i++;

                Meter2Interval = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Meter2Units = reader.GetString(i);
                }
                else
                {
                    Meter2Units = "";

                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    NextDueDate = reader.GetDateTime(i);
                }
                else
                {
                    NextDueDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    TimeThresoldType = reader.GetString(i);
                }
                else
                {
                    TimeThresoldType = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LastPerformedDate = reader.GetDateTime(i);
                }
                else
                {
                    LastPerformedDate = DateTime.MinValue;
                }
                i++;

                LastPerformedMeter1 = reader.GetDecimal(i++);

                LastPerformedMeter2 = reader.GetDecimal(i++);

                NextDueMeter1 = reader.GetDecimal(i++);

                NextDueMeter2 = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    Meter1Type = reader.GetString(i);
                }
                else
                {
                    Meter1Type = "";

                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Meter2Type = reader.GetString(i);
                }
                else
                {
                    Meter2Type = "";

                }
                i++;

                //TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }


                try { reader["ScheduledServiceId"].ToString(); }
                catch { missing.Append("ScheduledServiceId "); }

                try { reader["ServiceTaskId"].ToString(); }
                catch { missing.Append("ServiceTaskId "); }

                try { reader["ServiceTasksDescription"].ToString(); }
                catch { missing.Append("ServiceTasksDescription "); }

                try { reader["TimeInterval"].ToString(); }
                catch { missing.Append("TimeInterval "); }

                try { reader["TimeIntervalType"].ToString(); }
                catch { missing.Append("TimeIntervalType "); }

                try { reader["Meter1Interval"].ToString(); }
                catch { missing.Append("Meter1Interval "); }

                try { reader["Meter1Interval"].ToString(); }
                catch { missing.Append("Meter1Interval "); }

                try { reader["Meter1Units"].ToString(); }
                catch { missing.Append("Meter1Units "); }

                try { reader["Meter2Interval"].ToString(); }
                catch { missing.Append("Meter2Interval "); }

                try { reader["Meter2Units"].ToString(); }
                catch { missing.Append("Meter2Units "); }

                try { reader["NextDueDate"].ToString(); }
                catch { missing.Append("NextDueDate "); }

                try { reader["TimeThresoldType"].ToString(); }
                catch { missing.Append("TimeThresoldType "); }

                try { reader["LastPerformedDate"].ToString(); }
                catch { missing.Append("LastPerformedDate "); }

                try { reader["LastPerformedMeter1"].ToString(); }
                catch { missing.Append("LastPerformedMeter1 "); }

                try { reader["LastPerformedMeter2"].ToString(); }
                catch { missing.Append("LastPerformedMeter2"); }

                try { reader["NextDueMeter1"].ToString(); }
                catch { missing.Append("NextDueMeter1"); }

                try { reader["NextDueMeter2"].ToString(); }
                catch { missing.Append("NextDueMeter2 "); }

                try { reader["Meter1Type"].ToString(); }
                catch { missing.Append("Meter1Type"); }

                try { reader["Meter2Type"].ToString(); }
                catch { missing.Append("Meter2Type"); }



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

        public void RetrieveScheduledServiceByEquipmentIdV2(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref List<b_ScheduledService> results
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
                results = Database.StoredProcedure.usp_ScheduledService_RetrieveByEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Schduled service for Service order
        public void RetrieveScheduledServiceForServiceOrder(
      SqlConnection connection,
      SqlTransaction transaction,
      long callerUserInfoId,
      string callerUserName,
      ref b_ScheduledService results
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
                results = Database.StoredProcedure.usp_FleetScheduledService_RetrieveByAssetIdForServiceOrder_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_ScheduledService ProcessRowScheduledServiceForServiceOrder(SqlDataReader reader)
        {
            b_ScheduledService SchduleService = new b_ScheduledService();
            SchduleService.LoadFromScheduledServiceForServiceOrder(reader);
            return SchduleService;
        }
        public int LoadFromScheduledServiceForServiceOrder(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // ScheduledServiceId
                ScheduledServiceId = reader.GetInt64(i++);

                //Service Task Id 
                ServiceTaskId = reader.GetInt64(i++);

                //  Service Tasks Description
                ServiceTasksDescription = reader.GetString(i++);                

                Schedule = reader.GetString(i++);

                NextDue = reader.GetString(i++);

                LastCompletedstr = reader.GetString(i++);

                //TotalCount
                TotalCount = reader.GetInt32(i++);

                if (false == reader.IsDBNull(i))
                {
                    RepairReason = reader.GetString(i);
                }
                else
                {
                    RepairReason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VMRSSystem = reader.GetString(i);
                }
                else
                {
                    VMRSSystem = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VMRSAssembly = reader.GetString(i);
                }
                else
                {
                    VMRSAssembly = "";
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
                catch { missing.Append("SiteId "); }

                try { reader["ScheduledServiceId"].ToString(); }
                catch { missing.Append("ScheduledServiceId "); }

                try { reader["ServiceTasksId"].ToString(); }
                catch { missing.Append("ServiceTasksId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Schedule"].ToString(); }
                catch { missing.Append("Schedule "); }

                try { reader["NextDue"].ToString(); }
                catch { missing.Append("NextDue "); }

                try { reader["LastCompleted"].ToString(); }
                catch { missing.Append("LastCompleted "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["RepairReason"].ToString(); }
                catch { missing.Append("RepairReason "); }

                try { reader["VMRSSystem"].ToString(); }
                catch { missing.Append("VMRSSystem "); }

                try { reader["VMRSAssembly"].ToString(); }
                catch { missing.Append("VMRSAssembly "); }

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

        #region Validate for activate inctivate
        public void ValidateInactivateOrActivate(
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
                results = Database.StoredProcedure.usp_ScheduledService_ValidateByInactivateOrActivate_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #endregion
    }
}
