using Client.BusinessWrapper.Common;
using Client.Models.Configuration.CategoryMaster;

using Data.DataContracts;

using DataContracts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.Configuration
{
    public class CategoryMasterWrapper : CommonWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public CategoryMasterWrapper(UserData userData) : base(userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<CategoryMasterModel> GetCategoryList(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, string _clientLookupId = "", string _description = null, bool _inactiveFlag = false)
        {
            List<CategoryMasterModel> CategoryMasterModelList = new List<CategoryMasterModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            CategoryMasterModel categoryMasterModel;
            PartCategoryMaster category = new PartCategoryMaster();
            category.ClientId = userData.DatabaseKey.Client.ClientId;
            category.InactiveFlag = _inactiveFlag;
            category.orderbyColumn = orderbycol;
            category.orderBy = orderDir;
            category.offset1 = skip;
            category.nextrow = length;
            category.ClientLookupId = _clientLookupId;
            category.Description = _description;
            
            var cList = category.RetrieveChunkSearch(userData.DatabaseKey);

            foreach (var item in cList)
            {
                categoryMasterModel = new CategoryMasterModel();
                categoryMasterModel.PartCategoryMasterId = item.PartCategoryMasterId;
                categoryMasterModel.ClientLookupId = item.ClientLookupId;
                categoryMasterModel.Description = item.Description;
                categoryMasterModel.TotalCount = item.totalCount;
                CategoryMasterModelList.Add(categoryMasterModel);
            }

            return CategoryMasterModelList;
        }

        #endregion

        #region Add
        public List<String> AddOrEditCategoryMasterRecord(CategoryMasterModel manModel)
        {
            if (manModel.PartCategoryMasterId == 0)
            {
                Data.DataContracts.PartCategoryMaster CategoryMaster = new Data.DataContracts.PartCategoryMaster();
                CategoryMaster.PartCategoryMasterId = -1;
                CategoryMaster.ClientId = userData.DatabaseKey.Personnel.ClientId;
                CategoryMaster.ClientLookupId = manModel.ClientLookupId;
                CategoryMaster.Description = manModel.Description;
                CategoryMaster.InactiveFlag = manModel.InactiveFlag;
                CategoryMaster.Add(userData.DatabaseKey);
                return CategoryMaster.ErrorMessages;
            }
            else
            {
                Data.DataContracts.PartCategoryMaster cMaster = new Data.DataContracts.PartCategoryMaster()
                {
                    PartCategoryMasterId = manModel.PartCategoryMasterId,//this.userData.DatabaseKey.Client.ClientId,
                };
                cMaster.Retrieve(userData.DatabaseKey);
                cMaster.ClientId = userData.DatabaseKey.Personnel.ClientId;
                cMaster.Description = manModel.Description;
                cMaster.InactiveFlag = manModel.InactiveFlag;
                cMaster.Update(userData.DatabaseKey);
                return cMaster.ErrorMessages;
            }
        }
        #endregion

        #region Delete
        public List<String> DeleteCategoryMasterRecord(long PartCategoryMasterId)
        {
            Data.DataContracts.PartCategoryMaster categoryMasterMaster = new Data.DataContracts.PartCategoryMaster()
            {
                PartCategoryMasterId = PartCategoryMasterId
            };
            categoryMasterMaster.Delete(userData.DatabaseKey);
            return categoryMasterMaster.ErrorMessages;
        }
        #endregion


        public CategoryMasterModel RetrieveCategoryMasterDetailsById(long PartCategoryMasterId)
        {
            PartCategoryMaster partCategoryMaster = new PartCategoryMaster()
            {
                PartCategoryMasterId = PartCategoryMasterId,
                ClientId = userData.DatabaseKey.Client.ClientId
            };
            partCategoryMaster.Retrieve(userData.DatabaseKey);

            CategoryMasterModel categoryMasterModel;

            categoryMasterModel = new CategoryMasterModel();
            categoryMasterModel.PartCategoryMasterId = partCategoryMaster.PartCategoryMasterId;
            categoryMasterModel.ClientLookupId = partCategoryMaster.ClientLookupId;
            categoryMasterModel.Description = partCategoryMaster.Description;
            categoryMasterModel.InactiveFlag = partCategoryMaster.InactiveFlag;

            return categoryMasterModel;
        }
        #region V2-717 lookuplist
        public List<CategoryMasterModel> GetPartCategoryLookupList(string orderbycol = "", int length = 0, string orderDir = "", int skip = 0, string _clientLookupId = "", string _description = null)
        {
            List<CategoryMasterModel> CategoryMasterModelList = new List<CategoryMasterModel>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            CategoryMasterModel categoryMasterModel;
            PartCategoryMaster category = new PartCategoryMaster();
            category.ClientId = userData.DatabaseKey.Client.ClientId;
            category.orderbyColumn = orderbycol;
            category.orderBy = orderDir;
            category.offset1 = skip;
            category.nextrow = length;
            category.ClientLookupId = _clientLookupId;
            category.Description = _description;

            var cList = category.RetrievelookuplistChunkSearch(userData.DatabaseKey);

            foreach (var item in cList)
            {
                categoryMasterModel = new CategoryMasterModel();
                categoryMasterModel.PartCategoryMasterId = item.PartCategoryMasterId;
                categoryMasterModel.ClientLookupId = item.ClientLookupId;
                categoryMasterModel.Description = item.Description;
                categoryMasterModel.TotalCount = item.totalCount;
                CategoryMasterModelList.Add(categoryMasterModel);
            }

            return CategoryMasterModelList;
        }

        #endregion
    }
}