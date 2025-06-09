using Client.Models.MultiSitePartSearch;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.MultiSitePartSearch
{
    public class MultiSitePartSearchWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public MultiSitePartSearchWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<MultiSitePartSearchModel> GetMultiSitePartSearchData(string srchText)
        {
            List<MultiSitePartSearchModel> MultiSitePartSearchModelList = new List<MultiSitePartSearchModel>();
            MultiSitePartSearchModel objMultiSitePartSearchModel;


            DataContracts.Part part = new DataContracts.Part()
            {

                FilterText = srchText

            };

            List<DataContracts.Part> partList = part.RetrieveForSearchForMultipleSite(this.userData.DatabaseKey);


            foreach (var v in partList)
            {
                objMultiSitePartSearchModel = new MultiSitePartSearchModel();
                objMultiSitePartSearchModel.ClientLookupId = v.ClientLookupId;
                objMultiSitePartSearchModel.Description = v.Description;
                objMultiSitePartSearchModel.Quantity = v.QtyOnHand;
                objMultiSitePartSearchModel.Manufacturer = v.Manufacturer;
                objMultiSitePartSearchModel.ManufacturerId = v.ManufacturerId;               
                objMultiSitePartSearchModel.City = v.AddressCity;
                objMultiSitePartSearchModel.State = v.AddressState;
                objMultiSitePartSearchModel.Name = v.Name;
                MultiSitePartSearchModelList.Add(objMultiSitePartSearchModel);
            }
            return MultiSitePartSearchModelList;
        }
        #endregion Search
    }
}