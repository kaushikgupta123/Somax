/*
/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* Part.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
*============ ======== ================== ==========================================================
* 2014-Dec-20 SOM-451  Roger Lawton       Added Account_ClientLookupId, AltPart1-3, LastPurchaseCost
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Database;
using Database.Business;
using Newtonsoft.Json;
using Common.Structures;
using static Database.Part_ValidateByStockTypeIssueUnit;

namespace DataContracts
{
    [JsonObject]
    public partial class Part : DataContractBase, IStoredProcedureValidation
    {
        #region properties
        // Storeroom Table Information

        public decimal AverageCostBefore { get; set; }
        public decimal AverageCostAfter { get; set; }
        public decimal CostAfter { get; set; }
        public decimal CostBefore { get; set; }
        public Int64 PerformById { get; set; }
        public string TransactionType { get; set; }
        public string ProcessMode { get; set; }
        public Int64 PersonnelId { get; set; }
        public Int64 PartStoreroomId { get; set; }
        public int CountFrequency { get; set; }
        public DateTime? LastCounted { get; set; }
        public string Location { get; set; }

        public string Location1_1 { get; set; }
        public string Location1_5 { get; set; }
        public string Location1_2 { get; set; }
        public string Location1_3 { get; set; }
        public string Location1_4 { get; set; }
        public decimal QtyMaximum { get; set; }
        public decimal QtyOnHand { get; set; }
        public decimal QtyReorderLevel { get; set; }
        public string ReorderMethod { get; set; }
        public int Storeroom_UpdateIndex { get; set; }
        // Calculated Quantities
        public decimal QtyOnOrder { get; set; }
        public decimal QtyOnRequest { get; set; }
        public decimal QtyReserved { get; set; }
        //----------updated on 21 July
        // public string ManufacturerId { get; set; }
        public string Name { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }

        string ValidateFor = string.Empty;
        public decimal QtyAvailtoIssue
        {
            get { return this.QtyOnHand - QtyReserved; }
        }
        public decimal QtyAvailtoRequest
        {
            get { return this.QtyOnHand + QtyOnOrder - QtyReserved; }
        }

        public decimal LastPurchaseCost { get; set; }           // SOM-451
                                                                // Additional ClientLookups
        public string Account_ClientLookupId { get; set; }      // SOM-451
        public string AltPartId1_ClientLookupId { get; set; }   // SOM-451
        public string AltPartId2_ClientLookupId { get; set; }   // SOM-451
        public string AltPartId3_ClientLookupId { get; set; }   // SOM-451
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public string PartMasterClientLookupId { get; set; }
        public string ShortDescription { get; set; }
        public string LastPurchaseVendor { get; set; }
        public string SiteName { get; set; }
        public string Inactive { get; set; }
        // SOM-1495
        public int CustomQueryDisplayId { get; set; }
        public string PartIdList { get; set; }
        public string Flag { get; set; }


        public string PONumber { get; set; }

        public string POType { get; set; }

        public string POStatus { get; set; }

        public int LineNumber { get; set; }

        public string LineStatus { get; set; }
        public string PlDesc { get; set; }
        public string VendorClientlookupId { get; set; }
        public string VendorName { get; set; }

        //V2-291
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public string ManufactureId { get; set; }
        public string Section { get; set; }
        public string Row { get; set; }
        public string Shelf { get; set; }
        public string Bin { get; set; }
        public string PlaceArea { get; set; }
        public string Stock { get; set; }
        public string SearchText { get; set; }
        public string PMClientlookupId { get; set; }
        public string PMDescription { get; set; }
        public Int64 VendorId { get; set; }
        public Int64 VendorMasterId { get; set; }
        public Int64 VendorCatalogItemId { get; set; }
        public Int64 VendorCatalogId { get; set; }
        public string PurchaseUOM { get; set; }
        public string VendorPartNumber { get; set; }
        public decimal UnitCost { get; set; }
        public int VendorLeadTime { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public bool PartInactiveFlag { get; set; }
        public string CategoryDescription { get; set; }
        public UtilityAdd utilityAdd { get; set; }
        public List<Part> listOfPart { get; set; }
        public int TotalCount { get; set; }
        public long RowNum { get; set; }
        public bool CheckFlag { get; set; }
        public string AttachmentUrl { get; set; }
        public string CatalogType { get; set; }
        public long PoPrId { get; set; }
        public int Page { get; set; }
        public int ResultsPerPage { get; set; }
        //V2-553
        public string IssueUOM { get; set; }
        public decimal UOMConversion { get; set; }
        public bool UOMConvRequired { get; set; }
        public int VC_Count { get; set; }
        public decimal IssueOrder { get; set; }
        public decimal TransactionQuantity { get; set; }
        public DateTime? GenerateThrough { get; set; }
        public long PartHistoryId { get; set; }

        //V2-668
        public string PartMaster_ClientLookupId { get; set; }
        public string LongDescription { get; set; }
        // public string ShortDescription { get; set; }
        public string Category { get; set; }
        public string CategoryDesc { get; set; }
        //V2-670
        public int ChildCount { get; set; }
        public string AccounntClientLookupId { get; set; }
        public string IssueUnitDescription { get; set; }
        public string StockTypeDescription { get; set; }
        //V2-690
        public long PartCategoryMasterId { get; set; }
        public long IndexId { get; set; }
        //V2-932
        public decimal OnOrderQty { get; set; }
        public decimal OnRequestQTY { get; set; }
        public string UnitOfMeasure { get; set; }
        #region V2-1025
        public string DefStoreroom { get; set; }
        public string Storerooms { get; set; }
        public string StoreroomNameWithDescription { get; set; }
        public decimal TotalOnRequest { get; set; }
        public decimal TotalOnOrder { get; set; }
        public decimal TotalOnHand { get; set; }
        #endregion
        public bool AutoPurchase { get; set; }
        #endregion
        public static List<Part> UpdateFromDatabaseObjectList(List<b_Part> dbObjs)
        {
            List<Part> result = new List<Part>();

            foreach (b_Part dbObj in dbObjs)
            {
                Part tmp = new Part();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.PMClientlookupId = dbObj.PMClientlookupId;
                tmp.PMDescription = dbObj.PMDescription;
                tmp.VendorId = dbObj.VendorId;
                tmp.VendorName = dbObj.VendorName;
                tmp.VendorMasterId = dbObj.VendorMasterId;
                tmp.VendorCatalogItemId = dbObj.VendorCatalogItemId;
                tmp.VendorCatalogId = dbObj.VendorCatalogId;
                tmp.PartMasterId = dbObj.PartMasterId;
                tmp.PurchaseUOM = dbObj.PurchaseUOM;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.VendorPartNumber = dbObj.VendorPartNumber;
                tmp.VendorLeadTime = dbObj.VendorLeadTime;
                tmp.MinimumOrderQuantity = dbObj.MinimumOrderQuantity;
                tmp.PartInactiveFlag = dbObj.PartInactiveFlag;
                tmp.CategoryDescription = dbObj.CategoryDescription;
                tmp.TotalCount = dbObj.TotalCount;
                tmp.AttachmentUrl = dbObj.AttachmentUrl;
                tmp.CatalogType = dbObj.CatalogType;
                //V2-424
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.QtyMaximum = dbObj.QtyMaximum;
                tmp.QtyReorderLevel = dbObj.QtyReorderLevel;
                //V2-424

                //V2-553
                tmp.IssueUOM = dbObj.IssueUOM;
                tmp.UOMConversion = dbObj.UOMConversion;
                tmp.UOMConvRequired = dbObj.UOMConvRequired;
                tmp.VC_Count = dbObj.VC_Count;
                tmp.IssueOrder = dbObj.IssueOrder;
                tmp.IssueUnit = dbObj.IssueUnit;
                //V2-717
                tmp.PartCategoryMasterId = dbObj.PartCategoryMasterId;
                //V2-932
                tmp.OnOrderQty = dbObj.OnOrderQty;
                tmp.OnRequestQTY = dbObj.OnRequestQTY;
                tmp.IndexId = dbObj.IndexId;
                tmp.AccountId = dbObj.AccountId;
                result.Add(tmp);
            }
            return result;
        }
        public static List<Part> UpdateFromDatabaseCartWOObjectList(List<b_Part> dbObjs)
        {
            List<Part> result = new List<Part>();

            foreach (b_Part dbObj in dbObjs)
            {
                Part tmp = new Part();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.PMClientlookupId = dbObj.PMClientlookupId;
                tmp.PMDescription = dbObj.PMDescription;
                tmp.VendorId = dbObj.VendorId;
                tmp.VendorName = dbObj.VendorName;
                tmp.VendorMasterId = dbObj.VendorMasterId;
                tmp.VendorCatalogItemId = dbObj.VendorCatalogItemId;
                tmp.VendorCatalogId = dbObj.VendorCatalogId;
                tmp.PartMasterId = dbObj.PartMasterId;
                tmp.PurchaseUOM = dbObj.PurchaseUOM;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.VendorPartNumber = dbObj.VendorPartNumber;
                tmp.VendorLeadTime = dbObj.VendorLeadTime;
                tmp.MinimumOrderQuantity = dbObj.MinimumOrderQuantity;
                tmp.PartInactiveFlag = dbObj.PartInactiveFlag;
                tmp.CategoryDescription = dbObj.CategoryDescription;
                tmp.TotalCount = dbObj.TotalCount;
                tmp.AttachmentUrl = dbObj.AttachmentUrl;
                tmp.CatalogType = dbObj.CatalogType;
                //V2-424
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.QtyMaximum = dbObj.QtyMaximum;
                tmp.QtyReorderLevel = dbObj.QtyReorderLevel;
                //V2-424

                //V2-553
                tmp.IssueUOM = dbObj.IssueUOM;
                tmp.UOMConversion = dbObj.UOMConversion;
                tmp.UOMConvRequired = dbObj.UOMConvRequired;
                tmp.VC_Count = dbObj.VC_Count;
                tmp.IssueOrder = dbObj.IssueOrder;
                tmp.IssueUnit = dbObj.IssueUnit; //V2-1068 UnitOfMeasure is already taken IssueUnit from Part table
                //V2-690
                tmp.VendorClientlookupId = dbObj.VendorClientlookupId;
                tmp.IndexId=dbObj.IndexId;
                //V2-1068
                tmp.AccountId = dbObj.AccountId; //V2-1068
                tmp.UnitOfMeasure = dbObj.IssueUnit; //V2-1068
                result.Add(tmp);
            }
            return result;
        }
        public static List<Part> UpdateFromDatabaseWOObjectList(List<b_Part> dbObjs)
        {
            List<Part> result = new List<Part>();

            foreach (b_Part dbObj in dbObjs)
            {
                Part tmp = new Part();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.PMClientlookupId = dbObj.PMClientlookupId;
                tmp.PMDescription = dbObj.PMDescription;
                tmp.VendorId = dbObj.VendorId;
                tmp.VendorName = dbObj.VendorName;
                tmp.VendorMasterId = dbObj.VendorMasterId;
                tmp.VendorCatalogItemId = dbObj.VendorCatalogItemId;
                tmp.VendorCatalogId = dbObj.VendorCatalogId;
                tmp.PartMasterId = dbObj.PartMasterId;
                tmp.PurchaseUOM = dbObj.PurchaseUOM;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.VendorPartNumber = dbObj.VendorPartNumber;
                tmp.VendorLeadTime = dbObj.VendorLeadTime;
                tmp.MinimumOrderQuantity = dbObj.MinimumOrderQuantity;
                tmp.PartInactiveFlag = dbObj.PartInactiveFlag;
                tmp.CategoryDescription = dbObj.CategoryDescription;
                tmp.TotalCount = dbObj.TotalCount;
                tmp.AttachmentUrl = dbObj.AttachmentUrl;
                tmp.CatalogType = dbObj.CatalogType;
                //V2-424
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.QtyMaximum = dbObj.QtyMaximum;
                tmp.QtyReorderLevel = dbObj.QtyReorderLevel;
                //V2-424

                //V2-553
                tmp.IssueUOM = dbObj.IssueUOM;
                tmp.UOMConversion = dbObj.UOMConversion;
                tmp.UOMConvRequired = dbObj.UOMConvRequired;
                tmp.VC_Count = dbObj.VC_Count;
                tmp.IssueOrder = dbObj.IssueOrder;
                tmp.IssueUnit = dbObj.IssueUnit;
                //V2-690
                tmp.PartCategoryMasterId = dbObj.PartCategoryMasterId;
                tmp.VendorClientlookupId = dbObj.VendorClientlookupId;
                tmp.IndexId = dbObj.IndexId;
                tmp.AccountId = dbObj.AccountId;//V2-1068
                tmp.UnitOfMeasure = dbObj.IssueUOM;//V2-1068
                result.Add(tmp);
            }
            return result;
        }
        public void RetrieveByClientLookUpIdForInventory(DatabaseKey dbKey)
        {

            ValidateFor = "ValidateClientLookUpIdForPI";
            Validate<Part>(dbKey);
            if (IsValid)
            {
                Part_RetrieveByPartId trans = new Part_RetrieveByPartId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Part = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObjectForPI(trans.Part);
            }
        }
        public void UpdateFromDatabaseObjectForPI(b_Part dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PartId = dbObj.PartId;
            this.SiteId = dbObj.SiteId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Location = dbObj.Location;
            this.PartMasterClientLookupId = dbObj.PartMasterClientLookupId;


        }

        public void RetriveByPartId(DatabaseKey dbKey)
        {
            PartsStorerooms_RetrieveByPartId trans = new PartsStorerooms_RetrieveByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.Part);
        }

        /**********************Added By Indusnet Technologies***********************************/
        public List<Part> RetrieveForSearchBySiteId(DatabaseKey dbKey)
        {
            Parts_RetrieveForSearchBySiteId trans = new Parts_RetrieveForSearchBySiteId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDatabaseObject();
            // SOM-1495
            trans.Part.CustomQueryDisplayId = this.CustomQueryDisplayId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Part> partList = new List<Part>();
            foreach (b_Part parts in trans.PartList)
            {
                Part tmpParts = new Part();
                tmpParts.UpdateFromDatabaseObjectExtended(parts);
                partList.Add(tmpParts);
            }

            return partList;
        }
        public List<Part> RetrieveForSearchForMultipleSite(DatabaseKey dbKey)
        {
            Parts_RetrieveForSearchForMultipltSite trans = new Parts_RetrieveForSearchForMultipltSite()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,

            };
            trans.Part = this.ToDatabaseObject();
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Part> partList = new List<Part>();
            foreach (b_Part parts in trans.PartList)
            {
                Part tmpParts = new Part();
                tmpParts.UpdateFromDatabaseObjectExtended(parts);
                //{
                //    PartId = parts.PartId,
                //    ClientLookupId = parts.ClientLookupId
                //};
                partList.Add(tmpParts);
            }

            return partList;
        }
        public List<Part> RetrievePartSiteReview(DatabaseKey dbKey, string Timezone)
        {
            Part_RetrievePartSiteReview trans = new Part_RetrievePartSiteReview()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.UseTransaction = false;
            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Part> PartMasterList = new List<Part>();
            foreach (b_Part Part in trans.PartList)
            {
                Part tmpPartMaster = new Part();

                tmpPartMaster.UpdateFromDatabaseObjectForPartSiteReview(Part);
                PartMasterList.Add(tmpPartMaster);
            }
            return PartMasterList;
        }
        public void UpdateFromDatabaseObjectForPartSiteReview(b_Part dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.SiteName = dbObj.SiteName;
            this.QtyOnOrder = dbObj.QtyOnOrder;
            this.QtyOnRequest = dbObj.QtyOnRequest;
            this.QtyOnHand = dbObj.QtyOnHand;
            this.LastPurchaseCost = dbObj.LastPurchaseCost;
            this.LastPurchaseDate = dbObj.LastPurchaseDate;
            this.LastPurchaseVendor = dbObj.LastPurchaseVendor;

            switch (dbObj.InactiveFlag)
            {
                case true:
                    Inactive = "True";
                    break;
                case false:
                    Inactive = "False";
                    break;
                default:
                    break;
            }
        }
        public void RetrieveByClientLookUpIdNUPCCode(DatabaseKey dbKey)
        {
            Part_RetrieveByClientLookUpIdNUPCCode trans = new Part_RetrieveByClientLookUpIdNUPCCode()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.Part);
        }
        public List<Part> RetrievePartListByFilterText(DatabaseKey dbKey)
        {
            RetrievePartListByFilterText trans = new RetrievePartListByFilterText()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.RetPartList));

        }

        public List<Part> SearchForCartWO(DatabaseKey dbKey)
        {
            SearchForCartWOTrans trans = new SearchForCartWOTrans()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseCartWOObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseCartWOObjectList(trans.RetPartList));

        }
        public List<Part> SearchForCart(DatabaseKey dbKey)
        {
            SearchForCartTrans trans = new SearchForCartTrans()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.RetPartList));

        }
        public List<Part> SearchForCart_VendorCatalog(DatabaseKey dbKey)
        {
            SearchForCartTrans_VendorCatalog trans = new SearchForCartTrans_VendorCatalog()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.RetPartList));

        }
        public List<Part> SearchForCart_VendorCatalogWO(DatabaseKey dbKey)
        {
            SearchForCartTrans_VendorCatalogWO trans = new SearchForCartTrans_VendorCatalogWO()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseWOObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseWOObjectList(trans.RetPartList));

        }
        public void UpdateFromDatabaseObjectForSeacchCriteria(b_Part dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ShortDescription = dbObj.ShortDescription;
        }
        public List<Part> Part_RetrieveAll(DatabaseKey dbKey)
        {
            List<Part> result = new List<Part>();
            Database.Part_RetrievesAll trans = new Database.Part_RetrievesAll
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                ClientId = dbKey.Personnel.ClientId
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            result = UpdateFromDatabaseObjectList(trans.PartList);
            return result;
        }
        /*************************End***********************************************************/
        public void UpdateByPartId(DatabaseKey dbKey)
        {
            //Validate<Part>(dbKey);
            // Have to add a RetrieveStoredProcValidationData method to this class
            // Then stored procedure for validation       

            if (IsValid)
            {

                PartsStorerooms_UpdateByPartId trans = new PartsStorerooms_UpdateByPartId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Part = this.ToDatabaseObjectExtended();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The update procedure changed the Update Index.
                UpdateFromDatabaseObjectExtended(trans.Part);
            }
        }
        public void UpdateFromDatabaseObjectExtended(b_Part dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PartStoreroomId = dbObj.PartStoreroomId;
            this.CountFrequency = dbObj.CountFrequency;
            this.LastCounted = dbObj.LastCounted;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.QtyOnHand = dbObj.QtyOnHand;
            this.QtyReorderLevel = dbObj.QtyReorderLevel;
            this.ReorderMethod = dbObj.ReorderMethod;
            this.Storeroom_UpdateIndex = dbObj.Storeroom_UpdateIndex;
            this.QtyOnOrder = dbObj.QtyOnOrder;
            this.QtyOnRequest = dbObj.QtyOnRequest;
            this.QtyReserved = dbObj.QtyReserved;
            // SOM-451 - Begin
            this.Account_ClientLookupId = dbObj.Account_ClientLookupId;
            this.AltPartId1_ClientLookupId = dbObj.AltPartId1_ClientLookupId;
            this.AltPartId2_ClientLookupId = dbObj.AltPartId2_ClientLookupId;
            this.AltPartId3_ClientLookupId = dbObj.AltPartId3_ClientLookupId;
            this.LastPurchaseCost = dbObj.LastPurchaseCost;
            // SOM-451 - End
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;

            // updtaed on 21July
            this.Name = dbObj.Name;
            this.ManufacturerId = dbObj.ManufacturerId;
            this.AddressCity = dbObj.AddressCity;
            this.AddressState = dbObj.AddressState;
            this.PartStoreroomId = dbObj.PartStoreroomId;//V2-1196
        }
        public b_Part ToDatabaseObjectExtended()
        {
            b_Part dbObj = this.ToDatabaseObject();
            dbObj.PartStoreroomId = this.PartStoreroomId;
            dbObj.CountFrequency = this.CountFrequency;
            dbObj.LastCounted = this.LastCounted;
            dbObj.Location1_1 = this.Location1_1;
            dbObj.Location1_2 = this.Location1_2;
            dbObj.Location1_3 = this.Location1_3;
            dbObj.Location1_4 = this.Location1_4;
            dbObj.Location1_5 = this.Location1_5;
            dbObj.QtyMaximum = this.QtyMaximum;
            dbObj.QtyOnHand = this.QtyOnHand;
            dbObj.QtyReorderLevel = this.QtyReorderLevel;
            dbObj.ReorderMethod = this.ReorderMethod;
            dbObj.Storeroom_UpdateIndex = this.Storeroom_UpdateIndex;
            dbObj.QtyOnOrder = this.QtyOnOrder;
            dbObj.QtyOnRequest = this.QtyOnRequest;
            dbObj.FilterText = this.FilterText;
            dbObj.FilterStartIndex = this.FilterStartIndex;
            dbObj.FilterEndIndex = this.FilterEndIndex;
            dbObj.QtyReserved = this.QtyReserved;
            // SOM-451 - Begin
            dbObj.Account_ClientLookupId = this.Account_ClientLookupId;
            dbObj.AltPartId1_ClientLookupId = this.AltPartId1_ClientLookupId;
            dbObj.AltPartId2_ClientLookupId = this.AltPartId2_ClientLookupId;
            dbObj.AltPartId3_ClientLookupId = this.AltPartId3_ClientLookupId;
            dbObj.LastPurchaseCost = this.LastPurchaseCost;
            // SOM-451 - End
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.SearchText = this.SearchText;
            dbObj.CheckFlag = this.CheckFlag;
            //V2-533
            dbObj.VendorId = this.VendorId;
            //V2-1068
            dbObj.AccountId = this.AccountId;
            return dbObj;
        }

        public b_Part ToDatabaseCartWOObjectExtended()
        {
            b_Part dbObj = new b_Part();
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.SearchText = this.SearchText;
            return dbObj;
        }
        public b_Part ToDatabaseWOObjectExtended()
        {
            //  b_Part dbObj = this.ToDatabaseObject();
            b_Part dbObj = new b_Part();
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.SearchText = this.SearchText;
            dbObj.CheckFlag = this.CheckFlag;
            //V2-533
           // dbObj.VendorId = this.VendorId;
            return dbObj;
        }
        public void ProcessPart(DatabaseKey dbKey)
        {

            Part_ProcessPart trans = new Part_ProcessPart();
            trans.Part = this.ToDatabaseObject();
            trans.Part.ProcessMode = this.ProcessMode;
            trans.Part.PersonnelId = this.PersonnelId;
            trans.Part.PartMasterClientLookupId = this.PartMasterClientLookupId;
            trans.Part.AverageCostBefore = this.AverageCostBefore;
            trans.Part.AverageCostAfter = this.AverageCostAfter;
            trans.Part.CostAfter = this.CostAfter;
            trans.Part.CostBefore = this.CostBefore;
            trans.Part.PerformById = this.PerformById;
            trans.Part.TransactionType = this.TransactionType;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Part);
        }

        public static List<PartMultipleDataStructure> RetrieveBySiteId(DatabaseKey dbKey, long siteId)
        {
            Part_RetrieveBySiteId trans = new Part_RetrieveBySiteId()
            {
                dbKey = dbKey.ToTransDbKey(),
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                SiteId = siteId
            };

            trans.Execute();
            return trans.RawList;
        }

        public List<Part> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Parts_RetrieveClientLookupIdBySearchCriteria trans = new Parts_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Part> partList = new List<Part>();
            foreach (b_Part parts in trans.PartList)
            {
                Part tmpParts = new Part()
                {
                    PartId = parts.PartId,
                    ClientLookupId = parts.ClientLookupId,
                    Description = parts.Description
                };
                partList.Add(tmpParts);
            }

            return partList;
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateClientLookUpIdForPI")
            {
                Part_ValidateClientLookupIdForPI ptrans = new Part_ValidateClientLookupIdForPI()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.Part = this.ToDatabaseObject();
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            if (ValidateFor == "ValidateClientLookUpId")
            {
                Part_ValidateClientLookupIdTransaction ptrans = new Part_ValidateClientLookupIdTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.Part = this.ToDatabaseObject();
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            if (ValidateFor == "ValidateStockTypeIssueUnit")
            {
                Part_ValidateByStockTypeIssueUnit trans = new Part_ValidateByStockTypeIssueUnit()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                // RKL - 2014-Jul-31 - Use ToDatabaseObjectExtended 
                trans.Part = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                if (trans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in trans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            if (ValidateFor == "ValidateAdd")
            {
                Part_ValidateAddTransaction ptrans = new Part_ValidateAddTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.Part = this.ToDatabaseObject();
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            if (ValidateFor == "CheckIfInactivateorActivate")
            {
                Part_ValidateByInactivateorActivate ptrans = new Part_ValidateByInactivateorActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.Part = this.ToDatabaseObject();
                ptrans.Part.Flag = this.Flag;
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            if (ValidateFor == "ValidateClientLookUpIdV2")
            {
                Part_ValidateClientLookupIdV2Transaction ptrans = new Part_ValidateClientLookupIdV2Transaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.Part = this.ToDatabaseObject();
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            return errors;
        }


        #region Transactions

        //public void RetrieveInitialSearchConfigurationData(DatabaseKey dbKey, ClientWebSite local)
        //{

        //    Part_RetrieveInitialSearchConfigurationData trans = new Part_RetrieveInitialSearchConfigurationData()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName
        //    };
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();
        //    SearchCriteria = trans.SearchCriteria;

        //    // Add the Dates
        //    Load_DateSelection(local);

        //    // Add the 'Columns'
        //    Load_ColumnSelection(local);
        //}

        public void ChangeClientLoopupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateClientLookUpId";
            Validate<Part>(dbKey);

            if (IsValid)
            {
                PartsStorerooms_UpdateByPartId trans = new PartsStorerooms_UpdateByPartId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };

                trans.Part = this.ToDatabaseObjectExtended();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The update procedure changed the Update Index.
                UpdateFromDatabaseObjectExtended(trans.Part);
            }
        }

        //------SOM-786------//
        public void RetrieveCreateModifyDate(DatabaseKey dbKey)
        {
            Part_RetrieveCreateModifyDate trans = new Part_RetrieveCreateModifyDate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Part);
            this.CreateBy = trans.Part.CreateBy;
            this.CreateDate = trans.Part.CreateDate;
            this.ModifyBy = trans.Part.ModifyBy;
            this.ModifyDate = trans.Part.ModifyDate;
        }
        #endregion

        #region Search Parameter Lists
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }

        //private void Load_DateSelection(ClientWebSite local)
        //{

        //    List<KeyValuePair<string, string>> search_DateSelection = new List<KeyValuePair<string, string>>();
        //    foreach (PropertyInfo pi in this.GetType().GetProperties())
        //    {
        //        if (pi.PropertyType == typeof(DateTime) || pi.PropertyType == typeof(DateTime?))
        //        {
        //            if (pi.Name.ToString() == "LastCounted")
        //            {
        //                search_DateSelection.Add(new KeyValuePair<string, string>(pi.Nameal.PartStoreroom.GetLabelTextByFieldName(pi.Name)));    // 20110024
        //            }
        //            else
        //            {
        //                search_DateSelection.Add(new KeyValuePair<string, string>(pi.Nameal.Part.GetLabelTextByFieldName(pi.Name)));    // 20110024
        //            }
        //        }

        //    }
        //    SearchCriteria.Add("dates", search_DateSelection);
        //}

        //private void Load_ColumnSelection(ClientWebSite local)
        //{
        //    List<KeyValuePair<string, string>> search_ColumnSelection = new List<KeyValuePair<string, string>>();

        //    search_ColumnSelection.Add(new KeyValuePair<string, string>("ClientLookupId"al.Part.ClientLookupId.LabelText));         // 20110019,20110024
        //    search_ColumnSelection.Add(new KeyValuePair<string, string>("Description"al.Part.Description.LabelText));
        //    search_ColumnSelection.Add(new KeyValuePair<string, string>("Manufacturer"al.Part.Manufacturer.LabelText));
        //    search_ColumnSelection.Add(new KeyValuePair<string, string>("ManufacturerId"al.Part.ManufacturerId.LabelText));
        //    search_ColumnSelection.Add(new KeyValuePair<string, string>("Location1_1"al.PartStoreroom.Location1_1.LabelText));

        //    SearchCriteria.Add("columns", search_ColumnSelection);
        //}


        #endregion
        //------------From Api Calls--------------------------------------------------------------------------------
        public List<Part> RetrieveBySiteIdAndClientLookUpId(DatabaseKey dbKey)
        {
            Part_RetrieveBySiteIdAndClientLookUpId trans = new Part_RetrieveBySiteIdAndClientLookUpId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.PartList));

        }
        public void CreateWithValidationStockTypeIssueUnit(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateStockTypeIssueUnit";
            Validate<Part>(dbKey);
            if (IsValid)
            {
                Part_Create trans = new Part_Create();
                trans.Part = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Part);
            }
        }

        public void UpdateWithValidationStockTypeIssueUnit(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateStockTypeIssueUnit";
            Validate<Part>(dbKey);
            if (IsValid)
            {
                Part_Update trans = new Part_Update();
                trans.Part = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Part);
            }
        }
        public void ValidateAdd(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateAdd";
            Validate<Part>(dbKey);
        }

        public void CheckPartIsInactivateorActivate(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfInactivateorActivate";
            Validate<Part>(dbKey);
        }

        public List<Part> RetrieveAll(DatabaseKey dbKey)
        {
            //Part_Retrieve trans = new Part_Retrieve();
            //trans.Part = this.ToDatabaseObject();
            //trans.dbKey = dbKey.ToTransDbKey();
            //trans.Execute();
            //UpdateFromDatabaseObject(trans.Part);

            Part_RetrieveAll_V2 trans = new Part_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Part> PartList = new List<Part>();
            foreach (b_Part Part in trans.PartList)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObject(Part);
                PartList.Add(tmpPart);
            }
            return PartList;
        }
        public void UpdateBulk(DatabaseKey dbKey)
        {
            Part_UpdateBulk trans = new Part_UpdateBulk();
            trans.Part = this.ToDatabaseObject();
            trans.Part.PartIdList = this.PartIdList;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Part);
        }


        public List<Part> RetrievePOandPRforPart(DatabaseKey dbKey)
        {

            Part_RetrievePOandPR trans = new Part_RetrievePOandPR()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Part> PartList = new List<Part>();
            foreach (b_Part Part in trans.PartList)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectforRetrievePOandPR(Part);
                PartList.Add(tmpPart);
            }
            return PartList;
        }


        public void UpdateFromDatabaseObjectforRetrievePOandPR(b_Part dbObjs)
        {
            this.PartId = dbObjs.PartId;
            this.POType = dbObjs.POType;
            this.PONumber = dbObjs.PONumber;
            this.POStatus = dbObjs.POStatus;
            this.LineNumber = dbObjs.LineNumber;
            this.LineStatus = dbObjs.LineStatus;
            this.PlDesc = dbObjs.PlDesc;
            this.VendorClientlookupId = dbObjs.VendorClientlookupId;
            this.VendorName = dbObjs.VendorName;
            this.CreateDate = dbObjs.CreateDate;
            this.PoPrId = dbObjs.PoPrId;
        }

        public void PartRetrieveforMentionAlert(DatabaseKey dbKey)
        {
            Part_RetrieveforMentionAlert trans = new Part_RetrieveforMentionAlert()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.Part);
            this.CreateBy = trans.Part.CreateBy;
            this.CreateDate = trans.Part.CreateDate;
            this.ModifyBy = trans.Part.ModifyBy;
            this.ModifyDate = trans.Part.ModifyDate;

        }

        public Part PartChunkSearchV2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchV2 trans = new Part_ChunkSearchV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearch(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }


        public Part PartSearchForPrintV2(DatabaseKey dbKey, string TimeZone)
        {
            Part_SearchForPrintV2 trans = new Part_SearchForPrintV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearch(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }

        public b_Part ToDateBaseObjectForChunkSearch()
        {
            b_Part dbObj = this.ToDatabaseObject();

            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.SearchText = this.SearchText;
            dbObj.Section = this.Section;
            dbObj.Row = this.Row;
            dbObj.Shelf = this.Shelf;
            dbObj.Bin = this.Bin;
            dbObj.QtyOnHand = this.QtyOnHand;
            dbObj.QtyReorderLevel = this.QtyReorderLevel;
            dbObj.PlaceArea = this.PlaceArea;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForChunkSearch(b_Part dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.PrevClientLookupId = dbObj.PrevClientLookupId;
            this.Location1_1 = dbObj.Location1_1;
            this.QtyOnHand = dbObj.QtyOnHand;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.QtyReorderLevel = dbObj.QtyReorderLevel;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.AppliedCost = dbObj.AppliedCost;//V2-840
            this.TotalCount = dbObj.TotalCount;

        }

        public Part PartChunkSearchForMultiPartStoreroomV2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchForMultiPartStoreroomV2 trans = new Part_ChunkSearchForMultiPartStoreroomV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForForMultiPartStoreroomChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForMultiPartStoreroomChunkSearch(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        public b_Part ToDateBaseObjectForForMultiPartStoreroomChunkSearch()
        {
            b_Part dbObj= new b_Part();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PersonnelId = this.PersonnelId;

            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.CustomQueryDisplayId = this.CustomQueryDisplayId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.SearchText = this.SearchText;
            dbObj.StockType = this.StockType;
            dbObj.Storerooms = this.Storerooms;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForMultiPartStoreroomChunkSearch(b_Part dbObj, string TimeZone)
        {
            this.PartId = dbObj.PartId;
            this.ClientLookupId = dbObj.ClientLookupId;                 
            this.Description = dbObj.Description;
            this.StockType = dbObj.StockType;
            this.AppliedCost = dbObj.AppliedCost;
            this.ChildCount = dbObj.ChildCount;
            this.TotalCount = dbObj.TotalCount;
            this.DefStoreroom = dbObj.DefStoreroom;
        }

        public Part PartChunkSearchLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchLookupListV2 trans = new Part_ChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearchLookupListV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearchLookupListV2(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }


        public b_Part ToDateBaseObjectForChunkSearchLookupListV2()
        {
            b_Part dbObj = this.ToDatabaseObject();
            dbObj.Page = this.Page;
            dbObj.ResultsPerPage = this.ResultsPerPage;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.VendorId = this.VendorId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForChunkSearchLookupListV2(b_Part dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCount = dbObj.TotalCount;
            this.PartStoreroomId = dbObj.PartStoreroomId;
        }

        public Part RetrievePartIdByClientLookupId(DatabaseKey dbKey)
        {
            Parts_RetrievePartIdClientLookupId trans = new Parts_RetrievePartIdClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForRetrievePartIdByClientLookupId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Part tmpParts = new Part()
            {
                PartId = trans.PartResult.PartId,
                ClientLookupId = trans.PartResult.ClientLookupId,
            };

            return tmpParts;
        }

        public b_Part ToDateBaseObjectForRetrievePartIdByClientLookupId()
        {
            b_Part dbObj = this.ToDatabaseObject();
            dbObj.ClientLookupId = this.ClientLookupId;
            
            return dbObj;
        }

        #region V2-563
       

        public List<Part> RetrieveCalatogEntriesForPartByPartId(DatabaseKey dbKey)
        {
            Part_SearchForCalatogEntriesForPartChunkSearch trans = new Part_SearchForCalatogEntriesForPartChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectForCalatogEntriesForPartChunkSearch(trans.RetPartList));

        }
        public static List<Part> UpdateFromDatabaseObjectForCalatogEntriesForPartChunkSearch(List<b_Part> dbObjs)
        {
            List<Part> result = new List<Part>();

            foreach (b_Part dbObj in dbObjs)
            {
                Part tmp = new Part();
               // tmp.UpdateFromDatabaseObject(dbObj);
                tmp.PartId = dbObj.PartId;
                tmp.VendorId = dbObj.VendorId;
                tmp.ClientLookupId = dbObj.ClientLookupId;
                tmp.VendorClientlookupId = dbObj.Vendor_ClientLookupId;
                tmp.VendorName = dbObj.Vendor_Name;
                tmp.VendorPartNumber = dbObj.VendorPartNumber;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.PurchaseUOM = dbObj.PurchaseUOM;
                tmp.Description = dbObj.Description;
                tmp.PartStoreroomId = dbObj.PartStoreroomId;
                tmp.IssueUnit = dbObj.IssueUnit;
                tmp.VendorCatalogItemId = dbObj.VendorCatalogItemId;
                tmp.TotalCount = dbObj.TotalCount;
               
                result.Add(tmp);
            }
            return result;
        }
        #endregion
        #region Part cycle count chunk search
        public Part PartCycleCountChunkSearch(DatabaseKey dbKey)
        {
            Part_CycleCountChunkSearch trans = new Part_CycleCountChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForPartCycleCountChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForPartCycleCountChunkSearch(Part);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        public b_Part ToDateBaseObjectForPartCycleCountChunkSearch()
        {          
            b_Part dbObj = new b_Part();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;          
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;         
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.Section = this.Section;
            dbObj.PlaceArea = this.PlaceArea;
            dbObj.Row = this.Row;
            dbObj.Shelf = this.Shelf;
            dbObj.Bin = this.Bin;
            dbObj.StockType = this.StockType;
            dbObj.Critical = this.Critical;
            dbObj.Consignment = this.Consignment;
            dbObj.GenerateThrough = this.GenerateThrough;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForPartCycleCountChunkSearch(b_Part dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.QtyOnHand = dbObj.QtyOnHand;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;         
            this.TotalCount = dbObj.TotalCount;

        }
        #endregion

        #region V2-668
        public void RetriveByPartId_V2(DatabaseKey dbKey)
        {
            PartsStorerooms_RetrieveByPartId_V2 trans = new PartsStorerooms_RetrieveByPartId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended_V2(trans.Part);
        }

        public void UpdateFromDatabaseObjectExtended_V2(b_Part dbObj)
        {
            this.UpdateFromDatabaseObjectExtended(dbObj);
            this.PartMaster_ClientLookupId = dbObj.PartMaster_ClientLookupId;
            this.LongDescription = dbObj.LongDescription;
            this.ShortDescription = dbObj.ShortDescription;
            this.Category = dbObj.Category;
            this.CategoryDesc = dbObj.CategoryDesc;
            this.VendorClientlookupId = dbObj.VendorClientlookupId;
            this.VendorName = dbObj.VendorName;
            this.AutoPurchase = dbObj.AutoPurchase;
        }
        #endregion

        #region V2-670
        public void ValidateAddMultiStoreroomPart(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateClientLookUpIdV2";
            Validate<Part>(dbKey);
        }
        public void MultiStoreroomRetriveByPartId(DatabaseKey dbKey)
        {
            MultiStoreroomPart_RetrieveByPartId trans = new MultiStoreroomPart_RetrieveByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtendedByPartid_V2(trans.Part);
        }

        public void UpdateFromDatabaseObjectExtendedByPartid_V2(b_Part dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PartId = dbObj.PartId;
            this.SiteId = dbObj.SiteId;
            this.AreaId = dbObj.AreaId;
            this.DepartmentId = dbObj.DepartmentId;
            this.StoreroomId = dbObj.StoreroomId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.AccountId = dbObj.AccountId;
            this.AccounntClientLookupId = dbObj.AccountClientLookupId;
            this.AppliedCost = dbObj.AppliedCost;
            this.AverageCost = dbObj.AverageCost;
            this.Consignment = dbObj.Consignment;
            this.Critical = dbObj.Critical;
            this.Description = dbObj.Description;
            this.InactiveFlag = dbObj.InactiveFlag;
            this.IssueUnit = dbObj.IssueUnit;
            this.IssueUnitDescription = dbObj.IssueUnitDescription;
            this.Manufacturer = dbObj.Manufacturer;
            this.ManufacturerId = dbObj.ManufacturerId;
            this.MSDSContainerCode = dbObj.MSDSContainerCode;
            this.MSDSPressureCode = dbObj.MSDSPressureCode;
            this.MSDSReference = dbObj.MSDSReference;
            this.MSDSRequired = dbObj.MSDSRequired;
            this.MSDSTemperatureCode = dbObj.MSDSTemperatureCode;
            this.NoEquipXref = dbObj.NoEquipXref;
            this.StockType = dbObj.StockType;
            this.StockTypeDescription = dbObj.StockTypeDescription;
            this.UPCCode = dbObj.UPCCode;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.QtyReorderLevel = dbObj.QtyReorderLevel;
            this.ABCCode = dbObj.ABCCode;
            this.ABCStoreCost = dbObj.ABCStoreCost;
            this.TotalOnHand = dbObj.TotalOnHand;
            this.TotalOnRequest = dbObj.TotalOnRequest;
            this.TotalOnOrder = dbObj.TotalOnOrder;
            this.DefaultStoreroom = dbObj.DefaultStoreroom;
            this.DefStoreroom = dbObj.DefStoreroom;
            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion

        #region V2-687
        public Part RetrievePartIdByStoreroomIdAndClientLookupId(DatabaseKey dbKey)
        {
            Parts_RetrievePartIdByStoreroomIdAndClientLookupId trans = new Parts_RetrievePartIdByStoreroomIdAndClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForRetrievePartIdByStoreroomIdAndClientLookupId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Part tmpParts = new Part()
            {
                PartId = trans.PartResult.PartId,
                ClientLookupId = trans.PartResult.ClientLookupId,
            };

            return tmpParts;
        }

        public b_Part ToDateBaseObjectForRetrievePartIdByStoreroomIdAndClientLookupId()
        {
            b_Part dbObj = this.ToDatabaseObject();
            dbObj.ClientLookupId = this.ClientLookupId;

            return dbObj;
        }
        public Part PartChunkSearchLookupListForMultiStoreroom_V2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchLookupListForMultiStoreroom_V2 trans = new Part_ChunkSearchLookupListForMultiStoreroom_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearchLookupListForMultiStoreroom_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearchLookupListForMultiStoreroom_V2(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }


        public b_Part ToDateBaseObjectForChunkSearchLookupListForMultiStoreroom_V2()
        {
            b_Part dbObj = this.ToDatabaseObject();
            dbObj.Page = this.Page;
            dbObj.ResultsPerPage = this.ResultsPerPage;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.VendorId = this.VendorId;
            return dbObj;
        }

        public void UpdateFromDatabaseObjectForChunkSearchLookupListForMultiStoreroom_V2(b_Part dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCount = dbObj.TotalCount;
            this.PartStoreroomId = dbObj.PartStoreroomId;

        }

        #region Part cycle count chunk search For MultiStoreroom
        public Part PartCycleCountChunkSearchForMultiStoreroom(DatabaseKey dbKey)
        {
            Part_CycleCountChunkSearchForMultiStoreroom trans = new Part_CycleCountChunkSearchForMultiStoreroom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForPartCycleCountChunkSearchForMultiStoreroom();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForPartCycleCountChunkSearchForMultiStoreroom(Part);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        public b_Part ToDateBaseObjectForPartCycleCountChunkSearchForMultiStoreroom()
        {
            b_Part dbObj = new b_Part();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Description = this.Description;
            dbObj.Section = this.Section;
            dbObj.PlaceArea = this.PlaceArea;
            dbObj.Row = this.Row;
            dbObj.Shelf = this.Shelf;
            dbObj.Bin = this.Bin;
            dbObj.StockType = this.StockType;
            dbObj.Critical = this.Critical;
            dbObj.Consignment = this.Consignment;
            dbObj.GenerateThrough = this.GenerateThrough;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }
        public void UpdateFromDatabaseObjectForPartCycleCountChunkSearchForMultiStoreroom(b_Part dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.QtyOnHand = dbObj.QtyOnHand;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.TotalCount = dbObj.TotalCount;

        }
        #endregion
        #region Retrive By PartId For MultiStoreroom 
        public void RetriveByPartIdForMultiStoreroom_V2(DatabaseKey dbKey)
        {
            PartsStorerooms_RetrieveByPartIdForMultiStoreroom_V2 trans = new PartsStorerooms_RetrieveByPartIdForMultiStoreroom_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtendedForMultiStoreroom_V2(trans.Part);
        }

        public void UpdateFromDatabaseObjectExtendedForMultiStoreroom_V2(b_Part dbObj)
        {
            this.UpdateFromDatabaseObjectExtended(dbObj);
            this.PartMaster_ClientLookupId = dbObj.PartMaster_ClientLookupId;
            this.LongDescription = dbObj.LongDescription;
            this.ShortDescription = dbObj.ShortDescription;
            this.Category = dbObj.Category;
            this.CategoryDesc = dbObj.CategoryDesc;
        }
        #endregion

        #endregion

        #region V2-732
        public List<Part> SearchForCart_VendorCatalogWOMultiStoreroom(DatabaseKey dbKey)
        {
            SearchForCartTrans_VendorCatalogWOMultiStoreroom trans = new SearchForCartTrans_VendorCatalogWOMultiStoreroom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseWOObjectExtendedWOMultiStoreroom();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseWOMultiStoreroomObjectList(trans.RetPartList));

        }
        public b_Part ToDatabaseWOObjectExtendedWOMultiStoreroom()
        {
            //  b_Part dbObj = this.ToDatabaseObject();
            b_Part dbObj = new b_Part();
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.SearchText = this.SearchText;
            dbObj.CheckFlag = this.CheckFlag;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }
        public static List<Part> UpdateFromDatabaseWOMultiStoreroomObjectList(List<b_Part> dbObjs)
        {
            List<Part> result = new List<Part>();

            foreach (b_Part dbObj in dbObjs)
            {
                Part tmp = new Part();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.PMClientlookupId = dbObj.PMClientlookupId;
                tmp.PMDescription = dbObj.PMDescription;
                tmp.VendorId = dbObj.VendorId;
                tmp.VendorName = dbObj.VendorName;
                tmp.VendorMasterId = dbObj.VendorMasterId;
                tmp.VendorCatalogItemId = dbObj.VendorCatalogItemId;
                tmp.VendorCatalogId = dbObj.VendorCatalogId;
                tmp.PartMasterId = dbObj.PartMasterId;
                tmp.PurchaseUOM = dbObj.PurchaseUOM;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.VendorPartNumber = dbObj.VendorPartNumber;
                tmp.VendorLeadTime = dbObj.VendorLeadTime;
                tmp.MinimumOrderQuantity = dbObj.MinimumOrderQuantity;
                tmp.PartInactiveFlag = dbObj.PartInactiveFlag;
                tmp.CategoryDescription = dbObj.CategoryDescription;
                tmp.TotalCount = dbObj.TotalCount;
                tmp.AttachmentUrl = dbObj.AttachmentUrl;
                tmp.CatalogType = dbObj.CatalogType;
                //V2-424
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.QtyMaximum = dbObj.QtyMaximum;
                tmp.QtyReorderLevel = dbObj.QtyReorderLevel;
                //V2-424

                //V2-553
                tmp.IssueUOM = dbObj.IssueUOM;
                tmp.UOMConversion = dbObj.UOMConversion;
                tmp.UOMConvRequired = dbObj.UOMConvRequired;
                tmp.VC_Count = dbObj.VC_Count;
                tmp.IssueOrder = dbObj.IssueOrder;
                tmp.IssueUnit = dbObj.IssueUnit;
                //V2-690
                tmp.PartCategoryMasterId = dbObj.PartCategoryMasterId;
                tmp.VendorClientlookupId = dbObj.VendorClientlookupId;
                tmp.IndexId = dbObj.IndexId;
                tmp.PartStoreroomId = dbObj.PartStoreroomId;
                tmp.StoreroomId = dbObj.StoreroomId; 
                tmp.AccountId = dbObj.AccountId;  //V2- 1068
                tmp.UnitOfMeasure = dbObj.IssueUnit;//V2-1068
                result.Add(tmp);
            }
            return result;
        }
        public List<Part> SearchForCartWOMultiStoreroom(DatabaseKey dbKey)
        {
            SearchForCartWOTransMultiStoreroom trans = new SearchForCartWOTransMultiStoreroom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseCartWOObjectExtendedMultiStoreroom();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseCartWOMultiStoreroomObjectList(trans.RetPartList));

        }
        public b_Part ToDatabaseCartWOObjectExtendedMultiStoreroom()
        {
            b_Part dbObj = new b_Part();
            dbObj.SiteId = this.SiteId;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.SearchText = this.SearchText;
            dbObj.StoreroomId = this.StoreroomId;
            return dbObj;
        }
        public static List<Part> UpdateFromDatabaseCartWOMultiStoreroomObjectList(List<b_Part> dbObjs)
        {
            List<Part> result = new List<Part>();

            foreach (b_Part dbObj in dbObjs)
            {
                Part tmp = new Part();
                tmp.UpdateFromDatabaseObject(dbObj);
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.PMClientlookupId = dbObj.PMClientlookupId;
                tmp.PMDescription = dbObj.PMDescription;
                tmp.VendorId = dbObj.VendorId;
                tmp.VendorName = dbObj.VendorName;
                tmp.VendorMasterId = dbObj.VendorMasterId;
                tmp.VendorCatalogItemId = dbObj.VendorCatalogItemId;
                tmp.VendorCatalogId = dbObj.VendorCatalogId;
                tmp.PartMasterId = dbObj.PartMasterId;
                tmp.PurchaseUOM = dbObj.PurchaseUOM;
                tmp.UnitCost = dbObj.UnitCost;
                tmp.VendorPartNumber = dbObj.VendorPartNumber;
                tmp.VendorLeadTime = dbObj.VendorLeadTime;
                tmp.MinimumOrderQuantity = dbObj.MinimumOrderQuantity;
                tmp.PartInactiveFlag = dbObj.PartInactiveFlag;
                tmp.CategoryDescription = dbObj.CategoryDescription;
                tmp.TotalCount = dbObj.TotalCount;
                tmp.AttachmentUrl = dbObj.AttachmentUrl;
                tmp.CatalogType = dbObj.CatalogType;
                //V2-424
                tmp.QtyOnHand = dbObj.QtyOnHand;
                tmp.QtyMaximum = dbObj.QtyMaximum;
                tmp.QtyReorderLevel = dbObj.QtyReorderLevel;
                //V2-424

                //V2-553
                tmp.IssueUOM = dbObj.IssueUOM;
                tmp.UOMConversion = dbObj.UOMConversion;
                tmp.UOMConvRequired = dbObj.UOMConvRequired;
                tmp.VC_Count = dbObj.VC_Count;
                tmp.IssueOrder = dbObj.IssueOrder;
                tmp.IssueUnit = dbObj.IssueUnit;
                //V2-690
                tmp.VendorClientlookupId = dbObj.VendorClientlookupId;
                tmp.IndexId=dbObj.IndexId;
                tmp.PartStoreroomId = dbObj.PartStoreroomId;
                tmp.StoreroomId = dbObj.StoreroomId;
                tmp.AccountId = dbObj.AccountId;//V2-1068
                tmp.UnitOfMeasure = dbObj.IssueUnit;//V2-1068
                result.Add(tmp);
            }
            return result;
        }
        #endregion
        #region V2-738
        public List<Part> SearchForCartMultiStoreroom(DatabaseKey dbKey)
        {
            SearchForCartTransMultiStoreroom trans = new SearchForCartTransMultiStoreroom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.RetPartList));

        }
        public List<Part> SearchForCart_VendorCatalogMultiStoreroom(DatabaseKey dbKey)
        {
            SearchForCartTrans_VendorCatalogForMultiStoreroom trans = new SearchForCartTrans_VendorCatalogForMultiStoreroom()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.RetPartList));

        }
        #endregion

        #region V2-736
        public Part PartChunkSearchLookupListForMultiStoreroomMobile_V2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchLookupListForMultiStoreroomMobile_V2 trans = new Part_ChunkSearchLookupListForMultiStoreroomMobile_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearchLookupListForMultiStoreroom_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearchLookupListForMultiStoreroom_V2(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        public Part PartChunkSearchLookupListMobileV2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchLookupListMobileV2 trans = new Part_ChunkSearchLookupListMobileV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearchLookupListV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearchLookupListV2(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        #endregion
        #region Change Part Clientlookup
        public void ChangeClientLookupId_V2(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateClientLookUpId";
            Validate<Part>(dbKey);
            if (IsValid)
            {

                Part_ChangeClientLookupId_V2 trans = new Part_ChangeClientLookupId_V2()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                AuditEnabled = true;
                trans.Part = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.Part);
            }
        }
        #endregion

        #region V2-1045
        public Part RetrievePartIdByClientLookupIdForFindPart(DatabaseKey dbKey)
        {
            Parts_RetrievePartIdClientLookupIdForFindPart trans = new Parts_RetrievePartIdClientLookupIdForFindPart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForRetrievePartIdByClientLookupId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            Part tmpParts = new Part()
            {
                PartId = trans.PartResult.PartId,
                ClientLookupId = trans.PartResult.ClientLookupId,
            };

            return tmpParts;
        }
        #endregion
        #region RKL MAIL -Label Printing from Receipts
        public void RetrieveByPartIdAndPartStoreroomId_V2(DatabaseKey dbKey)
        {
            PartsStorerooms_RetrieveByPartIdAndPartStoreroomId_V2 trans = new PartsStorerooms_RetrieveByPartIdAndPartStoreroomId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part = this.ToDatabaseObject();
            trans.Part.PartStoreroomId = this.PartStoreroomId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.Part);
        }
        #endregion
        #region V2-1167
        public Part PartChunkSearchLookupListForSingleStockLineItemV2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchLookupListForSingleStockLineItemV2 trans = new Part_ChunkSearchLookupListForSingleStockLineItemV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearchLookupListV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
         
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearchLookupListV2(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        public Part PartChunkSearchLookupListForSingleStockLineItemMultiStoreroom_V2(DatabaseKey dbKey, string TimeZone)
        {
            Part_ChunkSearchLookupListForSingleStockLineItemMultiStoreroom_V2 trans = new Part_ChunkSearchLookupListForSingleStockLineItemMultiStoreroom_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part = this.ToDateBaseObjectForChunkSearchLookupListForMultiStoreroom_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPart = new List<Part>();


            List<Part> Partlist = new List<Part>();
           
            foreach (b_Part Part in trans.Part.listOfPart)
            {
                Part tmpPart = new Part();

                tmpPart.UpdateFromDatabaseObjectForChunkSearchLookupListForMultiStoreroom_V2(Part, TimeZone);
                Partlist.Add(tmpPart);
            }
            this.listOfPart.AddRange(Partlist);
            return this;
        }
        #endregion
    }
}
