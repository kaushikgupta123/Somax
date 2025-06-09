/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;

namespace DataContracts
{
    public partial class AlertUser
    {
        public static int GetPendingAlertCount(DatabaseKey dbKey)
        {
            AlertUser_RetrievePendingCount trans = new AlertUser_RetrievePendingCount();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return trans.PendingCount;
        }

        public b_AlertUser ToExtendedDatabaseObject()
        {
            b_AlertUser dbObj = this.ToDatabaseObject();
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ObjectId = this.ObjectId;
            return dbObj;
        }
        public b_AlertUser ToExtendedClearDataDatabaseObject()
        {
            b_AlertUser dbObj = this.ToDatabaseObject();
            dbObj.UserId = this.UserId;
            dbObj.ObjectId = this.ObjectId;
            dbObj.SelectedNotificationTab = this.SelectedNotificationTab;
            return dbObj;
        }
        public long PersonnelId { get; set; }
        public long ObjectId { get; set; }

        public string SelectedNotificationTab { get; set; }

        public void RemoveAlert(DatabaseKey dbKey)
        {
            AlertUser_RemoveByObjectId trans = new AlertUser_RemoveByObjectId();

            trans.AlertUser = this.ToExtendedDatabaseObject();

         
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AlertUser);
        }
        public void UpdateAlertByUserId(DatabaseKey dbKey)
        {
            AlertUser_UpdateByUserId trans = new AlertUser_UpdateByUserId();

            trans.AlertUser = this.ToExtendedDatabaseObject();


            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AlertUser);
        }
        public void UpdateAlertByUserIdAndNotificationTab(DatabaseKey dbKey)
        {
            AlertUser_UpdateByUserIdAndNotificationTab trans = new AlertUser_UpdateByUserIdAndNotificationTab();

            trans.AlertUser = this.ToExtendedClearDataDatabaseObject();


            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AlertUser);
        }
        public void UpdateAlertByNotificationType(DatabaseKey dbKey)
        {
            AlertUser_UpdateAlertByNotificationType trans = new AlertUser_UpdateAlertByNotificationType();

            trans.AlertUser = this.ToExtendedClearDataDatabaseObject();


            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AlertUser);
        }
    }
}
