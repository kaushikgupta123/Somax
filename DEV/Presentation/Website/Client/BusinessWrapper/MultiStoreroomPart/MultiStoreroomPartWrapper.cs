using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Common.Constants;
using DataContracts;
using Client.Models.MultiStoreroomPart;
using Client.BusinessWrapper.Common;
using Client.Models.Parts;
using Client.Common;
using System.Web.Mvc;
using Client.Models;
using System.Data;
using Client.Localization;
using System.Reflection;
using Client.Models.MultiStoreroomPart.UIConfiguration;

namespace Client.BusinessWrapper.MultiStoreroomPart
{
    public class MultiStoreroomPartWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        List<string> errorMessage = new List<string>();
        public MultiStoreroomPartWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region V2-670

        public List<MultiStoreroomPartModel> GetMultiPartStoreroomChunkList(int CustomQueryDisplayId, int skip = 0, int length = 0, string orderbycol = "", string orderDir = "", string PartClientLookUpId = "", string Description = "", string StockType = "", string SearchText = "", List<string> Storerooms = null)
        {
            Part part = new Part();
            MultiStoreroomPartModel MultiStoreroomPartsModel;
            List<MultiStoreroomPartModel> MultiStoreroomPartsModelList = new List<MultiStoreroomPartModel>();
            List<string> StockTypeList = new List<string>();
            CommonWrapper commonWrapper = new CommonWrapper(userData);
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            part.CustomQueryDisplayId = CustomQueryDisplayId;
            part.OrderbyColumn = orderbycol;
            part.OrderBy = orderDir;
            part.OffSetVal = skip;
            part.NextRow = length;
            part.ClientLookupId = PartClientLookUpId;
            part.Description = Description;
            part.StockType = StockType;
            part.SearchText = SearchText;
            part.Storerooms = string.Join(",", Storerooms ?? new List<string>());
            part.PartChunkSearchForMultiPartStoreroomV2(this.userData.DatabaseKey, userData.Site.TimeZone);
            foreach (var p in part.listOfPart)
            {
                MultiStoreroomPartsModel = new MultiStoreroomPartModel();
                MultiStoreroomPartsModel.PartId = p.PartId;
                MultiStoreroomPartsModel.ClientLookupId = p.ClientLookupId;
                MultiStoreroomPartsModel.Description = p.Description;
                MultiStoreroomPartsModel.Manufacturer = p.Manufacturer;
                MultiStoreroomPartsModel.ManufacturerID = p.ManufacturerId;
                MultiStoreroomPartsModel.StockType = p.StockType;
                MultiStoreroomPartsModel.AppliedCost = p.AppliedCost;
                MultiStoreroomPartsModel.ChildCount = p.ChildCount;
                MultiStoreroomPartsModel.TotalCount = p.TotalCount;
                MultiStoreroomPartsModel.DefStoreroom = p.DefStoreroom;
                MultiStoreroomPartsModelList.Add(MultiStoreroomPartsModel);
            }
            return MultiStoreroomPartsModelList;
        }

        public List<MultiStoreroomPartChildModel> PopulateChilditems(long PartId)
        {
            MultiStoreroomPartChildModel objChildItem;
            List<MultiStoreroomPartChildModel> ChildItemList = new List<MultiStoreroomPartChildModel>();

            PartStoreroom PartStoreroom = new PartStoreroom();
            List<PartStoreroom> PartStoreroomList = new List<PartStoreroom>();
            PartStoreroom.PartId = PartId;
            PartStoreroom.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            PartStoreroom.ClientId = userData.DatabaseKey.Client.ClientId;
            PartStoreroom.PartStoreroomSearchForChildGridV2(this.userData.DatabaseKey, userData.Site.TimeZone);

            //if (PartStoreroomList != null)
            //{
            foreach (var item in PartStoreroom.listOfPartStoreroom)
            {
                objChildItem = new MultiStoreroomPartChildModel();
                objChildItem.PartClientLookupId = item.PartIdClientLookupId;
                objChildItem.PartStoreroomId = item.PartStoreroomId;
                objChildItem.QtyOnHand = item.QtyOnHand;
                objChildItem.QtyMaximum = Math.Round(item.QtyMaximum, 2);
                objChildItem.QtyReorderLevel = item.QtyReorderLevel;
                objChildItem.Location1_1 = item.Location1_1;
                objChildItem.Location1_2 = item.Location1_2;
                objChildItem.Location1_3 = item.Location1_3;
                objChildItem.Location1_4 = item.Location1_4;
                objChildItem.Location1_5 = item.Location1_5;
                objChildItem.CountFrequency = item.CountFrequency;
                objChildItem.LastCounted = item.LastCounted;
                objChildItem.AutoTransfer = item.AutoTransfer;
                objChildItem.StoreroomName = item.StoreroomName;
                objChildItem.StoreroomId = item.StoreroomId;
                objChildItem.Maintain = item.Maintain;
                objChildItem.Issue = item.Issue; //V2-755
                objChildItem.PhysicalInventory = item.PhysicalInventory; //V2-755
                objChildItem.StoreroomNameWithDescription = item.StoreroomNameWithDescription; //V2-1025
                objChildItem.TotalOnRequest = item.TotalOnRequest;
                objChildItem.TotalOnOrder = item.TotalOnOrder;
                ChildItemList.Add(objChildItem);
            }
            //}

            return ChildItemList;
        }
        #endregion

        #region MultiStoreroomPartDetails
        public MultiStoreroomPartModel PopulateMultiStoreroomPartDetails(long PartId)
        {
            MultiStoreroomPartModel objPart = new MultiStoreroomPartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            obj.MultiStoreroomRetriveByPartId(userData.DatabaseKey);
            objPart = initializeControls(obj);

            return objPart;
        }
        public MultiStoreroomPartModel initializeControls(Part obj)
        {
            MultiStoreroomPartModel objPart = new MultiStoreroomPartModel();

            objPart.ClientLookupId = obj.ClientLookupId;
            objPart.PartId = obj.PartId;
            objPart.AccountId = obj?.AccountId ?? 0;
            objPart.AccountClientLookupId = obj?.AccounntClientLookupId ?? string.Empty;
            objPart.AppliedCost = obj?.AppliedCost ?? 0;
            objPart.LastPurchaseCost = obj?.LastPurchaseCost ?? 0;
            objPart.AverageCost = obj?.AverageCost ?? 0;
            objPart.Description = obj?.Description ?? string.Empty;
            objPart.InactiveFlag = obj?.InactiveFlag ?? false;
            objPart.CriticalFlag = obj?.Critical ?? false;
            objPart.IssueUnit = obj?.IssueUnit ?? string.Empty;
            objPart.IssueUnitDescription = obj?.IssueUnitDescription ?? string.Empty;

            objPart.Manufacturer = obj?.Manufacturer ?? string.Empty;
            objPart.ManufacturerID = obj?.ManufacturerId ?? string.Empty;
            objPart.StockType = obj?.StockType ?? string.Empty;
            objPart.StockTypeDescription = obj?.StockTypeDescription ?? string.Empty;
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
            objPart.MSDSContainerCode = obj.MSDSContainerCode;
            objPart.MSDSPressureCode = obj.MSDSPressureCode;
            objPart.MSDSReference = obj.MSDSReference;
            objPart.MSDSRequired = obj.MSDSRequired;
            objPart.MSDSTemperatureCode = obj.MSDSTemperatureCode;
            objPart.NoEquipXref = obj.NoEquipXref;
            objPart.ABCStoreCost = obj.ABCStoreCost;
            objPart.TotalOnHand = obj.TotalOnHand;
            objPart.TotalOnRequest = obj.TotalOnRequest;
            objPart.TotalOnOrder = obj.TotalOnOrder;
            objPart.DefStoreroom = obj.DefStoreroom;
            return objPart;
        }
        #endregion

        #region Actions
        #region Created / Last Updated
        public CreatedLastUpdatedPartForMultiStoreroomPartModel PopulateCreateModifyDate(long partId)
        {
            CreatedLastUpdatedPartForMultiStoreroomPartModel createdLastUpdatedPartModel = new CreatedLastUpdatedPartForMultiStoreroomPartModel();
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
        #region Change Part Id       
        public List<String> ChangePartId(MultiStoreroomPartVM objModel, long PartId)
        {
            List<string> EMsg = new List<string>();
            Part p = new Part();
            string OldClientLookupId = string.Empty;
            p.ClientId = userData.DatabaseKey.Client.ClientId;
            p.SiteId = userData.DatabaseKey.Personnel.SiteId;
            p.PartId = PartId;
            p.Retrieve(userData.DatabaseKey);
            OldClientLookupId = p.ClientLookupId;
            p.ClientLookupId = objModel.ChangePartIdModel.ClientLookupId;
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

        #endregion

        #region Common
        public MultiStoreroomPartModel PopulateDropdownControls(MultiStoreroomPartModel objMSP)
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
                    objMSP.LookupStokeTypeList = objLookupStokeType.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }

                objLookupIssueUnit = AllLookUps.Where(x => x.ListName == LookupListConstants.Unit_Of_Measure).ToList();
                if (objLookupIssueUnit != null)
                {
                    objMSP.LookupIssueUnitList = objLookupIssueUnit.Select(x => new SelectListItem { Text = x.ListValue + "  -  " + x.Description, Value = x.ListValue });
                }
            }

            return objMSP;
        }

        public List<string> CreatePartEvent(long PartId, string Event, string Comment)
        {
            PartEventLog partEventLog = new PartEventLog();
            partEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            partEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            partEventLog.PartId = PartId;
            partEventLog.TransactionDate = DateTime.UtcNow;
            partEventLog.Event = Event;
            partEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            partEventLog.Comments = Comment;
            partEventLog.SourceId = 0;
            partEventLog.Create(userData.DatabaseKey);
            return partEventLog.ErrorMessages;
        }
        #endregion

        #region Part-Add/Edit
        public MultiStoreroomPartModel PopulateMultiStoreroomPartEditDetails(long PartId)
        {
            MultiStoreroomPartModel objPart = new MultiStoreroomPartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            obj.MultiStoreroomRetriveByPartId(userData.DatabaseKey);
            objPart = initializeControls(obj);

            return objPart;
        }
        public Part SaveParts(MultiStoreroomPartModel _mspModel)
        {
            Part objPart = new DataContracts.Part();
            if (_mspModel.PartId != 0)
            {
                objPart = EditParts(_mspModel);
            }
            else
            {
                objPart = AddParts(_mspModel);
            }
            return objPart;
        }
        public Part AddParts(MultiStoreroomPartModel mspModel)
        {
            Part part = new Part();

            part.ClientLookupId = mspModel.ClientLookupId;
            part.SiteId = _dbKey.Personnel.SiteId;
            part.Description = mspModel.Description;
            part.AccountId = mspModel.AccountId ?? 0;
            part.StockType = mspModel.StockType;
            part.IssueUnit = mspModel.IssueUnit;
            part.Manufacturer = mspModel.Manufacturer;
            part.ManufacturerId = mspModel.ManufacturerID;
            part.AverageCost = mspModel.AverageCost ?? 0;
            part.AppliedCost = mspModel.AppliedCost ?? 0;

            // Check if duplicate part 
            //var part1 = part.RetrieveForSearchBySiteId(userData.DatabaseKey).Where(x => x.ClientLookupId == part.ClientLookupId);

            // SOM-1679 - Part Add allows a duplicate part if the existing part is inactive
            // The RetrieveForSearchBySiteId only returned active
            // Replace with RetrieveBySiteIdAndClientLookupId 
            part.ValidateAddMultiStoreroomPart(this.userData.DatabaseKey);
            if (part.ErrorMessages==null||part.ErrorMessages.Count==0)
            {
                part.Create(_dbKey);
            }
            return part;

        }
        public Part EditParts(MultiStoreroomPartModel mspModel)
        {

            Part part = new Part()
            {
                PartId = mspModel.PartId
            };
            part.Retrieve(_dbKey);



            #region Initialize

            part.ClientLookupId = mspModel?.ClientLookupId ?? string.Empty;
            part.AccountId = mspModel?.AccountId ?? 0;
            part.AppliedCost = mspModel?.AppliedCost ?? 0;
            part.AverageCost = mspModel?.AverageCost ?? 0;
            part.Description = mspModel?.Description ?? string.Empty;
            part.Critical = mspModel?.CriticalFlag ?? false;
            part.IssueUnit = mspModel?.IssueUnit ?? string.Empty;
            part.Consignment = mspModel?.Consignment ?? false;
            //// The site must be using the PartMaster and the client must be either 4 or 6
            //if (userData.Site.UsePartMaster && (userData.DatabaseKey.Client.ClientId == 4 || userData.DatabaseKey.Client.ClientId == 6))
            //{
            //    // part.Manufacturer = uicManufactureLT.Text;
            //}
            //else
            //{
            part.Manufacturer = mspModel?.Manufacturer ?? string.Empty;
            //}
            part.ManufacturerId = mspModel?.ManufacturerID ?? string.Empty;
            part.StockType = mspModel?.StockType ?? string.Empty;
            part.UPCCode = mspModel?.UPCCode ?? string.Empty;
            part.MSDSTemperatureCode = mspModel?.MSDSTemperatureCode ?? string.Empty;
            part.MSDSRequired = mspModel?.MSDSRequired ?? false;
            part.MSDSReference = mspModel?.MSDSReference ?? string.Empty;
            part.MSDSContainerCode = mspModel?.MSDSContainerCode ?? string.Empty;
            part.MSDSPressureCode = mspModel?.MSDSPressureCode ?? string.Empty;
            part.NoEquipXref = mspModel?.NoEquipXref ?? false;
            part.UPCCode = mspModel?.UPCCode ?? string.Empty;
            part.ABCCode = mspModel?.ABCCode ?? string.Empty;
            part.ABCStoreCost = mspModel?.ABCStoreCost ?? string.Empty;
            #endregion

            part.Update(userData.DatabaseKey);
            if (part.ErrorMessages != null && part.ErrorMessages.Count > 0)
            {
                return part;

            }
            return part;
        }
        #endregion

        #region Storeroom-Add/Edit 
        public PartStoreroom SaveStoreroom(StoreroomModel _sModel)
        {
            PartStoreroom objPartStoreroom = new PartStoreroom();
            if (_sModel.Id != 0)
            {
                objPartStoreroom = EditStoreroom(_sModel);
            }
            else
            {
                objPartStoreroom = AddStoreroom(_sModel);
            }
            return objPartStoreroom;
        }

        public PartStoreroom AddStoreroom(StoreroomModel mspModel)
        {
            StoreroomModel storeroom = new StoreroomModel();
            PartStoreroom partStoreroom = new PartStoreroom();
            PartHistory partHistory = new PartHistory();

            partStoreroom.ClientId = _dbKey.Client.ClientId;
            partStoreroom.PartId = mspModel.PartId;
            partStoreroom.StoreroomId = mspModel.StoreroomId;
            partStoreroom.Location1_1 = mspModel.Section??string.Empty;
            partStoreroom.Location1_2 = mspModel.Row??string.Empty;
            partStoreroom.Location1_3 = mspModel.Shelf??string.Empty;
            partStoreroom.Location1_4 = mspModel.Bin??string.Empty;
            partStoreroom.QtyOnHand = mspModel.QuantityOnHand??0;
            partStoreroom.QtyMaximum = mspModel.MaximumQuantity??0;
            partStoreroom.QtyReorderLevel =mspModel.MinimumQuantity??0;
            partStoreroom.CountFrequency = mspModel.CountFrequency;
            partStoreroom.LastCounted = mspModel.LastCounted == null ? DateTime.UtcNow : Convert.ToDateTime(mspModel.LastCounted);

            // Check if duplicate Storeroom
            partStoreroom.ValidateAddMultiStoreroomPartStoreroom(this.userData.DatabaseKey);
            if (partStoreroom.ErrorObj==null||partStoreroom.ErrorMessages.Count==0)
            {
                //add partstoreroom record
                partStoreroom.Create(userData.DatabaseKey);

                partHistory.ClientId = _dbKey.Client.ClientId;
                partHistory.PartId = partStoreroom.PartId;
                partHistory.PartStoreroomId = partStoreroom.PartStoreroomId;
                partHistory.AccountId = 0;
                partHistory.AverageCostBefore = 0;
                partHistory.AverageCostAfter = 0;
                partHistory.ChargeType_Primary = "";
                partHistory.ChargeToId_Primary = 0;
                partHistory.Comments = "";
                partHistory.Cost = 0;
                partHistory.CostAfter = 0;
                partHistory.CostBefore = 0;
                partHistory.Description = "";
                partHistory.DepartmentId = 0;
                partHistory.PerformedById = userData.DatabaseKey.Personnel.PersonnelId;
                partHistory.QtyAfter = mspModel.QuantityOnHand ?? 0;
                partHistory.QtyBefore = 0;
                partHistory.RequestorId = userData.DatabaseKey.Personnel.PersonnelId;
                partHistory.StockType = "";
                partHistory.StoreroomId = mspModel.StoreroomId;
                partHistory.TransactionDate = DateTime.UtcNow;
                partHistory.TransactionQuantity = mspModel.QuantityOnHand ?? 0;
                partHistory.TransactionType = PartHistoryTranTypes.StoreroomAdd;
                partHistory.UnitofMeasure = "";
                partHistory.VMRSFailure = "";
                partHistory.CreatedBy = userData.DatabaseKey.UserName;
                partHistory.CreatedDate = DateTime.UtcNow;
                //add parthistory record
                partHistory.Create(userData.DatabaseKey);

            }
            return partStoreroom;

        }

        public PartStoreroom EditStoreroom(StoreroomModel mspModel)
        {
            StoreroomModel storeroomModel = new StoreroomModel();
            PartStoreroom partStoreroom = new PartStoreroom();
            partStoreroom.PartStoreroomId = mspModel.PartStoreroomId;

            partStoreroom.Retrieve(_dbKey);

            partStoreroom.PartId = mspModel.PartId;
            partStoreroom.StoreroomId = mspModel.StoreroomId;
            partStoreroom.Location1_1 = mspModel.Section??string.Empty;
            partStoreroom.Location1_2 = mspModel.Row??string.Empty;
            partStoreroom.Location1_3 = mspModel.Shelf??string.Empty;
            partStoreroom.Location1_4 = mspModel.Bin??string.Empty;
            partStoreroom.Location2_1 = mspModel.Section2 ?? string.Empty;
            partStoreroom.Location2_2 = mspModel.Row2 ?? string.Empty;
            partStoreroom.Location2_3 = mspModel.Shelf2 ?? string.Empty;
            partStoreroom.Location2_4 = mspModel.Bin2 ?? string.Empty;
            partStoreroom.Critical = mspModel.Critical;
            partStoreroom.AutoPurchase = mspModel.AutoPurchase;
            partStoreroom.PartVendorId = mspModel.PartVendorId ?? 0;
            partStoreroom.QtyMaximum = mspModel.MaximumQuantity??0;
            partStoreroom.QtyReorderLevel =mspModel.MinimumQuantity??0;
            partStoreroom.CountFrequency = mspModel.CountFrequency;
            partStoreroom.LastCounted = mspModel.LastCounted == null ? DateTime.UtcNow : Convert.ToDateTime(mspModel.LastCounted);

            //Update partstoreroom record
            partStoreroom.Update(userData.DatabaseKey);

            return partStoreroom;

        }
        #endregion

        #region Vendor
        public Part_Vendor_Xref AddPartVendorXref(MultiStoreroomPartVendorModel _PVXModel, long objectId)//
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
        public Part_Vendor_Xref UpdatePartVendorXref(MultiStoreroomPartVendorModel _PVXModel, long objectId)
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
        public MultiStoreroomPartVendorModel EditVendor(long partId, long _part_Vendor_XrefId, string clientLookupId, int updatedIndex)
        {
            Part_Vendor_Xref pvx = new Part_Vendor_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = partId,
                Part_Vendor_XrefId = _part_Vendor_XrefId
            };
            pvx.Retrieve(userData.DatabaseKey);

            MultiStoreroomPartVendorModel mspVendorXref = new MultiStoreroomPartVendorModel();
            mspVendorXref.PartId = partId;
            mspVendorXref.PartClientLookupId = clientLookupId;
            mspVendorXref.CatalogNumber = pvx.CatalogNumber;
            mspVendorXref.Manufacturer = pvx.Manufacturer;
            mspVendorXref.ManufacturerID = pvx.ManufacturerId;
            mspVendorXref.OrderQuantity = pvx.OrderQuantity;
            mspVendorXref.OrderUnit = pvx.OrderUnit;
            mspVendorXref.Price = pvx.Price;
            mspVendorXref.PreferredVendor = pvx.PreferredVendor;
            mspVendorXref.PartVendorXrefId = pvx.Part_Vendor_XrefId;
            mspVendorXref.IssueOrder = pvx.IssueOrder;
            mspVendorXref.UOMConvRequired = pvx.UOMConvRequired;
            if (pvx.UOMConvRequired)
            {
                mspVendorXref.DefaultissueOrdervalue = 1.000001m;
                mspVendorXref.DefaultPricevalue = 0.00001m;
            }

            Vendor vendor = new Vendor()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                VendorId = pvx.VendorId
            };
            vendor.Retrieve(userData.DatabaseKey);
            mspVendorXref.VendorClientLookupId = vendor.ClientLookupId;

            return mspVendorXref;
        }
        #endregion

        #region Asset
        public List<MultiStoreroomPartEquipmentGridModel> PopulateMSPEquipment(long _partId)
        {
            List<Equipment_Parts_Xref> epx = PopulatePartEquipmentDetail(_partId);
            List<MultiStoreroomPartEquipmentGridModel> PartsEquipmentGridModelList = new List<MultiStoreroomPartEquipmentGridModel>();
            MultiStoreroomPartEquipmentGridModel objPartsEquipmentGridModel;
            foreach (var v in epx)
            {
                objPartsEquipmentGridModel = new MultiStoreroomPartEquipmentGridModel();
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
                PartsEquipmentGridModelList.Add(objPartsEquipmentGridModel);
            }
            return PartsEquipmentGridModelList;
        }
        public MultiStoreroomPartEquipmentXrefModel EditPartEquipment(long partId, long _equipment_Parts_XrefId, string clientLookupId, int updatedIndex)
        {
            Equipment_Parts_Xref eqx = new Equipment_Parts_Xref()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                PartId = partId,
                Equipment_Parts_XrefId = _equipment_Parts_XrefId
            };
            eqx.Retrieve(userData.DatabaseKey);
            MultiStoreroomPartEquipmentXrefModel _equipmentPartXrefModel = new MultiStoreroomPartEquipmentXrefModel();
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
        public Equipment_Parts_Xref AddEquipmentPartsXref(MultiStoreroomPartEquipmentXrefModel equipmentPartXrefModel, long ObjectId)
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
        public Equipment_Parts_Xref UpdateEquipmentPartsXref(MultiStoreroomPartEquipmentXrefModel equipmentPartXrefModel, long ObjectId)
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
        #endregion

        #region History
       


        #region V2-760
        //**V2-760 History list view default sort to be Date(PartHistory.TransactionDate) in descending order**
        public List<MultiStoreroomPartHistoryModel> GetDetailsMSPHistory(long partId = 0, int daterange = 0)
        {
            PartHistoryReview partHistoryAllReview = new PartHistoryReview();
            partHistoryAllReview.ClientId = userData.DatabaseKey.Client.ClientId;
            partHistoryAllReview.PartId = partId;
            partHistoryAllReview.DateRange = Convert.ToString(daterange);
            List<PartHistoryReview> _partHistoryAllReview = PartHistoryReview.RetrievePartHistoryReview_V2(userData.DatabaseKey, partHistoryAllReview, userData.Site.TimeZone); //V2-1180 Converted to local date
            //List<PartHistoryReview> _partHistoryAllReview = PartHistoryReview.RetrievePartHistoryReview(userData.DatabaseKey, partHistoryAllReview);

            List<MultiStoreroomPartHistoryModel> MSPHistoryModelList = new List<MultiStoreroomPartHistoryModel>();
            MultiStoreroomPartHistoryModel mspHistoryModel;

            foreach (var p in _partHistoryAllReview)
            {
                mspHistoryModel = new MultiStoreroomPartHistoryModel();
                mspHistoryModel.PartId = p.PartId;
                mspHistoryModel.TransactionType = p.TransactionType;
                mspHistoryModel.Requestor_Name = p.Requestor_Name;
                mspHistoryModel.PerformBy_Name = p.PerformBy_Name;
                mspHistoryModel.TransactionDate = p.TransactionDate;
                mspHistoryModel.TransactionQuantity = p.TransactionQuantity;
                mspHistoryModel.Cost = p.Cost;
                mspHistoryModel.ChargeType_Primary = p.ChargeType_Primary;
                mspHistoryModel.ChargeTo_ClientLookupId = p.ChargeTo_ClientLookupId;
                mspHistoryModel.Account_ClientLookupId = p.Account_ClientLookupId;
                mspHistoryModel.PurchaseOrder_ClientLookupId = p.PurchaseOrder_ClientLookupId;
                mspHistoryModel.ChargeType_Primary = p.ChargeType_Primary;
                mspHistoryModel.Vendor_ClientLookupId = p.Vendor_ClientLookupId;
                mspHistoryModel.Vendor_Name = p.Vendor_Name;
                mspHistoryModel.Storeroom = p.Storeroom;

                MSPHistoryModelList.Add(mspHistoryModel);
            }

            return MSPHistoryModelList;
        }
        #endregion end V2-760

        #endregion

        #region Part_Receipt
        public List<MultiStoreroomReceiptModel> GetDetailsMSPartReceipt(long partId = 0, int daterange = 0)
        {
            POReceipt pOReceipt = new POReceipt();
            pOReceipt.ClientId = userData.DatabaseKey.User.ClientId;
            pOReceipt.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            pOReceipt.PartId = partId;
            pOReceipt.DateRange = Convert.ToString(daterange);
            List<POReceipt> listReceipt = new List<POReceipt>();
            listReceipt = pOReceipt.RetrievePOFromParts(userData.DatabaseKey, userData.Site.TimeZone);
            List<MultiStoreroomReceiptModel> MSPartReceiptModelList = new List<MultiStoreroomReceiptModel>();
            MultiStoreroomReceiptModel mspartReceiptModel;

            foreach (var p in listReceipt)
            {
                mspartReceiptModel = new MultiStoreroomReceiptModel();
                mspartReceiptModel.POClientLookupId = p.POClientLookupId;

                if (p.OrderDate != null && p.OrderDate == default(DateTime))
                {
                    mspartReceiptModel.ReceivedDate = null;
                }
                else
                {
                    mspartReceiptModel.ReceivedDate = p.OrderDate;
                }
                mspartReceiptModel.VendorClientLookupId = p.VendorClientLookupId;
                mspartReceiptModel.VendorName = p.VendorName;
                mspartReceiptModel.OrderQuantity = p.OrderQuantity;
                mspartReceiptModel.UnitCost = p.UnitCost;

                MSPartReceiptModelList.Add(mspartReceiptModel);
            }

            return MSPartReceiptModelList;
        }
        #endregion

        #region V2-755
        #region Part Checkout
        internal List<PartHistory> SavePartCheckOut(Client.Models.MultiStoreroomPart.ParInvCheckoutModel data)
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
                tmpItem.MultiStoreroom = true;
                tmpItem.StoreroomId = data.StoreroomId;
                tmpList.Add(tmpItem);
                PartHistory parthistory = new PartHistory() { PartHistoryList = tmpList };
                PartHistoryListTemp = parthistory.CreateByForeignKeysnew(userData.DatabaseKey);
            }
            return PartHistoryListTemp;
        }
        #endregion
        #region Adjust On Hand Quantity
        public PartHistory SaveHandsOnQty(MultiStoreroomPartGridPhysicalInvList model)
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
                SiteId = userData.Site.SiteId,
                PartClientLookupId = model?.PartClientLookupId ?? string.Empty,
                Description = model?.Description ?? string.Empty,
                PartUPCCode = model?.PartUPCCode ?? string.Empty,
                PartStoreroomQtyOnHand = QtyOnHand,
                PartQtyCounted = model?.QuantityCount ?? 0,
                PartStoreroomUpdateIndex = UpdateIndex,
                PerformedById = userData.DatabaseKey.Personnel.PersonnelId,
                StoreroomId = model?.StoreroomId ?? 0
            };
            lstPartHistory.Add(tmpModel);
            PartHistory parthistory = new PartHistory() { PartHistoryList = lstPartHistory };
            parthistory.PhysicalInventory_V2(userData.DatabaseKey);
            return parthistory;
        }
        #endregion
        #endregion
        #region V2-751
        public IEnumerable<SelectListItem> GetIssuingStoreroomList(long Partid ,long RequestStoreroomid)
        {
            PartStoreroom partstoreroom = new PartStoreroom();
            partstoreroom.ClientId = userData.DatabaseKey.Client.ClientId;
            partstoreroom.PartId = Partid;
            partstoreroom.StoreroomId = RequestStoreroomid;
            var storeroomList = partstoreroom.RetrieveIssuingStoreroomListForPartTransferRequest(userData.DatabaseKey);
            return storeroomList.Select(x => new SelectListItem { Text = x.StoreroomName, Value = Convert.ToString(x.PartStoreroomId)+ "#"+  Convert.ToString(x.StoreroomId) });
        }

        public StoreroomTransfer savePartTransferRequest(AddPartTransferRequest addPartTransfer)
        {
            var PartStoreroomIdAndStoreroomId = addPartTransfer.IssuePartStoreroomIdAndStoreroomId.Split('#');
            StoreroomTransfer storeroomTransfer = new StoreroomTransfer();
            storeroomTransfer.SiteId= userData.DatabaseKey.Personnel.SiteId;
            storeroomTransfer.ClientId= userData.DatabaseKey.Client.ClientId;
            storeroomTransfer.RequestPartId = addPartTransfer.PartId;
            storeroomTransfer.RequestPTStoreroomID = addPartTransfer.RequestPartStoreroomId;
            storeroomTransfer.RequestStoreroomId = addPartTransfer.RequestStoreroomId;
            storeroomTransfer.RequestQuantity = addPartTransfer.RequestQuantity ?? 0;
            storeroomTransfer.Reason = addPartTransfer.Reason ?? string.Empty;
            storeroomTransfer.IssuePartId = addPartTransfer.PartId;
            storeroomTransfer.CreatedBy = userData.DatabaseKey.UserName;
            storeroomTransfer.IssuePTStoreroomID = Convert.ToInt64(PartStoreroomIdAndStoreroomId[0]);
            storeroomTransfer.IssueStoreroomId = Convert.ToInt64(PartStoreroomIdAndStoreroomId[1]);
            storeroomTransfer.Creator_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            storeroomTransfer.Status = PartTransferStatusConstants.Open;

            storeroomTransfer.Create(userData.DatabaseKey);
           
            return storeroomTransfer;
        }
        #endregion

        #region V2-1025
        public List<Storeroom> GetStoreroomListByClientIdSiteId()
        {
            LookUpListModel model = new LookUpListModel();
            Storeroom storeroom = new Storeroom()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                SiteId = this.userData.DatabaseKey.User.DefaultSiteId
            };

            List<Storeroom> StoreroomList = storeroom.RetrieveAllStoreroomForLookupList(this.userData.DatabaseKey);
            return StoreroomList;
        }
        public StoreroomInnerChildModel PopulateStoreroomInnerGridViewData(long PartStoreroomId)
        {

            PartStoreroom partStoreroom = new PartStoreroom();
            partStoreroom.PartStoreroomId = PartStoreroomId;
            partStoreroom.ClientId = userData.DatabaseKey.Client.ClientId;
            partStoreroom = partStoreroom.RetriveforStoreroomGridChildDetailsViewByPartStoreroomIdV2(this.userData.DatabaseKey, userData.Site.TimeZone);
            StoreroomInnerChildModel objChildItem = new StoreroomInnerChildModel();
            objChildItem.PartStoreroomId = partStoreroom.PartStoreroomId;
            objChildItem.Location1_1 = partStoreroom.Location1_1;
            objChildItem.Location1_2 = partStoreroom.Location1_2;
            objChildItem.Location1_3 = partStoreroom.Location1_3;
            objChildItem.Location1_4 = partStoreroom.Location1_4;
            objChildItem.Location1_5 = partStoreroom.Location1_5;
            objChildItem.Location2_1 = partStoreroom.Location2_1;
            objChildItem.Location2_2 = partStoreroom.Location2_2;
            objChildItem.Location2_3 = partStoreroom.Location2_3;
            objChildItem.Location2_4 = partStoreroom.Location2_4;
            objChildItem.Location2_5 = partStoreroom.Location2_5;
            objChildItem.CountFrequency = partStoreroom.CountFrequency;
            objChildItem.LastCounted = partStoreroom.LastCounted;
            objChildItem.StoreroomName = partStoreroom.StoreroomName;
            objChildItem.StoreroomId = partStoreroom.StoreroomId;
            objChildItem.AutoPurchase = partStoreroom.AutoPurchase;
            objChildItem.Critical = partStoreroom.Critical;
            objChildItem.QuantityOnHand = partStoreroom.QtyOnHand;
            objChildItem.MaximumQuantity = partStoreroom.QtyMaximum;
            objChildItem.MinimumQuantity = partStoreroom.QtyReorderLevel;
            objChildItem.VendorName = partStoreroom.VendorName;
            objChildItem.PartVendorId = partStoreroom.PartVendorId;
            objChildItem.VendorClientLookupId = partStoreroom.VendorClientLookupId;
            return objChildItem;
        }
        #endregion

        #region V2-1045
        public PartModel RetrievePartIdByClientLookUpForFindPart(string ClientLookupId = "")
        {
            Part part = new Part();
            part.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            part.ClientId = userData.DatabaseKey.Client.ClientId;
            part.ClientLookupId = ClientLookupId;
            var partResult = part.RetrievePartIdByClientLookupIdForFindPart(userData.DatabaseKey);

            var partModel = new PartModel()
            {
                PartId = partResult.PartId,

            };
            return partModel;
        }
        #endregion

        #region V2-1059
        public PartStoreroom AddtoAutoTransferUpdate(AddToAutoTransfer addToAutoTransfer)
        {
            List<string> EMsg = new List<string>();

            PartStoreroom partStoreroom = new PartStoreroom()
            {
                ClientId = _dbKey.Client.ClientId,
                PartStoreroomId = addToAutoTransfer.PartStoreroomId,
            };

            partStoreroom.PartId = addToAutoTransfer.PartId;
            partStoreroom.StoreroomId = addToAutoTransfer.StoreroomId;
            partStoreroom.AutoTransferIssueStoreroom = addToAutoTransfer.AutoTransferIssueStoreroom;

            //Check if duplicate Storeroom
            partStoreroom.ValidateAddMultiStoreroomPartStoreroomSameAutoTransferIssueStoreroomId(this.userData.DatabaseKey);

            if (partStoreroom.ErrorObj == null || partStoreroom.ErrorMessages.Count == 0)
            {
                partStoreroom.Retrieve(_dbKey);
                partStoreroom.AutoTransfer = 1;
                partStoreroom.AutoTransferIssueStoreroom = addToAutoTransfer.AutoTransferIssueStoreroom;
                partStoreroom.AutoTransferMaxQty = addToAutoTransfer.AutoTransferMaxQty ?? 0;
                partStoreroom.AutoTransferMinQty = addToAutoTransfer.AutoTransferMinQty ?? 0;
                partStoreroom.Update(userData.DatabaseKey);
                string Event = "AddAutoTransfer";
                string Comments = "";
                CreatePartStoreroomEventLog(addToAutoTransfer, Event, Comments);
            }
            else
            {
                EMsg = partStoreroom.ErrorMessages;
            }
            return partStoreroom;
        }

        public PartStoreroom RemovefromAutoTransferUpdate(AddToAutoTransfer addToAutoTransfer)
        {
            List<string> EMsg = new List<string>();

            PartStoreroom partStoreroom = new PartStoreroom()
            {
                ClientId = _dbKey.Client.ClientId,
                PartStoreroomId = addToAutoTransfer.PartStoreroomId,
            };
            if (partStoreroom.ErrorObj == null || partStoreroom.ErrorMessages.Count == 0)
            {
                partStoreroom.Retrieve(_dbKey);
                partStoreroom.AutoTransfer = 0;
                partStoreroom.AutoTransferIssueStoreroom = 0;
                partStoreroom.AutoTransferMaxQty =  0;
                partStoreroom.AutoTransferMinQty = 0;
                partStoreroom.Update(userData.DatabaseKey);
                string Event = "RemAutoTransfer";
                string Comments = "Remove Part from Auto Transfer";
                CreatePartStoreroomEventLog(addToAutoTransfer, Event, Comments);
            }
            else
            {
                EMsg = partStoreroom.ErrorMessages;
            }
            return partStoreroom;
        }

        public List<string> CreatePartStoreroomEventLog(AddToAutoTransfer addToAutoTransfer,  string Event, string Comment)
        {
            PartEventLog partEventLog = new PartEventLog();
            partEventLog.ClientId = userData.DatabaseKey.User.ClientId;
            partEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            partEventLog.PartId = addToAutoTransfer.PartId;
            partEventLog.StoreroomId = addToAutoTransfer.StoreroomId;
            partEventLog.TransactionDate = DateTime.UtcNow;
            partEventLog.Event = Event;
            partEventLog.PersonnelId = userData.DatabaseKey.Personnel.PersonnelId;
            partEventLog.Comments = Comment;
            partEventLog.SourceTable = "PartStoreroom";
            partEventLog.SourceId = addToAutoTransfer.PartStoreroomId;
            partEventLog.Create(userData.DatabaseKey);
            return partEventLog.ErrorMessages;
        }
        #endregion
        #region V2-1070
        public PartStoreroom ValidatePartStatusChange(long partId, string flag, string clientLookupId)
        {
            PartStoreroom partStoreroom = new PartStoreroom()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartId = partId,
                Flag = flag,
                ClientLookupId = clientLookupId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId
            };
            partStoreroom.CheckPartIsInactivateorActivateForMultiStoreroom(userData.DatabaseKey);
            return partStoreroom;
        }

        public List<string> MakeActiveInactive(bool InActiveFlag, long PartId)
        {
            Part part = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            part.Retrieve(userData.DatabaseKey);
            part.InactiveFlag = !InActiveFlag;
            part.Update(userData.DatabaseKey);
            if (part.ErrorMessages == null)
            {
                var createEventStatus = CreatePartEvent(PartId, InActiveFlag);
            }
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
        #endregion
        #region V2-1187 Add Part Dynamic Ui Configuration
        public Part AddPartsDynamic(MultiStoreroomPartVM _PartModel)
        {
            Part objPart = new DataContracts.Part();
            objPart = AddPartsDyn(_PartModel);
            return objPart;
        }
        public Part AddPartsDyn(MultiStoreroomPartVM partModel)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            Part part = new Part();
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

                //DyanamicDataBind(ref equipment, ref equipmentUDF);

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

        #region V2-1187 Edit Part Dynamic Ui Configuration
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
            part.MultiStoreroomRetriveByPartId(_dbKey);
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

        public Part EditPartDynamic(MultiStoreroomPartVM objPartVM)
        {
            PropertyInfo getpropertyInfo, setpropertyInfo;
            string emptyValue = string.Empty;
            List<string> ErrorList = new List<string>();
            Part part = new Part()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                PartId = Convert.ToInt64(objPartVM.EditPart.PartId)
            };
            part.Retrieve(userData.DatabaseKey);

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

            part.Update(userData.DatabaseKey);

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

        #region V2-1187 PartDetail Dynamic
        public PartModel PopulatePartDetails_V2(long PartId)
        {
            PartModel objPart = new PartModel();
            Part obj = new Part()
            {
                ClientId = _dbKey.Client.ClientId,
                PartId = PartId
            };
            obj.Retrieve(userData.DatabaseKey);
            objPart = PopulatePartDetailsinitializeControls(obj);
            objPart.PartMaster_ClientLookupId = obj.PartMaster_ClientLookupId;
            objPart.LongDescription = obj.LongDescription;
            objPart.ShortDescription = obj.ShortDescription;
            objPart.Category = obj.Category;
            objPart.CategoryDesc = obj.CategoryDesc;

            return objPart;
        }
        public PartModel PopulatePartDetailsinitializeControls(Part obj)
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
           
            objPart.MSDSContainerCode = obj.MSDSContainerCode;
            objPart.MSDSPressureCode = obj.MSDSPressureCode;
            objPart.MSDSReference = obj.MSDSReference;
            objPart.MSDSRequired = obj.MSDSRequired;
            objPart.MSDSTemperatureCode = obj.MSDSTemperatureCode;
            objPart.NoEquipXref = obj.NoEquipXref;

            return objPart;
        }
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

        #region V2-1203 Part Model
        public AddPartModelDynamic RetrievePartDetailsByPartIdForMSPartModel(long PartId)
        {
            AddPartModelDynamic addPartModelDynamic = new AddPartModelDynamic();
            Part part = RetrievePartByPartId(PartId);
            PartUDF partUDF = RetrievePartUDFByPartId(PartId);

            addPartModelDynamic = MapPartUDFDataForMSPartModel(addPartModelDynamic, partUDF);
            addPartModelDynamic = MapPartDataForMSPartModel(addPartModelDynamic, part);
            return addPartModelDynamic;
        }

        private AddPartModelDynamic MapPartUDFDataForMSPartModel(AddPartModelDynamic addPartModelDynamic, PartUDF partUDF)
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
        public AddPartModelDynamic MapPartDataForMSPartModel(AddPartModelDynamic addPartModelDynamic, Part part)
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

        public Part AddPartsModel(MultiStoreroomPartVM _PartModel)
        {
            Part objPart = new Part();
            objPart = AddPartModelPartsDynamic(_PartModel);
            return objPart;
        }
        public Part AddPartModelPartsDynamic(MultiStoreroomPartVM partModel)
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
                if (part.ErrorMessages == null || part.ErrorMessages.Count == 0)
                {
                    //Add PartUDFDynamic
                    if (configDetails.Any(x => x.Display && x.UDF  && x.Section == false))
                    {
                        IEnumerable<string> errorsPartUDF = AddPartUDFDynamic(partModel.AddPart, part.PartId, configDetails);
                        if (errorsPartUDF != null && errorsPartUDF.Count() > 0)
                        {
                            part.ErrorMessages.AddRange(errorsPartUDF);
                        }
                    }

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
                    ////Add Notes
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
                                notes.ObjectId = part.PartId;
                                notes.TableName = AttachmentTableConstant.Part;
                                notes.Create(_dbKey);

                            }

                        }
                    }
                    //Add PartEventLog 
                    PartEventLog partEventLog = new PartEventLog();
                    partEventLog.ClientId = userData.DatabaseKey.User.ClientId;
                    partEventLog.SiteId = userData.DatabaseKey.User.DefaultSiteId;
                    partEventLog.PartId = part.PartId;
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
    }
}