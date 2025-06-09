/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2014 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== =========================================================
* 2014-Sep-15 SOM-106  Roger Lawton       Added RetrieveForSearch, CUstomQueryDisplayId
****************************************************************************************************
*/


using Database;
using Database.Business;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DataContracts
{
    public partial class Vendor : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public string FilterText { get; set; }
        public int FilterStartIndex { get; set; }
        public int FilterEndIndex { get; set; }
        private bool m_validateClientLookupId;
        private bool m_validateClientLookupIdSiteId;
        public int CustomQueryDisplayId { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        string ValidateFor = string.Empty;
        public string Flag { get; set; }

        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public Int32 offset1 { get; set; }
        public Int32 nextrow { get; set; }
        public string SearchText { get; set; }
        public int totalCount { get; set; }
        public UtilityAdd utilityAdd { get; set; }

        //V2-375
        public string External { get; set; }
        public List<Vendor> listOfVendor { get; set; }

        #endregion
        public static List<Vendor> UpdateFromDatabaseObjectList(List<b_Vendor> dbObjs)
        {
            List<Vendor> result = new List<Vendor>();

            foreach (b_Vendor dbObj in dbObjs)
            {
                Vendor tmp = new Vendor();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        //
        public void CreateWithValidation(DatabaseKey dbKey)
        {
            //m_validateClientLookupId = true;
            ValidateFor = "ValidateClientIdVendorAndMaster";
            Validate<Vendor>(dbKey);

            if (IsValid)
            {
                Vendor_Create trans = new Vendor_Create();
                trans.Vendor = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Vendor);
            }
        }
        public void ValidateClientLookUpId(DatabaseKey dbKey)
        {
            m_validateClientLookupIdSiteId = true;
            Validate<Vendor>(dbKey);
        }   
        

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            if (m_validateClientLookupId)
            {
                ProcedureVendorCreateValidationTransaction trans = new ProcedureVendorCreateValidationTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Vendor = this.ToDatabaseObject();
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
            if (m_validateClientLookupIdSiteId)
            {
                VendorNotExistValidationTransaction trans = new VendorNotExistValidationTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.Vendor = this.ToDatabaseObject();
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
            if (ValidateFor == "ValidateClientLookUpId")
            {
                ValidateClientLookupIdTransaction vtrans=new ValidateClientLookupIdTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                vtrans.Vendor = this.ToDatabaseObject();
                vtrans.dbKey = dbKey.ToTransDbKey();
                vtrans.Execute();
                if (vtrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in vtrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }

            if (ValidateFor == "CheckIfInactivateorActivate")
            {
                ValidateByInactivateorActivate vtrans = new ValidateByInactivateorActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                vtrans.Vendor = this.ToDatabaseObject();
                vtrans.Vendor.Flag = this.Flag;
                vtrans.dbKey = dbKey.ToTransDbKey();
                vtrans.Execute();
                if (vtrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in vtrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            if (ValidateFor=="ValidateClientIdVendorAndMaster")
            {
                ValidateClientIdVendorAndMaster vtrans = new ValidateClientIdVendorAndMaster()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                vtrans.Vendor = this.ToDatabaseObject();
                vtrans.Vendor.Flag = this.Flag;
                vtrans.dbKey = dbKey.ToTransDbKey();
                vtrans.Execute();
                if (vtrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in vtrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errors.Add(tmp);
                    }
                }
            }
            return errors;
        }

        //
        public void CheckVendorIsInactivateorActivate(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfInactivateorActivate";
            Validate<Vendor>(dbKey);
        }
        public List<Vendor> RetrieveClientLookupIdBySearchCriteria(DatabaseKey dbKey)
        {
            Vendor_RetrieveClientLookupIdBySearchCriteria trans = new Vendor_RetrieveClientLookupIdBySearchCriteria()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Vendor = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Vendor> vendorList = new List<Vendor>();
            foreach (b_Vendor vendor in trans.VendorList)
            {
                Vendor tmpParts = new Vendor()
                {
                    VendorId = vendor.VendorId,
                    ClientLookupId = vendor.ClientLookupId
                };
                vendorList.Add(tmpParts);
            }

            return vendorList;
        }
        public List<Vendor> RetrieveForLookupList(DatabaseKey dbKey)
        {                
            Vendor_RetrieveForLookupList trans = new Vendor_RetrieveForLookupList()
            
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Vendor = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            return (UpdateFromDatabaseObjectlist(trans.VendorList));
        }
        public List<Vendor> UpdateFromDatabaseObjectlist(List<b_Vendor> dbObjlist)
        {
            List<Vendor> temp = new List<Vendor>();

            Vendor objPer;

            foreach (b_Vendor per in dbObjlist)
            {
                objPer = new Vendor();
                objPer.UpdateFromDatabaseObject(per);
                temp.Add(objPer);
            }

            return (temp);
        }

        public List<Vendor> RetrieveAll(DatabaseKey dbKey)
        {
            Vendor_RetrieveAll_V2 trans = new Vendor_RetrieveAll_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Vendor> VendorList = new List<Vendor>();
            foreach (b_Vendor Vendor in trans.VendorList)
            {
                Vendor tmpVendor = new Vendor();

                tmpVendor.UpdateFromDatabaseObject(Vendor);
                VendorList.Add(tmpVendor);
            }
            return VendorList;

        }

        public List<Vendor> RetrieveForSearch(DatabaseKey dbKey)
        {
            Vendor_RetrieveForSearch trans = new Vendor_RetrieveForSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Vendor = this.ToDatabaseObject();
            trans.Vendor.CustomQueryDisplayId = this.CustomQueryDisplayId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<Vendor> vendorlist = new List<Vendor>();
            foreach (b_Vendor vendor in trans.VendorList)
            {
                Vendor tmpvendor = new Vendor();
                tmpvendor.UpdateFromDatabaseObject(vendor);
                vendorlist.Add(tmpvendor);
            }
            return vendorlist;
        }

        // These are TEMPORARY METHODS UNTIL WE REPLACE THE VENDOR LOOKUP WITH THE LABELED TABLE SEARCH GRID
        public List<Vendor> Vendor_RetrieveForLookup5000(DatabaseKey dbKey)
        {
          Vendor_RetrieveForLookup trans = new Vendor_RetrieveForLookup()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName,
          };
          trans.Vendor = this.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();

          return UpdateFromDatabaseObjectList5000(trans.VendorList);
        }
        public static List<Vendor> UpdateFromDatabaseObjectList5000(List<b_Vendor> dbObjs)
        {
          List<Vendor> result = new List<Vendor>();

          int limit = 0;
          foreach (b_Vendor dbObj in dbObjs)
          {          
            Vendor tmp = new Vendor();
            tmp.UpdateFromDatabaseObject(dbObj);
            result.Add(tmp);
            if(++limit > 5000) break;          
          }
          return result;
        }
        public List<Vendor> Vendor_RetrieveForLookup(DatabaseKey dbKey)
        {
            Vendor_RetrieveForLookup trans = new Vendor_RetrieveForLookup()
              {
                  CallerUserInfoId = dbKey.User.UserInfoId,
                  CallerUserName = dbKey.UserName,
              };
            trans.Vendor = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return UpdateFromDatabaseObjectList(trans.VendorList);
        }

        /***********************Added By Indusnet Technologies************************************/

        public List<Vendor> RetrieveVendorListByFilterText(DatabaseKey dbKey)
        {
            RetrieveVendorListByFilterText trans = new RetrieveVendorListByFilterText()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Vendor = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.RetVendorList));

        }
        public void UpdateFromDatabaseObjectExtended(b_Vendor dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.FilterText = dbObj.FilterText;
            this.FilterStartIndex = dbObj.FilterStartIndex;
            this.FilterEndIndex = dbObj.FilterEndIndex;
        }
        public b_Vendor ToDatabaseObjectExtended()
        {
            b_Vendor dbObj = this.ToDatabaseObject();
            dbObj.FilterText = this.FilterText;
            dbObj.FilterStartIndex = this.FilterStartIndex;
            dbObj.FilterEndIndex = this.FilterEndIndex;

            return dbObj;
        }

     
        public void ChangeClientLookupId(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateClientIdVendorAndMaster";
            Validate<Vendor>(dbKey);

            if (IsValid)
            {
                //Vendor_Update trans = new Vendor_Update();
                Vendor_ChangeClientLookupId trans = new Vendor_ChangeClientLookupId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.User.UserName
                };
                trans.Vendor = this.ToDatabaseObject();
                trans.ChangeLog = GetChangeLogObject(dbKey);
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.Vendor);
            }
        }

        /***********************Added By Indusnet Technologies************************************/

        #region Transactions



        #endregion

        #region Search Parameter Lists
        public Dictionary<string, List<KeyValuePair<string, string>>> SearchCriteria { get; set; }



        //------SOM-788------//
        //public void RetrieveCreateModifyDate(DatabaseKey dbKey)
        //{
        //    Vendor_RetrieveCreateModifyDate trans = new Vendor_RetrieveCreateModifyDate()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName
        //    };

        //    trans.Vendor = this.ToDatabaseObject();
        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();

        //    UpdateFromDatabaseObject(trans.Vendor);
        //    this.CreateBy = trans.Vendor.CreateBy;
        //    this.CreateDate = trans.Vendor.CreateDate;
        //    this.ModifyBy = trans.Vendor.ModifyBy;
        //    this.ModifyDate = trans.Vendor.ModifyDate;
        //}

        #endregion

        //-----------------From API Calls-SOM-918-------------------------------------------------
        public List<Vendor> RetrieveBySiteIdAndClientLookUpId(DatabaseKey dbKey)
        {
            Vendor_RetrieveBySiteIdAndClientLookUpId trans = new Vendor_RetrieveBySiteIdAndClientLookUpId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Vendor = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return (UpdateFromDatabaseObjectList(trans.VendorList));

        }

        public List<Vendor> RetrieveChunkSearch(DatabaseKey dbKey)
        {
            Vendor_RetrieveChunkSearch trans = new Vendor_RetrieveChunkSearch()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Vendor = this.ToDatabaseObject();
            trans.Vendor.orderbyColumn = this.orderbyColumn;
            trans.Vendor.orderBy = this.orderBy;
            trans.Vendor.offset1 = this.offset1;
            trans.Vendor.nextrow = this.nextrow;
            trans.Vendor.SearchText = this.SearchText;
            trans.Vendor.CustomQueryDisplayId = this.CustomQueryDisplayId;
            trans.Vendor.External = this.External;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();            

            List<Vendor>Vendorlist = new List<Vendor>();

            foreach (b_Vendor v in trans.VendorList)
            {
                Vendor tmpVendor = new Vendor();
                tmpVendor.UpdateFromDatabaseObject(v);
                tmpVendor.totalCount = v.totalCount;
                Vendorlist.Add(tmpVendor);
            }
            return Vendorlist;

        }

        public b_Vendor ToDateBaseObjectForVendorLookuplistChunkSearch()
        {
            b_Vendor dbObj = this.ToDatabaseObject();


            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientLookupId = this.ClientLookupId;
            dbObj.Name = this.Name;
            dbObj.SearchText = this.SearchText; //V2-1068
            return dbObj;
        }
        public List<Vendor> GetAllVendorLookupListV2(DatabaseKey dbKey, string TimeZone)
        {
            Vendor_RetrieveChunkSearchLookupListV2 trans = new Vendor_RetrieveChunkSearchLookupListV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Vendor = this.ToDateBaseObjectForVendorLookuplistChunkSearch();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            this.listOfVendor = new List<Vendor>();

            List<Vendor> Vendortlist = new List<Vendor>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_Vendor Vendor in trans.VendorList)
            {
                Vendor tmpVendor = new Vendor();

                tmpVendor.UpdateFromDatabaseObjectForVendorLookupListChunkSearch(Vendor, TimeZone);
                Vendortlist.Add(tmpVendor);
            }
            return Vendortlist;
        }

        public void UpdateFromDatabaseObjectForVendorLookupListChunkSearch(b_Vendor dbObj, string TimeZone)
        {
            this.UpdateFromDatabaseObject(dbObj);

            this.totalCount = dbObj.totalCount;
        }

        public void UpdateFromDatabaseObjectRetrieveLookupListBySearchCriteriaV2(b_Vendor dbObj)
        {          
            this.VendorId = dbObj.VendorId;          
            this.ClientLookupId = dbObj.ClientLookupId;            
            this.Name = dbObj.Name;           
        }
    }
}

