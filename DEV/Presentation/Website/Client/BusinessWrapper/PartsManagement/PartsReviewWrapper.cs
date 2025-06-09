using Client.Models.PartsManagement.PartsReview;
using DataContracts;
using System.Collections.Generic;

namespace Client.BusinessWrapper.PartsManagement
{
    public class PartsReviewWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;

        public PartsReviewWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<PartsReviewModel> GetPartsReview(string SearchText)
        {
            PartsReviewModel partsReviewModel;
            List<PartsReviewModel> ListPartsReviewModel = new List<PartsReviewModel>();
            PartMaster pm = new PartMaster();
            pm.CallerUserInfoId = this.userData.DatabaseKey.User.UserInfoId;

            pm.CallerUserName = this.userData.DatabaseKey.UserName;
            pm.Siteid = userData.DatabaseKey.User.DefaultSiteId;
            pm.SearchCriteria = SearchText;

            List<PartMaster> listPartMaster = pm.PartmasterReview_RetrieveBySearchCriteria(this.userData.DatabaseKey);
            foreach(var part in listPartMaster)
            {
                partsReviewModel = new PartsReviewModel();
                partsReviewModel.PartId = part.PartMasterId;
                partsReviewModel.ClientLookupId = part.ClientLookupId;
                partsReviewModel.LongDescription = part.LongDescription;
                partsReviewModel.Manufacturer = part.Manufacturer;
                partsReviewModel.ManufacturerId = part.ManufacturerId;
                partsReviewModel.Category = part.Category;
                partsReviewModel.CategoryMasterDescription = part.CategoryMasterDescription;
                partsReviewModel.ImageURL = part.ImageURL; //V2-1215
                ListPartsReviewModel.Add(partsReviewModel);
            }
            return ListPartsReviewModel;
        }

    }
}