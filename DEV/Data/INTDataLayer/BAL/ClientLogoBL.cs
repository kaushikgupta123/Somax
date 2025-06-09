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
  public  class ClientLogoBL
    {
      public static Int64 InsertLogo(ClientLogoEL cl, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_ClientLogo_Create"))
          {
              Int64 LogoId = 0;
              int outp = 0;
              proc.AddBigIntegerPara("@ClientId", cl.ClientId);
              proc.AddBigIntegerPara("@SiteId", cl.SiteId);
              proc.AddNVarcharPara("@CallerUserName", 256, cl.CallerUserName);
              proc.AddNVarcharPara("@Type", 15, cl.Type);
              proc.AddVarbinary("@Image", cl.Image.ToString());
              proc.AddBigIntegerPara("@LogoId", 0, QyParameterDirection.Output);

              LogoId = proc.RunActionQuery(conString);
              LogoId = Convert.ToInt64(proc.GetParaValue("@LogoId"));
              return LogoId;
          }
      }
      public static Int64 UpdateLogo(ClientLogoEL cl, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_ClientLogo_UpdateByPK"))
          {
              Int64 UpdateIndex = 0;
              int outp = 0;
              proc.AddBigIntegerPara("@ClientId", cl.ClientId);
              proc.AddBigIntegerPara("@LogoId", cl.LogoId);
              proc.AddNVarcharPara("@CallerUserName", 256, cl.CallerUserName);
              proc.AddBigIntegerPara("@CallerUserInfoId", cl.CallerUserInfoId);
              proc.AddVarbinaryData("@Image", (byte[])cl.Image);
              proc.AddBigIntegerPara("@UpdateIndexOut", 0, QyParameterDirection.Output);
              UpdateIndex = proc.RunActionQuery(conString);
              UpdateIndex = Convert.ToInt64(proc.GetParaValue("@UpdateIndexOut"));
              return UpdateIndex;
          }
      }
      public static Int64 InsertUpdateLogo(ClientLogoEL cl, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_ClientLogo_InsertUpdate"))
          {
              Int64 LogoId = 0;
              int outp = 0;
              proc.AddBigIntegerPara("@ClientId", cl.ClientId);
              proc.AddBigIntegerPara("@SiteId", cl.SiteId);
              proc.AddNVarcharPara("@CallerUserName", 256, cl.CallerUserName);
              proc.AddNVarcharPara("@Type", 15, cl.Type);
              proc.AddVarbinaryData("@Image", (byte[])cl.Image);
              proc.AddBigIntegerPara("@LogoId", 0, QyParameterDirection.Output);

              LogoId = proc.RunActionQuery(conString);
              LogoId = Convert.ToInt64(proc.GetParaValue("@LogoId"));
              return LogoId;
          }
      }
      public static Int64 DeleteLogo(ClientLogoEL cl, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_ClientLogo_DeleteByPK"))
          {
              Int64 UpdateIndex = 0;
              int outp = 0;
              proc.AddBigIntegerPara("@ClientId", cl.ClientId);
              proc.AddNVarcharPara("@CallerUserName", 256, cl.CallerUserName);
              proc.AddBigIntegerPara("@LogoId", cl.LogoId);
              UpdateIndex = proc.RunActionQuery(conString);
             
              return UpdateIndex;
          }
      }
      public static DataTable GetLogoByType(ClientLogoEL cl, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_ClientLogo_RetrieveByType"))
          {
              proc.AddBigIntegerPara("@ClientId", cl.ClientId);
              proc.AddBigIntegerPara("@SiteId", cl.SiteId);
              proc.AddNVarcharPara("@CallerUserName",256, cl.CallerUserName);
              proc.AddNVarcharPara("@Type", 15, cl.Type);
              DataTable dslogo = new DataTable();
              dslogo = proc.GetTable(conString);
              return dslogo;
          }
      }
      public static DataTable GetLogoByClientSite(ClientLogoEL cl, string conString)
      {
          using (ProcedureExecute proc = new ProcedureExecute("usp_ClientLogo_RetrieveByClientSite"))
          {
              proc.AddBigIntegerPara("@ClientId", cl.ClientId);
              proc.AddBigIntegerPara("@SiteId", cl.SiteId);
              proc.AddNVarcharPara("@CallerUserName", 256, cl.CallerUserName);
              DataTable dslogo = new DataTable();
              dslogo = proc.GetTable(conString);
              return dslogo;
          }
      }
    }
}
