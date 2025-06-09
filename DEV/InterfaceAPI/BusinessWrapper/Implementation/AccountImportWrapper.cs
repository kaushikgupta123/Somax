using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.PurchaseOrderReceipt;
using DataContracts;
using InterfaceAPI.Models.Account;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
  public class AccountImportWrapper : CommonWrapper, IAccountImportWrapper
  {
    //private DatabaseKey _dbKey;
    //private UserData _userData;
    // AccountImport _objAccountImport;
    public AccountImportWrapper(UserData userData) : base(userData)
    {
      _userData = userData;
      _dbKey = userData.DatabaseKey;
    }

    #region Create temporary table
    public ProcessLogModel CreateAccountImport(List<AccountImportModel> AccountImportModelList)
    {
      int TotalProcess = 0;
      int SuccessfulProcess = 0;
      int FailedProcess = 0;
      string logMessage = string.Empty;
      AccountImport _accountImport;
      TotalProcess = AccountImportModelList.Count;
      ProcessLogModel objProcessLogModel = new ProcessLogModel();
      foreach (var item in AccountImportModelList)
      {
        _accountImport = new AccountImport();
        //--Retrieve header
        _accountImport.ClientId = item.ClientId;
        _accountImport.SiteId = item.SiteId;
        _accountImport.AccountNumber = item.AccountNumber;
        _accountImport.ExAccountId = item.ExAccountId;

        _accountImport.GetAccoutnImportRetrievedDataByExAccountId(_userData.DatabaseKey);
        if (_accountImport.AccountImportId <= 0) //item does not exist in the table
        {
          //--Create Account--
          InsertAccount(_accountImport, item);
        }
        else  //item exists in the table
        {
          UpdateAccount(_accountImport, item, "", "");
        }
        if (_accountImport.ErrorMessages == null || _accountImport.ErrorMessages.Count == 0)
        {
          SuccessfulProcess++;
        }
        else
        {
          FailedProcess++;
        }
      }//end of foreach
      if (FailedProcess == 0)
      {
        logMessage = "Json data inserted into temporary table.";
      }
      else
      {
        logMessage = "Conversion of json data to temporary data tables failed.";
      }
      objProcessLogModel.TotalProcess = TotalProcess;
      objProcessLogModel.SuccessfulProcess = SuccessfulProcess;
      objProcessLogModel.FailedProcess = FailedProcess;
      objProcessLogModel.logMessage = logMessage;
      return objProcessLogModel;
    }
    #endregion

    #region Validate
    public List<AccountErrorResponseModel> AccountImportValidate(long logId, List<AccountImportModel> AccountImportModelList)
    {
      int TotalProcess = 0;
      int SuccessfulProcess = 0;
      int FailedProcess = 0;
      string logMessage = string.Empty;
      string errCodes = string.Empty;
      string errMsgs = string.Empty;

      AccountErrorResponseModel objAccountErrorResponseModel;

      List<AccountErrorResponseModel> AccountErrorResponseList = new List<AccountErrorResponseModel>();
      // V2-416 
      // At this point we have created AccountImport records from the models
      // Process the records - not the models.
      // Retrieve all the records for this client
      AccountImport AI = new AccountImport()
      {
        ClientId = _userData.DatabaseKey.Client.ClientId
      };
      List<AccountImport> AIList = AI.AccountImportRetrieveAll(_userData.DatabaseKey);

      TotalProcess = AIList.Count;
      foreach (AccountImport _AI in AIList)
      {
        objAccountErrorResponseModel = new AccountErrorResponseModel();
        errCodes = "";
        errMsgs = "";
        _AI.CheckAccountImportValidate(_userData.DatabaseKey);
        if (_AI.ErrorMessages != null && _AI.ErrorMessages.Count > 0) //---validation fails
        {
          objAccountErrorResponseModel.ExAccountId = (int)_AI.ExAccountId;
          errCodes = "";
          errMsgs = "";
          if (_AI.ErrorCodd != null)
          {
            foreach (var v in _AI.ErrorCodd)
            {
              errCodes += v;
              errCodes += ",";
            }
            errCodes = errCodes.TrimEnd(',');
          }
          foreach (var v in _AI.ErrorMessages)
          {
            errMsgs += v;
            errMsgs += ",";
            objAccountErrorResponseModel.AcctErrMsgList.Add(v);
          }
          errMsgs = errMsgs.TrimEnd(',');

          // Update the accountimport entry with the error messages
          // Add the error code(2) to the AccountImport table
          _AI.ErrorCodes = errCodes;
          _AI.ErrorMessage = errMsgs;
          _AI.LastProcess = DateTime.UtcNow;
          _AI.UpdateCustom(_userData.DatabaseKey);
          logMessage = logMessage + errMsgs;
        }
        else //----validation successful
        {
          //---process the account import record
          _AI.GetAccountImportProcessImport(_userData.DatabaseKey);

        }
        if (objAccountErrorResponseModel.AcctErrMsgList.Count() > 0)
        {
          AccountErrorResponseList.Add(objAccountErrorResponseModel);
        }
        if (errMsgs == null || errMsgs.Length == 0)
        {
          SuccessfulProcess++;
        }
        else
        {
          FailedProcess++;
        }
      }//end of foreach
      if (FailedProcess == 0)
      {
        logMessage = "Process Complete – Account import successful ";
      }
      //---update log---
      CommonWrapper commonWrapper = new CommonWrapper(_userData);
      commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.DeanFoodsAccountImport);
      return AccountErrorResponseList;
    }
    #endregion

    #region Common
    private void InsertAccount(AccountImport _accountImport, AccountImportModel item)
    {
      _accountImport.ClientId = item.ClientId;
      _accountImport.SiteId = item.SiteId;
      _accountImport.ClientLookupId = item.AccountNumber;
      _accountImport.ExAccountId = item.ExAccountId;
      _accountImport.Name = item.Name;
      _accountImport.Enabled = item.Status;
      ////_accountImport.EmailAddress = item.EmailAddress;//-- not in db
      _accountImport.CreateCustom(_userData.DatabaseKey);
    }
    private void UpdateAccount(AccountImport _accountImport, AccountImportModel item, string ErrorCodes, string ErrorMessage)
    {
      _accountImport.ClientId = item.ClientId;
      _accountImport.SiteId = item.SiteId;
      _accountImport.ClientLookupId = item.AccountNumber;
      _accountImport.ExAccountId = item.ExAccountId;
      _accountImport.Name = item.Name;
      _accountImport.Enabled = item.Status;
      ////_accountImport.EmailAddress = item.EmailAddress;//-- not in db
      _accountImport.ErrorCodes = ErrorCodes;
      _accountImport.ErrorMessage = ErrorMessage;
      _accountImport.LastProcess = DateTime.UtcNow;
      _accountImport.UpdateCustom(_userData.DatabaseKey);
    }
    #endregion
  }
}