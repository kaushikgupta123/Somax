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
    /// Business object that stores a record from the UIConfig table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class UIConfig : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public UIConfig()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_UIConfig dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.UIConfigId = dbObj.UIConfigId;
            this.SiteId = dbObj.SiteId;
            this.ViewName = dbObj.ViewName;
            this.TableName = dbObj.TableName;
            this.ColumnName = dbObj.ColumnName;
            this.Required = dbObj.Required;
            this.Hide = dbObj.Hide;
            this.IsExternal = dbObj.IsExternal;
            this.Disable = dbObj.Disable;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_UIConfig dbObj = new b_UIConfig();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_UIConfig ToDatabaseObject()
        {
            b_UIConfig dbObj = new b_UIConfig();
            dbObj.ClientId = this.ClientId;
            dbObj.UIConfigId = this.UIConfigId;
            dbObj.SiteId = this.SiteId;
            dbObj.ViewName = this.ViewName;
            dbObj.TableName = this.TableName;
            dbObj.ColumnName = this.ColumnName;
            dbObj.Required = this.Required;
            dbObj.Hide = this.Hide;
            dbObj.IsExternal = this.IsExternal;
            dbObj.Disable = this.Disable;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            UIConfig_Create trans = new UIConfig_Create();
            trans.UIConfig = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.UIConfig);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            UIConfig_Retrieve trans = new UIConfig_Retrieve();
            trans.UIConfig = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.UIConfig);
        }

        public void Update(DatabaseKey dbKey) 
        {
            UIConfig_Update trans = new UIConfig_Update();
            trans.UIConfig = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.UIConfig);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            UIConfig_Delete trans = new UIConfig_Delete();
            trans.UIConfig = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.UIConfigId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _UIConfigId;
        private long _SiteId;
        private string _ViewName;
        private string _TableName;
        private string _ColumnName;
        private bool _Required;
        private bool _Hide;
        private bool _IsExternal;
        private bool _Disable;
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
        /// UIConfigId property
        /// </summary>
        [DataMember]
        public long UIConfigId
        {
            get { return _UIConfigId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _UIConfigId); }
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
        /// ViewName property
        /// </summary>
        [DataMember]
        public string ViewName
        {
            get { return _ViewName; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ViewName); }
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
        /// ColumnName property
        /// </summary>
        [DataMember]
        public string ColumnName
        {
            get { return _ColumnName; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ColumnName); }
        }

        /// <summary>
        /// Required property
        /// </summary>
        [DataMember]
        public bool Required
        {
            get { return _Required; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Required); }
        }

        /// <summary>
        /// Hide property
        /// </summary>
        [DataMember]
        public bool Hide
        {
            get { return _Hide; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Hide); }
        }

        /// <summary>
        /// IsExternal property
        /// </summary>
        [DataMember]
        public bool IsExternal
        {
            get { return _IsExternal; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _IsExternal); }
        }

        /// <summary>
        /// Disable property
        /// </summary>
        [DataMember]
        public bool Disable
        {
            get { return _Disable; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Disable); }
        }
        #endregion
		
		
    }
}
