using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_SanitationJobSchedule
    {

        #region Properties
        public long SiteId { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }

        #endregion

        #region Insert,  UpDate,  Delete Work

        public void InsertIntoDatabase_ForSanitationJob(
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
               Database.StoredProcedure.usp_SanitationJobSchedule_CreateForSanitationJob.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void UpdateInDatabase_ForSanitationJob(
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
                Database.StoredProcedure.usp_SanitationJobSchedule_UpdateByPKForSanitationJob.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void DeleteFromDatabase_ForSanitationJob(
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
                Database.StoredProcedure.usp_SanitationJobSchedule_DeleteByPKForSanitationJob.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveAllFromDatabaseBy_SanitationJobId(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName,
           ref b_SanitationJobSchedule[] data
       )
        {
           Database.SqlClient.ProcessRow<b_SanitationJobSchedule> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_SanitationJobSchedule[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJobSchedule>(reader => { b_SanitationJobSchedule obj = new b_SanitationJobSchedule(); obj.LoadFromDatabaseForRetrive(reader); return obj; });
                results = Database.StoredProcedure.usp_SanitationJobSchedule_RetrieveAllBy_SanitationJobId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_SanitationJobSchedule[])results.ToArray(typeof(b_SanitationJobSchedule));
                }
                else
                {
                    data = new b_SanitationJobSchedule[0];
                }

                // Clear the results collection
                if (null != results)
                {
                    results.Clear();
                    results = null;
                }
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }
                processRow = null;
                results = null;
                message = String.Empty;
                callerUserInfoId = 0;
                callerUserName = String.Empty;
            }
        }


        public void RetrieveSingleBy_SanitationJobScheduleId(
         SqlConnection connection,
         SqlTransaction transaction,
         long callerUserInfoId,
         string callerUserName
     )
        {
            Database.SqlClient.ProcessRow<b_SanitationJobSchedule> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_SanitationJobSchedule>(reader => { this.LoadFromDatabaseForRetrive(reader); return this; });
                Database.StoredProcedure.usp_SanitationJobSchedule_RetrieveSingleBy_SanitationJobId.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public int LoadFromDatabaseForRetrive(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SanitationJobScheduleId column, bigint, not null
                SanitationJobScheduleId = reader.GetInt64(i++);

                // SanitationJobId column, bigint, not null
                SanitationJobId = reader.GetInt64(i++);

                // PersonnelId column, bigint, not null
                PersonnelId = reader.GetInt64(i++);

                ClientLookupId = reader.GetString(i++);

                // ScheduledStartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;

                // ScheduledHours column, decimal(10,2), not null
                ScheduledHours = reader.GetDecimal(i++);

                CreatedBy = reader.GetString(i++);

                Name = reader.GetString(i++);

                // ScheduledStartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;


                ModifyBy = reader.GetString(i++);

                // ScheduledStartDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                }
                i++;

                // UpdateIndex column, int, not null
                UpdateIndex = reader.GetInt32(i++);

                // Del column, bit, not null
                Del = reader.GetBoolean(i++);

              
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SanitationJobScheduleId"].ToString(); }
                catch { missing.Append("SanitationJobScheduleId "); }

                try { reader["SanitationJobId"].ToString(); }
                catch { missing.Append("SanitationJobId "); }

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["ScheduledHours"].ToString(); }
                catch { missing.Append("ScheduledHours "); }

                try { reader["CreatedBy"].ToString(); }
                catch { missing.Append("CreatedBy "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append("Name "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }               

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }


                try { reader["Del"].ToString(); }
                catch { missing.Append("Del "); }

              

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
