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
* Date        JIRA Item Person            Description
* =========== ========= ================= ========================================================
* 2014-Nov-23 SOM-453   Roger Lawton      "General" lookup list retriev should exclude the 
*                                         inactive
*                                         Added new method to retrieve lookup lists for the 
*                                         lookuplists page - these should return inactive
**************************************************************************************************
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
using System.Data.SqlClient;

namespace INTDataLayer.BAL
{
    public class LookUpListBAL
    {
        public static DataTable GetLocalisedList(string Culturename, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "lookuplistsName");
                proc.AddNVarcharPara("@culturename", 50, Culturename);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
                return proc.GetTable(conString);
            }
        }
        public static string GetLocalisedDescList(string ListName, string conString)
        {
            string localisedDesc = string.Empty;
            DataTable dt = new DataTable();
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "lookuplistsDescByListName");
                proc.AddNVarcharPara("@ListName", 100, ListName);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
                dt = proc.GetTable(conString);
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        localisedDesc = dt.Rows[0]["Description"].ToString();
                    }
                    catch { ;}
                }
                return localisedDesc;
            }
        }

        // SOM-453
        public static DataTable GetLookUpListList(string LocalisedName, Int64 ClientId, string conString, string Language = null, string Cult = null, string Filter = null)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "LookupListsForGrid");
                proc.AddNVarcharPara("@ListName", 1000, LocalisedName);
                proc.AddNVarcharPara("@Filter", 15, Filter);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddNVarcharPara("@Language", 15, Language);
                proc.AddNVarcharPara("@Culture", 15, Cult);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));

                return proc.GetTable(conString);
            }
        }
        // SOM-1711
        public static DataTable GetLookWOTypeList(string LocalisedName, Int64 ClientId, string conString, string Language = null, string Cult = null, string Filter = null)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "WorkOrderTypeList");
                proc.AddNVarcharPara("@ListName", 1000, LocalisedName);
                proc.AddNVarcharPara("@Filter", 15, Filter);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddNVarcharPara("@Language", 15, Language);
                proc.AddNVarcharPara("@Culture", 15, Cult);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));

                return proc.GetTable(conString);
            }
        }

        // SOM-453
        public static DataTable GetLookUpListListMaint(string LocalisedName, Int64 ClientId, string conString, string Language = null, string Cult = null, string Filter = null)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "LookupListsIsReadOnlyForGridMaint");
                proc.AddNVarcharPara("@ListName", 1000, LocalisedName);
                proc.AddNVarcharPara("@Filter", 15, Filter);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddNVarcharPara("@Language", 15, Language);
                proc.AddNVarcharPara("@Culture", 15, Cult);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));

                return proc.GetTable(conString);
            }
        }

        public static DataTable GetLookUpListbyPKId(Int64 PKId, Int64 ClientId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "lookuplistsByPKIdV2");
                proc.AddBigIntegerPara("@LookupListId", PKId);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
                return proc.GetTable(conString);
            }
        }

        public static DataTable GetCultureList(string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "CultureList");

                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
                return proc.GetTable(conString);
            }
        }

        public static int UpdateLookUpList(UserEL objUserEL, LookupListEL objLookupListEL, string conString)
        {
            int QuestionID = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_LookupList_UpdateByLookupListId");
            proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
            proc.AddNVarcharPara("@CallerUsername", 128, objUserEL.UserFullName);
            proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@LookupListId", objLookupListEL.LookupListId);
            proc.AddBigIntegerPara("@UpdateIndex", objLookupListEL.UpdateIndex);

            proc.AddNVarcharPara("@Description", 63, objLookupListEL.Description);
            proc.AddBooleanPara("@InactiveFlag", objLookupListEL.InactiveFlag);
            proc.AddIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);

            QuestionID = proc.RunActionQuery(conString);
            QuestionID = Convert.ToInt32(proc.GetParaValue("@UpdateIndexOut"));
            return QuestionID;
        }

        public static Int64 CreateLookUpList(UserEL objUserEL, LookupListEL objLookupListEL, string conString)
        {
            Int64 LookupListId = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_LookupList_Insert");
            proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
            proc.AddNVarcharPara("@CallerUsername", 128, objUserEL.UserFullName);
            proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@SiteId", objLookupListEL.SiteID);
            proc.AddBigIntegerPara("@AreaId", objLookupListEL.AreaId);
            proc.AddBigIntegerPara("@DepartmentId", objLookupListEL.DepartmentId);
            proc.AddBigIntegerPara("@StoreroomId", objLookupListEL.StoreRoomId);
            proc.AddNVarcharPara("@ListName", 25, objLookupListEL.ListName);
            proc.AddNVarcharPara("@ListValue", 15, objLookupListEL.ListValue);
            proc.AddNVarcharPara("@Filter", 15, objLookupListEL.Filter);
            proc.AddNVarcharPara("@Description", 63, objLookupListEL.Description);
            proc.AddBooleanPara("@InactiveFlag", objLookupListEL.InactiveFlag);
            proc.AddNVarcharPara("@Language", 256, objLookupListEL.Language);
            proc.AddNVarcharPara("@Culture", 256, objLookupListEL.Culture);
            proc.AddBigIntegerPara("@LookupListId", 0, QyParameterDirection.Output);
            LookupListId = proc.RunActionQuery(conString);
            LookupListId = Convert.ToInt64(proc.GetParaValue("@LookupListId"));
            return LookupListId;
        }



        public static Int64 DeleteLookUpListByPKId(UserEL objUserEL, LookupListEL objLookupListEL, string conString)
        {
            Int64 LookupListId = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_LookupList_DeleteByLookupListId_V2");
            proc.AddBigIntegerPara("@CallerUserInfoId", objUserEL.UserInfoId);
            proc.AddNVarcharPara("@CallerUsername", 128, objUserEL.UserFullName);
            proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@LookupListId", objLookupListEL.LookupListId);
            proc.AddBigIntegerPara("@ResMSG", 0, QyParameterDirection.Output);

            LookupListId = proc.RunActionQuery(conString);
            LookupListId = Convert.ToInt64(proc.GetParaValue("@ResMSG"));
            return LookupListId;
        }

        public static Int64 SharedLookUpListByPKId(UserEL objUserEL, LookupListEL objLookupListEL, string conString)
        {
            Int64 LookupListId = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_Localised_List");

            proc.AddNVarcharPara("@Mode", 50, "ShareLookuplist");
            proc.AddBigIntegerPara("@LocalizedListId", objLookupListEL.LookupListId);

            proc.AddBigIntegerPara("@ClientId", objUserEL.ClientId);
            proc.AddNVarcharPara("@CallerUsername", 128, objUserEL.UserFullName);
            proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
            int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));

            LookupListId = proc.RunActionQuery(conString);
            return LookupListId;
        }

        public static bool CheckduplicateEntry(UserEL objUserEL, LookupListEL objLookupListEL, string conString)
        {
            bool Duplicate = false;
            Int64 LookupListId = 0;
            ProcedureExecute proc = new ProcedureExecute("usp_Localised_List");
            proc.AddNVarcharPara("@Mode", 50, "CheckDuplicateEntry");
            proc.AddNVarcharPara("@ListName", 15, objLookupListEL.ListName);
            proc.AddNVarcharPara("@ListValue", 15, objLookupListEL.ListValue);
            proc.AddBigIntegerPara("@clientId", objUserEL.ClientId);
            proc.AddBigIntegerPara("@SiteId", objLookupListEL.SiteID);
            proc.AddBigIntegerPara("@ResMSG", 0, QyParameterDirection.Output);

            LookupListId = proc.RunActionQuery(conString);
            LookupListId = Convert.ToInt64(proc.GetParaValue("@ResMSG"));
            Duplicate = Convert.ToBoolean(LookupListId);
            return Duplicate;
        }
        //----SOM 496 --07-01-2015----------
        public static DataTable GetLookUpListByName(string LookupListName, Int64 ClientId, Int64 SiteId, string conString, string Language = null, string Cult = null, string Filter = null)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
            {
                proc.AddNVarcharPara("@Mode", 50, "lookuplistsByListName");
                proc.AddNVarcharPara("@ListName", 1000, LookupListName);
                proc.AddBigIntegerPara("@ClientId", ClientId);
                proc.AddBigIntegerPara("@SiteId", SiteId);
                proc.AddNVarcharPara("@Language", 15, Language);
                proc.AddNVarcharPara("@Culture", 15, Cult);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
                return proc.GetTable(conString);
            }
        }

        //public static DataTable GetEquipmentLookUpList(EquipmentEL EqEL, Int64 ClientId, Int64 SiteId, string conString)
        //{
        //    using (ProcedureExecute proc = new ProcedureExecute("usp_Localised_List"))
        //    {
        //        proc.AddNVarcharPara("@Mode", 50, "EquipmentLookUpList");
        //        proc.AddBigIntegerPara("@CallerUserInfoId", EqEL.CallerUserInfoId);
        //        proc.AddNVarcharPara("@CallerUserName", 256, EqEL.CallerUserName);
        //        proc.AddBigIntegerPara("@ClientId", EqEL.ClientId);
        //        proc.AddNVarcharPara("@ClientLookupId", 31, EqEL.ClientLookupId);
        //        proc.AddNVarcharPara("@Name", 63, EqEL.Name);
        //        proc.AddNVarcharPara("@Model", 63, EqEL.Model);
        //        proc.AddBigIntegerPara("@SiteId", EqEL.SiteId);
        //        proc.AddNVarcharPara("@Type", 63, EqEL.Type);
        //        proc.AddNVarcharPara("@SerialNumber", 63, EqEL.SerialNumber);
        //        proc.AddIntegerPara("@Page",EqEL.Page);
        //        proc.AddIntegerPara("@ResultsPerPage",EqEL.ResultsPerPage);
        //        proc.AddNVarcharPara("@OrderColumn", 256, EqEL.OrderColumn);
        //        proc.AddNVarcharPara("@OrderDirection", 256, EqEL.OrderDirection);              
        //        proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
        //        int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
        //        return proc.GetTable(conString);
        //    }
        //}
    
    }
}
