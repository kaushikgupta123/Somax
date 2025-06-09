using Client.BusinessWrapper.Common;
using Client.Models.Configuration.StoreroomSetup;
using Data.DataContracts;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Client.BusinessWrapper.Configuration
{
    public class StoreroomWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public StoreroomWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<StoreroomModel> GetStoreroomSetupList(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, string _name = "", string _description = null, long _siteId = 0,string searchText="",int Case=0)
        {
            List<StoreroomModel> StoreroomModelList = new List<StoreroomModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            StoreroomModel StoreroomModel;
            Storeroom storeroom = new Storeroom();
            storeroom.ClientId = userData.DatabaseKey.Client.ClientId;
            storeroom.orderbyColumn = orderbycol;
            storeroom.orderBy = orderDir;
            storeroom.offset1 = skip;
            storeroom.nextrow = length;
            storeroom.Name = _name;
            storeroom.Description = _description;
            storeroom.SiteId=_siteId;
            storeroom.SearchText=searchText;
            storeroom.Case=Case;
            var cList = storeroom.RetrieveChunkSearch(userData.DatabaseKey);
            foreach (var item in cList)
            {
                StoreroomModel = new StoreroomModel();
                StoreroomModel.StoreroomId = item.StoreroomId;
                StoreroomModel.SiteId = item.SiteId;
                StoreroomModel.Name = item.Name;
                StoreroomModel.SiteName = item.SiteName;
                StoreroomModel.Description = item.Description;
                StoreroomModel.InactiveFlag = item.InactiveFlag;
                StoreroomModel.totalCount = item.totalCount;
                StoreroomModelList.Add(StoreroomModel);
            }
            return StoreroomModelList;
        }

        #region Details
        public StoreroomModel StoreroomDetails(long StoreroomId)
        {
            StoreroomModel returnModel=new StoreroomModel();
            DataContracts.Storeroom storeroom = new DataContracts.Storeroom()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                StoreroomId = StoreroomId
            };
            storeroom.RetrieveforDetails(this.userData.DatabaseKey);
            StoreroomModel SRModel = new StoreroomModel();
            SRModel.StoreroomId = storeroom.StoreroomId;
            SRModel.SiteId = storeroom.SiteId;
            SRModel.Name = storeroom.Name;
            SRModel.Description= storeroom.Description;
            SRModel.InactiveFlag = storeroom.InactiveFlag;
            SRModel.SiteName = storeroom.SiteName;
            returnModel = SRModel;
            return returnModel;
        }
        #endregion
        #endregion
        #region Add OR Update
        public List<String> AddStoreroom(StoreroomModel obj, ref long StoreroomID)
        {
            DataContracts.Storeroom storeroom = new DataContracts.Storeroom();
            storeroom.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            storeroom.Name = obj.Name;
            storeroom.Description= obj.Description;
            storeroom.SiteId = obj.SiteId;
            storeroom.StoreroomId = obj.StoreroomId;
            storeroom.CheckDuplicateSite(this.userData.DatabaseKey);
            if (storeroom.ErrorMessages == null || storeroom.ErrorMessages.Count == 0)
            {
                storeroom.Create(userData.DatabaseKey);
                StoreroomID = storeroom.StoreroomId;
            }        
            return storeroom.ErrorMessages;
        }
        public List<String> UpdateStoreroom(StoreroomModel obj, ref long StoreroomID)
        {
            DataContracts.Storeroom storeroom = new DataContracts.Storeroom()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                StoreroomId = obj.StoreroomId
            };
            storeroom.Retrieve(this.userData.DatabaseKey);

            storeroom.Name = obj.Name;
            storeroom.Description = obj.Description;
            storeroom.CheckDuplicateSite(this.userData.DatabaseKey);
            if (storeroom.ErrorMessages == null || storeroom.ErrorMessages.Count == 0)
            {
                storeroom.Update(this.userData.DatabaseKey);
                StoreroomID = storeroom.StoreroomId;
            }
            return storeroom.ErrorMessages;
        }
        #endregion
    }
}
