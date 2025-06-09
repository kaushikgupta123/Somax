using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class DashboardListing : DataContractBase
    {
        public List<DashboardListing> RetrieveAllCustom(DatabaseKey dbKey)
        {
            DashboardListing_RetrieveAll trans = new DashboardListing_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.DashboardListingList = new List<b_DashboardListing>();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.DashboardListingList);
        }

        public static List<DashboardListing> UpdateFromDatabaseObjectList(List<b_DashboardListing> dbObjs)
        {
            List<DashboardListing> result = new List<DashboardListing>();

            foreach (b_DashboardListing dbObj in dbObjs)
            {
                DashboardListing tmp = new DashboardListing();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
    }
}
