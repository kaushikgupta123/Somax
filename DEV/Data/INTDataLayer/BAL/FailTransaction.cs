using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

//using SOMAX.G4.Data.Common;
using SOMAX.G4.Data.INTDataLayer.DAL;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
    public class FailTransaction
    {
        public DataTable GetFailTransactionDetails(string conString, Int64 userid, Int64 clientid, Int64 siteid, string username, int val)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_FailTransaction_RetrieveAll"))
            {
                proc.AddBigIntegerPara("@CallerUserInfoId", userid);
                proc.AddNVarcharPara("@CallerUserName", 255, username);
                proc.AddBigIntegerPara("@ClientId", clientid);
                proc.AddBigIntegerPara("@SiteId", siteid);
                proc.AddIntegerPara("@Val", val);
                return proc.GetTable(conString);
            }
        }

    }
}