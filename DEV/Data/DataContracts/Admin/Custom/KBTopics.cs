using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class KBTopics : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
       
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

       

        public string ValidateFor = string.Empty;
       
        public Int64 ObjectId { get; set; }
        public string ObjectName { get; set; }

       
        public string Flag { get; set; }

       
        public int CustomQueryDisplayId { get; set; }
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }

        public string SearchText { get; set; }
       
        public List<KBTopics> listOfKBTopics { get; set; }
        public Int32 TotalCount { get; set; }
        public string Assigned { get; set; }
        public List<b_KBTopics> KBTopicsList { get; set; }
        public List<List<KBTopics>> TotalRecords { get; set; }
        public string ClientLookupId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public string NameMiddle { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Tag_Name { get; set; }
        public long PersonnelId { get; set; }
        public string CategoryName { get; set; }
        public string locallang { get; set; }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Search
        public KBTopics KBTopicRetrieveChunkSearchV2(DatabaseKey dbKey)
        {
            KBTopics_RetrieveChunkSearchV2 trans = new KBTopics_RetrieveChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.KBTopics = this.ToDateBaseObjectForChunkSearch();
            trans.KBTopics.locallang= dbKey.Localization;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfKBTopics = new List<KBTopics>();


            List<KBTopics> KBTopicslist = new List<KBTopics>();
            foreach (b_KBTopics KBTopics in trans.KBTopics.listOfKBTopics)
            {
                KBTopics tmpKBTopics = new KBTopics();

                tmpKBTopics.UpdateFromDatabaseObjectForChunkSearch(KBTopics);
                KBTopicslist.Add(tmpKBTopics);
            }
            this.listOfKBTopics.AddRange(KBTopicslist);
            return this;
        }
        public void UpdateFromDatabaseObjectForChunkSearch(b_KBTopics dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Category = dbObj.Category;
            this.CategoryName = dbObj.CategoryName;
            this.TotalCount = dbObj.TotalCount;



        }

        public b_KBTopics ToDateBaseObjectForChunkSearch()
        {
            b_KBTopics dbObj = this.ToDatabaseObject();
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.SearchText = this.SearchText;
            dbObj.Category = this.Category;



            return dbObj;
        }

        #endregion
        #region Details
        public void RetrieveByPKForeignKeys_V2(DatabaseKey dbKey)
        {
            KBTopics_RetrieveByForeignKeys_V2 trans = new KBTopics_RetrieveByForeignKeys_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.KBTopics = this.ToDatabaseObject();
            trans.KBTopics.locallang = dbKey.Localization;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.CategoryName = trans.KBTopics.CategoryName;
            UpdateFromDatabaseObject(trans.KBTopics);
           
        }
        #endregion

        public List<List<KBTopics>> Retrievetags(DatabaseKey dbKey)
        {
            KbTopics_RetrieveTags trans = new KbTopics_RetrieveTags()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.KbTopicsList = this.ToDatabaseObjectPersonnelList();
            trans.KBTopics = this.ToDatabaseObjectPersonnel();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();

            TotalRecords = new List<List<KBTopics>>();
            this.TotalRecords.Add(KBTopics.UpdateFromRetrieveTags(trans.KbTopicsPersonnelList[0]));
            //this.TotalRecords.Add(KBTopics.UpdateFromRetrieveTags(trans.KbTopicsPersonnelList[1]));

            return this.TotalRecords;

           
        }

       
        public b_KBTopics ToDateBaseObjectFortags()
        {
            b_KBTopics dbObj = this.ToDatabaseObject();
            dbObj.Assigned = this.Assigned;
            return dbObj;
        }
        public List<b_KBTopics> ToDatabaseObjectPersonnelList()
        {
            List<b_KBTopics> dbObj = new List<b_KBTopics>();
            dbObj = this.KBTopicsList;
            return dbObj;
        }
        public b_KBTopics ToDatabaseObjectPersonnel()
        {
            b_KBTopics dbObj = new b_KBTopics();
            dbObj.KBTopicsId = this.KBTopicsId;
            dbObj.Flag = this.Flag;
            return dbObj;
        }
        public static List<KBTopics> UpdateFromRetrieveTags(List<b_KBTopics> dbObjs)
        {

            List<KBTopics> result = new List<KBTopics>();

            foreach (b_KBTopics dbObj in dbObjs)
            {
                KBTopics tmp = new KBTopics();
                tmp.UpdateFromExtendedDatabaseObjectPersonnel(dbObj);
                result.Add(tmp);
            }
            return result;

        }

        

        public void UpdateFromExtendedDatabaseObjectPersonnel(b_KBTopics dbObj)
        {
            this.PersonnelId = dbObj.PersonnelId;
            this.NameFirst = string.IsNullOrEmpty(dbObj.NameFirst) ? "" : dbObj.NameFirst;
            this.NameLast = string.IsNullOrEmpty(dbObj.NameLast) ? "" : dbObj.NameLast;
            this.NameMiddle = string.IsNullOrEmpty(dbObj.NameMiddle) ? "" : dbObj.NameMiddle;
            this.FullName = string.IsNullOrEmpty(dbObj.FullName) ? "" : dbObj.FullName;
            this.Tag_Name = string.IsNullOrEmpty(dbObj.UserName) ? "" : dbObj.UserName;
            this.Email = string.IsNullOrEmpty(dbObj.Email) ? "" : dbObj.Email;
        }
    }
}
