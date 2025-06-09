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
* 2014-Sep-30 SOM-349  Nick Fuchs         Move the Date to user time zone conversion to the BAL
* 2014-Oct-27 SOM-371  Nick Fuchs         Added GetPartEvaluation 
* 2014-Dec-04 SOM-466  Nick Fuchs         Added GetPartHistoryTransactions 
* 2014-Dec-16 SOM-477  Nick Fuchs         Added GetPartHistoryReceipts 
* 2014-Dec-17 SOM-484  Nick Fuchs         Added GetPartHistoryPhysicalInventory
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOMAX.G4.Data.INTDataLayer.DAL;
using SOMAX.G4.Data.INTDataLayer.EL;
using SOMAX.G4.Data.Common.Extensions;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
  public class PartBAL
  {
    public DataTable GetPartDetails(string strClientLookupId, string conString, Int64 userid, Int64 clientid, string username)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Part_RetrieveAll"))
      {
        proc.AddBigIntegerPara("@CallerUserInfoId", userid);
        proc.AddNVarcharPara("@CallerUserName", 255, username);
        proc.AddBigIntegerPara("@ClientId", clientid);
        return proc.GetTable(conString);
      }
    }

    public DataTable GetPartsIssuedList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
        ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_IssuesRetrieveForListReport");

        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        proc.AddBigIntegerPara("@clientId", ClientID);
        proc.AddBigIntegerPara("@SiteId", SiteID);
        proc.AddDateTimePara("@stDate", BeginDate);
        proc.AddDateTimePara("@fnDate", EndDate);

        DataTable dt = proc.GetTable(ConnectionString);
        foreach (DataRow ptrow in dt.Rows)
        {
          DateTime newDateTime = ptrow.Field<DateTime>("TransactionDate").ToUserTimeZone(userTimeZone);
          ptrow.SetField<DateTime>("TransactionDate", newDateTime);
        }    
        return dt;
    }

    // SOM-371
    public DataTable GetPartEvaluation(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_Part_InventoryValuationReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }

    // SOM-466
    public DataTable GetPartHistoryTransactions(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_PartTransactionsReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      foreach (DataRow ptrow in dt.Rows)
      {
        DateTime newDateTime = ptrow.Field<DateTime>("TransactionDate").ToUserTimeZone(userTimeZone);
        ptrow.SetField<DateTime>("TransactionDate", newDateTime);
      }
      return dt;
    }

    // SOM-477
    public DataTable GetPartHistoryReceipts(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_PartReceiptsReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      foreach (DataRow ptrow in dt.Rows)
      {
        DateTime newDateTime = ptrow.Field<DateTime>("TransactionDate").ToUserTimeZone(userTimeZone);
        ptrow.SetField<DateTime>("TransactionDate", newDateTime);
      }
      return dt;
    }

    // SOM-484
    public DataTable GetPartHistoryPhysicalInventory(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_PhysicalInventoryReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      foreach (DataRow ptrow in dt.Rows)
      {
        DateTime newDateTime = ptrow.Field<DateTime>("TransactionDate").ToUserTimeZone(userTimeZone);
        ptrow.SetField<DateTime>("TransactionDate", newDateTime);
      }
      return dt;
    }
  }
}
