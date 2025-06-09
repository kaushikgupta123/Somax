using Client.Models.Meters;
using Common.Constants;
using Common.Enumerations;
using DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client.BusinessWrapper.Meters
{
    public class MetersWrapper
    {
        private DatabaseKey _dbKey;
        private UserData userData;
        public List<long> WOGen = new List<long>();
        public MetersWrapper(UserData userData)
        {
            this.userData = userData;
            _dbKey = userData.DatabaseKey;
        }
        #region Search
        internal List<MetersModel> GetMeterGridData()
        {
            MetersModel objMetersModel;
            List<MetersModel> meterList = new List<MetersModel>();
            DataContracts.Meter meter = new DataContracts.Meter();
            meter.SiteId = userData.DatabaseKey.User.DefaultSiteId;
            List<DataContracts.Meter> meters = meter.RetrieveForSearchBySiteAndReadingDate_V2(this.userData.DatabaseKey);
            foreach (var item in meters)
            {
                objMetersModel = new MetersModel();
                objMetersModel.MeterClientLookUpId = item.ClientLookupId;
                objMetersModel.MeterId = item.MeterId;
                objMetersModel.MeterName = item.Name;
                objMetersModel.ReadingCurrent = item.ReadingCurrent;
                objMetersModel.StringReadingCurrent = Convert.ToString(item.ReadingCurrent);
                if (item.ReadingDate != null && item.ReadingDate == default(DateTime))
                {
                    objMetersModel.ReadingDate = null;
                }
                else
                {
                    objMetersModel.ReadingDate = item.ReadingDate;
                }
                objMetersModel.ReadingBy = item.ReadingBy;
                objMetersModel.PersonnelClientLookUpId = item.PersonnelClientLookUpId;
                objMetersModel.ReadingLife = item.ReadingLife;
                objMetersModel.StringReadingLife = Convert.ToString(item.ReadingLife);
                objMetersModel.MaxReading = item.ReadingMax;
                objMetersModel.StringMaxReading = Convert.ToString(item.ReadingMax);
                objMetersModel.InActive = item.InactiveFlag;
                objMetersModel.ReadingUnits = item.ReadingUnits;
                meterList.Add(objMetersModel);
            }
            return meterList;
        }
        #endregion
        #region Add Or Edit
        public List<String> AddMeter(MetersModel metersModel, ref long MeterId)
        {
            DataContracts.Meter Meter = new DataContracts.Meter();
            Meter.SiteId = this.userData.DatabaseKey.Personnel.SiteId;
            Meter.ClientId = this.userData.DatabaseKey.Personnel.ClientId;           
            Meter.ClientLookupId = metersModel.MeterClientLookUpId;
            Meter.Name = metersModel.MeterName;
            if (!string.IsNullOrEmpty(metersModel.MaxReading.ToString()))
            {
                Meter.ReadingMax = Convert.ToDecimal(metersModel.MaxReading.ToString());
            }
            else
            {
                Meter.ReadingMax = 0;
            }
            Meter.ReadingUnits = metersModel.ReadingUnits;
            Meter.ReadingBy = Convert.ToInt64(userData.DatabaseKey.Personnel.PersonnelId);
            Meter.CreateMode = true;
            Meter.CreateByForeignKeys(this.userData.DatabaseKey);
            if (Meter.ErrorMessages.Count == 0)
            {
                MeterId = Meter.MeterId;
            }
            return Meter.ErrorMessages;
        }

        public List<String> EditMeter(MetersModel metersModel, ref long MeterId)
        {
            DataContracts.Meter Meter = new DataContracts.Meter();
            Meter.MeterId = metersModel.MeterId;          
            Meter.ClientId = this.userData.DatabaseKey.Personnel.ClientId;
            Meter.Retrieve(userData.DatabaseKey);           
            Meter.ClientLookupId = metersModel.MeterClientLookUpId;
            Meter.Name = metersModel.MeterName;
            if (!string.IsNullOrEmpty(metersModel.MaxReading.ToString()))
            {
                Meter.ReadingMax = Convert.ToDecimal(metersModel.MaxReading.ToString());
            }
            else
            {
                Meter.ReadingMax = 0;
            }
            Meter.ReadingUnits = metersModel.ReadingUnits;
            if (Meter.ReadingDate == System.DateTime.MinValue)
            {
                Meter.ReadingDate = null;
            }
               
            Meter.ReadingBy = Convert.ToInt64(userData.DatabaseKey.Personnel.PersonnelId);

            Meter.CreateMode = false;
            Meter.UpdateByPKForeignKeys(this.userData.DatabaseKey);

            if (Meter.ErrorMessages.Count == 0)
            {
                MeterId = Meter.MeterId;
            }
            return Meter.ErrorMessages;
        }

        #endregion


        #region Common
        public List<DataContracts.LookupList> GetReadingUnitList()
        {
            List<DataContracts.LookupList> objType = new Models.LookupList().RetrieveAll(userData.DatabaseKey).Where(x => x.ListName == LookupListConstants.Lookup_ListName).ToList();
            return objType;
        }
        #endregion
        #region Details
        internal MetersModel populateMeterDetails(long MeterId)
        {
            MetersModel objMetersModel = new MetersModel();
            Meter meter = new Meter()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                MeterId = MeterId
            };
            meter.RetrieveByPKForeignKeys(userData.DatabaseKey);
            objMetersModel = PopulateModel(meter);
            return objMetersModel;
        }

        internal MetersModel PopulateModel(Meter dbObj)
        {
            MetersModel oModel = new MetersModel();
            oModel.MeterClientLookUpId = dbObj.ClientLookupId;
            oModel.MeterName = dbObj.Name;
            oModel.ReadingUnits = dbObj.ReadingUnits;
            oModel.MaxReading = dbObj.ReadingMax;
            oModel.InActive = dbObj.InactiveFlag;
            oModel.ReadingCurrent = dbObj.ReadingCurrent;
            oModel.ReadingLife = dbObj.ReadingLife;

            if (dbObj.ReadingDate != null && dbObj.ReadingDate != default(DateTime))
            {
                oModel.ReadingDate = dbObj.ReadingDate;
            }
            else
            {
                oModel.ReadingDate = null;
            }
            oModel.MeterId = dbObj.MeterId;
            return oModel;
        }
        internal bool MakeActiveInactive(bool InActiveFlag, long MeterId)
        {
            DataContracts.Meter meter = new DataContracts.Meter()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                MeterId = MeterId,
                InactiveFlag = !InActiveFlag
            };
            meter.ActiveInactiveByprimaryKey(userData.DatabaseKey);
            return meter.InactiveFlag;
        }
        #endregion

        #region Readings
        public List<MetersReadingModel> GetMeterReadingsList(long meterId)
        {
            MetersReadingModel objMetersReadingModel;
            List<MetersReadingModel> MetersReadingModelList = new List<MetersReadingModel>();

            MeterReading meterreading = new DataContracts.MeterReading()
            {
                ClientId = this.userData.DatabaseKey.Client.ClientId,
                MeterId = meterId
            };
            List<DataContracts.MeterReading> meterreadings = MeterReading.RetriveByMeterId(this.userData.DatabaseKey, meterreading);
            foreach (var item in meterreadings)
            {
                objMetersReadingModel = new MetersReadingModel();
                objMetersReadingModel.Reading = item.Reading;
                objMetersReadingModel.StringReading = Convert.ToString(item.Reading);
                objMetersReadingModel.ReadByClientLookupId = item.ReadByClientLookupId;
                if (item.DateRead != null)
                {
                    objMetersReadingModel.DateRead = DateTime.ParseExact(item.DateRead, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    objMetersReadingModel.DateRead = null;
                }
                objMetersReadingModel.Reset = item.Reset;
                MetersReadingModelList.Add(objMetersReadingModel);
            }
            return MetersReadingModelList;
        }

        #region Add
        public List<String> AddReadings(MetersReadingModel metersReadingModel, ref string woLookupIds)
        {

            List<Int64> createdWorkOrderList = new List<Int64>();

            // Create new meterreading contract
            DataContracts.MeterReading newMeterReading = new DataContracts.MeterReading()
            {
                ClientId = userData.DatabaseKey.Client.ClientId,
                SiteId = userData.DatabaseKey.User.DefaultSiteId,
                meter_clientlookupid = metersReadingModel.MeterClientLookUpId,
                MeterId = metersReadingModel.MeterId,
                Reading = metersReadingModel.Reading,
                ReadingBy_PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                ReadByClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId,
                ReadingDate = metersReadingModel.DateRead
            };
            // Validate the Meter Reading
            newMeterReading.ValidateMeterReadingProcess(userData.DatabaseKey);
            // If valid
            if (newMeterReading.ErrorMessages.Count == 0)
            {
                // Create New Meter Reading
                // Also checks Preventive Maintenance Schedule Records to See If a Work Order Needs to be generated
                newMeterReading.Meter_Reading(userData.DatabaseKey);
                // Recorded the meter reading
                // Now see if any Preventive Maintenance Records Need Work Orders Generated.
                Meter Meter_PMGen = new Meter()
                {
                    ClientId = userData.DatabaseKey.Client.ClientId,
                    SiteId = userData.DatabaseKey.User.DefaultSiteId,
                    MeterId = metersReadingModel.MeterId,
                    PersonnelId = userData.DatabaseKey.Personnel.PersonnelId,
                };
                Meter_PMGen.GeneratePMWorkOrders(userData.DatabaseKey);
                WOGen = Meter_PMGen.PMWOList;
                if (WOGen == null)
                    WOGen = new List<long>();
                string cMessage = "";
                if (WOGen.Count > 0)
                {
                    // If work orders generated - send alerts
                    // The WOGen list has a list of workorder id numbers generated (WorkOrder.WorkOrderID). Need to retrieve based on that not meter and reading date.
                    cMessage = "Reading recorded - Work Order(s) Generated \\n";
                    int iCount = 0;
                    foreach (long woid in WOGen)
                    {
                        if (iCount > 0)
                        {
                            cMessage += ", " + WOGen[iCount].ToString();
                        }
                        else
                        {
                            cMessage += WOGen[iCount].ToString();
                        }
                        ++iCount;
                    }

                    DataContracts.WorkOrder workOrderContract = new DataContracts.WorkOrder();
                    workOrderContract.MeterId = metersReadingModel.MeterId;
                    if (metersReadingModel.DateRead.HasValue)
                    { workOrderContract.CreateDate = metersReadingModel.DateRead.Value; }

                    List<DataContracts.WorkOrder> WorkOrderList = new List<DataContracts.WorkOrder>();

                    WorkOrderList = workOrderContract.WorkOrderRetrieveWorkOrderListByMeterIdAndReadingDate(this.userData.DatabaseKey);

                    foreach (WorkOrder workOrder in WorkOrderList)
                    {
                        createdWorkOrderList.Add(workOrder.WorkOrderId);
                    }

                    if (createdWorkOrderList.Count > 0)
                    {
                        string commaSeparetedWorkOrders = string.Empty;
                        createdWorkOrderList.ForEach(x =>
                        {
                            if (x != 0)
                                commaSeparetedWorkOrders += x.ToString() + ",";
                        });

                        DataContracts.WorkOrder workOrder = new DataContracts.WorkOrder();
                        AlertCreate(workOrder, AlertTypeEnum.WorkOrderSchedule, createdWorkOrderList);
                    }
                    //V2-784 To Display Generated WorkOrder ClientLookupIds
                    woLookupIds = Meter_PMGen.PMWOClientLookupIds;
                }
            }

            return newMeterReading.ErrorMessages;
        }

        private void AlertCreate(DataContracts.WorkOrder workOrder, AlertTypeEnum alertTypeEnum, List<Int64> ListWorkOrderId) //Process Alert
        {
            DataContracts.WorkOrder workorder = new DataContracts.WorkOrder();
            ProcessAlert objAlert = new ProcessAlert(this.userData);

            List<object> lstWOID = new List<object>();
            foreach (Int64 objItem in ListWorkOrderId)
            {
                lstWOID.Add(objItem);
            }
            try
            {
                objAlert.CreateAlert<DataContracts.WorkOrder>(this.userData, workorder, alertTypeEnum, this.userData.DatabaseKey.User.UserInfoId, lstWOID);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        #endregion
        #endregion

        #region Reset Meter
        public List<string> ResetMeter(MetersResetModel metersModel)
        {
            DataContracts.MeterReading resetMeter = new DataContracts.MeterReading()
            {               
                meter_clientlookupid = metersModel.MeterClientLookUpId,
                MeterId = metersModel.MeterId,
                Reading = metersModel.Reading,                
                ReadByClientLookupId = userData.DatabaseKey.Personnel.ClientLookupId,
                ReadingDate = metersModel.ReadingDate,
                Reset = true
            };

            resetMeter.ValidateResetMeterAdd(userData.DatabaseKey);
            if (resetMeter.ErrorMessages.Count == 0)
            {
                resetMeter.ResetMeter(userData.DatabaseKey);               
            }
            return resetMeter.ErrorMessages;

        }
        #endregion
    }
}