/*
 **************************************************************************************************
 * PROPRIETARY DATA 
 **************************************************************************************************
 * This work is PROPRIETARY to SOMAX Inc and is protected 
 * under Federal Law as an unpublished Copyrighted work and under State Law as 
 * a Trade Secret. 
 **************************************************************************************************
 * Copyright (c) 2017-2020 by SOMAX Inc.
 * All rights reserved. 
 **************************************************************************************************
 * Data Contract Class for Vendor Master Import
 **************************************************************************************************
 * Date        Log Entry Developer          Description
 * =========== ========= ================== =======================================================
 * 2020-Oct-29 V2-416    Roger Lawton       Added VendorMasterImportRetrieveAll
 **************************************************************************************************
 */
using Database;
using Database.Business;
using Database.Client.Custom.Transactions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace DataContracts
{
    public partial class VendorMasterImport : DataContractBase, IStoredProcedureValidation
    {

        #region Properties
        public string VendorNumber { get; set; }
        public string Status { get; set; }
        public string _ClientLookupID { get; set; }
        public int error_message_count { get; set; }
        #endregion

        #region Transaction Method
        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckValidiateMasterImport(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateImport";
            Validate<VendorMasterImport>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateImport")
            {
                VendorMasterImport_Validation trans = new VendorMasterImport_Validation()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMasterImport = this.ToDatabaseObject();
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

        #region process 

        public void GetVendorMasteImportProcess(DatabaseKey dbKey)
        {

                VendorMasterImport_ProcessImport trans = new VendorMasterImport_ProcessImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMasterImport = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObject(trans.VendorMasterImport);

        }

        #endregion

        #region Retrieve
        //public void GetVendorImportRetrievedDataByVendorNumber(DatabaseKey dbKey)
        //{

        //    VendorMasterImport_RetrieveByVendorNumber trans = new VendorMasterImport_RetrieveByVendorNumber()
        //    {
        //        CallerUserInfoId = dbKey.User.UserInfoId,
        //        CallerUserName = dbKey.UserName,
        //    };
        //    trans.VendorMasterImport = this.ToDatabaseObject();
        //    trans.VendorMasterImport.SiteId = this.SiteId;

        //    trans.dbKey = dbKey.ToTransDbKey();
        //    trans.Execute();

        //    UpdateFromDatabaseObject(trans.VendorMasterImport);

        //}
        /// <summary>
        /// V2-416 - Retrieve all for the current Client
        /// </summary>
        /// <param name="dbKey"></param>
        /// <returns></returns>
        public List<VendorMasterImport> VendorMasterImportRetrieveAll(DatabaseKey dbKey)
        {
            VendorMasterImport_RetrieveAll trans = new VendorMasterImport_RetrieveAll()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName
            };

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            return UpdateFromDatabaseObjectList(trans.VendorMasterImportList);
        }
        /// <summary>
        /// V2-416 - Load the DB Object List 
        /// </summary>
        /// <param name="dbKey"></param>
        /// <returns></returns>
        public static List<VendorMasterImport> UpdateFromDatabaseObjectList(List<b_VendorMasterImport> dbObjs)
        {
            List<VendorMasterImport> result = new List<VendorMasterImport>();

            foreach (b_VendorMasterImport dbObj in dbObjs)
            {
                VendorMasterImport tmp = new VendorMasterImport();
                tmp.UpdateFromDatabaseObject(dbObj);
                result.Add(tmp);
            }
            return result;
        }

        public void GetVendorImportRetrievedDataByExVendorId(DatabaseKey dbKey)
        {

            VendorMasterImport_RetrieveByExVendorId trans = new VendorMasterImport_RetrieveByExVendorId()
            //VendorMasterImport_
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.VendorMasterImport = this.ToDatabaseObject();
            trans.VendorMasterImport.SiteId = this.SiteId;

            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.VendorMasterImport);

        }
        #endregion

        #region create 
        public void CreateCustom(DatabaseKey dbKey)
        {
            VendorMasterImport_CreateCustom trans = new VendorMasterImport_CreateCustom();
            trans.VendorMasterImport = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.VendorMasterImport);
        }
        #endregion

        #region Update
        public void UpdateCustom(DatabaseKey dbKey)
        {
            VendorMasterImport_UpdateCustom trans = new VendorMasterImport_UpdateCustom();
            trans.VendorMasterImport = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.VendorMasterImport);
        }
        #endregion

       
       
        public void RetrieveByClientIdVendorExIdVendorExSiteId(DatabaseKey dbKey)
        {
            RetrieveByClientIdVendorExIdVendorExSiteId trans = new RetrieveByClientIdVendorExIdVendorExSiteId();
            trans._ClientLookupID = this._ClientLookupID;
            trans.VendorMasterImport = ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();
            UpdateFromDatabaseObject(trans.VendorMasterImport);
        }


        public void Create_VendorMasterProcessInterface(DatabaseKey dbKey)
        {


            if (IsValid)
            {
                VendorMasterImport_ProcessInterface trans = new VendorMasterImport_ProcessInterface()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.VendorMasterImport = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                // The create procedure may have populated an auto-incremented key. 
                UpdateFromDatabaseObject(trans.VendorMasterImport);
                this.error_message_count = trans.VendorMasterImport.error_message_count;
            }
        }

        #endregion
    }
}
