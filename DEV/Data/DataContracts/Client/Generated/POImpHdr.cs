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

using Database.Business;
using Database;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the POImpHdr table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class POImpHdr : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public POImpHdr()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_POImpHdr dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.POImpHdrId = dbObj.POImpHdrId;
            this.SiteId = dbObj.SiteId;
            this.PONumber = dbObj.PONumber;
            this.Revision = dbObj.Revision;
            this.EXPOID = dbObj.EXPOID;
            this.EXPRID = dbObj.EXPRID;
            this.SOMAXPRNumber = dbObj.SOMAXPRNumber;
            this.SOMAXPRID = dbObj.SOMAXPRID;
            this.POCreateDate = dbObj.POCreateDate;
            this.Currency = dbObj.Currency;
            this.EXVendor = dbObj.EXVendor;
            this.EXVendorId = dbObj.EXVendorId;
            this.RequiredDate = dbObj.RequiredDate;
            this.PaymentTerms = dbObj.PaymentTerms;
            this.Status = dbObj.Status;
            this.ErrorCodes = dbObj.ErrorCodes;
            this.ErrorMessage = dbObj.ErrorMessage;
            this.LastProcess = dbObj.LastProcess;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_POImpHdr dbObj = new b_POImpHdr();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_POImpHdr ToDatabaseObject()
        {
            b_POImpHdr dbObj = new b_POImpHdr();
            dbObj.ClientId = this.ClientId;
            dbObj.POImpHdrId = this.POImpHdrId;
            dbObj.SiteId = this.SiteId;
            dbObj.PONumber = this.PONumber;
            dbObj.Revision = this.Revision;
            dbObj.EXPOID = this.EXPOID;
            dbObj.EXPRID = this.EXPRID;
            dbObj.SOMAXPRNumber = this.SOMAXPRNumber;
            dbObj.SOMAXPRID = this.SOMAXPRID;
            dbObj.POCreateDate = this.POCreateDate;
            dbObj.Currency = this.Currency;
            dbObj.EXVendor = this.EXVendor;
            dbObj.EXVendorId = this.EXVendorId;
            dbObj.RequiredDate = this.RequiredDate;
            dbObj.PaymentTerms = this.PaymentTerms;
            dbObj.Status = this.Status;
            dbObj.ErrorCodes = this.ErrorCodes;
            dbObj.ErrorMessage = this.ErrorMessage;
            dbObj.LastProcess = this.LastProcess;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            POImpHdr_Create trans = new POImpHdr_Create();
            trans.POImpHdr = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.POImpHdr);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            POImpHdr_Retrieve trans = new POImpHdr_Retrieve();
            trans.POImpHdr = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.POImpHdr);
        }

        public void Update(DatabaseKey dbKey) 
        {
            POImpHdr_Update trans = new POImpHdr_Update();
            trans.POImpHdr = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.POImpHdr);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            POImpHdr_Delete trans = new POImpHdr_Delete();
            trans.POImpHdr = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.POImpHdrId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _POImpHdrId;
        private long _SiteId;
        private string _PONumber;
        private int _Revision;
        private long _EXPOID;
        private long _EXPRID;
        private string _SOMAXPRNumber;
        private long _SOMAXPRID;
        private DateTime? _POCreateDate;
        private string _Currency;
        private string _EXVendor;
        private long? _EXVendorId;
        private DateTime? _RequiredDate;
        private string _PaymentTerms;
        private string _Status;
        private string _ErrorCodes;
        private string _ErrorMessage;
        private DateTime? _LastProcess;
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
        /// POImpHdrId property
        /// </summary>
        [DataMember]
        public long POImpHdrId
        {
            get { return _POImpHdrId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _POImpHdrId); }
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
        /// PONumber property
        /// </summary>
        [DataMember]
        public string PONumber
        {
            get { return _PONumber; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PONumber); }
        }

        /// <summary>
        /// Revision property
        /// </summary>
        [DataMember]
        public int Revision
        {
            get { return _Revision; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _Revision); }
        }

        /// <summary>
        /// EXPOID property
        /// </summary>
        [DataMember]
        public long EXPOID
        {
            get { return _EXPOID; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _EXPOID); }
        }

        /// <summary>
        /// EXPRID property
        /// </summary>
        [DataMember]
        public long EXPRID
        {
            get { return _EXPRID; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _EXPRID); }
        }

        /// <summary>
        /// SOMAXPRNumber property
        /// </summary>
        [DataMember]
        public string SOMAXPRNumber
        {
            get { return _SOMAXPRNumber; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _SOMAXPRNumber); }
        }

        /// <summary>
        /// SOMAXPRID property
        /// </summary>
        [DataMember]
        public long SOMAXPRID
        {
            get { return _SOMAXPRID; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SOMAXPRID); }
        }

        /// <summary>
        /// POCreateDate property
        /// </summary>
        [DataMember]
        public DateTime? POCreateDate
        {
            get { return _POCreateDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _POCreateDate); }
        }

        /// <summary>
        /// Currency property
        /// </summary>
        [DataMember]
        public string Currency
        {
            get { return _Currency; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Currency); }
        }

        /// <summary>
        /// EXVendor property
        /// </summary>
        [DataMember]
        public string EXVendor
        {
            get { return _EXVendor; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _EXVendor); }
        }

        /// <summary>
        /// EXVendorId property
        /// </summary>
        [DataMember]
        public long? EXVendorId
        {
            get { return _EXVendorId; }
            set { Set<long?>(MethodBase.GetCurrentMethod().Name, value, ref _EXVendorId); }
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
        /// PaymentTerms property
        /// </summary>
        [DataMember]
        public string PaymentTerms
        {
            get { return _PaymentTerms; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PaymentTerms); }
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
        /// ErrorCodes property
        /// </summary>
        [DataMember]
        public string ErrorCodes
        {
            get { return _ErrorCodes; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ErrorCodes); }
        }

        /// <summary>
        /// ErrorMessage property
        /// </summary>
        [DataMember]
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ErrorMessage); }
        }

        /// <summary>
        /// LastProcess property
        /// </summary>
        [DataMember]
        public DateTime? LastProcess
        {
            get { return _LastProcess; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _LastProcess); }
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
