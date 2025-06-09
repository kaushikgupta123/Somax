/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person          Description
* =========== ======== ================ ============================================================
* 2014-Sep-09 SOM-304  Roger Lawton     Update Prev Maint Sched record on Complete
* 2014-Sep-29 SOM-346  Roger Lawton     Added ProcessRowForList and 
*                                       LoadFromDatabaseForList
* 2015-Jun-06 SOM-687  Roger Lawton     Grid to show First Name Last Name - NOT User Name
* 2016-Sep-03 SOM-1082 Roger Lawton     Added Shift to LoadFromDatabaseForWorkOrderRetriveAllForSearch
* 2016-Oct-05 SOM-1123 Roger Lawton     Added Denied info to LoadFromDatabaseForWorkOrderRetriveAllForSearch
* 2016-Nov-09 SOM-1154 Roger Lawton     Added Complete Comments (100 chars) to 
*                                         LoadFromDatabaseForWorkOrderRetriveAllForSearch
* 2017-Mar-30 SOM-1278 Roger Lawton     Added workassigne_name when getting workorder for print
 * ****************************************************************************************************
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Database.Business
{
    /// <summary>
    /// Business object that stores a record from the TechSpecs table.
    /// </summary>
    public partial class b_WorkOrder : IWorkflow
    {
        #region Property
        // SOM-1123
        public string DeniedBy_PersonnelId_ClientLookupId { get; set; }
        public string DeniedBy_PersonnelId_Name { get; set; }
        public string MaintOnDemandClientLookUpId { get; set; }

        public string Requestor_PersonnelClientLookupId { get; set; }
        public string Creator_PersonnelClientLookupId { get; set; }
        public string ApproveBy_PersonnelClientLookupId { get; set; }
        public string Planner_PersonnelClientLookupId { get; set; }
        public string Scheduler_PersonnelClientLookupId { get; set; }
        public string SignoffBy_PersonnelClientLookupId { get; set; }
        public string WorkAssigned_PersonnelClientLookupId { get; set; }
        public string CompleteBy_PersonnelClientLookupId { get; set; }
        public string CloseBy_PersonnelClientLookupId { get; set; }
        public string ReleaseBy_PersonnelClientLookupId { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string Requestor_Name { get; set; }
        public string WorkAssigned_Name { get; set; }
        public string DepartmentName { get; set; }
        public string Creator { get; set; }
        public string Assigned { get; set; }
        public string DateRange { get; set; }
        public string DateColumn { get; set; }
        public DateTime CreateDate { get; set; }
        public Int64 ScheduleId { get; set; }
        public Int64 PersonnelId { get; set; } //-----SOM-666 
                                               //For Workbench
        public string Createby { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public Int64 UserInfoId { get; set; }
        public string ApproveFlag { get; set; }
        public string ScheduleFlag { get; set; }
        public string DeniedFlag { get; set; }
        public string PersonnelClientLookupId { get; set; }
        public string CreateBy_PersonnelName { get; set; }
        public string CompleteBy_PersonnelName { get; set; }
        public string IncludeCopletedInCompletionWorkbench { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string Search { get; set; }
        public System.Data.DataTable WorkOrderNoList { get; set; }
        public string WorkOrderNo { get; set; }
        public string CompleteBy { get; set; }
        public long WOCountAsType { get; set; }
        public long WOCountAsStatus { get; set; }
        public int Month { get; set; }
        public long WOCount { get; set; }
        public long LoggedInUserPEID { get; set; }

        #region for seacrh grid load
        public UtilityAdd utilityAdd { get; set; }
        public List<b_WorkOrder> listOfWO { get; set; }

        public List<b_Attachment> listOfAttachment { get; set; }

        public List<b_WorkOrderTask> listOfWOTask { get; set; }

        public List<b_Timecard> listOfTimecard { get; set; }
        public List<b_PartHistory> listOfPartHistory { get; set; }
        public List<b_OtherCosts> listOfOtherCosts { get; set; }

        public List<b_OtherCosts> listOfSummery { get; set; }
        public List<b_Instructions> listOfInstructions { get; set; }
        #region V2-944
        public List<b_WorkOrderUDF> listOfWorkOrderUDF { get; set; }
        public List<b_WorkOrderSchedule> listOfWorkOrderSchedule { get; set; }
        #endregion


        public int TotalCount { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string Scheduled { get; set; }
        public string ActualFinish { get; set; }
        public string Completed { get; set; }
        public string ChargeTo { get; set; }
        public string FilterText { get; set; }


        //V2  524
        public string WorkOrderClientLookupId { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public decimal ScheduledHours { get; set; }
        //V2-271
        public string CompletedDate { get; set; }
        public string AssignedFullName { get; set; }
        public string SearchText { get; set; }

        //V2-276
        public Int64 ObjectId { get; set; }
        public string IsActualOrEstimated { get; set; }
        public decimal PartCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OtherCost { get; set; }
        public decimal TotalCost { get; set; }

        public string WONumber { get; set; }
        public string PONumber { get; set; }

        public string POType { get; set; }

        public string POStatus { get; set; }

        public int LineNumber { get; set; }

        public string LineStatus { get; set; }
        public string LineDesc { get; set; }
        public string VendorClientlookupId { get; set; }
        public string VendorName { get; set; }
        public string PersonnelList { get; set; }

        //1060
        public string WorkOrderSchedIdsList { get; set; }
        public string WorkOrderClientLookupIdsList { get; set; }

        public string Personnels { get; set; }
        public string PersonnelFull { get; set; }
        public bool IsDeleteFlag { get; set; }
        #endregion
        //V2-524
        public long WorkOrderScheduleId { get; set; }
        public string ScheduledDateStart { get; set; }
        public string ScheduledDateEnd { get; set; }
        public string RequireDate { get; set; }
        public string CalendarDateStart { get; set; }
        public string CalendarDateEnd { get; set; }
        public string NameLast { get; set; }

        public string PersonnelName { get; set; }
        public string AssetGroup1ClientlookupId { get; set; }
        public string AssetGroup2ClientlookupId { get; set; }
        public string AssetGroup3ClientlookupId { get; set; }

        public string AssetGroup1Description { get; set; }
        public string AssetGroup2Description { get; set; }
        public string AssetGroup3Description { get; set; }
        //<!--(Added on 25/06/2020)-->

        public long AssetGroup1Id { get; set; }
        public long AssetGroup2Id { get; set; }
        public long AssetGroup3Id { get; set; }
        public string AssetGroup1Ids { get; set; }
        public string AssetGroup2Ids { get; set; }
        public string AssetGroup3Ids { get; set; }
        public string PrevMaintSchedType { get; set; }
        public string PrevMaintMasterType { get; set; }

        public string AssetGroup1AdvSearchId { get; set; }
        public string AssetGroup2AdvSearchId { get; set; }
        public string AssetGroup3AdvSearchId { get; set; }
        //<!--(Added on 25/06/2020)-->
        //V2-347
        public string StartActualFinishDateVw { get; set; }
        public string EndActualFinishDateVw { get; set; }
        public string StartCreateDate { get; set; }
        public string EndCreateDate { get; set; }
        public string StartScheduledDate { get; set; }
        public string EndScheduledDate { get; set; }
        public string StartActualFinishDate { get; set; }
        public string EndActualFinishDate { get; set; }
        public string CreateStartDateVw { get; set; }
        public string CreateEndDateVw { get; set; }
        //public int Pcount { get; set; }
        //public int Scount { get; set; }
        public string PerNextValue { get; set; }
        public DateTime SDNextValue { get; set; }
        public decimal SumPersonnelHour { get; set; }
        public decimal SumScheduledateHour { get; set; }
        public decimal GrandTotalHour { get; set; }
        public long PerIDNextValue { get; set; }

        //V2 576
        public string Mode { get; set; }
        public string TableName { get; set; }
        public string AssetLocation { get; set; }
        public string AssetName { get; set; } //V2-610
        public string ProjectClientLookupId { get; set; }
        public string AccountClientLookupId { get; set; }
        public string SourceIdClientLookupId { get; set; }
        //630
        public string ChargeToDescription { get; set; }

        // V2-634
        public bool TimecardTab { get; set; }
        public bool AutoAddTimecard { get; set; }
        // V2-663
        public string WorkOrderIDList { get; set; }
        public List<b_Timecard> TimecardList { get; set; }
        public b_WorkOrderUDF WorkOrderUDF { get; set; }
        //V2-634
        public string SignoffBy_PersonnelClientLookupIdName { get; set; }
        public string EquipmentType { get; set; }
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public bool RemoveFromService { get; set; }
        public string WOCompCriteriaTitle { get; set; }
        public string WOCompCriteria { get; set; }
        public bool WOCompCriteriaTab { get; set; }
        public bool? downRequired { get; set; }//V2-892
        public long CreatedWorkOrderId { get; set; }//V2-1051
        public string PlannerFullName { get; set; } //V2-1078
        public string ProjectIds { get; set; } //V2-1135
        #endregion
        public static b_WorkOrder ProcessRowForList(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            workOrder.LoadFromDatabaseForList(reader);

            // Return result
            return workOrder;
        }

        public void LoadFromDatabaseForList(SqlDataReader reader)
        {
            // Only Retrieving data used in lists and permission data
            // If we need more columns must change this method and sp
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // AreaId column, bigint, not null
                AreaId = reader.GetInt64(i++);

                // DepartmentId column, bigint, not null
                DepartmentId = reader.GetInt64(i++);

                // StoreroomId column, bigint, not null
                StoreroomId = reader.GetInt64(i++);

                // Description
                ClientLookupId = reader.GetString(i++);

                // Description
                Description = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

                // Type
                Type = reader.GetString(i++);

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

                // WorkAssigned_PersonnelClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkAssigned_PersonnelClientLookupId = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["AreaId"].ToString(); }
                catch { missing.Append("AreaId "); }

                try { reader["DepartmentId"].ToString(); }
                catch { missing.Append("DepartmentId "); }

                try { reader["StoreroomId"].ToString(); }
                catch { missing.Append("StoreroomId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["WorkAssigned_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }



        public static b_WorkOrder ProcessRowForClientIdLookup(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            workOrder.LoadFromDatabaseForClientIdLookup(reader);

            // Return result
            return workOrder;
        }

        public void LoadFromDatabaseForClientIdLookup(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PersonnelId"].ToString(); }
                catch { missing.Append("PersonnelId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static object ProcessRowWorkorderLookUp(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseLookUpworkOrder(reader);

            // Return result
            return (object)obj;
        }

        public void LoadFromDatabaseLookUpworkOrder(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null               
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i);
                }
                else
                {
                    ClientLookupId = "";
                }
                i++;

                // ChargeTo_Name column, nvarchar(63), not null
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i);
                }
                else
                {
                    ChargeTo_Name = "";
                }
                i++;

                // Description column, nvarchar(MAX), not null
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i);
                }
                else
                {
                    Description = "";
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

                // Type column, nvarchar(15), not null
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;

                //
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i);
                }
                else
                {
                    WorkAssigned_Name = "";
                }
                i++;

                //
                if (false == reader.IsDBNull(i))
                {
                    Requestor_Name = reader.GetString(i);
                }
                else
                {
                    Requestor_Name = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                try { reader["Requestor_Name"].ToString(); }
                catch { missing.Append("Requestor_Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }

            //int i = this.LoadFromDatabase(reader);

            //try { WorkAssigned_Name = reader.GetString(i++); }
            //catch { WorkAssigned_Name = ""; }

            //try { Requestor_Name = reader.GetString(i++); }
            //catch { Requestor_Name = ""; }

            //try
            //{


            //}
            //catch (Exception ex)
            //{
            //    // Diagnostics
            //    StringBuilder missing = new StringBuilder();
            //    try { reader["WorkAssigned_Name"].ToString(); }
            //    catch { missing.Append("WorkAssigned_Name "); }

            //    try { reader["Requestor_Name"].ToString(); }
            //    catch { missing.Append("Requestor_Name "); }

            //    StringBuilder msg = new StringBuilder();
            //    msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
            //    if (missing.Length > 0)
            //    {
            //        msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
            //    }

            //    throw new Exception(msg.ToString(), ex);
            //}
        }

        public void LoadFromDatabaseByPKForeignKey(SqlDataReader reader)
        {
            int i = this.LoadFromDatabase(reader);
            //int i = 0;
            try
            {

                if (false == reader.IsDBNull(i))
                {
                    Requestor_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    Requestor_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Creator_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    Creator_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ApproveBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    ApproveBy_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Planner_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    Planner_PersonnelClientLookupId = "";
                }
                i++;
                // V2-1157 - RKL - 2025-Jan-21
                if (false == reader.IsDBNull(i))
                {
                    PlannerFullName = reader.GetString(i);
                }
                else
                {
                    PlannerFullName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Scheduler_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    Scheduler_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    SignoffBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    SignoffBy_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    WorkAssigned_PersonnelClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    CompleteBy_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CloseBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    CloseBy_PersonnelClientLookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ReleaseBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    ReleaseBy_PersonnelClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    CreateDate = DateTime.Now;
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Createby = reader.GetString(i);
                }
                else
                {
                    Createby = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CreateBy_PersonnelName = reader.GetString(i);
                }
                else
                {
                    CreateBy_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelName = reader.GetString(i);
                }
                else
                {
                    CompleteBy_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i);
                }
                else
                {
                    ModifyDate = DateTime.Now;
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
                // SOM-1037 - Denied by
                if (false == reader.IsDBNull(i))
                {
                    DeniedBy_PersonnelId_ClientLookupId = reader.GetString(i);
                }
                else
                {
                    DeniedBy_PersonnelId_ClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    DeniedBy_PersonnelId_Name = reader.GetString(i);
                }
                else
                {
                    DeniedBy_PersonnelId_Name = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssignedFullName = reader.GetString(i);
                }
                else
                {
                    AssignedFullName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProjectClientLookupId = reader.GetString(i);
                }
                else
                {
                    ProjectClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    AccountClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SourceIdClientLookupId = reader.GetString(i);
                }
                else
                {
                    SourceIdClientLookupId = "";
                }
                i++;
                //**V2-847**

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientlookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientlookupId = "";
                }
                i++;
                //***               
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1Description = reader.GetString(i);
                }
                else
                {
                    AssetGroup1Description = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2Description = reader.GetString(i);
                }
                else
                {
                    AssetGroup2Description = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProjectClientLookupId  = reader.GetString(i);
                }
                else
                {
                    ProjectClientLookupId = "";
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["Requestor_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelClientLookupId "); }

                try { reader["Creator_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Creator_PersonnelClientLookupId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ApproveBy_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("ApproveBy_PersonnelClientLookupId "); }

                try { reader["Planner_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Planner_PersonnelClientLookupId "); }

                try { reader["Scheduler_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Scheduler_PersonnelClientLookupId "); }

                try { reader["SignoffBy_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("SignoffBy_PersonnelClientLookupId "); }

                try { reader["WorkAssigned_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelClientLookupId "); }

                try { reader["CompleteBy_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelClientLookupId "); }

                try { reader["CloseBy_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("CloseBy_PersonnelClientLookupId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }

                try { reader["CompleteBy_PersonnelName"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelName "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["Created"].ToString(); }
                catch { missing.Append("Created "); }

                //**847**
                try { reader["AssetGroup1ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup1ClientlookupId "); }

                try { reader["AssetGroup2ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup2ClientlookupId "); }
                //**
                try { reader["AssetGroup1Description"].ToString(); }
                catch { missing.Append("AssetGroup1Description "); }

                try { reader["AssetGroup2Description"].ToString(); }
                catch { missing.Append("AssetGroup2Description "); }

                //V2-1012
                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }

                try { reader["ProjectClientLookupId"].ToString(); }
                catch { missing.Append("ProjectClientLookupId "); }
                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }
        // RKL - Do not need
        /*
        public void LoadFromDatabaseWithDepartName(SqlDataReader reader)
        {
            int i = LoadFromDatabaseForWorkOrderRetriveAllForSearch(reader);

            if (false == reader.IsDBNull(i))
            {
                DepartmentName = reader.GetString(i++);
            }
            else
            {
                DepartmentName = ""; i++;
            }
            if (false == reader.IsDBNull(i))
            {
                Creator = reader.GetString(i++);
            }
            else
            {
                Creator = ""; i++;
            }
            if (false == reader.IsDBNull(i))
            {
                Assigned = reader.GetString(i++);
            }
            else
            {
                Assigned = ""; i++;
            }
        }
        */
        public void LoadFromDatabaseWithWorkbenchDetail(SqlDataReader reader)
        {
            int i = LoadFromDatabaseForWorkbenchRetriveAllForSearch(reader);


        }

        public void LoadFromDatabaseWithCompletionWorkbenchForFPDetail(SqlDataReader reader)
        {
            int i = LoadFromDatabaseForCompletionWorkbenchForFPRetriveAll(reader);


        }

        /// <summary>
        /// Retrieve User table records with specified primary key from the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// 





        public static b_WorkOrder ProcessRowForWorkOrderCostWidget(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workorder = new b_WorkOrder();

            // Load the object from the database
            workorder.LoadFromDatabaseForWorkOrderCostWidget(reader);

            // Return result
            return workorder;
        }

        public void LoadFromDatabaseForWorkOrderCostWidget(SqlDataReader reader)
        {
            int i = 0;
            try
            {


                PartCost = reader.GetDecimal(i++);

                LaborCost = reader.GetDecimal(i++);

                OtherCost = reader.GetDecimal(i++);

                TotalCost = reader.GetDecimal(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["PartCost"].ToString(); }
                catch { missing.Append("PartCost "); }

                try { reader["LaborCost"].ToString(); }
                catch { missing.Append("LaborCost "); }


                try { reader["OtherCost"].ToString(); }
                catch { missing.Append("OtherCost"); }


                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost"); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public void RetrieveForWorkOrderCostWidget(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref List<b_WorkOrder> data
)
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrder> results = null;
            data = new List<b_WorkOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrder_CostWidget.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrder>();
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
        public void RetrieveClientLookupIdBySearchCriteriaFromDatabase(
                SqlConnection connection,
                SqlTransaction transaction,
                long callerUserInfoId,
                string callerUserName,
                ref List<b_WorkOrder> data
            )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrder> results = null;
            data = new List<b_WorkOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrder_RetrieveClientLookupIdBySearchCriteria.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrder>();
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

        public void RetrieveByEquipmentIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            bool retrieveactive,
            ref List<b_WorkOrder> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrder> results = null;
            data = new List<b_WorkOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrder_RetrieveByEquipmentId.CallStoredProcedure(command, callerUserInfoId, callerUserName, retrieveactive, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrder>();
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

        public void RetrieveByEquipmentBIMGuidFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          long ClientId,
          Guid BIMGuid,
          ref List<b_WorkOrder> data
    )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrder> results = null;
            data = new List<b_WorkOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrder_RetrieveByEquipmentBIMGuid.CallStoredProcedure(command, callerUserInfoId, callerUserName, ClientId, BIMGuid);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrder>();
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


        public void RetrieveByLocationIdFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            bool retrieveactive,            // SOM-346
            ref List<b_WorkOrder> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrder> results = null;
            data = new List<b_WorkOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                //SOM-346
                results = Database.StoredProcedure.usp_WorkOrder_RetrieveByLocationId.CallStoredProcedure(command, callerUserInfoId, callerUserName, retrieveactive, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrder>();
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

        public void ValidateByClientLookupIdFromDatabase(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
               string callerUserName,
               ref List<b_StoredProcValidationError> data,
               bool createMode
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
                results = Database.StoredProcedure.usp_WorkOrder_ValidateByClientLookupId_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, createMode);

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


        //-----som-987 

        public void ValidateScheduleWorkByClientLookupIdFromDatabase(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_StoredProcValidationError> data,
          bool createMode
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
                results = Database.StoredProcedure.usp_WorkOrder_ScheduleWorkValidateByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this, createMode);

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

        //------end
        // SOM-1384 - Create Work Order based on sensor reading
        public void CreateForSensorReading(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName)
        {
            SqlCommand command = null;

            try
            {
                command = connection.CreateCommand();
                if (null != transaction)
                {
                    command.Transaction = transaction;
                }
                Database.StoredProcedure.usp_WorkOrder_CreateForSensorReading.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        //--------------------------Add work request for Sanitation----------------------------------------------------------------


        public void CreateFromOnDemandMaster(
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
                Database.StoredProcedure.usp_WorkOrder_CreateFromOnDemandMaster.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void CreateFromOnDemandMaster_V2(
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
                Database.StoredProcedure.usp_WorkOrder_CreateFromOnDemandMaster_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void InsertByForeignKeysIntoDatabase(
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
                Database.StoredProcedure.usp_WorkOrder_CreateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void InsertByForeignKeysForSanitationIntoDatabase(
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
                Database.StoredProcedure.usp_WorkOrder_CreateByForeignKeysForSanitation.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateByForeignKeysIntoDatabase(
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
                Database.StoredProcedure.usp_WorkOrder_UpdateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateWorkPlanner(
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
                Database.StoredProcedure.usp_WorkOrder_UpdatePlanner_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void CompleteWorkOrder(
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
                // We have our command and the SQL transaction is active
                // First - Update the work order 
                Database.StoredProcedure.usp_WorkOrder_UpdateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // Now Complete the tasks
                if (this.CompleteAllTasks)
                {
                    Database.StoredProcedure.usp_WorkOrder_UpdateTasksByWorkOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                }

                // Now add the timecard 
                Database.StoredProcedure.usp_TimeCard_CreateForCompleteWorkOrder.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // SOM-304 - If a prevmaint work order then update the pm as required
                if (this.SourceType == "PreventiveMaint" && this.SourceId > 0)
                    Database.StoredProcedure.usp_WorkOrder_UpdatePrevMaintOnComplete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void CompleteWorkOrderFromWizard(
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
                // We have our command and the SQL transaction is active
                // First - Update the work order 
                StoredProcedure.usp_WorkOrder_UpdateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                // as we are using UIConfiguraion feature so we are maintaining the WorkOrderUDF table also
                if (WorkOrderUDF != null)
                {
                    if (WorkOrderUDF.WorkOrderUDFId == 0)
                    {
                        StoredProcedure.usp_WorkOrderUDF_Create.CallStoredProcedure(command, callerUserInfoId, callerUserName, WorkOrderUDF);
                    }
                    else
                    {
                        StoredProcedure.usp_WorkOrderUDF_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, WorkOrderUDF);
                    }
                }

                // Now Complete the tasks
                if (this.CompleteAllTasks)
                {
                    StoredProcedure.usp_WorkOrder_UpdateTasksByWorkOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                }

                // if autoTimeCard = true
                if (AutoAddTimecard)
                {
                    StoredProcedure.usp_TimeCard_CreateForCompleteWorkOrder.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
                }

                // List Of Labor
                if (TimecardTab)
                {
                    foreach (var obj in TimecardList)
                    {
                        StoredProcedure.usp_Timecard_CreateByForeignKeys.CallStoredProcedure(command, callerUserInfoId, callerUserName, obj);
                    }
                }

                // SOM-304 - If a prevmaint work order then update the pm as required
                if (this.SourceType == "PreventiveMaint" && this.SourceId > 0)
                    Database.StoredProcedure.usp_WorkOrder_UpdatePrevMaintOnComplete.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateByWorkbenchIntoDatabase(
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
                Database.StoredProcedure.usp_Wo_Workbench_UpdateRecord.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //SOM-1479
        public void CancelWorkorder(
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
                Database.StoredProcedure.usp_WorkOrder_Cancel_Manual.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void UpdateTasksByWorkOrderId(
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
                Database.StoredProcedure.usp_WorkOrder_UpdateTasksByWorkOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void RetrieveByForeignKeysFromDatabase(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName
        )
        {
            Database.SqlClient.ProcessRow<b_WorkOrder> processRow = null;
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_WorkOrder>(reader => { this.LoadFromDatabaseByPKForeignKey(reader); return this; });
                Database.StoredProcedure.usp_WorkOrder_RetrieveByPKForeignKeys.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

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

        #region Workflow
        public void UpdateWorkflowObjectInDatabase(
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
                Database.StoredProcedure.usp_WorkOrder_UpdateByPK.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        //--------------------------------------Added By Indusnet Technologies-------------------------------

        public void RetrieveWorkOrderForPrint(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveWorkOrderForPrint.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveAllForSearch(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrievePersonnelInitial(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrievePersonnelInitial_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        //   public void RetrieveAllWorkOrderId(
        //SqlConnection connection,
        //SqlTransaction transaction,
        //long callerUserInfoId,
        //string callerUserName,
        //ref List<b_WorkOrder> results
        //)
        //   {
        //       SqlCommand command = null;
        //       string message = String.Empty;

        //       try
        //       {
        //           // Create the command to use in calling the stored procedures
        //           command = new SqlCommand();
        //           command.Connection = connection;
        //           command.Transaction = transaction;

        //           // Call the stored procedure to retrieve the data

        //           results = Database.StoredProcedure.usp_WorkOrder_RetriveAllWorkOrderId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

        //       }
        //       finally
        //       {
        //           if (null != command)
        //           {
        //               command.Dispose();
        //               command = null;
        //           }

        //           message = String.Empty;
        //           callerUserInfoId = 0;
        //           callerUserName = String.Empty;
        //       }
        //   }


        public void RetrieveAllWorkbenchSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_Wo_Workbench_RetrieveAll.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        //---som-987
        public void RetrieveUnscheduledWorkOrder(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_UnScheduledWorkOrders.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrieveByClientLookupId(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveByClientLookupId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveByScheduleId(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveByScheduleId.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void CreateAndValidateWorkOrderSchedule(
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
                Database.StoredProcedure.usp_WorkOrderSchedule_CreateAndValidateWorkOrderSchedule.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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


        public void AddScheduleRecord(
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
                Database.StoredProcedure.usp_WorkOrderSchedule_AddScheduleRecord_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void ReassignPersonnelScheduleRecord(
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
                Database.StoredProcedure.usp_WorkOrderSchedule_ReassignPersonnel_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public static b_WorkOrder ProcessRowRetrievePersonnelInitial(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();
            workOrder.LoadFromDatabaseRetrievePersonnelInitial(reader);
            return workOrder;
        }
        public int LoadFromDatabaseRetrievePersonnelInitial(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // Personnels column, nvarchar(512), not null               
                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = string.Empty;
                }
                i++;

                // PersonnelFull column, nvarchar(512), not null               
                if (false == reader.IsDBNull(i))
                {
                    PersonnelFull = reader.GetString(i);
                }
                else
                {
                    PersonnelFull = string.Empty;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["Personnels"].ToString(); }
                catch { missing.Append("Personnels "); }

                try { reader["PersonnelFull"].ToString(); }
                catch { missing.Append("PersonnelFull "); }

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


        public static b_WorkOrder ProcessRowForWorkOrderRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            // RKL - Do not need the LoadFromDatabaseWithDepartName method
            workOrder.LoadFromDatabaseForWorkOrderRetriveAllForSearch(reader);
            //workOrder.LoadFromDatabaseWithDepartName(reader);
            // Return result
            return workOrder;
        }
        public static b_WorkOrder ProcessRowForRetrieveAllWorkOrderId(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            // RKL - Do not need the LoadFromDatabaseWithDepartName method
            workOrder.LoadFromDatabaseRetrieveAllWorkOrderId(reader);
            //workOrder.LoadFromDatabaseWithDepartName(reader);
            // Return result
            return workOrder;
        }



        public static b_WorkOrder ProcessRowForWorkbenchRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            //workOrder.LoadFromDatabaseForWorkOrderRetriveAllForSearch(reader);
            workOrder.LoadFromDatabaseWithWorkbenchDetail(reader);
            // Return result
            return workOrder;
        }
        public static b_WorkOrder ProcessRowForCompletionWorkbenchForFPRetriveAll(SqlDataReader reader)
        {

            b_WorkOrder workOrder = new b_WorkOrder();


            workOrder.LoadFromDatabaseWithCompletionWorkbenchForFPDetail(reader);

            return workOrder;
        }
        public int LoadFromDatabaseForWorkOrderRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // Description
                Description = reader.GetString(i++);

                // Charge To Id  
                ChargeToId = reader.GetInt64(i++);

                // Charge To ClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);

                // Charge To Name
                ChargeTo_Name = reader.GetString(i++);

                // Requestor Personnel Client LookupId 
                Requestor_PersonnelClientLookupId = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

                // Shift - SOM-1082
                Shift = reader.GetString(i++);

                // Type  
                Type = reader.GetString(i++);

                // Priority
                Priority = reader.GetString(i++);

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

                // Department Name
                //if (false == reader.IsDBNull(i))
                //{
                //  DepartmentName = reader.GetString(i++);
                //}
                //else
                //{
                //  DepartmentName = ""; i++;
                //}

                // Creator
                if (false == reader.IsDBNull(i))
                {
                    Creator = reader.GetString(i++);
                }
                else
                {
                    Creator = ""; i++;
                }

                // Assigned (Work Assigned)
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i++);
                }
                else
                {
                    Assigned = ""; i++;
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

                // Actual Start Date
                if (false == reader.IsDBNull(i))
                {
                    ActualStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualStartDate = DateTime.MinValue;
                }
                i++;

                // Actual Finish Date
                if (false == reader.IsDBNull(i))
                {
                    ActualFinishDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinishDate = DateTime.MinValue;
                }
                i++;

                // failure Code
                if (false == reader.IsDBNull(i))
                {
                    FailureCode = reader.GetString(i++);
                }
                else
                {
                    FailureCode = ""; i++;
                }
                // Fitst 100 characters of CompleteComments 
                if (false == reader.IsDBNull(i))
                {
                    CompleteComments = reader.GetString(i++);
                }
                else
                {
                    CompleteComments = "";
                    i++;
                }
                // Required Date
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Requestor_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                // SOM-1082
                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["FailureCode"].ToString(); }
                catch { missing.Append("FailureCode "); }

                try { reader["ActualFinishDate"].ToString(); }
                catch { missing.Append("ActualFinishDate "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

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
        public int LoadFromDatabaseRetrieveAllWorkOrderId(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);
                Status = reader.GetString(i++);
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

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

        //==========som-987===============
        public static b_WorkOrder ProcessRowForUnscheduledWorkOrder(SqlDataReader reader)
        {

            b_WorkOrder workOrder = new b_WorkOrder();


            workOrder.LoadFromDatabaseForUnscheduledWorkOrder(reader);

            return workOrder;
        }
        public static b_WorkOrder ProcessRowForClientLookupId(SqlDataReader reader)
        {

            b_WorkOrder workOrder = new b_WorkOrder();


            workOrder.LoadFromDatabase(reader);

            return workOrder;
        }
        public static b_WorkOrder ProcessRowForListLaborSchedulingChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForListLaborSchedulingChunkSearch(reader);

            // Return result
            return obj;
        }
        public static b_WorkOrder ProcessRowForCalendarLaborSchedulingChunkSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForCalendarLaborSchedulingChunkSearch(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForListLaborSchedulingChunkSearch(SqlDataReader reader)
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
                //Pcount = reader.GetInt32(i++);
                //Scount = reader.GetInt32(i++);
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
                PartsOnOrder = reader.GetInt32(i++); //---V2-838
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
                //V2-838
                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }

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

        public int LoadFromDatabaseForCalendarLaborSchedulingChunkSearch(SqlDataReader reader)
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

                // PersonnelFull column, bigint, not null
                PersonnelFull = reader.GetString(i++);

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
                PartsOnOrder = reader.GetInt32(i++);  //V2-838
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
                //V2-838
                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }

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
        public int LoadFromDatabaseForUnscheduledWorkOrder(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description
                Description = reader.GetString(i++);
                // Status
                Status = reader.GetString(i++);

                // Charge To Id  
                ChargeToId = reader.GetInt64(i++);

                // ChargeType
                ChargeType = reader.GetString(i++);

                // Type  
                Type = reader.GetString(i++);

                // Charge To ClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);

                // Charge To ClientLookupId
                DownRequired = reader.GetBoolean(i++);

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

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType"); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

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

        //==========end=================== 

        public int LoadFromDatabaseForWorkbenchRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i++);
                }
                else
                {
                    ClientId = 0;
                    i++;
                }

                // WorkOrderId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderId = reader.GetInt64(i++);
                }
                else
                {
                    WorkOrderId = 0;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = string.Empty;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = string.Empty;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ChargeToId = reader.GetInt64(i++);
                }
                else
                {
                    ChargeToId = 0;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                    i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_PersonnelId = reader.GetInt64(i++);
                }
                else
                {
                    WorkAssigned_PersonnelId = 0;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_PersonnelClientLookupId = reader.GetString(i++);
                }
                else
                {
                    WorkAssigned_PersonnelClientLookupId = string.Empty;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i++);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                    i++;
                }

                ScheduledDuration = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i++);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    Createby = reader.GetString(i++);
                }
                else
                {
                    Createby = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i++);
                }
                else
                {
                    ModifyBy = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i++);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    DeniedReason = Convert.ToString(reader[i++]);
                }
                else
                {
                    DeniedReason = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    DeniedDate = reader.GetDateTime(i++);
                }
                else
                {
                    DeniedDate = DateTime.MinValue;
                    i++;
                }

                var s = reader[i];
                if (false == reader.IsDBNull(i))
                {
                    DeniedBy_PersonnelId = reader.GetInt64(i++);
                }
                else
                {
                    DeniedBy_PersonnelId = 0;
                    i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i++);
                }
                else
                {
                    UpdateIndex = 0;
                    i++;
                }



            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Requestor_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelClientLookupId "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["ScheduledDuration"].ToString(); }
                catch { missing.Append("ScheduledDuration "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy"].ToString(); }
                catch { missing.Append("CreateBy "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                try { reader["DeniedReason"].ToString(); }
                catch { missing.Append("DeniedReason "); }

                try { reader["DeniedDate"].ToString(); }
                catch { missing.Append("DeniedDate "); }

                try { reader["DeniedBy_PersonnelId"].ToString(); }
                catch { missing.Append("DeniedBy_PersonnelId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

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



        // SOM-687
        public int LoadFromDatabaseForCompletionWorkbenchForFPRetriveAll(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                if (false == reader.IsDBNull(i))
                {
                    ClientId = reader.GetInt64(i++);
                }
                else
                {
                    ClientId = 0;
                    i++;
                }

                // WorkOrderId column, bigint, not null
                if (false == reader.IsDBNull(i))
                {
                    WorkOrderId = reader.GetInt64(i++);
                }
                else
                {
                    WorkOrderId = 0;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = string.Empty;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = string.Empty;
                    i++;
                }


                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ChargeToId = reader.GetInt64(i++);
                }
                else
                {
                    ChargeToId = 0;
                    i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = string.Empty;
                    i++;
                }
                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ChargeToClientLookupId = string.Empty;
                    i++;
                }

                // SOM-687
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i);
                }
                else
                {
                    WorkAssigned_Name = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i++);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                    i++;
                }

                ScheduledDuration = reader.GetDecimal(i++);

                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i++);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    CreateBy_PersonnelName = reader.GetString(i++);
                }
                else
                {
                    CreateBy_PersonnelName = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ModifyBy = reader.GetString(i++);
                }
                else
                {
                    ModifyBy = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    ModifyDate = reader.GetDateTime(i++);
                }
                else
                {
                    ModifyDate = DateTime.MinValue;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    DeniedReason = Convert.ToString(reader[i++]);
                }
                else
                {
                    DeniedReason = string.Empty;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    DeniedDate = reader.GetDateTime(i++);
                }
                else
                {
                    DeniedDate = DateTime.MinValue;
                    i++;
                }

                var s = reader[i];
                if (false == reader.IsDBNull(i))
                {
                    DeniedBy_PersonnelId = reader.GetInt64(i++);
                }
                else
                {
                    DeniedBy_PersonnelId = 0;
                    i++;
                }

                if (false == reader.IsDBNull(i))
                {
                    UpdateIndex = reader.GetInt32(i++);
                }
                else
                {
                    UpdateIndex = 0;
                    i++;
                }



            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["ScheduledDuration"].ToString(); }
                catch { missing.Append("ScheduledDuration "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }

                try { reader["ModifyBy"].ToString(); }
                catch { missing.Append("ModifyBy "); }

                try { reader["ModifyDate"].ToString(); }
                catch { missing.Append("ModifyDate "); }

                try { reader["DeniedReason"].ToString(); }
                catch { missing.Append("DeniedReason "); }

                try { reader["DeniedDate"].ToString(); }
                catch { missing.Append("DeniedDate "); }

                try { reader["DeniedBy_PersonnelId"].ToString(); }
                catch { missing.Append("DeniedBy_PersonnelId "); }

                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

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
        ///  SOM-1278
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public int LoadFromDB(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);
                // Work Order ID
                WorkOrderId = reader.GetInt64(i++);
                // ClientLookupId
                ClientLookupId = reader.GetString(i++);
                // Description
                Description = reader.GetString(i++);
                // ChargeType
                ChargeType = reader.GetString(i++);
                // ChargeToId
                ChargeToId = reader.GetInt64(i++);
                // ChargeToClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);
                // ChargeToName
                ChargeTo_Name = reader.GetString(i++);
                // Type
                Type = reader.GetString(i++);
                // Status
                Status = reader.GetString(i++);
                // Shift
                Shift = reader.GetString(i++);
                // Priority
                Priority = reader.GetString(i++);

                // CreateDate
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i++);
                }
                else
                {
                    CreateDate = DateTime.MinValue;
                    i++;
                }
                // Creator_PersonnelId 
                Creator_PersonnelId = reader.GetInt64(i++);
                // CreateBy_PersonnelName
                if (false == reader.IsDBNull(i))
                {
                    CreateBy_PersonnelName = reader.GetString(i++);
                }
                else
                {
                    CreateBy_PersonnelName = string.Empty;
                    i++;
                }
                // WorkAssigned_PersonnelId
                WorkAssigned_PersonnelId = reader.GetInt64(i++);
                // Work Assigned Name
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i++);
                }
                else
                {
                    WorkAssigned_Name = string.Empty;
                    i++;
                }
                // ScheduleStartDate
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i++);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                    i++;
                }
                // SourceType
                SourceType = reader.GetString(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeType"].ToString(); }
                catch { missing.Append("ChargeType "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Creator_PersonnelId"].ToString(); }
                catch { missing.Append("Creator_PersonnelId "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

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

        public void WorkOrder_CreateForPrevMaintenance(SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref b_WorkOrder workorder)
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

                Database.StoredProcedure.usp_WorkOrder_CreateForPrevMaintenance.CallStoredProcedure
                    (command, callerUserInfoId, callerUserName, ref workorder);

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

        public void WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary(SqlConnection connection,
     SqlTransaction transaction,
     long callerUserInfoId,
     string callerUserName,
     ref b_WorkOrder workorder)
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

                Database.StoredProcedure.usp_WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary.CallStoredProcedure
                    (command, callerUserInfoId, callerUserName, ref workorder);

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

        public void WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2(SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref b_WorkOrder workorder)
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

                Database.StoredProcedure.usp_WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2.CallStoredProcedure
                    (command, callerUserInfoId, callerUserName, ref workorder);

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
        #region New code implementation for grid load and advance search


        public void RetrieveV2Search(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveChunkForSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveV2Print(
   SqlConnection connection,
   SqlTransaction transaction,
   long callerUserInfoId,
   string callerUserName,
   ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveChunkForPrint_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrder ProcessWRDashboard(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();

            workOrder.LoadFromDatabaseForWRDashboard(reader);
            return workOrder;
        }

        public int LoadFromDatabaseForWRDashboard(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // Description
                Description = reader.GetString(i++);

                // Charge To Id  
                ChargeToId = reader.GetInt64(i++);

                // Charge To ClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);

                // Charge To Name
                ChargeTo_Name = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

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

                // Assigned (Work Assigned)
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i++);
                }
                else
                {
                    Assigned = ""; i++;
                }
                // Scheduled Finish Date
                if (false == reader.IsDBNull(i))
                {
                    ScheduledFinishDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledFinishDate = DateTime.MinValue;
                }
                i++;

                // Complete  Date
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                TotalCount = reader.GetInt32(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                // SOM-1082
                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["ScheduledFinishDate"].ToString(); }
                catch { missing.Append("ScheduledFinishDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

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
        public static b_WorkOrder ProcessRetrieveV2(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();

            workOrder.LoadFromDatabaseForWorkOrderRetrieveV2(reader);
            return workOrder;
        }

        public int LoadFromDatabaseForWorkOrderRetrieveV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // Description
                Description = reader.GetString(i++);

                // Charge To Id  
                ChargeToId = reader.GetInt64(i++);

                // Charge To ClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);

                // Charge To Name
                ChargeTo_Name = reader.GetString(i++);

                // AssetLocation
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                // Requestor Personnel Client LookupId 
                Requestor_PersonnelClientLookupId = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

                // Shift - SOM-1082
                Shift = reader.GetString(i++);

                // Type  
                Type = reader.GetString(i++);

                // Priority
                Priority = reader.GetString(i++);

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


                // Creator
                if (false == reader.IsDBNull(i))
                {
                    Creator = reader.GetString(i);
                }
                else
                {
                    Creator = "";
                }
                i++;
                // Assigned (Work Assigned)
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = "";
                }
                i++;
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

                // Actual Start Date
                if (false == reader.IsDBNull(i))
                {
                    ActualStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualStartDate = DateTime.MinValue;
                }
                i++;

                // Actual Finish Date
                if (false == reader.IsDBNull(i))
                {
                    ActualFinishDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinishDate = DateTime.MinValue;
                }
                i++;

                // failure Code
                if (false == reader.IsDBNull(i))
                {
                    FailureCode = reader.GetString(i);
                }
                else
                {
                    FailureCode = "";
                }
                i++;
                // Fitst 100 characters of CompleteComments 
                if (false == reader.IsDBNull(i))
                {
                    CompleteComments = reader.GetString(i);
                }
                else
                {
                    CompleteComments = "";

                }
                i++;
                // Required Date
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                //V2-271
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
                    AssignedFullName = reader.GetString(i);

                }
                else
                {
                    AssignedFullName = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = "";

                }
                i++;
                WorkAssigned_PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientlookupId = "";

                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientlookupId = "";

                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup3ClientlookupId = "";

                }
                i++;
                AssetGroup1Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                AssetGroup2Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                AssetGroup3Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                ActualDuration = reader.GetDecimal(i++);

                SourceType = reader.GetString(i++);//<!--Added on 23/06/2020-->
                PartsOnOrder = reader.GetInt32(i++);//
                //****V2-850***
                if (false == reader.IsDBNull(i))
                {
                    ProjectClientLookupId = reader.GetString(i);
                }
                else
                {
                    ProjectClientLookupId = "";
                }
                i++;
                //******V2-892***             
                DownRequired = reader.GetBoolean(i);
                i++;
                //V2-1078
                Planner_PersonnelId = reader.GetInt64(i++);
                if (false == reader.IsDBNull(i))
                {
                    PlannerFullName = reader.GetString(i);
                }
                else
                {
                    PlannerFullName = "";

                }
                i++;
                //******
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }

                try { reader["Requestor_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                // SOM-1082
                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["FailureCode"].ToString(); }
                catch { missing.Append("FailureCode "); }

                try { reader["ActualFinishDate"].ToString(); }
                catch { missing.Append("ActualFinishDate "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }
                //V2-271
                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["AssignedFullName"].ToString(); }
                catch { missing.Append("AssignedFullName "); }

                try { reader["Personnels"].ToString(); }
                catch { missing.Append("Personnels "); }

                try { reader["AssetGroup1ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup1ClientlookupId "); }

                try { reader["AssetGroup2ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup2ClientlookupId "); }

                try { reader["AssetGroup3ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup3ClientlookupId "); }

                //<!--(Added on 25/06/2020)-->
                try { reader["AssetGroup1Id"].ToString(); }
                catch { missing.Append("AssetGroup1Id "); }

                try { reader["AssetGroup2Id"].ToString(); }
                catch { missing.Append("AssetGroup2Id "); }

                try { reader["AssetGroup3Id"].ToString(); }
                catch { missing.Append("AssetGroup3Id "); }
                //<!--(Added on 25/06/2020)-->

                try { reader["ActualDuration"].ToString(); }
                catch { missing.Append("ActualDuration "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }

                try { reader["Project_ClientLookupId"].ToString(); }
                catch { missing.Append("Project_ClientLookupId "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }

                try { reader["PlannerFullName"].ToString(); }
                catch { missing.Append("PlannerFullName "); }

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

        public static b_WorkOrder ProcessRowForWorkOrderPrint(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();

            workOrder.LoadFromDatabaseForWorkOrderPrint(reader);
            return workOrder;
        }

        public void LoadFromDatabaseForWorkOrderPrint(SqlDataReader reader)
        {
            //  int i = this.LoadFromDatabase(reader);
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(15), not null
                ClientLookupId = reader.GetString(i++);

                // ChargeTo_Name column, nvarchar(63), not null
                ChargeTo_Name = reader.GetString(i++);

                // CompleteBy_PersonnelId column, bigint, not null
                CompleteBy_PersonnelId = reader.GetInt64(i++);

                // CompleteComments column, nvarchar(MAX), not null
                CompleteComments = reader.GetString(i++);

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

                // Description column, nvarchar(MAX), not null
                Description = reader.GetString(i++);

                // DownRequired column, bit, not null
                DownRequired = reader.GetBoolean(i++);
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

                // ScheduledDuration column, decimal(8,2), not null
                ScheduledDuration = reader.GetDecimal(i++);

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

                // SignoffBy_PersonnelId column, bigint, not null
                SignoffBy_PersonnelId = reader.GetInt64(i++);

                // Status column, nvarchar(15), not null
                Status = reader.GetString(i++);

                // Type column, nvarchar(15), not null
                Type = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    ChargeToClientLookupId = reader.GetString(i);
                }
                else
                {
                    ChargeToClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SignoffBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    SignoffBy_PersonnelClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SignoffBy_PersonnelClientLookupIdName = reader.GetString(i);
                }
                else
                {
                    SignoffBy_PersonnelClientLookupIdName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelClientLookupId = reader.GetString(i);
                }
                else
                {
                    CompleteBy_PersonnelClientLookupId = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    Createby = reader.GetString(i);
                }
                else
                {
                    Createby = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    CreateBy_PersonnelName = reader.GetString(i);
                }
                else
                {
                    CreateBy_PersonnelName = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy_PersonnelName = reader.GetString(i);
                }
                else
                {
                    CompleteBy_PersonnelName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssignedFullName = reader.GetString(i);
                }
                else
                {
                    AssignedFullName = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;

                EquipmentType = reader.GetString(i++);

                SerialNumber = reader.GetString(i++);

                Make = reader.GetString(i++);

                Model = reader.GetString(i++);

                AssetGroup1ClientlookupId = reader.GetString(i++);

                AssetGroup2ClientlookupId = reader.GetString(i++);

                AssetGroup3ClientlookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    RemoveFromService = reader.GetBoolean(i);
                }
                else
                {
                    RemoveFromService = false;
                }
                i++;

                ChargeToId = reader.GetInt64(i++);

                WOCompCriteriaTitle = reader.GetString(i++);

                WOCompCriteria = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    WOCompCriteriaTab = reader.GetBoolean(i);
                }
                else
                {
                    WOCompCriteriaTab = false;
                }
                i++;

                // Priority column, nvarchar(15), not null
                Priority = reader.GetString(i++);

                // Labor Account Number
                if (false == reader.IsDBNull(i))
                {
                    AccountClientLookupId = reader.GetString(i);
                }
                else
                {
                    AccountClientLookupId = "";
                }
                i++;
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["CompleteBy_PersonnelId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelId "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["DownRequired"].ToString(); }
                catch { missing.Append("DownRequired "); }


                try { reader["RequiredDate"].ToString(); }
                catch { missing.Append("RequiredDate "); }

                try { reader["ScheduledDuration"].ToString(); }
                catch { missing.Append("ScheduledDuration "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["SignoffBy_PersonnelId"].ToString(); }
                catch { missing.Append("SignoffBy_PersonnelId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["SignoffBy_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("SignoffBy_PersonnelClientLookupId "); }

                try { reader["CompleteBy_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelClientLookupId "); }

                try { reader["Assigned"].ToString(); }
                catch { missing.Append("Assigned "); }

                try { reader["Createby"].ToString(); }
                catch { missing.Append("Createby "); }

                try { reader["CreateBy_PersonnelName"].ToString(); }
                catch { missing.Append("CreateBy_PersonnelName "); }

                try { reader["CompleteBy_PersonnelName"].ToString(); }
                catch { missing.Append("CompleteBy_PersonnelName "); }

                try { reader["AssignedFullName"].ToString(); }
                catch { missing.Append("AssignedFullName "); }

                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }

                try { reader["EquipmentType"].ToString(); }
                catch { missing.Append("EquipmentType "); }

                try { reader["SerialNumber"].ToString(); }
                catch { missing.Append("SerialNumber "); }

                try { reader["Make"].ToString(); }
                catch { missing.Append("Make "); }

                try { reader["Model"].ToString(); }
                catch { missing.Append("Model "); }

                try { reader["AssetGroup1ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup1ClientlookupId "); }

                try { reader["AssetGroup2ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup2ClientlookupId "); }

                try { reader["AssetGroup3ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup3ClientlookupId "); }

                try { reader["RemoveFromService"].ToString(); }
                catch { missing.Append("RemoveFromService "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["WOCompCriteriaTitle"].ToString(); }
                catch { missing.Append("WOCompCriteriaTitle "); }

                try { reader["WOCompCriteria"].ToString(); }
                catch { missing.Append("WOCompCriteria "); }

                try { reader["UseWOCompletionWizard"].ToString(); }
                catch { missing.Append("UseWOCompletionWizard "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["AccountClientLookupId"].ToString(); }
                catch { missing.Append("AccountClientLookupId "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        public static b_WorkOrder ProcessRowAttachment(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();

            workOrder.LoadFromDatabaseForAttachmentV2(reader);
            return workOrder;
        }

        public int LoadFromDatabaseForAttachmentV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // Description
                Description = reader.GetString(i++);

                // Charge To Id  
                ChargeToId = reader.GetInt64(i++);

                // Charge To ClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);

                // Charge To Name
                ChargeTo_Name = reader.GetString(i++);

                // AssetLocation
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                // Requestor Personnel Client LookupId 
                Requestor_PersonnelClientLookupId = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

                // Shift - SOM-1082
                Shift = reader.GetString(i++);

                // Type  
                Type = reader.GetString(i++);

                // Priority
                Priority = reader.GetString(i++);

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


                // Creator
                if (false == reader.IsDBNull(i))
                {
                    Creator = reader.GetString(i);
                }
                else
                {
                    Creator = "";
                }
                i++;
                // Assigned (Work Assigned)
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i);
                }
                else
                {
                    Assigned = "";
                }
                i++;
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

                // Actual Start Date
                if (false == reader.IsDBNull(i))
                {
                    ActualStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualStartDate = DateTime.MinValue;
                }
                i++;

                // Actual Finish Date
                if (false == reader.IsDBNull(i))
                {
                    ActualFinishDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinishDate = DateTime.MinValue;
                }
                i++;

                // failure Code
                if (false == reader.IsDBNull(i))
                {
                    FailureCode = reader.GetString(i);
                }
                else
                {
                    FailureCode = "";
                }
                i++;
                // Fitst 100 characters of CompleteComments 
                if (false == reader.IsDBNull(i))
                {
                    CompleteComments = reader.GetString(i);
                }
                else
                {
                    CompleteComments = "";

                }
                i++;
                // Required Date
                if (false == reader.IsDBNull(i))
                {
                    RequiredDate = reader.GetDateTime(i);
                }
                else
                {
                    RequiredDate = DateTime.MinValue;
                }
                i++;
                //V2-271
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
                    AssignedFullName = reader.GetString(i);

                }
                else
                {
                    AssignedFullName = "";

                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = "";

                }
                i++;
                WorkAssigned_PersonnelId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup1ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup1ClientlookupId = "";

                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup2ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup2ClientlookupId = "";

                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    AssetGroup3ClientlookupId = reader.GetString(i);
                }
                else
                {
                    AssetGroup3ClientlookupId = "";

                }
                i++;
                AssetGroup1Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                AssetGroup2Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                AssetGroup3Id = reader.GetInt64(i++);//<!--(Added on 25/06/2020)-->
                ActualDuration = reader.GetDecimal(i++);

                SourceType = reader.GetString(i++);//<!--Added on 23/06/2020-->
                PartsOnOrder = reader.GetInt32(i++);//
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }

                try { reader["Requestor_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                // SOM-1082
                try { reader["Shift"].ToString(); }
                catch { missing.Append("Shift "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["FailureCode"].ToString(); }
                catch { missing.Append("FailureCode "); }

                try { reader["ActualFinishDate"].ToString(); }
                catch { missing.Append("ActualFinishDate "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }
                //V2-271
                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["AssignedFullName"].ToString(); }
                catch { missing.Append("AssignedFullName "); }

                try { reader["Personnels"].ToString(); }
                catch { missing.Append("Personnels "); }

                try { reader["AssetGroup1ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup1ClientlookupId "); }

                try { reader["AssetGroup2ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup2ClientlookupId "); }

                try { reader["AssetGroup3ClientlookupId"].ToString(); }
                catch { missing.Append("AssetGroup3ClientlookupId "); }

                //<!--(Added on 25/06/2020)-->
                try { reader["AssetGroup1Id"].ToString(); }
                catch { missing.Append("AssetGroup1Id "); }

                try { reader["AssetGroup2Id"].ToString(); }
                catch { missing.Append("AssetGroup2Id "); }

                try { reader["AssetGroup3Id"].ToString(); }
                catch { missing.Append("AssetGroup3Id "); }
                //<!--(Added on 25/06/2020)-->

                try { reader["ActualDuration"].ToString(); }
                catch { missing.Append("ActualDuration "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }


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

        //------------------------------------End Added By Indusnet Technologies-----------------------------

        public void RetrieveAllCompletionWorkbenchForFPSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WO_CompletionWorkbenchFP_RetrieveAll.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        //-------WorkRequest Retrieve--------------------------------------------------------------------
        public void RetrieveAllWorkRequestForSearch(SqlConnection connection, SqlTransaction transaction, long callerUserInfoId, string callerUserName, ref List<b_WorkOrder> results)
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
                results = Database.StoredProcedure.usp_WorkRequest_RetrieveAllForSearch.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public int LoadFromDatabaseForWorkRequestRetriveAllForSearch(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // Description
                Description = reader.GetString(i++);

                // Charge To Id  
                ChargeToId = reader.GetInt64(i++);

                // Charge To ClientLookupId
                ChargeToClientLookupId = reader.GetString(i++);

                // Charge To Name
                ChargeTo_Name = reader.GetString(i++);

                // Requestor Personnel Client LookupId 
                Requestor_PersonnelClientLookupId = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

                // Type  
                Type = reader.GetString(i++);

                // Priority
                Priority = reader.GetString(i++);

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

                // Department Name
                if (false == reader.IsDBNull(i))
                {
                    DepartmentName = reader.GetString(i++);
                }
                else
                {
                    DepartmentName = ""; i++;
                }

                // Creator
                if (false == reader.IsDBNull(i))
                {
                    Creator = reader.GetString(i++);
                }
                else
                {
                    Creator = ""; i++;
                }

                // Assigned (Work Assigned)
                if (false == reader.IsDBNull(i))
                {
                    Assigned = reader.GetString(i++);
                }
                else
                {
                    Assigned = ""; i++;
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

                // Actual Start Date
                if (false == reader.IsDBNull(i))
                {
                    ActualStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualStartDate = DateTime.MinValue;
                }
                i++;

                // Actual Finish Date
                if (false == reader.IsDBNull(i))
                {
                    ActualFinishDate = reader.GetDateTime(i);
                }
                else
                {
                    ActualFinishDate = DateTime.MinValue;
                }
                i++;

                // failure Code
                if (false == reader.IsDBNull(i))
                {
                    FailureCode = reader.GetString(i++);
                }
                else
                {
                    FailureCode = ""; i++;
                }
                // schedule Finish Date
                if (false == reader.IsDBNull(i))
                {
                    ScheduledFinishDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledFinishDate = DateTime.MinValue;
                }
                i++;
                // Complete Date
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                // SOM-1123
                // Denied Reason
                DeniedReason = reader.GetString(i);
                i++;
                // Denied Comment
                DeniedComment = reader.GetString(i);
                i++;
                // Denied By Personnel ClientLookupId 
                DeniedBy_PersonnelId_ClientLookupId = reader.GetString(i);
                i++;
                // Denied By Personnel Name
                DeniedBy_PersonnelId_Name = reader.GetString(i);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeToClientLookupId"].ToString(); }
                catch { missing.Append("ChargeToClientLookupId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Requestor_PersonnelClientLookupId"].ToString(); }
                catch { missing.Append("Requestor_PersonnelClientLookupId "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["FailureCode"].ToString(); }
                catch { missing.Append("FailureCode "); }

                try { reader["ActualFinishDate"].ToString(); }
                catch { missing.Append("ActualFinishDate "); }

                try { reader["ScheduledFinishDate"].ToString(); }
                catch { missing.Append("ScheduledFinishDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }
                // SOM-1123
                try { reader["DeniedReason"].ToString(); }
                catch { missing.Append("DeniedReason "); }

                try { reader["DeniedComment"].ToString(); }
                catch { missing.Append("DeniedComment "); }

                try { reader["DeniedBy_PersonnelId_ClientLookupId"].ToString(); }
                catch { missing.Append("DeniedBy_PersonnelId_ClientLookupId "); }

                try { reader["DeniedBy_PersonnelId_Name"].ToString(); }
                catch { missing.Append("DeniedBy_PersonnelId_Name "); }

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
        public static b_WorkOrder ProcessRowForWorkRequestRetriveAllForSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            // RKL - Do not need the LoadFromDatabaseWithDepartName method
            workOrder.LoadFromDatabaseForWorkRequestRetriveAllForSearch(reader);
            //workOrder.LoadFromDatabaseWithDepartName(reader);
            // Return result
            return workOrder;
        }


        //-SOM: 928
        public void WorkOrder_RetrieveWorkOrderListByMeterIdAndReadingDate(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveWorkOrderListByMeterIdAndReadingDate.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void WORetrieveForExtraction(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
            string callerUserName,
            ref List<b_WorkOrder> data
        )
        {

            SqlCommand command = null;
            string message = String.Empty;
            List<b_WorkOrder> results = null;
            data = new List<b_WorkOrder>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkOrder_RetrieveForExtraction.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

                if (results != null)
                {
                    data = results;
                }
                else
                {
                    data = new List<b_WorkOrder>();
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
        public static b_WorkOrder ProcessRowForExtract(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForExtract(reader);

            // Return result
            return obj;
        }
        public void LoadFromDatabaseForExtract(SqlDataReader reader)
        {
            int i = 0;

            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                WorkOrderNo = reader.GetString(i++);
                // Status 
                Status = reader.GetString(i++);

                // CompleteBy 
                if (false == reader.IsDBNull(i))
                {
                    CompleteBy = reader.GetString(i);
                }
                else
                {
                    CompleteBy = "";
                }
                i++;
                // Complete Date
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }
                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;
                CompleteComments = reader.GetString(i++);
                // ScheduledDate
                if (false == reader.IsDBNull(i))
                {
                    ScheduledStartDate = reader.GetDateTime(i);
                }
                else
                {
                    ScheduledStartDate = DateTime.MinValue;
                }
                i++;
                // Assigned To
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i);
                }
                else
                {
                    WorkAssigned_Name = "";
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

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["WorkOrderNo"].ToString(); }
                catch { missing.Append("WorkOrderNo "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["CompleteBy"].ToString(); }
                catch { missing.Append("CompleteBy "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["CompleteComments"].ToString(); }
                catch { missing.Append("CompleteComments "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                StringBuilder msg = new StringBuilder();
                msg.Append(string.Format("Exception occurred at index {0}: {1}", i, ex.Message));
                if (missing.Length > 0)
                {
                    msg.Append(string.Format(" The following columns were expected but not found: {0}", missing.ToString()));
                }

                throw new Exception(msg.ToString(), ex);
            }
        }

        //------------------------------------------------------------------------
        public static b_WorkOrder ProcessRowForWorkOrderRetriveAllForMeter(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder workOrder = new b_WorkOrder();

            // Load the object from the database
            // RKL - Do not need the LoadFromDatabaseWithDepartName method
            workOrder.LoadFromDatabase(reader);
            //workOrder.LoadFromDatabaseWithDepartName(reader);
            // Return result
            return workOrder;
        }
        public void ValidateByEquipmentLocation(
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
            //data = new List<b_StoredProcValidationError>();

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                results = Database.StoredProcedure.usp_WorkWorder_ValidateBy_Equipment_Location.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        /// <summary>
        /// Retrieve all WorkOrder table records represented by this object in the database.
        /// </summary>
        /// <param name="connection">SqlConnection containing the database connection</param>
        /// <param name="transaction">SqlTransaction containing the database transaction</param>
        /// <param name="callerUserInfoId">long that identifies the user calling the database</param>
        /// <param name="callerUserName">string that identifies the user calling the database</param>
        /// <param name="data">b_WorkOrder[] that contains the results</param>
        public void RetrieveAll_V2(
            SqlConnection connection,
            SqlTransaction transaction,
            long callerUserInfoId,
      string callerUserName,
            ref b_WorkOrder[] data
        )
        {
            Database.SqlClient.ProcessRow<b_WorkOrder> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_WorkOrder[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_WorkOrder>(reader => { b_WorkOrder obj = new b_WorkOrder(); obj.LoadFromDatabase(reader); return obj; });
                results = Database.StoredProcedure.usp_WorkOrder_RetrieveAll_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, ClientId, SiteId);

                // Extract the results
                if (null != results)
                {
                    data = (b_WorkOrder[])results.ToArray(typeof(b_WorkOrder));
                }
                else
                {
                    data = new b_WorkOrder[0];
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

        public void WRDashboardRetrieveAllForSearch(
  SqlConnection connection,
  SqlTransaction transaction,
  long callerUserInfoId,
  string callerUserName,
  ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WorkOrder_WRDashboardRetrieveAllForSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void WorkOrder_WRDashboardRetrieveAllForPrint_V2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_WRDashboardRetrieveAllForPrint_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        public void RetrievePOandPR(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
     string callerUserName,
           ref b_WorkOrder[] data
       )
        {
            Database.SqlClient.ProcessRow<b_WorkOrder> processRow = null;
            ArrayList results = null;
            SqlCommand command = null;
            string message = String.Empty;

            // Initialize the results
            data = new b_WorkOrder[0];

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                // Call the stored procedure to retrieve the data
                processRow = new Database.SqlClient.ProcessRow<b_WorkOrder>(reader => { b_WorkOrder obj = new b_WorkOrder(); obj.LoadFromDatabaseforPOandPR(reader); return obj; });
                results = Database.StoredProcedure.usp_WorkOrder_RetrievePOandPR_V2.CallStoredProcedure(command, processRow, callerUserInfoId, callerUserName, this);

                // Extract the results
                if (null != results)
                {
                    data = (b_WorkOrder[])results.ToArray(typeof(b_WorkOrder));
                }
                else
                {
                    data = new b_WorkOrder[0];
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


        public int LoadFromDatabaseforPOandPR(SqlDataReader reader)
        {
            int i = 0;
            try
            {



                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                if (false == reader.IsDBNull(i))
                {
                    WONumber = reader.GetString(i);
                }
                else
                {
                    WONumber = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    POType = reader.GetString(i);
                }
                else
                {
                    POType = "";
                }
                i++;


                if (false == reader.IsDBNull(i))
                {
                    PONumber = reader.GetString(i);
                }
                else
                {
                    PONumber = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    POStatus = reader.GetString(i);
                }
                else
                {
                    POStatus = "";
                }
                i++;

                LineNumber = reader.GetInt32(i++);



                if (false == reader.IsDBNull(i))
                {
                    LineStatus = reader.GetString(i);
                }
                else
                {
                    LineStatus = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    LineDesc = reader.GetString(i);
                }
                else
                {
                    LineDesc = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorClientlookupId = reader.GetString(i);
                }
                else
                {
                    VendorClientlookupId = "";
                }
                i++;

                if (false == reader.IsDBNull(i))
                {
                    VendorName = reader.GetString(i);
                }
                else
                {
                    VendorName = "";
                }
                i++;

                // Create Date 
                if (false == reader.IsDBNull(i))
                {
                    CreateDate = reader.GetDateTime(i);
                }
                else
                {
                    // Create Date should never be null
                    CreateDate = DateTime.Now;
                }
                i++;

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }


                try { reader["WONumber"].ToString(); }
                catch { missing.Append("WONumber "); }


                try { reader["POType"].ToString(); }
                catch { missing.Append("POType "); }

                try { reader["PONumber"].ToString(); }
                catch { missing.Append("PONumber "); }

                try { reader["POStatus"].ToString(); }
                catch { missing.Append("POStatus "); }

                try { reader["LineNumber"].ToString(); }
                catch { missing.Append("LineNumber "); }

                try { reader["LineStatus"].ToString(); }
                catch { missing.Append("LineStatus "); }

                try { reader["LineDesc"].ToString(); }
                catch { missing.Append("LineDesc "); }

                try { reader["VendorClientlookupId"].ToString(); }
                catch { missing.Append("VendorClientlookupId "); }

                try { reader["VendorName"].ToString(); }
                catch { missing.Append("VendorName "); }

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

        public void UpdateOnRemovingSchedule(
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
                Database.StoredProcedure.usp_WorkOrder_UpdateOnRemovingSchedule.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdateListPartsonOrder(
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
                Database.StoredProcedure.usp_WorkOrder_UpdateListPartsonOrder_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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

        public void UpdatePartsonOrder(
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
                Database.StoredProcedure.usp_WorkOrder_UpdatePartsonOrder_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        public void RetrieveListForLaborSchedulingSearch(
           SqlConnection connection,
           SqlTransaction transaction,
           long callerUserInfoId,
           string callerUserName,
           ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_List_RetrieveForSearchByLaborScheduling_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveCalendarForLaborSchedulingSearch(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_Calendar_RetrieveForSearchByLaborScheduling_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public static b_WorkOrder ProcessRowForAvailableWorkLaborSchedulingSearch(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForAvailableWorkLaborSchedulingSearch(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForAvailableWorkLaborSchedulingSearch(SqlDataReader reader)
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

                PartsOnOrder = reader.GetInt32(i++); //V2-838
                #region V2-984

                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = "";

                }
                i++;
                WorkAssigned_PersonnelId = reader.GetInt64(i++);
                #endregion 
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

                //V2-838
                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }

                //V2-984

                try { reader["Personnels"].ToString(); }
                catch { missing.Append("Personnels "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

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

        public void RetrieveAvailableWorkForLaborSchedulingSearch(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_GetWorkOrderBySearchCriteria_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public void RetrieveApprovedWorkOrderForLaborScheduling(
        SqlConnection connection,
        SqlTransaction transaction,
        long callerUserInfoId,
        string callerUserName,
        ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_Workorder_RetrieveApprovedWOForLaborScheduling_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrder ProcessRowForCalendarWorkOrderList(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseCalendarWorkOrderList(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseCalendarWorkOrderList(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                ClientLookupId = reader.GetString(i++);

                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = "";
                    i++;
                }
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

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
        public static b_WorkOrder ProcessRowForAvailableWorkLaborSchedulingSearchCalendar(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForAvailableWorkLaborSchedulingSearchCalendar(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForAvailableWorkLaborSchedulingSearchCalendar(SqlDataReader reader)
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

                PartsOnOrder = reader.GetInt32(i++);//V2-838
                #region V2-984

                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = "";

                }
                i++;
                WorkAssigned_PersonnelId = reader.GetInt64(i++);
                #endregion 
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
                //V2-838
                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }
                //V2-984

                try { reader["Personnels"].ToString(); }
                catch { missing.Append("Personnels "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

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

        public void RetrieveAvailableWorkForLaborSchedulingSearchCalendar(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_GetWorkOrderBySearchCriteriaForCalendar_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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


        #region ChunkSearch

        public static b_WorkOrder ProcessRowForChunkSearchLookupList(SqlDataReader reader)
        {
            b_WorkOrder WorkOrder = new b_WorkOrder();
            WorkOrder.LoadFromDatabaseForChunkSearchList(reader);
            return WorkOrder;
        }

        public void LoadFromDatabaseForChunkSearchList(SqlDataReader reader)
        {
            // Only Retrieving data used in lists and permission data
            // If we need more columns must change this method and sp
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = "";
                }

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = "";
                }

                // ChargeToId
                ChargeToId = reader.GetInt64(i++);

                // ChargeToName
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = "";
                    i++;
                }

                //statius
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = "";
                    i++;
                }

                //updateIndex
                UpdateIndex = reader.GetInt32(i++);


                //Work Assigned Name
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i++);
                }
                else
                {
                    WorkAssigned_Name = "";
                    i++;
                }

                // RequestorName
                if (false == reader.IsDBNull(i))
                {
                    Requestor_Name = reader.GetString(i++);
                }
                else
                {
                    Requestor_Name = "";
                    i++;
                }

                //Total count
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }


                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }


                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                try { reader["Requestor_Name"].ToString(); }
                catch { missing.Append("Requestor_Name "); }

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
        }
        #endregion


        #region Workorder Chunk Search Lookup list
        public void RetrieveWorkOrderLookuplistChunkSearchV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveChunkSearchLookupList_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Retrieve chunk search for Work Order Completion Workbench
        public static b_WorkOrder ProcessRetrieveForWorkbenchV2(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();

            workOrder.LoadFromDatabaseForWorkOrderRetrieveCompletionWorkbenchV2(reader);
            return workOrder;
        }

        public int LoadFromDatabaseForWorkOrderRetrieveCompletionWorkbenchV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

                // Description
                Description = reader.GetString(i++);

                // EquipmentClientLookupId column, nvarchar(31), not null
                EquipmentClientLookupId = reader.GetString(i++);

                // AssetName column, nvarchar(31), not null
                AssetName = reader.GetString(i++);

                // Status
                Status = reader.GetString(i++);

                // Type  
                Type = reader.GetString(i++);

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

                // Priority
                Priority = reader.GetString(i++);

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

                // CompleteDate
                if (false == reader.IsDBNull(i))
                {
                    CompleteDate = reader.GetDateTime(i);
                }

                else
                {
                    CompleteDate = DateTime.MinValue;
                }
                i++;

                // WorkAssigned_PersonnelId
                WorkAssigned_PersonnelId = reader.GetInt64(i++);

                // AssignedFullName
                if (false == reader.IsDBNull(i))
                {
                    AssignedFullName = reader.GetString(i);
                }
                else
                {
                    AssignedFullName = String.Empty;
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
                if (false == reader.IsDBNull(i))
                {
                    AssetLocation = reader.GetString(i);
                }
                else
                {
                    AssetLocation = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    ProjectClientLookupId = reader.GetString(i);
                }
                else
                {
                    ProjectClientLookupId = "";
                }
                i++;
                if (false == reader.IsDBNull(i))
                {
                    SourceType = reader.GetString(i);
                }
                else
                {
                    SourceType = "";
                }

                i++;
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();


                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["EquipmentClientLookupId"].ToString(); }
                catch { missing.Append("EquipmentClientLookupId "); }

                try { reader["AssetName"].ToString(); }
                catch { missing.Append("AssetName "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["ScheduledStartDate"].ToString(); }
                catch { missing.Append("ScheduledStartDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

                try { reader["AssignedFullName"].ToString(); }
                catch { missing.Append("AssignedFullName "); }

                try { reader["AssetLocation"].ToString(); }
                catch { missing.Append("AssetLocation "); }

                try { reader["ProjectClientLookupId"].ToString(); }
                catch { missing.Append("ProjectClientLookupId "); }

                try { reader["ProjectClientLookupId"].ToString(); }
                catch { missing.Append("ProjectClientLookupId "); }


                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

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

        public void RetrieveV2SearchForCompletionWorkbench(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveChunkSearchForCompletionWorkbench_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region Retrive Approval Workbench Search V2-630
        public void RetrieveAllWorkbenchSearchV2(
       SqlConnection connection,
       SqlTransaction transaction,
       long callerUserInfoId,
       string callerUserName,
       ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_Workorder_RetrieveApprovalWorkbenchforSearch_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region  Available Work order Daily LaborScheduling V2-630
        public static b_WorkOrder ProcessRowForAvailableWorkDailyLaborSchedulingBySearchV2(SqlDataReader reader)
        {
            // Create instance of object
            b_WorkOrder obj = new b_WorkOrder();

            // Load the object from the database
            obj.LoadFromDatabaseForAvailableWorkDailyLaborSchedulingBySearchV2(reader);

            // Return result
            return obj;
        }
        public int LoadFromDatabaseForAvailableWorkDailyLaborSchedulingBySearchV2(SqlDataReader reader)
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
                // ChargeToDescription
                if (false == reader.IsDBNull(i))
                {
                    ChargeToDescription = reader.GetString(i++);
                }
                else
                {
                    ChargeToDescription = "";
                    i++;
                }
                PartsOnOrder = reader.GetInt32(i++); //V2-838
                #region V2-984

                if (false == reader.IsDBNull(i))
                {
                    Personnels = reader.GetString(i);
                }
                else
                {
                    Personnels = "";

                }
                i++;
                WorkAssigned_PersonnelId = reader.GetInt64(i++);

                // TotalCount
                TotalCount = reader.GetInt32(i++);
                #endregion

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

                try { reader["ChargeToDescription"].ToString(); }
                catch { missing.Append("ChargeToDescription "); }

                //V2-838
                try { reader["PartsOnOrder"].ToString(); }
                catch { missing.Append("PartsOnOrder "); }

                //V2-984

                try { reader["Personnels"].ToString(); }
                catch { missing.Append("Personnels "); }

                try { reader["WorkAssigned_PersonnelId"].ToString(); }
                catch { missing.Append("WorkAssigned_PersonnelId "); }

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

        public void RetrieveAvailableWorkForDailyLaborSchedulingBySearchV2(
    SqlConnection connection,
    SqlTransaction transaction,
    long callerUserInfoId,
    string callerUserName,
    ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_GetAvailableWorkOrderDailyLaborSchedulingBySearchCriteria_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        public void RetrieveAllByWorkOrdeV2Print(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WOPrint_RetrieveAllByWorkOrder_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        #region V2-1051
        public void CreateWorkOrderModel(
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
                Database.StoredProcedure.usp_WorkOrder_CreateWOModel_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region ChunkSearch V2-1031

        public static b_WorkOrder ProcessRowForChunkSearchLookupListForIssuePart(SqlDataReader reader)
        {
            b_WorkOrder WorkOrder = new b_WorkOrder();
            WorkOrder.LoadFromDatabaseForChunkSearchListForIssuePart(reader);
            return WorkOrder;
        }

        public void LoadFromDatabaseForChunkSearchListForIssuePart(SqlDataReader reader)
        {
            // Only Retrieving data used in lists and permission data
            // If we need more columns must change this method and sp
            int i = 0;
            try
            {
                // ClientId column, bigint, not null
                ClientId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // SiteId column, bigint, not null
                SiteId = reader.GetInt64(i++);

                // ClientLookupId
                if (false == reader.IsDBNull(i))
                {
                    ClientLookupId = reader.GetString(i++);
                }
                else
                {
                    ClientLookupId = "";
                }

                // Description
                if (false == reader.IsDBNull(i))
                {
                    Description = reader.GetString(i++);
                }
                else
                {
                    Description = "";
                }

                // ChargeToId
                ChargeToId = reader.GetInt64(i++);

                // ChargeToName
                if (false == reader.IsDBNull(i))
                {
                    ChargeTo_Name = reader.GetString(i++);
                }
                else
                {
                    ChargeTo_Name = "";
                    i++;
                }

                //statius
                if (false == reader.IsDBNull(i))
                {
                    Status = reader.GetString(i++);
                }
                else
                {
                    Status = "";
                    i++;
                }

                //updateIndex
                UpdateIndex = reader.GetInt32(i++);


                //Work Assigned Name
                if (false == reader.IsDBNull(i))
                {
                    WorkAssigned_Name = reader.GetString(i++);
                }
                else
                {
                    WorkAssigned_Name = "";
                    i++;
                }

                // RequestorName
                if (false == reader.IsDBNull(i))
                {
                    Requestor_Name = reader.GetString(i++);
                }
                else
                {
                    Requestor_Name = "";
                    i++;
                }

                //Total count
                TotalCount = reader.GetInt32(i++);

            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["ClientId"].ToString(); }
                catch { missing.Append("ClientId "); }

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["SiteId"].ToString(); }
                catch { missing.Append("SiteId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }


                try { reader["Description"].ToString(); }
                catch { missing.Append("Description "); }

                try { reader["ChargeToId"].ToString(); }
                catch { missing.Append("ChargeToId "); }

                try { reader["ChargeTo_Name"].ToString(); }
                catch { missing.Append("ChargeTo_Name "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }


                try { reader["UpdateIndex"].ToString(); }
                catch { missing.Append("UpdateIndex "); }

                try { reader["WorkAssigned_Name"].ToString(); }
                catch { missing.Append("WorkAssigned_Name "); }

                try { reader["Requestor_Name"].ToString(); }
                catch { missing.Append("Requestor_Name "); }

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
        }


        public void RetrieveWorkOrderLookuplistChunkSearchForIssuePartsV2(
SqlConnection connection,
SqlTransaction transaction,
long callerUserInfoId,
string callerUserName,
ref List<b_WorkOrder> results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveChunkSearchLookupListForPartsIssue_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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

        #region V2-1087 ProjectCosting Dashboard
        public void WorkOrderStatusesCountForDashboard_V2(
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

                results = StoredProcedure.usp_WorkOrder_RetrieveScheduleComplianceStatusCount_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);
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
        #region V2-1178
        public void RetrieveWorkOrderIdByClientLookupIdV2FromDatabase(
               SqlConnection connection,
               SqlTransaction transaction,
               long callerUserInfoId,
string callerUserName,
               ref b_WorkOrder result
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

                result = Database.StoredProcedure.usp_WorkOrder_RetrieveByClientLookUpId_V2.CallStoredProcedure(command, this);

            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
                ClientLookupId = String.Empty;
            }
        }
            public static b_WorkOrder ProcessRowForWorkOrderIdByClientIdLookupV2(SqlDataReader reader)
            {
                // Create instance of object
                b_WorkOrder workorder = new b_WorkOrder();

                // Load the object from the database
                workorder.LoadFromDatabaseForWorkOrderIdByClientIdLookupV2(reader);

                // Return result
                return workorder;
            }

        public void LoadFromDatabaseForWorkOrderIdByClientIdLookupV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);
            }
            catch (Exception ex)
            {
                // Diagnostics
                StringBuilder missing = new StringBuilder();

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

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

        #region V2-1177
        public void RetrieveAnalyticsWOStatusDashboardV2(
          SqlConnection connection,
          SqlTransaction transaction,
          long callerUserInfoId,
          string callerUserName,
          ref b_WorkOrder results
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

                results = Database.StoredProcedure.usp_WorkOrder_RetrieveForAnalyticsDashboard_V2.CallStoredProcedure(command, callerUserInfoId, callerUserName, this);

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
        public static b_WorkOrder ProcessAnalyticsWOStatusDashboardV2(SqlDataReader reader)
        {
            b_WorkOrder workOrder = new b_WorkOrder();

            workOrder.LoadFromDatabaseForAnalyticsWOStatusDashboardV2(reader);
            return workOrder;
        }

        public int LoadFromDatabaseForAnalyticsWOStatusDashboardV2(SqlDataReader reader)
        {
            int i = 0;
            try
            {
                // Client Id
                ClientId = reader.GetInt64(i++);

                // Site Id
                SiteId = reader.GetInt64(i++);

                // WorkOrderId column, bigint, not null
                WorkOrderId = reader.GetInt64(i++);

                // ClientLookupId column, nvarchar(31), not null
                ClientLookupId = reader.GetString(i++);

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
                    Status = reader.GetString(i);
                }
                else
                {
                    Status = "";
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    Type = reader.GetString(i);
                }
                else
                {
                    Type = "";
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    Priority = reader.GetString(i);
                }
                else
                {
                    Priority = "";
                }
                i++;
                
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
                    ActualMaterialCosts = reader.GetDecimal(i);
                }
                else
                {
                    ActualMaterialCosts = 0;
                }
                i++;
                
                if (false == reader.IsDBNull(i))
                {
                    ActualLaborHours = reader.GetDecimal(i);
                }
                else
                {
                    ActualLaborHours = 0;
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
                    TotalCost = reader.GetDecimal(i);
                }
                else
                {
                    TotalCost = 0;
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

                try { reader["WorkOrderId"].ToString(); }
                catch { missing.Append("WorkOrderId "); }

                try { reader["ClientLookupId"].ToString(); }
                catch { missing.Append("ClientLookupId "); }

                try { reader["CreateDate"].ToString(); }
                catch { missing.Append("CreateDate "); }

                try { reader["CompleteDate"].ToString(); }
                catch { missing.Append("CompleteDate "); }

                try { reader["Status"].ToString(); }
                catch { missing.Append("Status "); }

                try { reader["Type"].ToString(); }
                catch { missing.Append("Type "); }

                try { reader["Priority"].ToString(); }
                catch { missing.Append("Priority "); }

                try { reader["SourceType"].ToString(); }
                catch { missing.Append("SourceType "); }

                try { reader["ActualMaterialCosts"].ToString(); }
                catch { missing.Append("ActualMaterialCosts "); }

                try { reader["ActualLaborHours"].ToString(); }
                catch { missing.Append("ActualLaborHours "); }

                try { reader["LabourCost"].ToString(); }
                catch { missing.Append("LabourCost "); }

                try { reader["TotalCost"].ToString(); }
                catch { missing.Append("TotalCost "); }

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
