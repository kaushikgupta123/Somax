/*
****************************************************************************************************
* PROPRIETARY DATA 
****************************************************************************************************
* This work is PROPRIETARY to SOMAX Inc and is protected 
* under Federal Law as an unpublished Copyrighted work and under State Law as 
* a Trade Secret. 
****************************************************************************************************
* Copyright (c) 2015 by SOMAX Inc.
* PreventiveMaintenanceDetails.aspx.cs
* All rights reserved. 
****************************************************************************************************
* Date        JIRA-ID  Person             Description
* =========== ======== ================== ==========================================================
* 2015-Feb-17 SOM-562  Roger Lawton       Part/Vendor Cross-References not showing up
****************************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Database.Business;
using Database;

namespace DataContracts
{
    public partial class Equipment_Parts_Xref : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        [DataMember]
        public string Equipment_ClientLookupId { get; set; }
        [DataMember]
        public string Equipment_Name { get; set; }
        [DataMember]
        public string Part_ClientLookupId { get; set; }
        [DataMember]
        public string Part_Description { get; set; }

        public long ParentSiteId { get; set; }
        public string PartClientLookUpId { get; set; }
        public string Description { get; set; }
        public string StockType { get; set; }
        #endregion

        public static List<Equipment_Parts_Xref> UpdateFromDatabaseObjectList(List<b_Equipment_Parts_Xref> dbObjs)
        {
            List<Equipment_Parts_Xref> result = new List<Equipment_Parts_Xref>();

            foreach (b_Equipment_Parts_Xref dbObj in dbObjs)
            {
                Equipment_Parts_Xref tmp = new Equipment_Parts_Xref();
                tmp.UpdateFromExtendedDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void UpdateFromExtendedDatabaseObject(b_Equipment_Parts_Xref dbObj)
        {
            UpdateFromDatabaseObject(dbObj);
            this.Equipment_ClientLookupId = dbObj.Equipment_ClientLookupId;
            this.Equipment_Name = dbObj.Equipment_Name;
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
            this.Part_Description = dbObj.Part_Description;
        }

        public b_Equipment_Parts_Xref ToExtendedDatabaseObject()
        {
            b_Equipment_Parts_Xref dbObj = this.ToDatabaseObject();
            dbObj.Equipment_ClientLookupId = this.Equipment_ClientLookupId;
            dbObj.Equipment_Name = this.Equipment_Name;
            dbObj.Part_ClientLookupId = this.Part_ClientLookupId;
            dbObj.Part_Description = this.Part_Description;
            return dbObj;
        }

        public static List<b_Equipment_Parts_Xref> ToDatabaseObjectList(List<Equipment_Parts_Xref> objs)
        {
            List<b_Equipment_Parts_Xref> result = new List<b_Equipment_Parts_Xref>();
            foreach (Equipment_Parts_Xref obj in objs)
            {
                result.Add(obj.ToExtendedDatabaseObject());
            }

            return result;
        }

        public static List<Equipment_Parts_Xref> RetrieveByEquipmentId(DatabaseKey dbKey, Equipment_Parts_Xref eq)
        {
            Equipment_Parts_Xref_RetrieveByEquipmentId trans = new Equipment_Parts_Xref_RetrieveByEquipmentId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment_Parts_Xref = eq.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Equipment_Parts_Xref.UpdateFromDatabaseObjectList(trans.Equipment_Parts_XrefList);
        }
        public static List<Equipment_Parts_Xref> RetrieveByEquipmentId_V2(DatabaseKey dbKey, Equipment_Parts_Xref eq)
        {
            Equipment_Parts_Xref_RetrieveByEquipmentId_V2 trans = new Equipment_Parts_Xref_RetrieveByEquipmentId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment_Parts_Xref = eq.ToDatabaseObjectXref();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Equipment_Parts_Xref.UpdateFromDatabaseObjectList(trans.Equipment_Parts_XrefList);
        }
        // SOM-562 - Begin
        public static List<Equipment_Parts_Xref> RetriveByPartId(DatabaseKey dbKey, Equipment_Parts_Xref eq)
        {
            Equipment_Parts_Xref_RetrieveByPartId trans = new Equipment_Parts_Xref_RetrieveByPartId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment_Parts_Xref = eq.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Equipment_Parts_Xref.UpdateFromDatabaseObjectList(trans.Equipment_Parts_XrefList);
        }
        // SOM-562 - End

        public void ValidateByPartId(DatabaseKey dbKey)
        {
            Validate<Equipment_Parts_Xref>(dbKey);
        }

        public void CreatePKForeignKeys(DatabaseKey dbKey)
        {
            //Validate<Equipment_Parts_Xref>(dbKey);

            //if (IsValid)
            //{
            Equipment_Parts_Xref_CreatePKForeignKeys trans = new Equipment_Parts_Xref_CreatePKForeignKeys()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };
            trans.Equipment_Parts_Xref = this.ToDatabaseObject();
            trans.Equipment_Parts_Xref.ParentSiteId = this.ParentSiteId;
            trans.Equipment_Parts_Xref.Equipment_ClientLookupId = string.IsNullOrEmpty(this.Equipment_ClientLookupId) ? "" : this.Equipment_ClientLookupId;
            trans.Equipment_Parts_Xref.Part_ClientLookupId = string.IsNullOrEmpty(this.Part_ClientLookupId) ? "" : this.Part_ClientLookupId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.Equipment_Parts_Xref);
            this.Equipment_ClientLookupId = trans.Equipment_Parts_Xref.Equipment_ClientLookupId;
            //}
        }


        public void UpdatePKForeignKeys(DatabaseKey dbKey)
        {



            Equipment_Parts_Xref_Update trans = new Equipment_Parts_Xref_Update();
            trans.Equipment_Parts_Xref = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.Equipment_Parts_Xref);

        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            Equipment_Parts_Xref_ValidateByClientLookupId trans = new Equipment_Parts_Xref_ValidateByClientLookupId()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.Equipment_Parts_Xref = this.ToDatabaseObject();
            trans.Equipment_Parts_Xref.ParentSiteId = this.ParentSiteId;
            trans.Equipment_Parts_Xref.Equipment_ClientLookupId = string.IsNullOrEmpty(this.Equipment_ClientLookupId) ? "" : this.Equipment_ClientLookupId;
            trans.Equipment_Parts_Xref.Part_ClientLookupId = string.IsNullOrEmpty(this.Part_ClientLookupId) ? "" : this.Part_ClientLookupId;
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

        #region V2-1007
        public static List<Equipment_Parts_Xref> RetrieveByEquipmentIdPartId_V2(DatabaseKey dbKey, Equipment_Parts_Xref eq)
        {
            Equipment_Parts_Xref_RetrieveByEquipmentIdPartId_V2 trans = new Equipment_Parts_Xref_RetrieveByEquipmentIdPartId_V2()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.Equipment_Parts_Xref = eq.ToDatabaseObjectXrefByEquipmentIdPartId();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            return Equipment_Parts_Xref.UpdateFromDatabaseObjectListByEquipmentIdPartId(trans.Equipment_Parts_XrefList);
        }
        public b_Equipment_Parts_Xref ToDatabaseObjectXrefByEquipmentIdPartId()
        {
            b_Equipment_Parts_Xref dbObj = new b_Equipment_Parts_Xref();
            dbObj.ClientId = this.ClientId;
            dbObj.Equipment_Parts_XrefId = this.Equipment_Parts_XrefId;
            dbObj.EquipmentId = this.EquipmentId;
            dbObj.PartId = this.PartId;
            return dbObj;
        }
        public static List<Equipment_Parts_Xref> UpdateFromDatabaseObjectListByEquipmentIdPartId(List<b_Equipment_Parts_Xref> dbObjs)
        {
            List<Equipment_Parts_Xref> result = new List<Equipment_Parts_Xref>();

            foreach (b_Equipment_Parts_Xref dbObj in dbObjs)
            {
                Equipment_Parts_Xref tmp = new Equipment_Parts_Xref();
                tmp.UpdateFromExtendedDatabaseObjectByEquipmentIdPartId(dbObj);
                result.Add(tmp);
            }
            return result;
        }
        public void UpdateFromExtendedDatabaseObjectByEquipmentIdPartId(b_Equipment_Parts_Xref dbObj)
        {
            this.ClientId = dbObj.ClientId;
            this.Equipment_Parts_XrefId = dbObj.Equipment_Parts_XrefId;
            this.EquipmentId = dbObj.EquipmentId;
            this.PartId = dbObj.PartId;
            this.Comment = dbObj.Comment;
            this.QuantityNeeded = dbObj.QuantityNeeded;
            this.QuantityUsed = dbObj.QuantityUsed;
            this.UpdateIndex = dbObj.UpdateIndex;
            this.Equipment_ClientLookupId = dbObj.Equipment_ClientLookupId;
            this.Equipment_Name = dbObj.Equipment_Name;
            this.Part_ClientLookupId = dbObj.Part_ClientLookupId;
            this.Part_Description = dbObj.Part_Description;
        }
        #endregion

    }
}
