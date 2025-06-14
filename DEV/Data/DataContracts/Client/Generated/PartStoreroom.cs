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
    /// Business object that stores a record from the PartStoreroom table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class PartStoreroom : DataContractBase
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public PartStoreroom()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_PartStoreroom dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PartStoreroomId = dbObj.PartStoreroomId;
            this.PartId = dbObj.PartId;
            this.StoreroomId = dbObj.StoreroomId;
            this.CountFrequency = dbObj.CountFrequency;
            this.LastCounted = dbObj.LastCounted;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.QtyOnHand = dbObj.QtyOnHand;
            this.QtyReorderLevel = dbObj.QtyReorderLevel;
            this.ReorderMethod = dbObj.ReorderMethod;
            this.LastIssueDate = dbObj.LastIssueDate;
            this.LastPurchasePrice = dbObj.LastPurchasePrice;
            this.LastPurchaseDate = dbObj.LastPurchaseDate;
            this.Location2_1 = dbObj.Location2_1;
            this.Location2_2 = dbObj.Location2_2;
            this.Location2_3 = dbObj.Location2_3;
            this.Location2_4 = dbObj.Location2_4;
            this.Location2_5 = dbObj.Location2_5;
            this.Critical = dbObj.Critical;
            this.AutoPurchase = dbObj.AutoPurchase;
            this.PartVendorId = dbObj.PartVendorId;
            this.AutoTransfer = dbObj.AutoTransfer;
            this.AutoTransferIssueStoreroom = dbObj.AutoTransferIssueStoreroom;
            this.AutoTransferMaxQty = dbObj.AutoTransferMaxQty;
            this.AutoTransferMinQty = dbObj.AutoTransferMinQty;
            this.UpdateIndex = dbObj.UpdateIndex;

            // Turn on auditing
            AuditEnabled = true;
        }

        private void Initialize()
        {
            b_PartStoreroom dbObj = new b_PartStoreroom();
            UpdateFromDatabaseObject(dbObj);

            // Turn off auditing for object initialization
            AuditEnabled = false;
        }

        public b_PartStoreroom ToDatabaseObject()
        {
            b_PartStoreroom dbObj = new b_PartStoreroom();
            dbObj.ClientId = this.ClientId;
            dbObj.PartStoreroomId = this.PartStoreroomId;
            dbObj.PartId = this.PartId;
            dbObj.StoreroomId = this.StoreroomId;
            dbObj.CountFrequency = this.CountFrequency;
            dbObj.LastCounted = this.LastCounted;
            dbObj.Location1_1 = this.Location1_1;
            dbObj.Location1_2 = this.Location1_2;
            dbObj.Location1_3 = this.Location1_3;
            dbObj.Location1_4 = this.Location1_4;
            dbObj.Location1_5 = this.Location1_5;
            dbObj.QtyMaximum = this.QtyMaximum;
            dbObj.QtyOnHand = this.QtyOnHand;
            dbObj.QtyReorderLevel = this.QtyReorderLevel;
            dbObj.ReorderMethod = this.ReorderMethod;
            dbObj.LastIssueDate = this.LastIssueDate;
            dbObj.LastPurchasePrice = this.LastPurchasePrice;
            dbObj.LastPurchaseDate = this.LastPurchaseDate;
            dbObj.Location2_1 = this.Location2_1;
            dbObj.Location2_2 = this.Location2_2;
            dbObj.Location2_3 = this.Location2_3;
            dbObj.Location2_4 = this.Location2_4;
            dbObj.Location2_5 = this.Location2_5;
            dbObj.Critical = this.Critical;
            dbObj.AutoPurchase = this.AutoPurchase;
            dbObj.PartVendorId = this.PartVendorId;
            dbObj.AutoTransfer = this.AutoTransfer;
            dbObj.AutoTransferIssueStoreroom = this.AutoTransferIssueStoreroom;
            dbObj.AutoTransferMaxQty = this.AutoTransferMaxQty;
            dbObj.AutoTransferMinQty = this.AutoTransferMinQty;
            dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }

        #endregion

        #region Transaction Methods

        public void Create(DatabaseKey dbKey)
        {
            PartStoreroom_Create trans = new PartStoreroom_Create();
            trans.PartStoreroom = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.PartStoreroom);
        }

        public void Retrieve(DatabaseKey dbKey)
        {
            PartStoreroom_Retrieve trans = new PartStoreroom_Retrieve();
            trans.PartStoreroom = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PartStoreroom);
        }

        public void Update(DatabaseKey dbKey)
        {
            PartStoreroom_Update trans = new PartStoreroom_Update();
            trans.PartStoreroom = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.PartStoreroom);
        }

        public void Delete(DatabaseKey dbKey)
        {
            PartStoreroom_Delete trans = new PartStoreroom_Delete();
            trans.PartStoreroom = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }

        protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.PartStoreroomId;
            return BuildChangeLogDbObject(dbKey);
        }

        #endregion

        #region Private Variables

        private long _ClientId;
        private long _PartStoreroomId;
        private long _PartId;
        private long _StoreroomId;
        private int _CountFrequency;
        private DateTime? _LastCounted;
        private string _Location1_1;
        private string _Location1_2;
        private string _Location1_3;
        private string _Location1_4;
        private string _Location1_5;
        private decimal _QtyMaximum;
        private decimal _QtyOnHand;
        private decimal _QtyReorderLevel;
        private string _ReorderMethod;
        private DateTime? _LastIssueDate;
        private decimal _LastPurchasePrice;
        private DateTime? _LastPurchaseDate;
        private string _Location2_1;
        private string _Location2_2;
        private string _Location2_3;
        private string _Location2_4;
        private string _Location2_5;
        private bool _Critical;
        private bool _AutoPurchase;
        private long _PartVendorId;
        private long _AutoTransfer;
        private long _AutoTransferIssueStoreroom;
        private decimal _AutoTransferMaxQty;
        private decimal _AutoTransferMinQty;
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
        /// PartStoreroomId property
        /// </summary>
        [DataMember]
        public long PartStoreroomId
        {
            get { return _PartStoreroomId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartStoreroomId); }
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
        /// StoreroomId property
        /// </summary>
        [DataMember]
        public long StoreroomId
        {
            get { return _StoreroomId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _StoreroomId); }
        }

        /// <summary>
        /// CountFrequency property
        /// </summary>
        [DataMember]
        public int CountFrequency
        {
            get { return _CountFrequency; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _CountFrequency); }
        }

        /// <summary>
        /// LastCounted property
        /// </summary>
        [DataMember]
        public DateTime? LastCounted
        {
            get { return _LastCounted; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _LastCounted); }
        }

        /// <summary>
        /// Location1_1 property
        /// </summary>
        [DataMember]
        public string Location1_1
        {
            get { return _Location1_1; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location1_1); }
        }

        /// <summary>
        /// Location1_2 property
        /// </summary>
        [DataMember]
        public string Location1_2
        {
            get { return _Location1_2; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location1_2); }
        }

        /// <summary>
        /// Location1_3 property
        /// </summary>
        [DataMember]
        public string Location1_3
        {
            get { return _Location1_3; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location1_3); }
        }

        /// <summary>
        /// Location1_4 property
        /// </summary>
        [DataMember]
        public string Location1_4
        {
            get { return _Location1_4; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location1_4); }
        }

        /// <summary>
        /// Location1_5 property
        /// </summary>
        [DataMember]
        public string Location1_5
        {
            get { return _Location1_5; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location1_5); }
        }

        /// <summary>
        /// QtyMaximum property
        /// </summary>
        [DataMember]
        public decimal QtyMaximum
        {
            get { return _QtyMaximum; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QtyMaximum); }
        }

        /// <summary>
        /// QtyOnHand property
        /// </summary>
        [DataMember]
        public decimal QtyOnHand
        {
            get { return _QtyOnHand; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QtyOnHand); }
        }

        /// <summary>
        /// QtyReorderLevel property
        /// </summary>
        [DataMember]
        public decimal QtyReorderLevel
        {
            get { return _QtyReorderLevel; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _QtyReorderLevel); }
        }

        /// <summary>
        /// ReorderMethod property
        /// </summary>
        [DataMember]
        public string ReorderMethod
        {
            get { return _ReorderMethod; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _ReorderMethod); }
        }

        /// <summary>
        /// LastIssueDate property
        /// </summary>
        [DataMember]
        public DateTime? LastIssueDate
        {
            get { return _LastIssueDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _LastIssueDate); }
        }

        /// <summary>
        /// LastPurchasePrice property
        /// </summary>
        [DataMember]
        public decimal LastPurchasePrice
        {
            get { return _LastPurchasePrice; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _LastPurchasePrice); }
        }

        /// <summary>
        /// LastPurchaseDate property
        /// </summary>
        [DataMember]
        public DateTime? LastPurchaseDate
        {
            get { return _LastPurchaseDate; }
            set { Set<DateTime?>(MethodBase.GetCurrentMethod().Name, value, ref _LastPurchaseDate); }
        }

        /// <summary>
        /// Location2_1 property
        /// </summary>
        [DataMember]
        public string Location2_1
        {
            get { return _Location2_1; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location2_1); }
        }

        /// <summary>
        /// Location2_2 property
        /// </summary>
        [DataMember]
        public string Location2_2
        {
            get { return _Location2_2; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location2_2); }
        }

        /// <summary>
        /// Location2_3 property
        /// </summary>
        [DataMember]
        public string Location2_3
        {
            get { return _Location2_3; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location2_3); }
        }

        /// <summary>
        /// Location2_4 property
        /// </summary>
        [DataMember]
        public string Location2_4
        {
            get { return _Location2_4; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location2_4); }
        }

        /// <summary>
        /// Location2_5 property
        /// </summary>
        [DataMember]
        public string Location2_5
        {
            get { return _Location2_5; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Location2_5); }
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
        /// AutoPurchase property
        /// </summary>
        [DataMember]
        public bool AutoPurchase
        {
            get { return _AutoPurchase; }
            set { Set<bool>(MethodBase.GetCurrentMethod().Name, value, ref _AutoPurchase); }
        }

        /// <summary>
        /// PartVendorId property
        /// </summary>
        [DataMember]
        public long PartVendorId
        {
            get { return _PartVendorId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _PartVendorId); }
        }

        /// <summary>
        /// AutoTransfer property
        /// </summary>
        [DataMember]
        public long AutoTransfer
        {
            get { return _AutoTransfer; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AutoTransfer); }
        }

        /// <summary>
        /// AutoTransferIssueStoreroom property
        /// </summary>
        [DataMember]
        public long AutoTransferIssueStoreroom
        {
            get { return _AutoTransferIssueStoreroom; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _AutoTransferIssueStoreroom); }
        }

        /// <summary>
        /// AutoTransferMaxQty property
        /// </summary>
        [DataMember]
        public decimal AutoTransferMaxQty
        {
            get { return _AutoTransferMaxQty; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _AutoTransferMaxQty); }
        }

        /// <summary>
        /// AutoTransferMinQty property
        /// </summary>
        [DataMember]
        public decimal AutoTransferMinQty
        {
            get { return _AutoTransferMinQty; }
            set { Set<decimal>(MethodBase.GetCurrentMethod().Name, value, ref _AutoTransferMinQty); }
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
