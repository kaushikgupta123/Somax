/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2013-2017 by SOMAX Inc.. All rights reserved. 
***************************************************************************************************
* Change Log
***************************************************************************************************
* Date        JIRA Item Entry Person    Description
* =========== ========= =============== ===========================================================
* 2017-Aug-08 SOM-      Roger Lawton    Removed LoadFromDatabaseForEquipment_Sensor_Xref method
***************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the SanOnDemandMaster table.
    /// </summary>
    public partial class b_Equipment_Sensor_Xref
    {
        #region Properties
        public string SensorAlertProcedureClientLookupId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string AssignedTo_Name { get; set; }

        #endregion

        //SOM-1351
        public void RetriveBySensorIdFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Equipment_Sensor_Xref> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment_Sensor_Xref>(reader => { this.LoadFromDatabase(reader); return this; });
                Database.StoredProcedure.usp_Equipment_Sensor_Xref_RetrieveBySensorId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public void RetrieveAllEquipmentSensorFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_Equipment_Sensor_Xref> data)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment_Sensor_Xref> results = null;
            data = new List<b_Equipment_Sensor_Xref>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_Sensor_Xref_RetrieveSensorList.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment_Sensor_Xref>();
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

        public void RetriveByExrefIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_Equipment_Sensor_Xref> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Equipment_Sensor_Xref>(reader => { this.LoadFromDatabaseForEquipmentSensor(reader); return this; });
                Database.StoredProcedure.usp_Equipment_Sensor_Xref_RetrieveByExrefId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        //SOM-1351
        public void RetrieveByEquipmentId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_Equipment_Sensor_Xref> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_Equipment_Sensor_Xref> results = null;
            data = new List<b_Equipment_Sensor_Xref>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_Equipment_Sensor_Xref_RetrieveByEquipmentId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_Equipment_Sensor_Xref>();
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
        public static b_Equipment_Sensor_Xref ProcessRowForEquipmentId(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment_Sensor_Xref obj = new b_Equipment_Sensor_Xref();
            int i = 0;
            try
            {

                // Load the object from the database
                i = obj.LoadFromDatabase(reader);
                obj.SensorAlertProcedureClientLookupId = reader.GetString(i++);
                obj.AssignedTo_Name = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["SensorAlertProcedureClientLookupId"].ToString(); }
                catch { missing.Append("SensorAlertProcedureClientLookupId "); }

                try { reader["AssignedTo_Name"].ToString(); }
                catch { missing.Append("AssignedTo_Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }
            // Return result
            return obj;
        }
        public static object ProcessRowForEquipmentSensor(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment_Sensor_Xref obj = new b_Equipment_Sensor_Xref();

            // Load the object from the database
            obj.LoadFromDatabaseForEquipmentSensor(reader);

            // Return result
            return (object)obj;
        }
        public int LoadFromDatabaseForEquipmentSensor(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // Equipment_Sensor_XrefId column, bigint, not null
                Equipment_Sensor_XrefId = reader.GetInt64(i++);

                // SensorId column, int, not null
                SensorId = reader.GetInt32(i++);

                // SensorName column, nvarchar(63), not null
                SensorName = reader.GetString(i++);

                // LastReading column, decimal(17,2), not null
                LastReading = reader.GetDecimal(i++);

                // EquipmentClientLookupId column, 
                EquipmentClientLookupId = reader.GetString(i++);

                // EquipmentName column,
                EquipmentName = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["Equipment_Sensor_XrefId"].ToString(); }
                catch { missing.Append("Equipment_Sensor_XrefId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SensorId"].ToString(); }
                catch { missing.Append("SensorId "); }

                try { reader["SensorName"].ToString(); }
                catch { missing.Append("SensorName "); }

                try { reader["LastReading"].ToString(); }
                catch { missing.Append("LastReading "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["EquipmentName"].ToString(); }
                catch { missing.Append("EquipmentName "); }



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
        public static object ProcessRowForSanOnDemandList(SqlDataReader reader)
        {
            // Create instance of object
            b_Equipment_Sensor_Xref obj = new b_Equipment_Sensor_Xref();

            // Load the object from the database
            obj.LoadFromDBSanOnDemandList(reader);

            // Return result
            return (object)obj;
        }
        public int LoadFromDBSanOnDemandList(SqlDataReader reader)
        {
            int i = 0;
            try
            {



                // PrevMaintMasterId column
                SensorAlertProcedureId = reader.GetInt64(i++);

                // ClientLookupId column
                ClientLookupId = reader.GetString(i++);


                // Description column
                Description = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["Equipment_Sensor_XrefId"].ToString(); }
                catch { missing.Append("Equipment_Sensor_XrefId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["SensorId"].ToString(); }
                catch { missing.Append("SensorId "); }


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
