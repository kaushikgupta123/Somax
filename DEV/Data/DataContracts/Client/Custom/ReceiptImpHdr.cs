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


    public partial class ReceiptImpHdr : DataContractBase, IStoredProcedureValidation
    {

        public Int64 SiteId { get; set; }
        public Int64 PersonnelId { get; set; }
        public int spErrCode { get; set; }
        public string PurchasingEventCreate { get; set; }
        public string PurchasingEventUpdate { get; set; }

        #region Validation Methods
        public string ValidateFor = string.Empty;

        public void CheckReceiptImpValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateImport";
            Validate<ReceiptImpHdr>(dbKey);
        }
        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateImport")
            {
                ReceiptImpHdr_ValidateImport trans = new ReceiptImpHdr_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpHdr = this.ToDatabaseObject();

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


        #region Process Receipt Import

        public void GetReceiptImpHdrProcessImport(DatabaseKey dbKey)
        {
                ReceiptImpHdr_ProcessImport trans = new ReceiptImpHdr_ProcessImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpHdr = this.ToDatabaseObject();
                trans.ReceiptImpHdr.PurchasingEventCreate = this.PurchasingEventCreate;
                trans.ReceiptImpHdr.PurchasingEventUpdate = this.PurchasingEventUpdate;
                //trans.ReceiptImpHdr.SiteId = this.SiteId;
                //trans.ReceiptImpHdr.PersonnelId = this.PersonnelId;
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObject(trans.ReceiptImpHdr);
                this.spErrCode = trans.ReceiptImpHdr.spErrCode;
           

        }

        #endregion



        #region Retrieve Receipt Import


        //public void GetReceiptImpHdrRetrievedDataByPOId(DatabaseKey dbKey)
        //{
        //    Validate<ReceiptImpHdr>(dbKey);

        //    if (IsValid)
        //    {
        //        ReceiptImpHdr_RetrievebyPOIdFromDatabase trans = new ReceiptImpHdr_RetrievebyPOIdFromDatabase()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.ReceiptImpHdr = this.ToDatabaseObject();
        //        trans.ReceiptImpHdr.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.ReceiptImpHdr);
        //    }

        //}


        public void GetReceiptImpHdrRetrievedDataByReceiptId(DatabaseKey dbKey)
        {
            //Validate<ReceiptImpHdr>(dbKey);

            if (IsValid)
            {
                ReceiptImpHdr_RetrievebyReceiptIdFromDatabase trans = new ReceiptImpHdr_RetrievebyReceiptIdFromDatabase()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpHdr = this.ToDatabaseObject();
                trans.ReceiptImpHdr.SiteId = this.SiteId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.ReceiptImpHdr);
            }

        }


    #endregion


    #region  Create
    public void CreateCustom(DatabaseKey dbKey)
    {
      ReceiptImpHdr_CreateCustom trans = new ReceiptImpHdr_CreateCustom();
      trans.ReceiptImpHdr = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.ReceiptImpHdr);
    }
    #endregion

    #region Update
    public void UpdateCustom(DatabaseKey dbKey)
    {
      ReceiptImpHdr_UpdateCustom trans = new ReceiptImpHdr_UpdateCustom();
      trans.ReceiptImpHdr = this.ToDatabaseObject();
      trans.ChangeLog = GetChangeLogObject(dbKey);
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure changed the Update Index.
      UpdateFromDatabaseObject(trans.ReceiptImpHdr);
    }
    #endregion
  }
}



