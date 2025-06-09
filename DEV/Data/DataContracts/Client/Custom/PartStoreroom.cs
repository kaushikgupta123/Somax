/*
 ******************************************************************************
 * PROPRIETARY DATA 
 ******************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 ******************************************************************************
 * Copyright (c) 2011 by SOMAX Inc.
 * All rights reserved. 
 ******************************************************************************
 * Date        Log Entry Developer          Description
 * =========== ========= ================== ===================================
 * 2012-Mar-30           Roger Lawton       Created
 ******************************************************************************
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


namespace DataContracts
{
    public partial class PartStoreroom : DataContractBase, IStoredProcedureValidation
    {

        #region Properties
        [DataMember]
        public string ClientLookupId { get; set; }  // not sure where this is used
        public string StoreroomName { get; set; }
        public string AccountId_ClientLookupId { get; set; }
        public string ATSource_StoreroomName { get; set; }
        public long SiteId { get; set; }
        public bool CreateMode { get; set; }
        public string ReorderMethod_Lookup_ListName { get; set; }
        public string Location1_1_Lookup_ListName { get; set; }
        public string Location1_2_Lookup_ListName { get; set; }
        public string Location1_3_Lookup_ListName { get; set; }
        public string Location1_4_Lookup_ListName { get; set; }
        //V2-670
        string ValidateFor = string.Empty;
        //V2-1059
        string ValidateForStoreroomId = string.Empty;
        public string StoreroomIDList { get; set; }
        public string orderbyColumn { get; set; }
        public string orderBy { get; set; }
        public string offset1 { get; set; }
        public string nextrow { get; set; }
        public long RowId { get; set; }
        public string RequestStr { get; set; }
        public string IssueStr { get; set; }
        public string PartDescription { get; set; }
        public decimal? TransferQuantity { get; set; }
        public decimal? Max { get; set; }
        public decimal? Min { get; set; }
        public decimal? OnHand { get; set; }
        public long RequestPTStoreroomId { get; set; }
        public long RequestStoreroomId { get; set; }
        public long RequestPartId { get; set; }
        public long IssuePTStoreroomId { get; set; }
        public long IssueStoreroomId { get; set; }
        public long IssuePartId { get; set; }
        public long Creator_PersonnelId { get; set; }
        public int TotalCount { get; set; }
        public List<PartStoreroom> listOfPartStoreroom { get; set; }
        public string PartIdClientLookupId { get; set; }
        public bool Maintain { get; set; }
        public long PersonnelId { get; set; }
        #region V2-755
        public bool Issue { get; set; }
        public bool PhysicalInventory { get; set; }
        #endregion
        #region 1025
        public string StoreroomNameWithDescription { get; set; }
        public decimal TotalOnRequest { get; set; }
        public decimal TotalOnOrder { get; set; }
        public string VendorName { get; set; }
        public string VendorClientLookupId { get; set; }
        #endregion

        public string Flag { get; set; } //V2-1070
        #endregion


        public void RetrieveByPartStoreroomId(DatabaseKey dbKey)
        {
            PartStoreroom_RetrieveByPartStoreroomId trans = new PartStoreroom_RetrieveByPartStoreroomId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartStoreroom = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.PartStoreroom);
        }
        public static List<PartStoreroom> RetrieveByPartId(DatabaseKey dbKey, PartStoreroom ps)
        {
            PartStoreroom_RetrieveByPartId trans = new PartStoreroom_RetrieveByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.PartStoreroom = ps.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return PartStoreroom.UpdateFromDatabaseObjectList(trans.PartStoreroomList);
        }
        public static List<PartStoreroom> UpdateFromDatabaseObjectList(List<b_PartStoreroom> dbObjs)
        {
            List<PartStoreroom> result = new List<PartStoreroom>();

            foreach (b_PartStoreroom dbObj in dbObjs)
            {
                PartStoreroom tmp = new PartStoreroom();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtended(b_PartStoreroom dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.StoreroomName = dbObj.StoreroomName;
            this.AccountId_ClientLookupId = dbObj.AccountId_ClientLookupId;
            this.ATSource_StoreroomName = dbObj.ATSource_StoreroomName;
        }
        public b_PartStoreroom ToDatabaseObjectExtended()
        {
            b_PartStoreroom dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.StoreroomName = string.IsNullOrEmpty(this.StoreroomName) ? "" : this.StoreroomName;
            dbObj.ATSource_StoreroomName = string.IsNullOrEmpty(this.ATSource_StoreroomName) ? "" : this.ATSource_StoreroomName;
            dbObj.AccountId_ClientLookupId = string.IsNullOrEmpty(this.AccountId_ClientLookupId) ? "" : this.AccountId_ClientLookupId;
            dbObj.createmode = this.CreateMode;
            return dbObj;
        }

        public void UpdateByPartId(DatabaseKey dbKey)
        {
            this.CreateMode = false;
            Validate<PartStoreroom>(dbKey);

            if (IsValid)
            {
                PartStoreroom_UpdateByPartId trans = new PartStoreroom_UpdateByPartId()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.PartStoreroom = this.ToDatabaseObjectExtended();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObject(trans.PartStoreroom);
            }
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            #region V2-670
            if (ValidateFor == "ValidateStoreroomId")
            {
                List<StoredProcValidationError> errorslist = new List<StoredProcValidationError>();
                PartStoreroom_ValidateStoreroomIdTransaction ptrans = new PartStoreroom_ValidateStoreroomIdTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.PartStoreroom = this.ToDatabaseObject();
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errorslist.Add(tmp);
                    }
                }
                return errorslist;
            }
            #endregion
            #region V2-1059
            else if (ValidateForStoreroomId == "ValidateSameStoreroomId")
            {
                List<StoredProcValidationError> errorslist = new List<StoredProcValidationError>();
                PartStoreroom_ValidateSameStoreroomIdTransaction ptrans = new PartStoreroom_ValidateSameStoreroomIdTransaction()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.PartStoreroom = this.ToDatabaseObject();
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errorslist.Add(tmp);
                    }
                }
                return errorslist;
            }
            #endregion
            #region V2-1070
            if (ValidateFor == "CheckIfInactivateorActivatePartForMultiStoreroom")
            {
                List<StoredProcValidationError> errorslist = new List<StoredProcValidationError>();
                PartStoreroom_ValidateByInactivateorActivate ptrans = new PartStoreroom_ValidateByInactivateorActivate()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                ptrans.PartStoreroom = this.ToDatabaseObject();
                ptrans.PartStoreroom.Flag = this.Flag;
                ptrans.PartStoreroom.PartIdClientLookupId = this.ClientLookupId;
                ptrans.PartStoreroom.SiteId = this.SiteId;
                ptrans.dbKey = dbKey.ToTransDbKey();
                ptrans.Execute();
                if (ptrans.StoredProcValidationErrorList != null)
                {
                    foreach (b_StoredProcValidationError error in ptrans.StoredProcValidationErrorList)
                    {
                        StoredProcValidationError tmp = new StoredProcValidationError();
                        tmp.UpdateFromDatabaseObject(error);
                        errorslist.Add(tmp);
                    }
                }
                return errorslist;
            }
            #endregion

            // Create a table to hold the columns that need to be validated against their lookup list
            // Should this be a property of the contract and database object
            System.Data.DataTable lulist = new DataTable("lulist");
            lulist.Columns.Add("RowID", typeof(Int64));
            lulist.Columns.Add("SiteID", typeof(Int64));
            lulist.Columns.Add("ColumnName", typeof(string));
            lulist.Columns.Add("ColumnValue", typeof(string));
            lulist.Columns.Add("ListName", typeof(string));
            lulist.Columns.Add("ListFilter", typeof(string));
            lulist.Columns.Add("ErrorID", typeof(Int64));

            // Add a row for each column to be validated
            int rowid = 0;
            string filter = "";
            // Reorder Method
            lulist.Rows.Add(++rowid, SiteId, "Type", ReorderMethod, ReorderMethod_Lookup_ListName, "", 3);
            // Location1_1
            lulist.Rows.Add(++rowid, SiteId, "Location1_1", Location1_1, Location1_1_Lookup_ListName, "", 4);
            // Location1_2
            lulist.Rows.Add(++rowid, SiteId, "Location1_2", Location1_2, Location1_2_Lookup_ListName, "", 5);
            // Location1_3
            lulist.Rows.Add(++rowid, SiteId, "Location1_3", Location1_3, Location1_3_Lookup_ListName, "", 6);
            // Location1_4
            lulist.Rows.Add(++rowid, SiteId, "Location1_4", Location1_4, Location1_4_Lookup_ListName, "", 7);

            PartStoreroom_ValidateByPartId trans = new PartStoreroom_ValidateByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartStoreroom = this.ToDatabaseObjectExtended();
            trans.PartStoreroom.LuList = lulist;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

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



        public PartStoreroom PartStoreroomSearchForChildGridV2(DatabaseKey dbKey, string TimeZone)
        {
            PartStoreroom_SearchForMultiPartStoreroomChildGridV2 trans = new PartStoreroom_SearchForMultiPartStoreroomChildGridV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartStoreroom = this.ToDateBaseObjectForPartStoreroomSearchForChildGrid();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            this.listOfPartStoreroom = new List<PartStoreroom>();


            List<PartStoreroom> PartStoreroomlist = new List<PartStoreroom>();
            // Moved this to UpdateFromDatabaseObjectForRetriveV2
            foreach (b_PartStoreroom PartStoreroom in trans.PartStoreroom.listOfPartStoreroom)
            {
                PartStoreroom tmpPart = new PartStoreroom();

                tmpPart.UpdateFromDatabaseObjectForMultiPartStoreroomChildGrid(PartStoreroom, TimeZone);
                PartStoreroomlist.Add(tmpPart);
            }
            this.listOfPartStoreroom.AddRange(PartStoreroomlist);
            return this;
        }
        public b_PartStoreroom ToDateBaseObjectForPartStoreroomSearchForChildGrid()
        {
            b_PartStoreroom dbObj = new b_PartStoreroom();

            dbObj.PartId=this.PartId;
            dbObj.PersonnelId = this.PersonnelId;
            dbObj.ClientId = this.ClientId;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForMultiPartStoreroomChildGrid(b_PartStoreroom dbObj, string TimeZone)
        {
            this.PartIdClientLookupId = dbObj.PartIdClientLookupId;
            this.PartStoreroomId = dbObj.PartStoreroomId;
            this.QtyOnHand = dbObj.QtyOnHand;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.QtyReorderLevel = dbObj.QtyReorderLevel;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.CountFrequency = dbObj.CountFrequency;
            this.LastCounted = dbObj.LastCounted;
            this.AutoTransfer = dbObj.AutoTransfer;
            this.StoreroomName = dbObj.StoreroomName;
            this.StoreroomId = dbObj.StoreroomId;
            this.Maintain = dbObj.Maintain;
            this.Issue = dbObj.Issue;
            this.PhysicalInventory = dbObj.PhysicalInventory;
            this.StoreroomNameWithDescription = dbObj.StoreroomNameWithDescription;
            this.TotalOnRequest = dbObj.TotalOnRequest;
            this.TotalOnOrder = dbObj.TotalOnOrder;
        }

        //V2-670
        public void ValidateAddMultiStoreroomPartStoreroom(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateStoreroomId";
            Validate<PartStoreroom>(dbKey);
        }
        public List<PartStoreroom> RetrieveByStoreroomIdAndPartId(DatabaseKey dbKey)
        {
            PartStoreroom_RetrieveByStoreroomIdAndPartId trans = new PartStoreroom_RetrieveByStoreroomIdAndPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PartStoreroom = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabasePartstoreroombyStoreroomIdList(trans.PartStoreroomList);

        }

        public static List<PartStoreroom> UpdateFromDatabasePartstoreroombyStoreroomIdList(List<b_PartStoreroom> dbObjs)
        {
            List<PartStoreroom> result = new List<PartStoreroom>();

            foreach (b_PartStoreroom dbObj in dbObjs)
            {
                PartStoreroom tmp = new PartStoreroom();
                tmp.PartId = dbObj.PartId;
                tmp.PartIdClientLookupId = dbObj.PartIdClientLookupId;
                tmp.StoreroomId = dbObj.StoreroomId;
                tmp.Location1_1 = dbObj.Location1_1;
                tmp.Location1_2 = dbObj.Location1_2;
                tmp.Location1_3 = dbObj.Location1_3;
                tmp.Location1_4 = dbObj.Location1_4;
                tmp.Location1_5 = dbObj.Location1_5;
                tmp.PartStoreroomId = dbObj.PartStoreroomId;
                result.Add(tmp);
            }
            return result;
        }


        /*
        public void UpdateFromCompositeDatabaseObject(b_PartStoreroom dbObj)
        {
          this.UpdateFromDatabaseObject(dbObj);
          this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
          this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
        }
        public void UpdateFromExtendedDatabaseObject(b_PartStoreroom dbObj)
        {
          this.UpdateFromDatabaseObject(dbObj);
          this.ClientLookupId = dbObj.ClientLookupId;
          this.Description = dbObj.Description;
          this.EstimatedTotalCosts = dbObj.EstimatedTotalCosts;
          this.AssignedTo_PersonnelClientLookupId = dbObj.AssignedTo_PersonnelClientLookupId;
          this.ChargeToClientLookupId = dbObj.ChargeToClientLookupId;
        }

        public static List<b_PartStoreroom> ToDatabaseObjectList(List<PartStoreroom> objs)
        {
          List<b_PartStoreroom> result = new List<b_PartStoreroom>();
          foreach (PartStoreroom obj in objs)
          {
            result.Add(obj.ToExtendedDatabaseObject());
          }

          return result;
        }

        public b_PartStoreroom ToExtendedDatabaseObject()
        {
          b_PartStoreroom dbObj = this.ToDatabaseObject();
          dbObj.ClientLookupId = this.ClientLookupId;
          dbObj.Description = this.Description;
          dbObj.EstimatedTotalCosts = this.EstimatedTotalCosts;
          dbObj.ChargeToClientLookupId = this.ChargeToClientLookupId;
          dbObj.AssignedTo_PersonnelClientLookupId = this.AssignedTo_PersonnelClientLookupId;
          return dbObj;
        }

        public static List<PartStoreroom> RetriveByEquipmentId(DatabaseKey dbKey, PartStoreroom pm)
        {
          PartStoreroom_RetrieveByEquipmentId trans = new PartStoreroom_RetrieveByEquipmentId()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };

          trans.PartStoreroom = pm.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          return PartStoreroom.UpdateFromDatabaseObjectList(trans.PartStoreroomList);
        }

        public static List<PartStoreroom> RetrieveByLocationId(DatabaseKey dbKey, PartStoreroom pm)
        {
          PartStoreroom_RetrieveByLocationId trans = new PartStoreroom_RetrieveByLocationId()
          {
            CallerUserInfoId = dbKey.User.UserInfoId,
            CallerUserName = dbKey.UserName
          };

          trans.PartStoreroom = pm.ToDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          return PartStoreroom.UpdateFromDatabaseObjectList(trans.PartStoreroomList);
        }



        public void CreateByForeignKeys(DatabaseKey dbKey)
        {
          PartStoreroom_CreateByPKForeignKeys trans = new PartStoreroom_CreateByPKForeignKeys();
          trans.PartStoreroom = this.ToExtendedDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();

          // The create procedure may have populated an auto-incremented key. 
          UpdateFromExtendedDatabaseObject(trans.PartStoreroom);
        }

        public void RetrieveByForeignKeys(DatabaseKey dbKey)
        {
          PartStoreroom_RetrieveByPKForeignKeys trans = new PartStoreroom_RetrieveByPKForeignKeys();
          trans.PartStoreroom = this.ToExtendedDatabaseObject();
          trans.dbKey = dbKey.ToTransDbKey();
          trans.Execute();
          UpdateFromExtendedDatabaseObject(trans.PartStoreroom);
        }


        */

        #region V2-751
        public List<PartStoreroom> RetrieveIssuingStoreroomListForPartTransferRequest(DatabaseKey dbKey)
        {
            PartStoreroom_RetrieveIssuingStoreroomListForPartTransferRequest trans = new PartStoreroom_RetrieveIssuingStoreroomListForPartTransferRequest()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.PartStoreroom = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseIssuingStoreroomListForPartTransferRequest(trans.PartStoreroomList);

        }

        public static List<PartStoreroom> UpdateFromDatabaseIssuingStoreroomListForPartTransferRequest(List<b_PartStoreroom> dbObjs)
        {
            List<PartStoreroom> result = new List<PartStoreroom>();

            foreach (b_PartStoreroom dbObj in dbObjs)
            {
                PartStoreroom tmp = new PartStoreroom();
                tmp.PartStoreroomId = dbObj.PartStoreroomId;
                tmp.StoreroomId = dbObj.StoreroomId;
                tmp.StoreroomName = dbObj.StoreroomName;
                result.Add(tmp);
            }
            return result;
        }


        #endregion

        #region V2-1025
        public PartStoreroom RetriveforStoreroomGridChildDetailsViewByPartStoreroomIdV2(DatabaseKey dbKey, string TimeZone)
        {
            PartStoreroom_StoreroomGridChildDetailsViewByPartStoreroomIdV2 trans = new PartStoreroom_StoreroomGridChildDetailsViewByPartStoreroomIdV2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartStoreroom = this.ToDateBaseObjectForStoreroomGridChildDetailsViewByPartStoreroomIdV2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

          
            PartStoreroom tmpPart = new PartStoreroom();
           tmpPart.UpdateFromDatabaseObjectForStoreroomGridChildDetailsViewByPartStoreroomIdV2(trans.PartStoreroom, TimeZone);
                
         
            return tmpPart;
        }
        public b_PartStoreroom ToDateBaseObjectForStoreroomGridChildDetailsViewByPartStoreroomIdV2()
        {
            b_PartStoreroom dbObj = new b_PartStoreroom();
            dbObj.PartStoreroomId = this.PartStoreroomId;
            dbObj.ClientId = this.ClientId;
            return dbObj;
        }


        public void UpdateFromDatabaseObjectForStoreroomGridChildDetailsViewByPartStoreroomIdV2(b_PartStoreroom dbObj, string TimeZone)
        {
            
            this.PartStoreroomId = dbObj.PartStoreroomId;
            this.StoreroomName = dbObj.StoreroomName;
            this.StoreroomId = dbObj.StoreroomId;
            this.CountFrequency = dbObj.CountFrequency;
            this.LastCounted = dbObj.LastCounted;
            this.Critical = dbObj.Critical;
            this.Location1_1 = dbObj.Location1_1;
            this.Location1_2 = dbObj.Location1_2;
            this.Location1_3 = dbObj.Location1_3;
            this.Location1_4 = dbObj.Location1_4;
            this.Location1_5 = dbObj.Location1_5;
            this.Location2_1 = dbObj.Location2_1;
            this.Location2_2 = dbObj.Location2_2;
            this.Location2_3 = dbObj.Location2_3;
            this.Location2_4 = dbObj.Location2_4;
            this.Location2_5 = dbObj.Location2_5;
            this.AutoPurchase = dbObj.AutoPurchase;
            this.QtyOnHand = dbObj.QtyOnHand;
            this.QtyMaximum = dbObj.QtyMaximum;
            this.QtyReorderLevel = dbObj.QtyReorderLevel;
            this.VendorName = dbObj.VendorName;
            this.PartVendorId = dbObj.PartVendorId;
            this.VendorClientLookupId = dbObj.VendorClientLookupId;

        }
        #endregion

        #region V2-1059
        public void ValidateAddMultiStoreroomPartStoreroomSameAutoTransferIssueStoreroomId(DatabaseKey dbKey)
        {
            ValidateForStoreroomId = "ValidateSameStoreroomId";
            Validate<PartStoreroom>(dbKey);
        }
       
        public List<PartStoreroom> RetrieveChunkSearchAutoTRGeneration(DatabaseKey dbKey)
        {
            PartStoreroom_RetrieveChunkSearchAutoTRGenerationTransaction trans = new PartStoreroom_RetrieveChunkSearchAutoTRGenerationTransaction()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.PartStoreroom = this.ToDateBaseObjectForRetrieveChunkSearchAutoTRGeneration();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            List<PartStoreroom> PartStoreroomlist = new List<PartStoreroom>();

            foreach (b_PartStoreroom PartStoreroom in trans.PartStoreroomList)
            {
                PartStoreroom tmppartStoreroom = new PartStoreroom();
                tmppartStoreroom.UpdateFromDatabaseObjectForRetriveForSearchAutoTRGeneration(PartStoreroom);
                PartStoreroomlist.Add(tmppartStoreroom);
            }
            return PartStoreroomlist;
        }
        public b_PartStoreroom ToDateBaseObjectForRetrieveChunkSearchAutoTRGeneration()
        {
            b_PartStoreroom dbObj = this.ToDatabaseObject();
            dbObj.orderbyColumn = this.orderbyColumn;
            dbObj.orderBy = this.orderBy;
            dbObj.offset1 = this.offset1;
            dbObj.nextrow = this.nextrow;
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.StoreroomIdList = this.StoreroomIDList;
            dbObj.PersonnelId = this.PersonnelId;


            return dbObj;
        }
        public void UpdateFromDatabaseObjectForRetriveForSearchAutoTRGeneration(b_PartStoreroom dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.RowId = dbObj.RowId;
            this.PartIdClientLookupId = dbObj.PartIdClientLookupId;
            this.RequestStr = dbObj.RequestStr;
            this.IssueStr = dbObj.IssueStr;
            this.PartDescription = dbObj.PartDescription;
            this.TransferQuantity = dbObj.TransferQuantity;
            this.Max = dbObj.Max;
            this.Min = dbObj.Min;
            this.OnHand = dbObj.OnHand;
            this.RequestPTStoreroomId = dbObj.RequestPTStoreroomId;
            this.RequestStoreroomId = dbObj.RequestStoreroomId;
            this.RequestPartId = dbObj.RequestPartId;
            this.IssuePTStoreroomId = dbObj.IssuePTStoreroomId;
            this.IssueStoreroomId = dbObj.IssueStoreroomId;
            this.IssuePartId = dbObj.IssuePartId;
            this.Creator_PersonnelId = dbObj.Creator_PersonnelId;
            this.TotalCount = dbObj.TotalCount;
        }

        #endregion

        #region V2-1070
        public void CheckPartIsInactivateorActivateForMultiStoreroom(DatabaseKey dbKey)
        {
            ValidateFor = "CheckIfInactivateorActivatePartForMultiStoreroom";
            Validate<PartStoreroom>(dbKey);
        }
        #endregion
    }
}
