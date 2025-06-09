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
* 2017-Mar-01 SOM-1214 Nick Fuchs         Next Due does NOT need to be converted
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
    public class PreventiveMainenanceBAL
    {
        public DataTable GetPrevMaintSchedList(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PrevMaintMaster_PrevMaintListReport");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);

            DataTable dt = proc.GetTable(ConnectionString);
            foreach (DataRow pmrow in dt.Rows)
            {
                // sp returns the following dates: [LastPerformed],[NextDueDate]
                #region Date Conversions
                if (pmrow.Field<DateTime?>("LastPerformed").HasValue)    // checking if date is null
                {
                    DateTime newDateTime = pmrow.Field<DateTime>("LastPerformed").ToUserTimeZone(userTimeZone);
                    pmrow.SetField<DateTime>("LastPerformed", newDateTime);
                }

                //SOM-1214
                //if (pmrow.Field<DateTime?>("NextDueDate").HasValue)    // checking if date is null
                //{
                //  DateTime newDateTime = pmrow.Field<DateTime>("NextDueDate").ToUserTimeZone(userTimeZone);
                //  pmrow.SetField<DateTime>("NextDueDate", newDateTime);
                //}
                #endregion
            }
            return dt;
        }
        public DataTable GetAverageWorkloadByMechanic(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PrevMaintSched_Average_Workload_Mechanic_Report");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public DataTable GetMasterJobDetailsBAL(long UserInfoId, string UserName, long ClientID, long PrevMaintMstrId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PrevMaintMaster_RetrieveByPK");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PrevMaintMasterId", PrevMaintMstrId);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public DataTable GetScheduleRecordsBAL(long UserInfoId, string UserName, long ClientID, long SiteID, long PrevMaintMstrId, string userTimeZone, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PrevMaintSched_RetrieveByPrevMaintMasterId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddBigIntegerPara("@PrevMaintMasterId", PrevMaintMstrId);

            DataTable dt = proc.GetTable(ConnectionString);
            foreach (DataRow pmrow in dt.Rows)
            {
                if (pmrow.Field<DateTime?>("NextDueDate").HasValue)    // checking if date is null
                {
                    DateTime newDateTime = pmrow.Field<DateTime>("NextDueDate").ToUserTimeZone(userTimeZone);
                    pmrow.SetField<DateTime>("NextDueDate", newDateTime);
                }
            }
            return dt;
        }
        public DataTable GetPMTaksBAL(long UserInfoId, string UserName, long ClientID, long PrevMaintMstrId, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PrevMaintTask_RetrieveByPrevMaintMasterId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PrevMaintMasterId", PrevMaintMstrId);

            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetPMEstimatesPartBAL(long UserInfoId, string UserName, long ClientID, long PrevMaintMstrId, string Category, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_EstimatedCosts_RetrieveForPrevMaintMaster");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PrevMaintMasterId", PrevMaintMstrId);
            proc.AddNVarcharPara("@Category", 15, Category);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetPMEstimatesLaborBAL(long UserInfoId, string UserName, long ClientID, long PrevMaintMstrId, string Category, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_EstimatedCosts_RetrieveForPrevMaintMaster");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@PrevMaintMasterId", PrevMaintMstrId);
            proc.AddNVarcharPara("@Category", 15, Category);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetPMEstimatesOtherBAL(long UserInfoId, string UserName, long ClientID, long PrevMaintMstrId, string Object, string Category, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_EstimatedCosts_RetrieveByObjectId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@ObjectId", PrevMaintMstrId);
            proc.AddNVarcharPara("ObjectType", 15, Object);
            proc.AddNVarcharPara("@Category", 20, Category);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }
        public DataTable GetPMEstimatesSummaryBAL(long UserInfoId, string UserName, long ClientID, long PrevMaintMstrId, string Object, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_EstimateSummeryRetrieveByObjectId");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddNVarcharPara("ObjectType", 15, Object);
            proc.AddBigIntegerPara("@ObjectId", PrevMaintMstrId);


            DataTable dt = proc.GetTable(ConnectionString);

            return dt;
        }



    }
}

