/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person                 Description
* ===========  ========= ====================== =================================================
* 2014-Jul-30  SOM-263   Roger Lawton           Corrected creation/updating of schedule records
**************************************************************************************************
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using System.Data.SqlClient;
using Database.StoredProcedure;
using Common.Enumerations;

namespace Database.Client.Custom.Transactions
{
    public class WorkOrderScheduleTransactions : WorkOrderSchedule_TransactionBaseClass
    {


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

        }
    }

    public class WorkOrderSchdule_RetrieveByWorkOrderId : WorkOrderSchedule_TransactionBaseClass
    {
        public List<b_WorkOrderSchedule> WorkOrderScheduleList { get; set; }

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
            List<b_WorkOrderSchedule> tmpArray = null;

            WorkOrderSchedule.RetrieveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderScheduleList = new List<b_WorkOrderSchedule>();
            foreach (b_WorkOrderSchedule tmpObj in tmpArray)
            {
                WorkOrderScheduleList.Add(tmpObj);
            }
        }
    }

    // SOM-263
    public class WorkOrderSchedule_CreateForWorkOrder : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId > 0)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.CreateForWorkOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderSchedule.WorkOrderSchedId > 0);
        }
    }

    // SOM-263
    public class WorkOrderSchedule_UpdateForWorkOrder : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.UpdateForWorkOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderSchedule.WorkOrderSchedId > 0);
        }
    }
    // SOM-1134
    public class WorkOrderSchedule_RemoveRecord : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.RemoveRecord(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderSchedule.WorkOrderSchedId > 0);
        }
    }

    //==========
    public class WorkOrderSchedule_EditandSaveScheduled : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.EditandSaveScheduledWorkOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderSchedule.WorkOrderSchedId > 0);
        }
    }
    public class WorkOrderSchedule_DeleteScheduledWorOrder : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.DeleteScheduledWorOrder(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }

        public override void Postprocess()
        {
            base.Postprocess();
            System.Diagnostics.Debug.Assert(WorkOrderSchedule.WorkOrderSchedId > 0);
        }
    }

    public class WorkOrderSchdule_RetrievePersonnel : WorkOrderSchedule_TransactionBaseClass
    {
        public List<List<b_WorkOrderSchedule>> WorkOrderSchedulePersonnelList { get; set; }
        public List<b_WorkOrderSchedule> WorkOrderScheduleList { get; set; }

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
            List<List<b_WorkOrderSchedule>> tmpArray = null;

            WorkOrderSchedule.RetrievePersonnel(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderSchedulePersonnelList = new List<List<b_WorkOrderSchedule>>();
            WorkOrderScheduleList = new List<b_WorkOrderSchedule>();
            foreach (List<b_WorkOrderSchedule> tmpObj in tmpArray)
            {
                foreach (b_WorkOrderSchedule tmpObj2 in tmpObj)
                {
                    WorkOrderScheduleList.Add(tmpObj2);
                }
                WorkOrderSchedulePersonnelList.Add(tmpObj);
            }

           
        }
    }

    public class WorkOrderSchdule_RetrievePersonnelByAssetGroupMasterQuery : WorkOrderSchedule_TransactionBaseClass
    {
        public List<List<b_WorkOrderSchedule>> WorkOrderSchedulePersonnelList { get; set; }
        public List<b_WorkOrderSchedule> WorkOrderScheduleList { get; set; }

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
            List<List<b_WorkOrderSchedule>> tmpArray = null;

            WorkOrderSchedule.RetrievePersonnelByAssetGroupMasterQuery(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);

            WorkOrderSchedulePersonnelList = new List<List<b_WorkOrderSchedule>>();
            WorkOrderScheduleList = new List<b_WorkOrderSchedule>();
            foreach (List<b_WorkOrderSchedule> tmpObj in tmpArray)
            {
                foreach (b_WorkOrderSchedule tmpObj2 in tmpObj)
                {
                    WorkOrderScheduleList.Add(tmpObj2);
                }
                WorkOrderSchedulePersonnelList.Add(tmpObj);
            }


        }
    }
    public class WorkOrderSchdule_RetrieveByWorkOrderIdAndSchdeuleId : WorkOrderSchedule_TransactionBaseClass
    {
        public b_WorkOrderSchedule results { get; set; }
        public override void PerformWorkItem()
        {

            //WorkOrderSchedule.RetrieveByWorkOrderIdFromDatabase(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);            
            b_WorkOrderSchedule WOSchedule = null;
            WorkOrderSchedule.RetrieveByWorkOrderIdAndSchdeuleId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName,ref WOSchedule);
            results = WOSchedule;
            //WorkOrderScheduleList = new List<b_WorkOrderSchedule>();
            //foreach (b_WorkOrderSchedule tmpObj in tmpArray)
            //{
            //    WorkOrderScheduleList.Add(tmpObj);
            //}
        }
    }
    public class WorkOrderSchedule_RemoveWorkOrderScheduleForLaborScheduling : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.RemoveWorkOrderScheduleForLaborScheduling(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class WorkOrderSchedule_RemoveWorkOrderScheduleForResourceList : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.RemoveWorkOrderScheduleForResourceList(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }
    public class WorkOrderSchedule_DragWorkOrderScheduleForLaborScheduling : WorkOrderSchedule_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (WorkOrderSchedule.WorkOrderSchedId < 1)
            {
                string message = "WorkOrderSchedule has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            WorkOrderSchedule.DragWorkOrderScheduleForLaborScheduling(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    #region  Dashboard
    public class WorkOrderSchedule_RetrieveCountWorkOrderScheduleByComplete : WorkOrderSchedule_TransactionBaseClass
    {
        public WorkOrderSchedule_RetrieveCountWorkOrderScheduleByComplete()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public long CountWorkorderScheduleByComplete { get; set; }
        public long PersonnelId { get; set; }
        public long CaseNo { get; set; }
        public List<b_WorkOrderSchedule> WorkOrderScheduleList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();           
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            List<b_WorkOrderSchedule> tmpArray = null;
            WorkOrderSchedule.RetrieveCountWorkOrderScheduleByComplete(this.Connection, this.Transaction, this.dbKey.User.UserInfoId, this.dbKey.UserName,ref tmpArray);
            //WorkOrderSchedule.RetrieveLoginRecordsCountByCreateDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            WorkOrderScheduleList = new List<b_WorkOrderSchedule>();
            foreach (b_WorkOrderSchedule tmpObj in tmpArray)
            {
                WorkOrderScheduleList.Add(tmpObj);
            }
            //CountWorkorderScheduleByComplete = WorkOrderSchedule.RetrieveCountWorkOrderScheduleByComplete(this.Connection, this.Transaction, this.dbKey.User.UserInfoId, this.dbKey.UserName, this.dbKey.Client.ClientId, this.dbKey.User.DefaultSiteId, this.PersonnelId, this.CaseNo);
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }
    public class WorkOrderSchedule_RetrieveCountWorkOrderScheduleByInComplete : WorkOrderSchedule_TransactionBaseClass
    {
        public WorkOrderSchedule_RetrieveCountWorkOrderScheduleByInComplete()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public long CountWorkorderScheduleByInComplete { get; set; }
        public long PersonnelId { get; set; }
        public long CaseNo { get; set; }
        public List<b_WorkOrderSchedule> WorkOrderScheduleList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            base.UseTransaction = false;            
            List<b_WorkOrderSchedule> tmpArray = null;
            WorkOrderSchedule.RetrieveCountWorkOrderScheduleByInComplete(this.Connection, this.Transaction, this.dbKey.User.UserInfoId, this.dbKey.UserName, ref tmpArray);
            //WorkOrderSchedule.RetrieveLoginRecordsCountByCreateDate(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpArray);
            WorkOrderScheduleList = new List<b_WorkOrderSchedule>();
            foreach (b_WorkOrderSchedule tmpObj in tmpArray)
            {
                WorkOrderScheduleList.Add(tmpObj);
            }
            //CountWorkorderScheduleByInComplete = WorkOrderSchedule.RetrieveCountWorkOrderScheduleByInComplete(this.Connection, this.Transaction, this.dbKey.User.UserInfoId, this.dbKey.UserName, this.dbKey.Client.ClientId, this.dbKey.User.DefaultSiteId, this.PersonnelId, this.CaseNo);
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }
    #endregion
}
