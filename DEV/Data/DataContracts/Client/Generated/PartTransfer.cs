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
    /// Business object that stores a record from the PartTransfer table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class PartTransfer : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PartTransfer()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_PartTransfer dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.PartTransferId = dbObj.PartTransferId;
            this.RequestSiteId = dbObj.RequestSiteId;
            this.RequestPartId = dbObj.RequestPartId;
            this.IssueSiteId = dbObj.IssueSiteId;
            this.IssuePartId = dbObj.IssuePartId;
            this.Creator_PersonnelId = dbObj.Creator_PersonnelId;
            this.Quantity = dbObj.Quantity;
            this.Reason = dbObj.Reason;
            this.RequiredDate = dbObj.RequiredDate;
            this.ShippingAccountId = dbObj.ShippingAccountId;
            this.Status = dbObj.Status;
            this.QuantityIssued = dbObj.QuantityIssued;
            this.QuantityReceived = dbObj.QuantityReceived;
            this.LastEvent = dbObj.LastEvent;
            this.LastEventDate = dbObj.LastEventDate;
            this.LastEventBy_PersonnelId = dbObj.LastEventBy_PersonnelId;
            this.CreatedBy = dbObj.CreatedBy;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_PartTransfer dbObj = new b_PartTransfer();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_PartTransfer ToDatabaseObject()
        {
            b_PartTransfer dbObj = new b_PartTransfer();
            dbObj.ClientId = this.ClientId;
            dbObj.PartTransferId = this.PartTransferId;
            dbObj.RequestSiteId = this.RequestSiteId;
            dbObj.RequestPartId = this.RequestPartId;
            dbObj.IssueSiteId = this.IssueSiteId;
            dbObj.IssuePartId = this.IssuePartId;
            dbObj.Creator_PersonnelId = this.Creator_PersonnelId;
            dbObj.Quantity = this.Quantity;
            dbObj.Reason = this.Reason;
            dbObj.RequiredDate = this.RequiredDate;
            dbObj.ShippingAccountId = this.ShippingAccountId;
            dbObj.Status = this.Status;
            dbObj.QuantityIssued = this.QuantityIssued;
            dbObj.QuantityReceived = this.QuantityReceived;
            dbObj.LastEvent = this.LastEvent;
            dbObj.LastEventDate = this.LastEventDate;
            dbObj.LastEventBy_PersonnelId = this.LastEventBy_PersonnelId;
            dbObj.CreatedBy = this.CreatedBy;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            PartTransfer_Create trans = new PartTransfer_Create();
            trans.PartTransfer = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.PartTransfer);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            PartTransfer_Retrieve trans = new PartTransfer_Retrieve();
            trans.PartTransfer = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PartTransfer);
        }

        public void Update(DatabaseKey dbKey) 
        {
            PartTransfer_Update trans = new PartTransfer_Update();
            trans.PartTransfer = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PartTransfer);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            PartTransfer_Delete trans = new PartTransfer_Delete();
            trans.PartTransfer = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PartTransferId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _PartTransferId;
        private long _RequestSiteId;
        private long _RequestPartId;
        private long _IssueSiteId;
        private long _IssuePartId;
        private long _Creator_PersonnelId;
        private decimal _Quantity;
        private string _Reason;
        private DateTime? _RequiredDate;
        private long _ShippingAccountId;
        private string _Status;
        private decimal _QuantityIssued;
        private decimal _QuantityReceived;
        private string _LastEvent;
        private DateTime? _LastEventDate;
        private long _LastEventBy_PersonnelId;
        private string _CreatedBy;
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
        /// PartTransferId property
        /// </summary>
        [DataMember]
        public long PartTransferId
        {
            get { return _PartTransferId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartTransferId); }
        }

        /// <summary>
        /// RequestSiteId property
        /// </summary>
        [DataMember]
        public long RequestSiteId
        {
            get { return _RequestSiteId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _RequestSiteId); }
        }

        /// <summary>
        /// RequestPartId property
        /// </summary>
        [DataMember]
        public long RequestPartId
        {
            get { return _RequestPartId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _RequestPartId); }
        }

        /// <summary>
        /// IssueSiteId property
        /// </summary>
        [DataMember]
        public long IssueSiteId
        {
            get { return _IssueSiteId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _IssueSiteId); }
        }

        /// <summary>
        /// IssuePartId property
        /// </summary>
        [DataMember]
        public long IssuePartId
        {
            get { return _IssuePartId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _IssuePartId); }
        }

        /// <summary>
        /// Creator_PersonnelId property
        /// </summary>
        [DataMember]
        public long Creator_PersonnelId
        {
            get { return _Creator_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _Creator_PersonnelId); }
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
        /// Reason property
        /// </summary>
        [DataMember]
        public string Reason
        {
            get { return _Reason; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Reason); }
        }

        /// <summary>
        /// RequiredDate property
        /// </summary>
        [DataMember]
        public DateTime? RequiredDate
        {
            get { return _RequiredDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _RequiredDate); }
        }

        /// <summary>
        /// ShippingAccountId property
        /// </summary>
        [DataMember]
        public long ShippingAccountId
        {
            get { return _ShippingAccountId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ShippingAccountId); }
        }

        /// <summary>
        /// Status property
        /// </summary>
        [DataMember]
        public string Status
        {
            get { return _Status; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Status); }
        }

        /// <summary>
        /// QuantityIssued property
        /// </summary>
        [DataMember]
        public decimal QuantityIssued
        {
            get { return _QuantityIssued; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QuantityIssued); }
        }

        /// <summary>
        /// QuantityReceived property
        /// </summary>
        [DataMember]
        public decimal QuantityReceived
        {
            get { return _QuantityReceived; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QuantityReceived); }
        }

        /// <summary>
        /// LastEvent property
        /// </summary>
        [DataMember]
        public string LastEvent
        {
            get { return _LastEvent; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _LastEvent); }
        }

        /// <summary>
        /// LastEventDate property
        /// </summary>
        [DataMember]
        public DateTime? LastEventDate
        {
            get { return _LastEventDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _LastEventDate); }
        }

        /// <summary>
        /// LastEventBy_PersonnelId property
        /// </summary>
        [DataMember]
        public long LastEventBy_PersonnelId
        {
            get { return _LastEventBy_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _LastEventBy_PersonnelId); }
        }

        /// <summary>
        /// CreatedBy property
        /// </summary>
        [DataMember]
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _CreatedBy); }
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
