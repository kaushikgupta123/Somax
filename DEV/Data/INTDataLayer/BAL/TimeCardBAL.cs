using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

//using SOMAX.G4.Data.Common;
using SOMAX.G4.Data.INTDataLayer.DAL;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
    public class TimeCardBAL
    {


        public DataTable GetTimeCardDetails(string ConnectionString, string TimeFilter, long UserInfoId, long ClientID, long SiteID, long PersonnelId)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_Timecard_RetrieveAllForSearch");
            // IOS01-125 - RKL 
            // ANV001-89  - RKL
            // Need to send the personnelid - NOT the userinfoid 

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddBigIntegerPara("@PersonnelId", PersonnelId);
            proc.AddNVarcharPara("@TimeFilter", 15, TimeFilter);

            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }

    }
}