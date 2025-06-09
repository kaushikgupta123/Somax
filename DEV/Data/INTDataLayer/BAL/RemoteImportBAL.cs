/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2017 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date         JIRA Item Person            Description
* ===========  ========= ================= =======================================================
* 
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
  public class RemoteImportBAL
  {
        string Sqlstr ;
        public DataTable GetCNCData(int daqChannel, string ConnectionString)
        {
            switch (daqChannel)
            {
                case 0:
                    Sqlstr = "SELECT top(100) c.[Time], c.[Spindle Load], c.[daqChannel] FROM CNCData c WHERE  c.[daqChannel] =" + daqChannel.ToString();
                    break;
                case 1:
                    Sqlstr = "SELECT top(100) c.[Time], c.[VibrationVelocity], c.[daqChannel] FROM CNCData c WHERE  c.[daqChannel] =" + daqChannel.ToString();
                    break;

            }

            ProcedureExecute proc = new ProcedureExecute(Sqlstr, true);
            DataTable dt = proc.GetTable(ConnectionString);
            return dt;
        }
        public DataTable GetBladeSpeedData(string ConnectionString,string IntConString)
        {
          // Blade Speed Sensor Id = 1100
          // Retrieve only the data that is later than the data we have 
          // Retieve the date/time stamp from latest sensor reading for this sensor 
          string SqlString_ForDate = "Select Top 1 IsNull(messagedate,'2000-01-01 00:00:00') messagedate from SensorReading where sensorid = 1100 Order by messagedate desc";
          ProcedureExecute proc1 = new ProcedureExecute(SqlString_ForDate, true);
          DataTable dtlastdate = proc1.GetTable(IntConString);
          DateTime lastdate;
          if (dtlastdate.Rows.Count > 0)
          {
            lastdate = Convert.ToDateTime(dtlastdate.Rows[0]["messagedate"]);
          }
          else
          {
            lastdate = Convert.ToDateTime("2000-01-01 00:00:00");
          }
          // Now build the string to get the data 
          string strLastDate = lastdate.ToString("yyyy-MM-dd HH:mm:ss");
          string line1 = string.Format("Declare @StartTime DateTime = '{0}' ", strLastDate);
          StringBuilder sbSqlStr = new StringBuilder();          
          sbSqlStr.Append(line1);
          sbSqlStr.Append("Declare @BS Table(RowId int identity(1, 1), RowNum int,[Time] DateTime,[Blade Speed] float, [Metric] nvarchar(50)) ");
          sbSqlStr.Append("Insert @BS(RowNum,[Time],[Blade Speed],[Metric]) Select 0,[Time],[Blade Speed],[Metric] From Saw_BladeSpeed ");
          sbSqlStr.Append("Where[Time]> @StartTime and[Blade Speed] > 0 ");
          sbSqlStr.Append("; WITH cte as(SELECT RowId, ROW_NUMBER() OVER(PARTITION BY[Time] ORDER BY[Time] DESC) RowNum,[Time],[Blade Speed]  FROM @BS) ");
          sbSqlStr.Append("Update @BS Set RowNum = cte.RowNum From @BS INNER JOIN cte on cte.RowId = [@BS].RowId ");
          sbSqlStr.Append("Delete from @BS Where RowNum > 1 ");
          sbSqlStr.Append("SELECT c.[Time], c.[Blade Speed], c.[Metric] FROM @BS c WHERE isnull(c.[Blade Speed],0) > 0   and[Time] >= @StartTime order by[time] ");
          Sqlstr = sbSqlStr.ToString();
          //Sqlstr = "SELECT top(100) c.[Time], c.[Blade Speed], c.[Metric] FROM Saw_BladeSpeed c";       
          ProcedureExecute proc = new ProcedureExecute(Sqlstr, true);
          DataTable dt = proc.GetTable(ConnectionString);
          return dt;
        }
        public DataTable GetMainMotorCurrentData(string ConnectionString, string IntConString)
        {
          // Motor Current Sensor Id = 1200
          // Retrieve only the data that is later than the data we have 
          // Retieve the date/time stamp from latest sensor reading for this sensor 
          string SqlString_ForDate = "Select Top 1 IsNull(messagedate,'2000-01-01 00:00:00') messagedate from SensorReading where sensorid = 1200 Order by messagedate desc";
          ProcedureExecute proc1 = new ProcedureExecute(SqlString_ForDate, true);
          DataTable dtlastdate = proc1.GetTable(IntConString);
          DateTime lastdate;
          if (dtlastdate.Rows.Count > 0)
          {
            lastdate = Convert.ToDateTime(dtlastdate.Rows[0]["messagedate"]);
          }
          else
          {
            lastdate = Convert.ToDateTime("2000-01-01 00:00:00");
          }
          // Now build the string to get the data 
          string strLastDate = lastdate.ToString("yyyy-MM-dd HH:mm:ss");
          string line1 = string.Format("Declare @StartTime DateTime = '{0}' ", strLastDate);
          StringBuilder sbSqlStr = new StringBuilder();
          sbSqlStr.Append(line1);
          sbSqlStr.Append("Declare @BS Table(RowId int identity(1, 1), RowNum int,[Time] DateTime,[Motor Current] float, [Metric] nvarchar(50)) ");
          sbSqlStr.Append("Insert @BS(RowNum,[Time],[Motor Current],[Metric]) Select 0,[Time],[Motor Current],[Metric] From Saw_MainMotorCurrent ");
          sbSqlStr.Append("Where[Time]> @StartTime and [Motor Current] > 0 ");
          sbSqlStr.Append("; WITH cte as(SELECT RowId, ROW_NUMBER() OVER(PARTITION BY[Time] ORDER BY[Time] DESC) RowNum,[Time],[Motor Current]  FROM @BS) ");
          sbSqlStr.Append("Update @BS Set RowNum = cte.RowNum From @BS INNER JOIN cte on cte.RowId = [@BS].RowId ");
          sbSqlStr.Append("Delete from @BS Where RowNum > 1 ");
          sbSqlStr.Append("SELECT c.[Time], c.[Motor Current], c.[Metric] FROM @BS c WHERE isnull(c.[Motor Current],0) > 0   and[Time] >= @StartTime order by[time] ");
          Sqlstr = sbSqlStr.ToString();
          ProcedureExecute proc = new ProcedureExecute(Sqlstr, true);
          DataTable dt = proc.GetTable(ConnectionString);
          return dt;

      /*
        Sqlstr = "SELECT top(100) c.[Time], c.[Motor Current], c.[Metric] FROM Saw_MainMotorCurrent c";
        ProcedureExecute proc = new ProcedureExecute(Sqlstr, true);
        DataTable dt = proc.GetTable(ConnectionString);
        return dt;
    */
    }
  }
}
