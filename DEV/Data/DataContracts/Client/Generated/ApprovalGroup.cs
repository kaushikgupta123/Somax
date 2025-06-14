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
    /// Business object that stores a record from the ApprovalGroup table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class ApprovalGroup : DataContractBase, IPermission 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ApprovalGroup()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_ApprovalGroup dbObj)
        {
		            this.ApprovalGroupId = dbObj.ApprovalGroupId;
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.Description = dbObj.Description;
            this.RequestType = dbObj.RequestType;
            this.AssetGroup1 = dbObj.AssetGroup1;
            this.AssetGroup2 = dbObj.AssetGroup2;
            this.AssetGroup3 = dbObj.AssetGroup3;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_ApprovalGroup dbObj = new b_ApprovalGroup();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_ApprovalGroup ToDatabaseObject()
        {
            b_ApprovalGroup dbObj = new b_ApprovalGroup();
            dbObj.ApprovalGroupId = this.ApprovalGroupId;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.AreaId = this.AreaId;
            dbObj.DepartmentId = this.DepartmentId;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.Description = this.Description;
            dbObj.RequestType = this.RequestType;
            dbObj.AssetGroup1 = this.AssetGroup1;
            dbObj.AssetGroup2 = this.AssetGroup2;
            dbObj.AssetGroup3 = this.AssetGroup3;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            ApprovalGroup_Create trans = new ApprovalGroup_Create();
            trans.ApprovalGroup = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ApprovalGroup);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            ApprovalGroup_Retrieve trans = new ApprovalGroup_Retrieve();
            trans.ApprovalGroup = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.ApprovalGroup);
        }

        public void Update(DatabaseKey dbKey) 
        {
            ApprovalGroup_Update trans = new ApprovalGroup_Update();
            trans.ApprovalGroup = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ApprovalGroup);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            ApprovalGroup_Delete trans = new ApprovalGroup_Delete();
            trans.ApprovalGroup = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.ApprovalGroupId;
			AuditTargetSiteId = this.SiteId;
			AuditTargetAreaId = this.AreaId;
			AuditTargetDepartmentId = this.DepartmentId;
			AuditTargetStoreRoomId = this.StoreroomId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ApprovalGroupId;
        private long _ClientId;
        private long _SiteId;
        private long _AreaId;
        private long _DepartmentId;
        private long _StoreroomId;
        private string _Description;
        private string _RequestType;
        private long _AssetGroup1;
        private long _AssetGroup2;
        private long _AssetGroup3;
        #endregion
        
        #region Properties


        /// <summary>
        /// ApprovalGroupId property
        /// </summary>
        [DataMember]
        public long ApprovalGroupId
        {
            get { return _ApprovalGroupId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ApprovalGroupId); }
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
        /// Description property
        /// </summary>
        [DataMember]
        public string Description
        {
            get { return _Description; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Description); }
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
        /// AssetGroup1 property
        /// </summary>
        [DataMember]
        public long AssetGroup1
        {
            get { return _AssetGroup1; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AssetGroup1); }
        }

        /// <summary>
        /// AssetGroup2 property
        /// </summary>
        [DataMember]
        public long AssetGroup2
        {
            get { return _AssetGroup2; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AssetGroup2); }
        }

        /// <summary>
        /// AssetGroup3 property
        /// </summary>
        [DataMember]
        public long AssetGroup3
        {
            get { return _AssetGroup3; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AssetGroup3); }
        }
        #endregion
		
		
    }
}
