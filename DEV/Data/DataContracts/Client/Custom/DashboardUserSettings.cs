using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class DashboardUserSettings : DataContractBase
    {
        public void RetrieveByDashboardlistingIdandUserinfoId(DatabaseKey dbKey)
        {
            DashboardUserSettings_RetrieveByDashboardIdandUserinfoId trans = new DashboardUserSettings_RetrieveByDashboardIdandUserinfoId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.DashboardUserSettings = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObjectForDashboardUserSettings(trans.DashboardUserSettings);

        }
        public void UpdateFromDatabaseObjectForDashboardUserSettings(b_DashboardUserSettings dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

        }
        public void RetrieveDefaultDashboardListingId(DatabaseKey dbKey)
        {
            DashboardUserSettings_RetrieveDefaultDashboardListingId trans = new DashboardUserSettings_RetrieveDefaultDashboardListingId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.DashboardUserSettings = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.DashboardUserSettings);

        }
        public void CreateFromDatabase_V2(DatabaseKey dbKey)
        {
            DashboardUserSettings_CreateFromDatabase_V2 trans = new DashboardUserSettings_CreateFromDatabase_V2();
            trans.DashboardUserSettings = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.DashboardUserSettings);
        }

        public void UpdateFromDatabase_V2(DatabaseKey dbKey)
        {
            DashboardUserSettings_UpdateFromDatabase_V2 trans = new DashboardUserSettings_UpdateFromDatabase_V2();
            trans.DashboardUserSettings = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.DashboardUserSettings);
        }
    }
}
