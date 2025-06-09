using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INTDataLayer.DAL;
using  INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;

namespace INTDataLayer.BAL
{
  public static class AlertRepositoryBL
    {
      public static DataTable GeAlertNotification(Int64 ClientId, Int64 SiteID, Int64 CallerUserInfoId, string CallerUserName, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Alert_RepositoryPackageLevel"))  //V2-172
            {
                proc.AddNVarcharPara("@Mode", 50, "AlertNotification");
                proc.AddBigIntegerPara("@clientId", ClientId);
                proc.AddBigIntegerPara("@SiteId", SiteID);
                proc.AddBigIntegerPara("@CallerUserInfoId", CallerUserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 256, CallerUserName);
                proc.AddIntegerPara("@ResMSG", 0, QyParameterDirection.Output);
                int xID = Convert.ToInt32(proc.GetParaValue("@ResMSG"));
                return proc.GetTable(conString);
            }
        }
     public static DataTable ToDataTable<T>(this IList<T> data)
      {
          PropertyDescriptorCollection properties =
              TypeDescriptor.GetProperties(typeof(T));
          DataTable table = new DataTable();
          foreach (PropertyDescriptor prop in properties)
              table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
          foreach (T item in data)
          {
              DataRow row = table.NewRow();
              foreach (PropertyDescriptor prop in properties)
                  row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
              table.Rows.Add(row);
          }
          return table;
      }
    }
}
