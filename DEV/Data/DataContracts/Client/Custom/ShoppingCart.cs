/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* All rights reserved. 
****************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== =======================================================
* 2015-Mar-21 SOM-585  Roger Lawton        Changed Parameters
* 2015-Mar-24 SOM-585  Roger Lawton        Localized the Status
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

namespace DataContracts
{
    public partial class ShoppingCart : DataContractBase, IStoredProcedureValidation
    {
        public decimal TotalCost { get; set; }
        public decimal LineItems { get; set; }
        public string CreateBy_Name { get; set; }
        public string ApprovedBy_Name { get; set; }
        public string ProcessedBy_Name { get; set; }
        public string CreatedBy { get; set; }
        public string Created { get; set; }
        public string StatusDrop { get; set; }
        public long PersonnelId { get; set; }
        public long SHOPPING_CART_ID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Flag { get; set; }
        public int CartsCreated { get; set; }
        public int CartItemsCreated { get; set; }
        public DateTime StartDate { get; set; }

        [DataMember]
    public bool CompleteAllTasks { get; set; }
    public List<ShoppingCart> ShoppingCart_RetrieveAll(DatabaseKey dbKey, string TimeZone)
        {
            ShoppingCart_RetrieveAll trans = new ShoppingCart_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            foreach (b_ShoppingCart ShoppingCart in trans.ShoppingCartList)
            {
                ShoppingCart tmpShoppingCart = new ShoppingCart();

                tmpShoppingCart.UpdateFromDatabaseObjectForRetriveByInactiveFlag(ShoppingCart, TimeZone);
                ShoppingCartList.Add(tmpShoppingCart);
            }
            return ShoppingCartList;
        }
        public void ShoppingCart_ConvertToPurchaseRequest(DatabaseKey dbKey)
        {
            ShoppingCart_ConvertToPurchaseRequest trans = new ShoppingCart_ConvertToPurchaseRequest()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ShoppingCart);

        }
        public List<ShoppingCart> ShoppingCart_RetrieveAllData(DatabaseKey dbKey, string TimeZone)
        {
            ShoppingCart_RetrieveAllData trans = new ShoppingCart_RetrieveAllData()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.UseTransaction = false;
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShoppingCart> ShoppingCartList = new List<ShoppingCart>();
            foreach (b_ShoppingCart ShoppingCart in trans.ShpingCartList)
            {
                ShoppingCart tmpShoppingCart = new ShoppingCart();

                tmpShoppingCart.UpdateFromDatabaseObjectForRetriveByInactiveFlag(ShoppingCart, TimeZone);
                ShoppingCartList.Add(tmpShoppingCart);
            }
            return ShoppingCartList;
        }
        public List<ShoppingCart> RetrieveForReviewWorkBenchSearch(DatabaseKey dbKey,string TimeZone)
        {
            ShoppingCart_RetrieveForReviewWorkBenchSearch trans = new ShoppingCart_RetrieveForReviewWorkBenchSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                UseTransaction = false
            };
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.ShoppingCart.Created = this.Created;
            trans.ShoppingCart.StatusDrop = this.StatusDrop;
            trans.ShoppingCart.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShoppingCart> PRlist = new List<ShoppingCart>();

            foreach (b_ShoppingCart ShoppingCart in trans.ShoppingCartList)
            {
                ShoppingCart tmpShoppingCartRequest = new ShoppingCart();
                tmpShoppingCartRequest.UpdateFromDatabaseObjectForRetriveAll(ShoppingCart, TimeZone);
                PRlist.Add(tmpShoppingCartRequest);
            }
            return PRlist;
        }
        public void RetrieveForNotification(DatabaseKey dbKey, string timezone)
        {
          ShoppingCart_RetrieveForNotification trans = new ShoppingCart_RetrieveForNotification();
          trans.ShoppingCart = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          UpdateFromDatabaseObject(trans.ShoppingCart);
          // Convert to user time zone
          // Per Support Ticket 3901
          // Add JIRA entry to track 
          this.Approved_Date = this.Approved_Date.ToUserTimeZone(timezone);
          this.Processed_Date = this.Processed_Date.ToUserTimeZone(timezone);
          this.TotalCost = trans.ShoppingCart.TotalCost;
          this.LineItems = trans.ShoppingCart.LineItems;
          this.CreateBy_Name = trans.ShoppingCart.CreateBy_Name;
          this.ApprovedBy_Name = trans.ShoppingCart.ApprovedBy_Name;
          this.ProcessedBy_Name = trans.ShoppingCart.ProcessedBy_Name;
    }
        public List<ShoppingCart> RetrieveWorkBenchForSearchNew(DatabaseKey dbKey, string TimeZone)
        {
            ShoppingCart_RetrieveAllWorkbenchSearch trans = new ShoppingCart_RetrieveAllWorkbenchSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                UseTransaction = false
            };
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.ShoppingCart.Created = this.Created;
            trans.ShoppingCart.StatusDrop = this.StatusDrop;
            // RKL 2016-11-07 - PersonnelId - Not UserInfoId
            trans.ShoppingCart.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<ShoppingCart> PRlist = new List<ShoppingCart>();

            foreach (b_ShoppingCart ShoppingCart in trans.ShoppingCartList)
            {
                ShoppingCart tmpShoppingCartRequest = new ShoppingCart();
                // RKL - Moved to the .UpdateFromDatabaseObjectForRetriveAll method
                tmpShoppingCartRequest.UpdateFromDatabaseObjectForRetriveAll(ShoppingCart, TimeZone);
                PRlist.Add(tmpShoppingCartRequest);
            }
            return PRlist;
        }

        public void RetrieveByPK(DatabaseKey dbKey, string TimeZone)
        {
            ShoppingCart_RetrieveByPK trans = new ShoppingCart_RetrieveByPK();
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectByPK(trans.ShoppingCart, TimeZone);
        }

        public void UpdateFromDatabaseObjectByPK(b_ShoppingCart shoppingCart, string TimeZone)
        {
            this.UpdateFromDatabaseObject(shoppingCart);
            if (shoppingCart.CreateDate != null && shoppingCart.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = shoppingCart.CreateDate.ConvertFromUTCToUser(TimeZone);
            }
            else
            {
                this.CreateDate = shoppingCart.CreateDate;
            }
        }

        #region validation
        public void ValidateAdd(DatabaseKey dbKey)
        {
            Validate<ShoppingCart>(dbKey);
        }
        #endregion
        public void UpdateByForeignKeys(DatabaseKey dbKey)
        {
            ShoppingCart_UpdateByPKForeignKeys trans = new ShoppingCart_UpdateByPKForeignKeys();
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.ShoppingCart.Flag = this.Flag;
            trans.ShoppingCart.PersonnelId = this.PersonnelId;
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateByDatabase(trans.ShoppingCart);
        }
        public void UpdateByDatabase(b_ShoppingCart dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.ShoppingCartId = dbObj.ShoppingCartId;
            this.SiteId = dbObj.SiteId;
            this.ApprovedBy_PersonnelId = dbObj.ApprovedBy_PersonnelId;
            this.Approved_Date = dbObj.Approved_Date;
            this.Processed_Date = dbObj.Processed_Date;
            this.ProcessBy_PersonnelId = dbObj.ProcessBy_PersonnelId;
            this.Status = dbObj.Status;
            this.UpdateIndex = dbObj.UpdateIndex;
        }
        private void UpdateFromDatabaseObjectForRetriveAll(b_ShoppingCart dbObj, string TimeZone)
        {

            this.UpdateFromDatabaseObject(dbObj);
            this.Reason = dbObj.Reason;
            this.CreatedBy = dbObj.CreatedBy;
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ConvertFromUTCToUser(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            this.TotalCost = dbObj.TotalCost;
        }
        public void UpdateFromDatabaseObjectForRetriveByInactiveFlag(b_ShoppingCart dbObj, string TimeZone)
        {
            SHOPPING_CART_ID = dbObj.ShoppingCartId;

            this.UpdateFromDatabaseObject(dbObj);
            this.CreateBy_Name = dbObj.CreateBy_Name;
            this.ApprovedBy_Name = dbObj.ApprovedBy_Name;
            this.ProcessedBy_Name = dbObj.ProcessedBy_Name;
            this.LineItems = dbObj.LineItems;
            this.TotalCost = dbObj.TotalCost;
            // - Convert the create date to the user's time zone
            if (dbObj.CreateDate != null && dbObj.CreateDate != DateTime.MinValue)
            {
                this.CreateDate = dbObj.CreateDate.ConvertFromUTCToUser(TimeZone);
            }
            else
            {
                this.CreateDate = dbObj.CreateDate;
            }
            // - Convert the Approved_Date to the user's time zone
            if (dbObj.Approved_Date != null && dbObj.Approved_Date != DateTime.MinValue)
            {
                this.Approved_Date = dbObj.Approved_Date.ToUserTimeZone(TimeZone);
            }
            else
            {
                this.Approved_Date = dbObj.Approved_Date;
            }

        }
  
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

           
                ShoppingCart_Validate_ByClientlookupIdAndPersonnelId trans = new ShoppingCart_Validate_ByClientlookupIdAndPersonnelId
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                
                trans.ShoppingCart = this.ToDatabaseObject();
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

        public void AutoGeneration(DatabaseKey dbKey)
        {
            ShoppingCart_AutoGeneration trans = new ShoppingCart_AutoGeneration();
            trans.ShoppingCart = this.ToDatabaseObject();
            trans.ShoppingCart.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.ShoppingCart);
            this.CartsCreated = trans.ShoppingCart.CartsCreated;
            this.CartItemsCreated = trans.ShoppingCart.CartItemsCreated;
        }
      
        //===============
    }

}
