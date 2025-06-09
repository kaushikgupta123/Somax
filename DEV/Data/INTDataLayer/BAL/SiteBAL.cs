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
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 2014-Oct-15  SOM-369   Roger Lawton      Added SiteId as a parameter
* 2019-Jul-17  SOM-1714  Roger Lawton      Added method to receive site information
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
    public class SiteBAL
    {

        public bool UpdateURNInSite(SiteEL ObjSiteEL,string conString, string CallerUserName)
        {
            bool Res = false; string strError = string.Empty;
            using (ProcedureExecute proc = new ProcedureExecute("usp_Site_URNUpdateByPK"))
            {
                proc.AddNVarcharPara("@CallerUserName", 255, CallerUserName);
                proc.AddBigIntegerPara("@ClientId", ObjSiteEL.ClientId);
                proc.AddBigIntegerPara("@SiteId", ObjSiteEL.SiteId);
                proc.AddNVarcharPara("@BIMURN", 255, ObjSiteEL.BIMURN);
                proc.AddIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
                int i = proc.RunActionQuery(conString);
                i = Convert.ToInt32(proc.GetParaValue("@UpdateIndexOut"));
                if (i >= 1)
                {
                    Res = true;
                }
                else if (i == -101)
                {
                    Res = false;
                }
                return Res;
            }
        }

        public DataTable GetSettingBySettingName(long ClientID, long SiteID, string SettingName, string ConnectionString)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_SettingDefault_RetrieveBySettingName");
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddNVarcharPara("@SettingName",50, SettingName);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public int UpdateSettingBySettingName(long ClientID, long SiteID, string SettingName, string SettingValue, string ConnectionString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_SettingDefault_UpdateBySettingName"))
            {
                int index = 0;
                int outp = 0;
                proc.AddBigIntegerPara("ClientId", ClientID);
                proc.AddBigIntegerPara("@SiteId", SiteID);
                proc.AddNVarcharPara("@SettingName", 50, SettingName);
                proc.AddNVarcharPara("@SettingValue", 50, SettingValue);

                index = proc.RunActionQuery(ConnectionString);
                return index;
            }
        }
        public DataTable RetrieveSiteforAdmin(long ClientID, long SiteID,string ConnectionString)
        {
          using (ProcedureExecute proc = new ProcedureExecute("[usp_Site_RetrieveByPK]"))
          {
            proc.AddBigIntegerPara("@CallerUserInfoId", 0);
            proc.AddNVarcharPara("@CallerUserName",255, "SOMAX");
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            return proc.GetTable(ConnectionString);
          }
        }

  }
}
