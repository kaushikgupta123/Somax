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
    /// Business object that stores a record from the AlertUser table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class AlertUser : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public AlertUser()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_AlertUser dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.AlertUserId = dbObj.AlertUserId;
            this.UserId = dbObj.UserId;
            this.AlertsId = dbObj.AlertsId;
            this.ActiveDate = dbObj.ActiveDate;
            this.Permission = dbObj.Permission;
            this.IsRead = dbObj.IsRead;
            this.IsDeleted = dbObj.IsDeleted;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_AlertUser dbObj = new b_AlertUser();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_AlertUser ToDatabaseObject()
        {
            b_AlertUser dbObj = new b_AlertUser();
            dbObj.ClientId = this.ClientId;
            dbObj.AlertUserId = this.AlertUserId;
            dbObj.UserId = this.UserId;
            dbObj.AlertsId = this.AlertsId;
            dbObj.ActiveDate = this.ActiveDate;
            dbObj.Permission = this.Permission;
            dbObj.IsRead = this.IsRead;
            dbObj.IsDeleted = this.IsDeleted;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            AlertUser_Create trans = new AlertUser_Create();
            trans.AlertUser = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.AlertUser);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            AlertUser_Retrieve trans = new AlertUser_Retrieve();
            trans.AlertUser = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.AlertUser);
        }

        public void Update(DatabaseKey dbKey) 
        {
            AlertUser_Update trans = new AlertUser_Update();
            trans.AlertUser = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.AlertUser);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            AlertUser_Delete trans = new AlertUser_Delete();
            trans.AlertUser = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.AlertUserId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _AlertUserId;
        private long _UserId;
        private long _AlertsId;
        private DateTime _ActiveDate;
        private string _Permission;
        private bool _IsRead;
        private bool _IsDeleted;
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
        /// AlertUserId property
        /// </summary>
        [DataMember]
        public long AlertUserId
        {
            get { return _AlertUserId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AlertUserId); }
        }

        /// <summary>
        /// UserId property
        /// </summary>
        [DataMember]
        public long UserId
        {
            get { return _UserId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _UserId); }
        }

        /// <summary>
        /// AlertsId property
        /// </summary>
        [DataMember]
        public long AlertsId
        {
            get { return _AlertsId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AlertsId); }
        }

        /// <summary>
        /// ActiveDate property
        /// </summary>
        [DataMember]
        public DateTime ActiveDate
        {
            get { return _ActiveDate; }
            set { Set<DateTime>(MethodBase.GetCurrentMethod().Name, value, ref _ActiveDate); }
        }

        /// <summary>
        /// Permission property
        /// </summary>
        [DataMember]
        public string Permission
        {
            get { return _Permission; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Permission); }
        }

        /// <summary>
        /// IsRead property
        /// </summary>
        [DataMember]
        public bool IsRead
        {
            get { return _IsRead; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _IsRead); }
        }

        /// <summary>
        /// IsDeleted property
        /// </summary>
        [DataMember]
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _IsDeleted); }
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
