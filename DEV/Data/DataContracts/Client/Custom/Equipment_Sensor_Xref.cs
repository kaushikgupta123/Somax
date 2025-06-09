/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person        Description
* =========== ======== ============== =============================================================
* 2011-Dec-09 20110019 Roger Lawton   Added ClientLookupId to search results
* 2011-Dec-14 20110039 Roger Lawton   Added Lookuplist validation
* 2014-Aug-10 SOM-280  Roger Lawton   Modified UpdateFromDataObjectList to include 
*                                     LaborAccountClientLookupId
* 2015-Mar-03 SOM-590  Roger Lawton   Removed validation on columns we do not support
* 2015-Sep-14 SOM-805  Roger Lawton   Location - Show Location.ClientLookupId if FACILITIES
* 2017-Nov-06 SOM-1351 Roger Lawton   Added some columns - cleaned up a bit
***************************************************************************************************
*/
using System;
using System.Collections.Generic;
using Database;
using Database.Business;
using Database.Transactions;
using Common.Constants;

namespace DataContracts
{
    public partial class Equipment_Sensor_Xref : DataContractBase
    {
        #region Properties
        public string AssignedTo_Name { get; set; }
        public string ClientLookupId { get; set; }
        public string Description { get; set; }
        public string EquipmentClientLookupId { get; set; }
        public string EquipmentName { get; set; }
        public string SensorAlertProcedureClientLookUpId { get; set; }
        public long SiteId { get; set; }
        public string Sensor { get; set; }
        #endregion

        // Used by the Alert Procedure 
        // Used by the WebHook Procedure
        public void RetriveBySensorId(DatabaseKey dbKey)
        {
            Equipment_Sensor_Xref_RetriveBySensorId trans = new Equipment_Sensor_Xref_RetriveBySensorId();
            trans.Equipment_Sensor_Xref = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Equipment_Sensor_Xref);
        }
        // Used in the SensorEdit page
        public void RetrieveByExrefId(DatabaseKey dbKey)
        {
            Equipment_Sensor_Xref_RetriveByExrefId trans = new Equipment_Sensor_Xref_RetriveByExrefId();
            trans.Equipment_Sensor_Xref = this.ToDbObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromExtendedDatabaseObject(trans.Equipment_Sensor_Xref);
        }

        // Used on the SensorSearch Page
        public List<Equipment_Sensor_Xref> RetrieveAllEquipmentSensorData(DatabaseKey dbKey)
        {
            Equipment_Sensor_Xref_RetrieveAllEquipmentSensor trans = new Equipment_Sensor_Xref_RetrieveAllEquipmentSensor
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment_Sensor_Xref = this.ToDbObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.Equipment_Sensor_XrefList);

        }

        // Used on the Equipment Edit page (Sensor tab in the additional information panel)
        public List<Equipment_Sensor_Xref> RetriveByEquipmentId(DatabaseKey dbKey)
        {
            Equipment_Sensor_Xref_RetriveByEquipmentId trans = new Equipment_Sensor_Xref_RetriveByEquipmentId
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment_Sensor_Xref = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.Equipment_Sensor_XrefList);

        }

        //private void UpdateFromExtendedDatabaseObject(b_Equipment_Sensor_Xref dbObj)
        //{
        //    this.UpdateFromDatabaseObject(dbObj);

        //    this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
        //    this.EquipmentName = dbObj.EquipmentName;
        //}

        public List<Equipment_Sensor_Xref> UpdateFromDatabaseObjectList(List<b_Equipment_Sensor_Xref> dbObjs)
        {
            List<Equipment_Sensor_Xref> result = new List<Equipment_Sensor_Xref>();

            foreach (b_Equipment_Sensor_Xref dbObj in dbObjs)
            {
                Equipment_Sensor_Xref tmp = new Equipment_Sensor_Xref();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public List<Equipment_Sensor_Xref> UpdateFromDbList(List<b_Equipment_Sensor_Xref> dbObjs)
        {
            List<Equipment_Sensor_Xref> result = new List<Equipment_Sensor_Xref>();

            foreach (b_Equipment_Sensor_Xref dbObj in dbObjs)
            {
                Equipment_Sensor_Xref tmp = new Equipment_Sensor_Xref();
                tmp.UpdateFromExtendedDb(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDb(b_Equipment_Sensor_Xref dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
        }

        public void UpdateFromExtendedDatabaseObject(b_Equipment_Sensor_Xref dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.EquipmentClientLookupId = dbObj.EquipmentClientLookupId;
            this.EquipmentName = dbObj.EquipmentName;
            this.SensorAlertProcedureClientLookUpId = dbObj.SensorAlertProcedureClientLookupId;
            this.AssignedTo_Name = dbObj.AssignedTo_Name;

            switch (this.SensorAppId)
            {
                case 1:
                    this.Sensor = SensorConstant.AppID1;
                    break;
                case 2:
                    this.Sensor = SensorConstant.AppID2;
                    break;
                case 3:
                    this.Sensor = SensorConstant.AppID3;
                    break;
                case 4:
                    this.Sensor = SensorConstant.AppID4;
                    break;
                case 5:
                    this.Sensor = SensorConstant.AppID5;
                    break;
                case 6:
                    this.Sensor = SensorConstant.AppID6;
                    break;
                case 7:
                    this.Sensor = SensorConstant.AppID7;
                    break;
                case 8:
                    this.Sensor = SensorConstant.AppID8;
                    break;
                case 9:
                    this.Sensor = SensorConstant.AppID9;
                    break;
                case 10:
                    this.Sensor = SensorConstant.AppID10;
                    break;
                case 11:
                    this.Sensor = SensorConstant.AppID11;
                    break;
                case 12:
                    this.Sensor = SensorConstant.AppID12;
                    break;
                default:
                    this.Sensor = string.Empty;
                    break;
            }
        }
        public b_Equipment_Sensor_Xref ToDbObject()
        {
            b_Equipment_Sensor_Xref dbObj = new b_Equipment_Sensor_Xref();

            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.Equipment_Sensor_XrefId = this.Equipment_Sensor_XrefId;
            dbObj.SensorId = this.SensorId;
            dbObj.SensorName = this.SensorName;
            dbObj.LastReading = this.LastReading;
            dbObj.EquipmentClientLookupId = this.EquipmentClientLookupId;
            dbObj.EquipmentName = this.EquipmentName;

            return dbObj;
        }

    }
}
