/*
**************************************************************************************************
* PROPRIETARY DATA 
**************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc. and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
**************************************************************************************************
* Attachment.cs - Custom Data Contract 
**************************************************************************************************
* Copyright (c) 2013-2018 by SOMAX Inc.. All rights reserved. 
**************************************************************************************************
* Date        JIRA Item Person         Description
* =========== ========= ============= ==========================================================
* 2018-Oct-31 SOM-1650  Roger Lawton  Modified the RetrieveAll method 
*                                     Added DeleteDbStoredAttachment method 
*                                     Added RetrieveURLCountByAttachmentId method
*                                     Added URLCount property
**************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Serialization; Needed?
using Common.Extensions;
using Database;
using Database.Business;

namespace DataContracts
{
    public partial class Attachment : DataContractBase
    {
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UploadedBy { get; set; }
        public int URLCount { get; set; }
        public string FullName
        {
            get { return string.Format("{0}.{1}", FileName, FileType); }
        }
        public bool IsEditable { get; set; }

        #region V2-716
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public int TotalCount { get; set; }
        #endregion

        /// <summary>
        /// Retrieve All of the attachments for an Object
        /// SOM-1650 
        /// Required to send Attachment.ClientId, Attachment.ObjectName and Attachment.ObjectID
        /// </summary>
        /// <param name="dbKey"></param>
        /// <param name="Timezone"></param>
        /// <returns></returns>
        public List<Attachment> RetrieveAllAttachmentsForObject(DatabaseKey dbKey, string Timezone, bool IncludeProfile, bool edit_secure, string RequestType = "")
        {
            Attachment_RetrieveAllAttachmentsForObject trans = new Attachment_RetrieveAllAttachmentsForObject()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Attachment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<Attachment> Alist = new List<Attachment>();
            if (!string.IsNullOrEmpty(RequestType) && RequestType == "woprint")
            {
                Alist = UpdateFromDatabaseObjectList(trans.AttachmentList, Timezone, dbKey.User.IsSuperUser, dbKey.Personnel.PersonnelId, edit_secure).ToList();
            }
            else
            {
                Alist = UpdateFromDatabaseObjectList(trans.AttachmentList, Timezone, dbKey.User.IsSuperUser, dbKey.Personnel.PersonnelId, edit_secure).Where(x => x.Profile == IncludeProfile).ToList();
            }
            return Alist;
        }

        /// <summary>
        /// Retrieve All of the attachments
        /// </summary>
        /// <param name="dbKey"></param>
        /// <param name="Timezone"></param>
        /// <returns></returns>
        public List<Attachment> RetrieveAllAttachment(DatabaseKey dbKey, string Timezone)
        {
            Attachment_RetrieveAllAttachmentList trans = new Attachment_RetrieveAllAttachmentList()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Attachment = this.ToDatabaseObject();

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.AttachmentList, Timezone);

        }
        // SOM-1635 - Retrieve the profile attachment URL
        public string RetrieveProfileAttachmentURL(DatabaseKey dbKey, string Timezone)
        {
            Attachment_RetrieveProfileAttachments trans = new Attachment_RetrieveProfileAttachments()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Attachment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            List<Attachment> profile_images = UpdateFromDatabaseObjectList(trans.AttachmentList, Timezone);
            string Image_URL = string.Empty;
            if (profile_images.Count > 0)
            {
                Attachment profile = profile_images.First();
                if (profile_images.Count > 0)
                {
                    Image_URL = profile.AttachmentURL;
                }
            }
            return Image_URL;
        }
        public List<Attachment> RetrieveProfileAttachments(DatabaseKey dbKey, string Timezone)
        {
            Attachment_RetrieveProfileAttachments trans = new Attachment_RetrieveProfileAttachments()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Attachment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.AttachmentList, Timezone);

        }
        public void RetrieveLogo(DatabaseKey dbKey, Int64 siteid)
        {
            Attachment_RetrieveLogo trans = new Attachment_RetrieveLogo();
            trans.Attachment = this.ToDatabaseObject();
            trans.Attachment.siteid = siteid;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.Attachment);
        }
        public void RetrieveAllByFileName(DatabaseKey dbKey)
        {
            Attachment_RetrieveAllByFileName trans = new Attachment_RetrieveAllByFileName();
            trans.Attachment = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDObject(trans.Attachment);
        }
        // SOM-1693,SOM-1694, SOM-1695 - No longer need the DeleteDbStoredAttachment method
        public void RetrieveURLCount(DatabaseKey dbkey)
        {
            Attachment_RetrieveURLCount trans = new Attachment_RetrieveURLCount();
            trans.Attachment = this.ToDatabaseObject();
            trans.dbKey = dbkey.ToTransDbKey();
            trans.Execute();
            UpdateFromDObject(trans.Attachment);
            this.URLCount = trans.Attachment.URLCount;
        }

        public int RetrieveAttachmentCount_V2(DatabaseKey dbkey)
        {
            Attachment_RetrieveURLCount_V2 trans = new Attachment_RetrieveURLCount_V2();
            trans.Attachment = this.ToDatabaseObject();
            trans.dbKey = dbkey.ToTransDbKey();
            trans.Execute();
            //UpdateFromDObject(trans.Attachment);
           // this.URLCount = trans.Attachment.URLCount;
           return trans.Attachment.URLCount;
        }
        // SOM-1693, SOM-1694, SOM-1695 - No longer need the UpdateForMigrate method
        // SOM-1693, SOM-1694, SOM-1695 - No longer need the CreateForMigrate method

        public void UpdateFromDObject(b_Attachment dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            // Created Date
            this.CreateDate = dbObj.CreateDate;
            this.CreateBy = dbObj.CreateBy;
            // Turn on auditing
            AuditEnabled = true;
        }
        private List<Attachment> UpdateFromDatabaseObjectList(List<b_Attachment> attachmentList, string Timezone, bool IsSuperUser, long personnelid, bool edit_secure)
        {
            List<Attachment> result = new List<Attachment>();

            foreach (b_Attachment dbObj in attachmentList)
            {
                Attachment tmp = new Attachment();
                tmp.UpdateFromDbObject(dbObj, Timezone, IsSuperUser, personnelid, edit_secure);
                result.Add(tmp);
            }
            return result;
        }
        private List<Attachment> UpdateFromDatabaseObjectList(List<b_Attachment> attachmentList, string Timezone)
        {
            List<Attachment> result = new List<Attachment>();

            foreach (b_Attachment dbObj in attachmentList)
            {
                Attachment tmp = new Attachment();
                tmp.UpdateFromDbObject(dbObj, Timezone);
                result.Add(tmp);
            }
            return result;
        }
        private void UpdateFromDbObject(b_Attachment dbObj, string Timezone, bool IsSuperUser, long personnelid, bool edit_secure)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.UploadedBy = dbObj.UploadedBy;
            this.CreateBy = dbObj.CreateBy;
            this.ModifiedBy = dbObj.ModifiedBy;
            this.IsEditable = (IsSuperUser || this.UploadedBy_PersonnelId == personnelid || edit_secure);
            if (dbObj.UpdateIndex > 0)
            {
                if (dbObj.ModifiedDate != null && dbObj.ModifiedDate != DateTime.MinValue)
                {
                    this.CreateDate = dbObj.ModifiedDate.ToUserTimeZone(Timezone);
                }
                else
                {
                    this.CreateDate = dbObj.ModifiedDate;
                }
            }
            else
            {
                // Created Date
                if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
                {
                    this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
                }
                else
                {
                    this.CreateDate = dbObj.CreateDate;
                }
            }
        }
        private void UpdateFromDbObject(b_Attachment dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.UploadedBy = dbObj.UploadedBy;
            this.CreateBy = dbObj.CreateBy;
            this.ModifiedBy = dbObj.ModifiedBy;
            this.IsEditable = false;
            if (dbObj.UpdateIndex > 0)
            {
                if (dbObj.ModifiedDate != null && dbObj.ModifiedDate != DateTime.MinValue)
                {
                    this.CreateDate = dbObj.ModifiedDate.ToUserTimeZone(Timezone);
                }
                else
                {
                    this.CreateDate = dbObj.ModifiedDate;
                }
            }
            else
            {
                // Created Date
                if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
                {
                    this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
                }
                else
                {
                    this.CreateDate = dbObj.CreateDate;
                }
            }
        }


        public void UpdateFromDatabaseObjectAttachmentPrintExtended(b_Attachment dbObj, string Timezone)
        {
            this.ObjectId = dbObj.ObjectId;
            this.AttachmentURL = dbObj.AttachmentURL;
            this.FileName = dbObj.FileName;
            this.ContentType = dbObj.ContentType;
            this.FileSize = dbObj.FileSize;
            this.Image = dbObj.Image;
            this.Profile = dbObj.Profile;
            this.PrintwithForm = dbObj.PrintwithForm;
        }
        #region V2-716
        public List<Attachment> RetrieveMultipleProfileAttachments(DatabaseKey dbKey, string Timezone)
        {
            Attachment_RetrieveMultipleProfileAttachments trans = new Attachment_RetrieveMultipleProfileAttachments()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Attachment = this.ToDatabaseObjectForMultipleImage();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectListForMultipleImage(trans.AttachmentList, Timezone);

        }
        public b_Attachment ToDatabaseObjectForMultipleImage()
        {
            b_Attachment dbObj = new b_Attachment();
            dbObj.ClientId = this.ClientId;
            dbObj.AttachmentId = this.AttachmentId;
            dbObj.ObjectName = this.ObjectName;
            dbObj.ObjectId = this.ObjectId;
            dbObj.AttachmentURL = this.AttachmentURL;
            dbObj.UploadedBy_PersonnelId = this.UploadedBy_PersonnelId;
            dbObj.Description = this.Description;
            dbObj.FileName = this.FileName;
            dbObj.FileType = this.FileType;
            dbObj.Image = this.Image;
            dbObj.Profile = this.Profile;
            dbObj.External = this.External;
            dbObj.Reference = this.Reference;
            dbObj.ContentType = this.ContentType;
            dbObj.FileSize = this.FileSize;
            dbObj.UpdateIndex = this.UpdateIndex;
            dbObj.offset1=this.offset1;
            dbObj.nextrow=this.nextrow;
            return dbObj;
        }
        private List<Attachment> UpdateFromDatabaseObjectListForMultipleImage(List<b_Attachment> attachmentList, string Timezone)
        {
            List<Attachment> result = new List<Attachment>();

            foreach (b_Attachment dbObj in attachmentList)
            {
                Attachment tmp = new Attachment();
                tmp.UpdateFromDbObjectForMultipleImage(dbObj, Timezone);
                result.Add(tmp);
            }
            return result;
        }
        private void UpdateFromDbObjectForMultipleImage(b_Attachment dbObj, string Timezone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.UploadedBy = dbObj.UploadedBy;
            this.CreateBy = dbObj.CreateBy;
            this.ModifiedBy = dbObj.ModifiedBy;
            this.IsEditable = false;
            this.TotalCount= dbObj.TotalCount;
            if (dbObj.UpdateIndex > 0)
            {
                if (dbObj.ModifiedDate != null && dbObj.ModifiedDate != DateTime.MinValue)
                {
                    this.CreateDate = dbObj.ModifiedDate.ToUserTimeZone(Timezone);
                }
                else
                {
                    this.CreateDate = dbObj.ModifiedDate;
                }
            }
            else
            {
                // Created Date
                if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
                {
                    this.CreateDate = dbObj.CreateDate.ToUserTimeZone(Timezone);
                }
                else
                {
                    this.CreateDate = dbObj.CreateDate;
                }
            }
        }
        public int RetrieveAttachmentCount_ByObjectAndFileName(DatabaseKey dbkey)
        {
            Attachment_RetrieveURLCount_ByObjectAndFileName_V2 trans = new Attachment_RetrieveURLCount_ByObjectAndFileName_V2();
            trans.Attachment = this.ToDatabaseObject_RetrieveURLCount_ByObjectAndFileName();
            trans.dbKey = dbkey.ToTransDbKey();
            trans.Execute();
            //UpdateFromDObject(trans.Attachment);
            // this.URLCount = trans.Attachment.URLCount;
            return trans.Attachment.URLCount;
        }
        public b_Attachment ToDatabaseObject_RetrieveURLCount_ByObjectAndFileName()
        {
            b_Attachment dbObj = new b_Attachment();
            dbObj.ClientId = this.ClientId;
            //dbObj.AttachmentId = this.AttachmentId;
            dbObj.ObjectName = this.ObjectName;
            dbObj.ObjectId = this.ObjectId;
            //dbObj.AttachmentURL = this.AttachmentURL;
            //dbObj.UploadedBy_PersonnelId = this.UploadedBy_PersonnelId;
            //dbObj.Description = this.Description;
            dbObj.FileName = this.FileName;
            //dbObj.FileType = this.FileType;
            //dbObj.Image = this.Image;
            //dbObj.Profile = this.Profile;
            //dbObj.External = this.External;
            //dbObj.Reference = this.Reference;
            //dbObj.ContentType = this.ContentType;
            //dbObj.FileSize = this.FileSize;
            //dbObj.UpdateIndex = this.UpdateIndex;
            return dbObj;
        }
        #endregion

        #region V2-949
        public void UpdateFromDatabaseObjectPOAttachmentPrintExtended(b_Attachment dbObj, string Timezone)
        {
            this.ObjectId = dbObj.ObjectId;
            this.AttachmentURL = dbObj.AttachmentURL;
        }
        #endregion
    }
}
