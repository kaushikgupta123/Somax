using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Business
{
    public partial class b_Project
    {
        #region Property
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string SearchText { get; set; }
        public string WorkOrderClientLookupId { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        public string CompleteStartDateVw { get; set; }
        public string CompleteEndDateVw { get; set; }
        public string CloseStartDateVw { get; set; }
        public string CloseEndDateVw { get; set; }
        public DateTime CreateDate { get; set; }
        public string Coordinator { get; set; }
        public string Priority { get; set; }
        public long ChargeToId { get; set; }
        public string ChargeTo_Name { get; set; }
        public long WorkOrderId { get; set; }
        public DateTime? RequiredDate { get; set; }
        public int TotalCount { get; set; }
        public int ChildCount { get; set; }
        public string ChargeTo { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public long PurchasOrderId { get; set; }
        public string PurchasOrderClientLookupId { get; set; }
        public int Line { get; set; }
        public string PartID { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public string Buyer { get; set; }
        //V2-1087
        public string AG1ClientLookupId { get; set; }
        public string AG2ClientLookupId { get; set; }
        public string AG3ClientLookupId { get; set; }
        public string CoordinatorFullName { get; set; }
        public string OwnerFullName { get; set; }
        public decimal PurchasingCost { get; set; }
        public decimal Spent { get; set; }
        public decimal Remaining { get; set; }
        public decimal SpentPercentage { get; set; }
        public decimal RemainingPercentage { get; set; }


        #region Project Costing WorkOrder Tab Properties add
        public string Planner { get; set; }
        public string PC_WO_CompleteDate { get; set; }
        
        public decimal? MaterialCost { get; set; }
        public decimal? LaborCost { get; set; }
        public decimal? TotalCost { get; set; }

      
        public string PC_PO_CompleteDate { get; set; }

        #endregion
        #endregion

        #region Project Chunk Search
        public static b_Project ProcessRowForProjectRetriveAllChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Project proj = new b_Project();
            proj.LoadFromDatabaseForProjectRetriveAllForSearch(reader);
            return proj;
        }
        public int LoadFromDatabaseForProjectRetriveAllForSearch(SqlDataReader reader)
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

                // ProjectId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    ProjectId = reader.GetInt64(i);
                }
                else
                {
                    ProjectId = 0;
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

                // ClientLookupId column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
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

                // ActualStart column, datetime2, not null             
                if (false == reader.IsDBNull(i))
                {
                    ActualStart = reader.GetDateTime(i);
                }
                else
                {
                    ActualStart = DateTime.MinValue;
                }
                i++;

                // ActualFinish column, datetime2, not null                
                if (false == reader.IsDBNull(i))
                {
                    ActualFinish = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinish = DateTime.MinValue;
                }
                i++;

                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
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

                // ClientLookupId column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkOrderClientLookupId = "";
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

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientlookupId"].ToString(); }
                catch { missing.Append("ClientlookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ActualStart"].ToString(); }
                catch { missing.Append("ActualStart "); }

                try { reader["ActualFinish"].ToString(); }
                catch { missing.Append("ActualFinish "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["WorkOrderClientLookupId"].ToString(); }
                catch { missing.Append("WorkOrderClientLookupId "); }

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
ref List<b_Project> results
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

                results = Database.StoredProcedure.usp_Project_RetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        #region Project tab header
        public void RetrieveProjectByProjectIdForHeader(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref b_Project results
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

                results = Database.StoredProcedure.usp_Project_RetrieveByProjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static b_Project ProcessRowForProjectRetrieveByProjectIdIdForHeader(SqlDataReader reader)
        {
            // Create instance of object
            b_Project obj = new b_Project();

            // Load the object from the database
            obj.LoadFromDatabaseForProjectRetrieveByProjectIdForHeader(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForProjectRetrieveByProjectIdForHeader(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ProjectId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    ProjectId = reader.GetInt64(i);
                }
                else
                {
                    ProjectId = 0;
                }
                i++;

                // ClientLookupId column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
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

                // Coordinator column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    Coordinator = reader.GetString(i);
                }
                else
                {
                    Coordinator = "";
                }
                i++;

                // ScheduleStart column, datetime2, null
                if (false == reader.IsDBNull(i))
                {
                    ScheduleStart = reader.GetDateTime(i);
                }
                else
                {
                    ScheduleStart = DateTime.MinValue;
                }
                i++;

                // ScheduleFinish column, datetime2, null
                if (false == reader.IsDBNull(i))
                {
                    ScheduleFinish = reader.GetDateTime(i);
                }
                else
                {
                    ScheduleFinish = DateTime.MinValue;
                }
                i++;

                // CompleteDate column, datetime2, not
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                // Budget column, Decimal, not null             
                if (false == reader.IsDBNull(i))
                {
                    Budget = reader.GetDecimal(i);
                }
                else
                {
                    Budget = 0;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["ClientlookupId"].ToString(); }
                catch { missing.Append("ClientlookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Coordinator"].ToString(); }
                catch { missing.Append("Coordinator "); }

                try { reader["ScheduleStart"].ToString(); }
                catch { missing.Append("ScheduleStart "); }

                try { reader["ScheduleFinish"].ToString(); }
                catch { missing.Append("ScheduleFinish "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Budget"].ToString(); }
                catch { missing.Append("Budget "); }

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

        #region WorkOrder Project LookupList By Search Criteria
        public void RetrieveWorkOrder_ProjectDetailsLookupListBySearchCriteria(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_Project> results
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

                results = Database.StoredProcedure.usp_WorkOrder_ProjectLookupListBySearchCriteria_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_Project ProcessRowForWorkOrder_ProjectDetailsLookupListBySearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_Project obj = new b_Project();

            // Load the object from the database
            obj.LoadFromDatabaseForWorkOrder_ProjectDetailsLookupListBySearchCriteria(reader);

            // Return result
            return obj;
        }

        public int LoadFromDatabaseForWorkOrder_ProjectDetailsLookupListBySearchCriteria(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // Type column, nvarchar(31), not null
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


        #endregion

        #region Validate ClientlookupId
        public void ValidateClientLookupId(
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
                results = Database.StoredProcedure.usp_Project_ValidateClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Chunk Search Lookuplist
        public void RetrieveProjectLookuplistChunkSearchV2(SqlConnection connection, SqlTransaction transaction,
                       long callerUserInfoId, string callerUserName, ref List<b_Project> results)
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

                results = Database.StoredProcedure.usp_Project_RetrieveChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_Project ProcessRowForChunkSearchLookupList(SqlDataReader reader)
        {
            b_Project project = new b_Project();

            project.LoadFromDatabaseForLookupListChunkSearchV2(reader);
            return project;
        }
        public int LoadFromDatabaseForLookupListChunkSearchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // ProjectId column, bigint, not null
                ProjectId = reader.GetInt64(i++);

                //ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                //Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
                }
                i++;

                //totalCount 
                if (false == reader.IsDBNull(i))
                {
                    totalCount = reader.GetInt32(i);
                }
                else
                {
                    totalCount = 0;
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

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
        #endregion
        #region V2-1135
       
        public void RetrieveWorkoderProjectLookupList_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Project> results
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

                results = Database.StoredProcedure.usp_Project_RetrieveWorkoderProjectLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public static b_Project ProcessRowForRetrieveWorkoderProjectLookupList_V2(SqlDataReader reader)
        {
            // Create instance of object
            b_Project obj = new b_Project();

            // Load the object from the database
            obj.LoadFromDatabaseForRetrieveWorkoderProjectLookupList_V2(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForRetrieveWorkoderProjectLookupList_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // ProjectId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    ProjectId = reader.GetInt64(i);
                }
                else
                {
                    ProjectId = 0;
                }
                i++;

                // ClientLookupId column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["ClientlookupId"].ToString(); }
                catch { missing.Append("ClientlookupId "); }

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

        //V2-1087
        #region Project Costing WorkOrder Tab

        public static b_Project ProcessRowForProject_ProjectCostingWorkOrderTabLookupListBySearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_Project proj = new b_Project();
            proj.LoadFromDatabaseForProjectCostingWORetriveAllForSearch(reader);
            return proj;
        }
        public int LoadFromDatabaseForProjectCostingWORetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                WorkOrderId = reader.GetInt32(i++);
                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // Description column, nvarchar(max), not null
                Description = reader.GetString(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Description column, nvarchar(max), not null
                Planner = reader.GetString(i++);

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

                // MaterialCost column, Decimal not null
                if (false == reader.IsDBNull(i))
                {
                    MaterialCost = reader.GetDecimal(i);
                }
                else
                {
                    MaterialCost = 0;
                    
                }
                i++;

                // LaborCost column, Decimal not null
                if (false == reader.IsDBNull(i))
                {
                    LaborCost = reader.GetDecimal(i);
                }
                else
                {
                    LaborCost = 0;
                    
                }
                i++;

                // TotalCost column, Decimal not null
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
                   
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

                try { reader["ClientlookupId"].ToString(); }
                catch { missing.Append("ClientlookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Planner"].ToString(); }
                catch { missing.Append("Planner "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["MaterialCost"].ToString(); }
                catch { missing.Append("MaterialCost "); }

                try { reader["LaborCost"].ToString(); }
                catch { missing.Append("LaborCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

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

        #endregion

        #region V2-1087 Project Costing
        public void RetrieveByPK_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_Project> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_Project>(reader => { this.LoadFromDatabaseByPK_V2(reader); return this; });
                Database.StoredProcedure.usp_Project_RetrieveByPK_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        public int LoadFromDatabaseByPK_V2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // ProjectId column, bigint, not null
                ProjectId = reader.GetInt64(i++);

                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // ActualFinish column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ActualFinish = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinish = DateTime.MinValue;
                }
                i++;
                // ActualStart column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ActualStart = reader.GetDateTime(i);
                }
                else
                {
                    ActualStart = DateTime.MinValue;
                }
                i++;
                // Budget column, decimal(15,3), not null
                Budget = reader.GetDecimal(i++);

                // CancelDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CancelDate = reader.GetDateTime(i);
                }
                else
                {
                    CancelDate = DateTime.MinValue;
                }
                i++;
                // CancelBy_PersonnelId column, bigint, not null
                CancelBy_PersonnelId = reader.GetInt64(i++);

                // CancelReason column, nvarchar(15), not null
                CancelReason = reader.GetString(i++);

                // CloseDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    CloseDate = reader.GetDateTime(i);
                }
                else
                {
                    CloseDate = DateTime.MinValue;
                }
                i++;
                // CloseBy_PersonnelId column, bigint, not null
                CloseBy_PersonnelId = reader.GetInt64(i++);

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
                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // Coordinator_PersonnelId column, bigint, not null
                Coordinator_PersonnelId = reader.GetInt64(i++);

                // Description column, nvarchar(MAX), not null
                Description = reader.GetString(i++);

                // FiscalYear column, int, not null
                FiscalYear = reader.GetInt32(i++);

                // HoldDate column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    HoldDate = reader.GetDateTime(i);
                }
                else
                {
                    HoldDate = DateTime.MinValue;
                }
                i++;
                // HoldBy_PersonnelId column, bigint, not null
                HoldBy_PersonnelId = reader.GetInt64(i++);

                // Owner_PersonnelId column, bigint, not null
                Owner_PersonnelId = reader.GetInt64(i++);

                // ReturnFunds column, decimal(15,3), not null
                ReturnFunds = reader.GetDecimal(i++);

                // ScheduleFinish column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduleFinish = reader.GetDateTime(i);
                }
                else
                {
                    ScheduleFinish = DateTime.MinValue;
                }
                i++;
                // ScheduleStart column, datetime2, not null
                if (false == reader.IsDBNull(i))
                {
                    ScheduleStart = reader.GetDateTime(i);
                }
                else
                {
                    ScheduleStart = DateTime.MinValue;
                }
                i++;
                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                // Category column, nvarchar(15), not null
                Category = reader.GetString(i++);

                // AssignedAssetGroup1 column, bigint, not null
                AssignedAssetGroup1 = reader.GetInt64(i++);

                // AssignedAssetGroup2 column, bigint, not null
                AssignedAssetGroup2 = reader.GetInt64(i++);

                // AssignedAssetGroup3 column, bigint, not null
                AssignedAssetGroup3 = reader.GetInt64(i++);

                CoordinatorFullName= reader.GetString(i++);

                OwnerFullName= reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    AG1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AG1ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AG2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AG2ClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AG3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AG3ClientLookupId = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ActualFinish"].ToString(); }
                catch { missing.Append("ActualFinish "); }

                try { reader["ActualStart"].ToString(); }
                catch { missing.Append("ActualStart "); }

                try { reader["Budget"].ToString(); }
                catch { missing.Append("Budget "); }

                try { reader["CancelDate"].ToString(); }
                catch { missing.Append("CancelDate "); }

                try { reader["CancelBy_PersonnelId"].ToString(); }
                catch { missing.Append("CancelBy_PersonnelId "); }

                try { reader["CancelReason"].ToString(); }
                catch { missing.Append("CancelReason "); }

                try { reader["CloseDate"].ToString(); }
                catch { missing.Append("CloseDate "); }

                try { reader["CloseBy_PersonnelId"].ToString(); }
                catch { missing.Append("CloseBy_PersonnelId "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["Coordinator_PersonnelId"].ToString(); }
                catch { missing.Append("Coordinator_PersonnelId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["FiscalYear"].ToString(); }
                catch { missing.Append("FiscalYear "); }

                try { reader["HoldDate"].ToString(); }
                catch { missing.Append("HoldDate "); }

                try { reader["HoldBy_PersonnelId"].ToString(); }
                catch { missing.Append("HoldBy_PersonnelId "); }

                try { reader["Owner_PersonnelId"].ToString(); }
                catch { missing.Append("Owner_PersonnelId "); }

                try { reader["ReturnFunds"].ToString(); }
                catch { missing.Append("ReturnFunds "); }

                try { reader["ScheduleFinish"].ToString(); }
                catch { missing.Append("ScheduleFinish "); }

                try { reader["ScheduleStart"].ToString(); }
                catch { missing.Append("ScheduleStart "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Category"].ToString(); }
                catch { missing.Append("Category "); }

                try { reader["AssignedAssetGroup1"].ToString(); }
                catch { missing.Append("AssignedAssetGroup1 "); }

                try { reader["AssignedAssetGroup2"].ToString(); }
                catch { missing.Append("AssignedAssetGroup2 "); }

                try { reader["AssignedAssetGroup3"].ToString(); }
                catch { missing.Append("AssignedAssetGroup3 "); }

                try { reader["CoordinatorFullName"].ToString(); }
                catch { missing.Append("CoordinatorFullName "); }

                try { reader["OwnerFullName"].ToString(); }
                catch { missing.Append("OwnerFullName "); }

                try { reader["AssignedGroup1_ClientLookupId"].ToString(); }
                catch { missing.Append("AssignedGroup1_ClientLookupId "); }

                try { reader["AssignedGroup2_ClientLookupId"].ToString(); }
                catch { missing.Append("AssignedGroup2_ClientLookupId "); }

                try { reader["AssignedGroup3_ClientLookupId"].ToString(); }
                catch { missing.Append("AssignedGroup3_ClientLookupId "); }


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

        #region Retrieve for Dashboard

        public void RetrieveForProjectCostByProjectIdForDashboard_V2(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref b_Project results
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

                results = StoredProcedure.usp_Project_RetrieveForProjectCostDashboardByProjectId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_Project ProcessRowRetrieveByProjectIdForDashboard(SqlDataReader reader)
        {
            // Create instance of object
            b_Project obj = new b_Project();

            // Load the object from the database
            obj.LoadFromDatabaseRetrieveByProjectIdForDashboard(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseRetrieveByProjectIdForDashboard(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Budget column, Decimal, not null             
                if (false == reader.IsDBNull(i))
                {
                    Budget = reader.GetDecimal(i);
                }
                else
                {
                    Budget = 0;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    MaterialCost = reader.GetDecimal(i);
                }
                else
                {
                    MaterialCost = 0;
                }
                i++;          
                if (false == reader.IsDBNull(i))
                {
                    LaborCost = reader.GetDecimal(i);
                }
                else
                {
                    LaborCost = 0;
                }
                i++;           
                if (false == reader.IsDBNull(i))
                {
                    PurchasingCost = reader.GetDecimal(i);
                }
                else
                {
                    PurchasingCost = 0;
                }
                i++;           
                if (false == reader.IsDBNull(i))
                {
                    Spent = reader.GetDecimal(i);
                }
                else
                {
                    Spent = 0;
                }
                i++;           
                if (false == reader.IsDBNull(i))
                {
                    Remaining = reader.GetDecimal(i);
                }
                else
                {
                    Remaining = 0;
                }
                i++;           
                if (false == reader.IsDBNull(i))
                {
                    SpentPercentage = reader.GetDecimal(i);
                }
                else
                {
                    SpentPercentage = 0;
                }
                i++;            
                if (false == reader.IsDBNull(i))
                {
                    RemainingPercentage = reader.GetDecimal(i);
                }
                else
                {
                    RemainingPercentage = 0;
                }
                i++;
    }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["Budget"].ToString(); }
                catch { missing.Append("Budget "); }

                try { reader["MaterialCost"].ToString(); }
                catch { missing.Append("MaterialCost "); }

                try { reader["LaborCost"].ToString(); }
                catch { missing.Append("LaborCost "); }

                try { reader["PurchasingCost"].ToString(); }
                catch { missing.Append("PurchasingCost "); }

                try { reader["Spent"].ToString(); }
                catch { missing.Append("Spent "); }

                try { reader["Remaining"].ToString(); }
                catch { missing.Append("Remaining "); }

                try { reader["SpentPercentage"].ToString(); }
                catch { missing.Append("SpentPercentage "); }

                try { reader["RemainingPercentage"].ToString(); }
                catch { missing.Append("RemainingPercentage "); }

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
        public void RetrieveProjectCostingWorkOrderTabLookupListBySearchCriteria(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_Project> results
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
                results = Database.StoredProcedure.usp_Project_ProjectCostingWorkOrderTabRetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #endregion

        #region Project Costing Purchasing Tab

        public static b_Project ProcessRowForProject_ProjectCostingPurchasingTabLookupListBySearchCriteria(SqlDataReader reader)
        {
            // Create instance of object
            b_Project proj = new b_Project();
            proj.LoadFromDatabaseForProjectCostingPurchasingRetriveAllForSearch(reader);
            return proj;
        }
        public int LoadFromDatabaseForProjectCostingPurchasingRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                PurchasOrderId = reader.GetInt64(i++);
                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                Line = reader.GetInt32(i++);

                // PartID column, nvarchar(max), not null
                PartID = reader.GetString(i++);

                // Description column, nvarchar(max), not null
                Description = reader.GetString(i++);

                // Quantity column, Decimal not null
                if (false == reader.IsDBNull(i))
                {
                    Quantity = reader.GetDecimal(i);
                }
                else
                {
                    Quantity = 0;

                }
                i++;

                // UnitCost column, Decimal not null
                if (false == reader.IsDBNull(i))
                {
                    UnitCost = reader.GetDecimal(i);
                }
                else
                {
                    UnitCost = 0;

                }
                i++;

                // TotalCost column, Decimal not null
                if (false == reader.IsDBNull(i))
                {
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;

                }
                i++;

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Description column, nvarchar(max), not null
                Buyer = reader.GetString(i++);

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
                // TotalCount column, int, not null
                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PurchasOrderId"].ToString(); }
                catch { missing.Append("PurchasOrderId "); }

                try { reader["ClientlookupId"].ToString(); }
                catch { missing.Append("ClientlookupId "); }

                try { reader["Line"].ToString(); }
                catch { missing.Append("Line "); }

                try { reader["PartID"].ToString(); }
                catch { missing.Append("PartID "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Quantity"].ToString(); }
                catch { missing.Append("Quantity "); }

                try { reader["UnitCost"].ToString(); }
                catch { missing.Append("UnitCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Buyer"].ToString(); }
                catch { missing.Append("Buyer "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

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


        public void RetrieveProjectCostingPurchasingTabSearchCriteria(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_Project> results
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
                results = Database.StoredProcedure.usp_Project_ProjectCostingPurchasingTabRetrieveChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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




        #region V2-1087 Project Costing Chunk Search
        public static b_Project ProcessRowForProjectCostingRetriveAllChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_Project proj = new b_Project();
            proj.LoadFromDatabaseForProjectCostingRetriveAllForSearch(reader);
            return proj;
        }
        public int LoadFromDatabaseForProjectCostingRetriveAllForSearch(SqlDataReader reader)
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

                // ProjectId column, bigint, not null             
                if (false == reader.IsDBNull(i))
                {
                    ProjectId = reader.GetInt64(i);
                }
                else
                {
                    ProjectId = 0;
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

                // ClientLookupId column, string, not null               
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
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

                // ActualStart column, datetime2, not null             
                if (false == reader.IsDBNull(i))
                {
                    ActualStart = reader.GetDateTime(i);
                }
                else
                {
                    ActualStart = DateTime.MinValue;
                }
                i++;

                // ActualFinish column, datetime2, not null                
                if (false == reader.IsDBNull(i))
                {
                    ActualFinish = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinish = DateTime.MinValue;
                }
                i++;

                // Status column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
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
                // Budget column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Budget = reader.GetDecimal(i);
                }
                else
                {
                    Budget = 0;
                }
                i++;
                // AG1ClientLookupId column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    AG1ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AG1ClientLookupId = "";
                }
                i++;
                // AG2ClientLookupId column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    AG2ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AG2ClientLookupId = "";
                }
                i++;
                // AG3ClientLookupId column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    AG3ClientLookupId = reader.GetString(i);
                }
                else
                {
                    AG3ClientLookupId = "";
                }
                i++;

                // Coordinator column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    Coordinator = reader.GetString(i);
                }
                else
                {
                    Coordinator = "";
                }
                i++;

                //TotalCount
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["ProjectId"].ToString(); }
                catch { missing.Append("ProjectId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["ClientlookupId"].ToString(); }
                catch { missing.Append("ClientlookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ActualStart"].ToString(); }
                catch { missing.Append("ActualStart "); }

                try { reader["ActualFinish"].ToString(); }
                catch { missing.Append("ActualFinish "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["TotalCount"].ToString(); }
                catch { missing.Append("TotalCount "); }

                try { reader["Budget"].ToString(); }
                catch { missing.Append("Budget "); }

                try { reader["AG1ClientLookupId"].ToString(); }
                catch { missing.Append("AG1ClientLookupId "); }

                try { reader["AG2ClientLookupId"].ToString(); }
                catch { missing.Append("AG2ClientLookupId "); }

                try { reader["AG3ClientLookupId"].ToString(); }
                catch { missing.Append("AG3ClientLookupId "); }

                try { reader["Coordinator"].ToString(); }
                catch { missing.Append("Coordinator "); }

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
        public void RetrieveProjectCostingChunkSearch(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_Project> results
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

                results = Database.StoredProcedure.usp_Project_RetrieveProjectCostingChunkSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
