/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2015-Jan-31 SOM-506  Nick Fuchs         Added date conversions to the GetOpenPurchaseOrderList()
* 2015-Oct-29 SOM-835  Indus Net          Added Purchase Order Cost by Account Summary
* 2016-Aug-01 SOM-1043 Roger Lawton       GetComittedDollars - converted created date to user's time
*                                         zone
*                                         GetPurchaseOrderCostByAccount - convert create date to 
*                                         user's time zone
*                                         GetPurchaseOrderCostByVendor - convert create date to 
*                                         user's time zone
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using INTDataLayer.DAL;
using Common.Extensions;

namespace INTDataLayer.BAL
{
    public class PurchaseOrderBAL
    {
        public DataTable GetOpenPurchaseOrderList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrder_OpenPO_Report");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);

            DataTable dt = proc.GetTable(ConnectionString);
            foreach (DataRow POrow in dt.Rows)
            {
                // sp returns the following dates: [PODate] (actually is the PurchaseOrder.CreateDate), [EstimatedDelivery]
                #region Date Conversions
                if (POrow.Field<DateTime?>("PODate").HasValue)    // checking if date is null
                {
                    DateTime newDateTime = POrow.Field<DateTime>("PODate").ToUserTimeZone(userTimeZone);
                    POrow.SetField<DateTime>("PODate", newDateTime);
                }
                if (POrow.Field<DateTime?>("EstimatedDelivery").HasValue)    // checking if date is null
                {
                    DateTime minDate = new DateTime(1900, 01, 01);
                    DateTime EstimatedDelivery = (DateTime)POrow.Field<DateTime?>("EstimatedDelivery");
                    if (minDate.Date == EstimatedDelivery.Date)
                    {
                        DateTime? newDateTime = null;
                        POrow.SetField<DateTime?>("EstimatedDelivery", newDateTime);
                    }
                }
                if (POrow.Field<DateTime?>("EstimatedDelivery").HasValue)    // checking if date is null
                {
                    DateTime minDate = new DateTime(0001, 01, 01);
                    DateTime EstimatedDelivery = (DateTime)POrow.Field<DateTime?>("EstimatedDelivery");
                    if (minDate.Date == EstimatedDelivery.Date)
                    {
                        DateTime? newDateTime = null;
                        POrow.SetField<DateTime?>("EstimatedDelivery", newDateTime);
                    }
                }
                #endregion
            }
            return dt;
        }

        //SOM-522 Start
        public DataTable GetComittedDollars(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrder_CommittedDollarsReport_Report");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            DataTable dt = proc.GetTable(ConnectionString);
            // SOM-1043
            foreach (DataRow ptrow in dt.Rows)
            {
                if (ptrow.Field<DateTime?>("PurchaseOrder.CreateDate").HasValue)
                {
                    DateTime newDateTime = ptrow.Field<DateTime>("PurchaseOrder.CreateDate").ToUserTimeZone(userTimeZone);
                    ptrow.SetField<DateTime>("PurchaseOrder.CreateDate", newDateTime);
                }
            }
            return dt;
        }
        //SOM-522 END

        public DataTable GetDetailByPurchaseOrderId(long UserInfoId, string UserName, long ClientID, long PurchaseOrderId,long POReceiptHeaderId ,string ConnectionString, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrderLineItem_RetrieveForReport");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PurchaseOrderId", PurchaseOrderId);
            proc.AddBigIntegerPara("@POReceiptHeaderId", POReceiptHeaderId);

            DataTable dt = proc.GetTable(ConnectionString);
            
            return dt;
        }

        public DataTable GetReceiptNumberbyPurchaseOrderId(long UserInfoId, string UserName, long ClientID, long POReceiptHeaderId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_POReceiptHeader_RetrieveReceiptItemByPurchaseOrderId");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@POReceiptHeaderId", POReceiptHeaderId);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public DataTable GetVendorDetailsbyPurchaseOrderIdandReceiptNumber(long UserInfoId, string UserName, long ClientID, long PurchaseOrderId, long POReceiptHeaderId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrderLineItem_RetrieveForReportHeader");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PurchaseOrderId", PurchaseOrderId);
            proc.AddBigIntegerPara("@POReceiptHeaderId", POReceiptHeaderId);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

        // SOM-553
        public DataTable GetReceiptListByAccounts(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_PartReceiptsByAccountReport");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);
            DataTable dt = proc.GetTable(ConnectionString);
            foreach (DataRow ptrow in dt.Rows)
            {
                if (ptrow.Field<DateTime?>("TransactionDate").HasValue)
                {
                    DateTime newDateTime = ptrow.Field<DateTime>("TransactionDate").ToUserTimeZone(userTimeZone);
                    ptrow.SetField<DateTime>("TransactionDate", newDateTime);
                }
            }
            return dt;
        }
        // SOM-554
        public DataTable GetPurchaseOrderAmountReceived(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrder_AmountReceivedReport_Report");
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);
            DataTable dt = proc.GetTable(ConnectionString);
            foreach (DataRow ptrow in dt.Rows)
            {
                if (ptrow.Field<DateTime?>("PO.CreateDate").HasValue)
                {
                    DateTime newDateTime = ptrow.Field<DateTime>("PO.CreateDate").ToUserTimeZone(userTimeZone);
                    ptrow.SetField<DateTime>("PO.CreateDate", newDateTime);
                }
            }
            return dt;
        }
        public DataTable RetrievePurchaseOrderByAccount(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
        {

            using (ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrders_by_Account"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, UserName);
                proc.AddBigIntegerPara("@ClientId", ClientID);
                proc.AddDateTimePara("@stDate", BeginDate);
                proc.AddDateTimePara("@fnDate", EndDate);
                proc.AddBigIntegerPara("@SiteId", SiteID);

                DataTable dt = proc.GetTable(ConnectionString);
                foreach (DataRow ptrow in dt.Rows)
                {
                    if (ptrow.Field<DateTime?>("CreateDate").HasValue)
                    {
                        DateTime newDateTime = ptrow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
                        ptrow.SetField<DateTime>("CreateDate", newDateTime);
                    }
                }
                return dt;
            }
        }
        //SOM - 835
        public DataTable GetPurchaseOrderCostByAccount(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone, DateTime BeginDate, DateTime EndDate)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrderCosts_by_Account");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);

            DataTable dt = proc.GetTable(ConnectionString);
            // SOM-1043
            // Returning PurchaseOrder.CreateDate - not shown on report - but need to convert in case is it shown later
            foreach (DataRow ptrow in dt.Rows)
            {
                if (ptrow.Field<DateTime?>("CreateDate").HasValue)
                {
                    DateTime newDateTime = ptrow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
                    ptrow.SetField<DateTime>("CreateDate", newDateTime);
                }
            }
            return dt;
        }
        public DataTable GetPurchaseOrderCostByVendor(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone, DateTime BeginDate, DateTime EndDate)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrderCosts_by_Vendor");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);

            DataTable dt = proc.GetTable(ConnectionString);
            // SOM-1043
            // Returning PurchaseOrder.CreateDate - not shown on report - but need to convert in case is it shown later
            foreach (DataRow ptrow in dt.Rows)
            {
                if (ptrow.Field<DateTime?>("CreateDate").HasValue)
                {
                    DateTime newDateTime = ptrow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
                    ptrow.SetField<DateTime>("CreateDate", newDateTime);
                }
            }
            return dt;
        }
        public DataTable RetrievePurchaseOrderByVendor(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
        {

            using (ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrderCosts_by_Vendor"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, UserName);
                proc.AddBigIntegerPara("@ClientId", ClientID);
                proc.AddDateTimePara("@stDate", BeginDate);
                proc.AddDateTimePara("@fnDate", EndDate);
                proc.AddBigIntegerPara("@SiteId", SiteID);

                DataTable dt = proc.GetTable(ConnectionString);
                foreach (DataRow ptrow in dt.Rows)
                {
                    if (ptrow.Field<DateTime?>("CreateDate").HasValue)
                    {
                        DateTime newDateTime = ptrow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
                        ptrow.SetField<DateTime>("CreateDate", newDateTime);
                    }
                }
                return dt;
            }
        }

    }
}