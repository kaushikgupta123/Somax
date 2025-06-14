/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2020 by SOMAX Inc.
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
    /// Business object that stores a record from the ServiceOrderEventLog table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class ServiceOrderEventLog : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ServiceOrderEventLog()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_ServiceOrderEventLog dbObj)
        {
		            this.ServiceOrderEventLogId = dbObj.ServiceOrderEventLogId;
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.ServiceOrderId = dbObj.ServiceOrderId;
            this.TransactionDate = dbObj.TransactionDate;
            this.Event = dbObj.Event;
            this.PersonnelId = dbObj.PersonnelId;
            this.Comments = dbObj.Comments;
            this.SourceTable = dbObj.SourceTable;
            this.SourceId = dbObj.SourceId;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_ServiceOrderEventLog dbObj = new b_ServiceOrderEventLog();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_ServiceOrderEventLog ToDatabaseObject()
        {
            b_ServiceOrderEventLog dbObj = new b_ServiceOrderEventLog();
            dbObj.ServiceOrderEventLogId = this.ServiceOrderEventLogId;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.ServiceOrderId = this.ServiceOrderId;
            dbObj.TransactionDate = this.TransactionDate;
            dbObj.Event = this.Event;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.Comments = this.Comments;
            dbObj.SourceTable = this.SourceTable;
            dbObj.SourceId = this.SourceId;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            ServiceOrderEventLog_Create trans = new ServiceOrderEventLog_Create();
            trans.ServiceOrderEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ServiceOrderEventLog);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            ServiceOrderEventLog_Retrieve trans = new ServiceOrderEventLog_Retrieve();
            trans.ServiceOrderEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.ServiceOrderEventLog);
        }

        public void Update(DatabaseKey dbKey) 
        {
            ServiceOrderEventLog_Update trans = new ServiceOrderEventLog_Update();
            trans.ServiceOrderEventLog = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ServiceOrderEventLog);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            ServiceOrderEventLog_Delete trans = new ServiceOrderEventLog_Delete();
            trans.ServiceOrderEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.ServiceOrderEventLogId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ServiceOrderEventLogId;
        private long _ClientId;
        private long _SiteId;
        private long _ServiceOrderId;
        private DateTime? _TransactionDate;
        private string _Event;
        private long _PersonnelId;
        private string _Comments;
        private string _SourceTable;
        private long _SourceId;
        #endregion
        
        #region Properties


        /// <summary>
        /// ServiceOrderEventLogId property
        /// </summary>
        [DataMember]
        public long ServiceOrderEventLogId
        {
            get { return _ServiceOrderEventLogId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ServiceOrderEventLogId); }
        }

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
        /// ServiceOrderId property
        /// </summary>
        [DataMember]
        public long ServiceOrderId
        {
            get { return _ServiceOrderId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ServiceOrderId); }
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
        /// Comments property
        /// </summary>
        [DataMember]
        public string Comments
        {
            get { return _Comments; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Comments); }
        }

        /// <summary>
        /// SourceTable property
        /// </summary>
        [DataMember]
        public string SourceTable
        {
            get { return _SourceTable; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _SourceTable); }
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
        #endregion
		
		
    }
}
