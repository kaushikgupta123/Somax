using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


//using Common;
using INTDataLayer.DAL;
using System.Text;

namespace INTDataLayer.BAL
{
    public class Mobile
    {

        public DataTable GetLookUpListByName(long UserInfoId, string UserName, long ClientID, string ListName, string Filter, string ConnectionString,long SiteId)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_LookupList_RetrieveLookupListBySearchCriteria");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@ListName", 15, ListName);
            proc.AddNVarcharPara("@ListValue", 15, "");
            proc.AddNVarcharPara("@Filter", 15, Filter);
            proc.AddNVarcharPara("@Description", 63, "");
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@OrderColumn", 256, "");
            proc.AddNVarcharPara("@OrderDirection", 256, "asc");


            DataSet ds = proc.GetDataSet(ConnectionString);
            return (ds.Tables[1]);
        }

        public DataTable MyWorkOrders(long UserInfoId, string UserName, long ClientID, string NoOfDays, bool? IncludeCompleted, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_MyWorkOrders");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            if (!string.IsNullOrEmpty(NoOfDays))
            {
                proc.AddNVarcharPara("@NoOfDays", 20, NoOfDays);
            }
            if (IncludeCompleted.HasValue)
            {
                proc.AddBooleanPara("@IncludeCompleted", IncludeCompleted.Value);
            }

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        public DataTable MyWorkOrdersWithCreatorWorkOrder(long UserInfoId, string UserName, long ClientID, string NoOfDays, bool? IncludeCompleted, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_MyWorkOrders");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            if (!string.IsNullOrEmpty(NoOfDays))
            {
                proc.AddNVarcharPara("@NoOfDays", 20, NoOfDays);
            }
            if (IncludeCompleted.HasValue)
            {
                proc.AddBooleanPara("@IncludeCompleted", IncludeCompleted.Value);
            }
            proc.AddBooleanPara("@IncludeCreatorWorkOrder", true);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        // RKL - This is used by pre version 1.4
        // Comment out - for SOM-1695 - Removal of the workimage column
        //public long WorkRequestAdd(long UserInfoId, string UserName, long ClientID, string ClientLookupId, long siteId, long ChargeToID, string ChargeType, string Description, DateTime? RequestDate, string Type, byte[] Picture,string WorkOrderStatus,string SourceType,long? RequestorID,string connectionString)
        //{
        //    long i=0;
        //    try
        //    {
        //        ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_AddWorkRequest");
        //        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        //        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        //        proc.AddBigIntegerPara("@clientId", ClientID);
        //        proc.AddNVarcharPara("@ClientLookupId", 15, ClientLookupId);
        //        proc.AddBigIntegerPara("@SiteId", siteId);
        //        proc.AddBigIntegerPara("@ChargeToId", ChargeToID);
        //        proc.AddNVarcharPara("@ChargeType", 15, ChargeType);
        //        proc.AddNVarcharPara("@Description", -1, Description);
        //        if (RequestDate!=null )//--added on 29-07-2014----by Indusnet
        //        {
        //            proc.AddDateTimePara("@RequestDate", RequestDate.Value);
        //        }

        //        proc.AddNVarcharPara("@Type", 15, Type);
        //        if (Picture != null && Picture.Length > 0)
        //        {
        //            proc.AddImagePara("@WorkImage", Picture);
        //        }
        //        proc.AddNVarcharPara("@WorkOrderStatus", 15, WorkOrderStatus);
        //        proc.AddNVarcharPara("@SourceType", 15, SourceType);

        //        if (RequestorID.HasValue)
        //        {
        //            proc.AddBigIntegerPara("@Requestor_PersonnelId", RequestorID.Value);
        //        }

        //        proc.AddBigIntegerPara("@WorkOrderId", 0, QyParameterDirection.Output);
        //        i = proc.RunActionQuery(connectionString);

        //        i = Convert.ToInt64(proc.GetParaValue("@WorkOrderId"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return i;
        //}
        // RKL - This is used by pre version 1.4
        // Comment out - for SOM-1695 - Removal of the workimage column
        /*
        public long WorkRequestAdd(long UserInfoId, string UserName, long ClientID, string ClientLookupId, long siteId, long ChargeToID, string ChargeType, string Description, DateTime? RequestDate, string Type, byte[] Picture, string WorkOrderStatus, string SourceType, long? RequestorID, string connectionString, long PersonnelId)
        {
            long i = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_AddWorkRequest");
                proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, UserName);
                proc.AddBigIntegerPara("@clientId", ClientID);
                proc.AddNVarcharPara("@ClientLookupId", 15, ClientLookupId);
                proc.AddBigIntegerPara("@SiteId", siteId);
                proc.AddBigIntegerPara("@ChargeToId", ChargeToID);
                proc.AddNVarcharPara("@ChargeType", 15, ChargeType);
                proc.AddNVarcharPara("@Description", -1, Description);
                if (RequestDate != null)//--added on 29-07-2014----by Indusnet
                {
                    proc.AddDateTimePara("@RequestDate", RequestDate.Value);
                }

                proc.AddNVarcharPara("@Type", 15, Type);
                if (Picture != null && Picture.Length > 0)
                {
                    proc.AddImagePara("@WorkImage", Picture);
                }
                proc.AddNVarcharPara("@WorkOrderStatus", 15, WorkOrderStatus);
                proc.AddNVarcharPara("@SourceType", 15, SourceType);

                if (RequestorID.HasValue)
                {
                    proc.AddBigIntegerPara("@Requestor_PersonnelId", RequestorID.Value);
                }
                proc.AddBigIntegerPara("@PersonnelId", PersonnelId);

                proc.AddBigIntegerPara("@WorkOrderId", 0, QyParameterDirection.Output);
                i = proc.RunActionQuery(connectionString);

                i = Convert.ToInt64(proc.GetParaValue("@WorkOrderId"));
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        */
        public DataSet WorkOrderDetail(long UserInfoId, string UserName, long ClientID, long WorkOrderId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_RetrieveByPKWithTask");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderId", WorkOrderId);

            DataSet ds = proc.GetDataSet(ConnectionString);
            return ds;
        }

        public DataTable WorkOrderTaskList(long clientId, int WorkOrderId, string connectionStr)
        {
            ProcedureExecute proc = new ProcedureExecute("sp_MobileService");
            proc.AddIntegerPara("@WorkOrderId", WorkOrderId);
            proc.AddBigIntegerPara("@ClientId", clientId);
            proc.AddNVarcharPara("@Action", 64, "WorkOrderTaskList");
            DataTable dt = proc.GetTable(connectionStr);
            return dt;
        }

        public DataTable LoadTaskDetail(long UserInfoId, string UserName, long ClientID, long WorkOrderTaskId, string ConnectionString)
        {

            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderTask_RetrieveByPKWithWorkOrder");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderTaskId", WorkOrderTaskId);

            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }

        public int CompleteTaskDetail(long UserInfoId, string UserName, long ClientID, long WorkOrderTaskId, DateTime StartDate, DateTime CompleteDate, decimal ActHours, int UpdateIndex, string ConnectionString)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderTask_CompleteTask");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderTaskId", WorkOrderTaskId);
            proc.AddDateTimePara("@ScheduledStartDate", StartDate);
            proc.AddDateTimePara("@CompleteDate", CompleteDate);
            proc.AddDecimalPara("@ActualDuration", 2, 8, ActHours);
            proc.AddIntegerPara("@UpdateIndex", UpdateIndex);
            proc.AddIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
            i = proc.RunActionQuery(ConnectionString);

            i = Convert.ToInt32(proc.GetParaValue("@UpdateIndexOut"));

            return i;
        }

        public DataTable WorkOrderList(long UserInfoId, string UserName, long ClientID, string ChargeTye, long? chargeTo, string Filter, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_RetrieveByCharge");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);

            if (!string.IsNullOrEmpty(ChargeTye))
            {
                proc.AddNVarcharPara("@ChargeType", 15, ChargeTye);
            }
            if (chargeTo.HasValue)
            {
                proc.AddBigIntegerPara("@ChargeToId", chargeTo.Value);
            }
            if (!string.IsNullOrEmpty(Filter))
            {
                proc.AddNVarcharPara("@Filter", 10, Filter);
            }

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        public int CancelTaskDetail(long UserInfoId, string UserName, long ClientID, long WorkOrderTaskId, string CancelReason, int UpdateIndex, string ConnectionString)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderTask_CancelTask");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderTaskId", WorkOrderTaskId);
            proc.AddNVarcharPara("@CancelReason", 15, CancelReason);
            proc.AddIntegerPara("@UpdateIndex", UpdateIndex);
            proc.AddIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
            i = proc.RunActionQuery(ConnectionString);

            i = Convert.ToInt32(proc.GetParaValue("@UpdateIndexOut"));

            return i;
        }

        public DataTable LookUpListRetrieval(long UserInfoId, string UserName, long ClientID, long LookupListId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_LookupList_RetrieveByPK");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@LookupListId", LookupListId);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }


        public int WorkOrderCompleteScreen(long UserInfoId, string UserName, long ClientID, long WorkOrderId, DateTime FinishDate, decimal ActualDuration, string FailureCode, bool CompleteAllTasksBit, string Comments, int UpdateIndex, string connectionStr)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_Complete");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderId", WorkOrderId);
            proc.AddDateTimePara("@ActualFinishDate", FinishDate);
            proc.AddDecimalPara("@ActualDuration", 2, 8, ActualDuration);
            proc.AddVarcharPara("@FailureCode", 15, FailureCode);
            proc.AddBooleanPara("@CompleteAllTasks", CompleteAllTasksBit);
            proc.AddNVarcharPara("@CompleteComments", -1, Comments);
            proc.AddIntegerPara("@UpdateIndex", UpdateIndex);
            proc.AddIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
            i = proc.RunActionQuery(connectionStr);

            i = Convert.ToInt32(proc.GetParaValue("@UpdateIndexOut"));

            return i;
        }

        public DataTable GetWorkOrderComplete(long UserInfoId, string UserName, long ClientID, long WorkOrderId, string connectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_RetrieveByWorkOrderID");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderId", WorkOrderId);
            DataTable dt = proc.GetTable(connectionString);
            return dt;
        }


        public int WorkOrderCancelScreen(long UserInfoId, string UserName, long ClientID, long WorkOrderId, DateTime CompleteDate, string CancelReason, string Comments, int UpdateIndex, string connectionStr)
        {
            int i = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_Cancel");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderId", WorkOrderId);
            proc.AddDateTimePara("@CompleteDate", CompleteDate);
            proc.AddNVarcharPara("@CancelReason", 15, CancelReason);
            proc.AddNVarcharPara("@CompleteComments", -1, Comments);
            proc.AddIntegerPara("@UpdateIndex", UpdateIndex);
            proc.AddIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
            i = proc.RunActionQuery(connectionStr);
            i = Convert.ToInt32(proc.GetParaValue("@UpdateIndexOut"));

            return i;
        }

        public DataTable WorkOrderPartsIssuedRetrieval(long UserInfoId, string UserName, long ClientID, long WorkOrderID, string ConnectionString, string ChargeType_Primary)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_RetrieveByWorkOrderId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            // proc.AddBigIntegerPara("@WorkOrderId", WorkOrderID);
            proc.AddNVarcharPara("@ChargeType_Primary", 256, ChargeType_Primary);
            proc.AddBigIntegerPara("@ChargeToId_Primary", WorkOrderID);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        //public DataTable WorkOrderPartsIssuedRetrieval(long UserInfoId, string UserName, long ClientID,long WorkOrderID,string ConnectionString)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_RetrieveByWorkOrderId");

        //    proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        //    proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        //    proc.AddBigIntegerPara("@clientId", ClientID);
        //    proc.AddBigIntegerPara("@WorkOrderId", WorkOrderID);

        //    DataTable dt = proc.GetTable(ConnectionString);
        //    return dt;
        //}

        public DataTable GetRequestorList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Personnel_Issueto");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        public DataTable PartSearchByFilterText(long UserInfoId, string UserName,
            long ClientID, long SiteId, string ConnectionString,
            string SearchText, int PageNumber, int PageSize)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Part_SearchByFilterText");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@SiteId", SiteId);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        /*-------------WorkOrder Completion Workbench----------------------------------------------------------------------------------------------------------------*/
        public DataTable WorkOrderSearchAndSortByFilterText(long UserInfoId, string UserName,
            long ClientID, long SiteId, string ConnectionString,
            string SearchText, string SortType, string ColumnName, int PageNumber, int PageSize, bool? IncludeCompleted, long WorkAssignPersonnelId)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_SortSearchByFilterText");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@SortType", 256, SortType);
            proc.AddNVarcharPara("@ColumnName", 256, ColumnName);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@WorkAssignPersonnelId", WorkAssignPersonnelId);
            if (IncludeCompleted.HasValue)
            {
                proc.AddBooleanPara("@IncludeCompleted", IncludeCompleted.Value);
            }

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        /*----------Equipment Module-------------------------------------------------------------------------------------------------------------------------*/
        //public DataTable EquipmentSearchByFilterText(long UserInfoId, string UserName, long ClientId
        //    , long SiteId, string ConnectionString, string SeatchText)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("usp_Equipment_SearchByFilterText");
        //    proc.AddBigIntegerPara("@clientId", ClientId);
        //    proc.AddNVarcharPara("@SearchText", 256, SeatchText);
        //    proc.AddBigIntegerPara("@SiteId", SiteId);

        //    DataTable dt = proc.GetTable(ConnectionString);
        //    return dt;
        //}

        //public DataTable EquipmentSortByFilterText(long UserInfoId, string UserName, long ClientId
        //    , long SiteId, string ConnectionString, string SeatchText, string SortType)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("usp_Equipment_SortByFilterText");
        //    proc.AddBigIntegerPara("@clientId", ClientId);
        //    proc.AddNVarcharPara("@SortType", 10, SortType);
        //    proc.AddNVarcharPara("@SearchText", 256, SeatchText);
        //    proc.AddBigIntegerPara("@SiteId", SiteId);

        //    DataTable dt = proc.GetTable(ConnectionString);
        //    return dt;
        //}
        // SOM-1693 - Not using this 
        //public DataTable GetEquipmentDetails(long UserInfoId, string UserName, long ClientId
        //    , long SiteId, string ConnectionString, string SearchText)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("usp_Equipment_GetEquipmentDetails");
        //    proc.AddNVarcharPara("@SearchText", 256, SearchText);
        //    proc.AddBigIntegerPara("@ClientId", ClientId);
        //    proc.AddBigIntegerPara("@SiteId", SiteId);

        //    DataTable dt = proc.GetTable(ConnectionString);
        //    return dt;
        //}
        // SOM-1693 not using this 
        //public long EquipmentImageUpload(long UserInfoId, string UserName, long ClientID, long SiteId, Int64 EquipmentId, byte[] Picture, string connectionStr)
        //{
        //    long i = 0;
        //    try
        //    {
        //        ProcedureExecute proc = new ProcedureExecute("usp_Equipment_ImageUpload");
        //        proc.AddBigIntegerPara("@EquipmentId", EquipmentId);
        //        if (Picture != null && Picture.Length > 0)
        //        {
        //            proc.AddImagePara("@ImageBinary", Picture);
        //        }
        //        proc.AddBigIntegerPara("@ClientId", ClientID);
        //        proc.AddBigIntegerPara("@SiteId", SiteId);
        //        proc.AddBigIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
        //        i = proc.RunActionQuery(connectionStr);
        //        i = Convert.ToInt64(proc.GetParaValue("@UpdateIndexOut"));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return i;
        //}
        public DataTable EquipmentSearchandSortByFilterText(long UserInfoId, string UserName, long ClientId, long SiteId, string ConnectionString, string SearchText, string SortText, string SortType, int PageNumber, int PageSize)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Equipment_SortandSearchByFilterText");
            proc.AddBigIntegerPara("@clientId", ClientId);
            proc.AddVarcharPara("@SortText", 50, SortText);
            proc.AddNVarcharPara("@SortType", 10, SortType);
            proc.AddNVarcharPara("@SearchText", 100, SearchText);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        // SOM-1693 - Not using this
        //public long EquipmentInsert(long UserInfoId, string UserName, long ClientID, long SiteID, string Name, string Make, string Model, string SerialNumber, string Location, byte[] Picture, string connectionString)
        //{
        //    long i = 0;
        //    try
        //    {
        //        ProcedureExecute proc = new ProcedureExecute("usp_Equipment_AddEquipmentRequest");
        //        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        //        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        //        proc.AddBigIntegerPara("@clientId", ClientID);
        //        proc.AddBigIntegerPara("@SiteID", SiteID);
        //        proc.AddNVarcharPara("@Name", 63, Name);
        //        proc.AddNVarcharPara("@Make", 31, Make);
        //        proc.AddNVarcharPara("@Model", 63, Model);
        //        proc.AddNVarcharPara("@SerialNumber", 63, SerialNumber);
        //        proc.AddNVarcharPara("@Location", 63, Location);
        //        proc.AddBigIntegerPara("@EquipmentId", 0, QyParameterDirection.Output);
        //        if (Picture != null && Picture.Length > 0)
        //        {
        //            proc.AddImagePara("@EquipmentImage", Picture);
        //        }
        //        i = proc.RunActionQuery(connectionString);
        //        i = Convert.ToInt64(proc.GetParaValue("@EquipmentId"));
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return i;
        //}
        public DataTable EquipmentFileList(long UserInfoId, string UserName, long ClientID, string SearchText, string connectionStr)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Equipment_FileNameList");
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            DataTable dt = proc.GetTable(connectionStr);
            return dt;
        }
        public DataTable GetEquipmentFileDetails(long UserInfoId, string UserName, long ClientID, string SearchText, string connectionStr)
        {
            StringBuilder sb = new StringBuilder();
            ProcedureExecute proc = new ProcedureExecute("usp_Equipment_FileByClientLookupId");
            proc.AddNVarcharPara("@SearchText", 100, SearchText);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            DataTable dt = proc.GetTable(connectionStr);
            return dt;
        }

        public DataTable WorkOrderFileList(long UserInfoId, string UserName, long ClientID, string SearchText, string connectionStr)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_FileNameList");
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            DataTable dt = proc.GetTable(connectionStr);
            return dt;
        }
        //SOM-1650 - Get Attachment List
        public DataTable GetAttachmentList(long UserInfoId, string UserName, long ClientID, string ObjectName, long ObjectID, string connectionStr)
        {
          ProcedureExecute proc = new ProcedureExecute("usp_Attachment_RetrieveListMobile");
          proc.AddNVarcharPara("@CallerUserName", 256, UserName);
          proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
          proc.AddBigIntegerPara("@ClientId", ClientID);
          proc.AddNVarcharPara("@ObjectName", 256, ObjectName);
          proc.AddBigIntegerPara("@ObjectID", ObjectID);
          DataTable dt = proc.GetTable(connectionStr);
          return dt;
        }

        public DataTable GetWorkOrderFileDetails(long UserInfoId, string UserName, long ClientID, string SearchText, string connectionStr)
        {
            StringBuilder sb = new StringBuilder();
            ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_FileByClientLookupId");
            proc.AddNVarcharPara("@SearchText", 100, SearchText);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            DataTable dt = proc.GetTable(connectionStr);
            return dt;
        }
        /*----------------- Part Module--------------------------------------- */
        // SOM-1694
        // Removed PartImage columns
        /*
        public long PartImageUpload(long UserInfoId, string UserName, long ClientID, long SiteId, Int64 PartId, byte[] Picture, string connectionStr)
        {
            long i = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("usp_Part_ImageUpload");
                proc.AddBigIntegerPara("@PartId", PartId);
                if (Picture != null && Picture.Length > 0)
                {
                    proc.AddImagePara("@ImageBinary", Picture);
                }
                proc.AddBigIntegerPara("@ClientId", ClientID);
                proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddBigIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
                i = proc.RunActionQuery(connectionStr);
                i = Convert.ToInt64(proc.GetParaValue("@UpdateIndexOut"));
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        */
        //public DataTable PartFileList(long UserInfoId, string UserName, long ClientID, long PartId, string connectionStr)
        //{
        //    ProcedureExecute proc = new ProcedureExecute("usp_Part_FileNameList");
        //    proc.AddBigIntegerPara("@PartId", PartId);
        //    proc.AddBigIntegerPara("@ClientId", ClientID);
        //    DataTable dt = proc.GetTable(connectionStr);
        //    return dt;
        //}

        public DataTable GetPartFileDetails(long UserInfoId, string UserName, long ClientID, long FileInfoId, string connectionStr)
        {
            StringBuilder sb = new StringBuilder();
            ProcedureExecute proc = new ProcedureExecute("usp_Part_FileDownload");
            proc.AddBigIntegerPara("@FileInfoId", FileInfoId);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            DataTable dt = proc.GetTable(connectionStr);
            return dt;
        }

        /*-------------------Work Request Module------------------------------- */

        public DataTable WorkRequestSearchAndSortByFilterText(long UserInfoId, string UserName,
           long ClientID, long SiteId, string ConnectionString,
           string SearchText, string SortType, string ColumnName, int PageNumber, int PageSize)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_WorkRequest_SortSearchByFilterText");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@SortType", 256, SortType);
            proc.AddNVarcharPara("@ColumnName", 256, ColumnName);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }



        //------------------------------ Sanitation Job --------------------------------------------//
        public DataTable SanitationJobSearchAndSortByFilterText(long UserInfoId, string UserName,
           long ClientID, long SiteId, string ConnectionString,
           string SearchText, string SortType, string ColumnName, int PageNumber, int PageSize, bool? IncludeCompleted, long AssignPersonnelId)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_SanitationJob_SortSearchByFilterText");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@SortType", 256, SortType);
            proc.AddNVarcharPara("@ColumnName", 256, ColumnName);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@AssignPersonnelId", AssignPersonnelId);
            if (IncludeCompleted.HasValue)
            {
                proc.AddBooleanPara("@IncludeCompleted", IncludeCompleted.Value);
            }

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        //----------------------------  Sanitation Job  ----------------------------------------------//
        //----------------------------  Sanitation Verification Job----------------------------------//
        public DataTable SanitationJobVerificationWorkbenchSearchAndSortByFilterText(long UserInfoId, string UserName,
         long ClientID, long SiteId, string ConnectionString, string SearchText, string SortType, string ColumnName, int PageNumber,
         int PageSize, bool? IncludeCompleted, long AssignPersonnelId)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_SanitationJob_VerificationSortSearchByFilterText");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@SortType", 256, SortType);
            proc.AddNVarcharPara("@ColumnName", 256, ColumnName);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@AssignPersonnelId", AssignPersonnelId);
            if (IncludeCompleted.HasValue)
            {
                proc.AddBooleanPara("@IncludeCompleted", IncludeCompleted.Value);
            }

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        //----------------------------  Sanitation Verification Job----------------------------------//
        public DataTable GetAlertList(long UserInfoId, long ClientID, string ConnectionString, int PageNumber, int PageSize)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Alert_AlertList_Retrieval");

            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@UserId", UserInfoId);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        //AV2-23 /IV2-38 Begin
        public DataTable GetAlertListByType(long UserInfoId, long ClientID, string ConnectionString, int PageNumber, int PageSize, string NotificationType)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Alert_AlertList_RetrievalByType");

            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@UserId", UserInfoId);
            proc.AddNVarcharPara("@NotificationType",20, NotificationType);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        //AV2-23 /IV2-38 End
        public DataTable GetAlertDetails(long UserInfoId, long ClientID, string ConnectionString, long AlertId)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Alert_GetAlertDetails");

            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@AlertId", AlertId);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        public int SaveLoginRegistrationKey(long UserId, string RegistrationKey, string DeviceType, string ConnectionString)
        {

            int i = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("usp_InsertLoginRegistrationKey");
                proc.AddBigIntegerPara("@UserId", UserId);
                proc.AddNVarcharPara("@RegistrationKey", 256, RegistrationKey);
                proc.AddNVarcharPara("@DeviceType", 256, DeviceType);

                i = proc.RunActionQuery(ConnectionString);


            }
            catch (Exception ex)
            {

            }
            return i;

        }


        public int DeleteLoginRegistrationKey(long UserId, string RegistrationKey, string ConnectionString)
        {
            int i = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("usp_DeleteLoginRegistrationKey");

                proc.AddBigIntegerPara("@UserId", UserId);
                proc.AddNVarcharPara("@RegistrationKey", 256, RegistrationKey);
                i = proc.RunActionQuery(ConnectionString);
            }
            catch (Exception ex)
            {

            }
            return i;
        }

        public DataTable GetLoginRegDetails(long UserId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_LoginRegDetails_RetrieveAll");

            proc.AddBigIntegerPara("@UserId", UserId);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public int Updatetoken(long UserId, string OldToken, string NewToken, string DeviceType, string ConnectionString)
        {
            int i = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("usp_LoginRegDetails_UpdateToken");

                proc.AddBigIntegerPara("@UserId", UserId);
                proc.AddNVarcharPara("@OldTokenKey", 600, OldToken);
                proc.AddNVarcharPara("@NewTokenKey", 600, NewToken);
                proc.AddNVarcharPara("@DeviceType", 10, DeviceType);

                i = proc.RunActionQuery(ConnectionString);
            }
            catch (Exception ex)
            {

            }
            return i;
        }

        public int UpdateBadge(string RegistrationKey, string ConnectionString, bool BadgeReset = false)
        {
            int i = 0;
            try
            {
                ProcedureExecute proc = new ProcedureExecute("usp_LoginRegDetails_UpdateBadge");

                proc.AddNVarcharPara("@RegistrationKey", 256, RegistrationKey);
                proc.AddBooleanPara("@BadgeReset", BadgeReset);
                i = proc.RunActionQuery(ConnectionString);
            }
            catch (Exception ex)
            {

            }
            return i;
        }
        // RKL - SOM-1650 
        // SOM-1693,SOM-1694,SOM-1695 - Removed the ClearImageInTable Method - Columns no longer exist
        //--IV2-46/Av2-26--//
        public DataTable EventInfoSearchSortByFilterText(long UserInfoId, string UserName, long ClientID, long SiteId, string ConnectionString,
            string SearchText, string SortType, string ColumnName, int PageNumber, int PageSize, int CaseNo)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_EventInfo_SearchSortByFilterText");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@SearchText", 256, SearchText);
            proc.AddBigIntegerPara("@SiteId", SiteId);
            proc.AddNVarcharPara("@SortType", 256, SortType);
            proc.AddNVarcharPara("@ColumnName", 256, ColumnName);
            proc.AddBigIntegerPara("@PageNumber", PageNumber);
            proc.AddBigIntegerPara("@PageSize", PageSize);
            proc.AddBigIntegerPara("@CaseNo", CaseNo);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
    }
}
