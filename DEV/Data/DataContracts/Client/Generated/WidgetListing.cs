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
    /// Business object that stores a record from the WidgetListing table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class WidgetListing : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public WidgetListing()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_WidgetListing dbObj)
        {
		            this.WidgetListingId = dbObj.WidgetListingId;
            this.Name = dbObj.Name;
            this.Description = dbObj.Description;
            this.GridColWidth = dbObj.GridColWidth;
            this.ViewName = dbObj.ViewName;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_WidgetListing dbObj = new b_WidgetListing();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_WidgetListing ToDatabaseObject()
        {
            b_WidgetListing dbObj = new b_WidgetListing();
            dbObj.WidgetListingId = this.WidgetListingId;
            dbObj.Name = this.Name;
            dbObj.Description = this.Description;
            dbObj.GridColWidth = this.GridColWidth;
            dbObj.ViewName = this.ViewName;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            WidgetListing_Create trans = new WidgetListing_Create();
            trans.WidgetListing = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.WidgetListing);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            WidgetListing_Retrieve trans = new WidgetListing_Retrieve();
            trans.WidgetListing = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.WidgetListing);
        }

        public void Update(DatabaseKey dbKey) 
        {
            WidgetListing_Update trans = new WidgetListing_Update();
            trans.WidgetListing = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.WidgetListing);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            WidgetListing_Delete trans = new WidgetListing_Delete();
            trans.WidgetListing = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.WidgetListingId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _WidgetListingId;
        private string _Name;
        private string _Description;
        private int _GridColWidth;
        private string _ViewName;
        #endregion
        
        #region Properties


        /// <summary>
        /// WidgetListingId property
        /// </summary>
        [DataMember]
        public long WidgetListingId
        {
            get { return _WidgetListingId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _WidgetListingId); }
        }

        /// <summary>
        /// Name property
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Name); }
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
        /// GridColWidth property
        /// </summary>
        [DataMember]
        public int GridColWidth
        {
            get { return _GridColWidth; }
            set { Set<int>(MethodBase.GetCurrentMethod().Name, value, ref _GridColWidth); }
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
        #endregion
		
		
    }
}
