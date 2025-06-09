using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;
using Common.Constants;
using System.Xml.Linq;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the TechSpecs table.
    /// </summary>
    public partial class b_IoTEvent
    {
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public string EventNifoId { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }

        //public List<b_IoTEvent> listOfEventInfo { get; set; }

        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int CaseNo { get; set; }
        public int TotalCount { get; set; }
        public string ProcessBy_Personnel { get; set; }
        public string WOClientLookupId { get; set; }
        public string EquipClientLookupId { get; set; }        
        public string IoTDeviceClientLookupId { get; set; }
        #region V2-538
        public decimal IoTReading_Reading { get; set; }
        public string IoTDevice_SensorUnit { get; set; }
        public decimal IoTDevice_MeterInterval { get; set; }
        public decimal IoTDevice_TriggerHigh { get; set; }
        public decimal IoTDevice_TriggerLow { get; set; }
        public decimal IoTDevice_TriggerHighCrit { get; set; }
        public decimal IoTDevice_TriggerLowCrit { get; set; }
        #endregion
        public static b_IoTEvent ProcessRowForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            b_IoTEvent ClientMessage = new b_IoTEvent();

            ClientMessage.LoadFromDatabaseForRetrieveChunkSearchFromDetails(reader);
            return ClientMessage;
        }
        public int LoadFromDatabaseForRetrieveChunkSearchFromDetails(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);
                IoTEventId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    SourceType = reader.GetString(i);
                }
                else
                {
                    SourceType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    EventType = reader.GetString(i);
                }
                else
                {
                    EventType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetId = reader.GetString(i);
                }
                else
                {
                    AssetId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i);
                }
                else
                {
                    AssetName = "";
                }
                i++;
               
               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Disposition = reader.GetString(i);
                }
                else
                {
                    Disposition = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    WOClientLookupId = reader.GetString(i);
                }
                else
                {
                    WOClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FaultCode = reader.GetString(i);
                }
                else
                {
                    FaultCode = "";
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
                //IoTDeviceId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    IoTDeviceClientLookupId = reader.GetString(i);
                }
                else
                {
                    IoTDeviceClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProcessDate = reader.GetDateTime(i);
                }
                else
                {
                    ProcessDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProcessBy_Personnel = reader.GetString(i);
                }
                else
                {
                    ProcessBy_Personnel = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Comments = reader.GetString(i);
                }
                else
                {
                    Comments = "";
                }
                i++;
                TotalCount = reader.GetInt32(i);
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["IoTEventId"].ToString(); }
                catch { missing.Append("IoTEventId "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["EventType"].ToString(); }
                catch { missing.Append("EventType "); }

                try { reader["AssetId"].ToString(); }
                catch { missing.Append("AssetId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Disposition"].ToString(); }
                catch { missing.Append("Disposition "); }

                try { reader["WOClientLookupId"].ToString(); }
                catch { missing.Append("WOClientLookupId "); }

                try { reader["FaultCode"].ToString(); }
                catch { missing.Append("FaultCode "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["IoTDeviceClientLookupId"].ToString(); }
                catch { missing.Append("IoTDeviceClientLookupId "); }


                try { reader["ProcessDate"].ToString(); }
                catch { missing.Append("ProcessDate "); }

                try { reader["ProcessBy_Personnel"].ToString(); }
                catch { missing.Append("ProcessBy_Personnel "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

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

        /// <summary>
        /// Update the Retrieve ChunkSearch IoTEvent Details table record represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="clientId">long that identifies the user calling the database</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        public void RetrieveChunkSearchIotEventDetails(
        SqlConnection connection,
        SqlTransaction transaction,
        long clientId,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_IoTEvent> results
        )
        {
            SqlCommand command = null;
            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_IoTEvent_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, clientId, this);
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


        #region IoTEvent Details

        public void RetrieveByPKForeignkey(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_IoTEvent> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_IoTEvent>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                StoredProcedure.usp_IoTEvent_RetrieveByPKForeignkey_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                ClientId = reader.GetInt64(i++);
                IoTEventId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    SourceType = reader.GetString(i);
                }
                else
                {
                    SourceType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    EventType = reader.GetString(i);
                }
                else
                {
                    EventType = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    EquipClientLookupId = reader.GetString(i);
                }
                else
                {
                    EquipClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i);
                }
                else
                {
                    AssetName = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Disposition = reader.GetString(i);
                }
                else
                {
                    Disposition = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    DismissReason = reader.GetString(i);
                }
                else
                {
                    DismissReason = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    WOClientLookupId = reader.GetString(i);
                }
                else
                {
                    WOClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    FaultCode = reader.GetString(i);
                }
                else
                {
                    FaultCode = "";
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
                    IoTDeviceClientLookupId = reader.GetString(i);
                }
                else
                {
                    IoTDeviceClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProcessDate = reader.GetDateTime(i);
                }
                else
                {
                    ProcessDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProcessBy_Personnel = reader.GetString(i);
                }
                else
                {
                    ProcessBy_Personnel = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Comments = reader.GetString(i);
                }
                else
                {
                    Comments = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTReading_Reading = reader.GetDecimal(i);
                }
                else
                {
                    IoTReading_Reading = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTDevice_SensorUnit = reader.GetString(i);
                }
                else
                {
                    IoTDevice_SensorUnit = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTDevice_MeterInterval = reader.GetDecimal(i);
                }
                else
                {
                    IoTDevice_MeterInterval = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTDevice_TriggerHigh = reader.GetDecimal(i);
                }
                else
                {
                    IoTDevice_TriggerHigh = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTDevice_TriggerLow = reader.GetDecimal(i);
                }
                else
                {
                    IoTDevice_TriggerLow = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTDevice_TriggerHighCrit = reader.GetDecimal(i);
                }
                else
                {
                    IoTDevice_TriggerHighCrit = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    IoTDevice_TriggerLowCrit = reader.GetDecimal(i);
                }
                else
                {
                    IoTDevice_TriggerLowCrit = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["IoTEventId"].ToString(); }
                catch { missing.Append("IoTEventId "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["EventType"].ToString(); }
                catch { missing.Append("EventType "); }

                try { reader["AssetId"].ToString(); }
                catch { missing.Append("AssetId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Disposition"].ToString(); }
                catch { missing.Append("Disposition "); }

                try { reader["WOClientLookupId"].ToString(); }
                catch { missing.Append("WOClientLookupId "); }

                try { reader["FaultCode"].ToString(); }
                catch { missing.Append("FaultCode "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["IoTDeviceClientLookupId"].ToString(); }
                catch { missing.Append("IoTDeviceClientLookupId "); }


                try { reader["ProcessDate"].ToString(); }
                catch { missing.Append("ProcessDate "); }

                try { reader["ProcessBy_Personnel"].ToString(); }
                catch { missing.Append("ProcessBy_Personnel "); }

                try { reader["Comments"].ToString(); }
                catch { missing.Append("Comments "); }

                try { reader["IoTReading_Reading"].ToString(); }
                catch { missing.Append("IoTReading_Reading "); }

                try { reader["IoTDevice_SensorUnit"].ToString(); }
                catch { missing.Append("IoTDevice_SensorUnit "); }

                try { reader["IoTDevice_MeterInterval"].ToString(); }
                catch { missing.Append("IoTDevice_MeterInterval "); }

                try { reader["IoTDevice_TriggerHigh"].ToString(); }
                catch { missing.Append("IoTDevice_TriggerHigh "); }

                try { reader["IoTDevice_TriggerLow"].ToString(); }
                catch { missing.Append("IoTDevice_TriggerLow "); }

                try { reader["IoTDevice_TriggerHighCrit"].ToString(); }
                catch { missing.Append("IoTDevice_TriggerHighCrit "); }

                try { reader["IoTDevice_TriggerLowCrit"].ToString(); }
                catch { missing.Append("IoTDevice_TriggerLowCrit "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }
                throw new Exception(msg.ToString(), ex);
            }
        }

        #endregion IoTEvent Details


        

    }
}
