/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015-2017 by SOMAX Inc.
* PurchaseRequestLineItem.cs (Data Contract)
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Oct-17 SOM-369  Roger Lawton       
* 2014-Nov-03 SOM-398  Roger Lawton       Clean up method LoadFromDatabaseExtended
* 2014-Nov-12 SOM-419  Roger Lawton       Modified              
* 2015-Mar-08 SOM-594  Roger Lawton       Removed PartStoreroomId (handled in LoadFromDatabase)
* 2015-Mar-18 SOM-608  Roger Lawton       Added Account_ClientLookupId
*                                         Changed to use PurchaseRequestLineItem_CreateWithReplication
* 2017-Mar-21 SOM-1286 Nick Fuchs         Add Mfg and Mfg ID                                         
****************************************************************************************************
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Extensions;
using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{
    public partial class PurchaseRequestLineItem : DataContractBase,IStoredProcedureValidation
    {
        //public WorkFlowLog workflowlog { get; set; }
        //public DateTime CreateDate { get; set; }
        //public string VendorName { get; set; }
        //public string VendorPhoneNumber { get; set; }
        //public string Creator_PersonnelName { get; set; }
        public string PartClientLookupId { get; set; }
        public decimal TotalCost { get; set; }
        public string ChargeToClientLookupId { get; set; }
        public string ChargeTo_Name { get; set; }
        public string Account_ClientLookupId { get; set; }
        public string ErrorMessageRow { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityToDate { get; set; }
        public decimal CurrentAverageCost { get; set; }
        public decimal CurrentAppliedCost { get; set; }
        public decimal CurrentOnHandQuantity { get; set; }
        // Remove this - it is a duplicate of PartStoreroomId
        // RKL -2020-Dec-12
        //public Int64 PartStoreRoomId { get; set; }
        public string StockType { get; set; }
        public decimal QuantityBackOrdered { get; set; }
        public string Part_Manufacturer { get; set; }
        public string Part_ManufacturerID { get; set; }
        public bool Ispunchout { get; set; } //V2-548
        public bool UOMConvRequired { get; set; } //V2-553
        public string PartCategoryMasterClientLookupId { get; set; }
        public long StoreroomId { get; set; }
        public string ClientLookupId { get; set; }//V2-894
        public string Name { get; set; }//V2-894
        public DateTime CreateDate { get; set; }//V2-894
        public string OrderbyColumn { get; set; }
        public string OrderBy { get; set; }
        public int OffSetVal { get; set; }
        public int NextRow { get; set; }
        public long TotalCount { get; set; }
        #region V2-1046
        public string VendorClientLookupId { get; set; }
        public string VendorName { get; set; }
        public long SiteId { get; set; }
        public long PersonnelId { get; set; }
        public string PRLineItemIds { get; set; }
        #endregion

        #region V2-1063
        public string WorkOrderClientLookupId { get; set; }
        public decimal UnitCostQuantity { get; set; }
        #endregion
        public static List<PurchaseRequestLineItem> PurchaseRequestLineItemRetrieveByPurchaseRequestId_Latest(DatabaseKey dbKey, PurchaseRequestLineItem purchaseorderlineitem)
        {
            List<PurchaseRequestLineItem> PurchaseRequestLineItemList = new List<PurchaseRequestLineItem>();

            PurchaseRequestLineItem_RetrieveByPurchaseRequestId trans = new PurchaseRequestLineItem_RetrieveByPurchaseRequestId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };

            trans.PurchaseRequestLineItem = purchaseorderlineitem.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PurchaseRequestLineItem.UpdateFromDatabaseObjectList_Latest(trans.PurchaseRequestLineItemList);

        }
        public void UpdateFromDataBaseObjectExtended(b_PurchaseRequestLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCost = dbObj.TotalCost;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.Account_ClientLookupId = dbObj.Account_ClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;            
        }

        public void UpdateFromDataBaseObjectExtendedV2(b_PurchaseRequestLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientId = dbObj.ClientId;
            this.PurchaseRequestLineItemId = dbObj.PurchaseRequestLineItemId;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.PartId = dbObj.PartId;
            this.LineNumber = dbObj.LineNumber;
            this.Description = dbObj.Description;
            this.OrderQuantity = dbObj.OrderQuantity;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.UnitCost = dbObj.UnitCost;
            this.AccountId = dbObj.AccountId;
            this.ChargeType = dbObj.ChargeType;
            this.ChargeToID = dbObj.ChargeToID;            
            this.Ispunchout = dbObj.Ispunchout;
            this.TotalCost = dbObj.TotalCost;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.Account_ClientLookupId = dbObj.Account_ClientLookupId;
            this.ChargeTo_Name = dbObj.ChargeTo_Name;
            this.PurchaseUOM = dbObj.PurchaseUOM;
            this.UOMConvRequired = dbObj.UOMConvRequired;
            this.UOMConversion = dbObj.UOMConversion;
            this.RequiredDate = dbObj.RequiredDate;
            this.PartCategoryMasterClientLookupId = dbObj.PartCategoryMasterClientLookupId;
        }
        public void UpdateFromDataBaseObjectExtendedPR(b_PurchaseRequestLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCost = dbObj.TotalCost;
            if (this.PartId == 0)
            {
                this.PartClientLookupId = "Non-Stock";
            }
            else
            {
                this.PartClientLookupId = dbObj.PartClientLookupId;
            }
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.Account_ClientLookupId = dbObj.Account_ClientLookupId;
            this.Part_Manufacturer = dbObj.Part_Manufacturer;
            this.Part_ManufacturerID = dbObj.Part_ManufacturerId;
        }
        public static List<PurchaseRequestLineItem> UpdateFromDatabaseObjectList_Latest(List<b_PurchaseRequestLineItem> dbObjs)
        {
            List<PurchaseRequestLineItem> result = new List<PurchaseRequestLineItem>();

            foreach (b_PurchaseRequestLineItem dbObj in dbObjs)
            {
                PurchaseRequestLineItem tmp = new PurchaseRequestLineItem();
                tmp.UpdateFromDataBaseObjectExtendedPR(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void PurchaseRequestLineItemRetrieveByPurchaseRequestLineItemId(DatabaseKey dbKey)
        {
            PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId trans = new PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };


            trans.PurchaseRequestLineItem = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //UpdateFromDatabaseObject(trans.WorkOrderTask);
            UpdateFromDataBaseObjectExtended(trans.PurchaseRequestLineItem);
        }

        public void PurchaseRequestLineItemRetrieveByPurchaseRequestLineItemIdV2(DatabaseKey dbKey)
        {
            PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId_V2 trans = new PurchaseRequestLineItem_RetrieveByPurchaseRequestLineItemId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };


            trans.PurchaseRequestLineItem = this.ToExtendedDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //UpdateFromDatabaseObject(trans.WorkOrderTask);
            UpdateFromDataBaseObjectExtendedV2(trans.PurchaseRequestLineItem);
        }

        //public void UpdateFromExtendedDatabaseObject(b_PurchaseOrderLineItem dbObj)
        //{
        //    this.UpdateFromDatabaseObject(dbObj);
        //    this.PartClientLookupId = string.IsNullOrEmpty(dbObj.PartClientLookupId) ? "" : dbObj.PartClientLookupId;
        //    this.TotalCost = dbObj.TotalCost;
        //    this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
        //    this.QuantityReceived = dbObj.QuantityReceived;
        //    this.QuantityToDate = dbObj.QuantityToDate;
        //    this.CurrentAverageCost = dbObj.CurrentAverageCost;
        //    this.CurrentAppliedCost = dbObj.CurrentAppliedCost;
        //    this.CurrentOnHandQuantity = dbObj.CurrentOnHandQuantity;
        //    this.StockType = dbObj.StockType;
        //    this.PartStoreRoomId = dbObj.PartStoreRoomId;
        //    this.QuantityBackOrdered = dbObj.QuantityBackOrdered;



        //}
        public b_PurchaseRequestLineItem ToExtendedDatabaseObject()
        {
            b_PurchaseRequestLineItem dbObj = this.ToDatabaseObject();
            dbObj.TotalCost = this.TotalCost;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
            dbObj.ChargeTo_Name = this.ChargeTo_Name;
            dbObj.PartCategoryMasterClientLookupId = this.PartCategoryMasterClientLookupId;
            return dbObj;
        }

        public void CreateWithValidation(DatabaseKey dbKey)
        {
            //m_validateClientLookupId = true;
            Validate<PurchaseRequestLineItem>(dbKey);

            if (IsValid)
            {
              PurchaseRequestLineItem_CreateWithReplication trans = new PurchaseRequestLineItem_CreateWithReplication();
                trans.PurchaseRequestLineItem = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
            }
        }

        public void CreateFromShoppingCart(DatabaseKey dbKey)
        {           
                PurchaseRequestLineItem_CreateFromShoppingCart trans = new PurchaseRequestLineItem_CreateFromShoppingCart();
                trans.PurchaseRequestLineItem = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
        }

        public void CreateFromOPunchOutShoppingCart(DatabaseKey dbKey)
        {
            PurchaseRequestLineItem_CreateFromPunchOutShoppingCart trans = new PurchaseRequestLineItem_CreateFromPunchOutShoppingCart();
            trans.PurchaseRequestLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            PurchaseRequestLineItem_ValidationTransaction trans = new PurchaseRequestLineItem_ValidationTransaction()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequestLineItem = ToExtendedDatabaseObject();
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

            return errors;
        }

        public void PRReOrderLineNumber(DatabaseKey dbKey)
        {

            PurchaseRequestLineItem_ReOrderLineNumber trans = new PurchaseRequestLineItem_ReOrderLineNumber()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequestLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);

        }

        public void UpdateByPKForeignKeys(DatabaseKey dbKey)
        {
            Validate<PurchaseRequestLineItem>(dbKey);

            if (IsValid)
            {
                PurchaseRequestLineItem_Update trans = new PurchaseRequestLineItem_Update()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };

                trans.dbKey = dbKey.ToTransDbKey();
                trans.PurchaseRequestLineItem = ToExtendedDatabaseObject();
                trans.Execute();

                UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
            }
        }
        //Som - 867
       
        public static List<PurchaseRequestLineItem> PurchaseRequestLineItemRetrieveByPurchaseRequestId(DatabaseKey dbKey, PurchaseRequestLineItem purchaseRequestlineitem)
        {
            List<PurchaseRequestLineItem> PurchaseRequestLineItemList = new List<PurchaseRequestLineItem>();

            Database.PurchaseRequestLineItem_RetrieveByPurchaseRequestId trans = new Database.PurchaseRequestLineItem_RetrieveByPurchaseRequestId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseRequestLineItem = purchaseRequestlineitem.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PurchaseRequestLineItem.UpdateFromDatabaseObjectList(trans.PurchaseRequestLineItemList);
        }
        //Som - 867
        public static List<PurchaseRequestLineItem> UpdateFromDatabaseObjectList(List<b_PurchaseRequestLineItem> dbObjs)
        {
            List<PurchaseRequestLineItem> result = new List<PurchaseRequestLineItem>();

            foreach (b_PurchaseRequestLineItem dbObj in dbObjs)
            {
                PurchaseRequestLineItem tmp = new PurchaseRequestLineItem();
                tmp.UpdateFromDataBaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }


        public List<PurchaseRequestLineItem> RetrieveByPurchaseOrderLineItemId(DatabaseKey dbKey)
        {
            PurchaseRequestLineItem_RetrieveByPurchaseOrderLineItem trans = new PurchaseRequestLineItem_RetrieveByPurchaseOrderLineItem()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequestLineItem = new b_PurchaseRequestLineItem
            {
                PurchaseRequestId = this.PurchaseRequestId
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return UpdateFromDatabaseObjectListItem(trans.PRLineItemList);
        }

        public static List<PurchaseRequestLineItem> UpdateFromDatabaseObjectListItem(List<b_PurchaseRequestLineItem> dbObjs)
        {
            List<PurchaseRequestLineItem> result = new List<PurchaseRequestLineItem>();

            foreach (b_PurchaseRequestLineItem dbObj in dbObjs)
            {
                PurchaseRequestLineItem tmp = new PurchaseRequestLineItem();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_PurchaseRequestLineItem dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.TotalCost = Convert.ToDecimal(dbObj.UnitCost * dbObj.OrderQuantity);
            this.PartClientLookupId = dbObj.PartClientLookupId;
        }

        #region V2-563
        public void CreateFromAdditionalCatalogItem(DatabaseKey dbKey)
        {
            PurchaseRequestLineItem_CreateFromAdditionalCatalogItem trans = new PurchaseRequestLineItem_CreateFromAdditionalCatalogItem();
            trans.PurchaseRequestLineItem = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
        }
        #endregion

        #region V2-693 SOMAX to SAP Purchase request export
        public static List<PurchaseRequestLineItem> RetrievePRLineItemByIdForExportSAP(DatabaseKey dbKey, PurchaseRequestLineItem purchaseorderlineitem)
        {
            //List<PurchaseRequestLineItem> PurchaseRequestLineItemList = new List<PurchaseRequestLineItem>();

            PurchaseRequestLineItem_RetrievePRLineItemByIdForExportSAP trans = new PurchaseRequestLineItem_RetrievePRLineItemByIdForExportSAP()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };

            trans.PurchaseRequestLineItem = purchaseorderlineitem.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PurchaseRequestLineItem.UpdateFromDatabaseObjectList_Latest(trans.PurchaseRequestLineItemList);

        }
        #endregion

        #region V2-738
        public void CreateFromShoppingCartForMultiStoreroom(DatabaseKey dbKey)
        {
            PurchaseRequestLineItem_CreateFromShoppingCartForMultiStoreroom trans = new PurchaseRequestLineItem_CreateFromShoppingCartForMultiStoreroom();
            trans.PurchaseRequestLineItem = this.ToDatabaseObject();
            trans.PurchaseRequestLineItem.StoreroomId = this.StoreroomId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
        }
        #endregion
        #region V2-894
        public List<PurchaseRequestLineItem> PurchaseRequestLineItemRetrievelookuplistByPartIdV2(DatabaseKey dbKey)
        {
            List<PurchaseRequestLineItem> PurchaseRequestLineItemList = new List<PurchaseRequestLineItem>();
            PurchaseRequestLineItem_RetrievelookuplistByPartId_V2 trans = new PurchaseRequestLineItem_RetrievelookuplistByPartId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseRequestLineItem = this.ToDatabaseObjectRetrievelookuplistByPartIdV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            //UpdateFromDatabaseObject(trans.WorkOrderTask);
            List<PurchaseRequestLineItem> result = new List<PurchaseRequestLineItem>();

            foreach (b_PurchaseRequestLineItem dbObj in trans.PurchaseRequestLineItemList)
            {
                PurchaseRequestLineItem tmp = new PurchaseRequestLineItem();
                tmp.UpdateFromDataBaseObjectLineItemOnOder(dbObj);
                result.Add(tmp);
            }
            return result;
           
        }
        public b_PurchaseRequestLineItem ToDatabaseObjectRetrievelookuplistByPartIdV2()
        {
            b_PurchaseRequestLineItem dbObj = new b_PurchaseRequestLineItem();
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.LineNumber = this.LineNumber;
            dbObj.OrderQuantity = this.OrderQuantity;
            dbObj.Name = this.Name;
            dbObj.PartId = this.PartId;
            dbObj.UnitofMeasure = this.UnitofMeasure;
            dbObj.CreateDate = this.CreateDate;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OrderBy = this.OrderBy;
            dbObj.NextRow = this.NextRow;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.TotalCount = this.TotalCount;
            return dbObj;
        }
     
        public void UpdateFromDataBaseObjectLineItemOnOder(b_PurchaseRequestLineItem dbObj)
        {
            this.ClientLookupId = dbObj.ClientLookupId;
            this.LineNumber = dbObj.LineNumber;
            this.OrderQuantity = dbObj.OrderQuantity;
            this.Name = dbObj.Name;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.CreateDate = dbObj.CreateDate;
            this.TotalCount = dbObj.TotalCount;

        }
        #endregion

        #region V2-945
        public void UpdateFromDatabaseObjectPRLineItemPrintExtended(b_PurchaseRequestLineItem dbObj, string Timezone)
        {
            this.ClientId = dbObj.ClientId;
            this.PurchaseRequestLineItemId = dbObj.PurchaseRequestLineItemId;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
            this.Description = dbObj.Description;
            if (dbObj.RequiredDate != null && dbObj.RequiredDate != DateTime.MinValue)
            {
                this.RequiredDate = dbObj.RequiredDate.ToUserTimeZone(Timezone);
            }
            else
            {
                this.RequiredDate = dbObj.RequiredDate;
            }
            this.LineNumber = dbObj.LineNumber;
            this.OrderQuantity = dbObj.OrderQuantity;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.UnitCost = dbObj.UnitCost;
            this.TotalCost = dbObj.TotalCost;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
            this.Account_ClientLookupId = dbObj.Account_ClientLookupId;
        }
        #endregion

        #region V2-1046
        public List<PurchaseRequestLineItem> PurchaseRequestLineItemRetrieveForConsolidate(DatabaseKey dbKey)
        {
            List<PurchaseRequestLineItem> PurchaseRequestLineItemList = new List<PurchaseRequestLineItem>();

            Database.PurchaseRequestLineItem_RetrieveForConsolidate trans = new Database.PurchaseRequestLineItem_RetrieveForConsolidate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseRequestLineItem = this.ToDateBaseObjectForConsolidateSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            foreach (b_PurchaseRequestLineItem dbObj in trans.PurchaseRequestLineItemList)
            {
                PurchaseRequestLineItem tmp = new PurchaseRequestLineItem();
                tmp.UpdateFromDataBaseObjectForConsolidateSearch(dbObj);
                PurchaseRequestLineItemList.Add(tmp);
            }
            return PurchaseRequestLineItemList;
        }
        public b_PurchaseRequestLineItem ToDateBaseObjectForConsolidateSearch()
        {
            b_PurchaseRequestLineItem dbObj = new b_PurchaseRequestLineItem();
            dbObj.ClientId = this.ClientId;
            dbObj.PurchaseRequestId = this.PurchaseRequestId;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.Description = this.Description;
            dbObj.VendorClientLookupId = this.VendorClientLookupId;
            dbObj.VendorName = this.VendorName;
            return dbObj;
        }
        public void UpdateFromDataBaseObjectForConsolidateSearch(b_PurchaseRequestLineItem dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PurchaseRequestLineItemId = dbObj.PurchaseRequestLineItemId;
            if (dbObj.PartClientLookupId == "")
            {
                this.PartClientLookupId = "Non-Stock";
            }
            else
            {
                this.PartClientLookupId = dbObj.PartClientLookupId;
            }
            this.Description = dbObj.Description;
            this.OrderQuantity = dbObj.OrderQuantity;
            this.UnitofMeasure = dbObj.UnitofMeasure;
            this.UnitCost = dbObj.UnitCost;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;
            this.VendorName = dbObj.VendorName;
            this.TotalCount = dbObj.TotalCount;
        }
        public void PRConsolidateProcess(DatabaseKey dbKey)
        {

            PurchaseRequestLineItem_PRConsolidate trans = new PurchaseRequestLineItem_PRConsolidate()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PurchaseRequestLineItem = this.ToDateBaseObjectForConsolidateProcess();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.PurchaseRequestLineItem);
        }
        public b_PurchaseRequestLineItem ToDateBaseObjectForConsolidateProcess()
        {
            b_PurchaseRequestLineItem dbObj = new b_PurchaseRequestLineItem();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PurchaseRequestId = this.PurchaseRequestId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.PRLineItemIds = this.PRLineItemIds;
            return dbObj;
        }
        #endregion

        #region V2-1063
        public List<PurchaseRequestLineItem> LineItemRetrieveForMaterialRequest(DatabaseKey dbKey)
        {
            List<PurchaseRequestLineItem> PurchaseRequestLineItemList = new List<PurchaseRequestLineItem>();

            PurchaseRequestLineItem_RetrieveForMaterialRequest trans = new PurchaseRequestLineItem_RetrieveForMaterialRequest()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PurchaseRequestLineItem = this.ToDateBaseObjectForMaterialRequest();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            foreach (b_PurchaseRequestLineItem dbObj in trans.PurchaseRequestLineItemList)
            {
                PurchaseRequestLineItem tmp = new PurchaseRequestLineItem();
                tmp.UpdateFromDataBaseObjectForMaterialRequest(dbObj);
                PurchaseRequestLineItemList.Add(tmp);
            }
            return PurchaseRequestLineItemList;
        }
        public b_PurchaseRequestLineItem ToDateBaseObjectForMaterialRequest()
        {
            b_PurchaseRequestLineItem dbObj = new b_PurchaseRequestLineItem();
            dbObj.ClientId = this.ClientId;
            dbObj.PurchaseRequestId = this.PurchaseRequestId;
            dbObj.OrderBy = this.OrderBy;
            dbObj.OrderbyColumn = this.OrderbyColumn;
            dbObj.OffSetVal = this.OffSetVal;
            dbObj.NextRow = this.NextRow;
            dbObj.Description = this.Description;
            dbObj.PartClientLookupId = this.PartClientLookupId;
            dbObj.WorkOrderClientLookupId = this.WorkOrderClientLookupId;
            return dbObj;
        }
        public void UpdateFromDataBaseObjectForMaterialRequest(b_PurchaseRequestLineItem dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.EstimatedCostsId = dbObj.EstimatedCostsId;
            if (dbObj.PartClientLookupId == "")
            {
                this.PartClientLookupId = "Non-Stock";
            }
            else
            {
                this.PartClientLookupId = dbObj.PartClientLookupId;
            }
            this.Description = dbObj.Description;
            this.OrderQuantity = dbObj.Quantity;
            this.UnitCostQuantity = dbObj.UnitCostQuantity;
            this.UnitCost = dbObj.UnitCost;
            this.WorkOrderClientLookupId = dbObj.WorkOrderClientLookupId;
            this.TotalCount = dbObj.TotalCount;
        }
        #endregion
    }
}
