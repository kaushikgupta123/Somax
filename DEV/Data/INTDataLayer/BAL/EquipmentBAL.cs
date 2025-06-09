/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014-2015 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2015-Feb-07 SOM-534  Nick Fuchs         Added Down Date parameter
* 2016-Mar-01 SOM-931  Roger Lawton       Do NOT convert downdate
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTDataLayer.DAL;
using INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
//using System.Data.SqlClient;
using Common.Extensions;

namespace INTDataLayer.BAL
{
    public class EquipmentBAL
    {

        public DataTable GetEquipmentDtlsRpt(string strClientLookupId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Equipment_RetrieveEquipByClientLookupIdForDtlsRpt"))
            {
                proc.AddNVarcharPara("@ClientLookupId", 128, strClientLookupId);
                return proc.GetTable(conString);
            }
        }

        public DataTable RetrieveSitesForListRpt(UserEL objUserEL, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_UserPermission_RetrieveSitesForListRpt"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                return proc.GetTable(conString);
            }
        }

        //SOM-515
        public DataTable RetrieveEquipmentUsageForListRpt(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_PartHistory_EquipmentUsage_Report"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, UserName);
                proc.AddBigIntegerPara("@clientId", ClientID);
                proc.AddBigIntegerPara("@SiteId", SiteID);

                DataTable dt = proc.GetTable(ConnectionString);
                return dt;
            }
        }

        //SOM-514, SOM-534
        public DataTable GetOpenEquipmentDownTime(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Downtime_ListReport");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);

            DataTable dt = proc.GetTable(ConnectionString);
            // SOM-931
            /*
            foreach (DataRow tcrow in dt.Rows)
            {
                // sp returns the following date: Downtime.DownDate
                #region Date Conversions
                if (tcrow.Field<DateTime?>("Downtime.DateDown").HasValue)    // checking if date is null
                {
                    DateTime newDateTime = tcrow.Field<DateTime>("Downtime.DateDown").ToUserTimeZone(userTimeZone);
                    tcrow.SetField<DateTime>("Downtime.DateDown", newDateTime);
                }
                #endregion
            }
            */
            return dt;
        }
        //SOM - 516

        //public DataTable GetOpenEquipmentDownTime(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone) {
        //    ProcedureExecute proc = new ProcedureExecute("usp_Downtime_ListReport");

        //    proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
        //    proc.AddNVarcharPara("@CallerUserName", 256, UserName);
        //    proc.AddBigIntegerPara("@clientId", ClientID);
        //    proc.AddBigIntegerPara("@SiteId", SiteID);

        //    DataTable dt = proc.GetTable(ConnectionString);
        //    return dt;
        //}
        public DataTable GetTopTenEquipmentDownTime(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Downtime_Top10Equipment");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);

            DataTable dt = proc.GetTable(ConnectionString);
            //foreach (DataRow pmrow in dt.Rows)
            //{
            //    // sp returns the following dates: [LastPerformed],[NextDueDate]
            //    #region Date Conversions
            //    if (pmrow.Field<DateTime?>("LastPerformed").HasValue)    // checking if date is null
            //    {
            //        DateTime newDateTime = pmrow.Field<DateTime>("LastPerformed").ToUserTimeZone(userTimeZone);
            //        pmrow.SetField<DateTime>("LastPerformed", newDateTime);
            //    }
            //    if (pmrow.Field<DateTime?>("NextDueDate").HasValue)    // checking if date is null
            //    {
            //        DateTime newDateTime = pmrow.Field<DateTime>("NextDueDate").ToUserTimeZone(userTimeZone);
            //        pmrow.SetField<DateTime>("NextDueDate", newDateTime);
            //    }
            //    #endregion
            //}
            return dt;
        }


        public static DataSet GetEquipmentRetrieveByClientLookUpId(UserEL objUserEL, string ClientLookupId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Equipment_RetrieveByClientLookupId"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@Siteid", objUserEL.SiteId);
                proc.AddNVarcharPara("@ClientLookupId", 31, ClientLookupId);

                DataSet dsVendor = new DataSet();

                dsVendor = proc.GetDataSet(conString);
                return dsVendor;
            }

        }

        // Som: 934
        public DataTable GetEquipmentSummery(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone, DateTime BeginDate, DateTime EndDate, bool IncludeAll)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Equipment_Costs_Summary_Report");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);
            proc.AddBooleanPara("@includeAll", IncludeAll);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public DataTable GetEquipmentCostSummeryForDashBoard(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone, DateTime BeginDate, DateTime EndDate, bool IncludeAll)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_EquipmentCosts_Retrieve");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddBooleanPara("@includeAll", IncludeAll);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);


            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public DataTable GetPlantLocationEquipmentCostSummary(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, string userTimeZone, DateTime BeginDate, DateTime EndDate, bool IncludeAll)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_PlantLocation_RetrieveforEquipmentCostsSummary");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddBooleanPara("@includeAll", IncludeAll);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);


            DataTable dt = proc.GetTable(ConnectionString);
            
            foreach (DataRow tcrow in dt.Rows)
            {
                #region Date Conversions
                if (tcrow.Field<DateTime?>("CostDate").HasValue)    // checking if date is null
                {
                    DateTime newDateTime = tcrow.Field<DateTime>("CostDate").ToUserTimeZone(userTimeZone);
                    tcrow.SetField<DateTime>("CostDate", newDateTime);
                }
                #endregion
            }
            return dt;
        }
    #region image and attachment migration
    public DataTable GetClientList(string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetClientList"))
      {
        return proc.GetTable(conString);
      }
    }
    public Int32 GetImageCount(Int64 clientid, string table_name, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetImageCount"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddNVarcharPara("@TableName", 63, table_name);
        DataTable dt = proc.GetTable(conString);
        return dt.Rows[0].Field<Int32>("RecCount");
      }
    }
    public DataTable GetProfileAttachment(Int64 clientid, string table_name, Int64 tableid, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetProfileAttachment"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddNVarcharPara("@TableName", 63, table_name);
        proc.AddBigIntegerPara("@TableId", tableid);
        return proc.GetTable(conString);
      }
    }
    public DataTable GetImageRows(Int64 clientid, string table_name, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetImageRecords"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddNVarcharPara("@TableName", 63, table_name);
        return proc.GetTable(conString);
      }
    }
    public DataTable GetProfileImage(Int64 clientid, string table_name, Int64 tableid, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetProfileImage"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddNVarcharPara("@TableName", 63, table_name);
        proc.AddBigIntegerPara("@TableId", tableid);
        return proc.GetTable(conString);
      }
    }
    public int ClearProfileImage(Int64 clientid, string table_name, Int64 tableid, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_ClearImageData"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddNVarcharPara("@TableName", 63, table_name);
        proc.AddBigIntegerPara("@TableId", tableid);
        return proc.GetTable(conString).Rows[0].Field<int>("RecCount");
      }
    }
    public DataTable GetFileInfoForClient(Int64 clientid, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetFileInfoForClient"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        return proc.GetTable(conString);
      }
    }
    public DataTable GetFileInfoForMigrate(Int64 clientid, Int64 fileinfoid,  string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_GetFileInfoForMigrate"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddBigIntegerPara("@FileInfoId", fileinfoid);
        return proc.GetTable(conString);
      }
    }
    public int UpdateAttachmentURL(Int64 clientid, Int64 fileinfoid, string url, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_UpdateAttachmentURL"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddBigIntegerPara("@FileInfoId", fileinfoid);
        proc.AddNVarcharPara("@URL",511, url);
        return proc.GetTable(conString).Rows[0].Field<int>("RecCount");
      }
    }
    public int UpdateAttachmentProperties(Int64 clientid, Int64 AttachId, string content_type, int file_size, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_UpdateAttachmentProps"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddBigIntegerPara("@AttachmentId", AttachId);
        proc.AddNVarcharPara("@ContentType", 127, content_type);
        proc.AddIntegerPara("@FileSize", file_size);
        return proc.GetTable(conString).Rows[0].Field<int>("RecCount");
      }
    }
    public int CreateAttachmentRec(Int64 clientid, string table_name, Int64 object_id, string url, Int64 uploadby,  
                                   string desc, string file_name, string file_type, bool image, bool profile, bool external, bool refer, 
                                   string content_type, int file_size, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Attachment_Create"))
      {
        proc.AddBigIntegerPara("@CallerUserInfoId", 0L);
        proc.AddNVarcharPara("@CallerUserName", 256, "SOMAX_MIGRATE");
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddBigIntegerPara("@AttachmentId", 0,QyParameterDirection.Output);
        proc.AddNVarcharPara("@ObjectName", 31, table_name);
        proc.AddBigIntegerPara("@ObjectId", object_id);
        proc.AddNVarcharPara("@AttachmentURL", 511, url);
        proc.AddBigIntegerPara("@UploadedBy_PersonnelId", uploadby);
        proc.AddNVarcharPara("@Description", 254, desc);
        proc.AddNVarcharPara("@FileName", 511, file_name);
        proc.AddNVarcharPara("@FileType", 15, file_type);
        proc.AddBooleanPara("@Image", image);
        proc.AddBooleanPara("@Profile", profile);
        proc.AddBooleanPara("@External", external);
        proc.AddBooleanPara("@Reference", refer);
        proc.AddNVarcharPara("@ContentType", 127, content_type);
        proc.AddIntegerPara("@FileSize", file_size);
        proc.AddBigIntegerPara("@FileAttachmentId", 0);
        proc.AddBigIntegerPara("@FileInfoId", 0);
        return proc.RunActionQuery(conString);
      }
    }
    public int CreateErrorLogEntry(Int64 clientid, string extype, string msg, string conString)
    {
      using (ProcedureExecute proc = new ProcedureExecute("usp_Images_CreateErrorLog"))
      {
        proc.AddBigIntegerPara("@ClientId", clientid);
        proc.AddNVarcharPara("@ExceptionType", 15, extype);
        proc.AddNVarcharPara("@ErrorMessage", 1023, msg);
        return proc.GetTable(conString).Rows[0].Field<int>("RecCount");
      }
    }

    #endregion image and attachment migration
  }
}
