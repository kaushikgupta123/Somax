/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person          Description
* =========== ========= =============== ==========================================================
* 2016-Nov-11 SOM-1126  Roger Lawton    Convert date to local from UTC
*/


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Extensions;
using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    /// <summary>
    /// Business object that stores a record from the Notes table.
    /// </summary>
    public partial class Notes : DataContractBase 
    {
        #region Properties


        /// <summary>
        /// ModifiedDate property
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public long UserInfoId { get; set; }

        [DataMember]
        public bool IsEditable { get; set; }

        private long _userInfoId;
        public string PersonnelInitial { get; set; }

        public long SiteId { get; set; }

        #endregion


        #region Public Methods

        public static List<Notes> UpdateFromDatabaseObjectList(List<b_Notes> dbObj, long userInfoId, string time_zone)
        {
            List<Notes> result = new List<Notes>();
            foreach (b_Notes notes in dbObj)
            {
                Notes tmp = new Notes() { UserInfoId = userInfoId };
                tmp.UpdateFromExtendedDatabaseObject(notes, time_zone);
                result.Add(tmp);
            }

            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_Notes dbObj, string time_zone)
        {
            UpdateFromDatabaseObject(dbObj);
            // SOM-706 - Convert the create date to the user's time zone
            if (dbObj.ModifiedDate != null && dbObj.ModifiedDate != DateTime.MinValue)
            {
              this.ModifiedDate = dbObj.ModifiedDate.ToUserTimeZone(time_zone);
            }
            else
            {
              this.ModifiedDate = dbObj.ModifiedDate;
            }

            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(time_zone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            //this.ModifiedDate = dbObj.ModifiedDate;
            this.ObjectId = dbObj.ObjectId;
            this.TableName = dbObj.TableName;
            this.PersonnelInitial = dbObj.PersonnelInitial;
            IsEditable = (UserInfoId == OwnerId); // The notes can only be edited by the owner
        }

        public static List<b_Notes> ToDatabaseObjectList(List<Notes> objs)
        {
            List<b_Notes> result = new List<b_Notes>();
            foreach (Notes obj in objs)
            {
                result.Add(obj.ToExtendedDatabaseObject());
            }

            return result;
        }

        public b_Notes ToExtendedDatabaseObject()
        {
            b_Notes dbObj = this.ToDatabaseObject();
            dbObj.ModifiedDate = this.ModifiedDate;
            dbObj.TableName = this.TableName;
            dbObj.ObjectId = this.ObjectId;
            return dbObj;
        }
        public b_Notes ToExtendedDatabaseObjectPO()
        {
            b_Notes dbObj = this.ToDatabaseObject();
            dbObj.ModifiedDate = this.ModifiedDate;
            dbObj.SiteId = this.SiteId;
            dbObj.ObjectId = this.ObjectId;
            return dbObj;
        }

        #endregion

        #region Transaction Methods
        public List<Notes> RetrieveByOwnerId(DatabaseKey dbKey, string time_zone)
        {
            Notes_RetrieveByOwnerId trans = new Notes_RetrieveByOwnerId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Notes = this.ToExtendedDatabaseObject();
            trans.NotesList = new List<b_Notes>();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Notes.UpdateFromDatabaseObjectList(trans.NotesList, dbKey.User.UserInfoId, time_zone);
        }

        public List<Notes> RetrieveByObjectId(DatabaseKey dbKey, string time_zone)
        {
            Notes_RetrieveByObjectId trans = new Notes_RetrieveByObjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Notes = this.ToExtendedDatabaseObject();
            trans.NotesList = new List<b_Notes>();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            _userInfoId = dbKey.User.UserInfoId;

            return Notes.UpdateFromDatabaseObjectList(trans.NotesList, dbKey.User.UserInfoId, time_zone);
        }
        public List<Notes> RetrieveByObjectIdForPurchaseOrder_V2(DatabaseKey dbKey, string time_zone)
        {
            Notes_RetrieveByObjectIdForPurchaseOrder_V2 trans = new Notes_RetrieveByObjectIdForPurchaseOrder_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Notes = this.ToExtendedDatabaseObjectPO();
            trans.NotesList = new List<b_Notes>();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            _userInfoId = dbKey.User.UserInfoId;

            return Notes.UpdateFromDatabaseObjectList(trans.NotesList, dbKey.User.UserInfoId, time_zone);
        }

        #endregion


    }
}
