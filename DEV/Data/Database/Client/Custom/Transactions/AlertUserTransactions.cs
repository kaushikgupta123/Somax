/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
***************************************************************************************************
* Date         JIRA Item Person         Description
* ===========  ========= ============== ===========================================================
* 2016-Sep-04 SOM-1037   Roger Lawton   AlertComposite_RetrieveBySearchCriteria
*                                       Retrieve by PersonnelId not UserInfoId
***************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Database;
using Database.Business;
using Common.Enumerations;

namespace Database
{
    public class AlertUser_MarkAsRead : AlertUser_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            AlertUser.IsRead = true;
            AlertUser.UpdateByAlertIdAndUserId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }

    public class AlertUser_MarkAsDeleted : AlertUser_TransactionBaseClass
    {
        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            AlertUser.IsRead = true;
            AlertUser.IsDeleted = true;
            AlertUser.UpdateByAlertIdAndUserId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
        }
    }


    public class AlertUser_RetrievePendingCount : AbstractTransactionManager
    {
        public AlertUser_RetrievePendingCount()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public int PendingCount { get; set; }

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;
            PendingCount = b_AlertUser.RetrievePendingByUserId(this.Connection, this.Transaction, dbKey.User.UserInfoId, this.dbKey.Client.ClientId);
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }

    public class AlertComposite_RetrieveBySearchCriteria : AbstractTransactionManager
    {
        public AlertComposite_RetrieveBySearchCriteria()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }

        public bool RetrieveActionItems { get; set; }
        public bool RetrieveMessages { get; set; }

        public string AlertType { get; set; }
        public string From { get; set; }
        public string ObjectName { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public string Column { get; set; }
        public string SearchText { get; set; }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDirection { get; set; }

        public int ResultCount { get; set; }

        public List<KeyValuePair<b_Alerts, b_AlertUser>> AlertData;

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            base.UseTransaction = false;

            int resultCount;
            // SOM-1037 - Return by PersonnelId not UserInfoId
            AlertData = b_AlertUser.RetrieveAllBySearchCriteria(this.Connection, this.Transaction, dbKey.User.UserInfoId,
                this.dbKey.Client.ClientId,this.dbKey.Personnel.PersonnelId,
                RetrieveActionItems, RetrieveMessages, AlertType, From, ObjectName, DateStart, DateEnd, Column, 
                SearchText, PageNumber, ResultsPerPage, OrderColumn, OrderDirection, out resultCount);
            ResultCount = resultCount;
        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }


    public class AlertComposite_RetrieveCountBySearchCriteria : AbstractTransactionManager
    {
        public AlertComposite_RetrieveCountBySearchCriteria()
        {
            // Set the database in which this table resides.
            // This must be called prior to base.PerformLocalValidation(), 
            // since that process will validate that the appropriate 
            // connection string is set.
            UseDatabase = DatabaseTypeEnum.Client;
        }
        public bool IsRead { get; set; }
        public int ResultCount { get; set; }
        public int ResultMaintenanceCount { get; set; }
        public int ResultInventoryCount { get; set; }
        public int ResultProcurementCount { get; set; }
        public int ResultSystemCount { get; set; }

        public int AlertData;

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
        }

        public override void PerformWorkItem()
        {
            int resultMaintenanceCount;
            int resultInventoryCount;
            int resultProcurementCount;
            int resultSystemCount;
            base.UseTransaction = false;
            // SOM-1037 - Return by PersonnelId not UserInfoId
            AlertData = b_AlertUser.RetrieveCountBySearchCriteria(this.Connection, this.Transaction, 
                this.dbKey.Client.ClientId, this.dbKey.Personnel.PersonnelId,IsRead,out resultMaintenanceCount,out resultInventoryCount,out resultProcurementCount, out resultSystemCount);
            ResultCount = AlertData;
            ResultMaintenanceCount = resultMaintenanceCount;
            ResultInventoryCount = resultInventoryCount;
            ResultProcurementCount = resultProcurementCount;
            ResultSystemCount = resultSystemCount;

        }

        public override void Preprocess()
        {
            // nothing to do here
        }

        public override void Postprocess()
        {
            // nothing to do here
        }
    }

    public class AlertUser_RemoveByObjectId : AlertUser_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();
            
        }

        public override void PerformWorkItem()
        {
            AlertUser.RemoveAlertByObjectId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);
            
        }
    }
    public class AlertUser_UpdateByUserId : AlertUser_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            AlertUser.UpdateAlertByUserId(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    public class AlertUser_UpdateByUserIdAndNotificationTab : AlertUser_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            AlertUser.UpdateAlertByUserIdAndNotificationTab(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
    
    public class AlertUser_UpdateAlertByNotificationType : AlertUser_TransactionBaseClass
    {

        public override void PerformLocalValidation()
        {
            base.PerformLocalValidation();

        }

        public override void PerformWorkItem()
        {
            AlertUser.UpdateAlertByNotificationType(this.Connection, this.Transaction, CallerUserInfoId, CallerUserName);

        }
    }
}
