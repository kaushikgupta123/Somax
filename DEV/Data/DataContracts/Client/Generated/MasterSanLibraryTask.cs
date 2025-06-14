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
    /// Business object that stores a record from the MasterSanLibraryTask table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class MasterSanLibraryTask : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MasterSanLibraryTask()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_MasterSanLibraryTask dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.MasterSanLibraryTaskId = dbObj.MasterSanLibraryTaskId;
            this.MasterSanLibraryId = dbObj.MasterSanLibraryId;
            this.TaskId = dbObj.TaskId;
            this.Description = dbObj.Description;
            this.Del = dbObj.Del;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_MasterSanLibraryTask dbObj = new b_MasterSanLibraryTask();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_MasterSanLibraryTask ToDatabaseObject()
        {
            b_MasterSanLibraryTask dbObj = new b_MasterSanLibraryTask();
            dbObj.ClientId = this.ClientId;
            dbObj.MasterSanLibraryTaskId = this.MasterSanLibraryTaskId;
            dbObj.MasterSanLibraryId = this.MasterSanLibraryId;
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
            MasterSanLibraryTask_Create trans = new MasterSanLibraryTask_Create();
            trans.MasterSanLibraryTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.MasterSanLibraryTask);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            MasterSanLibraryTask_Retrieve trans = new MasterSanLibraryTask_Retrieve();
            trans.MasterSanLibraryTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.MasterSanLibraryTask);
        }

        public void Update(DatabaseKey dbKey) 
        {
            MasterSanLibraryTask_Update trans = new MasterSanLibraryTask_Update();
            trans.MasterSanLibraryTask = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.MasterSanLibraryTask);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            MasterSanLibraryTask_Delete trans = new MasterSanLibraryTask_Delete();
            trans.MasterSanLibraryTask = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.MasterSanLibraryTaskId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _MasterSanLibraryTaskId;
        private long _MasterSanLibraryId;
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
        /// MasterSanLibraryTaskId property
        /// </summary>
        [DataMember]
        public long MasterSanLibraryTaskId
        {
            get { return _MasterSanLibraryTaskId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _MasterSanLibraryTaskId); }
        }

        /// <summary>
        /// MasterSanLibraryId property
        /// </summary>
        [DataMember]
        public long MasterSanLibraryId
        {
            get { return _MasterSanLibraryId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _MasterSanLibraryId); }
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
