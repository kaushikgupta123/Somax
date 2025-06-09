using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

using Common.Enumerations;
using Database.Business;
using Database.StoredProcedure;

namespace Database
{
    public class PrevMaintBatchEntryTransactions : AbstractTransactionManager
    {
        public long clientId { get; set; }
        public long  siteid { get; set; }
        public string ScheduleType { get; set; }
        public DateTime ScheduleThroughDate { get; set; }
        public string OnDemandgroup { get; set; }
        public bool PrintWorkOrders { get; set; }
        public bool PrintAttachments { get; set; }

        public List<b_PrevMaintBatchEntry> PrevMaintBatchEntryList { get; set; }

        public PrevMaintBatchEntryTransactions()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }


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
            base.UseTransaction = true;

            //base.Transaction.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintBatchEntry> tempList = null;
            PrevMaintBatchEntryList = new List<b_PrevMaintBatchEntry>();

            try
            {

                b_PrevMaintBatchEntry be = new b_PrevMaintBatchEntry();
                be.PrevMaintBatchEntry_ForWorkOrder(this.Connection,this.Transaction,
                    CallerUserInfoId, CallerUserName, clientId, siteid, ScheduleType,
                    ScheduleThroughDate, OnDemandgroup, PrintWorkOrders, PrintAttachments,
                    ref tempList);

                if (tempList.Count > 0)
                {
                    tempList.ForEach(x =>
                    {
                        PrevMaintBatchEntryList.Add(x);
                    });
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
            }

        }

    }

    public class PrevMaintBatchEntryTransactionsFromPrevMaintLibrary : AbstractTransactionManager
    {
        public long clientId { get; set; }
        public long siteid { get; set; }
        public string ScheduleType { get; set; }
        public DateTime ScheduleThroughDate { get; set; }
        public string OnDemandgroup { get; set; }
        public bool PrintWorkOrders { get; set; }
        public bool PrintAttachments { get; set; }

        public List<b_PrevMaintBatchEntry> PrevMaintBatchEntryList { get; set; }

        public PrevMaintBatchEntryTransactionsFromPrevMaintLibrary()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }


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
            base.UseTransaction = true;

            //base.Transaction.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintBatchEntry> tempList = null;
            PrevMaintBatchEntryList = new List<b_PrevMaintBatchEntry>();

            try
            {

                b_PrevMaintBatchEntry be = new b_PrevMaintBatchEntry();
                be.PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary(this.Connection, this.Transaction,
                    CallerUserInfoId, CallerUserName, clientId, siteid, ScheduleType,
                    ScheduleThroughDate, OnDemandgroup, PrintWorkOrders, PrintAttachments,
                    ref tempList);

                if (tempList.Count > 0)
                {
                    tempList.ForEach(x =>
                    {
                        PrevMaintBatchEntryList.Add(x);
                    });
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
            }

        }

    }

    public class PrevMaintBatchEntryTransactionsFromPrevMaintLibrary_V2 : AbstractTransactionManager
    {
        public long clientId { get; set; }
        public long siteid { get; set; }
        public string ScheduleType { get; set; }
        public DateTime ScheduleThroughDate { get; set; }
        public string OnDemandgroup { get; set; }
        public bool PrintWorkOrders { get; set; }
        public bool PrintAttachments { get; set; }
        public string AssetGroup1Ids { get; set; }
        public  string AssetGroup2Ids { get; set; }
        public string AssetGroup3Ids { get; set; }
        public string PrevMaintSchedType { get; set; }
        public string PrevMaintMasterType { get; set; }

        public List<b_PrevMaintBatchEntry> PrevMaintBatchEntryList { get; set; }

        public PrevMaintBatchEntryTransactionsFromPrevMaintLibrary_V2()
        {
            base.UseDatabase = DatabaseTypeEnum.Client;
        }


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
            base.UseTransaction = true;

            //base.Transaction.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintBatchEntry> tempList = null;
            PrevMaintBatchEntryList = new List<b_PrevMaintBatchEntry>();

            try
            {

                b_PrevMaintBatchEntry be = new b_PrevMaintBatchEntry();
                be.PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibrary_V2(this.Connection, this.Transaction,
                    CallerUserInfoId, CallerUserName, clientId, siteid, ScheduleType,
                    ScheduleThroughDate, OnDemandgroup, PrintWorkOrders, PrintAttachments, AssetGroup1Ids, AssetGroup2Ids, AssetGroup3Ids, PrevMaintSchedType, PrevMaintMasterType,
                    ref tempList);

                if (tempList.Count > 0)
                {
                    tempList.ForEach(x =>
                    {
                        PrevMaintBatchEntryList.Add(x);
                    });
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
            }

        }

    }

    public class PrevMaintBatchEntryTransactionsFromPrevMaintLibraryForGenWOChunkSearch_V2 : PrevMaintBatchEntry_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            if (PrevMaintBatchEntry.PrevMaintBatchEntryId > 0)
            {
                string message = "PrevMaintBatchEntry has an invalid ID.";
                throw new Exception(message);
            }
        }
        public override void PerformWorkItem()
        {
            b_PrevMaintBatchEntry tmpList = null;
            PrevMaintBatchEntry.PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibraryChunkSearch_V2(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName, ref tmpList);
        }

        public override void Postprocess()
        {
            base.Postprocess();
        }
    }

    //public class PrevMaintBatchEntryTransactionsFromPrevMaintLibraryForGenWOChunkSearch_V2 : PrevMaintBatchEntry_TransactionBaseClass
    //{
    //    //public long clientId { get; set; }
    //    //public long siteid { get; set; }
    //    //public string ScheduleType { get; set; }
    //    //public DateTime ScheduleThroughDate { get; set; }
    //    //public string OnDemandgroup { get; set; }
    //    //public bool PrintWorkOrders { get; set; }
    //    //public bool PrintAttachments { get; set; }
    //    public long AssetGroup1Id { get; set; }
    //    public long AssetGroup2Id { get; set; }
    //    public long AssetGroup3Id { get; set; }
    //    public string PrevMaintSchedType { get; set; }
    //    public string PrevMaintMasterType { get; set; }

    //    public string OrderbyColumn { get; set; }
    //    public string OrderBy { get; set; }
    //    public  long OffSetVal { get; set; }
    //    public  long NextRow { get; set; }

    //    //public List<b_PrevMaintBatchEntry> PrevMaintBatchEntryList { get; set; }
    //    public b_PrevMaintBatchEntry PrevMaintBatchEntry { get; set; }
    //    public PrevMaintBatchEntryTransactionsFromPrevMaintLibraryForGenWOChunkSearch_V2()
    //    {
    //        base.UseDatabase = DatabaseTypeEnum.Client;
    //    }


    //    public override void Preprocess()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public override void Postprocess()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    public override void PerformLocalValidation()
    //    {
    //        base.PerformLocalValidation();
    //    }

    //    public override void PerformWorkItem()
    //    {
    //        base.UseTransaction = true;

    //        //base.Transaction.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

    //        SqlCommand command = null;
    //        string message = String.Empty;
    //        List<b_PrevMaintBatchEntry> tempList = null;
    //        PrevMaintBatchEntryList = new List<b_PrevMaintBatchEntry>();

    //        try
    //        {

    //            b_PrevMaintBatchEntry be = new b_PrevMaintBatchEntry();
    //            be.PrevMaintBatchEntry_ForWorkOrderFromPrevMaintLibraryChunkSearch_V2(this.Connection, this.Transaction,
    //                CallerUserInfoId, CallerUserName, clientId, siteid, OrderbyColumn, OrderBy, OffSetVal, NextRow,ScheduleType,
    //                ScheduleThroughDate, OnDemandgroup, PrintWorkOrders, PrintAttachments, AssetGroup1Id, AssetGroup2Id, AssetGroup3Id, PrevMaintSchedType, PrevMaintMasterType,
    //                ref tempList);

    //            if (tempList.Count > 0)
    //            {
    //                tempList.ForEach(x =>
    //                {
    //                    PrevMaintBatchEntryList.Add(x);
    //                });
    //            }

    //        }
    //        finally
    //        {
    //            if (null != command)
    //            {
    //                command.Dispose();
    //                command = null;
    //            }

    //            message = String.Empty;
    //        }

    //    }

    //}
    public class PrevMaintBatchEntry_RetrieveByBatchHeaderId : AbstractTransactionManager
    {
        public long PrevMaintBatchHeaderId { get; set; }
        public List<b_PrevMaintBatchEntry> PrevMaintBatchEntryList { get; set; }

        public PrevMaintBatchEntry_RetrieveByBatchHeaderId ()
	    {
            base.UseDatabase = DatabaseTypeEnum.Client;
	    }

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
            base.UseTransaction = true;

            //base.Transaction.IsolationLevel = System.Data.IsolationLevel.ReadUncommitted;

            SqlCommand command = null;
            string message = String.Empty;
            List<b_PrevMaintBatchEntry> tempList = null;
            PrevMaintBatchEntryList = new List<b_PrevMaintBatchEntry>();

            try
            {

                b_PrevMaintBatchEntry be = new b_PrevMaintBatchEntry();
                be.PrevMaintBatchEntry_ForWorkOrder(this.Connection, this.Transaction,
                    CallerUserInfoId, CallerUserName, PrevMaintBatchHeaderId,
                    ref tempList);

                if (tempList.Count > 0)
                {
                    tempList.ForEach(x =>  {PrevMaintBatchEntryList.Add(x);});
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
            }

        }

    }
}
