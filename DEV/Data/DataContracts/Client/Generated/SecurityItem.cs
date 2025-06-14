/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2020 by SOMAX Inc.
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
    /// Business object that stores a record from the SecurityItem table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class SecurityItem : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SecurityItem()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_SecurityItem dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.SecurityItemId = dbObj.SecurityItemId;
            this.SecurityProfileId = dbObj.SecurityProfileId;
            this.ItemName = dbObj.ItemName;
            this.SortOrder = dbObj.SortOrder;
            this.SingleItem = dbObj.SingleItem;
            this.ItemAccess = dbObj.ItemAccess;
            this.ItemCreate = dbObj.ItemCreate;
            this.ItemEdit = dbObj.ItemEdit;
            this.ItemDelete = dbObj.ItemDelete;
            this.ReportItem = dbObj.ReportItem;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_SecurityItem dbObj = new b_SecurityItem();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_SecurityItem ToDatabaseObject()
        {
            b_SecurityItem dbObj = new b_SecurityItem();
            dbObj.ClientId = this.ClientId;
            dbObj.SecurityItemId = this.SecurityItemId;
            dbObj.SecurityProfileId = this.SecurityProfileId;
            dbObj.ItemName = this.ItemName;
            dbObj.SortOrder = this.SortOrder;
            dbObj.SingleItem = this.SingleItem;
            dbObj.ItemAccess = this.ItemAccess;
            dbObj.ItemCreate = this.ItemCreate;
            dbObj.ItemEdit = this.ItemEdit;
            dbObj.ItemDelete = this.ItemDelete;
            dbObj.ReportItem = this.ReportItem;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            SecurityItem_Create trans = new SecurityItem_Create();
            trans.SecurityItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SecurityItem);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            SecurityItem_Retrieve trans = new SecurityItem_Retrieve();
            trans.SecurityItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.SecurityItem);
        }

        public void Update(DatabaseKey dbKey) 
        {
            SecurityItem_Update trans = new SecurityItem_Update();
            trans.SecurityItem = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SecurityItem);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            SecurityItem_Delete trans = new SecurityItem_Delete();
            trans.SecurityItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.SecurityItemId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _SecurityItemId;
        private long _SecurityProfileId;
        private string _ItemName;
        private int _SortOrder;
        private bool _SingleItem;
        private bool _ItemAccess;
        private bool _ItemCreate;
        private bool _ItemEdit;
        private bool _ItemDelete;
        private bool _ReportItem;
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
        /// SecurityItemId property
        /// </summary>
        [DataMember]
        public long SecurityItemId
        {
            get { return _SecurityItemId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SecurityItemId); }
        }

        /// <summary>
        /// SecurityProfileId property
        /// </summary>
        [DataMember]
        public long SecurityProfileId
        {
            get { return _SecurityProfileId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SecurityProfileId); }
        }

        /// <summary>
        /// ItemName property
        /// </summary>
        [DataMember]
        public string ItemName
        {
            get { return _ItemName; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ItemName); }
        }

        /// <summary>
        /// SortOrder property
        /// </summary>
        [DataMember]
        public int SortOrder
        {
            get { return _SortOrder; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _SortOrder); }
        }

        /// <summary>
        /// SingleItem property
        /// </summary>
        [DataMember]
        public bool SingleItem
        {
            get { return _SingleItem; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _SingleItem); }
        }

        /// <summary>
        /// ItemAccess property
        /// </summary>
        [DataMember]
        public bool ItemAccess
        {
            get { return _ItemAccess; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _ItemAccess); }
        }

        /// <summary>
        /// ItemCreate property
        /// </summary>
        [DataMember]
        public bool ItemCreate
        {
            get { return _ItemCreate; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _ItemCreate); }
        }

        /// <summary>
        /// ItemEdit property
        /// </summary>
        [DataMember]
        public bool ItemEdit
        {
            get { return _ItemEdit; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _ItemEdit); }
        }

        /// <summary>
        /// ItemDelete property
        /// </summary>
        [DataMember]
        public bool ItemDelete
        {
            get { return _ItemDelete; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _ItemDelete); }
        }

        /// <summary>
        /// ReportItem property
        /// </summary>
        [DataMember]
        public bool ReportItem
        {
            get { return _ReportItem; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _ReportItem); }
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
