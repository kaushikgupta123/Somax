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

namespace Database
{
    public class EventInfo_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public EventInfo_RetrieveBySearchCriteria()
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
        public DateTime? CreateDate { get; set; }
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
    public class EventInfo_RetrieveAllForSearch : EventInfo_TransactionBaseClass
    {
        public List<b_EventInfo> EventInfoList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EventInfo.EventInfoId < 0)
            {
                string message = "EventInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_EventInfo> tmpList = null;
            EventInfo.RetrieveAllForSearch(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EventInfoList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class EventInfo_RetrieveByPKForeignkey : EventInfo_TransactionBaseClass
    {
        public List<b_EventInfo> EventInfoList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EventInfo.EventInfoId < 0)
            {
                string message = "EventInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            List<b_EventInfo> tmpList = null;
            EventInfo.RetrieveByPKForeignkey(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EventInfoList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class EventInfo_RetrieveByInfoStatus : EventInfo_TransactionBaseClass
    {
        public int EventInfoStatusCount { get; set; }
        public object PageStatusCount { get; set; }
        public System.Data.DataTable pelist { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EventInfo.EventInfoId < 0)
            {
                string message = "EventInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            int count = EventInfo.RetrieveStatusCount(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            EventInfoStatusCount = count;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class EventInfo_RetrieveAPMCountHozBar : EventInfo_TransactionBaseClass
    {
        public List<int> EventCountList { get; set; }
        SqlCommand command = null;
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }
        public override void PerformWorkItem()
        {
            command = new SqlCommand()
            {
                Connection = this.Connection,
                Transaction = this.Transaction
            };
            List<int> tmpList; ;
            tmpList = usp_EventInfo_RetrieveAPMCountHozBar.CallStoredProcedure(command, dbKey.User.UserInfoId, dbKey.UserName, EventInfo);
            EventCountList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class EventInfo_RetrieveForAPMBarChart : EventInfo_TransactionBaseClass
    {
        public List<b_EventInfo> EventInfoList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (EventInfo.EventInfoId < 0)
            //{
            //    string message = "EventInfo has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_EventInfo> tmpList = null;
            EventInfo.RetrieveForAPMBarChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EventInfoList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class EventInfo_RetrieveForAPMDoughChart : EventInfo_TransactionBaseClass
    {
        public List<b_EventInfo> EventInfoList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (EventInfo.EventInfoId < 0)
            //{
            //    string message = "EventInfo has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_EventInfo> tmpList = null;
            EventInfo.RetrieveForAPMDoughChart(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EventInfoList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    public class EventInfo_RetrieveAPMCountHozBarV2 : EventInfo_TransactionBaseClass
    {
        public List<b_EventInfo> EventInfoList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            //if (EventInfo.EventInfoId < 0)
            //{
            //    string message = "EventInfo has an invalid ID.";
            //    throw new Exception(message);
            //}
        }
        public override void PerformWorkItem()
        {
            List<b_EventInfo> tmpList = null;
            EventInfo.RetrieveAPMCountHozBarV2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
            EventInfoList = tmpList;
        }
        public override void Postprocess()
        {
            base.Postprocess();
        }



    }


    public class EventInfo_RetrieveForIoT : EventInfo_TransactionBaseClass
    {
        public List<b_EventInfo> EventInfoList { get; set; }
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (EventInfo.EventInfoId < 0)
            {
                string message = "EventInfo has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_EventInfo tmpList = null;
            EventInfo.RetrieveForIoT(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);

        }
        public override void Postprocess()
        {
            base.Postprocess();
        }
    }
}

    

