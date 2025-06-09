using Common.Enumerations;
using DataContracts;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.IoTReading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class IoTReadingImportWrapper : CommonWrapper, IIoTReadingImportWrapper
    {
        public IoTReadingImportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Create and Save in table
        public ProcessLogModel CreateIoTReadingImport(List<IoTReadingImportJsonModel> ioTReadingImportJsonModelList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            IoTReadingImport _ioTReadingImport;
            IoTReadingImportModel ioTReadingImportModel = new IoTReadingImportModel();
            TotalProcess = ioTReadingImportJsonModelList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                foreach (var jItem in ioTReadingImportJsonModelList)
                {
                    _ioTReadingImport = new IoTReadingImport();
                    //--Retrieve header
                    ioTReadingImportModel.ClientId = _userData.DatabaseKey.Client.ClientId;
                    ioTReadingImportModel.SiteId = _userData.Site.SiteId;
                    ioTReadingImportModel.IoTDevice = jItem.IoTDevice ?? "";
                    ioTReadingImportModel.Reading = jItem.Reading=="" ? 0 : Convert.ToDecimal(jItem.Reading);
                    ioTReadingImportModel.ReadingDate = jItem.ReadingDate == null || jItem.ReadingDate.ToString() == "" ? DateTime.MinValue : jItem.ReadingDate;
                    ioTReadingImportModel.ReadingUnit = jItem.ReadingUnit ?? "";

                    // If an invalid site - skip for now
                    if (!string.IsNullOrEmpty(ioTReadingImportModel.IoTDevice))
                    {
                        _ioTReadingImport = SaveIoTReadingImport(ioTReadingImportModel);
                        ++NewTransactions;
                        if (_ioTReadingImport.ErrorMessage.Count() > 0)
                        {
                            return objProcessLogModel;
                        }
                    }

                    if (_ioTReadingImport.ErrorMessages == null || _ioTReadingImport.ErrorMessages.Count == 0)
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
        private IoTReadingImport SaveIoTReadingImport(IoTReadingImportModel ioTReadingImportModel)
        {
            IoTReadingImport ioTReadingImport = new IoTReadingImport();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                ioTReadingImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                ioTReadingImport.SiteId = _userData.Site.SiteId;
                ioTReadingImport.IoTDevice = ioTReadingImportModel.IoTDevice;
                ioTReadingImport.Reading = ioTReadingImportModel.Reading;
                ioTReadingImport.ReadingUnit = ioTReadingImportModel.ReadingUnit;
                ioTReadingImport.ReadingDate = ioTReadingImportModel.ReadingDate;
                ioTReadingImport.ErrorMessage = string.Empty; //check
                ioTReadingImport.LastProcess = System.DateTime.UtcNow;

                try
                {
                    ioTReadingImport.Create(_userData.DatabaseKey);
                }
                catch (Exception ex)
                {
                    errMsg = ex.ToString();
                    ioTReadingImport.ErrorMessage = errMsg;
                }
            }
            return ioTReadingImport;
        }
        #endregion

        #region Validation
        public List<IoTReadingErrorResponseModel> IoTReadingImportValidate(long logId)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            string errCodes = string.Empty;
            string errMsgs = string.Empty;
            IoTReadingErrorResponseModel objIoTReadingErrorModel;

            List<IoTReadingErrorResponseModel> ioTReadingErrorModelList = new List<IoTReadingErrorResponseModel>();
            IoTReadingImport IR = new IoTReadingImport()
            {
                ClientId = _userData.DatabaseKey.Client.ClientId,
                SiteId = _userData.Site.SiteId
            };
            List<IoTReadingImport> IRList = IR.IoTReadingImportRetrieveAll(_userData.DatabaseKey);
            TotalProcess = IRList.Count;
            if(TotalProcess>0)
            {
                foreach(IoTReadingImport _IRImp in IRList)
                {
                    objIoTReadingErrorModel = new IoTReadingErrorResponseModel();
                    errCodes = "";
                    errMsgs = "";
                    _IRImp.ErrorMessages = null;
                    _IRImp.CheckIoTReadingImportValidate(_userData.DatabaseKey);    //---validation 
                    if (_IRImp.ErrorMessages!=null && _IRImp.ErrorMessages.Count>0)
                    {
                        if(_IRImp.ErrorCodes!=null)
                        {
                            foreach(var v in _IRImp.ErrorCodd)
                            {
                                errCodes += v;
                                errCodes += ",";
                            }
                            errCodes = errCodes.TrimEnd(',');
                            foreach(var v in _IRImp.ErrorMessages)
                            {
                                errMsgs += v;
                                errMsgs += ",";
                                objIoTReadingErrorModel.IoTRErrMsgList.Add(v);
                            }
                            errMsgs = errMsgs.TrimEnd(',');
                            logMessage = logMessage + errMsgs;
                            // Add the error code(2) to the IoTReadingImport table
                            _IRImp.ErrorCodes = errCodes;
                            _IRImp.ErrorMessage = errMsgs;
                            _IRImp.LastProcess = DateTime.UtcNow;
                            _IRImp.Update(_userData.DatabaseKey);
                        }
                    }
                    else
                    {
                        // Process the record
                        _IRImp.PersonnelId = _userData.DatabaseKey.Personnel.PersonnelId;
                        _IRImp.GetIoTReadingImportProcess(_userData.DatabaseKey);
                        if (_IRImp.AlertName != "" && _IRImp.AlertName != null)
                        {
                            ProcessAlert objAlert = new ProcessAlert(_userData);
                            List<long> ioTEventIds = new List<long>() { _IRImp.IoTEventId };
                            if (_IRImp.AlertName == "APMMeterEvent")
                            {
                                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.IoTEvent>(AlertTypeEnum.APMMeterEvent, ioTEventIds));
                            }
                            else if (_IRImp.AlertName == "APMWarningEvent")
                            {
                                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.IoTEvent>(AlertTypeEnum.APMWarningEvent, ioTEventIds));
                            }
                            else if (_IRImp.AlertName == "APMCriticalEvent")
                            {
                                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.IoTEvent>(AlertTypeEnum.APMCriticalEvent, ioTEventIds));
                            }
                        }
                    }
                    if (errMsgs == null || errMsgs.Length == 0)
                    {
                        SuccessfulProcess++;
                    }
                    else
                    {
                        FailedProcess++;
                    }
                }
            }

            if (FailedProcess == 0)
            {
                logMessage = "Process Complete";
            }
            //---update log---
            CommonWrapper commonWrapper = new CommonWrapper(_userData);
            commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.SFTPIoTReadingImport);
            return ioTReadingErrorModelList;
        }
        #endregion
    }
}