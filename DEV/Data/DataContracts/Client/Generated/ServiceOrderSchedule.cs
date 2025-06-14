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
    /// Business object that stores a record from the ServiceOrderSchedule table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class ServiceOrderSchedule : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ServiceOrderSchedule()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_ServiceOrderSchedule dbObj)
        {
		            this.ServiceOrderScheduleId = dbObj.ServiceOrderScheduleId;
            this.ClientId = dbObj.ClientId;
            this.ServiceOrderId = dbObj.ServiceOrderId;
            this.PersonnelId = dbObj.PersonnelId;
            this.ScheduledDate = dbObj.ScheduledDate;
            this.ScheduledHours = dbObj.ScheduledHours;
            this.Shift = dbObj.Shift;
            this.Del = dbObj.Del;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_ServiceOrderSchedule dbObj = new b_ServiceOrderSchedule();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_ServiceOrderSchedule ToDatabaseObject()
        {
            b_ServiceOrderSchedule dbObj = new b_ServiceOrderSchedule();
            dbObj.ServiceOrderScheduleId = this.ServiceOrderScheduleId;
            dbObj.ClientId = this.ClientId;
            dbObj.ServiceOrderId = this.ServiceOrderId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ScheduledDate = this.ScheduledDate;
            dbObj.ScheduledHours = this.ScheduledHours;
            dbObj.Shift = this.Shift;
            dbObj.Del = this.Del;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            ServiceOrderSchedule_Create trans = new ServiceOrderSchedule_Create();
            trans.ServiceOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ServiceOrderSchedule);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            ServiceOrderSchedule_Retrieve trans = new ServiceOrderSchedule_Retrieve();
            trans.ServiceOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.ServiceOrderSchedule);
        }

        public void Update(DatabaseKey dbKey) 
        {
            ServiceOrderSchedule_Update trans = new ServiceOrderSchedule_Update();
            trans.ServiceOrderSchedule = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.ServiceOrderSchedule);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            ServiceOrderSchedule_Delete trans = new ServiceOrderSchedule_Delete();
            trans.ServiceOrderSchedule = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.ServiceOrderScheduleId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ServiceOrderScheduleId;
        private long _ClientId;
        private long _ServiceOrderId;
        private long _PersonnelId;
        private DateTime? _ScheduledDate;
        private decimal _ScheduledHours;
        private string _Shift;
        private bool _Del;
        #endregion
        
        #region Properties


        /// <summary>
        /// ServiceOrderScheduleId property
        /// </summary>
        [DataMember]
        public long ServiceOrderScheduleId
        {
            get { return _ServiceOrderScheduleId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ServiceOrderScheduleId); }
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
        /// ServiceOrderId property
        /// </summary>
        [DataMember]
        public long ServiceOrderId
        {
            get { return _ServiceOrderId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _ServiceOrderId); }
        }

        /// <summary>
        /// PersonnelId property
        /// </summary>
        [DataMember]
        public long PersonnelId
        {
            get { return _PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PersonnelId); }
        }

        /// <summary>
        /// ScheduledDate property
        /// </summary>
        [DataMember]
        public DateTime? ScheduledDate
        {
            get { return _ScheduledDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _ScheduledDate); }
        }

        /// <summary>
        /// ScheduledHours property
        /// </summary>
        [DataMember]
        public decimal ScheduledHours
        {
            get { return _ScheduledHours; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _ScheduledHours); }
        }

        /// <summary>
        /// Shift property
        /// </summary>
        [DataMember]
        public string Shift
        {
            get { return _Shift; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Shift); }
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
        #endregion
		
		
    }
}
