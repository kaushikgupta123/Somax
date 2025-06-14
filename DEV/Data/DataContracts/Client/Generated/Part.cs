/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2019 by SOMAX Inc.
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
    /// Business object that stores a record from the Part table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class Part : DataContractBase, IPermission
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Part()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_Part dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PartId = dbObj.PartId;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.ABCCode = dbObj.ABCCode;
            this.ABCStoreCost = dbObj.ABCStoreCost;
            this.AccountId = dbObj.AccountId;
            this.AltPartId1 = dbObj.AltPartId1;
            this.AltPartId2 = dbObj.AltPartId2;
            this.AltPartId3 = dbObj.AltPartId3;
            this.AppliedCost = dbObj.AppliedCost;
            this.AverageCost = dbObj.AverageCost;
            this.Consignment = dbObj.Consignment;
            this.CostCalcMethod = dbObj.CostCalcMethod;
            this.CostMultiplier = dbObj.CostMultiplier;
            this.Critical = dbObj.Critical;
            this.Description = dbObj.Description;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.IssueUnit = dbObj.IssueUnit;
            this.Manufacturer = dbObj.Manufacturer;
            this.ManufacturerId = dbObj.ManufacturerId;
            this.MSDSContainerCode = dbObj.MSDSContainerCode;
            this.MSDSPressureCode = dbObj.MSDSPressureCode;
            this.MSDSReference = dbObj.MSDSReference;
            this.MSDSRequired = dbObj.MSDSRequired;
            this.MSDSTemperatureCode = dbObj.MSDSTemperatureCode;
            this.NoEquipXref = dbObj.NoEquipXref;
            this.PrintNoLabel = dbObj.PrintNoLabel;
            this.PurchaseText = dbObj.PurchaseText;
            this.RepairablePart = dbObj.RepairablePart;
            this.StockType = dbObj.StockType;
            this.TaxLevel1 = dbObj.TaxLevel1;
            this.TaxLevel2 = dbObj.TaxLevel2;
            this.Taxable = dbObj.Taxable;
            this.Tool = dbObj.Tool;
            this.Type = dbObj.Type;
            this.UPCCode = dbObj.UPCCode;
            this.UseCostMultiplier = dbObj.UseCostMultiplier;
            this.Chemical = dbObj.Chemical;
            this.AutoPurch = dbObj.AutoPurch;
            this.PartMasterId = dbObj.PartMasterId;
            this.PrevClientLookupId = dbObj.PrevClientLookupId;
            this.DefaultStoreroom = dbObj.DefaultStoreroom;
            this.UpdateIndex = dbObj.UpdateIndex;

            // Turn on auditing
            AuditEnabled = true;
        }

        private void Initialize()
        {
            b_Part dbObj = new b_Part();
            UpdateFromDatabaseObject(dbObj);

            // Turn off auditing for object initialization
            AuditEnabled = false;
        }

        public b_Part ToDatabaseObject()
        {
            b_Part dbObj = new b_Part();
            dbObj.ClientId = this.ClientId;
            dbObj.PartId = this.PartId;
            dbObj.SiteId = this.SiteId;
            dbObj.AreaId = this.AreaId;
            dbObj.DepartmentId = this.DepartmentId;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.ABCCode = this.ABCCode;
            dbObj.ABCStoreCost = this.ABCStoreCost;
            dbObj.AccountId = this.AccountId;
            dbObj.AltPartId1 = this.AltPartId1;
            dbObj.AltPartId2 = this.AltPartId2;
            dbObj.AltPartId3 = this.AltPartId3;
            dbObj.AppliedCost = this.AppliedCost;
            dbObj.AverageCost = this.AverageCost;
            dbObj.Consignment = this.Consignment;
            dbObj.CostCalcMethod = this.CostCalcMethod;
            dbObj.CostMultiplier = this.CostMultiplier;
            dbObj.Critical = this.Critical;
            dbObj.Description = this.Description;
            dbObj.InactiveFlag = this.InactiveFlag;
            dbObj.IssueUnit = this.IssueUnit;
            dbObj.Manufacturer = this.Manufacturer;
            dbObj.ManufacturerId = this.ManufacturerId;
            dbObj.MSDSContainerCode = this.MSDSContainerCode;
            dbObj.MSDSPressureCode = this.MSDSPressureCode;
            dbObj.MSDSReference = this.MSDSReference;
            dbObj.MSDSRequired = this.MSDSRequired;
            dbObj.MSDSTemperatureCode = this.MSDSTemperatureCode;
            dbObj.NoEquipXref = this.NoEquipXref;
            dbObj.PrintNoLabel = this.PrintNoLabel;
            dbObj.PurchaseText = this.PurchaseText;
            dbObj.RepairablePart = this.RepairablePart;
            dbObj.StockType = this.StockType;
            dbObj.TaxLevel1 = this.TaxLevel1;
            dbObj.TaxLevel2 = this.TaxLevel2;
            dbObj.Taxable = this.Taxable;
            dbObj.Tool = this.Tool;
            dbObj.Type = this.Type;
            dbObj.UPCCode = this.UPCCode;
            dbObj.UseCostMultiplier = this.UseCostMultiplier;
            dbObj.Chemical = this.Chemical;
            dbObj.AutoPurch = this.AutoPurch;
            dbObj.PartMasterId = this.PartMasterId;
            dbObj.PrevClientLookupId = this.PrevClientLookupId;
            dbObj.DefaultStoreroom = this.DefaultStoreroom;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }

        #endregion

        #region Transaction Methods

        public void Create(DatabaseKey dbKey)
        {
            Part_Create trans = new Part_Create();
            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Part);
        }

        public void Retrieve(DatabaseKey dbKey)
        {
            Part_Retrieve trans = new Part_Retrieve();
            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Part);
        }

        public void Update(DatabaseKey dbKey)
        {
            Part_Update trans = new Part_Update();
            trans.Part = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Part);
        }

        public void Delete(DatabaseKey dbKey)
        {
            Part_Delete trans = new Part_Delete();
            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PartId;
            AuditTargetSiteId = this.SiteId;
            AuditTargetAreaId = this.AreaId;
            AuditTargetDepartmentId = this.DepartmentId;
            AuditTargetStoreRoomId = this.StoreroomId;
            return BuildChangeLogDbObject(dbKey);
        }

        #endregion

        #region Private Variables

        private long _ClientId;
        private long _PartId;
        private long _SiteId;
        private long _AreaId;
        private long _DepartmentId;
        private long _StoreroomId;
        private string _ClientLookupId;
        private string _ABCCode;
        private string _ABCStoreCost;
        private long _AccountId;
        private long _AltPartId1;
        private long _AltPartId2;
        private long _AltPartId3;
        private decimal _AppliedCost;
        private decimal _AverageCost;
        private bool _Consignment;
        private string _CostCalcMethod;
        private decimal _CostMultiplier;
        private bool _Critical;
        private string _Description;
        private bool _InactiveFlag;
        private string _IssueUnit;
        private string _Manufacturer;
        private string _ManufacturerId;
        private string _MSDSContainerCode;
        private string _MSDSPressureCode;
        private string _MSDSReference;
        private bool _MSDSRequired;
        private string _MSDSTemperatureCode;
        private bool _NoEquipXref;
        private bool _PrintNoLabel;
        private string _PurchaseText;
        private bool _RepairablePart;
        private string _StockType;
        private decimal _TaxLevel1;
        private decimal _TaxLevel2;
        private bool _Taxable;
        private bool _Tool;
        private int _Type;
        private string _UPCCode;
        private bool _UseCostMultiplier;
        private bool _Chemical;
        private bool _AutoPurch;
        private long _PartMasterId;
        private string _PrevClientLookupId;
        private long _DefaultStoreroom;
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
        /// PartId property
        /// </summary>
        [DataMember]
        public long PartId
        {
            get { return _PartId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartId); }
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
        /// ClientLookupId property
        /// </summary>
        [DataMember]
        public string ClientLookupId
        {
            get { return _ClientLookupId; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ClientLookupId); }
        }

        /// <summary>
        /// ABCCode property
        /// </summary>
        [DataMember]
        public string ABCCode
        {
            get { return _ABCCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ABCCode); }
        }

        /// <summary>
        /// ABCStoreCost property
        /// </summary>
        [DataMember]
        public string ABCStoreCost
        {
            get { return _ABCStoreCost; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ABCStoreCost); }
        }

        /// <summary>
        /// AccountId property
        /// </summary>
        [DataMember]
        public long AccountId
        {
            get { return _AccountId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AccountId); }
        }

        /// <summary>
        /// AltPartId1 property
        /// </summary>
        [DataMember]
        public long AltPartId1
        {
            get { return _AltPartId1; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AltPartId1); }
        }

        /// <summary>
        /// AltPartId2 property
        /// </summary>
        [DataMember]
        public long AltPartId2
        {
            get { return _AltPartId2; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AltPartId2); }
        }

        /// <summary>
        /// AltPartId3 property
        /// </summary>
        [DataMember]
        public long AltPartId3
        {
            get { return _AltPartId3; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AltPartId3); }
        }

        /// <summary>
        /// AppliedCost property
        /// </summary>
        [DataMember]
        public decimal AppliedCost
        {
            get { return _AppliedCost; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _AppliedCost); }
        }

        /// <summary>
        /// AverageCost property
        /// </summary>
        [DataMember]
        public decimal AverageCost
        {
            get { return _AverageCost; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _AverageCost); }
        }

        /// <summary>
        /// Consignment property
        /// </summary>
        [DataMember]
        public bool Consignment
        {
            get { return _Consignment; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Consignment); }
        }

        /// <summary>
        /// CostCalcMethod property
        /// </summary>
        [DataMember]
        public string CostCalcMethod
        {
            get { return _CostCalcMethod; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _CostCalcMethod); }
        }

        /// <summary>
        /// CostMultiplier property
        /// </summary>
        [DataMember]
        public decimal CostMultiplier
        {
            get { return _CostMultiplier; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _CostMultiplier); }
        }

        /// <summary>
        /// Critical property
        /// </summary>
        [DataMember]
        public bool Critical
        {
            get { return _Critical; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Critical); }
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
        /// InactiveFlag property
        /// </summary>
        [DataMember]
        public bool InactiveFlag
        {
            get { return _InactiveFlag; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _InactiveFlag); }
        }

        /// <summary>
        /// IssueUnit property
        /// </summary>
        [DataMember]
        public string IssueUnit
        {
            get { return _IssueUnit; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _IssueUnit); }
        }

        /// <summary>
        /// Manufacturer property
        /// </summary>
        [DataMember]
        public string Manufacturer
        {
            get { return _Manufacturer; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Manufacturer); }
        }

        /// <summary>
        /// ManufacturerId property
        /// </summary>
        [DataMember]
        public string ManufacturerId
        {
            get { return _ManufacturerId; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ManufacturerId); }
        }

        /// <summary>
        /// MSDSContainerCode property
        /// </summary>
        [DataMember]
        public string MSDSContainerCode
        {
            get { return _MSDSContainerCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _MSDSContainerCode); }
        }

        /// <summary>
        /// MSDSPressureCode property
        /// </summary>
        [DataMember]
        public string MSDSPressureCode
        {
            get { return _MSDSPressureCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _MSDSPressureCode); }
        }

        /// <summary>
        /// MSDSReference property
        /// </summary>
        [DataMember]
        public string MSDSReference
        {
            get { return _MSDSReference; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _MSDSReference); }
        }

        /// <summary>
        /// MSDSRequired property
        /// </summary>
        [DataMember]
        public bool MSDSRequired
        {
            get { return _MSDSRequired; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _MSDSRequired); }
        }

        /// <summary>
        /// MSDSTemperatureCode property
        /// </summary>
        [DataMember]
        public string MSDSTemperatureCode
        {
            get { return _MSDSTemperatureCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _MSDSTemperatureCode); }
        }

        /// <summary>
        /// NoEquipXref property
        /// </summary>
        [DataMember]
        public bool NoEquipXref
        {
            get { return _NoEquipXref; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _NoEquipXref); }
        }

        /// <summary>
        /// PrintNoLabel property
        /// </summary>
        [DataMember]
        public bool PrintNoLabel
        {
            get { return _PrintNoLabel; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _PrintNoLabel); }
        }

        /// <summary>
        /// PurchaseText property
        /// </summary>
        [DataMember]
        public string PurchaseText
        {
            get { return _PurchaseText; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PurchaseText); }
        }

        /// <summary>
        /// RepairablePart property
        /// </summary>
        [DataMember]
        public bool RepairablePart
        {
            get { return _RepairablePart; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _RepairablePart); }
        }

        /// <summary>
        /// StockType property
        /// </summary>
        [DataMember]
        public string StockType
        {
            get { return _StockType; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _StockType); }
        }

        /// <summary>
        /// TaxLevel1 property
        /// </summary>
        [DataMember]
        public decimal TaxLevel1
        {
            get { return _TaxLevel1; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _TaxLevel1); }
        }

        /// <summary>
        /// TaxLevel2 property
        /// </summary>
        [DataMember]
        public decimal TaxLevel2
        {
            get { return _TaxLevel2; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _TaxLevel2); }
        }

        /// <summary>
        /// Taxable property
        /// </summary>
        [DataMember]
        public bool Taxable
        {
            get { return _Taxable; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Taxable); }
        }

        /// <summary>
        /// Tool property
        /// </summary>
        [DataMember]
        public bool Tool
        {
            get { return _Tool; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Tool); }
        }

        /// <summary>
        /// Type property
        /// </summary>
        [DataMember]
        public int Type
        {
            get { return _Type; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _Type); }
        }

        /// <summary>
        /// UPCCode property
        /// </summary>
        [DataMember]
        public string UPCCode
        {
            get { return _UPCCode; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _UPCCode); }
        }

        /// <summary>
        /// UseCostMultiplier property
        /// </summary>
        [DataMember]
        public bool UseCostMultiplier
        {
            get { return _UseCostMultiplier; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _UseCostMultiplier); }
        }

        /// <summary>
        /// Chemical property
        /// </summary>
        [DataMember]
        public bool Chemical
        {
            get { return _Chemical; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _Chemical); }
        }

        /// <summary>
        /// AutoPurch property
        /// </summary>
        [DataMember]
        public bool AutoPurch
        {
            get { return _AutoPurch; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _AutoPurch); }
        }

        /// <summary>
        /// PartMasterId property
        /// </summary>
        [DataMember]
        public long PartMasterId
        {
            get { return _PartMasterId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartMasterId); }
        }

        /// <summary>
        /// PrevClientLookupId property
        /// </summary>
        [DataMember]
        public string PrevClientLookupId
        {
            get { return _PrevClientLookupId; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _PrevClientLookupId); }
        }

        /// <summary>
        /// DefaultStoreroom property
        /// </summary>
        [DataMember]
        public long DefaultStoreroom
        {
            get { return _DefaultStoreroom; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _DefaultStoreroom); }
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
