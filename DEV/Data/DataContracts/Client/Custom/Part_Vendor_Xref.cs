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
* 2014-Sep-18 SOM-106  Roger Lawton       Added 
****************************************************************************************************
*/

using Database.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Transactions;

namespace DataContracts
{
    public partial class Part_Vendor_Xref : DataContractBase, IStoredProcedureValidation
    {
        #region Properties    
        [DataMember]
        public string Part_ClientLookupId { get; set; }

        [DataMember]
        public string Part_Description { get; set; }

        [DataMember]
        public string Vendor_ClientLookupId { get; set; }

        [DataMember]
        public string Vendor_Name { get; set; }



        public long SiteId { get; set; }
        #endregion

        public static List<Part_Vendor_Xref> UpdateFromDatabaseObjectList(List<b_Part_Vendor_Xref> dbObjs)
        {
            List<Part_Vendor_Xref> result = new List<Part_Vendor_Xref>();

            foreach (b_Part_Vendor_Xref dbObj in dbObjs)
            {
                Part_Vendor_Xref tmp = new Part_Vendor_Xref();
                tmp.UpdateFromDatabaseObjectExtended(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromDatabaseObjectExtended(b_Part_Vendor_Xref dbObj)
        {
            this.UpdateFromDatabaseObject(dbObj);
            this.SiteId = dbObj.SiteId;
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
            this.Part_Description = dbObj.Part_Description;
            this.Vendor_ClientLookupId = dbObj.Vendor_ClientLookupId;
            this.Vendor_Name = dbObj.Vendor_Name;
        }

        public static List<b_Part_Vendor_Xref> ToDatabaseObjectList(List<Part_Vendor_Xref> objs)
        {
            List<b_Part_Vendor_Xref> result = new List<b_Part_Vendor_Xref>();
            foreach (Part_Vendor_Xref obj in objs)
            {
                result.Add(obj.ToDatabaseObjectExtended());
            }

            return result;
        }

        public b_Part_Vendor_Xref ToDatabaseObjectExtended()
        {
            b_Part_Vendor_Xref dbObj = this.ToDatabaseObject();
            dbObj.SiteId = this.SiteId;
            dbObj.Part_ClientLookupId = this.Part_ClientLookupId;
            dbObj.Part_Description = this.Part_Description;
            dbObj.Vendor_ClientLookupId = this.Vendor_ClientLookupId;
            dbObj.Vendor_Name = this.Vendor_Name;
            return dbObj;
        }

        public void RetrieveByPKExtended(DatabaseKey dbKey)
        {
            Part_Vendor_Xref_RetrieveByPKExtended trans = new Part_Vendor_Xref_RetrieveByPKExtended();
            trans.Part_Vendor_Xref = this.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObjectExtended(trans.Part_Vendor_Xref);
        }

        public static List<Part_Vendor_Xref> RetrieveListByPartId(DatabaseKey dbKey, Part_Vendor_Xref ptxref)
        {
            Part_Vendor_Xref_RetrieveListByPartId trans = new Part_Vendor_Xref_RetrieveListByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part_Vendor_Xref = ptxref.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Part_Vendor_Xref.UpdateFromDatabaseObjectList(trans.PartVendorList);
        }

        public static List<Part_Vendor_Xref> RetrieveListByVendorId(DatabaseKey dbKey, Part_Vendor_Xref ptxref)
        {
            Part_Vendor_Xref_RetrieveListByVendorId trans = new Part_Vendor_Xref_RetrieveListByVendorId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Part_Vendor_Xref = ptxref.ToDatabaseObjectExtended();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Part_Vendor_Xref.UpdateFromDatabaseObjectList(trans.PartVendorList);
        }

        public void ValidateAdd(DatabaseKey dbKey)
        {
            Validate<Part_Vendor_Xref>(dbKey);

        }

        public void ValidateSave(DatabaseKey dbKey)
        {
            Validate<Part_Vendor_Xref>(dbKey);

        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();

            Part_Vendor_Xref_ValidateByDuplicacy trans = new Part_Vendor_Xref_ValidateByDuplicacy()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Part_Vendor_Xref = this.ToDatabaseObject();
            // SOm-1679 - Validate Part and Vendor 
            trans.Part_Vendor_Xref.Part_ClientLookupId = this.Part_ClientLookupId;
            trans.Part_Vendor_Xref.Vendor_ClientLookupId = this.Vendor_ClientLookupId;
            trans.Part_Vendor_Xref.SiteId = this.SiteId;
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
        #region V2-1119 Retrieve For ShoppingCartDataImport
        public List<Part_Vendor_Xref> RetrieveForShoppingCartDataImport(DatabaseKey dbKey)
        {
            Part_Vendor_Xref_RetrieveForShoppingCartDataImport trans = new Part_Vendor_Xref_RetrieveForShoppingCartDataImport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Part_Vendor_Xref = this.ToDatabaseObjectForShoppingCartDataImport();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return Part_Vendor_Xref.UpdateFromDatabaseObjectListForShoppingCartDataImport(trans.PartVendorList);
        }
        public static List<Part_Vendor_Xref> UpdateFromDatabaseObjectListForShoppingCartDataImport(List<b_Part_Vendor_Xref> dbObjs)
        {
            List<Part_Vendor_Xref> result = new List<Part_Vendor_Xref>();
            if (dbObjs != null)
            {
                foreach (b_Part_Vendor_Xref dbObj in dbObjs)
                {
                    Part_Vendor_Xref tmp = new Part_Vendor_Xref();
                    tmp.UpdateFromDatabaseObjectExtendedForShoppingCartDataImport(dbObj);
                    result.Add(tmp);
                }
            }
            return result;
        }
        public void UpdateFromDatabaseObjectExtendedForShoppingCartDataImport(b_Part_Vendor_Xref dbObj)
        {
            this.PartId = dbObj.PartId;
            this.Part_Vendor_XrefId = dbObj.Part_Vendor_XrefId;
            this.OrderUnit = dbObj.OrderUnit;
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
        }
        public b_Part_Vendor_Xref ToDatabaseObjectForShoppingCartDataImport()
        {
            b_Part_Vendor_Xref dbObj = new b_Part_Vendor_Xref();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.VendorId = this.VendorId;
            dbObj.Manufacturer = this.Manufacturer;
            dbObj.ManufacturerId = this.ManufacturerId;
            return dbObj;
        }

        #region V2-1119 Add or Update Part/Vendor Cross-Reference when Processing Shopping Cart Item
        public void Part_Vendor_Xref_Create_Update_Punchout(DatabaseKey dbKey)
        {
            Part_Vendor_Xref_Create_Update_Punchout_V2 trans = new Part_Vendor_Xref_Create_Update_Punchout_V2();
            trans.Part_Vendor_Xref = this.ToDatabaseObject_V2();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject_V2(trans.Part_Vendor_Xref);
        }
        public b_Part_Vendor_Xref ToDatabaseObject_V2()
        {
            b_Part_Vendor_Xref dbObj = new b_Part_Vendor_Xref();
            dbObj.ClientId = this.ClientId;
            dbObj.SiteId = this.SiteId;
            dbObj.PartId = this.PartId;
            dbObj.VendorId = this.VendorId;
            dbObj.CatalogNumber = this.CatalogNumber;
            dbObj.IssueOrder = this.IssueOrder;
            dbObj.Manufacturer = this.Manufacturer;
            dbObj.ManufacturerId = this.ManufacturerId;
            dbObj.OrderQuantity = this.OrderQuantity;
            dbObj.OrderUnit = this.OrderUnit;
            dbObj.Price = this.Price;
            dbObj.UOMConvRequired = this.UOMConvRequired;
            dbObj.Punchout = this.Punchout;
            return dbObj;
        }

        public void UpdateFromDatabaseObject_V2(b_Part_Vendor_Xref dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.SiteId = this.SiteId;
            this.PartId = dbObj.PartId;
            this.VendorId = dbObj.VendorId;
            this.PreferredVendor = dbObj.PreferredVendor;
            this.CatalogNumber = dbObj.CatalogNumber;
            this.IssueOrder = dbObj.IssueOrder;
            this.Manufacturer = dbObj.Manufacturer;
            this.ManufacturerId = dbObj.ManufacturerId;
            this.OrderQuantity = dbObj.OrderQuantity;
            this.OrderUnit = dbObj.OrderUnit;
            this.Price = dbObj.Price;
            this.UOMConvRequired = dbObj.UOMConvRequired;
            this.Punchout = dbObj.Punchout;
            this.UpdateIndex = dbObj.UpdateIndex;

            // Turn on auditing
            AuditEnabled = true;
        }
        #endregion
        #endregion
    }
}
