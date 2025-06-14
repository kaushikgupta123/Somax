﻿/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the SiteMaintenance table.
    /// </summary>
    public partial class ClientMessage : DataContractBase
    {
        #region property
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public int TotalCount { get; set; }
        public DateTime CreateDate { get; set; }
        public long CustomClientId { get; set; }
        public string EasternStartTime { get; set; }
        public string EasternEndTime { get; set; }

        private string _TimeZone;
        public List<ClientMessage> listOfAllClientMeassge { get; set; }
        /// <summary>
        /// MessageText property
        /// </summary>
        [DataMember]
        public string TimeZone
        {
            get { return _TimeZone; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _TimeZone); }
        }
        #endregion



        public b_ClientMessage ToDateBaseObjectExtended()
        {
            b_ClientMessage dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.CustomClientId = this.CustomClientId;

            return dbObj;
        }

        public void UpdateFromDatabaseObjectExtended(b_ClientMessage dbObj)
        {
            this.ClientId=dbObj.ClientId;
            this.ClientMessageId = dbObj.ClientMessageId;
            this.TotalCount = dbObj.TotalCount;
            this.Message = dbObj.Message;
            this.StartDate = dbObj.StartDate;
            this.EndDate = dbObj.EndDate;
            this.CreateDate = dbObj.CreateDate;
        }

        public List<ClientMessage> MessageSelectedClientDetailsChunkSearch(DatabaseKey dbKey)
        {
            ClientMessage_RetrieveChunkSearchFromDetails trans = new ClientMessage_RetrieveChunkSearchFromDetails
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ClientMessage = ToDateBaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();


            List<ClientMessage> ClientMessagelist = new List<ClientMessage>();
            foreach (b_ClientMessage ClientMessage in trans.ClientMessageList)
            {
                ClientMessage tmpClientMessage = new ClientMessage();

                tmpClientMessage.UpdateFromDatabaseObjectExtended(ClientMessage);
                ClientMessagelist.Add(tmpClientMessage);
            }
            return ClientMessagelist;
        }

        public void Create_V2(DatabaseKey dbKey)
        {
            ClientMessage_Create_V2 trans = new ClientMessage_Create_V2();
            trans.ClientMessage = this.ToDatabaseObject_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject_V2(trans.ClientMessage);
        }

        public void Retrieve_V2(DatabaseKey dbKey)
        {
            ClientMessage_Retrieve_V2 trans = new ClientMessage_Retrieve_V2();
            trans.ClientMessage = this.ToDatabaseObject_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject_V2(trans.ClientMessage);
        }

        public void Update_V2(DatabaseKey dbKey)
        {
            ClientMessage_Update_V2 trans = new ClientMessage_Update_V2();
            trans.ClientMessage = this.ToDatabaseObject_V2();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject_V2(trans.ClientMessage);
        }

        public b_ClientMessage ToDatabaseObject_V2()
        {
            b_ClientMessage dbObj = new b_ClientMessage();
            dbObj.CustomClientId = this.CustomClientId;
            dbObj.ClientMessageId = this.ClientMessageId;
            dbObj.Message = this.Message;
            dbObj.StartDate = this.StartDate;
            dbObj.EndDate = this.EndDate;
            dbObj.EasternStartTime = this.EasternStartTime;
            dbObj.EasternEndTime = this.EasternEndTime;
            dbObj.TimeZone = this.TimeZone;

            return dbObj;
        }

        public void UpdateFromDatabaseObject_V2(b_ClientMessage dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.CustomClientId = dbObj.CustomClientId;
            this.ClientMessageId = dbObj.ClientMessageId;
            this.Message = dbObj.Message;
            this.StartDate = dbObj.StartDate;
            this.EndDate = dbObj.EndDate;
            this.EasternStartTime = dbObj.EasternStartTime;
            this.EasternEndTime = dbObj.EasternEndTime;

            // Turn on auditing
            AuditEnabled = true;
        }




        #region  System Client Message Notification
        public ClientMessage RetrieveClientMessageSch(DatabaseKey dbKey, string timeZone, long ClientId)
        {
            ClientMessage_RetrieveClientMessageSch trans = new ClientMessage_RetrieveClientMessageSch()
            {
                ClientId = dbKey.User.ClientId,
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.ClientMessage = this.ToDatabaseObject_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ClientMessage> AllClientMessagelist = new List<ClientMessage>();

            this.listOfAllClientMeassge = new List<ClientMessage>();
            foreach (b_ClientMessage AllClientMessage in trans.ClientMessage.listOfAllClientMeassge)
            {
                ClientMessage tmpAllClientMessage = new ClientMessage();
                tmpAllClientMessage.UpdateFromDatabaseObjectClientMessageExtended(AllClientMessage, TimeZone);
                AllClientMessagelist.Add(tmpAllClientMessage);
            }
            this.listOfAllClientMeassge.AddRange(AllClientMessagelist);
            return this;
        }

        public void UpdateFromDatabaseObjectClientMessageExtended(b_ClientMessage dbObj, string Timezone)
        {
            this.ClientId = dbObj.ClientId;
            this.ClientMessageId = dbObj.ClientMessageId;
            this.Message = dbObj.Message;
            this.StartDate = dbObj.StartDate;
            this.EndDate = dbObj.EndDate;
            this.EasternEndTime = dbObj.EasternEndTime;
            this.EasternStartTime = dbObj.EasternStartTime;
            this.CreateDate = dbObj.CreateDate;
            this.TimeZone = dbObj.TimeZone;
        }
        #endregion


    }
}
