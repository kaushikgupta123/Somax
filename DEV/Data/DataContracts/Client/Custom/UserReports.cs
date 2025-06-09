using Database;
using Database.Business;
using System;
using System.Collections.Generic;
namespace DataContracts
{
    public partial class UserReports : DataContractBase
    {
        #region Properties
        public int IsFavorite { get; set; }
        public long ReportFavoritesId { get; set; }       
        public long PersonnelId { get; set; }
        
        #endregion

        public Int32 Count { get; set; }
        public List<UserReports> RetrieveCountForReportNameExist(DatabaseKey dbKey)
        {
            Retrieve_CountforReportName trans = new Retrieve_CountforReportName()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.UserReports = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();

            trans.Execute();
            UpdateFromDatabaseObject(trans.UserReports);
            List<UserReports> UserReportsList = new List<UserReports>();
            foreach (b_UserReports UserReports in trans.countList)
            {
                UserReports tmpUserReports = new UserReports()
                {
                    Count = UserReports.Count,
                };
                UserReportsList.Add(tmpUserReports);
            }
            return UserReportsList;
        }

        public List<UserReports> RetrieveByGroup(DatabaseKey dbKey)
        {
            UserReports_RetrieveByGroup trans = new UserReports_RetrieveByGroup();
            trans.UserReports = this.ToDatabaseForRetrieveByGroup();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseReportListingForRetrieveByGroup(trans.UserReportsList);
        }
        public b_UserReports ToDatabaseForRetrieveByGroup()
        {
            b_UserReports dbObj = new b_UserReports();
            dbObj.SiteId = this.SiteId;
            dbObj.ReportGroup = this.ReportGroup;
            dbObj.PersonnelId = this.PersonnelId;
            return dbObj;
        }
        public List<UserReports> UpdateFromDatabaseReportListingForRetrieveByGroup(List<b_UserReports> dbObjs)
        {
            List<UserReports> result = new List<UserReports>();

            foreach (b_UserReports dbObj in dbObjs)
            {
                UserReports tmp = new UserReports();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.IsFavorite = dbObj.IsFavorite;
                tmp.ReportFavoritesId = dbObj.ReportFavoritesId;
                result.Add(tmp);
            }
            return result;
        }
    }
}
