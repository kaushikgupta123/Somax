/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2019 by SOMAX Inc.
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
    /// Business object that stores a record from the PartTransferEventLog table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class PartTransferEventLog : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PartTransferEventLog()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_PartTransferEventLog dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.PartTransferEventLogId = dbObj.PartTransferEventLogId;
            this.PartTransferId = dbObj.PartTransferId;
            this.TransactionDate = dbObj.TransactionDate;
            this.Event = dbObj.Event;
            this.PersonnelId = dbObj.PersonnelId;
            this.EventCode = dbObj.EventCode;
            this.Quantity = dbObj.Quantity;
            this.Comments = dbObj.Comments;
            this.SourceId = dbObj.SourceId;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_PartTransferEventLog dbObj = new b_PartTransferEventLog();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_PartTransferEventLog ToDatabaseObject()
        {
            b_PartTransferEventLog dbObj = new b_PartTransferEventLog();
            dbObj.ClientId = this.ClientId;
            dbObj.PartTransferEventLogId = this.PartTransferEventLogId;
            dbObj.PartTransferId = this.PartTransferId;
            dbObj.TransactionDate = this.TransactionDate;
            dbObj.Event = this.Event;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.EventCode = this.EventCode;
            dbObj.Quantity = this.Quantity;
            dbObj.Comments = this.Comments;
            dbObj.SourceId = this.SourceId;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            PartTransferEventLog_Create trans = new PartTransferEventLog_Create();
            trans.PartTransferEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.PartTransferEventLog);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            PartTransferEventLog_Retrieve trans = new PartTransferEventLog_Retrieve();
            trans.PartTransferEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PartTransferEventLog);
        }

        public void Update(DatabaseKey dbKey) 
        {
            PartTransferEventLog_Update trans = new PartTransferEventLog_Update();
            trans.PartTransferEventLog = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PartTransferEventLog);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            PartTransferEventLog_Delete trans = new PartTransferEventLog_Delete();
            trans.PartTransferEventLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PartTransferEventLogId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _PartTransferEventLogId;
        private long _PartTransferId;
        private DateTime? _TransactionDate;
        private string _Event;
        private long _PersonnelId;
        private string _EventCode;
        private decimal _Quantity;
        private string _Comments;
        private long _SourceId;
        private int _UpdateIndex;
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
        /// PartTransferEventLogId property
        /// </summary>
        [DataMember]
        public long PartTransferEventLogId
        {
            get { return _PartTransferEventLogId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartTransferEventLogId); }
        }

        /// <summary>
        /// PartTransferId property
        /// </summary>
        [DataMember]
        public long PartTransferId
        {
            get { return _PartTransferId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartTransferId); }
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
        /// EventCode property
        /// </summary>
        [DataMember]
        public string EventCode
        {
            get { return _EventCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _EventCode); }
        }

        /// <summary>
        /// Quantity property
        /// </summary>
        [DataMember]
        public decimal Quantity
        {
            get { return _Quantity; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Quantity); }
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
        /// SourceId property
        /// </summary>
        [DataMember]
        public long SourceId
        {
            get { return _SourceId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SourceId); }
        }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        [DataMember]
        public int UpdateIndex
        {
            get { return _UpdateIndex; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _UpdateIndex); }
        }
        #endregion
		
		
    }
}
