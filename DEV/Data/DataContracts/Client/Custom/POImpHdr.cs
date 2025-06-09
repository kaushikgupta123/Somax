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
  public partial class POImpHdr : DataContractBase, IStoredProcedureValidation
  {
        public Int64 PersonnelId { get; set; }
        public int spErrCode { get; set; }
        public string PurchasingEventCreate { get; set; }
        public string PurchasingEventUpdate { get; set; }
        public Int64 PurchaseOrderId { get; set; }

        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckPOImpValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateImport";
            Validate<POImpHdr>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateImport")
            {
                POImpHdr_ValidateImport trans = new POImpHdr_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.POImpHdr = this.ToDatabaseObject();

                trans.dbKey = dbKey.ToTransDbKey();
                trans.POImpHdr.ClientId = this.ClientId;
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


        #region process import

        public void GetPOImpoHdrProcessImport(DatabaseKey dbKey)
        {
            POImpHdr_ProcessImport trans = new POImpHdr_ProcessImport()
            {
                CallerUserInfoId = dbKey.User.UserInfoId,
                CallerUserName = dbKey.UserName,
            };
            trans.POImpHdr = this.ToDatabaseObject();
            trans.POImpHdr.PurchasingEventCreate = this.PurchasingEventCreate;
            trans.POImpHdr.PurchasingEventUpdate = this.PurchasingEventUpdate;
            trans.POImpHdr.SiteId = this.SiteId;
            trans.POImpHdr.PersonnelId = this.PersonnelId;
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            UpdateFromDatabaseObject(trans.POImpHdr);
            this.PurchaseOrderId = trans.POImpHdr.PurchaseOrderId; 
        }

        #endregion


        #region Retrieve POImpHdr Data

        //public void GetPOImpHdrRetrievedDataByPOId(DatabaseKey dbKey)
        //{


        //    if (IsValid)
        //    {
        //        POImpHdr_RetrievebyPOIdFromDatabase trans = new POImpHdr_RetrievebyPOIdFromDatabase()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.POImpHdr = this.ToDatabaseObject();
        //        trans.POImpHdr.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.POImpHdr);
        //    }

        //}

        public void GetPOImpHdrRetrievedDataByPRId(DatabaseKey dbKey)
        {


            if (IsValid)
            {
                POImpHdr_RetrievebyPRIdFromDatabase trans = new POImpHdr_RetrievebyPRIdFromDatabase()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.POImpHdr = this.ToDatabaseObject();
                trans.POImpHdr.SiteId = this.SiteId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObject(trans.POImpHdr);
            }

        }



        //public void GetPOImpHdrRetrievedDataBySomaxPRId(DatabaseKey dbKey)
        //{


        //    if (IsValid)
        //    {
        //        POImpHdr_RetrievebySomaxPRIdFromDatabase trans = new POImpHdr_RetrievebySomaxPRIdFromDatabase()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.POImpHdr = this.ToDatabaseObject();
        //        trans.POImpHdr.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.POImpHdr);
        //    }

        //}



        public void GetPOImpHdrRetrievedDataByEXPOId(DatabaseKey dbKey)
        {


            if (IsValid)
            {
                POImpHdr_RetrievebyExPOIdFromDatabase trans = new POImpHdr_RetrievebyExPOIdFromDatabase()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.POImpHdr = this.ToDatabaseObject();
                trans.POImpHdr.SiteId = this.SiteId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObject(trans.POImpHdr);
            }

        }


        //public void POImpHdrRetrievedDataByPKCustom(DatabaseKey dbKey)
        //{


        //    if (IsValid)
        //    {
        //        POImpHdr_RetrieveByPKCustom trans = new POImpHdr_RetrieveByPKCustom()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.POImpHdr = this.ToDatabaseObject();
        //        trans.POImpHdr.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.POImpHdr);
        //    }

        //}

        #endregion

        #region Create
        public void CustomCreate(DatabaseKey dbKey)
        {
            POImpHdr_CreateCustom trans = new POImpHdr_CreateCustom();
            trans.POImpHdr = this.ToDatabaseObject();
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure may have populated an auto-incremented key. 
            UpdateFromDatabaseObject(trans.POImpHdr);
        }

        #endregion

        #region Update

        public void UpdateCustom(DatabaseKey dbKey)
        {
            POImpHdr_UpdateCustom trans = new POImpHdr_UpdateCustom();
            trans.POImpHdr = this.ToDatabaseObject();
            trans.ChangeLog = GetChangeLogObject(dbKey);
            trans.dbKey = dbKey.ToTransDbKey();
            trans.Execute();

            // The create procedure changed the Update Index.
            UpdateFromDatabaseObject(trans.POImpHdr);
        }
        #endregion

    }
}



