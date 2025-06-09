using Client.Models.Configuration.ManufacturerMaster;
using Data.Database;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper
{
    public class ManufacturerMasterWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public ManufacturerMasterWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        List<string> errorMessage = new List<string>();
        string BodyHeader = string.Empty;
        string BodyContent = string.Empty;
        string FooterSignature = string.Empty;
        public List<ManufacturerModel> RetrieveManufacturerMasterDetails(bool activeStatus)
        {
            Data.DataContracts.ManufacturerMaster manufacturerMaster = new Data.DataContracts.ManufacturerMaster()
            {
                ManufacturerMasterId = -1,
                InactiveFlag = activeStatus
            };
            List<Data.DataContracts.ManufacturerMaster> manufacturerMasterlist = manufacturerMaster.ManufacturerMaster_RetrieveAll_ByInactiveFlag(this.userData.DatabaseKey);
            ManufacturerModel objManufacturerModel;
            List<ManufacturerModel> ManufacturerModelList = new List<ManufacturerModel>();
            foreach (var v in manufacturerMasterlist)
            {
                objManufacturerModel = new ManufacturerModel();
                objManufacturerModel.ManufacturerID = v.ManufacturerMasterId;
                objManufacturerModel.ClientLookupId = v.ClientLookupId;
                objManufacturerModel.Name = v.Name;
                objManufacturerModel.Inactive = v.InactiveFlag;
                ManufacturerModelList.Add(objManufacturerModel);
            }
            return ManufacturerModelList;
        }
        public List<String> AddOrEditManMasterRecord(ManufacturerModel manModel)
        {
            if (manModel.ManufacturerID == 0)
            {
                Data.DataContracts.ManufacturerMaster ManufacturerMaster = new Data.DataContracts.ManufacturerMaster();
                ManufacturerMaster.ManufacturerMasterId = -1;
                ManufacturerMaster.ClientId = userData.DatabaseKey.Personnel.ClientId;
                ManufacturerMaster.ClientLookupId = manModel.ClientLookupId;
                ManufacturerMaster.Name = manModel.Name;
                ManufacturerMaster.InactiveFlag = manModel.Inactive;
                ManufacturerMaster.Add(userData.DatabaseKey);
                return ManufacturerMaster.ErrorMessages;
            }
            else
            {
                Data.DataContracts.ManufacturerMaster mMaster = new Data.DataContracts.ManufacturerMaster()
                {
                    ManufacturerMasterId = manModel.ManufacturerID,//this.userData.DatabaseKey.Client.ClientId,
                };
                mMaster.Retrieve(userData.DatabaseKey);
                mMaster.ClientId = userData.DatabaseKey.Personnel.ClientId;
                mMaster.Name = manModel.Name;
                mMaster.InactiveFlag = manModel.Inactive;
                mMaster.Update(userData.DatabaseKey);
                return mMaster.ErrorMessages;
            }
        }
        public List<String> DeleteManMasterRecord(long ManufacturerID)
        {
            Data.DataContracts.ManufacturerMaster manufacturerMaster = new Data.DataContracts.ManufacturerMaster()
            {
                ManufacturerMasterId = ManufacturerID
            };
            manufacturerMaster.Delete(userData.DatabaseKey);
            return manufacturerMaster.ErrorMessages;
        }

    }
}