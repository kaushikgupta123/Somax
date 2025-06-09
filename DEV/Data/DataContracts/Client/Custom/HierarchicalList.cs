using Database;
using Database.Business;
using System.Collections.Generic;

namespace DataContracts
{
    public partial class HierarchicalList : DataContractBase
    {
        #region Transaction Methods
        public List<HierarchicalList> RetrieveActiveListByName(DatabaseKey dbKey)
        {
            HierarchicalList_RetrieveActiveListByName trans = new HierarchicalList_RetrieveActiveListByName()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.HierarchicalList = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<b_HierarchicalList> lookup = trans.HierarchicalLists;

            List<HierarchicalList> result = new List<HierarchicalList>();

            foreach (b_HierarchicalList li in lookup)
            {
                HierarchicalList temp = new HierarchicalList()
                {
                    ClientId = li.ClientId,
                    AreaId = li.AreaId,
                    DepartmentId = li.DepartmentId,
                    SiteId = li.SiteId,
                    ListName = li.ListName,
                    Level1Value = li.Level1Value,
                    Level1Description = li.Level1Description,
                    Level2Value = li.Level2Value,
                    Level2Description = li.Level2Description,
                    Level3Value = li.Level3Value,
                    Level3Description = li.Level3Description,
                    Level4Value = li.Level4Value,
                    Level4Description = li.Level4Description,
                };
                result.Add(temp);
            }
            return result;

        }
        #endregion
    }
}
