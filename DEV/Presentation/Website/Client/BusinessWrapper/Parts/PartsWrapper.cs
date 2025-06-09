using Client.BusinessWrapper.Common;
using Client.Common;
using Client.Controllers;
using Client.Localization;
using Client.Models;
using Client.Models.Common;
using Client.Models.Parts;
using Client.Models.Parts.UIConfiguration;

using Common.Constants;
using Common.Enumerations;
using Common.Extensions;

using Database.Business;

using DataContracts;

using DevExpress.XtraRichEdit.Model;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

using static DevExpress.Office.PInvoke.Win32;

namespace Client.BusinessWrapper
{
    public class PartsWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();

        public long RequestPartId { get; set; }
        public long IssuePartId { get; set; }
        public long RequestSiteId { get; set; }
        public long IssueSiteId { get; set; }
        Part RequestPart { get; set; }
        Part IssuePart { get; set; }
        PartTransfer partTransfer { get; set; }
        PartStoreroom partStore { get; set; }
        Site RequestSite { get; set; }
        Site IssueSite { get; set; }

        public PartsWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Common

        public List<PartModel> GetPartChunkList(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string PartClientLookUpId = "", string Description = "", string Section = "", decimal MinimumQuantity = 0, decimal OnHandQuantity = 0, string Manufacturer = "", string ManufacturerID = "", string StockType = "", string Row = "", string Bin = "", string SearchText = "", string Shelf = "", string PlaceArea = "")
        {
            Part part = new Part();
            PartModel PartsModel;
            List<PartModel> PartsModelList = new List<PartModel>();
            List<string> StockTypeList = new List<string>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.CustomQueryDisplayId = CustomQueryDisplayId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.ClientLookupId = PartClientLookUpId;
            part.Description = Description;
            part.Section = Section;
            part.QtyOnHand = OnHandQuantity;
            part.QtyReorderLevel = MinimumQuantity;
            part.Manufacturer = Manufacturer;
            part.ManufacturerId = ManufacturerID;
            part.StockType = StockType;
            part.Row = Row;
            part.Bin = Bin;
            part.SearchText = SearchText;
            part.Shelf = Shelf;
            part.PlaceArea = PlaceArea;
            part.PartChunkSearchV2(this.userData.DatabaseKey, userData.Site.TimeZone);
            //StockTypeList = part.utilityAdd.list1;
            //lookupLists = new Dictionary<string, Dictionary<string, string>>();
            //lookupLists.Add("StockTypeList", StockTypeList.ToDictionary(key => key, value => value));
            foreach (var p in part.listOfPart)
            {
                PartsModel = new PartModel();
                PartsModel.PartId = p.PartId;
                PartsModel.ClientLookupId = p.ClientLookupId;
                PartsModel.Description = p.Description;
                PartsModel.AppliedCost = p.AppliedCost; //V2-836
                PartsModel.OnHandQuantity = p.QtyOnHand;
                PartsModel.MinimumQuantity = p.QtyReorderLevel;//MinimumQuantity
                PartsModel.Manufacturer = p.Manufacturer;
                PartsModel.ManufacturerID = p.ManufacturerId;
                PartsModel.StockType = p.StockType;
                PartsModel.PlaceArea = p.Location1_5;
                PartsModel.Section = p.Location1_1;
                PartsModel.Row = p.Location1_2;
                PartsModel.Shelf = p.Location1_3;
                PartsModel.Bin = p.Location1_4;
                PartsModel.Consignment = p.Consignment;
                PartsModel.RepairablePart = p.RepairablePart;
                PartsModel.PreviousId = p.PrevClientLookupId;
                PartsModel.UPCCode = p.UPCCode;
                PartsModel.IssueUnit = p.IssueUnit;
                PartsModel.Maximum = p.QtyMaximum;
                PartsModel.TotalCount = p.TotalCount;
                PartsModelList.Add(PartsModel);
            }
            return PartsModelList;
        }

        public List<PartModel> GetDetailsByPartPrint(int status, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string PartClientLookUpId = "", string Description = "", string Section = "", decimal MinimumQuantity = 0, decimal OnHandQuantity = 0, string Manufacturer = "", string ManufacturerID = "", string StockType = "", string Row = "", string Bin = "", string SearchText = "", string Shelf = "", string PlaceArea = "")
        {
            Part part = new Part();
            PartModel PartsModel;
            List<PartModel> PartsModelList = new List<PartModel>();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.CustomQueryDisplayId = status;
            //
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.ClientLookupId = PartClientLookUpId;
            part.Description = Description;
            part.Section = Section;
            part.QtyOnHand = OnHandQuantity;
            part.QtyReorderLevel = MinimumQuantity;
            part.Manufacturer = Manufacturer;
            part.ManufacturerId = ManufacturerID;
            part.StockType = StockType;
            part.Row = Row;
            part.Bin = Bin;
            part.SearchText = SearchText;
            part.Shelf = Shelf;
            part.PlaceArea = PlaceArea;

            //part.PartSearchForPrintV2(userData.DatabaseKey, userData.Site.TimeZone);
            part.PartChunkSearchV2(userData.DatabaseKey, userData.Site.TimeZone);
            //
            foreach (var p in part.listOfPart)
            {
                PartsModel = new PartModel();
                PartsModel.PartId = p.PartId;
                PartsModel.ClientLookupId = p.ClientLookupId;
                PartsModel.Description = p.Description;
                PartsModel.OnHandQuantity = p.QtyOnHand;
                PartsModel.MinimumQuantity = p.QtyReorderLevel;//MinimumQuantity
                PartsModel.Manufacturer = p.Manufacturer;
                PartsModel.ManufacturerID = p.ManufacturerId;
                PartsModel.StockType = p.StockType;
                PartsModel.PlaceArea = p.Location1_5;
                PartsModel.Section = p.Location1_1;
                PartsModel.Row = p.Location1_2;
                PartsModel.Shelf = p.Location1_3;
                PartsModel.Bin = p.Location1_4;
                PartsModel.Consignment = p.Consignment;
                PartsModel.RepairablePart = p.RepairablePart;
                PartsModel.PreviousId = p.PrevClientLookupId;
                PartsModel.UPCCode = p.UPCCode;
                PartsModel.Maximum = p.QtyMaximum;
                PartsModelList.Add(PartsModel);
            }
            return PartsModelList;
        }
        public PartModel PopulatePartDetails(long PartId)
        {
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            obj.RetriveByPartId(userData.DatabaseKey);
            objPart = initializeControls(obj);

            return objPart;
        }
        public PartModel initializeControls(Part obj)
        {
            PartModel objPart = new PartModel();

            objPart.ClientLookupId = obj.ClientLookupId;
            objPart.PartId = obj.PartId;

            objPart.AccountId = obj?.AccountId ?? 0;
            objPart.AccountClientLookupId = obj?.Account_ClientLookupId ?? string.Empty;
            objPart.AppliedCost = obj?.AppliedCost ?? 0;
            objPart.LastPurchaseCost = obj?.LastPurchaseCost ?? 0;
            objPart.AverageCost = obj?.AverageCost ?? 0;
            objPart.Description = obj?.Description ?? string.Empty;
            objPart.InactiveFlag = obj?.InactiveFlag ?? false;
            objPart.CriticalFlag = obj?.Critical ?? false;
            objPart.IssueUnit = obj?.IssueUnit ?? string.Empty;
            // The site must be using the PartMaster and the client must be either 4 or 6

            objPart.Manufacturer = obj?.Manufacturer ?? string.Empty;
            objPart.ManufacturerID = obj?.ManufacturerId ?? string.Empty;
            objPart.StockType = obj?.StockType ?? string.Empty;
            objPart.UPCCode = obj?.UPCCode ?? string.Empty;
            objPart.CountFrequency = obj?.CountFrequency ?? 0;
            objPart.LastCounted = obj?.LastCounted ?? DateTime.MinValue;
            objPart.Section = obj?.Location1_1 ?? string.Empty;
            objPart.PlaceArea = obj?.Location1_5 ?? string.Empty;
            objPart.Row = obj?.Location1_2 ?? string.Empty;
            objPart.Shelf = obj?.Location1_3 ?? string.Empty;
            objPart.Bin = obj?.Location1_4 ?? string.Empty;
            objPart.Consignment = obj?.Consignment ?? false;
            objPart.Maximum = obj?.QtyMaximum ?? 0;
            objPart.OnHandQuantity = obj?.QtyOnHand ?? 0;
            objPart.Minimum = obj?.QtyReorderLevel ?? 0;
            objPart.OnOrderQuantity = obj?.QtyOnOrder ?? 0;
            objPart.OnRequestQuantity = obj?.QtyOnRequest ?? 0;
            objPart.AutoPurchaseFlag = obj?.AutoPurch ?? false;
            objPart.ClientOnPremise = userData.DatabaseKey.Client.OnPremise;
            //V2-641
            objPart.MSDSContainerCode = obj.MSDSContainerCode;
            objPart.MSDSPressureCode = obj.MSDSPressureCode;
            objPart.MSDSReference = obj.MSDSReference;
            objPart.MSDSRequired = obj.MSDSRequired;
            objPart.MSDSTemperatureCode = obj.MSDSTemperatureCode;
            objPart.NoEquipXref = obj.NoEquipXref;
            objPart.PartStoreroomId = obj.PartStoreroomId;//V2-1196
            
            return objPart;
        }
        public PartModel PopulateDropdownControls(PartModel objParts)
        {
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            List<DataContracts.LookupList> objLookupStokeType = new List<DataContracts.LookupList>();
            List<DataContracts.LookupList> objLookupIssueUnit = new List<DataContracts.LookupList>();
            var AllLookUps = commonWrapper.GetAllLookUpList();
            if (AllLookUps != null)
            {
                objLookupStokeType = AllLookUps.Where(x => x.ListName == LookupListConstants.STOCK_TYPE).ToList();
                if (objLookupStokeType != null)
                {
                    objParts.LookupStokeTypeList = objLookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                objLookupIssueUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupIssueUnit != null)
                {
                    objParts.LookupIssueUnitList = objLookupIssueUnit.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

            }

            return objParts;
        }

        #endregion

        #region Photos
        //public void DeleteImage(long PartId, string TableName, bool Profile, bool Image, ref string rtrMsg)
        //{ // Check if there is a profile image attachment record for the object 
        //    Attachment attach = new Attachment()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        ObjectName = TableName,
        //        ObjectId = PartId,
        //        Profile = Profile,
        //        Image = Image
        //    };
        //    attach.ClientId = userData.DatabaseKey.Client.ClientId;
        //    List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
        //    if (AList.Count > 0)
        //    {
        //        // Profile Image Attachment Record Exists
        //        string image_url = AList.First().AttachmentURL;
        //        bool external = AList.First().External;
        //        attach.AttachmentId = AList.First().AttachmentId;
        //        attach.Delete(userData.DatabaseKey);
        //        // If the image is NOT external then we delete the image 
        //        if (!external)
        //        {
        //            AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //            aset.DeleteBlobByURL(image_url);
        //            rtrMsg = "Success";
        //        }
        //        else
        //        {
        //            rtrMsg = "External";
        //        }
        //    }
        //    else
        //    {
        //        // We still may have URL refrences in the Equipment. 
        //        // If so we need to delete the URL from the Equipment
        //        Part part = new Part();
        //        part.ClientId = userData.DatabaseKey.Client.ClientId;
        //        part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
        //        part.PartId = PartId;
        //        part.Retrieve(userData.DatabaseKey);
        //        //if (part.PartImage.Length > 0)
        //        //{
        //        //    part.PartImage = new byte[0];
        //        //    part.Update(userData.DatabaseKey);
        //        //    rtrMsg = "Success";
        //        //}
        //    }

        //}
        //public string GetPartImageUrl(long PartId, PartModel part = null)
        //{
        //    string imageurl = string.Empty;
        //    bool lExternal = false;
        //    string sasToken = string.Empty;
        //    byte[] ImageData = new byte[0];

        //    Attachment attach = new Attachment()
        //    {
        //        ClientId = userData.DatabaseKey.Client.ClientId,
        //        ObjectName = "Part",
        //        ObjectId = PartId,
        //        Profile = true,
        //        Image = true
        //    };
        //    List<Attachment> AList = attach.RetrieveProfileAttachments(userData.DatabaseKey, userData.Site.TimeZone);
        //    if (AList.Count > 0)// Check Attachment Table count, If count exists
        //    {
        //        lExternal = AList.First().External;
        //        imageurl = AList.First().AttachmentURL;
        //    }

        //    if (!lExternal && imageurl != "")// 1.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is false
        //    {
        //        AzureUtil.AzureSetup aset = new AzureUtil.AzureSetup();
        //        sasToken = aset.GetSASUrlClientSite(userData.DatabaseKey.Client.ClientId, userData.DatabaseKey.User.DefaultSiteId, imageurl);//---SAS appended Url------
        //        imageurl = sasToken;
        //    }

        //    else if (lExternal && imageurl != "") // 2.If there is an AttachmentURL returned from an associated Attachment record and the External Flag is true
        //    {
        //        imageurl = imageurl;
        //    }
        //    else
        //    {
        //        if (part == null)
        //        {
        //            part = new PartModel();
        //            Part part1 = new Part()
        //            {
        //                ClientId = _dbKey.Client.ClientId,
        //                PartId = PartId
        //            };
        //            part1.RetriveByPartId(userData.DatabaseKey);
        //           // part.PartImage = part1.PartImage ?? null;

        //        }
        //        ImageData = null;//part.PartImage;// Totally obsolete now as PartImage is removed from table.
        //        if (part != null && ImageData != null && part.PartImage.Length > 20) //3.If no AttachmentURL is returned from an associated Attachment record and Equipment.EquipmentImage > 0
        //        {
        //            const string UploadDirectory = "../Images/DisplayImg/";
        //            string imgName = "Part" + "_" + part.ClientLookupId + "." + "jpg";
        //            //Save the Byte Array as File.
        //            string uploadDirectoryServerPath = System.Web.HttpContext.Current.Server.MapPath(UploadDirectory);
        //            if (!Directory.Exists(uploadDirectoryServerPath))
        //            {
        //                Directory.CreateDirectory(uploadDirectoryServerPath);
        //            }
        //            string filePath = UploadDirectory + Path.GetFileName(imgName);
        //            File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(filePath), ImageData);
        //            imageurl = filePath;
        //        }
        //        else if ((imageurl == null || imageurl == "") || ImageData == null || ImageData.Length == 0)// 4.	If no AttachmentURL is found and there is no data in Equipment.EquipmentImage or Attachment table count is zero
        //        {
        //            const string UploadDirectory = "../Images/DisplayImg/";
        //            const string ThumbnailFileName = "NoImage.jpg";
        //            imageurl = UploadDirectory + ThumbnailFileName;
        //        }
        //    }
        //    return imageurl;
        //}
        #endregion

        #region Part_Vendor_Xref
        public Part_Vendor_Xref AddPartVendorXref(PartsVendorModel _PVXModel, long objectId)//
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = objectId
            };
            string Vendor_ClientLookupId = _PVXModel.VendorClientLookupId;
            Vendor vendor = new Vendor { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = Vendor_ClientLookupId };
            List<Vendor> vlist = vendor.RetrieveBySiteIdAndClientLookUpId(_dbKey);

            if (vlist != null && vlist.Count > 0)
            { pvx.VendorId = vlist[0].VendorId; }
            pvx.CatalogNumber = _PVXModel?.CatalogNumber ?? string.Empty;
            pvx.Manufacturer = _PVXModel?.Manufacturer ?? string.Empty;
            pvx.ManufacturerId = _PVXModel?.ManufacturerID ?? string.Empty;
            pvx.OrderQuantity = _PVXModel?.OrderQuantity ?? 0;
            pvx.OrderUnit = _PVXModel?.OrderUnit ?? string.Empty;
            pvx.Price = _PVXModel.Price ?? 0;
            pvx.PreferredVendor = _PVXModel.PreferredVendor;
            pvx.IssueOrder = _PVXModel.IssueOrder ?? 1;
            pvx.UOMConvRequired = _PVXModel.UOMConvRequired;

            pvx.ValidateAdd(_dbKey);
            if (pvx.ErrorMessages.Count == 0)
            {
                pvx.Create(_dbKey);
            }
            return pvx;
        }
        public Part_Vendor_Xref UpdatePartVendorXref(PartsVendorModel _PVXModel, long objectId)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = objectId,
                Part_Vendor_XrefId = _PVXModel.PartVendorXrefId

            };
            pvx.Retrieve(_dbKey);
            string Vendor_ClientLookupId = _PVXModel.VendorClientLookupId;
            Vendor vendor = new Vendor { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = Vendor_ClientLookupId };
            List<Vendor> vlist = vendor.RetrieveBySiteIdAndClientLookUpId(_dbKey);
            if (vlist != null && vlist.Count > 0)
            {
                pvx.VendorId = vlist[0].VendorId;
            }
            else
            {
                pvx.VendorId = 0;
            }
            pvx.CatalogNumber = _PVXModel?.CatalogNumber ?? string.Empty;
            pvx.Manufacturer = _PVXModel?.Manufacturer ?? string.Empty;
            pvx.ManufacturerId = _PVXModel?.ManufacturerID ?? string.Empty;
            pvx.OrderQuantity = _PVXModel?.OrderQuantity ?? 0;
            pvx.OrderUnit = _PVXModel?.OrderUnit ?? string.Empty;
            pvx.Price = _PVXModel.Price ?? 0;
            pvx.PreferredVendor = _PVXModel.PreferredVendor;
            pvx.IssueOrder = _PVXModel.IssueOrder ?? 1;
            pvx.UOMConvRequired = _PVXModel.UOMConvRequired;

            pvx.ValidateSave(_dbKey);
            if (pvx.ErrorMessages.Count == 0)
            {
                pvx.Update(_dbKey);
            }

            return pvx;
        }
        public List<Part_Vendor_Xref> PopulateParts(long objectId)
        {

            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                SiteId = _dbKey.User.DefaultSiteId,
                PartId = objectId

            };
            return Part_Vendor_Xref.RetrieveListByPartId(_dbKey, pvx);
        }
        #endregion

        #region Part Add
        public Part AddParts(PartModel partModel)
        {
            Part part = new Part();

            part.ClientLookupId = partModel.ClientLookupId;
            part.SiteId = _dbKey.Personnel.SiteId;
            part.Description = partModel.Description;
            part.UPCCode = partModel.UPCCode;
            part.AccountId = partModel.AccountId ?? 0;
            part.StockType = partModel.StockType;
            part.IssueUnit = partModel.IssueUnit;
            part.Manufacturer = partModel.Manufacturer;
            part.ManufacturerId = partModel.ManufacturerID;
            part.InactiveFlag = partModel.InactiveFlag;
            part.Critical = partModel.CriticalFlag;
            part.AverageCost = partModel.AverageCost ?? 0;
            part.AppliedCost = partModel.AppliedCost ?? 0;
            part.AutoPurch = partModel?.AutoPurchaseFlag ?? false;
            part.LastPurchaseCost = partModel.LastPurchaseCost ?? 0;

            // Check if duplicate part 
            //var part1 = part.RetrieveForSearchBySiteId(userData.DatabaseKey).Where(x => x.ClientLookupId == part.ClientLookupId);

            // SOM-1679 - Part Add allows a duplicate part if the existing part is inactive
            // The RetrieveForSearchBySiteId only returned active
            // Replace with RetrieveBySiteIdAndClientLookupId 
            var part1 = part.RetrieveBySiteIdAndClientLookUpId(userData.DatabaseKey).Where(x => x.ClientLookupId == part.ClientLookupId);
            if (part1.Count() > 0)
            {
                part.ErrorMessages = new List<string>();
                part.ErrorMessages.Add("Part Id Exists");
            }
            else
            {

                part.Create(_dbKey);

                PartStoreroom partStoreroom = new PartStoreroom();
                partStoreroom.PartId = part.PartId;
                partStoreroom.ClientId = _dbKey.Client.ClientId;
                partStoreroom.StoreroomId = part.StoreroomId;


                partStoreroom.Location1_1 = partModel.Section;
                partStoreroom.Location1_2 = partModel.Row;
                partStoreroom.Location1_3 = partModel.Shelf;
                partStoreroom.Location1_4 = partModel.Bin;
                partStoreroom.Location1_5 = partModel.PlaceArea;
                partStoreroom.CountFrequency = partModel.CountFrequency ?? 0;
                partStoreroom.LastCounted = partModel.LastCounted == null ? DateTime.UtcNow : Convert.ToDateTime(partModel.LastCounted);


                partStoreroom.QtyMaximum = partModel.Maximum ?? 0;
                partStoreroom.QtyOnHand = partModel.OnHandQuantity ?? 0;
                partStoreroom.QtyReorderLevel = partModel.Minimum ?? 0;

                partStoreroom.ReorderMethod = part.ReorderMethod;
                partStoreroom.Create(userData.DatabaseKey);

            }
            return part;

        }
        #endregion

        #region Part Edit
        public Part EditParts(PartModel partModel)
        {

            Part part = new Part()
            {
                PartId = partModel.PartId
            };
            part.RetriveByPartId(_dbKey);
            Account acc = new Account();
            if (partModel.AccountId != null && partModel.AccountId != 0)
            {

                acc.ClientId = userData.DatabaseKey.Client.ClientId;
                acc.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                acc.AccountId = partModel.AccountId ?? 0;

                acc.Retrieve(userData.DatabaseKey);
            }
            //-------Retrieve account client lookup id-----


            #region Initialize

            part.ClientLookupId = partModel?.ClientLookupId ?? string.Empty;
            part.AccountId = partModel?.AccountId ?? 0;
            part.Account_ClientLookupId = partModel.AccountId != 0 ? acc.ClientLookupId : string.Empty;
            part.AppliedCost = partModel?.AppliedCost ?? 0;
            part.AverageCost = partModel?.AverageCost ?? 0;
            part.LastPurchaseCost = partModel?.LastPurchaseCost ?? 0;
            part.Description = partModel?.Description ?? string.Empty;
            part.InactiveFlag = partModel?.InactiveFlag ?? false;
            part.Critical = partModel?.CriticalFlag ?? false;
            part.IssueUnit = partModel?.IssueUnit ?? string.Empty;
            part.Consignment = partModel?.Consignment ?? false;
            // The site must be using the PartMaster and the client must be either 4 or 6
            if (userData.Site.UsePartMaster && (userData.DatabaseKey.Client.ClientId == 4 || userData.DatabaseKey.Client.ClientId == 6))
            {
                // part.Manufacturer = uicManufactureLT.Text;
            }
            else
            {
                part.Manufacturer = partModel?.Manufacturer ?? string.Empty;
            }
            part.ManufacturerId = partModel?.ManufacturerID ?? string.Empty;
            part.StockType = partModel?.StockType ?? string.Empty;
            part.UPCCode = partModel?.UPCCode ?? string.Empty;
            part.CountFrequency = partModel?.CountFrequency ?? 0;

            part.Location1_1 = partModel?.Section ?? string.Empty;
            part.Location1_5 = partModel?.PlaceArea ?? string.Empty;
            part.Location1_2 = partModel?.Row ?? string.Empty;
            part.Location1_3 = partModel?.Shelf ?? string.Empty;
            part.Location1_4 = partModel?.Bin ?? string.Empty;
            part.QtyMaximum = partModel.Maximum ?? 0;
            part.QtyOnHand = partModel?.OnHandQuantity ?? 0;
            part.QtyReorderLevel = partModel?.Minimum ?? 0;
            part.AutoPurch = partModel?.AutoPurchaseFlag ?? false;

            #endregion

            part.UpdateByPartId(userData.DatabaseKey);
            if (part.ErrorMessages != null && part.ErrorMessages.Count > 0)
            {
                return part;

            }
            return part;
        }
        public Part UpdateParts(PartModel _PartModel)
        {
            Part objPart = new DataContracts.Part();
            if (_PartModel.PartId != 0)
            {
                objPart = EditParts(_PartModel);
            }
            else
            {
                objPart = AddParts(_PartModel);
            }
            return objPart;
        }
        public List<string> MakeActiveInactive(bool InActiveFlag, long PartId)
        {
            Part part = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            part.RetriveByPartId(userData.DatabaseKey);
            part.InactiveFlag = !InActiveFlag;
            part.UpdateByPartId(userData.DatabaseKey);
            return part.ErrorMessages;
        }
        public List<string> CreatePartEvent(long PartId, bool InactiveStatus)
        {
            PartEventLog partEventLog = new PartEventLog();
            partEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            partEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            partEventLog.PartId = PartId;
            partEventLog.TransactionDate = DateTime.UtcNow;
            if (InactiveStatus)
            {
                partEventLog.Event = ActivationStatusConstant.Activate;
            }
            else
            {
                partEventLog.Event = ActivationStatusConstant.InActivate;
            }
            partEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            partEventLog.Comments = string.Empty;
            partEventLog.SourceId = 0;
            partEventLog.Create(userData.DatabaseKey);
            return partEventLog.ErrorMessages;
        }
        public Part ValidatePartStatusChange(long partId, string flag, string clientLookupId)
        {
            Part part = new Part()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartId = partId,
                Flag = flag,
                ClientLookupId = clientLookupId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            part.CheckPartIsInactivateorActivate(userData.DatabaseKey);
            return part;
        }
        #endregion

        #region Equipment_Part_Xref
        public List<Equipment_Parts_Xref> PopulatePartEquipmentDetail(long ObjectId)
        {
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = ObjectId


            };
            return Equipment_Parts_Xref.RetriveByPartId(_dbKey, eqx);
        }
        public Equipment_Parts_Xref AddEquipmentPartsXref(EquipmentPartXrefModel equipmentPartXrefModel, long ObjectId)
        {
            List<string> errList = new List<string>();
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = ObjectId
            };
            string EquipmentLookupId = equipmentPartXrefModel.Equipment_ClientLookupId;
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = EquipmentLookupId };
            equipment.RetrieveByClientLookupId(_dbKey);
            eqx.Equipment_ClientLookupId = equipmentPartXrefModel.Equipment_ClientLookupId;
            eqx.EquipmentId = equipment.EquipmentId;
            eqx.ParentSiteId = equipment.SiteId;
            eqx.QuantityNeeded = equipmentPartXrefModel.QuantityNeeded ?? 0;
            eqx.QuantityUsed = equipmentPartXrefModel.QuantityUsed ?? 0;
            eqx.Comment = equipmentPartXrefModel.Comment ?? String.Empty;
            eqx.CreatePKForeignKeys(this.userData.DatabaseKey);
            return eqx;
        }
        public Equipment_Parts_Xref UpdateEquipmentPartsXref(EquipmentPartXrefModel equipmentPartXrefModel, long ObjectId)
        {
            List<string> errList = new List<string>();
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = ObjectId,
                Equipment_Parts_XrefId = equipmentPartXrefModel.Equipment_Parts_XrefId
            };
            eqx.Retrieve(_dbKey);

            string EquipmentLookupId = equipmentPartXrefModel.Equipment_ClientLookupId;
            Equipment equipment = new Equipment { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = EquipmentLookupId };
            equipment.RetrieveByClientLookupId(_dbKey);
            eqx.Equipment_ClientLookupId = equipmentPartXrefModel.Equipment_ClientLookupId;
            eqx.ParentSiteId = equipment.SiteId;
            eqx.EquipmentId = equipment.EquipmentId;
            eqx.QuantityNeeded = equipmentPartXrefModel.QuantityNeeded ?? 0;
            eqx.QuantityUsed = equipmentPartXrefModel.QuantityUsed ?? 0;
            eqx.Comment = equipmentPartXrefModel.Comment ?? string.Empty;
            eqx.UpdatePKForeignKeys(this.userData.DatabaseKey);
            return eqx;
        }
        #endregion

        #region Part_History
        public List<PartsHistoryModel> GetDetailsPartHistory(long partId = 0, int daterange = 0)
        {
            PartHistoryReview partHistoryAllReview = new PartHistoryReview();
            partHistoryAllReview.ClientId = userData.DatabaseKey.Client.ClientId;
            partHistoryAllReview.PartId = partId;
            partHistoryAllReview.DateRange = Convert.ToString(daterange);
            List<PartHistoryReview> _partHistoryAllReview = PartHistoryReview.RetrievePartHistoryReview_V2(userData.DatabaseKey, partHistoryAllReview, userData.Site.TimeZone); //V2-1180 Converted to local date
            List<PartsHistoryModel> PartHistoryModelList = new List<PartsHistoryModel>();
            PartsHistoryModel partHistoryModel;

            foreach (var p in _partHistoryAllReview)
            {
                partHistoryModel = new PartsHistoryModel();
                partHistoryModel.PartId = p.PartId;
                partHistoryModel.TransactionType = p.TransactionType;
                partHistoryModel.Requestor_Name = p.Requestor_Name;
                partHistoryModel.PerformBy_Name = p.PerformBy_Name;
                partHistoryModel.TransactionDate = p.TransactionDate;
                partHistoryModel.TransactionQuantity = p.TransactionQuantity;
                partHistoryModel.Cost = p.Cost;
                partHistoryModel.ChargeType_Primary = p.ChargeType_Primary;
                partHistoryModel.ChargeTo_ClientLookupId = p.ChargeTo_ClientLookupId;
                partHistoryModel.Account_ClientLookupId = p.Account_ClientLookupId;
                partHistoryModel.PurchaseOrder_ClientLookupId = p.PurchaseOrder_ClientLookupId;
                partHistoryModel.ChargeType_Primary = p.ChargeType_Primary;
                partHistoryModel.Vendor_ClientLookupId = p.Vendor_ClientLookupId;
                partHistoryModel.Vendor_Name = p.Vendor_Name;

                PartHistoryModelList.Add(partHistoryModel);
            }

            return PartHistoryModelList;
        }
        #endregion

        #region Part_Receipt
        public List<PartsReceiptModel> GetDetailsPartReceipt(long partId = 0, int daterange = 0)
        {
            POReceipt pOReceipt = new POReceipt();
            pOReceipt.ClientId = userData.DatabaseKey.User.ClientId;
            pOReceipt.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pOReceipt.PartId = partId;
            pOReceipt.DateRange = Convert.ToString(daterange);
            List<POReceipt> listReceipt = new List<POReceipt>();
            listReceipt = pOReceipt.RetrievePOFromParts(userData.DatabaseKey, userData.Site.TimeZone);
            List<PartsReceiptModel> PartReceiptModelList = new List<PartsReceiptModel>();
            PartsReceiptModel partReceiptModel;

            foreach (var p in listReceipt)
            {
                partReceiptModel = new PartsReceiptModel();
                partReceiptModel.POClientLookupId = p.POClientLookupId;

                if (p.OrderDate != null && p.OrderDate == default(DateTime))
                {
                    partReceiptModel.ReceivedDate = null;
                }
                else
                {
                    partReceiptModel.ReceivedDate = p.OrderDate;
                }
                partReceiptModel.VendorClientLookupId = p.VendorClientLookupId;
                partReceiptModel.VendorName = p.VendorName;
                partReceiptModel.OrderQuantity = p.OrderQuantity;
                partReceiptModel.UnitCost = p.UnitCost;

                PartReceiptModelList.Add(partReceiptModel);
            }

            return PartReceiptModelList;
        }
        #endregion

        #region ReviewSite

        public List<Client.Models.Parts.ReviewSiteModel> PopulateReviewSite(long PartMasterId)
        {
            Client.Models.Parts.ReviewSiteModel objReviewSiteModel;
            List<Client.Models.Parts.ReviewSiteModel> ReviewSiteModelList = new List<Client.Models.Parts.ReviewSiteModel>();
            Part pm = new Part
            {
                ClientId = this.userData.DatabaseKey.User.ClientId,
                PartMasterId = PartMasterId
            };
            List<Part> PMList = pm.RetrievePartSiteReview(this.userData.DatabaseKey, userData.Site.TimeZone);

            if (PMList != null)
            {
                foreach (var item in PMList)
                {
                    objReviewSiteModel = new Client.Models.Parts.ReviewSiteModel();
                    objReviewSiteModel.PartId = item.PartId;
                    objReviewSiteModel.ClientLookupId = item.ClientLookupId;        // V2-711
                    objReviewSiteModel.SiteName = item.SiteName;
                    objReviewSiteModel.ClientLookupId = item.ClientLookupId;
                    objReviewSiteModel.Description = item.Description;
                    objReviewSiteModel.QtyOnHand = item.QtyOnHand;
                    objReviewSiteModel.QtyOnOrder = item.QtyOnOrder;
                    if (item.LastPurchaseDate != null && item.LastPurchaseDate == default(DateTime))
                    {
                        objReviewSiteModel.LastPurchaseDate = null;
                    }
                    else
                    {
                        objReviewSiteModel.LastPurchaseDate = item.LastPurchaseDate;
                    }

                    objReviewSiteModel.LastPurchaseCost = item.LastPurchaseCost;
                    objReviewSiteModel.LastPurchaseVendor = item.LastPurchaseVendor;
                    objReviewSiteModel.InactiveFlag = item.InactiveFlag;
                    if (userData.Site.Name != item.SiteName)
                    {
                        objReviewSiteModel.RequestTransferStatus = true;
                    }
                    else 
                    {
                        objReviewSiteModel.RequestTransferStatus = false;
                    }
                    ReviewSiteModelList.Add(objReviewSiteModel);
                }
            }



            return ReviewSiteModelList;
        }
        public List<string> AddPartTransfer(PartTransferModel objPartTransferModel)
        {
            PartTransfer pt = new PartTransfer();
            PartTransferEventLog log = new PartTransferEventLog();
            RequestPart = new Part()
            {
                PartId = objPartTransferModel.RequestPartId,
                ClientId = userData.DatabaseKey.Client.ClientId//,
                                                               // SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            IssuePart = new Part()
            {
                PartId = objPartTransferModel.IssuePartId,
                ClientId = userData.DatabaseKey.Client.ClientId//,
                                                               // SiteId = userData.DatabaseKey.Personnel.SiteId
            };
            partTransfer = new PartTransfer()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                //   SiteId = userData.DatabaseKey.Personnel.SiteId,
                PartTransferId = 0
            };
            //partStore = new PartStoreroom()
            //{
            //    ClientId = userData.DatabaseKey.Client.ClientId,
            //    //  SiteId = userData.DatabaseKey.Personnel.SiteId,
            //    PartStoreroomId = objPartTransferModel.PartId
            //};
            RequestPart.RetriveByPartId(userData.DatabaseKey);
            IssuePart.RetriveByPartId(userData.DatabaseKey);
            //partStore.Retrieve(userData.DatabaseKey);
            RequestSite = new Site()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = RequestPart.SiteId
            };
            RequestSite.Retrieve(userData.DatabaseKey);
            IssueSite = new Site()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = IssuePart.SiteId
            };
            IssueSite.Retrieve(userData.DatabaseKey);
            pt.ClientId = userData.DatabaseKey.Client.ClientId;
            pt.RequestSiteId = RequestSite.SiteId;
            pt.RequestPartId = RequestPart.PartId;
            pt.IssueSiteId = IssueSite.SiteId;
            pt.IssuePartId = IssuePart.PartId;
            pt.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            pt.CreatedBy = userData.DatabaseKey.UserName;
            pt.Quantity = Convert.ToDecimal(objPartTransferModel.Qty);
            pt.Reason = objPartTransferModel.Reason;
            if (objPartTransferModel.RequiredDate != null && objPartTransferModel.RequiredDate == default(DateTime))
            {
                pt.RequiredDate = null;
            }
            else
            {
                pt.RequiredDate = objPartTransferModel.RequiredDate;
            }
            pt.ShippingAccountId = objPartTransferModel.ShippingAccountId ?? 0;
            pt.Status = PartTransferStatus.Open;
            pt.LastEvent = PartTransferEvents.Created;
            pt.LastEventDate = DateTime.UtcNow;
            pt.LastEventBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.ClientId = userData.DatabaseKey.Client.ClientId;
            log.TransactionDate = DateTime.UtcNow;
            log.Event = PartTransferEvents.Created;
            log.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            log.SourceId = 0;
            pt.Create(userData.DatabaseKey);
            log.PartTransferId = pt.PartTransferId;
            log.Create(userData.DatabaseKey);
            return pt.ErrorMessages;
        }
        #endregion

        #region Notes
        public Notes AddNotes(PartsVM _NotesModel)
        {
            Notes notes = new Notes()
            {
                OwnerId = userData.DatabaseKey.User.UserInfoId,
                OwnerName = userData.DatabaseKey.User.FullName,
                Subject = String.IsNullOrEmpty(_NotesModel.notesModel.Subject) ? "No Subject" : _NotesModel.notesModel.Subject,
                Content = _NotesModel.notesModel.Content,
                Type = _NotesModel.notesModel.Type,
                TableName = "Part",
                ObjectId = _NotesModel.notesModel.PartId,
                UpdateIndex = _NotesModel.notesModel.updatedindex,
                NotesId = _NotesModel.notesModel.NotesId
            };
            if (_NotesModel.notesModel.NotesId == 0)
            {
                notes.Create(userData.DatabaseKey);
            }
            else
            {
                notes.Update(userData.DatabaseKey);
            }
            return notes;
        }
        public Notes RetrieveNotesForEdit(long _notesId)
        {
            Notes notes = new Notes() { NotesId = _notesId };
            notes.Retrieve(userData.DatabaseKey);
            return notes;
        }
        public bool DeleteNote(long _notesId)
        {
            try
            {
                Notes notes = new Notes()
                {
                    NotesId = Convert.ToInt64(_notesId)
                };
                notes.Delete(userData.DatabaseKey);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Vendor
        public bool DeleteVendor(long _PartVendorXrefId)
        {
            try
            {
                Part_Vendor_Xref pvx = new Part_Vendor_Xref()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    Part_Vendor_XrefId = _PartVendorXrefId
                };
                pvx.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartsVendorModel EditVendor(long partId, long _part_Vendor_XrefId, string clientLookupId, int updatedIndex)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = partId,
                Part_Vendor_XrefId = _part_Vendor_XrefId
            };
            pvx.Retrieve(userData.DatabaseKey);

            PartsVendorModel partVendorXref = new PartsVendorModel();
            partVendorXref.PartId = partId;
            partVendorXref.PartClientLookupId = clientLookupId;
            partVendorXref.CatalogNumber = pvx.CatalogNumber;
            partVendorXref.Manufacturer = pvx.Manufacturer;
            partVendorXref.ManufacturerID = pvx.ManufacturerId;
            partVendorXref.OrderQuantity = pvx.OrderQuantity;
            partVendorXref.OrderUnit = pvx.OrderUnit;
            partVendorXref.Price = pvx.Price;
            partVendorXref.PreferredVendor = pvx.PreferredVendor;
            partVendorXref.PartVendorXrefId = pvx.Part_Vendor_XrefId;
            partVendorXref.IssueOrder = pvx.IssueOrder;
            partVendorXref.UOMConvRequired = pvx.UOMConvRequired;
            if (pvx.UOMConvRequired)
            {
                partVendorXref.DefaultissueOrdervalue = 1.000001m;
                partVendorXref.DefaultPricevalue = 0.00001m;
            }

            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorId = pvx.VendorId
            };
            vendor.Retrieve(userData.DatabaseKey);
            partVendorXref.VendorClientLookupId = vendor.ClientLookupId;

            return partVendorXref;
        }
        #endregion

        #region Equipment
        public List<PartsEquipmentGridModel> PopulatePartEquipment(long _partId)
        {
            List<Equipment_Parts_Xref> epx = PopulatePartEquipmentDetail(_partId);
            List<PartsEquipmentGridModel> PartsEquipmentGridModelList = new List<PartsEquipmentGridModel>();
            PartsEquipmentGridModel objPartsEquipmentGridModel;
            foreach (var v in epx)
            {
                objPartsEquipmentGridModel = new PartsEquipmentGridModel();
                objPartsEquipmentGridModel.Equipment_ClientLookupId = v.Equipment_ClientLookupId;
                objPartsEquipmentGridModel.Equipment_Name = v.Equipment_Name;
                objPartsEquipmentGridModel.Part_ClientLookupId = v.Part_ClientLookupId;
                objPartsEquipmentGridModel.Equipment_Parts_XrefId = v.Equipment_Parts_XrefId;
                objPartsEquipmentGridModel.EquipmentId = v.EquipmentId;
                objPartsEquipmentGridModel.PartId = v.PartId;
                objPartsEquipmentGridModel.Comment = v.Comment;
                objPartsEquipmentGridModel.QuantityNeeded = v.QuantityNeeded;
                objPartsEquipmentGridModel.QuantityUsed = v.QuantityUsed;
                objPartsEquipmentGridModel.UpdateIndex = v.UpdateIndex;
                //objPartsEquipmentGridModel.PartEquipmentSecurity = userData.Security.Parts.Part_Equipment_XRef == true ? "true" : "false";
                PartsEquipmentGridModelList.Add(objPartsEquipmentGridModel);
            }
            return PartsEquipmentGridModelList;
        }
        public EquipmentPartXrefModel EditPartEquipment(long partId, long _equipment_Parts_XrefId, string clientLookupId, int updatedIndex)
        {
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = partId,
                Equipment_Parts_XrefId = _equipment_Parts_XrefId
            };
            eqx.Retrieve(userData.DatabaseKey);
            EquipmentPartXrefModel _equipmentPartXrefModel = new EquipmentPartXrefModel();
            _equipmentPartXrefModel.PartId = partId;
            _equipmentPartXrefModel.Equipment_ClientLookupId = clientLookupId;
            _equipmentPartXrefModel.QuantityNeeded = eqx.QuantityNeeded;
            _equipmentPartXrefModel.QuantityUsed = eqx.QuantityUsed;
            _equipmentPartXrefModel.Comment = eqx.Comment;
            _equipmentPartXrefModel.EquipmentId = eqx.EquipmentId;
            _equipmentPartXrefModel.Equipment_Parts_XrefId = eqx.Equipment_Parts_XrefId;
            Equipment equipment = new Equipment()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                EquipmentId = eqx.EquipmentId
            };
            equipment.Retrieve(userData.DatabaseKey);
            _equipmentPartXrefModel.Equipment_ClientLookupId = equipment.ClientLookupId;
            return _equipmentPartXrefModel;
        }
        public bool DeleteEquipment(long _equipment_Parts_XrefId)
        {
            try
            {
                Equipment_Parts_Xref epx = new Equipment_Parts_Xref()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    Equipment_Parts_XrefId = _equipment_Parts_XrefId
                };
                epx.Delete(userData.DatabaseKey);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region CreateRetrieveBy
        public CreatedLastUpdatedPartModel PopulateCreateModifyDate(long partId)
        {
            CreatedLastUpdatedPartModel createdLastUpdatedPartModel = new CreatedLastUpdatedPartModel();
            DataContracts.Part p = new DataContracts.Part();
            p.ClientId = this.userData.DatabaseKey.Client.ClientId;
            p.PartId = partId;
            p.RetrieveCreateModifyDate(userData.DatabaseKey);
            createdLastUpdatedPartModel.CreatedDateValue = p.CreateDate.ToString();
            createdLastUpdatedPartModel.CreatedUserValue = p.CreateBy;
            createdLastUpdatedPartModel.ModifyDatevalue = p.ModifyDate.ToString();
            createdLastUpdatedPartModel.ModifyUserValue = p.ModifyBy;
            return createdLastUpdatedPartModel;
        }
        #endregion

        #region Change Part ID
        public List<String> ChangePartID(PartsVM objModel, long PartId)
        {
            List<string> EMsg = new List<string>();
            Part p = new Part();
            string OldClientLookupId = string.Empty;
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.Personnel.SiteId;
            p.PartId = PartId;
            p.Retrieve(userData.DatabaseKey);
            OldClientLookupId = p.ClientLookupId;
            p.ClientLookupId = objModel.changePartIdModel.ClientLookupId;
            p.ChangeClientLookupId_V2(userData.DatabaseKey);
            if (p.ErrorMessages.Count == 0)
            {
                string Event = "ChangeID";
                string Comments = "Previous Part ID – " + OldClientLookupId;
                EMsg = CreatePartEvent(PartId, Event, Comments);
            }
            else
            {
                EMsg = p.ErrorMessages;

            }
            return EMsg;
        }
        #endregion

        #region Review Site
        public PartMasterModel PopulateReviewSiteDetails(long PartId)
        {
            PartMasterModel objPartMasterModel = new PartMasterModel();

            Part part = new Part()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                PartId = PartId
            };
            part.Retrieve(userData.DatabaseKey);

            if (part.PartMasterId > 0)
            {
                PartMaster pm = new PartMaster()
                {
                    PartMasterId = part.PartMasterId
                };

                pm.Retrieve(userData.DatabaseKey);
                objPartMasterModel.PartMasterId = pm.PartMasterId;
                objPartMasterModel.ClientLookupId = pm.ClientLookupId;
                objPartMasterModel.ShortDescription = pm.ShortDescription;
                objPartMasterModel.LongDescription = pm.LongDescription;
            }
            else { objPartMasterModel.PartMasterId = part.PartMasterId; }



            return objPartMasterModel;
        }

        public List<Client.Models.NotesModel> PopulateNotes(long _partId)
        {
            Client.Models.NotesModel objNotesModel;
            List<Client.Models.NotesModel> NotesModelList = new List<Client.Models.NotesModel>();
            Notes note = new Notes()
            {
                ObjectId = _partId,
                TableName = "Part"
            };
            List<Notes> NotesList = note.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
            if (NotesList != null)
            {
                foreach (var item in NotesList)
                {
                    objNotesModel = new Client.Models.NotesModel();
                    objNotesModel.NotesId = item.NotesId;
                    objNotesModel.Subject = item.Subject;
                    objNotesModel.OwnerName = item.OwnerName;
                    objNotesModel.ModifiedDate = item.ModifiedDate;
                    objNotesModel.ObjectId = item.ObjectId;
                    objNotesModel.updatedindex = item.UpdateIndex;
                    NotesModelList.Add(objNotesModel);
                }
            }
            return NotesModelList;
        }
        #endregion

        #region BulkUpload
        public List<string> PartBulkUpload(PartBulkUpdateModel pModel)
        {
            List<string> errorMessage = new List<string>();
            bool isChanged = false;
            if (!String.IsNullOrEmpty(pModel.PartIdList))
            {
                foreach (var item in pModel.PartIdList.Split(',').ToList())
                {
                    Part part = new Part()
                    {
                        PartId = Convert.ToInt64(item)
                    };
                    part.RetriveByPartId(_dbKey);
                    if (!String.IsNullOrWhiteSpace(pModel.StockType))
                    {
                        part.StockType = pModel.StockType;
                        isChanged = true;
                    }
                    if (!String.IsNullOrWhiteSpace(pModel.IssueUnit))
                    {
                        part.IssueUnit = pModel.IssueUnit;
                        isChanged = true;
                    }
                    if (!String.IsNullOrWhiteSpace(pModel.Manufacturer))
                    {
                        part.Manufacturer = pModel.Manufacturer;
                        isChanged = true;
                    }
                    if (!String.IsNullOrWhiteSpace(pModel.ManufacturerID))
                    {
                        part.ManufacturerId = pModel.ManufacturerID;
                        isChanged = true;
                    }
                    if (!string.IsNullOrEmpty(pModel.Account_ClientLookupId))
                    {
                        part.Account_ClientLookupId = pModel.Account_ClientLookupId.Trim();
                        isChanged = true;
                    }
                    if (isChanged == true)
                    {
                        part.UpdateByPartId(userData.DatabaseKey);
                        if (part.ErrorMessages != null && part.ErrorMessages.Count > 0)
                        {
                            errorMessage.AddRange(part.ErrorMessages);
                        }
                    }
                }
            }
            return errorMessage;
        }
        #endregion BulkUpload

        #region POPR Item    
        internal List<POPRModel> PopulatePOPRitems(long PartId)
        {
            POPRModel objPOPRItem;
            List<POPRModel> POPRList = new List<POPRModel>();

            Part part = new Part()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartId = PartId
            };
            var reqData = part.RetrievePOandPRforPart(this.userData.DatabaseKey);

            if (reqData != null)
            {
                foreach (var item in reqData)
                {
                    objPOPRItem = new POPRModel();
                    objPOPRItem.PartId = item.PartId;
                    objPOPRItem.ClientLookupId = item.PONumber;
                    objPOPRItem.Status = item.POStatus;
                    objPOPRItem.Vendor = item.VendorClientlookupId;
                    objPOPRItem.VendorName = item.VendorName;
                    objPOPRItem.CreateDate = item.CreateDate;
                    objPOPRItem.POType = item.POType;
                    objPOPRItem.PoPrId = item.PoPrId;
                    POPRList.Add(objPOPRItem);
                }
            }

            return POPRList;
        }
        #endregion

        #region PartHistory Log
        public List<PartsHistoryModel> PopulatePartHistoryLog(long partId)
        {
            PartsHistoryModel objPartsHistoryModel;
            List<PartsHistoryModel> PartsHistoryModelList = new List<PartsHistoryModel>();

            PartHistory partHistory = new PartHistory();
            List<PartHistory> data = new List<PartHistory>();
            partHistory.ClientId = userData.DatabaseKey.Client.ClientId;
            partHistory.SiteId = userData.DatabaseKey.Personnel.SiteId;
            partHistory.PartId = partId;
            data = partHistory.RetriveByPartId(userData.DatabaseKey);

            if (data != null)
            {
                foreach (var item in data)
                {
                    objPartsHistoryModel = new PartsHistoryModel();
                    objPartsHistoryModel.TransactionType = item.TransactionType;
                    objPartsHistoryModel.TransactionQuantity = item.TransactionQuantity;
                    objPartsHistoryModel.PersonnelInitial = item.PersonnelInitial;
                    if (item.TransactionDate != null && item.TransactionDate == default(DateTime))
                    {
                        objPartsHistoryModel.TransactionDate = null;
                    }
                    else
                    {
                        objPartsHistoryModel.TransactionDate = item.TransactionDate.ToUserTimeZone(userData.Site.TimeZone);
                    }

                    PartsHistoryModelList.Add(objPartsHistoryModel);
                }
            }
            return PartsHistoryModelList;
        }

        //public void test()
        //{
        //    Part p = new Part();
        //    p.CallerUserInfoId = 1;
        //    p.CallerUserName = "indusnet";
        //    p.ClientId = 1;
        //    p.SiteId = 1;
        //    p.CustomQueryDisplayId = 1;
        //    p.OrderbyColumn = "0";
        //    p.OrderBy = "asc";
        //    p.OffSetVal = 1;
        //    p.NextRow = 10;
        //    p.ClientLookupId = "";
        //    p.Description = "";
        //    p.Manufacturer = "";
        //    p.ManufactureId = "";
        //    p.Section = "";
        //    p.Row = "";
        //    p.Shelf = "";
        //    p.Bin = "";
        //    p.PlaceArea = "";
        //    p.Stock = "";
        //    p.SearchText = "";
        //    p.PartChunkSearchV2(userData.DatabaseKey,userData.Site.TimeZone);

        //}
        #endregion

        #region Additional Part Functions
        #region Adjust On Hand Quantity
        public PartHistory SaveHandsOnQty(PartGridPhysicalInvList model)
        {
            PartStoreroom objPartStoreroom = new PartStoreroom()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = model.PartId.Value
            };
            List<PartStoreroom> PartStorerommList = PartStoreroom.RetrieveByPartId(this.userData.DatabaseKey, objPartStoreroom);
            decimal QtyOnHand = 0;
            int UpdateIndex = 0;
            if (PartStorerommList != null && PartStorerommList.Count > 0)
            {
                QtyOnHand = PartStorerommList[0].QtyOnHand;
                UpdateIndex = PartStorerommList[0].UpdateIndex;
            }

            List<PartHistory> lstPartHistory = new List<PartHistory>();
            PartHistory tmpModel = new PartHistory();
            tmpModel = new PartHistory
            {
                PartId = model?.PartId ?? 0,
                PartClientLookupId = model?.PartClientLookupId ?? string.Empty,
                Description = model?.Description ?? string.Empty,
                PartUPCCode = model?.PartUPCCode ?? string.Empty,
                PartStoreroomQtyOnHand = QtyOnHand,
                PartQtyCounted = model?.QuantityCount ?? 0,
                PartStoreroomUpdateIndex = UpdateIndex,
                PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                SiteId = this.userData.DatabaseKey.Personnel.SiteId
            };
            lstPartHistory.Add(tmpModel);
            PartHistory parthistory = new PartHistory() { PartHistoryList = lstPartHistory };
            parthistory.PhysicalInventory(userData.DatabaseKey);
            return parthistory;
        }
        #endregion
        #region Part Checkout
        internal List<PartHistory> SavePartCheckOut(ParInvCheckoutModel data)
        {
            List<PartHistory> tmpList = new List<PartHistory>();
            PartHistory tmpItem = new PartHistory();
            List<PartHistory> PartHistoryListTemp = new List<PartHistory>();
            if (data != null)
            {

                tmpItem.IssueToClientLookupId = data.IssueToClentLookupId;
                tmpItem.IssuedTo = Convert.ToString(data.PersonnelId);
                tmpItem.PartStoreroomId = data.PartStoreroomId ?? 0;
                tmpItem.TransactionDate = System.DateTime.UtcNow;
                tmpItem.ChargeType_Primary = data.ChargeType;
                tmpItem.ChargeToClientLookupId = data.ChargeToClientLookupId;
                tmpItem.ChargeToId_Primary = Convert.ToInt64(data.ChargeToId);
                tmpItem.TransactionQuantity = data.Quantity ?? 0;
                tmpItem.PartClientLookupId = data.PartClientLookupId;
                tmpItem.PartId = data.PartId ?? 0;
                tmpItem.Description = data.PartDescription;
                tmpItem.SiteId = userData.DatabaseKey.Personnel.SiteId;
                tmpItem.TransactionType = PartHistoryTranTypes.PartIssue;
                tmpItem.IsPartIssue = true;
                tmpItem.ErrorMessagerow = null;
                tmpItem.PartUPCCode = data.UPCCode;
                tmpItem.PerformedById = data.PersonnelId ?? 0;
                tmpItem.RequestorId = data.PersonnelId ?? 0;
                tmpItem.IsPerformAdjustment = true;
                tmpList.Add(tmpItem);
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateByForeignKeysnew(userData.DatabaseKey);
            }
            return PartHistoryListTemp;
        }
        #endregion

        #endregion

        #region V2-641 Add Part Dynamic Ui Configuration
        public Part AddPartsDynamic(PartsVM _PartModel)
        {
            Part objPart = new DataContracts.Part();
            objPart = AddPartsDyn(_PartModel);
            return objPart;
        }
        public Part AddPartsDyn(PartsVM partModel)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Part part = new Part();
            PartStoreroom partStoreroom = new PartStoreroom();
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = partModel.AddPart.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(partModel.AddPart);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                if (item.TableName == "Part")
                {
                    setpropertyInfo = part.GetType().GetProperty(item.ColumnName);
                    setpropertyInfo.SetValue(part, val);
                }
                else
                {
                    setpropertyInfo = partStoreroom.GetType().GetProperty(item.ColumnName);
                    setpropertyInfo.SetValue(partStoreroom, val);
                }
            }

            var part1 = part.RetrieveBySiteIdAndClientLookUpId(userData.DatabaseKey).Where(x => x.ClientLookupId == part.ClientLookupId);
            if (part1.Count() > 0)
            {
                part.ErrorMessages = new List<string>();
                part.ErrorMessages.Add("Part Id Exists");
            }
            else
            {
                part.Create(_dbKey);

                
                partStoreroom.PartId = part.PartId;
                partStoreroom.ClientId = _dbKey.Client.ClientId;
                partStoreroom.StoreroomId = part.StoreroomId;

                partStoreroom.ReorderMethod = part.ReorderMethod;
                partStoreroom.Create(userData.DatabaseKey);

                if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false))
                {
                    IEnumerable<string> errors = AddPartUDFDynamic(partModel.AddPart, part.PartId, configDetails);
                    if (errors != null && errors.Count() > 0)
                    {
                        part.ErrorMessages.AddRange(errors);
                    }
                }

            }
            return part;

        }

        private void AssignDefaultOrNullValue(ref object val, Type t)
        {
            if (t.Equals(typeof(long?)))
            {
                val = val ?? 0;
            }
            else if (t.Equals(typeof(DateTime?)))
            {
                //val = val ?? null;
            }
            else if (t.Equals(typeof(decimal?)))
            {
                val = val ?? 0M;
            }
            else if (t.Name == "String")
            {
                val = val ?? string.Empty;
            }
        }

        public List<string> AddPartUDFDynamic(AddPartModelDynamic part, long PartId, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PartUDF partUDF = new PartUDF();
            partUDF.ClientId = userData.DatabaseKey.Client.ClientId;
            partUDF.PartId = PartId;

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = part.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(part);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = partUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(partUDF, val);
            }

            partUDF.Create(_dbKey);
            return partUDF.ErrorMessages;
        }
        #endregion

        #region V2-641 Edit Part Dynamic Ui Configuration
        public EditPartModelDynamic RetrievePartDetailsByPartId(long PartId)
        {
            EditPartModelDynamic editPartModelDynamic = new EditPartModelDynamic();
            Part part = RetrievePartByPartId(PartId);
            PartUDF partUDF = RetrievePartUDFByPartId(PartId);

            editPartModelDynamic = MapPartUDFDataForEdit(editPartModelDynamic, partUDF);
            editPartModelDynamic = MapPartDataForEdit(editPartModelDynamic, part);
            return editPartModelDynamic;
        }
        public Part RetrievePartByPartId(long PartId)
        {
            Part part = new Part()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartId = PartId
            };
            part.RetriveByPartId(_dbKey);
            return part;
        }
        public PartUDF RetrievePartUDFByPartId(long PartId)
        {
            PartUDF partUDF = new PartUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = PartId
            };

            partUDF = partUDF.RetrieveByPartId(this.userData.DatabaseKey);
            return partUDF;
        }
        private EditPartModelDynamic MapPartUDFDataForEdit(EditPartModelDynamic editPartModelDynamic, PartUDF partUDF)
        {
            if (partUDF != null)
            {
                editPartModelDynamic.PartUDFId = partUDF.PartUDFId;

                editPartModelDynamic.Text1 = partUDF.Text1;
                editPartModelDynamic.Text2 = partUDF.Text2;
                editPartModelDynamic.Text3 = partUDF.Text3;
                editPartModelDynamic.Text4 = partUDF.Text4;

                if (partUDF.Date1 != null && partUDF.Date1 == DateTime.MinValue)
                {
                    editPartModelDynamic.Date1 = null;
                }
                else
                {
                    editPartModelDynamic.Date1 = partUDF.Date1;
                }
                if (partUDF.Date2 != null && partUDF.Date2 == DateTime.MinValue)
                {
                    editPartModelDynamic.Date2 = null;
                }
                else
                {
                    editPartModelDynamic.Date2 = partUDF.Date2;
                }
                if (partUDF.Date3 != null && partUDF.Date3 == DateTime.MinValue)
                {
                    editPartModelDynamic.Date3 = null;
                }
                else
                {
                    editPartModelDynamic.Date3 = partUDF.Date3;
                }
                if (partUDF.Date4 != null && partUDF.Date4 == DateTime.MinValue)
                {
                    editPartModelDynamic.Date4 = null;
                }
                else
                {
                    editPartModelDynamic.Date4 = partUDF.Date4;
                }

                editPartModelDynamic.Bit1 = partUDF.Bit1;
                editPartModelDynamic.Bit2 = partUDF.Bit2;
                editPartModelDynamic.Bit3 = partUDF.Bit3;
                editPartModelDynamic.Bit4 = partUDF.Bit4;

                editPartModelDynamic.Numeric1 = partUDF.Numeric1;
                editPartModelDynamic.Numeric2 = partUDF.Numeric2;
                editPartModelDynamic.Numeric3 = partUDF.Numeric3;
                editPartModelDynamic.Numeric4 = partUDF.Numeric4;

                editPartModelDynamic.Select1 = partUDF.Select1;
                editPartModelDynamic.Select2 = partUDF.Select2;
                editPartModelDynamic.Select3 = partUDF.Select3;
                editPartModelDynamic.Select4 = partUDF.Select4;
            }
            return editPartModelDynamic;
        }
        public EditPartModelDynamic MapPartDataForEdit(EditPartModelDynamic editPartModelDynamic, Part part)
        {
            editPartModelDynamic.PartId = part.PartId;
            editPartModelDynamic.ClientLookupId = part.ClientLookupId;
            editPartModelDynamic.Description = part.Description;
            editPartModelDynamic.UPCCode = part.UPCCode;
            editPartModelDynamic.AccountId = part.AccountId;
            editPartModelDynamic.StockType = part.StockType;
            editPartModelDynamic.IssueUnit = part.IssueUnit;
            editPartModelDynamic.Manufacturer = part.Manufacturer;
            editPartModelDynamic.ManufacturerId = part.ManufacturerId;
            editPartModelDynamic.Critical = part.Critical;
            editPartModelDynamic.MSDSContainerCode = part.MSDSContainerCode;
            editPartModelDynamic.MSDSPressureCode = part.MSDSPressureCode;
            editPartModelDynamic.MSDSReference = part.MSDSReference;
            editPartModelDynamic.MSDSRequired = part.MSDSRequired;
            editPartModelDynamic.MSDSTemperatureCode = part.MSDSTemperatureCode;
            editPartModelDynamic.NoEquipXref = part.NoEquipXref;
            editPartModelDynamic.AverageCost = part.AverageCost;
            editPartModelDynamic.AppliedCost = part.AppliedCost;
            editPartModelDynamic.Consignment = part.Consignment;
            editPartModelDynamic.AutoPurch = part.AutoPurch;
            editPartModelDynamic.Location1_5 = part.Location1_5;
            editPartModelDynamic.Location1_1 = part.Location1_1;
            editPartModelDynamic.Location1_2 = part.Location1_2;
            editPartModelDynamic.Location1_3 = part.Location1_3;
            editPartModelDynamic.Location1_4 = part.Location1_4;
            editPartModelDynamic.CountFrequency = part.CountFrequency;
            if (part.LastCounted != null && part.LastCounted != default(DateTime))
            {
                editPartModelDynamic.LastCounted = part.LastCounted;
            }
            else
            {
                editPartModelDynamic.LastCounted = null;
            }
            editPartModelDynamic.QtyOnHand = part.QtyOnHand;
            editPartModelDynamic.QtyMaximum = part.QtyMaximum;
            editPartModelDynamic.QtyReorderLevel = part.QtyReorderLevel;
            editPartModelDynamic.AccountClientLookupId = part.Account_ClientLookupId;
            editPartModelDynamic.QtyOnRequest = part.QtyOnRequest;
            editPartModelDynamic.QtyOnOrder = part.QtyOnOrder;
            editPartModelDynamic.InactiveFlag = part.InactiveFlag;

            return editPartModelDynamic;
        }

        public Part EditPartDynamic(PartsVM objPartVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Part part = new Part()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartId = Convert.ToInt64(objPartVM.EditPart.PartId)
            };
            part.RetriveByPartId(userData.DatabaseKey);

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.EditPart, userData);
            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == false && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = objPartVM.EditPart.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(objPartVM.EditPart);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }

                setpropertyInfo = part.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(part, val);
            }

            part.UpdateByPartId(userData.DatabaseKey);
            
            if (configDetails.Any(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false))
            {
                IEnumerable<string> errors = EditPartUDFDynamic(objPartVM.EditPart, configDetails);
                if (errors != null && errors.Count() > 0)
                {
                    part.ErrorMessages.AddRange(errors);
                }
            }
            return part;
        }

        public List<string> EditPartUDFDynamic(EditPartModelDynamic part, List<UIConfigurationDetailsForModelValidation> configDetails)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            PartUDF partUDF = new PartUDF()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = part.PartId
            };
            partUDF = partUDF.RetrieveByPartId(_dbKey);

            var ColumnDetails = configDetails.Where(x => x.Display == true && x.UDF == true && x.Section == false && x.ViewOnly == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = part.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(part);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                setpropertyInfo = partUDF.GetType().GetProperty(item.ColumnName);
                setpropertyInfo.SetValue(partUDF, val);
            }
            if (partUDF.PartUDFId > 0)
            {
                partUDF.Update(_dbKey);
            }
            else
            {
                partUDF.ClientId = userData.DatabaseKey.Client.ClientId;
                partUDF.PartId = part.PartId;
                partUDF.Create(_dbKey);
            }

            return partUDF.ErrorMessages;
        }
        #endregion

        #region V2-641 PartDetail Dynamic
        public ViewPartModelDynamic MapPartDataForView(ViewPartModelDynamic viewPartModelDynamic, PartModel partmodel)
        {

            viewPartModelDynamic.PartId = partmodel.PartId;
            viewPartModelDynamic.ClientLookupId = partmodel.ClientLookupId;
            viewPartModelDynamic.Description = partmodel.Description;
            viewPartModelDynamic.UPCCode = partmodel.UPCCode;
            viewPartModelDynamic.AccountId = partmodel.AccountId;
            viewPartModelDynamic.StockType = partmodel.StockType;
            viewPartModelDynamic.IssueUnit = partmodel.IssueUnit;
            viewPartModelDynamic.Manufacturer = partmodel.Manufacturer;
            viewPartModelDynamic.ManufacturerId = partmodel.ManufacturerID;
            viewPartModelDynamic.Critical = partmodel.CriticalFlag;
            viewPartModelDynamic.MSDSContainerCode = partmodel.MSDSContainerCode;
            viewPartModelDynamic.MSDSPressureCode = partmodel.MSDSPressureCode;
            viewPartModelDynamic.MSDSReference = partmodel.MSDSReference;
            viewPartModelDynamic.MSDSRequired = partmodel.MSDSRequired;
            viewPartModelDynamic.MSDSTemperatureCode = partmodel.MSDSTemperatureCode;
            viewPartModelDynamic.NoEquipXref = partmodel.NoEquipXref;
            viewPartModelDynamic.AverageCost = partmodel.AverageCost;
            viewPartModelDynamic.AppliedCost = partmodel.AppliedCost;
            viewPartModelDynamic.Consignment = partmodel.Consignment;
            viewPartModelDynamic.AutoPurch = partmodel.AutoPurchaseFlag;//--V2-778
            viewPartModelDynamic.Location1_5 = partmodel.PlaceArea;
            viewPartModelDynamic.Location1_1 = partmodel.Section;
            viewPartModelDynamic.Location1_2 = partmodel.Row;
            viewPartModelDynamic.Location1_3 = partmodel.Shelf;
            viewPartModelDynamic.Location1_4 = partmodel.Bin;
            viewPartModelDynamic.CountFrequency = partmodel.CountFrequency;
            if (partmodel.LastCounted != null && partmodel.LastCounted != default(DateTime))
            {
                viewPartModelDynamic.LastCounted = partmodel.LastCounted;
            }
            else
            {
                viewPartModelDynamic.LastCounted = null;
            }
            viewPartModelDynamic.QtyOnHand = (decimal)partmodel.OnHandQuantity;
            viewPartModelDynamic.QtyMaximum = (decimal)partmodel.Maximum;
            viewPartModelDynamic.QtyReorderLevel = (decimal)partmodel.Minimum;
            viewPartModelDynamic.AccountClientLookupId = partmodel.AccountClientLookupId;
            viewPartModelDynamic.QtyOnRequest = (decimal)partmodel.OnRequestQuantity;
            viewPartModelDynamic.QtyOnOrder = (decimal)partmodel.OnOrderQuantity;
            viewPartModelDynamic.InactiveFlag = partmodel.InactiveFlag;
            viewPartModelDynamic.AutoPurchase = partmodel.AutoPurchase;
            
            viewPartModelDynamic.VendorClientLookupIdVendorName = string.IsNullOrWhiteSpace(partmodel.VendorClientLookupId)
                ? partmodel.VendorName ?? string.Empty
                : string.IsNullOrWhiteSpace(partmodel.VendorName)
                    ? partmodel.VendorClientLookupId
                    : $"{partmodel.VendorClientLookupId}-{partmodel.VendorName}";



            return viewPartModelDynamic;
        }

        public ViewPartModelDynamic MapPartUDFDataForView(ViewPartModelDynamic viewPartModelDynamic, PartUDF partUDF)
        {
            if (partUDF != null)
            {
                viewPartModelDynamic.PartUDFId = partUDF.PartUDFId;

                viewPartModelDynamic.Text1 = partUDF.Text1;
                viewPartModelDynamic.Text2 = partUDF.Text2;
                viewPartModelDynamic.Text3 = partUDF.Text3;
                viewPartModelDynamic.Text4 = partUDF.Text4;

                if (partUDF.Date1 != null && partUDF.Date1 == DateTime.MinValue)
                {
                    viewPartModelDynamic.Date1 = null;
                }
                else
                {
                    viewPartModelDynamic.Date1 = partUDF.Date1;
                }
                if (partUDF.Date2 != null && partUDF.Date2 == DateTime.MinValue)
                {
                    viewPartModelDynamic.Date2 = null;
                }
                else
                {
                    viewPartModelDynamic.Date2 = partUDF.Date2;
                }
                if (partUDF.Date3 != null && partUDF.Date3 == DateTime.MinValue)
                {
                    viewPartModelDynamic.Date3 = null;
                }
                else
                {
                    viewPartModelDynamic.Date3 = partUDF.Date3;
                }
                if (partUDF.Date4 != null && partUDF.Date4 == DateTime.MinValue)
                {
                    viewPartModelDynamic.Date4 = null;
                }
                else
                {
                    viewPartModelDynamic.Date4 = partUDF.Date4;
                }

                viewPartModelDynamic.Bit1 = partUDF.Bit1;
                viewPartModelDynamic.Bit2 = partUDF.Bit2;
                viewPartModelDynamic.Bit3 = partUDF.Bit3;
                viewPartModelDynamic.Bit4 = partUDF.Bit4;

                viewPartModelDynamic.Numeric1 = partUDF.Numeric1;
                viewPartModelDynamic.Numeric2 = partUDF.Numeric2;
                viewPartModelDynamic.Numeric3 = partUDF.Numeric3;
                viewPartModelDynamic.Numeric4 = partUDF.Numeric4;

                viewPartModelDynamic.Select1 = partUDF.Select1;
                viewPartModelDynamic.Select2 = partUDF.Select2;
                viewPartModelDynamic.Select3 = partUDF.Select3;
                viewPartModelDynamic.Select4 = partUDF.Select4;

                viewPartModelDynamic.Select1Name = partUDF.Select1Name;
                viewPartModelDynamic.Select2Name = partUDF.Select2Name;
                viewPartModelDynamic.Select3Name = partUDF.Select3Name;
                viewPartModelDynamic.Select4Name = partUDF.Select4Name;
            }
            return viewPartModelDynamic;
        }


        #endregion

        #region V2-668
        public PartModel PopulatePartDetails_V2(long PartId)
        {
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            obj.RetriveByPartId_V2(userData.DatabaseKey);
            objPart = initializeControls(obj);
            objPart.PartMaster_ClientLookupId = obj.PartMaster_ClientLookupId;
            objPart.LongDescription = obj.LongDescription;
            objPart.ShortDescription = obj.ShortDescription;
            objPart.Category = obj.Category;
            objPart.CategoryDesc = obj.CategoryDesc;
            objPart.VendorClientLookupId = obj.VendorClientlookupId;
            objPart.VendorName = obj.VendorName;
            objPart.AutoPurchase = obj.AutoPurchase;
            objPart.PartStoreroomId = obj.PartStoreroomId;
            return objPart;
        }

        #endregion

        #region V2-880
        public List<PartsVendorCatalogItemGridModel> GetPartVendorCatalogItem(long PartMasterId, int skip = 0, int length = 10, string orderbycol = "1", string orderDir = "asc")
        {
            List<PartsVendorCatalogItemGridModel> VendorCatalogItemModelList = new List<PartsVendorCatalogItemGridModel>();
            PartsVendorCatalogItemGridModel objVendorCatalogItemModel;
            VendorCatalogItem vendorCatalogItem = new VendorCatalogItem()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartMasterId = PartMasterId
            };
            //**
            vendorCatalogItem.OrderbyColumn = orderbycol;
            vendorCatalogItem.OrderBy = orderDir;
            vendorCatalogItem.offset1 = skip;
            vendorCatalogItem.nextrow = length;
            //**
            List<VendorCatalogItem> VendorCatalogItemList = VendorCatalogItem.RetrieveByPartMasterId_V2(this.userData.DatabaseKey, vendorCatalogItem);
            if (VendorCatalogItemList != null)
            {
                //var workOrderList = workOrderIdVendorCatalogItemList.Select(x => new { x.WorkOrderId, x.WorkOrderClientLookupId, x.DateDown, x.MinutesDown, x.VendorCatalogItemId, x.ReasonForDownDescription, x.TotalCount, x.TotalMinutesDown }).ToList();
                foreach (var v in VendorCatalogItemList)
                {
                    objVendorCatalogItemModel = new PartsVendorCatalogItemGridModel();
                    objVendorCatalogItemModel.VendorCatalogItemId = v.VendorCatalogItemId;
                    objVendorCatalogItemModel.Vendor_Name = v.VendorName;
                    objVendorCatalogItemModel.UnitCost = v.UnitCost;
                    objVendorCatalogItemModel.PurchaseUOM = v.PurchaseUOM;
                    objVendorCatalogItemModel.TotalCount = v.TotalCount;
                    VendorCatalogItemModelList.Add(objVendorCatalogItemModel);
                }
            }
            return VendorCatalogItemModelList;
        }

        #endregion

        #region V2-906     
        public Part GetUpdatePartCosts(long PartId, decimal? appliedCost, decimal? averageCost)
        { 
                Part part = new Part();
                part.ClientId = userData.DatabaseKey.Client.ClientId;
                part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                part.PartId = PartId;
                part.Retrieve(userData.DatabaseKey);
            part.AppliedCost = appliedCost==null?0:(Decimal)appliedCost;
            part.AverageCost = averageCost==null?0:(Decimal)averageCost;
            part.Update(userData.DatabaseKey);
            return part;
        }
        public List<string> CreatePartEvent(long PartId, string Event,string Comments)
        {
            PartEventLog partEventLog = new PartEventLog();
            partEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            partEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            partEventLog.PartId = PartId;
            partEventLog.TransactionDate = DateTime.UtcNow;
            partEventLog.Event = Event;            
            partEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            partEventLog.Comments = Comments;
            partEventLog.SourceId = 0;
            partEventLog.Create(userData.DatabaseKey);
            return partEventLog.ErrorMessages;
        }
        #endregion

        #region V2-1203
        public AddPartModelDynamic RetrievePartDetailsByPartIdForPartModel(long PartId)
        {
            AddPartModelDynamic addPartModelDynamic = new AddPartModelDynamic();
            Part part = RetrievePartByPartId(PartId);
            PartUDF partUDF = RetrievePartUDFByPartId(PartId);

            addPartModelDynamic = MapPartUDFDataForPartModel(addPartModelDynamic, partUDF);
            addPartModelDynamic = MapPartDataForPartModel(addPartModelDynamic, part);
            return addPartModelDynamic;
        }

        private AddPartModelDynamic MapPartUDFDataForPartModel(AddPartModelDynamic addPartModelDynamic, PartUDF partUDF)
        {
            if (partUDF != null)
            {
                addPartModelDynamic.Text1 = partUDF.Text1;
                addPartModelDynamic.Text2 = partUDF.Text2;
                addPartModelDynamic.Text3 = partUDF.Text3;
                addPartModelDynamic.Text4 = partUDF.Text4;
                addPartModelDynamic.Date1 = partUDF.Date1 == DateTime.MinValue ? null : partUDF.Date1;
                addPartModelDynamic.Date2 = partUDF.Date2 == DateTime.MinValue ? null : partUDF.Date2;
                addPartModelDynamic.Date3 = partUDF.Date3 == DateTime.MinValue ? null : partUDF.Date3;
                addPartModelDynamic.Date4 = partUDF.Date4 == DateTime.MinValue ? null : partUDF.Date4;

                addPartModelDynamic.Bit1 = partUDF.Bit1;
                addPartModelDynamic.Bit2 = partUDF.Bit2;
                addPartModelDynamic.Bit3 = partUDF.Bit3;
                addPartModelDynamic.Bit4 = partUDF.Bit4;
                addPartModelDynamic.Numeric1 = partUDF.Numeric1;
                addPartModelDynamic.Numeric2 = partUDF.Numeric2;
                addPartModelDynamic.Numeric3 = partUDF.Numeric3;
                addPartModelDynamic.Numeric4 = partUDF.Numeric4;
                addPartModelDynamic.Select1 = partUDF.Select1;
                addPartModelDynamic.Select2 = partUDF.Select2;
                addPartModelDynamic.Select3 = partUDF.Select3;
                addPartModelDynamic.Select4 = partUDF.Select4;
            }
            return addPartModelDynamic;
        }
        public AddPartModelDynamic MapPartDataForPartModel(AddPartModelDynamic addPartModelDynamic, Part part)
        {
            addPartModelDynamic.ClientLookupId = part.ClientLookupId;
            addPartModelDynamic.Description = part.Description;
            addPartModelDynamic.UPCCode = part.UPCCode;
            addPartModelDynamic.AccountId = part.AccountId;
            addPartModelDynamic.StockType = part.StockType;
            addPartModelDynamic.IssueUnit = part.IssueUnit;
            addPartModelDynamic.Manufacturer = part.Manufacturer;
            addPartModelDynamic.ManufacturerId = part.ManufacturerId;
            addPartModelDynamic.Critical = part.Critical;
            addPartModelDynamic.MSDSContainerCode = part.MSDSContainerCode;
            addPartModelDynamic.MSDSPressureCode = part.MSDSPressureCode;
            addPartModelDynamic.MSDSReference = part.MSDSReference;
            addPartModelDynamic.MSDSRequired = part.MSDSRequired;
            addPartModelDynamic.MSDSTemperatureCode = part.MSDSTemperatureCode;
            addPartModelDynamic.NoEquipXref = part.NoEquipXref;
            addPartModelDynamic.AverageCost = part.AverageCost;
            addPartModelDynamic.AppliedCost = part.AppliedCost;
            addPartModelDynamic.Consignment = part.Consignment;
            addPartModelDynamic.AutoPurch = part.AutoPurch;
            addPartModelDynamic.Location1_5 = part.Location1_5;
            addPartModelDynamic.Location1_1 = part.Location1_1;
            addPartModelDynamic.Location1_2 = part.Location1_2;
            addPartModelDynamic.Location1_3 = part.Location1_3;
            addPartModelDynamic.Location1_4 = part.Location1_4;
            addPartModelDynamic.CountFrequency = part.CountFrequency;
            if (part.LastCounted != null && part.LastCounted != default(DateTime))
            {
                addPartModelDynamic.LastCounted = part.LastCounted;
            }
            else
            {
                addPartModelDynamic.LastCounted = null;
            }
            addPartModelDynamic.QtyOnHand = part.QtyOnHand;
            addPartModelDynamic.QtyMaximum = part.QtyMaximum;
            addPartModelDynamic.QtyReorderLevel = part.QtyReorderLevel;
            addPartModelDynamic.AccountClientLookupId = part.Account_ClientLookupId;
            addPartModelDynamic.QtyOnRequest = part.QtyOnRequest;

            return addPartModelDynamic;
        }

        public Part AddPartsModel(PartsVM _PartModel)
        {
            Part objPart = new Part();
            objPart = AddPartModelPartsDynamic(_PartModel);
            return objPart;
        }
        public Part AddPartModelPartsDynamic(PartsVM partModel)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Part part = new Part();
            PartStoreroom partStoreroom = new PartStoreroom();
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;

            List<UIConfigurationDetailsForModelValidation> configDetails = new RetrieveDataForUIConfiguration()
                                        .Retrieve(DataDictionaryViewNameConstant.AddPart, userData);
            var ColumnDetails = configDetails.Where(x => x.Display && x.UDF == false && x.Section == false);
            foreach (var item in ColumnDetails)
            {
                getpropertyInfo = partModel.AddPart.GetType().GetProperty(item.ColumnName);
                object val = getpropertyInfo.GetValue(partModel.AddPart);

                Type t = getpropertyInfo.PropertyType;

                if (val == null)
                {
                    AssignDefaultOrNullValue(ref val, t);
                }
                if (item.TableName == AttachmentTableConstant.Part)
                {
                    setpropertyInfo = part.GetType().GetProperty(item.ColumnName);
                    setpropertyInfo.SetValue(part, val);
                }
                else
                {
                    setpropertyInfo = partStoreroom.GetType().GetProperty(item.ColumnName);
                    setpropertyInfo.SetValue(partStoreroom, val);
                }
            }

            part.ValidateAddMultiStoreroomPart(this.userData.DatabaseKey);

            if (part.ErrorMessages == null || part.ErrorMessages.Count == 0)
            {
                part.Create(_dbKey);
                if (part.ErrorMessages == null||part.ErrorMessages.Count==0)
                {
                    //Add PartUDFDynamic
                    if (configDetails.Any(x => x.Display && x.UDF && x.Section == false))
                    {
                        IEnumerable<string> errorsPartUDF = AddPartUDFDynamic(partModel.AddPart, part.PartId, configDetails);
                        if (errorsPartUDF != null && errorsPartUDF.Count() > 0)
                        {
                            part.ErrorMessages.AddRange(errorsPartUDF);
                        }
                    }
                    //Add PartStoreroom
                    partStoreroom.PartId = part.PartId;
                    partStoreroom.ClientId = _dbKey.Client.ClientId;
                    partStoreroom.Location1_1 = partModel.AddPart.Location1_1;
                    partStoreroom.Location1_2 = partModel.AddPart.Location1_2;
                    partStoreroom.Location1_3 = partModel.AddPart.Location1_3;
                    partStoreroom.Location1_4 = partModel.AddPart.Location1_4;
                    partStoreroom.Location1_5 = partModel.AddPart.Location1_5;
                    partStoreroom.QtyMaximum = partModel.AddPart.QtyMaximum;
                    partStoreroom.QtyReorderLevel = partModel.AddPart.QtyReorderLevel;
                    partStoreroom.Create(userData.DatabaseKey);

                    //Add Equipment_Parts_Xref
                    if (partModel.AddPart.Copy_Equipment_Xref)
                    {
                        Equipment_Parts_Xref eqx = new Equipment_Parts_Xref()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            PartId = partModel.CurrentPartId
                        };
                        List<Equipment_Parts_Xref> equipmentList = Equipment_Parts_Xref.RetriveByPartId(_dbKey, eqx);
                        if (equipmentList != null)
                        {
                            foreach (var data in equipmentList)
                            {
                                Equipment_Parts_Xref equipment = new Equipment_Parts_Xref();
                                equipment.PartId = part.PartId;
                                equipment.EquipmentId = data.EquipmentId;
                                equipment.Comment = data.Comment ?? String.Empty;
                                equipment.QuantityNeeded = data.QuantityNeeded;
                                equipment.Create(_dbKey);
                            }
                        }
                    }
                    //Add Part_Vendor_Xref
                    if (partModel.AddPart.Copy_Vendor_Xref)
                    {
                        Part_Vendor_Xref vendor = new Part_Vendor_Xref()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            SiteId = _dbKey.User.DefaultSiteId,
                            PartId = partModel.CurrentPartId

                        };
                        List<Part_Vendor_Xref> vendorList = Part_Vendor_Xref.RetrieveListByPartId(_dbKey, vendor);
                        if (vendorList != null)
                        {
                            foreach (var data in vendorList)
                            {
                                vendor = new Part_Vendor_Xref();
                                vendor.PartId = part.PartId;
                                vendor.VendorId = data.VendorId;
                                vendor.PreferredVendor = data.PreferredVendor;
                                vendor.CatalogNumber = data?.CatalogNumber ?? string.Empty;
                                vendor.IssueOrder = data.IssueOrder;
                                vendor.Manufacturer = data?.Manufacturer ?? string.Empty;
                                vendor.ManufacturerId = data?.ManufacturerId;
                                vendor.OrderQuantity = data?.OrderQuantity ?? 0;
                                vendor.OrderUnit = data?.OrderUnit ?? string.Empty;
                                vendor.Price = data.Price;
                                vendor.UOMConvRequired = data.UOMConvRequired;
                                vendor.Punchout = data.Punchout;
                                vendor.Create(_dbKey);
                            }
                        }
                    }
                    //Add Notes
                    if (partModel.AddPart.Copy_Notes)
                    {
                        Notes notes = new Notes()
                        {
                            ObjectId = partModel.CurrentPartId,
                            TableName = AttachmentTableConstant.Part
                        };
                        List<Notes> NotesList = notes.RetrieveByObjectId(userData.DatabaseKey, userData.Site.TimeZone);
                        if (NotesList != null)
                        {
                            foreach (var item in NotesList)
                            {
                                notes = new Notes();
                                notes.OwnerId = item.OwnerId;
                                notes.OwnerName = item.OwnerName;
                                notes.Subject = item.Subject;
                                notes.Content = item.Content;
                                notes.Type = item.Type;
                                notes.ObjectId=part.PartId;
                                notes.TableName = AttachmentTableConstant.Part;
                                notes.Create(_dbKey);
                                
                            }

                        }
                    }
                    //Add PartEventLog 
                    PartEventLog partEventLog = new PartEventLog();
                    partEventLog.ClientId = userData.DatabaseKey.User.ClientId;
                    partEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    partEventLog.PartId =part.PartId;
                    partEventLog.TransactionDate = DateTime.UtcNow;
                    partEventLog.Event = "Create";
                    partEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
                    partEventLog.Comments = "Created from Part Model";
                    partEventLog.SourceId = part.PartId;
                    partEventLog.Create(userData.DatabaseKey);
                }

            }
            return part;

        }

        #endregion
        #region V2-1196
        public Part_Vendor_Xref AddPartVendorXrefPartsConfigureAutoPurchasing(long PartId, long VendorId, string VendorClientLoopId)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            string Vendor_ClientLookupId = VendorClientLoopId;
            Vendor vendor = new Vendor { ClientId = _dbKey.Client.ClientId, SiteId = _dbKey.User.DefaultSiteId, ClientLookupId = Vendor_ClientLookupId };
               var ValidVendor = vendor.RetrieveBySiteIdAndClientLookUpId(_dbKey).FirstOrDefault();
            if (ValidVendor != null && ValidVendor.VendorId == VendorId)
            { 
                
                    pvx.PartId = PartId;
                    pvx.VendorId = ValidVendor.VendorId;
                    pvx.PreferredVendor = false;
                    pvx.CatalogNumber =  string.Empty;
                    pvx.Manufacturer =  string.Empty;
                    pvx.ManufacturerId =  string.Empty;
                    pvx.OrderQuantity = 0;
                    pvx.OrderUnit =  string.Empty;
                    pvx.Price = 0;
                    pvx.IssueOrder = 1;
                    pvx.UOMConvRequired = false;
                    pvx.Punchout = false;
                    pvx.ValidateAdd(_dbKey);
                    if (pvx.ErrorMessages.Count == 0)
                    {
                        pvx.Create(_dbKey);
                    }

            }
            else
            {
                pvx.ErrorMessages = new List<string>();
                pvx.ErrorMessages.Add(GlobalConstants.InvalidVendorRecord);
            }

                return pvx;
        }
        public PartStoreroom UpdatePartsConfigureAutoPurchasing(PartsConfigureAutoPurchasingModel partsConfigureAutoPurchasingModel)
        {
           
            PartStoreroom partStoreroom = new PartStoreroom();
            if (partsConfigureAutoPurchasingModel != null && partsConfigureAutoPurchasingModel.PartStoreroomId > 0)
            {
                partStoreroom.PartStoreroomId = partsConfigureAutoPurchasingModel.PartStoreroomId;
                partStoreroom.Retrieve(_dbKey);
                partStoreroom.AutoPurchase = partsConfigureAutoPurchasingModel.IsAutoPurchase;
                partStoreroom.PartVendorId = partsConfigureAutoPurchasingModel.PartVendorId??0;
                partStoreroom.QtyMaximum = partsConfigureAutoPurchasingModel.QtyMaximum ?? 0;
                partStoreroom.QtyReorderLevel = partsConfigureAutoPurchasingModel.QtyReorderLevel ?? 0;
                partStoreroom.Update(userData.DatabaseKey);
            }
            else
            {
                partStoreroom.ErrorMessages = new List<string>();
                partStoreroom.ErrorMessages.Add(GlobalConstants.InvalidRecord);
            }
            return partStoreroom;
        }
        #endregion
    }
}
