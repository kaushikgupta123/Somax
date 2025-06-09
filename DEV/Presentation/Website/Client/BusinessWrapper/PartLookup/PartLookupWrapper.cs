using Client.BusinessWrapper.Common;
using Client.Common.Constants;
using Client.Models.PartLookup;
using Common.Constants;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.BusinessWrapper.PartLookup
{
    public class PartLookupWrapper
    {
        private DatabaseKey _dbKey;
        private UserData _userData;

        public PartLookupWrapper(UserData userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        public List<PartLookupModel> SearchForCart(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "",long VendorId=0)
        {

            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            part.VendorId = VendorId;
            parts = part.SearchForCart(_dbKey);

            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                partLookupModel.indexid = item.IndexId;
                partLookupModel.PartId = item.PartId;
                partLookupModel.ClientLookupId = item.ClientLookupId;
                partLookupModel.Description = item.Description;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.AppliedCost;
                partLookupModel.Unit = item.IssueUnit;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL= item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    SASImageURL = commonWrapper.GetNoImageUrl();
                }
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.ImageUrl = SASImageURL;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.IssueOrder = item.IssueOrder;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                //V2-932
                partLookupModel.OnOrderQty = item.OnOrderQty;
                partLookupModel.OnRequestQTY = item.OnRequestQTY;
                partLookupModels.Add(partLookupModel);
            }
            return partLookupModels;
        }

        public List<PartLookupModel> SearchForCartWO(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "")
        {

            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;

            parts = part.SearchForCartWO(_dbKey);
            //long index = skip;
            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                partLookupModel.PartId = item.PartId;
                partLookupModel.indexid = item.IndexId;
                partLookupModel.ClientLookupId = item.ClientLookupId;
                partLookupModel.Description = item.Description;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.AppliedCost;
                partLookupModel.Unit = item.IssueUnit;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL = item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    SASImageURL = commonWrapper.GetNoImageUrl();
                }
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.ImageUrl = SASImageURL;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.IssueOrder = item.IssueOrder;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                partLookupModel.VendorId = item.VendorId;
                partLookupModel.VendorClientlookupId = item.VendorClientlookupId;
                //V2-1068
                partLookupModel.AccountId = item.AccountId;// V2 - 1068 AccountId 
                partLookupModel.UnitOfMeasure = item.IssueUnit; //V2-1068 UnitOfMeasure is already taken IssueUnit from Part table
                partLookupModels.Add(partLookupModel);
                //index++;
            }
            return partLookupModels;
        }
        public List<PartLookupModel> SearchForCart_VendorCatalog(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "",long VendorId=0)
        {
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            part.VendorId = VendorId; //V2-553
            parts = part.SearchForCart_VendorCatalog(_dbKey);
            long index = 0;
            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                if (item.CatalogType == "Catalog")
                {
                    partLookupModel.InVendorCatalog = true;
                }
                partLookupModel.indexid = item.IndexId;
                partLookupModel.PartId = item.PartId;
                partLookupModel.indexid = index;
                partLookupModel.ClientLookupId = item.PMClientlookupId;
                partLookupModel.Description = item.PMDescription;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.UnitCost;
                partLookupModel.Unit = item.PurchaseUOM;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL= item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                partLookupModel.ImageUrl = SASImageURL;
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.VendorPartNumber = item.VendorPartNumber;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424

                //V2-553
                partLookupModel.IssueUOM = item.IssueUOM;
                partLookupModel.UOMConversion = item.UOMConversion;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.VC_Count = item.VC_Count;
                partLookupModel.VendorId = item.VendorId;
                partLookupModel.VendorCatalogItemId = item.VendorCatalogItemId;
                partLookupModel.IssueOrder = item.UOMConversion;
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                partLookupModel.IssueUnit = item.IssueUnit;
                partLookupModel.PartCategoryMasterId = item.PartCategoryMasterId;
                //V2-932
                partLookupModel.OnOrderQty = item.OnOrderQty;
                partLookupModel.OnRequestQTY = item.OnRequestQTY;
                partLookupModel.AccountId = item.AccountId;//V2-1068
                partLookupModels.Add(partLookupModel);
                index++;
            }
            return partLookupModels;
        }


        public List<PartLookupModel> SearchForCart_VendorCatalogWO(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "")
        {
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            //  part.VendorId = VendorId; //V2-553
            parts = part.SearchForCart_VendorCatalogWO(_dbKey);
            //int index = skip;
            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                if (item.CatalogType == "Catalog")
                {
                    partLookupModel.InVendorCatalog = true;
                }
                partLookupModel.PartId = item.PartId;
                partLookupModel.ClientLookupId = item.PMClientlookupId;
                partLookupModel.Description = item.PMDescription;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.UnitCost;
                partLookupModel.Unit = item.PurchaseUOM;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL = item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                partLookupModel.ImageUrl = SASImageURL;
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.VendorPartNumber = item.VendorPartNumber;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424

                //V2-553
                partLookupModel.IssueUOM = item.IssueUOM;
                partLookupModel.UOMConversion = item.UOMConversion;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.VC_Count = item.VC_Count;
                partLookupModel.VendorId = item.VendorId;
                partLookupModel.VendorCatalogItemId = item.VendorCatalogItemId;
                partLookupModel.IssueOrder = item.UOMConversion;
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                partLookupModel.IssueUnit = item.IssueUnit;
                //V2-690
                partLookupModel.PartCategoryMasterId = item.PartCategoryMasterId;
                partLookupModel.VendorClientlookupId = item.VendorClientlookupId;
                partLookupModel.indexid = item.IndexId;
                partLookupModel.AccountId = item.AccountId;//V2-1068
                partLookupModel.UnitOfMeasure = item.IssueUOM; //V2- 1068
                partLookupModels.Add(partLookupModel);
                //index++;
            }
            return partLookupModels;
        }
        //
        public PurchaseOrderLineItem PartLookUpPurrchaseOrder(PartAddToCartProcessModel PartAddToCartProcessModel)
        {
            PurchaseOrderLineItem pordLineItem = new PurchaseOrderLineItem();
            pordLineItem.ClientId=_dbKey.Client.ClientId;
            pordLineItem.PurchaseRequestId = PartAddToCartProcessModel.PurchaseRequestID;
            pordLineItem.Description = PartAddToCartProcessModel.Description;
            pordLineItem.LineNumber = PartAddToCartProcessModel.LineNumber;
            pordLineItem.PartId = int.Parse(PartAddToCartProcessModel.PartId);
            pordLineItem.OrderQuantity = PartAddToCartProcessModel.OrderQuantity;
            pordLineItem.UnitCost = PartAddToCartProcessModel.UnitCost;

            pordLineItem.CreateFromShoppingCart(_dbKey);
            return pordLineItem;
        }
        public PurchaseRequestLineItem PartLookUpPurrchaseRequest(PartAddToCartProcessModel PartAddToCartProcessModel)
        {
            PurchaseRequestLineItem pordLineItem = new PurchaseRequestLineItem();
            pordLineItem.ClientId = _dbKey.Client.ClientId;
            pordLineItem.PurchaseRequestId = PartAddToCartProcessModel.PurchaseRequestID;
            pordLineItem.Description = PartAddToCartProcessModel.Description;
            pordLineItem.LineNumber = PartAddToCartProcessModel.LineNumber;
            pordLineItem.PartId = int.Parse(PartAddToCartProcessModel.PartId);
            pordLineItem.OrderQuantity = PartAddToCartProcessModel.OrderQuantity;
            pordLineItem.UnitCost = PartAddToCartProcessModel.UnitCost;
            pordLineItem.CreateFromShoppingCart(_dbKey);
            return pordLineItem;
        }

        internal Dictionary<long, string> InsertPartLookUpPurchaseRequest(List<PartAddToCartProcessModel> list, long StoreroomId = 0)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            PurchaseRequestLineItem purchaseRequestLineitem;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        purchaseRequestLineitem = new PurchaseRequestLineItem();
                        Part part = new Part()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            PartId = int.Parse(item.PartId),
                        };
                        if (_userData.DatabaseKey.Client.UseMultiStoreroom)   /*V2-738*/
                        {
                            part.StoreroomId = StoreroomId;
                            part.RetriveByPartIdForMultiStoreroom_V2(_dbKey);
                            purchaseRequestLineitem.StoreroomId = StoreroomId;
                        }
                        else
                        {
                            part.RetriveByPartId(_dbKey);
                        }

                        purchaseRequestLineitem.ClientId = _dbKey.Client.ClientId;
                        purchaseRequestLineitem.PurchaseRequestId = item.PurchaseRequestID;
                        purchaseRequestLineitem.AccountId = part.AccountId;
                        purchaseRequestLineitem.Description = part.Description;
                        purchaseRequestLineitem.LineNumber = item.LineNumber;
                        purchaseRequestLineitem.PartId = int.Parse(item.PartId);
                        purchaseRequestLineitem.OrderQuantity = item.OrderQuantity;
                        purchaseRequestLineitem.UnitCost = item.UnitCost;
                        purchaseRequestLineitem.UnitofMeasure = part.IssueUnit;

                        purchaseRequestLineitem.PurchaseUOM = item.UOMConvRequired ? item.PurchaseUOM : item.IsVendorCatalog ? item.PurchaseUOM : part.IssueUnit;
                        purchaseRequestLineitem.UOMConversion = item.UOMConvRequired ? item.IssueOrder : 1;
                        purchaseRequestLineitem.VendorCatalogItemId = item.IsVendorCatalog ? item.VendorCatalogItemId : 0;
                        purchaseRequestLineitem.Creator_PersonnelId= _userData.DatabaseKey.Personnel.PersonnelId;
                        if (item.RequiredDate == null)
                        {
                            purchaseRequestLineitem.RequiredDate = DateTime.MinValue;
                        }
                        else
                        {
                            purchaseRequestLineitem.RequiredDate = item.RequiredDate;
                        }
                        if (_userData.Site.ShoppingCart)
                        {
                            purchaseRequestLineitem.UNSPSC = item.PartCategoryMasterId;

                        }
                        else
                        {
                            purchaseRequestLineitem.UNSPSC = 0;

                        }
                        if (_userData.DatabaseKey.Client.UseMultiStoreroom)   /*V2-738*/
                        {
                            purchaseRequestLineitem.CreateFromShoppingCartForMultiStoreroom(_dbKey);

                        }
                        else
                        {
                            purchaseRequestLineitem.CreateFromShoppingCart(_dbKey);
                        }
                        purchaseRequestLineitem.PRReOrderLineNumber(_dbKey);
                    }
                }

            }
            return retValue;

        }

        internal Dictionary<long, string> InsertPartLookUpPurchaseOrder(List<PartAddToCartProcessModel> list, long StoreroomId = 0)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            PurchaseOrderLineItem purchaseOrderLineitem;
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        purchaseOrderLineitem = new PurchaseOrderLineItem();
                        Part part = new Part()
                        {
                            ClientId = _dbKey.Client.ClientId,
                            PartId = int.Parse(item.PartId),
                        };
                        if (_userData.DatabaseKey.Client.UseMultiStoreroom)   /*V2-738*/
                        {
                            part.StoreroomId = StoreroomId;
                            part.RetriveByPartIdForMultiStoreroom_V2(_dbKey);
                            purchaseOrderLineitem.StoreroomId = StoreroomId;
                        }
                        else
                        {
                            part.RetriveByPartId(_dbKey);
                        }



                        purchaseOrderLineitem.ClientId = _dbKey.Client.ClientId;
                        purchaseOrderLineitem.PurchaseOrderId = item.PurchaseOrderID;
                        purchaseOrderLineitem.AccountId = part.AccountId;
                        purchaseOrderLineitem.Description = item.Description;
                        purchaseOrderLineitem.LineNumber = item.LineNumber;
                        purchaseOrderLineitem.PartId = int.Parse(item.PartId);
                        purchaseOrderLineitem.OrderQuantity = item.OrderQuantity;
                        purchaseOrderLineitem.UnitCost = item.UnitCost;
                        purchaseOrderLineitem.UnitOfMeasure = item.UnitofMeasure ?? string.Empty;
                        purchaseOrderLineitem.Status = PurchaseOrderStatusConstants.Open;
                        purchaseOrderLineitem.PurchaseUOM = item.UOMConvRequired ? item.PurchaseUOM : item.IsVendorCatalog ? item.PurchaseUOM : part.IssueUnit;
                        purchaseOrderLineitem.UOMConversion = item.UOMConvRequired ? item.IssueOrder : 1;
                        purchaseOrderLineitem.VendorCatalogItemId = item.IsVendorCatalog ? item.VendorCatalogItemId : 0;
                        purchaseOrderLineitem.Creator_PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                        //purchaseOrderLineitem.AccountId = part.AccountId;
                        if (_userData.Site.ShoppingCart)
                        {
                            purchaseOrderLineitem.UNSPSC = item.PartCategoryMasterId;

                        }
                        else
                        {
                            purchaseOrderLineitem.UNSPSC = 0;

                        }
                        if (_userData.DatabaseKey.Client.UseMultiStoreroom)   /*V2-738*/
                        {
                            purchaseOrderLineitem.CreateFromShoppingCartForMultiStoreroom(_dbKey);
                        }
                        else
                        {
                            purchaseOrderLineitem.CreateFromShoppingCart(_dbKey);
                        }
                        purchaseOrderLineitem.ReOrderLineNumber(_dbKey);
                    }
                }

            }
            return retValue;

        }

        #region V2-563
        internal List<AdditionalCatalogItemModel> PopulateAdditionalCatalogitems(long PartId, string orderbycol = "", string orderDir = "", int skip = 0, int length = 0)
        {
            AdditionalCatalogItemModel objAdditionalCatalog;
            List<AdditionalCatalogItemModel> AdditionalCatalogList = new List<AdditionalCatalogItemModel>();
            Part part = new Part();
            part.ClientId = this._userData.DatabaseKey.Client.ClientId;
            part.SiteId = this._userData.DatabaseKey.Personnel.SiteId;
            part.PartId = PartId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            var AdditionalCatalog = part.RetrieveCalatogEntriesForPartByPartId(this._userData.DatabaseKey);

            if (AdditionalCatalog != null)
            {
                foreach (var item in AdditionalCatalog)
                {
                    objAdditionalCatalog = new AdditionalCatalogItemModel();
                    objAdditionalCatalog.PartId = item.PartId;
                    objAdditionalCatalog.VendorId = item.VendorId;
                    objAdditionalCatalog.PartClientLookupId = item.ClientLookupId;
                    objAdditionalCatalog.VendorClientLookupId = item.VendorClientlookupId;
                    objAdditionalCatalog.VendorName = item.VendorName;
                    objAdditionalCatalog.VendorPartNumber = item.VendorPartNumber;
                    objAdditionalCatalog.UnitCost = Math.Round(item.UnitCost, 2);
                    objAdditionalCatalog.PurchaseUOM = item.PurchaseUOM;
                    objAdditionalCatalog.Description = item.Description;
                    objAdditionalCatalog.PartStoreroomId = item.PartStoreroomId;
                    objAdditionalCatalog.IssueUnit = item.IssueUnit;
                    objAdditionalCatalog.VendorCatalogItemid = item.VendorCatalogItemId;

                    AdditionalCatalogList.Add(objAdditionalCatalog);
                }
            }

            return AdditionalCatalogList;
        }
        #endregion

        #region V2-690
        internal Dictionary<long, string> InsertPartLookUpWorkOrder(List<PartAddToCartProcessModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            EstimatedCosts estimatedCosts;
            //V2-726 Start
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(_userData.DatabaseKey).FirstOrDefault();
            //V2-726 End
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        estimatedCosts = new EstimatedCosts();
                        estimatedCosts.ClientId = _dbKey.Client.ClientId;
                        estimatedCosts.ObjectId = item.WorkOrderID;
                        estimatedCosts.ObjectType = "WorkOrder";
                        estimatedCosts.Category = "Parts";
                        estimatedCosts.CategoryId = int.Parse(item.PartId);
                        estimatedCosts.Description = item.Description;
                        estimatedCosts.Quantity = item.OrderQuantity;
                        estimatedCosts.UnitCost = item.UnitCost;
                        estimatedCosts.VendorId = item.VendorId;
                        estimatedCosts.Source = "Internal";
                        estimatedCosts.UpdateIndex = 0;
                        estimatedCosts.UNSPSC = item.PartCategoryMasterId;
                        estimatedCosts.AccountId=item.AccountId;
                        estimatedCosts.UnitOfMeasure=item.UnitOfMeasureIssue;

                        //V2-726 Start
                        if (approvalGroupSettingsList != null)
                        {
                            if (approvalGroupSettingsList.MaterialRequests == true)
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Initiated;
                            }
                            else
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Approved;
                            }
                        }
                        else
                        {
                            estimatedCosts.Status ="";
                        }
                        //V2-726 End
                        estimatedCosts.CreateFromShoppingCart(_dbKey);
                    }
                }

            }
            return retValue;

        }
        #endregion

        #region V2-691
        internal Dictionary<long, string> InsertPartLookUpMaterialRequest(List<PartAddToCartProcessModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            EstimatedCosts estimatedCosts;
            //V2-726 Start
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(_userData.DatabaseKey).FirstOrDefault();
            //V2-726 End
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        //Part part = new Part()
                        //{
                        //    ClientId = _dbKey.Client.ClientId,
                        //    PartId = int.Parse(item.PartId),
                        //};

                        //part.RetriveByPartId(_dbKey);
                        estimatedCosts = new EstimatedCosts();


                        estimatedCosts.ClientId = _dbKey.Client.ClientId;
                        estimatedCosts.ObjectId = item.MaterialRequestId;
                        estimatedCosts.ObjectType = "MR";
                        estimatedCosts.Category = "Parts";
                        estimatedCosts.CategoryId = int.Parse(item.PartId);
                        estimatedCosts.Description = item.Description;
                        estimatedCosts.Quantity = item.OrderQuantity;
                        estimatedCosts.UnitCost = item.UnitCost;
                        estimatedCosts.VendorId = item.VendorId;
                        estimatedCosts.Source = "Internal";
                        estimatedCosts.UpdateIndex = 0;
                        estimatedCosts.UNSPSC = item.PartCategoryMasterId;
                        //V2-726 Start
                        if (approvalGroupSettingsList != null)
                        {
                            if (approvalGroupSettingsList.MaterialRequests == true)
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Initiated;
                            }
                            else
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Approved;
                            }
                        }
                        else
                        {
                            estimatedCosts.Status ="";
                        }
                        //V2-726 End
                        estimatedCosts.CreateFromShoppingCart(_dbKey);
                    }
                }

            }
            return retValue;

        }
        #endregion

        #region V2-732

        public List<PartLookupModel> SearchForCartWOMultiStoreroom(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "",long storeroomId=0)
        {

            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            part.StoreroomId = storeroomId;

            parts = part.SearchForCartWOMultiStoreroom(_dbKey);
            //long index = skip;
            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                partLookupModel.PartId = item.PartId;
                partLookupModel.indexid = item.IndexId;
                partLookupModel.ClientLookupId = item.ClientLookupId;
                partLookupModel.Description = item.Description;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.AppliedCost;
                partLookupModel.Unit = item.IssueUnit;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL = item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    SASImageURL = commonWrapper.GetNoImageUrl();
                }
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.ImageUrl = SASImageURL;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.IssueOrder = item.IssueOrder;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                partLookupModel.VendorId = item.VendorId;
                partLookupModel.VendorClientlookupId = item.VendorClientlookupId;
                partLookupModel.StoreroomId = item.StoreroomId;
                partLookupModel.PartStoreroomId = item.PartStoreroomId;
                partLookupModel.AccountId = item.AccountId; //V2-1068
                partLookupModel.UnitOfMeasure = item.IssueUnit; //V2-1068
                partLookupModels.Add(partLookupModel);
                //index++;
            }
            return partLookupModels;
        }
        public List<PartLookupModel> SearchForCart_VendorCatalogWOMultiStoreroom(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "", long storeroomId = 0)
        {
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            part.StoreroomId = storeroomId;
            //  part.VendorId = VendorId; //V2-553
            parts = part.SearchForCart_VendorCatalogWOMultiStoreroom(_dbKey);
            //int index = skip;
            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                if (item.CatalogType == "Catalog")
                {
                    partLookupModel.InVendorCatalog = true;
                }
                partLookupModel.PartId = item.PartId;
                partLookupModel.ClientLookupId = item.PMClientlookupId;
                partLookupModel.Description = item.PMDescription;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.UnitCost;
                partLookupModel.Unit = item.PurchaseUOM;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL = item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                partLookupModel.ImageUrl = SASImageURL;
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.VendorPartNumber = item.VendorPartNumber;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424

                //V2-553
                partLookupModel.IssueUOM = item.IssueUOM;
                partLookupModel.UOMConversion = item.UOMConversion;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.VC_Count = item.VC_Count;
                partLookupModel.VendorId = item.VendorId;
                partLookupModel.VendorCatalogItemId = item.VendorCatalogItemId;
                partLookupModel.IssueOrder = item.UOMConversion;
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                partLookupModel.IssueUnit = item.IssueUnit;
                //V2-690
                partLookupModel.PartCategoryMasterId = item.PartCategoryMasterId;
                partLookupModel.VendorClientlookupId = item.VendorClientlookupId;
                partLookupModel.indexid = item.IndexId;
                partLookupModel.StoreroomId = item.StoreroomId;
                partLookupModel.PartStoreroomId = item.PartStoreroomId;
                partLookupModel.AccountId= item.AccountId; //V2- 1068
                partLookupModel.UnitOfMeasure = item.IssueUOM; //V2- 1068
                partLookupModels.Add(partLookupModel);
                //index++;
            }
            return partLookupModels;
        }


        internal Dictionary<long, string> InsertPartLookUpWorkOrderMultiStoreroom(List<PartAddToCartProcessModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            EstimatedCosts estimatedCosts;
            //V2-726 Start
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(_userData.DatabaseKey).FirstOrDefault();
            //V2-726 End
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        estimatedCosts = new EstimatedCosts();
                        estimatedCosts.ClientId = _dbKey.Client.ClientId;
                        estimatedCosts.ObjectId = item.WorkOrderID;
                        estimatedCosts.ObjectType = "WorkOrder";
                        estimatedCosts.Category = "Parts";
                        estimatedCosts.CategoryId = int.Parse(item.PartId);
                        estimatedCosts.Description = item.Description;
                        estimatedCosts.Quantity = item.OrderQuantity;
                        estimatedCosts.UnitCost = item.UnitCost;
                        estimatedCosts.VendorId = item.VendorId;
                        estimatedCosts.Source = "Internal";
                        estimatedCosts.UpdateIndex = 0;
                        estimatedCosts.UNSPSC = item.PartCategoryMasterId;

                        //V2-726 Start
                        if (approvalGroupSettingsList != null)
                        {
                            if (approvalGroupSettingsList.MaterialRequests == true)
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Initiated;
                            }
                            else
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Approved;
                            }
                        }
                        else
                        {
                            estimatedCosts.Status ="";
                        }
                        //V2-726 End
                        estimatedCosts.StoreroomId = item.StoreroomId;
                        estimatedCosts.PartStoreroomId = item.PartStoreroomId;
                        //V2-1068
                        estimatedCosts.AccountId = item.AccountId;
                        estimatedCosts.UnitOfMeasure = item.UnitOfMeasureIssue;
                        estimatedCosts.CreateFromShoppingCartMultiStoreroom(_dbKey);
                    }
                }

            }
            return retValue;

        }
        internal Dictionary<long, string> InsertPartLookUpMaterialRequestMultiStoreroom(List<PartAddToCartProcessModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            EstimatedCosts estimatedCosts;
            //V2-726 Start
            ApprovalGroupSettings approvalGroupSettings = new ApprovalGroupSettings
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId
            };
            var approvalGroupSettingsList = approvalGroupSettings.RetrieveApprovalGroupSettings_V2(_userData.DatabaseKey).FirstOrDefault();
            //V2-726 End
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        estimatedCosts = new EstimatedCosts();


                        estimatedCosts.ClientId = _dbKey.Client.ClientId;
                        estimatedCosts.ObjectId = item.MaterialRequestId;
                        estimatedCosts.ObjectType = "MR";
                        estimatedCosts.Category = "Parts";
                        estimatedCosts.CategoryId = int.Parse(item.PartId);
                        estimatedCosts.Description = item.Description;
                        estimatedCosts.Quantity = item.OrderQuantity;
                        estimatedCosts.UnitCost = item.UnitCost;
                        estimatedCosts.VendorId = item.VendorId;
                        estimatedCosts.Source = "Internal";
                        estimatedCosts.UpdateIndex = 0;
                        estimatedCosts.UNSPSC = item.PartCategoryMasterId;
                        //V2-726 Start
                        if (approvalGroupSettingsList != null)
                        {
                            if (approvalGroupSettingsList.MaterialRequests == true)
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Initiated;
                            }
                            else
                            {
                                estimatedCosts.Status =MaterialRequestLineStatus.Approved;
                            }
                        }
                        else
                        {
                            estimatedCosts.Status ="";
                        }
                        //V2-726 End
                        estimatedCosts.StoreroomId = item.StoreroomId;
                        estimatedCosts.PartStoreroomId = item.PartStoreroomId;
                        estimatedCosts.CreateFromShoppingCartMultiStoreroom(_dbKey);
                    }
                }

            }
            return retValue;

        }
        #endregion
        #region V2-738
        public List<PartLookupModel> SearchForCartMultiStoreroom(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "", long VendorId = 0, long StoreroomId = 0)
        {

            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            part.VendorId = VendorId;
            part.StoreroomId = StoreroomId;
            parts = part.SearchForCartMultiStoreroom(_dbKey);

            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                partLookupModel.indexid = item.IndexId;
                partLookupModel.PartId = item.PartId;
                partLookupModel.ClientLookupId = item.ClientLookupId;
                partLookupModel.Description = item.Description;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.AppliedCost;
                partLookupModel.Unit = item.IssueUnit;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL = item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    SASImageURL = commonWrapper.GetNoImageUrl();
                }
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.ImageUrl = SASImageURL;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.IssueOrder = item.IssueOrder;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                //V2-932
                partLookupModel.OnOrderQty = item.OnOrderQty;
                partLookupModel.OnRequestQTY = item.OnRequestQTY;
                partLookupModels.Add(partLookupModel);
            }
            return partLookupModels;
        }
        public List<PartLookupModel> SearchForCart_VendorCatalogForMultiStoreroom(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string searchString = "", long VendorId = 0, long StoreroomId = 0)
        {
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            PartLookupModel partLookupModel;
            List<PartLookupModel> partLookupModels = new List<PartLookupModel>();
            List<Part> parts = new List<Part>();

            Part part = new Part();
            part.ClientId = _dbKey.Client.ClientId;
            part.SiteId = _dbKey.User.DefaultSiteId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.SearchText = searchString;
            part.VendorId = VendorId; //V2-553
            part.StoreroomId = StoreroomId;
            parts = part.SearchForCart_VendorCatalogMultiStoreroom(_dbKey);
            long index = 0;
            foreach (var item in parts)
            {
                partLookupModel = new PartLookupModel();
                if (item.CatalogType == "Catalog")
                {
                    partLookupModel.InVendorCatalog = true;
                }
                partLookupModel.indexid = item.IndexId;
                partLookupModel.PartId = item.PartId;
                partLookupModel.indexid = index;
                partLookupModel.ClientLookupId = item.PMClientlookupId;
                partLookupModel.Description = item.PMDescription;
                partLookupModel.Manufacturer = item.Manufacturer;
                partLookupModel.ManufacturerId = item.ManufacturerId;
                partLookupModel.Price = item.UnitCost;
                partLookupModel.Unit = item.PurchaseUOM;
                // RKL - Need to get the SAS URL
                string SASImageURL = string.Empty;
                if (!string.IsNullOrEmpty(item.AttachmentUrl))
                {
                    SASImageURL = item.AttachmentUrl;
                    SASImageURL = commonWrapper.GetSasAttachmentUrl(ref SASImageURL);
                }
                else
                {
                    commonWrapper.GetNoImageUrl();
                }
                partLookupModel.ImageUrl = SASImageURL;
                //partLookupModel.ImageUrl = !string.IsNullOrEmpty(item.AttachmentUrl) ? item.AttachmentUrl : commonWrapper.GetNoImageUrl();
                partLookupModel.VendorPartNumber = item.VendorPartNumber;
                partLookupModel.TotalCount = item.TotalCount;
                //V2-424
                partLookupModel.QtyOnHand = item.QtyOnHand;
                partLookupModel.QtyMaximum = item.QtyMaximum;
                partLookupModel.QtyReorderLevel = item.QtyReorderLevel;
                //V2-424

                //V2-553
                partLookupModel.IssueUOM = item.IssueUOM;
                partLookupModel.UOMConversion = item.UOMConversion;
                partLookupModel.UOMConvRequired = item.UOMConvRequired;
                partLookupModel.VC_Count = item.VC_Count;
                partLookupModel.VendorId = item.VendorId;
                partLookupModel.VendorCatalogItemId = item.VendorCatalogItemId;
                partLookupModel.IssueOrder = item.UOMConversion;
                partLookupModel.PurchaseUOM = item.PurchaseUOM;
                partLookupModel.RequiredDate = DateTime.UtcNow.AddDays(7);
                partLookupModel.IssueUnit = item.IssueUnit;
                partLookupModel.PartCategoryMasterId = item.PartCategoryMasterId;
                //V2-932
                partLookupModel.OnOrderQty = item.OnOrderQty;
                partLookupModel.OnRequestQTY = item.OnRequestQTY;
                partLookupModels.Add(partLookupModel);
                index++;
            }
            return partLookupModels;
        }


        #endregion

        #region V2-894
        public List<LineItemOnOrderCheckModel> getLineitemLookupList(int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", long PartId = 0)
        {
            List<LineItemOnOrderCheckModel> result = new List<LineItemOnOrderCheckModel>();
            PurchaseRequestLineItem pr = new PurchaseRequestLineItem();
            pr.ClientId = _dbKey.Client.ClientId;
            pr.PartId = PartId;
            pr.OrderbyColumn = orderbycol;
            pr.OrderBy = orderDir;
            pr.OffSetVal = skip;
            pr.NextRow = length;
            var PurchaseRequestLineItemlist = pr.PurchaseRequestLineItemRetrievelookuplistByPartIdV2(_dbKey);
            foreach (var p in PurchaseRequestLineItemlist)
            {
                var dModel = new LineItemOnOrderCheckModel();
                dModel.ClientLookupId = p.ClientLookupId;
                dModel.OrderQuantity = p.OrderQuantity;
                dModel.Name = p.Name;
                dModel.LineNumber = p.LineNumber;
                dModel.CreateDate = p.CreateDate;
                dModel.UnitofMeasure = p.UnitofMeasure;
                dModel.TotalCount = p.TotalCount;
                result.Add(dModel);
            }
            return result;
        }
        #endregion

        #region V2-1151 PM Material Request Support

        internal Dictionary<long, string> InsertPMMaterialRequest(List<PartAddToCartProcessModel> list)
        {
            Dictionary<long, string> retValue = new Dictionary<long, string>();
            EstimatedCosts estimatedCosts;

            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (item.OrderQuantity > 0)
                    {
                        estimatedCosts = new EstimatedCosts();

                        estimatedCosts.ClientId = _dbKey.Client.ClientId;
                        estimatedCosts.ObjectId = item.PreventiveMaintainId;
                        estimatedCosts.ObjectType = SearchCategoryConstants.TBL_PREVMAINTMASTER;
                        estimatedCosts.Category = GlobalConstants.Parts;
                        estimatedCosts.CategoryId = int.Parse(item.PartId);
                        estimatedCosts.Description = item.Description;
                        estimatedCosts.Quantity = item.OrderQuantity;
                        estimatedCosts.UnitCost = item.UnitCost;
                        estimatedCosts.Source = SourceTypeConstant.Internal;
                        if (_userData.Site.ShoppingCart)
                        {
                            estimatedCosts.VendorId = item.VendorId;
                            estimatedCosts.UNSPSC = item.PartCategoryMasterId;
                        }
                        if (_userData.DatabaseKey.Client.UseMultiStoreroom)
                        {
                            estimatedCosts.StoreroomId = item.StoreroomId;
                            estimatedCosts.PartStoreroomId = item.PartStoreroomId;
                            estimatedCosts.CreateFromShoppingCartMultiStoreroom(_dbKey);
                        }
                        else
                        {
                            estimatedCosts.CreateFromShoppingCart(_dbKey);
                        }

                    }
                }

            }
            return retValue;

        }
        #endregion
    }
}