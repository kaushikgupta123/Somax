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
* Notes
**************************************************************************************************
* Date        JIRA Item  Person           Description
* =========== ========== ================ ========================================================
* 2014-Nov-24 SOM-457    Roger Lawton     Returning incorrect work orders
*                                         Added @SiteId parameter to 
*                                           GetWorkOrderSearchCriteria
*                                           GetWorkOrderSchedulesSearchCriteria
* 2017-Feb-06 SOM-1221   Nick Fuchs       Remove UTC conversion                                           
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTDataLayer.DAL;
using INTDataLayer.EL;
using Common.Extensions;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;


namespace INTDataLayer.BAL
{
    public class LaborSchedulingBAL
    {
        public DataTable GetQueryResult(UserEL objUserEL, string sqlString, string conString, bool Flag)
        {
            using (ProcedureExecute proc = new ProcedureExecute(sqlString, true))
            {
                if (Flag == true)
                {
                    //  string conn = ConfigurationManager.ConnectionStrings[WebConfigConstants.CLIENT_CONNECTION_STRING].ToString();
                    string conn = conString;
                    return proc.GetTable(conn);
                }
                else
                    return proc.GetTable(conString);

            }

        }
        public DataTable GetFileAttachmentDetailsForPrint(UserEL objUserEL, Int64 objectId, string tableName, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_FileInfo_RetrieveByObjectId"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@ObjectId", objectId);
                proc.AddVarcharPara("@TableName", 128, tableName);
                return proc.GetTable(conString);
            }
        }
        public DataTable GetScheduledWorkOrders(UserEL objUserEL, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_ScheduledWorkOrders"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", objUserEL.SiteId);
                proc.AddBigIntegerPara("@PersonnelId", objUserEL.PersonnelId);
                return proc.GetTable(conString);
            }
        }

        //public DataTable GetWorkOrderSearchCriteria(UserEL objUserEL, string DateFilter, string IncludeDate, string workordertype, string workorderpriority, string flag, string personnelid, string conString)
        //{
        //    using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_GetWorkOrderBySearchCriteria"))
        //    {
        //        proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
        //        proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
        //        proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
        //     //   proc.AddNVarcharPara("@DateSelection", 128, DateFilter);
        //        proc.AddNVarcharPara("@NoOfDays", 128, IncludeDate);
        //     //   proc.AddNVarcharPara("@WorkOrderType", 128, workordertype);
        //      //  proc.AddIntegerPara("@WorkOrderPriority", Convert.ToInt32(workorderpriority));
        //        proc.AddIntegerPara("@Flag", Convert.ToInt32(flag));
        //       // proc.AddIntegerPara("@PersonnelId", Convert.ToInt32(personnelid));
        //        return proc.GetTable(conString);
                
        //    }
        //}
        //public DataTable GetWorkOrderSchedulesSearchCriteria(UserEL objUserEL, string DateFilter, string IncludeDate, string workordertype, string workorderpriority, string flag, string personnelid, string conString)
        //{
        //    using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_GetAvailWorkOrderSchedulesBySearchCriteria"))
        //    {
        //        proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
        //        proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
        //        proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
        //       // proc.AddNVarcharPara("@DateSelection", 128, DateFilter);
        //        proc.AddNVarcharPara("@NoOfDays", 128, IncludeDate);
        //       // proc.AddNVarcharPara("@WorkOrderType", 128, workordertype);
        //        //proc.AddIntegerPara("@WorkOrderPriority", Convert.ToInt32(workorderpriority));
        //        proc.AddIntegerPara("@Flag", Convert.ToInt32(flag));
        //      //  proc.AddIntegerPara("@PersonnelId", Convert.ToInt32(personnelid));
        //        return proc.GetTable(conString);
        //    }
        //}
        // SOM-457
        public DataTable GetWorkOrderSearchCriteria(UserEL objUserEL,string flag,string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_GetWorkOrderBySearchCriteria"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", objUserEL.SiteId);
                proc.AddIntegerPara("@Flag", Convert.ToInt32(flag));
                return proc.GetTable(conString);
            }
        }
        // SOM-457
        public DataTable GetWorkOrderSchedulesSearchCriteria(UserEL objUserEL, string flag, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrder_GetAvailWorkOrderSchedulesBySearchCriteria"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", objUserEL.SiteId);
                proc.AddIntegerPara("@Flag", Convert.ToInt32(flag));
                return proc.GetTable(conString);
            }
        }
        
        public DataTable GetScheduledWorkOrderSearchCriteria(Int64 ClientId, Int64 SiteId, Int64 PersonnelId, string DateFilter, Int32 flag, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_GetScheduledWorkOrderBySearchCriteria"))
            {
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddBigIntegerPara("@PersonnelId", PersonnelId);
                proc.AddNVarcharPara("@DateFilter", 256, DateFilter);
                proc.AddIntegerPara("@flag", flag);
                return proc.GetTable(conString);
            }
        }
        //V2 631
        public DataTable GetScheduledWorkOrderSearchCriteriaV2(UserEL objUserEL, Int64 ClientId, Int64 SiteId, Int64 PersonnelId, string DateFilter, Int32 flag, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_GetScheduledWorkOrderBySearchCriteria_V2"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddBigIntegerPara("@PersonnelId", PersonnelId);
                proc.AddNVarcharPara("@DateFilter", 256, DateFilter);
                proc.AddIntegerPara("@flag", flag);
                return proc.GetTable(conString);
            }
        }
        public DataTable GetScheduledWorkOrderByWorkOrderIDAndDate(Int64 WOID, Int64 ClientId, Int64 SiteId, Int64 PersonnelId, string DateFilter, Int16 flag, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_GetScheduledWorkOrderByWorkOrderIDAndDate"))
            {
                proc.AddBigIntegerPara("@WOID", WOID);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddBigIntegerPara("@PersonnelId", PersonnelId);
                proc.AddNVarcharPara("@DateFilter", 256, DateFilter);
                proc.AddIntegerPara("@flag", flag);
                return proc.GetTable(conString);
            }
        }


        public DataTable GetScheduledWorkOrderTaskSearchCriteria(Int64 WOId, Int64 ClientId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderScheduleTask_GetScheduledWorkOrderTaskBySearchCriteria"))
            {
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@WOId", WOId);
                return proc.GetTable(conString);
            }
        }

        public DataTable GetScheduledWorkOrderByWorkOrderID(Int64 WOId, Int64 ClientId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_GetScheduledWorkOrderByWorkOrderID"))
            {
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@WOId", WOId);
                return proc.GetTable(conString);
            }
        }

        public DataTable GetAvailWorkOrderTaskSearchCriteria(Int64 WOId, Int64 ClientId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderAvailTask_GetAvailWorkOrderTaskBySearchCriteria"))
            {
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@WOId", WOId);
                return proc.GetTable(conString);
            }
        }


        public DataSet WorkOrderScheduleRetrieveByScheduledDate(UserEL objUserEL, int PersonnelId, DateTime ScheduledDate, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_RetrieveByScheduledDate"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@PersonnelId", PersonnelId);
                proc.AddDateTimePara("@ScheduledDate", ScheduledDate);

                DataSet dsWorkOrderSchedule = new DataSet();

                dsWorkOrderSchedule = proc.GetDataSet(conString);
                return dsWorkOrderSchedule;
            }

        }



        public Int32 InsertScheduleRecord(List<object> lstWorkOrderScheduleEL, WorkOrderScheduleEL objWorkOrderScheduleEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = ""; Responcetxt = "";

            foreach (object obj in lstWorkOrderScheduleEL)
            {
                using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_InsertRecord"))
                {

                    proc.AddBigIntegerPara("@ClientId", objWorkOrderScheduleEL.ClientId);
                    
                    proc.AddBigIntegerPara("@WorkOrderId", Convert.ToInt64(obj));
                   

                    proc.AddBigIntegerPara("@PersonnelId", objWorkOrderScheduleEL.PersonnelId);

                    
                    proc.AddDateTimePara("@ScheduledStartDate", objWorkOrderScheduleEL.ScheduledStartDate);
                    
                    proc.AddNVarcharPara("@ModifyBy", 255, objWorkOrderScheduleEL.ModifyBy);
                    proc.AddDateTimePara("@ModifyDate", objWorkOrderScheduleEL.ModifyDate);
                    DataTable dt = new DataTable();
                    dt = proc.GetTable(conString);
                    Responcetxt = strError;
                    foreach (DataRow row in dt.Rows)
                    {
                        Res = Convert.ToInt32(row["SCOPE_IDENTITY"]);
                    }
                }

            }

            return Res;

        }

        public Int32 InsertScheduleRecordWO(string item, WorkOrderScheduleEL objWorkOrderScheduleEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = ""; Responcetxt = "";

            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_InsertRecord"))
            {


                proc.AddBigIntegerPara("@ClientId", objWorkOrderScheduleEL.ClientId);
                proc.AddBigIntegerPara("@WorkOrderId", Convert.ToInt64(item));
                proc.AddBigIntegerPara("@PersonnelId", objWorkOrderScheduleEL.PersonnelId);
                proc.AddDateTimePara("@ScheduledStartDate", objWorkOrderScheduleEL.ScheduledStartDate);
                proc.AddNVarcharPara("@ModifyBy", 255, objWorkOrderScheduleEL.ModifyBy);
                proc.AddDateTimePara("@ModifyDate", objWorkOrderScheduleEL.ModifyDate);

                DataTable dt = new DataTable();
                dt = proc.GetTable(conString);
                Responcetxt = strError;
                foreach (DataRow row in dt.Rows)
                {
                    Res = Convert.ToInt32(row["SCOPE_IDENTITY"]);
                }
            }

            return Res;

        }

        public Int32 UpdateScheduleRecord(WorkOrderScheduleEL objWorkOrderScheduleEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = ""; Responcetxt = "";

            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_UpdateScheduleRecord"))
            {
                // WorkOrderScheduleEL objLstWorkOrderScheduleEL = (WorkOrderScheduleEL)obj;
                proc.AddBigIntegerPara("@WOSchId", objWorkOrderScheduleEL.WorkOrderSchedId);
                proc.AddDecimalPara("@ScheduledHours", 8, 18, objWorkOrderScheduleEL.ScheduledHours);
                proc.AddNVarcharPara("@ModifyBy", 255, objWorkOrderScheduleEL.ModifyBy);
                proc.AddDateTimePara("@ModifyDate", objWorkOrderScheduleEL.ModifyDate);
                DataTable dt = new DataTable();
                dt = proc.GetTable(conString);
                Responcetxt = strError;
                foreach (DataRow row in dt.Rows)
                {
                   // Res = Convert.ToInt32(row["SCOPE_IDENTITY"]);
                }
            }

            return Res;

        }
        #region V2-562 Updating Hours and Schedule start Date
        public Int32 UpdateScheduleRecord_V2(WorkOrderScheduleEL objWorkOrderScheduleEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = ""; Responcetxt = "";
            using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_UpdateScheduleRecord_V2"))
            {
                // WorkOrderScheduleEL objLstWorkOrderScheduleEL = (WorkOrderScheduleEL)obj;

                proc.AddBigIntegerPara("@WOSchId", objWorkOrderScheduleEL.WorkOrderSchedId);
                proc.AddDecimalPara("@ScheduledHours", 8, 18, objWorkOrderScheduleEL.ScheduledHours);
                proc.AddDateTimePara("@ScheduledDate", objWorkOrderScheduleEL.ScheduledStartDate);
                proc.AddNVarcharPara("@ModifyBy", 255, objWorkOrderScheduleEL.ModifyBy);
                proc.AddDateTimePara("@ModifyDate", objWorkOrderScheduleEL.ModifyDate);
                DataTable dt = new DataTable();
                dt = proc.GetTable(conString);
                Responcetxt = strError;
                foreach (DataRow row in dt.Rows)
                {
                    // Res = Convert.ToInt32(row["SCOPE_IDENTITY"]);
                }
            }

            return Res;

        }
        #endregion
        public Int32 RemoveScheduleRecordWO(string item, WorkOrderScheduleEL objWorkOrderScheduleEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = ""; Responcetxt = "";

            
                using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_RemoveRecord"))
                {
                    // WorkOrderScheduleEL objLstWorkOrderScheduleEL = (WorkOrderScheduleEL)obj;

                    proc.AddBigIntegerPara("@WorkOrderSchedId", Convert.ToInt64(item));

                    DataTable dt = new DataTable();
                    dt = proc.GetTable(conString);
                    Responcetxt = strError;
                    foreach (DataRow row in dt.Rows)
                    {
                        Res = Convert.ToInt32(row["ROW_COUNT"]);
                    }
                }

            return Res;

        }

        public Int32 RemoveScheduleRecord(List<object> lstWorkOrderScheduleEL, WorkOrderScheduleEL objWorkOrderScheduleEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = ""; Responcetxt = "";

            foreach (object obj in lstWorkOrderScheduleEL)
            {
                using (ProcedureExecute proc = new ProcedureExecute("usp_WorkOrderSchedule_RemoveRecord"))
                {
                    // WorkOrderScheduleEL objLstWorkOrderScheduleEL = (WorkOrderScheduleEL)obj;

                    proc.AddBigIntegerPara("@WorkOrderSchedId", Convert.ToInt64(obj));

                    DataTable dt = new DataTable();
                    dt = proc.GetTable(conString);
                    Responcetxt = strError;
                    foreach (DataRow row in dt.Rows)
                    {
                        Res = Convert.ToInt32(row["ROW_COUNT"]);
                    }
                }

            }

            return Res;

        }

        //Somax -518
        public DataTable RetrieveLaborInputbyShiftandPersonnel(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Timecard_Date_Shift_Personnel_Report"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, UserName);
                proc.AddBigIntegerPara("@clientId", ClientID);
                proc.AddBigIntegerPara("@SiteId", SiteID);
                proc.AddDateTimePara("@stDate", BeginDate);
                proc.AddDateTimePara("@fnDate", EndDate);
                DataTable dt = proc.GetTable(ConnectionString);
                // SOM-1221
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
        }
    }
}
