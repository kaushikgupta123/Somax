using Common.Enumerations;
using DataContracts;
using InterfaceAPI.BusinessWrapper.Interface;
using InterfaceAPI.Models.Common;
using InterfaceAPI.Models.MonnitIoTReading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterfaceAPI.BusinessWrapper.Implementation
{
    public class MonnitIoTReadingImportWrapper : CommonWrapper, IMonnitIoTReadingImportWrapper
    {
        public MonnitIoTReadingImportWrapper(UserData userData) : base(userData)
        {
            _userData = userData;
            _dbKey = userData.DatabaseKey;
        }

        #region Create and Save in table
        public ProcessLogModel CreateIoTReadingImport(List<MonnitIoTReadingImportJsonModel> MonnitIoTReadingImportList)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            int NewTransactions = 0;
            string logMessage = string.Empty;
            IoTReadingImport _IoTReadingImport;
            MonnitIoTReadingImportModel monnitIoTReadingImportModel = new MonnitIoTReadingImportModel();
            TotalProcess = MonnitIoTReadingImportList.Count;
            ProcessLogModel objProcessLogModel = new ProcessLogModel();
            try
            {
                foreach (var jItem in MonnitIoTReadingImportList)
                {
                    _IoTReadingImport = new IoTReadingImport();
                    //--Retrieve header
                    monnitIoTReadingImportModel.ClientId = _userData.DatabaseKey.Client.ClientId;
                    monnitIoTReadingImportModel.SiteId = _userData.Site.SiteId;
                    monnitIoTReadingImportModel.sensorID = jItem.sensorID ?? "";
                    monnitIoTReadingImportModel.sensorName = jItem.sensorName ?? "";
                    monnitIoTReadingImportModel.applicationID = jItem.applicationID ?? "";
                    monnitIoTReadingImportModel.networkID = jItem.networkID ?? "";
                    monnitIoTReadingImportModel.dataMessageGUID = jItem.dataMessageGUID ?? "";
                    monnitIoTReadingImportModel.state = jItem.state??0;
                    monnitIoTReadingImportModel.messageDate = jItem.messageDate == null || jItem.messageDate.ToString() == "" ? DateTime.MinValue : jItem.messageDate;
                    monnitIoTReadingImportModel.rawData = jItem.rawData == "" ? 0 : Convert.ToDecimal(jItem.rawData);
                    monnitIoTReadingImportModel.datatype = jItem.datatype ?? "";
                    monnitIoTReadingImportModel.dataValue = jItem.dataValue == "" ? 0 : Convert.ToDecimal(jItem.dataValue);
                    monnitIoTReadingImportModel.plotValues = jItem.plotValues == "" ? 0 : Convert.ToDecimal(jItem.plotValues);
                    monnitIoTReadingImportModel.plotLabels = jItem.plotLabels ?? "";
                    monnitIoTReadingImportModel.batteryLevel = jItem.batteryLevel ?? 0;
                    monnitIoTReadingImportModel.signalStrength = jItem.signalStrength ?? 0;
                    monnitIoTReadingImportModel.pendingChange = jItem.pendingChange ?? "";
                    monnitIoTReadingImportModel.voltage = jItem.voltage == "" ? 0 : Convert.ToDecimal(jItem.voltage);
                    // If an invalid site - skip for now
                    if (!string.IsNullOrEmpty(monnitIoTReadingImportModel.sensorID))
                    {
                        _IoTReadingImport = SaveIoTReadingImport(monnitIoTReadingImportModel);
                        ++NewTransactions;
                        if (_IoTReadingImport.ErrorMessage.Count() > 0)
                        {
                            return objProcessLogModel;
                        }
                    }

                    if (_IoTReadingImport.ErrorMessages == null || _IoTReadingImport.ErrorMessages.Count == 0)
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
        private IoTReadingImport SaveIoTReadingImport(MonnitIoTReadingImportModel monnitIoTReadingImportModel)
        {
            IoTReadingImport ioTReadingImport = new IoTReadingImport();
            string errMsg = "";
            Boolean valid = true;
            if (valid)
            {
                ioTReadingImport.ClientId = _userData.DatabaseKey.Client.ClientId;
                ioTReadingImport.SiteId = _userData.Site.SiteId;
                ioTReadingImport.IoTDevice = monnitIoTReadingImportModel.sensorID;
                ioTReadingImport.Reading = monnitIoTReadingImportModel.plotValues;
                ioTReadingImport.ReadingDate = monnitIoTReadingImportModel.messageDate;
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
        public List<MonnitIoTReadingErrorResponseModel> IoTReadingImportValidate(long logId)
        {
            int TotalProcess = 0;
            int SuccessfulProcess = 0;
            int FailedProcess = 0;
            string logMessage = string.Empty;
            string errCodes = string.Empty;
            string errMsgs = string.Empty;
            MonnitIoTReadingErrorResponseModel objIoTReadingErrorModel;

            List<MonnitIoTReadingErrorResponseModel> ioTReadingErrorModelList = new List<MonnitIoTReadingErrorResponseModel>();
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
                    objIoTReadingErrorModel = new MonnitIoTReadingErrorResponseModel();
                    errCodes = "";
                    errMsgs = "";
                    _IRImp.ErrorMessages = null;
                    _IRImp.CheckIoTReadingImportValidate(_userData.DatabaseKey);    //---validation  remove ??? 
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
                            List<long> IoTEventIds = new List<long>() { _IRImp.IoTEventId };
                            if (_IRImp.AlertName == "APMWarningEvent")
                            {
                                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.IoTEvent>(AlertTypeEnum.APMWarningEvent, IoTEventIds));
                            }
                            else if (_IRImp.AlertName == "APMCriticalEvent")
                            {
                                Task CreateAlertTask = Task.Factory.StartNew(() => objAlert.CreateAlert<DataContracts.IoTEvent>(AlertTypeEnum.APMCriticalEvent, IoTEventIds));
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
            commonWrapper.UpdateLog(logId, TotalProcess, SuccessfulProcess, FailedProcess, logMessage, Common.Constants.ApiConstants.MonnitIoTReadingImport);
            return ioTReadingErrorModelList;
        }
        #endregion
    }
}