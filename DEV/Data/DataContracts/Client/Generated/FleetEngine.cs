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
    /// Business object that stores a record from the FleetEngine table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class FleetEngine : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public FleetEngine()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_FleetEngine dbObj)
        {
		            this.FleetEngineId = dbObj.FleetEngineId;
            this.ClientId = dbObj.ClientId;
            this.EquipmentId = dbObj.EquipmentId;
            this.EngineBrand = dbObj.EngineBrand;
            this.Aspiration = dbObj.Aspiration;
            this.Bore = dbObj.Bore;
            this.Cam = dbObj.Cam;
            this.Compression = dbObj.Compression;
            this.Cylinders = dbObj.Cylinders;
            this.Displacement = dbObj.Displacement;
            this.FuelInduction = dbObj.FuelInduction;
            this.FuelQuality = dbObj.FuelQuality;
            this.MaxHP = dbObj.MaxHP;
            this.MaxTorque = dbObj.MaxTorque;
            this.RedlineRPM = dbObj.RedlineRPM;
            this.Stroke = dbObj.Stroke;
            this.Valves = dbObj.Valves;
            this.TransmissionBrand = dbObj.TransmissionBrand;
            this.TransmissionType = dbObj.TransmissionType;
            this.Gears = dbObj.Gears;
            this.UpdateIndex = dbObj.UpdateIndex;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_FleetEngine dbObj = new b_FleetEngine();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_FleetEngine ToDatabaseObject()
        {
            b_FleetEngine dbObj = new b_FleetEngine();
            dbObj.FleetEngineId = this.FleetEngineId;
            dbObj.ClientId = this.ClientId;
            dbObj.EquipmentId = this.EquipmentId;
            dbObj.EngineBrand = this.EngineBrand;
            dbObj.Aspiration = this.Aspiration;
            dbObj.Bore = this.Bore;
            dbObj.Cam = this.Cam;
            dbObj.Compression = this.Compression;
            dbObj.Cylinders = this.Cylinders;
            dbObj.Displacement = this.Displacement;
            dbObj.FuelInduction = this.FuelInduction;
            dbObj.FuelQuality = this.FuelQuality;
            dbObj.MaxHP = this.MaxHP;
            dbObj.MaxTorque = this.MaxTorque;
            dbObj.RedlineRPM = this.RedlineRPM;
            dbObj.Stroke = this.Stroke;
            dbObj.Valves = this.Valves;
            dbObj.TransmissionBrand = this.TransmissionBrand;
            dbObj.TransmissionType = this.TransmissionType;
            dbObj.Gears = this.Gears;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            FleetEngine_Create trans = new FleetEngine_Create();
            trans.FleetEngine = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.FleetEngine);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            FleetEngine_Retrieve trans = new FleetEngine_Retrieve();
            trans.FleetEngine = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.FleetEngine);
        }

        public void Update(DatabaseKey dbKey) 
        {
            FleetEngine_Update trans = new FleetEngine_Update();
            trans.FleetEngine = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.FleetEngine);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            FleetEngine_Delete trans = new FleetEngine_Delete();
            trans.FleetEngine = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.FleetEngineId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _FleetEngineId;
        private long _ClientId;
        private long _EquipmentId;
        private string _EngineBrand;
        private string _Aspiration;
        private decimal _Bore;
        private string _Cam;
        private decimal _Compression;
        private decimal _Cylinders;
        private decimal _Displacement;
        private string _FuelInduction;
        private decimal _FuelQuality;
        private decimal _MaxHP;
        private decimal _MaxTorque;
        private decimal _RedlineRPM;
        private decimal _Stroke;
        private decimal _Valves;
        private string _TransmissionBrand;
        private string _TransmissionType;
        private decimal _Gears;
        private int _UpdateIndex;
        #endregion
        
        #region Properties


        /// <summary>
        /// FleetEngineId property
        /// </summary>
        [DataMember]
        public long FleetEngineId
        {
            get { return _FleetEngineId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _FleetEngineId); }
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
        /// EquipmentId property
        /// </summary>
        [DataMember]
        public long EquipmentId
        {
            get { return _EquipmentId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _EquipmentId); }
        }

        /// <summary>
        /// EngineBrand property
        /// </summary>
        [DataMember]
        public string EngineBrand
        {
            get { return _EngineBrand; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _EngineBrand); }
        }

        /// <summary>
        /// Aspiration property
        /// </summary>
        [DataMember]
        public string Aspiration
        {
            get { return _Aspiration; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Aspiration); }
        }

        /// <summary>
        /// Bore property
        /// </summary>
        [DataMember]
        public decimal Bore
        {
            get { return _Bore; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Bore); }
        }

        /// <summary>
        /// Cam property
        /// </summary>
        [DataMember]
        public string Cam
        {
            get { return _Cam; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Cam); }
        }

        /// <summary>
        /// Compression property
        /// </summary>
        [DataMember]
        public decimal Compression
        {
            get { return _Compression; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Compression); }
        }

        /// <summary>
        /// Cylinders property
        /// </summary>
        [DataMember]
        public decimal Cylinders
        {
            get { return _Cylinders; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Cylinders); }
        }

        /// <summary>
        /// Displacement property
        /// </summary>
        [DataMember]
        public decimal Displacement
        {
            get { return _Displacement; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Displacement); }
        }

        /// <summary>
        /// FuelInduction property
        /// </summary>
        [DataMember]
        public string FuelInduction
        {
            get { return _FuelInduction; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _FuelInduction); }
        }

        /// <summary>
        /// FuelQuality property
        /// </summary>
        [DataMember]
        public decimal FuelQuality
        {
            get { return _FuelQuality; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _FuelQuality); }
        }

        /// <summary>
        /// MaxHP property
        /// </summary>
        [DataMember]
        public decimal MaxHP
        {
            get { return _MaxHP; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _MaxHP); }
        }

        /// <summary>
        /// MaxTorque property
        /// </summary>
        [DataMember]
        public decimal MaxTorque
        {
            get { return _MaxTorque; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _MaxTorque); }
        }

        /// <summary>
        /// RedlineRPM property
        /// </summary>
        [DataMember]
        public decimal RedlineRPM
        {
            get { return _RedlineRPM; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _RedlineRPM); }
        }

        /// <summary>
        /// Stroke property
        /// </summary>
        [DataMember]
        public decimal Stroke
        {
            get { return _Stroke; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Stroke); }
        }

        /// <summary>
        /// Valves property
        /// </summary>
        [DataMember]
        public decimal Valves
        {
            get { return _Valves; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Valves); }
        }

        /// <summary>
        /// TransmissionBrand property
        /// </summary>
        [DataMember]
        public string TransmissionBrand
        {
            get { return _TransmissionBrand; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _TransmissionBrand); }
        }

        /// <summary>
        /// TransmissionType property
        /// </summary>
        [DataMember]
        public string TransmissionType
        {
            get { return _TransmissionType; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _TransmissionType); }
        }

        /// <summary>
        /// Gears property
        /// </summary>
        [DataMember]
        public decimal Gears
        {
            get { return _Gears; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _Gears); }
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
