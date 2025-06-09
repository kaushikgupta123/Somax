using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SOMAX.G4.Data.INTDataLayer.DAL;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
    public class MeterBAL
    {
        public DataTable GetQueryResult(string sqlString, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute(sqlString, true))
            {          
                    string conn = conString;
                    return proc.GetTable(conn);                

            }

        }
        public DataTable GetMeterRetrieveByClientLookUpId(long UserInfoId, string UserFullName, long ClientID, long SiteId, string ClientLookupId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_MeterReading_RetrieveByClientLookUpId"))
            {

                proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
                proc.AddNVarcharPara("@CallerUserName", 128, UserFullName);
                proc.AddBigIntegerPara("@ClientId", ClientID);
                proc.AddBigIntegerPara("@Siteid", SiteId);
                proc.AddNVarcharPara("@MeterClientLookupId", 31, ClientLookupId);

                DataTable dsMeter = new DataTable();

                dsMeter = proc.GetTable(conString);
                return dsMeter;
            }

        }
    }
}
