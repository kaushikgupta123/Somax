using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts
{
    public partial class DashboardContent:DataContractBase
    {
        #region Property
        public int GridColWidth { get; set; }
        public string ViewName { get; set; }
        public string Name { get; set; }
        #endregion
        public List<DashboardContent> RetriveDashboardContentV2(DatabaseKey dbKey)
        {
            DashboardContent_GetAllV2 trans = new DashboardContent_GetAllV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.DashboardContent = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<DashboardContent> DashboardContentlist = new List<DashboardContent>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_DashboardContent dashboardcontent in trans.DashboardContentList)
            {
                DashboardContent tmpDashboardContent = new DashboardContent();

                tmpDashboardContent.UpdateFromDatabaseObjectForDashboardContent(dashboardcontent);
                DashboardContentlist.Add(tmpDashboardContent);
            }
            
            return DashboardContentlist;
        }

        public void UpdateFromDatabaseObjectForDashboardContent(b_DashboardContent dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.GridColWidth = dbObj.GridColWidth;
            this.ViewName = dbObj.ViewName;
            this.Name = dbObj.Name;
        }
    }
}
