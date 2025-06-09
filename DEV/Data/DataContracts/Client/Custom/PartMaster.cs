using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using Business.Localization;
using Common.Extensions;

using Database;
using Database.Business;

namespace DataContracts
{
    //, IStoredProcedureValidation
    public partial class PartMaster : DataContractBase, IStoredProcedureValidation
    {
        #region Part Master Search Page
        public Int64 ShoppingCartKey { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Createby { get; set; }
        public DateTime CreateDate { get; set; }
        public string Mode { get; set; }
        public string Inactive { get; set; }
        public string ValidateFor = string.Empty;
        public string CM_Description { get; set; }
        public decimal VI_UnitCost { get; set; }
        public string VI_PurchaseUOM { get; set; }
        public Int64 VI_VendorCatalogId { get; set; }
        public Int64 VI_VendorCatalogItemId { get; set; }
        public string VCI_PartNumber { get; set; }
        public string SearchCriteria { get; set; }
        public string CategoryDescription { get; set; }
        public long Siteid { get; set; }
        public long PartId { get; set; }
        public long VendorId { get; set; }
        public string PartClientLookupId { get; set; }
        public string PartDescription { get; set; }
        public long VendorCatalogItemId { get; set; }
        public string PartNumber { get; set; }
        public string CategoryMasterDescription { get; set; }
        public string VendorCatalogDescription { get; set; }
        public string VendorName { get; set; }
        public string VendorClientLookupId { get; set; }
        public string Qty { get; set; }
        public decimal QtyOnHand { get; set; }
        public string Price { get; set; }
        public bool CheckFlag { get; set; }
        public string CatalogType { get; set; }
        public decimal PartPrice { get; set; }
        public string Description { get; set; }
        public DateTime RequiredDate { get; set; }   //SOM-1526
        public void UpdatePartMasterDetails(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForClientlookupId&Manufacturer";
            Validate<PartMaster>(dbKey);
            if (IsValid)
            {
                PartMaster_Update trans = new PartMaster_Update();
                trans.PartMaster = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.PartMaster);
            }
        }
        public void ChangeClientLookupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForChangeClientlookupId";
            Validate<PartMaster>(dbKey);
            if (IsValid)
            {
                PartMaster_ChangeClientLookupId trans = new PartMaster_ChangeClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName
                };
                trans.PartMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                // The create procedure changed the Update Index.
                UpdateFromDatabaseObject(trans.PartMaster);
            }
        }
        public List<PartMaster> PartMaster_RetrieveAll_ByInactiveFlag(DatabaseKey dbKey, string Timezone,bool InactiveFlag)
        {
            PartMasterRetrieveAll_ByInactiveFlag trans = new PartMasterRetrieveAll_ByInactiveFlag()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                InactiveFlag= InactiveFlag
            };
            trans.UseTransaction = false;
            trans.PartMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartMaster> PartMasterList = new List<PartMaster>();
            foreach (b_PartMaster PartMaster in trans.PartMasterList)
            {
                PartMaster tmpPartMaster = new PartMaster();

                tmpPartMaster.UpdateFromDatabaseObjectForRetriveByInactiveFlag(PartMaster);
                PartMasterList.Add(tmpPartMaster);
            }
            return PartMasterList;
        }

        public List<PartMaster> PartMaster_RetrieveAll(DatabaseKey dbKey,string Timezone)
        {
            PartMaster_RetrieveAll trans = new PartMaster_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartMaster> PartMasterList = new List<PartMaster>();
            foreach (b_PartMaster PartMaster in trans.PartMasterList)
            {
                PartMaster tmpPartMaster = new PartMaster();

                tmpPartMaster.UpdateFromDatabaseObjectForRetriveByInactiveFlag(PartMaster);
                PartMasterList.Add(tmpPartMaster);
            }
            return PartMasterList;
        }

        public void UpdateFromDatabaseObjectForRetriveByInactiveFlag(b_PartMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.CategoryDescription = dbObj.CategoryDescription;
            //switch (dbObj.InactiveFlag)
            //{
            //    case true:
            //        Inactive = ActiveMethod.True;
            //        break;
            //    case false:
            //        Inactive = ActiveMethod.False;
            //        break;
            //    default:
            //        break;
            //}
        }

        public void RetrieveByPKForeignKeys(DatabaseKey dbKey)
        {

            PartMaster_RetrieveByForeignKeys trans = new PartMaster_RetrieveByForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.PartMaster);

        }
        private void UpdateFromDatabaseObjectExtended(b_PartMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.Createby = dbObj.Createby;
            this.CreateDate = dbObj.CreateDate;
            this.ModifyBy = dbObj.ModifyBy;
            this.ModifyDate = dbObj.ModifyDate;
            this.Description = dbObj.Description;
        }



        #endregion Part Master Search Page

        #region Add New Part Master
        public void Add_PartMaster(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForClientlookupId&Manufacturer";
            Validate<PartMaster>(dbKey);
            if (IsValid)
            {
                PartMaster_Create trans = new PartMaster_Create()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.PartMaster);
            }
        }

        #endregion
        public List<PartMaster> ShoppingCart_RetrieveBySearchCriteria(DatabaseKey dbKey)
        {
            ShoppingCart_RetrieveBySearchCriteria trans = new ShoppingCart_RetrieveBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                Siteid = this.Siteid,
                SearchCriteria = this.SearchCriteria
            };
            trans.PartMaster = this.ToDatabaseObject();
            trans.PartMaster.CheckFlag = this.CheckFlag;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.Execute();

            List<PartMaster> PartMasterList = new List<PartMaster>();
            foreach (b_PartMaster PartMaster in trans.PartMasterList)
            {
                PartMaster tmpPartMaster = new PartMaster();

                tmpPartMaster.UpdateFromDatabaseObjectForRetriveForShoppingCart(PartMaster);
                PartMasterList.Add(tmpPartMaster);
            }
            return PartMasterList;
        }
        public void UpdateFromDatabaseObjectForRetriveForShoppingCart(b_PartMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.ShoppingCartKey = dbObj.ShoppingCartKey;
            this.VendorCatalogDescription = dbObj.VendorCatalogDescription;
            this.VendorName = dbObj.VendorName;
            this.PartNumber = dbObj.PartNumber;
            this.CategoryMasterDescription = dbObj.CategoryMasterDescription;
            this.Category = dbObj.Category;
            this.Qty = "1";
            this.QtyOnHand = dbObj.QtyOnHand;
            this.Price = dbObj.VUnitCost.ToString("0.0000") + " " + dbObj.PurchaseUOM;
            this.PartId = dbObj.PartId;
            this.VendorId = dbObj.VendorId;
            this.VendorCatalogItemId = dbObj.VendorCatalogItemId;
            this.PartPrice = dbObj.PartPrice;
            this.CatalogType = dbObj.CatalogType;
            this.PartDescription = dbObj.PartDescription;
            this.PartClientLookupId = dbObj.PartClientLookupId;
            this.RequiredDate = DateTime.UtcNow.AddDays(7);                         //SOM-1526
        }

        #region Validation Methods
        public b_PartMaster ToDbObject()
        {
            b_PartMaster dbObj = new b_PartMaster();
            dbObj.ClientId = this.ClientId;
            dbObj.PartMasterId = this.PartMasterId;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.OEMPart = this.OEMPart;
            dbObj.EXPartId = this.EXPartId;
            dbObj.EXAltPartId1 = this.EXAltPartId1;
            dbObj.EXAltPartId2 = this.EXAltPartId2;
            dbObj.EXAltPartId3 = this.EXAltPartId3;
            dbObj.ExUniqueId = this.ExUniqueId;
            dbObj.InactiveFlag = this.InactiveFlag;
            dbObj.LongDescription = this.LongDescription;
            dbObj.Manufacturer = this.Manufacturer;
            dbObj.ManufacturerId = this.ManufacturerId;
            dbObj.ShortDescription = this.ShortDescription;
            dbObj.UnitCost = this.UnitCost;
            dbObj.UnitOfMeasure = this.UnitOfMeasure;
            dbObj.Category = this.Category;
            dbObj.UPCCode = this.UPCCode;
            dbObj.ImageURL = this.ImageURL;
            dbObj.Mode = this.Mode;
            return dbObj;
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateForClientlookupId&Manufacturer")
            {
                PartMaster_ValidateByClientlookupId trans = new PartMaster_ValidateByClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartMaster = this.ToDbObject();
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
            else if (ValidateFor == "ValidateForChangeClientlookupId")
            {

                PartMaster_ValidateForChangeClientlookupId trans = new PartMaster_ValidateForChangeClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartMaster = this.ToDatabaseObject();

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

        #endregion

        #region PartMaster_VendorCatalog
        public List<PartMaster> RetrieveVendorCatalogBySearchCriteria(DatabaseKey dbKey)
        {
            PartMaster_RetrieveVendorCatalogBySearchCriteria trans = new PartMaster_RetrieveVendorCatalogBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartMaster = this.ToDatabaseObjectbySearchCriteria();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartMaster> partmaster = new List<PartMaster>();
            foreach (b_PartMaster pm in trans.PartMasterList)
            {
                PartMaster tmpPM = new PartMaster()
                {
                    ClientId = pm.ClientId,
                    PartMasterId = pm.PartMasterId,
                    ClientLookupId = pm.ClientLookupId,
                    LongDescription = pm.LongDescription,
                    Category = pm.Category,
                    Manufacturer = pm.Manufacturer,
                    ManufacturerId = pm.ManufacturerId,
                    CM_Description = pm.CM_Description,
                    VI_UnitCost = pm.VI_UnitCost,
                    VI_PurchaseUOM = VI_PurchaseUOM,
                    VI_VendorCatalogId = pm.VI_VendorCatalogId,
                    VI_VendorCatalogItemId = pm.VI_VendorCatalogItemId,
                    VCI_PartNumber = pm.VCI_PartNumber,
                    VendorName = pm.VendorName,
                    VendorClientLookupId = pm.VendorClientLookupId

                };
                partmaster.Add(tmpPM);
            }

            return partmaster;
        }
        public b_PartMaster ToDatabaseObjectbySearchCriteria()
        {
            b_PartMaster dbObj = new b_PartMaster();
            dbObj.ClientId = this.ClientId;
            dbObj.SearchCriteria = this.SearchCriteria;
            return dbObj;
        }
        #endregion

        //SOM-1497
        public List<PartMaster> PartmasterReview_RetrieveBySearchCriteria(DatabaseKey dbKey)
        {
            PartMasterReview_RetrieveBySearchCriteria trans = new PartMasterReview_RetrieveBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
                Siteid = this.Siteid,
                SearchCriteria = this.SearchCriteria
            };
            trans.PartMaster = this.ToDatabaseObject();
            trans.PartMaster.CheckFlag = this.CheckFlag;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.Execute();

            List<PartMaster> PartMasterList = new List<PartMaster>();
            foreach (b_PartMaster PartMaster in trans.PartMasterList)
            {
                PartMaster tmpPartMaster = new PartMaster();

                tmpPartMaster.UpdateFromDatabaseObjectForRetriveForPartMasterReview(PartMaster);
                PartMasterList.Add(tmpPartMaster);
            }
            return PartMasterList;
        }



        //SOM-1497
        public void UpdateFromDatabaseObjectForRetriveForPartMasterReview(b_PartMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.CategoryMasterDescription = dbObj.CategoryMasterDescription;
           
        }
        public void PartMaster_RetrieveByPart(DatabaseKey dbKey)
        {

            PartMasterRetrieve_ByPart trans = new PartMasterRetrieve_ByPart()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.PartMaster);
        }
        public List<PartMaster> RetrieveAll(DatabaseKey dbKey)
        {
            List<PartMaster> result = new List<PartMaster>();
            Database.PartMaster_RetrieveAll trans = new Database.PartMaster_RetrieveAll
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            result = UpdateFromDatabaseObjectList(trans.PartMasterList);
            return result;
        }
        public List<PartMaster> Partmaster_RetrieveByImageURL(DatabaseKey dbKey)
        {
            Partmaster_RetrieveByImageURL trans = new Partmaster_RetrieveByImageURL()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.UseTransaction = false;
            trans.Execute();

            List<PartMaster> PartMasterList = new List<PartMaster>();
            foreach (b_PartMaster PartMaster in trans.PartMasterList)
            {
                PartMaster tmpPartMaster = new PartMaster();

                tmpPartMaster.UpdateFromDatabaseObject(PartMaster);
                PartMasterList.Add(tmpPartMaster);
            }
            return PartMasterList;
        }

        private List<PartMaster> UpdateFromDatabaseObjectList(List<b_PartMaster> partMasterList)
        {
            List<PartMaster> result = new List<PartMaster>();

            foreach (b_PartMaster dbObj in partMasterList)
            {
                PartMaster tmp = new PartMaster();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }
    }
}
