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
    public class UserInfoBAL
    {
        public DataTable GetUserActiveStatusCount(long clientId, long UserInfoId, string conString)
        {
            using (ProcedureExecute proc = new ProcedureExecute("usp_UserInfo_RetrieveActiveStatusCount_V2"))
            {
                proc.AddBigIntegerPara("@ClientId", clientId);
                proc.AddBigIntegerPara("@UserInfoId", UserInfoId);               
                return proc.GetTable(conString);
            }
        }
    }
}
