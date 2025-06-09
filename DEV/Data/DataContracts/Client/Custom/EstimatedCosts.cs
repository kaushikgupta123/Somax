/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2016 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2016-Sep-20 SOM-1090 Roger Lawton       Change the TotalCost calculation for crafts
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database;
using Database.Business;
using Database.Client.Custom.Transactions;

namespace DataContracts
{

    public partial class EstimatedCosts : DataContractBase, IStoredProcedureValidation
    {
        public string ClientLookupId { get; set; }
        public string VendorClientLookupId { get; set; }
        public decimal TotalPartCost { get; set; }
        public decimal TotalLaborHours { get; set; }
        public decimal TotalCraftCost { get; set; }
        public decimal TotalExternalCost { get; set; }
        public decimal TotalInternalCost { get; set; }
        public List<b_EstimatedCosts> EstimateListList { get; set; }

        public string Unit { get; set; }
        public string AccountClientLookupId { get; set; }

        public string PartCategoryClientLookupId { get; set; }

        public decimal TotalCost { get; set; }
        public decimal TotalSummeryCost { get; set; }
        public string PartClientLookupId { get; set; }


        #region  Properties

        string ValidateFor = string.Empty;

        #endregion
        public static List<EstimatedCosts> UpdateFromDatabaseObjectList(List<b_EstimatedCosts> dbObjs)
        {
            List<EstimatedCosts> result = new List<EstimatedCosts>();

            foreach (b_EstimatedCosts dbObj in dbObjs)
            {
                EstimatedCosts tmp = new EstimatedCosts();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                // SOM-1090
                if (tmp.Category.ToUpper() == "CRAFT")
                {
                    tmp.TotalCost = Convert.ToDecimal(dbObj.UnitCost * dbObj.Quantity * dbObj.Duration);
                }
                else
                {
                    tmp.TotalCost = Convert.ToDecimal(dbObj.UnitCost * dbObj.Quantity);
                }
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_EstimatedCosts dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ClientLookupId = string.IsNullOrEmpty(dbObj.ClientLookupId) ? "" : dbObj.ClientLookupId;
            this.VendorClientLookupId = string.IsNullOrEmpty(dbObj.VendorClientLookupId) ? "" : dbObj.VendorClientLookupId;
            this.TotalPartCost = dbObj.TotalPartCost;
            this.TotalLaborHours = dbObj.TotalLaborHours;
            this.TotalCraftCost = dbObj.TotalCraftCost;
            this.TotalExternalCost = dbObj.TotalExternalCost;
            this.TotalInternalCost = dbObj.TotalInternalCost;
            this.TotalPurchaseCost = dbObj.TotalPurchaseCost;
            this.TotalSummeryCost = this.TotalPartCost + this.TotalCraftCost + this.TotalExternalCost + this.TotalInternalCost + this.TotalPurchaseCost;
            //SOM-1210
            //this.TotalSummeryCost = this.TotalPartCost + this.TotalCraftCost + this.TotalExternalCost + this.TotalInternalCost + this.TotalLaborHours;
        }


        public List<b_EstimatedCosts> ToDatabaseObjectList()
        {
            List<b_EstimatedCosts> dbObj = new List<b_EstimatedCosts>();
            dbObj = this.EstimateListList;
            return dbObj;
        }


        public List<EstimatedCosts> RetriveByObjectId(DatabaseKey dbKey)
        {
            EstimatedCosts_RetrieveByObjectId trans = new EstimatedCosts_RetrieveByObjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.EstimatedCosts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return EstimatedCosts.UpdateFromDatabaseObjectList(trans.EstimatatedCostsList);
        }

        public List<EstimatedCosts> SummeryRetriveByObjectId(DatabaseKey dbKey)
        {
            EstimatedCosts_SummeryRetrieveByObjectId trans = new EstimatedCosts_SummeryRetrieveByObjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.EstimatedCosts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return EstimatedCosts.UpdateFromDatabaseObjectList(trans.EstimatatedCostsList);
        }






        /*ADDED BY INDUSNET TECHNOLOGIES*/
        public List<EstimatedCosts> RetrieveForPrevMaintFromDatabase(DatabaseKey dbKey, long PrevMaintMasterId, string category)
        {
            EstimatedCosts_RetrieveForPrevMaintFromDatabase trans = new EstimatedCosts_RetrieveForPrevMaintFromDatabase()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                Client = dbKey.Personnel.ClientId,
                PrevMaintMasterId = PrevMaintMasterId,
                Category = category
            };

            trans.EstimatedCosts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return EstimatedCosts.UpdateFromDatabaseObjectList(trans.EstimatatedCostsList);
        }


        public EstimatedCosts EstimatedCosts_Retrieve(DatabaseKey dbKey, EstimatedCosts EstimatedCosts)
        {
            EstimatedCosts_Retrieve trans = new EstimatedCosts_Retrieve();
            trans.EstimatedCosts = new b_EstimatedCosts();
            trans.EstimatedCosts.EstimatedCostsId = EstimatedCosts.EstimatedCostsId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.EstimatedCosts);
            return this;
        }


        public decimal TotalPurchaseCost { get; set; }

        public void CheckDuplicateCraftForAdd(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateDuplicateAdd";
            Validate<EstimatedCosts>(dbKey);
        }
        public void CheckDuplicateCraftForUpdate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateDuplicateUpdate";
            Validate<EstimatedCosts>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateDuplicateAdd")
            {
                EstimatedCost_ValidateAddTransaction ptrans = new EstimatedCost_ValidateAddTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.EstimatedCosts = this.ToDatabaseObject();
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
            if (ValidateFor == "ValidateDuplicateUpdate")
            {
                EstimatedCost_ValidateUpdateTransaction trans = new EstimatedCost_ValidateUpdateTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.EstimatedCosts = this.ToDatabaseObject();
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
            return errors;
        }

        #region V2-691

        public static List<EstimatedCosts> EstimatedCostsRetrieveByObjectId_ForChild(DatabaseKey dbKey, EstimatedCosts estimatedCostsList)
        {
            List<EstimatedCosts> EstimatedCostsList = new List<EstimatedCosts>();

            EstimatedCosts_RetrieveForChildGridByObjectId trans = new EstimatedCosts_RetrieveForChildGridByObjectId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName

            };

            trans.EstimatedCosts = estimatedCostsList.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return EstimatedCosts.UpdateFromDatabaseObjectList_Latest(trans.EstimatedCostsList);

        }

        public static List<EstimatedCosts> UpdateFromDatabaseObjectList_Latest(List<b_EstimatedCosts> dbObjs)
        {
            List<EstimatedCosts> result = new List<EstimatedCosts>();

            foreach (b_EstimatedCosts dbObj in dbObjs)
            {
                EstimatedCosts tmp = new EstimatedCosts();
                tmp.UpdateFromDataBaseObjectExtendedMR(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDataBaseObjectExtendedMR(b_EstimatedCosts dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.EstimatedCostsId = dbObj.EstimatedCostsId;
            this.ObjectId = dbObj.ObjectId;
            this.TotalCost = dbObj.TotalCost;
            this.CategoryId = dbObj.CategoryId;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.UnitCost = dbObj.UnitCost;
            this.Quantity = dbObj.Quantity;
            this.Unit = dbObj.Unit;
            this.AccountClientLookupId = dbObj.AccountClientLookupId;
            this.VendorClientLookupId = dbObj.VClientLookupId;
            this.PartCategoryClientLookupId = dbObj.PartCategoryClientLookupId;
            this.AccountId = dbObj.AccountId;
            this.UnitOfMeasure = dbObj.UnitOfMeasure;
            this.VendorId = dbObj.VendorId;
            this.UNSPSC = dbObj.PartCategoryMasterId;
            this.Description = dbObj.Description;
            this.Status = dbObj.Status;
            this.PurchaseRequestId = dbObj.PurchaseRequestId;
        }
        #endregion

        #region V2-690
        public void CreateFromShoppingCart(DatabaseKey dbKey)
        {
            EstimatedCosts_CreateFromShoppingCart trans = new EstimatedCosts_CreateFromShoppingCart();
            trans.EstimatedCosts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.EstimatedCosts);
        }

        #endregion

        #region V2-732
        public void CreateFromShoppingCartMultiStoreroom(DatabaseKey dbKey)
        {
            EstimatedCosts_CreateFromShoppingCartMultiStoreroom trans = new EstimatedCosts_CreateFromShoppingCartMultiStoreroom();
            trans.EstimatedCosts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.EstimatedCosts);
        }

        #endregion

        #region V2-1204
        public List<EstimatedCosts> RetriveByObjectId_V2(DatabaseKey dbKey)
        {
            EstimatedCosts_RetrieveByObjectId_V2 trans = new EstimatedCosts_RetrieveByObjectId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.EstimatedCosts = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return EstimatedCosts.UpdateFromDatabaseObjectList(trans.EstimatatedCostsList);
        }
        #endregion
    }
}
