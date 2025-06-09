using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Database.SqlClient;

namespace Database.Business
{
    public partial class b_BBUKPI
    {
        #region Properties
        public List<b_BBUKPI> listOfBBUKPI { get; set; }
        public string Sites { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string SubmitStartDateVw { get; set; }
        public string SubmitEndDateVw { get; set; }
        public string SiteName { get; set; }
        //public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public DateTime CreateDate { get; set; }
        public string SubmitBy_Name { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        #region V2-909
        public string YearWeek { get; set; }
        public long Id { get; set; }
        public string YearWeekLists { get; set; }
        #endregion
        #endregion

        #region EnterPrise
        public void RetrieveChunkEnterpriseSearch(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref b_BBUKPI results
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

                results = Database.StoredProcedure.usp_BBUKPI_RetrieveChunkForEnterpriseSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_BBUKPI ProcessRetrieveChunkEnterpriseSearch(SqlDataReader reader)
        {
            b_BBUKPI bBUKPI = new b_BBUKPI();

            bBUKPI.LoadFromDatabaseForChunkEnterpriseSearch(reader);
            return bBUKPI;
        }

        public int LoadFromDatabaseForChunkEnterpriseSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // BBUKPIId
                BBUKPIId = reader.GetInt64(i++);

                // Week
                if (false == reader.IsDBNull(i))
                {
                    Week = reader.GetString(i);
                }
                else
                {
                    Week = "";
                }
                i++;

                // Year
                if (false == reader.IsDBNull(i))
                {
                    Year = reader.GetString(i);
                }
                else
                {
                    Year = "";
                }
                i++;

                // SiteName
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;

                // Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;

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

                // PMWOCompleted
                if (false == reader.IsDBNull(i))
                {
                    PMWOCompleted = reader.GetDecimal(i);
                }
                else
                {
                    PMWOCompleted = 0;
                }
                i++;

                // WOBacklogCount
                if (false == reader.IsDBNull(i))
                {
                    WOBacklogCount = reader.GetInt32(i);
                }
                else
                {
                    WOBacklogCount = 0;
                }
                i++;
                // Submit Date
                if (false == reader.IsDBNull(i))
                {
                    SubmitDate = reader.GetDateTime(i);
                }
                else
                {
                    SubmitDate = DateTime.MinValue;
                }
                i++;

                //*****V2-909***
                // Week Start
                if (false == reader.IsDBNull(i))
                {
                    WeekStart = reader.GetDateTime(i);
                }
                else
                {
                    WeekStart = DateTime.MinValue;
                }
                i++;
                // Week End
                if (false == reader.IsDBNull(i))
                {
                    WeekEnd = reader.GetDateTime(i);
                }
                else
                {
                    WeekEnd = DateTime.MinValue;
                }
                i++;
                // Cycle Count Accuracy 
                if (false == reader.IsDBNull(i))
                {
                    PhyInvAccuracy = reader.GetDecimal(i);
                }
                else
                {
                    PhyInvAccuracy = 0;
                }
                i++;


                #region HIDDEN FIELD
                //[PMFollowUpComp][int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    PMFollowUpComp = reader.GetInt32(i);
                }
                else
                {
                    PMFollowUpComp = 0;
                }
                i++;

                //[ActiveMechUsers] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    ActiveMechUsers = reader.GetInt32(i);
                }
                else
                {
                    ActiveMechUsers = 0;
                }
                i++;

                //[RCACount] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    RCACount = reader.GetInt32(i);
                }
                else
                {
                    RCACount = 0;
                }
                i++;

                //[TTRCount] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    TTRCount = reader.GetInt32(i);
                }
                else
                {
                    TTRCount = 0;
                }
                i++;

                //[InvValueOverMax] [decimal](18, 2)NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    InvValueOverMax = reader.GetDecimal(i);
                }
                else
                {
                    InvValueOverMax = 0;
                }
                i++;

                //[CycleCountProgress] [decimal](6, 3)NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    CycleCountProgress = reader.GetDecimal(i);
                }
                else
                {
                    CycleCountProgress = 0;
                }
                i++;

                //[EVTrainingHrs] [decimal](8, 2)NOT NULL
                if (false == reader.IsDBNull(i))
                {
                    EVTrainingHrs = reader.GetDecimal(i);
                }
                else
                {
                    EVTrainingHrs = 0;
                }
                i++;
                #endregion
                //*********

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["BBUKPIId"].ToString(); }
                catch { missing.Append("BBUKPIId "); }

                try { reader["Week"].ToString(); }
                catch { missing.Append("Week "); }

                try { reader["Year"].ToString(); }
                catch { missing.Append("Year "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["PMWOCompleted"].ToString(); }
                catch { missing.Append("PMWOCompleted "); }

                try { reader["WOBacklogCount"].ToString(); }
                catch { missing.Append("WOBacklogCount "); }

                try { reader["SubmitDate"].ToString(); }
                catch { missing.Append("SubmitDate "); }
                //v2-909**
                try { reader["WeekStart"].ToString(); }
                catch { missing.Append("WeekStart "); }

                try { reader["WeekEnd"].ToString(); }
                catch { missing.Append("WeekEnd "); }

                try { reader["PhyInvAccuracy"].ToString(); }
                catch { missing.Append("PhyInvAccuracy "); }

                //***HIDDEN FIELD
                try { reader["PMFollowUpComp"].ToString(); }
                catch { missing.Append("PMFollowUpComp "); }
                try { reader["ActiveMechUsers]"].ToString(); }
                catch { missing.Append("ActiveMechUsers "); }
                try { reader["RCACount]"].ToString(); }
                catch { missing.Append("RCACount "); }
                try { reader["TTRCount]"].ToString(); }
                catch { missing.Append("TTRCount "); }
                try { reader["InvValueOverMax]"].ToString(); }
                catch { missing.Append("InvValueOverMax "); }
                try { reader["CycleCountProgress]"].ToString(); }
                catch { missing.Append("CycleCountProgress "); }
                try { reader["EVTrainingHrs]"].ToString(); }
                catch { missing.Append("EVTrainingHrs "); }
                //***
                //****
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

        public void RetrieveForEnterpriseSiteFilter(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref List<b_BBUKPI> results
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
                command.CommandTimeout = 60;  // RKL for local machine

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_BBUKPI_RetrieveForEnterpriseSiteFilter_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_BBUKPI ProcessRetrieveForEnterpriseSiteFilter(SqlDataReader reader)
        {
            b_BBUKPI bBUKPI = new b_BBUKPI();

            bBUKPI.LoadFromDatabaseForEnterpriseSiteFilter(reader);
            return bBUKPI;
        }

        public int LoadFromDatabaseForEnterpriseSiteFilter(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // SiteName
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
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

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

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
        public void BBUKPIRetrieveEnterpriseDetailsByBBUKPIId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_BBUKPI> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_BBUKPI>(reader => { this.LoadFromDatabaseEnterpriseDetailsByBBUKPIId_V2(reader); return this; });
                Database.StoredProcedure.usp_BBUKPI_RetrieveEnterpriseDetailsByClientIdAndBBUKPIId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void LoadFromDatabaseEnterpriseDetailsByBBUKPIId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);
                BBUKPIId = reader.GetInt64(i++);
                SiteId = reader.GetInt64(i++);
                //SiteName = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;
                //PMWOCompleted = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    PMWOCompleted = reader.GetDecimal(i);
                }
                else
                {
                    PMWOCompleted = 0;
                }
                i++;
                //WOBacklogCount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    WOBacklogCount = reader.GetInt32(i);
                }
                else
                {
                    WOBacklogCount = 0;
                }
                i++;
                //RCACount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    RCACount = reader.GetInt32(i);
                }
                else
                {
                    RCACount = 0;
                }
                i++;
                //TTRCount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    TTRCount = reader.GetInt32(i);
                }
                else
                {
                    TTRCount = 0;
                }
                i++;
                //InvValueOverMax = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    InvValueOverMax = reader.GetDecimal(i);
                }
                else
                {
                    InvValueOverMax = 0;
                }
                i++;
                //PhyInvAccuracy = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    PhyInvAccuracy = reader.GetDecimal(i);
                }
                else
                {
                    PhyInvAccuracy = 0;
                }
                i++;
                //EVTrainingHrs = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    EVTrainingHrs = reader.GetDecimal(i);
                }
                else
                {
                    EVTrainingHrs = 0;
                }
                i++;
                // DownDaySched = reader.GetBoolean(i++);
                if (false == reader.IsDBNull(i))
                {
                    DownDaySched = reader.GetBoolean(i);
                }
                else
                {
                    DownDaySched = false;
                }
                i++;
                //OptPMPlansCompleted = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    OptPMPlansCompleted = reader.GetInt32(i);
                }
                else
                {
                    OptPMPlansCompleted = 0;
                }
                i++;
                //OptPMPlansAdopted = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    OptPMPlansAdopted = reader.GetInt32(i);
                }
                else
                {
                    OptPMPlansAdopted = 0;
                }
                i++;
                //MLT = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    MLT = reader.GetDecimal(i);
                }
                else
                {
                    MLT = 0;
                }
                i++;
                //TrainingPlanImp = reader.GetBoolean(i++);
                if (false == reader.IsDBNull(i))
                {
                    TrainingPlanImp = reader.GetBoolean(i);
                }
                else
                {
                    TrainingPlanImp = false;
                }
                i++;
                //SubmitDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    SubmitDate = reader.GetDateTime(i);
                }
                else
                {
                    SubmitDate = DateTime.MinValue;
                }
                i++;
                //SubmitBy_PersonnelId = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    SubmitBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    SubmitBy_PersonnelId = 0;
                }
                i++;
                //SubmitBy_Name = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    SubmitBy_Name = reader.GetString(i);
                }
                else
                {
                    SubmitBy_Name = "";
                }
                i++;
                //Status = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                //Week = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Week = reader.GetString(i);
                }
                else
                {
                    Week = "";
                }
                i++;
                //Year = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Year = reader.GetString(i);
                }
                else
                {
                    Year = "";
                }
                i++;
                //CreateBy = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;
                //CreateDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                //ModifyBy = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
                //ModifyDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                }
                i++;
                //****V2-909**
                //[PMFollowUpComp][int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    PMFollowUpComp = reader.GetInt32(i);
                }
                else
                {
                    PMFollowUpComp = 0;
                }
                i++;

                //[ActiveMechUsers] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    ActiveMechUsers = reader.GetInt32(i);
                }
                else
                {
                    ActiveMechUsers = 0;
                }
                i++;

                //[CycleCountProgress] [decimal](6, 3)NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    CycleCountProgress = reader.GetDecimal(i);
                }
                else
                {
                    CycleCountProgress = 0;
                }
                i++;

                // [WeekStart] datetime2,

                if (false == reader.IsDBNull(i))
                {
                    WeekStart = reader.GetDateTime(i);
                }
                else
                {
                    WeekStart = DateTime.MinValue;
                }
                i++;

                //[WeekEnd] datetime2
                if (false == reader.IsDBNull(i))
                {
                    WeekEnd = reader.GetDateTime(i);
                }
                else
                {
                    WeekEnd = DateTime.MinValue;
                }
                i++;
                //****
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["BBUKPIId"].ToString(); }
                catch { missing.Append("BBUKPIId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["PMWOCompleted"].ToString(); }
                catch { missing.Append("PMWOCompleted "); }

                try { reader["WOBacklogCount"].ToString(); }
                catch { missing.Append("WOBacklogCount "); }

                try { reader["RCACount"].ToString(); }
                catch { missing.Append("RCACount "); }

                try { reader["TTRCount"].ToString(); }
                catch { missing.Append("TTRCount "); }

                try { reader["InvValueOverMax"].ToString(); }
                catch { missing.Append("InvValueOverMax "); }

                try { reader["PhyInvAccuracy"].ToString(); }
                catch { missing.Append("PhyInvAccuracy "); }

                try { reader["EVTrainingHrs"].ToString(); }
                catch { missing.Append("EVTrainingHrs "); }

                try { reader["DownDaySched"].ToString(); }
                catch { missing.Append("DownDaySched "); }

                try { reader["OptPMPlansCompleted"].ToString(); }
                catch { missing.Append("OptPMPlansCompleted "); }

                try { reader["MLT"].ToString(); }
                catch { missing.Append("MLT "); }

                try { reader["TrainingPlanImp"].ToString(); }
                catch { missing.Append("TrainingPlanImp "); }

                try { reader["SubmitDate"].ToString(); }
                catch { missing.Append("SubmitDate "); }

                try { reader["SubmitBy_Name"].ToString(); }
                catch { missing.Append("SubmitBy_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Week"].ToString(); }
                catch { missing.Append("Week "); }

                try { reader["Year"].ToString(); }
                catch { missing.Append("Year "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }
                //***V2-909
                try { reader["PMFollowUpComp"].ToString(); }
                catch { missing.Append("PMFollowUpComp "); }

                try { reader["ActiveMechUsers"].ToString(); }
                catch { missing.Append("ActiveMechUsers "); }

                try { reader["CycleCountProgress"].ToString(); }
                catch { missing.Append("CycleCountProgress "); }

                try { reader["WeekStart"].ToString(); }
                catch { missing.Append("WeekStart "); }

                try { reader["WeekEnd"].ToString(); }
                catch { missing.Append("WeekEnd "); }


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

        #region Site
        public void RetrieveChunkSiteSearch(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref b_BBUKPI results
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

                results = Database.StoredProcedure.usp_BBUKPI_RetrieveChunkForSiteSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_BBUKPI ProcessRetrieveChunkSiteSearch(SqlDataReader reader)
        {
            b_BBUKPI bBUKPI = new b_BBUKPI();

            bBUKPI.LoadFromDatabaseForChunkSiteSearch(reader);
            return bBUKPI;
        }

        public int LoadFromDatabaseForChunkSiteSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // BBUKPIId
                BBUKPIId = reader.GetInt64(i++);

                // Week
                if (false == reader.IsDBNull(i))
                {
                    Week = reader.GetString(i);
                }
                else
                {
                    Week = "";
                }
                i++;

                // Year
                if (false == reader.IsDBNull(i))
                {
                    Year = reader.GetString(i);
                }
                else
                {
                    Year = "";
                }
                i++;

                // SiteName
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;

                // Status
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;

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

                // PMWOCompleted
                if (false == reader.IsDBNull(i))
                {
                    PMWOCompleted = reader.GetDecimal(i);
                }
                else
                {
                    PMWOCompleted = 0;
                }
                i++;

                // WOBacklogCount
                if (false == reader.IsDBNull(i))
                {
                    WOBacklogCount = reader.GetInt32(i);
                }
                else
                {
                    WOBacklogCount = 0;
                }
                i++;
                // Submit Date
                if (false == reader.IsDBNull(i))
                {
                    SubmitDate = reader.GetDateTime(i);
                }
                else
                {
                    SubmitDate = DateTime.MinValue;
                }
                i++;
                //*****V2-909***
                // Week Start
                if (false == reader.IsDBNull(i))
                {
                    WeekStart = reader.GetDateTime(i);
                }
                else
                {
                    WeekStart = DateTime.MinValue;
                }
                i++;
                // Week End
                if (false == reader.IsDBNull(i))
                {
                    WeekEnd = reader.GetDateTime(i);
                }
                else
                {
                    WeekEnd = DateTime.MinValue;
                }
                i++;
                // Cycle Count Accuracy 
                if (false == reader.IsDBNull(i))
                {
                    PhyInvAccuracy = reader.GetDecimal(i);
                }
                else
                {
                    PhyInvAccuracy = 0;
                }
                i++;


                #region HIDDEN FIELD
                //[PMFollowUpComp][int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    PMFollowUpComp = reader.GetInt32(i);
                }
                else
                {
                    PMFollowUpComp = 0;
                }
                i++;

                //[ActiveMechUsers] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    ActiveMechUsers = reader.GetInt32(i);
                }
                else
                {
                    ActiveMechUsers = 0;
                }
                i++;

                //[RCACount] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    RCACount = reader.GetInt32(i);
                }
                else
                {
                    RCACount = 0;
                }
                i++;

                //[TTRCount] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    TTRCount = reader.GetInt32(i);
                }
                else
                {
                    TTRCount = 0;
                }
                i++;

                //[InvValueOverMax] [decimal](18, 2)NOT NULL,
                    if (false == reader.IsDBNull(i))
                {
                    InvValueOverMax = reader.GetDecimal(i);
                }
                else
                {
                    InvValueOverMax = 0;
                }
                i++;

                //[CycleCountProgress] [decimal](6, 3)NOT NULL,
                    if (false == reader.IsDBNull(i))
                {
                    CycleCountProgress = reader.GetDecimal(i);
                }
                else
                {
                    CycleCountProgress = 0;
                }
                i++;

                //[EVTrainingHrs] [decimal](8, 2)NOT NULL
                    if (false == reader.IsDBNull(i))
                {
                    EVTrainingHrs = reader.GetDecimal(i);
                }
                else
                {
                    EVTrainingHrs = 0;
                }
                i++;
                #endregion
                //*********

                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["BBUKPIId"].ToString(); }
                catch { missing.Append("BBUKPIId "); }

                try { reader["Week"].ToString(); }
                catch { missing.Append("Week "); }

                try { reader["Year"].ToString(); }
                catch { missing.Append("Year "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["PMWOCompleted"].ToString(); }
                catch { missing.Append("PMWOCompleted "); }

                try { reader["WOBacklogCount"].ToString(); }
                catch { missing.Append("WOBacklogCount "); }

                try { reader["SubmitDate"].ToString(); }
                catch { missing.Append("SubmitDate "); }

                try { reader["WeekStart"].ToString(); }
                catch { missing.Append("WeekStart "); }

                try { reader["WeekEnd"].ToString(); }
                catch { missing.Append("WeekEnd "); }

                try { reader["PhyInvAccuracy"].ToString(); }
                catch { missing.Append("PhyInvAccuracy "); }

                //***HIDDEN FIELD
                try { reader["PMFollowUpComp"].ToString(); }
                catch { missing.Append("PMFollowUpComp "); }
                try { reader["ActiveMechUsers]"].ToString(); }
                catch { missing.Append("ActiveMechUsers "); }
                try { reader["RCACount]"].ToString(); }
                catch { missing.Append("RCACount "); }
                try { reader["TTRCount]"].ToString(); }
                catch { missing.Append("TTRCount "); }
                try { reader["InvValueOverMax]"].ToString(); }
                catch { missing.Append("InvValueOverMax "); }
                try { reader["CycleCountProgress]"].ToString(); }
                catch { missing.Append("CycleCountProgress "); }
                try { reader["EVTrainingHrs]"].ToString(); }                                    
                catch { missing.Append("EVTrainingHrs "); }
                //***

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

        public void BBUKPIRetrieveSiteDetailsByBBUKPIId(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            Database.SqlClient.ProcessRow<b_BBUKPI> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_BBUKPI>(reader => { this.LoadFromDatabaseSiteDetailsByBBUKPIId_V2(reader); return this; });
                Database.StoredProcedure.usp_BBUKPI_RetrieveSiteDetailsByClientIdSiteIdAndBBUKPIId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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


        public void LoadFromDatabaseSiteDetailsByBBUKPIId_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                ClientId = reader.GetInt64(i++);
                BBUKPIId = reader.GetInt64(i++);
                SiteId = reader.GetInt64(i++);
                //SiteName = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    SiteName = reader.GetString(i);
                }
                else
                {
                    SiteName = "";
                }
                i++;
                //PMWOCompleted = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    PMWOCompleted = reader.GetDecimal(i);
                }
                else
                {
                    PMWOCompleted = 0;
                }
                i++;
                //WOBacklogCount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    WOBacklogCount = reader.GetInt32(i);
                }
                else
                {
                    WOBacklogCount = 0;
                }
                i++;
                //RCACount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    RCACount = reader.GetInt32(i);
                }
                else
                {
                    RCACount = 0;
                }
                i++;
                //TTRCount = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    TTRCount = reader.GetInt32(i);
                }
                else
                {
                    TTRCount = 0;
                }
                i++;
                //InvValueOverMax = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    InvValueOverMax = reader.GetDecimal(i);
                }
                else
                {
                    InvValueOverMax = 0;
                }
                i++;
                //PhyInvAccuracy = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    PhyInvAccuracy = reader.GetDecimal(i);
                }
                else
                {
                    PhyInvAccuracy = 0;
                }
                i++;
                //EVTrainingHrs = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    EVTrainingHrs = reader.GetDecimal(i);
                }
                else
                {
                    EVTrainingHrs = 0;
                }
                i++;
                // DownDaySched = reader.GetBoolean(i++);
                if (false == reader.IsDBNull(i))
                {
                    DownDaySched = reader.GetBoolean(i);
                }
                else
                {
                    DownDaySched = false;
                }
                i++;
                //OptPMPlansCompleted = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    OptPMPlansCompleted = reader.GetInt32(i);
                }
                else
                {
                    OptPMPlansCompleted = 0;
                }
                i++;
                //OptPMPlansAdopted = reader.GetInt32(i++);
                if (false == reader.IsDBNull(i))
                {
                    OptPMPlansAdopted = reader.GetInt32(i);
                }
                else
                {
                    OptPMPlansAdopted = 0;
                }
                i++;
                //MLT = reader.GetDecimal(i++);
                if (false == reader.IsDBNull(i))
                {
                    MLT = reader.GetDecimal(i);
                }
                else
                {
                    MLT = 0;
                }
                i++;
                //TrainingPlanImp = reader.GetBoolean(i++);
                if (false == reader.IsDBNull(i))
                {
                    TrainingPlanImp = reader.GetBoolean(i);
                }
                else
                {
                    TrainingPlanImp = false;
                }
                i++;
                //SubmitDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    SubmitDate = reader.GetDateTime(i);
                }
                else
                {
                    SubmitDate = DateTime.MinValue;
                }
                i++;
                //SubmitBy_PersonnelId = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    SubmitBy_PersonnelId = reader.GetInt64(i);
                }
                else
                {
                    SubmitBy_PersonnelId = 0;
                }
                i++;
                //SubmitBy_Name = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    SubmitBy_Name = reader.GetString(i);
                }
                else
                {
                    SubmitBy_Name = "";
                }
                i++;
                //Status = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                //Week = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Week = reader.GetString(i);
                }
                else
                {
                    Week = "";
                }
                i++;
                //Year = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    Year = reader.GetString(i);
                }
                else
                {
                    Year = "";
                }
                i++;
                //CreateBy = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    CreateBy = reader.GetString(i);
                }
                else
                {
                    CreateBy = "";
                }
                i++;
                //CreateDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                }
                i++;
                //ModifyBy = reader.GetString(i++);
                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i);
                }
                else
                {
                    ModifyBy = "";
                }
                i++;
                //ModifyDate = reader.GetDateTime(i++);
                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                }
                i++;
                //****V2-909**
                //[PMFollowUpComp][int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    PMFollowUpComp = reader.GetInt32(i);
                }
                else
                {
                    PMFollowUpComp = 0;
                }
                i++;

                //[ActiveMechUsers] [int] NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    ActiveMechUsers = reader.GetInt32(i);
                }
                else
                {
                    ActiveMechUsers = 0;
                }
                i++;

                //[CycleCountProgress] [decimal](6, 3)NOT NULL,
                if (false == reader.IsDBNull(i))
                {
                    CycleCountProgress = reader.GetDecimal(i);
                }
                else
                {
                    CycleCountProgress = 0;
                }
                i++;

               // [WeekStart] datetime2,

                if (false == reader.IsDBNull(i))
                {
                    WeekStart = reader.GetDateTime(i);
                }
                else
                {
                    WeekStart = DateTime.MinValue;
                }
                i++;

                //[WeekEnd] datetime2
                if (false == reader.IsDBNull(i))
                {
                    WeekEnd = reader.GetDateTime(i);
                }
                else
                {
                    WeekEnd = DateTime.MinValue;
                }
                i++;
                //****
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["BBUKPIId"].ToString(); }
                catch { missing.Append("BBUKPIId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["SiteName"].ToString(); }
                catch { missing.Append("SiteName "); }

                try { reader["PMWOCompleted"].ToString(); }
                catch { missing.Append("PMWOCompleted "); }

                try { reader["WOBacklogCount"].ToString(); }
                catch { missing.Append("WOBacklogCount "); }

                try { reader["RCACount"].ToString(); }
                catch { missing.Append("RCACount "); }

                try { reader["TTRCount"].ToString(); }
                catch { missing.Append("TTRCount "); }

                try { reader["InvValueOverMax"].ToString(); }
                catch { missing.Append("InvValueOverMax "); }

                try { reader["PhyInvAccuracy"].ToString(); }
                catch { missing.Append("PhyInvAccuracy "); }

                try { reader["EVTrainingHrs"].ToString(); }
                catch { missing.Append("EVTrainingHrs "); }

                try { reader["DownDaySched"].ToString(); }
                catch { missing.Append("DownDaySched "); }

                try { reader["OptPMPlansCompleted"].ToString(); }
                catch { missing.Append("OptPMPlansCompleted "); }

                try { reader["MLT"].ToString(); }
                catch { missing.Append("MLT "); }

                try { reader["TrainingPlanImp"].ToString(); }
                catch { missing.Append("TrainingPlanImp "); }

                try { reader["SubmitDate"].ToString(); }
                catch { missing.Append("SubmitDate "); }

                try { reader["SubmitBy_Name"].ToString(); }
                catch { missing.Append("SubmitBy_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Week"].ToString(); }
                catch { missing.Append("Week "); }

                try { reader["Year"].ToString(); }
                catch { missing.Append("Year "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }
                //***V2-909
                try { reader["PMFollowUpComp"].ToString(); }
                catch { missing.Append("PMFollowUpComp "); }

                try { reader["ActiveMechUsers"].ToString(); }
                catch { missing.Append("ActiveMechUsers "); }

                try { reader["CycleCountProgress"].ToString(); }
                catch { missing.Append("CycleCountProgress "); }

                try { reader["WeekStart"].ToString(); }
                catch { missing.Append("WeekStart "); }

                try { reader["WeekEnd"].ToString(); }
                catch { missing.Append("WeekEnd "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);

            }

        }

        public void RetrieveYearWeekForFilter(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref List<b_BBUKPI> results
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
                command.CommandTimeout = 60;  // RKL for local machine

                // Call the stored procedure to retrieve the data

                results = Database.StoredProcedure.usp_BBUKPI_RetrieveYearWeekForFilter_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_BBUKPI ProcessRetrieveYearWeekForFilter(SqlDataReader reader)
        {
            b_BBUKPI bBUKPI = new b_BBUKPI();

            bBUKPI.LoadFromDatabaseYearWeekForFilter(reader);
            return bBUKPI;
        }

        public int LoadFromDatabaseYearWeekForFilter(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Id
                Id = reader.GetInt64(i++);

                // YearWeek
                if (false == reader.IsDBNull(i))
                {
                    YearWeek = reader.GetString(i);
                }
                else
                {
                    YearWeek = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Id"].ToString(); }
                catch { missing.Append("Id"); }

                try { reader["YearWeek"].ToString(); }
                catch { missing.Append("YearWeek"); }

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
    }
}
