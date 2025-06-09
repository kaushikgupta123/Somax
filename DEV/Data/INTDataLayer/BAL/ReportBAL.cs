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
using System.Data.SqlClient;

/*
******************************************************************************
* PROPRIETARY DATA 
******************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
******************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
******************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ===================================
* 2014-Nov-21 SOM-448  Nick Fuchs          Changed select to nvarchar(max)
* 2019-Jul-17 SOM-1714 Roger Lawton        Change to use language not culture
******************************************************************************
*/
namespace INTDataLayer.BAL
{

    public class ReportBAL
    {

        public DataTable RetrieveListRpt(bool boolIsSomaxOwened, UserEL objUserEL, string strSelect, string strDateRange, string strFromDate, string strToDate, string strSite, string strSortCat, string strSortOrder, string strDateCol, char charRptType, string conString)
        {
            try
            {
                using (ProcedureExecute proc = new ProcedureExecute("usp_SOMAXG4_RetrieveListRpt"))
                {

                    proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                    proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                    proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                    proc.AddNVarcharPara("@strDateRange", 64, strDateRange);
                    proc.AddNVarcharPara("@strSartDt", 128, strFromDate);
                    proc.AddNVarcharPara("@strEndDt", 128, strToDate);
                    proc.AddNVarcharPara("@strSite", 128, strSite);
                    proc.AddNVarcharPara("@strSortCat", 128, strSortCat);
                    proc.AddNVarcharPara("@strSortOrder", 128, strSortOrder);
                    //proc.AddNVarcharPara("@strSelect", 256, strSelect);         // SOM-448
                    proc.AddNVarcharPara("@strSelect", -1, strSelect);            // SOM-448
                    proc.AddNVarcharPara("@strDateCol", 50, strDateCol);
                    proc.AddCharPara("@charRptType", 50, charRptType);
                    proc.AddBooleanPara("@btSomaxOwned", boolIsSomaxOwened);
                    return proc.GetTable(conString);
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
        }
        public DataTable GetAllDateRange(DataTable dt, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_SOMAXG4_GetDateRange"))
            {
                proc.AddTVP("@AllDateRange", dt);
                return proc.GetTable(conString);
            }
        }


        public DataTable RetrieveDefaultListRpt(Int64 strRuntimeId, UserEL objUserEL, string strSelect, string strDateRange, string FromDate, string ToDate, string strSite, string strSortCat, string strSortOrder, string strDateCol, char charRptType, string conString)
        {

            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_RetrieveDefaultListRpt"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddBigIntegerPara("@RuntimeId", strRuntimeId);
                proc.AddNVarcharPara("@strSite", 128, strSite);
                proc.AddNVarcharPara("@StDate", 128, FromDate);
                proc.AddNVarcharPara("@EdDate", 128, ToDate);
                //proc.AddDateTimePara("@StDate", FromDate);
                //proc.AddDateTimePara("@EdDate", ToDate);

                return proc.GetTable(conString);
            }
        }


        public DataTable RetrieveViewColsDtls(EquipmentEL objEquipmentEL, UserEL objUserEL, string strSite, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_RetrieveViewColsDtls"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, objUserEL.UserFullName);
                proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
                proc.AddNVarcharPara("@strSite", 128, strSite);
                proc.AddBigIntegerPara("@ReportId", objEquipmentEL.ReportId);
                return proc.GetTable(conString);
            }
        }




        public DataTable RtrieveBasicReportSetup(string conString, Int64 longReportId)
        {

            //using (ProcedureExecute proc = new ProcedureExecute("usp_MemorizedSearch_RetrieveMemorizedSearchedReports"))
            using (ProcedureExecute proc = new ProcedureExecute("usp_BasicReportSetup_RtrieveBasicReportSetup"))
            {
                proc.AddBigIntegerPara("@ReportId", longReportId);
                return proc.GetTable(conString);
            }
        }


        public DataTable RetreiveReports(Int64 longClientId, Int64 longUserInfoId, string strLanguage, char charRptCat, string conString)
        {
            try
            {

                using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_RetrieveReports"))
                {

                    proc.AddBigIntegerPara("@ClientId", longClientId);
                    proc.AddBigIntegerPara("@UserInfoId", longUserInfoId);
                    proc.AddNVarcharPara("@Language", 50, strLanguage);         // SOM-1714
                    //proc.AddNVarcharPara("@Culture", 50, strCulture);         // SOM-1714
                    proc.AddCharPara("@ReportCategory", 1, charRptCat);
                    return proc.GetTable(conString);
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }

        }
        public DataTable RetreiveReportsByRuntimeId(string strRuntimeId, string conString)
        {
            try
            {

                using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_RetrieveReportById"))
                {

                    proc.AddNVarcharPara("@RuntimeId", 50, strRuntimeId);
                    return proc.GetTable(conString);

                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }

        }


        public DataTable RetrieveLocCols(Int64 longClientId, Int64 longUserInfoId, string strLanguage, string strCat, string conString, long strRuntimeId)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_RetrieveLocCols"))
            {

                proc.AddBigIntegerPara("@ClientId", longClientId);
                proc.AddBigIntegerPara("@UserInfoId", longUserInfoId);
                proc.AddNVarcharPara("@Language", 50, strLanguage);       // SOM-1714
                //proc.AddNVarcharPara("@Culture", 50, strCulture);       // SOM-1714
                proc.AddNVarcharPara("@Category", 50, strCat);
                proc.AddBigIntegerPara("@RuntimeId", strRuntimeId);
                return proc.GetTable(conString);
            }
        }
        public string RetriveGroupNamebyId(ReportEL objReportEL, string conString)
        {
            string ReportGroup = "";
            using (ProcedureExecute proc = new ProcedureExecute("usp_BasicReportSetup_RetriveGroupNamebyId"))
            {

                proc.AddBigIntegerPara("@ReportId", objReportEL.ReportId);
                DataTable dt = new DataTable();
                dt = proc.GetTable(conString);
                ReportGroup = dt.Rows[0]["ReportGroup"].ToString();
                return ReportGroup;
            }
        }
        public DataTable GetRuntimeReportGroup(string conString)
        {

            //using (ProcedureExecute proc = new ProcedureExecute("usp_MemorizedSearch_RetrieveMemorizedSearchedReports"))
            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_GetRuntimeReportGroup"))
            {

                return proc.GetTable(conString);
            }
        }

        public Int32 InsertRuntimeRecord(ReportEL ObjReportEL, out string Responcetxt, string conString)
        {
            Int32 Res = 1; string strError = string.Empty; string strId = "";
            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_InsertRecord"))
            {

                proc.AddBigIntegerPara("@ReportId", ObjReportEL.ReportId);
                proc.AddBigIntegerPara("@ClientId", ObjReportEL.ClientId);
                proc.AddBigIntegerPara("@UserInfoId", ObjReportEL.UserInfoId);
                proc.AddNVarcharPara("@Language", 50, ObjReportEL.Language);      // SOM-1714
                proc.AddNVarcharPara("@Culture", 50, ObjReportEL.Culture);
                proc.AddNVarcharPara("@ReportName", 250, ObjReportEL.ReportName);
                proc.AddNVarcharPara("@ReportTitle", 250, ObjReportEL.ReportTitle);
                proc.AddNVarcharPara("@Description", 250, ObjReportEL.Description);
                proc.AddNVarcharPara("@SortCols", 250, ObjReportEL.SortCols);
                // This must be larger than 10 characters
                proc.AddNVarcharPara("@SortOrder", 31, ObjReportEL.SortOrder);
                //proc.AddNVarcharPara("@SortOrder", 10, ObjReportEL.SortOrder);
                proc.AddNVarcharPara("@ViewCols", 250, ObjReportEL.ViewCols);
                proc.AddNVarcharPara("@TotCols", 250, ObjReportEL.TotCols);
                proc.AddNVarcharPara("@GroupCols", 128, ObjReportEL.GroupCols);
                proc.AddNVarcharPara("@Sites", 200, ObjReportEL.Sites);
                proc.AddNVarcharPara("@QryOperator", 250, "");
                proc.AddNVarcharPara("@QryVal", -1, ObjReportEL.Query);
                proc.AddNVarcharPara("@HeaderText", 250, ObjReportEL.HeaderText);
                proc.AddBooleanPara("@IncludeHeader", ObjReportEL.ShowReportTitle);
                proc.AddBooleanPara("@IncludeCompanyName", ObjReportEL.ShowCompanyName);
                proc.AddNVarcharPara("@FooterText", 256, ObjReportEL.FooterText);
                proc.AddBooleanPara("@SomaxOwned", ObjReportEL.SomaxOwned);
                proc.AddDateTimePara("@CreateDate", DateTime.Now);
                proc.AddNVarcharPara("@CreateBy", 256, ObjReportEL.CreateBy);
                proc.AddDateTimePara("@ModifyDate", DateTime.Now);
                proc.AddNVarcharPara("@ModifyBy", 128, ObjReportEL.ModifyBy);
                proc.AddBigIntegerPara("@AccessNo", ObjReportEL.AccessNo);
                proc.AddBooleanPara("@ShowDateRange", ObjReportEL.ShowDateRange);
                proc.AddBooleanPara("@ShowSites", ObjReportEL.ShowSites);
                proc.AddBooleanPara("@ShowReportName", ObjReportEL.ShowReportName);
                proc.AddBooleanPara("@ShowRunDate", ObjReportEL.ShowRunDate);
                proc.AddBooleanPara("@ShowRunTime", ObjReportEL.ShowRunTime);
                proc.AddDateTimePara("@StartDate", ObjReportEL.Startdate);
                proc.AddDateTimePara("@EndDate", ObjReportEL.EndDate);
                proc.AddNVarcharPara("@ReportGroup", 30, ObjReportEL.ReportGroup);
                proc.AddNVarcharPara("@DateRange", 30, ObjReportEL.DateRange);
                proc.AddIntegerPara("@UpdateIndex", 0);
                proc.AddBooleanPara("@IsGrouped", ObjReportEL.IsGrouped);
                proc.AddBooleanPara("@Public", ObjReportEL.Public);

                //int i = proc.RunActionQuery(conString); //-----SOM-886
                DataTable dt = new DataTable();
                dt = proc.GetTable(conString);
                Responcetxt = strError;
                foreach (DataRow row in dt.Rows)
                {
                    Res = Convert.ToInt32(row["SCOPE_IDENTITY"]);
                }
                return Res;
            }
        }

        public bool DeleteRecort(ReportEL objReportEL, out string Responcetxt, string conString)
        {
            bool Res = false; string strError = string.Empty; string strId = "";
            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_DeleteRecord"))
            {
                proc.AddBigIntegerPara("@RuntimeId", objReportEL.RuntimeId);
                proc.AddBigIntegerPara("@ClientId", objReportEL.ClientId);
                proc.AddBigIntegerPara("@UserInfoId", objReportEL.UserInfoId);
                proc.AddDateTimePara("@ModifyDate", DateTime.Now);
                proc.AddNVarcharPara("@ModifyBy", 256, objReportEL.ModifyBy);
                int i = proc.RunActionQuery(conString);
                // return I;
                if (i >= 1)
                {
                    Res = true;

                    strError = "Custom Report Inserted successfully";
                }
                else if (i == -101)
                {
                    strError = "update Failure";
                }
                else
                    strError = Convert.ToString(proc.GetParaValue("@ResMSG"));

                Responcetxt = strError;
                return Res;
            }
        }
        public bool UpdateRuntimeRecord(ReportEL ObjReportEL, out string Responcetxt, string conString)
        {
            bool Res = false; string strError = string.Empty; string strId = "";
            using (ProcedureExecute proc = new ProcedureExecute("usp_RuntimeReportSetup_UpdateRecord"))
            {

                proc.AddBigIntegerPara("@RuntimeId", ObjReportEL.RuntimeId);
                proc.AddBigIntegerPara("@ReportId", ObjReportEL.ReportId);
                proc.AddBigIntegerPara("@ClientId", ObjReportEL.ClientId);
                proc.AddBigIntegerPara("@UserInfoId", ObjReportEL.UserInfoId);
                proc.AddNVarcharPara("@Language", 50, ObjReportEL.Language);          // SOM-1714
                proc.AddNVarcharPara("@Culture", 50, ObjReportEL.Culture);
                proc.AddNVarcharPara("@ReportName", 250, ObjReportEL.ReportName);
                proc.AddNVarcharPara("@ReportTitle", 250, ObjReportEL.ReportTitle);
                proc.AddNVarcharPara("@Description", 250, ObjReportEL.Description);
                proc.AddNVarcharPara("@SortCols", 250, ObjReportEL.SortCols);
                proc.AddNVarcharPara("@SortOrder", 31, ObjReportEL.SortOrder);
                //proc.AddNVarcharPara("@SortOrder", 10, ObjReportEL.SortOrder);
                proc.AddNVarcharPara("@ViewCols", 250, ObjReportEL.ViewCols);
                proc.AddNVarcharPara("@TotCols", 250, ObjReportEL.TotCols);
                proc.AddNVarcharPara("@GroupCols", 128, ObjReportEL.GroupCols);
                proc.AddNVarcharPara("@Sites", 200, ObjReportEL.Sites);
                proc.AddNVarcharPara("@QryOperator", 250, "");
                //SOM-1360 - Changed to max (-1)
                proc.AddNVarcharPara("@QryVal", -1, ObjReportEL.Query);
                proc.AddNVarcharPara("@HeaderText", 250, ObjReportEL.HeaderText);
                proc.AddBooleanPara("@IncludeHeader", ObjReportEL.IncludeHeader);
                proc.AddBooleanPara("@IncludeCompanyName", ObjReportEL.ShowCompanyName);
                proc.AddNVarcharPara("@FooterText", 256, ObjReportEL.FooterText);
                proc.AddBooleanPara("@SomaxOwned", ObjReportEL.SomaxOwned);
                //proc.AddDateTimePara("@CreateDate", ObjReportEL.CreateDate);
                proc.AddNVarcharPara("@CreateBy", 256, ObjReportEL.CreateBy);
                proc.AddDateTimePara("@ModifyDate", DateTime.Now);
                proc.AddNVarcharPara("@ModifyBy", 256, ObjReportEL.ModifyBy);
                proc.AddBigIntegerPara("@AccessNo", ObjReportEL.AccessNo);
                proc.AddBooleanPara("@ShowDateRange", ObjReportEL.ShowDateRange);
                proc.AddBooleanPara("@ShowSites", ObjReportEL.ShowSites);
                proc.AddBooleanPara("@ShowReportName", ObjReportEL.ShowReportName);
                proc.AddBooleanPara("@ShowRunDate", ObjReportEL.ShowRunDate);
                proc.AddBooleanPara("@ShowRunTime", ObjReportEL.ShowRunTime);
                proc.AddDateTimePara("@StartDate", ObjReportEL.Startdate);
                proc.AddDateTimePara("@EndDate", ObjReportEL.EndDate);
                proc.AddBigIntegerPara("@UpdateIndex", ObjReportEL.UpdateIndex);
                proc.AddBooleanPara("@Public", ObjReportEL.Public);

                proc.AddIntegerPara("@UpdateIndexOut", Convert.ToInt32(ObjReportEL.UpdateIndex), QyParameterDirection.Output);

                int i = proc.RunActionQuery(conString);
                // return I;
                if (i >= 1)
                {
                    Res = true;

                    strError = "Custom Report Inserted successfully";
                }
                else if (i == -101)
                {
                    strError = "update Failure";
                }
                else
                    strError = Convert.ToString(proc.GetParaValue("@ResMSG"));

                Responcetxt = strError;
                return Res;
            }
        }

        public DataTable RetrieveShipment(Int64 ClientId, Int64 PartTransferEventLogId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_PartTransfer_Shipment"))
            {
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@CallerUserInfoId", 0);
                proc.AddNVarcharPara("@CallerUserName", 67, "");
                proc.AddBigIntegerPara("@PartTransferEventLogId", PartTransferEventLogId);
                return proc.GetTable(conString);
            }
        }


        #region Reports_V2-353
        public DataTable GetDataFromSource(long ReportListingId, bool UseSp, long CallerUserInfoId, string CallerUserName, Int64 ClientId, Int64 SiteId, string conString, string SourceName,
           bool includePromp, string Prompt1Source, string Prompt2Source, string Prompt1Type, string Prompt2Type, string MultiSelectData1, string MultiSelectData2,
           int CaseNo1, int CaseNo2, DateTime? stDate1, DateTime? fnDate1, DateTime? stDate2, DateTime? fnDate2, bool IncludeChild, string ChildSourceName, string MasterLinkColumn, string ChildLinkColumn,
           bool IsEnterprise, bool IsUserReport, string BaseQuery)
        {

            using (ProcedureExecute proc = new ProcedureExecute("usp_ReportListing_GetDataFromSource"))
            {
                proc.AddBooleanPara("@UseSp", UseSp);
                proc.AddBigIntegerPara("@CallerUserInfoId", CallerUserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, CallerUserName); // V2-1116
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddBigIntegerPara("@ReportListingId", ReportListingId);
                proc.AddNVarcharPara("@SourceName", 250, SourceName);    // V2-1116
                proc.AddBooleanPara("@includePrompt", includePromp);
                proc.AddNVarcharPara("@Prompt1Source", 50, Prompt1Source);
                proc.AddNVarcharPara("@Prompt2Source", 50, Prompt2Source);
                proc.AddNVarcharPara("@Prompt1Type", 50, Prompt1Type);
                proc.AddNVarcharPara("@Prompt2Type", 50, Prompt2Type);
                proc.AddIntegerPara("@CaseNo1", CaseNo1);
                proc.AddIntegerPara("@CaseNo2", CaseNo2);
                proc.AddNVarcharPara("@MultiSelectData1", 100, MultiSelectData1);
                proc.AddNVarcharPara("@MultiSelectData2", 100, MultiSelectData2);
                proc.AddDateTimePara("@stDate1", stDate1);
                proc.AddDateTimePara("@fnDate1", fnDate1);
                proc.AddDateTimePara("@stDate2", stDate2);
                proc.AddDateTimePara("@fnDate2", fnDate2);
                proc.AddBooleanPara("@IncludeChild", IncludeChild);
                proc.AddNVarcharPara("@ChildSourceName", 250, ChildSourceName); // V2-1116
                proc.AddNVarcharPara("@MasterLinkColumn", 50, MasterLinkColumn);
                proc.AddNVarcharPara("@ChildLinkColumn", 50, ChildLinkColumn);
                proc.AddBooleanPara("@IsEnterprise", IsEnterprise);
                proc.AddBooleanPara("@IsUserReport", IsUserReport);
                proc.AddNVarcharPara("@BaseQuery", 150, BaseQuery);
                return proc.GetTable(conString);
            }

        }
        #region RKL-Mail Report Timeout
        public DataTable GetDataFromSourceReport(long ReportListingId, bool UseSp, long CallerUserInfoId, string CallerUserName, Int64 ClientId, Int64 SiteId, string conString, string SourceName,
            bool includePromp, string Prompt1Source, string Prompt2Source, string Prompt1Type, string Prompt2Type, string MultiSelectData1, string MultiSelectData2,
            int CaseNo1, int CaseNo2, DateTime? stDate1, DateTime? fnDate1, DateTime? stDate2, DateTime? fnDate2, bool IncludeChild, string ChildSourceName, string MasterLinkColumn, string ChildLinkColumn,
            bool IsEnterprise, bool IsUserReport, string BaseQuery, ref string timeoutError)
        {


            try
            {
                using (ProcedureExecute proc = new ProcedureExecute("usp_ReportListing_GetDataFromSource"))
                {
                    proc.AddBooleanPara("@UseSp", UseSp);
                    proc.AddBigIntegerPara("@CallerUserInfoId", CallerUserInfoId);
                    proc.AddNVarcharPara("@CallerUserName", 256, CallerUserName); // V2-1116
                    proc.AddBigIntegerPara("@ClientId", ClientId);
                    proc.AddBigIntegerPara("@SiteId", SiteId);
                    proc.AddBigIntegerPara("@ReportListingId", ReportListingId);
                    proc.AddNVarcharPara("@SourceName", 250, SourceName);         // V2-1116
                    proc.AddBooleanPara("@includePrompt", includePromp);
                    proc.AddNVarcharPara("@Prompt1Source", 50, Prompt1Source);
                    proc.AddNVarcharPara("@Prompt2Source", 50, Prompt2Source);
                    proc.AddNVarcharPara("@Prompt1Type", 50, Prompt1Type);
                    proc.AddNVarcharPara("@Prompt2Type", 50, Prompt2Type);
                    proc.AddIntegerPara("@CaseNo1", CaseNo1);
                    proc.AddIntegerPara("@CaseNo2", CaseNo2);
                    proc.AddNVarcharPara("@MultiSelectData1", 100, MultiSelectData1);
                    proc.AddNVarcharPara("@MultiSelectData2", 100, MultiSelectData2);
                    proc.AddDateTimePara("@stDate1", stDate1);
                    proc.AddDateTimePara("@fnDate1", fnDate1);
                    proc.AddDateTimePara("@stDate2", stDate2);
                    proc.AddDateTimePara("@fnDate2", fnDate2);
                    proc.AddBooleanPara("@IncludeChild", IncludeChild);
                    proc.AddNVarcharPara("@ChildSourceName", 250, ChildSourceName); // V2-1116
                    proc.AddNVarcharPara("@MasterLinkColumn", 50, MasterLinkColumn);
                    proc.AddNVarcharPara("@ChildLinkColumn", 50, ChildLinkColumn);
                    proc.AddBooleanPara("@IsEnterprise", IsEnterprise);
                    proc.AddBooleanPara("@IsUserReport", IsUserReport);
                    proc.AddNVarcharPara("@BaseQuery", 150, BaseQuery);
                    return proc.GetTableReport(conString, ref timeoutError);


                }
            }
            catch (OperationCanceledException)
            {
                timeoutError = "Timeout";
            }
            catch (InvalidOperationException)
            {
                timeoutError = "Timeout";
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2) // -2 represents TimeoutExpired error
                {
                    // Handle SQL timeout exception
                    timeoutError = "Timeout";
                }
                else
                {
                    // Handle other SQL exceptions
                    timeoutError = "SQL Exception: {ex.Message}";
                }
            }
            catch (Exception ex)
            {
                timeoutError = "An error occurred: {ex.Message}";
            }

            return (new DataTable());

        }
        #endregion
        #endregion

        #region Reports_V2-407
        public DataTable GetChildGridData(long CallerUserInfoId, string CallerUserName, long ClientId, long SiteId, string conString, string ChildSourceName, long MasterLinkColumnValue,
           string ChildLinkColumn)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_ReportListing_GetChildGridData"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", CallerUserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, CallerUserName);   // V2-1116
                proc.AddBigIntegerPara("@ClientId", ClientId);
                //proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddNVarcharPara("@ChildSourceName", 250, ChildSourceName); // V2-1116
                proc.AddBigIntegerPara("@MasterLinkColumnValue", MasterLinkColumnValue);
                proc.AddNVarcharPara("@ChildLinkColumn", 50, ChildLinkColumn);
                return proc.GetTable(conString);
            }
        }
        #endregion
    }
    public class ReportParamsEL
    {
        public bool UseSp { get; set; }
        public long CallerUserInfoId { get; set; }
        public string CallerUserName { get; set; }
        public long ClientId { get; set; }
        public long SiteId { get; set; }
        public string ConString { get; set; }
        public string SourceName { get; set; }
        public bool includePromp { get; set; }
        public string Prompt1Source { get; set; }
        public string Prompt2Source { get; set; }
        public string Prompt1Type { get; set; }
        public string Prompt2Type { get; set; }
        public string MultiSelectData1 { get; set; }
        public string MultiSelectData2 { get; set; }
        public int CaseNo1 { get; set; }
        public int CaseNo2 { get; set; }
        public DateTime? stDate1 { get; set; }
        public DateTime? fnDate1 { get; set; }
        public DateTime? stDate2 { get; set; }
        public DateTime? fnDate2 { get; set; }
        public bool IncludeChild { get; set; }
        public string ChildSourceName { get; set; }
        public string MasterLinkColumn { get; set; }
    }
}
