/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2013 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * THIS CODE HAS BEEN GENERATED BY AN AUTOMATED PROCESS.
 * DO NOT MODIFY BY HAND.    MODIFY THE TEMPLATE AND REGENERATE THE CODE 
 * USING THE CURRENT DATABASE IF MODIFICATIONS ARE NEEDED.
 ******************************************************************************
 */


using System;
using System.Runtime.Serialization;
using System.Reflection;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the AlertTarget table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class AlertTarget : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AlertTarget()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_AlertTarget dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.AlertTargetId = dbObj.AlertTargetId;
            this.AlertSetupId = dbObj.AlertSetupId;
            this.UserInfoId = dbObj.UserInfoId;
            this.IsActive = dbObj.IsActive;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_AlertTarget dbObj = new b_AlertTarget();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_AlertTarget ToDatabaseObject()
        {
            b_AlertTarget dbObj = new b_AlertTarget();
            dbObj.ClientId = this.ClientId;
            dbObj.AlertTargetId = this.AlertTargetId;
            dbObj.AlertSetupId = this.AlertSetupId;
            dbObj.UserInfoId = this.UserInfoId;
            dbObj.IsActive = this.IsActive;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            AlertTarget_Create trans = new AlertTarget_Create();
            trans.AlertTarget = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.AlertTarget);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            AlertTarget_Retrieve trans = new AlertTarget_Retrieve();
            trans.AlertTarget = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AlertTarget);
        }

        public void Update(DatabaseKey dbKey) 
        {
            AlertTarget_Update trans = new AlertTarget_Update();
            trans.AlertTarget = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AlertTarget);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            AlertTarget_Delete trans = new AlertTarget_Delete();
            trans.AlertTarget = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.AlertTargetId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _AlertTargetId;
        private long _AlertSetupId;
        private long _UserInfoId;
        private bool _IsActive;
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
        /// AlertTargetId property
        /// </summary>
        [DataMember]
        public long AlertTargetId
        {
            get { return _AlertTargetId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AlertTargetId); }
        }

        /// <summary>
        /// AlertSetupId property
        /// </summary>
        [DataMember]
        public long AlertSetupId
        {
            get { return _AlertSetupId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AlertSetupId); }
        }

        /// <summary>
        /// UserInfoId property
        /// </summary>
        [DataMember]
        public long UserInfoId
        {
            get { return _UserInfoId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _UserInfoId); }
        }

        /// <summary>
        /// IsActive property
        /// </summary>
        [DataMember]
        public bool IsActive
        {
            get { return _IsActive; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _IsActive); }
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
