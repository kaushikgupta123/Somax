/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PopupAddWorkRequest
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2016-Aug-10 SOM-1037 Roger Lawton       Added Public Variables and RetreiveForNotification
****************************************************************************************************
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
    public partial class AlertSetup
    {
        #region Constants
        private const int OUT_STOCK_ERROR_CODE = 43;
        private const int ISSUE_DATE_ERROR_CODE = 44;
        public decimal TotalCost { get; set; }
        #endregion

        #region Private Variables
        private bool m_validateProcess;
        private bool m_validateAdd;
        private bool m_validateInventoryReceiptAdd;
        private bool m_validateInventoryReceiptProcess;
        private bool m_validatePhysicalInventoryRecordCount;
        #endregion
        #region Public Variables
        public bool Alert_Active { get; set; }
        public bool Alert_TargetList { get; set; }
        public string Alert_LangCult { get; set; }
        public string Alert_Name { get; set; }
        public string Alert_Headline { get; set; }
        public string Alert_Summary { get; set; }
        public string Alert_Type { get; set; }
        public string Alert_Details { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool SMSSend { get; set; }
        public string Type { get; set; }
        public bool TargetList { get; set; }
         
        #endregion 

        #region Transaction Methods
        /// <summary>
        /// Retrieve alert info to generate notification
        /// </summary>
        /// <param name="dbKey"></param>
        /// <returns></returns>
        public AlertSetup RetrieveForNotification(DatabaseKey dbKey)
        {
            AlertSetup_RetrieveForNotification trans = new AlertSetup_RetrieveForNotification()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.AlertSetup = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.AlertSetup);
            return this;
        }
        public b_AlertSetup ToDatabaseObjectExtended()
        {
            b_AlertSetup dbObj = this.ToDatabaseObject();
            // Extended Information
            dbObj.Alert_Active = this.Alert_Active;
            dbObj.Alert_TargetList = this.Alert_TargetList;
            dbObj.Alert_LangCult = this.Alert_LangCult;
            dbObj.Alert_Name = this.Alert_Name;
            dbObj.Alert_Headline = this.Alert_Headline;
            dbObj.Alert_Summary = this.Alert_Summary;
            dbObj.Alert_Type = this.Alert_Type;
            dbObj.Alert_Details = this.Alert_Details;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectExtended(b_AlertSetup dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            // Entended Information
            this.Alert_Active = dbObj.Alert_Active;
            this.Alert_TargetList = dbObj.Alert_TargetList;
            this.Alert_LangCult = dbObj.Alert_LangCult;
            this.Alert_Name = dbObj.Alert_Name;
            this.Alert_Headline = dbObj.Alert_Headline;
            this.Alert_Details = dbObj.Alert_Details;
            this.Alert_Summary = dbObj.Alert_Summary;
            this.Alert_Type = dbObj.Alert_Type;
        }


        public List<AlertSetup> RetrieveAll(DatabaseKey dbKey)
        {
            AlertSetup_RetrieveAll trans = new AlertSetup_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.AlertSetupList);
        }

        public static List<AlertSetup> UpdateFromDatabaseObjectList(List<b_AlertSetup> dbObjs)
        {
            List<AlertSetup> result = new List<AlertSetup>();

            foreach (b_AlertSetup dbObj in dbObjs)
            {
                AlertSetup tmp = new AlertSetup();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        #endregion
        public b_AlertSetup ToDatabaseObjectExd()
        {
            b_AlertSetup dbObj = this.ToDatabaseObject();
            dbObj.AlertSetupId = this.AlertSetupId;
            dbObj.AlertDefineId = this.AlertDefineId;
            dbObj.AlertLocalId = this.AlertLocalId;
            dbObj.IsActive = this.IsActive;
            dbObj.EmailSend = this.EmailSend;
            dbObj.EmailAttachment = this.EmailAttachment;
            dbObj.UpdateIndex = this.UpdateIndex;
            dbObj.Description = this.Description;
            dbObj.TargetList = this.TargetList;
            dbObj.Name = this.Name;
            dbObj.SMSSend = this.SMSSend;
            dbObj.Type = this.Type;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectExd(b_AlertSetup dbObj)
        {
            this.AlertSetupId = dbObj.AlertSetupId;
            this.AlertDefineId = dbObj.AlertDefineId;
            this.AlertLocalId = dbObj.AlertLocalId;
            this.IsActive = dbObj.IsActive;
            this.EmailSend = dbObj.EmailSend;
            this.EmailAttachment = dbObj.EmailAttachment;
            this.UpdateIndex = dbObj.UpdateIndex;
            this.Description = dbObj.Description;
            this.TargetList = dbObj.TargetList;
            this.Name = dbObj.Name;
            this.SMSSend = dbObj.SMSSend;
            this.Type = dbObj.Type;
        }
        public AlertSetup RetrieveNotificationDetails(DatabaseKey dbKey)
        {
            AlertSetup_RetrieveNotificationDetails trans = new AlertSetup_RetrieveNotificationDetails()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.AlertSetup = this.ToDatabaseObjectExd();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExd(trans.AlertSetup);
            return this;
        }
    }
}
