/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2021 by SOMAX Inc.
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
    /// Business object that stores a record from the Metrics table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class Metrics : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Metrics()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_Metrics dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.MetricsId = dbObj.MetricsId;
            this.SiteId = dbObj.SiteId;
            this.MetricName = dbObj.MetricName;
            this.MetricValue = dbObj.MetricValue;
            this.RunDate = dbObj.RunDate;
            this.DataDate = dbObj.DataDate;
            this.WeekNumber = dbObj.WeekNumber;

      // Turn on auditing
      AuditEnabled = true;
		}

        private void Initialize()
        {
            b_Metrics dbObj = new b_Metrics();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_Metrics ToDatabaseObject()
        {
            b_Metrics dbObj = new b_Metrics();
            dbObj.ClientId = this.ClientId;
            dbObj.MetricsId = this.MetricsId;
            dbObj.SiteId = this.SiteId;
            dbObj.MetricName = this.MetricName;
            dbObj.MetricValue = this.MetricValue;
            dbObj.RunDate = this.RunDate;
            dbObj.DataDate = this.DataDate;
            dbObj.WeekNumber = this.WeekNumber;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            Metrics_Create trans = new Metrics_Create();
            trans.Metrics = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Metrics);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            Metrics_Retrieve trans = new Metrics_Retrieve();
            trans.Metrics = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Metrics);
        }

        public void Update(DatabaseKey dbKey) 
        {
            Metrics_Update trans = new Metrics_Update();
            trans.Metrics = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Metrics);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            Metrics_Delete trans = new Metrics_Delete();
            trans.Metrics = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.MetricsId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _MetricsId;
        private long _SiteId;
        private string _MetricName;
        private decimal _MetricValue;
        private DateTime _RunDate;
        private DateTime _DataDate;
        private int? _WeekNumber;
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
        /// MetricsId property
        /// </summary>
        [DataMember]
        public long MetricsId
        {
            get { return _MetricsId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _MetricsId); }
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
        /// MetricName property
        /// </summary>
        [DataMember]
        public string MetricName
        {
            get { return _MetricName; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _MetricName); }
        }

        /// <summary>
        /// MetricValue property
        /// </summary>
        [DataMember]
        public decimal MetricValue
        {
            get { return _MetricValue; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _MetricValue); }
        }

        /// <summary>
        /// RunDate property
        /// </summary>
        [DataMember]
        public DateTime RunDate
        {
            get { return _RunDate; }
            set { Set<DateTime>(MethodBase.GetCurrentMethod().Name, value, ref _RunDate); }
        }

        /// <summary>
        /// DataDate property
        /// </summary>
        [DataMember]
        public DateTime DataDate
        {
            get { return _DataDate; }
            set { Set<DateTime>(MethodBase.GetCurrentMethod().Name, value, ref _DataDate); }
        }

        /// <summary>
        /// WeekNumber property
        /// </summary>
        [DataMember]
        public int? WeekNumber
        {
            get { return _WeekNumber; }
            set { Set<int?>(MethodBase.GetCurrentMethod().Name, value, ref _WeekNumber); }
        }
        #endregion
		
		
    }
}
