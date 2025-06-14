/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2021 by SOMAX Inc.
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
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;

using Database;
using Database.Business;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the AssetAvailabilityLog table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class AssetAvailabilityLog : DataContractBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AssetAvailabilityLog()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_AssetAvailabilityLog dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.AssetAvailabilityLogId = dbObj.AssetAvailabilityLogId;
            this.ObjectId = dbObj.ObjectId;
            this.TransactionDate = dbObj.TransactionDate;
            this.Event = dbObj.Event;
            this.PersonnelId = dbObj.PersonnelId;
            this.ReturnToService = dbObj.ReturnToService;
            this.Reason = dbObj.Reason;
            this.SourceId = dbObj.SourceId;
            this.ReasonCode = dbObj.ReasonCode;

            // Turn on auditing
            AuditEnabled = true;
        }

        private void Initialize()
        {
            b_AssetAvailabilityLog dbObj = new b_AssetAvailabilityLog();
            UpdateFromDatabaseObject(dbObj);

            // Turn off auditing for object initialization
            AuditEnabled = false;
        }

        public b_AssetAvailabilityLog ToDatabaseObject()
        {
            b_AssetAvailabilityLog dbObj = new b_AssetAvailabilityLog();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.AssetAvailabilityLogId = this.AssetAvailabilityLogId;
            dbObj.ObjectId = this.ObjectId;
            dbObj.TransactionDate = this.TransactionDate;
            dbObj.Event = this.Event;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ReturnToService = this.ReturnToService;
            dbObj.Reason = this.Reason;
            dbObj.SourceId = this.SourceId;
            dbObj.ReasonCode = this.ReasonCode;
            return dbObj;
        }

        #endregion

        #region Transaction Methods

        public void Create(DatabaseKey dbKey)
        {
            AssetAvailabilityLog_Create trans = new AssetAvailabilityLog_Create();
            trans.AssetAvailabilityLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.AssetAvailabilityLog);
        }

        public void Retrieve(DatabaseKey dbKey)
        {
            AssetAvailabilityLog_Retrieve trans = new AssetAvailabilityLog_Retrieve();
            trans.AssetAvailabilityLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AssetAvailabilityLog);
        }

        public void Update(DatabaseKey dbKey)
        {
            AssetAvailabilityLog_Update trans = new AssetAvailabilityLog_Update();
            trans.AssetAvailabilityLog = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AssetAvailabilityLog);
        }

        public void Delete(DatabaseKey dbKey)
        {
            AssetAvailabilityLog_Delete trans = new AssetAvailabilityLog_Delete();
            trans.AssetAvailabilityLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.AssetAvailabilityLogId;
            return BuildChangeLogDbObject(dbKey);
        }

        #endregion

        #region Private Variables

        private long _ClientId;
        private long _SiteId;
        private long _AssetAvailabilityLogId;
        private long _ObjectId;
        private DateTime? _TransactionDate;
        private string _Event;
        private long _PersonnelId;
        private DateTime? _ReturnToService;
        private string _Reason;
        private long _SourceId;
        private string _ReasonCode;
        #endregion

        #region Properties


        /// <summary>
        /// ClientId property
        /// </summary>
        [DataMember]
        public long ClientId
        {
            get { return _ClientId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ClientId); }
        }

        /// <summary>
        /// SiteId property
        /// </summary>
        [DataMember]
        public long SiteId
        {
            get { return _SiteId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SiteId); }
        }

        /// <summary>
        /// AssetAvailabilityLogId property
        /// </summary>
        [DataMember]
        public long AssetAvailabilityLogId
        {
            get { return _AssetAvailabilityLogId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AssetAvailabilityLogId); }
        }

        /// <summary>
        /// ObjectId property
        /// </summary>
        [DataMember]
        public long ObjectId
        {
            get { return _ObjectId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ObjectId); }
        }

        /// <summary>
        /// TransactionDate property
        /// </summary>
        [DataMember]
        public DateTime? TransactionDate
        {
            get { return _TransactionDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _TransactionDate); }
        }

        /// <summary>
        /// Event property
        /// </summary>
        [DataMember]
        public string Event
        {
            get { return _Event; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Event); }
        }

        /// <summary>
        /// PersonnelId property
        /// </summary>
        [DataMember]
        public long PersonnelId
        {
            get { return _PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PersonnelId); }
        }

        /// <summary>
        /// ReturnToService property
        /// </summary>
        [DataMember]
        public DateTime? ReturnToService
        {
            get { return _ReturnToService; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _ReturnToService); }
        }

        /// <summary>
        /// Reason property
        /// </summary>
        [DataMember]
        public string Reason
        {
            get { return _Reason; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Reason); }
        }

        /// <summary>
        /// SourceId property
        /// </summary>
        [DataMember]
        public long SourceId
        {
            get { return _SourceId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SourceId); }
        }

        /// <summary>
        /// ReasonCode property
        /// </summary>
        [DataMember]
        public string ReasonCode
        {
            get { return _ReasonCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ReasonCode); }
        }
        #endregion


    }
}
