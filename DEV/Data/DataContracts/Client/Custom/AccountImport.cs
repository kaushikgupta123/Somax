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


  public partial class AccountImport : DataContractBase, IStoredProcedureValidation
  {

    public string AccountNumber { get; set; }
    public int spErrCode { get; set; }
    #region Validation Methods
    public string ValidateFor = string.Empty;

    public void CheckAccountImportValidate(DatabaseKey dbKey)
    {
      ValidateFor = "ValidateImport";
      Validate<AccountImport>(dbKey);
    }
    public List<StoredProcValidationError> RetrieveStoredProcValidationData(DatabaseKey dbKey)
    {
      List<StoredProcValidationError> errors = new List<StoredProcValidationError>();


      if (ValidateFor == "ValidateImport")
      {
        AccountImport_Validation trans = new AccountImport_Validation()
        {
          CallerUserInfoId = dbKey.User.UserInfoId,
          CallerUserName = dbKey.UserName,
        };
        trans.AccountImport = this.ToDatabaseObject();
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


    #region  AccountImport Process 

    public void GetAccountImportProcessImport(DatabaseKey dbKey)
    {


      AccountImport_ProcessImport trans = new AccountImport_ProcessImport()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,
      };
      trans.AccountImport = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      UpdateFromDatabaseObject(trans.AccountImport);
      this.spErrCode = trans.AccountImport.spErrCode;

    }

    #endregion


    #region Retrieve
    /// <summary>
    /// V2-416 - Retrieve all for the current Client
    /// </summary>
    /// <param name="dbKey"></param>
    /// <returns></returns>
    public List<AccountImport> AccountImportRetrieveAll(DatabaseKey dbKey)
    {
      AccountImport_RetrieveAll trans = new AccountImport_RetrieveAll()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName
      };

      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();
      return UpdateFromDatabaseObjectList(trans.AccountImportList);
    }
    /// <summary>
    /// V2-416 - Load the DB Object List 
    /// </summary>
    /// <param name="dbKey"></param>
    /// <returns></returns>
    public static List<AccountImport> UpdateFromDatabaseObjectList(List<b_AccountImport> dbObjs)
    {
      List<AccountImport> result = new List<AccountImport>();

      foreach (b_AccountImport dbObj in dbObjs)
      {
        AccountImport tmp = new AccountImport();
        tmp.UpdateFromDatabaseObject(dbObj);
        result.Add(tmp);
      }
      return result;
    }


    //public void GetAccoutnImportRetrievedDataByAccountNumber(DatabaseKey dbKey)
    //{

    //    AccountImport_RetrieveByAccountNumber trans = new AccountImport_RetrieveByAccountNumber()
    //    {
    //        CallerUserInfoId = dbKey.User.UserInfoId,
    //        CallerUserName = dbKey.UserName,
    //    };
    //    trans.AccountImport = this.ToDatabaseObject();


    //    trans.dbKey = dbKey.ToTransDbKey();
    //    trans.Execute();

    //    UpdateFromDatabaseObject(trans.AccountImport);

    //}



    public void GetAccoutnImportRetrievedDataByExAccountId(DatabaseKey dbKey)
    {

      AccountImport_RetrieveByExAccountId trans = new AccountImport_RetrieveByExAccountId()
      {
        CallerUserInfoId = dbKey.User.UserInfoId,
        CallerUserName = dbKey.UserName,
      };
      trans.AccountImport = this.ToDatabaseObject();
      trans.AccountImport.SiteId = this.SiteId;

      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      UpdateFromDatabaseObject(trans.AccountImport);

    }
    #endregion


    #region Create
    public void CreateCustom(DatabaseKey dbKey)
    {
      AccountImport_CreateCustom trans = new AccountImport_CreateCustom();
      trans.AccountImport = this.ToDatabaseObject();
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure may have populated an auto-incremented key. 
      UpdateFromDatabaseObject(trans.AccountImport);
    }
    #endregion

    #region Update
    public void UpdateCustom(DatabaseKey dbKey)
    {
      AccountImport_UpdateCustom trans = new AccountImport_UpdateCustom();
      trans.AccountImport = this.ToDatabaseObject();
      trans.ChangeLog = GetChangeLogObject(dbKey);
      trans.dbKey = dbKey.ToTransDbKey();
      trans.Execute();

      // The create procedure changed the Update Index.
      UpdateFromDatabaseObject(trans.AccountImport);
    }
    #endregion
  }
}



