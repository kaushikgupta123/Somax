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
  public  class VendorBAL
    {

      public static DataSet GetVendorRetrieveByClientLookUpId(UserEL objUserEL, string ClientLookupId, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_Vendor_RetrieveByClientLookupId"))
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
     
    }
}
