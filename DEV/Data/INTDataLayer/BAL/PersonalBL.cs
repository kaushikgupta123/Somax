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
  public  class PersonalBL
    {

      public static DataTable GetIssueToList( Int64 UserInfoId ,Int64 ClientId,Int64 SiteId,string conString, string OrderDirection="asc")
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_Personnel_Issueto"))
          {

              proc.AddBigIntegerPara("ClientId", ClientId);
              proc.AddBigIntegerPara("@SiteId", SiteId);
              proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
              //  proc.AddNVarcharPara("@strSelect", 256, strSelect);

              DataTable dsStore = new DataTable();

              dsStore = proc.GetTable(conString);
              return dsStore;
          }
      }
    }
}
