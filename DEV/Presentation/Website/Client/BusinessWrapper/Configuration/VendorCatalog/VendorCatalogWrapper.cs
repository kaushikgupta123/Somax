using Client.Models.Configuration.VendorCatalog;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.Configuration.VendorCatalog
{
    public class VendorCatalogWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public VendorCatalogWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Search
        public List<VendorCatalogModel> GetVendorCatData(string srchText)
        {
            List<VendorCatalogModel> VendorCatalogModelList = new List<VendorCatalogModel>();
            VendorCatalogModel objVendorCatalogModel;
            DataContracts.PartMaster pm = new DataContracts.PartMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SearchCriteria = srchText,
            };
            List<DataContracts.PartMaster> partmasterList = pm.RetrieveVendorCatalogBySearchCriteria(this.userData.DatabaseKey);
            foreach(var v in partmasterList)
            {
                objVendorCatalogModel = new VendorCatalogModel();
                objVendorCatalogModel.ClientLookupId =v.ClientLookupId;
                objVendorCatalogModel.LongDescription =v.LongDescription;
                objVendorCatalogModel.Manufacturer =v.Manufacturer;
                objVendorCatalogModel.ManufacturerId =v.ManufacturerId;
                objVendorCatalogModel.Category =v.Category;
                objVendorCatalogModel.CM_Description =v.CM_Description;
                objVendorCatalogModel.UnitCost =v.UnitCost;
                objVendorCatalogModel.VI_PurchaseUOM =v.VI_PurchaseUOM;
                objVendorCatalogModel.VCI_PartNumber  =v.VCI_PartNumber;
                objVendorCatalogModel.VendorName =v.VendorName;
                objVendorCatalogModel.VendorClientLookupId =v.VendorClientLookupId;
                VendorCatalogModelList.Add(objVendorCatalogModel);
            }
            return VendorCatalogModelList;
        }
        #endregion Search
    }
}