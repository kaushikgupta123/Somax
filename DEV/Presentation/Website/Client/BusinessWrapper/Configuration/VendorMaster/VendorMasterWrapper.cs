using Client.Models.Configuration.VendorMaster;
using DataContracts;
using System;
using System.Collections.Generic;

namespace Client.BusinessWrapper.Configuration.VendorMaster
{
    public class VendorMasterWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public VendorMasterWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        internal List<VendorMasterModel> RetrieveAllMasterList(bool InactiveFlag)
        {
            List<VendorMasterModel> mList = new List<VendorMasterModel>();
            VendorMasterModel objModel;

            DataContracts.VendorMaster VendorMasterSearch = new DataContracts.VendorMaster();
            VendorMasterSearch.InactiveFlag = InactiveFlag;
            List<DataContracts.VendorMaster> rList = VendorMasterSearch.VendorMaster_RetrieveAll_ByInactiveFlag(this.userData.DatabaseKey, userData.Site.TimeZone);

            foreach (var item in rList)
            {
                objModel = new VendorMasterModel();
                objModel.VendorMasterId = item.VendorMasterId;
                objModel.ClientLookupId = item.ClientLookupId;
                objModel.Name = item.Name;
                objModel.ExVendorSiteCode = item.ExVendorSiteCode;
                objModel.AddressCity = item.AddressCity;
                objModel.AddressState = item.AddressState;
                objModel.Type = item.Type;
                objModel.InactiveFlag = item.InactiveFlag;
                objModel.IsExternal = item.IsExternal;
                mList.Add(objModel);
            }
            return mList;
        }
        #endregion

        #region Details

        public VendorMasterModel GetVendorMasterDetails(long ObjectId)
        {
            VendorMasterModel obj = new VendorMasterModel();
            DataContracts.VendorMaster VM = new DataContracts.VendorMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorMasterId = ObjectId
            };
            VM.Retrieve(userData.DatabaseKey);
            obj.ClientLookupId = VM.ClientLookupId;
            obj.InactiveFlag = VM.InactiveFlag;
            obj.Name = VM.Name;
            obj.PhoneNumber = VM.PhoneNumber;
            obj.FaxNumber = VM.FaxNumber;
            obj.EmailAddress = VM.EmailAddress;
            obj.RemitUseBusiness = VM.RemitUseBusiness;
            obj.Address1 = VM.Address1;
            obj.Address2 = VM.Address2;
            obj.Address3 = VM.Address3;
            obj.AddressCity = VM.AddressCity;
            obj.AddressState = VM.AddressState;
            obj.AddressPostCode = VM.AddressPostCode;
            obj.AddressCountry = VM.AddressCountry;

            obj.Terms = VM.Terms;
            obj.Type = VM.Type;
            obj.FOBCode = VM.FOBCode;
            obj.IsExternal = VM.IsExternal;
            if (VM.RemitUseBusiness)
            {
                obj.RemitAddress1 = VM.RemitAddress1;
                obj.RemitAddress2 = VM.RemitAddress2;
                obj.RemitAddress3 = VM.RemitAddress3;
                obj.RemitAddressCity = VM.RemitAddressCity;
                obj.RemitAddressState = VM.RemitAddressState;
                obj.RemitAddressPostCode = VM.RemitAddressPostCode;
                obj.RemitAddressCountry = VM.RemitAddressCountry;
            }
            else
            {
                obj.RemitAddress1 = VM.RemitAddress1;
                obj.RemitAddress2 = VM.RemitAddress2;
                obj.RemitAddress3 = VM.RemitAddress3;
                obj.RemitAddressCity = VM.RemitAddressCity;
                obj.RemitAddressState = VM.RemitAddressState;
                obj.RemitAddressPostCode = VM.RemitAddressPostCode;
                obj.RemitAddressCountry = VM.RemitAddressCountry;

            }
            return obj;
        }


        #endregion Details

        #region Add/Update
        internal List<String> UpdateVendorMaster(VendorMasterModel obj, ref long VendorMasterId)
        {
            DataContracts.VendorMaster VM = new DataContracts.VendorMaster()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                VendorMasterId = obj.VendorMasterId
            };
            VM.Retrieve(userData.DatabaseKey);
            VM.InactiveFlag = obj.InactiveFlag;
            VM.Name = obj.Name;
            VM.PhoneNumber = obj.PhoneNumber == null ? string.Empty : obj.PhoneNumber;
            VM.FaxNumber = obj.FaxNumber == null ? string.Empty : obj.FaxNumber;

            VM.Terms = obj.Terms == null ? string.Empty : obj.Terms;
            VM.FOBCode = obj.FOBCode == null ? string.Empty : obj.FOBCode;
            VM.Type = obj.Type == null ? string.Empty : obj.Type;

            VM.EmailAddress = obj.EmailAddress == null ? string.Empty : obj.EmailAddress;
            VM.RemitUseBusiness = obj.RemitUseBusiness;
            VM.Address1 = obj.Address1 == null ? string.Empty : obj.Address1;
            VM.Address2 = obj.Address2 == null ? string.Empty : obj.Address2;
            VM.Address3 = obj.Address3 == null ? string.Empty : obj.Address3;
            VM.AddressCity = obj.AddressCity == null ? string.Empty : obj.AddressCity;
            VM.AddressState = obj.AddressState == null ? string.Empty : obj.AddressState;
            VM.AddressPostCode = obj.AddressPostCode == null ? string.Empty : obj.AddressPostCode;
            VM.AddressCountry = obj.AddressCountry == null ? string.Empty : obj.AddressCountry;


            VM.RemitAddress1 = obj.RemitUseBusiness == true ? VM.Address1 : obj.RemitAddress1 == null ? string.Empty : obj.RemitAddress1;
            VM.RemitAddress2 = obj.RemitUseBusiness == true ? VM.Address2 : obj.RemitAddress2 == null ? string.Empty : obj.RemitAddress2;
            VM.RemitAddress3 = obj.RemitUseBusiness == true ? VM.Address3 : obj.RemitAddress3 == null ? string.Empty : obj.RemitAddress3;
            VM.RemitAddressCity = obj.RemitUseBusiness == true ? VM.AddressCity : obj.RemitAddressCity == null ? string.Empty : obj.RemitAddressCity;
            VM.RemitAddressState = obj.RemitUseBusiness == true ? VM.AddressState : obj.RemitAddressState == null ? string.Empty : obj.RemitAddressState;
            VM.RemitAddressPostCode = obj.RemitUseBusiness == true ? VM.AddressPostCode : obj.RemitAddressPostCode == null ? string.Empty : obj.RemitAddressPostCode;
            VM.RemitAddressCountry = obj.RemitUseBusiness == true ? VM.AddressCountry : obj.RemitAddressCountry == null ? string.Empty : obj.RemitAddressCountry;

            VM.Update_VendorMasterDetails(userData.DatabaseKey);
            VendorMasterId = obj.VendorMasterId;
            return VM.ErrorMessages;
        }
        internal List<String> AddVendorMaster(VendorMasterModel obj, ref long VendorMasterId)
        {
            DataContracts.VendorMaster VendorMaster = new DataContracts.VendorMaster
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId
            };
            VendorMaster.ClientLookupId = obj.ClientLookupId;
            VendorMaster.Name = obj.Name;
            VendorMaster.Add_VendorMaster(this.userData.DatabaseKey);
            VendorMasterId = VendorMaster.VendorMasterId;
            return VendorMaster.ErrorMessages;
        }
        internal List<VMAddGridModel> PopulateVMAddGrid()
        {
            VMAddGridModel objVMAddGridModel;
            List<VMAddGridModel> VMAddGridModelList = new List<VMAddGridModel>();
            DataContracts.VendorMaster vendorMaster = new DataContracts.VendorMaster();
            vendorMaster.ClientId = this.userData.DatabaseKey.User.ClientId;
            vendorMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<DataContracts.VendorMaster> VendorMasterlist = vendorMaster.RetrieveVendorFromVM(this.userData.DatabaseKey);
            foreach (var item in VendorMasterlist)
            {
                objVMAddGridModel = new VMAddGridModel();
                objVMAddGridModel.VendorMasterId = item.VendorMasterId;
                objVMAddGridModel.ClientLookupId = item.ClientLookupId;
                objVMAddGridModel.Name = item.Name;
                objVMAddGridModel.AddressCity = item.AddressCity;
                objVMAddGridModel.AddressState = item.AddressState;
                objVMAddGridModel.Type = item.Type;
                VMAddGridModelList.Add(objVMAddGridModel);
            }
            return VMAddGridModelList;
        }
        public List<VendorAddFromVMGridErrorModel> AddVendorFromVMGrid(List<long> VendorsIds)
        {
            List<VendorAddFromVMGridErrorModel> errorList = new List<VendorAddFromVMGridErrorModel>();
            DataContracts.VendorMaster vendorMaster = new DataContracts.VendorMaster();
            VendorAddFromVMGridErrorModel objVendorAddFromVMGridErrorModel;
            vendorMaster.ClientId = userData.DatabaseKey.Client.ClientId;
            vendorMaster.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            foreach (var id in VendorsIds)
            {
                vendorMaster.VendorMasterId = id;
                vendorMaster.Retrieve(userData.DatabaseKey);
                vendorMaster.Add_Vendor(userData.DatabaseKey);
                if (vendorMaster.ErrorMessages != null && vendorMaster.ErrorMessages.Count > 0)
                {
                    objVendorAddFromVMGridErrorModel = new VendorAddFromVMGridErrorModel();
                    objVendorAddFromVMGridErrorModel.ClinetLookUpId = vendorMaster.ClientLookupId;
                    objVendorAddFromVMGridErrorModel.ErrorMessage = vendorMaster.ErrorMessages;
                    errorList.Add(objVendorAddFromVMGridErrorModel);
                }
            }
            return errorList;
        }
        #endregion

        #region Change-VendorId Pop-Up
        internal List<string> ChangeVendorID(ChangeVendorModel model, ref long UpdateIndex)
        {
            DataContracts.VendorMaster VendorMaster = new DataContracts.VendorMaster();
            VendorMaster.ClientId = userData.DatabaseKey.Client.ClientId;
            VendorMaster.VendorMasterId = model.VendorMasterId;
            VendorMaster.Retrieve(userData.DatabaseKey);
            VendorMaster.ClientLookupId = model.ClientLookupId;
            VendorMaster.ChangeClientLookupId(userData.DatabaseKey);
            UpdateIndex = VendorMaster.UpdateIndex;
            return VendorMaster.ErrorMessages;
        }
        #endregion

        #region CreatedLastupdate
        internal DataContracts.VendorMaster CreatedAndLastupdate(long ObjectId)
        {
            DataContracts.VendorMaster VM = new DataContracts.VendorMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorMasterId = ObjectId
            };
            VM.RetrieveByFK(userData.DatabaseKey);
            return VM;
        }
        #endregion
    }
}