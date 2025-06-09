/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
* 2015-Mar-24 SOM-585  Roger Lawton        Localized the Status
****************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class SensorReading : DataContractBase
    {
        public DateTime BackDate { get; set; }
        public DateTime CurrentDate { get; set; }


        public List<SensorReading> RetrieveBySensorID(DatabaseKey dbKey, string timezone)
        {
            SensorReading_RetrieveBySensorID trans = new SensorReading_RetrieveBySensorID
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SensorReading = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDB(trans.SensorReadingList, timezone);
        }
        public List<SensorReading> RetrieveAll(DatabaseKey dbKey, string timezone)
        {
            SensorReading_RetrieveAllByDate trans = new SensorReading_RetrieveAllByDate
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SensorReading = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDB(trans.SensorReadingList, timezone);

        }
        public List<SensorReading> RetrieveSensorDataAll(DatabaseKey dbKey, string timezone)
        {
            SensorReading_RetrieveAll trans = new SensorReading_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDB(trans.SensorReadingList, timezone);

        }

        public List<SensorReading> RetrieveBySensorIDForTimeSeries(DatabaseKey dbKey, string timezone)
        {
            SensorReading_RetrieveBySensorIDForTimeSeriesChart trans = new SensorReading_RetrieveBySensorIDForTimeSeriesChart
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SensorReading = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return ToDatabaseObjectForTimeSeriesChart(trans.SensorReadingList, timezone);
        }

        private b_SensorReading ToDatabaseObjectExtended()
        {
            b_SensorReading dbObj = this.ToDatabaseObject();
            dbObj.ClientId = this.ClientId;
            dbObj.BackDate = this.BackDate;
            dbObj.CurrentDate = this.CurrentDate;
            return dbObj;
        }

        public List<SensorReading> UpdateFromDB(List<b_SensorReading> sensorReadingList, string timezone)
        {
            List<SensorReading> result = new List<SensorReading>();
            foreach (b_SensorReading item in sensorReadingList)
            {
                SensorReading tmp = new SensorReading();
                tmp.UpdateFromExtendedDatabaseObject(item, timezone);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_SensorReading dbObj, string timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.MessageDate = dbObj.MessageDate.ToUserTimeZone(timezone);
        }

        private List<SensorReading> ToDatabaseObjectForTimeSeriesChart(List<b_SensorReading> sensorReadingList, string timezone)
        {
            List<SensorReading> result = new List<SensorReading>();
            foreach (b_SensorReading item in sensorReadingList)
            {
                SensorReading tmp = new SensorReading();
                tmp.MessageDate = item.MessageDate.ToUserTimeZone(timezone);
                tmp.PlotValues = item.PlotValues;
                tmp.PlotLabels = item.PlotLabels;
                result.Add(tmp);
            }
            return result;
        }
    }
}
