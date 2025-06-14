/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2022 by SOMAX Inc.
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
    /// Business object that stores a record from the PartCategoryMasterImport table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class PartCategoryMasterImport : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PartCategoryMasterImport()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_PartCategoryMasterImport dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.PartCategoryMasterImportId = dbObj.PartCategoryMasterImportId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.ErrorMessage = dbObj.ErrorMessage;
            this.LastProcess = dbObj.LastProcess;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_PartCategoryMasterImport dbObj = new b_PartCategoryMasterImport();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_PartCategoryMasterImport ToDatabaseObject()
        {
            b_PartCategoryMasterImport dbObj = new b_PartCategoryMasterImport();
            dbObj.ClientId = this.ClientId;
            dbObj.PartCategoryMasterImportId = this.PartCategoryMasterImportId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.InactiveFlag = this.InactiveFlag;
            dbObj.ErrorMessage = this.ErrorMessage;
            dbObj.LastProcess = this.LastProcess;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            PartCategoryMasterImport_Create trans = new PartCategoryMasterImport_Create();
            trans.PartCategoryMasterImport = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.PartCategoryMasterImport);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            PartCategoryMasterImport_Retrieve trans = new PartCategoryMasterImport_Retrieve();
            trans.PartCategoryMasterImport = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PartCategoryMasterImport);
        }

        public void Update(DatabaseKey dbKey) 
        {
            PartCategoryMasterImport_Update trans = new PartCategoryMasterImport_Update();
            trans.PartCategoryMasterImport = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PartCategoryMasterImport);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            PartCategoryMasterImport_Delete trans = new PartCategoryMasterImport_Delete();
            trans.PartCategoryMasterImport = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PartCategoryMasterImportId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _PartCategoryMasterImportId;
        private string _ClientLookupId;
        private string _Description;
        private bool _InactiveFlag;
        private string _ErrorMessage;
        private DateTime? _LastProcess;
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
        /// PartCategoryMasterImportId property
        /// </summary>
        [DataMember]
        public long PartCategoryMasterImportId
        {
            get { return _PartCategoryMasterImportId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartCategoryMasterImportId); }
        }

        /// <summary>
        /// ClientLookupId property
        /// </summary>
        [DataMember]
        public string ClientLookupId
        {
            get { return _ClientLookupId; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ClientLookupId); }
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
        /// InactiveFlag property
        /// </summary>
        [DataMember]
        public bool InactiveFlag
        {
            get { return _InactiveFlag; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _InactiveFlag); }
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
        #endregion
		
		
    }
}
