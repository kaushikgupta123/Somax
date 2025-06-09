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
* 2014-Aug-04 SOM-284  Nick Fuchs         Created
* 2014-Sep-30 SOM-349  Nick Fuchs         Move the Date to user time zone conversion to the BAL
* 2014-Oct-04 SOM-353  Nick Fuchs         Add PMPercentCompletion
* 2014-Oct-10 SOM-365  Nick Fuchs         Replace the CompletionDate with the ActualFinishDate
* 2014-Oct-10 SOM-366  Nick Fuchs         Replace the CompletionDate with the ActualFinishDate
* 2014-Oct-17 SOM-375  Nick Fuchs         Added GetWorkOrdersByTypePieChartData
* 2014-Nov-20 SOM-437  Nick Fuchs         Added GetWOLaborHoursByTypePieChartData
* 2014-Nov-22 SOM-438  Nick Fuchs         Added GetWOProactiveVsReactivePieChartData
* 2014-Nov-22 SOM-442  Nick Fuchs         Added GetWOTop10EmergencyHrsChartData
* 2014-Nov-24 SOM-455  Nick Fuchs         Added GetWOHoursByPersonnelData
* 2014-Dec-09 SOM-468  Nick Fuchs         Added GetWOMaintenanceManagementReportData 
* 2015-Feb-01 SOM-507  Nick Fuchs         Added GetWOShiftHoursChartData 
* 2015-Feb-19 SOM-567  Nick Fuchs         Added GetPersonnelShiftHoursChartData 
* 2015-Feb-27 SOM-583  Nick Fuchs         No need to use the time value of the date columns
* 2016-Mar-31 SOM-958  Roger Lawton       Remove conversion between UTC and User's Timezone 
* 2016-Nov-08 SOM-1165 Roger Lawton       Corrections to date handling 
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
  public class WorkOrderBAL
  {
     
    public DataTable GetWorkOrderByTypeList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ByTypeRetrieveForGroupReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      foreach (DataRow worow in dt.Rows)
      {
        // sp returns the following dates: [CreateDate],[ScheduledStartDate],[ActualFinishDate]
        #region Date Conversions 
        if (worow.Field<DateTime?>("ActualFinishDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = worow.Field<DateTime>("ActualFinishDate").ToUserTimeZone(userTimeZone);
          worow.SetField<DateTime>("ActualFinishDate", newDateTime);
        }
        if (worow.Field<DateTime?>("ScheduledStartDate").HasValue)    // checking if date is null
        {
            DateTime minDate = new DateTime(0001, 01, 01);
            DateTime SchedStartDate = (DateTime)worow.Field<DateTime>("ScheduledStartDate");
            if (minDate.Date == SchedStartDate.Date)
            {
                DateTime? newDateTime = null;
                worow.SetField<DateTime?>("ScheduledStartDate", newDateTime);
            }
            else
            {
                DateTime ScheduledStartDate = worow.Field<DateTime>("ScheduledStartDate").ToUserTimeZone(userTimeZone);
                worow.SetField<DateTime>("ScheduledStartDate", ScheduledStartDate);
            }
        }
        if (worow.Field<DateTime?>("CreateDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = worow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
          worow.SetField<DateTime>("CreateDate", newDateTime);
        }
        
        #endregion
      }
      return dt;
    }

    public DataTable GetWorkOrderCompletedByAssignedList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_CompletedByAssignedReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      
      foreach (DataRow worow in dt.Rows)
      {
        // sp returns the following dates: WorkOrder.CreateDate,ActualFinishDate
        #region Date Conversions
        // SOM_365
        if (worow.Field<DateTime?>("CreateDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = worow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
          worow.SetField<DateTime>("CreateDate", newDateTime);
        }
        // SOM-1165 - Actual Finish Data is NOT in UTC
        //if (worow.Field<DateTime?>("ActualFinishDate").HasValue)    // checking if date is null
        //{
        //  DateTime newDateTime = worow.Field<DateTime>("ActualFinishDate").ToUserTimeZone(userTimeZone);
        //  worow.SetField<DateTime>("ActualFinishDate", newDateTime);
        //}
        #endregion
      }
      return dt;
    }

    public DataTable GetWorkOrderAssignedList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_GetAssignedList");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
    
      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }

    // SOM-353 Begin
    public DataTable GetWorkOrderPMCompletion(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_PM_Percent_CompletionReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      // SOM-583 Begin
      //foreach (DataRow worow in dt.Rows)
      //{
      //  // sp returns the following dates: [ScheduledStartDate],[CompleteDate]
      //  // SOM-366
      //  #region Date Conversions
      //  if (worow.Field<DateTime?>("ScheduledStartDate").HasValue)    // checking if date is null
      //  {
      //    DateTime newDateTime = worow.Field<DateTime>("ScheduledStartDate").ToUserTimeZone(userTimeZone);
      //    worow.SetField<DateTime>("ScheduledStartDate", newDateTime);
      //  }
      //  if (worow.Field<DateTime?>("ActualFinishDate").HasValue)  
      //  {
      //    DateTime newDateTime = worow.Field<DateTime>("ActualFinishDate").ToUserTimeZone(userTimeZone);
      //    worow.SetField<DateTime>("ActualFinishDate", newDateTime);
      //  }
      //  #endregion
      //}
      // SOM-583 End
      return dt;
    }
    // SOM-353 End

    // SOM-375 Begin
    //public DataTable GetWorkOrdersByTypePieChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, long RegionID)
    public DataTable GetWorkOrdersByTypePieChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ByTypeChartReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);
      //proc.AddBigIntegerPara("@RegionId", RegionID);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
    // SOM-375 End

    // SOM-437 Begin
    public DataTable GetWOLaborHoursByTypePieChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_LaborHoursByTypeChartReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
    // SOM-437 End

    // SOM-438 Begin
    public DataTable GetWOProactiveVsReactivePieChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ProactiveReactiveChartReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
    // SOM-438 End
    
    // SOM-442 Begin
    public DataTable GetWOTop10EmergencyHrsChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_Top10EmergencyHoursChartReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
    // SOM-442 End
    // SOM-455 Begin
    public DataTable GetWOHoursByPersonnelData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_HoursByAssignedReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      // SOM-958 - do NOT convert these date/times
      /* 
      foreach (DataRow tcrow in dt.Rows)
      {
        // sp returns the following date: WorkOrder.StartDate
        #region Date Conversions
        if (tcrow.Field<DateTime?>("StartDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = tcrow.Field<DateTime>("StartDate").ToUserTimeZone(userTimeZone);
          tcrow.SetField<DateTime>("StartDate", newDateTime);
        }
        #endregion
      }
      */
      return dt;
    }
    // SOM-455 End

    // SOM-468 Begin
    public DataTable GetWOMaintenanceManagementReportData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_MaintenanceMgmtReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);

      DataTable dt = proc.GetTable(ConnectionString);
      foreach (DataRow tcrow in dt.Rows)
      {
        // sp returns the following dates: WorkOrder.CreateDate, WorkOrder.ScheduledStartDate, WorkOrder.ActualFinishDate, Timecard.StartDate
        #region Date Conversions
        if (tcrow.Field<DateTime?>("CreateDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = tcrow.Field<DateTime>("CreateDate").ToUserTimeZone(userTimeZone);
          tcrow.SetField<DateTime>("CreateDate", newDateTime);
        }
        if (tcrow.Field<DateTime?>("ScheduledStartDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = tcrow.Field<DateTime>("ScheduledStartDate").ToUserTimeZone(userTimeZone);
          tcrow.SetField<DateTime>("ScheduledStartDate", newDateTime);
        }
        if (tcrow.Field<DateTime?>("ActualFinishDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = tcrow.Field<DateTime>("ActualFinishDate").ToUserTimeZone(userTimeZone);
          tcrow.SetField<DateTime>("ActualFinishDate", newDateTime);
        }
        if (tcrow.Field<DateTime?>("StartDate").HasValue)    // checking if date is null
        {
          DateTime newDateTime = tcrow.Field<DateTime>("StartDate").ToUserTimeZone(userTimeZone);
          tcrow.SetField<DateTime>("StartDate", newDateTime);
        }
        #endregion
      }
      return dt;
    }
    // SOM-468 End

      // SOM-507 Begin
    public DataTable GetWOShiftHoursChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string Shift, DateTime BeginDate, DateTime EndDate)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ShiftHourChart_Report");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddNVarcharPara("@Shift", 15, Shift);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
    // SOM-507 End


    // SOM-517 & SOM-519
    public DataTable GetDailyLabourReport(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone) {
        ProcedureExecute proc = new ProcedureExecute("usp_Timecard_Daily_Labor_Report");
        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        proc.AddBigIntegerPara("@clientId", ClientID);
        proc.AddBigIntegerPara("@SiteId", SiteID);
        proc.AddDateTimePara("@stDate", BeginDate);
        proc.AddDateTimePara("@fnDate", EndDate);
        DataTable dt = proc.GetTable(ConnectionString);
        // SOM-839
        // The Timecard.StartDate is stored in the user's local time not utc
        // No conversion is needed
        //foreach (DataRow ptrow in dt.Rows) 
        //{
        //    if (ptrow.Field<DateTime?>("Timecard.StartDate").HasValue) 
        //    {
        //        DateTime newDateTime = ptrow.Field<DateTime>("Timecard.StartDate").ToUserTimeZone(userTimeZone);
        //        ptrow.SetField<DateTime>("Timecard.StartDate", newDateTime);
        //    }
        //}
        return dt;
    }
    // SOM-521 Begin
    public DataTable GetMaintenancePersonalReportCard(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone) 
    {
        using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_Personnel_ReportCard")) {

            proc.AddBigIntegerPara("ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);
            DataTable dsPersonalReportCard = new DataTable();

            dsPersonalReportCard = proc.GetTable(ConnectionString);
            return dsPersonalReportCard;
        }
     }
    // SOM-521 END
    // SOM-567 Begin
    public DataTable GetPersonnelShiftHoursChartData(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, long PersonnelId, DateTime BeginDate, DateTime EndDate)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_PersonnelLaborHoursByTypeChartReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);
      proc.AddBigIntegerPara("@PersonnelId", PersonnelId);
      proc.AddDateTimePara("@stDate", BeginDate);
      proc.AddDateTimePara("@fnDate", EndDate);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
    // SOM-567 End
    public DataTable GetWorkOrderActualCost(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime StartDate, DateTime EndDate, string userTimeZone)
    {
        ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ActualCosts_Report");

        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        proc.AddBigIntegerPara("@clientId", ClientID);
        proc.AddBigIntegerPara("@SiteId", SiteID);
        proc.AddDateTimePara("@stDate", StartDate);
        proc.AddDateTimePara("@fnDate", EndDate);
        DataTable dt = proc.GetTable(ConnectionString);
        return dt;
    }
    public DataTable WorkOrderActualCostsbyLocation(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
    {
        using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ActualCostsByEquipmentLocationId_Report"))
        {
            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
    }

    public DataTable GetWorkOrderByScheduleId(long UserInfoId, string UserName, long ClientID, long SiteID,long ScheduleId, string ConnectionString)
    {
        ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_RetrieveByScheduleId");

        proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        proc.AddBigIntegerPara("@clientId", ClientID);
        proc.AddBigIntegerPara("@SiteId", SiteID);
        proc.AddBigIntegerPara("@ScheduleId", ScheduleId);

        DataTable dt = proc.GetTable(ConnectionString);
        return dt;
    }


        public DataTable GetPartHistoryRetrieveByWorkOrderId(long UserInfoId, string UserName, long ClientID, long WorkOrderId, string ChargeType_Primary, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_RetrieveByWorkOrderId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@ChargeType_Primary", 15, ChargeType_Primary);
            proc.AddBigIntegerPara("@ChargeToId_Primary", WorkOrderId);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetTimecardRetrieveByWorkOrderId(long UserInfoId, string UserName, long ClientID, long WorkOrderId,string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Timecard_RetrieveByWorkOrderId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@WorkOrderId", WorkOrderId);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetOtherCostsRetrieveByObjectId(long UserInfoId, string UserName, long ClientID, long WorkOrderId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_OtherCosts_RetrieveByObjectId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@ObjectId", WorkOrderId);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetOtherCostsSummeryRetrieveByObjectId(long UserInfoId, string UserName, long ClientID, long WorkOrderId, string Object, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_OtherCosts_SummeryRetrieveByObjectId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("@ObjectType", 15, Object);
            proc.AddBigIntegerPara("@ObjectId", WorkOrderId);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }

    }

}

