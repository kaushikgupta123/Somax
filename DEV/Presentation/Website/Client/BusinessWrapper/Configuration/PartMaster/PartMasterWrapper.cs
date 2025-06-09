using Client.BusinessWrapper.Common;
using Client.Models.Configuration.PartMaster;
using Common.Constants;
using Data.DataContracts;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.Configuration
{
    public class PartMasterWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public PartMasterWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        public List<PartMasterModel> GetPartMasterData(bool inactiveFlag)
        {
            DataContracts.PartMaster partMaster = new DataContracts.PartMaster();
            partMaster.ClientId = userData.DatabaseKey.Client.ClientId;
            List<DataContracts.PartMaster> partList = partMaster.PartMaster_RetrieveAll_ByInactiveFlag(this.userData.DatabaseKey, userData.Site.TimeZone, inactiveFlag);
            //partList = partList.Where(x => x.InactiveFlag == inactiveFlag).ToList();
            List<PartMasterModel> PartMasterModelList = new List<PartMasterModel>();
            PartMasterModel objpartmastermodel;
            foreach (var p in partList)
            {
                objpartmastermodel = new PartMasterModel();
                objpartmastermodel.ClientLookupId = p.ClientLookupId;
                objpartmastermodel.Category = p.Category;
                objpartmastermodel.EXPartId = p.EXPartId;
                objpartmastermodel.ExUniqueId = p.ExUniqueId;
                objpartmastermodel.ImageURL = p.ImageURL;
                objpartmastermodel.InactiveFlag = p.InactiveFlag;
                objpartmastermodel.LongDescription = p.LongDescription;
                objpartmastermodel.Manufacturer = p.Manufacturer;
                objpartmastermodel.ManufacturerId = p.ManufacturerId;
                objpartmastermodel.OEMPart = p.OEMPart;
                objpartmastermodel.PartMasterId = p.PartMasterId;
                objpartmastermodel.ShortDescription = p.ShortDescription;
                objpartmastermodel.PartId = p.PartId;
                objpartmastermodel.Manufacturer = p.Manufacturer;
                objpartmastermodel.ManufacturerId = p.ManufacturerId;
                objpartmastermodel.CategoryDescription = p.CategoryDescription;
                //objpartmastermodel.Description = p.Description;
                objpartmastermodel.Inactive = p.Inactive;

                PartMasterModelList.Add(objpartmastermodel);
            }
            return PartMasterModelList;
        }


        public PartMasterModel GetPartMasterDetails(long partMasterId)
        {
            PartMasterModel objPartMasterModel = new PartMasterModel();
            DataContracts.PartMaster partmaster = new DataContracts.PartMaster()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartMasterId = partMasterId
            };
            partmaster.Retrieve(userData.DatabaseKey);
            objPartMasterModel.ClientLookupId = partmaster.ClientLookupId;
            objPartMasterModel.PartMasterId = partmaster.PartMasterId;
            objPartMasterModel.EXPartId = partmaster.EXPartId;
            objPartMasterModel.UPCCode = partmaster.UPCCode;
            objPartMasterModel.Manufacturer = partmaster.Manufacturer;
            objPartMasterModel.ManufacturerId = partmaster.ManufacturerId;
            objPartMasterModel.ShortDescription = partmaster.ShortDescription;
            objPartMasterModel.LongDescription = partmaster.LongDescription;
            objPartMasterModel.UnitCost = partmaster.UnitCost;
            objPartMasterModel.UnitOfMeasure = partmaster.UnitOfMeasure;
            objPartMasterModel.Category = partmaster.Category;
            objPartMasterModel.OEMPart = partmaster.OEMPart;
            objPartMasterModel.ImageURL = partmaster.ImageURL;
            objPartMasterModel.InactiveFlag = partmaster.InactiveFlag;
            objPartMasterModel.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            //issueunit??
            return objPartMasterModel;
        }



        #region Add_Edit PartMaster
        public List<String> AddOrEditPartMaster(PartMasterModel partMasterModel, ref string Mode, ref long partMasterId)
        {
            DataContracts.PartMaster pm = new DataContracts.PartMaster();
            if (partMasterModel.PartMasterId == 0)
            {
                Mode = "add";
                pm.ClientLookupId = partMasterModel.ClientLookupId.Trim();
                pm.ShortDescription = partMasterModel.ShortDescription;
                pm.Manufacturer = partMasterModel.Manufacturer;
                pm.ManufacturerId = partMasterModel.ManufacturerId;
                pm.Category = partMasterModel.Category;
                pm.CategoryDescription = partMasterModel.CategoryDescription;
                pm.UnitOfMeasure = partMasterModel.UnitOfMeasure;
                pm.UnitCost = partMasterModel.UnitCost ?? 0;
                pm.Add_PartMaster(this.userData.DatabaseKey);
                if (pm.ErrorMessages.Count == 0)
                {
                    partMasterId = pm.PartMasterId;
                }
                return pm.ErrorMessages;
            }
            else
            {
                Mode = "update";
                DataContracts.PartMaster pmEdit = new DataContracts.PartMaster()
                {
                    ClientId = this.userData.DatabaseKey.Client.ClientId,
                    PartMasterId = partMasterModel.PartMasterId,
                };
                pmEdit.Retrieve(userData.DatabaseKey);
                pmEdit.ClientLookupId = partMasterModel.ClientLookupId;
                pmEdit.ShortDescription = partMasterModel.ShortDescription == null ? string.Empty : partMasterModel.ShortDescription;
                pmEdit.Manufacturer = partMasterModel.Manufacturer ?? string.Empty;
                pmEdit.ManufacturerId = partMasterModel.ManufacturerId ?? string.Empty;
                pmEdit.Category = partMasterModel.Category ?? string.Empty;
                pmEdit.CategoryDescription = partMasterModel.CategoryDescription ?? string.Empty;
                pmEdit.UnitOfMeasure = partMasterModel.UnitOfMeasure ?? string.Empty;
                pmEdit.UnitCost = partMasterModel.UnitCost ?? 0;
                pmEdit.LongDescription = partMasterModel.LongDescription == null ? string.Empty : partMasterModel.LongDescription;
                pmEdit.UPCCode = partMasterModel.UPCCode == null ? string.Empty : partMasterModel.UPCCode;
                pmEdit.OEMPart = partMasterModel.OEMPart;
                pmEdit.EXPartId = partMasterModel.EXPartId ?? 0;
                pmEdit.InactiveFlag = partMasterModel.InactiveFlag;
                pmEdit.Mode = "Check";
                // Validation 
                //pmEdit.SaveValidation(userData.DatabaseKey);
                if (pmEdit.IsValid)
                {
                    pmEdit.UpdatePartMasterDetails(userData.DatabaseKey);
                    // pmEdit.Update(userData.DatabaseKey);
                }
                if (pmEdit.ErrorMessages.Count == 0)
                {
                    partMasterId = pmEdit.PartMasterId;
                }
                return pmEdit.ErrorMessages;
            }
        }

        public List<PartCategoryMaster> GetCategoryList()
        {
            PartCategoryMaster categoryMaster = new PartCategoryMaster();
            PartMasterModel partMasterModel = new PartMasterModel();
            List<PartCategoryMaster> obj_CMlist = categoryMaster.RetrieveAll(this.userData.DatabaseKey).Where(x => x.InactiveFlag == false).ToList();
            return obj_CMlist;
        }

        public List<DataContracts.LookupList> GetUnitofMeasureList()
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            var AllLookUps = commonWrapper.GetAllLookUpList();
            return AllLookUps;
        }
        #endregion Add_Edit EqpMaster
        /*public List<DataContracts.LookupList> GetCategoryDescriptionList()
        {
            List<LookupList> obj_Lookuplist = new LookupList().RetrieveAll(this.userData.DatabaseKey);
            PartCategoryMaster categoryMaster = new PartCategoryMaster();
            List<DataContracts.LookupList> objType = new Models.LookupList().RetrieveAll(userData.DatabaseKey).Where(x => x.ListName == LookupListConstants.EQUIP_TYPE).ToList();
            return objType;
        }*/
        public List<ManufacturerMasterModel> PopulateManufacturerIds(int pageNumber, int ResultsPerPage, string OrderColumn, string OrderDirection, string ClientLookupId, string Name)
        {
            ManufacturerMasterModel objManufacturerMasterModel;
            List<ManufacturerMasterModel> ManufacturerMasterModelList = new List<ManufacturerMasterModel>();


            DataContracts.LookupListResultSet.ManufacturerLookupListResultSet MMList
                = new DataContracts.LookupListResultSet.ManufacturerLookupListResultSet();

            DataContracts.LookupListResultSet.ManufacturerLookupListResultSetTransactionParameters settings = new DataContracts.LookupListResultSet.ManufacturerLookupListResultSetTransactionParameters()
            {
                PageNumber = pageNumber, // Note that the displayed page number is 1 through N, but the stored proc assumes page 0 is the first page.
                ResultsPerPage = ResultsPerPage,//this.userData.DatabaseKey.User.ResultsPerPage,
                OrderColumn = OrderColumn,
                OrderDirection = OrderDirection
            };

            settings.ClientLookupId = ClientLookupId;
            settings.Name = Name;

            MMList.UpdateSettings(userData.DatabaseKey, settings);
            MMList.RetrieveResults();

            foreach (var item in MMList.Items)
            {
                objManufacturerMasterModel = new ManufacturerMasterModel();
                objManufacturerMasterModel.ManufacturerMasterId = item.ManufacturerMasterId;
                objManufacturerMasterModel.ClientLookupId = item.ClientLookupId;
                objManufacturerMasterModel.Name = item.Name;
                objManufacturerMasterModel.TotalCount = MMList.Count;
                ManufacturerMasterModelList.Add(objManufacturerMasterModel);
            }
            return ManufacturerMasterModelList;

        }


        //public string GetPartMasterImageUrl(long PartMasterId)
        //{
        //    string imageurl = string.Empty;
        //    bool lExternal = false;
        //    string sasToken = string.Empty;
        //    PartMaster partMaster = new PartMaster()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //       // SiteId = userData.DatabaseKey.User.DefaultSiteId,
        //        CallerUserName = userData.DatabaseKey.Client.CallerUserName,
        //        PartMasterId = PartMasterId
        //    };

        //    Attachment attach = new Attachment()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        ObjectName = "PartMaster",
        //        ObjectId = PartMasterId,
        //        Profile = true,
        //        Image = true
        //    };
        //    List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
        //    if (AList.Count > 0)
        //    {
        //        lExternal = AList.First().External;
        //        imageurl = AList.First().AttachmentURL;
        //    }

        //    else
        //    {
        //        imageurl = partMaster.ImageURL;
        //        if (imageurl == null || imageurl == "")
        //        {
        //            imageurl = "No Image";
        //            lExternal = true;
        //        }
        //        else if (imageurl.Contains("somaxclientstorage"))
        //        {
        //            lExternal = false;
        //        }
        //        else
        //        {
        //            lExternal = true;
        //        }
        //    }
        //    if (lExternal)
        //    {
        //        if (imageurl == "No Image")
        //        {
        //            const string UploadDirectory = "../Images/DisplayImg/";
        //            const string ThumbnailFileName = "NoImage.jpg";
        //            imageurl = UploadDirectory + ThumbnailFileName;
        //        }
        //        else
        //        {
        //            imageurl = imageurl;
        //        }
        //    }
        //    else // SOMAX Storage 
        //    {
        //        AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //        sasToken = aset.GetSASUrlClient(userData.DatabaseKey.Client.ClientId,  imageurl);
        //        if (sasToken == null || sasToken == "" || imageurl.Contains("No Image"))
        //        {
        //            const string UploadDirectory = "../Images/DisplayImg/";
        //            const string ThumbnailFileName = "NoImage.jpg";
        //            imageurl = UploadDirectory + ThumbnailFileName;
        //        }
        //        else
        //        {
        //            imageurl = sasToken;
        //        }
        //    }
        //    return imageurl;
        //}

        #region Photos
        public void DeleteImage(long PartMasterId, string TableName, bool Profile, bool Image, ref string rtrMsg)
        { // Check if there is a profile image attachment record for the object 
            Attachment attach = new Attachment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                ObjectName = "PartMaster",
                ObjectId = PartMasterId,
                Profile = Profile,
                Image = Image
            };
            attach.ClientId = userData.DatabaseKey.Client.ClientId;
            List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
            if (AList.Count > 0)
            {
                // Profile Image Attachment Record Exists
                string image_url = AList.First().AttachmentURL;
                bool external = AList.First().External;
                attach.AttachmentId = AList.First().AttachmentId;
                attach.Delete(userData.DatabaseKey);
                // If the image is NOT external then we delete the image 
                if (!external)
                {
                    AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
                    aset.DeleteBlobByURL(image_url);
                    rtrMsg = "Success";
                }
                else
                {
                    rtrMsg = "External";
                }
            }
            else
            {
                // We still may have URL refrences in the Equipment. 
                // If so we need to delete the URL from the Equipment
                PartMaster PartMaster = new PartMaster();
                PartMaster.ClientId = userData.DatabaseKey.Client.ClientId;               
                PartMaster.PartMasterId = PartMasterId;
                PartMaster.Retrieve(userData.DatabaseKey);
                if (PartMaster.ImageURL != "")
                {
                    PartMaster.ImageURL = ""; //Garima
                    PartMaster.Update(userData.DatabaseKey);
                    rtrMsg = "Success";
                }
            }

        }
        #endregion
    }
}