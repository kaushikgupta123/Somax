/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2018 by SOMAX Inc.
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
    /// Business object that stores a record from the MaintOnDemandMasterTask table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class MaintOnDemandMasterTask : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MaintOnDemandMasterTask()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_MaintOnDemandMasterTask dbObj)
        {
		     this.ClientId = dbObj.ClientId;
            this.MaintOnDemandMasterTaskId = dbObj.MaintOnDemandMasterTaskId;
            this.MaintOnDemandMasterId = dbObj.MaintOnDemandMasterId;
            this.TaskId = dbObj.TaskId;
            this.Description = dbObj.Description;
            this.Del = dbObj.Del;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_MaintOnDemandMasterTask dbObj = new b_MaintOnDemandMasterTask();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_MaintOnDemandMasterTask ToDatabaseObject()
        {
            b_MaintOnDemandMasterTask dbObj = new b_MaintOnDemandMasterTask();
            dbObj.ClientId = this.ClientId;
            dbObj.MaintOnDemandMasterTaskId = this.MaintOnDemandMasterTaskId;
            dbObj.MaintOnDemandMasterId = this.MaintOnDemandMasterId;
            dbObj.TaskId = this.TaskId;
            dbObj.Description = this.Description;
            dbObj.Del = this.Del;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            MaintOnDemandMasterTask_Create trans = new MaintOnDemandMasterTask_Create();
            trans.MaintOnDemandMasterTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.MaintOnDemandMasterTask);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            MaintOnDemandMasterTask_Retrieve trans = new MaintOnDemandMasterTask_Retrieve();
            trans.MaintOnDemandMasterTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.MaintOnDemandMasterTask);
        }

        public void Update(DatabaseKey dbKey) 
        {
            MaintOnDemandMasterTask_Update trans = new MaintOnDemandMasterTask_Update();
            trans.MaintOnDemandMasterTask = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.MaintOnDemandMasterTask);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            MaintOnDemandMasterTask_Delete trans = new MaintOnDemandMasterTask_Delete();
            trans.MaintOnDemandMasterTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.MaintOnDemandMasterTaskId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _MaintOnDemandMasterTaskId;
        private long _MaintOnDemandMasterId;
        private string _TaskId;
        private string _Description;
        private bool _Del;
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
        /// MaintOnDemandMasterTaskId property
        /// </summary>
        [DataMember]
        public long MaintOnDemandMasterTaskId
        {
            get { return _MaintOnDemandMasterTaskId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _MaintOnDemandMasterTaskId); }
        }

        /// <summary>
        /// MaintOnDemandMasterId property
        /// </summary>
        [DataMember]
        public long MaintOnDemandMasterId
        {
            get { return _MaintOnDemandMasterId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _MaintOnDemandMasterId); }
        }

        /// <summary>
        /// TaskId property
        /// </summary>
        [DataMember]
        public string TaskId
        {
            get { return _TaskId; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _TaskId); }
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
        /// Del property
        /// </summary>
        [DataMember]
        public bool Del
        {
            get { return _Del; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Del); }
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
