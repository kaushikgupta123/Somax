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
    /// Business object that stores a record from the PartMasterRequest table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class PartMasterRequest : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PartMasterRequest()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_PartMasterRequest dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.PartMasterRequestId = dbObj.PartMasterRequestId;
            this.SiteId = dbObj.SiteId;
            this.Critical = dbObj.Critical;
            this.PurchaseFreq = dbObj.PurchaseFreq;
            this.PurchaseLeadTime = dbObj.PurchaseLeadTime;
            this.PurchaseCost = dbObj.PurchaseCost;
            this.Justification = dbObj.Justification;
            this.Description = dbObj.Description;
            this.Manufacturer = dbObj.Manufacturer;
            this.ManufacturerId = dbObj.ManufacturerId;
            this.UnitOfMeasure = dbObj.UnitOfMeasure;
            this.Status = dbObj.Status;
            this.PartMasterId = dbObj.PartMasterId;
            this.CompleteBy_PersonnelId = dbObj.CompleteBy_PersonnelId;
            this.CompleteDate = dbObj.CompleteDate;
            this.CreatedBy_PersonnelId = dbObj.CreatedBy_PersonnelId;
            this.ApproveDenyBy_PersonnelId = dbObj.ApproveDenyBy_PersonnelId;
            this.ApproveDeny_Date = dbObj.ApproveDeny_Date;
            this.LastReviewedBy_PersonnelId = dbObj.LastReviewedBy_PersonnelId;
            this.LastReviewed_Date = dbObj.LastReviewed_Date;
            this.ImageURL = dbObj.ImageURL;
            this.ApproveDenyBy2_PersonnelId = dbObj.ApproveDenyBy2_PersonnelId;
            this.ApproveDeny2_Date = dbObj.ApproveDeny2_Date;
            this.CreatedBy = dbObj.CreatedBy;
            this.CreatedDate = dbObj.CreatedDate;
            this.RequestType = dbObj.RequestType;
            this.SourceId = dbObj.SourceId;
            this.PartId = dbObj.PartId;
            this.ExportLogId = dbObj.ExportLogId;
            this.UnitCost = dbObj.UnitCost;
            this.Location = dbObj.Location;
            this.QtyMinimum = dbObj.QtyMinimum;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_PartMasterRequest dbObj = new b_PartMasterRequest();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_PartMasterRequest ToDatabaseObject()
        {
            b_PartMasterRequest dbObj = new b_PartMasterRequest();
            dbObj.ClientId = this.ClientId;
            dbObj.PartMasterRequestId = this.PartMasterRequestId;
            dbObj.SiteId = this.SiteId;
            dbObj.Critical = this.Critical;
            dbObj.PurchaseFreq = this.PurchaseFreq;
            dbObj.PurchaseLeadTime = this.PurchaseLeadTime;
            dbObj.PurchaseCost = this.PurchaseCost;
            dbObj.Justification = this.Justification;
            dbObj.Description = this.Description;
            dbObj.Manufacturer = this.Manufacturer;
            dbObj.ManufacturerId = this.ManufacturerId;
            dbObj.UnitOfMeasure = this.UnitOfMeasure;
            dbObj.Status = this.Status;
            dbObj.PartMasterId = this.PartMasterId;
            dbObj.CompleteBy_PersonnelId = this.CompleteBy_PersonnelId;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.CreatedBy_PersonnelId = this.CreatedBy_PersonnelId;
            dbObj.ApproveDenyBy_PersonnelId = this.ApproveDenyBy_PersonnelId;
            dbObj.ApproveDeny_Date = this.ApproveDeny_Date;
            dbObj.LastReviewedBy_PersonnelId = this.LastReviewedBy_PersonnelId;
            dbObj.LastReviewed_Date = this.LastReviewed_Date;
            dbObj.ImageURL = this.ImageURL;
            dbObj.ApproveDenyBy2_PersonnelId = this.ApproveDenyBy2_PersonnelId;
            dbObj.ApproveDeny2_Date = this.ApproveDeny2_Date;
            dbObj.CreatedBy = this.CreatedBy;
            dbObj.CreatedDate = this.CreatedDate;
            dbObj.RequestType = this.RequestType;
            dbObj.SourceId = this.SourceId;
            dbObj.PartId = this.PartId;
            dbObj.ExportLogId = this.ExportLogId;
            dbObj.UnitCost = this.UnitCost;
            dbObj.Location = this.Location;
            dbObj.QtyMinimum = this.QtyMinimum;
            dbObj.QtyMaximum = this.QtyMaximum;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            PartMasterRequest_Create trans = new PartMasterRequest_Create();
            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.PartMasterRequest);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            PartMasterRequest_Retrieve trans = new PartMasterRequest_Retrieve();
            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PartMasterRequest);
        }

        public void Update(DatabaseKey dbKey) 
        {
            PartMasterRequest_Update trans = new PartMasterRequest_Update();
            trans.PartMasterRequest = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PartMasterRequest);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            PartMasterRequest_Delete trans = new PartMasterRequest_Delete();
            trans.PartMasterRequest = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PartMasterRequestId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _PartMasterRequestId;
        private long _SiteId;
        private bool _Critical;
        private string _PurchaseFreq;
        private string _PurchaseLeadTime;
        private string _PurchaseCost;
        private string _Justification;
        private string _Description;
        private string _Manufacturer;
        private string _ManufacturerId;
        private string _UnitOfMeasure;
        private string _Status;
        private long _PartMasterId;
        private long _CompleteBy_PersonnelId;
        private DateTime? _CompleteDate;
        private long _CreatedBy_PersonnelId;
        private long _ApproveDenyBy_PersonnelId;
        private DateTime? _ApproveDeny_Date;
        private long _LastReviewedBy_PersonnelId;
        private DateTime? _LastReviewed_Date;
        private string _ImageURL;
        private long _ApproveDenyBy2_PersonnelId;
        private DateTime? _ApproveDeny2_Date;
        private string _CreatedBy;
        private DateTime? _CreatedDate;
        private string _RequestType;
        private long _SourceId;
        private long _PartId;
        private long _ExportLogId;
        private decimal _UnitCost;
        private string _Location;
        private decimal _QtyMinimum;
        private decimal _QtyMaximum;
        private long _UpdateIndex;
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
        /// PartMasterRequestId property
        /// </summary>
        [DataMember]
        public long PartMasterRequestId
        {
            get { return _PartMasterRequestId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartMasterRequestId); }
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
        /// Critical property
        /// </summary>
        [DataMember]
        public bool Critical
        {
            get { return _Critical; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Critical); }
        }

        /// <summary>
        /// PurchaseFreq property
        /// </summary>
        [DataMember]
        public string PurchaseFreq
        {
            get { return _PurchaseFreq; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PurchaseFreq); }
        }

        /// <summary>
        /// PurchaseLeadTime property
        /// </summary>
        [DataMember]
        public string PurchaseLeadTime
        {
            get { return _PurchaseLeadTime; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PurchaseLeadTime); }
        }

        /// <summary>
        /// PurchaseCost property
        /// </summary>
        [DataMember]
        public string PurchaseCost
        {
            get { return _PurchaseCost; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PurchaseCost); }
        }

        /// <summary>
        /// Justification property
        /// </summary>
        [DataMember]
        public string Justification
        {
            get { return _Justification; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Justification); }
        }

        /// <summary>
        /// Description property
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return _Description; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Description); }
        }

        /// <summary>
        /// Manufacturer property
        /// </summary>
        [DataMember]
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Manufacturer); }
        }

        /// <summary>
        /// ManufacturerId property
        /// </summary>
        [DataMember]
        public string ManufacturerId
        {
            get { return _ManufacturerId; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ManufacturerId); }
        }

        /// <summary>
        /// UnitOfMeasure property
        /// </summary>
        [DataMember]
        public string UnitOfMeasure
        {
            get { return _UnitOfMeasure; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _UnitOfMeasure); }
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
        /// PartMasterId property
        /// </summary>
        [DataMember]
        public long PartMasterId
        {
            get { return _PartMasterId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartMasterId); }
        }

        /// <summary>
        /// CompleteBy_PersonnelId property
        /// </summary>
        [DataMember]
        public long CompleteBy_PersonnelId
        {
            get { return _CompleteBy_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _CompleteBy_PersonnelId); }
        }

        /// <summary>
        /// CompleteDate property
        /// </summary>
        [DataMember]
        public DateTime? CompleteDate
        {
            get { return _CompleteDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _CompleteDate); }
        }

        /// <summary>
        /// CreatedBy_PersonnelId property
        /// </summary>
        [DataMember]
        public long CreatedBy_PersonnelId
        {
            get { return _CreatedBy_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _CreatedBy_PersonnelId); }
        }

        /// <summary>
        /// ApproveDenyBy_PersonnelId property
        /// </summary>
        [DataMember]
        public long ApproveDenyBy_PersonnelId
        {
            get { return _ApproveDenyBy_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ApproveDenyBy_PersonnelId); }
        }

        /// <summary>
        /// ApproveDeny_Date property
        /// </summary>
        [DataMember]
        public DateTime? ApproveDeny_Date
        {
            get { return _ApproveDeny_Date; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _ApproveDeny_Date); }
        }

        /// <summary>
        /// LastReviewedBy_PersonnelId property
        /// </summary>
        [DataMember]
        public long LastReviewedBy_PersonnelId
        {
            get { return _LastReviewedBy_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _LastReviewedBy_PersonnelId); }
        }

        /// <summary>
        /// LastReviewed_Date property
        /// </summary>
        [DataMember]
        public DateTime? LastReviewed_Date
        {
            get { return _LastReviewed_Date; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _LastReviewed_Date); }
        }

        /// <summary>
        /// ImageURL property
        /// </summary>
        [DataMember]
        public string ImageURL
        {
            get { return _ImageURL; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ImageURL); }
        }

        /// <summary>
        /// ApproveDenyBy2_PersonnelId property
        /// </summary>
        [DataMember]
        public long ApproveDenyBy2_PersonnelId
        {
            get { return _ApproveDenyBy2_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ApproveDenyBy2_PersonnelId); }
        }

        /// <summary>
        /// ApproveDeny2_Date property
        /// </summary>
        [DataMember]
        public DateTime? ApproveDeny2_Date
        {
            get { return _ApproveDeny2_Date; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _ApproveDeny2_Date); }
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
        /// CreatedDate property
        /// </summary>
        [DataMember]
        public DateTime? CreatedDate
        {
            get { return _CreatedDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _CreatedDate); }
        }

        /// <summary>
        /// RequestType property
        /// </summary>
        [DataMember]
        public string RequestType
        {
            get { return _RequestType; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _RequestType); }
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
        /// PartId property
        /// </summary>
        [DataMember]
        public long PartId
        {
            get { return _PartId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartId); }
        }

        /// <summary>
        /// ExportLogId property
        /// </summary>
        [DataMember]
        public long ExportLogId
        {
            get { return _ExportLogId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ExportLogId); }
        }

        /// <summary>
        /// UnitCost property
        /// </summary>
        [DataMember]
        public decimal UnitCost
        {
            get { return _UnitCost; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _UnitCost); }
        }

        /// <summary>
        /// Location property
        /// </summary>
        [DataMember]
        public string Location
        {
            get { return _Location; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location); }
        }

        /// <summary>
        /// QtyMinimum property
        /// </summary>
        [DataMember]
        public decimal QtyMinimum
        {
            get { return _QtyMinimum; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QtyMinimum); }
        }

        /// <summary>
        /// QtyMaximum property
        /// </summary>
        [DataMember]
        public decimal QtyMaximum
        {
            get { return _QtyMaximum; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QtyMaximum); }
        }

        /// <summary>
        /// UpdateIndex property
        /// </summary>
        [DataMember]
        public long UpdateIndex
        {
            get { return _UpdateIndex; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _UpdateIndex); }
        }
        #endregion
		
		
    }
}
