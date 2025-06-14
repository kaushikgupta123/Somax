/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2022 by SOMAX Inc.
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
    /// Business object that stores a record from the STNotes table.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/SOMAX.DataContracts")]
    public partial class STNotes : DataContractBase 
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public STNotes()
        {
            Initialize();
        }

        public void Clear()
        {
            Initialize();
        }

        public void UpdateFromDatabaseObject(b_STNotes dbObj)
        {
		            this.STNotesId = dbObj.STNotesId;
            this.SupportTicketId = dbObj.SupportTicketId;
            this.Type = dbObj.Type;
            this.Content = dbObj.Content;
            this.From_PersonnelId = dbObj.From_PersonnelId;
			
			// Turn on auditing
			AuditEnabled = true;
		}

        private void Initialize()
        {
            b_STNotes dbObj = new b_STNotes();
            UpdateFromDatabaseObject(dbObj);
			
			// Turn off auditing for object initialization
			AuditEnabled = false;
        }

        public b_STNotes ToDatabaseObject()
        {
            b_STNotes dbObj = new b_STNotes();
            dbObj.STNotesId = this.STNotesId;
            dbObj.SupportTicketId = this.SupportTicketId;
            dbObj.Type = this.Type;
            dbObj.Content = this.Content;
            dbObj.From_PersonnelId = this.From_PersonnelId;
            return dbObj;
        }      

        #endregion

        #region Transaction Methods
        
        public void Create(DatabaseKey dbKey) 
        {
            STNotes_Create trans = new STNotes_Create();
            trans.STNotes = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.STNotes);
        }

        public void Retrieve(DatabaseKey dbKey) 
        {
            STNotes_Retrieve trans = new STNotes_Retrieve();
            trans.STNotes = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.STNotes);
        }

        public void Update(DatabaseKey dbKey) 
        {
            STNotes_Update trans = new STNotes_Update();
            trans.STNotes = this.ToDatabaseObject();
			trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            
            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.STNotes);
        }

        public void Delete(DatabaseKey dbKey) 
        {
            STNotes_Delete trans = new STNotes_Delete();
            trans.STNotes = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
        }
		
		protected b_ChangeLog GetChangeLogObject(DatabaseKey dbKey)
        {
            AuditTargetObjectId = this.STNotesId;
			return BuildChangeLogDbObject(dbKey);
        }
        
        #endregion
		
		#region Private Variables

        private long _STNotesId;
        private long _SupportTicketId;
        private string _Type;
        private string _Content;
        private long _From_PersonnelId;
        #endregion
        
        #region Properties


        /// <summary>
        /// STNotesId property
        /// </summary>
        [DataMember]
        public long STNotesId
        {
            get { return _STNotesId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _STNotesId); }
        }

        /// <summary>
        /// SupportTicketId property
        /// </summary>
        [DataMember]
        public long SupportTicketId
        {
            get { return _SupportTicketId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _SupportTicketId); }
        }

        /// <summary>
        /// Type property
        /// </summary>
        [DataMember]
        public string Type
        {
            get { return _Type; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Type); }
        }

        /// <summary>
        /// Content property
        /// </summary>
        [DataMember]
        public string Content
        {
            get { return _Content; }
            set { Set<string>(MethodBase.GetCurrentMethod().Name, value, ref _Content); }
        }

        /// <summary>
        /// From_PersonnelId property
        /// </summary>
        [DataMember]
        public long From_PersonnelId
        {
            get { return _From_PersonnelId; }
            set { Set<long>(MethodBase.GetCurrentMethod().Name, value, ref _From_PersonnelId); }
        }
        #endregion
		
		
    }
}
