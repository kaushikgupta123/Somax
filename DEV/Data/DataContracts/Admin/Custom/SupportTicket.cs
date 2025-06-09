
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Database.Business;
using System.Data.SqlClient;
using System.Data;
using Database.Transactions;
using Database;

namespace DataContracts
{
    public partial class SupportTicket : DataContractBase
    {
        #region Property
        public DateTime? CreateDate { get; set; }
        public int CustomQueryDisplayId { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public int offset1 { get; set; }
        public int nextrow { get; set; }
        public string SearchText { get; set; }
        public int TotalCount { get; set; }
        public string Contact { get; set; }
        public string Agent { get; set; }
        public long PersonnelId { get; set; }
        public string TagName { get; set; }
        public List<List<SupportTicket>> TotalRecords { get; set; }
        public List<b_SupportTicket> SupportTicketList { get; set; }
        #endregion
        public List<SupportTicket> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            SupportTicket_RetrieveChunkSearch trans = new SupportTicket_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.SupportTicket = this.ToDateBaseObjectForRetrieveChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<SupportTicket> SupportTicketlist = new List<SupportTicket>();

            foreach (b_SupportTicket MaterialRequest in trans.SupportTicketList)
            {
                SupportTicket tmpSupportTicketlist = new SupportTicket();
                tmpSupportTicketlist.UpdateFromDatabaseObjectForRetriveChunkSearch(MaterialRequest);
                SupportTicketlist.Add(tmpSupportTicketlist);
            }
            return SupportTicketlist;
        }

        public b_SupportTicket ToDateBaseObjectForRetrieveChunkSearch()
        {
            b_SupportTicket dbObj = new b_SupportTicket();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.SupportTicketId = this.SupportTicketId;

            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.Subject = this.Subject;
            dbObj.Status = this.Status;
            dbObj.Contact = this.Contact;
            dbObj.Agent = this.Agent;
            dbObj.Contact_PersonnelId = this.Contact_PersonnelId;
            dbObj.Agent_PersonnelId = this.Agent_PersonnelId;
            dbObj.Subject = this.Subject;
            dbObj.CreateDate = this.CreateDate;
            dbObj.CompleteDate = this.CompleteDate;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForRetriveChunkSearch(b_SupportTicket dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = dbObj.SiteId;
            this.SupportTicketId = dbObj.SupportTicketId;
            this.Subject = dbObj.Subject;
            this.Contact = dbObj.Contact;
            this.Agent = dbObj.Agent;
            this.Status = dbObj.Status;
            this.CreateDate = dbObj.CreateDate;
            this.CompleteDate = dbObj.CompleteDate;
            this.orderbyColumn = dbObj.orderbyColumn;
            this.orderBy = dbObj.orderBy;
            this.offset1 = dbObj.offset1;
            this.nextrow = dbObj.nextrow;
            this.SearchText = dbObj.SearchText;
            this.TotalCount = dbObj.TotalCount;
        }
        public void CreateInAdmintSite(DatabaseKey dbKey)
        {
            SupportTicket_CreateInAdminSite trans = new SupportTicket_CreateInAdminSite();
            trans.SupportTicket = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SupportTicket);
        }
        public void RetrieveByPKForAdmin(DatabaseKey dbKey)
        {
            SupportTicket_RetrieveByPKForAdmin trans = new SupportTicket_RetrieveByPKForAdmin()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.SupportTicket = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.SupportTicket);
        }
        public void UpdateInAdmintSite(DatabaseKey dbKey)
        {
            SupportTicket_UpdateInAdminSite trans = new SupportTicket_UpdateInAdminSite();
            trans.SupportTicket = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.SupportTicket);
        }
        public List<List<SupportTicket>> Retrievetags(DatabaseKey dbKey)
        {
            SupportTicket_RetrieveTags trans = new SupportTicket_RetrieveTags()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.SupportTicketList = this.ToDatabaseObjectTagList();
            trans.SupportTicket = this.ToDatabaseObjectTag();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();

            TotalRecords = new List<List<SupportTicket>>();
            this.TotalRecords.Add(SupportTicket.UpdateFromRetrieveTags(trans.SupportTicketPersonnelList[0]));

            return this.TotalRecords;


        }
        public List<b_SupportTicket> ToDatabaseObjectTagList()
        {
            List<b_SupportTicket> dbObj = new List<b_SupportTicket>();
            dbObj = this.SupportTicketList;
            return dbObj;
        }
        public b_SupportTicket ToDatabaseObjectTag()
        {
            b_SupportTicket dbObj = new b_SupportTicket();
            return dbObj;
        }
        public static List<SupportTicket> UpdateFromRetrieveTags(List<b_SupportTicket> dbObjs)
        {

            List<SupportTicket> result = new List<SupportTicket>();

            foreach (b_SupportTicket dbObj in dbObjs)
            {
                SupportTicket tmp = new SupportTicket();
                tmp.UpdateFromExtendedDatabaseObjectTags(dbObj);
                result.Add(tmp);
            }
            return result;

        }
        public void UpdateFromExtendedDatabaseObjectTags(b_SupportTicket dbObj)
        {
            this.TagName = string.IsNullOrEmpty(dbObj.TagName) ? "" : dbObj.TagName;
        }
    }
}
