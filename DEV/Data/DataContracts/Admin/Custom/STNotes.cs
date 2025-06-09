using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using Common.Extensions;

using Database.Business;
using Database.Transactions;

namespace DataContracts
{
    public partial class STNotes:DataContractBase
    {
        #region Properties


        /// <summary>
        /// ModifiedDate property
        /// </summary>
        [DataMember]
        public DateTime ModifyDate { get; set; }
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public long UserInfoId { get; set; }

        private long _userInfoId;
        #endregion
        #region Public Method
        public static List<STNotes> UpdateFromDatabaseObjectList(List<b_STNotes> dbObj, long userInfoId, string time_zone)
        {
            List<STNotes> result = new List<STNotes>();
            foreach (b_STNotes notes in dbObj)
            {
                STNotes tmp = new STNotes() { UserInfoId = userInfoId };
                tmp.UpdateFromExtendedDatabaseObject(notes, time_zone);
                result.Add(tmp);
            }

            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_STNotes dbObj, string time_zone)
        {
            UpdateFromDatabaseObject(dbObj);
            if (dbObj.ModifyDate != null && dbObj.ModifyDate != DateTime.MinValue)
            {
                this.ModifyDate = dbObj.ModifyDate.ToUserTimeZone(time_zone);
            }
            else
            {
                this.ModifyDate = dbObj.ModifyDate;
            }

            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ToUserTimeZone(time_zone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.SupportTicketId = dbObj.SupportTicketId;
        }

        #endregion
        public List<STNotes> RetrieveBySupportTicketId(DatabaseKey dbKey, string time_zone)
        {
            STNotes_RetrieveBySupportTicketId trans = new STNotes_RetrieveBySupportTicketId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.STNotes = this.ToExtendedDatabaseObject();
            trans.STNotesList = new List<b_STNotes>();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            _userInfoId = dbKey.User.UserInfoId;

            return STNotes.UpdateFromDatabaseObjectList(trans.STNotesList, dbKey.User.UserInfoId, time_zone);
        }
        public b_STNotes ToExtendedDatabaseObject()
        {
            b_STNotes dbObj = this.ToDatabaseObject();
            dbObj.ModifyDate = this.ModifyDate;
            dbObj.SupportTicketId = this.SupportTicketId;
            return dbObj;
        }
        public void CreateInAdminSite(DatabaseKey dbKey)
        {
            STNotes_CreateInAdminSite trans = new STNotes_CreateInAdminSite();
            trans.STNotes = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.STNotes);
        }
    }
}
