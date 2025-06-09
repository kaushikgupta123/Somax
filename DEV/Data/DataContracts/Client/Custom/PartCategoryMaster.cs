/*
***************************************************************************************************
* PROPRIETARY DATA 
***************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
***************************************************************************************************
* Copyright (c) 2011 by SOMAX Inc.
* All rights reserved. 
***************************************************************************************************
* Date        Task ID   Person             Description
* =========== ======== =================== ========================================================
* 2011-Dec-09 20110019 Roger Lawton        Added ClientLookupId to search results
* 2011-Dec-14 20110039 Roger Lawton        Added Lookuplist validation
* 2014-Aug-10 SOM-280  Roger Lawton        Modified UpdateFromDataObjectList to include 
*                                          LaborAccountClientLookupId
* 2015-Mar-03 SOM-590  Roger Lawton        Removed validation on columns we do not support
* 2015-Sep-14 SOM-805  Roger Lawton        Location - Show Location.ClientLookupId if FACILITIES
***************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Data;

using Data.Database;
using Data.Database.Business;
using DataContracts;
using Database.Business;

namespace Data.DataContracts
{
    public partial class PartCategoryMaster : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string Inactive { get; set; }
        string ValidateFor = string.Empty;

        //V2-666
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public int totalCount { get; set; }
        public string SearchText{ get; set; }//V2-1068
        #endregion

        public List<PartCategoryMaster> RetrieveAll(DatabaseKey dbKey)
        {
            PartCategoryMaster_RetrieveAll trans = new PartCategoryMaster_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartCategoryMaster> PartCategoryMasterList = new List<PartCategoryMaster>();
            foreach (b_PartCategoryMaster PartCategoryMaster in trans.PartCategoryMasterList)
            {
                PartCategoryMaster tmpPartCategoryMaster = new PartCategoryMaster();

                tmpPartCategoryMaster.UpdateFromDatabaseObject(PartCategoryMaster);
                PartCategoryMasterList.Add(tmpPartCategoryMaster);
            }
            return PartCategoryMasterList;

        }
        public void Validate(DatabaseKey dbKey)
        {
            Validate<PartCategoryMaster>(dbKey);
        }
        public void Add(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateForClientlookupId&PartCategory";
            Validate<PartCategoryMaster>(dbKey);
            if (IsValid)
            {
                PartCategoryMaster_Create trans = new PartCategoryMaster_Create()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartCategoryMaster = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.PartCategoryMaster);
            }

        }

        #region Validation Methods
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();
            if (ValidateFor == "ValidateForClientlookupId&PartCategory")
            {
                PartCategoryMaster_ValidateByClientlookupId trans = new PartCategoryMaster_ValidateByClientlookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartCategoryMaster = this.ToDatabaseObject();

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

        public List<PartCategoryMaster> PartCategoryMaster_RetrieveByInactiveFlag(DatabaseKey dbKey)
        {
            PartCategoryMaster_RetrieveByInactiveFlag trans = new PartCategoryMaster_RetrieveByInactiveFlag()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartCategoryMaster = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartCategoryMaster> partCategoryMasterList = new List<PartCategoryMaster>();
            foreach (b_PartCategoryMaster PartCategoryMaster in trans.PartCategoryMasterList)
            {
                PartCategoryMaster tmppartCategoryMasterList = new PartCategoryMaster();

                tmppartCategoryMasterList.UpdateFromDatabaseObjectForRetriveByInactiveFlag(PartCategoryMaster);
                partCategoryMasterList.Add(tmppartCategoryMasterList);
            }
            return partCategoryMasterList;
        }
        public void UpdateFromDatabaseObjectForRetriveByInactiveFlag(b_PartCategoryMaster dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            /*switch (dbObj.InactiveFlag)
            {
                case true:
                    Inactive = loc.ActiveMethod.True;
                    break;
                case false:
                    Inactive = loc.ActiveMethod.False;
                    break;
                default:
                    break;
            }*/
        }

        public List<PartCategoryMaster> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            PartCategoryMaster_RetrieveChunkSearch trans = new PartCategoryMaster_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PartCategoryMaster = this.ToDatabaseObject();
            trans.PartCategoryMaster.orderbyColumn = this.orderbyColumn;
            trans.PartCategoryMaster.orderBy = this.orderBy;
            trans.PartCategoryMaster.offset1 = this.offset1;
            trans.PartCategoryMaster.nextrow = this.nextrow;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartCategoryMaster> Categorylist = new List<PartCategoryMaster>();

            foreach (b_PartCategoryMaster v in trans.CategoryList)
            {
                PartCategoryMaster tmpVendor = new PartCategoryMaster();
                tmpVendor.UpdateFromDatabaseObject(v);
                tmpVendor.totalCount = v.totalCount;
                Categorylist.Add(tmpVendor);
            }
            return Categorylist;

        }

        #region V2-717
        public List<PartCategoryMaster> RetrievelookuplistChunkSearch(DatabaseKey dbKey)
        {
            PartCategoryMaster_RetrieveLookupListChunkSearch trans = new PartCategoryMaster_RetrieveLookupListChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PartCategoryMaster = this.ToDatabaseObject();
            trans.PartCategoryMaster.orderbyColumn = this.orderbyColumn;
            trans.PartCategoryMaster.orderBy = this.orderBy;
            trans.PartCategoryMaster.offset1 = this.offset1;
            trans.PartCategoryMaster.nextrow = this.nextrow;
            trans.PartCategoryMaster.SearchText=this.SearchText;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartCategoryMaster> Categorylist = new List<PartCategoryMaster>();

            foreach (b_PartCategoryMaster v in trans.CategoryList)
            {
                PartCategoryMaster tmpPCM = new PartCategoryMaster();
                tmpPCM.UpdateFromDatabaseObjectRetrieveLookupListChunkSearch(v);
                tmpPCM.totalCount = v.totalCount;
                Categorylist.Add(tmpPCM);
            }
            return Categorylist;

        }
        public void UpdateFromDatabaseObjectRetrieveLookupListChunkSearch(b_PartCategoryMaster dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.PartCategoryMasterId = dbObj.PartCategoryMasterId;
            this.ClientLookupId = dbObj.ClientLookupId;
            this.Description = dbObj.Description;
            this.SearchText = dbObj.SearchText;

            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion

    }
}
