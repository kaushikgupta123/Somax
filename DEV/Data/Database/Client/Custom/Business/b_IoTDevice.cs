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
    public partial class b_IoTDevice
    {
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public string DeviceId { get; set; }
        public string AssetId { get; set; }
        public string AssetCategory { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public string AssetName { get; set; }
        public List<b_IoTDevice> listOfDevice { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int CaseNo { get; set; }       
        public int TotalCount { get; set; }
        #region V2-536
        public string SensorAlertProcedureClientLooukupId { get; set; }
        public string CriticalProcedureClientLooukupId { get; set; }
        public string CMMSMeterClientLooukupId { get; set; }
        #endregion



        public static b_IoTDevice ProcessRetrieveV2(SqlDataReader reader)
        {
            b_IoTDevice IotDevice = new b_IoTDevice();

            IotDevice.LoadFromDatabaseForIotDeviceRetrieveV2(reader);
            return IotDevice;
        }

        public int LoadFromDatabaseForIotDeviceRetrieveV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                IoTDeviceId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {

                    Name = reader.GetString(i);
                }

                else
                {
                    Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    IoTDeviceCategory = reader.GetString(i);

                }
                else
                {
                    IoTDeviceCategory = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SensorType = reader.GetString(i);
                }

                else
                {
                    SensorType = "";
                }
                i++;

                EquipmentId = reader.GetInt64(i++);

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
                    AssetId = reader.GetString(i);
                }

                else
                {
                    AssetId = "";
                }
                i++;                            
                LastReading = reader.GetDecimal(i++);
                SensorID = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    LastReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    LastReadingDate = DateTime.MinValue;
                }
                i++;
                InactiveFlag = reader.GetBoolean(i++);
                TotalCount = reader.GetInt32(i++);
            }


            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["IoTDeviceId"].ToString(); }
                catch { missing.Append("IoTDeviceId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["IoTDeviceCategory"].ToString(); }
                catch { missing.Append("IoTDeviceCategory "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["AssetId"].ToString(); }
                catch { missing.Append("AssetId "); }

                try { reader["SensorType"].ToString(); }
                catch { missing.Append("SensorType "); }
               
                try { reader["LastReading"].ToString(); }
                catch { missing.Append("LastReading "); }

                try { reader["SensorID"].ToString(); }
                catch { missing.Append("SensorID "); }


                try { reader["LastReadingDate"].ToString(); }
                catch { missing.Append("LastReadingDate "); }


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


        public void RetrieveV2Search(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref b_IoTDevice results
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

                results = Database.StoredProcedure.usp_IotDevice_RetrieveAllForSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrieveByForeignKeysFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName
      )
        {
            Database.SqlClient.ProcessRow<b_IoTDevice> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_IoTDevice>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                StoredProcedure.usp_IoTDevice_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
            int i = this.LoadFromDatabase(reader);

            try
            {
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
                    ModifyBy = reader.GetString(i);
                }

                else
                {
                    ModifyBy = "";
                }
                i++;


                ModifyDate = reader.GetDateTime(i++);


                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }

                else
                {
                    CreateBy = "";
                }
                i++;

                CreateDate = reader.GetDateTime(i++);

                if (false == reader.IsDBNull(i))
                {
                    SensorAlertProcedureClientLooukupId = reader.GetString(i);
                }
                else
                {
                    SensorAlertProcedureClientLooukupId = "";
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    CriticalProcedureClientLooukupId = reader.GetString(i);
                }
                else
                {
                    CriticalProcedureClientLooukupId = "";
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    CMMSMeterClientLooukupId = reader.GetString(i);
                }
                else
                {
                    CMMSMeterClientLooukupId = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader[" AssetId"].ToString(); }
                catch { missing.Append(" AssetId "); }

                try { reader[" AssetName"].ToString(); }
                catch { missing.Append(" AssetName "); }

                try { reader[" ModifyBy"].ToString(); }
                catch { missing.Append(" ModifyBy "); }

                try { reader[" ModifyDate"].ToString(); }
                catch { missing.Append(" ModifyDate "); }

                try { reader[" CreateBy"].ToString(); }
                catch { missing.Append(" CreateBy "); }

                try { reader[" CreateDate"].ToString(); }
                catch { missing.Append(" CreateDate "); }
                
                try { reader[" SensorAlertProcedureClientLooukupId"].ToString(); }
                catch { missing.Append(" SensorAlertProcedureClientLooukupId "); }
                
                try { reader[" CriticalAlertProcedureClientLooukupId"].ToString(); }
                catch { missing.Append(" CriticalAlertProcedureClientLooukupId "); }
                
                try { reader[" CMMSMeterClientLooukupId"].ToString(); }
                catch { missing.Append(" CMMSMeterClientLooukupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }


        public int LoadFromDB(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);
                // ClientLookupId
                ClientLookupId = reader.GetString(i++);
                // Name
                Name = reader.GetString(i++);
                // IoTDeviceCategory
                IoTDeviceCategory = reader.GetString(i++);
                // SensorType
                SensorType = reader.GetString(i++);
                // EquipmentId
                EquipmentId = reader.GetInt64(i++);
                // AssetName
                if (false == reader.IsDBNull(i))
                {
                    AssetName = reader.GetString(i++);
                }
                else
                {
                    AssetName = "";
                }
                // AssetId
                if (false == reader.IsDBNull(i))
                {
                    AssetId = reader.GetString(i++);
                }
                else
                {
                    AssetId = "";
                }
                // LastReading
                LastReading = reader.GetDecimal(i++);
                
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["IoTDeviceCategory"].ToString(); }
                catch { missing.Append("IoTDeviceCategory "); }

                try { reader["SensorType"].ToString(); }
                catch { missing.Append("SensorType "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["AssetId"].ToString(); }
                catch { missing.Append("AssetId "); }

                try { reader["LastReading"].ToString(); }
                catch { missing.Append("LastReading "); }

                

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


        public void RetrieveIotDeviceForPrint(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_IoTDevice> results
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

                results = Database.StoredProcedure.usp_IotDevice_RetrieveAllForPrint_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region V2-537
        public void RetrieveChunkSearchByEqipmentId(
 SqlConnection connection,
 SqlTransaction transaction,
 long callerUserInfoId,
 string callerUserName,
 ref b_IoTDevice results
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

                results = Database.StoredProcedure.usp_IoTDevice_RetrieveChunkSearchByEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_IoTDevice ProcessRetrieveChunkSearchByEqipmentId(SqlDataReader reader)
        {
            b_IoTDevice IotDevice = new b_IoTDevice();

            IotDevice.LoadFromDatabaseForIotDeviceRetrieveChunkSearchByEqipmentId(reader);
            return IotDevice;
        }

        public int LoadFromDatabaseForIotDeviceRetrieveChunkSearchByEqipmentId(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                ClientId = reader.GetInt64(i++);

                IoTDeviceId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {

                    Name = reader.GetString(i);
                }
                else
                {
                    Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SensorType = reader.GetString(i);
                }
                else
                {
                    SensorType = "";
                }
                i++;

                LastReading = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    SensorUnit = reader.GetString(i);
                }
                else
                {
                    SensorUnit = "";
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    LastReadingDate = reader.GetDateTime(i);
                }
                else
                {
                    LastReadingDate = DateTime.MinValue;
                }
                i++;

                InactiveFlag = reader.GetBoolean(i++);

                TotalCount = reader.GetInt32(i++);
            }


            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["IoTDeviceId"].ToString(); }
                catch { missing.Append("IoTDeviceId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["SensorType"].ToString(); }
                catch { missing.Append("SensorType "); }

                try { reader["LastReading"].ToString(); }
                catch { missing.Append("LastReading "); }

                try { reader["SensorUnit"].ToString(); }
                catch { missing.Append("SensorUnit "); }
                
                try { reader["LastReadingDate"].ToString(); }
                catch { missing.Append("LastReadingDate "); }

                try { reader["InactiveFlag"].ToString(); }
                catch { missing.Append("InactiveFlag "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["SensorID"].ToString(); }
                catch { missing.Append("SensorID "); }


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

        #region V2-536
        public void ValidateByClientLookupIdFromDatabase_V2(
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
                results = StoredProcedure.usp_IoTDevice_ValidateByClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void ValidateAdd_V2(
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
                results = StoredProcedure.usp_IoTDevice_ValidateAdd_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
