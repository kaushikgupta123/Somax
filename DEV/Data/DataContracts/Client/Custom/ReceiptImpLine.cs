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
    public partial class ReceiptImpLine : DataContractBase, IStoredProcedureValidation
    {
        #region Properties
        public long SiteId { get; set; }
        public long PersonnelId { get; set; }
        public int spErrCode { get; set; }
        public long EXPOID { get; set; } 
        public string ValidateFor = string.Empty;
        #endregion Properties

        #region Validation Methods
        public void CheckReceiptImpLineValidate(DatabaseKey dbKey)
        {
            ValidateFor = "ValidateImport";
            Validate<ReceiptImpLine>(dbKey);
        }

        public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
        {
            List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


            if (ValidateFor == "ValidateImport")
            {
                ReceiptImpLine_ValidateImport trans = new ReceiptImpLine_ValidateImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpLine = this.ToDatabaseObject();

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


        #region process import

        public void GetReceiptImpLineProcessImport(DatabaseKey dbKey)
        {
            
                ReceiptImpLine_ProcessImport trans = new ReceiptImpLine_ProcessImport()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpLine = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.ReceiptImpLine);
                this.spErrCode = trans.ReceiptImpLine.spErrCode;
          

        }

        #endregion



        #region Retrieve RecieptImpLine


        public void GetReceiptImpLineRetrievedDataByPOLineId(DatabaseKey dbKey)
        {
           
                ReceiptImpLine_RetrievebyPOLineIdFromDatabase trans = new ReceiptImpLine_RetrievebyPOLineIdFromDatabase()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpLine = this.ToDatabaseObject();
                trans.ReceiptImpLine.SiteId = this.SiteId;

                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();

                UpdateFromDatabaseObject(trans.ReceiptImpLine);
           

        }


        //public void GetReceiptImpLineRetrievedDataByReceiptId(DatabaseKey dbKey)
        //{
           
        //        ReceiptImpLine_RetrievebyReceiptIdFromDatabase trans = new ReceiptImpLine_RetrievebyReceiptIdFromDatabase()
        //        {
        //            CallerUserInfoId = dbKey.User.UserInfoId,
        //            CallerUserName = dbKey.UserName,
        //        };
        //        trans.ReceiptImpLine = this.ToDatabaseObject();
        //        trans.ReceiptImpLine.SiteId = this.SiteId;

        //        trans.dbKey = dbKey.ToTransDbKey();
        //        trans.Execute();

        //        UpdateFromDatabaseObject(trans.ReceiptImpLine);
           

        //}

        public void GetReceiptImpLineRetrievedDataByEXReceiptTxnId(DatabaseKey dbKey)
        {
           
                ReceiptImpLine_RetrievebyEXReceiptTxnIdFromDatabase trans = new ReceiptImpLine_RetrievebyEXReceiptTxnIdFromDatabase()
                {
                    CallerUserInfoId = dbKey.User.UserInfoId,
                    CallerUserName = dbKey.UserName,
                };
                trans.ReceiptImpLine = this.ToDatabaseObject();
                trans.dbKey = dbKey.ToTransDbKey();
                trans.Execute();
                UpdateFromDatabaseObject(trans.ReceiptImpLine);
        }

    #endregion



    #region Create
    public void CreateCustom(DatabaseKey dbKey)
    {
      ReceiptImpLine_CreateCustom trans = new ReceiptImpLine_CreateCustom();
      trans.ReceiptImpLine = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.ReceiptImpLine);
    }
    #endregion

    #region Update
    public void UpdateCustom(DatabaseKey dbKey)
    {
      ReceiptImpLine_UpdateCustom trans = new ReceiptImpLine_UpdateCustom();
      trans.ReceiptImpLine = this.ToDatabaseObject();
      trans.ChangeLog = GetChangeLogObject(dbKey);
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure changed the Update Index.
      UpdateFromDatabaseObject(trans.ReceiptImpLine);
    }
    #endregion
  }
}



