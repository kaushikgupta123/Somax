/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2017 by SOMAX Inc.
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
    /// Business object that stores a record from the SensorReading table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class SensorReading : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SensorReading()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_SensorReading dbObj)
        {
		            this.ClientId = dbObj.ClientId;
            this.SensorReadingId = dbObj.SensorReadingId;
            this.SiteId = dbObj.SiteId;
            this.SensorID = dbObj.SensorID;
            this.NetworkId = dbObj.NetworkId;
            this.MessageGUID = dbObj.MessageGUID;
            this.MessageState = dbObj.MessageState;
            this.MessageDate = dbObj.MessageDate;
            this.RawData = dbObj.RawData;
            this.DataType = dbObj.DataType;
            this.DataValue = dbObj.DataValue;
            this.PlotValues = dbObj.PlotValues;
            this.PlotLabels = dbObj.PlotLabels;
            this.BatteryLevel = dbObj.BatteryLevel;
            this.SignalStrength = dbObj.SignalStrength;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_SensorReading dbObj = new b_SensorReading();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_SensorReading ToDatabaseObject()
        {
            b_SensorReading dbObj = new b_SensorReading();
            dbObj.ClientId = this.ClientId;
            dbObj.SensorReadingId = this.SensorReadingId;
            dbObj.SiteId = this.SiteId;
            dbObj.SensorID = this.SensorID;
            dbObj.NetworkId = this.NetworkId;
            dbObj.MessageGUID = this.MessageGUID;
            dbObj.MessageState = this.MessageState;
            dbObj.MessageDate = this.MessageDate;
            dbObj.RawData = this.RawData;
            dbObj.DataType = this.DataType;
            dbObj.DataValue = this.DataValue;
            dbObj.PlotValues = this.PlotValues;
            dbObj.PlotLabels = this.PlotLabels;
            dbObj.BatteryLevel = this.BatteryLevel;
            dbObj.SignalStrength = this.SignalStrength;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            SensorReading_Create trans = new SensorReading_Create();
            trans.SensorReading = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SensorReading);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            SensorReading_Retrieve trans = new SensorReading_Retrieve();
            trans.SensorReading = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.SensorReading);
        }

        public void Update(DatabaseKey dbKey) 
        {
            SensorReading_Update trans = new SensorReading_Update();
            trans.SensorReading = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.SensorReading);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            SensorReading_Delete trans = new SensorReading_Delete();
            trans.SensorReading = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.SensorReadingId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _ClientId;
        private long _SensorReadingId;
        private long _SiteId;
        private int _SensorID;
        private int _NetworkId;
        private Guid _MessageGUID;
        private int _MessageState;
        private DateTime? _MessageDate;
        private string _RawData;
        private string _DataType;
        private string _DataValue;
        private decimal _PlotValues;
        private string _PlotLabels;
        private int _BatteryLevel;
        private int _SignalStrength;
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
        /// SensorReadingId property
        /// </summary>
        [DataMember]
        public long SensorReadingId
        {
            get { return _SensorReadingId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SensorReadingId); }
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
        /// SensorID property
        /// </summary>
        [DataMember]
        public int SensorID
        {
            get { return _SensorID; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _SensorID); }
        }

        /// <summary>
        /// NetworkId property
        /// </summary>
        [DataMember]
        public int NetworkId
        {
            get { return _NetworkId; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _NetworkId); }
        }

        /// <summary>
        /// MessageGUID property
        /// </summary>
        [DataMember]
        public Guid MessageGUID
        {
            get { return _MessageGUID; }
            set { Set<Guid>(MethodBase.GetCurrentMethod().Name, value, ref _MessageGUID); }
        }

        /// <summary>
        /// MessageState property
        /// </summary>
        [DataMember]
        public int MessageState
        {
            get { return _MessageState; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _MessageState); }
        }

        /// <summary>
        /// MessageDate property
        /// </summary>
        [DataMember]
        public DateTime? MessageDate
        {
            get { return _MessageDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _MessageDate); }
        }

        /// <summary>
        /// RawData property
        /// </summary>
        [DataMember]
        public string RawData
        {
            get { return _RawData; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _RawData); }
        }

        /// <summary>
        /// DataType property
        /// </summary>
        [DataMember]
        public string DataType
        {
            get { return _DataType; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _DataType); }
        }

        /// <summary>
        /// DataValue property
        /// </summary>
        [DataMember]
        public string DataValue
        {
            get { return _DataValue; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _DataValue); }
        }

        /// <summary>
        /// PlotValues property
        /// </summary>
        [DataMember]
        public decimal PlotValues
        {
            get { return _PlotValues; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _PlotValues); }
        }

        /// <summary>
        /// PlotLabels property
        /// </summary>
        [DataMember]
        public string PlotLabels
        {
            get { return _PlotLabels; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PlotLabels); }
        }

        /// <summary>
        /// BatteryLevel property
        /// </summary>
        [DataMember]
        public int BatteryLevel
        {
            get { return _BatteryLevel; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _BatteryLevel); }
        }

        /// <summary>
        /// SignalStrength property
        /// </summary>
        [DataMember]
        public int SignalStrength
        {
            get { return _SignalStrength; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _SignalStrength); }
        }
        #endregion
		
		
    }
}
