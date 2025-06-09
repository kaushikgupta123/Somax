using Admin.Models;
using Admin.Models.Client;
using DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.BusinessWrapper
{
    public class CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public CommonWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public string GetHostedUrl()
        {
            string resetUrl;
            int iPort = HttpContext.Current.Request.Url.Port;
            if (iPort != 443 && iPort != 80)
            {
                string[] url = new string[3];
                url[0] = HttpContext.Current.Request.Url.Host;
                url[1] = HttpContext.Current.Request.Url.Port.ToString();
                resetUrl = string.Format(HttpContext.Current.Request.Url.Scheme + "://{0}:{1}", url);
            }
            else
            {
                string[] url = new string[2];
                url[0] = HttpContext.Current.Request.Url.Host;
                resetUrl = string.Format(HttpContext.Current.Request.Url.Scheme + "://{0}", url);
            }
            return resetUrl;
        }

        

        #region Text Search Implementation

        public List<MemorizeSearch> GetSearchOptionList(string tableName)
        {
            MemorizeSearch memorizeSearch = new MemorizeSearch()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ObjectName = tableName
            };

            var optionList = memorizeSearch.MemorizeSearchRetrieveForSearch(userData.DatabaseKey);
            return optionList;
        }

        public List<MemorizeSearch> ModifySearchOptionList(string tableName, string searchText, bool isClear)
        {
            MemorizeSearch memorizeSearch = new MemorizeSearch()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ObjectName = tableName,
                SearchText = searchText,
                IsClear = isClear
            };

            var optionList = memorizeSearch.MemorizeSearchRetrieveafterCreateAndDelete(userData.DatabaseKey);
            return optionList;
        }
        #endregion

        //#region LookUpList
        //public List<DataContracts.LookupList> GetAllLookUpList()
        //{
        //    List<DataContracts.LookupList> objLookUp = new Models.LookupList().RetrieveAll(userData.DatabaseKey);
        //    return objLookUp;
        //}
        //#endregion
        #region V2-389
        public List<DropDownModel> GetListFromConstVals(string ConstantType)
        {
            List<DropDownModel> values = new List<DropDownModel>();
            DropDownModel multiSelectModel;

            DataConstant dataConstant = new DataConstant();
            dataConstant.ConstantType = ConstantType;
            dataConstant.LocaleId = _dbKey.Localization;

            var dtValues = dataConstant.RetrieveLocaleForConstantType_V2(_dbKey);
            foreach (var item in dtValues)
            {
                multiSelectModel = new DropDownModel();
                multiSelectModel.value = item.Value;
                multiSelectModel.text = item.LocalName;
                values.Add(multiSelectModel);
            }
            return values;
        }

        public List<DropDownWithIdModel> GetListWithIdFromConstVals(string ConstantType)
        {
            List<DropDownWithIdModel> values = new List<DropDownWithIdModel>();
            DropDownWithIdModel multiSelectModel;

            DataConstant dataConstant = new DataConstant();
            dataConstant.ConstantType = ConstantType;
            dataConstant.LocaleId = _dbKey.Localization;

            var dtValues = dataConstant.RetrieveLocaleForConstantTypeWithId_V2(_dbKey);
            foreach (var item in dtValues)
            {
                multiSelectModel = new DropDownWithIdModel();
                multiSelectModel.value = item.DataConstantId;
                multiSelectModel.text = item.LocalName;
                values.Add(multiSelectModel);
            }
            return values;
        }


        #endregion

        #region V2-806
        public IEnumerable<SelectListItem> PopulateCustomQueryDisplay(string TableName, bool IsSelectAll = false)
        {

            List<KeyValuePair<string, string>> customList = new List<KeyValuePair<string, string>>();
            customList = CustomQueryDisplay.RetrieveQueryItemsByTableAndLanguage(userData.DatabaseKey, TableName, userData.Site.LocalizationLanguage, userData.Site.LocalizationCulture);
            if (IsSelectAll && customList.Count > 0)
            {
                customList.Insert(0, new KeyValuePair<string, string>("0", "--Select All--"));
            }
            return customList.Select(x => new SelectListItem { Text = x.Value, Value = x.Key }).OrderBy(x => x.Value);
        }
        #endregion

        #region V2-964
        public List<ClientModel> GetAllActiveClient()
        {
            DataContracts.Client client = new DataContracts.Client();
            var retData = client.RetrieveAllActiveClient(this.userData.DatabaseKey);
            ClientModel clientModel;
            List<ClientModel> ClientModelList = new List<ClientModel>();
            foreach (var item in retData)
            {
                clientModel = new ClientModel();
                clientModel.ClientId = item.ClientId;
                clientModel.Name = item.CompanyName;
                ClientModelList.Add(clientModel);
            }
            return ClientModelList;
        }
        public List<DataContracts.Site> AllActiveSiteList()
        {
            DataContracts.Site p = new DataContracts.Site();
            List<DataContracts.Site> Mlist = new List<DataContracts.Site>();
            Mlist = p.RetrieveForAllActiveLookupListV2(userData.DatabaseKey);
            return Mlist;
        }
        #endregion
    }
}