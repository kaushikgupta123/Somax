using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
   public partial class b_FleetIssues
    {
        #region Property
       
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string SearchText { get; set; }
        public string ClientLookupId { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string VIN { get; set; }   
     
        public string Make { get; set; }
        public string SerialNo { get; set; }
        public string ModelNo { get; set; }
        public string ReadingStartDate { get; set; }
        public string ReadingEndDate { get; set; }
        public string ServiceOrderClientLookupId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }

        public string RecordStartDate { get; set; }
        public string RecordEndDate { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public Int32 TotalCount { get; set; }
        public string EquipImage { get; set; }
        public List<b_FleetIssues> listOfFleetIssues { get; set; }
        public long PrevFleeissueId { get; set; }

        #region Fleet Only
        public int FleetIssuesCount { get; set; }

        #endregion
        #endregion

        public void RetrieveFleetIssueChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_FleetIssues results
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

                results = Database.StoredProcedure.usp_FleetIssues_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void RetrieveByFleetIssuesIdFromDatabase(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName
       )
        {
            Database.SqlClient.ProcessRow<b_FleetIssues> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_FleetIssues>(reader => { this.LoadFromDatabaseforRetrieveByFleetIssuesId(reader); return this; });
                StoredProcedure.usp_FleetIssues_RetrieveByFleetIssuesId_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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
        public static b_FleetIssues ProcessRetrieveForFleetIssueChunkV2(SqlDataReader reader)
        {
            b_FleetIssues FleetIssues = new b_FleetIssues();

            FleetIssues.LoadFromDatabaseForFleetIssueChunkSearchV2(reader);
            return FleetIssues;
        }
        public int LoadFromDatabaseForFleetIssueChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //  ClientLookupId
                ClientLookupId = reader.GetString(i++);

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
                    Make = reader.GetString(i);
                }
                else
                {
                    Make = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Model = reader.GetString(i);
                }
                else
                {
                    Model = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    VIN = reader.GetString(i);
                }
                else
                {
                    VIN = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    EquipImage = reader.GetString(i);
                }
                else
                {
                    EquipImage = "";
                }
                i++;
                FleetIssuesId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    RecordDate = reader.GetDateTime(i);
                }
                else
                {
                    RecordDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
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
                    Defects = reader.GetString(i);
                }
                else
                {
                    Defects = "";
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
                    ServiceOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    ServiceOrderClientLookupId = "";
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

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Name"].ToString(); }
                catch { missing.Append(" Name "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append(" Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append(" Model "); }

                try { reader["VIN"].ToString(); }
                catch { missing.Append(" VIN "); }

                try { reader["EquipImage"].ToString(); }
                catch { missing.Append(" EquipImage "); }

                try { reader["FleetIssuesId"].ToString(); }
                catch { missing.Append(" FleetIssuesId "); }

                try { reader["RecordDate"].ToString(); }
                catch { missing.Append(" RecordDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append(" Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append(" Status "); }

                try { reader["Defects"].ToString(); }
                catch { missing.Append(" Defects "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append(" CompleteDate "); }

                try { reader["ServiceOrderClientLookupId"].ToString(); }
                catch { missing.Append(" ServiceOrderClientLookupId "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append(" TotalCount "); }
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
        public int LoadFromDatabaseforRetrieveByFleetIssuesId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                //  FleetIssuesId column, bigint, not null
                FleetIssuesId = reader.GetInt64(i++);
                //  EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    EquipmentClientLookupId = reader.GetString(i);

                }
                else
                {
                    EquipmentClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    RecordDate = reader.GetDateTime(i);
                }
                else
                {
                    RecordDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Defects = reader.GetString(i);

                }
                else
                {
                    Defects = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);

                }
                else
                {
                    Description = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    DriverName = reader.GetString(i);

                }
                else
                {
                    DriverName = "";
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


        public static b_FleetIssues ProcessRetrieveForFleetIssueLookupList(SqlDataReader reader)
        {
            b_FleetIssues FleetIssues = new b_FleetIssues();

            FleetIssues.LoadFromDatabaseForFleetIssueLookupList(reader);
            return FleetIssues;
        }

        public int LoadFromDatabaseForFleetIssueLookupList(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                //FleetIssuesId
                FleetIssuesId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    RecordDate = reader.GetDateTime(i);
                }
                else
                {
                    RecordDate = DateTime.MinValue;
                }
                i++;                

                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
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
                    Defects = reader.GetString(i);
                }
                else
                {
                    Defects = "";
                }
                i++;
             

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

             

                try { reader["FleetIssuesId"].ToString(); }
                catch { missing.Append(" FleetIssuesId "); }

                try { reader["RecordDate"].ToString(); }
                catch { missing.Append(" RecordDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append(" Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append(" Status "); }

                try { reader["Defects"].ToString(); }
                catch { missing.Append(" Defects "); }
               
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

        #region Fleet Only
        public void LoadFromDatabaseDashBoard(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                FleetIssuesCount = reader.GetInt32(i);
                i++;               
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();
                try { reader[" FleetIssuesCount"].ToString(); }
                catch { missing.Append(" FleetIssuesCount "); }

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
            b_FleetIssues obj = new b_FleetIssues();

            // Load the object from the database
            obj.LoadFromDatabaseDashBoard(reader);

            // Return result
            return (object)obj;
        }
        public void FleetIssues_RetrieveDashboardChart(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_FleetIssues> data)
        {
            SqlCommand command = null;
            string message = String.Empty;
            List<b_FleetIssues> results = null;
            data = new List<b_FleetIssues>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_FleetIssues_RetrieveDashboardChart_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_FleetIssues>();
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

        #region Retrieve By Equipment Id
        public static b_FleetIssues ProcessRetrieveForFleetIssueByEquipmentIdV2(SqlDataReader reader)
        {
            b_FleetIssues FleetIssues = new b_FleetIssues();

            FleetIssues.LoadFromDatabaseForFleetIssueByEquipmentIdV2(reader);
            return FleetIssues;
        }

        public int LoadFromDatabaseForFleetIssueByEquipmentIdV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                FleetIssuesId= reader.GetInt64(i++);

                // EquipmentId column, bigint, not null
                EquipmentId = reader.GetInt64(i++);

                //  ClientLookupId
                EquipmentClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    RecordDate = reader.GetDateTime(i);
                }
                else
                {
                    RecordDate = DateTime.MinValue;
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Defects = reader.GetString(i);
                }
                else
                {
                    Defects = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    DriverName = reader.GetString(i);
                }
                else
                {
                    DriverName = "";
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
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ServiceOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    ServiceOrderClientLookupId ="";
                }
                i++;
                 
                

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["FleetIssuesId"].ToString(); }
                catch { missing.Append("FleetIssuesId "); }

                try { reader["EquipmentId"].ToString(); }
                catch { missing.Append("EquipmentId "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["RecordDate"].ToString(); }
                catch { missing.Append(" RecordDate "); }

                try { reader["Defects"].ToString(); }
                catch { missing.Append(" Defects "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append(" Description "); }

                try { reader["DriverName"].ToString(); }
                catch { missing.Append(" DriverName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append(" Status "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append(" CompleteDate "); }

                try { reader["ServiceOrderClientLookupId"].ToString(); }
                catch { missing.Append(" ServiceOrderClientLookupId "); }

              
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

        public void RetrieveFleetIssueByEquipmentIdV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_FleetIssues> results
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

                results = Database.StoredProcedure.usp_FleetIssues_RetrieveByEquipmentId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        

        public void RetrieveByServiceOrderIdFromDatabase_V2(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_FleetIssues> temp
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
                temp = StoredProcedure.usp_FleetIssues_RetrieveByServiceorderId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);


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

        public static b_FleetIssues ProcessRetrieveByServiceOrderIdV2(SqlDataReader reader)
        {
            b_FleetIssues FleetIssues = new b_FleetIssues();

            FleetIssues.LoadFromDatabase(reader);
            return FleetIssues;
        }


        public void FleetIssuesUpdateforPrevandNewFleetissuesInDatabase(
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
                StoredProcedure.usp_FleetIssues_UpdateforPrevandNewFleetissues_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void ValidateIfServiceOrdrExist(
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
                results = Database.StoredProcedure.usp_FleetIssues_ValidateIfServiceOrderExist_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
    }
}
