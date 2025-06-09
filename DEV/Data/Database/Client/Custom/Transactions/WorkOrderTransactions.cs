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
* Date        Task ID   Person             Description
* =========== ======== =================== =========================================================
* 2014-Sep-30 SOM-346  Roger Lawton        Added bActive property to WorkOrder_RetrieveByEquipmentId
****************************************************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Database;
using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;
using Database.Transactions;

namespace Database
{
    public class WorkOrder_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public WorkOrder_RetrieveBySearchCriteria()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public string Query { get; set; }
        public string Area { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Shift { get; set; }
        public string ColumnName { get; set; }
        public string ColumnSearchText { get; set; }
        public bool MatchAnywhere { get; set; }
        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }
        public DateTime? CreateDate { get; set; } //----code added on 08-05-2014------
        // Result Sets
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public int ResultCount { get; set; }
        public string Search { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;

            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                int tmp;

                WorkOrderList = usp_WorkOrder_RetrieveBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, Query, Site, Area, Department, Source,
                        Type, Status, Priority, Shift, DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, PageNumber, ResultsPerPage, MatchAnywhere, OrderColumn, OrderDirection, out tmp);

                ResultCount = tmp;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrder_RetrieveClientLookupIdBySearchCriteria : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_WorkOrder> tmpList = null;

            WorkOrder.RetrieveClientLookupIdBySearchCriteriaFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            WorkOrderList = new List<b_WorkOrder>();
            foreach (b_WorkOrder tmpObj in tmpList)
            {
                WorkOrderList.Add(tmpObj);
            }
        }
    }

    public class WorkOrder_RetrieveLookupListBySearchCriteria : AbstractTransactionManager
    {
        public WorkOrder_RetrieveLookupListBySearchCriteria()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }
        /*Modified INT-1 */
        public string ChargeTo_Name { get; set; }
        public string WorkAssigned_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public int ResultCount { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                int tmp;

                WorkOrderList = usp_WorkOrder_RetrieveLookupListBySearchCriteria.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Description, ChargeTo_Name, WorkAssigned_Name, Requestor_Name, Status, SiteId,
                        PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

                ResultCount = tmp;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class WorkOrder_RetrieveLookupListBySearchCriteria_V2 : AbstractTransactionManager
    {
        public WorkOrder_RetrieveLookupListBySearchCriteria_V2()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        //public string PersonnelId { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public long SiteId { get; set; }
        /*Modified INT-1 */
        public string ChargeTo_Name { get; set; }
        public string WorkAssigned_Name { get; set; }
        public string Requestor_Name { get; set; }
        public string Status { get; set; }
        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }

        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        // Result Sets
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public int ResultCount { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                int tmp;

                WorkOrderList = usp_WorkOrder_RetrieveLookupListBySearchCriteria_V2.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, ClientLookupId, Description, ChargeTo_Name, WorkAssigned_Name, Requestor_Name, Status, SiteId,
                        PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out tmp);

                ResultCount = tmp;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }


    public class WorkOrder_RetrieveByEquipmentId : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }
        // SOM-346
        public bool bActive { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_WorkOrder> tmpArray = null;
            // SOM-346
            WorkOrder.RetrieveByEquipmentIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.bActive, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>();
            foreach (b_WorkOrder tmpObj in tmpArray)
            {
                WorkOrderList.Add(tmpObj);
            }
        }
    }

    public class WorkOrder_RetrieveByEquipmentBIMGuidId : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public long ClientId { get; set; }
        public Guid BIMGuid { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_WorkOrder> tmpArray = null;

            WorkOrder.RetrieveByEquipmentBIMGuidFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ClientId, BIMGuid, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>();
            foreach (b_WorkOrder tmpObj in tmpArray)
            {
                WorkOrderList.Add(tmpObj);
            }
        }
    }



    public class WorkOrder_RetrieveByLocationId : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }
        // SOM-346
        public bool bActive { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_WorkOrder> tmpArray = null;
            //SOM-346
            WorkOrder.RetrieveByLocationIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, this.bActive, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>();
            foreach (b_WorkOrder tmpObj in tmpArray)
            {
                WorkOrderList.Add(tmpObj);
            }
        }
    }

    public class WorkOrder_RetrieveByStatus : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }


        public string ColumnName { get; set; }
        public string ColumnSearchText { get; set; }
        public bool MatchAnywhere { get; set; }
        public string DateSelection { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;

            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                WorkOrderList = usp_WorkOrder_RetrieveByStatus.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId, WorkOrder.Status,
                    DateSelection, DateStart, DateEnd, ColumnName, ColumnSearchText, MatchAnywhere);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }
    }

    public class WorkOrder_RetrieveInitialSearchConfigurationData : AbstractTransactionManager
    {
        public WorkOrder_RetrieveInitialSearchConfigurationData()
        {
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria;

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                // Create the command to use in calling the stored procedures
                command = new SqlCommand()
                {
                    Connection = this.Connection,
                    Transaction = this.Transaction
                };

                // Call the stored procedure to retrieve the data
                SearchCriteria = usp_WorkOrder_RetrieveInitialSearchConfigurationData.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, dbKey.Client.ClientId);
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }

    public class WorkOrder_ValidateByClientLookupId : WorkOrder_TransactionBaseClass
    {
        public bool CreateMode { get; set; }
        public string Requestor_PersonnelClientLookupId { get; set; }

        public WorkOrder_ValidateByClientLookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                WorkOrder.ValidateByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors, CreateMode);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }
    //----som-987
    public class WorkOrder_ValidateClientLookupId : WorkOrder_TransactionBaseClass
    {
        public bool CreateMode { get; set; }
        public string Requestor_PersonnelClientLookupId { get; set; }

        public WorkOrder_ValidateClientLookupId()
        {
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        // Result Sets
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }

        public override void PerformWorkItem()
        {
            SqlCommand command = null;
            string message = String.Empty;

            try
            {
                List<b_StoredProcValidationError> errors = null;

                WorkOrder.ValidateScheduleWorkByClientLookupIdFromDatabase(this.Connection, this.Transaction, this.CallerUserInfoId, this.CallerUserName, ref errors, CreateMode);

                StoredProcValidationErrorList = errors;
            }
            finally
            {
                if (null != command)
                {
                    command.Dispose();
                    command = null;
                }

                message = String.Empty;
            }
        }

        public override void Preprocess()
        {
            // throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            // throw new NotImplementedException();
        }
    }


    //----end
    // SOM-1384
    public class WorkOder_CreateForSensorReading : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.CreateForSensorReading(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }
    public class WorkOrder_CreateFromOnDemandMaster : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.CreateFromOnDemandMaster(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_CreateFromOnDemandMaster_V2 : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.CreateFromOnDemandMaster_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_CreateByForeignKeys : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_CreateByForeignKeysForSanitation : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.InsertByForeignKeysForSanitationIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }


    public class WorkOrder_UpdateByForeignKeys : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdateByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }


    public class WorkOrder_UpdatePlanner : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdateWorkPlanner(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_CompleteWorkOrder : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.CompleteWorkOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }



    public class WorkOrder_CompleteWorkOrderFromWizard : WorkOrder_TransactionBaseClass
    {
        public List<b_Timecard> TimecardList { get; set; }
        public b_WorkOrderUDF WorkOrderUDF { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.TimecardList = TimecardList;
            WorkOrder.WorkOrderUDF = WorkOrderUDF;
            WorkOrder.CompleteWorkOrderFromWizard(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }


    public class UpdateTasksByWorkOrderId : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdateTasksByWorkOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_UpdateList : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public List<b_ChangeLog> ChangeLogList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            foreach (b_WorkOrder workOrder in WorkOrderList)
            {
                if (workOrder.WorkOrderId == 0)
                {
                    string message = "WorkOrder has an invalid ID.";
                    throw new Exception(message);
                }
            }

        }
        public override void PerformWorkItem()
        {
            foreach (b_WorkOrder workOrder in WorkOrderList)
            {
                workOrder.UpdateInDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

                b_ChangeLog changeLog = ChangeLogList.Find(c => c.ObjectId == workOrder.WorkOrderId);
                if (changeLog != null) { changeLog.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName); }
            }

        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_RetrieveByPks : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            foreach (b_WorkOrder workOrder in WorkOrderList)
            {
                if (workOrder.WorkOrderId == 0)
                {
                    string message = "WorkOrder has an invalid ID.";
                    throw new Exception(message);
                }
            }

        }
        public override void PerformWorkItem()
        {

            foreach (b_WorkOrder workOrder in WorkOrderList)
            {
                workOrder.ClientId = dbKey.Client.ClientId;
                workOrder.RetrieveByPKFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            }

        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_RetrieveByForeignKeys : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            WorkOrder.RetrieveByForeignKeysFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class WorkOrder_RetrieveAllForSearch : WorkOrder_TransactionBaseClass
    {

        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            //WorkOrder.LoadFromDatabaseWithDepartName(
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_RetrievePersonnelInitial : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpWO = null;
            WorkOrder.RetrievePersonnelInitial(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpWO);
            WorkOrder = tmpWO;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    //public class WorkOrder_RetrieveAllWorkOrderId : WorkOrder_TransactionBaseClass
    //{

    //    public List<b_WorkOrder> WorkOrderList { get; set; }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //        if (WorkOrder.WorkOrderId > 0)
    //        {
    //            string message = "WorkOrder has an invalid ID.";
    //            throw new Exception(message);
    //        }
    //    }
    //    public override void PerformWorkItem()
    //    {
    //        List<b_WorkOrder> tmpList = null;
    //        WorkOrder.RetrieveAllWorkOrderId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
    //        //WorkOrder.LoadFromDatabaseWithDepartName(
    //        WorkOrderList = tmpList;
    //    }

    //    public override void Postprocess()
    //    {
    //        base.Postprocess();
    //    }
    //}
    public class WorkOrder_RetrieveAllWorkRequestForSearch : WorkOrder_TransactionBaseClass
    {

        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAllWorkRequestForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_CreateForPrevMaintenance : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder b_WorkOrder = new b_WorkOrder();
            b_WorkOrder.PrevMaintBatchId = WorkOrder.PrevMaintBatchId;
            b_WorkOrder.ClientLookupId = WorkOrder.ClientLookupId;
            b_WorkOrder.Requestor_PersonnelId = WorkOrder.Requestor_PersonnelId;
            WorkOrder.WorkOrder_CreateForPrevMaintenance(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref b_WorkOrder);
            WorkOrder = b_WorkOrder;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder b_WorkOrder = new b_WorkOrder();
            b_WorkOrder.PrevMaintBatchId = WorkOrder.PrevMaintBatchId;
            b_WorkOrder.ClientLookupId = WorkOrder.ClientLookupId;
            b_WorkOrder.Requestor_PersonnelId = WorkOrder.Requestor_PersonnelId;
            WorkOrder.WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref b_WorkOrder);
            WorkOrder = b_WorkOrder;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2 : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder b_WorkOrder = new b_WorkOrder();
            b_WorkOrder.ClientId = WorkOrder.ClientId;
            b_WorkOrder.SiteId = WorkOrder.SiteId;
            b_WorkOrder.PrevMaintBatchId = WorkOrder.PrevMaintBatchId;
            b_WorkOrder.ClientLookupId = WorkOrder.ClientLookupId;
            b_WorkOrder.Requestor_PersonnelId = WorkOrder.Requestor_PersonnelId;
            b_WorkOrder.AssetGroup1Ids= WorkOrder.AssetGroup1Ids;
            b_WorkOrder.AssetGroup2Ids = WorkOrder.AssetGroup2Ids;
            b_WorkOrder.AssetGroup3Ids = WorkOrder.AssetGroup3Ids;
            b_WorkOrder.PrevMaintSchedType = WorkOrder.PrevMaintSchedType;
            b_WorkOrder.PrevMaintMasterType = WorkOrder.PrevMaintMasterType;
            WorkOrder.WorkOrder_CreateForPrevMaintenanceFromPrevMaintLibrary_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref b_WorkOrder);
            WorkOrder = b_WorkOrder;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_RetrieveAllWorkbenchSearch : WorkOrder_TransactionBaseClass
    {

        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (WorkOrder.WorkOrderId > 0)
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAllWorkbenchSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            //WorkOrder.LoadFromDatabaseWithDepartName(
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    //------som-987
    public class WorkOrder_RetrieveUnscheduledWorkOrder : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (WorkOrder.WorkOrderId > 0)
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveUnscheduledWorkOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_RetrieveByClientLookupId : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WOList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveByClientLookupId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WOList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class WorkOrder_RetrieveWorkOrderForPrint : WorkOrder_TransactionBaseClass
    {

        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveWorkOrderForPrint(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }

    }

    public class WorkOrder_RetrieveByScheduleId : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WOList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveByScheduleId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WOList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class WorkOrederSchedule_Create : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            WorkOrder.CreateAndValidateWorkOrderSchedule(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class WorkOrderSchedule_AddScheduleRecord : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            WorkOrder.AddScheduleRecord(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class WorkOrderSchedule_ReassignPersonnelRecord : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            WorkOrder.ReassignPersonnelScheduleRecord(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class WorkOrder_UpdateByWorkbench : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdateByWorkbenchIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }
    //SOM-1479
    public class WorkOrder_CancelWorkOrder : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.CancelWorkorder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }
    public class WorkOrder_RetrieveAllCompletionWorkbenchForFPSearch : WorkOrder_TransactionBaseClass
    {

        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (WorkOrder.WorkOrderId > 0)
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAllCompletionWorkbenchForFPSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            //WorkOrder.LoadFromDatabaseWithDepartName(
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class WorkOrder_RetrieveV2Search : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.RetrieveV2Search(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_RetrieveV2Print : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.RetrieveV2Print(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    public class WorkOrder_CreateEmergencyByForeignKeys : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.InsertByForeignKeysIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            // RKL - This creates a wos rec with client id of 0
            // not needed as schedule rec created by calling
            // need to review all calling code to make sure
            if (WorkOrder.WorkOrderId > 0)
            {
                b_WorkOrderSchedule WorkOrderSchedule = new b_WorkOrderSchedule
                {
                    WorkOrderId = WorkOrder.WorkOrderId,
                    PersonnelId = dbKey.Personnel.PersonnelId,
                    ScheduledStartDate = DateTime.UtcNow
                };
                WorkOrderSchedule.InsertIntoDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            }
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }
    public class WORetrieveForExtraction : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_WorkOrder> tmpArray = null;

            WorkOrder.WORetrieveForExtraction(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>();
            foreach (b_WorkOrder tmpObj in tmpArray)
            {
                WorkOrderList.Add(tmpObj);
            }
        }
    }


    public class WorkOrder_RetrieveWorkOrderListByMeterIdAndReadingDate : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (WorkOrder.WorkOrderId > 0)
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.WorkOrder_RetrieveWorkOrderListByMeterIdAndReadingDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            //WorkOrder.LoadFromDatabaseWithDepartName(
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class ValidateEquipmentLocationTransaction : WorkOrder_TransactionBaseClass
    {
        public List<b_StoredProcValidationError> StoredProcValidationErrorList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            List<b_StoredProcValidationError> errors = null;
            WorkOrder.ValidateByEquipmentLocation(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref errors);
            StoredProcValidationErrorList = errors;
        }

        public override void Postprocess()
        {

        }
    }

    public class WorkOrder_RetrieveAll_V2 : AbstractTransactionManager
    {

        public WorkOrder_RetrieveAll_V2()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            b_WorkOrder[] tmpArray = null;
            b_WorkOrder o = new b_WorkOrder();


            // Explicitly set tenant id from dbkey
            o.ClientId = this.dbKey.Client.ClientId;
            o.SiteId = this.dbKey.User.DefaultSiteId;


            o.RetrieveAll_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>(tmpArray);
        }
    }


    public class WorkOrder_WRDashboardRetrieveAllForSearch : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (string.IsNullOrEmpty(WorkOrder.ClientLookupId))
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.WRDashboardRetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }


    public class WorkOrder_WRDashboardRetrieveAllForSearchPrint : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (string.IsNullOrEmpty(WorkOrder.ClientLookupId))
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.WorkOrder_WRDashboardRetrieveAllForPrint_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }


    public class WorkOrder_RetrieveForWorkOrderCostWidget : WorkOrder_TransactionBaseClass
    {
        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpArray = null;


            WorkOrder.RetrieveForWorkOrderCostWidget(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>();
            foreach (b_WorkOrder tmpObj in tmpArray)
            {
                WorkOrderList.Add(tmpObj);
            }
        }
    }


    public class WorkOrder_RetrievePOandPR : WorkOrder_TransactionBaseClass
    {

        public WorkOrder_RetrievePOandPR()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }


        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void Preprocess()
        {
            //throw new NotImplementedException();
        }

        public override void Postprocess()
        {
            //throw new NotImplementedException();
        }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            b_WorkOrder[] tmpArray = null;
            b_WorkOrder o = new b_WorkOrder();


            // Explicitly set tenant id from dbkey
            //o.ClientId = this.dbKey.Client.ClientId;
            //o.SiteId = this.dbKey.User.DefaultSiteId;


            WorkOrder.RetrievePOandPR(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderList = new List<b_WorkOrder>(tmpArray);
        }
    }


    public class WorkOrder_UpdateOnRemovingSchedule : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdateOnRemovingSchedule(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_UpdateListPartsonOrder : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdateListPartsonOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            //System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }

    public class WorkOrder_UpdatePartsonOrder : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

            if (WorkOrder.WorkOrderId == 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.UpdatePartsonOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.WorkOrderId > 0);
        }
    }
    public class WorkOrder_ListForLaborSchedulingChunkSearch : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    public class WorkOrder_CalendarForLaborSchedulingChunkSearch : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveCalendarForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class WorkOrder_AvailableWorkForLaborSchedulingSearch : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAvailableWorkForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    public class WorkOrder_RetrieveApprovedWorkOrderForLaborScheduling : WorkOrder_TransactionBaseClass
    {
        //public override void PerformLocalValidation()
        //{
        //    base.PerformLocalValidation();
        //}
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveApprovedWorkOrderForLaborScheduling(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        //public override void Postprocess()
        //{
        //    base.Postprocess();
        //}
    }
    public class WorkOrder_AvailableWorkForLaborSchedulingSearchCalendar : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAvailableWorkForLaborSchedulingSearchCalendar(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
            //WorkOrder.RetrieveListForLaborSchedulingSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }


    #region WorkOrder Lookuplist chunk search
    public class WorkOrder_RetrieveChunkSearchLookupList_V2 : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveWorkOrderLookuplistChunkSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    #endregion

    #region Work Order Completion Workbench Chunk Search
    public class WorkOrder_RetrieveV2SearchForCompletionWorkbench : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.RetrieveV2SearchForCompletionWorkbench(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region  Workorder Approval Workbench Search V2-630
    public class WorkOrder_RetrieveAllWorkbenchSearchV2 : WorkOrder_TransactionBaseClass
    {

        public List<b_WorkOrder> WorkOrderList { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (WorkOrder.WorkOrderId > 0)
            //{
            //    string message = "WorkOrder has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAllWorkbenchSearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            //WorkOrder.LoadFromDatabaseWithDepartName(
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region Available Work order Daily LaborScheduling V2-630
    public class WorkOrder_AvailableWorkForDailyLaborSchedulingBySearchV2 : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {

            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveAvailableWorkForDailyLaborSchedulingBySearchV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;

        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    #endregion


    public class WorkOrder_RetrieveAllByWorkOrdeV2Print : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.WorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.RetrieveAllByWorkOrdeV2Print(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    #region V2-1051
    public class WorkOrder_CreateWOModel_V2 : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrder.CreatedWorkOrderId > 0)
            {
                string message = "WorkOrder has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrder.CreateWorkOrderModel(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrder.CreatedWorkOrderId > 0);
        }
    }
    #endregion
 
    #region WorkOrder Lookuplist chunk search V2-1031
    public class WorkOrder_RetrieveChunkSearchLookupListForIssueParts_V2 : WorkOrder_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }
        public List<b_WorkOrder> WorkOrderList { get; set; }
        public override void PerformWorkItem()
        {
            List<b_WorkOrder> tmpList = null;
            WorkOrder.RetrieveWorkOrderLookuplistChunkSearchForIssuePartsV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            WorkOrderList = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();

        }
    }
    #endregion

    #region V2-1087 ProjectCosting Dashboard 
    public class WorkOrderStatusCountForDashboard_V2 : PieChartBaseTransaction
    {
        public b_WorkOrder workOrder { get; set; }

        public override void PerformWorkItem()
        {
            List<KeyValuePair<string, long>> tmpArray = null;

            workOrder.WorkOrderStatusesCountForDashboard_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            Entries = new List<KeyValuePair<string, long>>();
            for (int i = 0; i < tmpArray.Count; i++)
            {
                Entries.Add(tmpArray[i]);
            }

        }
    }
    #endregion

    #region Retrieve WorkOrderId by ClientLookupId V2-1178
    public class WorkOrder_RetrieveWorkOrderIdbyClientLookupId_V2 : WorkOrder_TransactionBaseClass
    {

        public b_WorkOrder WorkOrderResult { get; set; }

        public override void PerformLocalValidation()
        {
            if (string.IsNullOrEmpty(WorkOrder.ClientLookupId))
            {
                string message = "WorkOrder has an Client Lookup ID.";
                throw new Exception(message);
            }
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.RetrieveWorkOrderIdByClientLookupIdV2FromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

            WorkOrderResult = tmpList;
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

    #region V2-1177
    public class WorkOrder_AnalyticsWOStatusDashboardV2 : WorkOrder_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            b_WorkOrder tmpList = null;
            WorkOrder.RetrieveAnalyticsWOStatusDashboardV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
    #endregion

}

