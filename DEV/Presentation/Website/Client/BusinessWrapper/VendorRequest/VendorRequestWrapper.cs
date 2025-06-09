using System;
using System.Collections.Generic;

using Client.Models.VendorRequest;
using Client.BusinessWrapper.Common;
using DataContracts;
using System.Linq;
using System.Web.Mvc;
using Common.Constants;

namespace Client.BusinessWrapper
{
    public class VendorRequestWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        internal List<string> errorMessage = new List<string>();
        public VendorRequestWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<VendorRequestSearchModel> GetVendorRequestchunkData(string orderbycol = "", string orderDir = "",  int skip = 0, int length = 0, int CaseNo = 0, string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _status = null,string srcData = "")
            {
            List<VendorRequestSearchModel> result = new List<VendorRequestSearchModel>();
            var model = new VendorRequestSearchModel();

            var vendorRequest = new VendorRequest
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                orderbyColumn = orderbycol,
                orderBy = orderDir,
                offset1 = skip,
                nextrow = length,
                CustomQueryDisplayId = CaseNo,
                Name = _name,
                AddressCity = _addresscity,
                AddressState = _addressstate,
                Status = _status,
                Type = _type,
                SearchText= srcData
            };
            var mList = vendorRequest.VendorRequestChunkSearch(userData.DatabaseKey, vendorRequest);
            foreach (var item in mList)
            {
                model = new VendorRequestSearchModel();

                model.VendorRequestId = item.VendorRequestId;
                model.Name = item.Name;
                model.City = item.AddressCity;
                model.State = item.AddressState;
                model.Type = item.Type;
                model.Status = item.Status;
                model.TotalCount = item.totalCount;
                result.Add(model);
            }
            
            return result;
        }
        #region Print
        public List<VendorRequestPrintModel> GetVendorRequestPrintData(string orderbycol = "", string orderDir = "", int skip = 0, int length = 0, int CaseNo = 0, string _name = null, string _addresscity = null, string _addressstate = null, string _type = null, string _status = null, string srcData = "")
        {
            List<VendorRequestPrintModel> result = new List<VendorRequestPrintModel>();
            var model = new VendorRequestPrintModel();

            var vendorRequest = new VendorRequest
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                orderbyColumn = orderbycol,
                orderBy = orderDir,
                offset1 = skip,
                nextrow = length,
                CustomQueryDisplayId = CaseNo,
                Name = _name,
                AddressCity = _addresscity,
                AddressState = _addressstate,
                Status = _status,
                Type = _type,
                SearchText = srcData
            };
            var mList = vendorRequest.VendorRequestChunkSearch(userData.DatabaseKey, vendorRequest);
            foreach (var item in mList)
            {
                model = new VendorRequestPrintModel();
                model.Name = item.Name;
                model.City = item.AddressCity;
                model.State = item.AddressState;
                model.Type = item.Type;
                model.Status = item.Status;
                result.Add(model);
            }

            return result;
        }
        #endregion
        #region Add /Edit
        public VendorRequestModel GetVendorRequestDetailsByVendorID(long VendorRequestId)
        {
            var vendorRequest = new VendorRequest
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorRequestId = VendorRequestId
            };
        
            vendorRequest.Retrieve(userData.DatabaseKey);
            VendorRequestModel vrm = new VendorRequestModel();
            vrm.VendorRequestId = vendorRequest.VendorRequestId;
            vrm.Address1 = vendorRequest.Address1;
            vrm.Address2 = vendorRequest.Address2;
            vrm.Address3 = vendorRequest.Address3;
            vrm.AddressCity = vendorRequest.AddressCity;
            vrm.AddressCountry = vendorRequest.AddressCountry;
            vrm.AddressPostCode = vendorRequest.AddressPostCode;
            vrm.AddressState = vendorRequest.AddressState;
            vrm.CustomerAccount = vendorRequest.CustomerAccount;
            vrm.EmailAddress = vendorRequest.EmailAddress;
            vrm.FaxNumber = vendorRequest.FaxNumber;
            vrm.FOBCode = vendorRequest.FOBCode;
            vrm.Name = vendorRequest.Name;
            vrm.PhoneNumber = vendorRequest.PhoneNumber;
            vrm.RemitAddress1 = vendorRequest.RemitAddress1;
            vrm.RemitAddress2 = vendorRequest.RemitAddress2;
            vrm.RemitAddress3 = vendorRequest.RemitAddress3;
            vrm.RemitCity = vendorRequest.RemitCity;
            vrm.RemitCountry = vendorRequest.RemitCountry;
            vrm.RemitPostCode = vendorRequest.RemitPostCode;
            vrm.RemitState = vendorRequest.RemitState;
            vrm.RemitUseBusiness = vendorRequest.RemitUseBusiness;
            vrm.Status = vendorRequest.Status;
            vrm.Terms = vendorRequest.Terms;
            vrm.Type = vendorRequest.Type;
            vrm.Website = vendorRequest.Website;
           
            return vrm;
        }

        public List<String> DeleteVendorRequest(long VendorRequestId)
        {
            var vendorRequest = new VendorRequest
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorRequestId = VendorRequestId
            };
            vendorRequest.Delete(userData.DatabaseKey);
            return vendorRequest.ErrorMessages;
        }
        public VendorRequestModel PopulateDropdownControls(VendorRequestModel objVendorRequestModel = null)
        {
            if (objVendorRequestModel == null)
            {
                objVendorRequestModel = new VendorRequestModel();
            }
            CommonWrapper cwrapper = new CommonWrapper(userData);
            var AllLookUpLists = cwrapper.GetAllLookUpList();
            if (AllLookUpLists != null)
            {
                List<DataContracts.LookupList> objLookupFOBCode = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_FOBCODE).ToList();
                if (objLookupFOBCode != null)
                {
                    objVendorRequestModel.LookupFOBList = objLookupFOBCode.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                List<DataContracts.LookupList> objLookupTerms = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TERMS).ToList();
                if (objLookupTerms != null)
                {
                    objVendorRequestModel.LookupTermList = objLookupTerms.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                List<DataContracts.LookupList> objLookupType = AllLookUpLists.Where(x => x.ListName == LookupListConstants.VENDOR_TYPE).ToList();
                if (objLookupType != null)
                {
                    objVendorRequestModel.LookupTypeList = objLookupType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            return objVendorRequestModel;
        }
        public List<String> AddOrEditVendorRequestRecord(VendorRequestModel vendorRequest)
        {
            VendorRequest vrm = new VendorRequest();
            vrm.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            vrm.ClientId = userData.DatabaseKey.Client.ClientId;
            vrm.Address1 = vendorRequest.Address1;
            vrm.Address2 = vendorRequest.Address2;
            vrm.Address3 = vendorRequest.Address3;
            vrm.AddressCity = vendorRequest.AddressCity;
            vrm.AddressCountry = vendorRequest.AddressCountry;
            vrm.AddressPostCode = vendorRequest.AddressPostCode;
            vrm.AddressState = vendorRequest.AddressState;
            vrm.CustomerAccount = vendorRequest.CustomerAccount;
            vrm.EmailAddress = vendorRequest.EmailAddress;
            vrm.FaxNumber = vendorRequest.FaxNumber;
            vrm.FOBCode = vendorRequest.FOBCode;
            vrm.Name = vendorRequest.Name;
            vrm.PhoneNumber = vendorRequest.PhoneNumber;
            vrm.RemitAddress1 = vendorRequest.RemitAddress1;
            vrm.RemitAddress2 = vendorRequest.RemitAddress2;
            vrm.RemitAddress3 = vendorRequest.RemitAddress3;
            vrm.RemitCity = vendorRequest.RemitCity;
            vrm.RemitCountry = vendorRequest.RemitCountry;
            vrm.RemitPostCode = vendorRequest.RemitPostCode;
            vrm.RemitState = vendorRequest.RemitState;
            vrm.RemitUseBusiness = vendorRequest.RemitUseBusiness;
            vrm.Status = VendorRequestConstant.Open;
            vrm.Terms = vendorRequest.Terms;
            vrm.Type = vendorRequest.Type;
            vrm.Website = vendorRequest.Website;
            if (vendorRequest.VendorRequestId == 0)
            {
                vrm.Create(userData.DatabaseKey);
            }
            else
            {
                vrm.VendorRequestId = vendorRequest.VendorRequestId;
                vrm.Update(userData.DatabaseKey);
            }
            return vrm.ErrorMessages;
        }

        #endregion

        #region Approval/deny Vendor Request
        public List<String> ApprovalVendorRequest(long VendorRequestId)
        {
            var vendorRequest = new VendorRequest
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorRequestId = VendorRequestId
            };
            vendorRequest.Retrieve(userData.DatabaseKey);
            if(vendorRequest.Status == VendorRequestConstant.Open)
            {
                vendorRequest.Status = VendorRequestConstant.Approved;
                vendorRequest.ApproveBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                vendorRequest.ApproveDate = System.DateTime.UtcNow;
                vendorRequest.Update(userData.DatabaseKey);

                #region  add new Vendor Entry in Vendor Table
                /////
                
                if (vendorRequest.ErrorMessages == null &&  VendorRequestConstant.Vendor_LOOKUP_AutoGenerateEnabled)
                {
                   var  newClientlookupId = CustomSequentialId.GetNextId(userData.DatabaseKey, AutoGenerateKey.Vendor_LOOKUP, userData.DatabaseKey.User.DefaultSiteId, "");
                    Vendor objvendor = new Vendor();
                    objvendor.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    objvendor.ClientId = userData.DatabaseKey.Client.ClientId;
                    objvendor.ClientLookupId = newClientlookupId;
                    objvendor.Address1 = vendorRequest.Address1;
                    objvendor.Address2 = vendorRequest.Address2;
                    objvendor.Address3 = vendorRequest.Address3;
                    objvendor.AddressCity = vendorRequest.AddressCity;
                    objvendor.AddressCountry = vendorRequest.AddressCountry;
                    objvendor.AddressPostCode = vendorRequest.AddressPostCode;
                    objvendor.AddressState = vendorRequest.AddressState;
                    objvendor.RemitAddress1 = vendorRequest.RemitAddress1;
                    objvendor.RemitAddress2 = vendorRequest.RemitAddress2;
                    objvendor.RemitAddress3 = vendorRequest.RemitAddress3;
                    objvendor.RemitCity = vendorRequest.RemitCity;
                    objvendor.RemitCountry = vendorRequest.RemitCountry;
                    objvendor.RemitPostCode = vendorRequest.RemitPostCode;
                    objvendor.RemitState = vendorRequest.RemitState;
                    objvendor.RemitUseBusiness = vendorRequest.RemitUseBusiness;
                    objvendor.CustomerAccount = vendorRequest.CustomerAccount;
                    objvendor.EmailAddress = vendorRequest.EmailAddress;
                    objvendor.FaxNumber = vendorRequest.FaxNumber;
                    objvendor.FOBCode = vendorRequest.FOBCode;
                    objvendor.Name = vendorRequest.Name;
                    objvendor.PhoneNumber = vendorRequest.PhoneNumber;
                    objvendor.Terms = vendorRequest.Terms;
                    objvendor.Type = vendorRequest.Type;
                    objvendor.Website = vendorRequest.Website;
                    objvendor.SourceId = vendorRequest.VendorRequestId;
                    objvendor.Create(userData.DatabaseKey);
                    
                }
                #endregion
            }
            return vendorRequest.ErrorMessages;
        }
        public List<String> DenyVendorRequest(long VendorRequestId)
        {
            var vendorRequest = new VendorRequest
            {
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorRequestId = VendorRequestId
            };
            vendorRequest.Retrieve(userData.DatabaseKey);
            if (vendorRequest.Status == VendorRequestConstant.Open)
            {
                vendorRequest.Status = VendorRequestConstant.Denied;
                vendorRequest.DenyBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                vendorRequest.DenyDate= System.DateTime.UtcNow;
                vendorRequest.Update(userData.DatabaseKey);
              
            }
            return vendorRequest.ErrorMessages;
        }
        #endregion
    }
}