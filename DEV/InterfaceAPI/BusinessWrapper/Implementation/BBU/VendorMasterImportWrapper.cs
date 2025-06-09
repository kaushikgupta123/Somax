using InterfaceAPI.BusinessWrapper.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using InterfaceAPI.Models;
using InterfaceAPI.Models.Common;
using DataContracts;
using InterfaceAPI.Models.BBU.VendorMasterImport;
using InterfaceAPI.BusinessWrapper.Interface.BBU;
using Common.Constants;

namespace InterfaceAPI.BusinessWrapper.Implementation.BBU
{
    public class VendorMasterImportWrapper : CommonWrapper, Interface.BBU.IVendorMasterImportWrapper
    {
        public VendorMasterImportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }


        #region Test Upload File
        public void EncryptandUploadFile(string fileName, string apiName)
        {
            EncryptandUploadFileToSFTP(fileName, apiName);
        }
        #endregion

        #region Download files in Local
        public string DownloadToLocal(string fileName, string apiName)
        {
            string filePath = string.Empty;
            filePath = DownloadFileToLocalDirectory(fileName, apiName);
            return filePath;
        }
        #endregion

        #region Decrypt File
        public string DecryptFile(string fileName, string encryptFilePath, string apiName)
        {
            return DecryptLocalFile(fileName, encryptFilePath, apiName);
        }
        #endregion

        #region Archive File
        public string ArchiveFileWithDBValue(string SourceFilePath, string sourceFileName, string DestinationPath)
        {
            return ArchiveFileWithDBvalue(SourceFilePath, sourceFileName, DestinationPath);
        }
        #endregion

        #region Create and Save in table
        public ProcessLogModel CreateVMImport(List<VendorMasterImportJSONModel> vmImportModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            VendorMasterImport _vmImport;
            VendorMasterImportModel Vmi = new VendorMasterImportModel();
            TotalProcess = vmImportModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            { 
                foreach (var jItem in vmImportModelList)
                {
                    Vmi.ClientId = _userData.DatabaseKey.Client.ClientId;
                    Vmi.ExVendorId = Convert.ToInt64(jItem.ORACLE_VENDOR_ID);
                    Vmi.ClientLookupId = jItem.ORACLE_VENDOR_NUMBER;
                    Vmi.Name = jItem.VENDOR_NAME;
                    Vmi.Type = jItem.VENDOR_TYPE;
                    Vmi.Enabled = jItem.ENABLED_FLAG;
                    Vmi.ExVendorSiteId = jItem.ORACLE_VENDOR_SITE_ID;
                    Vmi.ExVendorSiteCode = jItem.ORACLE_VENDOR_SITE_CODE;
                    Vmi.Address1 = jItem.ADDRESS_LINE1;
                    Vmi.Address2 = jItem.ADDRESS_LINE2;
                    Vmi.Address3 = jItem.ADDRESS_LINE3;
                    Vmi.AddressCity = jItem.CITY;
                    Vmi.AddressState = jItem.STATE;
                    Vmi.AddressCountry = jItem.COUNTRY;
                    Vmi.AddressPostCode = jItem.ZIP;
                    Vmi.InactiveDate = jItem.INACTIVE_DATE == null || jItem.INACTIVE_DATE.ToString() == "" ? DateTime.MinValue : jItem.INACTIVE_DATE;                  

                    _vmImport = SaveVendorMasterImport(Vmi);
                    ++NewTransactions;
                    if (_vmImport.ErrorMessage.Count() > 0)
                    {
                        return objProcessLogModel;
                    }

                    if (_vmImport.ErrorMessages == null || _vmImport.ErrorMessages.Count == 0)
                    {
                        SuccessfulProcess++;
                    }
                    else
                    {
                        FailedProcess++;
                    }
                }//end of foreach
            }
            catch (Exception ex)
            {

            }
            if (FailedProcess == 0)
            {
                logMessage = "Json data inserted into temporary table.";
            }
            else
            {
                logMessage = "Conversion of json data to temporary data tables failed.";
            }
            objProcessLogModel.NewProcess = NewTransactions;
            objProcessLogModel.TotalProcess = TotalProcess;
            objProcessLogModel.SuccessfulProcess = SuccessfulProcess;
            objProcessLogModel.FailedProcess = FailedProcess;
            objProcessLogModel.logMessage = logMessage;
            return objProcessLogModel;
        }
        private VendorMasterImport SaveVendorMasterImport(VendorMasterImportModel Vmi)
        {
            VendorMasterImport VendorMasterImport = new VendorMasterImport();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                VendorMasterImport vendorMasterImport = new VendorMasterImport();
                vendorMasterImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                vendorMasterImport.ClientLookupId = Vmi.ClientLookupId;
                vendorMasterImport.ExVendorId = Vmi.ExVendorId;
                vendorMasterImport.ExVendorSiteId = Vmi.ExVendorSiteId;
                vendorMasterImport.ExVendorSiteCode = Vmi.ExVendorSiteCode;
                vendorMasterImport.Name = Vmi.Name;
                vendorMasterImport.Type = Vmi.Type;
                vendorMasterImport.Enabled = Vmi.Enabled;
                vendorMasterImport.ExVendorSiteId = Vmi.ExVendorSiteId;
                vendorMasterImport.ExVendorSiteCode = Vmi.ExVendorSiteCode;
                vendorMasterImport.Address1 = Vmi.Address1;
                vendorMasterImport.Address2 = Vmi.Address2;
                vendorMasterImport.Address3 = Vmi.Address3;
                vendorMasterImport.AddressCity = Vmi.AddressCity;
                vendorMasterImport.AddressState = Vmi.AddressState;
                vendorMasterImport.AddressCountry = Vmi.AddressCountry;
                vendorMasterImport.AddressPostCode = Vmi.AddressPostCode;
                vendorMasterImport.ErrorMessage = String.Empty;// check;
                vendorMasterImport.LastProcess = System.DateTime.UtcNow;
                vendorMasterImport.InactiveDate = Vmi.InactiveDate;
                // Check if vendormasterimport record already exists
                // Match on ClientId, ExVendorId, EXVendorSiteId       
                VendorMasterImport check_VMI = new VendorMasterImport();
                check_VMI.ClientId = Vmi.ClientId;
                check_VMI.ExVendorId = Vmi.ExVendorId;
                check_VMI.ExVendorSiteId = Vmi.ExVendorSiteId;
                check_VMI.RetrieveByClientIdVendorExIdVendorExSiteId(_userData.DatabaseKey);

                try
                {
                    // If it does exists - update with values in table
                    if (check_VMI.VendorMasterImportId > 0)
                    {
                        vendorMasterImport.VendorMasterImportId = check_VMI.VendorMasterImportId;
                        vendorMasterImport.UpdateIndex = check_VMI.UpdateIndex;
                        vendorMasterImport.Update(_userData.DatabaseKey);
                    }
                    // If it does not exist - add a new one
                    else
                    {
                        vendorMasterImport.Create(_userData.DatabaseKey);
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    VendorMasterImport.ErrorMessage = errMsg;
                }
            }
            return VendorMasterImport;
        }
        #endregion

        #region Process Vendor Master Import
        public ProcessLogModel ProcessVMImport()
        {
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            VendorMasterImport vmImport = new VendorMasterImport();
            List<VendorMasterImport> vmImlist = vmImport.VendorMasterImportRetrieveAll(_userData.DatabaseKey);
            int TotalProcess = vmImlist.Count;
            int SucessfulTransaction = 0;
            int FailTransaction = 0;
            foreach (var item in vmImlist)
            {
                VendorMasterImport VendorMasterImportinterface = new VendorMasterImport();
                VendorMasterImportinterface.CallerUserInfoId = _userData.DatabaseKey.User.UserInfoId;
                VendorMasterImportinterface.CallerUserName = _userData.DatabaseKey.UserName;
                VendorMasterImportinterface.ClientId = _userData.DatabaseKey.Client.ClientId;
                VendorMasterImportinterface.VendorMasterImportId = item.VendorMasterImportId;
                try
                {
                    //There is no Store Procedure Validation. So, we put it in try catch block
                    VendorMasterImportinterface.Create_VendorMasterProcessInterface(_userData.DatabaseKey);
                    //VendorMasterImportinterface.Delete(_userData.DatabaseKey);
                    // Delete is handled by the SP
                    if (VendorMasterImportinterface.error_message_count == 0)
                    {
                        SucessfulTransaction++;
                        // Delete is actually handled in the stored procedure
                        //PartMasterImportinterface.Delete(UserData.DatabaseKey);
                    }
                    else
                    {
                        FailTransaction++;
                    }
                    objProcessLogModel.SuccessfulProcess = SucessfulTransaction;
                    objProcessLogModel.FailedProcess = FailTransaction;
                }
                catch (Exception ex)
                {
                    objProcessLogModel.logMessage = ex.Message.ToString();
                }
            }
            if (FailTransaction == 0)
            {
                objProcessLogModel.logMessage = "Process Complete – All items processed successfully";
            }
            else
            {
                objProcessLogModel.logMessage = "Process Complete – Not all items were processed successfully";
            }

            return objProcessLogModel;
        }
        #endregion
    }

}