using Client.Common;
using Client.Models.Configuration.ConfigCraft;
using DataContracts;
using System.Collections.Generic;

namespace Client.BusinessWrapper.Configuration.ConfigCraft
{
    public class CraftWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public CraftWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        internal List<CraftModel> GetCraftList()
        {
            List<CraftModel> craftModelList = new List<CraftModel>();
            CraftModel craftModel;
            DataContracts.Craft craft = new DataContracts.Craft()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };
            List<DataContracts.Craft> craftList = craft.RetriveAllForSite(this.userData.DatabaseKey);
            foreach (var item in craftList)
            {
                craftModel = new CraftModel();
                craftModel.CraftId = item.CraftId;
                craftModel.ClientLookupId = item.ClientLookupId;
                craftModel.Description = item.Description;
                craftModel.ChargeRate = item.ChargeRate;
                craftModel.InactiveFlag = item.InactiveFlag;
                craftModelList.Add(craftModel);
            }
            return craftModelList;
        }
        #endregion
        #region AddEdit
        internal Craft AddEditCraft(CraftModel objCraftModel)
        {
            DataContracts.Craft retCraft = new DataContracts.Craft();
            if (objCraftModel.CraftId == 0)
            {
                DataContracts.Craft craft = new DataContracts.Craft()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId
                };
                RetrievePageControl(objCraftModel, craft);
                craft.Create(this.userData.DatabaseKey);
                retCraft = craft;
            }
            else
            {
                DataContracts.Craft craft = new DataContracts.Craft()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    CraftId = objCraftModel.CraftId
                };
                
                craft.Retrieve(this.userData.DatabaseKey);
                RetrievePageControl(objCraftModel, craft);
                craft.UpdateIndex = objCraftModel.UpdateIndex;
                craft.Update(this.userData.DatabaseKey);
                retCraft = craft;
            }

            return retCraft;
        }
        private void RetrievePageControl(CraftModel objCraftModel, DataContracts.Craft craft)
        {
            craft.ClientLookupId = objCraftModel.ClientLookupId;
            craft.Description = objCraftModel.Description;
            craft.ChargeRate = objCraftModel.ChargeRate ?? 0;
            craft.InactiveFlag = objCraftModel.InactiveFlag;

            // NOT Setting SiteId at this time
            //craft.SiteId = this.UserData.DatabaseKey.User.DefaultSiteId;   // Defualt to a "shared" one for now
            craft.SiteId = 0;
            craft.AreaId = 0;
            craft.DepartmentId = 0;
            craft.StoreroomId = 0;
        }
        internal List<string> ValidateAddUpdate(CraftModel craft, string mode)
        {
            Craft Craft_Valid = new Craft()
            {
                ClientId = userData.DatabaseKey.Client.ClientId
            };

            if (mode == ActionModeEnum.add.ToString())
            {
                Craft_Valid.SiteId = 0;   // Default to zero for now shared
            }
            else
            {
                Craft_Valid.CraftId = craft.CraftId;
                Craft_Valid.Retrieve(userData.DatabaseKey);
            };

            // Client Lookup Id 
            Craft_Valid.ClientLookupId = craft.ClientLookupId.Trim();

            Craft_Valid.Description = craft.Description;
            // Inactive Flag 
            Craft_Valid.InactiveFlag = craft.InactiveFlag;

            // Validate 
            if (mode == ActionModeEnum.add.ToString())
                Craft_Valid.ValidateAdd(userData.DatabaseKey);
            else
                Craft_Valid.ValidateSave(userData.DatabaseKey);
            return Craft_Valid.ErrorMessages;

        }
        public CraftModel GetCraftByCraftId(long craftId)
        {
            CraftModel objCraftModel = new CraftModel();
            objCraftModel.CraftId = craftId;
            DataContracts.Craft craft = new DataContracts.Craft()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                CraftId = objCraftModel.CraftId
            };

            craft.Retrieve(this.userData.DatabaseKey);

            if (craft != null)
            {
                objCraftModel.CraftId = craft.CraftId;
                objCraftModel.ClientLookupId = craft.ClientLookupId;
                objCraftModel.Description = craft.Description;
                objCraftModel.ChargeRate = craft.ChargeRate;
                objCraftModel.UpdateIndex = craft.UpdateIndex;
                objCraftModel.InactiveFlag = craft.InactiveFlag;                
            }
            return objCraftModel;
        }
        #endregion
        #region Delete
        internal DataContracts.Craft DeleteCraft(long CraftId)
        {
            DataContracts.Craft craft = new DataContracts.Craft()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                CraftId = CraftId
            };

            // SOM-844
            // Delete     if no personnel or timecards reference
            // Inactivate if personnel or timecard references
            craft.Delete_Inactivate(this.userData.DatabaseKey);
            return craft;

        }
        #endregion
    }
}