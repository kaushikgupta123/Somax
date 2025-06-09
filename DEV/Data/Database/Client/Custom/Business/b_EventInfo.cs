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
    public partial class b_EventInfo
    {
        public int CustomQueryDisplayId { get; set; }
        public DateTime CreateDate { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public string WOClientLookupId { get; set; }
        public string EquipClientLookupId { get; set; }        
        public int TotalOpenCount { get; set; }
        public int OpenAssetCount { get; set; }
        public int MonitoredAssetCount { get; set; }
        public int QueryId { get; set; }
        public int EventCount { get; set; }
        public string IotClientlookupId { get; set; }

        public List<b_EventInfo> listOfEventInfo { get; set; }

        public void RetrieveAllForSearch(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_EventInfo> results
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

                results = Database.StoredProcedure.usp_EventInfo_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrieveByPKForeignkey(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref List<b_EventInfo> results
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

                results = Database.StoredProcedure.usp_EventInfo_RetrieveByPKForeignkey.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public int RetrieveStatusCount(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName
)
        {
            SqlCommand command = null;
            string message = String.Empty;
            int results = 0;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_EventInfo_RetrieveByEventInfoStatus.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
            return results;
        }
        public static b_EventInfo ProcessRowForEventInfoRetriveAllForSearch(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForEventInfoRetriveAllForSearch(reader);
            return eventInfo;
        }
       

        public int LoadFromDatabaseForEventInfoRetriveAllForSearch(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {
                

                // ProcessBy_Personnel  
                if (false == reader.IsDBNull(i))
                {
                    ProcessBy_Personnel = reader.GetString(i++);
                }
                else
                {
                    ProcessBy_Personnel = ""; i++;
                }

                // Create Date
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // WOClientLookupId  
                if (false == reader.IsDBNull(i))
                {
                    WOClientLookupId = reader.GetString(i++);
                }
                else
                {
                    WOClientLookupId = ""; i++;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EventInfoId"].ToString(); }
                catch { missing.Append("EventInfoId "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["EventType"].ToString(); }
                catch { missing.Append("EventType "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Disposition"].ToString(); }
                catch { missing.Append("Disposition "); }

                try { reader["FaultCode"].ToString(); }
                catch { missing.Append("FaultCode "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["SensorId"].ToString(); }
                catch { missing.Append("SensorId "); }

                try { reader["ProcessDate"].ToString(); }
                catch { missing.Append("ProcessDate "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["ProcessBy_PersonnelId"].ToString(); }
                catch { missing.Append("ProcessBy_PersonnelId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ProcessBy_Personnel"].ToString(); }
                catch { missing.Append("ProcessBy_Personnel "); }                

                try { reader["WOClientLookupId"].ToString(); }
                catch { missing.Append("WOClientLookupId "); }
                

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

        public static b_EventInfo ProcessRowForEventInfoRetriveByPKForeignkey(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForEventInfoRetriveByPKForeignkey(reader);
            return eventInfo;
        }

        public int LoadFromDatabaseForEventInfoRetriveByPKForeignkey(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {
                // ProcessBy_Personnel  
                if (false == reader.IsDBNull(i))
                {
                    ProcessBy_Personnel = reader.GetString(i++);
                }
                else
                {
                    ProcessBy_Personnel = ""; i++;
                }

                // Create Date
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                // WOClientLookupId  
                if (false == reader.IsDBNull(i))
                {
                    WOClientLookupId = reader.GetString(i++);
                }
                else
                {
                    WOClientLookupId = ""; i++;
                }

                // EquipClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    EquipClientLookupId = reader.GetString(i++);
                }
                else
                {
                    EquipClientLookupId = ""; i++;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EventInfoId"].ToString(); }
                catch { missing.Append("EventInfoId "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["EventType"].ToString(); }
                catch { missing.Append("EventType "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Disposition"].ToString(); }
                catch { missing.Append("Disposition "); }

                try { reader["FaultCode"].ToString(); }
                catch { missing.Append("FaultCode "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["SensorId"].ToString(); }
                catch { missing.Append("SensorId "); }

                try { reader["ProcessDate"].ToString(); }
                catch { missing.Append("ProcessDate "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["ProcessBy_PersonnelId"].ToString(); }
                catch { missing.Append("ProcessBy_PersonnelId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ProcessBy_Personnel"].ToString(); }
                catch { missing.Append("ProcessBy_Personnel "); }

                try { reader["WOClientLookupId"].ToString(); }
                catch { missing.Append("WOClientLookupId "); }

                try { reader["EquipClientLookupId"].ToString(); }
                catch { missing.Append("EquipClientLookupId "); }


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

        public static b_EventInfo ProcessRowForAPMCountHozBar(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForAPMCountHozBar(reader);
            return eventInfo;
        }
        public int LoadFromDatabaseForAPMCountHozBar(SqlDataReader reader)
        {
            //int i = LoadFromDatabase(reader);
            int i = 0;
            try
            {
                //TotalOpenCount
                if (false == reader.IsDBNull(i))
                {
                    TotalOpenCount = reader.GetInt32(i++);
                }
                else
                {
                    TotalOpenCount = 0; i++;
                }
                //OpenAssetCount
                if (false == reader.IsDBNull(i))
                {
                    OpenAssetCount = reader.GetInt32(i++);
                }
                else
                {
                    OpenAssetCount = 0; i++;
                }
                //MonitoredAssetCount
                if (false == reader.IsDBNull(i))
                {
                    MonitoredAssetCount = reader.GetInt32(i++);
                }
                else
                {
                    MonitoredAssetCount = 0; i++;
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["TotalOpenCount"].ToString(); }
                catch { missing.Append("TotalOpenCount "); }

                try { reader["OpenAssetCount"].ToString(); }
                catch { missing.Append("OpenAssetCount "); }

                try { reader["MonitoredAssetCount"].ToString(); }
                catch { missing.Append("MonitoredAssetCount "); }


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

        public void RetrieveForAPMBarChart(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_EventInfo> results
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

                results = Database.StoredProcedure.usp_EventInfo_RetrieveAPMBarChart.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_EventInfo ProcessRowForAPMBarChart(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForAPMBarChart(reader);
            return eventInfo;
        }
        public int LoadFromDatabaseForAPMBarChart(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EventCount  
                if (false == reader.IsDBNull(i))
                {
                    EventCount = reader.GetInt32(i++);
                }
                else
                {
                    EventCount = 0; i++;
                }

                // FaultCode
                if (false == reader.IsDBNull(i))
                {
                    FaultCode = reader.GetString(i);
                }
                else
                {
                    FaultCode = "";
                }
                i++;
               
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EventCount"].ToString(); }
                catch { missing.Append("EventCount "); }

                try { reader["FaultCode"].ToString(); }
                catch { missing.Append("FaultCode "); }

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

        public void RetrieveForAPMDoughChart(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_EventInfo> results
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

                results = Database.StoredProcedure.usp_EventInfo_RetrieveAPMDoughChart.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_EventInfo ProcessRowForAPMDoughChart(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForAPMDoughChart(reader);
            return eventInfo;
        }
        public int LoadFromDatabaseForAPMDoughChart(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // EventCount  
                if (false == reader.IsDBNull(i))
                {
                    EventCount = reader.GetInt32(i++);
                }
                else
                {
                    EventCount = 0; i++;
                }

                // FaultCode
                if (false == reader.IsDBNull(i))
                {
                    Disposition = reader.GetString(i);
                }
                else
                {
                    Disposition = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["EventCount"].ToString(); }
                catch { missing.Append("EventCount "); }

                try { reader["FaultCode"].ToString(); }
                catch { missing.Append("FaultCode "); }

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

        public void RetrieveAPMCountHozBarV2(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref List<b_EventInfo> results
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

                results = Database.StoredProcedure.usp_EventInfo_RetrieveAPMCountHozBarV2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_EventInfo ProcessRowAPMCountHozBarV2(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForAPMCountHozBarV2(reader);
            return eventInfo;
        }
        public int LoadFromDatabaseForAPMCountHozBarV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // TotalOpenCount  
                if (false == reader.IsDBNull(i))
                {
                    TotalOpenCount = reader.GetInt32(i++);
                }
                else
                {
                    TotalOpenCount = 0; i++;
                }

                // OpenAssetCount  
                if (false == reader.IsDBNull(i))
                {
                    OpenAssetCount = reader.GetInt32(i++);
                }
                else
                {
                    OpenAssetCount = 0; i++;
                }
                // MonitoredAssetCount  
                if (false == reader.IsDBNull(i))
                {
                    MonitoredAssetCount = reader.GetInt32(i++);
                }
                else
                {
                    MonitoredAssetCount = 0; i++;
                }

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["TotalOpenCount"].ToString(); }
                catch { missing.Append("TotalOpenCount "); }

                try { reader["OpenAssetCount"].ToString(); }
                catch { missing.Append("OpenAssetCount "); }

                try { reader["MonitoredAssetCount"].ToString(); }
                catch { missing.Append("MonitoredAssetCount "); }

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

        public static b_EventInfo ProcessRowForEventInfo_RetrieveForIoT(SqlDataReader reader)
        {
            b_EventInfo eventInfo = new b_EventInfo();
            eventInfo.LoadFromDatabaseForEventInfo_RetrieveForIoT(reader);
            return eventInfo;
        }

        public static b_IoTDevice ProcessRetrieveV2(SqlDataReader reader)
        {
            b_IoTDevice IotDevice = new b_IoTDevice();

            IotDevice.LoadFromDatabaseForIotDeviceRetrieveV2(reader);
            return IotDevice;
        }

        public int LoadFromDatabaseForEventInfo_RetrieveForIoT(SqlDataReader reader)
        {
            int i = LoadFromDatabase(reader);
            try
            {
                
                // ProcessBy_Personnel  
                if (false == reader.IsDBNull(i))
                {
                    ProcessBy_Personnel = reader.GetString(i++);
                }
                else
                {
                    ProcessBy_Personnel = ""; i++;
                }

                // Create Date
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
             
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                

                try { reader["ProcessBy_Personnel"].ToString(); }
                catch { missing.Append("ProcessBy_Personnel "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }               


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

        public void RetrieveForIoT(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_EventInfo results
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

                results = Database.StoredProcedure.usp_EventInfo_RetrieveForIoT.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
