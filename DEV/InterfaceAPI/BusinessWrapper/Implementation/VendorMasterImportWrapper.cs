using Business.Authentication;
using Common.Enumerations;
using DataContracts;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
  public class VendorMasterImportWrapper : CommonWrapper, IVendorMasterImportWrapper
  {
    public VendorMasterImportWrapper(UserData userData) : base(userData)
    {
      _userData = userData;
      _dbKey = userData.DatabaseKey;

    }

    #region Insert
    public ProcessLogModel InsertVendarMasterImportData(List<VendorMasterImportModel> VendorImportModelObjectList)
    {
      int TotalProcess = 0;
      int SuccessfulProcess = 0;
      int FailedProcess = 0;
      string logMessage = string.Empty;
      List<VendorMasterImportModel> VOImpMaster = new List<VendorMasterImportModel>();
      VendorMasterImport _VM;
      VendorMaster VM;
      TotalProcess = VendorImportModelObjectList.Count;
      ProcessLogModel objProcessLogModel = new ProcessLogModel();
      VendorMasterImportModel objVendorMasterImportModel = new VendorMasterImportModel();

      foreach (var item in VendorImportModelObjectList)
      {
        _VM = new VendorMasterImport();
        _VM.ClientId = item.ClientId;
        _VM.SiteId = item.SiteId;
        _VM.ExVendorId = item.ExVendorId;
        _VM.GetVendorImportRetrievedDataByExVendorId(_userData.DatabaseKey);
        if (_VM.VendorMasterImportId <= 0)
        {
          InsertVMImp(_VM, item);
        }
        else
        {
          UpdateVMimp(_VM, item, "", "");
        }
        if (_VM.ErrorMessages == null || _VM.ErrorMessages.Count == 0)
        {
          SuccessfulProcess++;
        }
        else
        {
          FailedProcess++;
        }
      }
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

    #region Validation
    // V2-416
    //public List<VendorMasterErrorModel> VendorMasterImportValidate(long logId, List<VendorMasterImportModel> VendorMasterImportModelObjectList)
    public List<VendorMasterErrorModel> VendorMasterImportValidate(long logId)
    {
      // V2-416
      // RKL 2020-10-29 
      // This is called from both the CSV and non-CSV file branches
      // At this point - we have "converted" the json string arrays to VendorImportMaster records
      // So - we will process those records rather than the VendorMasterObject List
      int TotalProcess = 0;
      int SuccessfulProcess = 0;
      int FailedProcess = 0;
      string logMessage = string.Empty;
      string errCodes = string.Empty;
      string errMsgs = string.Empty;
      VendorMasterErrorModel objVendorMasterErrorModel;

      List<VendorMasterErrorModel> VendorMasterErrorModellst = new List<VendorMasterErrorModel>();
      // V2-416 - retrieve all VendorMasterImport records for the client
      VendorMasterImport VM = new VendorMasterImport()
      {
        ClientId = _userData.DatabaseKey.Client.ClientId
      };
      List<VendorMasterImport> VMList = VM.VendorMasterImportRetrieveAll(_userData.DatabaseKey);
      TotalProcess = VMList.Count;
      if (TotalProcess > 0)
      {
        foreach (VendorMasterImport _VMImp in VMList)
        {
          objVendorMasterErrorModel = new VendorMasterErrorModel();
          errCodes = "";
          errMsgs = "";
          _VMImp.ErrorMessages = null;
          _VMImp.CheckValidiateMasterImport(_userData.DatabaseKey);           //---validation 
          if (_VMImp.ErrorMessages != null && _VMImp.ErrorMessages.Count > 0) //---validation fails
          {
            if (_VMImp.ErrorCodd != null)
            {
              foreach (var v in _VMImp.ErrorCodd)
              {
                errCodes += v;
                errCodes += ",";
              }
              errCodes = errCodes.TrimEnd(',');
              foreach (var v in _VMImp.ErrorMessages)
              {
                errMsgs += v;
                errMsgs += ",";
                objVendorMasterErrorModel.VendorMasterErrMsgList.Add(v);
              }
              errMsgs = errMsgs.TrimEnd(',');
              logMessage = logMessage + errMsgs;
              // Add the error code(2) to the VendorMasterImport table
              _VMImp.ErrorCodes = errCodes;
              _VMImp.ErrorMessage = errMsgs;
              _VMImp.LastProcess = DateTime.UtcNow;
              _VMImp.UpdateCustom(_userData.DatabaseKey);
            }
          }
          else //---validation successful
          {
            // Process the record
            _VMImp.GetVendorMasteImportProcess(_userData.DatabaseKey);
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
      } // Endof of the If TotalProcess > 0

      if (FailedProcess == 0)
      {
        logMessage = "Process Complete – Vendor Master import successful ";
      }
      //---update log---
      CommonWrapper commonWrapper = new CommonWrapper(_userData);
      commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.DeanFoodsVendorMasterImport);
      return VendorMasterErrorModellst;
    }
    #endregion

    #region Common
    private void InsertVMImp(VendorMasterImport vmasterimport, VendorMasterImportModel item)
    {
      vmasterimport.ClientLookupId = item.VendorNumber;
      vmasterimport.ExVendorId = item.ExVendorId;
      vmasterimport.ExVendorSiteId = 0;
      vmasterimport.Name = item.Name;
      vmasterimport.Address1 = item.Address1;
      vmasterimport.Address2 = item.Address2;
      vmasterimport.Address3 = item.Address3;
      vmasterimport.AddressCity = item.AddressCity;
      vmasterimport.AddressState = item.AddressState;
      vmasterimport.AddressPostCode = item.AddressPostCode;
      vmasterimport.AddressCountry = item.AddressCountry;
      vmasterimport.RemitAddress1 = item.RemitAddress1;
      vmasterimport.RemitAddress2 = item.RemitAddress2;
      vmasterimport.RemitAddress3 = item.RemitAddress3;
      vmasterimport.RemitAddressCity = item.RemitAddressCity;
      vmasterimport.RemitAddressState = item.RemitAddressState;
      vmasterimport.RemitAddressPostCode = item.RemitAddressPostCode;
      vmasterimport.RemitAddressCountry = item.RemitAddressCountry;
      if (item.AddressRemitUseBus.ToLower() == "yes")
      {
        vmasterimport.RemitUseBusiness = true;
      }
      else if (item.AddressRemitUseBus == "" || item.AddressRemitUseBus.ToLower() == "no")
      {
        vmasterimport.RemitUseBusiness = false;
      }
      else
      {
        vmasterimport.RemitUseBusiness = false;
      }
      vmasterimport.Enabled = item.Status;
      vmasterimport.FaxNumber = item.FaxNumber;
      vmasterimport.PhoneNumber = item.PhoneNumber;
      vmasterimport.EmailAddress = item.EmailAddress;
      vmasterimport.Terms = item.TermCode;
      vmasterimport.TermsDesc = item.TermDescription;
      vmasterimport.ErrorCodes = "";
      vmasterimport.ErrorMessage = "";
      vmasterimport.LastProcess = null;
      vmasterimport.CreateCustom(_userData.DatabaseKey);
    }


    private void UpdateVMimp(VendorMasterImport vmasterimport, VendorMasterImportModel item, string ErrorCodes, string ErrorMessage)
    {
      vmasterimport.ClientLookupId = item.VendorNumber;
      vmasterimport.Name = item.Name;
      vmasterimport.Address1 = item.Address1;
      vmasterimport.Address2 = item.Address2;
      vmasterimport.Address3 = item.Address3;
      vmasterimport.AddressCity = item.AddressCity;
      vmasterimport.AddressState = item.AddressState;
      vmasterimport.AddressPostCode = item.AddressPostCode;
      vmasterimport.AddressCountry = item.AddressCountry;
      vmasterimport.RemitAddress1 = item.RemitAddress1;
      vmasterimport.RemitAddress2 = item.RemitAddress2;
      vmasterimport.RemitAddress3 = item.RemitAddress3;
      vmasterimport.RemitAddressCity = item.RemitAddressCity;
      vmasterimport.RemitAddressState = item.RemitAddressState;
      vmasterimport.RemitAddressPostCode = item.RemitAddressPostCode;
      vmasterimport.RemitAddressCountry = item.RemitAddressCountry;
      if (item.AddressRemitUseBus.ToLower() == "yes")
      {
        vmasterimport.RemitUseBusiness = true;
      }
      else if (item.AddressRemitUseBus == "" || item.AddressRemitUseBus.ToLower() == "no")
      {
        vmasterimport.RemitUseBusiness = false;
      }
      else
      {
        vmasterimport.RemitUseBusiness = false;
      }
      vmasterimport.Enabled = item.Status;
      vmasterimport.FaxNumber = item.FaxNumber;
      vmasterimport.PhoneNumber = item.PhoneNumber;
      vmasterimport.EmailAddress = item.EmailAddress;
      vmasterimport.Terms = item.TermCode;
      vmasterimport.TermsDesc = item.TermDescription;
      vmasterimport.ErrorCodes = ErrorCodes;
      vmasterimport.ErrorMessage = ErrorMessage;
      vmasterimport.LastProcess = DateTime.UtcNow;
      vmasterimport.UpdateCustom(_userData.DatabaseKey);
    }

    #endregion

  }
}