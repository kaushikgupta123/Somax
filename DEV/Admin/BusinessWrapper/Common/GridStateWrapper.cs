using Admin.Models.Common;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.BusinessWrapper.Common
{
    public class GridStateWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public GridStateWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public void CreateUpdateState(string GridName, string LayOutInfo, string FilterInfo = "")
        {
            GridDataLayout gridlayout = new GridDataLayout();
            gridlayout.ClientId = this.userData.DatabaseKey.Client.ClientId;
            gridlayout.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            gridlayout.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            gridlayout.GridName = GridName;
            gridlayout.RetrievebyGridSiteUser(this.userData.DatabaseKey);
            gridlayout.LayoutInfo = LayOutInfo;
            if (!string.IsNullOrWhiteSpace(FilterInfo))
            {
                gridlayout.FilterInfo = FilterInfo;
            }

            if (gridlayout.GridDataLayoutId > 0)
            {
                gridlayout.Update(this.userData.DatabaseKey);
            }
            else
            {
                gridlayout.Create(this.userData.DatabaseKey);
            }
        }
        public bool EditState()
        {
            GridDataLayout objGridDataLayout = new GridDataLayout();
            objGridDataLayout.Retrieve(userData.DatabaseKey);
            objGridDataLayout.Update(userData.DatabaseKey);
            return false;
        }
        public string GetState(string GridName)
        {
            string LayOutInfo = string.Empty;
            GridDataLayout gridlayout = new GridDataLayout();
            gridlayout.ClientId = this.userData.DatabaseKey.Client.ClientId;
            gridlayout.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            gridlayout.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            gridlayout.GridName = GridName;
            gridlayout.RetrievebyGridSiteUser(this.userData.DatabaseKey);
            if (gridlayout != null)
            {
                LayOutInfo = gridlayout.LayoutInfo;
            }
            return LayOutInfo;
        }
        public GridDataLayoutModel GetLayout(string GridName)
        {
            string LayOutInfo = string.Empty;
            GridDataLayoutModel gridDataLayoutModel = new GridDataLayoutModel();
            GridDataLayout gridlayout = new GridDataLayout();
            gridlayout.ClientId = this.userData.DatabaseKey.Client.ClientId;
            gridlayout.SiteId = this.userData.DatabaseKey.User.DefaultSiteId;
            gridlayout.UserInfoId = this.userData.DatabaseKey.User.UserInfoId;
            gridlayout.GridName = GridName;
            gridlayout.RetrievebyGridSiteUser(this.userData.DatabaseKey);
            if (gridlayout != null)
            {
                gridDataLayoutModel.LayoutInfo = gridlayout.LayoutInfo;
                gridDataLayoutModel.FilterInfo = gridlayout.FilterInfo;
            }
            return gridDataLayoutModel;
        }
    }
}