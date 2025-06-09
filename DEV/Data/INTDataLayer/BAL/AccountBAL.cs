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
* 2014-Nov-19  SOM-382   Nick Fuchs        Created
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SOMAX.G4.Data.INTDataLayer.DAL;
using SOMAX.G4.Data.INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
  public class AccountBAL
  {

    public DataTable GetAccountListReport(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString)
    {
      ProcedureExecute proc = new ProcedureExecute("usp_Account_ListReport");

      proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
      proc.AddNVarcharPara("@CallerUserName", 256, UserName);
      proc.AddBigIntegerPara("@clientId", ClientID);
      proc.AddBigIntegerPara("@SiteId", SiteID);

      DataTable dt = proc.GetTable(ConnectionString);
      return dt;
    }
  }
}
