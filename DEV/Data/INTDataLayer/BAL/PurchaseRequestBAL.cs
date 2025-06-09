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
* 2015-Jan-31 SOM-823  Indusnet          Added date conversions to the GetPurchaseRequestList()
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SOMAX.G4.Data.INTDataLayer.DAL;
using SOMAX.G4.Data.Common.Extensions;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
    public class PurchaseRequestBAL
  {
    public DataTable GetPurchaseRequestList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_PurchaseOrder_OpenPO_Report");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);

      DataTable dt = proc.GetTable(ConnectionString);
     
      return dt;
    }
    public DataTable GetPurchaseRequestStatus(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString)
    {
        ProcedureExecute proc = new ProcedureExecute("usp_PurchaseRequest_RetriveStatus");

        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        proc.AddBigIntegerPara("@clientId", ClientID);
        proc.AddBigIntegerPara("@SiteId", SiteID);

        DataTable dt = proc.GetTable(ConnectionString);

        return dt;
    }
    public DataTable RetrievePurchaseRequestList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {

        using (ProcedureExecute proc = new ProcedureExecute("usp_PurchaseRequests_Report"))
        {

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 128, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);

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