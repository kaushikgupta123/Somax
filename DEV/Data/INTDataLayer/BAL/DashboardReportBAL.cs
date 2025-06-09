using INTDataLayer.DAL;
using System.Data;

namespace INTDataLayer.BAL
{
    public class DashboardReportBAL
    {
        public DataTable GetInventoryValuationData(long clientId, long siteId, int flag, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_Metrics_GetMetricsValueFilterByMetricsName_V2"))
            {
                proc.AddBigIntegerPara("@ClientId", clientId);
                proc.AddBigIntegerPara("@SiteId", siteId);
                proc.AddIntegerPara("@flag", flag);
                return proc.GetTable(conString);
            }
        }

    }
}
