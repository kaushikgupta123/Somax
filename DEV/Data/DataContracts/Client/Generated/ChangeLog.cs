/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2012 by SOMAX Inc.
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
    /// Business object that stores a record from the ChangeLog table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class ChangeLog : DataContractBase, IPermission 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ChangeLog()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_ChangeLog dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.ChangeLogId = dbObj.ChangeLogId;
            this.TableName = dbObj.TableName;
            this.ObjectId = dbObj.ObjectId;
            this.UserName = dbObj.UserName;
            this.UserInfoId = dbObj.UserInfoId;
            this.History = dbObj.History;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_ChangeLog dbObj = new b_ChangeLog();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_ChangeLog ToDatabaseObject()
        {
            b_ChangeLog dbObj = new b_ChangeLog();
            dbObj.ClientId = this.ClientId;
            dbObj.ChangeLogId = this.ChangeLogId;
            dbObj.TableName = this.TableName;
            dbObj.ObjectId = this.ObjectId;
            dbObj.UserName = this.UserName;
            dbObj.UserInfoId = this.UserInfoId;
            dbObj.History = this.History;
            dbObj.SiteId = this.SiteId;
            dbObj.AreaId = this.AreaId;
            dbObj.DepartmentId = this.DepartmentId;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            ChangeLog_Create trans = new ChangeLog_Create();
            trans.ChangeLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ChangeLog);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            ChangeLog_Retrieve trans = new ChangeLog_Retrieve();
            trans.ChangeLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.ChangeLog);
        }

        public void Update(DatabaseKey dbKey) 
        {
            ChangeLog_Update trans = new ChangeLog_Update();
            trans.ChangeLog = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ChangeLog);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            ChangeLog_Delete trans = new ChangeLog_Delete();
            trans.ChangeLog = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.ChangeLogId;
			AuditTargetSiteId = this.SiteId;
			AuditTargetAreaId = this.AreaId;
			AuditTargetDepartmentId = this.DepartmentId;
			AuditTargetStoreRoomId = this.StoreroomId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _ChangeLogId;
        private string _TableName;
        private long _ObjectId;
        private string _UserName;
        private long _UserInfoId;
        private string _History;
        private long _SiteId;
        private long _AreaId;
        private long _DepartmentId;
        private long _StoreroomId;
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
        /// ChangeLogId property
        /// </summary>
        [DataMember]
        public long ChangeLogId
        {
            get { return _ChangeLogId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ChangeLogId); }
        }

        /// <summary>
        /// TableName property
        /// </summary>
        [DataMember]
        public string TableName
        {
            get { return _TableName; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _TableName); }
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
        /// UserName property
        /// </summary>
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _UserName); }
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
        /// History property
        /// </summary>
        [DataMember]
        public string History
        {
            get { return _History; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _History); }
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
        /// AreaId property
        /// </summary>
        [DataMember]
        public long AreaId
        {
            get { return _AreaId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AreaId); }
        }

        /// <summary>
        /// DepartmentId property
        /// </summary>
        [DataMember]
        public long DepartmentId
        {
            get { return _DepartmentId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _DepartmentId); }
        }

        /// <summary>
        /// StoreroomId property
        /// </summary>
        [DataMember]
        public long StoreroomId
        {
            get { return _StoreroomId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _StoreroomId); }
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
