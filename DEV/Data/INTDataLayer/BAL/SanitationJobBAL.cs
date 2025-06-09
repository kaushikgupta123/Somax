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
using SOMAX.G4.Data.Common.Extensions;
using SOMAX.G4.Data.INTDataLayer.EL;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

namespace SOMAX.G4.Data.INTDataLayer.BAL
{
  public class SanitationJobBAL
    {

        public DataTable GetSanitationJobPercentageCompletion(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_SanitationJob_RetrieveForPercentageCompletion");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@clientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@stDate", BeginDate);
            proc.AddDateTimePara("@fnDate", EndDate);

            DataTable dt = proc.GetTable(ConnectionString);
            
            return dt;
        }
        public DataTable GetCompletedSanitationJobsbyLine(long UserInfoId, string UserName, long ClientID, long SiteID, string ConnectionString, DateTime BeginDate, DateTime EndDate,string userTimeZone)
        {
            ProcedureExecute proc = new ProcedureExecute("usp_SanitationJob_CompleteByLineReport");

            proc.AddBigIntegerPara("@CallerUserInfoId", UserInfoId);
            proc.AddNVarcharPara("@CallerUserName", 256, UserName);
            proc.AddBigIntegerPara("@ClientId", ClientID);
            proc.AddBigIntegerPara("@SiteId", SiteID);
            proc.AddDateTimePara("@StartDate", BeginDate);
            proc.AddDateTimePara("@EndDate", EndDate);

            DataTable dt = proc.GetTable(ConnectionString);
            foreach (DataRow sjrow in dt.Rows)
            {
              // sp returns the following dates: [CreateDate],[ScheduledStartDate],[ActualFinishDate]
              #region Date Conversions 
              if (sjrow.Field<DateTime?>("CompleteDate").HasValue)    // checking if date is null
              {
                DateTime newDateTime = sjrow.Field<DateTime>("CompleteDate").ToUserTimeZone(userTimeZone);
                sjrow.SetField<DateTime>("CompleteDate", newDateTime);
              }
              if (sjrow.Field<DateTime?>("SanitationJob_VerifDate").HasValue)    // checking if date is null
              {
                DateTime minDate = new DateTime(0001, 01, 01);
                DateTime SanitationJob_VerifDate = (DateTime)sjrow.Field<DateTime>("SanitationJob_VerifDate");
                if (minDate.Date == SanitationJob_VerifDate.Date)
                {
                  DateTime? newDateTime = null;
                  sjrow.SetField<DateTime?>("SanitationJob_VerifDate", newDateTime);
                }
                else
                {
                  SanitationJob_VerifDate = sjrow.Field<DateTime>("SanitationJob_VerifDate").ToUserTimeZone(userTimeZone);
                  sjrow.SetField<DateTime>("SanitationJob_VerifDate", SanitationJob_VerifDate);
                }
              }
              #endregion
            }
          return dt;
        }
    }
}
