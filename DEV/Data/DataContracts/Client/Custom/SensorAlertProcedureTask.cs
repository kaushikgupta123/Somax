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
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2011-Dec-09 20110019 Roger Lawton        Added ClientLookupId to search results
* 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
* 2014-Aug-10 SOM-280  Roger Lawton        Modified UpdateFromDataObjectList to include 
*                                          LaborAccountClientLookupId
* 2015-Mar-03 SOM-590  Roger Lawton        Removed validation on columns we do not support
* 2015-Sep-14 SOM-805  Roger Lawton        Location - Show Location.ClientLookupId if FACILITIES
***************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Common.Structures;
using Common.Extensions;

namespace DataContracts
{
    public partial class SensorAlertProcedureTask : DataContractBase
    {
        #region Properties
        public string Inactive { get; set; }
        string ValidateFor = string.Empty;

        #endregion


        public List<SensorAlertProcedureTask> SensorAlertProcedureTask_RetrieveByProcedureID(DatabaseKey dbKey, long ObjectId)
        {
            SensorAlertProcedureTaskRetrieveAll trans = new SensorAlertProcedureTaskRetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                SensorAlertProcedureId = ObjectId
            };
            trans.SensorAlertProcedureTask = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SensorAlertProcedureTask> SensorAlertProcedureTaskList = new List<SensorAlertProcedureTask>();
            foreach (b_SensorAlertProcedureTask SensorAlertProcedureTask in trans.SensorAlertProcedureTaskList)
            {
                SensorAlertProcedureTask tmpSensorAlertProcedureTask = new SensorAlertProcedureTask();

                tmpSensorAlertProcedureTask.UpdateFromDatabaseObject(SensorAlertProcedureTask);
                SensorAlertProcedureTaskList.Add(tmpSensorAlertProcedureTask);
            }
            return SensorAlertProcedureTaskList;

        }
        
       
    }
}
